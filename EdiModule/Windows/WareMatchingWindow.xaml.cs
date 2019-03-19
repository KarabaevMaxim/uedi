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
    using EdiModuleCore.Exceptions;

    /// <summary>
    /// Логика взаимодействия для WareMatchingWindow.xaml
    /// </summary>
    public partial class WareMatchingWindow : Window, ITableWindow
    {
        public WareMatchingWindow()
        {
            InitializeComponent();

            this.bindings = new Dictionary<string, string>
            {
                { "Название поставщика", "Ware.ExWare.Name" },
                { "Код поставщика", "Ware.ExWare.Code" },
                { "ЕИ", "Ware.ExWare.Unit.Name" },
                { "Количество", "Count" },
                { "Сумма", "Amount" },
				{ "Налоговая ставка, %", "TaxRate" },
				{ "Сумма налога", "TaxAmount" },
				{ "Внут. код", "Ware.InnerWare.Code" },
                { "Внут. название", "Ware.InnerWare.Name" },
                { "ШК", "Ware.ExWare.Barcode" }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.ParentWindow == null || this.Waybill == null)
                throw new NotInitializedException("Родительское окно или накладная не инициализированы.");

            this.UpdateTablePart();
            this.WaybillNumberLbl.Text = this.Waybill.ToString();
            this.SupplierNameLbl.Text = "Поставщик: " + this.Waybill.Supplier?.InnerCounteragent?.Name;
            this.OrganizationLbl.Text = "Организация: " + this.Waybill.Organization?.Name;
            this.TradeObjectLbl.Text = "Склад: " + this.Waybill.Warehouse?.InnerWarehouse?.Name;
			this.WbAmountLbl.Text = this.Waybill.Amount.ToString();
			this.WbAmountTxtWithTaxLbl.Text = this.Waybill.AmountWithTax.ToString();
			this.TaxAmountLbl.Text = Math.Round(this.Waybill.AmountWithTax - this.Waybill.Amount, 2).ToString();
		}

        public void UpdateTablePart()
        {
            this.PositionsTbl.Columns.Clear();

            foreach(var item in this.bindings)
                this.PositionsTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });
                
            this.PositionsTbl.ItemsSource = this.Waybill.Wares;
        }

        /// <summary>
        /// Нажатие на кнопку "Загрузить накладную".
        /// </summary>
        private void ApplyBtn_Click(object sender, RoutedEventArgs e) // todo: не дает провести накладную: не указан НДС, не указан ответственный, не указано количество 
        {
            try
            {
                DocumentManager.ProcessWaybill(this.Waybill);
                this.ParentWindow.UpdateTablePart();
                MessageBox.Show("Накладная успешно обработана.", "Успех.");
                this.Close();
            }
            catch(NotProcessedDocumentException ex)
            {
                MessageBox.Show(ex.Message, "Не удалось обработать накладную.");
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                if (row.DataContext is WaybillRow wbRow)
                {
					if (wbRow.Ware.ExWare.Supplier?.InnerCounteragent == null)
					{
						ProductReferenceWindow prodWindow = new ProductReferenceWindow
						{
							Ware = wbRow.Ware,
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

        /// <summary>
        /// Нажание на кнопку "Создать для всех строк".
        /// </summary>
        private void AddAllWaresBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.PositionsTbl.Items)
            {
				int i = 0;
				if ((item is WaybillRow row) && (row.Ware != null) && (row.Ware.InnerWare == null) && (row.Ware.ExWare != null))
				{
					try
					{
						bool res = object.ReferenceEquals(Waybill.Wares[i], row);
						MatchingModule.CreateNewInnerWareAndMatch(row.Ware);
						i++;
					}
					catch(NotMatchedException ex)
					{
						MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					}
					
				}
            }

            this.UpdateTablePart();
        }

        /// <summary>
        /// Нажатие на кнопку "Создать для выделенной строки".
        /// </summary>
        private void AddNewWareBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.PositionsTbl.SelectedItems)
            {
				if ((item is WaybillRow row) && (row.Ware != null) && (row.Ware.InnerWare == null) && (row.Ware.ExWare != null))
				{
					try
					{
						MatchingModule.CreateNewInnerWareAndMatch(row.Ware);
					}
					catch (NotMatchedException ex)
					{
						MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
            }

            this.UpdateTablePart();
        }

        private void PositionsTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        public ITableWindow ParentWindow { get; set; }
        public Waybill Waybill { get; set; }

        private Dictionary<string, string> bindings;
    }
        
}
