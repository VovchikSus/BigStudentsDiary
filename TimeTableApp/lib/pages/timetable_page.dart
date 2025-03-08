import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:dio/dio.dart';
import '../models/timetable_model.dart';
import '../utils/storage_helper.dart';
import '/flutter_flow/flutter_flow_icon_button.dart';
import '/flutter_flow/flutter_flow_theme.dart';
import '/flutter_flow/flutter_flow_util.dart';
import '/flutter_flow/flutter_flow_widgets.dart';

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
    dates.sort(); // Сортировка всего списка дат
    return dates;
  }

  Future<void> _fetchWeeklySchedule() async {
    setState(() => _isLoading = true);

    final token = await StorageHelper.getToken();

    // Добавлено: Проверка и логирование токена


    if (token == null || token.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Требуется авторизация')),
      );
      Navigator.pushReplacementNamed(context, '/auths');
      return;
    }

    try {
      final weekDates = _getWeekDates(_selectedDate);
      final newSchedule = <String, List<dynamic>>{};

      for (final date in weekDates) {
        final url = 'https://localhost:7049/date/${_formatDate(date)}';
        print('Запрос к URL: $url'); // Логирование URL

        final response = await _dio.get(
          url,
          options: Options(
            headers: {
              'Authorization': 'Bearer $token',
              'Content-Type': 'application/json',
            },
          ),
        );

        if (response.statusCode == 200) {
          final data = (response.data as List)
            ..sort((a, b) => a['timeStart'].compareTo(b['timeStart']));

          newSchedule[DateFormat('dd.MM.yyyy').format(date)] = data;
        }
      }

      setState(() => _weeklySchedule = newSchedule);
    } on DioException catch (e) {
      // Улучшена обработка ошибок
      print('Ошибка сети: ${e.message}');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка загрузки: ${e.message}')),
      );
    } catch (e) {
      print('Общая ошибка: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Неизвестная ошибка')),
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
          padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
          child: Material(
            color: Colors.transparent,
            elevation: 2,
            shape:
                RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            child: Container(
              width: MediaQuery.sizeOf(context).width,
              decoration: BoxDecoration(
                color: FlutterFlowTheme.of(context).primaryBackground,
                borderRadius: BorderRadius.circular(12),
              ),
              child: Padding(
                padding: EdgeInsetsDirectional.fromSTEB(16, 16, 16, 16),
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
                  ].divide(SizedBox(height: 12)),
                ),
              ),
            ),
          ));
    }

    return Padding(
        padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
        child: Material(
          color: Colors.transparent,
          elevation: 2,
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          child: Container(
            width: MediaQuery.sizeOf(context).width,
            decoration: BoxDecoration(
                color: FlutterFlowTheme.of(context).primaryBackground,
                borderRadius: BorderRadius.circular(12)),
            child: Padding(
              padding: EdgeInsetsDirectional.fromSTEB(16, 16, 16, 16),
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
                          padding: EdgeInsets.all(12),
                          child: Column(
                            children: [
                              Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
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
                            ].divide(SizedBox(height: 8)),
                          ),
                        ),
                      )),
                ].divide(SizedBox(height: 12)),
              ),
            ),
          ),
        ));
  }

  Color _getLessonColor(String discipline) {
    final colors = {
      'Статистика': Color(0xFFE3F2FD),
      'История': Color(0xFFFFF3E0),
      'Философия': Color(0xFFE8F5E9),
      'Высшая математика': Color(0xFFEDE7F6),
    };
    return colors[discipline] ?? Color(0xFFF5F5F5);
  }

  Color _getTextColor(String discipline) {
    final colors = {
      'Статистика': Color(0xFF1565C0),
      'История': Color(0xFFFF6F00),
      'Философия': Color(0xFF2E7D32),
      'Высшая математика': Color(0xFF4527A0),
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
        appBar: AppBar(
          backgroundColor: FlutterFlowTheme.of(context).primary,
          leading: FlutterFlowIconButton(
            borderColor: Colors.transparent,
            buttonSize: 40,
            icon: Icon(
              Icons.arrow_back_rounded,
              color: Colors.white,
              size: 24,
            ),
            onPressed: () => context.pop(),
          ),
          title: Padding(
            padding: EdgeInsetsDirectional.fromSTEB(24, 0, 0, 0),
            child: Text(
              'Расписание',
              style: FlutterFlowTheme.of(context).headlineMedium.override(
                    fontFamily: 'Inter Tight',
                    color: Colors.white,
                    fontSize: 22,
                  ),
            ),
          ),
          centerTitle: true,
          elevation: 2,
        ),
        body: SafeArea(
          child: Column(
            children: [
              Container(
                height: 180,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [Color(0xFF1A237E), Color(0xFF3F51B5)],
                    stops: [0, 1],
                    begin: AlignmentDirectional(0, -1),
                    end: AlignmentDirectional(0, 1),
                  ),
                ),
                child: Padding(
                  padding: EdgeInsetsDirectional.fromSTEB(24, 24, 24, 0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Недельное расписание',
                        style: FlutterFlowTheme.of(context)
                            .headlineMedium
                            .override(
                              fontFamily: 'Inter Tight',
                              color: Colors.white,
                              letterSpacing: 0.0,
                              fontWeight: FontWeight.bold,
                            ),
                      ),
                      Row(
                        mainAxisSize: MainAxisSize.max,
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            DateFormat('d MMMM yyyy', 'ru_RU').format(_selectedDate),
                            style: FlutterFlowTheme.of(context).bodyLarge.override(
                              fontFamily: 'Inter',
                              color: Color(0xFFE0E0E0),
                              letterSpacing: 0.0,
                            ),
                          ),
                          FlutterFlowIconButton(
                            borderRadius: 20,
                            buttonSize: 40,
                            fillColor: Color(0x33FFFFFF),
                            icon: Icon(
                              Icons.calendar_today,
                              color: Colors.white,
                              size: 24,
                            ),
                            onPressed: () => _pickDate(context),
                          ),
                        ],
                      ),
                    ].divide(SizedBox(height: 8)),
                  ),
                ),
              ),
              Expanded(
                child: Container(
                  decoration: BoxDecoration(
                    color: FlutterFlowTheme.of(context).secondaryBackground,
                    borderRadius: BorderRadius.only(
                      bottomLeft: Radius.circular(0),
                      bottomRight: Radius.circular(0),
                      topLeft: Radius.circular(32),
                      topRight: Radius.circular(0),
                    ),
                  ),
                  child: Padding(
                    padding: EdgeInsetsDirectional.fromSTEB(24, 24, 24, 0),
                    child: _isLoading
                        ? Center(child: CircularProgressIndicator())
                        : SingleChildScrollView(
                            child: Column(
                              children: [
                                Padding(
                                  padding: EdgeInsetsDirectional.fromSTEB(
                                      8, 0, 8, 0),
                                  child: Row(
                                    mainAxisSize: MainAxisSize.max,
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                    children: [
                                      FlutterFlowIconButton(
                                        buttonSize: 40,
                                        icon: Icon(
                                          Icons.chevron_left,
                                          color: FlutterFlowTheme.of(context)
                                              .primary,
                                          size: 24,
                                        ),
                                        onPressed: () {
                                          setState(() => _selectedDate =
                                              _selectedDate
                                                  .subtract(Duration(days: 7)));
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
                                      FlutterFlowIconButton(
                                        buttonSize: 40,
                                        icon: Icon(
                                          Icons.chevron_right,
                                          color: FlutterFlowTheme.of(context)
                                              .primary,
                                          size: 24,
                                        ),
                                        onPressed: () {
                                          setState(() => _selectedDate =
                                              _selectedDate
                                                  .add(Duration(days: 7)));
                                          _fetchWeeklySchedule();
                                        },
                                      ),
                                    ],
                                  ),
                                ),
                                ..._weeklySchedule.entries
                                    .map((e) => _buildDaySchedule(
                                  DateFormat('EEEE', 'ru_RU').format(
                                      DateFormat('dd.MM.yyyy').parse(e.key)
                                  ),
                                          e.value,
                                        )),
                              ].divide(SizedBox(height: 16)),
                            ),
                          ),
                  ),
                ),
              ),
              // Нижняя навигационная панель (оставить без изменений)
              Material(
                color: Colors.transparent,
                elevation: 8,
                child: Container(
                  width: MediaQuery.sizeOf(context).width,
                  height: 80,
                  decoration: BoxDecoration(
                    color: Colors.white,
                  ),
                  child: Padding(
                    padding: EdgeInsetsDirectional.fromSTEB(12, 24, 12, 24),
                    child: Row(
                      mainAxisSize: MainAxisSize.max,
                      mainAxisAlignment: MainAxisAlignment.spaceAround,
                      children: [
                        // ... Ваш код навигационной панели
                      ],
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
