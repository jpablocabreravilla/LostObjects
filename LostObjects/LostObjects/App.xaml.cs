﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LostObjects
{
	using LostObjects.Common.Models;
	using LostObjects.Helpers;
	using LostObjects.Services;
	using LostObjects.ViewModels;
	using Newtonsoft.Json;
	using System.Threading.Tasks;
	using Views;
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public static MasterPage Master { get; internal set; }


        public App()
        {
            InitializeComponent();

			var mainViewModel = MainViewModel.GetInstance();

			if (Settings.IsRemembered)
			{

				if (!string.IsNullOrEmpty(Settings.UserASP))
				{
					mainViewModel.UserASP = JsonConvert.DeserializeObject<MyUserASP>(Settings.UserASP);
                    MainViewModel.GetInstance().RegisterDevice();
                }

				mainViewModel.Objects = new ObjectsViewModel();
				this.MainPage = new MasterPage();
			}
			else
			{
				mainViewModel.Login = new LoginViewModel();
				this.MainPage = new NavigationPage(new LoginPage());
			}
		}

		public static Action HideLoginView
		{
			get
			{
				return new Action(() => Current.MainPage = new NavigationPage(new LoginPage()));
			}
		}


        public static async Task NavigateToProfile(TokenResponse token)
		{
			if (token == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPage());
				return;
			}

			Settings.IsRemembered = true;
			Settings.AccessToken = token.AccessToken;
			Settings.TokenType = token.TokenType;

			var apiService = new ApiService();
			var url = Application.Current.Resources["UrlAPI"].ToString();
			var prefix = Application.Current.Resources["UrlPrefix"].ToString();
			var controller = Application.Current.Resources["UrlUsersController"].ToString();
			var response = await apiService.GetUser(url, prefix, $"{controller}/GetUser", token.UserName, token.TokenType, token.AccessToken);
			if (response.IsSuccess)
			{
				var userASP = (MyUserASP)response.Result;
				MainViewModel.GetInstance().UserASP = userASP;
                MainViewModel.GetInstance().RegisterDevice();
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
			}

			MainViewModel.GetInstance().Objects = new ObjectsViewModel();
			Application.Current.MainPage = new MasterPage();
		}

		protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
