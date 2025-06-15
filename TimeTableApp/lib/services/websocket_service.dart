import 'dart:async';
import 'package:web_socket_channel/web_socket_channel.dart';


class WebSocketService {
  static final WebSocketService _instance = WebSocketService._internal();
  factory WebSocketService() => _instance;
  WebSocketService._internal();

  WebSocketChannel? _channel;
  StreamSubscription<dynamic>? _subscription;
  final _messageController = StreamController<dynamic>.broadcast();

  Stream<dynamic> get messageStream => _messageController.stream;

  Future<void> connect(String url) async {
    try {
      _channel = WebSocketChannel.connect(Uri.parse(url));
      _subscription = _channel!.stream.listen(
            (message) => _messageController.add(message),
        onError: (error) => _reconnect(url),
        onDone: () => _reconnect(url),
      );
      print('✅ WebSocket connected');
    } catch (e) {
      print('⛔ Connection error: $e');
      await _reconnect(url);
    }
  }

  Future<void> _reconnect(String url) async {
    await dispose();
    print('🔄 Reconnecting...');
    await connect(url);
  }

  Future<void> dispose() async {
    await _subscription?.cancel();
    await _channel?.sink?.close();
    _subscription = null;
    _channel = null;
  }
}

