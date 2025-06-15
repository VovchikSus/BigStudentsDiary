import 'dart:math';
import 'package:collection/collection.dart';
import 'package:dio/dio.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:vector_math/vector_math.dart' as vm;
import '../constants/graph_api.dart';
import '../models/Note.dart';
import '../models/models.dart';
import '../utils/NotesEditor.dart';
import '../utils/graph_painter.dart';
import '../utils/storage_helper.dart';
import '/flutter_flow/flutter_flow_theme.dart'; // Добавлено

class KnowledgeGraphPage extends StatefulWidget {
  @override
  _KnowledgeGraphPageState createState() => _KnowledgeGraphPageState();
}

class _KnowledgeGraphPageState extends State<KnowledgeGraphPage>
    with TickerProviderStateMixin {
  late AnimationController _controller;
  List<DisciplineNode> nodes = [];
  List<NodeLink> links = [];
  DisciplineNode? _tappedNode;
  Offset? _tapPosition;
  final double _maxRotationSpeed = 0.002;
  double _targetRotationSpeed = 0.002;
  bool _isHovered = false;
  DisciplineNode? selectedNode;
  bool _isLoading = true;
  double _rotationSpeed = 0.002;
  bool _isOrbiting = true;
  double _currentAngle = 0.0;
  final double _baseRadius = 250.0;
  late AnimationController _decelerationController;
  late Animation<double> _decelerationAnimation;
  String? _errorMessage;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 16),
    )..addListener(_updatePhysics)
      ..repeat();

    _decelerationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 500),
    );

    _decelerationAnimation = Tween<double>(begin: _maxRotationSpeed, end: 0).animate(
      CurvedAnimation(
        parent: _decelerationController,
        curve: Curves.easeOutQuad,
      ),
    )..addListener(() {
      _rotationSpeed = _decelerationAnimation.value;
    });
    _loadData();
  }

  Future<void> _loadData() async {
    try {
      setState(() => _isLoading = true);
      final data = await GraphApi.fetchGroupDisciplines();

      if (data['nodes'].isEmpty) {
        throw Exception('Получен пустой список дисциплин');
      }

      _createNodes(data['nodes']);
      _createLinks(data['links']);
      setState(() => _isLoading = false);

    } on DioException catch (e) {
      setState(() {
        _errorMessage = 'Ошибка: ${e.response?.data['title'] ?? e.message}';
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _errorMessage = 'Ошибка: ${e.toString()}';
        _isLoading = false;
      });
    }
  }

  void _createNodes(List<Map<String, dynamic>> data) {
    const colors = [Colors.blueAccent, Colors.green, Colors.orange, Colors.purple];
    final newNodes = <DisciplineNode>[];

    final centerX = MediaQuery.of(context).size.width / 2;
    final centerY = MediaQuery.of(context).size.height / 2;
    final angleStep = 2 * pi / data.length;

    for (int i = 0; i < data.length; i++) {
      final item = data[i];
      final angle = angleStep * i;
      newNodes.add(DisciplineNode(
        position: vm.Vector2(
          centerX + _baseRadius * cos(angle),
          centerY + _baseRadius * sin(angle),
        ),
        originalAngle: angle,
        title: item['discipline'].toString(),
        totalLessons: item['totalLessons'] ?? 0,
        color: colors[i % colors.length],
      ));
    }
    setState(() => nodes = newNodes);
  }

  vm.Vector2 _calculatePosition(vm.Vector2 center, int index, int total) {
    const baseRadius = 200.0;
    final angle = (2 * pi / total) * index;
    return vm.Vector2(
      center.x + baseRadius * cos(angle),
      center.y + baseRadius * sin(angle),
    );
  }

  void _createLinks(List<Map<String, dynamic>> linksData) {
    final nodeMap = {for (var node in nodes) node.title: node};
    links = linksData.map((link) {
      return NodeLink(
        nodeMap[link['source']]!,
        nodeMap[link['target']]!,
      );
    }).toList();
    setState(() {});
  }


  void _updatePhysics() {
    const double springForce = 0.5;
    const double friction = 0.7;
    const double maxSpeed = 50.0;
    final center = vm.Vector2(
      MediaQuery.of(context).size.width / 2,
      MediaQuery.of(context).size.height / 2,
    );

    if (_isOrbiting) {
      _currentAngle += _rotationSpeed;
    }

    for (var node in nodes) {
      if (node.isDragging) {
        if (_isOrbiting) {
          _isOrbiting = false;
          _decelerationController.forward();
        }
        node.velocity *= 0.5;
        continue;
      }

      final targetPosition = vm.Vector2(
        center.x + _baseRadius * cos(_currentAngle + node.originalAngle),
        center.y + _baseRadius * sin(_currentAngle + node.originalAngle),
      );

      final delta = targetPosition - node.position;
      final distance = delta.length;

      node.velocity += delta.normalized() * distance * springForce;

      if (node.velocity.length > maxSpeed) {
        node.velocity = node.velocity.normalized() * maxSpeed;
      }

      node.position += node.velocity;
      node.velocity *= friction;

      if (distance < 2.0) {
        node.position = targetPosition;
        node.velocity *= 0;
      }
    }

    if (mounted) setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.black,
      body: Listener(
        onPointerHover: _handleHover,
        child: _isLoading
            ? Center(child: CircularProgressIndicator())
            : _errorMessage != null
            ? Center(
            child: Text(
              _errorMessage!,
              style: TextStyle(color: Colors.white),
            )
        ): RawGestureDetector(
          gestures: {
            // Обработчик перетаскивания
            PanGestureRecognizer:
            GestureRecognizerFactoryWithHandlers<PanGestureRecognizer>(
                  () => PanGestureRecognizer(),
                  (instance) {
                instance.onStart = (DragStartDetails details) => _handlePanStart(details);
                instance.onUpdate = (DragUpdateDetails details) => _handlePanUpdate(details);
                instance.onEnd = (DragEndDetails details) => _handlePanEnd(details);
              },
            ),
            // Обработчик тапов с явным указанием типов
            TapGestureRecognizer:
            GestureRecognizerFactoryWithHandlers<TapGestureRecognizer>(
                  () => TapGestureRecognizer(), // Конструктор
                  (instance) { // Инициализатор
                instance
                  ..onTapDown = _handleTapDown
                  ..onTapUp = _handleTapUp;
              },
            ),
          },
          behavior: HitTestBehavior.opaque,
          child: CustomPaint(
            painter: GraphPainter(nodes, links, selectedNode),
            size: Size.infinite,
          ),
        ),
      ),
      // ДОБАВЛЕНА НАВИГАЦИОННАЯ ПАНЕЛЬ
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: 2, // Активная вкладка "Заметки"
        type: BottomNavigationBarType.fixed,
        selectedItemColor: FlutterFlowTheme.of(context).primary,
        unselectedItemColor: FlutterFlowTheme.of(context).secondaryText,
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.home_outlined),
            label: 'Главная',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.calendar_today),
            label: 'Расписание',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.menu_book),
            label: 'Заметки',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person_outline),
            label: 'Профиль',
          ),
        ],
        onTap: (index) {
          if (index == 0) Navigator.pushReplacementNamed(context, '/');
          if (index == 1) Navigator.pushReplacementNamed(context, '/timetable');
          if (index == 3) Navigator.pushReplacementNamed(context, '/user_profile');
        },
      ),
    );
  }

  void _handleTapDown(TapDownDetails details) {
    final tapOffset = details.localPosition;
    final tapVector = vm.Vector2(tapOffset.dx, tapOffset.dy);
    _tappedNode = nodes.firstWhereOrNull((node) => node.contains(tapVector));
  }

  void _handleTapUp(TapUpDetails details) {
    if (_tappedNode != null) {
      final tapVector = vm.Vector2(
        details.localPosition.dx,
        details.localPosition.dy,
      );
      final isTap = (_tappedNode!.position - tapVector).length < 15;
      if (isTap) _openNodeMenu(_tappedNode!);
    }
    _tappedNode = null;
  }

  void _handlePanStart(DragStartDetails details) {
    final offset = details.localPosition;
    final position = vm.Vector2(offset.dx, offset.dy);
    for (var node in nodes) {
      if (node.contains(position)) {
        setState(() {
          selectedNode = node;
          node.isDragging = true;
        });
        _tappedNode = null;
        return;
      }
    }
  }

  void _handlePanUpdate(DragUpdateDetails details) {
    final position = details.localPosition;
    if (selectedNode != null) {
      setState(() => selectedNode!.position = vm.Vector2(
          position.dx,
          position.dy
      ));
    }
  }

  void _handleHover(PointerHoverEvent event) {
    final offset = event.localPosition;
    final position = vm.Vector2(offset.dx, offset.dy);
    bool anyHovered = nodes.any((node) => node.contains(position));

    if (anyHovered && !_isHovered) {
      _isHovered = true;
      _targetRotationSpeed = 0;
      _decelerationController.animateTo(
        _targetRotationSpeed,
        duration: Duration(milliseconds: 800),
      );
    } else if (!anyHovered && _isHovered) {
      _isHovered = false;
      _targetRotationSpeed = _maxRotationSpeed;
      _decelerationController.animateTo(
        _targetRotationSpeed,
        duration: Duration(milliseconds: 1200),
      );
    }
  }

  void _openNodeMenu(DisciplineNode node) async {
    final token = await StorageHelper.getToken();
    if (token == null) return;

    final claims = GraphApi.parseJwt(token);
    final studentId = claims['studentId'] ?? 'defaultStudentId';

    showModalBottomSheet(
      context: context,
      builder: (context) => FutureBuilder<List<Note>>(
        future: GraphApi.getNotes(studentId, node.title),
        builder: (context, snapshot) {
          final notes = snapshot.data ?? [];
          return NotesEditor(
            studentId: studentId,
            discipline: node.title,
            notes: notes,
            totalLessons: node.totalLessons,
          );
        },
      ),
    );
  }

  void _handlePanEnd(DragEndDetails details) {
    if (selectedNode != null) {
      setState(() {
        selectedNode!.isDragging = false;
        final center = vm.Vector2(
          MediaQuery.of(context).size.width / 2,
          MediaQuery.of(context).size.height / 2,
        );
        final targetPosition = vm.Vector2(
          center.x + _baseRadius * cos(_currentAngle + selectedNode!.originalAngle),
          center.y + _baseRadius * sin(_currentAngle + selectedNode!.originalAngle),
        );
        final delta = targetPosition - selectedNode!.position;
        selectedNode!.velocity = delta.normalized() * delta.length * 0.3;
      });
    }
  }

  @override
  void dispose() {
    _controller.dispose();
    _decelerationController.dispose();
    super.dispose();
  }
}