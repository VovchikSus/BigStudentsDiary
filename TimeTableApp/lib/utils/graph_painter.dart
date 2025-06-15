import 'package:flutter/material.dart';
import '../models/models.dart';


class GraphPainter extends CustomPainter {
  final List<DisciplineNode> nodes;
  final List<NodeLink> links;
  final DisciplineNode? selectedNode;

  GraphPainter(this.nodes, this.links, this.selectedNode);

  @override
  void paint(Canvas canvas, Size size) {
    _drawLinks(canvas);
    _drawNodes(canvas);
  }

  void _drawLinks(Canvas canvas) {
    final paint = Paint()
      ..color = Colors.white.withOpacity(0.3)
      ..strokeWidth = 1.5
      ..style = PaintingStyle.stroke;

    for (final link in links) {
      final path = Path()
        ..moveTo(link.source.position.x, link.source.position.y)
        ..lineTo(link.target.position.x, link.target.position.y);

      canvas.drawPath(path, paint);
    }
  }

  void _drawNodes(Canvas canvas) {
    for (final node in nodes) {
      // Градиент для узлов
      final gradient = RadialGradient(
        colors: [node.color, node.color.withOpacity(0.7)],
        stops: [0.7, 1.0],
      );

      final paint = Paint()
        ..shader = gradient.createShader(Rect.fromCircle(
          center: Offset(node.position.x, node.position.y),
          radius: node.radius,
        ));

      canvas.drawCircle(
        Offset(node.position.x, node.position.y),
        node.radius,
        paint,
      );

      // Рисуем текст
      _drawText(canvas, node);
    }
  }

  void _drawText(Canvas canvas, DisciplineNode node) {
    final textStyle = TextStyle(
      color: Colors.white,
      fontSize: 14,
      fontWeight: FontWeight.bold,
    );

    final textSpan = TextSpan(text: node.title, style: textStyle);
    final textPainter = TextPainter(
      text: textSpan,
      textDirection: TextDirection.ltr,
    )..layout();

    textPainter.paint(
      canvas,
      Offset(
        node.position.x - textPainter.width / 2,
        node.position.y + node.radius + 8,
      ),
    );
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}