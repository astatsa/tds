using MobileApp.Api;
using MobileApp.View;
using Prism.Commands;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI;

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

        public ICommand LoginCommand => new DelegateCommand(async () => 
            {
                IsLoading = true;
                Message = "";

                try
                {
                    var authResult = await api.Auth(new { Username, Password });
                    if (String.IsNullOrWhiteSpace(authResult.Token))
                    {
                        Message = "Ошибка авторизации!";
                        return;
                    }
                    SessionContext.Employee = authResult.Employee;
                    SessionContext.Token = authResult.Token;
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
                App.Current.MainPage = new MaterialNavigationPage(new OrderPage());
            },
            () => !IsLoading && !String.IsNullOrEmpty(Username))
            .ObservesProperty(() => IsLoading)
            .ObservesProperty(() => Username);

        public LoginPageViewModel(ITdsApi tdsApi)
        {
            this.api = tdsApi;
        }

        private readonly ITdsApi api;
    }
}
