import 'dart:convert';
import 'package:http/http.dart' as http;

class AuthService {
  static const String _baseUrl = 'https://localhost:7049';

  Future<void> loginStudent(String login, String password) async {
    final url = Uri.parse('https://localhost:7049/student/login');
    final headers = {'Content-Type': 'application/json', 'Accept': '*/*'};
    final body = jsonEncode({'studentLogin': login, 'studentPassword': password});

    final response = await http.post(url, headers: headers, body: body);

    if (response.statusCode == 200) {
      print('Token: ${response.body}');
    } else if (response.statusCode == 401) {
      print('Unauthorized');
    } else {
      print('Error: ${response.statusCode}');
    }
  }

  Future<Map<String, dynamic>> loginAsTeacher(String login, String password) async {
    final url = Uri.parse('$_baseUrl/teacher/login');
    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'login': login, 'password': password}),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        if (data['token'] is String) {
          return {'success': true, 'token': data['token']};
        } else {
          throw TypeError();
        }
      } else {
        return {'success': false, 'message': 'Ошибка сервера: ${response.statusCode}'};
      }
    } catch (e) {
      return {'success': false, 'message': 'Ошибка: $e'};
    }
  }
}
