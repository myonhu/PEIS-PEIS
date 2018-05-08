using PEIS.Common;
using PEIS.Model;
using Maticsoft.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace PEIS.BLL
{
	public class CommonTeam
	{
		private const int _defaultCount = 50;

		private static readonly CommonTeam _instance = new CommonTeam();

		private int _maxRowCount = 50;

		private System.Collections.Hashtable _ALlFeeID = new System.Collections.Hashtable();

		private string _CacheUnionBusFee = "UnionBusFee";

		private string _CacheBusFee = "BusFee";

		private string _CacheTeamKey = "Team";

		private string _CacheTeamTaskKey = "TeamTask";

		private string _CacheTeamTaskInfoByKeyWordKey = "TeamTaskInfoByKeyWordKey";

		private string _CacheTeamTaskGroupKey = "TeamTaskGroup";

		private string _UserInfoKey = "UserInfo";

		private string _BusFeeInfoKey = "BusFee";

		public static CommonTeam Instance
		{
			get
			{
				return CommonTeam._instance;
			}
		}

		public DataTable GetTeamInfo(string ID_Team, string TeamName)
		{
			string text = "SELECT ID_TeaM,TeamName,Creator CreatorX,ID_Creator,Creator,CONVERT(varchar(10),CreateDate,120) CreateDate,InputCode,Note FROM Team WHERE 1=1 ";
			if (ID_Team != string.Empty)
			{
				text += string.Format(" AND ID_Team='{0}'", ID_Team);
			}
			if (TeamName != string.Empty)
			{
				text += string.Format(" AND TeamName='{0}'", TeamName);
			}
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public DataTable SearchUnionCustomerBusFee(string ID_Team, string ID_TeamTask, string ID_CustomerS, string UserName, string InputCode, string SelectedFee)
		{
			object cache = DataCache.GetCache(this._CacheUnionBusFee);
			DataTable dataTable;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string text = string.Format("SELECT DISTINCT ID_Customer,ISNULL(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,OnCustFee.OriginalPrice Price,OnCustFee.Discount Discount,OnCustFee.FactPrice FactPrice \r\nFROM OnCustFee\r\nLEFT JOIN BusFee on OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\nLEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee WHERE ISNULL(Is_FeeCharged,0)=0 and isnull(BusFee.Is_Banned,0)!=1", ID_CustomerS);
				text = text.Replace("@userName", UserName);
				text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
				dataTable = CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._CacheUnionBusFee, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			return this.GetSearchDT(ID_CustomerS, InputCode, SelectedFee, dataTable);
		}

		public DataTable SearchBusFee(string UserName, string InputCode, string SelectedFee)
		{
			object cache = DataCache.GetCache(this._CacheBusFee);
			DataTable dataTable;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string text = "SELECT distinct -1 ID_Customer,-1 CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice FROM BusFee\r\nINNER JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\nINNER JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee WHERE isnull(BusFee.Is_Banned,0)!=1 @where";
				text = text.Replace("@where", string.Empty);
				text = text.Replace("@Discount", "10");
				text = text.Replace("@userName", UserName);
				text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
				dataTable = CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._CacheBusFee, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			return this.GetSearchDT("", InputCode, SelectedFee, dataTable);
		}

		private DataTable GetSearchDT(string ID_Customers, string InputCode, string SelectedFee, DataTable dt)
		{
			this._ALlFeeID.Clear();
			if (!dt.Columns.Contains("IsChecked"))
			{
				dt.Columns.Add("IsChecked");
			}
			DataTable dataTable = dt.Clone();
			if (ID_Customers == string.Empty)
			{
				ID_Customers = "-1";
			}
			DataRow[] array = (SelectedFee.Trim() == string.Empty) ? null : dt.Select(string.Concat(new string[]
			{
				"ID_Customer IN(",
				ID_Customers,
				") and ID_Fee in(",
				SelectedFee,
				")"
			}));
			if (InputCode.Trim() != string.Empty)
			{
				DataRow[] array2;
				if (SelectedFee.Trim() != string.Empty)
				{
					array2 = dt.Select(string.Concat(new string[]
					{
						"ID_Customer IN(",
						ID_Customers,
						") and InputCode='",
						InputCode,
						"' and ID_Fee not in(",
						SelectedFee,
						")"
					}));
				}
				else
				{
					array2 = dt.Select(string.Concat(new string[]
					{
						"ID_Customer IN(",
						ID_Customers,
						") and InputCode='",
						InputCode,
						"'"
					}));
				}
				DataRow[] array3;
				if (SelectedFee.Trim() != string.Empty)
				{
					array3 = dt.Select(string.Concat(new string[]
					{
						"ID_Customer IN(",
						ID_Customers,
						") and InputCode<>'",
						InputCode,
						"' and InputCode like '",
						InputCode,
						"%' and ID_Fee not in(",
						SelectedFee,
						")"
					}));
				}
				else
				{
					array3 = dt.Select(string.Concat(new string[]
					{
						"ID_Customer IN(",
						ID_Customers,
						") and InputCode<>'",
						InputCode,
						"' and InputCode like '",
						InputCode,
						"%'"
					}));
				}
				DataRow[] array4 = dt.Select(string.Concat(new string[]
				{
					"ID_Customer IN(",
					ID_Customers,
					") and InputCode<>'",
					InputCode,
					"' and InputCode not like '",
					InputCode,
					"%' and InputCode like '%",
					InputCode,
					"%'"
				}));
				if (array2 != null)
				{
					DataRow[] array5 = array2;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable.ImportRow(dataRow);
						}
					}
				}
				if (array3 != null)
				{
					DataRow[] array5 = array3;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (dataTable.Rows.Count >= this._maxRowCount)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable.ImportRow(dataRow);
						}
					}
				}
				if (array != null)
				{
					DataRow[] array5 = array;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable.ImportRow(dataRow);
						}
					}
				}
				if (array4 != null)
				{
					DataRow[] array5 = array4;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (dataTable.Rows.Count >= this._maxRowCount)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable.ImportRow(dataRow);
						}
					}
				}
			}
			else
			{
				string text = string.Empty;
				DataRow[] array5;
				if (array != null)
				{
					array5 = array;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							text = text + dataRow["ID_Fee"].ToString() + ",";
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable.ImportRow(dataRow);
						}
					}
				}
				text = text.TrimEnd(new char[]
				{
					','
				});
				DataRow[] array6;
				if (text.Trim() != string.Empty)
				{
					array6 = dt.Select(string.Concat(new string[]
					{
						"ID_Customer IN(",
						ID_Customers,
						") and ID_Fee not in(",
						text,
						")"
					}));
				}
				else
				{
					array6 = dt.Select("ID_Customer IN(" + ID_Customers + ")");
				}
				array5 = array6;
				for (int i = 0; i < array5.Length; i++)
				{
					DataRow dataRow = array5[i];
					if (dataTable.Rows.Count >= this._maxRowCount)
					{
						break;
					}
					if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
					{
						this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
						dataTable.ImportRow(dataRow);
					}
				}
			}
			return dataTable;
		}

		public DataTable SaveTeamData(string ID_Team, string TeamName, string ID_Creator, string Creator, string CreateDate, string InputCode, string Note)
		{
			string sql = string.Format("IF NOT EXISTS(SELECT * FROM Team WHERE TeamName='{0}')\r\nBEGIN\r\n\tINSERT INTO Team(TeamName,InputCode,Note,ID_Creator,Creator,CreateDate)\r\n\tVALUES('{0}','{1}','{2}','{3}','{4}','{5}');\r\n    SELECT 0 Result,SCOPE_IDENTITY() ID_Team;\r\nEND\r\nELSE \r\nBEGIN\r\n\tUPDATE Team SET InputCode='{1}',Note='{2}' WHERE TeamName='{0}';\r\n    SELECT 1 Result,ID_Team,'{1}' InputCode FROM Team WHERE TeamName='{0}';\r\nEND;", new object[]
			{
				TeamName,
				InputCode,
				Note,
				ID_Creator,
				Creator,
				CreateDate
			});
			DataCache.DeleteCache(this._CacheTeamKey);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable ISCanDeleteTeamInfo(string ID_Team)
		{
			string sql = string.Format("SELECT ID_Team,ID_TeamTask FROM TeamTask WHERE ID_Team IN({0});", ID_Team);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable ISCanDeleteTeamTaskGroupFeeInfo(string TeamTaskGroupID)
		{
			TeamTaskGroupID = TeamTaskGroupID.TrimStart(new char[]
			{
				','
			}).TrimEnd(new char[]
			{
				','
			});
			string sql = string.Format("IF EXISTS(SELECT 1 FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN({0}))\r\nBEGIN\r\n\tSELECT 1 ISExists,TeamTaskGroupName from TeamTaskGroup WHERE ID_TeamTaskGroup IN({0});\r\nEND\r\nELSE\r\nBEGIN\r\n\tSELECT 0 ISExists,'' TeamTaskGroupName;\r\nEND", TeamTaskGroupID);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable IsCanSaveCustomer(string TeamTaskGroupID)
		{
			TeamTaskGroupID = TeamTaskGroupID.TrimStart(new char[]
			{
				','
			}).TrimEnd(new char[]
			{
				','
			});
			string sql = string.Format("SELECT ID_TeamTaskGroup,TeamTaskGroupName FROM TeamTaskGroup WHERE ID_TeamTaskGroup IN({0})\r\nAND NOT EXISTS(SELECT ID_TeamTaskGroup FROM TeamTaskGroupFee WHERE TeamTaskGroup.ID_TeamTaskGroup=TeamTaskGroupFee.ID_TeamTaskGroup AND TeamTaskGroupFee.ID_TeamTaskGroup IN({0}) GROUP BY ID_TeamTaskGroup);", TeamTaskGroupID);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable ISCanDeleteTeamTaskInfo(string ID_TeamTask)
		{
			string sql = string.Format("SELECT ID_Team,ID_TeamTask,ID_TeamTaskGroup FROM TeamTaskGroup WHERE ID_TeamTask IN({0});", ID_TeamTask);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable ISCanDeleteTeamTaskGroupInfo(string ID_TeamTaskGroup)
		{
			string sql = string.Format("SELECT ID_TeamTask,ID_TeamTaskGroup,ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN({0});", ID_TeamTaskGroup);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable ISCanDeleteTeamTaskGroupCustomerInfo(string ID_Customer)
		{
			string sql = string.Format("SELECT ID_Customer FROM OnCustPhysicalExamInfo WHERE Is_Team=1 AND Is_GuideSheetPrinted=1 AND ID_Customer IN({0})\r\nUNION ALL SELECT TOP 1 ID_Customer FROM OnCustFee WHERE ID_Customer IN({0}) AND Is_FeeCharged=1;", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable GetCustomerByTeamAndTask(string ID_Team, string ID_TeamTask)
		{
			string sql = string.Format("SELECT 1 exist,OnArcCust.NationID,OnArcCust.NationName,OnArcCust.MobileNo CustomerTel,OnArcCust.IDCard,OnArcCust.CustomerName,OnArcCust.ID_Gender,OnArcCust.GenderName,OnArcCust.ID_Marriage,OnArcCust.MarriageName,CONVERT(varchar(10),OnArcCust.BirthDay,120) CustomerBirthDay,TeamTaskGroup.RoleName CustomerRoleName,TeamTaskGroup.ID_Team,TeamTaskGroup.ID_TeamTask,TeamTaskGroup.ID_TeamTaskGroup,TeamTaskGroup.TeamTaskGroupName,TeamTaskGroupCust.ID_TeamTaskGroupCustomer,TeamTaskGroupCust.ID_Customer,TeamTaskGroupCust.Department DepartmentX,TeamTaskGroupCust.DepartSubA DepartmentA,TeamTaskGroupCust.DepartSubB DepartmentB,TeamTaskGroupCust.DepartSubC DepartmentC FROM TeamTaskGroupCust\r\nINNER JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\nINNER JOIN OnCustRelationCustPEInfo ON TeamTaskGroupCust.ID_Customer=OnCustRelationCustPEInfo.ID_Customer\r\nINNER JOIN OnArcCust ON OnCustRelationCustPEInfo.ID_ArcCustomer=OnArcCust.ID_ArcCustomer\r\nWHERE ID_Team='{0}' AND TeamTaskGroup.ID_TeamTask='{1}';", ID_Team, ID_TeamTask);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public int DelTeamData(string IDTeamS)
		{
			string text = string.Format("\r\nIF NOT EXISTS(SELECT 1 FROM TeamTask WHERE ID_Team IN({0}))\r\nBEGIN\r\n    SELECT ID_TeamTaskGroup INTO #TempAllGroup@RandNum FROM TeamTaskGroup WHERE ID_Team IN({0});\r\n    SELECT * INTO #TempGroupCust@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroup@RandNum);\r\n    SELECT * INTO #TempOCRPEInfo@RandNum FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroup@RandNum); \r\n    DELETE FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroup@RandNum); \r\n    DELETE FROM TeamTaskGroup WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroup@RandNum); \r\n    DELETE FROM TeamTask WHERE ID_Team IN({0});\r\n    DELETE FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM OnCustPhysicalExamInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM Team WHERE ID_Team IN({0});\r\n    DROP TABLE #TempGroupCust@RandNum;\r\n    DROP TABLE #TempOCRPEInfo@RandNum;\r\n    DROP TABLE #TempAllGroup@RandNum;\r\nEND", IDTeamS);
			text = text.Replace("@RandNum", Public.GetGuid("-", string.Empty));
			List<string> list = new List<string>(1);
			list.Add(text);
			DataCache.DeleteCache(this._CacheTeamKey);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int DeleteTeamTask(string CustTeamTaskID)
		{
			string text = string.Format("\r\nIF NOT EXISTS(SELECT 1 FROM TeamTaskGroup WHERE ID_TeamTask IN({0}))\r\nBEGIN\r\n    SELECT ID_TeamTaskGroup INTO #TempAllGroupInfo@RandNum FROM TeamTaskGroup WHERE ID_TeamTask IN({0});\r\n    SELECT * INTO #TempGCust@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroupInfo@RandNum);\r\n    SELECT * INTO #TempPE@RandNum FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGCust@RandNum);\r\n    DELETE FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ID_Customer IN(SELECT ID_Customer FROM #TempGCust@RandNum);\r\n    DELETE FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroupInfo@RandNum); \r\n    DELETE FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroupInfo@RandNum); \r\n    DELETE FROM TeamTaskGroup WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM #TempAllGroupInfo@RandNum); \r\n    DELETE FROM TeamTask WHERE ID_TeamTask IN({0});\r\n    DELETE FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGCust@RandNum);\r\n    DELETE FROM OnCustPhysicalExamInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGCust@RandNum);\r\n    DROP TABLE #TempGCust@RandNum;\r\n    DROP TABLE #TempPE@RandNum;\r\n    DROP TABLE #TempAllGroupInfo@RandNum;\r\nEND\r\n", CustTeamTaskID);
			text = text.Replace("@RandNum", Public.GetGuid("-", string.Empty));
			List<string> list = new List<string>(1);
			list.Add(text);
			int num = CommonExcuteSql.Instance.ExecuteSqlTran(list);
			if (num > 0)
			{
				DataCache.DeleteCache(this._CacheTeamTaskKey);
				DataCache.DeleteCache(this._CacheTeamTaskInfoByKeyWordKey);
			}
			return num;
		}

		public int DeleteTeamTaskGroup(string CustTeamTaskGroupID)
		{
			string text = string.Format("\r\nIF NOT EXISTS(SELECT 1 FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN({0}))\r\nBEGIN\r\n    SELECT * INTO #TempGroupCust@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN({0});\r\n    SELECT * INTO #TempOCRCPE@RandNum FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup IN({0}); \r\n    DELETE FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup IN({0}); \r\n    DELETE FROM TeamTaskGroup WHERE ID_TeamTaskGroup IN({0}); \r\n    DELETE FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DELETE FROM OnCustPhysicalExamInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempGroupCust@RandNum);\r\n    DROP TABLE #TempGroupCust@RandNum;\r\n    DROP TABLE #TempOCRCPE@RandNum;\r\nEND", CustTeamTaskGroupID);
			text = text.Replace("@RandNum", Public.GetGuid("-", string.Empty));
			List<string> list = new List<string>(1);
			list.Add(text);
			int num = CommonExcuteSql.Instance.ExecuteSqlTran(list);
			if (num > 0)
			{
				DataCache.DeleteCache(this._CacheTeamTaskGroupKey);
			}
			return num;
		}

		public int DeleteCustomerInfo(string ID_Customers)
		{
			string item = string.Format("\r\nIF NOT EXISTS(SELECT 1 from OnCustPhysicalExamInfo WHERE (Is_GuideSheetPrinted=1 OR Is_FeeSettled=1) AND ID_Customer IN({0}))\r\nBEGIN\r\n    DELETE FROM TeamTaskGroupCust WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ID_Customer IN({0});\r\n    DELETE FROM OnCustExamSection WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustRelationCustPEInfo WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustReportManage WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustPhysicalExamInfo WHERE ID_Customer IN({0});\r\nEND", ID_Customers);
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int DeleteInternatCustomerInfo(string ID_Customers)
		{
			string item = string.Format("\r\nIF NOT EXISTS(SELECT 1 from OnCustPhysicalExamInfo WHERE (Is_GuideSheetPrinted=1 OR Is_FeeSettled=1) AND ID_Customer IN({0}))\r\nBEGIN\r\n    DELETE FROM TeamTaskGroupCust WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ID_Customer IN({0});\r\n    DELETE FROM OnCustExamSection WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustRelationCustPEInfo WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustReportManage WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustPhysicalExamInfo WHERE ID_Customer IN({0});\r\n    DELETE FROM OnInternetDataImportLog WHERE ID_Customer IN({0});\r\n    DELETE FROM OnCustBackLog WHERE ID_Customer IN({0});\r\nEND", ID_Customers);
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int DeleteTeamTaskGroupFee(string CustTeamTaskGroupFeeID)
		{
			string item = string.Empty;
			string[] array = CustTeamTaskGroupFeeID.TrimEnd(new char[]
			{
				'|'
			}).Split(new char[]
			{
				'|'
			});
			string text = string.Empty;
			string text2 = string.Empty;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text3 = array2[i];
				string[] array3 = text3.TrimEnd(new char[]
				{
					'_'
				}).Split(new char[]
				{
					'_'
				});
				text = text + "'" + array3[0] + "',";
				text2 = text2 + "'" + array3[1] + "',";
			}
			item = string.Format("DELETE FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup IN({0}) AND ID_Fee in({1}); ", text.TrimEnd(new char[]
			{
				','
			}), text2.TrimEnd(new char[]
			{
				','
			}));
			List<string> list = new List<string>(1);
			list.Add(item);
			int num = CommonExcuteSql.Instance.ExecuteSqlTran(list);
			if (num > 0)
			{
				DataCache.DeleteCache(this._CacheTeamTaskGroupKey);
			}
			return num;
		}

		public DataTable GetTeamTaskInfoByTaskID(string TeamTaskID)
		{
			string sql = string.Format("SELECT ID_TeamTask,ID_Team,TargetDate,TaskNumCount,Contact,Tel,ID_Subscriber,Subscriber,CreateDate,CONVERT(varchar(10),TaskExamStartDate,120) TaskExamStartDate, CONVERT(varchar(10),TaskExamEndDate,120) TaskExamEndDate,InputCode,DispOrder,TeamTaskName FROM TeamTask WHERE ID_TeamTask='{0}'", TeamTaskID);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable GetTeamTaskInfoByKeyWord(string ColumnName, string ColumnValue, bool IsLike)
		{
			object cache = DataCache.GetCache(this._CacheTeamTaskInfoByKeyWordKey);
			DataTable dataTable;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string sql = "SELECT 1 exist,ID_TeamTask,TeamTask.ID_Team,TeamName,TargetDate,TaskNumCount,Contact,Tel,ID_Subscriber,Subscriber,CONVERT(varchar(10),TeamTask.CreateDate,120) CreateDate,CONVERT(varchar(10),TaskExamStartDate,120) TaskExamStartDate,CONVERT(varchar(10),TaskExamEndDate,120) TaskExamEndDate,TeamTask.InputCode,DispOrder,TeamTaskName FROM TeamTask INNER JOIN Team ON TeamTask.ID_Team=Team.ID_Team ORDER BY DispOrder;";
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._CacheTeamTaskInfoByKeyWordKey, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			DataTable dataTable2 = dataTable.Clone();
			DataTable dataTable3 = dataTable.Clone();
			DataTable result;
			if (ColumnValue == string.Empty)
			{
				result = dataTable;
			}
			else
			{
				DataRow[] array;
				if (IsLike)
				{
					array = dataTable.Select(ColumnName + " like '%" + ColumnValue + "%'");
				}
				else
				{
					array = dataTable.Select(ColumnName + "='" + ColumnValue + "'");
				}
				ColumnValue = ColumnValue.Trim().ToLower();
				if (array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow row = array2[i];
						dataTable2.ImportRow(row);
					}
				}
				dataTable3.Dispose();
				dataTable.Dispose();
				result = dataTable2;
			}
			return result;
		}

		public int SaveTeamTaskData(string ID_Team, string TeamTaskItems, string ID_Subscriber, string UserName)
		{
			int result;
			if (ID_Team == string.Empty || TeamTaskItems == string.Empty)
			{
				result = -1;
			}
			else
			{
				List<string> list = new List<string>();
				string[] array = TeamTaskItems.TrimEnd(new char[]
				{
					'|'
				}).Split(new char[]
				{
					'|'
				});
				string text = string.Empty;
				string text2 = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					text2 = string.Empty;
					string[] array2 = array[i].TrimEnd(new char[]
					{
						'_'
					}).Split(new char[]
					{
						'_'
					});
					int num = array2.Length;
					array2[6] = Secret.AES.EncryptPrefix(array2[6]);
					if (array2[num - 1].Trim() == "0")
					{
						text2 = ((array2[8] == string.Empty) ? Input.GetStringSpellCode(array2[1]) : array2[8].ToUpper());
						text2 = ((text2.Length > 20) ? text2.Substring(0, 20) : text2);
						text += string.Format("INSERT INTO TeamTask(ID_Team,TeamTaskName,TaskExamStartDate,TaskExamEndDate,TaskNumCount,Contact,Tel,DispOrder,InputCode,ID_Subscriber,Subscriber,CreateDate) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');", new object[]
						{
							ID_Team,
							array2[1],
							array2[2],
							array2[3],
							array2[4],
							array2[5],
							array2[6],
							array2[7],
							text2,
							ID_Subscriber,
							UserName,
							DateTime.Now
						});
					}
					if (array2[num - 1].Trim() == "1")
					{
						text2 = ((array2[8] == string.Empty) ? Input.GetStringSpellCode(array2[1]) : array2[8].ToUpper());
						text2 = ((text2.Length > 20) ? text2.Substring(0, 20) : text2);
						text += string.Format("UPDATE TeamTask SET TeamTaskName='{0}',TaskExamStartDate='{1}',TaskExamEndDate='{2}',TaskNumCount='{3}',Contact='{4}',Tel='{5}',DispOrder='{6}',InputCode='{7}' WHERE ID_TeamTask='{8}';", new object[]
						{
							array2[1],
							array2[2],
							array2[3],
							array2[4],
							array2[5],
							array2[6],
							array2[7],
							text2,
							array2[0]
						});
					}
					if (i != 0 && i % 50 == 0)
					{
						list.Add(text);
						text = string.Empty;
					}
				}
				if (text != string.Empty)
				{
					list.Add(text);
					text = string.Empty;
				}
				int num2 = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				if (num2 > 0)
				{
					DataCache.DeleteCache(this._CacheTeamTaskKey);
					DataCache.DeleteCache(this._CacheTeamTaskInfoByKeyWordKey);
				}
				result = num2;
			}
			return result;
		}

		public int SaveTemaTaskGroup(string ID_Team, string TeamName, string ID_TeamTask, string TeamTaskName, string TeamTaskItems, string ID_Subscriber, string UserName, ref System.Text.StringBuilder sb)
		{
			sb.Length = 0;
			int result;
			if (ID_Team == string.Empty || TeamTaskItems == string.Empty)
			{
				result = -1;
			}
			else
			{
				List<string> list = new List<string>();
				string[] array = TeamTaskItems.TrimEnd(new char[]
				{
					'|'
				}).Split(new char[]
				{
					'|'
				});
				string text = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].TrimEnd(new char[]
					{
						'_'
					}).Split(new char[]
					{
						'_'
					});
					int num = array2.Length;
					if (array2[num - 1].Trim() == "0")
					{
						if (array2[9] == "-1")
						{
							if (array2[15] != "-1")
							{
								text += string.Format("INSERT INTO TeamTaskGroup(ID_Team,TeamName,ID_TeamTask,TeamTaskName,TeamTaskGroupName\r\n,Forsex,Is_Marriage,MinAgeValue,MaxAgeValue,ID_ExamType,ExamTypeName,ID_FeeWay,FeeWayName,RoleName,SecurityLevel,Is_GroupPaused) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');", new object[]
								{
									ID_Team,
									TeamName,
									ID_TeamTask,
									TeamTaskName,
									array2[0],
									array2[1],
									array2[3],
									array2[5],
									array2[6],
									array2[7],
									array2[8],
									array2[11],
									array2[12],
									array2[13],
									array2[15],
									array2[16]
								});
							}
							else if (array2[15] != "-1")
							{
								text += string.Format("INSERT INTO TeamTaskGroup(ID_Team,TeamName,ID_TeamTask,TeamTaskName,TeamTaskGroupName\r\n,Forsex,Is_Marriage,MinAgeValue,MaxAgeValue,ID_ExamType,ExamTypeName,ID_FeeWay,FeeWayName,RoleName,SecurityLevel,Is_GroupPaused) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');", new object[]
								{
									ID_Team,
									TeamName,
									ID_TeamTask,
									TeamTaskName,
									array2[0],
									array2[1],
									array2[3],
									array2[5],
									array2[6],
									array2[7],
									array2[8],
									array2[11],
									array2[12],
									array2[13],
									array2[15],
									array2[16]
								});
							}
							else
							{
								text += string.Format("INSERT INTO TeamTaskGroup(ID_Team,TeamName,ID_TeamTask,TeamTaskName,TeamTaskGroupName\r\n,Forsex,Is_Marriage,MinAgeValue,MaxAgeValue,ID_ExamType,ExamTypeName,ID_FeeWay,FeeWayName,RoleName,Is_GroupPaused) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}');", new object[]
								{
									ID_Team,
									TeamName,
									ID_TeamTask,
									TeamTaskName,
									array2[0],
									array2[1],
									array2[3],
									array2[5],
									array2[6],
									array2[7],
									array2[8],
									array2[11],
									array2[12],
									array2[13],
									array2[16]
								});
							}
						}
						else if (array2[15] != "-1")
						{
							text += string.Format("INSERT INTO TeamTaskGroup(ID_Team,TeamName,ID_TeamTask,TeamTaskName,TeamTaskGroupName\r\n,Forsex,Is_Marriage,MinAgeValue,MaxAgeValue,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,\r\nID_FeeWay,FeeWayName,RoleName,SecurityLevel,Is_GroupPaused) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}');", new object[]
							{
								ID_Team,
								TeamName,
								ID_TeamTask,
								TeamTaskName,
								array2[0],
								array2[1],
								array2[3],
								array2[5],
								array2[6],
								array2[7],
								array2[8],
								array2[9],
								array2[10],
								array2[11],
								array2[12],
								array2[13],
								array2[15],
								array2[16]
							});
						}
						else
						{
							text += string.Format("INSERT INTO TeamTaskGroup(ID_Team,TeamName,ID_TeamTask,TeamTaskName,TeamTaskGroupName\r\n,Forsex,Is_Marriage,MinAgeValue,MaxAgeValue,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,\r\nID_FeeWay,FeeWayName,RoleName,Is_GroupPaused) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}');", new object[]
							{
								ID_Team,
								TeamName,
								ID_TeamTask,
								TeamTaskName,
								array2[0],
								array2[1],
								array2[3],
								array2[5],
								array2[6],
								array2[7],
								array2[8],
								array2[9],
								array2[10],
								array2[11],
								array2[12],
								array2[13],
								array2[16]
							});
						}
					}
					if (array2[num - 1].Trim() == "1")
					{
						if (array2[9] == "-1")
						{
							if (array2[15] != "-1")
							{
								text += string.Format("UPDATE TeamTaskGroup SET TeamTaskGroupName='{0}'\r\n,Forsex='{1}',Is_Marriage='{2}',MinAgeValue='{3}',MaxAgeValue='{4}',ID_ExamType='{5}',ExamTypeName='{6}',ID_FeeWay='{7}',FeeWayName='{8}',RoleName='{9}',SecurityLevel='{10}',Is_GroupPaused='{11}' WHERE ID_TeamTaskGroup='{10}';", new object[]
								{
									array2[0],
									array2[1],
									array2[3],
									array2[5],
									array2[6],
									array2[7],
									array2[8],
									array2[11],
									array2[12],
									array2[13],
									array2[14],
									array2[15],
									array2[16]
								});
							}
							else if (array2[15] != "-1")
							{
								text += string.Format("UPDATE TeamTaskGroup SET TeamTaskGroupName='{0}'\r\n,Forsex='{1}',Is_Marriage='{2}',MinAgeValue='{3}',MaxAgeValue='{4}',ID_ExamType='{5}',ExamTypeName='{6}',ID_FeeWay='{7}',FeeWayName='{8}',RoleName='{9}',SecurityLevel='{11}',Is_GroupPaused='{12}' WHERE ID_TeamTaskGroup='{10}';", new object[]
								{
									array2[0],
									array2[1],
									array2[3],
									array2[5],
									array2[6],
									array2[7],
									array2[8],
									array2[11],
									array2[12],
									array2[13],
									array2[14],
									array2[15],
									array2[16]
								});
							}
							else
							{
								text += string.Format("UPDATE TeamTaskGroup SET TeamTaskGroupName='{0}'\r\n,Forsex='{1}',Is_Marriage='{2}',MinAgeValue='{3}',MaxAgeValue='{4}',ID_ExamType='{5}',ExamTypeName='{6}',ID_FeeWay='{7}',FeeWayName='{8}',RoleName='{9}',Is_GroupPaused='{11}' WHERE ID_TeamTaskGroup='{10}';", new object[]
								{
									array2[0],
									array2[1],
									array2[3],
									array2[5],
									array2[6],
									array2[7],
									array2[8],
									array2[11],
									array2[12],
									array2[13],
									array2[14],
									array2[16]
								});
							}
						}
						else
						{
							text += string.Format("UPDATE TeamTaskGroup SET TeamTaskGroupName='{0}'\r\n,Forsex='{1}',Is_Marriage='{2}',MinAgeValue='{3}',MaxAgeValue='{4}',ID_ExamType='{5}',ExamTypeName='{6}',PEPackageID='{7}',PEPackageName='{8}',\r\nID_FeeWay='{9}',FeeWayName='{10}',RoleName='{11}',Is_GroupPaused='{13}' WHERE ID_TeamTaskGroup='{12}';", new object[]
							{
								array2[0],
								array2[1],
								array2[3],
								array2[5],
								array2[6],
								array2[7],
								array2[8],
								array2[9],
								array2[10],
								array2[11],
								array2[12],
								array2[13],
								array2[14],
								array2[16]
							});
						}
					}
					sb.AppendLine(string.Format("团体编号[{0}] 团体名称[{1}] 任务编号[{2}] 任务名称[{3}] 分组名称[{4}] 性别[{5}] 婚姻[{6}] 最小年龄[{7}] 最大年龄[{8}] 体检类型编号[{9}] 体检类型名称[{10}] 套餐编号[{11}] 套餐名称[{12}] 收费方式编号[{13}] 收费方式名称[{14}] 角色[{15}]", new object[]
					{
						ID_Team,
						TeamName,
						ID_TeamTask,
						TeamTaskName,
						array2[0],
						array2[1],
						array2[3],
						array2[5],
						array2[6],
						array2[7],
						array2[8],
						array2[9],
						array2[10],
						array2[11],
						array2[12],
						array2[13]
					}));
					if (i != 0 && i % 50 == 0)
					{
						list.Add(text);
						text = string.Empty;
					}
				}
				if (text != string.Empty)
				{
					list.Add(text);
					text = string.Empty;
				}
				int num2 = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				if (num2 > 0)
				{
					DataCache.DeleteCache(this._CacheTeamTaskGroupKey);
				}
				result = num2;
			}
			return result;
		}

		public int DeleteCustomerCustFeeOfBatch(string ID_Customers, string ID_Fees)
		{
			int result = 0;
			if (ID_Customers != string.Empty && ID_Fees != string.Empty)
			{
				string item = string.Format("DELETE FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ID_Fee IN({0}) AND ID_Customer IN({1});", ID_Fees, ID_Customers);
				List<string> list = new List<string>(0);
				list.Add(item);
				result = CommonExcuteSql.Instance.ExecuteSqlTran(list);
			}
			DataCache.DeleteCache(this._CacheUnionBusFee);
			return result;
		}

		public int AddCustomerCustFeeOfBatch(string ID_Operator, string Operator, string ID_Team, string ID_TeamTask, string ID_TeamTaskGroupS, string AllTeamTaskGroupFeeItem)
		{
			int num = 0;
			int result;
			if (AllTeamTaskGroupFeeItem.TrimEnd(new char[]
			{
				'|'
			}) == string.Empty)
			{
				result = -1;
			}
			else
			{
				string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				List<string> list = new List<string>(0);
				string text2 = string.Empty;
				string newValue = string.Empty;
				string newValue2 = string.Empty;
				string text3 = string.Empty;
				string[] array = AllTeamTaskGroupFeeItem.TrimEnd(new char[]
				{
					'|'
				}).Split(new char[]
				{
					'|'
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text4 = array2[i];
					string[] array3 = text4.TrimEnd(new char[]
					{
						'_'
					}).Split(new char[]
					{
						'_'
					});
					text2 = string.Format("IF NOT EXISTS(select ID_Customer FROM OnCustFee WHERE ID_Customer='{0}' AND ID_Fee='{1}')\r\nBEGIN\r\n     @TempInsertSql\r\nEND\r\nELSE \r\nBEGIN\r\n    @TempUpdateSql\r\nEND;", array3[0], array3[1]);
					newValue = string.Format("INSERT INTO OnCustFee(ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,RegistDate\r\n,OriginalPrice,Discount,FactPrice,ID_FeeType,ID_Discounter,DiscounterName,Is_FeeCharged,Is_Printed,ApplyID)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',0,0,'{12}');", new object[]
					{
						array3[0],
						array3[1],
						array3[2],
						ID_Operator,
						Operator,
						text,
						array3[3],
						array3[4],
						array3[5],
						array3[6],
						ID_Operator,
						Operator,
						Customer.CreateMaxApplyID()
					});
					newValue2 = string.Format("update OnCustFee set ID_Fee='{0}',FeeItemName='{1}',OriginalPrice='{2}',Discount='{3}',FactPrice='{4}',ID_FeeType='{5}' where ID_Customer='{6}' AND ID_Fee='{0}';", new object[]
					{
						array3[1],
						array3[2],
						array3[3],
						array3[4],
						array3[5],
						array3[6],
						array3[0]
					});
					text3 += text2.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text3 != string.Empty)
				{
					list.Add(text3);
					text3 = string.Empty;
					if (list.Count > 0)
					{
						string text5 = "1";
						lock (text5)
						{
							num = CommonExcuteSql.Instance.ExecuteSqlTran(list);
						}
						DataCache.DeleteCache(this._CacheUnionBusFee);
						result = num;
						return result;
					}
				}
				result = num;
			}
			return result;
		}

		private string GetdateDiff(string Title, DateTime BeginDate, DateTime EndDate)
		{
			System.TimeSpan ts = new System.TimeSpan(BeginDate.Ticks);
			System.TimeSpan timeSpan = new System.TimeSpan(EndDate.Ticks);
			System.TimeSpan timeSpan2 = timeSpan.Subtract(ts).Duration();
			return string.Concat(new string[]
			{
				Title,
				"耗时:",
				timeSpan2.Days.ToString(),
				"天,",
				timeSpan2.Hours.ToString(),
				"小时,",
				timeSpan2.Minutes.ToString(),
				"分钟,",
				timeSpan2.TotalSeconds.ToString(),
				"秒"
			});
		}

		public int SaveCustomerInfo_New(string ID_Operator, string Operator, string ID_Team, string TeamName, string ID_TeamTask, string ID_TeamTaskGroupS, string AllTeamTaskGroupFeeItem, ref System.Text.StringBuilder sb, ref bool IsOutDefaultMaxTeamSubScribNum)
		{
			IsOutDefaultMaxTeamSubScribNum = false;
			int result;
			if (AllTeamTaskGroupFeeItem.TrimEnd(new char[]
			{
				'|'
			}) == string.Empty)
			{
				result = -1;
			}
			else
			{
				List<string> list = new List<string>();
				string[] array = AllTeamTaskGroupFeeItem.TrimEnd(new char[]
				{
					'|'
				}).Split(new char[]
				{
					'|'
				});
				ID_TeamTaskGroupS = ID_TeamTaskGroupS.TrimEnd(new char[]
				{
					','
				});
				string sql = string.Format("\r\n--获取团体任务分组信息\r\nSELECT ID_TeamTaskGroup,ID_TeamTask,ID_ExamType,ExamTypeName,ID_Team,PEPackageID,PEPackageName,ID_FeeWay,FeeWayName,ISNULL(SecurityLevel,0)SecurityLevel,RoleName FROM TeamTaskGroup WHERE ID_TeamTask='{0}';\r\n--获取团体任务分组收费项目信息\r\nSELECT TeamTaskGroup.ID_Team,TeamTaskGroup.ID_TeamTask,TeamTaskGroup.ID_TeamTaskGroup,CASE WHEN ISNULL(TeamTaskGroupFee.ID_FeeWay,0)=0 THEN TeamTaskGroup.ID_FeeWay ELSE TeamTaskGroupFee.ID_FeeWay END ID_FeeWay,TeamTaskGroupFee.ID_Fee,TeamTaskGroupFee.OriginalPrice,TeamTaskGroupFee.Discount,TeamTaskGroupFee.FactPrice,BusFee.FeeName,SYSSection.ID_Section,SYSSection.SectionName FROM TeamTaskGroupFee\r\nINNER JOIN TeamTaskGroup ON TeamTaskGroupFee.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\nINNER JOIN BusFee ON TeamTaskGroupFee.ID_Fee=BusFee.ID_Fee\r\nINNER JOIN SYSSection ON BusFee.ID_Section=SYSSection.ID_Section\r\nWHERE TeamTaskGroupFee.ID_TeamTaskGroup in({1});\r\nSELECT * FROM TeamTask WHERE ID_TeamTask IN({0});\r\n", ID_TeamTask, ID_TeamTaskGroupS);
				DataSet allDctData = CommonOnArcCust.Instance.GetAllDctData();
				DataTable dataTable = allDctData.Tables[2];
				DataTable dataTable2 = allDctData.Tables[5];
				DataTable dataTable3 = allDctData.Tables[8];
				DataSet dataSet = CommonExcuteSql.Instance.ExcuteSql(sql);
				DataTable dataTable4 = dataSet.Tables[0];
				DataTable dataTable5 = dataSet.Tables[1];
				DataTable dataTable6 = dataSet.Tables[2];
				sb.Length = 0;
				string arg = "汉族";
				string text = string.Empty;
				string empty = string.Empty;
				int num = 1;
				string str = "declare @ID_ArcCustomer int;declare @NationName varchar(10);set @NationName=null;";
				string text2 = string.Empty;
				string empty2 = string.Empty;
				bool flag = false;
				string text3 = string.Empty;
				string str2 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				string text7 = string.Empty;
				string text8 = string.Empty;
				string text9 = string.Empty;
				string text10 = string.Empty;
				string text11 = string.Empty;
				string text12 = string.Empty;
				string text13 = string.Empty;
				string text14 = string.Empty;
				string text15 = string.Empty;
				string text16 = string.Empty;
				string text17 = string.Empty;
				string text18 = string.Empty;
				string text19 = string.Empty;
				System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
				string text20 = string.Empty;
				string text21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				string text22 = text21;
				if (dataTable6.Rows.Count > 0)
				{
					text22 = dataTable6.Rows[0]["TaskExamStartDate"].ToString();
				}
				int num2 = array.Length;
				sb.AppendLine(string.Format("本次备单共计{0}个客户", num2));
				for (int i = 0; i < num2; i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						'_'
					});
					text6 = array2[array2.Length - 1].Trim();
					text3 = array2[6].Trim();
					text8 = array2[9].Trim();
					text10 = array2[11].Trim();
					DataRow[] array3 = dataTable2.Select("GenderName='" + text8 + "'");
					DataRow[] array4 = dataTable.Select("MarriageName='" + text10 + "'");
					text11 = array2[12].Trim();
					text12 = array2[13].Trim();
					text12 = Secret.AES.EncryptPrefix(text12.Trim());
					text13 = array2[7].Trim();
					text20 = array2[1].Trim();
					text14 = array2[2].Trim();
					text15 = array2[3].Trim();
					text16 = array2[4].Trim();
					text17 = array2[5].Trim();
					text18 = array2[15].Trim();
					text19 = array2[16].Trim();
					int num3 = int.Parse(array2[17].Trim());
					DataRow[] array5 = dataTable4.Select("ID_TeamTaskGroup='" + text20 + "'");
					DataRow[] array6 = dataTable5.Select("ID_TeamTaskGroup='" + text20 + "'");
					sb.AppendLine(string.Format("姓名[{0}],性别[{1}],婚姻[{2}],体检号[@ID_Customer],证件号[{3}],出生日期[{4}],联系电话[{5}],部门[{6}],备注[{7}]", new object[]
					{
						text3,
						text8,
						text10,
						text13,
						text11,
						text12,
						string.Concat(new string[]
						{
							text14,
							"-",
							text15,
							"-",
							text16,
							"-",
							text17
						}),
						text18
					}));
					if (array3.Length > 0)
					{
						text7 = array3[0]["ID_Gender"].ToString();
					}
					else
					{
						sb.AppendLine(string.Format("从数据集{0}中获取客户{1}性别{2}失败", "GenderDT", text3, text8));
					}
					if (array4.Length > 0)
					{
						text9 = array4[0]["ID_Marriage"].ToString();
					}
					else
					{
						sb.AppendLine(string.Format("从数据集{0}中获取客户{1}婚姻{2}失败", "MarriageDT", text3, text10));
					}
					if (string.IsNullOrEmpty(array2[14].Trim()))
					{
						sb.AppendLine(string.Format("客户{0}民族未填写,系统默认为{1}", text3, arg));
					}
					else
					{
						text5 = array2[14].Trim();
					}
					if (text6 == string.Empty)
					{
						if (array5.Length > 0)
						{
							text = Customer.CreateMaxNumX(ref empty, 2);
							sb = sb.Replace("@ID_Customer", text);
							if (string.IsNullOrEmpty(text))
							{
								sb.AppendLine(string.Format("用于团体备单的体检号不足", text3, arg));
								IsOutDefaultMaxTeamSubScribNum = true;
								break;
							}
							DataRow[] array7 = dataTable3.Select("NationName='" + text5 + "'");
							if (array7.Length > 0)
							{
								text4 = array7[0]["NationID"].ToString();
								if (!string.IsNullOrEmpty(text9))
								{
									text2 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,ID_Marriage,MarriageName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,NationID,NationName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{7}',ID_Gender='{2}',GenderName='{3}',NationID='{11}',NationName='{12}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text3,
										text13,
										text7,
										text8,
										text9,
										text10,
										text11,
										text12,
										0,
										text21,
										text21,
										text4,
										text5
									});
								}
								else
								{
									text2 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,NationID,NationName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{5}',ID_Gender='{2}',GenderName='{3}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text3,
										text13,
										text7,
										text8,
										text11,
										text12,
										0,
										text21,
										text21,
										text4,
										text5
									});
								}
							}
							else
							{
								text4 = string.Empty;
								text5 = string.Empty;
								if (!string.IsNullOrEmpty(text9))
								{
									text2 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,ID_Marriage,MarriageName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{7}',NationID=NULL,NationName=NULL,ID_Gender='{2}',GenderName='{3}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text3,
										text13,
										text7,
										text8,
										text9,
										text10,
										text11,
										text12,
										0,
										text21,
										text21
									});
								}
								else
								{
									text2 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{7}',NationID=NULL,NationName=NULL,ID_Gender='{2}',GenderName='{3}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text3,
										text13,
										text7,
										text8,
										text11,
										text12,
										0,
										text21,
										text21
									});
								}
							}
							if (array5[0]["PEPackageID"].ToString().Trim() == string.Empty)
							{
								text2 += string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,CustomerName,Is_Team,ID_Team,ID_ExamType,ExamTypeName,ID_FeeWay,FeeWayName,ID_SubScriber,SubScriber,SubScriberOperDate,Note,Is_SectionLock,Is_FeeSettled,TeamName,SubScribDate,SecurityLevel\r\n,ID_Gender,GenderName,ID_Marriage,MarriageName,NationID,NationName,IDCard,BirthDay,MobileNo,Is_Paused,Department,DepartSubA,DepartSubB,DepartSubC)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',0,0,'{12}','{13}','{14}'\r\n,'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}');", new object[]
								{
									text,
									text3,
									1,
									ID_Team,
									array5[0]["ID_ExamType"].ToString(),
									array5[0]["ExamTypeName"].ToString(),
									array5[0]["ID_FeeWay"].ToString(),
									array5[0]["FeeWayName"].ToString(),
									ID_Operator,
									Operator,
									text21,
									text18,
									TeamName,
									text22,
									array5[0]["SecurityLevel"].ToString(),
									text7,
									text8,
									text9,
									text10,
									text4,
									text5,
									text13,
									text11,
									text12,
									num3,
									text14,
									text15,
									text16,
									text17
								});
							}
							else
							{
								text2 += string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,CustomerName,Is_Team,ID_Team,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,ID_FeeWay,FeeWayName,ID_SubScriber,SubScriber,SubScriberOperDate,Note,Is_SectionLock,Is_FeeSettled,TeamName,SubScribDate,SecurityLevel\r\n,ID_Gender,GenderName,ID_Marriage,MarriageName,NationID,NationName,IDCard,BirthDay,MobileNo,Is_Paused,Department,DepartSubA,DepartSubB,DepartSubC)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',0,0,'{14}','{15}','{16}'\r\n,'{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}');", new object[]
								{
									text,
									text3,
									1,
									ID_Team,
									array5[0]["ID_ExamType"].ToString(),
									array5[0]["ExamTypeName"].ToString(),
									array5[0]["PEPackageID"].ToString(),
									array5[0]["PEPackageName"].ToString(),
									array5[0]["ID_FeeWay"].ToString(),
									array5[0]["FeeWayName"].ToString(),
									ID_Operator,
									Operator,
									text21,
									text18,
									TeamName,
									text22,
									array5[0]["SecurityLevel"].ToString(),
									text7,
									text8,
									text9,
									text10,
									text4,
									text5,
									text13,
									text11,
									text12,
									num3,
									text14,
									text15,
									text16,
									text17
								});
							}
							text2 += string.Format("INSERT INTO OnCustRelationCustPEInfo(ID_ArcCustomer,IDCardNo,ExamCardNo,ID_Customer,Is_CompletePhysical,ExamState)\r\nVALUES(@ID_ArcCustomer,'{0}','{1}','{2}',0,0);", text13, string.Empty, text);
							string text23 = string.Empty;
							string a = string.Empty;
							DataRow[] array8 = array6;
							for (int j = 0; j < array8.Length; j++)
							{
								DataRow dataRow = array8[j];
								text23 = Customer.CreateMaxApplyID();
								while (a == text23)
								{
									text23 = Customer.CreateMaxApplyID();
								}
								a = text23;
								if (dataRow["ID_FeeWay"].ToString().Trim() == "2" || dataRow["ID_FeeWay"].ToString().Trim() == "3")
								{
									text2 += string.Format("INSERT INTO OnCustFee(ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,RegistDate\r\n                            ,OriginalPrice,Discount,FactPrice,ID_FeeType,ID_Discounter,DiscounterName,Is_FeeCharged,CustFeeChargeState,Is_Printed,ApplyID)\r\n                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',0,0,'{13}');", new object[]
									{
										text,
										dataRow["ID_Fee"].ToString(),
										dataRow["FeeName"].ToString(),
										ID_Operator,
										Operator,
										text21,
										dataRow["OriginalPrice"].ToString(),
										dataRow["Discount"].ToString(),
										dataRow["FactPrice"].ToString(),
										dataRow["ID_FeeWay"].ToString(),
										ID_Operator,
										Operator,
										0,
										text23
									});
								}
								else
								{
									text2 += string.Format("INSERT INTO OnCustFee(ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,RegistDate\r\n                            ,OriginalPrice,Discount,FactPrice,ID_FeeType,ID_Discounter,DiscounterName,Is_FeeCharged,CustFeeChargeState,Is_Printed,ApplyID)\r\n                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',0,0,'{13}');", new object[]
									{
										text,
										dataRow["ID_Fee"].ToString(),
										dataRow["FeeName"].ToString(),
										ID_Operator,
										Operator,
										text21,
										dataRow["OriginalPrice"].ToString(),
										dataRow["Discount"].ToString(),
										dataRow["FactPrice"].ToString(),
										dataRow["ID_FeeWay"].ToString(),
										ID_Operator,
										Operator,
										0,
										text23
									});
								}
								text2 += string.Format("if not exists(select ID_Section from OnCustExamSection where ID_Customer='{0}' and ID_Section='{1}')\r\n                                                begin\r\n                                                    INSERT INTO OnCustExamSection(ID_Customer,ID_Section,SectionName,CustomerName)\r\n                                                    values('{0}','{1}','{2}','{3}');\r\n                                                end;", new object[]
								{
									text,
									dataRow["ID_Section"].ToString(),
									dataRow["SectionName"].ToString(),
									text3
								});
							}
							text2 += string.Format("INSERT INTO TeamTaskGroupCust(ID_Customer,ID_TeamTaskGroup,ID_TeamTask,Department,DepartSubA,DepartSubB,DepartSubC)\r\n\tVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", new object[]
							{
								text,
								text20,
								ID_TeamTask,
								text14,
								text15,
								text16,
								text17
							});
						}
					}
					else
					{
						text2 += string.Format("UPDATE TeamTaskGroupCust SET ID_TeamTask='{0}',ID_TeamTaskGroup='{1}',Department='{2}',DepartSubA='{3}',DepartSubB='{4}',DepartSubC='{5}' WHERE ID_Customer='{6}';", new object[]
						{
							ID_TeamTask,
							text20,
							text14,
							text15,
							text16,
							text17,
							text6
						});
						DataRow[] array8 = array6;
						for (int j = 0; j < array8.Length; j++)
						{
							DataRow dataRow = array8[j];
							text2 += string.Format("UPDATE OnCustFee SET OriginalPrice='{0}',Discount='{1}',FactPrice='{2}',ID_FeeType='{3}',FeeItemName='{4}' WHERE ID_Fee='{5}' AND ID_Customer='{6}';", new object[]
							{
								dataRow["OriginalPrice"].ToString(),
								dataRow["Discount"].ToString(),
								dataRow["FactPrice"].ToString(),
								dataRow["ID_FeeWay"].ToString(),
								dataRow["FeeName"].ToString(),
								dataRow["ID_Fee"].ToString(),
								text6
							});
							str2 = string.Format("UPDATE OnCustExamSection SET CustomerName='{0}' WHERE ID_Customer='{1}' AND ID_Section='{2}';", text3, text6, dataRow["ID_Section"].ToString());
							if (!flag)
							{
								text2 += str2;
								flag = true;
							}
						}
						text2 += string.Format("UPDATE OnCustRelationCustPEInfo SET IDCardNo='{0}' WHERE ID_Customer='{1}';", text13, text6);
						text2 += string.Format("UPDATE OnCustPhysicalExamInfo SET CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',PEPackageID='{3}',PEPackageName='{4}',ID_FeeWay='{5}',FeeWayName='{6}',SecurityLevel='{8}' WHERE ID_Customer='{7}';", new object[]
						{
							text3,
							array5[0]["ID_ExamType"].ToString(),
							array5[0]["ExamTypeName"].ToString(),
							array5[0]["PEPackageID"].ToString(),
							array5[0]["PEPackageName"].ToString(),
							array5[0]["ID_FeeWay"].ToString(),
							array5[0]["FeeWayName"].ToString(),
							text6,
							array5[0]["SecurityLevel"].ToString()
						});
						DataRow[] array7 = dataTable3.Select("NationName='" + text5 + "'");
						if (array7.Length > 0)
						{
							text4 = array7[0]["NationID"].ToString();
							text2 += string.Format("UPDATE OnArcCust SET IDCard='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',BirthDay='{5}',MobileNo='{6}',NationID='{8}',NationName='{9}' WHERE ID_ArcCustomer=(SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{7}');", new object[]
							{
								text13,
								text7,
								text8,
								text9,
								text10,
								text11,
								text12,
								text6,
								text4,
								text5
							});
						}
						else
						{
							text2 += string.Format("UPDATE OnArcCust SET IDCard='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',BirthDay='{5}',MobileNo='{6}',NationID=NULL,NationName=NULL WHERE ID_ArcCustomer=(SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{7}');", new object[]
							{
								text13,
								text7,
								text8,
								text9,
								text10,
								text11,
								text12,
								text6
							});
						}
					}
					if (i != 0 && i % 50 == 0)
					{
						list.Add(str + text2);
						text2 = string.Empty;
					}
					if (!string.IsNullOrEmpty(text19))
					{
						if (!hashtable.Contains(text13))
						{
							byte[] array9 = Convert.FromBase64String(text19);
							MemoryStream memoryStream = new MemoryStream(array9);
							hashtable.Add(text13, new PEIS.Model.OnArcCust
							{
								IDCard = text13,
								CustomerName = text3,
								Photo = array9
							});
							memoryStream.Close();
							memoryStream.Dispose();
						}
					}
				}
				if (text2 != string.Empty)
				{
					list.Add(str + text2);
					text2 = string.Empty;
				}
				dataTable2.Dispose();
				dataTable.Dispose();
				dataTable4.Dispose();
				dataTable5.Dispose();
				dataSet.Dispose();
				allDctData.Dispose();
				if (list.Count > 0)
				{
					string text24 = "1";
					int num4 = 0;
					List<string> list2 = new List<string>(1);
					lock (text24)
					{
						try
						{
							if (num == 1)
							{
								foreach (string current in list)
								{
									list2.Clear();
									list2.Add(current);
									num4 += CommonExcuteSql.Instance.ExecuteSqlTran(list2);
								}
							}
							else if (num == 0)
							{
								num4 += CommonExcuteSql.Instance.ExecuteSqlTran(list);
							}
						}
						catch (System.Exception ex)
						{
							sb.Length = 0;
							sb.Append(string.Format("执行批量生成时出现异常:{0}", ex.Message));
							num4 = -200;
						}
					}
					foreach (System.Collections.DictionaryEntry dictionaryEntry in hashtable)
					{
						DateTime now = DateTime.Now;
						PEIS.Model.OnArcCust onArcCust = dictionaryEntry.Value as PEIS.Model.OnArcCust;
						if (onArcCust != null)
						{
							int num5 = CommonUser.Instance.UpdateCustomerPicInfo(onArcCust);
							if (num5 > 0)
							{
								sb.Append(string.Concat(new string[]
								{
									"保存客户头像信息 姓名：",
									onArcCust.CustomerName,
									",证件号:",
									onArcCust.IDCard,
									" ",
									Public.GetDateDiff(string.Empty, now, DateTime.Now)
								}));
							}
							else
							{
								sb.Append(string.Concat(new string[]
								{
									"保存客户头像信息 姓名：",
									onArcCust.CustomerName,
									",证件号:",
									onArcCust.IDCard,
									" ",
									Public.GetDateDiff(string.Empty, now, DateTime.Now)
								}));
							}
						}
					}
					result = num4;
				}
				else
				{
					result = -1;
				}
			}
			return result;
		}

		public int SaveInternatCustomerInfo(string ID_Operator, string Operator, string AllTeamTaskGroupFeeItem, ref System.Text.StringBuilder sb, ref bool IsOutDefaultMaxTeamSubScribNum, ref string Orders)
		{
			int result;
			if (AllTeamTaskGroupFeeItem.TrimEnd(new char[]
			{
				'|'
			}) == string.Empty)
			{
				result = -1;
			}
			else
			{
				Orders = string.Empty;
				List<string> list = new List<string>();
				string[] array = AllTeamTaskGroupFeeItem.TrimEnd(new char[]
				{
					'|'
				}).Split(new char[]
				{
					'|'
				});
				string empty = string.Empty;
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add("ID_Customer");
				dataTable.Columns.Add("UserOrderNO");
				dataTable.Columns.Add("ID_UserOrderDetail");
				dataTable.Columns.Add("ID_ExamSet");
				dataTable.Columns.Add("ExamPEPackageName");
				dataTable.Columns.Add("CustomerName");
				dataTable.Columns.Add("GenderName");
				dataTable.Columns.Add("IDCard");
				dataTable.Columns.Add("BirthDay");
				dataTable.Columns.Add("CustomerTel");
				dataTable.Columns.Add("ApplyrExamDate");
				dataTable.Columns.Add("Operator");
				dataTable.Columns.Add("ErrorMsg");
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				string text7 = string.Empty;
				string text8 = string.Empty;
				string value = string.Empty;
				string text9 = string.Empty;
				string text10 = string.Empty;
				string text11 = string.Empty;
				string text12 = string.Empty;
				int num = 1;
				DateTime dateTime = DateTime.Now;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text13 = array2[i];
					if (!string.IsNullOrEmpty(text13.Trim()))
					{
						text12 = string.Empty;
						string[] array3 = text13.Split(new char[]
						{
							'_'
						});
						text2 = array3[0].Trim();
						text3 = array3[1].Trim();
						Orders = Orders + "'" + text3 + "',";
						text4 = array3[2].Trim();
						text5 = array3[3].Trim();
						text6 = array3[4].Trim();
						text7 = array3[5].Trim();
						text8 = array3[6].Trim();
						value = array3[7].Trim();
						text9 = array3[8].Trim();
						text10 = array3[9].Trim();
						text11 = array3[10].Trim();
						if (string.IsNullOrEmpty(text3))
						{
							text12 += "订单号为空，";
						}
						if (string.IsNullOrEmpty(text4))
						{
							text12 += "客户编号为空，";
						}
						if (string.IsNullOrEmpty(text5))
						{
							text12 += "套餐为空，";
						}
						if (string.IsNullOrEmpty(text7))
						{
							text12 += "客户名称为空，";
						}
						if (string.IsNullOrEmpty(text8))
						{
							text12 += "客户性别为空，";
						}
						if (string.IsNullOrEmpty(value))
						{
							text12 += "证件号为空，";
						}
						if (string.IsNullOrEmpty(text9))
						{
							text12 += "出生日期为空，";
						}
						if (string.IsNullOrEmpty(text10))
						{
							text12 += "电话号码为空，";
						}
						if (string.IsNullOrEmpty(text11))
						{
							text12 += "预约体检日期为空，";
						}
						try
						{
							dateTime = DateTime.Parse(text9);
						}
						catch (System.Exception ex)
						{
							text12 += "出生日期格式不正确，";
						}
						try
						{
							dateTime = DateTime.Parse(text11);
						}
						catch (System.Exception ex)
						{
							text12 += "预约体检日期格式不正确，";
						}
						if (text12.Length > 0)
						{
							text12 = string.Concat(new object[]
							{
								"行",
								num,
								" ",
								text12.TrimEnd(new char[]
								{
									','
								})
							});
						}
						else
						{
							text = text + "'" + text5 + "',";
						}
						DataRow dataRow = dataTable.NewRow();
						dataRow["ID_Customer"] = text2;
						dataRow["UserOrderNO"] = text3;
						dataRow["ID_UserOrderDetail"] = text4;
						dataRow["ID_ExamSet"] = text5;
						dataRow["ExamPEPackageName"] = text6;
						dataRow["CustomerName"] = text7;
						dataRow["GenderName"] = text8;
						dataRow["IDCard"] = value;
						dataRow["BirthDay"] = text9;
						dataRow["CustomerTel"] = text10;
						dataRow["ApplyrExamDate"] = text11;
						dataRow["ErrorMsg"] = text12;
						dataTable.Rows.Add(dataRow);
					}
				}
				string str = string.Format("SELECT PEPackageID,ID_FeeItem FROM BusSetFeeDetail WHERE PEPackageID IN({0});", text.TrimEnd(new char[]
				{
					','
				}));
				string text14 = string.Format("SELECT '@ID_User' ID_Discounter,'@userName' DiscounterName,'@ID_User' ID_Register,'@userName' RegisterName,'@date' RegistDate,0 Is_Printed,0 Is_FeeRefund,0 Is_FeeCharged,0 FeeChargeStaute,'' ID_TeamTaskGroup,BusFee.ID_Section,BusFee.SectionName,'' ID_CustFee,'' ID_Customer,BusFee.ID_Fee,BusFee.FeeName,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,0 CustFeeChargeState,0 Is_FeeCharged,0 Is_FeeRefund,0 FeeChargeStaute,OperationalDate,BusFee.ID_Fee ID_Fee FROM BusFee\r\ninner join SYSSection on BusFee.ID_Section =SYSSection.ID_Section\r\nWHERE isnull(BusFee.Is_Banned,0)!=1 and BusFee.ID_Fee in(SELECT ID_FeeItem FROM BusSetFeeDetail WHERE PEPackageID IN({0}));", text.TrimEnd(new char[]
				{
					','
				}));
				text14 = text14.Replace("@ID_User", ID_Operator);
				text14 = text14.Replace("@userName", Operator);
				text14 = text14.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
				text14 = text14.Replace("@Discount", "10");
				DataSet dataSet = CommonExcuteSql.Instance.ExcuteSql(str + text14);
				DataTable dataTable2 = dataSet.Tables[0];
				DataTable dataTable3 = dataSet.Tables[1];
				DataSet allDctData = CommonOnArcCust.Instance.GetAllDctData();
				DataTable dataTable4 = allDctData.Tables[2];
				DataTable dataTable5 = allDctData.Tables[5];
				DataTable dataTable6 = allDctData.Tables[8];
				DataTable dataTable7 = allDctData.Tables[3];
				sb.Length = 0;
				string text15 = "";
				string empty2 = string.Empty;
				int num2 = 1;
				string str2 = "declare @ID_ArcCustomer int;declare @NationName varchar(10);set @NationName=null;";
				string text16 = string.Empty;
				string empty3 = string.Empty;
				string empty4 = string.Empty;
				string text17 = string.Empty;
				string text18 = string.Empty;
				string empty5 = string.Empty;
				string text19 = string.Empty;
				string empty6 = string.Empty;
				string empty7 = string.Empty;
				string text20 = string.Empty;
				string text21 = string.Empty;
				string empty8 = string.Empty;
				string empty9 = string.Empty;
				string empty10 = string.Empty;
				string empty11 = string.Empty;
				string empty12 = string.Empty;
				System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
				string empty13 = string.Empty;
				string text22 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				string text23 = "1";
				string text24 = "现金";
				int count = dataTable.Rows.Count;
				sb.AppendLine(string.Format("本次备单共计{0}个客户", count));
				int num3 = -1;
				string text25 = string.Empty;
				string filterExpression = string.Empty;
				foreach (DataRow dataRow2 in dataTable.Rows)
				{
					num3++;
					text12 = dataRow2["ErrorMsg"].ToString();
					if (string.IsNullOrEmpty(text12))
					{
						DataRow[] array4 = dataTable5.Select("GenderName='" + dataRow2["GenderName"].ToString() + "'");
						DataRow[] array5 = dataTable7.Select("PEPackageID='" + dataRow2["ID_ExamSet"].ToString() + "'");
						text2 = dataRow2["ID_Customer"].ToString();
						text3 = dataRow2["UserOrderNO"].ToString();
						text4 = dataRow2["ID_UserOrderDetail"].ToString();
						text5 = dataRow2["ID_ExamSet"].ToString();
						text6 = dataRow2["ExamPEPackageName"].ToString();
						if (array5.Length > 0)
						{
							text6 = array5[0]["PEPackageName"].ToString();
						}
						else
						{
							sb.AppendLine(string.Format("从数据集{0}中获取套餐{1}套餐名称{2}失败", "BusPEPackageDT", text5, text6));
						}
						text7 = dataRow2["CustomerName"].ToString();
						text8 = dataRow2["GenderName"].ToString();
						text21 = dataRow2["IDCard"].ToString();
						text9 = dataRow2["BirthDay"].ToString();
						text10 = dataRow2["CustomerTel"].ToString();
						text20 = text10;
						text11 = dataRow2["ApplyrExamDate"].ToString();
						text18 = string.Empty;
						sb.AppendLine(string.Format("姓名[{0}],性别[{1}],婚姻[{2}],体检号[@ID_Customer],证件号[{3}],出生日期[{4}],联系电话[{5}],部门[{6}],备注[{7}]", new object[]
						{
							text7,
							text8,
							string.Empty,
							text21,
							text9,
							text20,
							string.Concat(new string[]
							{
								empty8,
								"-",
								empty9,
								"-",
								empty10,
								"-",
								empty11
							}),
							string.Empty
						}));
						if (array4.Length > 0)
						{
							text19 = array4[0]["ID_Gender"].ToString();
						}
						else
						{
							sb.AppendLine(string.Format("从数据集{0}中获取客户{1}性别{2}失败", "GenderDT", text7, text8));
						}
						if (string.IsNullOrEmpty(text18))
						{
							sb.AppendLine(string.Format("客户{0}民族未填写,系统默认为{1}", text7, text15));
							text18 = text15;
						}
						if (string.IsNullOrEmpty(text2))
						{
							text2 = Customer.CreateMaxNumX(ref empty2, 3);
							sb = sb.Replace("@ID_Customer", text2);
							if (string.IsNullOrEmpty(text2))
							{
								sb.AppendLine(string.Format("用于团体备单的体检号不足", text7, text15));
								IsOutDefaultMaxTeamSubScribNum = true;
								break;
							}
							DataRow[] array6 = dataTable6.Select("NationName='" + text18 + "'");
							if (array6.Length > 0)
							{
								text17 = array6[0]["NationID"].ToString();
								if (!string.IsNullOrEmpty(empty6))
								{
									text16 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,ID_Marriage,MarriageName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,NationID,NationName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{7}',ID_Gender='{2}',GenderName='{3}',NationID='{11}',NationName='{12}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text7,
										text21,
										text19,
										text8,
										empty6,
										string.Empty,
										text9,
										text20,
										0,
										text22,
										text22,
										text17,
										text18
									});
								}
								else
								{
									text16 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,NationID,NationName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{5}',ID_Gender='{2}',GenderName='{3}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text7,
										text21,
										text19,
										text8,
										text9,
										text20,
										0,
										text22,
										text22,
										text17,
										text18
									});
								}
							}
							else
							{
								text17 = string.Empty;
								text18 = string.Empty;
								if (!string.IsNullOrEmpty(empty6))
								{
									text16 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,ID_Marriage,MarriageName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{7}',NationID=NULL,NationName=NULL,ID_Gender='{2}',GenderName='{3}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text7,
										text21,
										text19,
										text8,
										empty6,
										empty7,
										text9,
										text20,
										0,
										text22,
										text22
									});
								}
								else
								{
									text16 += string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{1}' AND CustomerName='{0}')\r\n            begin\r\n                INSERT INTO OnArcCust(CustomerName,IDCard,ID_Gender,GenderName,BirthDay,MobileNo,FinishedNum,FirstDatePE,LatestDatePE,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',1);\r\nSELECT @ID_ArcCustomer=SCOPE_IDENTITY();\r\n            end\r\n            else \r\n            begin\r\n                SELECT @ID_ArcCustomer=ID_ArcCustomer FROM OnArcCust WHERE CustomerName='{0}' AND IDCard='{1}';\r\n                UPDATE OnArcCust SET MobileNo='{7}',NationID=NULL,NationName=NULL,ID_Gender='{2}',GenderName='{3}' WHERE CustomerName='{0}' AND IDCard='{1}';\r\n            end;", new object[]
									{
										text7,
										text21,
										text19,
										text8,
										text9,
										text20,
										0,
										text22,
										text22
									});
								}
							}
							if (!string.IsNullOrEmpty(text5.Trim()))
							{
								text16 += string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,CustomerName,ID_FeeWay,FeeWayName,ID_SubScriber,SubScriber,SubScriberOperDate\r\n,SubScribDate,ID_Gender,GenderName,NationID,NationName,IDCard,BirthDay,MobileNo,PEPackageID,PEPackageName,Is_Subscribed,Is_InternatSubscribe,Is_Team,SecurityLevel,CustomerOrderNO)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}',1,1,0,1,'{17}');", new object[]
								{
									text2,
									text7,
									text23,
									text24,
									ID_Operator,
									Operator,
									text22,
									text11,
									text19,
									text8,
									text17,
									text18,
									text21,
									text9,
									text20,
									text5,
									text6,
									text3
								});
							}
							else
							{
								text16 += string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,CustomerName,ID_FeeWay,FeeWayName,ID_SubScriber,SubScriber,SubScriberOperDate\r\n,SubScribDate,ID_Gender,GenderName,NationID,NationName,IDCard,BirthDay,MobileNo,Is_Subscribed,Is_InternatSubscribe,Is_Team,SecurityLevel,CustomerOrderNO)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',1,1,0,1,'{15}');", new object[]
								{
									text2,
									text7,
									text23,
									text24,
									ID_Operator,
									Operator,
									text22,
									text11,
									text19,
									text8,
									text17,
									text18,
									text21,
									text9,
									text20,
									text3
								});
							}
							text16 += string.Format("INSERT INTO OnCustRelationCustPEInfo(ID_ArcCustomer,IDCardNo,ExamCardNo,ID_Customer,Is_CompletePhysical,ExamState)\r\nVALUES(@ID_ArcCustomer,'{0}','{1}','{2}',0,0);", text21, string.Empty, text2);
							text16 += string.Format("if not exists(select ID_Customer from OnInternetDataImportLog where ID_Customer='{2}')\r\n            begin\r\nINSERT INTO OnInternetDataImportLog(CustomerOrderNO,OrderDetail,ID_Customer,ID_Operator,Operator,OperateDate,Note)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}');\r\n            end;", new object[]
							{
								text3,
								text4,
								text2,
								ID_Operator,
								Operator,
								text22,
								"网上预约数据导入"
							});
							text16 += string.Format("if not exists(select ID_Customer from OnCustBackLog where ID_Customer='{0}' AND ID_BackLogType='{1}')\r\n                                    begin\r\n                        INSERT INTO OnCustBackLog(ID_Customer,ID_BackLogType,CreateDate,OperateDate,Is_Finished,ID_Operator,Operator)\r\n                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}');\r\n                                    end; else begin update OnCustBackLog set OperateDate='{3}',ID_Operator='{5}',Operator='{6}' where ID_Customer='{0}' AND ID_BackLogType='{1}' end            \r\n", new object[]
							{
								text2,
								1,
								text22,
								text11,
								1,
								ID_Operator,
								Operator
							});
							string text26 = string.Empty;
							string a = string.Empty;
							DataRow[] array7 = dataTable2.Select(string.Format("PEPackageID='{0}'", text5));
							if (array7.Length > 0)
							{
								text25 = string.Empty;
								DataRow[] array8 = array7;
								for (int i = 0; i < array8.Length; i++)
								{
									DataRow dataRow3 = array8[i];
									text25 = text25 + "'" + dataRow3["ID_FeeItem"].ToString() + "',";
								}
								text25 = text25.TrimEnd(new char[]
								{
									','
								});
								filterExpression = string.Format("ID_Fee IN({0})", text25);
								DataRow[] array9 = dataTable3.Select(filterExpression);
								array8 = array9;
								for (int i = 0; i < array8.Length; i++)
								{
									DataRow dataRow4 = array8[i];
									text26 = Customer.CreateMaxApplyID();
									while (a == text26)
									{
										text26 = Customer.CreateMaxApplyID();
									}
									a = text26;
									text16 += string.Format("INSERT INTO OnCustFee(ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,RegistDate\r\n                            ,OriginalPrice,Discount,FactPrice,ID_FeeType,ID_Discounter,DiscounterName,Is_FeeCharged,CustFeeChargeState,Is_Printed,ApplyID)\r\n                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',0,0,'{13}');", new object[]
									{
										text2,
										dataRow4["ID_Fee"].ToString(),
										dataRow4["FeeName"].ToString(),
										ID_Operator,
										Operator,
										text22,
										dataRow4["Price"].ToString(),
										dataRow4["Discount"].ToString(),
										dataRow4["FactPrice"].ToString(),
										text23,
										ID_Operator,
										Operator,
										0,
										text26
									});
									text16 += string.Format("if not exists(select ID_Section from OnCustExamSection where ID_Customer='{0}' and ID_Section='{1}')\r\n                                                begin\r\n                                                    INSERT INTO OnCustExamSection(ID_Customer,ID_Section,SectionName,CustomerName)\r\n                                                    values('{0}','{1}','{2}','{3}');\r\n                                                end;", new object[]
									{
										text2,
										dataRow4["ID_Section"].ToString(),
										dataRow4["SectionName"].ToString(),
										text7
									});
								}
							}
						}
						if (num3 != 0 && num3 % 50 == 0)
						{
							list.Add(str2 + text16);
							text16 = string.Empty;
						}
						if (!string.IsNullOrEmpty(empty12))
						{
							if (!hashtable.Contains(text21))
							{
								byte[] array10 = Convert.FromBase64String(empty12);
								MemoryStream memoryStream = new MemoryStream(array10);
								hashtable.Add(text21, new PEIS.Model.OnArcCust
								{
									IDCard = text21,
									CustomerName = text7,
									Photo = array10
								});
								memoryStream.Close();
								memoryStream.Dispose();
							}
						}
					}
				}
				if (text16 != string.Empty)
				{
					list.Add(str2 + text16);
					text16 = string.Empty;
				}
				dataTable5.Dispose();
				dataTable4.Dispose();
				allDctData.Dispose();
				if (list.Count > 0)
				{
					string text27 = "1";
					int num4 = 0;
					List<string> list2 = new List<string>(1);
					lock (text27)
					{
						try
						{
							if (num2 == 1)
							{
								foreach (string current in list)
								{
									list2.Clear();
									list2.Add(current);
									num4 += CommonExcuteSql.Instance.ExecuteSqlTran(list2);
								}
							}
							else if (num2 == 0)
							{
								num4 += CommonExcuteSql.Instance.ExecuteSqlTran(list);
							}
						}
						catch (System.Exception ex)
						{
							sb.Length = 0;
							sb.Append(string.Format("执行批量生成时出现异常:{0}", ex.Message));
							num4 = -200;
						}
					}
					foreach (System.Collections.DictionaryEntry dictionaryEntry in hashtable)
					{
						DateTime now = DateTime.Now;
						PEIS.Model.OnArcCust onArcCust = dictionaryEntry.Value as PEIS.Model.OnArcCust;
						if (onArcCust != null)
						{
							int num5 = CommonUser.Instance.UpdateCustomerPicInfo(onArcCust);
							if (num5 > 0)
							{
								sb.Append(string.Concat(new string[]
								{
									"保存客户头像信息 姓名：",
									onArcCust.CustomerName,
									",证件号:",
									onArcCust.IDCard,
									" ",
									Public.GetDateDiff(string.Empty, now, DateTime.Now)
								}));
							}
							else
							{
								sb.Append(string.Concat(new string[]
								{
									"保存客户头像信息 姓名：",
									onArcCust.CustomerName,
									",证件号:",
									onArcCust.IDCard,
									" ",
									Public.GetDateDiff(string.Empty, now, DateTime.Now)
								}));
							}
						}
					}
					result = num4;
				}
				else
				{
					result = -1;
				}
			}
			return result;
		}

		public int SaveTeamTaskGroupFee(string AllTeamTaskGroupFeeItem, ref System.Text.StringBuilder sb)
		{
			sb.Length = 0;
			int result;
			if (AllTeamTaskGroupFeeItem.TrimEnd(new char[]
			{
				'|'
			}) == string.Empty)
			{
				result = -1;
			}
			else
			{
				List<string> list = new List<string>();
				string[] array = AllTeamTaskGroupFeeItem.TrimEnd(new char[]
				{
					'|'
				}).Split(new char[]
				{
					'|'
				});
				string text = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].TrimEnd(new char[]
					{
						'_'
					}).Split(new char[]
					{
						'_'
					});
					text += string.Format("IF NOT EXISTS(SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup='{0}' AND ID_Fee='{1}')\r\n                                        BEGIN\r\n\t                                        INSERT INTO TeamTaskGroupFee(ID_TeamTaskGroup,ID_Fee,OriginalPrice,Discount,FactPrice,ID_FeeWay,FeeWayName) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}');\r\n                                        END\r\n                                        ELSE \r\n                                        BEGIN\r\n\t                                        UPDATE TeamTaskGroupFee SET OriginalPrice='{2}',Discount='{3}',FactPrice='{4}',ID_FeeWay='{5}',FeeWayName='{6}' WHERE ID_TeamTaskGroup='{0}' AND ID_Fee='{1}';\r\nUPDATE OnCustFee SET OriginalPrice='{2}',Discount='{3}',FactPrice='{4}' WHERE ID_Customer IN(SELECT ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup='{0}') AND ID_Fee='{1}';\r\n                                        END;", new object[]
					{
						array2[0],
						array2[1],
						array2[2],
						array2[3],
						array2[4],
						array2[5],
						array2[6]
					});
					sb.AppendLine(string.Format("任务编号[{0}] 收费项目编号[{1}] 原价[{2}] 折扣[{3}] 实价[{4}]", new object[]
					{
						array2[0],
						array2[1],
						array2[2],
						array2[3],
						array2[4]
					}));
					if (i != 0 && i % 50 == 0)
					{
						list.Add(text);
						text = string.Empty;
					}
				}
				if (text != string.Empty)
				{
					list.Add(text);
					text = string.Empty;
				}
				int num = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				if (num > 0)
				{
					DataCache.DeleteCache(this._CacheTeamTaskGroupKey);
				}
				result = num;
			}
			return result;
		}

		public DataTable GetTeamInfoByKeyWords(string InputCode)
		{
			object cache = DataCache.GetCache(this._CacheTeamKey);
			DataTable dataTable = null;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string sql = "SELECT ID_Team,TeamName,InputCode,Note FROM Team;";
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._CacheTeamKey, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			DataTable dataTable2 = dataTable.Clone();
			DataTable dataTable3 = dataTable.Clone();
			DataTable result;
			if (InputCode == string.Empty)
			{
				result = dataTable;
			}
			else
			{
				DataRow[] array = dataTable.Select(" InputCode like '%" + InputCode + "%'");
				InputCode = InputCode.Trim().ToLower();
				if (array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (dataRow["InputCode"].ToString().Trim().ToLower().StartsWith(InputCode))
						{
							dataTable2.ImportRow(dataRow);
						}
						else
						{
							dataTable3.ImportRow(dataRow);
						}
					}
				}
				foreach (DataRow dataRow in dataTable3.Rows)
				{
					//DataRow dataRow;
					dataTable2.ImportRow(dataRow);
				}
				dataTable3.Dispose();
				dataTable.Dispose();
				result = dataTable2;
			}
			return result;
		}

		public DataTable StatsUserInfoByRight(string RightID)
		{
			object cache = DataCache.GetCache(this._UserInfoKey);
			DataTable dataTable = null;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else if (!string.IsNullOrEmpty(RightID))
			{
				RightID = RightID.TrimStart(new char[]
				{
					','
				}).TrimEnd(new char[]
				{
					','
				});
				string sql = string.Format(" WITH\r\n   ALLDATA as (SELECT * \r\n            FROM [SysRoleRight]),\r\n            SUBDATA as ( \r\n\t        SELECT [RoleRightID],[RightID],[RoleID],[OperatorID],[CreateDate] FROM [SysRoleRight] WHERE [RightID] IN({0})\r\n            UNION all\r\n            Select ALLDATA.* FROM SUBDATA inner join ALLDATA\r\n            ON SUBDATA.RightID = ALLDATA.RightID\r\n            )\r\nSELECT [UserID],[UserName] FROM [SYSOpUser] WHERE UserID IN (\r\n\tSELECT UserID FROM SYSUserRole\r\n\tWHERE RoleID IN (\r\n\t\tSELECT [RoleID] FROM [SysRoleRight]\r\n\t\twhere RightID IN ({0}) \r\n\t\tor RightID IN(SELECT [RightID] FROM SUBDATA ))\r\n\tunion \r\n\tSELECT UserID FROM SYSUserRight\r\n\tWHERE  RightID in ({0})\r\n\tunion \r\n\tSELECT UserID FROM SYSUserRight\r\n\tWHERE  RightID in ( SELECT [RightID] FROM SUBDATA )\r\n)\r\nORDER BY UserName ASC", RightID);
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._UserInfoKey, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			return dataTable;
		}

		public DataTable StatsFeeName()
		{
			object cache = DataCache.GetCache(this._BusFeeInfoKey);
			DataTable dataTable;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string sql = "SELECT ID_Fee,FeeName,InputCode FROM BusFee ORDER BY FeeName";
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._BusFeeInfoKey, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			return dataTable;
		}

		public DataTable GetTeamTaskInfoByKeyWords(string InputCode, string ID_Team)
		{
			object cache = DataCache.GetCache(this._CacheTeamTaskKey);
			DataTable dataTable = null;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string sql = "SELECT CONVERT(varchar(10),CreateDate,120) CreateDate,ID_Team P_ID_Team,ID_TeamTask ID_Team,TeamTaskName TeamName,InputCode InputCode FROM TeamTask ORDER BY DispOrder;";
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._CacheTeamTaskKey, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			DataTable dataTable2 = dataTable.Clone();
			DataTable dataTable3 = dataTable.Clone();
			DataTable result;
			if (InputCode == string.Empty)
			{
				result = dataTable;
			}
			else
			{
				DataRow[] array = dataTable.Select(string.Concat(new string[]
				{
					" InputCode like '%",
					InputCode,
					"%' AND P_ID_Team='",
					ID_Team,
					"'"
				}));
				InputCode = InputCode.Trim().ToLower();
				if (array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (dataRow["InputCode"].ToString().Trim().ToLower().StartsWith(InputCode))
						{
							dataTable2.ImportRow(dataRow);
						}
						else
						{
							dataTable3.ImportRow(dataRow);
						}
					}
				}
				foreach (DataRow dataRow in dataTable3.Rows)
				{
					//DataRow dataRow;
					dataTable2.ImportRow(dataRow);
				}
				dataTable3.Dispose();
				dataTable.Dispose();
				result = dataTable2;
			}
			return result;
		}

		public DataSet GetTeamTaskGroupCustInfoOfTeamBath(string ID_Team, string ID_TeamTask, string ID_TeamTaskGroupS)
		{
			string sql = string.Format("SELECT * FROM TeamTaskGroup WHERE ID_Team='{0}' AND TeamTaskGroup.ID_TeamTask='{1}';\r\n--SELECT 1 exist,ISNULL(OnCustPhysicalExamInfo.Is_ExamStarted,Is_ExamStarted)Is_ExamStarted,OnArcCust.NationID,OnArcCust.NationName,OnArcCust.MobileNo CustomerTel,OnArcCust.IDCard,OnArcCust.CustomerName,OnArcCust.ID_Gender,OnArcCust.GenderName,OnArcCust.ID_Marriage,OnArcCust.MarriageName,CONVERT(varchar(10),OnArcCust.BirthDay,120) CustomerBirthDay,TeamTaskGroup.RoleName CustomerRoleName,TeamTaskGroup.ID_Team,TeamTaskGroup.ID_TeamTask,TeamTaskGroup.ID_TeamTaskGroup,TeamTaskGroup.TeamTaskGroupName,TeamTaskGroupCust.ID_TeamTaskGroupCustomer,TeamTaskGroupCust.ID_Customer,TeamTaskGroupCust.Department DepartmentX,TeamTaskGroupCust.DepartSubA DepartmentA,TeamTaskGroupCust.DepartSubB DepartmentB,TeamTaskGroupCust.DepartSubC DepartmentC FROM TeamTaskGroupCust\r\n--INNER JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\n--INNER JOIN OnCustRelationCustPEInfo ON TeamTaskGroupCust.ID_Customer=OnCustRelationCustPEInfo.ID_Customer\r\n--INNER JOIN OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\n--INNER JOIN OnArcCust ON OnCustRelationCustPEInfo.ID_ArcCustomer=OnArcCust.ID_ArcCustomer\r\n--WHERE TeamTaskGroup.ID_Team='{0}' AND TeamTaskGroup.ID_TeamTask='{1}';", ID_Team, ID_TeamTask);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetTeamTaskGroupCustInfo(string ID_Team, string ID_TeamTask, string ID_TeamTaskGroupS)
		{
			string sql = string.Format("SELECT *,CASE Forsex WHEN 0 THEN '女' WHEN 1 THEN '男' ELSE '' END GenderName,CASE Is_Marriage WHEN 0 THEN '未婚' WHEN 1 THEN '已婚' WHEN 2 THEN '视为已婚' ELSE '' END MarriageName FROM TeamTaskGroup WHERE ID_Team='{0}' AND TeamTaskGroup.ID_TeamTask='{1}';", ID_Team, ID_TeamTask);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataTable GetTeamTaskGroupCustomerInfo(string ID_Team, string ID_TeamTask, string ID_TeamTaskGroup)
		{
			string sql = string.Format("SELECT 1 exist,OnCustPhysicalExamInfo.Note,OnArcCust.NationID,OnArcCust.NationName,OnArcCust.MobileNo CustomerTel,OnArcCust.IDCard,OnArcCust.CustomerName,OnArcCust.ID_Gender,OnArcCust.GenderName,OnArcCust.ID_Marriage,OnArcCust.MarriageName,CONVERT(varchar(10),OnArcCust.BirthDay,120) CustomerBirthDay,TeamTaskGroup.RoleName CustomerRoleName,TeamTaskGroup.ID_Team,TeamTaskGroup.ID_TeamTask,TeamTaskGroup.ID_TeamTaskGroup,TeamTaskGroup.TeamTaskGroupName,TeamTaskGroupCust.ID_TeamTaskGroupCustomer,TeamTaskGroupCust.ID_Customer,TeamTaskGroupCust.Department DepartmentX,TeamTaskGroupCust.DepartSubA DepartmentA,TeamTaskGroupCust.DepartSubB DepartmentB,TeamTaskGroupCust.DepartSubC DepartmentC FROM TeamTaskGroupCust\r\nINNER JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\nINNER JOIN OnCustPhysicalExamInfo ON TeamTaskGroupCust.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nINNER JOIN OnCustRelationCustPEInfo ON TeamTaskGroupCust.ID_Customer=OnCustRelationCustPEInfo.ID_Customer\r\nINNER JOIN OnArcCust ON OnCustRelationCustPEInfo.ID_ArcCustomer=OnArcCust.ID_ArcCustomer\r\nWHERE TeamTaskGroup.ID_Team='{0}' AND TeamTaskGroup.ID_TeamTask='{1}' AND TeamTaskGroup.ID_TeamTaskGroup IN({2}) ORDER BY TeamTaskGroupCust.ID_Customer DESC;", ID_Team, ID_TeamTask, ID_TeamTaskGroup);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable GetInternatCustomerInfo(string Orders)
		{
			string sql = string.Format("SELECT 1 exist,ID_InternetDataImportLog,CustomerOrderNO,OrderDetail,ID_Customer,Operator,OperateDate FROM OnInternetDataImportLog WHERE CustomerOrderNO IN({0})", Orders);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataSet GetTeamTaskGroupCustomerCustInfo(string UserName, string ID_Team, string ID_TeamTask, string ID_TeamTaskGroupS, string ID_Customer)
		{
			string text = string.Format("SELECT ISNULL(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,1 exist,TeamTaskGroupCust.ID_TeamTaskGroup,DictFeeWay.FeeWayName,TeamTaskGroup.TeamTaskGroupName,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,OnCustFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,OnCustFee.OriginalPrice Price,OnCustFee.Discount Discount,OnCustFee.FactPrice FactPrice\r\nFROM (SELECT * FROM OnCustFee WHERE ID_Customer IN({0}))OnCustFee\r\nINNER JOIN DictFeeWay ON OnCustFee.ID_FeeType=DictFeeWay.ID_FeeWay\r\nINNER JOIN TeamTaskGroupCust ON OnCustFee.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nINNER JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\nINNER JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee WHERE isnull(BusFee.Is_Banned,0)!=1;", ID_Customer);
			text = text.Replace("@where", string.Empty);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			return CommonExcuteSql.Instance.ExcuteSql(text);
		}

		public DataTable GetTeamTaskGroupFeeDataByGroupID(string ID_TeamTaskGroupS, string UserName)
		{
			string text = "SELECT -1 CustFeeChargeState,1 exist,TeamTaskGroupFee.ID_TeamTaskGroup,TeamTaskGroupFee.ID_FeeWay,TeamTaskGroupFee.FeeWayName,TeamTaskGroup.TeamTaskGroupName,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,TeamTaskGroupFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,TeamTaskGroupFee.OriginalPrice Price,TeamTaskGroupFee.Discount Discount,TeamTaskGroupFee.FactPrice FactPrice\r\nFROM TeamTaskGroupFee \r\nINNER JOIN TeamTaskGroup ON TeamTaskGroupFee.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\nINNER JOIN BusFee ON TeamTaskGroupFee.ID_Fee=BusFee.ID_Fee WHERE 1=1 @where ORDER BY TeamTaskGroupFee.ID_TeamTaskGroupFee";
			string text2 = string.Empty;
			if (ID_TeamTaskGroupS != string.Empty)
			{
				text2 = text2 + " AND TeamTaskGroupFee.ID_TeamTaskGroup in(" + ID_TeamTaskGroupS + ")";
			}
			text = text.Replace("@where", text2);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public DataTable GetTeamTaskGroupInfoByTeamAndTask(string ID_Team, string ID_TeamTask, bool IsLike)
		{
			object cache = DataCache.GetCache(this._CacheTeamTaskGroupKey);
			DataTable dataTable;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				string sql = "SELECT 1 exist,ISNULL(Is_GroupPaused,0) Is_GroupPaused,ID_TeamTaskGroup,ID_Team,ID_TeamTask,TeamTaskGroupName,ForGender ID_Gender,CASE ForGender WHEN 0 THEN '女' WHEN 1 THEN '男' ELSE '' END GenderName,Is_Marriage Is_Married,CASE Is_Marriage WHEN 0 THEN '未婚' WHEN 1 THEN '已婚' WHEN 2 THEN '视为已婚' ELSE '' END MarriageName,MinAgeValue,MaxAgeValue,ID_ExamType,ExamTypeName,ID_Set,SetName,ID_FeeWay,FeeWayName,SecurityLevel,RoleName from TeamTaskGroup ORDER BY ID_TeamTaskGroup DESC;";
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int configInt = ConfigHelper.GetConfigInt("ModelCache");
				DataCache.SetCache(this._CacheTeamTaskGroupKey, dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			}
			DataTable dataTable2 = dataTable.Clone();
			DataRow[] array = null;
			DataTable result;
			if (ID_Team == string.Empty && ID_TeamTask == string.Empty)
			{
				result = dataTable;
			}
			else
			{
				if (ID_Team != string.Empty && ID_TeamTask == string.Empty)
				{
					if (IsLike)
					{
						array = dataTable.Select(" ID_Team like '%" + ID_Team + "%'");
					}
					else
					{
						array = dataTable.Select(" ID_Team='" + ID_Team + "'");
					}
				}
				if (ID_Team == string.Empty && ID_TeamTask != string.Empty)
				{
					if (IsLike)
					{
						array = dataTable.Select(" ID_TeamTask like '%" + ID_TeamTask + "%'");
					}
					else
					{
						array = dataTable.Select(" ID_TeamTask='" + ID_TeamTask + "'");
					}
				}
				if (ID_Team != string.Empty && ID_TeamTask != string.Empty)
				{
					if (IsLike)
					{
						array = dataTable.Select(string.Concat(new string[]
						{
							" ID_Team like '%",
							ID_Team,
							"%' OR ID_TeamTask like '%",
							ID_TeamTask,
							"%'"
						}));
					}
					else
					{
						array = dataTable.Select(string.Concat(new string[]
						{
							" ID_Team='",
							ID_Team,
							"' AND ID_TeamTask='",
							ID_TeamTask,
							"'"
						}));
					}
				}
				if (array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow row = array2[i];
						dataTable2.ImportRow(row);
					}
				}
				dataTable.Dispose();
				result = dataTable2;
			}
			return result;
		}

		public DataSet ImportCustomerInfoFromExcel(string svrAbsPath, string WorkSheetName, ref string ErrorMessage)
		{
			WorkSheetName = WorkSheetName.Replace("'", string.Empty).Replace("$", string.Empty);
			DataSet dataSet = ExcellCommand.ExcelToDataSet(svrAbsPath, WorkSheetName, ref ErrorMessage);
			DataSet result;
			if (ErrorMessage.Length > 0)
			{
				result = null;
			}
			else
			{
				if (dataSet == null || dataSet.Tables.Count == 0)
				{
					ErrorMessage = "从Excell中未读取到表[" + WorkSheetName + "]所对应的数据！";
				}
				if (dataSet.Tables[0].Rows.Count == 0)
				{
					ErrorMessage = "从表[" + WorkSheetName + "]中未读取到任何行！";
				}
				result = dataSet;
			}
			return result;
		}

		public DataSet ImportInternatCustomerInfoFromExcel(string svrAbsPath, string WorkSheetName, ref string ErrorMessage)
		{
			WorkSheetName = WorkSheetName.Replace("'", string.Empty).Replace("$", string.Empty);
			DataSet dataSet = ExcellCommand.ExcelToDataSet(svrAbsPath, WorkSheetName, ref ErrorMessage);
			DataSet result;
			if (ErrorMessage.Length > 0)
			{
				result = null;
			}
			else
			{
				if (dataSet == null || dataSet.Tables.Count == 0)
				{
					ErrorMessage = "从Excell中未读取到表[" + WorkSheetName + "]所对应的数据！";
				}
				if (dataSet.Tables[0].Rows.Count == 0)
				{
					ErrorMessage = "从表[" + WorkSheetName + "]中未读取到任何行！";
				}
				result = dataSet;
			}
			return result;
		}

		public DataTable GetTeamTaskByTeam(string ID_Team)
		{
			DataTable teamTaskInfoByKeyWords = this.GetTeamTaskInfoByKeyWords(string.Empty, ID_Team);
			DataTable dataTable = teamTaskInfoByKeyWords.Clone();
			DataRow[] array = teamTaskInfoByKeyWords.Select("P_ID_Team='" + ID_Team + "'");
			DataRow[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DataRow row = array2[i];
				dataTable.ImportRow(row);
			}
			return dataTable;
		}

		public DataTable GetCustomerUnionBusFee(string UserName, string ID_Team, string ID_TeamTask, string ID_TeamTaskGroup, string ID_CustomerS)
		{
			string text = string.Format("SELECT distinct ISNULL(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,OnCustFee.OriginalPrice Price,OnCustFee.Discount Discount,OnCustFee.FactPrice FactPrice \r\nFROM OnCustFee\r\nLEFT JOIN BusFee on OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\nLEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ISNULL(Is_Examined,0)=0 AND isnull(BusFee.Is_Banned,0)!=1 AND OnCustFee.ID_Customer IN({0}) ORDER BY InputCode ASC;", ID_CustomerS);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public DataTable CloneCustFee(string UserName, string Discount, string FromTeamTaskGroupID, string ToTeamTaskGroupID)
		{
			string text = string.Format("IF EXISTS(SELECT TOP 1 ID_TeamTaskGroup FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup='{1}')\r\nBEGIN\r\n    SELECT -1 CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,SYSSection.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice \r\n    FROM(SELECT ID_TeamTaskGroupFee,ID_Fee FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup='{0}'\r\n    AND ID_Fee NOT IN(SELECT ID_Fee FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup='{1}')\r\n    )A\r\n    INNER JOIN BusFee ON A.ID_Fee=BusFee.ID_Fee AND ISNULL(BusFee.Is_Banned,0)!=1 \r\n    INNER JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\n    LEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee\r\n    ORDER BY A.ID_TeamTaskGroupFee\r\nEND\r\nELSE\r\nBEGIN\r\n    SELECT -1 CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,SYSSection.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice\r\n    FROM(SELECT ID_TeamTaskGroupFee,ID_Fee FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup='{0}'\r\n    )A\r\n    INNER JOIN BusFee ON A.ID_Fee=BusFee.ID_Fee AND ISNULL(BusFee.Is_Banned,0)!=1 \r\n    INNER JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\n    LEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee\r\n    ORDER BY A.ID_TeamTaskGroupFee\r\nEND", FromTeamTaskGroupID, ToTeamTaskGroupID);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			text = text.Replace("@Discount", Discount.ToString());
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public int PauseTeamTaskGroup(string ID_TeamTaskGroup, int IsPaused)
		{
			string item = string.Empty;
			if (IsPaused == 1)
			{
				item = string.Format(" UPDATE TeamTaskGroup SET Is_GroupPaused='{1}' WHERE ID_TeamTaskGroup ='{0}';\r\n UPDATE OnCustPhysicalExamInfo SET Is_Paused='{1}' WHERE ISNULL(Is_GuideSheetPrinted,0)=0 AND ID_Customer IN(SELECT ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup='{0}');", ID_TeamTaskGroup, IsPaused);
			}
			else
			{
				item = string.Format(" UPDATE TeamTaskGroup SET Is_GroupPaused='{1}' WHERE ID_TeamTaskGroup ='{0}';\r\n UPDATE OnCustPhysicalExamInfo SET Is_Paused='{1}' WHERE ID_Customer IN(SELECT ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTaskGroup='{0}');", ID_TeamTaskGroup, IsPaused);
			}
			List<string> list = new List<string>(1);
			list.Add(item);
			int num = CommonExcuteSql.Instance.ExecuteSqlTran(list);
			if (num > 0)
			{
				DataCache.DeleteCache(this._CacheTeamTaskGroupKey);
			}
			return num;
		}

		public bool ISCanPauseTeamTaskGroup(int ID_TeamTaskGroup)
		{
			string sql = string.Format("SELECT ID_Customer,CustomerName FROM OnCustPhysicalExamInfo OCPEI WHERE EXISTS(SELECT ID_Customer FROM TeamTaskGroupCust TTGC WHERE ID_TeamTaskGroup='{0}' AND OCPEI.ID_Customer=TTGC.ID_Customer) \r\nAND Is_GuideSheetPrinted=1", ID_TeamTaskGroup);
			DataTable dataTable = null;
			bool result;
			try
			{
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				if (dataTable != null)
				{
					if (dataTable.Rows.Count == 0)
					{
						dataTable.Dispose();
						result = true;
						return result;
					}
				}
			}
			catch (System.Exception var_2_5E)
			{
			}
			if (dataTable != null)
			{
				dataTable.Dispose();
			}
			result = false;
			return result;
		}

		public bool ISCanPauseCustomer(string ID_Customer)
		{
			string sql = string.Format("SELECT ID_Customer,CustomerName FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}' AND Is_GuideSheetPrinted=1;", ID_Customer);
			DataTable dataTable = null;
			bool result;
			try
			{
				dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				if (dataTable != null)
				{
					if (dataTable.Rows.Count == 0)
					{
						dataTable.Dispose();
						result = true;
						return result;
					}
				}
			}
			catch (System.Exception var_2_59)
			{
			}
			if (dataTable != null)
			{
				dataTable.Dispose();
			}
			result = false;
			return result;
		}

		public int PauseCustomer(string ID_Customer, int IsPaused)
		{
			string item = string.Format("UPDATE OnCustPhysicalExamInfo SET Is_Paused='{1}' WHERE ID_Customer='{0}';", ID_Customer, IsPaused);
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public DataSet GetGuideSheetPrintedCustomer(string ID_Customer)
		{
			string sql = string.Format("SELECT ID_Customer,CustomerName FROM OnCustPhysicalExamInfo WHERE ID_Customer IN({0}) AND Is_GuideSheetPrinted=1", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataTable GetQuickTeamList(string inputcode)
		{
			string cacheKey = "AllTeamList";
			object obj = DataCache.GetCache(cacheKey);
			if (obj == null)
			{
				try
				{
					string sql = "\r\n                            SELECT [ID_Team]\r\n                                  ,[TeamName]\r\n                                  ,[InputCode]\r\n                              FROM [Team]\r\n                          ORDER BY [TeamName] ASC; ";
					obj = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
					if (obj != null)
					{
						int configInt = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(cacheKey, obj, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
					}
				}
				catch
				{
				}
			}
			DataTable dataTable = (DataTable)obj;
			DataTable dataTable2 = dataTable.Copy();
			HashSet<int> hashSet = new HashSet<int>();
			int num = 100;
			int num2 = 0;
			int num3 = 1;
			DataTable dataTable3 = dataTable2.Clone();
			if (string.IsNullOrEmpty(inputcode))
			{
				dataTable3 = dataTable2.Copy();
			}
			else
			{
				DataRow[] array = dataTable2.Select(" InputCode like '" + inputcode + "%' ", " ID_Team ASC ");
				if (array != null && array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (hashSet.Add(int.Parse(array[i]["ID_Team"].ToString())))
						{
							array[i]["InputCode"] = array[i]["InputCode"].ToString().ToUpper();
							dataTable3.ImportRow(array[i]);
							if (num2++ > num)
							{
								break;
							}
						}
					}
				}
				if (num2 < num)
				{
					if (inputcode.Length >= num3)
					{
						array = dataTable2.Select(" InputCode like '%" + inputcode + "%' ", " ID_Team ASC ");
						if (array != null && array.Length > 0)
						{
							for (int i = 0; i < array.Length; i++)
							{
								if (hashSet.Add(int.Parse(array[i]["ID_Team"].ToString())))
								{
									array[i]["InputCode"] = array[i]["InputCode"].ToString().ToUpper();
									dataTable3.ImportRow(array[i]);
									if (num2++ > num)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
			return dataTable3;
		}

		public DataTable GetQuickTeamTaskList(string inputcode, int CurrSelectedTeamID)
		{
			string cacheKey = "AllTeamTaskList";
			object obj = DataCache.GetCache(cacheKey);
			if (obj == null)
			{
				try
				{
					string sql = "\r\n                            SELECT [ID_TeamTask]\r\n                                  ,[ID_Team]\r\n                                  ,[TeamTaskName]\r\n                                  ,[InputCode]\r\n                              FROM [TeamTask]\r\n                          ORDER BY DispOrder ASC,[TeamTaskName] ASC; ";
					obj = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
					if (obj != null)
					{
						int configInt = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(cacheKey, obj, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
					}
				}
				catch
				{
				}
			}
			DataTable dataTable = (DataTable)obj;
			DataTable dataTable2 = dataTable.Copy();
			HashSet<int> hashSet = new HashSet<int>();
			int num = 100;
			int num2 = 0;
			int num3 = 1;
			DataTable dataTable3 = dataTable2.Clone();
			DataRow[] array;
			if (string.IsNullOrEmpty(inputcode))
			{
				array = dataTable2.Select(" ID_Team = " + CurrSelectedTeamID, " inputcode ASC, TeamTaskName ASC ");
			}
			else
			{
				array = dataTable2.Select(string.Concat(new object[]
				{
					" InputCode like '",
					inputcode,
					"%' AND ID_Team = ",
					CurrSelectedTeamID
				}), " inputcode ASC, TeamTaskName ASC ");
			}
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (hashSet.Add(int.Parse(array[i]["ID_TeamTask"].ToString())))
					{
						array[i]["InputCode"] = array[i]["InputCode"].ToString().ToUpper();
						dataTable3.ImportRow(array[i]);
						if (num2++ > num)
						{
							break;
						}
					}
				}
			}
			if (num2 < num)
			{
				if (inputcode.Length >= num3)
				{
					if (string.IsNullOrEmpty(inputcode))
					{
						array = dataTable2.Select(" ID_Team = " + CurrSelectedTeamID, " inputcode ASC, TeamTaskName ASC ");
					}
					else
					{
						array = dataTable2.Select(string.Concat(new object[]
						{
							" InputCode like '%",
							inputcode,
							"%' AND ID_Team = ",
							CurrSelectedTeamID
						}), " inputcode ASC, TeamTaskName ASC ");
					}
					if (array != null && array.Length > 0)
					{
						for (int i = 0; i < array.Length; i++)
						{
							if (hashSet.Add(int.Parse(array[i]["ID_TeamTask"].ToString())))
							{
								array[i]["InputCode"] = array[i]["InputCode"].ToString().ToUpper();
								dataTable3.ImportRow(array[i]);
								if (num2++ > num)
								{
									break;
								}
							}
						}
					}
				}
			}
			return dataTable3;
		}
	}
}
