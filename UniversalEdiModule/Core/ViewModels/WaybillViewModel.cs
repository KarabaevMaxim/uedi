namespace UniversalEdiModule.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WaybillViewModel
    {
        /// <summary>
        /// Идентификатор в системе.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Дата/время загрузки.
        /// </summary>
        public DateTime DownloadDate { get; set; }
        /// <summary>
        /// Номер накладной.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Дата накладной.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Поставщик из 1С.
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Покупатель из 1С.
        /// </summary>
        public string Buyer { get; set; }
        /// <summary>
        /// Торговый объект из 1С.
        /// </summary>
        public string DeliveryPlace { get; set; }

        public List<WarePositionViewModel> Positions { get; set; } = new List<WarePositionViewModel>();
    }
}
