using MobileApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModel
{
    class LoginPageViewModel
    {
        public Command ExitCommand;
        public Command LoginCommand => new Command<INavigation>((n) => 
        {
            App.Current.MainPage = new NavigationPage(new OrderPage());
        });
    }
}
