namespace UniversalEdiModule.Core.XEntities
{
    using System.Collections.Generic;
    using System.Text;
    public struct Header
    {
        /// <summary>
        /// SUPPLIER
        /// </summary>
        public string SupplierGln { get; set; }
        /// <summary>
        /// BUYER
        /// </summary>
        public string BuyerGln { get; set; }
        /// <summary>
        /// DELIVERYPLACE
        /// </summary>
        public string DeliveryPlace { get; set; }
        /// <summary>
        /// Список позиций в документе.
        /// </summary>
        public List<WarePosition> Positions { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("   ГЛН поставщика: {0}, ГЛН покупателя: {1}, ГЛН ТО: {2}\n", this.SupplierGln, this.BuyerGln, this.DeliveryPlace);

            foreach (var item in this.Positions)
            {
                result.AppendFormat("       Позиция: {0}\n", item);
            }
            return result.ToString();
        }
    }
}
