using MobileApp.Api;
using MobileApp.View;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModel
{
    class LoginPageViewModel : BindableObject
    {
        private string username;
        public string Username
        {
            get => username;
            set 
            { 
                username = value;
                Message = "";
                OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get => password; 
            set 
            { 
                password = value;
                Message = "";
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set 
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set 
            { 
                isLoading = value;
                OnPropertyChanged();
            }
        }

        private CancellationTokenSource cts;
        public Command CancelCommand => new Command(() =>
        {
            cts.Cancel();
        });

        public Command LoginCommand => new Command(async () => 
        {
            IsLoading = true;
            Message = "";

            var rest = RestService.For<ITdsApi>(new System.Net.Http.HttpClient()
            {
                BaseAddress = new Uri("http://192.168.1.2:8099"),
                Timeout = TimeSpan.FromMilliseconds(10000)
            });
            try
            {
                var authResult = await rest.Auth(new { Username, Password });
                if (String.IsNullOrWhiteSpace(authResult.Token))
                {
                    Message = "Ошибка авторизации!";
                    return;
                }
            }
            catch (Refit.ApiException apiEx) when (apiEx.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Message = apiEx.Content ?? apiEx.Message;
                return;
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                return;
            }
            finally
            {
                IsLoading = false;
            }

            App.Current.MainPage = new NavigationPage(new OrderPage());
        });

        public LoginPageViewModel()
        {
        }
    }
}
