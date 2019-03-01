namespace EdiModuleCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ArxOne.Ftp;
	using System.Net;
	using System.IO;
	using ArxOne.Ftp.Exceptions;

	public static class FtpService
	{
		public static bool DownloadDocuments(string serverURI, bool passiveMode, int timeoutSec,  string login, string password, string remoteFolder, string localFolder)
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

		public async static Task<bool> DownloadDocumentsAsync(string serverURI, bool passiveMode, int timeoutSec, string login, string password, string remoteFolder, string localFolder)
		{
			return await Task.Run(() => DownloadDocuments(serverURI, passiveMode, timeoutSec, login, password, remoteFolder, localFolder));
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
