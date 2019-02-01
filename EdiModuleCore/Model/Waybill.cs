namespace EdiModuleCore.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bridge1C.DomainEntities;

    /// <summary>
    /// Класс накладной слоя бизнес-логики.
    /// </summary>
    public class Waybill
    {
		/// <summary>
		/// Номер накладной.
		/// </summary>
        public string Number { get; set; }
		/// <summary>
		/// Дата накладной.
		/// </summary>
        public DateTime Date { get; set; }
		/// <summary>
		/// Поставщик.
		/// </summary>
        public MatchedCounteragent Supplier { get; set; }
		/// <summary>
		/// Организация.
		/// </summary>
        public Organization Organization { get; set; }
		/// <summary>
		/// Склад.
		/// </summary>
        public Warehouse Warehouse { get; set; }
		/// <summary>
		/// Строки товара.
		/// </summary>
        public List<WaybillRow> Wares { get; set; }
		/// <summary>
		/// Имя файла.
		/// </summary>
        public string FileName { get; set; }

        public override bool Equals(object obj)
        {
			try
			{
				if (obj is Waybill waybill)
					return this.Number == waybill.Number &&
							this.Date == waybill.Date &&
							this.Supplier.Equals(waybill.Supplier) &&
							this.Organization.Equals(waybill.Organization) &&
							this.Warehouse.Equals(waybill.Warehouse) &&
							this.Wares.Any(waybill.Wares.Contains);
				else
					return false;
			}
			catch(NullReferenceException)
			{
				return false;
			}
            
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();  
        }
    }
}
