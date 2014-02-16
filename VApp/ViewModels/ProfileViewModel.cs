using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VApp.Common;
using VApp.VAWebsite;
using System.Windows.Input;

namespace VApp.ViewModels
{
    public class ProfileViewModel : DefaultNotifyPropertyChanged
    {
        private string firstName;

        private string middleName;

        private string lastName;

        private string alias;

        private string occupation;

        private ObservableCollection<string> maritalStatuses;

        private string maritalStatus;

        private string contactMethod;

        private string email;

        private string homePhone;

        private string mobilePhone;

        private string workPhone;

        private string fax;

        private string pager;

        private string address1;

        private string address2;

        private string city;

        private string state;

        private List<string> states;

        private string zip;

        private string province;

        private string country;

        private List<string> countries;

        private ObservableCollection<string> contactMethods;

        private bool isVAPatient;

        private bool isVeteran;

        private bool isVeteranAdvocate;

        private bool isVAEmployee;

        private bool isHealthProvider;

        private bool isOther;

        private string bloodType;

        private List<string> bloodTypes;

        private bool isOrganDonor;

        private bool privacyAccepted;

        private bool termsAccepted;

        private DelegateCommand saveCommand;

        public ProfileViewModel()
        {
            this.PopulateFromWeb();
        }

        public string FirstName
        {
            get{   return this.firstName;  }

            set
            {
                if (value != null && !value.Equals(this.firstName))
                {
                    this.firstName = value;
                    this.NotifyPropertyChanged("FirstName");
                }
            }
        }

        public string MiddleName
        {
            get{    return this.middleName; }

            set
            {
                if (value != null && !value.Equals(this.middleName))
                {
                    this.middleName = value;
                    this.NotifyPropertyChanged("MiddleName");
                }
            }
        }

        public string LastName
        {
            get{    return this.lastName;   }

            set
            {
                if (value != null && !value.Equals(this.lastName))
                {
                    this.lastName = value;
                    this.NotifyPropertyChanged("LastName");
                }
            }
        }

        public string Alias
        {
            get{    return this.alias;  }

            set
            {
                if (value != null && !value.Equals(this.alias))
                {
                    this.alias = value;
                    this.NotifyPropertyChanged("Alias");
                }
            }
        }

        public string Occupation
        {
            get{    return this.occupation; }

            set
            {
                if (value != null && !value.Equals(this.occupation))
                {
                    this.occupation = value;
                    this.NotifyPropertyChanged("Occupation");
                }
            }
        }

        public ICollection<string> MaritalStatuses
        {
            get
            {
                if (this.maritalStatus == null)
                {
                    this.maritalStatuses = new ObservableCollection<string>() { "Divorced", "Married", "Single", "Widowed" };
                }

                return this.maritalStatuses;
            }
        }

        public string MaritalStatus
        {
            get{    return this.maritalStatus;  }

            set
            {
                if (value != null && !value.Equals(this.maritalStatus))
                {
                    this.maritalStatus = value;
                    this.NotifyPropertyChanged("MaritalStatus");
                }
            }
        }

        public ICollection<string> ContactMethods
        {
            get
            {
                if (this.contactMethods == null)
                {
                    this.contactMethods = new ObservableCollection<string>() { "Email (E)", "Fax (F)", "Home Phone (H)", "Mobile Phone (M)", "Pager (P)", "Work Phone (W)"};
                }

                return this.contactMethods;
            }
        }

        public string ContactMethod
        {
            get { return this.contactMethod; }

            set
            {
                if (value != null && !value.Equals(this.contactMethod))
                {
                    this.contactMethod = value;
                    this.NotifyPropertyChanged("ContactMethod");
                }
            }
        }

        public string Email
        {
            get { return this.email; }

            set
            {
                if (value != null && !value.Equals(this.email))
                {
                    this.email = value;
                    this.NotifyPropertyChanged("Email");
                }
            }
        }

        public string MobilePhone
        {
            get { return this.mobilePhone; }

            set
            {
                if (value != null && !value.Equals(this.mobilePhone))
                {
                    this.mobilePhone = value;
                    this.NotifyPropertyChanged("MobilePhone");
                }
            }
        }

        public string HomePhone
        {
            get { return this.homePhone; }

            set
            {
                if (value != null && !value.Equals(this.homePhone))
                {
                    this.homePhone = value;
                    this.NotifyPropertyChanged("HomePhone");
                }
            }
        }

        public string WorkPhone
        {
            get { return this.workPhone; }

            set
            {
                if (value != null && !value.Equals(this.workPhone))
                {
                    this.workPhone = value;
                    this.NotifyPropertyChanged("WorkPhone");
                }
            }
        }

        public string Fax
        {
            get { return this.fax; }

            set
            {
                if (value != null && !value.Equals(this.fax))
                {
                    this.fax = value;
                    this.NotifyPropertyChanged("Fax");
                }
            }
        }

        public string Pager
        {
            get { return this.pager; }

            set
            {
                if (value != null && !value.Equals(this.pager))
                {
                    this.pager = value;
                    this.NotifyPropertyChanged("Pager");
                }
            }
        }

        public string AddressLine1
        {
            get { return this.address1; }

            set
            {
                if (value != null && !value.Equals(this.address1))
                {
                    this.address1 = value;
                    this.NotifyPropertyChanged("AddressLine1");
                }
            }
        }

        public string AddressLine2
        {
            get { return this.address2; }

            set
            {
                if (value != null && !value.Equals(this.address2))
                {
                    this.address2 = value;
                    this.NotifyPropertyChanged("AddressLine2");
                }
            }
        }

        public string City
        {
            get { return this.city; }

            set
            {
                if (value != null && !value.Equals(this.city))
                {
                    this.city = value;
                    this.NotifyPropertyChanged("City");
                }
            }
        }

        public string State
        {
            get { return this.state; }

            set
            {
                if (value != null && !value.Equals(this.state))
                {
                    this.state = value;
                    this.NotifyPropertyChanged("State");
                }
            }
        }

        public List<string> States
        {
            get
            {
                if (this.states == null)
                {
                    this.states = new List<string>() { "AA", "AE", "AK", "AL", "AP", "AR", "AS", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "FM", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MH", "MI", "MN", "MO", "MP", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "PW", "RI", "SC", "SD", "TN", "TX", "UM", "US", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY" };
                }

                return this.states;
            }
        }

        public string ZipCode
        {
            get { return this.zip; }

            set
            {
                if (value != null && !value.Equals(this.zip))
                {
                    this.zip = value;
                    this.NotifyPropertyChanged("ZipCode");
                }
            }
        }

        public string Province
        {
            get { return this.province; }

            set
            {
                if (value != null && !value.Equals(this.province))
                {
                    this.province = value;
                    this.NotifyPropertyChanged("Province");
                }
            }
        }

        public string Country
        {
            get { return this.country; }

            set
            {
                if (value != null && !value.Equals(this.country))
                {
                    this.country = value;
                    this.NotifyPropertyChanged("Country");
                }
            }
        }

        public List<string> Countries
        {
            get
            {
                if (this.countries == null)
                {
                    this.countries = new List<string>() { "United States", "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo (Brazzaville)", "Congo (Kinshasa)", "Cook Islands", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Côte d'Ivoire", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard and McDonald Islands", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea (North)", "Korea (South)", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macao", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Norway", "Oman", "Pakistan", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Qatar", "Romania", "Russian Federation", "Rwanda", "Réunion", "Saint Helena", "Saint Kitts and Nevis", "Saint Lucia", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia and Montenegro", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and South Sandwich Islands", "Spain", "Sri Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor-Leste", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Viet Nam", "Virgin Islands (British)", "Wallis and Futuna Islands", "Western Sahara", "Yemen", "Zambia", "Zimbabwe" };
                }

                return this.countries;
            }
        }

        public bool IsVAPatient
        {
            get { return this.isVAPatient; }

            set
            {
                if (!value.Equals(this.isVAPatient))
                {
                    this.isVAPatient = value;
                    this.NotifyPropertyChanged("IsVAPatient");
                }
            }
        }

        public bool IsVeteran
        {
            get { return this.isVeteran; }

            set
            {
                if (!value.Equals(this.isVeteran))
                {
                    this.isVeteran = value;
                    this.NotifyPropertyChanged("IsVeteran");
                }
            }
        }

        public bool IsVeteranAdvocate
        {
            get { return this.isVeteranAdvocate; }

            set
            {
                if (!value.Equals(this.isVeteranAdvocate))
                {
                    this.isVeteranAdvocate = value;
                    this.NotifyPropertyChanged("IsVeteranAdvocate");
                }
            }
        }

        public bool IsVAEmployee
        {
            get { return this.isVAEmployee; }

            set
            {
                if (!value.Equals(this.isVAEmployee))
                {
                    this.isVAEmployee = value;
                    this.NotifyPropertyChanged("IsVAEmployee");
                }
            }
        }

        public bool IsHealthProvider
        {
            get { return this.isHealthProvider; }

            set
            {
                if (!value.Equals(this.isHealthProvider))
                {
                    this.isHealthProvider = value;
                    this.NotifyPropertyChanged("IsHealthProvider");
                }
            }
        }

        public bool IsOther
        {
            get { return this.isOther; }

            set
            {
                if (!value.Equals(this.isOther))
                {
                    this.isOther = value;
                    this.NotifyPropertyChanged("IsOther");
                }
            }
        }

        public string BloodType
        {
            get { return this.bloodType; }

            set
            {
                if (value != null && !value.Equals(this.bloodType))
                {
                    this.bloodType = value;
                    this.NotifyPropertyChanged("BloodType");
                }
            }
        }

        public List<string> BloodTypes
        {
            get
            {
                if (this.bloodTypes == null)
                {
                    this.bloodTypes = new List<string>() { "A+", "A-", "AB+", "AB-", "B+", "B-", "O+", "O-" };
                }

                return this.bloodTypes;
            }
        }

        public bool IsOrganDonor
        {
            get { return this.isOrganDonor; }

            set
            {
                if (!value.Equals(this.isOrganDonor))
                {
                    this.isOrganDonor = value;
                    this.NotifyPropertyChanged("IsOrganDonor");
                }
            }
        }

        public bool PrivacyAccepted
        {
            get { return this.privacyAccepted; }

            set
            {
                if (!value.Equals(this.privacyAccepted))
                {
                    this.privacyAccepted = value;
                    this.NotifyPropertyChanged("PrivacyAccepted");
                }
            }
        }

        public bool TermsAccepted
        {
            get { return this.termsAccepted; }

            set
            {
                if (!value.Equals(this.termsAccepted))
                {
                    this.termsAccepted = value;
                    this.NotifyPropertyChanged("TermsAccepted");
                }
            }
        }

        public string Question1 { get; set; }

        public string Question2 { get; set; }

        public string Answer1 { get; set; }

        public string Answer2 { get; set; }

        public string Gender { get; set; }

        public string BirthMonth { get; set; }

        public string BirthDate { get; set; }

        public string BirthYear { get; set; }

        public ICommand SaveCommand
        {
            get
            {
                if (this.saveCommand == null)
                {
                    this.saveCommand = new DelegateCommand(o => this.Save());
                }

                return this.saveCommand;
            }
        }

        public string Token { get; set; }

        private void PopulateFromWeb()
        {
            VACommunicator.PopulateProfile(this);
        }

        private void Save()
        {
            VACommunicator.SaveProfile(this);
        }
    }
}
