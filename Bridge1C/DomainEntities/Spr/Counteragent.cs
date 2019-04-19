namespace Bridge1C.DomainEntities.Spr
{
    public class Counteragent
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string GLN { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", this.Name);
        }

        public override bool Equals(object obj)
        {
			if ((obj != null) && (obj is Counteragent counteragent))
				return this.Code == counteragent.Code &&
						this.Name == counteragent.Name &&
						this.FullName == counteragent.FullName &&
                        this.GLN == counteragent.GLN;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
