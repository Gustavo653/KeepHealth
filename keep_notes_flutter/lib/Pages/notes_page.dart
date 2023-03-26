import 'package:flutter/material.dart';
import 'package:keep_notes_flutter/Pages/note_edit_page.dart';
import 'package:keep_notes_flutter/services/note_service.dart';
import 'package:keep_notes_flutter/Models/note.dart';

class NotesPage extends StatefulWidget {
  @override
  _NotesPageState createState() => _NotesPageState();
}

class _NotesPageState extends State<NotesPage> {
  final NoteService _noteService = NoteService();
  List<Note> _notes = [];

  @override
  void initState() {
    super.initState();
    _getNotes();
  }

  Future<void> _getNotes() async {
    final notes = await _noteService.getNotes();
    setState(() {
      _notes = notes;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Minhas Anotações'),
        automaticallyImplyLeading: false,
      ),
      body: FutureBuilder<List<Note>>(
        future: _noteService.getNotes(),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }
          if (snapshot.hasError) {
            return const Center(child: Text('Erro ao carregar as anotações'));
          }
          final notes = snapshot.data ?? [];
          return ListView.builder(
            itemCount: notes.length,
            itemBuilder: (BuildContext context, int index) {
              final note = notes[index];
              return Card(
                margin: const EdgeInsets.all(16),
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        note.title,
                        style: Theme.of(context).textTheme.headline6,
                      ),
                      const SizedBox(height: 8),
                      Text(
                        note.content,
                        style: Theme.of(context).textTheme.bodyText2,
                      ),
                    ],
                  ),
                ),
              );
            },
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.push(
            context,
            MaterialPageRoute(
              builder: (context) => const NoteEditPage(),
            ),
          );
        },
        child: const Icon(Icons.add),
      ),
    );
  }
}
