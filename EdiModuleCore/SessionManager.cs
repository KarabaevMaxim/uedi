﻿namespace EdiModuleCore
{
	using System;
	using System.Collections.Generic;

	public static class SessionManager
	{
		public static List<Session> Sessions { get; set; } = new List<Session>();

		public static Session CreateSession(string workFolder, 
			string archieveFolder, 
			string ftpUri, 
			bool ftpPassive, 
			int ftpTimeout, 
			string ftpLogin, 
			string ftpPassword, 
			string ftpRemoteFolder)
		{
			Session result = new Session
			{
				Key = Guid.NewGuid(),
				ArchieveFolder = archieveFolder,
				WorkFolder = workFolder,
				FtpURI = ftpUri,
				FtpPassive = ftpPassive,
				FtpTimeout = ftpTimeout,
				FtpLogin = ftpLogin,
				FtpPassword = ftpPassword,
				FtpRemoteFolder = ftpRemoteFolder
			};
			SessionManager.Sessions.Add(result);
			return result;
		}
	}
}
