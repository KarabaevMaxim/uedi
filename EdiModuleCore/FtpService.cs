namespace EdiModuleCore
{
	using ArxOne.Ftp;
	using ArxOne.Ftp.Exceptions;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;

	public static class FtpService
	{
		public static bool DownloadDocuments(string serverURI, bool passiveMode, int timeoutSec, string login, string password, string remoteFolder, string localFolder)
		{
			NetworkCredential networkCredential = new NetworkCredential(login, password);
			TimeSpan timeout = new TimeSpan(0, 0, timeoutSec);
			FtpClientParameters parametres = new FtpClientParameters
			{
				ConnectTimeout = timeout,
				ReadWriteTimeout = timeout,
				Passive = passiveMode
			};
			Uri uri = new Uri(serverURI);
			FtpPath ftpPath = new FtpPath(remoteFolder);

			using (var ftpClient = new FtpClient(uri, networkCredential, parametres))
			{
				try
				{
					var files = ftpClient.ListEntries(ftpPath).Where(en => en.Type == FtpEntryType.File);

					foreach (var item in files)
						FtpService.DownloadFile(ftpClient, item, localFolder);
				}
				catch(FtpAuthenticationException)
				{
					return false;
				}
				catch(FtpTransportException)
				{
					return false;
				}
			}

			return true;
		}

		public static void DownloadDocumentsNative(string serverURI, bool passiveMode, int timeoutSec, string login, string password, string remoteFolder, string localFolder)
		{
			NetworkCredential credential = new NetworkCredential(login, password);
			string folderPath = string.Format("{0}/{1}", serverURI, remoteFolder);
			List<string> fileNames = FtpService.GetFileList(passiveMode, timeoutSec, credential, folderPath);

			foreach (var item in fileNames)
			{
				if(FtpService.DownloadFile(passiveMode, timeoutSec, credential, folderPath, item, localFolder))
				{
					FtpService.RemoveFile(passiveMode, timeoutSec, credential, folderPath, item);
				}
			}
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
						return true;
					}
				}
			}
			catch(WebException)
			{
				return false;
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
			FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(directoryPath + "/" + fileName);
			ftpWebRequest.UsePassive = passiveMode;
			ftpWebRequest.UseBinary = true;
			ftpWebRequest.KeepAlive = true;
			ftpWebRequest.Credentials = credentials;
			ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;

			try
			{
				using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
					return ftpWebResponse.StatusCode == FtpStatusCode.FileActionOK;
			}
			catch (WebException)
			{
				return false;
			}
		}

		/// <summary>
		/// Загрузить файл с ФТП сервера.
		/// </summary>
		/// <param name="ftpClient">Объект ФТП клиента для соединения с сервером.</param>
		/// <param name="file">Объет файла на сервере.</param>
		/// <param name="localPath"></param>
		/// <returns></returns>
		private static bool DownloadFile(FtpClient ftpClient, FtpEntry file, string localPath)
		{
			if (file.Type != FtpEntryType.File)
				return false;

			using (var stream = ftpClient.Retr(file.Path)) // todo: Не может загрузить накладные с русскими символами
			{
				List<byte> byteList = new List<byte>();
				int curByte;

				while ((curByte = stream.ReadByte()) != -1)
					byteList.Add((byte)curByte);

				string newFileName = string.Empty;
				FileService.WriteBytesToFile(Path.Combine(localPath, file.Name), byteList);

				if (ftpClient.Dele(file.Path))
					return true;
				else
					return false;
			}
		}
	}
}
