namespace EdiModuleCore.XEntities
{
	using System;

	public interface IXEntity
	{
		/// <summary>
		/// NUMBER
		/// </summary>
		string Number { get; set; }
		/// <summary>
		/// DATE
		/// </summary>
		DateTime Date { get; set; }
	}
}
