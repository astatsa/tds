using MobileApp.Api;
using MobileApp.View;
using Prism.Commands;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;

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
                    var authResult = await api.Auth(new { Username, Password = GetPasswordHash() });
                    if (String.IsNullOrWhiteSpace(authResult.Token))
                    {
                        Message = "Ошибка авторизации!";
                        return;
                    }
                    SessionContext.Employee = authResult.Employee;
                    SessionContext.Token = authResult.Token;
                }
                catch (Refit.ApiException apiEx) when (apiEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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

        private string GetPasswordHash()
        {
            using (var sha = SHA256.Create())
            {
                return String.Join("", sha.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
            }
        }

        private readonly ITdsApi api;
    }
}
