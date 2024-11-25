import 'package:universal_html/html.dart' as html;

class CookieHelper {
  static void setCookie(String name, String value, {int? maxAge}) {
    String cookie = '$name=$value; path=/';
    if (maxAge != null) {
      cookie += '; max-age=$maxAge';
    }
    html.document.cookie = cookie;
  }

  static String? getCookie(String name) {
    final cookies = html.document.cookie?.split('; ') ?? [];
    for (final cookie in cookies) {
      final parts = cookie.split('=');
      if (parts[0] == name) {
        return parts[1];
      }
    }
    return null;
  }
}
