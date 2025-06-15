import 'dart:ui';

import 'package:vector_math/vector_math.dart' as vm;

class DisciplineNode {
  vm.Vector2 position;
  final double originalAngle;
  vm.Vector2 velocity = vm.Vector2.zero();
  String title;
  bool isDragging = false;
  Color color;
  final int totalLessons;
  double radius = 24;

  DisciplineNode({
    required this.position,
    required this.originalAngle,
    required this.title,
    required this.totalLessons,
    required this.color,
  });

  void applyForce(vm.Vector2 force) {
    velocity += force;
  }
  bool contains(vm.Vector2 point) { // Исправленный метод
    return (position.x - point.x).abs() < radius &&
        (position.y - point.y).abs() < radius;
  }
}

class NodeLink {
  final DisciplineNode source;
  final DisciplineNode target;

  NodeLink(this.source, this.target);
}