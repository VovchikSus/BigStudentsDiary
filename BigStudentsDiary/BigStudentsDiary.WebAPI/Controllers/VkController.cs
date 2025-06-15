using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace BigStudentsDiary.WebAPI.Controllers;

[ApiController]
[Route("api/callback")]
public class VkController : ControllerBase
{
    private readonly string _confirmationCode = "a85f6d25"; // Из настроек Callback API
    private readonly string _groupToken = "vk1.a.CaCRdH3AYkQf4DNHOnUs_DZkqAmuqrh82hItVc-unlHJqWGbY5cUwwcdJTJaOEbkC6TjQgKuQMyle_p5qiE8zgnOBGVYKHjmgF-ZiMm-gDZ851ak40CUSqVzQBmHbc_6g5AQOvB0aWjabOsVX3amtDdoIaBxkeWHk02W_A9eZW8LgCx-I13H59TblvP_PVDjOKWG1elMzz8W_w9Ip0ljvA";
    private readonly long _chatId = 2000000002; // ID беседы (peer_id)

    [HttpGet("ws")]
    public async Task HandleWebSocket()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString();
            WebSocketManager.AddSocket(socketId, webSocket);
        
            await ReceiveMessages(webSocket, socketId);
        }
    }

    private async Task ReceiveMessages(WebSocket socket, string socketId)
    {
        var buffer = new byte[1024 * 4];
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );

                // Добавьте очистку буфера
                Array.Clear(buffer, 0, buffer.Length);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closed by server",
                        CancellationToken.None
                    );
                    Log.Information("Соединение закрыто корректно");
                    break;
                }
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
            Log.Warning("Клиент разорвал соединение неожиданно");
        }
        finally
        {
            WebSocketManager.RemoveSocket(socketId);
            socket.Dispose();
        }
    }

// Менеджер подключений
    public static class WebSocketManager
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new();
        public static void AddSocket(string id, WebSocket socket) => _sockets.TryAdd(id, socket);
        public static void RemoveSocket(string id) => _sockets.TryRemove(id, out _);
        public static async Task SendToAllAsync(string message)
        {
            var tasks = new List<Task>();
            foreach (var socket in _sockets.Values.ToList())
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    { var bytes = Encoding.UTF8.GetBytes(message); tasks.Add(socket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None
                        ));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Send error");
                    }
                }
            }
            await Task.WhenAll(tasks);
        }
    }
    
    
    
    [HttpPost]
    public async Task<IActionResult> HandleCallback([FromBody] VkEventModel data)
    {
        Log.Information("Raw request: {Data}", data.ToString());
        
        if (data.Type == "confirmation")
            return Content(_confirmationCode, "text/plain");

        if (data.Object?.Message == null)
            return Ok("ignore");
        
        var notification = new 
        {
            title = "Деканат",
            body = data.Object.Message.Text
        };
    
        Log.Information("Отправка тестового сообщения: {Text}", data.Object.Message.Text);
        await WebSocketManager.SendToAllAsync(JsonConvert.SerializeObject(notification));
    
        return Ok("ok");
    }

    private bool IsImportantMessage(string message)
    {
        var keywords = new[] { "перенос", "пары", "день рождения" };
        return keywords.Any(keyword => 
            message.Contains(keyword, StringComparison.OrdinalIgnoreCase)
        );
    }
    
    private async Task SendToFlutter(string message)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=ВАШ_FCM_API_KEY");

        var fcmData = new
        {
            to = "ТОКЕН_УСТРОЙСТВА_FROM_FLUTTER",
            notification = new { title = "Деканат", body = message }
        };

        var response = await httpClient.PostAsJsonAsync(
            "https://fcm.googleapis.com/fcm/send", 
            fcmData
        );
    }
}







// Модели для десериализации JSON от VK
public class VkEventModel
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("object")]
    public VkMessageObject? Object { get; set; }
}

public class VkMessageObject
{
    [JsonProperty("message")]
    public VkMessage Message { get; set; }
}

public class VkMessage
{
    [JsonProperty("peer_id")]
    public long PeerId { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
    
    [JsonProperty("from_id")]
    public long FromId { get; set; }
    
    [JsonProperty("conversation_message_id")]
    public int ConversationMessageId { get; set; }
    
    [JsonProperty("id")]
    public long Id { get; set; } 
}