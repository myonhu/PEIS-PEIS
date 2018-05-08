using PEIS.Common;
using PEIS.Model;
using Maticsoft.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace PEIS.BLL
{
	public class CommonOnArcCust
	{
		private const string _CacheFeeWay = "FeeWay";

		private const string __CacheTeamTaskGroupFeeKey = "TeamTaskGroupFee";

		private const int _maxRowCount = 5;

		private static readonly CommonOnArcCust _instance = new CommonOnArcCust();

		private int _defaultCount = 200;

		public string[] tbName = new string[]
		{
			"BusReportWayDT",
			"ExamTypeDT",
			"DictMarriageDT",
			"BusPEPackageDT",
			"DictFeeWayDT",
			"DictGenderDT",
			"DictCountryDT",
			"DictCultrulDT",
			"DictNationDT",
			"DictVocationDT",
			"DictExamPlaceDT",
			"TeamDT"
		};

		private string _AllDct = "AllDct";

		private string _ExamTypeDataCache = "ExamTypeDataCache";

		private System.Collections.Hashtable _ALlFeeID = new System.Collections.Hashtable();

		private string _PrevSelectedFee = string.Empty;

		public static CommonOnArcCust Instance
		{
			get
			{
				return CommonOnArcCust._instance;
			}
		}

		public DataSet GetCacheExamType()
		{
			object cache = DataCache.GetCache(this._ExamTypeDataCache);
			DataSet dataSet;
			if (cache != null)
			{
				dataSet = (DataSet)cache;
			}
			else
			{
				string sql = "select * from DictExamType;";
				dataSet = CommonExcuteSql.Instance.ExcuteSql(sql);
				DataCache.SetCache(this._ExamTypeDataCache, dataSet);
			}
			return dataSet;
		}

		public DataSet GetAllDctData()
		{
			object cache = DataCache.GetCache(this._AllDct);
			DataSet result;
			if (cache != null)
			{
				result = (DataSet)cache;
			}
			else
			{
				string sql = "select * from DictReceiveReportWay;\r\n      select * from DictExamType;\r\n                            select * from DictMarriage;\r\n                            select * from BusPEPackage;\r\n                            select * from DictFeeWay;\r\n                            select GenderID,GenderName,InputCode from DictGender;\r\n                            select * from DictCountry;\r\n                            select * from DictCultrul;\r\n                            select * from DictNation;\r\n                            select * from DictVocation;\r\n                            select * from DictExamPlace;";
				result = CommonExcuteSql.Instance.ExcuteSql(sql);
			}
			return result;
		}

		public DataTable GetUser(string VocationType)
		{
			string sql = string.Format("SELECT * FROM SYSOpUser WHERE VocationType IN({0}) ORDER BY UserName;", VocationType);
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataSet GetOnArcustInfo(string ID_ArcCustomer, string ID_Customer, string Is_Subscribed)
		{
			string sql = string.Format("SELECT [dbo].StrToCode128(OnCustRelationCustPEInfo.ID_Customer) Code128c,ISNULL(OnCustPhysicalExamInfo.Is_GuideSheetPrinted,0) Is_GuideSheetPrinted,ISNULL(OnCustPhysicalExamInfo.Is_Subscribed,-1) Is_Subscribed ,OnCustPhysicalExamInfo.ID_Team,Team.TeamName,TeamTaskGroupCust.Department,TeamTaskGroupCust.DepartSubA,TeamTaskGroupCust.DepartSubB,TeamTaskGroupCust.DepartSubC,OnArcCust.Photo,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) SubScribDate,OnCustPhysicalExamInfo.SecurityLevel,OnArcCust.CultrulID,OnArcCust.CultrulName,OnArcCust.ID_Nation,OnArcCust.NationName,OnArcCust.NationName,OnArcCust.ID_Vocation,OnArcCust.VocationName,OnArcCust.IDCard,OnArcCust.CustomerName,OnArcCust.ExamCard,OnCustPhysicalExamInfo.ID_Customer,OnArcCust.ID_Gender,CONVERT(varchar(10),OnArcCust.BirthDay,120) BirthDay,OnArcCust.ID_Marriage,OnArcCust.MobileNo,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) RegisteDate,OnCustPhysicalExamInfo.Operator Register,OnCustPhysicalExamInfo.ID_ReportWay,OnCustPhysicalExamInfo.PEPackageID,OnCustPhysicalExamInfo.PEPackageName,OnCustPhysicalExamInfo.ID_ExamType,OnCustPhysicalExamInfo.ExamTypeName,OnCustPhysicalExamInfo.ID_FeeWay,OnCustPhysicalExamInfo.ExamPlaceID,OnCustPhysicalExamInfo.ExamPlaceName,OnCustPhysicalExamInfo.SecurityLevel,OnCustPhysicalExamInfo.ID_GuideNurse,OnCustPhysicalExamInfo.GuideNurse,OnArcCust.Email,OnCustPhysicalExamInfo.Note\r\nFROM OnArcCust INNER JOIN OnCustRelationCustPEInfo ON OnArcCust.ID_ArcCustomer=OnCustRelationCustPEInfo.ID_ArcCustomer\r\nINNER JOIN OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nLEFT JOIN Team ON OnCustPhysicalExamInfo.ID_Team=Team.ID_Team\r\nLEFT JOIN TeamTaskGroupCust ON OnCustPhysicalExamInfo.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nWHERE OnArcCust.ID_ArcCustomer='{0}' and OnCustRelationCustPEInfo.ID_Customer='{1}' AND OnCustPhysicalExamInfo.Is_Subscribed='{2}';", ID_ArcCustomer, ID_Customer, Is_Subscribed);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetOnArcustAndPhysicalInfo(string ID_Customer)
		{
			string sql = string.Format("SELECT [dbo].StrToCode128(OCPEI.ID_Customer) Code128c, Is_Paused,OCPEI.OperateDate, ISNULL(OCPEI.Is_Checked,0) Is_Checked,  ISNULL(OCPEI.Is_GuideSheetPrinted,0) Is_GuideSheetPrinted, ISNULL(OCPEI.Is_Subscribed,-1) Is_Subscribed ,OCPEI.ID_Customer,OCPEI.ID_Team,Team.TeamName, TeamTaskGroupCust.Department,TeamTaskGroupCust.DepartSubA,TeamTaskGroupCust.DepartSubB,TeamTaskGroupCust.DepartSubC,CONVERT(varchar(10),OCPEI.SubScribDate,120) SubScribDate, OCPEI.SecurityLevel, CONVERT(varchar(10),OCPEI.SubScribDate,120) RegisteDate, OCPEI.Operator Register,OCPEI.ID_ReportWay ReportWayID, OCPEI.ID_Set,OCPEI.SetName PEPackageName,OCPEI.ID_ExamType ExamTypeID,OCPEI.ExamTypeName,OCPEI.ID_FeeWay FeeWayID,OCPEI.ID_ExamPlace ExamPlaceID,OCPEI.ExamPlaceName,OCPEI.SecurityLevel,OCPEI.ID_GuideNurse GuideNurseID,OCPEI.GuideNurse,OCPEI.CustomerName,OCPEI.Note,RoleName  FROM OnCustPhysicalExamInfo OCPEI LEFT JOIN Team ON OCPEI.ID_Team=Team.ID_Team LEFT JOIN TeamTaskGroupCust ON OCPEI.ID_Customer=TeamTaskGroupCust.ID_Customer  LEFT JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup  WHERE OCPEI.ID_Customer='{0}';  SELECT Photo, (SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}' )ID_ArcCustomer, ID_Cultrul CultrulID,CultrulName, ID_Nation NationID,NationName, ID_Vocation VocationID,VocationName, IDCard,  FLOOR(DATEDIFF(DY,BirthDay,(SELECT CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END OperateDate ))/365.25) Age, ExamCard,   ID_Gender GenderID,  CONVERT(varchar(10),BirthDay,120) BirthDay, ID_Marriage MarriageID,MobileNo,    Email FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}'  ", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetCustomerRelation(string ID_Customer, bool CanReturnInterface)
		{
			string sql = string.Empty;
			if (CanReturnInterface)
			{
				sql = string.Format("SELECT OnCustPhysicalExamInfo.Is_FeeSettled,OnCustPhysicalExamInfo.ID_Customer,[dbo].StrToCode128(OnCustPhysicalExamInfo.ID_Customer) Code128c,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) SubScribDate,ID_ArcCustomer,OnCustRelationCustPEInfo.ID_Customer,ISNULL(Is_GuideSheetPrinted,0) Is_GuideSheetPrinted,ISNULL(Is_Subscribed,-1) Is_Subscribed FROM OnCustRelationCustPEInfo\r\nINNER JOIN OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\n WHERE OnCustRelationCustPEInfo.ID_Customer='{0}';\r\nSELECT OCE.IS_Refund,NS.InterfaceType,OCE.ID_Customer,OCE.ID_Section FROM(SELECT * FROM OnCustExamSection WHERE ID_Customer='{0}') OCE\r\nINNER JOIN (SELECT * FROM SYSSection WHERE InterfaceType='LAB' OR InterfaceType='PACS') NS ON OCE.ID_Section=NS.ID_Section;", ID_Customer);
			}
			else
			{
				sql = string.Format("SELECT OnCustPhysicalExamInfo.Is_FeeSettled,OnCustPhysicalExamInfo.ID_Customer,[dbo].StrToCode128(OnCustPhysicalExamInfo.ID_Customer) Code128c,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) SubScribDate,ID_ArcCustomer,OnCustRelationCustPEInfo.ID_Customer,ISNULL(Is_GuideSheetPrinted,0) Is_GuideSheetPrinted,ISNULL(Is_Subscribed,-1) Is_Subscribed FROM OnCustRelationCustPEInfo\r\nINNER JOIN OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\n WHERE OnCustRelationCustPEInfo.ID_Customer='{0}';", ID_Customer);
			}
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetOnCustWaitSendToInterfaceInfo(string ID_Customer, string InterfaceType)
		{
			string sql = string.Format("SELECT OCE.IS_Refund,NS.InterfaceType,OCE.ID_Customer,OCE.ID_Section FROM(SELECT * FROM OnCustExamSection WHERE ID_Customer='{0}') OCE\r\nINNER JOIN (SELECT * FROM SYSSection WHERE InterfaceType IN({1})) NS ON OCE.ID_Section=NS.ID_Section;", ID_Customer, InterfaceType);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetOnCustWaitSendToInterfaceInfo_Ex(string ID_Customer, string InterfaceType)
		{
			string sql = string.Format("SELECT OCE.IS_Refund,NS.InterfaceType,OCE.ID_Customer,OCE.ID_Section,OCPE.* FROM(SELECT * FROM OnCustExamSection WHERE ID_Customer='{0}') OCE\r\nINNER JOIN (SELECT * FROM SYSSection WHERE InterfaceType IN({1})) NS ON OCE.ID_Section=NS.ID_Section \r\nINNER JOIN(SELECT ID_Customer,ID_Gender,GenderName FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}')OCPE ON OCE.ID_Customer=OCPE.ID_Customer;", ID_Customer, InterfaceType);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public int AddCustomerInfo(CustomerInfo customerInfo)
		{
			bool isNew = customerInfo.IsNew;
			string text = DateTime.Now.ToString("yyyy-MM-dd");
			string a = customerInfo.Type.Trim().ToLower();
			string a2 = customerInfo.ModelName.Trim().ToLower();
			string iDCard = customerInfo.OnArcCust.IDCard;
			string customerName = customerInfo.OnArcCust.CustomerName;
			string examCard = customerInfo.OnArcCust.ExamCard;
			string text2 = customerInfo.OnArcCust.ID_Gender.ToString();
			string genderName = customerInfo.OnArcCust.GenderName;
			string text3 = customerInfo.OnArcCust.BirthDay.Value.ToString("yyyy-MM-dd");
			string text4 = customerInfo.OnArcCust.ID_Marriage.ToString();
			string marriageName = customerInfo.OnArcCust.MarriageName;
			string mobileNo = customerInfo.OnArcCust.MobileNo;
			string email = customerInfo.OnArcCust.Email;
			string text5 = customerInfo.OnArcCust.NationID.ToString();
			string nationName = customerInfo.OnArcCust.NationName;
			string text6 = customerInfo.OnArcCust.CultrulID.ToString();
			string cultrulName = customerInfo.OnArcCust.CultrulName;
			string text7 = customerInfo.OnArcCust.ID_ArcCustomer.ToString();
			string text8 = customerInfo.OnCustPhysicalExamInfo.ID_Customer.ToString();
			string text9 = customerInfo.OnCustPhysicalExamInfo.OperateDate.Value.ToString("yyyy-MM-dd");
			string text10 = customerInfo.OnCustPhysicalExamInfo.ID_GuideNurse.ToString();
			string guideNurse = customerInfo.OnCustPhysicalExamInfo.GuideNurse;
			string text11 = customerInfo.OnCustPhysicalExamInfo.ID_FeeWay.ToString();
			string feeWayName = customerInfo.OnCustPhysicalExamInfo.FeeWayName;
			string text12 = customerInfo.OnCustPhysicalExamInfo.ID_Operator.Value.ToString();
			string @operator = customerInfo.OnCustPhysicalExamInfo.Operator;
			string text13 = customerInfo.OnCustPhysicalExamInfo.ID_ReportWay.ToString();
			string reportWayName = customerInfo.OnCustPhysicalExamInfo.ReportWayName;
			string text14 = customerInfo.OnCustPhysicalExamInfo.ID_ExamType.ToString();
			string examTypeName = customerInfo.OnCustPhysicalExamInfo.ExamTypeName;
			string text15 = customerInfo.OnCustPhysicalExamInfo.SecurityLevel.ToString();
			string text16 = customerInfo.OnCustPhysicalExamInfo.PEPackageID.ToString();
			string setName = customerInfo.OnCustPhysicalExamInfo.PEPackageName;
			string note = customerInfo.OnCustPhysicalExamInfo.Note;
			string text17 = (customerInfo.OnCustPhysicalExamInfo.Is_Subscribed.ToString() == "1") ? "1" : "0";
			string a3 = (a2 == "sign") ? "0" : "1";
			string text18 = customerInfo.OnCustPhysicalExamInfo.ExamPlaceID.ToString();
			string examPlaceName = customerInfo.OnCustPhysicalExamInfo.ExamPlaceName;
			string text19 = customerInfo.OnCustPhysicalExamInfo.SubScribDate.Value.ToString("yyyy-MM-dd");
			string text20 = customerInfo.OnCustPhysicalExamInfo.OperateDate.Value.ToString("yyyy-MM-dd");
			string photoX = customerInfo.PhotoX;
			string newValue = string.Empty;
			string newValue2 = string.Empty;
			int count = customerInfo.OnCustFeeList.Count;
			string text21 = string.Empty;
			List<string> list = new List<string>();
			string text22 = string.Format("if not exists(select CultrulName from OnArcCust where IDCard='{0}' AND CustomerName='{1}')\r\n            BEGIN\r\n                @TempInsertSql\r\n            END\r\n            ELSE \r\n            BEGIN\r\n                @TempUpdateSql\r\n            END;", iDCard, customerName);
			string text23 = string.Format("IF NOT EXISTS(select ID_Customer FROM OnCustPhysicalExamInfo where ID_Customer='{0}')\r\nBEGIN\r\n     @TempInsertSql\r\nEND\r\nELSE \r\nBEGIN\r\n    @TempUpdateSql\r\nEND;", text8);
			string text24 = string.Format("IF NOT EXISTS(select ID_Customer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}')\r\nBEGIN\r\n     @TempInsertSql\r\nEND\r\nELSE \r\nBEGIN\r\n    @TempUpdateSql\r\nEND;", text8);
			string text25 = string.Empty;
			string sql = string.Format("SELECT ID_Customer,Is_Subscribed,ID_SubScriber,SubScriber,SubScriberOperDate,SubScribDate,ID_Operator,Operator,OperateDate,ISNULL(Is_FeeSettled,0) Is_FeeSettled,ISNULL(Is_GuideSheetPrinted,0) Is_GuideSheetPrinted,ISNULL(Is_Team,0) Is_Team FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}';", text8);
			DataTable dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (dataTable.Rows.Count > 0)
			{
				num2 = (bool.Parse(dataTable.Rows[0]["Is_GuideSheetPrinted"].ToString()) ? 1 : 0);
				num = (bool.Parse(dataTable.Rows[0]["Is_FeeSettled"].ToString()) ? 1 : 0);
				num3 = (bool.Parse(dataTable.Rows[0]["Is_Team"].ToString()) ? 1 : 0);
				num4 = (string.IsNullOrEmpty(dataTable.Rows[0]["Operator"].ToString()) ? 0 : 1);
			}
			string text26 = text12;
			string text27 = @operator;
			string text28 = text20;
			int num5 = int.Parse(this.GetFeeWay().Select("FeeWayName='统一收费'")[0]["ID_FeeWay"].ToString());
			dataTable.Dispose();
			if (a2 == "regist" || a2 == "sign")
			{
				if (text5 != "-1" && text6 != "-1")
				{
					newValue = string.Format("INSERT INTO OnArcCust(CustomerName,IDCard,ExamCard,BirthDay,ID_Gender,GenderName,ID_Marriage,MarriageName,MobileNo,Email,FinishedNum,FirstDatePE,LatestDatePE,ID_Nation,NationName,ID_Cultrul,CultrulName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}',1);", new object[]
					{
						customerName,
						iDCard,
						examCard,
						text3,
						text2,
						genderName,
						text4,
						marriageName,
						mobileNo,
						email,
						0,
						text9,
						text9,
						text5,
						nationName,
						text6,
						cultrulName
					});
					if (a == "add")
					{
						newValue2 = string.Format("update OnArcCust set ExamCard='{13}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',ID_Nation='{7}',NationName='{8}',CultrulID='{9}',CultrulName='{10}',BirthDay='{11}' where IDCard='{12}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text5,
							nationName,
							text6,
							cultrulName,
							text3,
							iDCard,
							examCard
						});
					}
					else if (a == "edit")
					{
						newValue2 = string.Format("update OnArcCust set ExamCard='{13}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',ID_Nation='{7}',NationName='{8}',CultrulID='{9}',CultrulName='{10}',BirthDay='{11}' where IDCard='{12}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text5,
							nationName,
							text6,
							cultrulName,
							text3,
							iDCard,
							examCard
						});
					}
					text21 = text22.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text5 != "-1" && text6 == "-1")
				{
					newValue = string.Format("INSERT INTO OnArcCust(CustomerName,IDCard,ExamCard,BirthDay,ID_Gender,GenderName,ID_Marriage,MarriageName,MobileNo,Email,FinishedNum,FirstDatePE,LatestDatePE,ID_Nation,NationName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',1);", new object[]
					{
						customerName,
						iDCard,
						examCard,
						text3,
						text2,
						genderName,
						text4,
						marriageName,
						mobileNo,
						email,
						0,
						text9,
						text9,
						text5,
						nationName
					});
					if (a == "add")
					{
						newValue2 = string.Format("update OnArcCust set ExamCard='{11}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',ID_Nation='{7}',NationName='{8}',BirthDay='{9}' where IDCard='{10}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text5,
							nationName,
							text3,
							iDCard,
							examCard
						});
					}
					else if (a == "edit")
					{
						newValue2 = string.Format("update OnArcCust set ExamCard='{11}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',ID_Nation='{7}',NationName='{8}',BirthDay='{9}' where IDCard='{10}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text5,
							nationName,
							text3,
							iDCard,
							examCard
						});
					}
					text21 = text22.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text5 == "-1" && text6 == "-1")
				{
					newValue = string.Format("INSERT INTO OnArcCust(CustomerName,IDCard,ExamCard,BirthDay,ID_Gender,GenderName,ID_Marriage,MarriageName,MobileNo,Email,FinishedNum,FirstDatePE,LatestDatePE,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',1);", new object[]
					{
						customerName,
						iDCard,
						examCard,
						text3,
						text2,
						genderName,
						text4,
						marriageName,
						mobileNo,
						email,
						0,
						text9,
						text9
					});
					if (a == "add")
					{
						newValue2 = string.Format("update OnArcCust set  ExamCard='{9}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',BirthDay='{7}' where IDCard='{8}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text3,
							iDCard,
							examCard
						});
					}
					else if (a == "edit")
					{
						newValue2 = string.Format("update OnArcCust set  ExamCard='{9}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',BirthDay='{7}' where IDCard='{8}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text3,
							iDCard,
							examCard
						});
					}
					text21 = text22.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text5 == "-1" && text6 != "-1")
				{
					newValue = string.Format("INSERT INTO OnArcCust(CustomerName,IDCard,ExamCard,BirthDay,ID_Gender,GenderName,ID_Marriage,MarriageName,MobileNo,Email,FinishedNum,FirstDatePE,LatestDatePE,CultrulID,CultrulName,UnfinishedNum)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',1);", new object[]
					{
						customerName,
						iDCard,
						examCard,
						text3,
						text2,
						genderName,
						text4,
						marriageName,
						mobileNo,
						email,
						0,
						text9,
						text9,
						text6,
						cultrulName
					});
					if (a == "add")
					{
						newValue2 = string.Format("update OnArcCust set ExamCard='{11}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',CultrulID='{7}',CultrulName='{8}',BirthDay='{9}' where IDCard='{10}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text6,
							cultrulName,
							text3,
							iDCard,
							examCard
						});
					}
					else if (a == "edit")
					{
						newValue2 = string.Format("update OnArcCust set ExamCard='{11}', CustomerName='{0}',ID_Gender='{1}',GenderName='{2}',ID_Marriage='{3}',MarriageName='{4}',MobileNo='{5}',Email='{6}',CultrulID='{7}',CultrulName='{8}',BirthDay='{9}' where IDCard='{10}' AND CustomerName='{0}';", new object[]
						{
							customerName,
							text2,
							genderName,
							text4,
							marriageName,
							mobileNo,
							email,
							text6,
							cultrulName,
							text3,
							iDCard,
							examCard
						});
					}
					text21 = text22.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text16 != "-1" && text10 != "-1")
				{
					if (a3 == "1")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team\r\n,PEPackageID,PEPackageName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName\r\n,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_SubScriber,SubScriber,SubScribDate,SubScriberOperDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}';", new object[]
						{
							text8,
							text14,
							0,
							text16,
							setName,
							text10,
							guideNurse,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					else if (a3 == "0")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team\r\n,PEPackageID,PEPackageName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName\r\n,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_Operator,Operator,SubScribDate,OperateDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}';", new object[]
						{
							text8,
							text14,
							0,
							text16,
							setName,
							text10,
							guideNurse,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					if (num4 == 0)
					{
						if (a3 == "0")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',PEPackageID='{4}',PEPackageName='{5}',ID_GuideNurse='{6}',GuideNurse='{7}',ID_ReportWay='{8}'\r\n,ReportWayName='{9}',ID_FeeWay='{10}',FeeWayName='{11}',SecurityLevel='{12}',Note='{13}',ExamPlaceID='{14}',SubScribDate='{15}',Is_Subscribed='{17}',ID_Operator='{18}',Operator='{19}',OperateDate='{20}' where ID_Customer='{16}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text16,
								setName,
								text10,
								guideNurse,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator,
								text20
							});
						}
						else if (a3 == "1")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',PEPackageID='{4}',PEPackageName='{5}',ID_GuideNurse='{6}',GuideNurse='{7}',ID_ReportWay='{8}'\r\n,ReportWayName='{9}',ID_FeeWay='{10}',FeeWayName='{11}',SecurityLevel='{12}',Note='{13}',ExamPlaceID='{14}',SubScribDate='{15}',Is_Subscribed='{17}' where ID_Customer='{16}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text16,
								setName,
								text10,
								guideNurse,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator
							});
						}
					}
					else
					{
						newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',PEPackageID='{4}',PEPackageName='{5}',ID_GuideNurse='{6}',GuideNurse='{7}',ID_ReportWay='{8}'\r\n,ReportWayName='{9}',ID_FeeWay='{10}',FeeWayName='{11}',SecurityLevel='{12}',Note='{13}',ExamPlaceID='{14}',SubScribDate='{15}',Is_Subscribed='{17}' where ID_Customer='{16}';", new object[]
						{
							customerName,
							text14,
							examTypeName,
							examPlaceName,
							text16,
							setName,
							text10,
							guideNurse,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							note,
							text18,
							text19,
							text8,
							text17,
							text12,
							@operator
						});
					}
					text21 += text23.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text16 != "-1" && text10 == "-1")
				{
					if (a3 == "1")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team\r\n,PEPackageID,PEPackageName,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName\r\n,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_SubScriber,SubScriber,SubScribDate,SubScriberOperDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}';", new object[]
						{
							text8,
							text14,
							0,
							text16,
							setName,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					else if (a3 == "0")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team\r\n,PEPackageID,PEPackageName,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName\r\n,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_Operator,Operator,SubScribDate,OperateDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}';", new object[]
						{
							text8,
							text14,
							0,
							text16,
							setName,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					if (num4 == 0)
					{
						if (a3 == "0")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',PEPackageID='{4}',PEPackageName='{5}',ID_ReportWay='{6}'\r\n,ReportWayName='{7}',ID_FeeWay='{8}',FeeWayName='{9}',SecurityLevel='{10}',Note='{11}',ExamPlaceID='{12}',SubScribDate='{13}',Is_Subscribed='{15}',ID_Operator='{16}',Operator='{17}',OperateDate='{18}' where ID_Customer='{14}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text16,
								setName,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator,
								text20
							});
						}
						else if (a3 == "1")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',PEPackageID='{4}',PEPackageName='{5}',ID_ReportWay='{6}'\r\n,ReportWayName='{7}',ID_FeeWay='{8}',FeeWayName='{9}',SecurityLevel='{10}',Note='{11}',ExamPlaceID='{12}',SubScribDate='{13}',Is_Subscribed='{15}' where ID_Customer='{14}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text16,
								setName,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator
							});
						}
					}
					else
					{
						newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',PEPackageID='{4}',PEPackageName='{5}',ID_ReportWay='{6}'\r\n,ReportWayName='{7}',ID_FeeWay='{8}',FeeWayName='{9}',SecurityLevel='{10}',Note='{11}',ExamPlaceID='{12}',SubScribDate='{13}',Is_Subscribed='{15}' where ID_Customer='{14}';", new object[]
						{
							customerName,
							text14,
							examTypeName,
							examPlaceName,
							text16,
							setName,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							note,
							text18,
							text19,
							text8,
							text17,
							text12,
							@operator
						});
					}
					text21 += text23.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text16 == "-1" && text10 != "-1")
				{
					if (a3 == "1")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName\r\n,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_SubScriber,SubScriber,SubScribDate,SubScriberOperDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}';", new object[]
						{
							text8,
							text14,
							0,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					else if (a3 == "0")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName\r\n,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_Operator,Operator,SubScribDate,OperateDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}';", new object[]
						{
							text8,
							text14,
							0,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					if (num4 == 0)
					{
						if (a3 == "0")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',ID_GuideNurse='{4}',GuideNurse='{5}',ID_ReportWay='{6}'\r\n,ReportWayName='{7}',ID_FeeWay='{8}',FeeWayName='{9}',SecurityLevel='{10}',Note='{11}',ExamPlaceID='{12}',SubScribDate='{13}',Is_Subscribed='{15}',ID_Operator='{16}',Operator='{17}',OperateDate='{18}' where ID_Customer='{14}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text10,
								guideNurse,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator,
								text20
							});
						}
						else if (a3 == "1")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',ID_GuideNurse='{4}',GuideNurse='{5}',ID_ReportWay='{6}'\r\n,ReportWayName='{7}',ID_FeeWay='{8}',FeeWayName='{9}',SecurityLevel='{10}',Note='{11}',ExamPlaceID='{12}',SubScribDate='{13}',Is_Subscribed='{15}' where ID_Customer='{14}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text10,
								guideNurse,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator
							});
						}
					}
					else
					{
						newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',ID_GuideNurse='{4}',GuideNurse='{5}',ID_ReportWay='{6}'\r\n,ReportWayName='{7}',ID_FeeWay='{8}',FeeWayName='{9}',SecurityLevel='{10}',Note='{11}',ExamPlaceID='{12}',SubScribDate='{13}',Is_Subscribed='{15}' where ID_Customer='{14}';", new object[]
						{
							customerName,
							text14,
							examTypeName,
							examPlaceName,
							text10,
							guideNurse,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							note,
							text18,
							text19,
							text8,
							text17,
							text12,
							@operator
						});
					}
					text21 += text23.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				if (text16 == "-1" && text10 == "-1")
				{
					if (a3 == "1")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team\r\n,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_SubScriber,SubScriber,SubScribDate,SubScriberOperDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}';", new object[]
						{
							text8,
							text14,
							0,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					else if (a3 == "0")
					{
						newValue = string.Format("INSERT INTO OnCustPhysicalExamInfo(ID_Customer,ID_ExamType,Is_Team\r\n,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName,SecurityLevel,Is_GuideSheetPrinted,Is_SectionLock,Is_FinalFinished,Is_Subscribed,Note,CustomerName,ExamPlaceName,ExamPlaceID,ExamTypeName,ID_Operator,Operator,SubScribDate,OperateDate)\r\nselect '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}';", new object[]
						{
							text8,
							text14,
							0,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							0,
							0,
							0,
							text17,
							note,
							customerName,
							examPlaceName,
							text18,
							examTypeName,
							text12,
							@operator,
							text19,
							text20
						});
					}
					if (num4 == 0)
					{
						if (a3 == "0")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',ID_ReportWay='{4}'\r\n,ReportWayName='{5}',ID_FeeWay='{6}',FeeWayName='{7}',SecurityLevel='{8}',Note='{9}',ExamPlaceID='{10}',SubScribDate='{11}',Is_Subscribed='{13}',ID_Operator='{14}',Operator='{15}',OperateDate='{16}' where ID_Customer='{12}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator,
								text20
							});
						}
						if (a3 == "1")
						{
							newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',ID_ReportWay='{4}'\r\n,ReportWayName='{5}',ID_FeeWay='{6}',FeeWayName='{7}',SecurityLevel='{8}',Note='{9}',ExamPlaceID='{10}',SubScribDate='{11}',Is_Subscribed='{13}' where ID_Customer='{12}';", new object[]
							{
								customerName,
								text14,
								examTypeName,
								examPlaceName,
								text13,
								reportWayName,
								text11,
								feeWayName,
								text15,
								note,
								text18,
								text19,
								text8,
								text17,
								text12,
								@operator
							});
						}
					}
					else
					{
						newValue2 = string.Format("update OnCustPhysicalExamInfo set CustomerName='{0}',ID_ExamType='{1}',ExamTypeName='{2}',ExamPlaceName='{3}',ID_ReportWay='{4}'\r\n,ReportWayName='{5}',ID_FeeWay='{6}',FeeWayName='{7}',SecurityLevel='{8}',Note='{9}',ExamPlaceID='{10}',SubScribDate='{11}',Is_Subscribed='{13}' where ID_Customer='{12}';", new object[]
						{
							customerName,
							text14,
							examTypeName,
							examPlaceName,
							text13,
							reportWayName,
							text11,
							feeWayName,
							text15,
							note,
							text18,
							text19,
							text8,
							text17,
							text12,
							@operator
						});
					}
					text21 += text23.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				}
				newValue = string.Format("INSERT INTO OnCustRelationCustPEInfo(ID_ArcCustomer,IDCardNo,ExamCardNo,ID_Customer,Is_CompletePhysical,ExamState)\r\nVALUES((SELECT TOP 1 ID_ArcCustomer FROM OnArcCust WHERE IDCard='{0}' AND CustomerName='{3}' ORDER BY ID_ArcCustomer DESC),'{0}','{1}','{2}',0,0);", new object[]
				{
					iDCard,
					examCard,
					text8,
					customerName
				});
				newValue2 = string.Format("UPDATE OnCustRelationCustPEInfo SET IDCardNo='{0}',ExamCardNo='{1}' WHERE ID_Customer='{2}';", iDCard, examCard, text8);
				text21 += text24.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
				for (int i = 0; i < count; i++)
				{
					int num6 = 0;
					int num7 = 0;
					if (num3 == 1)
					{
						if (customerInfo.OnCustFeeList[i].ID_FeeType.Value.ToString() != num5.ToString())
						{
							num7 = 1;
						}
					}
					else if (num2 == 1)
					{
						num7 = 1;
					}
					if (num == 0)
					{
						if (num7 == 1)
						{
							num = 1;
						}
					}
					text25 = string.Format(" IF NOT EXISTS(select ID_Customer FROM OnCustFee WHERE ID_CustFee='{0}') \r\nBEGIN\r\n     @TempInsertSql\r\nEND\r\nELSE \r\nBEGIN\r\n    @TempUpdateSql\r\nEND;", customerInfo.OnCustFeeList[i].ID_CustFee);
					if (customerInfo.OnCustFeeList[i].FeetypeName.Trim() == "统一收费" || customerInfo.OnCustFeeList[i].FeetypeName.Trim() == "记账")
					{
						num6 = 1;
					}
					if (num6 == 1)
					{
						newValue = string.Format("INSERT INTO OnCustFee(ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,RegistDate\r\n,OriginalPrice,Discount,FactPrice,ID_FeeType,ID_Discounter,DiscounterName,Is_FeeCharged,CustFeeChargeState,Is_Printed,ID_FeeCharger,FeeCharger,FeeChargeDate,ApplyID)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}');", new object[]
						{
							customerInfo.OnCustFeeList[i].ID_Customer.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Fee.Value.ToString(),
							customerInfo.OnCustFeeList[i].FeeItemName,
							customerInfo.OnCustFeeList[i].ID_Register.Value.ToString(),
							customerInfo.OnCustFeeList[i].RegisterName,
							customerInfo.OnCustFeeList[i].RegistDate.Value.ToString(),
							customerInfo.OnCustFeeList[i].OriginalPrice.ToString(),
							customerInfo.OnCustFeeList[i].Discount.ToString(),
							customerInfo.OnCustFeeList[i].FactPrice.ToString(),
							customerInfo.OnCustFeeList[i].ID_FeeType.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Discounter.Value.ToString(),
							customerInfo.OnCustFeeList[i].DiscounterName,
							num6,
							num7,
							0,
							text26,
							text27,
							text28,
							customerInfo.OnCustFeeList[i].ApplyID
						});
						newValue2 = string.Format("update OnCustFee set ID_Fee='{0}',FeeItemName='{1}',OriginalPrice='{2}',Discount='{3}',FactPrice='{4}',ID_FeeType='{5}',Is_FeeCharged='{9}',ID_FeeCharger='{10}',FeeCharger='{11}',FeeChargeDate='{12}',ID_Discounter='{7}',DiscounterName='{8}' where ID_Customer='{6}' AND ID_Fee='{0}' AND ISNULL(Is_FeeCharged,0)=0;", new object[]
						{
							customerInfo.OnCustFeeList[i].ID_Fee.Value.ToString(),
							customerInfo.OnCustFeeList[i].FeeItemName,
							customerInfo.OnCustFeeList[i].OriginalPrice.ToString(),
							customerInfo.OnCustFeeList[i].Discount.ToString(),
							customerInfo.OnCustFeeList[i].FactPrice.ToString(),
							customerInfo.OnCustFeeList[i].ID_FeeType.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Customer.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Discounter.Value.ToString(),
							customerInfo.OnCustFeeList[i].DiscounterName,
							num6,
							text26,
							text27,
							text28
						});
					}
					else
					{
						newValue = string.Format("INSERT INTO OnCustFee(ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,RegistDate\r\n,OriginalPrice,Discount,FactPrice,ID_FeeType,ID_Discounter,DiscounterName,CustFeeChargeState,Is_FeeCharged,Is_Printed,ApplyID)\r\nVALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',0,0,'{13}');", new object[]
						{
							customerInfo.OnCustFeeList[i].ID_Customer.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Fee.Value.ToString(),
							customerInfo.OnCustFeeList[i].FeeItemName,
							customerInfo.OnCustFeeList[i].ID_Register.Value.ToString(),
							customerInfo.OnCustFeeList[i].RegisterName,
							customerInfo.OnCustFeeList[i].RegistDate.Value.ToString(),
							customerInfo.OnCustFeeList[i].OriginalPrice.ToString(),
							customerInfo.OnCustFeeList[i].Discount.ToString(),
							customerInfo.OnCustFeeList[i].FactPrice.ToString(),
							customerInfo.OnCustFeeList[i].ID_FeeType.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Discounter.Value.ToString(),
							customerInfo.OnCustFeeList[i].DiscounterName,
							num7,
							customerInfo.OnCustFeeList[i].ApplyID
						});
						newValue2 = string.Format("update OnCustFee set ID_Fee='{0}',FeeItemName='{1}',OriginalPrice='{2}',Discount='{3}',FactPrice='{4}',ID_FeeType='{5}',ID_Discounter='{7}',DiscounterName='{8}' where ID_Customer='{6}' AND ID_Fee='{0}' AND ISNULL(Is_FeeCharged,0)=0;", new object[]
						{
							customerInfo.OnCustFeeList[i].ID_Fee.Value.ToString(),
							customerInfo.OnCustFeeList[i].FeeItemName,
							customerInfo.OnCustFeeList[i].OriginalPrice.ToString(),
							customerInfo.OnCustFeeList[i].Discount.ToString(),
							customerInfo.OnCustFeeList[i].FactPrice.ToString(),
							customerInfo.OnCustFeeList[i].ID_FeeType.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Customer.Value.ToString(),
							customerInfo.OnCustFeeList[i].ID_Discounter.Value.ToString(),
							customerInfo.OnCustFeeList[i].DiscounterName
						});
					}
					text21 += text25.Clone().ToString().Replace("@TempInsertSql", newValue).Replace("@TempUpdateSql", newValue2);
					text21 += string.Format("if not exists(select ID_Section from OnCustExamSection where ID_Customer='{0}' and ID_Section='{1}')\r\n                    begin\r\n                        INSERT INTO OnCustExamSection(ID_Customer,ID_Section,SectionName,CustomerName)\r\n                        values('{0}','{1}','{2}','{3}');\r\n                    end;", new object[]
					{
						customerInfo.OnCustFeeList[i].ID_Customer.Value.ToString(),
						customerInfo.OnCustFeeList[i].ID_Section,
						customerInfo.OnCustFeeList[i].SectionName,
						customerName
					});
					if (i != 0 && i % this._defaultCount == 0)
					{
						list.Add(text21);
						text21 = string.Empty;
					}
				}
				if (text21 != string.Empty)
				{
					list.Add(text21);
				}
				if (num == 1)
				{
					text21 += string.Format("UPDATE OnCustPhysicalExamInfo SET Is_FeeSettled=0 WHERE ID_Customer='{0}';", text8);
				}
				string item = string.Format("IF NOT EXISTS(select TOP 1 ID_Customer FROM OnCustFee WHERE ID_Customer='{0}' AND Is_FeeCharged!=1)\r\nBEGIN\r\n     UPDATE OnCustPhysicalExamInfo SET Is_FeeSettled=1 WHERE ID_Customer='{0}';\r\nEND\r\nELSE \r\nBEGIN\r\n     UPDATE OnCustPhysicalExamInfo SET Is_FeeSettled=0 WHERE ID_Customer='{0}';\r\nEND;", text8);
				list.Add(item);
				if (isNew)
				{
					list.Add(string.Format("UPDATE OnArcCust SET UnfinishedNum=ISNULL(UnfinishedNum,0)+1 WHERE IDCard='{0}' AND CustomerName='{1}';", iDCard, customerName));
				}
			}
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public DataTable GetFeeWay()
		{
			object cache = DataCache.GetCache("FeeWay");
			DataTable result;
			if (cache != null)
			{
				result = (DataTable)cache;
			}
			else
			{
				result = CommonExcuteSql.Instance.ExcuteSql("SELECT FeeWayID,FeeWayName,[Default],InputCode FROM DictFeeWay").Tables[0];
			}
			return result;
		}

		public DataTable GetCustomerUnionBusFee(string UserName, string ID_Team, string ID_TeamTask, string ID_TeamTaskGroup, string ID_CustomerS)
		{
			string text = string.Format("SELECT ISNULL(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,OnCustFee.OriginalPrice Price,OnCustFee.Discount Discount,OnCustFee.FactPrice FactPrice \r\nFROM OnCustFee\r\nLEFT JOIN BusFee on OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\nLEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee WHERE ISNULL(Is_FeeCharged,0)=0 and isnull(BusFee.Is_Banned,0)!=1 AND OnCustFee.ID_Customer IN({0});", ID_CustomerS);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public DataTable SearchCustomerUnionBusFee(string UserName, string ID_Team, string ID_TeamTask, string ID_TeamTaskGroup, string ID_CustomerS, string InputCode, string SelectedFee)
		{
			string text = string.Format("SELECT ISNULL(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,OnCustFee.OriginalPrice Price,OnCustFee.Discount Discount,OnCustFee.FactPrice FactPrice \r\nFROM OnCustFee\r\nLEFT JOIN BusFee on OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\nLEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee WHERE ISNULL(Is_FeeCharged,0)=0 AND ISNULL(Is_Examined,0)=0 AND isnull(BusFee.Is_Banned,0)!=1 AND OnCustFee.ID_Customer IN({0}) ORDER BY InputCode ASC;", ID_CustomerS);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			DataTable dataTable = CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
			if (!dataTable.Columns.Contains("IsChecked"))
			{
				dataTable.Columns.Add("IsChecked");
			}
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable(dataTable.Rows.Count);
			hashtable.Clear();
			DataTable dataTable2 = dataTable.Clone();
			DataRow[] array = null;
			if (InputCode == string.Empty)
			{
				dataTable2 = dataTable.Clone();
				if (!string.IsNullOrWhiteSpace(SelectedFee))
				{
					array = dataTable.Select("ID_Fee in(" + SelectedFee + ")");
					if (array.Length > 0)
					{
						DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataRow["IsChecked"] = 1;
								dataTable2.ImportRow(dataRow);
							}
						}
					}
					foreach (DataRow dataRow in dataTable.Rows)
					{
						//DataRow dataRow;
						if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				else
				{
					dataTable2 = dataTable.Copy();
				}
			}
			else if (dataTable.Rows.Count > 0)
			{
				DataRow[] array3;
				DataRow[] array4;
				DataRow[] array5;
				if (!string.IsNullOrWhiteSpace(SelectedFee))
				{
					array = dataTable.Select("ID_Fee in(" + SelectedFee + ")");
					array3 = dataTable.Select(string.Concat(new string[]
					{
						"ID_Fee NOT in(",
						SelectedFee,
						") AND InputCode='",
						InputCode,
						"'"
					}));
					array4 = dataTable.Select(string.Concat(new string[]
					{
						"ID_Fee NOT in(",
						SelectedFee,
						") AND InputCode<>'",
						InputCode,
						"' AND InputCode LIKE '",
						InputCode,
						"%'"
					}));
					array5 = dataTable.Select(string.Concat(new string[]
					{
						"ID_Fee NOT in(",
						SelectedFee,
						") AND InputCode<>'",
						InputCode,
						"' AND InputCode NOT LIKE '",
						InputCode,
						"%' AND InputCode LIKE '%",
						InputCode,
						"%'"
					}));
				}
				else
				{
					array3 = dataTable.Select("InputCode='" + InputCode + "'");
					array4 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' AND InputCode LIKE '",
						InputCode,
						"%'"
					}));
					array5 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' AND InputCode NOT LIKE '",
						InputCode,
						"%' AND InputCode LIKE '%",
						InputCode,
						"%'"
					}));
				}
				if (array3 != null)
				{
					if (array3.Length == 1)
					{
						DataRow[] array2 = array3;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataRow["IsChecked"] = 2;
								dataTable2.ImportRow(dataRow);
							}
						}
					}
					else
					{
						DataRow[] array2 = array3;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataTable2.ImportRow(dataRow);
							}
						}
					}
				}
				if (array4 != null)
				{
					if (array4.Length == 1)
					{
						DataRow[] array2 = array4;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataRow["IsChecked"] = 2;
								dataTable2.ImportRow(dataRow);
							}
						}
					}
					else
					{
						DataRow[] array2 = array4;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataTable2.ImportRow(dataRow);
							}
						}
					}
				}
				if (array5 != null)
				{
					DataRow[] array2 = array5;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				if (array != null)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (!hashtable.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							hashtable.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable2.ImportRow(dataRow);
						}
					}
				}
			}
			hashtable.Clear();
			dataTable.Dispose();
			return dataTable2;
		}

		public DataTable GetBusFeeNotINCustFeeID(string Gender, string CustFeeID, string UserName, string Discount)
		{
			DataCache.DeleteCache("TeamTaskGroupFee");
			string text = string.Empty;
			string empty = string.Empty;
			text = "SELECT distinct -1 CustFeeChargeState,ID_Fee,ForGender,BusSetFeeDetail.ID_Set,'@userName' userName,'@date' date,BusFee.ID_Section,SYSSection.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,OperationalDate FROM BusFee\r\nINNER JOIN SYSSection on BusFee.ID_Section=SYSSection.SectionID \r\nLEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee WHERE isnull(BusFee.Is_Banned,0)!=1 @where ORDER BY InputCode ASC";
			text = text.Replace("@where", empty);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			text = text.Replace("@Discount", Discount.ToString());
			DataTable dataTable = CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
			int configInt = ConfigHelper.GetConfigInt("ModelCache");
			DataCache.SetCache("TeamTaskGroupFee", dataTable, DateTime.Now.AddMinutes((double)configInt), System.TimeSpan.Zero);
			return dataTable;
		}

		public DataTable SearchBusFee(string InputCode, string Gender, string CustFeeID, string UserName, string Discount)
		{
			object cache = DataCache.GetCache("TeamTaskGroupFee");
			DataTable result;
			if (cache != null)
			{
				DataTable dataTable = (DataTable)cache;
				DataTable dataTable2 = dataTable.Clone();
				DataTable dataTable3 = dataTable.Clone();
				if (InputCode == string.Empty)
				{
					result = dataTable;
				}
				else
				{
					DataRow[] array = dataTable.Select("InputCode='" + InputCode + "'");
					DataRow[] array2 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' AND InputCode like '",
						InputCode,
						"%'"
					}));
					DataRow[] array3 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' AND InputCode like '%",
						InputCode,
						"'"
					}));
					DataRow[] array4 = array;
					for (int i = 0; i < array4.Length; i++)
					{
						DataRow row = array4[i];
						dataTable2.ImportRow(row);
					}
					array4 = array2;
					for (int i = 0; i < array4.Length; i++)
					{
						DataRow row = array4[i];
						dataTable2.ImportRow(row);
					}
					array4 = array3;
					for (int i = 0; i < array4.Length; i++)
					{
						DataRow row = array4[i];
						dataTable2.ImportRow(row);
					}
					dataTable3.Dispose();
					dataTable.Dispose();
					result = dataTable2;
				}
			}
			else
			{
				result = this.GetBusFeeNotINCustFeeID(Gender, CustFeeID, UserName, Discount);
			}
			return result;
		}

		public DataTable GetCustomerByIDCard(string modelName, string Key, string IDCard)
		{
			int num = 1;
			if (modelName.ToLower() == "sign")
			{
				num = 0;
			}
			string sql = string.Empty;
			if (Key.Trim() != string.Empty)
			{
				if (Key == "IDCard")
				{
					sql = string.Format("SELECT OnCustRelationCustPEInfo.ID_Customer,ISNULL(Is_Team,0) Is_Team,IDCard,OnArcCust.CustomerName,ExamCard,ID_Gender,ID_Marriage,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,CultrulID,MobileNo,Email,Photo\r\nFROM OnArcCust LEFT JOIN OnCustRelationCustPEInfo ON OnArcCust.ID_ArcCustomer=OnCustRelationCustPEInfo.ID_ArcCustomer\r\nLEFT JOIN (SELECT Is_Team,ID_Customer FROM OnCustPhysicalExamInfo WHERE Is_Subscribed='{1}' GROUP BY Is_Team,ID_Customer) OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nWHERE OnArcCust.IDCard='{0}';\r\nSELECT DISTINCT BusFee.ID_Section,BusFee.SectionName,isnull(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,OnCustFee.ID_FeeType,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,OnCustFee.RegisterName userName,CONVERT(varchar(10),OnCustFee.RegistDate,120) date,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice\r\nFROM BusFee INNER JOIN OnCustFee ON OnCustFee.ID_Fee=BusFee.ID_Fee \r\nWHERE ID_Customer IN(SELECT ID_Customer FROM OnCustRelationCustPEInfo WHERE IDCardNo='{0}');", IDCard, num);
				}
				else if (Key == "ExamCard")
				{
					sql = string.Format("SELECT OnCustRelationCustPEInfo.ID_Customer,ISNULL(Is_Team,0) Is_Team,IDCard,OnArcCust.CustomerName,ExamCard,ID_Gender,ID_Marriage,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,CultrulID,MobileNo,Email,Photo\r\nFROM OnArcCust LEFT JOIN OnCustRelationCustPEInfo ON OnArcCust.ID_ArcCustomer=OnCustRelationCustPEInfo.ID_ArcCustomer\r\nLEFT JOIN (SELECT Is_Team,ID_Customer FROM OnCustPhysicalExamInfo WHERE Is_Subscribed='{1}' GROUP BY Is_Team,ID_Customer) OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nWHERE OnArcCust.ExamCard='{0}';\r\nSELECT DISTINCT BusFee.ID_Section,BusFee.SectionName,isnull(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,OnCustFee.ID_FeeType,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,OnCustFee.RegisterName userName,CONVERT(varchar(10),OnCustFee.RegistDate,120) date,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice\r\nFROM BusFee INNER JOIN OnCustFee ON OnCustFee.ID_Fee=BusFee.ID_Fee \r\nWHERE ID_Customer IN(SELECT ID_Customer FROM OnCustRelationCustPEInfo WHERE ExamCardNo='{0}');", IDCard, num);
				}
				else if (Key == "ID_Customer")
				{
					sql = string.Format("SELECT OnCustRelationCustPEInfo.ID_Customer,ISNULL(Is_Team,0) Is_Team,IDCard,OnArcCust.CustomerName,ExamCard,ID_Gender,ID_Marriage,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,CultrulID,MobileNo,Email,Photo\r\nFROM OnArcCust INNER JOIN OnCustRelationCustPEInfo ON OnArcCust.ID_ArcCustomer=OnCustRelationCustPEInfo.ID_ArcCustomer\r\nINNER JOIN (SELECT Is_Team,ID_Customer FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}' GROUP BY Is_Team,ID_Customer) OnCustPhysicalExamInfo ON OnCustRelationCustPEInfo.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nWHERE OnCustRelationCustPEInfo.ID_Customer='{0}';\r\nSELECT DISTINCT BusFee.ID_Section,BusFee.SectionName,isnull(OnCustFee.CustFeeChargeState,-1) CustFeeChargeState,OnCustFee.ID_FeeType,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,OnCustFee.RegisterName userName,CONVERT(varchar(10),OnCustFee.RegistDate,120) date,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice \r\nFROM BusFee INNER JOIN OnCustFee ON OnCustFee.ID_Fee=BusFee.ID_Fee where ID_Customer='{0}';", IDCard);
				}
			}
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataTable GetCustomerByIDCardX(string modelName, string Key, string IDCard, string CustomerName, int IsGenerateCustomerCard)
		{
			string sql = string.Empty;
			if (Key.Trim() != string.Empty)
			{
				if (Key == "IDCard")
				{
					if (CustomerName != string.Empty)
					{
						sql = string.Format("SELECT ID_ArcCustomer,IDCard,CustomerName,ExamCard,ID_Gender,GenderName,ID_Marriage,MarriageName,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,ID_Cultrul,MobileNo,Email,Photo FROM OnArcCust WHERE IDCard='{0}' AND CustomerName='{1}';", IDCard, CustomerName);
					}
					else
					{
						sql = string.Format("SELECT ID_ArcCustomer,IDCard,CustomerName,ExamCard,ID_Gender,GenderName,ID_Marriage,MarriageName,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,ID_Cultrul,MobileNo,Email,Photo FROM OnArcCust WHERE IDCard='{0}';", IDCard);
					}
					if (IsGenerateCustomerCard == 1)
					{
						if (CustomerName != string.Empty)
						{
							sql = string.Format("SELECT ID_ArcCustomer,IDCard,CustomerName,ExamCard,ID_Gender,GenderName,ID_Marriage,MarriageName,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,ID_Cultrul,MobileNo,Email,Photo FROM OnArcCust WHERE IDCard='{0}' AND CustomerName='{1}';", IDCard, CustomerName);
						}
					}
				}
				else if (Key == "ExamCard")
				{
					if (CustomerName != string.Empty)
					{
						sql = string.Format("SELECT ID_ArcCustomer,IDCard,CustomerName,ExamCard,ID_Gender,GenderName,ID_Marriage,MarriageName,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,ID_Cultrul,MobileNo,Email,Photo FROM OnArcCust WHERE ExamCard='{0}' AND CustomerName='{1}';", IDCard, CustomerName);
					}
					else
					{
						sql = string.Format("SELECT ID_ArcCustomer,IDCard,CustomerName,ExamCard,ID_Gender,GenderName,ID_Marriage,MarriageName,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,ID_Cultrul,MobileNo,Email,Photo FROM OnArcCust WHERE ExamCard='{0}';", IDCard);
					}
				}
				else if (Key == "ID_Customer")
				{
					sql = string.Format("SELECT '{0}' ID_Customer,(SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}') ID_ArcCustomer,IDCard,CustomerName,ExamCard,ID_Gender,GenderName,ID_Marriage,MarriageName,CONVERT(varchar(10),BirthDay,120) date,ID_Nation,ID_Cultrul,MobileNo,Email,Photo FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}';", IDCard);
				}
			}
			return CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
		}

		public DataSet GetCustomerByIDCustomer(string ID_Customer)
		{
			string sql = string.Format("SELECT CustomerName,MarriageName,GenderName,CONVERT(varchar(10),BirthDay,120) date,IDCard,MobileNo,Photo FROM OnArcCust WHERE ID_ArcCustomer =(SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}');\r\nSELECT CONVERT(varchar(10),OnCustPhysicalExamInfo.OperateDate,120) OperateDate,Operator,[dbo].StrToCode128(ID_Customer) Code128c,ISNULL(Is_Checked,0) Is_Checked,Checker,CONVERT(varchar(10),CheckedDate,120) CheckedDate,ISNULL(OnCustPhysicalExamInfo.Is_GuideSheetPrinted,0) Is_GuideSheetPrinted,ISNULL(OnCustPhysicalExamInfo.Is_Subscribed,-1) Is_Subscribed,OnCustPhysicalExamInfo.ID_Customer,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) SubScribDate,OnCustPhysicalExamInfo.ExamTypeName,Team.TeamName FROM OnCustPhysicalExamInfo \r\nLEFT JOIN Team ON OnCustPhysicalExamInfo.ID_Team=Team.ID_Team\r\nWHERE OnCustPhysicalExamInfo.ID_Customer='{0}';SELECT ISNULL(Is_ReportPrinted,0) Is_ReportPrinted,ReportPrinter,CONVERT(varchar(10),ReportPrintedDate,120) ReportPrintedDate\r\n,ISNULL(Is_Informed,0) Is_Informed,Informer,CONVERT(varchar(10),InformedDate,120) InformedDate\r\n,ISNULL(Is_ReportReceipted,0) Is_ReportReceipted,ReportReceiptor,CONVERT(varchar(10),ReportReceiptedDate,120) ReportReceiptedDate FROM OnCustReportManage WHERE ID_Customer='{0}';", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetCustomerByIDCustomer(string AppSettingKey, string ID_Customer)
		{
			string sql = string.Format("SELECT OCPEI.*,[dbo].StrToCode128(OCPEI.ID_Customer) Code128c,ISNULL(Is_ReportPrinted,0) Is_ReportPrinted,ReportPrinter,CONVERT(varchar(10),ReportPrintedDate,120) ReportPrintedDate\r\n,ISNULL(Is_Informed,0) Is_Informed,Informer,CONVERT(varchar(10),InformedDate,120) InformedDate\r\n,ISNULL(OCRM.Is_ReportReceipted,0) Is_ReportReceipted,ReportReceiptor,CONVERT(varchar(10),ReportReceiptedDate,120) ReportReceiptedDate\r\n,ReportChecker,CONVERT(varchar(10),ReportCheckDate,120)ReportCheckDate,ReportOffer,CONVERT(varchar(10),ReportOffDate,120)ReportOffDate FROM(\r\nSELECT Is_GuideSheetPrinted,Is_Subscribed,ID_Customer,CONVERT(varchar(10),SubScribDate,120)SubScribDate,ExamTypeName,TeamName,SubScriber,CONVERT(varchar(10),SubScriberOperDate,120)SubScriberOperDate,Operator,CONVERT(varchar(10),OperateDate,120)OperateDate,GuideSheetReturnedby,\r\nCONVERT(varchar(10),GuideSheetReturnedDate,120)GuideSheetReturnedDate\r\n,FinalDoctor,CONVERT(varchar(10),FinalDate,120)FinalDate,ISNULL(Is_Checked,0)Is_Checked,Checker,CONVERT(varchar(10),CheckedDate,120)CheckedDate\r\n,CustomerName,MarriageName,GenderName,CONVERT(varchar(10),BirthDay,120) date\r\n,FLOOR(DATEDIFF(DY,BirthDay,(SELECT CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END OperateDate))/365.25) Age,IDCard,MobileNo,Photo \r\nFROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}')  OCPEI\r\nLEFT JOIN OnCustReportManage OCRM ON OCPEI.ID_Customer=OCRM.ID_Customer;", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(AppSettingKey, sql);
		}

		public DataTable GetBusFeeByCustomerID(string ID_Customer, string UserName)
		{
			string text = string.Empty;
			if (ID_Customer != string.Empty)
			{
				text = string.Format("SELECT * FROM(SELECT DISTINCT OperationalDate,TeamTaskGroupFee.ID_TeamTaskGroup ,ISNULL(BusFee.Is_Banned,0)Is_Banned,BusFee.ID_Section,BusFee.SectionName,OnCustFee.RegisterName Operator,CONVERT(varchar(10),OnCustFee.RegistDate,120) OperateDate,OnCustFee.ID_CustFee,OnCustFee.ID_Customer,ISNULL(OnCustFee.Is_Printed,0) Is_Printed,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice,OnCustFee.ID_Discounter,OnCustFee.DiscounterName,isnull(OnCustFee.CustFeeChargeState,0) CustFeeChargeState,ISNULL(Is_FeeCharged,0) Is_FeeCharged ,ISNULL(Is_FeeRefund,0) Is_FeeRefund\r\n ,CASE WHEN Is_FeeRefund=1 THEN 2 WHEN Is_FeeCharged=1 THEN 1 ELSE 0 END FeeChargeStaute,FeeCharger,CONVERT(varchar(10),OnCustFee.FeeChargeDate,120) FeeChargeDate,FeeRefunder,CONVERT(varchar(10),OnCustFee.FeeRefunderDate,120) FeeRefunderDate,ISNULL(OnCustFee.Is_Examined,0) Is_Examined,OnCustFee.ID_FeeType,DictFeeWay.FeeWayName,OnCustFee.ID_Register,OnCustFee.RegisterName RegisterName,CONVERT(varchar(20),OnCustFee.RegistDate,120) RegistDate,'@userName' userName,'@date' date FROM(SELECT * FROM OnCustFee WHERE ID_Customer='{0}')OnCustFee\r\nLEFT JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN (SELECT ID_TeamTaskGroup,ID_Fee FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup=(SELECT ID_TeamTaskGroup FROM TeamTaskGroupCust WHERE ID_Customer='{0}'))TeamTaskGroupFee ON OnCustFee.ID_Fee=TeamTaskGroupFee.ID_Fee\r\nLEFT JOIN DictFeeWay ON OnCustFee.ID_FeeType=DictFeeWay.FeeWayID WHERE ID_Customer='{0}') OnCustFee ORDER BY ID_CustFee ASC;", ID_Customer);
			}
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public DataTable GetBusFeeBySetID(string PEID, string PEPackageID, string UserID, string UserName, string Discount)
		{
			string text = string.Empty;
			if (PEPackageID != string.Empty)
			{
				if (PEID == string.Empty)
				{
					text = string.Format("SELECT '@UserID' ID_Discounter,'@userName' DiscounterName,'@UserID' ID_Register,'@userName' RegisterName,'@date' RegistDate,0 Is_Printed,0 Is_FeeRefund,0 Is_FeeCharged,0 FeeChargeStaute,'' ID_TeamTaskGroup,BusFee.ID_Section,BusFee.SectionName,'' ID_CustFee,'' ID_Customer,BusFee.ID_Fee,BusFee.FeeName,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,0 CustFeeChargeState,0 Is_FeeCharged,0 Is_FeeRefund,0 FeeChargeStaute,OperationalDate,'{0}' PEPackageID FROM BusFee inner join SYSSection on BusFee.ID_Section =SYSSection.SectionID where isnull(BusFee.Is_Banned,0)!=1 and BusFee.ID_Fee in(select ID_FeeItem from BusSetFeeDetail where ID_Set='{0}');", PEPackageID);
				}
				else
				{
					text = string.Format("SELECT '@PEID' ID_Discounter,'@userName' DiscounterName,'@UserID' ID_Register,'@userName' RegisterName,'@date' RegistDate,0 Is_Printed,0 Is_FeeRefund,0 Is_FeeCharged,0 FeeChargeStaute,TeamTaskGroupFee.ID_TeamTaskGroup,0 CustFeeChargeState,'{0}' PEPackageID,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,OperationalDate FROM BusFee  INNER JOIN SYSSection on BusFee.ID_Section =SYSSection.SectionID LEFT JOIN (SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup=(SELECT ID_TeamTaskGroup FROM TeamTaskGroupCust WHERE ID_Customer='{1}')) TeamTaskGroupFee ON BusFee.ID_Fee=TeamTaskGroupFee.ID_Fee\r\nWHERE ISNULL(BusFee.Is_Banned,0)!=1 and BusFee.ID_Fee in(select ID_FeeItem from BusSetFeeDetail where ID_Set='{0}');", PEPackageID, PEID);
				}
			}
			text = text.Replace("@UserID", UserID);
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			text = text.Replace("@Discount", Discount.ToString());
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public DataTable GetBusFeeBySetIDAndGroupID(string PEPackageID, string TeamTaskGroupID, string UserName, string Discount)
		{
			string text = string.Empty;
			if (PEPackageID != string.Empty && TeamTaskGroupID == string.Empty)
			{
				text = string.Format("SELECT -1 CustFeeChargeState,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,OperationalDate FROM BusFee \r\ninner join SYSSection on BusFee.ID_Section =SYSSection.SectionID\r\nwhere isnull(BusFee.Is_Banned,0)!=1 and BusFee.ID_Fee in(select ID_FeeItem from BusSetFeeDetail where PEPackageID='{0}');", PEPackageID);
			}
			if (PEPackageID != string.Empty && TeamTaskGroupID != string.Empty)
			{
				text = string.Format("SELECT -1 CustFeeChargeState,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,OperationalDate FROM BusFee \r\ninner join SYSSection on BusFee.ID_Section =SYSSection.SectionID\r\nwhere isnull(BusFee.Is_Banned,0)!=1 and BusFee.ID_Fee in(select ID_FeeItem from BusSetFeeDetail where PEPackageID='{0}' AND ID_FeeItem NOT IN({1}));", PEPackageID, TeamTaskGroupID);
			}
			text = text.Replace("@userName", UserName);
			text = text.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			text = text.Replace("@Discount", Discount.ToString());
			return CommonExcuteSql.Instance.ExcuteSql(text).Tables[0];
		}

		public int DelData(string ItemExamCard, string ItemArcCustomer)
		{
			string item = string.Format("delete from OnCustExamSection where ID_Customer in({0});\r\ndelete from OnCustFee where ID_Customer in({0});\r\ndelete from OnCustRelationCustPEInfo where ID_Customer in({0});\r\ndelete from OnCustPhysicalExamInfo where ID_Customer in({0});", ItemExamCard, ItemArcCustomer);
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int DeleteCustFee(string ID_Customer, string CustFeeID)
		{
			string item = string.Format("DELETE FROM OnCustFee WHERE ID_Customer='{0}' AND ID_CustFee IN({1});", ID_Customer, CustFeeID);
			List<string> list = new List<string>();
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int DoPrint(string ID_Customer)
		{
			string item = string.Format("", ID_Customer);
			List<string> list = new List<string>();
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public DataSet GetCustomerExamPhysicalInfo(string ID_ArcCustomer, int IsTeam)
		{
			string text = string.Format("DECLARE @CustomerName VARCHAR(30)\r\nDECLARE @IDCard VARCHAR(30)\r\nSELECT @CustomerName=CustomerName,@IDCard=IDCard FROM OnArcCust WHERE ID_ArcCustomer='{0}'\r\nPRINT @CustomerName\r\nPRINT @IDCard\r\n--将客户所有体检号存入到中间表\r\nSELECT  TOP 1 '{0}' ID_ArcCustomer,ID_Customer INTO #TempOnCustRelationCustPEInfo@RandNum FROM OnCustPhysicalExamInfo WHERE CustomerName=@CustomerName AND IDCard=@IDCard AND ISNULL(Is_Team,0)='{1}' AND ISNULL(Is_GuideSheetPrinted,0)=0 ORDER BY SubScriberOperDate DESC\r\n--找到某人的体检信息,存入体检信息中间表\r\nSELECT TOP 1 CONVERT(varchar(10),OPE.SubScribDate,120) SubScribDateX,[dbo].StrToCode128(OPE.ID_Customer) Code128c\r\n,OAC.*,OPE.*\r\n,RoleName,TeamTaskGroupCust.Department,TeamTaskGroupCust.DepartSubA,TeamTaskGroupCust.DepartSubB,TeamTaskGroupCust.DepartSubC \r\nINTO #TempOnCustPhysicalExamInfo@RandNum FROM (SELECT * FROM OnArcCust WHERE ID_ArcCustomer='{0}') OAC\r\nLEFT JOIN #TempOnCustRelationCustPEInfo@RandNum TORCPE ON OAC.ID_ArcCustomer=TORCPE.ID_ArcCustomer\r\nLEFT JOIN(SELECT ID_Customer,Is_Team,ID_Team,TeamName,Is_Paused,Is_SectionLock,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,ExamPlaceID,ExamPlaceName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,SecurityLevel,DiseaseLevel,Is_FeeSettled,Is_GuideSheetPrinted\r\n,SubScribDate,OperateDate,Is_Subscribed,Note FROM OnCustPhysicalExamInfo WHERE ISNULL(Is_Team,0)='{1}') OPE ON TORCPE.ID_Customer=OPE.ID_Customer\r\nLEFT JOIN TEAM ON OPE.ID_Team=Team.ID_Team\r\nLEFT JOIN TeamTaskGroupCust ON OPE.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nLEFT JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup\r\nWHERE ISNULL(Is_Team,0)='{1}' ORDER BY SubScribDateX DESC\r\n--返回客户对应团体的最近一次体检信息\r\nSELECT * FROM #TempOnCustPhysicalExamInfo@RandNum\r\n--返回客户收费项目明细信息\r\nDECLARE @ID_Customer BIGINT\r\nSELECT @ID_Customer=ID_Customer FROM #TempOnCustPhysicalExamInfo@RandNum\r\n--PRINT @ID_Customer\r\nSELECT * FROM(SELECT DISTINCT ISNULL(BusFee.Is_Banned,0) Is_Banned,TeamTaskGroupFee.ID_TeamTaskGroup ,BusFee.ID_Section,BusFee.SectionName,OnCustFee.ID_CustFee,ISNULL(OnCustFee.Is_Printed,0) Is_Printed,OnCustFee.ID_Customer,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice,OnCustFee.DiscounterName,isnull(OnCustFee.CustFeeChargeState,0) CustFeeChargeState,ISNULL(Is_FeeCharged,0) Is_FeeCharged ,ISNULL(Is_FeeRefund,0) Is_FeeRefund\r\n ,CASE WHEN Is_FeeRefund=1 THEN 2 WHEN Is_FeeCharged=1 THEN 1 ELSE 0 END FeeChargeStaute,FeeCharger,CONVERT(varchar(10),OnCustFee.FeeChargeDate,120) FeeChargeDate,FeeRefunder,CONVERT(varchar(10),OnCustFee.FeeRefunderDate,120) FeeRefunderDate,ISNULL(OnCustFee.Is_Examined,0) Is_Examined,OnCustFee.ID_FeeType,DictFeeWay.FeeWayName,OnCustFee.ID_Register,OnCustFee.RegisterName,CONVERT(varchar(10),OnCustFee.RegistDate,120) RegistDate\r\nFROM (SELECT * FROM OnCustFee WHERE ID_Customer=@ID_Customer) OnCustFee\r\nLEFT JOIN DictFeeWay ON OnCustFee.ID_FeeType=DictFeeWay.ID_FeeWay\r\nLEFT JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN (SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup=(SELECT ID_TeamTaskGroup FROM TeamTaskGroupCust WHERE ID_Customer=@ID_Customer)\r\n)TeamTaskGroupFee ON OnCustFee.ID_Fee=TeamTaskGroupFee.ID_Fee\r\nWHERE ID_Customer=@ID_Customer)OnCustFee ORDER BY ID_CustFee ASC;\r\nDROP TABLE #TempOnCustPhysicalExamInfo@RandNum\r\nDROP TABLE #TempOnCustRelationCustPEInfo@RandNum", ID_ArcCustomer, IsTeam);
			text = text.Replace("@RandNum", Public.GetGuid("-", string.Empty));
			return CommonExcuteSql.Instance.ExcuteSql(text);
		}

		public DataSet GetCustomerExamPhysicalInfo(string ID_ArcCustomer)
		{
			string text = string.Format("DECLARE @CustomerName VARCHAR(30)\r\nDECLARE @IDCard VARCHAR(30)\r\nSELECT @CustomerName=CustomerName,@IDCard=IDCard FROM OnArcCust WHERE ID_ArcCustomer='{0}'\r\nPRINT @CustomerName\r\nPRINT @IDCard\r\n--将客户所有体检号存入到中间表\r\nSELECT  TOP 1 '{0}' ID_ArcCustomer,ID_Customer INTO #TempOnCustRelationCustPEInfo@RandNum FROM OnCustPhysicalExamInfo WHERE CustomerName=@CustomerName AND IDCard=@IDCard ORDER BY SubScriberOperDate DESC\r\n--找到某人的体检信息,存入体检信息中间表\r\nSELECT TOP 1 CONVERT(varchar(10),OPE.SubScribDate,120) SubScribDateX,[dbo].StrToCode128(OPE.ID_Customer) Code128c\r\n,OAC.*,OPE.*\r\n,RoleName,TeamTaskGroupCust.Department,TeamTaskGroupCust.DepartSubA,TeamTaskGroupCust.DepartSubB,TeamTaskGroupCust.DepartSubC \r\nINTO #TempOnCustPhysicalExamInfo@RandNum FROM (SELECT * FROM OnArcCust WHERE ID_ArcCustomer='{0}') OAC\r\nLEFT JOIN #TempOnCustRelationCustPEInfo@RandNum TORCPE ON OAC.ID_ArcCustomer=TORCPE.ID_ArcCustomer\r\nLEFT JOIN(SELECT ID_Customer,Is_Team,ID_Team,TeamName,Is_Paused,Is_SectionLock,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,ExamPlaceID,ExamPlaceName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,SecurityLevel,DiseaseLevel,Is_FeeSettled,Is_GuideSheetPrinted\r\n,SubScribDate,OperateDate,Is_Subscribed,Note FROM OnCustPhysicalExamInfo) OPE ON TORCPE.ID_Customer=OPE.ID_Customer\r\nLEFT JOIN TEAM ON OPE.ID_Team=Team.ID_Team\r\nLEFT JOIN TeamTaskGroupCust ON OPE.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nLEFT JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup ORDER BY SubScribDateX DESC\r\n--返回客户对应团体的最近一次体检信息\r\nSELECT * FROM #TempOnCustPhysicalExamInfo@RandNum\r\n--返回客户收费项目明细信息\r\nDECLARE @ID_Customer BIGINT\r\nSELECT @ID_Customer=ID_Customer FROM #TempOnCustPhysicalExamInfo@RandNum\r\n--PRINT @ID_Customer\r\nSELECT * FROM(SELECT DISTINCT ISNULL(BusFee.Is_Banned,0)Is_Banned,TeamTaskGroupFee.ID_TeamTaskGroup ,BusFee.ID_Section,BusFee.SectionName,OnCustFee.ID_CustFee,ISNULL(OnCustFee.Is_Printed,0) Is_Printed,OnCustFee.ID_Customer,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice,OnCustFee.DiscounterName,isnull(OnCustFee.CustFeeChargeState,0) CustFeeChargeState,ISNULL(Is_FeeCharged,0) Is_FeeCharged ,ISNULL(Is_FeeRefund,0) Is_FeeRefund\r\n ,CASE WHEN Is_FeeRefund=1 THEN 2 WHEN Is_FeeCharged=1 THEN 1 ELSE 0 END FeeChargeStaute,FeeCharger,CONVERT(varchar(10),OnCustFee.FeeChargeDate,120) FeeChargeDate,FeeRefunder,CONVERT(varchar(10),OnCustFee.FeeRefunderDate,120) FeeRefunderDate,ISNULL(OnCustFee.Is_Examined,0) Is_Examined,OnCustFee.ID_FeeType,DictFeeWay.FeeWayName,OnCustFee.ID_Register,OnCustFee.RegisterName,CONVERT(varchar(10),OnCustFee.RegistDate,120) RegistDate\r\nFROM (SELECT * FROM OnCustFee WHERE ID_Customer=@ID_Customer) OnCustFee\r\nLEFT JOIN DictFeeWay ON OnCustFee.ID_FeeType=DictFeeWay.ID_FeeWay\r\nLEFT JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN (SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup=(SELECT ID_TeamTaskGroup FROM TeamTaskGroupCust WHERE ID_Customer=@ID_Customer)\r\n)TeamTaskGroupFee ON OnCustFee.ID_Fee=TeamTaskGroupFee.ID_Fee\r\nWHERE ID_Customer=@ID_Customer)OnCustFee ORDER BY ID_CustFee ASC;\r\nDROP TABLE #TempOnCustPhysicalExamInfo@RandNum\r\nDROP TABLE #TempOnCustRelationCustPEInfo@RandNum", ID_ArcCustomer);
			text = text.Replace("@RandNum", Public.GetGuid("-", string.Empty));
			return CommonExcuteSql.Instance.ExcuteSql(text);
		}

		public DataSet GetCustomerExamPhysicalInfoByID_Customer_Old(string ID_Customer)
		{
			string sql = string.Format("SELECT CASE WHEN Is_GuideSheetPrinted=1 THEN FLOOR(DATEDIFF(DY,BirthDay,OperateDate)/365.25) ELSE FLOOR(DATEDIFF(DY,BirthDay,GETDATE())/365.25) END Age,RoleName,OnCustPhysicalExamInfo.Note,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) SubScribDateX,[dbo].StrToCode128(OnCustPhysicalExamInfo.ID_Customer) Code128c,OnArcCust.ID_Nation,OnArcCust.NationName,OnCustPhysicalExamInfo.*,TEAM.TeamName,TeamTaskGroupCust.Department,TeamTaskGroupCust.DepartSubA,TeamTaskGroupCust.DepartSubB,TeamTaskGroupCust.DepartSubC FROM\r\n(SELECT * FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}') OnCustPhysicalExamInfo\r\nINNER JOIN (SELECT *,'{0}' ID_Customer FROM OnArcCust WHERE ID_ArcCustomer=(SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}'))OnArcCust ON OnCustPhysicalExamInfo.ID_Customer=OnArcCust.ID_Customer\r\nLEFT JOIN TEAM ON OnCustPhysicalExamInfo.ID_Team=Team.ID_Team\r\nLEFT JOIN TeamTaskGroupCust ON OnCustPhysicalExamInfo.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nLEFT JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup;SELECT * FROM(SELECT DISTINCT ID_CustFee,TeamTaskGroupFee.ID_TeamTaskGroup ,BusFee.ID_Section,BusFee.SectionName,OnCustFee.ID_Customer,ISNULL(OnCustFee.Is_Printed,0) Is_Printed,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice,OnCustFee.ID_Discounter,OnCustFee.ID_Register,OnCustFee.RegisterName,CONVERT(varchar(10),OnCustFee.RegistDate,120) RegistDate,OnCustFee.DiscounterName,isnull(OnCustFee.CustFeeChargeState,0) CustFeeChargeState,ISNULL(Is_FeeCharged,0) Is_FeeCharged ,ISNULL(Is_FeeRefund,0) Is_FeeRefund\r\n ,CASE WHEN Is_FeeRefund=1 THEN 2 WHEN Is_FeeCharged=1 THEN 1 ELSE 0 END FeeChargeStaute,FeeCharger,CONVERT(varchar(10),OnCustFee.FeeChargeDate,120) FeeChargeDate,FeeRefunder,CONVERT(varchar(10),OnCustFee.FeeRefunderDate,120) FeeRefunderDate,ISNULL(OnCustFee.Is_Examined,0) Is_Examined,OnCustFee.ID_FeeType,DictFeeWay.FeeWayName,OnCustFee.RegisterName userName,CONVERT(varchar(10),OnCustFee.RegistDate,120) date FROM (SELECT * FROM OnCustFee WHERE ID_Customer='{0}')OnCustFee\r\nLEFT JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN (SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup=(SELECT ID_TeamTaskGroup FROM TeamTaskGroupCust WHERE ID_Customer='{0}'))TeamTaskGroupFee ON OnCustFee.ID_Fee=TeamTaskGroupFee.ID_Fee\r\nLEFT JOIN DictFeeWay ON OnCustFee.ID_FeeType=DictFeeWay.ID_FeeWay\r\nWHERE ID_Customer='{0}') OnCustFee ORDER BY ID_CustFee ASC;", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetCustomerExamPhysicalInfoByID_Customer(string ID_Customer)
		{
			string sql = string.Format("SELECT CASE WHEN Is_GuideSheetPrinted=1 THEN FLOOR(DATEDIFF(DY,OnCustPhysicalExamInfo.BirthDay,OperateDate)/365.25) ELSE FLOOR(DATEDIFF(DY,OnCustPhysicalExamInfo.BirthDay,GETDATE())/365.25) END Age,RoleName,OnCustPhysicalExamInfo.Note,CONVERT(varchar(10),OnCustPhysicalExamInfo.SubScribDate,120) SubScribDateX,[dbo].StrToCode128(OnCustPhysicalExamInfo.ID_Customer) Code128c,ID_Nation,NationName,OnCustPhysicalExamInfo.*,TEAM.TeamName,TeamTaskGroupCust.Department,TeamTaskGroupCust.DepartSubA,TeamTaskGroupCust.DepartSubB,TeamTaskGroupCust.DepartSubC FROM\r\n(SELECT * FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}') OnCustPhysicalExamInfo\r\nLEFT JOIN TEAM ON OnCustPhysicalExamInfo.ID_Team=Team.ID_Team\r\nLEFT JOIN TeamTaskGroupCust ON OnCustPhysicalExamInfo.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nLEFT JOIN TeamTaskGroup ON TeamTaskGroupCust.ID_TeamTaskGroup=TeamTaskGroup.ID_TeamTaskGroup;\r\nSELECT * FROM(SELECT DISTINCT ID_CustFee,TeamTaskGroupFee.ID_TeamTaskGroup ,BusFee.ID_Section,BusFee.SectionName,OnCustFee.ID_Customer,ISNULL(OnCustFee.Is_Printed,0) Is_Printed,OnCustFee.ID_Fee,OnCustFee.FeeItemName FeeName,isnull(OnCustFee.OriginalPrice,0) Price,isnull(OnCustFee.Discount,100) Discount,isnull(OnCustFee.FactPrice,0) FactPrice,OnCustFee.ID_Discounter,OnCustFee.ID_Register,OnCustFee.RegisterName,CONVERT(varchar(20),OnCustFee.RegistDate,120) RegistDate,OnCustFee.DiscounterName,isnull(OnCustFee.CustFeeChargeState,0) CustFeeChargeState,ISNULL(Is_FeeCharged,0) Is_FeeCharged ,ISNULL(Is_FeeRefund,0) Is_FeeRefund\r\n ,CASE WHEN Is_FeeRefund=1 THEN 2 WHEN Is_FeeCharged=1 THEN 1 ELSE 0 END FeeChargeStaute,FeeCharger,CONVERT(varchar(20),OnCustFee.FeeChargeDate,120) FeeChargeDate,FeeRefunder,CONVERT(varchar(20),OnCustFee.FeeRefunderDate,120) FeeRefunderDate,ISNULL(OnCustFee.Is_Examined,0) Is_Examined,OnCustFee.ID_FeeType,DictFeeWay.FeeWayName,OnCustFee.RegisterName userName,CONVERT(varchar(20),OnCustFee.RegistDate,120) date FROM (SELECT * FROM OnCustFee WHERE ID_Customer='{0}')OnCustFee\r\nLEFT JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nLEFT JOIN (SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup=(SELECT ID_TeamTaskGroup FROM TeamTaskGroupCust WHERE ID_Customer='{0}'))TeamTaskGroupFee ON OnCustFee.ID_Fee=TeamTaskGroupFee.ID_Fee\r\nLEFT JOIN DictFeeWay ON OnCustFee.ID_FeeType=DictFeeWay.FeeWayID\r\nWHERE ID_Customer='{0}') OnCustFee ORDER BY ID_CustFee ASC", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public DataSet GetCustomerPrint(string ID_Customer)
		{
			string sql = string.Format("SELECT ISNULL(OCPEI.Is_GuideSheetPrinted,0) Is_GuideSheetPrinted,ISNULL(OCPEI.Is_Subscribed,-1) Is_Subscribed ,OCPEI.ID_Customer,OCPEI.ID_Team,Team.TeamName,CONVERT(varchar(10),OCPEI.SubScribDate,120) SubScribDate,CONVERT(varchar(10),TeamTask.TaskExamStartDate,120) TaskExamStartDate,CONVERT(varchar(10),TeamTask.TaskExamEndDate,120) TaskExamEndDate\r\nFROM OnCustPhysicalExamInfo OCPEI LEFT JOIN Team ON OCPEI.ID_Team=Team.ID_Team\r\nLEFT JOIN TeamTaskGroupCust ON OCPEI.ID_Customer=TeamTaskGroupCust.ID_Customer\r\nLEFT JOIN TeamTask ON TeamTaskGroupCust.ID_TeamTask=TeamTask.ID_TeamTask\r\nWHERE OCPEI.ID_Customer='{0}';", ID_Customer);
			return CommonExcuteSql.Instance.ExcuteSql(sql);
		}

		public int Charge(string ID_FeeCharger, string FeeCharger, string ID_Customer, string AllFeeID, string Invoice)
		{
			Invoice = Invoice.TrimStart(new char[]
			{
				','
			}).TrimEnd(new char[]
			{
				','
			});
			string item = string.Format("UPDATE OnCustFee SET Is_FeeCharged=1,ID_FeeCharger='{2}',FeeCharger='{3}',FeeChargeDate='{4}' WHERE ID_Customer IN({0}) AND ID_Fee IN({1});\r\nUPDATE OnCustPhysicalExamInfo SET Invoice=ISNULL(Invoice,'')+',{5}' WHERE ID_Customer='{0}';\r\nif not exists(select ID_Customer from OnCustFee where ID_Customer='{0}' and ISNULL(Is_FeeCharged,0)=0)\r\nbegin\r\n\tupdate OnCustPhysicalExamInfo set Is_FeeSettled=1 where ID_Customer='{0}';\r\nend\r\n", new object[]
			{
				ID_Customer,
				AllFeeID,
				ID_FeeCharger,
				FeeCharger,
				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				Invoice
			});
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int UnCharge(string ID_FeeRefunder, string FeeRefunder, string ID_Customer, string AllFeeID)
		{
			string item = string.Format("UPDATE OnCustFee SET Is_FeeRefund=1,ID_FeeRefunder='{2}',FeeRefunder='{3}',FeeRefunderDate='{4}',CustFeeChargeState=2 WHERE ID_Customer IN({0}) AND ID_Fee IN({1});", new object[]
			{
				ID_Customer,
				AllFeeID,
				ID_FeeRefunder,
				FeeRefunder,
				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
			});
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int UnCharge(int UnChargeType, string InvoiceStr, string ID_FeeRefunder, string FeeRefunder, string ID_Customer, string AllFeeID)
		{
			string sql = string.Format("SELECT ID_Customer,Invoice FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}';", ID_Customer);
			DataTable dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
			int result;
			if (dataTable.Rows.Count > 0)
			{
				string text = dataTable.Rows[0]["Invoice"].ToString();
				string[] array = InvoiceStr.Split(new char[]
				{
					','
				});
				if (array.Length > 0)
				{
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						if (!string.IsNullOrEmpty(text2))
						{
							text = text.Replace("," + text2, string.Empty);
						}
					}
					text = text.TrimEnd(new char[]
					{
						','
					});
				}
				string item = string.Empty;
				if (UnChargeType == 0)
				{
					item = string.Format("UPDATE OnCustFee SET Is_FeeRefund=1,ID_FeeRefunder='{2}',FeeRefunder='{3}',FeeRefunderDate='{4}',CustFeeChargeState=2 WHERE ID_Customer IN({0}) AND ID_Fee IN({1});\r\nUPDATE OnCustPhysicalExamInfo SET Is_Paused=1,Invoice='{5}' WHERE ID_Customer='{0}' AND Is_Team!=1;", new object[]
					{
						ID_Customer,
						AllFeeID,
						ID_FeeRefunder,
						FeeRefunder,
						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
						text
					});
				}
				else if (UnChargeType == 1)
				{
					if (string.IsNullOrEmpty(text))
					{
						item = string.Format("UPDATE OnCustFee SET Is_FeeRefund=1,ID_FeeRefunder='{2}',FeeRefunder='{3}',FeeRefunderDate='{4}',CustFeeChargeState=2 WHERE ID_Customer IN({0}) AND ID_Fee IN({1});\r\nUPDATE OnCustPhysicalExamInfo SET Is_Paused=1,Invoice='{5}' WHERE ID_Customer='{0}' AND Is_Team!=1;", new object[]
						{
							ID_Customer,
							AllFeeID,
							ID_FeeRefunder,
							FeeRefunder,
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
							text
						});
					}
					else
					{
						item = string.Format("UPDATE OnCustFee SET Is_FeeRefund=1,ID_FeeRefunder='{2}',FeeRefunder='{3}',FeeRefunderDate='{4}',CustFeeChargeState=2 WHERE ID_Customer IN({0}) AND ID_Fee IN({1});\r\nUPDATE OnCustPhysicalExamInfo SET Invoice='{5}',Is_Paused=0 WHERE ID_Customer='{0}';\r\n", new object[]
						{
							ID_Customer,
							AllFeeID,
							ID_FeeRefunder,
							FeeRefunder,
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
							text
						});
					}
				}
				List<string> list = new List<string>(1);
				list.Add(item);
				result = CommonExcuteSql.Instance.ExecuteSqlTran(list);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public int LoseInvoiceCharge(string ID_Customer, string InvoiceStr)
		{
			InvoiceStr = InvoiceStr.TrimStart(new char[]
			{
				','
			}).TrimEnd(new char[]
			{
				','
			});
			string item = string.Format("UPDATE OnCustPhysicalExamInfo SET Is_Paused=0,Invoice='{1}' WHERE ID_Customer='{0}';", ID_Customer, InvoiceStr);
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int SumSectionExamInfo(string ID_Customer)
		{
			string sql = string.Format("SELECT BusFee.ID_Section,OnCustFee.ID_Fee,ISNULL(OnCustFee.Is_FeeCharged,0) ID_Fee,ISNULL(OnCustFee.Is_FeeRefund,0) Is_FeeRefund FROM(SELECT ID_Fee,Is_FeeCharged,Is_FeeRefund FROM OnCustFee WHERE ID_Customer='{0}')OnCustFee\r\nINNER JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nORDER BY ID_Section ASC", ID_Customer);
			DataSet dataSet = CommonExcuteSql.Instance.ExcuteSql(sql);
			DataTable dataTable = dataSet.Tables[0];
			int count = dataTable.Rows.Count;
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable(count);
			string text = string.Empty;
			foreach (DataRow dataRow in dataTable.Rows)
			{
				text = text + dataRow["ID_Section"].ToString() + ",";
				if (!hashtable.ContainsKey(dataRow["ID_Section"].ToString()))
				{
					hashtable.Add(dataRow["ID_Section"].ToString(), dataRow["ID_Section"].ToString());
					DataRow[] array = dataTable.Select("ID_Section='" + dataRow["ID_Section"].ToString() + "'");
					DataRow[] array2 = dataTable.Select("ID_Section='" + dataRow["ID_Section"].ToString() + "'AND Is_FeeRefund='True'");
					if (array2.Length == array.Length)
					{
						hashtable[dataRow["ID_Section"].ToString()] = 1;
					}
					else
					{
						hashtable[dataRow["ID_Section"].ToString()] = 0;
					}
					dataRow.Delete();
				}
			}
			string text2 = string.Empty;
			foreach (System.Collections.DictionaryEntry dictionaryEntry in hashtable)
			{
				text2 += string.Format("UPDATE OnCustExamSection SET IS_Refund='{0}' WHERE ID_Customer='{1}' AND ID_Section='{2}';", dictionaryEntry.Value, ID_Customer, dictionaryEntry.Key);
			}
			List<string> list = new List<string>(2);
			if (text2 != string.Empty)
			{
				list.Add(text2);
			}
			if (!string.IsNullOrEmpty(text.TrimEnd(new char[]
			{
				','
			})))
			{
				string item = string.Format("DELETE FROM OnCustExamSection WHERE ID_Section NOT IN({0}) AND ID_Customer='{1}';", text.TrimEnd(new char[]
				{
					','
				}), ID_Customer);
				list.Add(item);
			}
			else
			{
				string item = string.Format("DELETE FROM OnCustExamSection WHERE ID_Customer IN({0});", ID_Customer);
				list.Add(item);
			}
			if (dataTable != null)
			{
				dataTable.Dispose();
				dataTable = null;
			}
			if (dataSet != null)
			{
				dataSet.Dispose();
				dataSet = null;
			}
			if (hashtable != null)
			{
				hashtable = null;
			}
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		public int SumSectionExamInfo_New(string ID_Customer)
		{
			string item = string.Format("SELECT BusFee.ID_Section,OnCustFee.ID_Fee,ISNULL(OnCustFee.Is_FeeCharged,0) Is_FeeCharged,ISNULL(OnCustFee.Is_FeeRefund,0) Is_FeeRefund INTO #SectionCustFee_{1} FROM\r\n(SELECT ID_Fee,Is_FeeCharged,Is_FeeRefund FROM OnCustFee WHERE ID_Customer='{0}')OnCustFee\r\nINNER JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\n--获取所有全部已退的科室(已退科室的退费个数=所有科室的数目，那么它为全部退费，否则为部分退费)\r\nSELECT A.ID_Section,A.SectionCount INTO #FeeRefundSection_{1} FROM(SELECT ID_Section,COUNT(ID_Section) SectionCount FROM #SectionCustFee_{1} GROUP BY ID_Section)A\r\nINNER JOIN (SELECT ID_Section,COUNT(Is_FeeRefund) FeeRefundCount FROM #SectionCustFee_{1} WHERE Is_FeeRefund=1 GROUP BY ID_Section,Is_FeeRefund)B\r\nON A.ID_Section=B.ID_Section AND A.SectionCount=B.FeeRefundCount\r\n--修改全部完全退费科室标记为已退费\r\nUPDATE OnCustExamSection SET IS_Refund=1 WHERE ID_Customer='{0}' AND ID_Section IN(SELECT ID_Section FROM #FeeRefundSection_{1});\r\n--修改未完全退费科室标记为未退费\r\nUPDATE OnCustExamSection SET IS_Refund=0 WHERE ID_Customer='{0}' AND ID_Section IN(\r\nSELECT ID_Section FROM (SELECT ID_Section FROM #SectionCustFee_{1} GROUP BY ID_Section) A WHERE NOT\r\nEXISTS(SELECT ID_Section FROM #FeeRefundSection_{1} WHERE A.ID_Section=#FeeRefundSection_{1}.ID_Section)\r\n)\r\n--删除科室检查信息中不包含的科室信息\r\nDELETE FROM OnCustExamSection WHERE ID_Customer='{0}' AND ID_Section NOT IN(SELECT ID_Section FROM #SectionCustFee_{1});\r\nDROP TABLE #FeeRefundSection_{1};\r\nDROP TABLE #SectionCustFee_{1};", ID_Customer, Public.GetGuid("-", string.Empty));
			List<string> list = new List<string>(1);
			list.Add(item);
			return CommonExcuteSql.Instance.ExecuteSqlTran(list);
		}

		private DataTable GetNewDT(DataTable dt, string Gender, string CustFeeID)
		{
			DataTable dataTable = dt.Clone();
			DataRow[] array = null;
			string empty = string.Empty;
			CustFeeID = ((CustFeeID == string.Empty) ? "ID_Fee" : CustFeeID);
			string text = string.Empty;
			if (dt.Rows.Count > 0)
			{
				if (Gender != string.Empty && Gender != "-1")
				{
					text = text + "Forsex in('" + Gender + "','2')";
				}
				if (CustFeeID != string.Empty)
				{
					text = text + " AND ID_Fee NOT IN(" + CustFeeID + ")";
				}
				if (text != string.Empty)
				{
					array = dt.Select(text);
				}
			}
			DataRow[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DataRow row = array2[i];
				dataTable.ImportRow(row);
			}
			return dataTable;
		}

		public DataTable SearchBusFeeByCustFeeID(string Gender, string UserName, string Discount, string CustFeeID, string SelectedFee, string InputCode)
		{
			object cache = DataCache.GetCache("TeamTaskGroupFee");
			DataTable dataTable = null;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
				if (!dataTable.Columns.Contains("IsChecked"))
				{
					dataTable.Columns.Add("IsChecked");
				}
				if (SelectedFee != string.Empty)
				{
					DataRow[] array = dataTable.Select("ID_Fee not in(" + SelectedFee.TrimStart(new char[]
					{
						','
					}).TrimEnd(new char[]
					{
						','
					}) + ")");
					if (array.Length > 0)
					{
						DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							dataRow["IsChecked"] = 0;
						}
					}
				}
				else if (InputCode == string.Empty)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						dataRow["IsChecked"] = 0;
					}
				}
			}
			else
			{
				dataTable = this.GetBusFeeNotINCustFeeID(Gender, CustFeeID, UserName, Discount);
				if (!dataTable.Columns.Contains("IsChecked"))
				{
					dataTable.Columns.Add("IsChecked");
				}
			}
			this._ALlFeeID.Clear();
			DataTable dataTable2 = dataTable.Clone();
			DataRow[] array3 = null;
			string text = CustFeeID.TrimEnd(new char[]
			{
				','
			}) + "," + SelectedFee.TrimEnd(new char[]
			{
				','
			});
			text = text.TrimEnd(new char[]
			{
				','
			}).TrimStart(new char[]
			{
				','
			});
			if (InputCode.Trim() != string.Empty)
			{
				DataRow[] array4;
				if (SelectedFee.Trim() != string.Empty)
				{
					if (CustFeeID.Trim() == string.Empty)
					{
						array4 = dataTable.Select(string.Concat(new string[]
						{
							"InputCode='",
							InputCode,
							"' and ID_Fee not in(",
							SelectedFee,
							")"
						}));
					}
					else
					{
						array4 = dataTable.Select(string.Concat(new string[]
						{
							"InputCode='",
							InputCode,
							"' and ID_Fee not in(",
							text,
							")"
						}));
					}
				}
				else if (CustFeeID.Trim() == string.Empty)
				{
					array4 = dataTable.Select("InputCode='" + InputCode + "'");
				}
				else
				{
					array4 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode='",
						InputCode,
						"' and ID_Fee not in(",
						CustFeeID,
						")"
					}));
				}
				DataRow[] array5;
				if (SelectedFee.Trim() != string.Empty)
				{
					if (CustFeeID.Trim() == string.Empty)
					{
						array5 = dataTable.Select(string.Concat(new string[]
						{
							"InputCode<>'",
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
						array5 = dataTable.Select(string.Concat(new string[]
						{
							"InputCode<>'",
							InputCode,
							"' and InputCode like '",
							InputCode,
							"%' and ID_Fee not in(",
							text,
							")"
						}));
					}
				}
				else if (CustFeeID.Trim() == string.Empty)
				{
					array5 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' and InputCode like '",
						InputCode,
						"%'"
					}));
				}
				else
				{
					array5 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' and InputCode like '",
						InputCode,
						"%' and ID_Fee not in(",
						CustFeeID,
						")"
					}));
				}
				DataRow[] array6;
				if (CustFeeID.Trim() == string.Empty)
				{
					array6 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' and InputCode not like '",
						InputCode,
						"%' and InputCode like '%",
						InputCode,
						"%'"
					}));
				}
				else
				{
					array6 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' and InputCode not like '",
						InputCode,
						"%' and InputCode like '%",
						InputCode,
						"%' and ID_Fee not in(",
						CustFeeID,
						")"
					}));
				}
				if (array4 != null)
				{
					if (array4.Length == 1)
					{
						DataRow[] array2 = array4;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataRow["IsChecked"] = 2;
								dataTable2.ImportRow(dataRow);
							}
						}
					}
					else
					{
						DataRow[] array2 = array4;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataTable2.ImportRow(dataRow);
							}
						}
					}
				}
				if (array5 != null)
				{
					if (array5.Length == 1)
					{
						DataRow[] array2 = array5;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (dataTable2.Rows.Count >= 5)
							{
								break;
							}
							if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataRow["IsChecked"] = 2;
								dataTable2.ImportRow(dataRow);
							}
						}
					}
					else
					{
						DataRow[] array2 = array5;
						for (int i = 0; i < array2.Length; i++)
						{
							DataRow dataRow = array2[i];
							if (dataTable2.Rows.Count >= 5)
							{
								break;
							}
							if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
							{
								this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
								dataTable2.ImportRow(dataRow);
							}
						}
					}
				}
				if (SelectedFee != string.Empty)
				{
					SelectedFee = SelectedFee.TrimStart(new char[]
					{
						','
					}).TrimEnd(new char[]
					{
						','
					});
					array3 = dataTable.Select("ID_Fee in(" + SelectedFee + ")");
				}
				if (array3 != null)
				{
					DataRow[] array2 = array3;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				if (array6 != null)
				{
					DataRow[] array2 = array6;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
			}
			else
			{
				if (SelectedFee != string.Empty)
				{
					SelectedFee = SelectedFee.TrimStart(new char[]
					{
						','
					}).TrimEnd(new char[]
					{
						','
					});
					array3 = dataTable.Select("ID_Fee in(" + SelectedFee + ")");
				}
				if (array3 != null)
				{
					DataRow[] array2 = array3;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				DataRow[] array7 = null;
				if (text != string.Empty)
				{
					text = text.TrimStart(new char[]
					{
						','
					});
					array7 = dataTable.Select("ID_Fee NOT IN(" + text + ")");
				}
				if (array7 == null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				else
				{
					DataRow[] array2 = array7;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
			}
			return dataTable2;
		}

		public DataTable SearchBusFeeByCustFeeIDX(string Gender, string UserName, string Discount, string CustFeeID, string SelectedFee, string InputCode)
		{
			object cache = DataCache.GetCache("TeamTaskGroupFee");
			DataTable dataTable;
			if (cache != null)
			{
				dataTable = (DataTable)cache;
			}
			else
			{
				dataTable = this.GetBusFeeNotINCustFeeID(Gender, CustFeeID, UserName, Discount);
			}
			this._ALlFeeID.Clear();
			if (!dataTable.Columns.Contains("IsChecked"))
			{
				dataTable.Columns.Add("IsChecked");
			}
			DataTable dataTable2 = dataTable.Clone();
			DataRow[] array = (SelectedFee.Trim() == string.Empty) ? null : dataTable.Select("ID_Fee in(" + SelectedFee + ")");
			if (InputCode.Trim() != string.Empty)
			{
				DataRow[] array2;
				if (SelectedFee.Trim() != string.Empty)
				{
					array2 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode='",
						InputCode,
						"' and ID_Fee not in(",
						SelectedFee,
						")"
					}));
				}
				else
				{
					array2 = dataTable.Select("InputCode='" + InputCode + "'");
				}
				DataRow[] array3;
				if (SelectedFee.Trim() != string.Empty)
				{
					array3 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
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
					array3 = dataTable.Select(string.Concat(new string[]
					{
						"InputCode<>'",
						InputCode,
						"' and InputCode like '",
						InputCode,
						"%'"
					}));
				}
				DataRow[] array4 = dataTable.Select(string.Concat(new string[]
				{
					"InputCode<>'",
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
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				if (array3 != null)
				{
					DataRow[] array5 = array3;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
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
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				if (array4 != null)
				{
					DataRow[] array5 = array4;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
			}
			else
			{
				string text = string.Empty;
				if (array != null)
				{
					DataRow[] array5 = array;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							text = text + dataRow["ID_Fee"].ToString() + ",";
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataRow["IsChecked"] = 1;
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				text = text.TrimEnd(new char[]
				{
					','
				});
				DataRow[] array6 = null;
				if (text.Trim() != string.Empty)
				{
					array6 = dataTable.Select("ID_Fee not in(" + text + ")");
				}
				if (array6 == null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
				else
				{
					DataRow[] array5 = array6;
					for (int i = 0; i < array5.Length; i++)
					{
						DataRow dataRow = array5[i];
						if (dataTable2.Rows.Count >= 5)
						{
							break;
						}
						if (!this._ALlFeeID.ContainsKey(dataRow["ID_Fee"].ToString()))
						{
							this._ALlFeeID.Add(dataRow["ID_Fee"].ToString(), dataRow["ID_Fee"].ToString());
							dataTable2.ImportRow(dataRow);
						}
					}
				}
			}
			return dataTable2;
		}

		public int UpdateCustomerSubscribDateOfTeam(string ID_Customer, object SubScribDate)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(ID_Customer))
			{
				string[] array = ID_Customer.TrimEnd(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				});
				if (array.Length > 0)
				{
					string text = string.Empty;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						text = text + text2.Replace("'", string.Empty) + ",";
					}
					string item = string.Format("UPDATE OnCustPhysicalExamInfo SET SubScribDate='{1}' WHERE ID_Customer IN({0});", text.TrimEnd(new char[]
					{
						','
					}), SubScribDate);
					List<string> list = new List<string>(1);
					list.Add(item);
					result = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				}
			}
			return result;
		}

		public int EncodeCustomerSecurityLevel(string ID_Customer)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(ID_Customer))
			{
				string[] array = ID_Customer.TrimEnd(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				});
				if (array.Length > 0)
				{
					string text = string.Empty;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						text = text + text2.Replace("'", string.Empty) + ",";
					}
					string item = string.Format("UPDATE OnCustPhysicalExamInfo SET SecurityLevel=SecurityLevel+100 WHERE ISNULL(SecurityLevel,0)<100 AND ID_Customer IN({0});", text.TrimEnd(new char[]
					{
						','
					}));
					List<string> list = new List<string>(1);
					list.Add(item);
					result = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				}
			}
			return result;
		}

		public int UpdateSecurityLevel(string ID_Customer, int SecurityLevel)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(ID_Customer))
			{
				string[] array = ID_Customer.TrimEnd(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				});
				if (array.Length > 0)
				{
					string text = string.Empty;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						text = text + text2.Replace("'", string.Empty) + ",";
					}
					string item = string.Format("UPDATE OnCustPhysicalExamInfo SET SecurityLevel='{1}' WHERE ID_Customer IN({0});", text.TrimEnd(new char[]
					{
						','
					}), SecurityLevel);
					List<string> list = new List<string>(1);
					list.Add(item);
					result = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				}
			}
			return result;
		}

		public int DecodeCustomerSecurityLevel(string ID_Customer)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(ID_Customer))
			{
				string[] array = ID_Customer.TrimEnd(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				});
				if (array.Length > 0)
				{
					string text = string.Empty;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						text = text + text2.Replace("'", string.Empty) + ",";
					}
					string item = string.Format("UPDATE OnCustPhysicalExamInfo SET SecurityLevel=SecurityLevel-100 WHERE ISNULL(SecurityLevel,0)>100 AND ID_Customer IN({0});", text.TrimEnd(new char[]
					{
						','
					}));
					List<string> list = new List<string>(1);
					list.Add(item);
					result = CommonExcuteSql.Instance.ExecuteSqlTran(list);
				}
			}
			return result;
		}
	}
}
