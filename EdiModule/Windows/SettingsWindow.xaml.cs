

namespace EdiModule.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using EdiModuleCore;
    using System.IO;

    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DestinationWaybillFolderPathTxt.Text = SettingsManager.Settings.DestinationWaybillFolder;
            this.StartWaybillFolderPathTxt.Text = SettingsManager.Settings.StartWaybillFolder;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings newSettings = new Settings
            {
                DestinationWaybillFolder = this.DestinationWaybillFolderPathTxt.Text,
                StartWaybillFolder = this.StartWaybillFolderPathTxt.Text
            };

            SettingsManager.SaveSettings(newSettings);

            this.Close();
        }
    }
}
