namespace EdiModuleCore.Model
{
    using Bridge1C.DomainEntities;

    /// <summary>
    /// Класс номенклатуры поставщика слоя бизнес-логики.
    /// </summary>
    public class ExWare
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public Unit Unit { get; set; }
        public Counteragent Supplier { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is ExWare ware))
                return  this.Code == ware.Code &&
                        this.Name == ware.Name &&
                        this.Barcode == ware.Barcode &&
                        this.Unit.Equals(ware.Unit) &&
                        this.Supplier.Equals(ware.Supplier);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
