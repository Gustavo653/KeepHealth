using KeepNotes.Maui.Models;
using KeepNotes.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KeepNotes.Maui.Pages;

public partial class NoteListPage : ContentPage
{
    private readonly INoteService _noteService;

    public ObservableCollection<Note> Notes { get; set; }
    public ICommand RefreshCommand { get; set; }
    public bool IsRefreshing { get; set; }

    public NoteListPage(INoteService noteService)
    {
        _noteService = noteService;
        Notes = new ObservableCollection<Note>();
        BindingContext = this;
        InitializeComponent();

        RefreshCommand = new Command(async () =>
        {
            await GetData();
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await GetData();
    }

    private async Task GetData()
    {
        IsBusy = true;
        var notes = await _noteService.GetAllNotesAsync();
        IsBusy = false;
        Notes.Clear();

        foreach (var note in notes)
        {
            Notes.Add(new Note
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                LastModified = note.LastModified
            });
        }
    }
}

