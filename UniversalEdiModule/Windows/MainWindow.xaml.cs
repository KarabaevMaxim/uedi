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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UniversalEdiModule
{
    using Core.DomainEntities;
    using Core;
    using Core.ViewModels;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Bindings.Add("Код", "ID");
            this.Bindings.Add("Номер накладной", "Number");
            this.Bindings.Add("Дата накладной", "Date");
            this.Bindings.Add("Дата загрузки", "DownloadDate");
            this.Bindings.Add("Поставщик", "Supplier");
            this.Bindings.Add("Склад", "DeliveryPlace");
        }

        private void UpdateUnprocessedWaybillTbl(List<WaybillViewModel> waybills)
        {
            this.UnprocessedWaybillTbl.Columns.Clear();
            foreach (var item in this.Bindings)
            {
                this.UnprocessedWaybillTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });
            }

            this.UnprocessedWaybillTbl.Items.Clear();

            foreach (var item in waybills)
            {
                this.UnprocessedWaybillTbl.Items.Add(item);
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateWaybills();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateWaybills();
        }

        private void UpdateWaybills()
        {
            DocumentManager.DownloadWaybills(DateTime.Now);
            WaybillViewModels = ViewModelMapper.GetWaybillViewModels(DocumentManager.UnprocessedWaybills);
            UpdateUnprocessedWaybillTbl(WaybillViewModels);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is WaybillViewModel waybill)
                {
                    WareMatchingWindow wareWindow = new WareMatchingWindow();
                    wareWindow.Positions = waybill.Positions;
                    wareWindow.ShowDialog();
                }
            }
        }

        private List<WaybillViewModel> WaybillViewModels { get; set; }
        private Dictionary<string, string> Bindings = new Dictionary<string, string>();
    }
}
