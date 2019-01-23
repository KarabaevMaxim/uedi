namespace EdiModule
{
	using System;
	using System.Runtime.InteropServices;

    [Guid("6A2B049B-7FEE-4F89-BE73-A59A06C7E07C")]
    public interface IEntryPoint
    {
		/// <summary>
		/// Метод подключения к файловой базе.
		/// </summary>
		/// <param name="userName">Имя пользовалея входа.</param>
		/// <param name="userPassword">Пароль пользователя входа.</param>
		/// <param name="dbPath">Путь до папки с базой.</param>
		/// <param name="waybillFolder">Рабочая папка пользователя.</param>
		/// <param name="archieveFolder">Папка архива.</param>
		void ConnectToFileBase(string userName,
			string userPassword,
			string dbPath,
			string waybillFolder,
			string archieveFolder,
			string ftpUri,
			bool ftpPassive,
			int ftpTimeout,
			string ftpLogin,
			string ftpPassword,
			string ftpRemoteFolder);

		void ConnectToServerBase(string connectionString, string waybillFolder, string archieveFolder);

	}
}
