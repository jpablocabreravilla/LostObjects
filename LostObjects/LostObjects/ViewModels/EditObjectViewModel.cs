namespace LostObjects.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using LostObjects.Common.Models;
	using LostObjects.Helpers;
	using LostObjects.Services;
	using Plugin.Media;
	using Plugin.Media.Abstractions;
	using System.Linq;
	using System.Windows.Input;
	using Xamarin.Forms;

	public class EditObjectViewModel : BaseViewModel
	{
		#region Attributes
		private Objectt objectt;

		private MediaFile file;

		private ImageSource imageSource;

		private ApiService apiService;

		private bool isRunning;

		private bool isEnabled;
		#endregion

		#region Properties
		public Objectt Objectt
		{
			get { return this.objectt; }
			set { this.SetValue(ref this.objectt, value); }
		}
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
		public EditObjectViewModel(Objectt objectt)
		{
			this.objectt = objectt;
			this.apiService = new ApiService();
			this.IsEnabled = true;
			this.ImageSource = objectt.ImageFullPath;
		}
		#endregion

		#region Commands
		public ICommand DeleteCommand
		{
			get
			{
				return new RelayCommand(Delete);
			}
		}

		private async void Delete()
		{
			var answer = await Application.Current.MainPage.DisplayAlert(
			   Languages.Confirm,
			   Languages.DeleteConfirmation,
			   Languages.Yes,
			   Languages.No);
			if (!answer)
			{
				return;
			}
			this.IsRunning = true;
			this.IsEnabled = false;

			var connection = await this.apiService.CheckConnection();
			if (!connection.IsSuccess)
			{
				this.IsRunning = false;
				this.IsEnabled = true;
				await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
				return;
			}

			var url = Application.Current.Resources["UrlAPI"].ToString();
			var prefix = Application.Current.Resources["UrlPrefix"].ToString();
			var controller = Application.Current.Resources["UrlObjectsController"].ToString();
			var response = await this.apiService.Delete(url, prefix, controller, this.Objectt.ObjectId, Settings.TokenType, Settings.AccessToken);
			if (!response.IsSuccess)
			{
				this.IsRunning = false;
				this.IsEnabled = true;
				await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
				return;
			}

			var objectsViewModel = ObjectsViewModel.GetInstance();
			var deletedObjectt = objectsViewModel.MyObjects.Where(o => o.ObjectId == this.Objectt.ObjectId).FirstOrDefault();
			if (deletedObjectt != null)
			{
				objectsViewModel.MyObjects.Remove(deletedObjectt);
			}

			objectsViewModel.RefreshList();
			this.IsRunning = false;
			this.IsEnabled = true;
			await App.Navigator.PopAsync();
			await App.Navigator.PopAsync();
		}

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
			if (string.IsNullOrEmpty(this.Objectt.Name))
			{
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					Languages.NameError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Objectt.Type))
			{
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					Languages.TypeError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Objectt.PhoneContact))
			{
				await Application.Current.MainPage.DisplayAlert(
					Languages.Error,
					Languages.PhoneContactError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Objectt.Location))
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Error,
					Languages.LocationError,
					Languages.Accept);
				return;
			}

			if (string.IsNullOrEmpty(this.Objectt.Description))
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Error,
					Languages.DescriptionsError,
					Languages.Accept);
				return;
			}

			if (this.Objectt.Description.Length > 150)
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
				this.Objectt.ImageArray = imageArray;
			}


			var url = Application.Current.Resources["UrlAPI"].ToString();
			var Prefix = Application.Current.Resources["UrlPrefix"].ToString();
			var Controller = Application.Current.Resources["UrlObjectsController"].ToString();
			var response = await this.apiService.Put(url, Prefix, Controller, this.Objectt, this.Objectt.ObjectId, Settings.TokenType, Settings.AccessToken);

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
			var oldObjectt = objectsViewModel.MyObjects.Where(o => o.ObjectId == this.Objectt.ObjectId).FirstOrDefault();
			if (oldObjectt != null)
			{
				objectsViewModel.MyObjects.Remove(oldObjectt);
			}

			objectsViewModel.MyObjects.Add(newObjectt);
			objectsViewModel.RefreshList();
			this.IsRunning = false;
			this.IsEnabled = true;
			await App.Navigator.PopAsync();
			await App.Navigator.PopAsync();

		}
		#endregion
	}
}
