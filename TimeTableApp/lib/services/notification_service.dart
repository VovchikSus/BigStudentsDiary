import 'package:flutter/material.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:timeago/timeago.dart' as timeago;
import 'dart:convert';

class NotificationService {
  static final NotificationService _instance = NotificationService._internal();
  factory NotificationService() => _instance;
  NotificationService._internal();

  static final FlutterLocalNotificationsPlugin _notificationsPlugin =
  FlutterLocalNotificationsPlugin();

  final List<AppNotification> _notifications = [];
  final List<VoidCallback> _listeners = [];

  List<AppNotification> get notifications => _notifications;

  Future<void> init() async {
    await _setupTimezone();
    await _initializeNotifications();
    await _requestPermissions();
  }

  Future<void> _setupTimezone() async {
    timeago.setLocaleMessages('ru', timeago.RuMessages());
  }

  Future<void> _initializeNotifications() async {
    const AndroidInitializationSettings androidSettings =
    AndroidInitializationSettings('@mipmap/ic_launcher');

    const DarwinInitializationSettings iosSettings = DarwinInitializationSettings(
      requestAlertPermission: true,
      requestBadgePermission: true,
      requestSoundPermission: true,
    );

    await _notificationsPlugin.initialize(
      const InitializationSettings(
        android: androidSettings,
        iOS: iosSettings,
      ),
      onDidReceiveNotificationResponse: _handleNotificationResponse,
    );
  }

  Future<void> _requestPermissions() async {
    await _notificationsPlugin
        .resolvePlatformSpecificImplementation<
        AndroidFlutterLocalNotificationsPlugin>()
        ?.requestPermission();
  }

  void _handleNotificationResponse(NotificationResponse response) {
    try {
      final payload = jsonDecode(response.payload ?? '{}') as Map<String, dynamic>;
      _addToHistory(
        payload['title']?.toString() ?? 'Уведомление',
        payload['body']?.toString() ?? '',
      );
    } catch (e) {
      _addToHistory('Ошибка', 'Некорректный формат уведомления');
    }
    print('Notification tapped: ${response.payload}');
  }

  Future<void> show(String title, String body, {String? payload}) async {
    const AndroidNotificationDetails androidDetails = AndroidNotificationDetails(
      'important_channel',
      'Важные уведомления',
      channelDescription: 'Канал для важных сообщений от деканата',
      importance: Importance.max,
      priority: Priority.high,
      showWhen: true,

    );

    const DarwinNotificationDetails iosDetails = DarwinNotificationDetails(
      presentAlert: true,
      presentBadge: true,
      presentSound: true,
    );

    await _notificationsPlugin.show(
      DateTime.now().millisecondsSinceEpoch ~/ 1000,
      title,
      body,
      const NotificationDetails(
        android: androidDetails,
        iOS: iosDetails,
      ),
      payload: jsonEncode({'title': title, 'body': body}),
    );

    _addToHistory(title, body);
  }

  void _addToHistory(String title, String body) {
    final notification = AppNotification(
      title: title,
      body: body,
      timestamp: DateTime.now(),
    );
    _notifications.insert(0, notification);
    print('Добавлено уведомление: ${notification.title} - ${notification.body}');
    _notifyListeners();
  }

  void addListener(VoidCallback listener) => _listeners.add(listener);
  void removeListener(VoidCallback listener) => _listeners.remove(listener);
  void _notifyListeners() {
    print('Уведомление ${_listeners.length} слушателей:');
    for (final listener in _listeners) {
      print(' - Слушатель: ${listener.hashCode}');
      listener();
    }
  }

  Future<void> clearAll() async {
    await _notificationsPlugin.cancelAll();
    _notifications.clear();
    _notifyListeners();
  }
}

class AppNotification {
  final String title;
  final String body;
  final DateTime timestamp;

  AppNotification({
    required this.title,
    required this.body,
    required this.timestamp,
  });

  String get timeAgo => timeago.format(timestamp, locale: 'ru');
}