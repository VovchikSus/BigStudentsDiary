import 'package:flutter/material.dart';

extension MediaQuerySize on MediaQueryData {
  static Size sizeOf(BuildContext context) => MediaQuery.of(context).size;
}

extension WidgetListExtensions on List<Widget> {
  List<Widget> divide(SizedBox divider) {
    return expand((widget) => [widget, divider]).toList()..removeLast();
  }
}

extension NavigationContext on BuildContext {
  void pop() => Navigator.of(this).pop();
}
