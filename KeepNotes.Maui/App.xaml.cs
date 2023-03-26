﻿using KeepNotes.Maui.Pages;
using KeepNotes.Maui.Services;

namespace KeepNotes.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //var noteService = new MockNoteService();
        //var navigationPage = new NavigationPage(new NoteListPage(noteService));

        var navigationPage = new NavigationPage(new LoginPage());

        MainPage = navigationPage;
    }
}
