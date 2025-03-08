import 'package:flutter/material.dart';

class FlutterFlowTheme {
  final Color primary;
  final Color secondary;
  final Color primaryBackground;
  final Color secondaryBackground;
  final Color primaryText;
  final Color secondaryText;
  final Color error;

  const FlutterFlowTheme({
    required this.primary,
    required this.secondary,
    required this.primaryBackground,
    required this.secondaryBackground,
    required this.primaryText,
    required this.secondaryText,
    required this.error,
  });

  static FlutterFlowTheme of(BuildContext context) {
    return const FlutterFlowTheme(
      primary: Color(0xFF3F51B5),
      secondary: Color(0xFFF57C00),
      primaryBackground: Color(0xFFF1F4F8),
      secondaryBackground: Color(0xFFFFFFFF),
      primaryText: Color(0xFF212121),
      secondaryText: Color(0xFF757575),
      error: Color(0xFFD32F2F),
    );
  }

  TextStyle get headlineLarge => TextStyle(
    fontSize: 32,
    fontWeight: FontWeight.bold,
    color: primaryText,
  );

  TextStyle get headlineMedium => TextStyle(
    fontSize: 22,
    fontWeight: FontWeight.bold,
    color: primaryText,
  );

  TextStyle get headlineSmall => TextStyle(
    fontSize: 20,
    fontWeight: FontWeight.w500,
    color: primaryText,
  );

  TextStyle get displaySmall => TextStyle(
    fontSize: 28,
    fontWeight: FontWeight.bold,
    color: primaryText,
  );

  TextStyle get titleLarge => TextStyle(
    fontSize: 18,
    fontWeight: FontWeight.w600,
    color: primaryText,
  );

  TextStyle get titleSmall => TextStyle(
    fontSize: 16,
    fontWeight: FontWeight.w500,
    color: primaryText,
  );

  TextStyle get bodyMedium => TextStyle(
    fontSize: 16,
    fontWeight: FontWeight.normal,
    color: secondaryText,
  );

  TextStyle get bodySmall => TextStyle(
    fontSize: 14,
    fontWeight: FontWeight.normal,
    color: secondaryText,
  );

  TextStyle get bodyLarge => TextStyle(
    fontSize: 18,
    fontWeight: FontWeight.normal,
    color: secondaryText,
  );

  TextStyle get titleMedium => TextStyle(
    fontSize: 18,
    fontWeight: FontWeight.normal,
    color: secondaryText,
  );
}

extension CustomTextStyleExtension on TextStyle {
  TextStyle override({
    String? fontFamily,
    Color? color,
    double? fontSize,
    FontWeight? fontWeight,
    double? letterSpacing,
  }) {
    return copyWith(
      fontFamily: fontFamily ?? this.fontFamily,
      color: color ?? this.color,
      fontSize: fontSize ?? this.fontSize,
      fontWeight: fontWeight ?? this.fontWeight,
      letterSpacing: letterSpacing ?? this.letterSpacing,
    );
  }
}
