import 'package:flutter/material.dart';

class AuthPageModel {
  // Контроллеры для текстовых полей
  TextEditingController? textController1;
  TextEditingController? textController2;
  TextEditingController? textController3;
  TextEditingController? textController4;
  TextEditingController? textController5;
  TextEditingController? textController6;

  // Узлы фокуса для текстовых полей
  FocusNode? textFieldFocusNode1;
  FocusNode? textFieldFocusNode2;
  FocusNode? textFieldFocusNode3;
  FocusNode? textFieldFocusNode4;
  FocusNode? textFieldFocusNode5;
  FocusNode? textFieldFocusNode6;

  // Видимость пароля
  bool passwordVisibility1 = false;
  bool passwordVisibility2 = false;

  // Валидаторы для текстовых полей
  String? Function(String?)? textController1Validator;
  String? Function(String?)? textController2Validator;
  String? Function(String?)? textController3Validator;
  String? Function(String?)? textController4Validator;
  String? Function(String?)? textController5Validator;
  String? Function(String?)? textController6Validator;

  // Конструктор
  AuthPageModel() {
    // В конструкторе AuthPageModel
    textController3Validator = (value) {
      if (value == null || value.isEmpty) return 'Введите имя';
      return null;
    };

    textController4Validator = (value) {
      if (value == null || value.isEmpty) return 'Введите фамилию';
      return null;
    };

    textController5Validator = (value) {
      if (value == null || value.isEmpty) return 'Введите логин';
      return null;
    };

    textController6Validator = (value) {
      if (value == null || value.isEmpty) return 'Введите пароль';
      if (value.length < 6) return 'Минимум 6 символов';
      return null;
    };
  }

  // Метод для преобразования валидатора
  String? Function(String?)? asValidator(
      String? Function(String?)? validator) {
    return validator;
  }

  // Метод для безопасного обновления состояния
  void safeSetState(VoidCallback callback, State state) {
    if (state.mounted) {
      state.setState(callback);
    }
  }

  // Очистка ресурсов
  void dispose() {
    textController1?.dispose();
    textController2?.dispose();
    textController3?.dispose();
    textController4?.dispose();
    textController5?.dispose();
    textController6?.dispose();

    textFieldFocusNode1?.dispose();
    textFieldFocusNode2?.dispose();
    textFieldFocusNode3?.dispose();
    textFieldFocusNode4?.dispose();
    textFieldFocusNode5?.dispose();
    textFieldFocusNode6?.dispose();
  }
}
