﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XFDemo.ViewModel
{
    public class ReverseStringViewModel : ViewModelBase
    {
        #region Private Fields

        private IDialogService dialogService;
        private string reverseString;

        private string myText;

        #endregion

        #region Private Methods

        private async Task ReverseCommandExectue()
        {
            if (string.IsNullOrEmpty(this.MyText))
            {
                await this.dialogService.ShowMessage("The input text is empty", "Reverse String Demo");
            }
            else
            {
                var charArray = this.myText.ToCharArray();
                Array.Reverse(charArray);
                this.ReverseString = new string(charArray);

                await this.dialogService.ShowMessage($"The input text in reversed order is: {this.ReverseString}", "Reverse String Demo");
            }
        }

        #endregion

        #region Public Constructors

        public ReverseStringViewModel(IDialogService dialogService)
        {
            this.ReverseCommand = new Command(async () => await this.ReverseCommandExectue());
            this.dialogService = dialogService;
            this.MyText = "hola";
        }

        #endregion

        #region Public Properties

        public string ReverseString
        {
            get { return reverseString; }
            set
            {
                reverseString = value;
                this.RaisePropertyChanged();
            }
        }

        public string MyText
        {
            get { return myText; }
            set
            {
                this.myText = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ReverseCommand { get; }

        #endregion
    }
}