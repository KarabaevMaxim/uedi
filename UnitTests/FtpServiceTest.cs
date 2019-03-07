using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	using EdiModuleCore;

	[TestClass]
	public class FtpServiceTest
	{
		[TestMethod]
		public void DownloadDocumentsTest()
		{
			FtpService.DownloadDocuments("ftp://192.168.5.5", false, 5, "polikon", "4BLRGC3XP48TP4", "Inbox\\Test", "C:\\Темп\\ФТП");
		}

		[TestMethod]
		public void DownloadDocuments1Test()
		{
			FtpService.DownloadDocuments1("ftp://192.168.5.5", false, 5, "polikon", "4BLRGC3XP48TP4", "Inbox\\Test", "C:\\Темп\\ФТП");
		}
	}
}
