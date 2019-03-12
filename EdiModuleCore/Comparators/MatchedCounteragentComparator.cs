namespace EdiModuleCore.Comparators
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Model;

	/// <summary>
	/// Класс для сравнения несопоставленных поставщиков по ГЛН внешнего поставщика.
	/// </summary>
	public class MatchedCounteragentComparator : IEqualityComparer<MatchedCounteragent>
	{
		public bool Equals(MatchedCounteragent x, MatchedCounteragent y)
		{
			bool result = x.ExCounteragent.Equals(y.ExCounteragent);
			return result;
		}

		public int GetHashCode(MatchedCounteragent obj)
		{
			return obj.GetHashCode();
		}
	}
}
