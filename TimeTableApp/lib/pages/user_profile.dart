import '../models/profile_model.dart';
import '/flutter_flow/flutter_flow_icon_button.dart';
import '/flutter_flow/flutter_flow_theme.dart';
import '/flutter_flow/flutter_flow_util.dart';
import 'package:flutter/material.dart';

export 'user_profile.dart';

class ProfileWidget extends StatefulWidget {
  const ProfileWidget({super.key});

  @override
  State<ProfileWidget> createState() => _ProfileWidgetState();
}

class _ProfileWidgetState extends State<ProfileWidget> {
  late ProfileModel _model;
  final scaffoldKey = GlobalKey<ScaffoldState>();

  @override
  void initState() {
    super.initState();
    _model = createModel(context, () => ProfileModel());
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
        // Удален AppBar (верхняя панель с кнопкой "назад")

        body: SafeArea(
          top: true,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              // Header Section
              Container(
                width: MediaQuery.sizeOf(context).width,
                height: 220,
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
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        mainAxisSize: MainAxisSize.max,
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Профиль',
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
                          Container(
                            width: 40,
                            height: 40,
                            decoration: BoxDecoration(
                              color: const Color(0x33FFFFFF),
                              borderRadius: BorderRadius.circular(20),
                            ),
                            child: const Icon(
                              Icons.settings,
                              color: Colors.white,
                              size: 24,
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 16),
                      Row(
                        mainAxisSize: MainAxisSize.max,
                        children: [
                          Material(
                            color: Colors.transparent,
                            elevation: 4,
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(40),
                            ),
                            child: Container(
                              width: 80,
                              height: 80,
                              decoration: BoxDecoration(
                                color: FlutterFlowTheme.of(context)
                                    .secondaryBackground,
                                borderRadius: BorderRadius.circular(40),
                              ),
                              child: ClipRRect(
                                borderRadius: BorderRadius.circular(40),
                                child: Image.network(
                                  'https://images.unsplash.com/photo-1511367461989-f85a21fda167?q=80&w=1931&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
                                  width: 80,
                                  height: 80,
                                  fit: BoxFit.cover,
                                ),
                              ),
                            ),
                          ),
                          Column(
                            mainAxisSize: MainAxisSize.min,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Padding(
                                padding: EdgeInsetsDirectional.fromSTEB(4, 0, 4, 0),
                                child: Text(
                                  'Владимир Сусов',
                                  style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 20,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsetsDirectional.fromSTEB(4, 0, 4, 0),
                                child: Text(
                                  'Прикладная информатика',
                                  style: FlutterFlowTheme.of(context)
                                      .bodyMedium
                                      .override(
                                    fontFamily: 'Inter',
                                    color: const Color(0xFFE0E0E0),
                                    letterSpacing: 0.0,
                                  ),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsetsDirectional.fromSTEB(
                                    4, 12, 4, 12),
                                child: Container(
                                  decoration: BoxDecoration(
                                    color: const Color(0x33FFFFFF),
                                    borderRadius: BorderRadius.circular(16),
                                  ),
                                  child: const Padding(
                                    padding: EdgeInsets.all(8),
                                    child: Text(
                                      '4 курс',
                                      style: TextStyle(
                                        color: Colors.white,
                                        fontSize: 14,
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ].divide(const SizedBox(width: 16)),
                      ),
                    ],
                  ),
                ),
              ),
              // Content Section
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
                          Material(
                            color: Colors.transparent,
                            elevation: 2,
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(16),
                            ),
                            child: Container(
                              width: MediaQuery.sizeOf(context).width,
                              decoration: BoxDecoration(
                                color: FlutterFlowTheme.of(context)
                                    .primaryBackground,
                                borderRadius: BorderRadius.circular(16),
                              ),
                              child: Padding(
                                padding: const EdgeInsetsDirectional.fromSTEB(
                                    20, 20, 20, 20),
                                child: Column(
                                  mainAxisSize: MainAxisSize.min,
                                  children: [
                                    Row(
                                      mainAxisSize: MainAxisSize.max,
                                      mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                      children: [
                                        Text(
                                          'Заметки',
                                          style: FlutterFlowTheme.of(context)
                                              .headlineSmall
                                              .override(
                                            fontFamily: 'Inter Tight',
                                            letterSpacing: 0.0,
                                          ),
                                        ),
                                        Icon(
                                          Icons.add_circle_outline,
                                          color: FlutterFlowTheme.of(context)
                                              .primary,
                                          size: 24,
                                        ),
                                      ],
                                    ),
                                    const SizedBox(height: 16),
                                    Container(
                                      width: MediaQuery.sizeOf(context).width,
                                      decoration: BoxDecoration(
                                        color: const Color(0xFFF5F5F5),
                                        borderRadius: BorderRadius.circular(12),
                                      ),
                                      child: Padding(
                                        padding: const EdgeInsetsDirectional.fromSTEB(
                                            16, 16, 16, 16),
                                        child: Column(
                                          mainAxisSize: MainAxisSize.min,
                                          children: [
                                            Row(
                                              mainAxisSize: MainAxisSize.max,
                                              mainAxisAlignment:
                                              MainAxisAlignment
                                                  .spaceBetween,
                                              children: [
                                                Text(
                                                  'Высшая математика',
                                                  style: FlutterFlowTheme.of(
                                                      context)
                                                      .bodyLarge
                                                      .override(
                                                    fontFamily: 'Inter',
                                                    color:
                                                    FlutterFlowTheme.of(
                                                        context)
                                                        .primary,
                                                    letterSpacing: 0.0,
                                                  ),
                                                ),
                                                Text(
                                                  'Сегодня',
                                                  style: FlutterFlowTheme.of(
                                                      context)
                                                      .bodySmall
                                                      .override(
                                                    fontFamily: 'Inter',
                                                    color:
                                                    FlutterFlowTheme.of(
                                                        context)
                                                        .secondaryText,
                                                    letterSpacing: 0.0,
                                                  ),
                                                ),
                                              ],
                                            ),
                                            const SizedBox(height: 12),
                                            Text(
                                              'Глава 7: Обзор дифференциальных уравнений и ключевые концепции для промежуточного экзамена',
                                              style:
                                              FlutterFlowTheme.of(context)
                                                  .bodyMedium
                                                  .override(
                                                fontFamily: 'Inter',
                                                letterSpacing: 0.0,
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),
                                    ),
                                    const SizedBox(height: 16),
                                    Container(
                                      width: MediaQuery.sizeOf(context).width,
                                      decoration: BoxDecoration(
                                        color: const Color(0xFFF5F5F5),
                                        borderRadius: BorderRadius.circular(12),
                                      ),
                                      child: Padding(
                                        padding: const EdgeInsetsDirectional.fromSTEB(
                                            16, 16, 16, 16),
                                        child: Column(
                                          mainAxisSize: MainAxisSize.min,
                                          children: [
                                            Row(
                                              mainAxisSize: MainAxisSize.max,
                                              mainAxisAlignment:
                                              MainAxisAlignment
                                                  .spaceBetween,
                                              children: [
                                                Text(
                                                  'Компьютерные технологии',
                                                  style: FlutterFlowTheme.of(
                                                      context)
                                                      .bodyLarge
                                                      .override(
                                                    fontFamily: 'Inter',
                                                    letterSpacing: 0.0,
                                                  ),
                                                ),
                                                Text(
                                                  'Вчера',
                                                  style: FlutterFlowTheme.of(
                                                      context)
                                                      .bodySmall
                                                      .override(
                                                    fontFamily: 'Inter',
                                                    color:
                                                    FlutterFlowTheme.of(
                                                        context)
                                                        .secondaryText,
                                                    letterSpacing: 0.0,
                                                  ),
                                                ),
                                              ],
                                            ),
                                            const SizedBox(height: 12),
                                            Text(
                                              'Анализ сложности алгоритма и примеры нотации Big O',
                                              style:
                                              FlutterFlowTheme.of(context)
                                                  .bodyMedium
                                                  .override(
                                                fontFamily: 'Inter',
                                                letterSpacing: 0.0,
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),

        bottomNavigationBar: BottomNavigationBar(
          currentIndex: 3,
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
          },
        ),
      ),
    );
  }
}