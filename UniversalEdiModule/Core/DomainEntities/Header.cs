namespace UniversalEdiModule.Core.DomainEntities
{
    using System.Collections.Generic;

    public class Header
    {
        /// <summary>
        /// Поставщик из 1С.
        /// </summary>
        public dynamic Supplier { get; set; }
        /// <summary>
        /// Покупатель из 1С.
        /// </summary>
        public dynamic Buyer { get; set; }
        /// <summary>
        /// Торговый объект из 1С.
        /// </summary>
        public dynamic DeliveryPlace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WarePosition> Positions { get; set; } = new List<WarePosition>();

        public Header() { }

        public Header(dynamic supplier, dynamic buyer, dynamic deliveryPlace, List<XEntities.WarePosition> positions)
        {
            this.Supplier = null;
            this.Buyer = null;
            this.DeliveryPlace = null;

            foreach (var item in positions)
            {
                this.Positions.Add(new WarePosition(item));
            }
        }

    }
}
