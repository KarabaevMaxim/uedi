
namespace EdiModule.Windows
{
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;
	using EdiModuleCore;
    using EdiModuleCore.Model;

    /// <summary>
    /// Логика взаимодействия для WareMatchingListWindow.xaml
    /// </summary>
    public partial class WarehouseMatchingListWindow : Window, ITableWindow
    {
        public WarehouseMatchingListWindow()
        {
            InitializeComponent();

            this.bindings = new Dictionary<string, string>
            {
                { "Внут. код", "Code" },
                { "Название", "Name" },
                { "ГЛН", "GLN" }
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

            this.WarehousesTbl.ItemsSource = CoreInit.ModuleRepository.GetWarehouses();
        }

        /// <summary>
        /// Автонумерация строк таблицы.
        /// </summary>
        private void WarehousesTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        /// <summary>
        /// Нажание на кнопку "Создать для всех строк".
        /// </summary>
        private void AddAllWarehousesBtn_Click(object sender, RoutedEventArgs e)
        {
            //foreach (var item in this.WarehousesTbl.Items)
            //{
            //    if ((item is MatchedWare ware) && (ware.ExWare != null) && (ware.InnerWare == null))
            //        MatchingModule.CreateNewInnerWareAndMatch(ware);
            //}

            this.UpdateTablePart();
        }

        private void AddSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            //foreach (var item in this.WarehousesTbl.SelectedItems)
            //{
            //    if ((item is MatchedWare ware) && (ware.ExWare != null) && (ware.InnerWare == null))
            //        MatchingModule.CreateNewInnerWareAndMatch(ware);
            //}

            this.UpdateTablePart();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is Bridge1C.DomainEntities.Warehouse warehouse)
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

        private Dictionary<string, string> bindings;
    }
}
