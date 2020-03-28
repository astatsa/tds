using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModel
{
    class OrderPageViewModel : BindableObject
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
                Orders = new ObservableCollection<Order>(await SessionContext.Api.GetOrders(SessionContext.Employee));
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

        public OrderPageViewModel()
        {
            RefreshCommand.Execute(null);
        }
    }
}
