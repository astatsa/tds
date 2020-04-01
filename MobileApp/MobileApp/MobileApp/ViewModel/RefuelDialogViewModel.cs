using MobileApp.Api;
using MobileApp.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

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


        private ITdsApi api;
        public RefuelDialogViewModel(ITdsApi api)
        {
            this.api = api;
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
