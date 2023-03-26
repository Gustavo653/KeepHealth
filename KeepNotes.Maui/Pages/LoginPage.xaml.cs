using System.ComponentModel;
using System.Windows.Input;

namespace KeepNotes.Maui.Pages
{
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }
        public ICommand NavigateToRegisterCommand { get; set; }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            }
        }

        public LoginPage()
        {
            InitializeComponent();
            LoginCommand = new Command(Login);
            NavigateToRegisterCommand = new Command(NavigateToRegister);
        }

        private async void Login()
        {
            // Implement your login logic here
        }

        private async void NavigateToRegister()
        {
            // Navigate to the register page
        }
    }
}