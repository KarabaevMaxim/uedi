namespace EdiModule.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using Bridge1C.DomainEntities;
    using EdiModuleCore;
    using EdiModuleCore.Exceptions;
    using EdiModuleCore.Model;

    /// <summary>
    /// Логика взаимодействия для ProductReference.xaml
    /// </summary>
    public partial class WarehouseReferenceWindow : Window
    {
        public WarehouseReferenceWindow()
        {
            InitializeComponent();
            bindings = new Dictionary<string, string>
            {
				{ "Код", "Code" },
				{ "Название", "Name" },
				{ "ГЛН", "GLN" }
			};

           // this.Warehouses = CoreInit.ModuleRepository.GetWarehouses();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ParentWindow == null || this.CurrentWarehouse == null)
                throw new NotInitializedException("Один из параметров формы выбора номенклатуры для сопоставления не инициализирован.");

            this.WarehousesTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.WarehousesTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            this.WarehousesTbl.Items.Clear();
            this.WarehousesTbl.ItemsSource = CoreInit.ModuleRepository.WarehouseReference;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is Warehouse warehouse)
                {
                    try
                    {
						this.CurrentWarehouse = MatchingModule.ManualWHMatching(warehouse, this.CurrentWarehouse);
                    }
                    catch(NotMatchedException ex)
                    {
                        MessageBox.Show(ex.Message, "Не удалось сопоставить склад.");
                    }
                }
            }
			
            this.ParentWindow?.UpdateTablePart();
            this.Close();
        }

        public ITableWindow ParentWindow { get; set; }
		public Warehouse CurrentWarehouse { get; set; }
        private Dictionary<string, string> bindings;
    }
}
