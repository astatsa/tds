using MobileApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModel
{
    class MainPageViewModel
    {
        public Command ExitCommand;
        public Command LoginCommand => new Command<INavigation>(async (n) => 
        {
            await n.PushModalAsync(new NavigationPage(new OrderPage()));
        });
    }
}
