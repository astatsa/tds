using MobileApp.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MobileApp.Api;
using System.Windows.Input;
using Prism.Commands;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Services.Dialogs;
using MobileApp.View;

namespace MobileApp.ViewModel
{
    class OrderPageViewModel : BindableBase, IApplicationLifecycleAware
    {
        private Order order;
        public Order Order
        {
            get => order;
            set
            {
                SetProperty(ref order, value);
            }
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
            (RefreshOrder,
            () => !IsRefreshing)
            .ObservesProperty(() => IsRefreshing);

        private ICommand changeStateCommand;
        public ICommand ChangeStateCommand 
        {
            get => changeStateCommand ?? 
                (changeStateCommand = new DelegateCommand(async () =>
                {
                    if (Order.OrderState.Name < OrderStates.Completed)
                    {
                        if (await SetState(Order.OrderState.Name + 1))
                        {
                            RefreshOrder();
                        }
                    }
                })
                .ObservesCanExecute(() => CanSave));
        }

        private ICommand refuelAddCommand;
        public ICommand RefuelAddCommand 
        {
            get => refuelAddCommand ??
                (refuelAddCommand = new DelegateCommand(() =>
                {
                    dialogService.ShowDialog(nameof(RefuelDialog));
                }));
        }

        private bool canSave;
        public bool CanSave
        {
            get => canSave;
            set => SetProperty(ref canSave, value);
        }


        public OrderPageViewModel(ITdsApi tdsApi, IDialogService dialogService)
        {
            this.api = tdsApi;
            this.dialogService = dialogService;

            RefreshCommand.Execute(null);
        }

        private ITdsApi api;
        private readonly IDialogService dialogService;

        private async void RefreshOrder()
        {
            IsRefreshing = true;
            try
            {
                CanSave = false;
                var result = await api.GetCurrentOrder();
                if (result.Error != null)
                {
                    Message = result.Error;
                    return;
                }

                if(isForeground && result.Result != null && result.Result.OrderState.Name == OrderStates.New)
                {
                    if(await SetState(OrderStates.Viewed, result.Result))
                    {
                        result.Result.OrderState.Name = OrderStates.Viewed;
                    }
                }
                Order = result.Result;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            finally
            {
                IsRefreshing = false;
                CanSave = true;
            }
        }

        private async Task<bool> SetState(OrderStates state, Order order = null)
        {
            if(order == null)
            {
                order = Order;
            }
            try
            {
                CanSave = false;
                var result = await api.SetOrderState(order.Id, state);
                if (result.Error != null)
                {
                    Message = result.Error;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
            finally
            {
                CanSave = true;
            }
            return true;
        }

        private bool isForeground = true;
        public async void OnResume()
        {
            isForeground = true;
            if(Order != null && Order.OrderState.Name == OrderStates.New)
            {
                if(await SetState(OrderStates.Viewed))
                {
                    RefreshOrder();
                }
            }
        }

        public void OnSleep()
        {
            isForeground = false;
        }
    }
}
