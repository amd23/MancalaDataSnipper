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

namespace MancalaDataSnipper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
           
            return w;
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{

        //    base.OnStartup(e);
        //    MainWindow main = new MainWindow();
        //}

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(object), typeof(MainWindow), nameof(MainWindow));
            containerRegistry.Register(typeof(object), typeof(BoardView), nameof(BoardView));

            //throw new NotImplementedException();
        }
    }
}
