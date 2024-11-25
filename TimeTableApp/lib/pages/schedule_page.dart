import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:dio/dio.dart';

class SchedulePage extends StatefulWidget {
  @override
  _SchedulePageState createState() => _SchedulePageState();
}

class _SchedulePageState extends State<SchedulePage> {
  DateTime selectedDate = DateTime.now();
  Map<String, List<dynamic>> weeklySchedule = {}; // Расписание по дням недели
  bool isLoading = false; // Для индикации загрузки
  final Dio dio = Dio(); // Для выполнения HTTP-запросов

  // Форматирование даты для API
  String formatDate(DateTime date) {
    final day = date.day.toString().padLeft(2, '0');
    final month = date.month.toString().padLeft(2, '0');
    final year = date.year.toString();
    return '$year-$month-$day';
  }

  // Получение списка дат на неделю
  List<DateTime> getWeekDates(DateTime startDate) {
    final weekDates = <DateTime>[];
    for (int i = 0; i < 7; i++) {
      weekDates.add(startDate.add(Duration(days: i)));
    }
    return weekDates;
  }

  // Загрузка расписания на неделю
  Future<void> fetchWeeklySchedule() async {
    final weekDates = getWeekDates(selectedDate);
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('Authorization');

    if (token == null) {
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Ошибка: Токен авторизации не найден'),
      ));
      return;
    }

    setState(() {
      isLoading = true;
      weeklySchedule = {};
    });

    try {
      for (var date in weekDates) {
        final formattedDate = formatDate(date);
        final url = 'https://localhost:7049/date/$formattedDate';

        final response = await dio.get(
          url,
          options: Options(headers: {
            'Authorization': 'Bearer $token',
            'Accept': 'application/json',
          }),
        );

        if (response.statusCode == 200) {
          final dailySchedule = response.data as List<dynamic>;

          // Сортировка занятий по времени начала
          dailySchedule.sort((a, b) {
            final timeA = DateTime.parse(a['timeStart']);
            final timeB = DateTime.parse(b['timeStart']);
            return timeA.compareTo(timeB);
          });

          setState(() {
            final readableDate = DateFormat('dd.MM.yyyy').format(date);
            weeklySchedule[readableDate] = dailySchedule;
          });
        }
      }
    } catch (e) {
      print('Ошибка: $e');
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text('Ошибка загрузки данных'),
      ));
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Выбор даты и обновление расписания
  Future<void> pickDate(BuildContext context) async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: selectedDate,
      firstDate: DateTime(2000),
      lastDate: DateTime(2100),
    );
    if (picked != null && picked != selectedDate) {
      setState(() {
        selectedDate = picked;
      });

      // Загружаем расписание на неделю
      fetchWeeklySchedule();
    }
  }

  @override
  void initState() {
    super.initState();
    fetchWeeklySchedule(); // Загружаем расписание сразу при открытии страницы
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Расписание')),
      body: Column(
        children: [
          // Выбор даты
          Row(
            children: [
              Text('Неделя начиная с: ${DateFormat('dd.MM.yyyy').format(selectedDate)}'),
              IconButton(
                icon: Icon(Icons.calendar_today),
                onPressed: () => pickDate(context),
              ),
            ],
          ),
          // Отображение расписания
          if (isLoading)
            Expanded(child: Center(child: CircularProgressIndicator()))
          else if (weeklySchedule.isNotEmpty)
            Expanded(
              child: ListView.builder(
                itemCount: weeklySchedule.keys.length,
                itemBuilder: (context, index) {
                  final date = weeklySchedule.keys.elementAt(index);
                  final lessons = weeklySchedule[date]!;
                  return Card(
                    margin: const EdgeInsets.symmetric(vertical: 8, horizontal: 16),
                    child: Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Дата: $date',
                            style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                          ),
                          Divider(),
                          ...lessons.map((lesson) => ListTile(
                            title: Text('${lesson['number']}. ${lesson['discipline']}'),
                            subtitle: Text('Время: ${lesson['timeRange']}\nЗдание: ${lesson['building']}, Аудитория: ${lesson['room']}'),
                          )),
                        ],
                      ),
                    ),
                  );
                },
              ),
            )
          else
            Expanded(child: Center(child: Text('Расписание отсутствует'))),
        ],
      ),
    );
  }
}
