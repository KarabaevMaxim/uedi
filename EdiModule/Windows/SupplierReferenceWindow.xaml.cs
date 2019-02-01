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
    public partial class SupplierReferenceWindow : Window, IDependentWindow
    {
        public SupplierReferenceWindow()
        {
            InitializeComponent();
            bindings = new Dictionary<string, string>
            {
				{ "Код", "Code" },
				{ "Наименование", "Name" },
				{ "Полн. наименование", "FullName" }
			};
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateTablePart();
        }

        private void UpdateTablePart()
        {
            if (this.ParentWindow == null || this.CurrentCounteragent == null)
                throw new NotInitializedException("Один из параметров формы выбора номенклатуры для сопоставления не инициализирован.");

            this.WarehousesTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.WarehousesTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            this.WarehousesTbl.ItemsSource = CoreInit.ModuleRepository.CounteragentReference;
        }

        private async void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is Counteragent innerCountergent)
                {
                    try
                    {
						if(!(await MatchingModule.ManualSupMatchingAsync(this.CurrentCounteragent, innerCountergent)))
							MessageBox.Show("При сопоставлении произошла ошибка.", "Не удалось сопоставить поставщика.");
                    }
                    catch(NotMatchedException ex)
                    {
                        MessageBox.Show(ex.Message, "Не удалось сопоставить поставщика.");
                    }
                }
            }
			
            this.ParentWindow?.UpdateTablePart();
            this.UpdateTablePart();
            this.Close();
        }

        public ITableWindow ParentWindow { get; set; }
		public MatchedCounteragent CurrentCounteragent { get; set; }
        private Dictionary<string, string> bindings;
    }
}
