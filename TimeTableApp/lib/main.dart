import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart'; // Добавляем импорт
import 'package:intl/date_symbol_data_local.dart'; // Добавляем импорт
import 'package:timetableapp/pages/auth_page.dart';
import 'package:timetableapp/pages/auths_page.dart';
import 'package:timetableapp/pages/knowledge_graph_page.dart';
import 'package:timetableapp/pages/main_page.dart';
import 'package:timetableapp/pages/notifications_page.dart';
import 'package:timetableapp/pages/timetable_page.dart';
import 'package:timetableapp/pages/user_profile.dart';
import 'package:timetableapp/services/notification_service.dart';
import 'package:timetableapp/services/websocket_service.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized(); // Обязательно для асинхронной инициализации
  await initializeDateFormatting('ru_RU', null); // Инициализация русской локализации
  await NotificationService().init();


  runApp(MyApp());

}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Университетское приложение',
      theme: ThemeData(primarySwatch: Colors.blue),

      // Настройки локализации
      localizationsDelegates: [
        GlobalMaterialLocalizations.delegate, // Локализация Material-виджетов
        GlobalWidgetsLocalizations.delegate, // Локализация текстовых направлений (LTR/RTL)
        GlobalCupertinoLocalizations.delegate, // Локализация iOS-стиля
      ],

      supportedLocales: [
        const Locale('ru', 'RU'), // Основная локаль - русский
        const Locale('en', 'US'), // Резервная локаль - английский
      ],

      locale: const Locale('ru', 'RU'), // Явное указание русской локали

      home: MainTimetableWidget(),
      initialRoute: '/main',
      routes: {
        '/main': (context) => MainTimetableWidget(),
        '/auths': (context) => AuthPageWidget(),
        '/timetable': (context) => TimetablePageWidget(),
        '/user_profile': (context) => ProfileWidget(),
        '/knowledgeGraph':(context)=>KnowledgeGraphPage(),
        '/notifications': (context) => NotificationsScreen()
      },
    );
  }
}