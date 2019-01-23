namespace EdiModule.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using EdiModuleCore;
    using EdiModuleCore.Model;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITableWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.bindings.Add("Номер накладной", "Number");
            this.bindings.Add("Дата накладной", "Date");
            this.bindings.Add("Поставщик", "Supplier.Name");
            this.bindings.Add("Склад", "Warehouse.Name");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateTablePart();
        }

        private void UpdateUnprocessedWaybillTbl(List<Waybill> waybills)
        {
            this.UnprocessedWaybillTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.UnprocessedWaybillTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            this.UnprocessedWaybillTbl.Items.Clear();

            foreach (var item in waybills)
                this.UnprocessedWaybillTbl.Items.Add(item);
        }

        public void UpdateTablePart()
        {
			Session session = SessionManager.Sessions[0];
			FtpService.DownloadDocuments(session.FtpURI, session.FtpPassive, session.FtpTimeout, session.FtpLogin, session.FtpPassword, session.FtpRemoteFolder, session.WorkFolder);
            DocumentManager.DownloadWaybills(session.WorkFolder);
            this.UpdateUnprocessedWaybillTbl(CoreInit.ModuleRepository.GetUnprocessedWaybills());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateTablePart();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
            
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is Waybill waybill)
                {
                    WareMatchingWindow wareWindow = new WareMatchingWindow();
                    wareWindow.ParentWindow = this;
                    wareWindow.Waybill = waybill;
                    wareWindow.ShowDialog();
                }
            }
        }

        private void ShowMatchingWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            WareMatchingListWindow window = new WareMatchingListWindow();
            window.ShowDialog();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Автонумерация строк таблицы.
        /// </summary>
        private void UnprocessedWaybillTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void UpdateWareRefenceBtn_Click(object sender, RoutedEventArgs e)
        {
            CoreInit.ModuleRepository.InitProductReference();
        }

        private Dictionary<string, string> bindings = new Dictionary<string, string>();

    }
}
