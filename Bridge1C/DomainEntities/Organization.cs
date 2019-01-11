namespace Bridge1C.DomainEntities
{
    public class Organization
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string GLN { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Organization org)
                return this.Code == org.Code &&
                    this.Name == org.Name &&
                    this.GLN == org.GLN;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
