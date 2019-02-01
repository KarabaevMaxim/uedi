namespace EdiModuleCore.Model
{
    public class ExCounteragent
    {
        public string GLN { get; set; }

		public override bool Equals(object obj)
		{
			if(obj is ExCounteragent other)
				return this.GLN == other.GLN;

			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return this.GLN;
		}
	}
}
