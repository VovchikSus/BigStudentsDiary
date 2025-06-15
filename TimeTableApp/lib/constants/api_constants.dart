// constants/api_constants.dart
class ApiConstants {
  // Базовый URL
  static const String baseUrl = 'https://a7ea39449996befb34dabf2b7dbb1df8.serveo.net';

  // WebSocket URL
  static String get webSocketUrl => baseUrl
      .replaceFirst('https://', 'wss://')
      .replaceFirst('http://', 'ws://') + '/api/callback/ws';

  // Эндпоинты
  static const String date = '$baseUrl/date';
  static const String posts = '$baseUrl/posts';

  //API
  static String dateByDay(String day) => '$date/$day';
  static String formattedDateEndpoint(String formattedDate) => '$baseUrl/date/$formattedDate';

 //TOKEN
  static Map<String, String> authHeaders(String token) => {
    'Authorization': 'Bearer $token',
    'Content-Type': 'application/json',
  };

}