namespace UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using EdiModuleCore;

	[TestClass]
	public class FtpServiceTest
	{
		[TestMethod]
		public void DownloadDocumentsTest()
		{
			//FtpService.DownloadDocuments("ftp://192.168.5.5", false, 5, "polikon", "4BLRGC3XP48TP4", "Inbox\\Test", "C:\\Темп\\ФТП");
		}

		[TestMethod]
		public void DownloadDocuments1Test()
		{
			FtpService.DownloadDocumentsNative("ftp://192.168.5.5", false, 5, "polikon", "4BLRGC3XP48TP4", "Inbox\\Test", "C:\\Темп\\ФТП");
		}

		[TestMethod]
		public void DownloadFileTest()
		{
			bool result = FtpService.DownloadFile(false, 5, new NetworkCredential("polikon", "4BLRGC3XP48TP4"), @"ftp://192.168.5.5/Inbox/Test", "Тест.txt", "C:\\Темп\\ФТП");
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void RemoveFileTest()
		{
			bool result = FtpService.RemoveFile(false, 5, new NetworkCredential("polikon", "4BLRGC3XP48TP4"), @"ftp://192.168.5.5/Inbox/Test", "Тест.txt");
			Assert.IsTrue(result);
		}


		[TestMethod]
		public void GetFileListTest()
		{
			List<string> result = FtpService.GetFileList(false, 5, new NetworkCredential("polikon", "4BLRGC3XP48TP4"), @"ftp://192.168.5.5/Inbox/Test");
			Assert.IsTrue(result.Any());
		}
	}
}
