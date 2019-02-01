namespace Bridge1C
{
    using System;
	using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    public class Repository : IRepository
    {
        /// <summary>
        /// Инициализирует объект в памяти.
        /// </summary>
        /// <param name="connector">Подключение к базе 1С.</param>
        public Repository(Connector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Получить контрагента.
        /// </summary>
        /// <param name="propertyName">Имя свойства, по которому будет производиться поиск.</param>
        /// <param name="propertyValue">Значение свойства.</param>
        /// <returns>Контрагент.</returns>
        public dynamic GetCounteragent(Requisites propertyName, string propertyValue)
        {
            try
            {
                dynamic counteragent = null;

                switch (propertyName)
                {
                    case Requisites.Code:
                        counteragent = this.Connector.Connection.Справочники.Контрагенты.НайтиПоКоду(propertyValue);
                        break;
                    case Requisites.Name:
                        counteragent = this.Connector.Connection.Справочники.Контрагенты.НайтиПоНаименованию(propertyValue);
                        break;
                    case Requisites.GLN:
						counteragent = this.Connector.Connection.Справочники.Контрагенты.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
						break;
                    default:
                        break;
                }
                return counteragent;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Получить справочник контрагентов.
        /// </summary>
        public List<dynamic> GetAllCounteragents()
        {
            try
            {
                List<dynamic> result = new List<dynamic>();
                dynamic выборка = this.Connector.Connection.Справочники.Контрагенты.Выбрать();

                while (выборка.Следующий())
                {
                    if (!выборка.ЭтоГруппа)
                    {
                        result.Add(выборка.Ссылка);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

		/// <summary>
		/// Инициализирует значением gln поле ГЛН контрагента с кодом counteragentCode, если перед этим в базе найден контрагент с 
		/// ГЛН gln, после будет переинициализировано пустой строкой.
		/// </summary>
		/// <param name="counteragentCode">Код контрагента для переициализации.</param>
		/// <param name="gln">ГНЛ.</param>
		/// <returns>true, если операция завершена успешно, иначе false.</returns>
		public bool RematchingCounteragent(string counteragentCode, string gln)
		{
			if (string.IsNullOrWhiteSpace(counteragentCode) || string.IsNullOrWhiteSpace(gln))
				return false;

			try
			{
				dynamic oldCounteragent = this.GetCounteragent(Requisites.GLN, gln);

				if (oldCounteragent != null && !string.IsNullOrWhiteSpace(oldCounteragent.Код))
				{
					dynamic oldCounteragentObj = oldCounteragent.ПолучитьОбъект();
					oldCounteragentObj.ГЛН = string.Empty;
					oldCounteragentObj.Записать();
				}
					

				dynamic newCounteragent = this.GetCounteragent(Requisites.Code, counteragentCode);

				if (newCounteragent == null || string.IsNullOrWhiteSpace(newCounteragent.Код))
					return false;

				dynamic newCounteragentObj = newCounteragent.ПолучитьОбъект();
				newCounteragentObj.ГЛН = gln;
				newCounteragentObj.Записать();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public dynamic GetWareHouse(Requisites propertyName, string propertyValue)
        {
            try
            {
                dynamic warehouse = null;

                switch (propertyName)
                {
                    case Requisites.Code:
                        warehouse = this.Connector.Connection.Справочники.Склады.НайтиПоКоду(propertyValue);
                        break;
                    case Requisites.Name:
                        warehouse = this.Connector.Connection.Справочники.Склады.НайтиПоНаименованию(propertyValue);
                        break;
                    default:
						warehouse = this.Connector.Connection.Справочники.Склады.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
                        break;
                }
                return warehouse;
            }
            catch(Exception)
            {
                return null;
            }
        }

		/// <summary>
		/// Получить справочник складов.
		/// </summary>
		/// <returns>Справочник складов.</returns>
		public List<dynamic> GetAllWarehouses()
		{
			try
			{
				List<dynamic> result = new List<dynamic>();
				dynamic выборка = this.Connector.Connection.Справочники.Склады.Выбрать();

				while (выборка.Следующий())
				{
					if (!выборка.ЭтоГруппа)
					{
						result.Add(выборка.Ссылка);
					}
				}

				return result;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Изменить ГЛН склада.
		/// </summary>
		/// <param name="warehouseCode">Код склада.</param>
		/// <param name="gln">Новый ГЛН.</param>
		/// <returns>true в случае успеха, иначе false.</returns>
		public bool UpdateWarehouseGLN(string warehouseCode, string gln)
		{
			if (string.IsNullOrWhiteSpace(warehouseCode) || string.IsNullOrWhiteSpace(gln))
				return false;

			var склад = this.GetWareHouse(Requisites.Code, warehouseCode);

			if (склад == null)
				return false;

			var старыйСклад = this.GetWareHouse(Requisites.GLN, gln);
			try
			{
				if (старыйСклад != null && !string.IsNullOrWhiteSpace(старыйСклад.Код))
				{
					var объектСтарогоСклада = старыйСклад.ПолучитьОбъект();
					объектСтарогоСклада.ГЛН = string.Empty;
					объектСтарогоСклада.Записать();
				}

				var объектСклада = склад.ПолучитьОбъект();
				объектСклада.ГЛН = gln;
				объектСклада.Записать();
				return true;	
			}
			catch
			{
				return false;
			}
		}

        public dynamic GetShop(dynamic warehouse)
        {
            if(warehouse != null)
                return warehouse.Магазин;
            else
                return null;
        }

        public dynamic GetShop(string warehouseCode)
        {
            var result = this.GetWareHouse(Requisites.Code, warehouseCode);

            if (result == null)
                return null;
            else
                return result.Магазин;
        }

        public dynamic GetOrganization(string warehouseCode)
        {
            var result = this.GetWareHouse(Requisites.Code, warehouseCode);

            if (result == null)
                return null;
            else
                return result.Организация;
        }

        public dynamic GetTaxRate(TaxRates rate)
        {
            try
            {
                switch (rate)
                {
                    case TaxRates.Tax_0:
                        return this.Connector.Connection.Перечисления.СтавкиНДС.НДС0;
                    case TaxRates.Tax_10:
                        return this.Connector.Connection.Перечисления.СтавкиНДС.НДС10;
                    case TaxRates.Tax_18:
                        return this.Connector.Connection.Перечисления.СтавкиНДС.НДС18;
                    case TaxRates.Tax_None:
                        return this.Connector.Connection.Перечисления.СтавкиНДС.БезНДС;
                    default:
                        return null;
                }
            } 
            catch(Exception)
            {
                return null;
            }
        }

        public dynamic GetUnit(Requisites propertyName, string propertyValue)
        {
            try
            {
                switch (propertyName)
                {
                    case Requisites.Code:
                        return this.Connector.Connection.Справочники.БазовыеЕдиницыИзмерения.НайтиПоКоду(propertyValue);
                    case Requisites.InternationalReduction_Unit:
                        return this.Connector.Connection.Справочники.БазовыеЕдиницыИзмерения.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public dynamic GetWare(Requisites propertyName, string propertyValue, string counteragentCode = "")
        {
            try
            {
                dynamic ware = null;

                switch (propertyName)
                {
                    case Requisites.Code:
                        ware = this.Connector.Connection.Справочники.Номенклатура.НайтиПоКоду(propertyValue);
                        break;
                    case Requisites.Name:
                        ware = this.Connector.Connection.Справочники.Номенклатура.НайтиПоНаименованию(propertyValue);
                        break;
                    case Requisites.ExCode_Ware:
                        dynamic запрос = this.Connector.Connection.NewObject("Запрос");
                        запрос.Текст = @"   ВЫБРАТЬ
                                                ЕДИ_СопоставлениеНоменклатуры.Номенклатура, ЕДИ_СопоставлениеНоменклатуры.Контрагент
                                            ИЗ
                                                РегистрСведений.ЕДИ_СопоставлениеНоменклатуры как ЕДИ_СопоставлениеНоменклатуры
                                            ГДЕ
                                                ЕДИ_СопоставлениеНоменклатуры.Контрагент.Код = &КодКонтрагента
                                            И
                                                ЕДИ_СопоставлениеНоменклатуры.ВнешнийКод = &ВнешнийКод";
                        запрос.УстановитьПараметр("КодКонтрагента", counteragentCode);
                        запрос.УстановитьПараметр("ВнешнийКод", propertyValue);
                        dynamic выборка = запрос.Выполнить().Выбрать();

                        if (выборка.Следующий())
                            ware = выборка.Номенклатура;
                        else
                            ware = null;

                        break;
                    default:
                        break;
                }
                return ware;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public dynamic GetBarCodeType(BarcodeTypes type)
        {
            try
            {
                switch (type)
                {
                    case BarcodeTypes.Ean_8:
                        return this.Connector.Connection.ПланыВидовХарактеристик.ТипыШтрихкодов.EAN8;
                    case BarcodeTypes.Ean_13:
                        return this.Connector.Connection.ПланыВидовХарактеристик.ТипыШтрихкодов.EAN13;
                    case BarcodeTypes.Ean_128:
                        return this.Connector.Connection.ПланыВидовХарактеристик.ТипыШтрихкодов.EAN128;
                    default:
                        return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public List<dynamic> GetWareExCodes(dynamic ware)
        {
            if (ware == null)
                return null;

            List<dynamic> result = new List<dynamic>();

            var отборТовар = this.Connector.Connection.NewObject("Структура");
            отборТовар.Вставить("Номенклатура", ware.Ссылка);
            var выборка = this.Connector.Connection.РегистрыСведений.ЕДИ_СопоставлениеНоменклатуры.Выбрать(отборТовар);

            while (выборка.Следующий())
            {
                var temp = this.Connector.Connection.NewObject("Структура");
                temp.Вставить("ВнешнийКод", выборка.ВнешнийКод);
                temp.Вставить("Контрагент", выборка.Контрагент);
                result.Add(temp);
            }

            return result;
        }

        public List<dynamic> GetAllWares()
        {
            try
            {
                List<dynamic> result = new List<dynamic>();
                dynamic выборка = this.Connector.Connection.Справочники.Номенклатура.Выбрать();

                while (выборка.Следующий())
                {
                    if (!выборка.ЭтоГруппа)
                    {
                        result.Add(выборка.Ссылка);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetWareBarcodes(dynamic ware)
        {
            if (ware == null)
                return null;

            List<string> result = new List<string>();

            var отборВладелец = this.Connector.Connection.NewObject("Структура");
            отборВладелец.Вставить("Владелец", ware.Ссылка);
            var выборка = this.Connector.Connection.РегистрыСведений.Штрихкоды.Выбрать(отборВладелец);

            while (выборка.Следующий())
                result.Add(выборка.Штрихкод);

            return result;
        }

        public bool UpdateWareExCode(string innerCode, string exCode, string supplierCode)
        {
            var товар = this.GetWare(Requisites.Code, innerCode);
            var поставщик = this.GetCounteragent(Requisites.Code, supplierCode);

            if (товар == null || поставщик == null)
                return false;

            try
            {
                var менеджерЗаписи = this.Connector.Connection.РегистрыСведений.ЕДИ_СопоставлениеНоменклатуры.СоздатьМенеджерЗаписи();

                менеджерЗаписи.Контрагент = поставщик;
                менеджерЗаписи.ВнешнийКод = exCode;
                менеджерЗаписи.Прочитать();

                if (!менеджерЗаписи.Выбран())
                {
                    менеджерЗаписи.Контрагент = поставщик;
                    менеджерЗаписи.ВнешнийКод = exCode;
                }

                менеджерЗаписи.Номенклатура = товар;
                менеджерЗаписи.Записать();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string AddNewWare(string name, string fullName, string unit, List<string> barCodes)
        {
            return AddNewWare("", name, fullName, unit, barCodes);
        }

        public string AddNewWare(string code, string name, string fullName, string unit, List<string> barCodes)
        {
            try
            {
                var товар = this.Connector.Connection.Справочники.Номенклатура.СоздатьЭлемент();
                товар.Код = code;
                товар.Наименование = name;
                товар.НаименованиеПолное = name;
                товар.СтавкаНДС = this.GetTaxRate(TaxRates.Tax_None);
                товар.ЕдиницаИзмерения = this.GetUnit(Requisites.InternationalReduction_Unit, unit);
                товар.Весовой = unit.ToLower() == "kgm" || unit.ToLower() == "grm";
                товар.Записать();

                foreach (var item in barCodes)
                    this.AddNewBarcode(товар, item);

                return товар.Код;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddNewBarcode(dynamic ware, string value)
        {
            try
            {
                if(ware == null || string.IsNullOrWhiteSpace(value) || value.Length < 4)
                    return false;

                var менеджер = this.Connector.Connection.РегистрыСведений.Штрихкоды.СоздатьМенеджерЗаписи();
                менеджер.Владелец = ware.Ссылка;
                менеджер.Штрихкод = value;
                менеджер.ТипШтрихкода = this.GetBarCodeType(value.Length <= 8 ? BarcodeTypes.Ean_8 : 
                                                                                value.Length <= 13 ? BarcodeTypes.Ean_13 : 
                                                                                BarcodeTypes.Ean_128);
                менеджер.Записать(true);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool AddNewBarcode(string wareCode, string value)
        {
            try
            {
                var ware = this.GetWare(Requisites.Code, wareCode);

                if (ware == null)
                    return false;

                return this.AddNewBarcode(ware, value);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool AddNewBarcodes(string wareCode, List<string> values)
        {
            try
            {
                if (values == null || !values.Any())
                    return false;

                var ware = this.GetWare(Requisites.Code, wareCode);

                if (ware == null)
                    return false;

                foreach (var item in values)
                    this.AddNewBarcode(ware, item);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNewExCode(dynamic ware, dynamic counteragent, string value)
        {
            try
            {
                if (ware == null || counteragent == null || string.IsNullOrWhiteSpace(value))
                    return false;

                var менеджер = this.Connector.Connection.РегистрыСведений.ЕДИ_СопоставлениеНоменклатуры.СоздатьМенеджерЗаписи();
                менеджер.Номенклатура = ware;
                менеджер.Контрагент = counteragent;
                менеджер.ВнешнийКод = value;
                менеджер.Записать(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNewExCode(string wareCode, string counteragentCode, string value)
        {
            try
            {
                var ware = this.GetWare(Requisites.Code, wareCode);
                var counteragent = this.GetCounteragent(Requisites.Code, counteragentCode);

                if (ware == null || counteragent == null)
                    return false;

                return this.AddNewExCode(ware, counteragent, value);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNewExCodes(string wareCode, List<string> counteragentCodes, List<string> values)
        {
            try
            {
                if (counteragentCodes == null || !counteragentCodes.Any() || values == null || !values.Any() || counteragentCodes.Count != values.Count)
                    return false;

                var ware = this.GetWare(Requisites.Code, wareCode);

                if (ware == null)
                    return false;

                for (int i = 0; i < values.Count; i++)
                    this.AddNewExCode(ware, this.GetCounteragent(Requisites.Code, counteragentCodes[i]), values[i]);
                   
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNewWaybill(string number, DateTime date, dynamic counteragent, dynamic warehouse, dynamic shop, List<DomainEntities.WaybillRow> rows)
        {
            try
            {
                if(counteragent == null || warehouse == null || shop == null || rows == null || !rows.Any() || string.IsNullOrWhiteSpace(number))
                {
                    return false;
                }

                var поступление = this.Connector.Connection.Документы.ПоступлениеТоваров.СоздатьДокумент();
                поступление.Контрагент = counteragent;
                поступление.Магазин = shop;
                поступление.Склад = warehouse;
               
                поступление.Дата = DateTime.Now;
                поступление.ДатаВходящегоДокумента = date;
                поступление.НомерВходящегоДокумента = number;

                foreach (var row in rows)
                {
                    var строкаТабличнойЧасти = поступление.Товары.Добавить();
                    строкаТабличнойЧасти.Номенклатура = this.GetWare(Requisites.Code, row.Ware.Code);
                    строкаТабличнойЧасти.КоличествоУпаковок = row.Count;
                    строкаТабличнойЧасти.Цена = row.Price;
                    строкаТабличнойЧасти.Сумма = row.Count * (float)row.Price;
                    строкаТабличнойЧасти.СтавкаНДС = row.TaxRate;
                    строкаТабличнойЧасти.СуммаНДС = row.TaxAmount;
                }

                поступление.Записать();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private Connector Connector { get; set; }
    }
}
