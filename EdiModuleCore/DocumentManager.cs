namespace EdiModuleCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Bridge1C;
    using Model;
    using Exceptions;

    public enum DocumentTypes
    {
        DESADV
    }

    /// <summary>
    /// Менеджер документов.
    /// </summary>
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
            if (expectedDocType.ToString() != Parser.GetDocumentTypeName(fileContent))
                return null;

            return Parser.GetWaybill(fileContent);
        }

        /// <summary>
        /// Загрузить накладную.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>true, если успешно, иначе false.</returns>
        public static bool DownloadWaybill(string fileContent, string fileName)
        {
            XEntities.Waybill waybill = DocumentManager.DownloadDocument(fileContent, DocumentTypes.DESADV);

            if (waybill == null)
                return false;

            CoreInit.ModuleRepository.AddWaybill(DocumentManager.ConvertWaybillToDomain(waybill, fileName));
            return true;
        }

        /// <summary>
        /// Загрузить все накладные из указанной в настройках папки.
        /// </summary>
        public static void DownloadWaybills()
        {
            string[] fileNames = FileService.GetFileList(SessionManager.Sessions[0].WorkFolder);

            foreach (var item in fileNames)
                DocumentManager.DownloadWaybill(FileService.ReadTextFile(item), item);
        }

        /// <summary>
        /// Конвертирует загруженную из XML накладную в доменную накладную.
        /// </summary>
        /// <param name="xWaybill">Загруженная накладная.</param>
        /// <param name="fileName">Имя файла накладной.</param>
        /// <returns>Объект доменной накладной.</returns>
        private static Model.Waybill ConvertWaybillToDomain(XEntities.Waybill xWaybill, string fileName)
        {
            Model.Waybill result = new Model.Waybill
            {
                Number = xWaybill.Number,
                Date = xWaybill.Date,
                Supplier = CoreInit.RepositoryService.GetCounteragent(Requisites.GLN, xWaybill.Header.SupplierGln),
                Warehouse = CoreInit.RepositoryService.GetWarehouse(Requisites.GLN, xWaybill.Header.DeliveryPlace),
                FileName = fileName,
                Wares = new List<Model.WaybillRow>()
            };
            result.Organization = CoreInit.RepositoryService.GetOrganization(result.Warehouse?.Code);

            foreach (var item in xWaybill.Header.Positions)
            {
                Model.WaybillRow row = new Model.WaybillRow();
                row.Amount = (decimal)item.Amount;
                row.Price = (decimal)item.Price;
                row.Count = item.Quantity;
                ExWare exWare = new ExWare
                {
                    Supplier = result.Supplier,
                    Barcode = item.Barcode,
                    Code = item.WareSupplierCode,
                    Name = item.WareName,
                    Unit = CoreInit.RepositoryService.GetUnit(Requisites.InternationalReduction_Unit, item.Unit)
                };

                MatchedWare ware = null;

                try
                {
                    ware = MatchingModule.AutomaticMatching(exWare);
                }
                catch (NotMatchedException)
                {
                    ware = new MatchedWare
                    {
                        ExWare = exWare
                    };
                }

                var test = CoreInit.ModuleRepository.GetMatchedWares().FirstOrDefault(w => w.Equals(ware));

                if(test != null)
                {
                    row.Ware = test;
                }
                else
                {
                    row.Ware = ware;
                    CoreInit.ModuleRepository.AddMatchedWare(ware);
                }

                result.Wares.Add(row);
            }

            return result;
        }

        /// <summary>
        /// Сохраняет в базу данных накладную.
        /// </summary>
        /// <returns>true, если успешно, иначе false.</returns>
        private static bool SaveWaybillToBase(Waybill waybill)
        {
            try
            {
                if (waybill == null)
                    return false;

                Bridge1C.DomainEntities.Waybill domainWaybill = new Bridge1C.DomainEntities.Waybill();
                domainWaybill.Number = waybill.Number;
                domainWaybill.Date = waybill.Date;
                domainWaybill.Supplier = waybill.Supplier;
                domainWaybill.Warehouse = waybill.Warehouse;
                domainWaybill.Organization = waybill.Organization;
                domainWaybill.Positions = new List<Bridge1C.DomainEntities.WaybillRow>();

                foreach (var item in waybill.Wares)
                {
                    domainWaybill.Positions.Add(new Bridge1C.DomainEntities.WaybillRow
                    {
                        Ware = item.Ware.InnerWare,
                        Unit = item.Ware.ExWare.Unit,
                        Count = item.Count,
                        Price = item.Price,
                        TaxAmount = item.TaxAmount,
                        TaxRate = item.TaxRate
                    });
                }

                return CoreInit.RepositoryService.AddNewWaybill(domainWaybill);
            }
            catch(Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Обработать накладную (записать в базу и удалить из необработанных).
        /// </summary>
        /// <returns>true, если успешно, иначе false.</returns>
        public static void ProcessWaybill(Waybill waybill)
        {
            if (!FileService.MoveFile(waybill.FileName, System.IO.Path.GetFullPath(SessionManager.Sessions[0].ArchieveFolder)))
                throw new NotProcessedDocumentException("Не удалось переместить файл накладной в архив.");

            if (!CoreInit.ModuleRepository.RemoveUnprocessedWaybill(waybill))
                throw new NotProcessedDocumentException("Возникла внутренняя ошибка при обработке накладной.");

            if (!DocumentManager.SaveWaybillToBase(waybill))
                throw new NotProcessedDocumentException("Не удалось загрузить накладную в базу.");
        }
    }
}
