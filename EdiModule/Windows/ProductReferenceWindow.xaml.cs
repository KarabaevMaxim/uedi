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
    public partial class ProductReferenceWindow : Window
    {
        public ProductReferenceWindow()
        {
            InitializeComponent();
            bindings = new Dictionary<string, string>
            {
                { "Код", "Code" },
                { "Наименование", "Name" },
                { "Полное Наименование", "FullName" }
            };

            this.Wares = CoreInit.ModuleRepository.ProductReference;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ParentWindow == null || this.Ware == null)
                throw new NotInitializedException("Один из параметров формы выбора номенклатуры для сопоставления не инициализирован.");

            this.WaresTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.WaresTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            this.WaresTbl.Items.Clear();
            this.WaresTbl.ItemsSource = this.Wares;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is Ware wareItem)
                {
                    try
                    {
                        MatchingModule.ManualMatching(wareItem, this.Ware);
                    }
                    catch(NotMatchedException ex)
                    {
                        MessageBox.Show(ex.Message, "Не удалось сопоставить номенклатуру.");
                    }
                }
            }
            if(this.ParentWindow != null)         
                this.ParentWindow.UpdateTablePart();

            this.Close();
        }

        public ITableWindow ParentWindow { get; set; }
        public MatchedWare Ware { get; set; }
        private List<Ware> Wares { get; set; }
        private Dictionary<string, string> bindings;


    }
}
