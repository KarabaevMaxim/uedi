namespace Bridge1C.DomainEntities
{
    public class WaybillRow
    {
        public Ware Ware { get; set; }
        public Unit Unit { get; set; }
        public float Count { get; set; }
        public decimal Price { get; set; }
        public int TaxRate { get; set; } // todo: налоги не записываются в базу
        public decimal TaxAmount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is WaybillRow row)
                return this.Ware.Equals(row.Ware) &&
                    this.Unit.Equals(row.Unit) &&
                    this.Count == row.Count &&
                    this.Price == row.Price &&
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
