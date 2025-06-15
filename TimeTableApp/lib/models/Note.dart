class Note {
  final String noteId;
  final int lessonNumber;
  String content;

  Note({
    required this.noteId,
    required this.lessonNumber,
    required this.content,
  });

  factory Note.fromJson(Map<String, dynamic> json) {
    return Note(
      noteId: json['noteId'],
      lessonNumber: json['lessonNumber'],
      content: json['content'],
    );
  }
  Note copyWith({
    String? noteId,
    int? lessonNumber,
    String? content,
  }) {
    return Note(
      noteId: noteId ?? this.noteId,
      lessonNumber: lessonNumber ?? this.lessonNumber,
      content: content ?? this.content,
    );
  }
  Note.empty(int lessonNumber)
      : noteId = '',
        lessonNumber = lessonNumber,
        content = '';
}