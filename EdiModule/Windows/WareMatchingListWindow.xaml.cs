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
	public partial class WareMatchingListWindow : Window, ITableWindow
    {
        public WareMatchingListWindow()
        {
            InitializeComponent();

            this.bindings = new Dictionary<string, string>
            {
                { "Название поставщика", "ExWare.Name" },
                { "Код поставщика", "ExWare.Code" },
                { "ШК", "ExWare.Barcode" },
                { "Внут. код", "InnerWare.Code" },
                { "Внут. название", "InnerWare.Name" }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateTablePart();
        }

        public void UpdateTablePart()
        {
            this.WaresTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.WaresTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            this.WaresTbl.ItemsSource = CoreInit.ModuleRepository.GetMatchedWares();
        }

        /// <summary>
        /// Автонумерация строк таблицы.
        /// </summary>
        private void WaresTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        /// <summary>
        /// Нажание на кнопку "Создать для всех строк".
        /// </summary>
        private void AddAllWaresBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.WaresTbl.Items)
            {
                if ((item is MatchedWare ware) && (ware.ExWare != null) && (ware.InnerWare == null))
                    MatchingModule.CreateNewInnerWareAndMatch(ware);
            }

            this.UpdateTablePart();
        }

        private void AddSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.WaresTbl.SelectedItems)
            {
                if ((item is MatchedWare ware) && (ware.ExWare != null) && (ware.InnerWare == null))
                    MatchingModule.CreateNewInnerWareAndMatch(ware);
            }

            this.UpdateTablePart();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is MatchedWare ware)
                {
					if(ware.ExWare.Supplier?.InnerCounteragent != null)
					{
						ProductReferenceWindow prodWindow = new ProductReferenceWindow
						{
							Ware = ware,
							ParentWindow = this
						};
						prodWindow.ShowDialog();
					}
					else
					{
						MessageBox.Show("Невозможно выполнить операцию, поставщик не указан.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					}
                }
            }
        }

        private Dictionary<string, string> bindings;
    }
}
