namespace EdiModuleCore.Model
{
    using DAL.DomainEntities.Spr;

    /// <summary>
    /// Класс номенклатуры поставщика слоя бизнес-логики.
    /// </summary>
    public class ExWare
    {
		/// <summary>
		/// Код товара поставщика.
		/// </summary>
        public string Code { get; set; }
		/// <summary>
		/// Наименование товара поставщика.
		/// </summary>
        public string Name { get; set; }
		/// <summary>
		/// Штрихкод товара поставщика.
		/// </summary>
        public string Barcode { get; set; }
		/// <summary>
		/// Единица измерения.
		/// </summary>
        public Unit Unit { get; set; }
		/// <summary>
		/// Поставщик.
		/// </summary>
        public MatchedCounteragent Supplier { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is ExWare ware))
			{
				return this.Code == ware.Code &&
					   this.Name == ware.Name &&
					   this.Barcode == ware.Barcode &&
					   this.Unit.Equals(ware.Unit) &&
					   this.Supplier.Equals(ware.Supplier);
			}
               

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
