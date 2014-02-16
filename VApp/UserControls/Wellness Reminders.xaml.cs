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
    public sealed partial class Wellness_Reminders : UserControl
    {
        public Wellness_Reminders()
        {
            this.InitializeComponent();

            Reminders.Items.Add("DUE NOW	Pneumonia Vaccine		JAMES E VAN ZANDT VAMC");
            Reminders.Items.Add("DUE NOW	Influenza Vaccine		BUTLER VETERANS AFFAIRS MEDICAL CENTER");
            Reminders.Items.Add("DUE NOW	Influenza Vaccine		JAMES E VAN ZANDT VAMC");
            Reminders.Items.Add("DUE NOW	Influenza Vaccine		PITTSBURGH HEALTH CARE SYSTEM - UNIVERSITY DRIVE DIVISION");
            Reminders.Items.Add("DUE NOW	Body Mass Index more than 25		JAMES E VAN ZANDT VAMC");
            Reminders.Items.Add(" DUE NOW	Body Mass Index more than 25		PITTSBURGH HEALTH CARE SYSTEM - UNIVERSITY DRIVE DIVISION");
        }
    }
}
