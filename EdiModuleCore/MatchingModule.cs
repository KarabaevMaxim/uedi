﻿namespace EdiModuleCore
{
	using System;
    using System.Collections.Generic;
	using System.Threading.Tasks;
    using DAL;
    using DAL.DomainEntities;
    using DAL.DomainEntities.Spr;
    using Model;
    using Exceptions;
	using NLog;
	using Newtonsoft.Json;

    public static class MatchingModule
    {
        public static MatchedWare ManualMatching(Ware ware, string exCode, MatchedCounteragent supplier)
        {
			if (ware == null)
				throw new ArgumentNullException("ware");

			if (string.IsNullOrWhiteSpace(exCode))
				throw new ArgumentNullException("exCode");

			if (supplier == null)
				throw new ArgumentNullException("supplier");

			MatchedWare result = new MatchedWare();
            result.InnerWare = ware;
            result.ExWare = new ExWare { Code = exCode, Supplier = supplier, Name = ware.Name };

			CoreInit.RepositoryService.RemoveExCode(new WareExCode { Value = exCode, Counteragent = supplier.InnerCounteragent });

            if (!CoreInit.RepositoryService.AddNewExCodeToWare(result.InnerWare,
                                        new WareExCode { Counteragent = result.ExWare.Supplier.InnerCounteragent, Value = result.ExWare.Code }))
                throw new NotMatchedException("Сопоставление не выполнено, не удалось добавить внешний код в базу.");

            return result;
        }

        public static MatchedWare ManualMatching(Ware ware, ExWare exWare)
        {
            if (ware == null)
                throw new ArgumentNullException("ware");

			if (exWare == null)
				throw new ArgumentNullException("exWare");

			if (exWare.Supplier == null)
				throw new ArgumentNullException("exWare.Supplier");

			if (string.IsNullOrWhiteSpace(exWare.Code))
				throw new ArgumentNullException("exWare.Code");

			MatchedWare result = new MatchedWare();
            result.InnerWare = ware;
            result.ExWare = exWare;

			CoreInit.RepositoryService.RemoveExCode(new WareExCode { Value = exWare.Code, Counteragent = exWare.Supplier.InnerCounteragent });

			if (!CoreInit.RepositoryService.AddNewExCodeToWare(result.InnerWare,
                                        new WareExCode { Counteragent = result.ExWare.Supplier.InnerCounteragent, Value = result.ExWare.Code }))
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
			if (ware == null)
				throw new ArgumentNullException("ware");

			if (matchedWare == null)
				throw new ArgumentNullException("matchedWare");

			if(string.IsNullOrWhiteSpace(ware.Code))
				throw new ArgumentNullException("ware.Code");

			matchedWare.InnerWare = ware;
			CoreInit.RepositoryService.RemoveExCode(new WareExCode { Value = matchedWare.ExWare.Code, Counteragent = matchedWare.ExWare.Supplier.InnerCounteragent });

			if (!CoreInit.RepositoryService.AddNewExCodeToWare(matchedWare.InnerWare,
										new WareExCode { Counteragent = matchedWare.ExWare.Supplier.InnerCounteragent, Value = matchedWare.ExWare.Code }))
				throw new NotMatchedException("Сопоставление не выполнено, не удалось добавить внешний код в базу.");
		}

		/// <summary>
		/// Выполняет поиск номенклатуры в базе по внешнему коду и сопоставляет с ней, если найдена.
		/// </summary>
		/// <param name="exWare">Объект внешней номенклатуры.</param>
		public static MatchedWare AutomaticMatching(ExWare exWare)
        {
			if (exWare == null)
				throw new ArgumentNullException("exWare");

            MatchedWare result = new MatchedWare();
            result.InnerWare = CoreInit.RepositoryService.GetWare(Requisites.ExCode_Ware, exWare.Code, exWare.Supplier?.ExCounteragent?.GLN); //todo: Валится исключение, если не найден поставщик

            if(result.InnerWare == null)
                throw new NotMatchedException("Автоматическое сопоставление не выполнено, по внешнему коду номенклатура не найдена");

            result.ExWare = exWare;

			return result;
        }

        /// <summary>
        /// Создает номенклатуру в базе и выполняет с ней сопоставление.
        /// </summary>
        /// <param name="matchedWare">Объект, где должно быть выполнено сопоставление.</param>
        public static void CreateNewInnerWareAndMatch(MatchedWare matchedWare)
        {
			if (matchedWare == null)
				throw new ArgumentNullException("matchedWare");

			if(matchedWare.ExWare == null)
				throw new ArgumentNullException("matchedWare.ExWare");

			if(matchedWare.InnerWare != null)
				throw new ArgumentNullException("matchedWare.InnerWare");

			if (matchedWare.ExWare.Supplier?.InnerCounteragent == null)
				throw new NotMatchedException("Автоматическое добавление номенклатуры не выполнено, поставщик не указан");

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
                    Counteragent = matchedWare.ExWare.Supplier.InnerCounteragent,
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
			if (matchedWare == null)
				throw new ArgumentNullException("matchedWare");

			if(matchedWare.ExWare == null)
				throw new ArgumentNullException("matchedWare.ExWare");

			matchedWare.InnerWare = CoreInit.RepositoryService.GetWare(Requisites.ExCode_Ware, matchedWare.ExWare.Code, matchedWare.ExWare.Supplier.InnerCounteragent.Code);

            if(matchedWare.InnerWare == null)
                throw new NotMatchedException("Автоматическое сопоставление не выполнено, по внешнему коду номенклатура не найдена.");
        }

		/// <summary>
		/// Автоматическое сопоставление складов по ГЛН.
		/// </summary>
		/// <param name="warehouse">Склад, который нужно сопоставить.</param>
		/// <returns>Объект сопоставленного склада.</returns>
		public static MatchedWarehouse AutomaticWHMatching(ExWarehouse warehouse)
		{
			if (warehouse == null || string.IsNullOrWhiteSpace(warehouse.GLN))
				throw new ArgumentNullException("warehouse");

			if (string.IsNullOrWhiteSpace(warehouse.GLN))
				throw new ArgumentNullException("warehouse.GLN");

			var cache = CoreInit.RepositoryService.GetWarehouse(Requisites.GLN, warehouse.GLN);

			MatchedWarehouse result = new MatchedWarehouse { ExWarehouse = warehouse, InnerWarehouse = cache };
			return result;
		}

		public static void UpdateWHMatching(MatchedWarehouse warehouse)
		{
			if (warehouse == null || string.IsNullOrWhiteSpace(warehouse.ExWarehouse?.GLN))
				throw new ArgumentNullException("warehouse");

			if (string.IsNullOrWhiteSpace(warehouse.ExWarehouse?.GLN))
				throw new ArgumentNullException("warehouse.ExWarehouse?.GLN");

			var cache = CoreInit.RepositoryService.GetWarehouse(Requisites.GLN, warehouse.ExWarehouse?.GLN);


			warehouse.InnerWarehouse = cache;
		}

		public static void UpdateWHMatching(IEnumerable<MatchedWarehouse> warehouse)
		{
			foreach (var item in warehouse)
				MatchingModule.UpdateWHMatching(item);
		}

		/// <summary>
		/// Выполняет сопоставление склада matchedWarehouse с warehouse.
		/// </summary>
		/// <param name="matchedWarehouse">Объект сопоставленного склада, с инициализированным объектом внешнего склада.</param>
		/// <param name="warehouse">Объекта склада из базы данных.</param>
		public static bool ManualWHMatching(MatchedWarehouse matchedWarehouse, Warehouse warehouse)
		{
			if (warehouse == null)
				throw new ArgumentNullException("matchedWarehouse");

			if (warehouse == null)
				throw new ArgumentNullException("warehouse");

			if (!CoreInit.RepositoryService.RematchingWarehouse(warehouse.Code, matchedWarehouse.ExWarehouse.GLN))
				throw new NotMatchedException("Сопоставление не выполнено, не удалось записать ГЛН в базу.");

			matchedWarehouse.InnerWarehouse = warehouse;

			return true;
        }

		/// <summary>
		/// Асинхронно выполняет сопоставление склада matchedWarehouse с warehouse.
		/// </summary>
		/// <param name="matchedWarehouse">Объект сопоставленного склада, с инициализированным объектом внешнего склада.</param>
		/// <param name="warehouse">Объекта склада из базы данных.</param>
		public async static Task<bool> ManualWHMatchingAsync(MatchedWarehouse matchedWarehouse, Warehouse warehouse)
		{
			return await Task.Run(() => ManualWHMatching(matchedWarehouse, warehouse));
		}

		public static MatchedCounteragent AutomaticSupMatching(ExCounteragent counteragent)
		{
			if (counteragent == null || string.IsNullOrWhiteSpace(counteragent.GLN))
				return null;

			MatchedCounteragent result = new MatchedCounteragent
			{
				ExCounteragent = counteragent,
				InnerCounteragent = null
			};

			var innerCount = CoreInit.RepositoryService.GetCounteragent(Requisites.GLN, counteragent.GLN);

			if (innerCount == null || string.IsNullOrWhiteSpace(innerCount.Code))
				return result;

			result.InnerCounteragent = innerCount;
			return result;
		}

		public static void UpdateSupMatching(MatchedCounteragent counteragent)
		{
			if (counteragent == null || string.IsNullOrWhiteSpace(counteragent.ExCounteragent?.GLN))
				throw new ArgumentNullException("Передан пустой параметр");

			var cache = CoreInit.RepositoryService.GetCounteragent(Requisites.GLN, counteragent.ExCounteragent?.GLN);

			if (cache == null || string.IsNullOrWhiteSpace(cache.Code))
				counteragent.InnerCounteragent = null;

			counteragent.InnerCounteragent = cache;
		}

		public static void UpdateSupMatching(IEnumerable<MatchedCounteragent> counteragents)
		{
			foreach (var item in counteragents)
				MatchingModule.UpdateSupMatching(item);
		}

		public async static Task<MatchedCounteragent> AutomaticSupMatchingAsync(ExCounteragent counteragent)
		{
			return await Task.Run(() => AutomaticSupMatching(counteragent));
		}

		public static bool ManualSupMatching(MatchedCounteragent matchedCounteragent, Counteragent counteragent)
		{
			if (matchedCounteragent == null)
				throw new ArgumentNullException("matchedCounteragent");

			if (counteragent == null)
				throw new ArgumentNullException("counteragent");

			if(!CoreInit.RepositoryService.RematchingCounteragent(counteragent, matchedCounteragent.ExCounteragent.GLN))
				throw new NotMatchedException("Сопоставление не выполнено, не удалось записать ГЛН в базу.");

			counteragent.GLN = matchedCounteragent.ExCounteragent.GLN;
			matchedCounteragent.InnerCounteragent = counteragent;
			return true;
		}
	}
}
