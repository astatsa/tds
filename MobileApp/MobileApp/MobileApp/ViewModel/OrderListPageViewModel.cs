using MobileApp.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TDSDTO.Documents;
using Xamarin.Forms;

namespace MobileApp.ViewModel
{
    class OrderListPageViewModel : BindableObject
    {
        private ObservableCollection<Order> orders;
        public ObservableCollection<Order> Orders 
        { 
            get => orders; 
            private set
            {
                orders = value;
                OnPropertyChanged();
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set 
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();
                }
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

        public Command RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            try
            {
                var result = await api.GetOrders(SessionContext.Employee);
                if(result.Error != null)
                {
                    Message = result.Error;
                    return;
                }
                Orders = new ObservableCollection<Order>(result.Result);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            finally
            {
                IsRefreshing = false;
            }
        });

        public OrderListPageViewModel(ITdsApi api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));

            RefreshCommand.Execute(null);
        }

        private readonly ITdsApi api;
    }
}
