namespace UniversalEdiModule.Core.DomainEntities
{
    public class WarePosition
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

        public WarePosition(XEntities.WarePosition xPosition)
        {
            this.Number = xPosition.Number;
            this.WareName = xPosition.WareName;
            this.Unit = xPosition.Unit;
            this.Quantity = xPosition.Quantity;
            this.AmountWithVat = xPosition.AmountWithVat;
            this.Price = xPosition.Price;
            this.Amount = xPosition.Amount;
            this.Barcode = xPosition.Barcode;
            this.WareSupplierCode = xPosition.WareSupplierCode;
            this.TaxRate = xPosition.TaxRate;
        }

        public override string ToString()
        {
            return string.Format("Номер: {0}, Название: {1}, Цена: {2}, Количество: {3}, ЕИ: {4}, Код поставщика: {5}", 
                                this.Number, this.WareName, this.Price, this.Quantity, this.Unit, this.WareSupplierCode);
        }
    }
}
