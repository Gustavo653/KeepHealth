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
                await DisplayAlert("Atenção", "Por favor, preencha seu nome de usuário e senha.", "OK");
                return;
            }

            // Realiza o login (substitua o código abaixo com a lógica real do seu serviço de autenticação)
            bool isAuthenticated = await AuthenticateAsync(usernameEntry.Text, passwordEntry.Text);

            // Verifica se o login foi bem sucedido
            if (isAuthenticated)
            {
                // Navega para a próxima página
                var mockNoteService = new MockNoteService();
                await Navigation.PushAsync(new NoteListPage(mockNoteService));
            }
            else
            {
                await DisplayAlert("Erro", "Nome de usuário ou senha inválidos.", "OK");
            }
        }

        private async Task<bool> AuthenticateAsync(string username, string password)
        {
            // Implemente a lógica real de autenticação aqui
            // Por exemplo, pode chamar um serviço RESTful ou verificar em um banco de dados
            // O exemplo abaixo é apenas uma simulação
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