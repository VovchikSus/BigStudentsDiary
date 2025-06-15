import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:dio/dio.dart';
import '../constants/api_constants.dart';
import '../models/timetable_model.dart';
import '../utils/storage_helper.dart';
import '/flutter_flow/flutter_flow_theme.dart';
import '/flutter_flow/flutter_flow_util.dart';

class TimetablePageWidget extends StatefulWidget {
  const TimetablePageWidget({super.key});

  @override
  State<TimetablePageWidget> createState() => _TimetablePageWidgetState();
}

class _TimetablePageWidgetState extends State<TimetablePageWidget> {
  late TimetableModel _model;
  final scaffoldKey = GlobalKey<ScaffoldState>();
  final Dio _dio = Dio();
  DateTime _selectedDate = DateTime.now();
  Map<String, List<dynamic>> _weeklySchedule = {};
  bool _isLoading = false;

  String _formatDate(DateTime date) => DateFormat('yyyy-MM-dd').format(date);

  List<DateTime> _getWeekDates(DateTime startDate) {
    final dates = List.generate(7, (i) => startDate.add(Duration(days: i)));
    dates.sort();
    return dates;
  }

  Future<void> _fetchWeeklySchedule() async {
    setState(() => _isLoading = true);

    final token = await StorageHelper.getToken();
    if (token == null || token.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Требуется авторизация')),
      );
      Navigator.pushReplacementNamed(context, '/auths');
      return;
    }

    try {
      final weekDates = _getWeekDates(_selectedDate);
      final newSchedule = <String, List<dynamic>>{};

      for (final date in weekDates) {
        final formattedDate = _formatDate(date);
        final url = ApiConstants.formattedDateEndpoint(formattedDate);

        final response = await _dio.get(
            url,
            options: Options(headers: ApiConstants.authHeaders(token))
        );

        if (response.statusCode == 200) {
          final data = (response.data as List)
            ..sort((a, b) => a['timeStart'].compareTo(b['timeStart']));

          newSchedule[DateFormat('dd.MM.yyyy').format(date)] = data;
        }
      }

      setState(() => _weeklySchedule = newSchedule);
    } on DioException catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка загрузки: ${e.message}')),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Неизвестная ошибка')),
      );
    } finally {
      setState(() => _isLoading = false);
    }
  }

  Future<void> _pickDate(BuildContext context) async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime(2000),
      lastDate: DateTime(2100),
    );

    if (picked != null && picked != _selectedDate) {
      setState(() => _selectedDate = picked);
      _fetchWeeklySchedule();
    }
  }

  @override
  void initState() {
    super.initState();
    _model = createModel(context, () => TimetableModel());
    _fetchWeeklySchedule();
  }

  @override
  void dispose() {
    _model.dispose();
    super.dispose();
  }

  Widget _buildDaySchedule(String dayName, List<dynamic>? lessons) {
    if (lessons == null || lessons.isEmpty) {
      return Padding(
        padding: const EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
        child: Material(
          color: Colors.transparent,
          elevation: 2,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          child: Container(
            width: MediaQuery.sizeOf(context).width,
            decoration: BoxDecoration(
              color: FlutterFlowTheme.of(context).primaryBackground,
              borderRadius: BorderRadius.circular(12),
            ),
            child: Padding(
              padding: const EdgeInsetsDirectional.fromSTEB(16, 16, 16, 16),
              child: Column(
                children: [
                  Text(dayName,
                      style: FlutterFlowTheme.of(context)
                          .titleMedium
                          .override(
                          fontFamily: 'Inter Tight',
                          color: FlutterFlowTheme.of(context).primary)),
                  Text('Занятий нет',
                      style: FlutterFlowTheme.of(context).bodyMedium.override(
                          color: FlutterFlowTheme.of(context).secondaryText)),
                ].divide(const SizedBox(height: 12)),
              ),
            ),
          ),
        ),
      );
    }

    return Padding(
      padding: const EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
      child: Material(
        color: Colors.transparent,
        elevation: 2,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        child: Container(
          width: MediaQuery.sizeOf(context).width,
          decoration: BoxDecoration(
              color: FlutterFlowTheme.of(context).primaryBackground,
              borderRadius: BorderRadius.circular(12)),
          child: Padding(
            padding: const EdgeInsetsDirectional.fromSTEB(16, 16, 16, 16),
            child: Column(
              children: [
                Text(dayName,
                    style: FlutterFlowTheme.of(context).titleMedium.override(
                        fontFamily: 'Inter Tight',
                        color: FlutterFlowTheme.of(context).primary)),
                ...lessons.map((lesson) => Container(
                  decoration: BoxDecoration(
                      color: _getLessonColor(lesson['discipline']),
                      borderRadius: BorderRadius.circular(8)),
                  child: Padding(
                    padding: const EdgeInsets.all(12),
                    child: Column(
                      children: [
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(lesson['discipline'],
                                style: FlutterFlowTheme.of(context)
                                    .bodyLarge
                                    .override(
                                    color: _getTextColor(
                                        lesson['discipline']))),
                            Text(lesson['timeRange'],
                                style: FlutterFlowTheme.of(context)
                                    .bodyMedium
                                    .override(
                                    color: _getTextColor(
                                        lesson['discipline']))),
                          ],
                        ),
                        Text(
                            '${lesson['building']} корпус, ауд. ${lesson['room']}',
                            style: FlutterFlowTheme.of(context)
                                .bodyMedium
                                .override(
                                color: FlutterFlowTheme.of(context)
                                    .secondaryText)),
                      ].divide(const SizedBox(height: 8)),
                    ),
                  ),
                )),
              ].divide(const SizedBox(height: 12)),
            ),
          ),
        ),
      ),
    );
  }

  Color _getLessonColor(String discipline) {
    final colors = {
      'Статистика': const Color(0xFFE3F2FD),
      'История': const Color(0xFFFFF3E0),
      'Философия': const Color(0xFFE8F5E9),
      'Высшая математика': const Color(0xFFEDE7F6),
    };
    return colors[discipline] ?? const Color(0xFFF5F5F5);
  }

  Color _getTextColor(String discipline) {
    final colors = {
      'Статистика': const Color(0xFF1565C0),
      'История': const Color(0xFFFF6F00),
      'Философия': const Color(0xFF2E7D32),
      'Высшая математика': const Color(0xFF4527A0),
    };
    return colors[discipline] ?? Colors.black;
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () => FocusScope.of(context).unfocus(),
      child: Scaffold(
        key: scaffoldKey,
        backgroundColor: FlutterFlowTheme.of(context).primaryBackground,
        // УДАЛЕН AppBar (верхняя панель с кнопкой "назад")

        body: SafeArea(
          child: Column(
            children: [
              // Header Section (как на главной странице)
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
                                'Расписание',
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
                              ),
                            ],
                          ),
                          IconButton(
                            icon: const Icon(Icons.calendar_today,
                                color: Colors.white, size: 28),
                            onPressed: () => _pickDate(context),
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
                    child: _isLoading
                        ? const Center(child: CircularProgressIndicator())
                        : SingleChildScrollView(
                      child: Column(
                        children: [
                          Padding(
                            padding: const EdgeInsetsDirectional.fromSTEB(
                                8, 0, 8, 0),
                            child: Row(
                              mainAxisSize: MainAxisSize.max,
                              mainAxisAlignment:
                              MainAxisAlignment.spaceBetween,
                              children: [
                                IconButton(
                                  icon: Icon(
                                    Icons.chevron_left,
                                    color: FlutterFlowTheme.of(context)
                                        .primary,
                                    size: 24,
                                  ),
                                  onPressed: () {
                                    setState(() => _selectedDate =
                                        _selectedDate
                                            .subtract(const Duration(days: 7)));
                                    _fetchWeeklySchedule();
                                  },
                                ),
                                Text(
                                  DateFormat('dd.MM.yyyy').format(_selectedDate),
                                  style: FlutterFlowTheme.of(context)
                                      .headlineSmall
                                      .override(
                                    fontFamily: 'Inter Tight',
                                    letterSpacing: 0.0,
                                  ),
                                ),
                                IconButton(
                                  icon: Icon(
                                    Icons.chevron_right,
                                    color: FlutterFlowTheme.of(context)
                                        .primary,
                                    size: 24,
                                  ),
                                  onPressed: () {
                                    setState(() => _selectedDate =
                                        _selectedDate
                                            .add(const Duration(days: 7)));
                                    _fetchWeeklySchedule();
                                  },
                                ),
                              ],
                            ),
                          ),
                          ..._weeklySchedule.entries
                              .map((e) => _buildDaySchedule(
                            DateFormat('EEEE', 'ru_RU').format(
                                DateFormat('dd.MM.yyyy').parse(e.key)),
                            e.value,
                          )),
                        ].divide(const SizedBox(height: 16)),
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
        // Добавлена стандартная нижняя навигация как на главной странице
        bottomNavigationBar: BottomNavigationBar(
          currentIndex: 1, // Активная вкладка "Расписание"
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
            if (index == 0) Navigator.pushReplacementNamed(context, '/');
            if (index == 1) Navigator.pushReplacementNamed(context, '/timetable');
            if (index == 2) Navigator.pushReplacementNamed(context, '/knowledgeGraph');
            if (index == 3) Navigator.pushReplacementNamed(context, '/user_profile');
          },
        ),
      ),
    );
  }
}