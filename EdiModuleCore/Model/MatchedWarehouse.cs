namespace EdiModuleCore.Model
{
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
	}
}
