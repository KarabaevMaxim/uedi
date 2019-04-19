namespace Bridge1C.DomainEntities.Spr
{
    public class Shop
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Shop shop)
                return this.Code == shop.Code &&
                    this.Name == shop.Name;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
