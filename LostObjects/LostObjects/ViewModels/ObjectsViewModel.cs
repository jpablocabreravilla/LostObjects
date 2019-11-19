namespace LostObjects.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using LostObjects.Common.Models;
	using LostObjects.Helpers;
	using LostObjects.Services;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using Xamarin.Forms;

	public class ObjectsViewModel: BaseViewModel
    {
        #region Attributes
        private string filter;

        public List<Objectt> MyObjects { get; set; }

        private ApiService apiService;

        private DataService dataService;


        private bool isRefreshing;

        private ObservableCollection<ObjecttItemViewModel> objects;

        #endregion

        #region Properties

        public ObservableCollection<ObjecttItemViewModel> Objects
        {
            get { return this.objects; }
            set { this.SetValue(ref this.objects, value); }
        } 

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }
        #endregion

        #region Constructors
        public ObjectsViewModel()
        {

            instance = this;
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadObjects();

        }
        #endregion

        #region Singelton
        private static ObjectsViewModel instance;

        public static ObjectsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ObjectsViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private async void LoadObjects()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (connection.IsSuccess)
            {
                var answer = await this.LoadObjectsFromAPI();
                if (answer)
                {
                    this.SaveObjectsToDB();
                }
            }
            else
            {
                await this.LoadObjectsFromDB();
            }

            if (this.MyObjects == null || this.MyObjects.Count == 0)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoObjectsMessage, Languages.Accept);
                return;
            }

            this.RefreshList();
            this.IsRefreshing = false;          
        }


        private async Task LoadObjectsFromDB()
        {
            this.MyObjects = await this.dataService.GetAllObjects();
        }

        private async Task SaveObjectsToDB()
        {
            await this.dataService.DeleteAllObjects();
            this.dataService.Insert(this.MyObjects);
        }

        private async Task<bool> LoadObjectsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var Prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var Controller = Application.Current.Resources["UrlObjectsController"].ToString();
            var response = await this.apiService.GetList<Objectt>(url, Prefix, Controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }
            this.MyObjects = (List<Objectt>)response.Result;
            return true;
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListObjecttItemViewModel = MyObjects.Select(o => new ObjecttItemViewModel
                {
                    Name = o.Name,
                    Type = o.Type,
                    PhoneContact = o.PhoneContact,
                    PublishOn = o.PublishOn,
                    Location = o.Location,
                    Description = o.Description,
                    IsDelivered = o.IsDelivered,
                    ImageArray = o.ImageArray,
                    ImagePath = o.ImagePath,
                    ObjectId = o.ObjectId,
                });

                this.Objects = new ObservableCollection<ObjecttItemViewModel>(myListObjecttItemViewModel.OrderBy(o => o.Name));
            }
            else
            {
                var myListObjecttItemViewModel = MyObjects.Select(o => new ObjecttItemViewModel
                {
                    Name = o.Name,
                    Type = o.Type,
                    PhoneContact = o.PhoneContact,
                    PublishOn = o.PublishOn,
                    Location = o.Location,
                    Description = o.Description,
                    IsDelivered = o.IsDelivered,
                    ImageArray = o.ImageArray,
                    ImagePath = o.ImagePath,
                    ObjectId = o.ObjectId,
                }).Where(o => (o.Name.ToLower().Contains(this.Filter.ToLower()) || o.Type.ToLower().Contains(this.Filter.ToLower()))).ToList();

                this.Objects = new ObservableCollection<ObjecttItemViewModel>(myListObjecttItemViewModel.OrderBy(o => o.Name));
            }
        }
        #endregion

        #region Commands

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadObjects);
            }
        }
        #endregion

    }
}
