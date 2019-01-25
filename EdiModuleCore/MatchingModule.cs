namespace EdiModuleCore
{
    using System.Collections.Generic;
    using Bridge1C;
    using Bridge1C.DomainEntities;
    using Model;
    using Exceptions;

    public static class MatchingModule
    {
        public static MatchedWare ManualMatching(Ware ware, string exCode, Counteragent supplier)
        {
            if (ware == null || supplier == null || string.IsNullOrWhiteSpace(exCode))
                 throw new NotMatchedException("Сопоставление не выполнено, внутренний или внешний товар не указан.");

            MatchedWare result = new MatchedWare();
            result.InnerWare = ware;
            result.ExWare = new ExWare { Code = exCode, Supplier = supplier, Name = ware.Name };

            if (!CoreInit.RepositoryService.AddNewExCodeToWare(result.InnerWare,
                                        new WareExCode { Counteragent = result.ExWare.Supplier, Value = result.ExWare.Code }))
                throw new NotMatchedException("Сопоставление не выполнено, не удалось добавить внешний код в базу.");

            return result;
        }

        public static MatchedWare ManualMatching(Ware ware, ExWare exWare)
        {
            if (ware == null || exWare == null)
                throw new NotMatchedException("Сопоставление не выполнено, внутренний или внешний товар не указан.");

            MatchedWare result = new MatchedWare();
            result.InnerWare = ware;
            result.ExWare = exWare;

            if (!CoreInit.RepositoryService.AddNewExCodeToWare(result.InnerWare,
                                        new WareExCode { Counteragent = result.ExWare.Supplier, Value = result.ExWare.Code }))
                throw new NotMatchedException("Сопоставление не выполнено, не удалось добавить внешний код в базу.");

            return result;
        }

        /// <summary>
        /// Вносит запись о сопоставлении указанной номенклатуры ware в matchedWare.
        /// </summary>
        /// <param name="ware">Номенклатура для сопоставления.</param>
        /// <param name="matchedWare">Объект, где должно быть выполнено сопоставление.</param>
        public static void ManualMatching(Ware ware, MatchedWare matchedWare)
        {
            if (ware == null || matchedWare == null)
                throw new NotMatchedException("Сопоставление не выполнено, внутренний или товар для сопоставления не указан.");

            matchedWare.InnerWare = ware;

            if (!CoreInit.RepositoryService.AddNewExCodeToWare(matchedWare.InnerWare,
                                        new WareExCode { Counteragent = matchedWare.ExWare.Supplier, Value = matchedWare.ExWare.Code }))
                throw new NotMatchedException("Сопоставление не выполнено, не удалось добавить внешний код в базу.");
        }

        /// <summary>
        /// Выполняет поиск номенклатуры в базе по внешнему коду и сопоставляет с ней, если найдена.
        /// </summary>
        /// <param name="exWare">Объект внешней номенклатуры.</param>
        public static MatchedWare AutomaticMatching(ExWare exWare)
        {
            if (exWare == null)
                return null;

            MatchedWare result = new MatchedWare();
            result.InnerWare = CoreInit.RepositoryService.GetWare(Requisites.ExCode_Ware, exWare.Code, exWare.Supplier?.Code); //todo: Валится исключение, если не найден поставщик

            if(result.InnerWare == null)
                throw new NotMatchedException("Автоматическое сопоставление не выполнено, по внешнему коду номенклатура не найдена.");

            result.ExWare = exWare;

            return result;
        }

        /// <summary>
        /// Создает наменклатуру в базе и выполняет с ней сопоставление.
        /// </summary>
        /// <param name="matchedWare">Объект, где должно быть выполнено сопоставление.</param>
        public static void CreateNewInnerWareAndMatch(MatchedWare matchedWare)
        {
            if(matchedWare.ExWare == null || matchedWare.InnerWare != null)
                throw new NotMatchedException("Автоматическое добавление номенклатуры не выполнено, внешний товар не инициализирован или " +
                    "инициализирован внутренний.");

            Ware newInnerWare = new Ware();
            newInnerWare.Name = matchedWare.ExWare.Name;
            newInnerWare.FullName = matchedWare.ExWare.Name;
            newInnerWare.Unit = matchedWare.ExWare.Unit;
            newInnerWare.Name = matchedWare.ExWare.Name;
            newInnerWare.BarCodes = new List<string> { matchedWare.ExWare.Barcode };
            newInnerWare.ExCodes = new List<WareExCode>
            {
                new WareExCode
                {
                    Counteragent = matchedWare.ExWare.Supplier,
                    Value = matchedWare.ExWare.Code
                }
            };

            if (CoreInit.RepositoryService.AddNewWare(newInnerWare))
                matchedWare.InnerWare = newInnerWare;
            else
                throw new NotMatchedException("Автоматическое добавление номенклатуры не выполнено, произошла внутренняя ошибка.");
        }

        public static void UpdateMatching(MatchedWare matchedWare)
        {
            if (matchedWare == null || matchedWare.ExWare == null)
                return;

            matchedWare.InnerWare = CoreInit.RepositoryService.GetWare(Requisites.ExCode_Ware, matchedWare.ExWare.Code, matchedWare.ExWare.Supplier.Code);

            if(matchedWare.InnerWare == null)
                throw new NotMatchedException("Автоматическое сопоставление не выполнено, по внешнему коду номенклатура не найдена.");
        }

		public static Warehouse AutomaticWHMatching(Warehouse warehouse)
		{
			if (warehouse == null || !string.IsNullOrWhiteSpace(warehouse.Code) || string.IsNullOrWhiteSpace(warehouse.GLN))
				return warehouse;

			var result = CoreInit.RepositoryService.GetWarehouse(Requisites.GLN, warehouse.GLN);

			if(string.IsNullOrWhiteSpace(result.Code))
				return warehouse;

			return result;
		}

		/// <summary>
		/// Сопоставляет склад1 со складом2.
		/// </summary>
		/// <param name="warehouse1">Склад из базы (должен содержать код и наименование).</param>
		/// <param name="warehouse2">Склад из накладной (должен содержать ГЛН).</param>
		/// <returns>Сопоставленный склад.</returns>
		public static Warehouse ManualWHMatching(Warehouse warehouse1, Warehouse warehouse2)
		{
			if (warehouse1 == null
					|| warehouse2 == null
					|| string.IsNullOrWhiteSpace(warehouse1.Code)
					|| string.IsNullOrWhiteSpace(warehouse1.Name)
					|| string.IsNullOrWhiteSpace(warehouse2.GLN))
				return null;

			Warehouse result = new Warehouse
			{
				Code = warehouse1.Code,
				Name = warehouse1.Name,
				GLN = warehouse2.GLN
			};

			if (!CoreInit.RepositoryService.UpdateWarehouseGLN(result.Code, result.GLN))
			{
				throw new NotMatchedException("Сопоставление не выполнено, не удалось записать ГЛН в базу.");
			}
			

			return result;
		}

		public static Counteragent AutomaticSupMatching(Counteragent counteragent)
		{
			if (counteragent == null || !string.IsNullOrWhiteSpace(counteragent.Code) || string.IsNullOrWhiteSpace(counteragent.GLN))
				return counteragent;

			var result = CoreInit.RepositoryService.GetCounteragent(Requisites.GLN, counteragent.GLN);

			if (string.IsNullOrWhiteSpace(result.Code))
				return counteragent;

			return result;
		}
	}
}
