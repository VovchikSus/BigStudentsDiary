import 'package:flutter/material.dart';

class FlutterFlowIconButton extends StatelessWidget {
  final double borderRadius;
  final double borderWidth;
  final Color borderColor;
  final double buttonSize;
  final Color? fillColor;
  final Icon icon;
  final VoidCallback onPressed;

  const FlutterFlowIconButton({
    Key? key,
    this.borderRadius = 0.0, // Значение по умолчанию
    this.borderWidth = 0.0,  // Значение по умолчанию
    this.borderColor = Colors.transparent, // Значение по умолчанию
    required this.buttonSize,
    this.fillColor, // Необязательный параметр
    required this.icon,
    required this.onPressed,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return InkWell(
      borderRadius: BorderRadius.circular(borderRadius),
      onTap: onPressed,
      child: Container(
        width: buttonSize,
        height: buttonSize,
        decoration: BoxDecoration(
          color: fillColor ?? Colors.transparent,
          borderRadius: BorderRadius.circular(borderRadius),
          border: borderWidth > 0
              ? Border.all(
            color: borderColor,
            width: borderWidth,
          )
              : null, // Убираем границу, если ширина = 0
        ),
        child: Center(child: icon),
      ),
    );
  }
}
