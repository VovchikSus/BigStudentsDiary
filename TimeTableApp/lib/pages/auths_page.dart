import 'package:flutter/material.dart';
import 'package:timetableapp/services/api_service.dart';
import 'package:timetableapp/services/log_service.dart';
import 'package:timetableapp/utils/storage_helper.dart';

import '../flutter_flow/ff_button_widget.dart';
import '../flutter_flow/flutter_flow_theme.dart';
import '../models/AuthPageModel.dart';

class AuthPageWidget extends StatefulWidget {
  const AuthPageWidget({super.key});

  @override
  State<AuthPageWidget> createState() => _AuthPageWidgetState();
}

class _AuthPageWidgetState extends State<AuthPageWidget> {
  late AuthPageModel _model;
  final scaffoldKey = GlobalKey<ScaffoldState>();
  List<Map<String, dynamic>> groups = [];
  int? selectedGroupId;
  bool isLoading = false;

  @override
  void initState() {
    super.initState();
    _model = AuthPageModel();
    _initControllers();
    _loadGroups();
  }

  void _initControllers() {
    _model.textController1 ??= TextEditingController();
    _model.textFieldFocusNode1 ??= FocusNode();
    _model.textController2 ??= TextEditingController();
    _model.textFieldFocusNode2 ??= FocusNode();
    _model.textController3 ??= TextEditingController();
    _model.textFieldFocusNode3 ??= FocusNode();
    _model.textController4 ??= TextEditingController();
    _model.textFieldFocusNode4 ??= FocusNode();
    _model.textController5 ??= TextEditingController();
    _model.textFieldFocusNode5 ??= FocusNode();
    _model.textController6 ??= TextEditingController();
    _model.textFieldFocusNode6 ??= FocusNode();
  }

  Future<void> _loadGroups() async {
    try {
      final data = await ApiService.getGroups();
      setState(() => groups = data);
    } catch (e) {
      LogService.error('Ошибка загрузки групп', e);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка загрузки групп: $e')),
      );
    }
  }

  Future<void> _handleLogin() async {
    setState(() => isLoading = true);
    try {
      final token = await ApiService.login(
        _model.textController1!.text,
        _model.textController2!.text,
      );

     // await StorageHelper.saveToken('$token');

      Navigator.pushNamed(context, '/user_profile');
    } catch (e) {
      LogService.error('Ошибка входа', e);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Неверный логин или пароль')),
      );
    } finally {
      setState(() => isLoading = false);
    }
  }

  Future<void> _handleRegister() async {
    if (selectedGroupId == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Выберите группу')),
      );
      return;
    }

    setState(() => isLoading = true);
    try {
      await ApiService.register(
        _model.textController3!.text,
        _model.textController4!.text,
        _model.textController5!.text,
        _model.textController6!.text,
        selectedGroupId!,
      );
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Регистрация успешна!')),
      );
      _model.safeSetState(() {
        _model.passwordVisibility1 = false;
        _model.passwordVisibility2 = false;
      }, this);
    } catch (e) {
      LogService.error('Ошибка регистрации', e);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка регистрации: $e')),
      );
    } finally {
      setState(() => isLoading = false);
    }
  }

  Widget _buildGroupDropdown() {
    return DropdownButtonFormField<int>(
      value: selectedGroupId,
      decoration: InputDecoration(
        filled: true,
        fillColor: Color(0xFFF5F5F5),
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(8),
          borderSide: BorderSide(color: Color(0xFFE0E0E0)),
        ),
      ),
      hint: Text('Выберите группу'),
      items: groups.map((group) => DropdownMenuItem<int>(
        value: group['groupId'],
        child: Text(group['groupCode']),
      )).toList(),
      onChanged: (value) => setState(() => selectedGroupId = value),
    );
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
          child: SingleChildScrollView(
            child: Container(
              height: MediaQuery.sizeOf(context).height,
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [Color(0xFF1A237E), Color(0xFF3F51B5)],
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                ),
              ),
              child: Padding(
                padding: EdgeInsets.all(24),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'С возвращением',
                      style: FlutterFlowTheme.of(context).headlineLarge.override(
                        fontFamily: 'Inter Tight',
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    SizedBox(height: 8),
                    Text(
                      'Войти чтобы продолжить или создай новый аккаунт',
                      style: FlutterFlowTheme.of(context).bodyLarge.override(
                        fontFamily: 'Inter',
                        color: Color(0xFFE0E0E0),
                      ),
                    ),
                    SizedBox(height: 24),
                    _buildAuthForm(
                      title: 'Войти',
                      children: [
                        _buildTextField(
                          controller: _model.textController1!,
                          focusNode: _model.textFieldFocusNode1!,
                          label: 'Логин',
                        ),
                        _buildTextField(
                          controller: _model.textController2!,
                          focusNode: _model.textFieldFocusNode2!,
                          label: 'Пароль',
                          isPassword: true,
                          visibility: _model.passwordVisibility1,
                          onVisibilityChanged: () => _model.safeSetState(
                                () => _model.passwordVisibility1 = !_model.passwordVisibility1,
                            this,
                          ),
                        ),
                        FFButtonWidget(
                          onPressed: isLoading ? null : _handleLogin,
                          text: 'Войти',
                          options: FFButtonOptions(
                            width: double.infinity,
                            height: 50,
                            color: FlutterFlowTheme.of(context).primary,
                            textStyle: FlutterFlowTheme.of(context).titleSmall.override(
                              fontFamily: 'Inter Tight',
                              color: Colors.white,
                            ),
                            borderRadius: BorderRadius.circular(25),
                            // Добавили обязательные параметры
                            padding: EdgeInsets.zero,
                            iconPadding: EdgeInsets.zero,
                            elevation: 0,
                          ),
                        ),
                      ],
                    ),
                    SizedBox(height: 24),
                    _buildAuthForm(
                      title: 'Создать аккаунт',
                      children: [
                        _buildTextField(
                          controller: _model.textController3!,
                          focusNode: _model.textFieldFocusNode3!,
                          label: 'Имя',
                        ),
                        _buildTextField(
                          controller: _model.textController4!,
                          focusNode: _model.textFieldFocusNode4!,
                          label: 'Фамилия',
                        ),
                        _buildTextField(
                          controller: _model.textController5!,
                          focusNode: _model.textFieldFocusNode5!,
                          label: 'Логин',
                        ),
                        _buildTextField(
                          controller: _model.textController6!,
                          focusNode: _model.textFieldFocusNode6!,
                          label: 'Пароль',
                          isPassword: true,
                          visibility: _model.passwordVisibility2,
                          onVisibilityChanged: () => _model.safeSetState(
                                () => _model.passwordVisibility2 = !_model.passwordVisibility2,
                            this,
                          ),
                        ),
                        _buildGroupDropdown(),
                        SizedBox(height: 20),
                        FFButtonWidget(
                          onPressed: isLoading ? null : _handleRegister,
                          text: 'Зарегистрироваться',
                          options: FFButtonOptions(
                            width: double.infinity,
                            height: 50,
                            color: FlutterFlowTheme.of(context).primary,
                            textStyle: FlutterFlowTheme.of(context).titleSmall.override(
                              fontFamily: 'Inter Tight',
                              color: Colors.white,
                            ),
                            borderRadius: BorderRadius.circular(25), padding: EdgeInsets.zero, iconPadding: EdgeInsets.zero, elevation: 0,
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildAuthForm({required String title, required List<Widget> children}) {
    return Material(
      elevation: 4,
      color: Colors.transparent,
      child: Container(
        width: double.infinity,
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(16),
        ),
        child: Padding(
          padding: EdgeInsets.all(24),
          child: Column(
            children: [
              Text(
                title,
                style: FlutterFlowTheme.of(context).headlineSmall.override(
                  fontFamily: 'Inter Tight',
                  color: FlutterFlowTheme.of(context).primary,
                  fontWeight: FontWeight.w600,
                ),
              ),
              SizedBox(height: 20),
              ...children.map((child) => Padding(
                padding: EdgeInsets.only(bottom: 20),
                child: child,
              )),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required FocusNode focusNode,
    required String label,
    bool isPassword = false,
    bool visibility = false,
    VoidCallback? onVisibilityChanged,
  }) {
    return TextFormField(
      controller: controller,
      focusNode: focusNode,
      obscureText: isPassword && !visibility,
      decoration: InputDecoration(
        labelText: label,
        filled: true,
        fillColor: Color(0xFFF5F5F5),
        suffixIcon: isPassword
            ? IconButton(
          icon: Icon(visibility ? Icons.visibility : Icons.visibility_off),
          onPressed: onVisibilityChanged,
        )
            : null,
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(8),
          borderSide: BorderSide(color: Color(0xFFE0E0E0)),
        ),
      ),
    );
  }
}