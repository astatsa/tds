﻿using Prism.Mvvm;
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
using TDSDTO.Documents;
using MobileApp.Repositories;
using MobileApp.Services;
using System.Runtime.CompilerServices;

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
                //SetProperty(ref order, value);
                order = value;
                RaisePropertyChanged();
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
                if (Order.OrderState < OrderStates.Completed)
                {
                    if (await SetState(Order.OrderState + 1))
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
                            GasStationId = vm.SelectedGasStation.Id,
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


        public OrderPageViewModel(ITdsApi tdsApi, IDialogService dialogService, ApiRepository apiRepository, RepeatFailedMethodService repeatFailedMethodService)
        {
            this.api = tdsApi;
            this.dialogService = dialogService;
            this.apiRepository = apiRepository;

            //repeatFailedMethodService.StartTryMethodsCall();
            //repeatFailedMethodService.RepeatAllFailedMethods();

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
        private readonly ApiRepository apiRepository;

        private async void RefreshOrder()
        {
            IsRefreshing = true;
            try
            {
                CanSave = false;
                await LoadOrder();
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
                double weight = 0;
                if(state == OrderStates.Loaded || state == OrderStates.Completed)
                {
                    var sWeight = await MaterialDialog.Instance.InputAsync(message: "Введите вес", 
                        inputPlaceholder: "Вес", dismissiveText: "Отмена",
                        configuration: new XF.Material.Forms.UI.Dialogs.Configurations.MaterialInputDialogConfiguration
                        {
                            InputType = MaterialTextFieldInputType.Numeric
                        });
                    if(String.IsNullOrEmpty(sWeight))
                    {
                        return false;
                    }
                    double.TryParse(sWeight, out weight);
                    if (weight <= 0)
                    {
                        _ = MaterialDialog.Instance.SnackbarAsync("Не указан вес!", MaterialSnackbar.DurationShort);
                        return false;
                    }
                }

                await apiRepository.SetOrderState(order.Id, state, weight, true);
                /*var result = await api.SetOrderState(order.Id, new TDSDTO.OrderWeightAndState
                {
                    OrderState = state,
                    Weight = weight
                });
                if (result.Error != null)
                {
                    Message = result.Error;
                    return false;
                }*/
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

        //private bool isForeground = true;
        public void OnResume()
        {
            /*isForeground = true;
            if(Order != null && Order.OrderState == OrderStates.New)
            {
                if(await SetState(OrderStates.Viewed))
                {
                    RefreshOrder();
                }
            }*/
        }

        public void OnSleep()
        {
            //isForeground = false;
        }

        private bool loadOrderRepeaterAlive;
        private async Task LoadOrder()
        {
            try
            {
                var result = await api.GetCurrentOrder();

                if (result.Error != null)
                {
                    Order = null;
                    Message = result.Error;
                    loadOrderRepeaterAlive = false;
                    return;
                }

                Order = result.Result;
                if(Order == null && !loadOrderRepeaterAlive)
                {
                    loadOrderRepeaterAlive = true;
                    await Task.Run(
                        async () =>
                        {
                            while(loadOrderRepeaterAlive)
                            {
                                await Task.Delay(10000);
                                await LoadOrder();
                            }
                        });
                }
                else if(Order != null)
                    loadOrderRepeaterAlive = false;
            }
            catch(Exception ex)
            {
                Message = ex.Message;
            }
        }


    }
}
