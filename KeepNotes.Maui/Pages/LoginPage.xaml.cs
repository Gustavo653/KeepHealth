using KeepNotes.Maui.Models;
using KeepNotes.Maui.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KeepNotes.Maui.Pages
{
    public partial class LoginPage : ContentPage
    {
        public ICommand LoginCommand { get; set; }

        public LoginPage()
        {
            InitializeComponent();

            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            // Verifica se os campos foram preenchidos
            if (string.IsNullOrEmpty(usernameEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Aten��o", "Por favor, preencha seu nome de usu�rio e senha.", "OK");
                return;
            }

            // Realiza o login (substitua o c�digo abaixo com a l�gica real do seu servi�o de autentica��o)
            bool isAuthenticated = await AuthenticateAsync(usernameEntry.Text, passwordEntry.Text);

            // Verifica se o login foi bem sucedido
            if (isAuthenticated)
            {
                // Navega para a pr�xima p�gina
                var mockNoteService = new MockNoteService();
                await Navigation.PushAsync(new NoteListPage(mockNoteService));
            }
            else
            {
                await DisplayAlert("Erro", "Nome de usu�rio ou senha inv�lidos.", "OK");
            }
        }

        private async Task<bool> AuthenticateAsync(string username, string password)
        {
            // Implemente a l�gica real de autentica��o aqui
            // Por exemplo, pode chamar um servi�o RESTful ou verificar em um banco de dados
            // O exemplo abaixo � apenas uma simula��o
            await Task.Delay(2000);

            if (username == "user" && password == "password")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}