namespace LostObjects.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using LostObjects.Common.Models;
	using LostObjects.Helpers;
	using LostObjects.Services;
	using Plugin.Geolocator;
	using Plugin.Geolocator.Abstractions;
	using Plugin.Media;
	using Plugin.Media.Abstractions;
	using System;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using Xamarin.Forms;

	public class AddObjectViewModel : BaseViewModel
	{
		#region Atributes
		private MediaFile file;
		private ImageSource imageSource;
		private ApiService apiService;

		private bool isRunning;

		private bool isEnabled;

		#endregion

		#region Properties

		public string Name { get; set; }

		public string Type { get; set; }

		public string PhoneContact { get; set; }

		public DateTime PublishOn { get; set; }

		public string Location { get; set; }

		public string Description { get; set; }

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { this.SetValue(ref this.isRunning, value); }
		}

		public bool IsEnabled
		{
			get { return this.isEnabled; }
			set { this.SetValue(ref this.isEnabled, value); }
		}

		public ImageSource ImageSource
		{
			get { return this.imageSource; }
			set { this.SetValue(ref this.imageSource, value); }
		}
		#endregion

		#region Constructors
		public AddObjectViewModel()
		{
			this.apiService = new ApiService();
			this.IsEnabled = true;
			this.ImageSource = "NoObject";

		}
		#endregion

		#region Commands
		public ICommand ChangeImageCommand
		{
			get
			{
				return new RelayCommand(ChangeImage);
			}
		}

		private async void ChangeImage()
		{
			await CrossMedia.Current.Initialize();

			var source = await Application.Current.MainPage.DisplayActionSheet(
				Languages.ImageSource,
				Languages.Cancel,
				null,
				Languages.FromGallery,
				Languages.NewPicture);

			if (source == Languages.Cancel)
			{
				this.file = null;
				return;
			}

			if (source == Languages.NewPicture)
			{
				this.file = await CrossMedia.Current.TakePhotoAsync(
					new StoreCameraMediaOptions
					{
						Directory = "Sample",
						Name = "test.jpg",
						PhotoSize = PhotoSize.Small,
					}
				);
			}
			else
			{
				this.file = await CrossMedia.Current.PickPhotoAsync();
			}

			if (this.file != null)
			{
				this.ImageSource = ImageSource.FromStream(() =>
				{
					var stream = this.file.GetStream();
					return stream;
				});
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				return new RelayCommand(Save);
			}
		}

		private async void Save()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					Languages.NameError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Type))
			{
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					Languages.TypeError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.PhoneContact))
			{
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					Languages.PhoneContactError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Location))
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Error,
					Languages.LocationPlaceholder,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Description))
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Error,
					Languages.DescriptionsError,
					Languages.Accept);
				return;
			}
			if ((this.Description.Length > 150))
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Description,
					Languages.MaxCharacter,
					Languages.Accept);
				return;
			}

			this.IsRunning = true;
			this.IsEnabled = false;

			var connection = await this.apiService.CheckConnection();
			if (!connection.IsSuccess)
			{
				this.IsRunning = false;
				this.IsEnabled = true;
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					connection.Message,
					Languages.Accept);
				return;
			}

			byte[] imageArray = null;
			if (this.file != null)
			{
				imageArray = FilesHelper.ReadFully(this.file.GetStream());
			}

			var location = await this.GetLocation();

			var objectt = new Objectt
			{
				Name = this.Name,
				Type = this.Type,
				PhoneContact = this.PhoneContact,
				PublishOn = this.PublishOn,
				Location = this.Location,
				Description = this.Description,
				IsDelivered = false,
				ImageArray = imageArray,
				Latitude = location == null ? 0 : location.Latitude,
				Longitude = location == null ? 0 : location.Longitude,
			};

			var url = Application.Current.Resources["UrlAPI"].ToString();
			var Prefix = Application.Current.Resources["UrlPrefix"].ToString();
			var Controller = Application.Current.Resources["UrlObjectsController"].ToString();
			var response = await this.apiService.Post(url, Prefix, Controller, objectt, Settings.TokenType, Settings.AccessToken);

			if (!response.IsSuccess)
			{
				this.IsRunning = false;
				this.IsEnabled = true;
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					response.Message,
					Languages.Accept);
				return;
			}

			var newObjectt = (Objectt)response.Result;
			var objectsViewModel = ObjectsViewModel.GetInstance();
			objectsViewModel.MyObjects.Add(newObjectt);
			objectsViewModel.RefreshList();
			this.IsRunning = false;
			this.IsEnabled = true;
			await App.Navigator.PopAsync();
		}

		private async Task<Position> GetLocation()
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			var location = await locator.GetPositionAsync();
			return location;
		}
		#endregion

	}
}
