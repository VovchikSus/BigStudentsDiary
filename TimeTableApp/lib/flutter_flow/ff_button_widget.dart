import 'package:flutter/material.dart';

class FFButtonWidget extends StatelessWidget {
  final VoidCallback? onPressed;
  final String text;
  final FFButtonOptions options;

  const FFButtonWidget({
    Key? key,
    required this.onPressed,
    required this.text,
    required this.options,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: options.width,
      height: options.height,
      child: ElevatedButton(
        onPressed: onPressed,
        style: ElevatedButton.styleFrom(
          padding: options.padding,
          backgroundColor: options.color,
          elevation: options.elevation,
          shape: RoundedRectangleBorder(
            borderRadius: options.borderRadius,
          ),
          textStyle: options.textStyle,
        ),
        child: Text(text),
      ),
    );
  }
}

class FFButtonOptions {
  final double width;
  final double height;
  final EdgeInsetsGeometry padding;
  final EdgeInsetsGeometry iconPadding;
  final Color color;
  final TextStyle? textStyle;
  final double elevation;
  final BorderRadius borderRadius;

  FFButtonOptions({
    required this.width,
    required this.height,
    required this.padding,
    required this.iconPadding,
    required this.color,
    this.textStyle,
    required this.elevation,
    required this.borderRadius,
  });
}
