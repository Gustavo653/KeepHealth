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

  Future<void> _deleteNoteDialog(Note note) async {
    bool confirmDelete = await showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text("Confirmar exclusão"),
        content: const Text("Deseja excluir esta nota?"),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(false),
            child: const Text("Cancelar"),
          ),
          TextButton(
            onPressed: () => Navigator.of(context).pop(true),
            child: const Text("Excluir"),
          ),
        ],
      ),
    );
    if (confirmDelete) {
      await _noteService.delete(note.id!);
      setState(() async {
        await _getNotes();
      });
    }
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
          return RefreshIndicator(
            onRefresh: _getNotes,
            child: ListView.builder(
              itemCount: notes.length,
              itemBuilder: (BuildContext context, int index) {
                final note = notes[index];
                return InkWell(
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => NoteEditPage(note: note),
                      ),
                    );
                  },
                  child: Card(
                    margin: const EdgeInsets.all(16),
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            note.title,
                            style: Theme.of(context).textTheme.titleLarge,
                          ),
                          const SizedBox(height: 8),
                          Text(
                            note.content,
                            style: Theme.of(context).textTheme.bodyMedium,
                          ),
                          const SizedBox(height: 8),
                          Row(
                            mainAxisAlignment: MainAxisAlignment.end,
                            children: [
                              IconButton(
                                icon: const Icon(Icons.delete),
                                onPressed: () => _deleteNoteDialog(note),
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                  ),
                );
              },
            ),
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
