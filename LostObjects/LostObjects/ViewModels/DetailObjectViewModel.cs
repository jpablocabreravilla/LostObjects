namespace LostObjects.ViewModels
{
	using LostObjects.Common.Models;
	using LostObjects.Services;
	using Plugin.Media.Abstractions;
	using Xamarin.Forms;

	public class DetailObjectViewModel : BaseViewModel
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
		public DetailObjectViewModel(Objectt objectt)
		{
			this.objectt = objectt;
			this.apiService = new ApiService();
			this.IsEnabled = true;
			this.ImageSource = objectt.ImageFullPath;
		}
		#endregion
	}
}
