using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UniversalEdiModule
{
    using Core;
    using Bridge1C;
    using System.Runtime.InteropServices;

    
    public partial class App : Application
    {
        public App()
        {
            SettingsManager.LoadSettings();
            ConnectionManager = new ConnectionManager();
        }

        public void Start()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }

        public static ConnectionManager ConnectionManager { get; private set; }
    }
}
