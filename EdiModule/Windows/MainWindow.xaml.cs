﻿namespace EdiModule.Windows
{
    using System;
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
            this.bindings.Add("Номер накладной", "Number");
            this.bindings.Add("Дата накладной", "Date");
            this.bindings.Add("Поставщик", "Supplier.InnerCounteragent.Name");
			this.bindings.Add("Организация", "Organization.Name");
			this.bindings.Add("Склад", "Warehouse.InnerWarehouse.Name");
			this.bindings.Add("Сумма с НДС", "AmountWithTax");
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			this.GetSession();
			this.DownloadDocuments();
			this.UpdateTablePart();
			UserNameTxt.Text = CurrentSession.UserName;
		}

		private void GetSession()
		{
			 this.CurrentSession = SessionManager.Sessions[0];
		}

		private bool DownloadDocuments()
		{
			var result = FtpService.DownloadDocuments(this.CurrentSession.FtpURI,
			this.CurrentSession.FtpPassive,
			this.CurrentSession.FtpTimeout,
			this.CurrentSession.FtpLogin,
			this.CurrentSession.FtpPassword,
			this.CurrentSession.FtpRemoteFolder,
			this.CurrentSession.WorkFolder);

			if (result)
				return DocumentManager.DownloadWaybills(this.CurrentSession.WorkFolder);
			else
				return false;
		}

		private async Task<bool> DownloadDocumentsAsync()
		{
			var result = await FtpService.DownloadDocumentsAsync(this.CurrentSession.FtpURI,
				this.CurrentSession.FtpPassive,
				this.CurrentSession.FtpTimeout,
				this.CurrentSession.FtpLogin,
				this.CurrentSession.FtpPassword,
				this.CurrentSession.FtpRemoteFolder,
				this.CurrentSession.WorkFolder);

			if (result)
				return await DocumentManager.DownloadWaybillsAsync(this.CurrentSession.WorkFolder);
			else
				return false;
		}

		public void UpdateTablePart()
        {
			this.UnprocessedWaybillTbl.Columns.Clear();

			foreach (var item in this.bindings)
				this.UnprocessedWaybillTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

			this.UnprocessedWaybillTbl.Items.Clear();

			foreach (var item in CoreInit.ModuleRepository.GetUnprocessedWaybills())
				this.UnprocessedWaybillTbl.Items.Add(item);
		}

		/// <summary>
		/// Загрузить накладные.
		/// </summary>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				await this.DownloadDocumentsAsync();
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

		private Dictionary<string, string> bindings = new Dictionary<string, string>();
		private Session CurrentSession { get; set; }
	}
}
