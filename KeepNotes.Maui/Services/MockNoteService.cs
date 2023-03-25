using KeepNotes.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepNotes.Maui.Services
{
    public class MockNoteService : INoteService
    {
        private readonly List<Note> _notes;

        public MockNoteService()
        {
            Random random = new Random();
            _notes = new List<Note>
            {
                new Note() { Id = 1, Title = "Title 1", Content = "Content 1", LastModified = DateTime.Now.AddHours(random.Next(-200, 200)) },
                new Note() { Id = 2, Title = "Title 2", Content = "Content 2", LastModified = DateTime.Now.AddDays(random.Next(-200, 200)) },
                new Note() { Id = 3, Title = "Title 3", Content = "Content 3", LastModified = DateTime.Now.AddMinutes(random.Next(-200, 200)) }
            };
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            await Task.Delay(1500);
            return await Task.FromResult(_notes);
        }

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            return await Task.FromResult(_notes.FirstOrDefault(n => n.Id == id));
        }

        public async Task<int> AddNoteAsync(Note note)
        {
            var maxId = _notes.Any() ? _notes.Max(n => n.Id) : 0;
            note.Id = maxId + 1;
            _notes.Add(note);
            return await Task.FromResult(note.Id);
        }

        public async Task<int> UpdateNoteAsync(Note note)
        {
            var existingNote = await GetNoteByIdAsync(note.Id);
            if (existingNote == null)
            {
                return 0;
            }

            existingNote.Title = note.Title;
            existingNote.Content = note.Content;

            return await Task.FromResult(1);
        }

        public async Task<int> DeleteNoteAsync(int id)
        {
            var existingNote = await GetNoteByIdAsync(id);
            if (existingNote == null)
            {
                return 0;
            }

            _notes.Remove(existingNote);

            return await Task.FromResult(1);
        }
    }

}
