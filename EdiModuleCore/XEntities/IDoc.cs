namespace EdiModuleCore.XEntities
{
	using System;

	public interface IDoc
	{
		string Number { get; set; }
		DateTime Date { get; set; }
        IDocHeader Header { get; set; }
	}
}
