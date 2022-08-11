﻿using Prism.Regions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using MancalaDataSnipper.Views;
using MancalaDataSnipper.Models;
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
        public DelegateCommand HelpCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        #endregion


        #region Constructors
        public  MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            ViewCommand = new DelegateCommand<string>(ViewCommandHandler);
            HelpCommand = new DelegateCommand(HelpCommandHandler);
            ExitCommand = new DelegateCommand(ExitCommandHandler);
            regionManager.RegisterViewWithRegion("MainContentRegion", typeof(StartUpView));        
        }
        #endregion

        #region CommandHandlers
        /// <summary>
        /// Command handler navigates to the region based on gametype.
        /// Gametype can be single or double.
        /// </summary>
        /// <param name="gameType"></param>
        private void ViewCommandHandler(string gameType)
        {
            string uri = "BoardView";
            var navigationParameters = new NavigationParameters();
            if (gameType == "SinglePlayer")
            {
                navigationParameters.Add("GameType", GameType.SinglePlayer);
            }
            else
            {
                navigationParameters.Add("GameType", GameType.DoublePlayer);
            }

            regionManager.RequestNavigate("MainContentRegion", uri, navigationParameters);
        }

        /// <summary>
        /// Command handler to open Mancala Wiki page.
        /// The user can click on it to get help.
        /// </summary>
        private void HelpCommandHandler()
        {
            var helpPageProcess = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://en.wikipedia.org/wiki/Mancala",
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(helpPageProcess);

        }
        /// <summary>
        /// Command handler to exit the application
        /// </summary>
        private void ExitCommandHandler()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
