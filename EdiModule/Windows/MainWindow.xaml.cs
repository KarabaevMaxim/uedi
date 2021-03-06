﻿namespace EdiModule.Windows
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
            this.bindings.Add("ГЛН поставщика", "Supplier.ExCounteragent.GLN");
            this.bindings.Add("Поставщик", "Supplier.InnerCounteragent.Name");
            this.bindings.Add("ГЛН организации", "Organization.GLN");
            this.bindings.Add("Организация", "Organization.Name");
            this.bindings.Add("ГЛН склада", "Warehouse.ExWarehouse.GLN");
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
            TotalLbl.Text = "Всего накладных: " + CoreInit.ModuleRepository.GetAllUnprocessedWaybills().Count;
            YoursLbl.Text = "Ваших накладных: " + CoreInit.ModuleRepository.GetUserWaybills(CoreInit.RepositoryService.GetCurrentUser());
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
			DocumentManager.DownloadUnprocessedWaybills(this.CurrentSession.WorkFolder);
			this.logger.Trace("Конец MainWindow.DownloadDocuments");
		}

		public void UpdateTablePart()
        {
			this.logger.Trace("MainWindow.UpdateTablePart");
            this.UpdateTablePart(UnprocessedWaybillTbl, CoreInit.ModuleRepository.GetUserWaybills(CoreInit.RepositoryService.GetCurrentUser()));
            this.UpdateTablePart(AllWaybillTbl, CoreInit.ModuleRepository.GetAllUnprocessedWaybills());
            this.UpdateTablePart(TotalWaybillTbl, CoreInit.ModuleRepository.GetTotalWaybillList());


            this.logger.Trace("Конец MainWindow.UpdateTablePart");
		}

        private void UpdateTablePart(DataGrid table, IEnumerable<Waybill> source)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (source == null)
                throw new ArgumentNullException("source");

            table.Columns.Clear();

            foreach (var item in this.bindings)
                table.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            table.Items.Clear();

            foreach (var item in source)
                table.Items.Add(item);
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

        private void ShowProcessedWbBtn_Click(object sender, RoutedEventArgs e)
        {
            ProcessedWaybillWindow window = new ProcessedWaybillWindow();
            window.Show();
        }

        private Dictionary<string, string> bindings = new Dictionary<string, string>();
        private Session CurrentSession { get; set; }
		private readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
