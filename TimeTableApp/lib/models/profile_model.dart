import 'package:flutter/material.dart';

/// Класс модели для управления данными профиля.
class ProfileModel {
  final String userName;
  final List<String> achievements;

  ProfileModel({
    this.userName = "Default User",
    this.achievements = const [],
  });

  /// Метод для очистки ресурсов, если это требуется.
  void dispose() {
    // Очистка ресурсов, если это нужно (например, потоки или таймеры).
  }
}

/// Функция для создания модели.
///
/// Она позволяет удобно создавать экземпляры модели
/// с возможной привязкой к контексту, если это потребуется.
T createModel<T>(BuildContext context, T Function() modelCreator) {
  return modelCreator();
}
