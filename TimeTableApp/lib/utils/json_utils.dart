import 'dart:convert';

/// Utility class for JSON encoding and decoding.
class JsonUtils {
  /// Decodes a JSON string into a Dart object.
  /// Returns `null` if the decoding fails.
  static dynamic decode(String jsonString) {
    try {
      return jsonDecode(jsonString);
    } catch (e) {
      print('Error decoding JSON: $e');
      return null;
    }
  }

  /// Encodes a Dart object into a JSON string.
  /// Returns `null` if the encoding fails.
  static String? encode(dynamic object) {
    try {
      return jsonEncode(object);
    } catch (e) {
      print('Error encoding JSON: $e');
      return null;
    }
  }
}
