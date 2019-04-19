namespace Bridge1C.DomainEntities.Spr
{
    public class Warehouse
    {
        public string Code { get; set; }
        public string Name { get; set; }
		public User User { get; set; }
        public Shop Shop { get; set; }

        public override bool Equals(object obj)
        {
			if (obj is Warehouse wh)
				return this.Code == wh.Code &&
						this.Name == wh.Name &&
						this.Shop.Equals(wh.Shop) &&
						this.User.Equals(wh.User);
			else
				return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
