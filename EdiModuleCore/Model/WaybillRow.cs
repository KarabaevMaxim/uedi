namespace EdiModuleCore.Model
{
    /// <summary>
    /// Класс строки накладной слоя бизнес-логики.
    /// </summary>
    public class WaybillRow
    {
		/// <summary>
		/// Номенклатура.
		/// </summary>
        public MatchedWare Ware { get; set; }
		/// <summary>
		/// Цена.
		/// </summary>
        public decimal Price { get; set; }
		/// <summary>
		/// Количество.
		/// </summary>
        public float Count { get; set; }
		/// <summary>
		/// Сумма.
		/// </summary>
        public decimal Amount { get; set; }
		/// <summary>
		/// Ставка НДС.
		/// </summary>
        public int TaxRate { get; set; }
		/// <summary>
		/// Сумма налогов.
		/// </summary>
        public decimal TaxAmount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is WaybillRow row)
                return this.Ware.Equals(row.Ware) &&
                    this.Price == row.Price &&
                    this.Count == row.Count &&
                    this.Amount == row.Amount &&
                    this.TaxRate == row.TaxRate &&
                    this.TaxAmount == row.TaxAmount;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
