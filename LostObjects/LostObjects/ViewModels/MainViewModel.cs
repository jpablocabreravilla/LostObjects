namespace LostObjects.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using LostObjects.Common.Models;
	using LostObjects.Helpers;
	using LostObjects.Interfaces;
	using LostObjects.Views;
	using Newtonsoft.Json;
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using Xamarin.Forms;

	public class MainViewModel
	{

		#region Atributes
		public DetailObjectViewModel DetailObject { get; set; }

		public EditObjectViewModel EditObject { get; set; }

		public ObjectsViewModel Objects { get; set; }

		public AddObjectViewModel AddObject { get; set; }

		public LoginViewModel Login { get; set; }

		public RegisterViewModel Register { get; set; }

		public ObservableCollection<MenuItemViewModel> Menu { get; set; }

		public MyUserASP UserASP { get; set; }

		public string UserFullName
		{
			get
			{
				if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
				{
					return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
				}

				return null;
			}
		}

		public string UserImageFullPath
		{
			get
			{
				foreach (var claim in this.UserASP.Claims)
				{
					if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri")
					{
						if (claim.ClaimValue.StartsWith("~"))
						{
							return $"https://lostobjectsapi.azurewebsites.net{this.UserASP.Claims[3].ClaimValue.Substring(1)}";
						}

						return claim.ClaimValue;
					}
				}
				return null;
			}
		}
		#endregion

		#region Constructor

		public MainViewModel()
		{
			instance = this;
			this.LoadMenu();
		}

		#endregion

		#region Singelton
		private static MainViewModel instance;

		public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
				return new MainViewModel();
			}

			return instance;
		}
		#endregion

		#region Methods
		private void LoadMenu()
		{
			this.Menu = new ObservableCollection<MenuItemViewModel>();

			this.Menu.Add(new MenuItemViewModel
			{
				Icon = "ic_location_on",
				PageName = "AboutPage",
				Title = Languages.maps,
			});

			this.Menu.Add(new MenuItemViewModel
			{
				Icon = "ic_info",
				PageName = "InfoDevelopersPage",
				Title = Languages.About,
			});

			this.Menu.Add(new MenuItemViewModel
			{
				Icon = "ic_exit_to_app",
				PageName = "LoginPage",
				Title = Languages.Exit,
			});
		}
		public void RegisterDevice()
		{
			var register = DependencyService.Get<IRegisterDevice>();
			register.RegisterDevice();
		}
		#endregion

		#region Commands
		public ICommand EditObjectCommand
		{
			get
			{
				return new RelayCommand(GoEditObject);
			}
		}

		public async void GoEditObject()
		{
			var objectt = JsonConvert.DeserializeObject<Objectt>(Settings.ObjectToEdit);
			MainViewModel.GetInstance().EditObject = new EditObjectViewModel(objectt);
			await App.Navigator.PushAsync(new EditObjecttPage());
		}


		public ICommand AddObjectCommand
		{
			get
			{
				return new RelayCommand(GoToAddObject);
			}
		}

		private async void GoToAddObject()
		{
			this.AddObject = new AddObjectViewModel();
			await App.Navigator.PushAsync(new AddObjectPage());
		}
		#endregion

	}
}
