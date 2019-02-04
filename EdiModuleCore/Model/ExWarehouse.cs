namespace EdiModuleCore.Model
{
	/// <summary>
	/// Внешняя сущность склада.
	/// </summary>
	public class ExWarehouse
	{
		/// <summary>
		/// ГЛН склада из накладной.
		/// </summary>
		public string GLN { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is ExWarehouse other)
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
