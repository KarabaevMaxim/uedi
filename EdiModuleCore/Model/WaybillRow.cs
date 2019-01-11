namespace EdiModuleCore.Model
{
    /// <summary>
    /// Класс строки накладной слоя бизнес-логики.
    /// </summary>
    public class WaybillRow
    {
        public MatchedWare Ware { get; set; }
        public decimal Price { get; set; }
        public float Count { get; set; }
        public decimal Amount { get; set; }
        public int TaxRate { get; set; }
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
