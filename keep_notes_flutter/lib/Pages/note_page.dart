import 'package:flutter/material.dart';
import 'package:keep_notes_flutter/Models/note.dart';

class NotesPage extends StatelessWidget {
  final List<Note> notes = [
    Note(
      title: 'Nota 1',
      content: 'Conteúdo da nota 1',
    ),
    Note(
      title: 'Nota 2',
      content: 'Conteúdo da nota 2',
    ),
    Note(
      title: 'Nota 3',
      content: 'Conteúdo da nota 3',
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Minhas Anotações'),
      ),
      body: ListView.builder(
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
                    style: Theme.of(context).textTheme.titleLarge,
                  ),
                  const SizedBox(height: 8),
                  Text(note.content),
                ],
              ),
            ),
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {},
        child: const Icon(Icons.add),
      ),
    );
  }
}
