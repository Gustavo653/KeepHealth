using KeepNotes.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepNotes.Maui.Services
{
    public interface INoteService
    {
        Task<List<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(int id);
        Task<int> AddNoteAsync(Note note);
        Task<int> UpdateNoteAsync(Note note);
        Task<int> DeleteNoteAsync(int id);
    }
}
