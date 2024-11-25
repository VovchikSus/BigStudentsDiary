import 'package:flutter/material.dart';
import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class HomePage extends StatefulWidget {
  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final Dio dio = Dio();
  final storage = FlutterSecureStorage();
  String? data;

  @override
  void initState() {
    super.initState();
    dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) async {
          final token = await storage.read(key: 'authToken');
          if (token != null) {
            options.headers['Authorization'] = 'Bearer $token';
          }
          return handler.next(options);
        },
      ),
    );
    _fetchData();
  }

  void _fetchData() async {
    try {
      final response = await dio.get('https://localhost:7049/date/2024-11-21');
      setState(() {
        data = response.data.toString();
      });
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка загрузки данных')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Главная страница')),
      body: Center(
        child: data != null
            ? Text('Данные: $data')
            : CircularProgressIndicator(),
      ),
    );
  }
}
