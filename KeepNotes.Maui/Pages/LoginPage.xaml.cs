using KeepNotes.Maui.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KeepNotes.Maui.Pages
{
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private bool _isLoggingIn;

        public LoginPage(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () =>
            {
                await Login();
            });

            InitializeComponent();
        }

        public ICommand LoginCommand { get; }

        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            set
            {
                if (_isLoggingIn != value)
                {
                    _isLoggingIn = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNotLoggingIn));
                }
            }
        }

        public bool IsNotLoggingIn => !IsLoggingIn;

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task Login()
        {
            IsLoggingIn = true;

            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            if (await _authService.LoginAsync(username, password))
            {
                var mockNoteService = new MockNoteService();
                await Navigation.PushAsync(new NoteListPage(mockNoteService));
            }
            else
            {
                await DisplayAlert("Erro de login", "Usuário ou senha inválidos", "OK", null);
            }

            IsLoggingIn = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}