namespace LostObjects.ViewModels
{
	using Common.Models;
	using GalaSoft.MvvmLight.Command;
	using Helpers;
	using Newtonsoft.Json;
	using Services;
	using System.Linq;
	using System.Windows.Input;
	using Views;
	using Xamarin.Forms;

	public class ObjecttItemViewModel : Objectt
	{
		#region Attibutes
		private ApiService apiService;
		#endregion

		#region Constructors
		public ObjecttItemViewModel()
		{
			this.apiService = new ApiService();
		}
		#endregion

		#region Commmands
		public ICommand EditObjecttCommand
		{
			get
			{
				return new RelayCommand(EditObjectt);
			}
		}

		private async void EditObjectt()
		{
			Settings.ObjectToEdit = JsonConvert.SerializeObject(this);
			MainViewModel.GetInstance().EditObject = new EditObjectViewModel(this);
			await App.Navigator.PushAsync(new EditObjecttPage());
		}

		public ICommand DeleteObjecttCommand
		{
			get
			{
				return new RelayCommand(DeleteObjectt);
			}
		}

		private async void DeleteObjectt()
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

			var connection = await this.apiService.CheckConnection();
			if (!connection.IsSuccess)
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
				return;
			}

			var url = Application.Current.Resources["UrlAPI"].ToString();
			var prefix = Application.Current.Resources["UrlPrefix"].ToString();
			var controller = Application.Current.Resources["UrlObjectsController"].ToString();
			var response = await this.apiService.Delete(url, prefix, controller, this.ObjectId, Settings.TokenType, Settings.AccessToken);
			if (!response.IsSuccess)
			{
				await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
				return;
			}

			var objectsViewModel = ObjectsViewModel.GetInstance();
			var deletedObjectt = objectsViewModel.MyObjects.Where(o => o.ObjectId == this.ObjectId).FirstOrDefault();
			if (deletedObjectt != null)
			{
				objectsViewModel.MyObjects.Remove(deletedObjectt);
			}

			objectsViewModel.RefreshList();
		}

		public ICommand DetailObjecttCommand
		{
			get
			{
				return new RelayCommand(DetailObjectt);
			}
		}

		private async void DetailObjectt()
		{
			Settings.ObjectToEdit = JsonConvert.SerializeObject(this);
			MainViewModel.GetInstance().DetailObject = new DetailObjectViewModel(this);
			await App.Navigator.PushAsync(new DetailObjectPage());
		}

		#endregion
	}
}

