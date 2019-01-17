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
			ediModuleEntryPoint.ConnectToFileBase("Админ", "123", @"C:\Users\Максим\Documents\InfoBase7", @"C:\Темп\Поликон накладные EDI\Визит", @"C:\Темп\Поликон накладные EDI\Архив");
		}
	}
}
