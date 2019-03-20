namespace EdiModule.Windows
{
    using System;
	using System.Linq;
	using System.Threading.Tasks;
    using System.Collections.Generic;
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
			//WaitAnim.Visibility = Visibility.Visible;
			this.bindings.Add("Номер накладной", "Number");
            this.bindings.Add("Дата накладной", "Date");
            this.bindings.Add("Поставщик", "Supplier.InnerCounteragent.Name");
			this.bindings.Add("Организация", "Organization.Name");
			this.bindings.Add("Склад", "Warehouse.InnerWarehouse.Name");
			this.bindings.Add("Сумма с НДС", "AmountWithTax");
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			this.Initialize();
		}

		private void Initialize()
		{
			this.GetSession();
			this.DownloadDocuments();
			this.UpdateTablePart();
			UserNameTxt.Text = CoreInit.RepositoryService.GetCurrentUser().Name;
			TotalLbl.Text = "Всего накладных: " + CoreInit.ModuleRepository.GetUnprocessedWaybills().Count;
		}

		private void GetSession()
		{
			 this.CurrentSession = SessionManager.Sessions[0];
		}

		private void DownloadDocuments()
		{
			FtpService.DownloadDocumentsNative(this.CurrentSession.FtpURI,
			this.CurrentSession.FtpPassive,
			this.CurrentSession.FtpTimeout,
			this.CurrentSession.FtpLogin,
			this.CurrentSession.FtpPassword,
			this.CurrentSession.FtpRemoteFolder,
			this.CurrentSession.WorkFolder);
			DocumentManager.ReloadWaybills(this.CurrentSession.WorkFolder);
		}

		public void UpdateTablePart()
        {
			this.UnprocessedWaybillTbl.Columns.Clear();

			foreach (var item in this.bindings)
				this.UnprocessedWaybillTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

			this.UnprocessedWaybillTbl.Items.Clear();

			var waybills = CoreInit.ModuleRepository.GetUnprocessedWaybills().Where(wb => CoreInit.RepositoryService.GetWarehousesByActiveUser().Contains(wb.Warehouse.InnerWarehouse));

			foreach (var item in waybills)
				this.UnprocessedWaybillTbl.Items.Add(item);
		}

		/// <summary>
		/// Загрузить накладные.
		/// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
			try
            {
				this.Initialize();
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

		private void ShowMatchingWarehouseBtn_Click(object sender, RoutedEventArgs e)
		{
			WarehouseMatchingListWindow window = new WarehouseMatchingListWindow();
			window.ParentWindow = this;
			window.ShowDialog();
		}

		private void ShowMatchingSupplierBtn_Click(object sender, RoutedEventArgs e)
		{
			SupplierMatchingListWindow window = new SupplierMatchingListWindow();
			window.ParentWindow = this;
			window.ShowDialog();
		}

		private void ShowWhListBtn_Click(object sender, RoutedEventArgs e)
		{
			var whNames = CoreInit.ModuleRepository.GetWarehouses().Where(wh => wh.InnerWarehouse?.User?.Code == CoreInit.RepositoryService.GetCurrentUser().Code).Select(wh => string.Format("{0} {1}", wh.InnerWarehouse.Name, wh.ExWarehouse.GLN));
			string result = whNames.Any() ? string.Join("\n", whNames) : "Список складов пуст, возможно у вас есть несопоставленные склады.";

			MessageBox.Show(result, "Ваши склады", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private Dictionary<string, string> bindings = new Dictionary<string, string>();
		private Session CurrentSession { get; set; }
	}
}
