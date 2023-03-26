import 'package:flutter/material.dart';
import 'package:keep_notes_flutter/Models/note.dart';
import 'package:keep_notes_flutter/Services/note_service.dart';

class NoteEditPage extends StatefulWidget {
  final Note? note;

  const NoteEditPage({Key? key, this.note}) : super(key: key);

  @override
  _NoteEditPageState createState() => _NoteEditPageState();
}

class _NoteEditPageState extends State<NoteEditPage> {
  final _formKey = GlobalKey<FormState>();
  late String _title;
  late String _content;

  @override
  void initState() {
    super.initState();
    if (widget.note != null) {
      _title = widget.note!.title;
      _content = widget.note!.content;
    } else {
      _title = '';
      _content = '';
    }
  }

  Future<void> _submitForm() async {
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save();

      // Save note to database or API
      final note = Note(
        id: widget.note?.id ?? DateTime.now().millisecondsSinceEpoch,
        title: _title,
        content: _content,
      );

      await NoteService().saveOrUpdate(note);

      Navigator.pop(_formKey.currentContext!, note);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.note == null ? 'Nova Anotação' : 'Editar Anotação'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              TextFormField(
                initialValue: _title,
                decoration: const InputDecoration(
                  labelText: 'Título',
                ),
                textInputAction: TextInputAction.next,
                validator: (value) {
                  if (value == null || value.trim().isEmpty) {
                    return 'O título é obrigatório';
                  }
                  return null;
                },
                onSaved: (value) => _title = value!,
              ),
              const SizedBox(height: 16),
              TextFormField(
                initialValue: _content,
                decoration: const InputDecoration(
                  labelText: 'Conteúdo',
                ),
                maxLines: null,
                validator: (value) {
                  if (value == null || value.trim().isEmpty) {
                    return 'O conteúdo é obrigatório';
                  }
                  return null;
                },
                onSaved: (value) => _content = value!,
              ),
              const SizedBox(height: 32),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: _submitForm,
                  child: Text(widget.note == null ? 'Salvar' : 'Atualizar'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
