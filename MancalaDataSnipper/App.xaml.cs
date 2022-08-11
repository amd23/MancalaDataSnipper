using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MancalaDataSnipper.Views;
using System.Threading;

namespace MancalaDataSnipper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private static readonly Mutex mutex = new Mutex(true);
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(object), typeof(MainWindow), nameof(MainWindow));
            containerRegistry.Register(typeof(object), typeof(BoardView), nameof(BoardView));
            containerRegistry.Register(typeof(object), typeof(StartUpView), nameof(StartUpView));
        }
    }
}
