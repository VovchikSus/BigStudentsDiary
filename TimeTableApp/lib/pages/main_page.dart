import '../models/timetable_model.dart';
import '/flutter_flow/flutter_flow_theme.dart';
import '/flutter_flow/flutter_flow_util.dart';
import '/flutter_flow/flutter_flow_widgets.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:provider/provider.dart';




class MainTimetableWidget extends StatefulWidget {
  /// I need a page with a schedule for a university so that there would be a
  /// navigator to go to the menu schedule profile main
  const MainTimetableWidget({super.key});

  @override
  State<MainTimetableWidget> createState() => _MainTimetableWidgetState();
}

class _MainTimetableWidgetState extends State<MainTimetableWidget> {
  late TimetableModel _model;

  final scaffoldKey = GlobalKey<ScaffoldState>();

  @override
  void initState() {
    super.initState();
    _model = createModel(context, () => TimetableModel());
  }

  @override
  void dispose() {
    _model.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () => FocusScope.of(context).unfocus(),
      child: Scaffold(
        key: scaffoldKey,
        backgroundColor: FlutterFlowTheme.of(context).primaryBackground,
        body: SafeArea(
          top: true,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Container(
                width: MediaQuery.sizeOf(context).width,
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
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Главная',
                        style: FlutterFlowTheme.of(context)
                            .headlineMedium
                            .override(
                          fontFamily: 'Inter Tight',
                          color: Colors.white,
                          letterSpacing: 0.0,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      Text(
                        'Весенний семестр',
                        style: FlutterFlowTheme.of(context).bodyLarge.override(
                          fontFamily: 'Inter',
                          color: Color(0xFFE0E0E0),
                          letterSpacing: 0.0,
                        ),
                      ),
                    ].divide(SizedBox(height: 8)),
                  ),
                ),
              ),
              Expanded(
                child: Container(
                  width: MediaQuery.sizeOf(context).width,
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
                    child: SingleChildScrollView(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Padding(
                            padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
                            child: Text(
                              'Сегодня',
                              style: FlutterFlowTheme.of(context)
                                  .headlineSmall
                                  .override(
                                fontFamily: 'Inter Tight',
                                letterSpacing: 0.0,
                              ),
                            ),
                          ),
                          Padding(
                            padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
                            child: Material(
                              color: Colors.transparent,
                              elevation: 2,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Container(
                                width: MediaQuery.sizeOf(context).width,
                                decoration: BoxDecoration(
                                  color: FlutterFlowTheme.of(context)
                                      .primaryBackground,
                                  borderRadius: BorderRadius.circular(12),
                                ),
                                child: Padding(
                                  padding: EdgeInsetsDirectional.fromSTEB(
                                      16, 16, 16, 16),
                                  child: Column(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                        children: [
                                          Text(
                                            'Высшая математика',
                                            style: FlutterFlowTheme.of(context)
                                                .titleMedium
                                                .override(
                                              fontFamily: 'Inter Tight',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .primary,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                          Padding(
                                            padding:
                                            EdgeInsetsDirectional.fromSTEB(
                                                4, 12, 4, 12),
                                            child: Container(
                                              decoration: BoxDecoration(
                                                color: Color(0xFFE3F2FD),
                                                borderRadius:
                                                BorderRadius.circular(16),
                                              ),
                                              child: Padding(
                                                padding: EdgeInsets.all(8),
                                                child: Text(
                                                  'Уже началась',
                                                  style: FlutterFlowTheme.of(
                                                      context)
                                                      .bodySmall
                                                      .override(
                                                    fontFamily: 'Inter',
                                                    color:
                                                    Color(0xFF1565C0),
                                                    letterSpacing: 0.0,
                                                  ),
                                                ),
                                              ),
                                            ),
                                          ),
                                        ],
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.access_time,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            '9:00  - 10:30',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.location_on,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            'Кабинет 303',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                    ].divide(SizedBox(height: 12)),
                                  ),
                                ),
                              ),
                            ),
                          ),
                          Padding(
                            padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
                            child: Material(
                              color: Colors.transparent,
                              elevation: 2,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Container(
                                width: MediaQuery.sizeOf(context).width,
                                decoration: BoxDecoration(
                                  color: FlutterFlowTheme.of(context)
                                      .primaryBackground,
                                  borderRadius: BorderRadius.circular(12),
                                ),
                                child: Padding(
                                  padding: EdgeInsetsDirectional.fromSTEB(
                                      16, 16, 16, 16),
                                  child: Column(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                        children: [
                                          Text(
                                            'Программная инженерия',
                                            style: FlutterFlowTheme.of(context)
                                                .titleMedium
                                                .override(
                                              fontFamily: 'Inter Tight',
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                          Padding(
                                            padding:
                                            EdgeInsetsDirectional.fromSTEB(
                                                4, 12, 4, 12),
                                            child: Container(
                                              decoration: BoxDecoration(
                                                color: Color(0xFFFFF3E0),
                                                borderRadius:
                                                BorderRadius.circular(16),
                                              ),
                                              child: Padding(
                                                padding: EdgeInsets.all(8),
                                                child: Text(
                                                  'Скоро будет',
                                                  style: FlutterFlowTheme.of(
                                                      context)
                                                      .bodySmall
                                                      .override(
                                                    fontFamily: 'Inter',
                                                    color:
                                                    Color(0xFFFF6F00),
                                                    letterSpacing: 0.0,
                                                  ),
                                                ),
                                              ),
                                            ),
                                          ),
                                        ],
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.access_time,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            '10:40 - 12:10',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.location_on,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            'Кабинет 201',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                    ].divide(SizedBox(height: 12)),
                                  ),
                                ),
                              ),
                            ),
                          ),
                          Padding(
                            padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
                            child: Text(
                              'Завтра',
                              style: FlutterFlowTheme.of(context)
                                  .headlineSmall
                                  .override(
                                fontFamily: 'Inter Tight',
                                letterSpacing: 0.0,
                              ),
                            ),
                          ),
                          Padding(
                            padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
                            child: Material(
                              color: Colors.transparent,
                              elevation: 2,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Container(
                                width: MediaQuery.sizeOf(context).width,
                                decoration: BoxDecoration(
                                  color: FlutterFlowTheme.of(context)
                                      .primaryBackground,
                                  borderRadius: BorderRadius.circular(12),
                                ),
                                child: Padding(
                                  padding: EdgeInsetsDirectional.fromSTEB(
                                      16, 16, 16, 16),
                                  child: Column(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Text(
                                        'Програмная инженерия',
                                        style: FlutterFlowTheme.of(context)
                                            .titleMedium
                                            .override(
                                          fontFamily: 'Inter Tight',
                                          letterSpacing: 0.0,
                                        ),
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.access_time,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            '9:30 AM - 11:00 AM',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.location_on,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            'Room 405, Humanities Building',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                    ].divide(SizedBox(height: 12)),
                                  ),
                                ),
                              ),
                            ),
                          ),
                          Padding(
                            padding: EdgeInsetsDirectional.fromSTEB(8, 0, 8, 0),
                            child: Material(
                              color: Colors.transparent,
                              elevation: 2,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Container(
                                width: MediaQuery.sizeOf(context).width,
                                decoration: BoxDecoration(
                                  color: FlutterFlowTheme.of(context)
                                      .primaryBackground,
                                  borderRadius: BorderRadius.circular(12),
                                ),
                                child: Padding(
                                  padding: EdgeInsetsDirectional.fromSTEB(
                                      16, 16, 16, 16),
                                  child: Column(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Text(
                                        'Physics Lab',
                                        style: FlutterFlowTheme.of(context)
                                            .titleMedium
                                            .override(
                                          fontFamily: 'Inter Tight',
                                          letterSpacing: 0.0,
                                        ),
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.access_time,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            '2:00 PM - 3:30 PM',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                      Row(
                                        mainAxisSize: MainAxisSize.max,
                                        children: [
                                          Icon(
                                            Icons.location_on,
                                            color: FlutterFlowTheme.of(context)
                                                .secondaryText,
                                            size: 20,
                                          ),
                                          Text(
                                            'Lab 102, Science Building',
                                            style: FlutterFlowTheme.of(context)
                                                .bodyMedium
                                                .override(
                                              fontFamily: 'Inter',
                                              color: FlutterFlowTheme.of(
                                                  context)
                                                  .secondaryText,
                                              letterSpacing: 0.0,
                                            ),
                                          ),
                                        ].divide(SizedBox(width: 8)),
                                      ),
                                    ].divide(SizedBox(height: 12)),
                                  ),
                                ),
                              ),
                            ),
                          ),
                        ].divide(SizedBox(height: 16)),
                      ),
                    ),
                  ),
                ),
              ),
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
                        Column(
                          mainAxisSize: MainAxisSize.min,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            Icon(
                              Icons.home_outlined,
                              color: FlutterFlowTheme.of(context).primary,
                              size: 24,
                            ),
                            Text(
                              'Главная',
                              style: FlutterFlowTheme.of(context)
                                  .bodySmall
                                  .override(
                                fontFamily: 'Inter',
                                color: FlutterFlowTheme.of(context)
                                    .primary,
                                letterSpacing: 0.0,
                              ),
                            ),
                          ].divide(SizedBox(height: 4)),
                        ),
                        Column(
                          mainAxisSize: MainAxisSize.min,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            InkWell(
                              onTap: () {
                                Navigator.pushNamed(context, '/timetable');
                              },
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: [
                                  Icon(
                                    Icons.calendar_today,
                                    color: FlutterFlowTheme.of(context).secondaryText,
                                    size: 16,
                                  ),
                                  Text(
                                    'Расписание',
                                    style: FlutterFlowTheme.of(context)
                                        .bodySmall
                                        .override(
                                      fontFamily: 'Inter',
                                      color: FlutterFlowTheme.of(context).secondaryText,
                                      letterSpacing: 0.0,
                                    ),
                                  ),
                                ].divide(SizedBox(height: 4)),
                              ),
                            ),
                          ],
                        ),

                        Column(
                          mainAxisSize: MainAxisSize.min,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            Icon(
                              Icons.menu_book,
                              color: FlutterFlowTheme.of(context).secondaryText,
                              size: 24,
                            ),
                            Text(
                              'Курсы',
                              style: FlutterFlowTheme.of(context)
                                  .bodySmall
                                  .override(
                                fontFamily: 'Inter',
                                color: FlutterFlowTheme.of(context)
                                    .secondaryText,
                                letterSpacing: 0.0,
                              ),
                            ),
                          ].divide(SizedBox(height: 4)),
                        ),
                        Column(
                          mainAxisSize: MainAxisSize.min,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            InkWell(
                              onTap: () {
                                // Переход на страницу профиля
                                Navigator.pushNamed(context, '/auths');
                              },
                              child: Column(
                                mainAxisSize: MainAxisSize.min,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: [
                                  Icon(
                                    Icons.person_outline,
                                    color: FlutterFlowTheme.of(context).secondaryText,
                                    size: 24,
                                  ),
                                  Text(
                                    'Профиль',
                                    style: FlutterFlowTheme.of(context)
                                        .bodySmall
                                        .override(
                                      fontFamily: 'Inter',
                                      color: FlutterFlowTheme.of(context).secondaryText,
                                      letterSpacing: 0.0,
                                    ),
                                  ),
                                ].divide(SizedBox(height: 4)),
                              ),
                            ),
                          ],
                        )
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
