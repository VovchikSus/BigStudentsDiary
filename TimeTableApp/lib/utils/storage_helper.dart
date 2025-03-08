import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class StorageHelper {
  static final _storage = FlutterSecureStorage();

  static Future<void> saveToken(String token) async {
    print('Saving token: $token'); // Добавьте это
    await _storage.write(key: 'Authorization', value: token);
  }

  static Future<String?> getToken() async {
    final token = await _storage.read(key: 'Authorization');
    print('Токен авторизации: $token'); // Добавьте это
    return token;
  }
}
