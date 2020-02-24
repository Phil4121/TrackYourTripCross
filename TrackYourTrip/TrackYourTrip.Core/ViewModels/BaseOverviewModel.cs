using Acr.UserDialogs;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.ViewModels
{
    public abstract class BaseOverviewModel<TNavParameter, TViewModel> : BaseViewModel<TViewModel>, IMvxViewModel<TNavParameter, TViewModel>
        where TViewModel : class
        where TNavParameter : OverviewArgs
    {
        public BaseOverviewModel(string pageTitle, IMvxLogProvider mvxLogProvider, IMvxNavigationService navigationService)
            : base(pageTitle, mvxLogProvider, navigationService){
        
        
        }

        bool _navigateBack = false;
        public bool NavigateBack
        {
            get => _navigateBack;
            set => SetProperty(ref _navigateBack, value);
        }

        public abstract override IDataServiceFactory<TViewModel> DataStore { get; set; }

        public override bool IsNew => throw new NotImplementedException();

        public TaskCompletionSource<object> CloseCompletionSource { get;set; }

        public virtual void Prepare(TNavParameter parameter)
        {
            this.NavigateBack = parameter.NavigateBack;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BaseOverviewModel<TNavParameter, TViewModel, TRoutingModel> : BaseOverviewModel<TNavParameter, TViewModel>, IMvxViewModel<TNavParameter, TRoutingModel>
    where TViewModel : class
    where TNavParameter : OverviewArgs
    where TRoutingModel : class
    {
        public BaseOverviewModel(string pageTitle, IMvxLogProvider mvxLogProvider, IMvxNavigationService navigationService)
            : base(pageTitle, mvxLogProvider, navigationService)
        {


        }

    }

        public class OverviewArgs
    {
        public bool NavigateBack { get; set; }

        public object Object { get; set; }

        public OverviewArgs(bool navigateBack)
        {
            this.NavigateBack = navigateBack;
        }

        public OverviewArgs(bool navigateBack, object obj)
        {
            this.NavigateBack = navigateBack;
            this.Object = obj;

        }
    }
}
