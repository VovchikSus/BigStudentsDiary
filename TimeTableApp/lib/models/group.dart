class Group {
  final int groupId;
  final String groupCode;

  Group({required this.groupId, required this.groupCode});

  factory Group.fromJson(Map<String, dynamic> json) {
    return Group(
      groupId: json['groupId'],
      groupCode: json['groupCode'],
    );
  }
}
