namespace EdiModuleCore.XEntities.DocOrder
{
	using System;

	public class Order : IDoc
	{
		public string Number { get; set; }
		public DateTime Date { get; set; }
        public IDocHeader Header { get; set; }
	}
}
