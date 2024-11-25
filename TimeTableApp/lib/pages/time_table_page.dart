import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';

class TimeTablePage extends StatefulWidget {
  const TimeTablePage({super.key});

  @override
  State<TimeTablePage> createState() => _TimeTablePageState();
}

class _TimeTablePageState extends State<TimeTablePage> {
  late String _selectedDate;
  List<dynamic>? _schedule;
  String? _errorMessage;
  late String _token;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    // Получаем токен из аргументов
    _token = ModalRoute.of(context)!.settings.arguments as String;
    // Устанавливаем текущую дату в формате yyyy-MM-dd
    _selectedDate = DateFormat('yyyy-MM-dd').format(DateTime.now());
    _fetchSchedule(_selectedDate);
  }

  Future<void> _fetchSchedule(String date) async {
    // Формируем URL для API
    final apiUrl = 'https://localhost:7049/date/$date';
    print('Requesting schedule from: $apiUrl'); // Для отладки
    try {
      final response = await http.get(
        Uri.parse(apiUrl),
        headers: {
          'Authorization': 'Bearer $_token',
          'Content-Type': 'application/json',
        },
      );

      if (response.statusCode == 200) {
        setState(() {
          _schedule = jsonDecode(response.body);
          _errorMessage = null;
        });
      } else {
        setState(() {
          _errorMessage =
          'Failed to load schedule. Status code: ${response.statusCode}';
        });
      }
    } catch (e) {
      setState(() {
        _errorMessage = 'An error occurred: $e';
      });
    }
  }

  Future<void> _selectDate(BuildContext context) async {
    final selectedDate = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime(2000),
      lastDate: DateTime(2100),
    );

    if (selectedDate != null) {
      final formattedDate = DateFormat('yyyy-MM-dd').format(selectedDate);
      setState(() {
        _selectedDate = formattedDate;
      });
      _fetchSchedule(formattedDate);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Time Table')),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: TextField(
              decoration: const InputDecoration(
                labelText: 'Selected Date',
                hintText: 'yyyy-MM-dd',
              ),
              controller: TextEditingController(text: _selectedDate),
              readOnly: true,
              onTap: () => _selectDate(context),
            ),
          ),
          if (_errorMessage != null)
            Text(_errorMessage!, style: const TextStyle(color: Colors.red)),
          if (_schedule != null)
            Expanded(
              child: ListView.builder(
                itemCount: _schedule!.length,
                itemBuilder: (context, index) {
                  final item = _schedule![index];
                  return ListTile(
                    title: Text(item['subjectName']),
                    subtitle: Text(item['time']),
                  );
                },
              ),
            ),
        ],
      ),
    );
  }
}
