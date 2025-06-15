import 'package:dio/dio.dart';
import 'package:timetableapp/services/log_service.dart';
import 'package:timetableapp/utils/storage_helper.dart';

import '../constants/api_constants.dart';

class ApiService {
  static final Dio _dio = Dio(BaseOptions(
    baseUrl: ApiConstants.baseUrl,
    connectTimeout: Duration(seconds: 5),
    receiveTimeout: Duration(seconds: 3),
  ));

  static Future<void> _configureDio() async {
    final token = await StorageHelper.getToken();
    _dio.options.headers['Authorization'] = token != null ? 'Bearer $token' : null;
  }

  static Future<List<Map<String, dynamic>>> getGroups() async {
    try {
      await _configureDio();
      final response = await _dio.get('/api/Groups');
      return List<Map<String, dynamic>>.from(response.data['result']);
    } catch (e) {
      LogService.error('Ошибка загрузки групп', e);
      rethrow;
    }
  }

  static Future<void> login(String login, String password) async {
    try {
      final response = await _dio.post(
        '/student/login',
        data: {'studentLogin': login, 'studentPassword': password},
      );

      await StorageHelper.saveToken(response.data as String);
      _dio.options.headers['Authorization'] = 'Bearer ${response.data}';
    } catch (e) {
      LogService.error('Ошибка входа', e);
      rethrow;
    }
  }

  static Future<void> register(
      String name,
      String surname,
      String login,
      String password,
      int groupId,
      ) async {
    try {
      await _dio.post(
        '/student/register',
        data: {
          'studentName': name,
          'studentSurname': surname,
          'studentLogin': login,
          'studentPassword': password,
          'groupId': groupId,
        },
      );
    } catch (e) {
      LogService.error('Ошибка регистрации', e);
      rethrow;
    }
  }
}