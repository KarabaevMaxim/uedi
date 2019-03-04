namespace EdiModuleCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
	using System.Threading.Tasks;
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
        /// <param name="fileName">Имя файла.</param>s
        /// <returns>true, если успешно, иначе false.</returns>
        public static void DownloadWaybill(string fileContent, string fileName)
        {
            XEntities.Waybill waybill = DocumentManager.DownloadDocument(fileContent, DocumentTypes.DESADV);

            if (waybill == null)
                return;

			Waybill domainWaybill = DocumentManager.ConvertWaybillToDomain(waybill, fileName);
			CoreInit.ModuleRepository.AddWaybill(domainWaybill);
        }

        /// <summary>
        /// Загрузить все накладные из указанной в настройках папки.
        /// </summary>
        public static bool DownloadWaybills(string workFolder)
		{ 
            string[] fileNames = FileService.GetFileList(workFolder);

            foreach (var item in fileNames)
                DocumentManager.DownloadWaybill(FileService.ReadTextFile(item), item);

			return true;
        }

		public async static Task<bool> DownloadWaybillsAsync(string workFolder)
		{
			return await Task.Run(() => DocumentManager.DownloadWaybills(workFolder));
		}

		private static Model.Waybill RaiseWaybill(XEntities.Waybill xWaybill, string fileName, MatchedWarehouse matchedWarehouse, MatchedCounteragent matchedCounteragent)
		{
			Model.Waybill result = new Model.Waybill
			{
				Number = xWaybill.Number,
				Date = xWaybill.Date,
				Supplier = matchedCounteragent,
				Warehouse = matchedWarehouse,
				FileName = fileName,
				Wares = new List<Model.WaybillRow>()
			};
			result.Organization = CoreInit.RepositoryService.GetOrganization(Requisites.GLN, xWaybill.Header.BuyerGln);

			foreach (var item in xWaybill.Header.Positions)
			{
				Model.WaybillRow row = new Model.WaybillRow();
				row.Amount = (decimal)item.Amount;
				row.Price = (decimal)item.Price;
				row.Count = item.Quantity;
				row.TaxRate = item.TaxRate;
				row.TaxAmount = (row.Price * (1.0m / row.TaxRate)) * (decimal)row.Count;

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

				if (test != null)
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

			result.Amount = (float)result.Wares.Sum(w => w.Amount);
			result.AmountWithTax = (float)result.Wares.Sum(w => (w.Amount + w.TaxAmount));
			return result;
		}

		private static Model.Waybill ConvertWaybillToDomain(XEntities.Waybill xWaybill, string fileName)
		{
			var warehouse = MatchingModule.AutomaticWHMatching(new ExWarehouse { GLN = xWaybill.Header.SupplierGln });
			var supplier = MatchingModule.AutomaticSupMatching(new ExCounteragent { GLN = xWaybill.Header.SupplierGln });
			return DocumentManager.RaiseWaybill(xWaybill, fileName, warehouse, supplier);
		}

		/// <summary>
		/// Конвертирует загруженную из XML накладную в доменную накладную.
		/// </summary>
		/// <param name="xWaybill">Загруженная накладная.</param>
		/// <param name="fileName">Имя файла накладной.</param>
		/// <returns>Объект доменной накладной.</returns>
		private async static Task<Model.Waybill> ConvertWaybillToDomainAsync(XEntities.Waybill xWaybill, string fileName)
        {
			var warehouse = await MatchingModule.AutomaticWHMatchingAsync(new ExWarehouse { GLN = xWaybill.Header.SupplierGln });
			var supplier = await MatchingModule.AutomaticSupMatchingAsync(new ExCounteragent { GLN = xWaybill.Header.SupplierGln });

			return DocumentManager.RaiseWaybill(xWaybill, fileName, warehouse, supplier);
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
                domainWaybill.Supplier = waybill.Supplier?.InnerCounteragent;
                domainWaybill.Warehouse = waybill.Warehouse?.InnerWarehouse;
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
			if (waybill == null)
				throw new NotProcessedDocumentException("Ссылка на накладную пустая.");

			if (waybill.Wares == null || !waybill.Wares.Any())
				throw new NotProcessedDocumentException("Товаров в накладной нет.");

			if (waybill.Wares.Where(w => w.Ware.InnerWare == null).Any())
				throw new NotProcessedDocumentException("В накладной присутствуют несопоставленные позиции.");

			if (waybill.Organization == null)
				throw new NotProcessedDocumentException("В накладной не указана организация.");

			if (waybill.Supplier?.InnerCounteragent == null)
				throw new NotProcessedDocumentException("В накладной не сопоставлен контрагент.");

			if (waybill.Warehouse?.InnerWarehouse == null)
				throw new NotProcessedDocumentException("В накладной не сопоставлен склад.");

			//todo: не забыть раскомментить
			//if (!FileService.MoveFile(waybill.FileName, System.IO.Path.GetFullPath(SessionManager.Sessions[0].ArchieveFolder)))
			//    throw new NotProcessedDocumentException("Не удалось переместить файл накладной в архив.");

			if (!CoreInit.ModuleRepository.RemoveUnprocessedWaybill(waybill))
                throw new NotProcessedDocumentException("Возникла внутренняя ошибка при обработке накладной.");

            if (!DocumentManager.SaveWaybillToBase(waybill))
                throw new NotProcessedDocumentException("Не удалось загрузить накладную в базу.");
        }
    }
}
