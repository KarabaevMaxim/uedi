namespace EdiModuleCore.Comparators
{
	using System.Collections.Generic;
	using Model;

	/// <summary>
	/// Класс для сравнения несопоставленных складов по ГЛН внешнего склада.
	/// </summary>
	public class MatchedWarehouseComparator : IEqualityComparer<MatchedWarehouse>
	{
		public bool Equals(MatchedWarehouse x, MatchedWarehouse y)
		{
			bool result = x.ExWarehouse.Equals(y.ExWarehouse);
			return result;
		}

		public int GetHashCode(MatchedWarehouse obj)
		{
			return obj.GetHashCode();
		}
	}
}
