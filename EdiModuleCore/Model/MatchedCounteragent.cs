namespace EdiModuleCore.Model
{
	using System;
	using Bridge1C.DomainEntities;

	/// <summary>
	/// Сопоставленная сущность контрагента.
	/// </summary>
    public class MatchedCounteragent
    {
		/// <summary>
		/// Контрагент из базы данных.
		/// </summary>
        public Counteragent InnerCounteragent { get; set; }
		/// <summary>
		/// Контрагент из накладной.
		/// </summary>
        public ExCounteragent ExCounteragent { get; set; }

		public override string ToString()
		{
			return string.Format("{0}", InnerCounteragent?.Name);
		}

		public override bool Equals(object obj)
		{
			if(obj is MatchedCounteragent other)
			{
				try
				{
					return this.InnerCounteragent.Equals(other.InnerCounteragent) &&
						this.ExCounteragent.Equals(other.ExCounteragent);
				}
				catch(NullReferenceException)
				{}
			}

			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
