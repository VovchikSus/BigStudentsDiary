import 'package:logger/logger.dart';

class LogService {
  static final Logger _logger = Logger(
    printer: PrettyPrinter(
      methodCount: 2, // Показывать стек из 2 методов
      errorMethodCount: 5, // Показывать стек ошибок из 5 методов
      lineLength: 80, // Длина строки
      colors: true, // Включить цвета
      printEmojis: true, // Использовать эмодзи
      printTime: true, // Печатать время
    ),
  );

  static void debug(String message) {
    _logger.d(message);
  }

  static void info(String message) {
    _logger.i(message);
  }

  static void warn(String message) {
    _logger.w(message);
  }

  static void error(String message, [dynamic error, StackTrace? stackTrace]) {
    _logger.e(message, error, stackTrace);
  }
}
