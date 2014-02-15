using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VApp.Common;
using VApp.VAWebsite;

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

        public void PopulateFromWeb()
        {
            VACommunicator.PopulateProfile(this);
        }
    }
}
