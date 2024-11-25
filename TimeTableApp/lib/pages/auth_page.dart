import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:dio/dio.dart';
import 'package:timetableapp/services/api_service.dart';
import 'schedule_page.dart';


class AuthPage extends StatefulWidget {
  @override
  _AuthPageState createState() => _AuthPageState();
}

class _AuthPageState extends State<AuthPage> {
  bool isLogin = true; // Переключатель между входом и регистрацией
  final TextEditingController _loginController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _surnameController = TextEditingController();
  String selectedGroup = ''; // Выбранная группа
  int? selectedGroupId; // ID группы для регистрации
  List<Map<String, dynamic>> groups = []; // Список групп из API

  @override
  void initState() {
    super.initState();
    fetchGroups(); // Загрузка списка групп
  }

  Future<void> fetchGroups() async {
    try {
      final data = await ApiService.getGroups();
      setState(() {
        groups = data;
      });
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Ошибка при загрузке групп: $e'),
      ));
    }
  }

  Future<void> loginStudent() async {
    try {
      await ApiService.login(_loginController.text, _passwordController.text);
      Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => SchedulePage()),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Ошибка входа: $e'),
      ));
    }
  }

  Future<void> registerStudent() async {
    if (selectedGroupId == null) {
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Выберите группу'),
      ));
      return;
    }

    try {
      await ApiService.register(
        _nameController.text,
        _surnameController.text,
        _loginController.text,
        _passwordController.text,
        selectedGroupId!,
      );
      setState(() {
        isLogin = true;
      });
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Регистрация успешна'),
      ));
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Ошибка регистрации: $e'),
      ));
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(isLogin ? 'Вход' : 'Регистрация')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            ToggleButtons(
              children: [Text('Войти'), Text('Зарегистрироваться')],
              isSelected: [isLogin, !isLogin],
              onPressed: (index) {
                setState(() {
                  isLogin = index == 0;
                });
              },
            ),
            if (!isLogin)
              Column(
                children: [
                  TextField(
                    controller: _nameController,
                    decoration: InputDecoration(labelText: 'Имя'),
                  ),
                  TextField(
                    controller: _surnameController,
                    decoration: InputDecoration(labelText: 'Фамилия'),
                  ),
                ],
              ),
            TextField(
              controller: _loginController,
              decoration: InputDecoration(labelText: 'Логин'),
            ),
            TextField(
              controller: _passwordController,
              decoration: InputDecoration(labelText: 'Пароль'),
              obscureText: true,
            ),
            if (!isLogin)
              DropdownButtonFormField<String>(
                value: selectedGroup.isEmpty ? null : selectedGroup,
                hint: Text('Выберите группу'),
                items: groups.map((group) {
                  return DropdownMenuItem<String>(
                    value: group['groupCode'],
                    child: Text(group['groupCode']),
                  );
                }).toList(),
                onChanged: (value) {
                  setState(() {
                    selectedGroup = value!;
                    selectedGroupId = groups.firstWhere(
                            (group) => group['groupCode'] == value)['groupId'];
                  });
                },
              ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: isLogin ? loginStudent : registerStudent,
              child: Text(isLogin ? 'Войти' : 'Зарегистрироваться'),
            ),
          ],
        ),
      ),
    );
  }
}
