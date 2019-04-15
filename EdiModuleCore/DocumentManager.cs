namespace EdiModuleCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
	using System.Threading.Tasks;
    using Bridge1C;
    using Model;
    using Exceptions;
	using NLog;
	using Newtonsoft.Json;

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
        public static XEntities.IXEntity DownloadDocument(string fileContent, DocumentTypes expectedDocType)
        {
			DocumentManager.logger.Info("Сериализация строки {0} в документ с типом {1}", fileContent, expectedDocType);

			if (string.IsNullOrWhiteSpace(fileContent))
				throw new ArgumentNullException("fileContent");

			XEntities.IXEntity result = null;

			switch (expectedDocType)
			{
				case DocumentTypes.DESADV:
					if (expectedDocType.ToString() != Parser.GetDocumentTypeName(fileContent))
						throw new ArgumentOutOfRangeException("Ожидаемый тип документа expectedDocType не совпадает с содержимым XML fileContent");

					result = Parser.GetWaybill(fileContent);
					break;
				default:
					break;
			}

			if(result == null)
				DocumentManager.logger.Error("Не удалось десериализовать документ");
			else
				DocumentManager.logger.Info("Документ с номером {0} от {1} десериализован", result.Number, result.Date);

			return result;
        }

        /// <summary>
        /// Загрузить накладную.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>s
        /// <returns>true, если успешно, иначе false.</returns>
        public static void DownloadWaybill(string fileContent, string fileName)
        {
			DocumentManager.logger.Info("Загрузка накладной из файла {0}", fileName);

			if (string.IsNullOrWhiteSpace(fileContent) || string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentNullException("fileContent или fileName");

            XEntities.Waybill waybill = (XEntities.Waybill)DocumentManager.DownloadDocument(fileContent, DocumentTypes.DESADV);

			Waybill domainWaybill = DocumentManager.ConvertWaybillToDomain(waybill, fileName);
			CoreInit.ModuleRepository.AddWaybill(domainWaybill);
			DocumentManager.logger.Info("Загрузка накладной завершена");
		}

        /// <summary>
        /// Загрузить все накладные из указанной в настройках папки.
        /// </summary>
        public static void DownloadWaybills(string workFolder)
		{
			DocumentManager.logger.Info("Загрузка всех накладных из папки {0}", workFolder);

			if (string.IsNullOrWhiteSpace(workFolder))
				throw new ArgumentNullException("workFolder");

            string[] fileNames = FileService.GetFileList(workFolder);

            foreach (var item in fileNames)
                DocumentManager.DownloadWaybill(FileService.ReadTextFile(item), item);

			DocumentManager.logger.Info("Загрузка всех накладных завершена", workFolder);
		}

		public static void ReloadWaybills(string workFolder)
		{
			DocumentManager.logger.Info("Перезагрузка всех накладных из папки {0}", workFolder);

			if (string.IsNullOrWhiteSpace(workFolder))
				throw new ArgumentNullException("workFolder");

			CoreInit.ModuleRepository.ClearWaybillLists();
			DocumentManager.DownloadWaybills(workFolder);
			DocumentManager.logger.Info("Перезагрузка завершена");
		}

		private static Model.Waybill RaiseWaybill(XEntities.Waybill xWaybill, string fileName, MatchedWarehouse matchedWarehouse, MatchedCounteragent matchedCounteragent)
		{
			DocumentManager.logger.Info("Заполнение реквизитов доменной накладной");

			if (xWaybill == null)
				throw new ArgumentNullException("xWaybill");

			if(string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentNullException("fileName");

			if (matchedWarehouse == null)
				throw new ArgumentNullException("matchedWarehouse");

			if (matchedCounteragent == null)
				throw new ArgumentNullException("matchedCounteragent");

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
				DocumentManager.logger.Info("Добавление строки {0} в накладную", JsonConvert.SerializeObject(item));
				Model.WaybillRow row = new Model.WaybillRow();
				row.Amount = (decimal)item.Amount;
				row.Price = (decimal)item.Price;
				row.Count = item.Quantity;
				row.TaxRate = item.TaxRate;
				row.TaxAmount = Math.Round((row.Price * (1.0m / row.TaxRate)) * (decimal)row.Count, 2);

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
					DocumentManager.logger.Info("Автоматическое сопоставление выполнено. Результат: {0}", JsonConvert.SerializeObject(ware));
				}
				catch (NotMatchedException ex)
				{
					DocumentManager.logger.Warn(ex, "Автоматическое сопоставление не выполнено");
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
				DocumentManager.logger.Info("Строка добавлена в накладную");
			}

			result.Amount = (float)result.Wares.Sum(w => w.Amount);
			result.AmountWithTax = (float)result.Wares.Sum(w => (w.Amount + w.TaxAmount));
			DocumentManager.logger.Info("Заполнение реквизитов доменной накладной завершено");
			return result;
		}

		private static Model.Waybill ConvertWaybillToDomain(XEntities.Waybill xWaybill, string fileName)
		{
			DocumentManager.logger.Info("Конвертация файловой накладной {0} в доменную", JsonConvert.SerializeObject(xWaybill));

			if (xWaybill == null)
				throw new ArgumentNullException("xWaybill");

			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentNullException("fileName");

			var warehouse = MatchingModule.AutomaticWHMatching(new ExWarehouse { GLN = xWaybill.Header.DeliveryPlace });

			if(warehouse.InnerWarehouse == null)
				DocumentManager.logger.Warn("Автоматическое складов сопоставление не выполнено. Результат: {0}", JsonConvert.SerializeObject(warehouse));
			else
				DocumentManager.logger.Info("Автоматическое сопоставление складов выполнено. Результат: {0}", JsonConvert.SerializeObject(warehouse));

			var supplier = MatchingModule.AutomaticSupMatching(new ExCounteragent { GLN = xWaybill.Header.SupplierGln });
			Model.Waybill result = DocumentManager.RaiseWaybill(xWaybill, fileName, warehouse, supplier);
			
			if(result == null)
				DocumentManager.logger.Error("Конвертация файловой накладной в доменную не проведена");
			else
				DocumentManager.logger.Info("Конвертация файловой накладной в доменную завершена");
			return result;
		}

		/// <summary>
		/// Сохраняет в базу данных накладную.
		/// </summary>
		/// <returns>true, если успешно, иначе false.</returns>
		private static bool SaveWaybillToBase(Waybill waybill)
        {
			DocumentManager.logger.Info("Сохранение накладной {0} в базу данных", JsonConvert.SerializeObject(waybill));

			if (waybill == null)
				throw new ArgumentNullException("waybill");

			try
            {
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

				bool result = CoreInit.RepositoryService.AddNewWaybill(domainWaybill);

				if(result == false)
					DocumentManager.logger.Warn("Накладная не загружена в базу");
				else
					DocumentManager.logger.Info("Накладная загружена в базу");

				return result;
            }
            catch(Exception ex)
            {
				DocumentManager.logger.Error(ex, "Накладная не загружена в базу");
				throw ex;
            }
		}

        /// <summary>
        /// Обработать накладную (записать в базу и удалить из необработанных).
        /// </summary>
        /// <returns>true, если успешно, иначе false.</returns>
        public static void ProcessWaybill(Waybill waybill)
        {
			DocumentManager.logger.Info("Обработка накладной");

			if (waybill == null)
				throw new ArgumentNullException("waybill");

			if (waybill.Wares == null || !waybill.Wares.Any())
			{
				DocumentManager.logger.Error("В накладной нет строк товара. Накладная не загружена.");
				throw new NotProcessedDocumentException("В накладной нет строк товара");
			}
				
			if (waybill.Wares.Where(w => w.Ware.InnerWare == null).Any())
			{
				DocumentManager.logger.Error("В накладной присутствуют несопоставленные позиции. Накладная не загружена.");
				throw new NotProcessedDocumentException("В накладной присутствуют несопоставленные позиции.");
			}

			if (waybill.Organization == null)
			{
				DocumentManager.logger.Error("В накладной не указана организация. Накладная не загружена.");
				throw new NotProcessedDocumentException("В накладной не указана организация.");
			}

			if (waybill.Supplier?.InnerCounteragent == null)
			{
				DocumentManager.logger.Error("В накладной не сопоставлен контрагент. Накладная не загружена.");
				throw new NotProcessedDocumentException("В накладной не сопоставлен контрагент.");
			}

			if (waybill.Warehouse?.InnerWarehouse == null)
			{
				DocumentManager.logger.Error("В накладной не сопоставлен склад. Накладная не загружена.");
				throw new NotProcessedDocumentException("В накладной не сопоставлен склад.");
			}

			//todo: не забыть раскомментить
			if (!FileService.MoveFile(waybill.FileName, System.IO.Path.GetFullPath(SessionManager.Sessions[0].ArchieveFolder)))
			{
				DocumentManager.logger.Error("Не удалось переместить файл накладной в архив. Накладная не загружена.");
				throw new NotProcessedDocumentException("Не удалось переместить файл накладной в архив.");
			}

			if (!CoreInit.ModuleRepository.RemoveUnprocessedWaybill(waybill))
			{
				DocumentManager.logger.Error("Возникла внутренняя ошибка при обработке накладной. Накладная не загружена.");
				throw new NotProcessedDocumentException("Возникла внутренняя ошибка при обработке накладной.");
			}

			if (!DocumentManager.SaveWaybillToBase(waybill))
			{
				DocumentManager.logger.Error("Не удалось загрузить накладную в базу. Накладная не загружена.");
				throw new NotProcessedDocumentException("Не удалось загрузить накладную в базу.");
			}

			DocumentManager.logger.Info("Обработка накладной завершена");
		}

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
	}
}
