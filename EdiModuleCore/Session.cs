namespace EdiModuleCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Session 
	{
		public Guid Key { get; set; }
		public string UserName { get; set; }
		public string WorkFolder { get; set; }
		public string ArchieveFolder { get; set; }
	}
}
