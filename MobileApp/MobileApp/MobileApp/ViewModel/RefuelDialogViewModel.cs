using MobileApp.Api;
using MobileApp.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XF.Material.Forms.UI.Dialogs;

namespace MobileApp.ViewModel
{
    class RefuelDialogViewModel : BindableBase, IDialogAware
    {
        private ICollection<GasStation> gasStations;
        public ICollection<GasStation> GasStations 
        { 
            get => gasStations; 
            private set => SetProperty(ref gasStations, value); 
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        private GasStation selectedGasStation;
        public GasStation SelectedGasStation
        {
            get => selectedGasStation;
            set => SetProperty(ref selectedGasStation, value);
        }

        private double volume;
        public double Volume 
        { 
            get => volume;
            set => SetProperty(ref volume, value);
        }

        private bool canSave;
        public bool CanSave 
        { 
            get => canSave;
            set => SetProperty(ref canSave, value);
        }

        #region Commands
        public ICommand CancelCommand => new DelegateCommand(() => RequestClose?.Invoke(null));

        public ICommand OKCommand => new DelegateCommand(
            () =>
            {
                CanSave = false;
                try
                {
                    api.AddRefuel(new Refuel
                    {
                        GasStation = SelectedGasStation,
                        Volume = Volume
                    });
                    MaterialDialog.Instance.SnackbarAsync("Заправка успешно добавлена!", MaterialSnackbar.DurationShort);
                    RequestClose?.Invoke(null);
                }
                catch(Exception ex)
                {
                    Message = ex.Message;
                }
                finally
                {
                    CanSave = true;
                }
            },
            () => SelectedGasStation != null && Volume > 0 && CanSave)
            .ObservesProperty(() => Volume)
            .ObservesProperty(() => SelectedGasStation)
            .ObservesProperty(() => CanSave);
        #endregion

        private ITdsApi api;
        public RefuelDialogViewModel(ITdsApi api)
        {
            this.api = api;
            CanSave = true;
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                var result = await api.GetGasStations();
                if(result.Error != null)
                {
                    Message = result.Error;
                    return;
                }
                GasStations = result.Result;
            }
            catch(Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}
