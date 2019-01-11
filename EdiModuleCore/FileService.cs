namespace EdiModuleCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    public static class FileService
    {
        static FileService()
        {
            FileService.defaultEncoding = Encoding.GetEncoding("windows-1251");
        }

        public static bool MoveFile(string fileName, string destinationPath)
        {
            try
            {
                File.Move(fileName, Path.Combine(destinationPath, Path.GetFileName(fileName)));
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static void CreateDirectory(params string[] folderPaths)
        {
            foreach (var item in folderPaths)
            {
                try
                {
                    Directory.CreateDirectory(item);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
            }
        }

        public static string ReadTextFile(string fileName, string encodingName = "")
        {
            try
            {
                string result = File.ReadAllText(fileName, FileService.GetEncoding(encodingName));
                return result;
            }
            catch(FileNotFoundException ex)
            {
                throw ex;
            }
        }

        public static List<string> ReadTextFiles(string[] fileNames, string encodingName = "")
        {
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
            File.WriteAllText(fileName, content, FileService.GetEncoding(encodingName));
        }

        private static Encoding GetEncoding(string encodingName = "")
        {
            Encoding encoding = null;

            if (encodingName == "")
            {
                encoding = defaultEncoding;
            }
            else
            {
                try
                {
                    encoding = Encoding.GetEncoding(encodingName);
                }
                catch (ArgumentException ex)
                {
                    throw ex;
                }
            }
            return encoding;
        }

        public static string[] GetFileList(string folderName)
        {
            try
            {
                return Directory.GetFiles(folderName);
            }
            catch(ArgumentNullException ex)
            {
                throw ex;
            }
            catch(DirectoryNotFoundException ex)
            {
                throw ex;
            }
        }

        private static readonly Encoding defaultEncoding;
    }
}
