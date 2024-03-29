﻿namespace LostObjects.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using Helpers;
	using System.Windows.Input;
	using Views;
	using Xamarin.Forms;

	public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(Goto);
            }
        }

        private async void Goto()
        {
            if (this.PageName == "LoginPage")
            {
                Settings.AccessToken = string.Empty;
                Settings.TokenType = string.Empty;
                Settings.IsRemembered = false;
                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else 
			if (this.PageName == "AboutPage")
            {
                App.Master.IsPresented = false;
                await App.Navigator.PushAsync(new MapPage());
            }
			if (this.PageName == "InfoDevelopersPage")
			{
				App.Master.IsPresented = false;
				await App.Navigator.PushAsync(new InfoDevelopersPage());
			}
		}
        #endregion
    }
}
