namespace EdiModuleCore.XEntities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Order : IXEntity
	{
		public string Number { get; set; }
		public DateTime Date { get; set; }
	}
}
