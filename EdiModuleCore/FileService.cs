namespace EdiModuleCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
	using System.Linq;
	using NLog;

    public static class FileService
    {
        static FileService()
        {
            FileService.defaultEncoding = Encoding.GetEncoding("windows-1251");
        }

        public static bool MoveFile(string fileName, string destinationPath)
        {
			if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(destinationPath))
				throw new ArgumentNullException("fileName или destinationPath");

            try
            {
                File.Move(fileName, Path.Combine(destinationPath, Path.GetFileName(fileName)));
				FileService.logger.Info("Файл {0} перемещен в папку {1}", fileName, destinationPath);
                return true;
            }
            catch(Exception ex)
            {
				FileService.logger.Warn(ex, "Не удалось переместить файл {0} в папку {1}", fileName	, destinationPath);
				return false;
            }
        }

        public static void CreateDirectory(params string[] folderPaths)
        {
			if(folderPaths == null || folderPaths.Length == 0)
				throw new ArgumentNullException("folderPaths");

			foreach (var item in folderPaths)
            {
                try
                {
                    Directory.CreateDirectory(item);
					FileService.logger.Info("Папка {0} создана", item);
				}
                catch (IOException ex)
                {
					FileService.logger.Error(ex, "Не удалось создать папку {0}", item);
					throw ex;
                }
            }
        }

        public static string ReadTextFile(string fileName, string encodingName = "")
        {
			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentNullException("fileName");

			try
            {
                string result = File.ReadAllText(fileName, FileService.GetEncoding(encodingName));
				FileService.logger.Info("Текст из файла {0} прочитан: {1}", fileName, result);
				return result;
            }
            catch(FileNotFoundException ex)
            {
				FileService.logger.Error(ex, "Не удалось прочитать текст из файла {0}", fileName);
				throw ex;
            }
        }

        public static List<string> ReadTextFiles(string[] fileNames, string encodingName = "")
        {
			if (fileNames == null || fileNames.Length == 0)
				throw new ArgumentNullException("fileNames");

            List<string> result = new List<string>();

            foreach (var item in fileNames)
            {
                try
                {
                    result.Add(FileService.ReadTextFile(item, encodingName));
                }
                catch (FileNotFoundException ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        public static void WriteTextFile(string fileName, string content, string encodingName = "")
        {
			if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(content))
				throw new ArgumentNullException("fileName или content");

            File.WriteAllText(fileName, content, FileService.GetEncoding(encodingName));
			FileService.logger.Info("Текст: {0} записан в файл {1}", content, fileName);
		}

        private static Encoding GetEncoding(string encodingName = "")
        {
            Encoding encoding = null;

            if (encodingName == "")
            {
                encoding = defaultEncoding;
				FileService.logger.Info("Получена стандартная кодировка {0}", defaultEncoding);
			}
            else
            {
                try
                {
                    encoding = Encoding.GetEncoding(encodingName);
                }
                catch (ArgumentException ex)
                {
					FileService.logger.Error(ex, "Кодировка с именем {0} не получена", encodingName);
					throw ex;
                }
            }

			FileService.logger.Info("Получена кодировка {0}", encoding.EncodingName);
			return encoding;
        }

        public static string[] GetFileList(string folderName)
        {
			if (string.IsNullOrWhiteSpace(folderName))
				throw new ArgumentNullException("folderName");

            try
            {
				string[] result = Directory.GetFiles(folderName);
				FileService.logger.Info("Получен список файлов папки {0}", folderName, String.Join(", ", result));
				return result;
			}
            catch(ArgumentNullException ex)
            {
				FileService.logger.Error(ex, "Не удалось получить список файлов");
				throw ex;
            }
            catch(DirectoryNotFoundException ex)
            {
				FileService.logger.Error(ex, "Не удалось получить список файлов");
				throw ex;
            }
        }

		public static void WriteBytesToFile(string fileName, IEnumerable<byte> bytes)
		{
			if (string.IsNullOrWhiteSpace(fileName) || bytes == null)
				throw new ArgumentNullException("fileName или bytes");
			try
			{
				File.WriteAllBytes(fileName, bytes.ToArray());
				FileService.logger.Info("Массив байтов записан в файл {0}", fileName);
			}
			catch (DirectoryNotFoundException ex)
			{
				FileService.logger.Error(ex, "Массив байтов не записан в файл {0}");
				throw ex;
			}
		}

        private static readonly Encoding defaultEncoding;
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
