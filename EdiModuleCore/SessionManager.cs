namespace EdiModuleCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class SessionManager
	{
		public static List<Session> Sessions { get; set; } = new List<Session>();

		public static Session CreateSession(string userName, string workFolder, string archieveFolder)
		{
			Session result = new Session
			{
				Key = Guid.NewGuid(),
				UserName = userName,
				ArchieveFolder = archieveFolder,
				WorkFolder = workFolder
			};
			SessionManager.Sessions.Add(result);
			return result;
		}
	}
}
