import 'dart:async';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:dio/dio.dart';

import '../constants/api_constants.dart';
import '../models/timetable_model.dart';
import '../utils/storage_helper.dart';
import '/flutter_flow/flutter_flow_theme.dart';
import '/flutter_flow/flutter_flow_util.dart';
import '/services/notification_service.dart';

class MainTimetableWidget extends StatefulWidget {
  const MainTimetableWidget({super.key});

  @override
  State<MainTimetableWidget> createState() => _MainTimetableWidgetState();
}

class _MainTimetableWidgetState extends State<MainTimetableWidget> {
  final NotificationService _notificationService = NotificationService();
  late TimetableModel _model;
  final scaffoldKey = GlobalKey<ScaffoldState>();
  final Dio _dio = Dio();
  Map<String, List<Map<String, dynamic>>> _schedule = {};
  bool _isLoading = false;
  DateTime _currentDate = DateTime.now();


  @override
  void initState() {
    super.initState();
    _notificationService.init();
    _model = createModel(context, () => TimetableModel());
    _loadSchedule();
  }


  Future<void> _loadSchedule() async {
    if (!mounted) return;
    setState(() => _isLoading = true);

    final token = await StorageHelper.getToken();
    if (token == null || token.isEmpty) {
      if (mounted) {
        setState(() => _isLoading = false); // Сбрасываем состояние перед навигацией
        _navigateToAuth();
      }
      return;
    }

    try {
      final today = DateFormat('yyyy-MM-dd').format(_currentDate);
      final tomorrow = DateFormat('yyyy-MM-dd')
          .format(_currentDate.add(const Duration(days: 1)));

      final responses = await Future.wait([
        _dio.get(ApiConstants.dateByDay(today),
            options: Options(headers: ApiConstants.authHeaders(token))),
            // options: Options(headers: {
            //   'Authorization': 'Bearer $token',
            //   'Content-Type': 'application/json',
            // })),
        _dio.get(ApiConstants.dateByDay(tomorrow),
            options: Options(headers: ApiConstants.authHeaders(token)))
            //options: Options(headers: {
             // 'Authorization': 'Bearer $token',
            //  'Content-Type': 'application/json',
           // }))
      ]);

      if (!mounted) return;
      setState(() {
        _schedule = {
          'today': (responses[0].data as List<dynamic>)
              .map((e) => e as Map<String, dynamic>)
              .toList(),
          'tomorrow': (responses[1].data as List<dynamic>)
              .map((e) => e as Map<String, dynamic>)
              .toList(),
        };
      });
    } on DioException catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Ошибка: ${e.message}')),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Неизвестная ошибка')),
        );
      }
    } finally {
      if (mounted) setState(() => _isLoading = false); // Всегда сбрасываем флаг загрузки
    }
  }

  Widget _buildNoLessonsCard() {
    return Padding(
      padding: const EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
      child: Material(
        color: Colors.transparent,
        elevation: 2,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12),
        ),
        child: Container(
          width: MediaQuery.sizeOf(context).width,
          decoration: BoxDecoration(
            color: FlutterFlowTheme.of(context).primaryBackground,
            borderRadius: BorderRadius.circular(12),
          ),
          child: Padding(
            padding: const EdgeInsetsDirectional.fromSTEB(16, 16, 16, 16),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  'Занятий нет',
                  style: FlutterFlowTheme.of(context).titleMedium.override(
                    fontFamily: 'Inter Tight',
                    color: FlutterFlowTheme.of(context).primary,
                    letterSpacing: 0.0,
                  ),
                ),
                const SizedBox(height: 12),
                Icon(
                  Icons.sentiment_satisfied_alt_outlined,
                  color: FlutterFlowTheme.of(context).primary,
                  size: 40,
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  void _navigateToAuth() {
    if (mounted) {
      Navigator.pushReplacementNamed(context, '/auths');
    }
  }

  String _getLessonStatus(Map<String, dynamic> lesson) {
    final now = DateTime.now();

    try {
      // Парсим только время из формата HH:mm
      final startTime = DateFormat('HH:mm').parse(lesson['timeStart']);
      final endTime = DateFormat('HH:mm').parse(lesson['timeEnd']);

      // Создаем DateTime с текущей датой и полученным временем
      final start = DateTime(now.year, now.month, now.day, startTime.hour, startTime.minute);
      final end = DateTime(now.year, now.month, now.day, endTime.hour, endTime.minute);

      if (now.isAfter(end)) return 'Завершена';
      if (now.isBefore(start)) return 'Скоро';
      return 'Идет сейчас';
    } catch (e) {
      print('Ошибка парсинга времени: $e');
      return 'Неизвестно';
    }
  }

  Color _getStatusColor(String status) {
    return switch (status) {
      'Завершена' => const Color(0xFFEF9A9A),
      'Идет сейчас' => const Color(0xFFC8E6C9),
      _ => const Color(0xFFFFF9C4),
    };
  }

  Widget _buildLessonCard(Map<String, dynamic> lesson) {
    final status = _getLessonStatus(lesson);
    return Padding(
      padding: const EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
      child: Material(
        elevation: 2,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12),
        ),
        child: Container(
          width: MediaQuery.sizeOf(context).width,
          decoration: BoxDecoration(
            color: FlutterFlowTheme.of(context).primaryBackground,
            borderRadius: BorderRadius.circular(12),
          ),
          child: Padding(
            padding: const EdgeInsetsDirectional.fromSTEB(16, 16, 16, 16),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text(
                      lesson['discipline'],
                      style: FlutterFlowTheme.of(context).titleMedium.override(
                        fontFamily: 'Inter Tight',
                        color: FlutterFlowTheme.of(context).primary,
                        letterSpacing: 0.0,
                      ),
                    ),
                    Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 12, vertical: 6),
                      decoration: BoxDecoration(
                        color: _getStatusColor(status),
                        borderRadius: BorderRadius.circular(16),
                      ),
                      child: Text(
                        status,
                        style: TextStyle(
                          color: status == 'Скоро'
                              ? const Color(0xFFF57C00)
                              : const Color(0xFF2E7D32),
                          fontSize: 12,
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 8),
                Row(
                  children: [
                    const Icon(Icons.access_time, size: 18),
                    const SizedBox(width: 8),
                    Text(
                      '${lesson['timeStart']} - ${lesson['timeEnd']}',
                      style: FlutterFlowTheme.of(context).bodyMedium,
                    ),
                  ],
                ),
                const SizedBox(height: 8),
                Row(
                  children: [
                    const Icon(Icons.location_on, size: 18),
                    const SizedBox(width: 8),
                    Text(
                      '${lesson['building']} корпус, ауд. ${lesson['room']}',
                      style: FlutterFlowTheme.of(context).bodyMedium,
                    ),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () => FocusScope.of(context).unfocus(),
      child: Scaffold(
        key: scaffoldKey,
        backgroundColor: FlutterFlowTheme.of(context).primaryBackground,
        body: SafeArea(
          child: _isLoading
              ? const Center(child: CircularProgressIndicator())
              : Column(
            children: [
              // Header Section
              Container(
                height: 180,
                decoration: const BoxDecoration(
                  gradient: LinearGradient(
                    colors: [Color(0xFF1A237E), Color(0xFF3F51B5)],
                    stops: [0, 1],
                    begin: AlignmentDirectional(0, -1),
                    end: AlignmentDirectional(0, 1),
                  ),
                ),
                child: Padding(
                  padding: const EdgeInsetsDirectional.fromSTEB(24, 24, 24, 0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                    Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Главная',
                            style: FlutterFlowTheme.of(context)
                                .headlineMedium
                                .override(
                              fontFamily: 'Inter Tight',
                              color: Colors.white,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          Text(
                            'Весенний семестр',
                            style: FlutterFlowTheme.of(context)
                                .bodyLarge
                                .override(color: const Color(0xFFE0E0E0)),
                          )],
                          ),
                          IconButton(
                            icon: const Icon(Icons.notifications_none,
                                color: Colors.white, size: 28),
                            onPressed: () {
                              Navigator.pushNamed(context, '/notifications');
                            },
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
              ),
              // Schedule Content
              Expanded(
                child: Container(
                  width: MediaQuery.sizeOf(context).width,
                  decoration: BoxDecoration(
                    color: FlutterFlowTheme.of(context).secondaryBackground,
                    borderRadius: const BorderRadius.only(
                      topLeft: Radius.circular(32),
                    ),
                  ),
                  child: Padding(
                    padding: const EdgeInsetsDirectional.fromSTEB(24, 24, 24, 0),
                    child: SingleChildScrollView(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          // Today's Schedule
                          _buildDaySection('Сегодня', _schedule['today'] ?? []),
                          const SizedBox(height: 24),
                          // Tomorrow's Schedule
                          _buildDaySection('Завтра', _schedule['tomorrow'] ?? []),
                          const SizedBox(height: 24),
                        ].divide(const SizedBox(height: 16)),
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
        bottomNavigationBar: BottomNavigationBar(
          currentIndex: 0,
          type: BottomNavigationBarType.fixed,
          selectedItemColor: FlutterFlowTheme.of(context).primary,
          unselectedItemColor: FlutterFlowTheme.of(context).secondaryText,
          items: const [
            BottomNavigationBarItem(
              icon: Icon(Icons.home_outlined),
              label: 'Главная',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.calendar_today),
              label: 'Расписание',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.menu_book),
              label: 'Заметки',
            ),
            BottomNavigationBarItem(
              icon: Icon(Icons.person_outline),
              label: 'Профиль',
            ),
          ],
          onTap: (index) {
            if (index == 1) Navigator.pushNamed(context, '/timetable');
            if (index == 2) Navigator.pushNamed(context, '/knowledgeGraph');
            if (index == 3) Navigator.pushNamed(context, '/user_profile');
          },
        ),
      ),
    );
  }

  Widget _buildDaySection(String title, List<Map<String, dynamic>> lessons) {
    return Column(
      children: [
        Padding(
          padding: const EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
          child: Text(
            title,
            style: FlutterFlowTheme.of(context).headlineSmall.override(
              fontFamily: 'Inter Tight',
              letterSpacing: 0.0,
            ),
          ),
        ),

        if (lessons.isEmpty)
          _buildNoLessonsCard()
        else
          ...lessons.map(_buildLessonCard),
      ],
    );
  }

  @override
  void dispose() {
    super.dispose();
  }
}