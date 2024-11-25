import 'package:flutter/material.dart';
import 'package:timetableapp/services/auth_service.dart';

class RegisterScreen extends StatefulWidget {
  @override
  _RegisterScreenState createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final TextEditingController nameController = TextEditingController();
  final TextEditingController surnameController = TextEditingController();
  final TextEditingController loginController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  final AuthService authService = AuthService('https://localhost:7049');

  Future<void> register() async {
    try {
      await authService.registerStudent({
        'name': nameController.text,
        'surname': surnameController.text,
        'login': loginController.text,
        'password': passwordController.text,
      });

      Navigator.pop(context); // Возвращаемся на экран входа
    } catch (error) {
      showDialog(
        context: context,
        builder: (_) => AlertDialog(
          title: Text('Ошибка'),
          content: Text(error.toString()),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Регистрация')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            TextField(
              controller: nameController,
              decoration: InputDecoration(labelText: 'Имя'),
            ),
            TextField(
              controller: surnameController,
              decoration: InputDecoration(labelText: 'Фамилия'),
            ),
            TextField(
              controller: loginController,
              decoration: InputDecoration(labelText: 'Логин'),
            ),
            TextField(
              controller: passwordController,
              decoration: InputDecoration(labelText: 'Пароль'),
              obscureText: true,
            ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: register,
              child: Text('Зарегистрироваться'),
            ),
          ],
        ),
      ),
    );
  }
}
