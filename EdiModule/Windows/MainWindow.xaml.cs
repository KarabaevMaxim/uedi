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
	using NLog;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITableWindow
    {
        public MainWindow()
        {
			this.logger.Trace("Конструктор MainWindow");
			InitializeComponent();
			this.bindings.Add("Номер накладной", "Number");
            this.bindings.Add("Дата накладной", "Date");
            this.bindings.Add("Поставщик", "Supplier.InnerCounteragent.Name");
			this.bindings.Add("Организация", "Organization.Name");
			this.bindings.Add("Склад", "Warehouse.InnerWarehouse.Name");
			this.bindings.Add("Сумма с НДС", "AmountWithTax");
			this.logger.Trace("Конец конструктора MainWindow");
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			this.logger.Trace("MainWindow.Window_Loaded");
			this.Initialize();
			this.logger.Trace("Конец MainWindow.Window_Loaded");
		}

		private void Initialize()
		{
			this.logger.Trace("MainWindow.Initialize");
			this.GetSession();
			this.DownloadDocuments();
			this.UpdateTablePart();
			UserNameTxt.Text = CoreInit.RepositoryService.GetCurrentUser().Name;
			TotalLbl.Text = "Всего накладных: " + CoreInit.ModuleRepository.GetUnprocessedWaybills().Count;
			this.logger.Trace("Конец MainWindow.Initialize");
		}

		private void GetSession()
		{
			this.logger.Trace("MainWindow.GetSession");
			this.CurrentSession = SessionManager.Sessions[0];
			this.logger.Trace("Конец MainWindow.GetSession");
		}

		private void DownloadDocuments()
		{
			this.logger.Trace("MainWindow.DownloadDocuments");
			FtpService.DownloadDocumentsNative(this.CurrentSession.FtpURI,
			this.CurrentSession.FtpPassive,
			this.CurrentSession.FtpTimeout,
			this.CurrentSession.FtpLogin,
			this.CurrentSession.FtpPassword,
			this.CurrentSession.FtpRemoteFolder,
			this.CurrentSession.WorkFolder);
			DocumentManager.ReloadWaybills(this.CurrentSession.WorkFolder);
			this.logger.Trace("Конец MainWindow.DownloadDocuments");
		}

		public void UpdateTablePart()
        {
			this.logger.Trace("MainWindow.UpdateTablePart");
			this.UnprocessedWaybillTbl.Columns.Clear();

			foreach (var item in this.bindings)
				this.UnprocessedWaybillTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

			this.UnprocessedWaybillTbl.Items.Clear();
			var warehouses = CoreInit.RepositoryService.GetWarehousesByActiveUser();

			if(warehouses == null)
			{
				MessageBox.Show("Произошла ошибка при загрузке складов. Подробности в файле логов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

            //	var waybills = CoreInit.ModuleRepository.GetUnprocessedWaybills().Where(wb => warehouses.Contains(wb.Warehouse.InnerWarehouse));

            var waybills = new List<Waybill>();

            foreach (var item in CoreInit.ModuleRepository.GetUnprocessedWaybills())
            {
                if (item.Warehouse.InnerWarehouse.Equals(warehouses[0]) || item.Warehouse.Equals(warehouses[1]))
                    waybills.Add(item);
            }

            foreach (var item in waybills)
            	this.UnprocessedWaybillTbl.Items.Add(item);

            this.AllWaybillTbl.Columns.Clear();
            foreach (var item in this.bindings)
                this.AllWaybillTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            waybills = CoreInit.ModuleRepository.GetGeneralWaybillList();

            this.AllWaybillTbl.ItemsSource = waybills;
            //foreach (var item in waybills)
            //    this.AllWaybillTbl.Items.Add(item);


            this.logger.Trace("Конец MainWindow.UpdateTablePart");
		}

		/// <summary>
		/// Загрузить накладные.
		/// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
			this.logger.Trace("MainWindow.Button_Click (загрузить накладные)");
			try
            {
				this.Initialize();
			}
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
			this.logger.Trace("Конец MainWindow.Button_Click (загрузить накладные)");
		}

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
			this.logger.Trace("MainWindow.DataGridRow_MouseDoubleClick");
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
			this.logger.Trace("Конец MainWindow.DataGridRow_MouseDoubleClick");
		}

        private void ShowMatchingWindowBtn_Click(object sender, RoutedEventArgs e)
        {
			this.logger.Trace("MainWindow.ShowMatchingWindowBtn_Click");
			WareMatchingListWindow window = new WareMatchingListWindow();
            window.ShowDialog();
			this.logger.Trace("Конец MainWindow.ShowMatchingWindowBtn_Click");
		}

        private void btn_Click(object sender, RoutedEventArgs e) // TODO: удалить 
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
			this.logger.Trace("MainWindow.UpdateWareRefenceBtn_Click");
			CoreInit.ModuleRepository.InitProductReference();
			this.logger.Trace("Конец MainWindow.UpdateWareRefenceBtn_Click");
		}

		private void ShowMatchingWarehouseBtn_Click(object sender, RoutedEventArgs e)
		{
			this.logger.Trace("MainWindow.ShowMatchingWarehouseBtn_Click");
			WarehouseMatchingListWindow window = new WarehouseMatchingListWindow();
			window.ParentWindow = this;
			window.ShowDialog();
			this.logger.Trace("Конец MainWindow.ShowMatchingWarehouseBtn_Click");
		}

		private void ShowMatchingSupplierBtn_Click(object sender, RoutedEventArgs e)
		{
			this.logger.Trace("MainWindow.ShowMatchingWarehouseBtn_Click");
			SupplierMatchingListWindow window = new SupplierMatchingListWindow();
			window.ParentWindow = this;
			window.ShowDialog();
			this.logger.Trace("Конец MainWindow.ShowMatchingSupplierBtn_Click");
		}

		private void ShowWhListBtn_Click(object sender, RoutedEventArgs e)
		{
			this.logger.Trace("MainWindow.ShowWhListBtn_Click");
            var whNames = CoreInit.RepositoryService.GetWarehousesByActiveUser().Select(wh => wh.Name);
            string result = whNames.Any() ? string.Join("\r\n", whNames) : "Список складов пуст, возможно у вас есть несопоставленные склады.";
			MessageBox.Show(result, "Ваши склады", MessageBoxButton.OK, MessageBoxImage.Information);
			this.logger.Trace("Конец MainWindow.ShowWhListBtn_Click");
		}

		private Dictionary<string, string> bindings = new Dictionary<string, string>();
		private Session CurrentSession { get; set; }
		private readonly Logger logger = LogManager.GetCurrentClassLogger();
	}
}
