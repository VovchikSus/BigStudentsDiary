import 'package:flutter/material.dart';

/// Класс модели для управления данными расписания.
class TimetableModel {
  final List<String> schedule;

  TimetableModel({
    this.schedule = const [],
  });

  /// Метод для очистки ресурсов, если требуется.
  void dispose() {
    // Очистка ресурсов, если нужно (например, потоки или таймеры).
  }
}

/// Функция для создания модели.
///
/// Она позволяет создавать экземпляры модели с привязкой к контексту, если нужно.
T createModel<T>(BuildContext context, T Function() modelCreator) {
  return modelCreator();
}
