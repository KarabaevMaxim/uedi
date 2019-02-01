namespace EdiModuleCore.Model
{
	/// <summary>
	/// Внешняя сущность контрагента.
	/// </summary>
    public class ExCounteragent
    {
		/// <summary>
		/// ГЛН контрагента из накладной.
		/// </summary>
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
