namespace LostObjects.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string Objects
        {
            get { return Resource.Objects; }
        }

        public static string Edit
        {
            get { return Resource.Edit; }
        }

        public static string Delete
        {
            get { return Resource.Delete; }
        }

        public static string Description
        {
            get { return Resource.Description; }
        }

        public static string Type
        {
            get { return Resource.Type; }
        }

        public static string PhoneContact
        {
            get { return Resource.PhoneContact; }
        }

        public static string PublishOn
        {
            get { return Resource.PublishOn; }
        }

        public static string Location
        {
            get { return Resource.Location; }
        }

		public static string NameError
		{
			get { return Resource.NameError; }
		}

		public static string TypeError
		{
			get { return Resource.TypeError; }
		}

		public static string PhoneContactError
		{
			get { return Resource.PhoneContactError; }
		}

		public static string DescriptionsError
		{
			get { return Resource.DescriptionsError; }
		}
        public static string ImageSource
        {
            get { return Resource.ImageSource; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string NewPicture
        {
            get { return Resource.NewPicture; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }
        public static string Yes
        {
            get { return Resource.Yes; }
        }
        public static string No
        {
            get { return Resource.No; }
        }
        public static string DeleteConfirmation
        {
            get { return Resource.DeleteConfirmation; }
        }
        public static string Confirm
        {
            get { return Resource.Confirm; }
        }
        public static string EditObject
        {
            get { return Resource.EditObject; }
        }
        public static string Search
        {
            get { return Resource.Search; }
        }

        public static string Login
        {
            get { return Resource.Login; }
        }

        public static string EMail
        {
            get { return Resource.EMail; }
        }

        public static string EmailPlaceHolder
        {
            get { return Resource.EmailPlaceHolder; }
        }

        public static string Password
        {
            get { return Resource.Password; }
        }

        public static string PasswordPlaceHolder
        {
            get { return Resource.PasswordPlaceHolder; }
        }

        public static string Rememberme
        {
            get { return Resource.Rememberme; }
        }

        public static string Forgot
        {
            get { return Resource.Forgot; }
        }

        public static string Register
        {
            get { return Resource.Register; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }

        public static string SomethingWrong
        {
            get { return Resource.SomethingWrong; }
        }

        public static string Menu
        {
            get { return Resource.Menu; }
        }

        public static string Setup
        {
            get { return Resource.Setup; }
        }

        public static string maps
		{
            get { return Resource.maps; }
        }

        public static string Exit
        {
            get { return Resource.Exit; }
        }
        public static string NoObjectsMessage
        {
            get { return Resource.NoObjectsMessage; }
        }

        public static string FirstName
        {
            get { return Resource.FirstName; }
        }

        public static string FirstNamePlaceholder
        {
            get { return Resource.FirstNamePlaceholder; }
        }

        public static string LastName
        {
            get { return Resource.LastName; }
        }

        public static string LastNamePlaceholder
        {
            get { return Resource.LastNamePlaceholder; }
        }

        public static string Phone
        {
            get { return Resource.Phone; }
        }

        public static string PhonePlaceHolder
        {
            get { return Resource.PhonePlaceHolder; }
        }

        public static string PasswordConfirm
        {
            get { return Resource.PasswordConfirm; }
        }

        public static string PasswordConfirmPlaceHolder
        {
            get { return Resource.PasswordConfirmPlaceHolder; }
        }

        public static string Address
        {
            get { return Resource.Address; }
        }

        public static string AddressPlaceHolder
        {
            get { return Resource.AddressPlaceHolder; }
        }

        public static string FirstNameError
        {
            get { return Resource.FirstNameError; }
        }

        public static string LastNameError
        {
            get { return Resource.LastNameError; }
        }

        public static string EMailError
        {
            get { return Resource.EMailError; }
        }

        public static string PhoneError
        {
            get { return Resource.PhoneError; }
        }

        public static string PasswordError
        {
            get { return Resource.PasswordError; }
        }

        public static string PasswordConfirmError
        {
            get { return Resource.PasswordConfirmError; }
        }

        public static string PasswordsNoMatch
        {
            get { return Resource.PasswordsNoMatch; }
        }

        public static string RegisterConfirmation
        {
            get { return Resource.RegisterConfirmation; }
        }

        public static string MaxCharacter
        {
            get { return Resource.MaxCharacter; }
        }

		public static string LocationPlaceholder
		{
			get { return Resource.LocationPlaceholder; }
		}
		
		public static string LocationError
		{
			get { return Resource.LocationError; }
		}
		
		public static string Detail
		{
			get { return Resource.Detail; }
		}
		
		public static string DetailObject
		{
			get { return Resource.DetailObject; }
		}
		
		public static string Missingitem
		{
			get { return Resource.Missingitem; }
		}

		public static string About
		{
			get { return Resource.About; }
		}

		public static string InfoDevelopers
		{
			get { return Resource.InfoDevelopers; }
		}
	}
}
