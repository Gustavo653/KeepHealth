import 'dart:async';
import 'package:keep_notes_flutter/Models/note.dart';

class NoteService {
  final List<Note> _notes = [
    Note(
      id: 1,
      title: 'Nota 1',
      content: 'Conteúdo da nota 1',
    ),
    Note(
      id: 2,
      title: 'Nota 2',
      content: 'Conteúdo da nota 2',
    ),
    Note(
      id: 3,
      title: 'Nota 3',
      content: 'Conteúdo da nota 3',
    ),
  ];

  Future<List<Note>> getNotes() async {
    await Future.delayed(const Duration(milliseconds: 500));
    return _notes;
  }

  Future<Note> getNoteById(int id) async {
    await Future.delayed(const Duration(milliseconds: 500));
    return _notes.firstWhere((note) => note.id == id);
  }

  Future<void> saveOrUpdate(Note note) async {
    if (note.id == null) {
      final newId = _notes.isNotEmpty ? _notes.last.id! + 1 : 1;
      _notes.add(note.copyWith(id: newId));
    } else {
      final index = _notes.indexWhere((n) => n.id == note.id);
      _notes[index] = note;
    }
    await Future.delayed(const Duration(milliseconds: 500));
  }

  Future<void> delete(int id) async {
    _notes.removeWhere((note) => note.id == id);
    await Future.delayed(const Duration(milliseconds: 500));
  }
}
