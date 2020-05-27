using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Interfaces;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Behaviors
{
    public class SharedFishedSpotTabBehavior : Behavior<MvxTabbedPage>
    {
        public MvxTabbedPage AssociatedObject { get; private set; }
        private ISharedFishedSpotViewModel _currentViewModel;

        protected override void OnAttachedTo(MvxTabbedPage bindable)
        {
            bindable.CurrentPageChanged += Bindable_CurrentPageChanged;
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
        }
        protected override void OnDetachingFrom(MvxTabbedPage bindable)
        {
            bindable.CurrentPageChanged -= Bindable_CurrentPageChanged;
            base.OnDetachingFrom(bindable);
            AssociatedObject = null;
        }

        private void Bindable_CurrentPageChanged(object sender, EventArgs e)
        {
            if (!(AssociatedObject?.CurrentPage is IMvxPage p))
                return;

            if (!(p.ViewModel is ISharedFishedSpotViewModel newViewModel))
                return;

            if (_currentViewModel != null)
            {
                newViewModel.FishedSpot = _currentViewModel.FishedSpot;
            }

            _currentViewModel = newViewModel;
        }
    }
}
