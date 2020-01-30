﻿using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TrackYourTrip.Core.ViewModels.Settings;

namespace TrackYourTrip.Core.Pages.Settings
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class FishingAreasPage : MvxContentPage<FishingAreasViewModel>
    {
        public FishingAreasPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (FishingAreasListView.SelectedItem != null)
            {
                FishingAreasListView.SelectedItem = null;
            }
        }
    }
}