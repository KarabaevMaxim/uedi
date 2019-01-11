namespace UniversalEdiModule.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WarePositionViewModel
    {
        /// <summary>
        /// POSITIONNUMBER
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// DESCRIPTION
        /// </summary>
        public string WareName { get; set; }
        /// <summary>
        /// DELIVEREDUNIT
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// DELIVEREDQUANTITY
        /// </summary>
        public float Quantity { get; set; }
        /// <summary>
        /// AMOUNTWITHVAT
        /// </summary>
        public float AmountWithVat { get; set; }
        /// <summary>
        /// PRICE
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// AMOUNT
        /// </summary>
        public float Amount { get; set; }
        /// <summary>
        /// PRODUCT
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// PRODUCTIDSUPPLIER
        /// </summary>
        public string WareSupplierCode { get; set; }
        /// <summary>
        /// TAXRATE
        /// </summary>
        public int TaxRate { get; set; }

        public string InnerCode { get; set; }
        public string InnerName { get; set; }
    }
}
