namespace EdiModuleCore
{
	using System;

	public class Session 
	{
		public Guid Key { get; set; }
		public string FtpURI { get; set; }
		public bool FtpPassive { get; set; }
		public int FtpTimeout { get; set; }
		public string FtpLogin { get; set; }
		public string FtpPassword { get; set; }
		public string FtpRemoteFolder { get; set; }
		public string UserName { get; set; }
		public string WorkFolder { get; set; }
		public string ArchieveFolder { get; set; }

	}
}
