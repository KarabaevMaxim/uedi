namespace EdiModule.Windows
{
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;
	using EdiModuleCore;
    using EdiModuleCore.Model;
	using NLog;
	using Newtonsoft.Json;

    /// <summary>
    /// Логика взаимодействия для WareMatchingListWindow.xaml
    /// </summary>
    public partial class WarehouseMatchingListWindow : Window, ITableWindow, IDependentWindow
    {
        public WarehouseMatchingListWindow()
        {
            InitializeComponent();

            this.bindings = new Dictionary<string, string>
            {
                { "Внут. код", "InnerWarehouse.Code" },
                { "Название", "InnerWarehouse.Name" },
                { "ГЛН", "ExWarehouse.GLN" }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateTablePart();
        }

        public void UpdateTablePart()
        {
            this.WarehousesTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.WarehousesTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

			var wareHouses = CoreInit.ModuleRepository.GetWarehouses();
			List<MatchedWarehouse> result = new List<MatchedWarehouse>();

			foreach (var item in wareHouses)
			{
				MatchedWarehouse mw = MatchingModule.AutomaticWHMatching(item.ExWarehouse);

				if (mw.InnerWarehouse == null)
					this.logger.Warn("Автоматическое сопоставление складов не выполнено. Результат: {0}", JsonConvert.SerializeObject(mw));
				else
					this.logger.Info("Автоматическое сопоставление складов выполнено. Результат: {0}", JsonConvert.SerializeObject(mw));

				result.Add(mw);
			}
			

            this.WarehousesTbl.ItemsSource = CoreInit.ModuleRepository.GetWarehouses();
        }

        /// <summary>
        /// Автонумерация строк таблицы.
        /// </summary>
        private void WarehousesTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is MatchedWarehouse warehouse)
                {
                   WarehouseReferenceWindow prodWindow = new WarehouseReferenceWindow
				   {
						CurrentWarehouse = warehouse,
                        ParentWindow = this
                    };
                    prodWindow.ShowDialog();
                }
            }
        }

		/// <summary>
		/// При закрытии окна.
		/// </summary>
		private void Window_Closed(object sender, System.EventArgs e)
		{
			this.ParentWindow.UpdateTablePart();
		}

		private Dictionary<string, string> bindings;
		private readonly Logger logger = LogManager.GetCurrentClassLogger();
		public ITableWindow ParentWindow { get; set; }


	}
}
