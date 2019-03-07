namespace EdiModule
{
    using System.Runtime.InteropServices;
    using Windows;

	[Guid("4E38FF4C-391E-448F-92F8-241D1BE55408"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IEvents))]
    public class EdiModuleEntryPoint : IEntryPoint
    {
		/// <summary>
		/// Метод подключения к серверной Базе.
		/// </summary>
		public void ConnectToServerBase(string connectionString,
			string waybillFolder,
			string archieveFolder,
			string ftpUri,
			bool ftpPassive,
			int ftpTimeout,
			string ftpLogin,
			string ftpPassword,
			string ftpRemoteFolder)
		{
			EdiModuleCore.CoreInit.ConnectToItida(connectionString);
			EdiModuleCore.CoreInit.Init();
			EdiModuleCore.SessionManager.CreateSession(waybillFolder, archieveFolder, ftpUri, ftpPassive,
														ftpTimeout, ftpLogin, ftpPassword, ftpRemoteFolder);
			MainWindow window = new MainWindow();
			window.ShowDialog();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userPassword"></param>
		/// <param name="dbPath"></param>
		/// <param name="waybillFolder"></param>
		/// <param name="archieveFolder"></param>
		/// <param name="ftpUri"></param>
		/// <param name="ftpPassive"></param>
		/// <param name="ftpTimeout"></param>
		/// <param name="ftpLogin"></param>
		/// <param name="ftpPassword"></param>
		/// <param name="ftpRemoteFolder"></param>
		public void ConnectToFileBase(string userName, 
			string userPassword, 
			string dbPath, 
			string waybillFolder, 
			string archieveFolder, 
			string ftpUri,
			bool ftpPassive,
			int ftpTimeout,
			string ftpLogin,
			string ftpPassword,
			string ftpRemoteFolder)
		{
			EdiModuleCore.CoreInit.Connect(userName, userPassword, dbPath);
			EdiModuleCore.CoreInit.Init();
			EdiModuleCore.SessionManager.CreateSession(waybillFolder, archieveFolder, ftpUri, ftpPassive, 
														ftpTimeout, ftpLogin, ftpPassword, ftpRemoteFolder);
			MainWindow window = new MainWindow();
			window.ShowDialog();
		}

	}
}
