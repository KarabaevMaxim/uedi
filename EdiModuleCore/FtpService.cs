namespace EdiModuleCore
{
	using ArxOne.Ftp;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using NLog;
	using Newtonsoft.Json;

	public static class FtpService
	{
		public static void DownloadDocumentsNative(string serverURI, bool passiveMode, int timeoutSec, string login, string password, string remoteFolder, string localFolder)
		{
			FtpService.logger.Info("Загрузка документов с фтп сервера по пути {0}/{1}", serverURI, remoteFolder);

			if (string.IsNullOrWhiteSpace(serverURI))
				throw new ArgumentNullException("serverURI");

			if (string.IsNullOrWhiteSpace(login))
				throw new ArgumentNullException("login");

			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentNullException("password");

			if (string.IsNullOrWhiteSpace(remoteFolder))
				throw new ArgumentNullException("remoteFolder");

			if (string.IsNullOrWhiteSpace(localFolder))
				throw new ArgumentNullException("localFolder");

			NetworkCredential credential = new NetworkCredential(login, password);
			string folderPath = string.Format("{0}/{1}", serverURI, remoteFolder);
			List<string> fileNames = FtpService.GetFileList(passiveMode, timeoutSec, credential, folderPath);

			foreach (var item in fileNames)
			{
				if(FtpService.DownloadFile(passiveMode, timeoutSec, credential, folderPath, item, localFolder))
					FtpService.RemoveFile(passiveMode, timeoutSec, credential, folderPath, item);
			}

			FtpService.logger.Info("Загрузка документов завершена");
		}

		/// <summary>
		/// Получает список файлой на FTP-сервере по указанному пути.
		/// </summary>
		/// <param name="passiveMode">Пассивный режим сервера.</param>
		/// <param name="timeoutSec">Таймаут ответа от сервера.</param>
		/// <param name="credentials">Параметры авторизации на сервере.</param>
		/// <param name="directoryPath">Путь до папки на сервере.</param>
		/// <returns>Список строк с именами файлов.</returns>
		public static List<string> GetFileList(bool passiveMode, int timeoutSec, NetworkCredential credentials, string directoryPath)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(directoryPath);
			ftpWebRequest.Credentials = credentials;
			ftpWebRequest.UseBinary = true;
			ftpWebRequest.UsePassive = passiveMode;
			ftpWebRequest.KeepAlive = true;
			ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
			List<string> result = new List<string>();

			using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
			{
				using (Stream ftpStream = ftpWebResponse.GetResponseStream())
				{
					using (StreamReader ftpReader = new StreamReader(ftpStream))
					{	
						while (ftpReader.Peek() != -1)
							result.Add(ftpReader.ReadLine());
					}
				}
			}

			FtpService.logger.Info("Получен список файлов. Список: {0}", JsonConvert.SerializeObject(result));
			return result;
		}

		/// <summary>
		/// Загружает файл с FTP-сервера на компьютер.
		/// </summary>
		/// <param name="passiveMode">Пассивный режим сервера.</param>
		/// <param name="timeoutSec">Таймаут ответа от сервера.</param>
		/// <param name="credentials">Параметры авторизации на сервере.</param>
		/// <param name="directoryPath">Путь до папки с файлом на сервере.</param>
		/// <param name="fileName">Имя файла.</param>
		/// <param name="localPath">Путь до папки, куда будет загружен файл.</param>
		/// <returns>true в случае успеха, иначе false.</returns>
		public static bool DownloadFile(bool passiveMode, int timeoutSec, NetworkCredential credentials, string directoryPath, string fileName, string localPath)
		{
			FtpService.logger.Info("Загрузка файла {0}/{1}", directoryPath, fileName);
			if (credentials == null || string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(localPath))
				throw new ArgumentNullException("credentials, directoryPath, fileName или localPath");

			FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(directoryPath + "/" + fileName);
			ftpWebRequest.UsePassive = passiveMode;
			ftpWebRequest.UseBinary = true;
			ftpWebRequest.KeepAlive = true;
			ftpWebRequest.Credentials = credentials;
			ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

			try
			{
				using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
				{
					using (Stream ftpStream = ftpWebResponse.GetResponseStream())
					{
						List<byte> byteList = new List<byte>();
						int curByte;

						while ((curByte = ftpStream.ReadByte()) != -1)
							byteList.Add((byte)curByte);

						FileService.WriteBytesToFile(Path.Combine(localPath, fileName), byteList);
						FtpService.logger.Info("Файл {0}/{1} загружен в папку {2}", directoryPath, fileName, localPath);
						return true;
					}
				}
			}
			catch(WebException ex)
			{
				FtpService.logger.Error(ex, "Ошибка при загрузке файла {0}/{1}", directoryPath, fileName);
				throw ex;
			}
		}

		/// <summary>
		/// Удаляет файл с FTP-сервера.
		/// </summary>
		/// <param name="passiveMode">Пассивный режим сервера.</param>
		/// <param name="timeoutSec">Таймаут ответа от сервера.</param>
		/// <param name="credentials">Параметры авторизации на сервере.</param>
		/// <param name="directoryPath">Путь до папки с файлом на сервере.</param>
		/// <param name="fileName">Имя файла.</param>
		/// <returns>true в случае успеха, иначе false.</returns>
		public static bool RemoveFile(bool passiveMode, int timeoutSec, NetworkCredential credentials, string directoryPath, string fileName)
		{
			FtpService.logger.Info("Удаление файла {0}/{1}", directoryPath, fileName);
			FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(directoryPath + "/" + fileName);
			ftpWebRequest.UsePassive = passiveMode;
			ftpWebRequest.UseBinary = true;
			ftpWebRequest.KeepAlive = true;
			ftpWebRequest.Credentials = credentials;
			ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;

			try
			{
				using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
				{
					bool result = ftpWebResponse.StatusCode == FtpStatusCode.FileActionOK;

					if(result)
						FtpService.logger.Info("Файл {0}/{1} удален", directoryPath, fileName);
					else
						FtpService.logger.Warn("Файл {0}/{1} не удален. Причина: {2}", directoryPath, fileName, ftpWebResponse.StatusDescription);

					return result;
				}
			}
			catch (WebException ex)
			{
				FtpService.logger.Error(ex, "Ошибка при удалении файла {0}/{1}", directoryPath, fileName);
				return false;
			}
		}

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
	}
}
