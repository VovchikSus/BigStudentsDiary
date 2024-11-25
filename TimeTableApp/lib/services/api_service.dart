import 'dart:convert';
import 'package:dio/dio.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ApiService {
  static final Dio _dio = Dio();

  /// Получение списка групп (не требует токена)
  static Future<List<Map<String, dynamic>>> getGroups() async {
    try {
      final response = await _dio.get('https://localhost:7049/api/Groups');
      return List<Map<String, dynamic>>.from(response.data['result']);
    } catch (e) {
      throw Exception('Ошибка при загрузке групп: $e');
    }
  }

  /// Авторизация студента
  static Future<void> login(String login, String password) async {
    try {
      final response = await _dio.post(
        'https://localhost:7049/student/login',
        data: jsonEncode({'studentLogin': login, 'studentPassword': password}),
        options: Options(headers: {'Content-Type': 'application/json'}),
      );

      // Проверяем, что токен присутствует в ответе
      if (response.data != null && response.data is String) {
        final token = response.data; // JWT-токен

        // Сохраняем токен в SharedPreferences
        final prefs = await SharedPreferences.getInstance();
        await prefs.setString('Authorization', token);

        // Устанавливаем токен в заголовок Authorization для всех будущих запросов
        _dio.options.headers['Authorization'] = 'Bearer $token';
      } else {
        throw Exception('Некорректный формат токена в ответе');
      }
    } catch (e) {
      throw Exception('Ошибка при авторизации: $e');
    }
  }

  /// Регистрация студента
  static Future<void> register(
      String name,
      String surname,
      String login,
      String password,
      int groupId,
      ) async {
    try {
      final data = {
        'name': name,
        'surname': surname,
        'studentLogin': login,
        'studentPassword': password,
        'groupId': groupId,
      };

      final response = await _dio.post(
        'https://localhost:7049/student/register',
        data: jsonEncode(data),
        options: Options(headers: {'Content-Type': 'application/json'}),
      );

      if (response.statusCode != 200) {
        throw Exception('Ошибка регистрации: ${response.data}');
      }
    } catch (e) {
      throw Exception('Ошибка при регистрации: $e');
    }
  }

  /// Выход из системы (удаление токена)
  static Future<void> logout() async {
    try {
      // Удаляем токен из SharedPreferences
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove('Authorization');

      // Убираем токен из заголовков
      _dio.options.headers.remove('Authorization');
    } catch (e) {
      throw Exception('Ошибка при выходе: $e');
    }
  }

  /// Пример запроса с использованием токена
  static Future<List<dynamic>> getSchedule(String formattedDate) async {
    try {
      // Загружаем токен из SharedPreferences
      final prefs = await SharedPreferences.getInstance();
      final token = prefs.getString('Authorization');

      if (token == null) {
        throw Exception('Токен не найден. Авторизуйтесь заново.');
      }

      // Устанавливаем токен в заголовки
      _dio.options.headers['Authorization'] = 'Bearer $token';

      final response = await _dio.get('https://localhost:7049/date/$formattedDate');
      return response.data as List<dynamic>;
    } catch (e) {
      throw Exception('Ошибка при загрузке расписания: $e');
    }
  }
}
