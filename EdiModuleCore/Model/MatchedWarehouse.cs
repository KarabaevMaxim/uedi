namespace EdiModuleCore.Model
{
	using System;
	using Bridge1C.DomainEntities;

	/// <summary>
	/// Сопоставленная сущность склада.
	/// </summary>
	public class MatchedWarehouse
	{
		/// <summary>
		/// Склад из базы.
		/// </summary>
		public Warehouse InnerWarehouse { get; set; }
		/// <summary>
		/// Склад из накладной.
		/// </summary>
		public ExWarehouse ExWarehouse { get; set; }

		public override bool Equals(object obj)
		{
			try
			{
				if(obj is MatchedWarehouse other)
					return this.InnerWarehouse.Equals(other.InnerWarehouse) &&
							this.ExWarehouse.Equals(other.ExWarehouse);
				return false;
			}
			catch(NullReferenceException)
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return this.InnerWarehouse?.Name;
		}
	}
}
