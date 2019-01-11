namespace Bridge1C.DomainEntities
{
    public class Warehouse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string GLN { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Warehouse wh)
                return this.Code == wh.Code &&
                    this.Name == wh.Name &&
                    this.GLN == wh.GLN;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
