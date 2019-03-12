using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	using EdiModule;
	[TestClass]
	public class EntryPointTest
	{
		[TestMethod]
		public void EdiModuleEntryPointConnectToFileBaseTest()
		{
			IEntryPoint ediModuleEntryPoint = new EdiModuleEntryPoint();
			ediModuleEntryPoint.ConnectToFileBase("Админ",
				"123",
                @"C:\Базы данных\1С\Розница для тестов ЕДИ модуля",
                @"C:\Базы данных\1С\Розница для тестов ЕДИ модуля\Рабочая папка",
                @"C:\Базы данных\1С\Розница для тестов ЕДИ модуля\Архивная папка",
				"ftp://192.168.5.5",
				false,
				10,
				"polikon",
				"4BLRGC3XP48TP4",
				"Inbox\\Test");
		}

		[TestMethod]
		public void EdiModuleEntryPointConnectToServerBase()
		{
			IEntryPoint ediModuleEntryPoint = new EdiModuleEntryPoint();
			ediModuleEntryPoint.ConnectToServerBase(@"Data Source = (local); Initial Catalog = ItidaTest; User ID = idleadmin; Password = itida",
				@"C:\Базы данных\1С\Розница для тестов ЕДИ модуля\Рабочая папка",
				@"C:\Базы данных\1С\Розница для тестов ЕДИ модуля\Архивная папка", 
				"ftp://192.168.5.5",
				false,
				10,
				"polikon",
				"4BLRGC3XP48TP4",
				"Inbox\\Test");
		}
	}
}
