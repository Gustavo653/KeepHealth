using KeepNotes.Maui.Models;
using KeepNotes.Maui.Services;
using System.Collections.ObjectModel;

namespace KeepNotes.Maui.Pages;

public partial class NoteListPage : ContentPage
{
    private readonly INoteService _noteService;

    public ObservableCollection<Note> Notes { get; set; }

    public NoteListPage(INoteService noteService)
    {
        _noteService = noteService;
        Notes = new ObservableCollection<Note>();
        BindingContext = this;

        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var notes = await _noteService.GetAllNotesAsync();
        Notes.Clear();

        foreach (var note in notes)
        {
            Notes.Add(new Note
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content
            });
        }
    }
}

