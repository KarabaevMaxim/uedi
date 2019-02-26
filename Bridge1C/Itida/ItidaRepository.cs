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
		/// <returns></returns>
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
					while(reader.Read())
					{
						Ware ware = new Ware
						{
							Code = ((string)reader.GetValue(0)).Trim(),
							Name = ((string)reader.GetValue(1)).Trim(),
							FullName = ((string)reader.GetValue(2)).Trim(),
							Unit = this.GetUnit(Requisites.Code, ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
							ExCodes = this.GetExCodes(((string)reader.GetValue(0)).Trim())
						};
						result.Add(ware);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Получить ЕИ.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
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
								International = ((string)reader.GetValue(0)).Trim(),
								FullName = ((string)reader.GetValue(1)).Trim(),
								Name = value.Trim()
							};
						}

						break;
					case Requisites.Name:
						break;
					case Requisites.InternationalReduction_Unit:
						break;
					default:
						break;
				}

				return result;
			}
		}

		/// <summary>
		/// Получить контрагента.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
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
						break;
				}

				return result;
			}
		}

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
						break;
				}

				return result;
			}
		}

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
								Name = ((string)reader.GetValue(0)).Trim(),
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
								Code = ((string)reader.GetValue(0)).Trim(),
								Name = ((string)reader.GetValue(1)).Trim(),
								Shop = null
							};
						}
						break;
					default:
						break;

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
						try
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
						catch(InvalidCastException ex)
						{
							
						}
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
		/// <param name="wareCode">Код товара (sprres.code или sprnn.nn).</param>
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
						result.Add(((string)reader.GetValue(0)).Trim());
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
								Code = ((string)reader.GetValue(0)).Trim(),
								FullName = ((string)reader.GetValue(1)).Trim(),
								Name = ((string)reader.GetValue(2)).Trim(),
								GLN = ((string)reader.GetValue(3)).Trim(),
							},
							Value = ((string)reader.GetValue(4)).Trim()
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
