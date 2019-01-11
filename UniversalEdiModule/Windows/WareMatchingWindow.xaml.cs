

namespace UniversalEdiModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Core.ViewModels;

    /// <summary>
    /// Логика взаимодействия для WareMatchingWindow.xaml
    /// </summary>
    public partial class WareMatchingWindow : Window
    {
        public WareMatchingWindow()
        {
            InitializeComponent();

            this.bindings = new Dictionary<string, string>
            {
                { "Название поставщика", "WareName" },
                { "Код поставщика", "WareSupplierCode" },
                { "ЕИ", "Unit" },
                { "Количество", "Quantity" },
                { "Сумма", "Amount" },
                { "Внут. код", "InnerCode" },
                { "Внут. название", "InnerName" },
                { "ШК", "Barcode" },
                { "Внут. ЕИ", "Barcode" }
            };
        }

        private void UpdatePositionsTbl()
        {


            this.PositionsTbl.ItemsSource = this.Positions;
        }

        public List<WarePositionViewModel> Positions { get; set; }
        private Dictionary<string, string> bindings;
    }
}
