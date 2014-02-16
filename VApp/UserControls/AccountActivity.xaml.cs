using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VApp.UserControls
{
    public sealed partial class AccountActivity : UserControl
    {
        public AccountActivity()
        {
            this.InitializeComponent();

            Activity.Items.Add("2/16/2014 4:32 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 4:04 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 3:30 AM CST	Self	Allergy         View	Successful");
            Activity.Items.Add("2/16/2014 3:29 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 3:27 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:52 AM CST	Self	WellnessReminders	View	Successful");
            Activity.Items.Add("2/16/2014 2:52 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:50 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:49 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:42 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:39 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:38 AM CST	Self	Login/Logout	Login	Successful");
            Activity.Items.Add("2/16/2014 2:36 AM CST	Self	Login/Logout	Login	Successful");
        }
    }
}
