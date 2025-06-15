import 'dart:convert';
import 'dart:math';
import 'package:dio/dio.dart';
import 'package:timetableapp/constants/api_constants.dart';
import 'package:timetableapp/utils/storage_helper.dart';

import '../models/Note.dart';

class GraphApi {
  static final Dio _dio = Dio();

  static Future<List<Note>> getNotes(String studentId, String discipline) async {
    try {
      final token = await StorageHelper.getToken();
      final response = await _dio.get(
        '${ApiConstants.baseUrl}/api/Notes/$studentId/$discipline',
        options: Options(headers: ApiConstants.authHeaders(token!)),
      );
      return (response.data as List).map((e) => Note.fromJson(e)).toList();
    } on DioException catch (e) {
      throw Exception('Ошибка загрузки заметок: ${e.message}');
    }
  }
  static Future<void> saveNote(String studentId, String discipline, Note note) async {
    try {
      final token = await StorageHelper.getToken();
      await _dio.put(
        '${ApiConstants.baseUrl}/api/Notes',
        data: {
          'noteId': note.noteId,
          'studentId': studentId,
          'discipline': discipline,
          'lessonNumber': note.lessonNumber,
          'content': note.content,
        },
        options: Options(headers: ApiConstants.authHeaders(token!)),
      );
    } on DioException catch (e) {
      throw Exception('Ошибка сохранения: ${e.message}');
    }
  }

  static Future<Map<String, dynamic>> fetchGroupDisciplines() async {
    try {
      final token = await StorageHelper.getToken();
      if (token == null) throw Exception('Токен не найден');

      final claims = parseJwt(token);
      final groupId = claims['groupId'];

      // Первый запрос: получаем базовые данные дисциплин
      final groupResponse = await _dio.get(
        '${ApiConstants.baseUrl}/api/Discipline/group/$groupId',
        options: Options(headers: ApiConstants.authHeaders(token)),
      );

      // Парсим список дисциплин с ID и количеством занятий
      final baseDisciplines = (groupResponse.data as List).map((item) {
        return {
          'disciplineId': item['disciplineId'],
          'totalLessons': item['totalLessons'],
        };
      }).toList();

      // Второй запрос: получаем названия для каждой дисциплины
      final disciplines = await Future.wait(
        baseDisciplines.map((item) async {
          try {
            final detailResponse = await _dio.get(
              '${ApiConstants.baseUrl}/api/Discipline/${item['disciplineId']}',
              options: Options(headers: ApiConstants.authHeaders(token)),
            );

            // Извлекаем название из вложенного объекта result
            final disciplineName = (detailResponse.data['result'] as List)
                .first['discipline'] as String;

            return {
              'discipline': disciplineName,
              'totalLessons': item['totalLessons'],
              'disciplineId': item['disciplineId'],
            };
          } catch (e) {
            print('Ошибка загрузки дисциплины ${item['disciplineId']}: $e');
            return null;
          }
        }),
      );

      return {
        'nodes': disciplines.whereType<Map<String, dynamic>>().toList(),
        'links': _generateRelations(disciplines.whereType<Map<String, dynamic>>().toList()),
      };
    } on DioException catch (e) {
      throw Exception('Ошибка: ${e.response?.data ?? e.message}');
    }
  }

  static List<Map<String, dynamic>> _generateRelations(List<Map<String, dynamic>> disciplines) {
    final links = <Map<String, dynamic>>[];
    final rnd = Random();

    for (int i = 0; i < disciplines.length; i++) {
      if (i > 0 && rnd.nextDouble() > 0.7) {
        links.add({
          'source': disciplines[i]['discipline'],
          'target': disciplines[i-1]['discipline']
        });
      }
    }

    return links;
  }

  static Map<String, dynamic> parseJwt(String token) {
    final parts = token.split('.');
    if (parts.length != 3) throw Exception('Invalid token');
    final payload = base64Url.normalize(parts[1]);
    final jsonString = utf8.decode(base64Url.decode(payload));
    return json.decode(jsonString) as Map<String, dynamic>;
  }


}
