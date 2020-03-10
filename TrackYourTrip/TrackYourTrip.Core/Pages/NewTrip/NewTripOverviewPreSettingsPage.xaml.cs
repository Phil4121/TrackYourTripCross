﻿using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackYourTrip.Core.Pages.NewTrip
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(TabbedPosition.Tab)]
    public partial class NewTripOverviewPreSettingsPage : MvxContentPage
    {
        public NewTripOverviewPreSettingsPage()
        {
            InitializeComponent();
        }
    }
}