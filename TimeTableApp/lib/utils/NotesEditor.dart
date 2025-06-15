// В файле NotesEditor.dart замените весь код на:

import 'package:flutter/material.dart';
import '../constants/graph_api.dart';
import '../flutter_flow/flutter_flow_theme.dart';
import '../models/Note.dart';

class NotesEditor extends StatefulWidget {
  final String studentId;
  final String discipline;
  final List<Note> notes;
  final int totalLessons;

  const NotesEditor({
    required this.studentId,
    required this.discipline,
    required this.totalLessons,
    required this.notes,
  });

  @override
  _NotesEditorState createState() => _NotesEditorState();
}

class _NotesEditorState extends State<NotesEditor> {
  late List<Note> _editedNotes;
  final Map<int, TextEditingController> _controllers = {};

  @override
  void initState() {
    super.initState();
    _initNotes();
  }

  void _initNotes() {
    _editedNotes = List.generate(widget.totalLessons, (index) {
      final lessonNumber = index + 1;
      final existingNote = widget.notes.firstWhere(
            (n) => n.lessonNumber == lessonNumber,
        orElse: () => Note.empty(lessonNumber),
      );

      // Инициализируем контроллеры
      _controllers[lessonNumber] = TextEditingController(text: existingNote.content);

      return existingNote;
    });
  }

  String _generateNoteId() {
    return '${DateTime.now().millisecondsSinceEpoch}-${widget.studentId}';
  }

  Future<void> _saveNote(int lessonNumber) async {
    final index = lessonNumber - 1;
    final content = _controllers[lessonNumber]!.text;

    try {
      final note = _editedNotes[index].copyWith(
        content: content,
        noteId: _editedNotes[index].noteId.isEmpty
            ? _generateNoteId()
            : _editedNotes[index].noteId,
      );

      await GraphApi.saveNote(
        widget.studentId,
        widget.discipline,
        note,
      );

      ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Занятие $lessonNumber сохранено'),
            duration: Duration(seconds: 1),
          ));
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Ошибка: ${e.toString()}')),
        );
      }
    }

  @override
  Widget build(BuildContext context) {
    final theme = FlutterFlowTheme.of(context); // Получаем текущую тему

    return Scaffold(
      appBar: AppBar(
        title: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              widget.discipline,
              style: theme.headlineMedium.override(
                color: theme.secondaryBackground,
              ),
            ),
            Text(
              'Занятий: ${widget.totalLessons}',
              style: theme.bodySmall.override(
                color: theme.secondaryBackground.withOpacity(0.8),
              ),
            ),
          ],
        ),
        flexibleSpace: Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [theme.primary, theme.secondary],
            ),
          ),
        ),
        iconTheme: IconThemeData(color: theme.secondaryBackground),
      ),
      body: Container(
        decoration: BoxDecoration(
          color: theme.primaryBackground,
        ),
        child: ListView.separated(
          padding: EdgeInsets.all(16),
          itemCount: widget.totalLessons,
          separatorBuilder: (context, index) => SizedBox(height: 12),
          itemBuilder: (context, index) {
            final lessonNumber = index + 1;
            final note = _editedNotes[index];

            return Material(
              elevation: 4,
              borderRadius: BorderRadius.circular(12),
              child: Container(
                decoration: BoxDecoration(
                  color: theme.secondaryBackground,
                  borderRadius: BorderRadius.circular(12),
                  boxShadow: [
                    BoxShadow(
                      color: theme.primaryText.withOpacity(0.1),
                      blurRadius: 6,
                      offset: Offset(0, 2),
                    )
                  ],
                ),
                child: Padding(
                  padding: EdgeInsets.all(16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          CircleAvatar(
                            backgroundColor: theme.primary.withOpacity(0.1),
                            child: Text(
                              lessonNumber.toString(),
                              style: theme.titleSmall.override(
                                color: theme.primary,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ),
                          SizedBox(width: 16),
                          Expanded(
                            child: Text(
                              'Занятие $lessonNumber',
                              style: theme.titleLarge,
                            ),
                          ),
                          IconButton(
                            icon: Icon(Icons.save, color: theme.secondary),
                            onPressed: () => _saveNote(lessonNumber),
                            tooltip: 'Сохранить',
                          ),
                        ],
                      ),
                      SizedBox(height: 12),
                      TextField(
                        controller: _controllers[lessonNumber],
                        maxLines: 5,
                        minLines: 3,
                        decoration: InputDecoration(
                          hintText: 'Введите заметки...',
                          hintStyle: theme.bodySmall,
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(8),
                            borderSide: BorderSide(
                                color: theme.primary.withOpacity(0.2)),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(8),
                            borderSide: BorderSide(color: theme.primary),
                          ),
                          contentPadding: EdgeInsets.all(14),
                          fillColor: theme.primaryBackground,
                          filled: true,
                        ),
                        style: theme.bodyMedium,
                        onChanged: (value) {
                          setState(() {
                            _editedNotes[index] =
                                _editedNotes[index].copyWith(content: value);
                          });
                        },
                      ),
                    ],
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }


  Widget _buildLessonCard(int lessonNumber, Note note) {
    return Material(
      elevation: 4,
      borderRadius: BorderRadius.circular(12),
      child: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(12),
          boxShadow: [
          BoxShadow(
          color: Colors.black12,
          blurRadius: 6,
          offset: Offset(0, 2),
          )],
        ),
        child: Padding(
          padding: EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  CircleAvatar(
                    backgroundColor: Colors.blue[100],
                    child: Text(
                      lessonNumber.toString(),
                      style: TextStyle(
                        color: Colors.blue[800],
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                  SizedBox(width: 16),
                  Expanded(
                    child: Text(
                      'Занятие $lessonNumber',
                      style: TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.w600,
                        color: Colors.grey[800],
                      ),
                    ),
                  ),
                  IconButton(
                    icon: Icon(Icons.save, color: Colors.blue),
                    onPressed: () => _saveNote(lessonNumber),
                    tooltip: 'Сохранить',
                  ),
                ],
              ),
              SizedBox(height: 12),
              TextField(
                controller: _controllers[lessonNumber],
                maxLines: 5,
                minLines: 3,
                decoration: InputDecoration(
                  hintText: 'Введите заметки...',
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                    borderSide: BorderSide(color: Colors.blue[200]!),
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                    borderSide: BorderSide(color: Colors.blue),
                  ),
                  contentPadding: EdgeInsets.all(14),
                  fillColor: Colors.blue[50],
                  filled: true,
                ),
                style: TextStyle(
                  fontSize: 14,
                  color: Colors.grey[800],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _handleChange(int lessonNumber, String value) {
    final index = lessonNumber - 1;
    if (index < _editedNotes.length) {
      setState(() {
        _editedNotes[index] = _editedNotes[index].copyWith(content: value);
      });
    }
  }
}