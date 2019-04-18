namespace EdiModule.Windows
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
    using EdiModuleCore;
    using EdiModuleCore.Model;

    /// <summary>
    /// Логика взаимодействия для ProcessedWaybillWindow.xaml
    /// </summary>
    public partial class ProcessedWaybillWindow : Window
    {
        public ProcessedWaybillWindow()
        {
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
        }

        private void ProcessedWaybillTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ProcessedWaybillTbl_Loaded(object sender, RoutedEventArgs e)
        {
            DocumentManager.DownloadProcessedWaybills(SessionManager.Sessions[0].ArchieveFolder);
            this.UpdateTablePart(ProcessedWaybillTbl, CoreInit.ModuleRepository.GetProcessedWaybills());
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

        private Dictionary<string, string> bindings = new Dictionary<string, string>();


    }
}
