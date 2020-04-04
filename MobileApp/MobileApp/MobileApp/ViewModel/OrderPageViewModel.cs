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
using System.ComponentModel;
using XF.Material.Forms.UI.Dialogs;
using Unity;
using XF.Material.Forms.UI;

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

        public ICommand ChangeStateCommand => new DelegateCommand(async () =>
            {
                if (Order.OrderState.Name < OrderStates.Completed)
                {
                    if (await SetState(Order.OrderState.Name + 1))
                    {
                        RefreshOrder();
                    }
                }
            },
            () => CanSave)
            .ObservesProperty(() => CanSave);
            //.ObservesCanExecute(() => CanSave);

        public ICommand RefuelAddCommand => new DelegateCommand(async () =>
            {
                var view = new RefuelDialog();
                var result = await MaterialDialog.Instance.ShowCustomContentAsync(view, "Информация о заправке", confirmingText: "OK", dismissiveText: "Отмена");
                if(result.HasValue && result.Value && view.BindingContext is RefuelDialogViewModel vm)
                {
                    if(vm.SelectedGasStation == null)
                    {
                        _ = MaterialDialog.Instance.SnackbarAsync("Не указана АЗС!", MaterialSnackbar.DurationShort);
                        return;
                    }
                    if(vm.Volume <= 0)
                    {
                        _ = MaterialDialog.Instance.SnackbarAsync("Не указан объем!", MaterialSnackbar.DurationShort);
                        return;
                    }
                    try
                    {
                        var apiResult = await api.AddRefuel(new Refuel
                        {
                            GasStation = vm.SelectedGasStation,
                            Volume = vm.Volume
                        });
                        if(apiResult.Error != null)
                        {
                            Message = apiResult.Error;
                            return;
                        }
                        _ = MaterialDialog.Instance.SnackbarAsync("Заправка успешно добавлена!", MaterialSnackbar.DurationShort);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                    }
                }
            });

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

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(Message) && !String.IsNullOrWhiteSpace(Message))
            {
                MaterialDialog.Instance.SnackbarAsync(Message, "Закрыть", MaterialSnackbar.DurationIndefinite);
                Message = null;
            }
            base.OnPropertyChanged(args);
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
                if(state == OrderStates.Loaded || state == OrderStates.Completed)
                {
                    var weight = await MaterialDialog.Instance.InputAsync(message: "Введите вес", 
                        inputPlaceholder: "Вес", dismissiveText: "Отмена",
                        configuration: new XF.Material.Forms.UI.Dialogs.Configurations.MaterialInputDialogConfiguration
                        {
                            InputType = MaterialTextFieldInputType.Numeric
                        });
                }

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
