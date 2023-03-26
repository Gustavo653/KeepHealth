import 'dart:async';

class AuthService {
  static String _validEmail = 'teste@teste.com';
  static String _validPassword = 'senha';

  Future<bool> authenticate(String email, String password) async {
    await Future.delayed(const Duration(seconds: 2));

    if (email == _validEmail && password == _validPassword) {
      return true;
    } else {
      return false;
    }
  }

  Future<bool> register(String email, String password) async {
    await Future.delayed(const Duration(seconds: 2));
    if (email.isEmpty || password.isEmpty) {
      return false;
    }
    _validEmail = email;
    _validPassword = password;
    return true;
  }
}
