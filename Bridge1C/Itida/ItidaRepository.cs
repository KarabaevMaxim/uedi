namespace Bridge1C.Itida
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using DomainEntities;

	public class ItidaRepository
	{
		public ItidaRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		/// <summary>
		/// Получить список товаров.
		/// </summary>
		public List<Ware> GetAllWares()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Ware> result = new List<Ware>();
				SqlCommand command = new SqlCommand("SELECT maincode, shortname, name, ed FROM sprnn", conn);
				SqlDataReader reader = command.ExecuteReader();

				if(reader.HasRows)
				{
					string wareCode = string.Empty;

					while (reader.Read())
					{
						wareCode = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
						Ware ware = new Ware
						{
							Code = wareCode,
							Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
							FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
							Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
							ExCodes = this.GetExCodes(wareCode),
							BarCodes = this.GetWareBarcodes(wareCode)
						};
						result.Add(ware);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает объект номенклатуры по указанному реквизиту.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="propertyValue">Значение реквизита для поиска.</param>
		/// <param name="counteragentGln">ГЛН контрагента (необходимо в случае поиска по внешнему коду номенклатуры).</param>
		public Ware GetWare(Requisites propertyName, string propertyValue, string counteragentGln = "")
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Ware result = null;
				SqlCommand command = null;
				SqlDataReader reader = null;
				switch (propertyName)
				{
					case Requisites.Name:
						break;
					case Requisites.Code:
						command = new SqlCommand("SELECT maincode, shortname, name, ed FROM sprnn WHERE maincode = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", propertyValue));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string wareCode = ((string)reader.GetValue(0)).Trim();
							result = new Ware
							{
								Code = wareCode,
								Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
								Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
								ExCodes = this.GetExCodes(wareCode),
								BarCodes = this.GetWareBarcodes(wareCode)
							};
						}
						break;
					case Requisites.ExCode_Ware:
						if (string.IsNullOrWhiteSpace(counteragentGln))
							throw new ArgumentOutOfRangeException("Значение counteragentGln не корректно");

						command = new SqlCommand(@"	SELECT spr.maincode, spr.shortname, spr.name, spr.ed
													FROM sprnn AS spr 
														LEFT JOIN sprres_clients AS excode ON spr.nn = excode.code 
														LEFT JOIN sprclient AS client ON excode.client = client.code 
													WHERE excode.ex_code = @wareEx AND client.ex_code = @clientEx", conn);
						command.Parameters.Add(new SqlParameter("@wareEx", propertyValue));
						command.Parameters.Add(new SqlParameter("@clientEx", counteragentGln));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string wareCode = ((string)reader.GetValue(0)).Trim();
							result = new Ware
							{
								Code = wareCode,
								Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
								Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
								ExCodes = this.GetExCodes(wareCode),
								BarCodes = this.GetWareBarcodes(wareCode)
							};
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение propertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить ЕИ.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Unit GetUnit(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Unit result = null;

				switch (propertyName)
				{
					case Requisites.Code:
						SqlCommand command = new SqlCommand("SELECT ex_Code, name FROM spredn WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						SqlDataReader reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Unit
							{
								Code = value.Trim(),
								International = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								FullName = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								Name = value.Trim()
							};
						}

						break;
					case Requisites.Name:
						break;
					case Requisites.InternationalReduction_Unit:
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить контрагента.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Counteragent GetCounteragent(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Counteragent result = null;

				switch(propertyName)
				{
					case Requisites.Code:
						SqlCommand command = new SqlCommand("SELECT name, shortname, ex_code FROM sprclient WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						SqlDataReader reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Counteragent
							{
								Code = value,
								FullName = ((string)reader.GetValue(0)).Trim(),
								Name = ((string)reader.GetValue(1)).Trim(),
								GLN = ((string)reader.GetValue(2)).Trim(),
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.GLN:
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить организацию.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Organization GetOrganization(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Organization result = null;

				switch (propertyName)
				{
					case Requisites.Code:
						SqlCommand command = new SqlCommand("SELECT shortname, ex_code FROM sprfirm WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						SqlDataReader reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Organization
							{
								Code = value,
								Name = ((string)reader.GetValue(0)).Trim(),
								GLN = ((string)reader.GetValue(1)).Trim()
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.GLN:
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить склад.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Warehouse GetWarehouse(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Warehouse result = null;
				SqlCommand command = null;
				SqlDataReader reader = null;

				switch (propertyName)
				{
					case Requisites.Code:
						command = new SqlCommand("SELECT name FROM sprskl WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Warehouse
							{
								Code = value,
								Name = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								Shop = null
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.GLN:
						command = new SqlCommand("SELECT code, name FROM sprskl WHERE ex_code = @gln", conn);
						command.Parameters.Add(new SqlParameter("@gln", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Warehouse
							{
								Code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								Shop = null
							};
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить список складов.
		/// </summary>
		public List<Warehouse> GetAllWarehouses()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Warehouse> result = new List<Warehouse>();
				SqlCommand command = new SqlCommand("SELECT code, name FROM sprskl", conn);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					string wareCode = string.Empty;

					while (reader.Read())
					{
						Warehouse warehouse = new Warehouse
						{
							Code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
							Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
							Shop = null
						};
						result.Add(warehouse);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает список всех накладных.
		/// </summary>
		public List<Waybill> GetAllWaybills()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Waybill> result = new List<Waybill>();

				SqlCommand command = new SqlCommand(@"SELECT wbtb.ndok, wbtb.date, cntr.code, cntr.name, cntr.shortname, cntr.ex_code,
														org.code, org.shortname, org.ex_code, skl.code, skl.name
													FROM spr001 as wbtb 
													LEFT JOIN sprclient as cntr ON wbtb.client = cntr.code 
													LEFT JOIN sprfirm as org ON wbtb.firm = org.code 
													LEFT JOIN sprskl as skl ON wbtb.sklad = skl.code", conn);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Waybill wb = new Waybill
						{
							Number = ((string)reader.GetValue(0)).Trim(),
							Date = ((DateTime)reader.GetValue(1)),
							Supplier = new Counteragent
							{
								Code = ((string)reader.GetValue(2)).Trim(),
								FullName = ((string)reader.GetValue(3)).Trim(),
								Name = ((string)reader.GetValue(4)).Trim(),
								GLN = ((string)reader.GetValue(5)).Trim(),
							},
							Organization = new Organization
							{
								Code = ((string)reader.GetValue(6)).Trim(),
								Name = ((string)reader.GetValue(7)).Trim(),
								GLN = ((string)reader.GetValue(8)).Trim()
							},
							Warehouse = new Warehouse
							{
								Code = ((string)reader.GetValue(9)).Trim(),
								Name = ((string)reader.GetValue(10)).Trim(),
								Shop = null
							},
							Positions = new List<WaybillRow>(),
							Shop = null
						};
						result.Add(wb);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает список накладных по указанному номеру. В идеале должен быть возвращен список из одной накладной, но не обязательно.
		/// </summary>
		/// <param name="number">Номер накладной.</param>
		public List<Waybill> GetWaybillsByNumber(string number)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Waybill> result = new List<Waybill>();

				SqlCommand command = new SqlCommand(@"SELECT wbtb.date, cntr.code, cntr.name, cntr.shortname, cntr.ex_code,
														org.code, org.shortname, org.ex_code, skl.code, skl.name
													FROM spr001 as wbtb 
													LEFT JOIN sprclient as cntr ON wbtb.client = cntr.code 
													LEFT JOIN sprfirm as org ON wbtb.firm = org.code 
													LEFT JOIN sprskl as skl ON wbtb.sklad = skl.code
													WHERE wbtb.ndok = @ndok", conn);
				command.Parameters.Add(new SqlParameter("@ndok", number));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Waybill wb = new Waybill
						{
							Number = number,
							Date = ((DateTime)reader.GetValue(0)),
							Shop = null
						};
						wb.Supplier = reader.GetValue(1) == DBNull.Value ? null : new Counteragent
						{
							Code = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
							FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
							Name = reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim(),
							GLN = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim()
						};
						wb.Organization = reader.GetValue(5) == DBNull.Value ? null : new Organization
						{
							Code = reader.GetValue(5) == DBNull.Value ? string.Empty : ((string)reader.GetValue(5)).Trim(),
							Name = reader.GetValue(6) == DBNull.Value ? string.Empty : ((string)reader.GetValue(6)).Trim(),
							GLN = reader.GetValue(7) == DBNull.Value ? string.Empty : ((string)reader.GetValue(7)).Trim()
						};
						wb.Warehouse = reader.GetValue(8) == DBNull.Value ? null : new Warehouse
						{
							Code = reader.GetValue(8) == DBNull.Value ? string.Empty : ((string)reader.GetValue(8)).Trim(),
							Name = reader.GetValue(9) == DBNull.Value ? string.Empty : ((string)reader.GetValue(9)).Trim(),
							Shop = null
						};
						wb.Positions = new List<WaybillRow>();
						result.Add(wb);
					}
				}

				return result;
			}
		}

		#region Закрытые члены класса (потенциально закрытые)

		/// <summary>
		/// Возвращает список ШК для товара с указанным кодом wareCode (sprres.maincode или sprnn.maincode).
		/// </summary>
		/// <param name="wareCode">Код товара (sprres.maincode или sprnn.maincode).</param>
		public List<string> GetWareBarcodes(string wareCode)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<string> result = new List<string>();
				SqlCommand command = new SqlCommand(@"SELECT bc
													FROM sprnn as spr left join sprnnbc as bc on spr.nn = bc.nn
													WHERE maincode = @mc", conn);
				command.Parameters.Add(new SqlParameter("@mc", wareCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						result.Add(reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim());
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает строки из накладной с указанным идентификатором (identity_column).
		/// </summary>
		/// <param name="wbIc">Идентификатор(identity_column)</param>
		public List<WaybillRow> GetWaybillRows(string wbIc)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<WaybillRow> result = new List<WaybillRow>();
				SqlCommand command = new SqlCommand(@"	SELECT spec.kolp, spec.cena, ISNULL((
															SELECT kodn.proc_ 
															FROM sprkodn kodn
															WHERE CHARINDEX(kodn.ntype, '05 06' ) <> 0 AND CHARINDEX( kodn.code, (
																SELECT list 
																FROM speclistkodn 
																WHERE code= spec.kodn AND date= (
																	SELECT MAX( date ) 
																	FROM speclistkodn 
																	WHERE code = spec.kodn AND date <= spr.date))) <> 0), 0 
														) AS TaxRate, spec.sumnds, edn.code, edn.name, edn.ex_code,
														nn.maincode, nn.shortname, nn.name, nn.ed
														FROM spec001 AS spec 
															INNER JOIN spr001 AS spr ON spec.ic = spr.identity_column 
															LEFT JOIN spredn as edn ON spec.ed = edn.code 
															LEFT JOIN sprnn AS nn ON spec.nn = nn.nn
														WHERE spec.ic = @ndok", conn);
				command.Parameters.Add(new SqlParameter("@ndok", wbIc));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						WaybillRow row = new WaybillRow();
						row.Count = reader.GetValue(0) == DBNull.Value ? 0 : (float)(double)reader.GetValue(0);
						row.Price = reader.GetValue(1) == DBNull.Value ? 0 : (decimal)(double)reader.GetValue(1);
						row.TaxRate = reader.GetValue(2) == DBNull.Value ? 0 : (int)Math.Round((double)reader.GetValue(2));
						row.TaxAmount = reader.GetValue(3) == DBNull.Value ? 0 : (decimal)(double)reader.GetValue(3);
						row.Unit = reader.GetValue(4) == DBNull.Value ? null : new Unit
						{
							Code = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim(),
							Name = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim(),
							FullName = reader.GetValue(5) == DBNull.Value ? string.Empty : ((string)reader.GetValue(5)).Trim(),
							International = reader.GetValue(6) == DBNull.Value ? string.Empty : ((string)reader.GetValue(6)).Trim()
						};
						string wareCode = reader.GetValue(7) == DBNull.Value ? string.Empty : ((string)reader.GetValue(7)).Trim();
						row.Ware = string.IsNullOrWhiteSpace(wareCode) ? null : new Ware
						{
							Code = wareCode,
							Name = reader.GetValue(8) == DBNull.Value ? string.Empty : ((string)reader.GetValue(8)).Trim(),
							FullName = reader.GetValue(9) == DBNull.Value ? string.Empty : ((string)reader.GetValue(9)).Trim(),
							Unit = this.GetUnit(Requisites.Code, reader.GetValue(9) == DBNull.Value ? string.Empty : ((string)reader.GetValue(10)).Trim()),
							ExCodes = this.GetExCodes(wareCode),
							BarCodes = this.GetWareBarcodes(wareCode)
						};

						result.Add(row);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Получить список внешних кодов номенклатуры по ее коду (sprnn.maincode или sprres.maincode).
		/// </summary>
		/// <param name="wareCode">Код номенклатуры (sprnn.maincode или sprres.maincode).</param>
		public List<WareExCode> GetExCodes(string wareCode)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<WareExCode> result = new List<WareExCode>();
				SqlCommand command = new SqlCommand(@"	SELECT client.code, client.name, client.shortname,client.ex_code, excode.ex_code 
														FROM sprnn AS spr 
															LEFT JOIN sprres_clients as excode on spr.nn = excode.code
															LEFT JOIN sprclient as client ON excode.client = client.code
														WHERE spr.maincode = @code",
														conn);
				command.Parameters.Add(new SqlParameter("@code", wareCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						WareExCode exCode = new WareExCode
						{
							Counteragent = new Counteragent
							{
								Code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								FullName = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								Name = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
								GLN = reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim(),
							},
							Value = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim()
						};

						result.Add(exCode);
					}
				}

				return result;
			}
		}

		private readonly string connectionString;

		#endregion
	}
}
