namespace EdiModule
{
    using System.Runtime.InteropServices;
    using Windows;

	[Guid("4E38FF4C-391E-448F-92F8-241D1BE55408"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IEvents))]
    public class EdiModuleEntryPoint : IEntryPoint
    {
 
		/// <summary>
		/// Метод подключения к файловой базе.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userPassword"></param>
		/// <param name="dbPath"></param>
		public void ConnectToFileBase(string userName, string userPassword, string dbPath, string waybillFolder, string archieveFolder)
		{
			EdiModuleCore.CoreInit.Connect(userName, userPassword, dbPath);
			EdiModuleCore.CoreInit.Init();
			EdiModuleCore.SessionManager.CreateSession(userName, waybillFolder, archieveFolder);
			MainWindow window = new MainWindow();
			window.ShowDialog();
		}

		/// <summary>
		/// Метод подключения к серверной Базе.
		/// </summary>
		public void ConnectToServerBase(string connectionString, string waybillFolder, string archieveFolder)
		{

		}
	}
}
