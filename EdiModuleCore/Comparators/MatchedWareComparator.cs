namespace EdiModuleCore.Comparators
{
	using System.Collections.Generic;
	using Model;

	/// <summary>
	/// Класс для сравнения несопоставленных товаров по ГЛН внешнего поставщика.
	/// </summary>
	public class MatchedWareComparator : IEqualityComparer<MatchedWare>
	{
		public bool Equals(MatchedWare x, MatchedWare y)
		{
			bool result = x.ExWare.Equals(y.ExWare);
			return result;
		}

		public int GetHashCode(MatchedWare obj)
		{
			return obj.GetHashCode();
		}
	}
}
