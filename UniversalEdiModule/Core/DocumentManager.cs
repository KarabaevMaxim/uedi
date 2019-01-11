using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalEdiModule.Core
{
    using Bridge1C;

    public enum DocumentTypes
    {
        DESADV
    }

    public static class DocumentManager
    {
        /// <summary>
        /// Загрузить документ.
        /// </summary>
        /// <param name="fileContent">Содержимое файла XML.</param>
        /// <param name="expectedDocType">Ожидаемый тип документа.</param>
        /// <returns>Объект полученной накладной.</returns>
        public static XEntities.Waybill DownloadDocument(string fileContent, DocumentTypes expectedDocType)
        {
            if(expectedDocType.ToString() != Parser.GetDocumentTypeName(fileContent))
            {
                return null;
            }
            
            return Parser.GetWaybill(fileContent);
        }

        /// <summary>
        /// Загрузить накладную.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="downloadDate">Дата загрузки.</param>
        /// <returns>true, если успешно, иначе false.</returns>
        public static bool DownloadWaybill(string fileName, DateTime downloadDate)
        {
            string xmlContent = FileService.ReadTextFile(fileName);
            XEntities.Waybill waybill = DocumentManager.DownloadDocument(xmlContent, DocumentTypes.DESADV);

            if(waybill == null)
            {
                return false;
            }

            DocumentManager.AddUnprocessedWaybill(waybill, downloadDate);
            return true;
        }

        public static void DownloadWaybills(DateTime downloadDate)
        {
            List<string> strings = FileService.ReadTextFiles(FileService.GetFileList(SettingsManager.Settings.WaybillFolder));
            DocumentManager.RemoveAllUnprocessedWaybills();

            foreach (var item in strings)
            {
                XEntities.Waybill waybill = DocumentManager.DownloadDocument(item, DocumentTypes.DESADV);

                if (waybill == null)
                {
                    continue;
                }

                DocumentManager.AddUnprocessedWaybill(waybill, downloadDate);
            }
        }

        /// <summary>
        /// Добавляет накладную в массив необработанных накладных.
        /// </summary>
        /// <param name="xWaybill">Загруженная накладная.</param>
        /// <param name="downloadDate">Дата загрузки.</param>
        private static void AddUnprocessedWaybill(XEntities.Waybill xWaybill, DateTime downloadDate)
        {
            DomainEntities.Waybill waybill = DocumentManager.ConvertWaybillToDomain(xWaybill);
            waybill.ID = DocumentManager.UnprocessedWaybills.Count;
            waybill.DownloadDate = downloadDate;
            DocumentManager.UnprocessedWaybills.Add(waybill);
        }

        /// <summary>
        /// Удалить из списка необработанных накладных.
        /// </summary>
        /// <param name="id">Идентификатор накладной.</param>
        public static void RemoveUnprocessedWaybill(int id)
        {
            DocumentManager.UnprocessedWaybills.Remove(DocumentManager.UnprocessedWaybills.FirstOrDefault(wb => wb.ID == id));
        }

        public static void RemoveAllUnprocessedWaybills()
        {
            DocumentManager.UnprocessedWaybills.Clear();
        }

        /// <summary>
        /// Конвертирует загруженную из XML наккладную в доменную накладную.
        /// </summary>
        /// <param name="xWaybill">Загруженная накладнаяю</param>
        /// <returns>Объект доменной накладной.</returns>
        private static DomainEntities.Waybill ConvertWaybillToDomain(XEntities.Waybill xWaybill)
        {
            repository = new Repository(App.ConnectionManager.Connector);

            DomainEntities.Header dHeader = new DomainEntities.Header
            {
                Buyer = repository.GetCounteragent(Requisites.GLN_Counteragent, xWaybill.Header.BuyerGln),
                DeliveryPlace = repository.GetWareHouse(Requisites.GLN_WareHouse, xWaybill.Header.DeliveryPlace),
                Supplier = repository.GetCounteragent(Requisites.GLN_Counteragent, xWaybill.Header.SupplierGln),
            };

            foreach (var item in xWaybill.Header.Positions)
            {
                dHeader.Positions.Add(new DomainEntities.WarePosition(item));
            }

            DomainEntities.Waybill waybill = new DomainEntities.Waybill(xWaybill.Number, xWaybill.Date, dHeader);

            return waybill;
        }

        /// <summary>
        /// Список необработанных накладных.
        /// </summary>
        public static List<DomainEntities.Waybill> UnprocessedWaybills { get; private set; } = new List<DomainEntities.Waybill>();

        private static Repository repository;
    }
}
