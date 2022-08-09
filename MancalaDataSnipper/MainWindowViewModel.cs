using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using MancalaDataSnipper.Views;
using System.Windows;

namespace MancalaDataSnipper
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Members
        private readonly IRegionManager regionManager;
        private IEventAggregator eventAggregator;


        #endregion


        #region Commands

        public DelegateCommand<string> ViewCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        #endregion


        #region Constructors
        public  MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            //regionManager.RegisterViewWithRegion("MainContentRegion")

            ViewCommand = new DelegateCommand<string>(ViewCommandHandler);
            ExitCommand = new DelegateCommand(ExitCommandHandler);
          

        }
        #endregion

        #region CommandHandlers
        
        private void ViewCommandHandler(string uri)
        {
       
            regionManager.RequestNavigate("MainContentRegion", uri);
        }

        private void ExitCommandHandler()
        {
            Application.Current.Shutdown();
        }

        #endregion


    }
}
