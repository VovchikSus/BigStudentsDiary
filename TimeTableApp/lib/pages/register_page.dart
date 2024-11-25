import 'package:flutter/material.dart';

class RegisterPage extends StatefulWidget {
  @override
  _RegisterPageState createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  final TextEditingController nameController = TextEditingController();
  final TextEditingController surnameController = TextEditingController();
  final TextEditingController loginController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  String selectedGroup = '';

  @override
  void initState() {
    super.initState();
    fetchGroups();
  }

  void fetchGroups() async {
    // TODO: Реализуйте API запрос для получения списка групп
    setState(() {
      selectedGroup = 'Группа 1'; // Пример значения, замените на API данные
    });
  }

  void registerUser() async {
    // TODO: Реализуйте API запрос для регистрации пользователя
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Регистрация')),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
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
            DropdownButton<String>(
              value: selectedGroup,
              onChanged: (newValue) {
                setState(() {
                  selectedGroup = newValue!;
                });
              },
              items: <String>['Группа 1', 'Группа 2', 'Группа 3'] // Замените на данные API
                  .map<DropdownMenuItem<String>>((String value) {
                return DropdownMenuItem<String>(
                  value: value,
                  child: Text(value),
                );
              }).toList(),
            ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: registerUser,
              child: Text('Зарегистрироваться'),
            ),
          ],
        ),
      ),
    );
  }
}
