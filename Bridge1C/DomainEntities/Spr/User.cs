namespace DAL.DomainEntities.Spr
{
	public class User
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is User user)
				return	this.Code == user.Code &&
						this.Name == user.Name;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", this.Code, this.Name);
		}
	}
}
