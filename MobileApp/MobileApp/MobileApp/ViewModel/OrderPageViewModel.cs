using MobileApp.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MobileApp.Api;
using System.Windows.Input;
using Prism.Commands;

namespace MobileApp.ViewModel
{
    class OrderPageViewModel : BindableBase
    {
        private Order order;
        public Order Order
        {
            get => order;
            set => SetProperty(ref order, value);
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }


        public ICommand RefreshCommand => new DelegateCommand
            (async () =>
            {
                IsRefreshing = true;
                try
                {
                    var result = await api.GetCurrentOrder();
                    if (result.Error != null)
                    {
                        Message = result.Error;
                        return;
                    }

                    Order = result.Result;
                }
                catch(Exception ex)
                {
                    Message = ex.Message;
                }
                finally
                {
                    IsRefreshing = false;
                }
            },
            () => !IsRefreshing)
            .ObservesProperty(() => IsRefreshing);

        public OrderPageViewModel(ITdsApi tdsApi)
        {
            this.api = tdsApi;

            RefreshCommand.Execute(null);
        }

        private ITdsApi api;
    }
}
