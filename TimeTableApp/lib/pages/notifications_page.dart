import 'dart:async';
import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../constants/api_constants.dart';
import '../services/websocket_service.dart';
import '/services/notification_service.dart';


class NotificationsScreen extends StatefulWidget {
  @override
  State<NotificationsScreen> createState() => _NotificationsScreenState();
}

class _NotificationsScreenState extends State<NotificationsScreen> {
  final NotificationService _service = NotificationService();
  final WebSocketService _webSocketService = WebSocketService();
  final DateFormat _timeFormat = DateFormat('HH:mm');
  StreamSubscription<dynamic>? _messageSub; // Объявляем подписку здесь

  @override
  void initState() {
    super.initState();
    _service.addListener(_update);
    _initWebSocket();
  }

  Future<void> _initWebSocket() async {
    await _webSocketService.connect(ApiConstants.webSocketUrl);
    _messageSub = _webSocketService.messageStream.listen(_handleMessage);
  }

  void _handleMessage(dynamic message) {
    try {
      final json = jsonDecode(message) as Map<String, dynamic>;
      _service.show(
        json['title']?.toString() ?? 'Уведомление',
        json['body']?.toString() ?? '',
      );
    } catch (e) {
      print('Error processing message: $e');
    }
  }

  @override
  void dispose() {
    _messageSub?.cancel();
    _service.removeListener(_update);
    _webSocketService.dispose();
    super.dispose();
  }

  void _update() => setState(() {});


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Уведомления')),
      body: _service.notifications.isEmpty
          ? _buildEmptyState()
          : _buildNotificationsList(),
    );
  }
  Widget _buildEmptyState() {
    return Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.notifications_off, size: 64, color: Colors.grey),
          SizedBox(height: 16),
          Text('Нет новых уведомлений', style: TextStyle(fontSize: 18)),
        ],
      ),
    );
  }
  Widget _buildNotificationsList() {
    return ListView.builder(
      padding: EdgeInsets.all(16),
      itemCount: _service.notifications.length,
      itemBuilder: (context, index) {
        final notification = _service.notifications[index];
        return Card(
          margin: EdgeInsets.only(bottom: 12),
          child: ListTile(
            contentPadding: EdgeInsets.all(16),
            leading: Icon(Icons.campaign, color: Colors.blue),
            title: Text(notification.title),
            subtitle: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(notification.body),
                SizedBox(height: 4),
                Text(
                  '${notification.timeAgo} (${_timeFormat.format(notification.timestamp)})',
                  style: TextStyle(color: Colors.grey, fontSize: 12),
                ),
              ],
            ),
          ),
        );
      },
    );
  }
  Widget _buildContent() {
    if (_service.notifications.isEmpty) {
      return const Center(child: Text('Нет новых уведомлений'));
    }

    return ListView.builder(
      itemCount: _service.notifications.length,
      itemBuilder: (context, index) {
        final notification = _service.notifications[index];
        return ListTile(
          title: Text(notification.title),
          subtitle: Text(notification.body),
          trailing: Text(notification.timeAgo),
        );
      },
    );
  }
}


