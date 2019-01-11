namespace Bridge1C.DomainEntities
{
    using System.Linq;
    using System.Collections.Generic;

    public class Ware
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public Unit Unit { get; set; }
        public List<string> BarCodes { get; set; }
        public List<WareExCode> ExCodes { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is Ware ware))
                return this.Code == ware.Code &&
                        this.Name == ware.Name &&
                        this.FullName == ware.FullName &&
                        this.Unit.Equals(ware.Unit) &&
                        ((this.BarCodes == null && ware.BarCodes == null) || (this.BarCodes.Count == 0 && ware.BarCodes.Count == 0) || this.BarCodes.Any(ware.BarCodes.Contains)) &&
                        ((this.ExCodes == null && ware.ExCodes == null) || (this.ExCodes.Count == 0 && ware.ExCodes.Count == 0) || this.ExCodes.Any(ware.ExCodes.Contains));

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
