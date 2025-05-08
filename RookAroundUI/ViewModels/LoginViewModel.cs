using StudyApp.Models;
using ReactiveUI;
using System.Reactive;

namespace StudyApp.ViewModels
{
    public class LogInViewModel : ViewModelBase
    {

        public string _username;
        public string _password;
        public string Username
        {
            get => _username;
            private set => this.RaiseAndSetIfChanged(ref _username, value);
        }
        public string Password 
        {
            get => _password;
            private set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public ReactiveCommand<Unit, Unit> Login { get; }

        public ReactiveCommand<Unit, Unit> Register { get; }


        public LogInViewModel()
        {
            //Enable the register button only when the user has entered a valid username
            var loginEnabled = this.WhenAnyValue(
                x => x.Username,
                x => !string.IsNullOrWhiteSpace(x));

            //Create the command to bind to the login and register buttons. Enable it only when loginEnabled is set to true.
            Login = ReactiveCommand.Create(() => { }, loginEnabled);
            Register = ReactiveCommand.Create(() => { }, loginEnabled);
            
        }

        public User? User { get; private set;}
        public User RegisterUser(){
            this.User = new User(Username, Password);
            return this.User;
        }

        public User LoginUser(){
            this.User = new User(Username, Password);
            return this.User;
        }



    }
}