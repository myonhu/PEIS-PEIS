using PEIS.Base;
using PEIS.Common;
using PEIS.BLL;
using PEIS.SQLServerDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;

namespace PEIS.Web.Ajax
{
	public class AjaxStatistics : BasePage
	{
		private int CommandTimeout = 30;

		private DateTime BeginDate;

		private int Discount = 10;

		private string toOnLineConnectionString = PubConstant.GetConnectionString("ConnectionString");

		private string toOffLineConnectionString = PubConstant.GetConnectionString("ToOffLineConnectionString");

		private string toWasteConnectionString = PubConstant.GetConnectionString("FYH_Waste");

		public string ErrorMessage = string.Empty;

		private string FilePath = HttpContext.Current.Request.PhysicalApplicationPath;

		private string _backLogPath = HttpContext.Current.Server.MapPath("~/config/base/BackLogSetting.xml");

		protected void Page_Load(object sender, EventArgs e)
		{
			int.TryParse(ConfigurationManager.AppSettings["CommandTimeout"], out this.CommandTimeout);
			this.ErrorMessage = "error";
			string @string = base.GetString("action");
			MethodInfo method = base.GetType().GetMethod(@string);
			try
			{
				method.Invoke(this, null);
			}
			catch
			{
				this.OutPutMessage(this.ErrorMessage);
			}
		}

		public void OutPutMessage(string msg)
		{
			base.Response.Write(msg);
		}

		public void TestOutMessage()
		{
			this.OutPutMessage("This is the Test Info ... ");
		}

		public void GetDoctorWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("flag", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_Doctor", -1);
					string text = base.Server.HtmlDecode(base.GetString("Doctor"));
					string text2 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
					string text3 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
					string str = text2.Replace("-", string.Empty);
					string str2 = text3.Replace("-", string.Empty);
					string str3 = string.Empty;
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " 00:00:00";
					}
					if (!string.IsNullOrEmpty(text3))
					{
						text3 += " 23:59:59";
					}
					string text4 = string.Empty;
					if (@int == 1)
					{
						str3 = "分检";
						text4 = string.Format("--分检\r\nSELECT UserName,SectionName,SUM(Num)SumNotTeamNum,SUM(TeamNum)SumTeamNum,SUM(Num)+SUM(TeamNum) Num FROM (\r\nSELECT [SummaryDoctorName] UserName,[SectionName] SectionName,ISNULL(CASE WHEN Is_Team<>1 THEN COUNT(0) END,0) Num,ISNULL(CASE WHEN Is_Team=1 THEN COUNT(0) END,0) TeamNum\r\nFROM(SELECT ID_Customer,[SummaryDoctorName],[SectionName] FROM OnCustExamSection WHERE Is_Check=1 AND (CheckDate BETWEEN '{0}' AND '{1}') @WHERE)OCES\r\nINNER JOIN OnCustPhysicalExamInfo OCPE ON OCES.ID_Customer=OCPE.ID_Customer \r\nGROUP BY [SummaryDoctorName],[SectionName],Is_Team)A GROUP BY UserName,SectionName ORDER BY UserName", text2, text3);
						if (int2 > -1)
						{
							text4 = text4.Replace("@WHERE", string.Format(" AND ID_SummaryDoctor='{0}'", int2, text));
						}
						else
						{
							text4 = text4.Replace("@WHERE", string.Empty);
						}
					}
					else if (@int == 2)
					{
						str3 = "总检";
						text4 = string.Format("SELECT UserName,SectionName,SUM(Num)SumNotTeamNum,SUM(TeamNum)SumTeamNum,SUM(Num)+SUM(TeamNum) Num FROM (\r\n(SELECT FinalDoctor UserName,'' SectionName,ISNULL(CASE WHEN Is_Team<>1 THEN COUNT(0) END,0) Num,ISNULL(CASE WHEN Is_Team=1 THEN COUNT(0) END,0) TeamNum\r\nFROM OnCustPhysicalExamInfo WHERE Is_Checked=1 AND (FinalDate BETWEEN '{0}' AND '{1}') @WHERE GROUP BY FinalDoctor,Is_Team))A \r\nGROUP BY UserName,SectionName ORDER BY UserName", text2, text3);
						if (int2 > -1)
						{
							text4 = text4.Replace("@WHERE", string.Format(" AND ID_FinalDoctor='{0}'", int2));
						}
						else
						{
							text4 = text4.Replace("@WHERE", string.Empty);
						}
					}
					else if (@int == 3)
					{
						str3 = "总审";
						text4 = string.Format("--总审\r\nSELECT UserName,SectionName,SUM(Num)SumNotTeamNum,SUM(TeamNum)SumTeamNum,SUM(Num)+SUM(TeamNum) Num FROM (\r\n(SELECT Checker UserName,'' SectionName,ISNULL(CASE WHEN Is_Team<>1 THEN COUNT(0) END,0) Num,ISNULL(CASE WHEN Is_Team=1 THEN COUNT(0) END,0) TeamNum\r\nFROM OnCustPhysicalExamInfo WHERE Is_Checked=1 AND (CheckedDate BETWEEN '{0}' AND '{1}') @WHERE GROUP BY Checker,Is_Team))A \r\nGROUP BY UserName,SectionName ORDER BY UserName", text2, text3);
						if (int2 > -1)
						{
							text4 = text4.Replace("@WHERE", string.Format(" AND ID_Checker='{0}'", int2));
						}
						else
						{
							text4 = text4.Replace("@WHERE", string.Empty);
						}
					}
					else if (@int == 4)
					{
						str3 = "录入";
						text4 = string.Format("--录入\r\nSELECT UserName,SectionName,SUM(Num)SumNotTeamNum,SUM(TeamNum)SumTeamNum,SUM(Num)+SUM(TeamNum) Num FROM (\r\nSELECT [CheckerName] UserName,[SectionName] SectionName,ISNULL(CASE WHEN Is_Team<>1 THEN COUNT(0) END,0) Num,ISNULL(CASE WHEN Is_Team=1 THEN COUNT(0) END,0) TeamNum\r\nFROM(SELECT ID_Customer,[CheckerName],[SectionName] FROM OnCustExamSection WHERE Is_Check=1 AND (CheckDate BETWEEN '{0}' AND '{1}') @WHERE)OCES\r\nINNER JOIN OnCustPhysicalExamInfo OCPE ON OCES.ID_Customer=OCPE.ID_Customer \r\nGROUP BY [CheckerName],[SectionName],Is_Team)A GROUP BY UserName,SectionName ORDER BY UserName", text2, text3);
						if (int2 > -1)
						{
							text4 = text4.Replace("@WHERE", string.Format(" AND ID_Checker='{0}'", int2, text));
						}
						else
						{
							text4 = text4.Replace("@WHERE", string.Empty);
						}
					}
					if (!string.IsNullOrEmpty(text4))
					{
						DataSet dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text4, new int?(this.CommandTimeout));
						DataTable dataTable = dataSet.Tables[0];
						string text5 = str3 + "统计";
						dataTable.TableName = str + "-" + str2 + text5;
						if (int2 > -1)
						{
							text5 = text + text5;
						}
						string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text6))
						{
							Directory.CreateDirectory(text6);
						}
						string str4 = text5 + ".xls";
						string text7 = text6 + "\\" + text5 + ".xls";
						if (File.Exists(text7))
						{
							File.Delete(text7);
						}
						Excel.DataSetToExcel(dataSet, text7);
						DataTable dataTable2 = new DataTable();
						dataTable2.Columns.Add("FilePath");
						DataRow dataRow = dataTable2.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str4;
						dataTable2.Rows.Add(dataRow);
						dataSet.Tables.Add(dataTable2);
						string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet);
						this.OutPutMessage(msg);
						dataTable.Dispose();
						dataSet.Dispose();
					}
				}
			}
		}

		public void GetTeamWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				string text = base.Server.HtmlDecode(base.GetString("TeamName"));
				int int2 = base.GetInt("ID_TeamTask", -1);
				string text2 = base.Server.HtmlDecode(base.GetString("TeamTaskName"));
				string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string text5 = text3.Replace("-", string.Empty);
				string text6 = text4.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text3))
				{
					text3 += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text4))
				{
					text4 += " 23:59:59";
				}
				string text7 = string.Format("SELECT ISNULL(TaskNumCount,0) TaskNumCount FROM TeamTask WHERE ID_TeamTask='{0}'\r\n                                            ;SELECT ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}';", int2);
				DataSet dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text7, new int?(this.CommandTimeout));
				ArrayList arrayList = null;
				DataTable dataTable = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				string text8 = string.Empty;
				string text9 = string.Empty;
				DataTable dataTable4 = dataSet.Tables[0];
				DataTable dataTable5 = dataSet.Tables[1];
				foreach (DataRow dataRow in dataTable5.Rows)
				{
					object obj2 = text8;
					text8 = string.Concat(new object[]
					{
						obj2,
						"'",
						dataRow["ID_Customer"],
						"',"
					});
				}
				if (!string.IsNullOrEmpty(text8))
				{
					int num12 = 0;
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num12);
					text8 = text8.TrimEnd(new char[]
					{
						','
					});
					text7 = string.Format("SELECT OCE.*,OPM.Is_ReportPrinted,OPM.Is_Informed,OPM.Is_ReportReceipted FROM(SELECT ID_Customer,ID_Operator,Is_ExamStarted Is_ExamStarted,Is_FinalFinished,Is_Checked FROM OnCustPhysicalExamInfo WHERE ID_Customer IN(SELECT ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}')) OCE\r\n                LEFT JOIN (SELECT * FROM OnCustReportManage WHERE ID_Customer IN(SELECT ID_Customer FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}')) OPM ON OCE.ID_Customer=OPM.ID_Customer;", int2);
					dataTable = PEIS.BLL.CommonStatistics.Instance.Query(text7, new int?(this.CommandTimeout)).Tables[0];
					if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
					{
						try
						{
							dataTable2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text7, new int?(this.CommandTimeout)).Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取团体完成量统计失败(GetTeamWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (num12 > 0)
					{
						arrayList = new ArrayList(num12);
						int i = 1;
						while (i <= num12)
						{
							text9 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text9 != null)
							{
								if (!string.IsNullOrEmpty(text9))
								{
									try
									{
										DataTable dataTable6 = PEIS.BLL.CommonStatistics.Instance.Query(text9, text7, new int?(this.CommandTimeout)).Tables[0];
										arrayList.Add(dataTable6);
										dataTable6.Dispose();
									}
									catch (Exception ex)
									{
										Log4J.Instance.Error(string.Concat(new string[]
										{
											Public.GetClientIP(),
											",",
											this.LoginUserModel.UserName,
											",查询团体完成量失败，失败原因为从",
											text9,
											"获取数据出现如下错误：",
											ex.Message
										}));
									}
								}
							}
							IL_44C:
							i++;
							continue;
							goto IL_44C;
						}
					}
					if (!string.IsNullOrEmpty(this.toWasteConnectionString))
					{
						try
						{
							dataTable3 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text7.Replace("@ExamState", "-1"), new int?(this.CommandTimeout)).Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",查询团体完成量失败，失败原因为从",
								this.toWasteConnectionString,
								"获取数据出现如下错误：",
								ex.Message
							}));
						}
					}
					if (dataTable2 == null)
					{
						dataTable2 = dataTable.Clone();
					}
					if (dataTable3 == null)
					{
						dataTable3 = dataTable.Clone();
					}
					DataRow[] array = dataTable.Select("ISNULL(Is_ExamStarted,0)=0");
					if (array != null)
					{
						num5 += array.Length;
					}
					DataRow[] array2 = dataTable2.Select("Is_ExamStarted=0");
					if (array2 != null)
					{
						num5 += array2.Length;
					}
					DataRow[] array3 = dataTable3.Select("Is_ExamStarted=0");
					if (array3 != null)
					{
						num5 += array3.Length;
					}
					num2 += dataTable.Rows.Count + dataTable2.Rows.Count + dataTable3.Rows.Count;
					array = dataTable.Select("ISNULL(ID_Operator,0)=0");
					if (array != null)
					{
						num4 += array.Length;
					}
					array2 = dataTable2.Select("ISNULL(ID_Operator,0)=0");
					if (array != null)
					{
						num4 += array2.Length;
					}
					array3 = dataTable3.Select("ISNULL(ID_Operator,0)=0");
					if (array3 != null)
					{
						num4 += array3.Length;
					}
					array = dataTable.Select("ID_Operator IS NOT NULL");
					if (array != null)
					{
						num3 += array.Length;
					}
					array2 = dataTable2.Select("ID_Operator IS NOT NULL");
					if (array != null)
					{
						num3 += array2.Length;
					}
					array3 = dataTable3.Select("ID_Operator IS NOT NULL");
					if (array3 != null)
					{
						num3 += array3.Length;
					}
					array = dataTable.Select("Is_ExamStarted=1");
					if (array != null)
					{
						num6 += array.Length;
					}
					array2 = dataTable2.Select("Is_ExamStarted=1");
					if (array != null)
					{
						num6 += array2.Length;
					}
					array3 = dataTable3.Select("Is_ExamStarted=1");
					if (array3 != null)
					{
						num6 += array3.Length;
					}
					array = dataTable.Select("Is_FinalFinished=1");
					if (array != null)
					{
						num7 += array.Length;
					}
					array2 = dataTable2.Select("Is_FinalFinished=1");
					if (array != null)
					{
						num7 += array2.Length;
					}
					array3 = dataTable3.Select("Is_FinalFinished=1");
					if (array3 != null)
					{
						num7 += array3.Length;
					}
					array = dataTable.Select("Is_Checked=1");
					if (array != null)
					{
						num8 += array.Length;
					}
					array2 = dataTable2.Select("Is_Checked=1");
					if (array != null)
					{
						num8 += array2.Length;
					}
					array3 = dataTable3.Select("Is_Checked=1");
					if (array3 != null)
					{
						num8 += array3.Length;
					}
					array = dataTable.Select("Is_ReportPrinted=1");
					if (array != null)
					{
						num9 += array.Length;
					}
					array2 = dataTable2.Select("Is_ReportPrinted=1");
					if (array != null)
					{
						num9 += array2.Length;
					}
					array3 = dataTable3.Select("Is_ReportPrinted=1");
					if (array3 != null)
					{
						num9 += array3.Length;
					}
					array = dataTable.Select("Is_Informed=1");
					if (array != null)
					{
						num10 += array.Length;
					}
					array2 = dataTable2.Select("Is_Informed=1");
					if (array != null)
					{
						num10 += array2.Length;
					}
					array3 = dataTable3.Select("Is_Informed=1");
					if (array3 != null)
					{
						num10 += array3.Length;
					}
					array = dataTable.Select("Is_ReportReceipted=1");
					if (array != null)
					{
						num11 += array.Length;
					}
					array2 = dataTable2.Select("Is_ReportReceipted=1");
					if (array != null)
					{
						num11 += array2.Length;
					}
					array3 = dataTable3.Select("Is_ReportReceipted=1");
					if (array3 != null)
					{
						num11 += array3.Length;
					}
					if (arrayList != null)
					{
						foreach (DataTable dataTable7 in arrayList)
						{
							if (dataTable7 != null)
							{
								num2 += dataTable7.Rows.Count;
								array = dataTable7.Select("ISNULL(Is_ExamStarted,0)=0");
								if (array != null)
								{
									num5 += array.Length;
								}
								array = dataTable7.Select("ISNULL(ID_Operator,0)=0");
								if (array != null)
								{
									num4 += array.Length;
								}
								array = dataTable7.Select("ID_Operator IS NOT NULL");
								if (array != null)
								{
									num3 += array.Length;
								}
								array = dataTable7.Select("Is_ExamStarted=1");
								if (array != null)
								{
									num6 += array.Length;
								}
								array = dataTable7.Select("Is_FinalFinished=1");
								if (array != null)
								{
									num7 += array.Length;
								}
								array = dataTable7.Select("Is_Checked=1");
								if (array != null)
								{
									num8 += array.Length;
								}
								array = dataTable7.Select("Is_ReportPrinted=1");
								if (array != null)
								{
									num9 += array.Length;
								}
								array = dataTable7.Select("Is_Informed=1");
								if (array != null)
								{
									num10 += array.Length;
								}
								array = dataTable7.Select("Is_ReportReceipted=1");
								if (array != null)
								{
									num11 += array.Length;
								}
							}
						}
					}
				}
				string text10 = "团体总体分析统计";
				DataSet dataSet2 = new DataSet();
				DataTable dataTable8 = new DataTable();
				dataTable8.TableName = text5 + "-" + text6 + text10;
				if (@int > -1)
				{
					dataTable8.TableName = string.Concat(new string[]
					{
						text5,
						"-",
						text6,
						text,
						"完成量统计"
					});
				}
				dataTable8.Columns.Add("TeamWorkLoadType");
				dataTable8.Columns.Add("TeamWorkLoadNum");
				DataRow dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "预计体检量";
				dataRow2["TeamWorkLoadNum"] = num;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "实际备单量";
				dataRow2["TeamWorkLoadNum"] = num2;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已完成登记";
				dataRow2["TeamWorkLoadNum"] = num3;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "未完成登记";
				dataRow2["TeamWorkLoadNum"] = num4;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "未进行分检";
				dataRow2["TeamWorkLoadNum"] = num5;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已进行分检";
				dataRow2["TeamWorkLoadNum"] = num6;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已完成总检";
				dataRow2["TeamWorkLoadNum"] = num7;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已完成总审";
				dataRow2["TeamWorkLoadNum"] = num8;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已打印报告";
				dataRow2["TeamWorkLoadNum"] = num9;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已通知领取";
				dataRow2["TeamWorkLoadNum"] = num10;
				dataTable8.Rows.Add(dataRow2);
				dataRow2 = dataTable8.NewRow();
				dataRow2["TeamWorkLoadType"] = "已完成领取";
				dataRow2["TeamWorkLoadNum"] = num11;
				dataTable8.Rows.Add(dataRow2);
				dataSet2.Tables.Add(dataTable8);
				string text11 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text11))
				{
					Directory.CreateDirectory(text11);
				}
				string str = text10 + ".xls";
				string text12 = text11 + "\\" + text10 + ".xls";
				if (File.Exists(text12))
				{
					File.Delete(text12);
				}
				Excel.DataSetToExcel(dataSet2, text12);
				DataTable dataTable9 = new DataTable();
				dataTable9.Columns.Add("FilePath");
				dataRow2 = dataTable9.NewRow();
				dataRow2["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str;
				dataTable9.Rows.Add(dataRow2);
				dataSet2.Tables.Add(dataTable9);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet2);
				this.OutPutMessage(msg);
				dataTable4.Dispose();
				dataTable5.Dispose();
				dataTable9.Dispose();
				dataSet2.Dispose();
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataTable5 != null)
				{
					dataTable5.Dispose();
					dataTable5 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable != null)
				{
					dataTable.Dispose();
					dataTable = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
			}
		}

		public void GetCustFeeWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				int int2 = base.GetInt("ID_Fee", -1);
				string text = base.Server.HtmlDecode(base.GetString("TeamName"));
				string text2 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text3.Replace("-", string.Empty);
				string str2 = text4.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text3))
				{
					text3 += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text4))
				{
					text4 += " 23:59:59";
				}
				ArrayList arrayList = null;
				ArrayList arrayList2 = null;
				DataSet dataSet = null;
				DataTable dataTable = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataTable dataTable4 = null;
				DataTable dataTable5 = null;
				DataTable dataTable6 = null;
				string text5 = string.Empty;
				string text6 = string.Empty;
				int num = 1;
				int num2 = 1;
				int num3 = 0;
				if (@int == -1)
				{
					if (int2 == -1)
					{
						text5 = string.Format("--获取指定日期内的体检号\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfoOfTeam@RandNum FROM OnCustPhysicalExamInfo WHERE ID_Customer \r\nIN(SELECT ID_Customer FROM OnCustExamSection WHERE Is_Check=1 AND SectionSummaryDate BETWEEN '{0}' AND '{1}');\r\n--获取指定日期内的体检号对应的所有收费项目\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,ExamDoctorName,FactPrice,Is_FeeRefund,Is_FeeCharged INTO #TEMPOnCustFee@RandNumOfTeam@RandNum FROM OnCustFee WHERE EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfoOfTeam@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfoOfTeam@RandNum.ID_Customer);\r\nSELECT ID_Fee,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFee@RandNumOfTeam@RandNum OnCustFee GROUP BY ID_Fee;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.FeeItemName,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice,ExamDoctorName,CASE WHEN ExamDoctorName IS NOT NULL THEN '已检' ELSE '未检' END ExamStated,CASE WHEN Is_FeeRefund=1 THEN '已退' WHEN Is_FeeCharged=1 THEN '已收' ELSE '未收' END FeeChargeStaute FROM #TEMPOnCustFee@RandNumOfTeam@RandNum TOC\r\nINNER JOIN #TEMPOnCustPhysicalExamInfoOfTeam@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfoOfTeam@RandNum;\r\nDROP TABLE #TEMPOnCustFee@RandNumOfTeam@RandNum;", text3, text4);
					}
					else
					{
						text5 = string.Format("--获取收费项目汇总\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfo@RandNum FROM OnCustPhysicalExamInfo WHERE ID_Customer \r\nIN(SELECT ID_Customer FROM OnCustExamSection WHERE Is_Check=1 AND SectionSummaryDate BETWEEN '{0}' AND '{1}' AND ID_Section IN (SELECT ID_Section FROM BusFee WHERE ID_Fee='{2}'));\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,ExamDoctorName,FactPrice,Is_FeeRefund,Is_FeeCharged INTO #TEMPOnCustFee@RandNum FROM OnCustFee WHERE ID_Fee='{2}' AND EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfo@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfo@RandNum.ID_Customer);\r\nSELECT ID_Fee,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFee@RandNum OnCustFee GROUP BY ID_Fee;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.FeeItemName,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice,ExamDoctorName,CASE WHEN ExamDoctorName IS NOT NULL THEN '已检' ELSE '未检' END ExamStated,CASE WHEN Is_FeeRefund=1 THEN '已退' WHEN Is_FeeCharged=1 THEN '已收' ELSE '未收' END FeeChargeStaute FROM #TEMPOnCustFee@RandNum TOC \r\nINNER JOIN #TEMPOnCustPhysicalExamInfo@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfo@RandNum;\r\nDROP TABLE #TEMPOnCustFee@RandNum;", text3, text4, int2);
					}
				}
				else if (int2 == -1)
				{
					text5 = string.Format("--获取指定日期内的体检号\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfoOfTeam@RandNum FROM OnCustPhysicalExamInfo WHERE ID_Team='{0}' AND ID_Customer \r\nIN(SELECT ID_Customer FROM OnCustExamSection WHERE Is_Check=1 AND SectionSummaryDate BETWEEN '{1}' AND '{2}');\r\n--获取指定日期内的体检号对应的所有收费项目\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,ExamDoctorName,FactPrice,Is_FeeRefund,Is_FeeCharged INTO #TEMPOnCustFee@RandNumOfTeam@RandNum FROM OnCustFee WHERE EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfoOfTeam@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfoOfTeam@RandNum.ID_Customer);\r\nSELECT ID_Fee,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFee@RandNumOfTeam@RandNum OnCustFee GROUP BY ID_Fee;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.FeeItemName,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice,ExamDoctorName,CASE WHEN ExamDoctorName IS NOT NULL THEN '已检' ELSE '未检' END ExamStated,CASE WHEN Is_FeeRefund=1 THEN '已退' WHEN Is_FeeCharged=1 THEN '已收' ELSE '未收' END FeeChargeStaute FROM #TEMPOnCustFee@RandNumOfTeam@RandNum TOC\r\nINNER JOIN #TEMPOnCustPhysicalExamInfoOfTeam@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfoOfTeam@RandNum;\r\nDROP TABLE #TEMPOnCustFee@RandNumOfTeam@RandNum;", @int, text3, text4);
				}
				else
				{
					text5 = string.Format("--获取收费项目汇总\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfo@RandNum FROM OnCustPhysicalExamInfo WHERE ID_Team='{0}' AND ID_Customer \r\nIN(SELECT ID_Customer FROM OnCustExamSection WHERE Is_Check=1 AND SectionSummaryDate BETWEEN '{1}' AND '{2}' AND ID_Section IN (SELECT ID_Section FROM BusFee WHERE ID_Fee='{3}'));\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,ExamDoctorName,FactPrice,Is_FeeRefund,Is_FeeCharged INTO #TEMPOnCustFee@RandNum FROM OnCustFee WHERE ID_Fee='{3}' AND EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfo@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfo@RandNum.ID_Customer);\r\nSELECT ID_Fee,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFee@RandNum OnCustFee GROUP BY ID_Fee;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.FeeItemName,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice,ExamDoctorName,CASE WHEN ExamDoctorName IS NOT NULL THEN '已检' ELSE '未检' END ExamStated,CASE WHEN Is_FeeRefund=1 THEN '已退' WHEN Is_FeeCharged=1 THEN '已收' ELSE '未收' END FeeChargeStaute FROM #TEMPOnCustFee@RandNum TOC \r\nINNER JOIN #TEMPOnCustPhysicalExamInfo@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfo@RandNum;\r\nDROP TABLE #TEMPOnCustFee@RandNum;", new object[]
					{
						@int,
						text3,
						text4,
						int2
					});
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
					int num4 = 0;
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num4);
					DataSet dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
					dataTable = dataSet2.Tables[0];
					dataTable2 = dataSet2.Tables[1];
					if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
					{
						try
						{
							DataSet dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
							dataTable3 = dataSet3.Tables[0];
							dataTable4 = dataSet3.Tables[1];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取收费项目统计信息失败(GetCustFeeWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (!string.IsNullOrEmpty(this.toWasteConnectionString))
					{
						try
						{
							DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
							dataTable5 = dataSet4.Tables[0];
							dataTable6 = dataSet4.Tables[1];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取收费项目统计信息失败(GetCustFeeWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (num4 > 0)
					{
						arrayList = new ArrayList(num4);
						arrayList2 = new ArrayList(num4);
						int i = 1;
						while (i <= num4)
						{
							text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text6 != null)
							{
								if (!string.IsNullOrEmpty(text6))
								{
									try
									{
										dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
										arrayList.Add(dataSet.Tables[0]);
										arrayList2.Add(dataSet.Tables[1]);
										dataSet.Dispose();
									}
									catch (Exception ex)
									{
										Log4J.Instance.Error(string.Concat(new string[]
										{
											Public.GetClientIP(),
											",",
											this.LoginUserModel.UserName,
											",查询收费项目汇总信息和明细信息失败，失败原因为从",
											text6,
											"获取数据出现如下错误：",
											ex.Message
										}));
									}
								}
							}
							IL_562:
							i++;
							continue;
							goto IL_562;
						}
					}
					if (dataTable3 != null)
					{
						dataTable.Merge(dataTable3);
					}
					if (dataTable5 != null)
					{
						dataTable.Merge(dataTable5);
					}
					IEnumerator enumerator;
					if (arrayList != null)
					{
						enumerator = arrayList.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DataTable dataTable7 = (DataTable)enumerator.Current;
								if (dataTable7 != null)
								{
									dataTable.Merge(dataTable7);
								}
							}
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					if (dataTable4 != null)
					{
						dataTable2.Merge(dataTable4);
					}
					if (dataTable6 != null)
					{
						dataTable2.Merge(dataTable6);
					}
					DataTable dataTable8 = dataTable.Clone();
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							DataRow[] array = dataTable8.Select("ID_Fee='" + dataRow["ID_Fee"] + "'");
							if (array.Length > 0)
							{
								DataRow[] array2 = array;
								for (int j = 0; j < array2.Length; j++)
								{
									DataRow dataRow2 = array2[j];
									dataRow2.BeginEdit();
									dataRow2["FeeNum"] = int.Parse(dataRow2["FeeNum"].ToString()) + int.Parse(dataRow["FeeNum"].ToString());
									dataRow2["SumFactPrice"] = (decimal.Parse(dataRow2["SumFactPrice"].ToString()) + decimal.Parse(dataRow["SumFactPrice"].ToString())).ToString("f2");
									dataRow2.EndEdit();
								}
							}
							else
							{
								dataRow.BeginEdit();
								dataRow["SumFactPrice"] = decimal.Parse(dataRow["SumFactPrice"].ToString()).ToString("f2");
								dataRow.EndEdit();
								dataTable8.ImportRow(dataRow);
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					dataTable = dataTable8;
					//Queue<string> queue = new Queue<string>(dataTable.Rows.Count);
					DataSet dataSet5 = new DataSet();
					DataTable dataTable9 = new DataTable();
					dataTable9.Columns.Add("OrderIndex");
					dataTable9.Columns.Add("ID_Fee");
					dataTable9.Columns.Add("FeeItemName");
					dataTable9.Columns.Add("FeeItemNum");
					dataTable9.Columns.Add("RegisterName");
					dataTable9.Columns.Add("ExamDoctorName");
					dataTable9.Columns.Add("SumFactPrice");
					dataTable9.Columns.Add("CustomerName");
					dataTable9.Columns.Add("ID_Customer");
					dataTable9.Columns.Add("ExamState");
					dataTable9.Columns.Add("IsParent");
					dataTable9.Columns.Add("TeamName");
					dataTable9.Columns.Add("ExamStated");
					dataTable9.Columns.Add("FeeChargeStaute");
					dataSet5.Tables.Add(dataTable9);
					string text7 = "收费项目统计";
					if (!string.IsNullOrEmpty(text))
					{
						text7 = text + text7;
					}
					dataTable9.TableName = str + "-" + str2 + text7;
					string value = string.Empty;
					DataTable dataTable10 = dataTable9.Clone();
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						DataRow dataRow3 = dataTable.Rows[i];
						DataRow dataRow4 = dataTable9.NewRow();
						dataRow4["SumFactPrice"] = dataRow3["SumFactPrice"];
						dataRow4["ID_Fee"] = dataRow3["ID_Fee"];
						dataRow4["FeeItemNum"] = dataRow3["FeeNum"];
						dataRow4["CustomerName"] = string.Empty;
						dataRow4["ID_Customer"] = string.Empty;
						dataRow4["ExamState"] = string.Empty;
						dataRow4["IsParent"] = 1;
						dataRow4["OrderIndex"] = num;
						dataTable9.Rows.Add(dataRow4);
						num++;
						DataRow[] array3 = dataTable2.Select("ID_Fee='" + dataRow3["ID_Fee"] + "'");
						if (array3.Length > 0)
						{
							DataRow[] array2 = array3;
							for (int j = 0; j < array2.Length; j++)
							{
								DataRow dataRow5 = array2[j];
								value = dataRow5["FeeItemName"].ToString();
								DataRow dataRow6 = dataTable9.NewRow();
								dataRow6["ID_Fee"] = dataRow5["ID_Fee"];
								dataRow6["FeeItemName"] = dataRow5["FeeItemName"];
								dataRow6["FeeItemNum"] = 0;
								dataRow6["ID_Customer"] = dataRow5["ID_Customer"];
								dataRow6["CustomerName"] = dataRow5["CustomerName"];
								dataRow6["RegisterName"] = dataRow5["RegisterName"];
								dataRow6["TeamName"] = dataRow5["TeamName"];
								dataRow6["ExamDoctorName"] = dataRow5["ExamDoctorName"];
								dataRow6["SumFactPrice"] = dataRow5["FactPrice"];
								dataRow6["ExamStated"] = dataRow5["ExamStated"];
								dataRow6["FeeChargeStaute"] = dataRow5["FeeChargeStaute"];
								int.TryParse(dataRow5["ExamState"].ToString(), out num3);
								if (num3 == 0)
								{
									dataRow6["ExamState"] = "在线库";
								}
								else if (num3 == 1)
								{
									dataRow6["ExamState"] = "离线库";
								}
								else if (num3 > 1)
								{
									dataRow6["ExamState"] = "归档库" + (num3 - 1);
								}
								dataRow6["OrderIndex"] = num2;
								dataRow6["FeeItemNum"] = num2;
								dataTable9.Rows.Add(dataRow6);
								num2++;
							}
							dataRow4["FeeItemName"] = value;
						}
						dataTable10.ImportRow(dataRow4);
						dataTable.Rows.Remove(dataRow3);
						i--;
					}
					string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text8))
					{
						Directory.CreateDirectory(text8);
					}
					string str3 = text7 + ".xls";
					string text9 = text8 + "\\" + text7 + ".xls";
					if (File.Exists(text9))
					{
						File.Delete(text9);
					}
					Excel.DataSetToExcel(dataSet5, text9);
					dataSet5.Tables.Clear();
					dataSet5.Tables.Add(dataTable10);
					DataTable dataTable11 = new DataTable();
					dataTable11.Columns.Add("FilePath");
					DataRow dataRow7 = dataTable11.NewRow();
					dataRow7["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
					dataTable11.Rows.Add(dataRow7);
					dataSet5.Tables.Add(dataTable11);
					string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
					this.OutPutMessage(msg);
					if (dataTable10 != null)
					{
						dataTable10.Dispose();
					}
					if (dataTable11 != null)
					{
						dataTable11.Dispose();
					}
					if (dataTable2 != null)
					{
						dataTable2.Dispose();
						dataTable2 = null;
					}
					if (dataTable4 != null)
					{
						dataTable4.Dispose();
						dataTable4 = null;
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
						dataTable3 = null;
					}
					if (dataTable9 != null)
					{
						dataTable9.Dispose();
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
					arrayList = null;
					arrayList2 = null;
				}
			}
		}

		public void GetCustFeeWorkLoad_Detail()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				int int2 = base.GetInt("ID_Fee", -1);
				string text = base.Server.HtmlDecode(base.GetString("TeamName"));
				string text2 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text3.Replace("-", string.Empty);
				string str2 = text4.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text3))
				{
					text3 += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text4))
				{
					text4 += " 23:59:59";
				}
				ArrayList arrayList = null;
				ArrayList arrayList2 = null;
				DataSet dataSet = null;
				DataTable dataTable = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataTable dataTable4 = null;
				string text5 = string.Empty;
				string text6 = string.Empty;
				int num = 1;
				int num2 = 1;
				int num3 = 0;
				if (@int == -1)
				{
					if (int2 == -1)
					{
						text5 = string.Format("--获取指定日期内的体检号\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfoOfTeam@RandNum FROM OnCustPhysicalExamInfo WHERE SubScribDate BETWEEN '{0}' AND '{1}';\r\n--获取指定日期内的体检号对应的所有收费项目\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,FactPrice INTO #TEMPOnCustFeeOfTeam@RandNum FROM OnCustFee WHERE EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfoOfTeam@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfoOfTeam@RandNum.ID_Customer);\r\nSELECT ID_Fee,RegisterName,FeeItemName,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFeeOfTeam@RandNum OnCustFee GROUP BY ID_Fee,RegisterName,FeeItemName;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice FROM #TEMPOnCustFeeOfTeam@RandNum TOC\r\nINNER JOIN #TEMPOnCustPhysicalExamInfoOfTeam@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfoOfTeam@RandNum;\r\nDROP TABLE #TEMPOnCustFeeOfTeam@RandNum;", text3, text4);
					}
					else
					{
						text5 = string.Format("--获取收费项目汇总\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfo@RandNum FROM OnCustPhysicalExamInfo WHERE SubScribDate BETWEEN '{0}' AND '{1}';\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,FactPrice INTO #TEMPOnCustFee@RandNum FROM OnCustFee WHERE ID_Fee='{2}' AND EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfo@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfo@RandNum.ID_Customer);\r\nSELECT ID_Fee,RegisterName,FeeItemName,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFee@RandNum OnCustFee GROUP BY ID_Fee,RegisterName,FeeItemName;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice FROM #TEMPOnCustFee@RandNum TOC \r\nINNER JOIN #TEMPOnCustPhysicalExamInfo@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfo@RandNum;\r\nDROP TABLE #TEMPOnCustFee@RandNum;", text3, text4, int2);
					}
				}
				else if (int2 == -1)
				{
					text5 = string.Format("--获取指定日期内的体检号\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfoOfTeam@RandNum FROM OnCustPhysicalExamInfo WHERE ID_Team='{0}' AND SubScribDate BETWEEN '{1}' AND '{2}';\r\n--获取指定日期内的体检号对应的所有收费项目\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,FactPrice INTO #TEMPOnCustFeeOfTeam@RandNum FROM OnCustFee WHERE EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfoOfTeam@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfoOfTeam@RandNum.ID_Customer);\r\nSELECT ID_Fee,RegisterName,FeeItemName,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFeeOfTeam@RandNum OnCustFee GROUP BY ID_Fee,RegisterName,FeeItemName;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice FROM #TEMPOnCustFeeOfTeam@RandNum TOC\r\nINNER JOIN #TEMPOnCustPhysicalExamInfoOfTeam@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfoOfTeam@RandNum;\r\nDROP TABLE #TEMPOnCustFeeOfTeam@RandNum;", @int, text3, text4);
				}
				else
				{
					text5 = string.Format("--获取收费项目汇总\r\nSELECT ID_Customer,CustomerName,TeamName INTO #TEMPOnCustPhysicalExamInfo@RandNum FROM OnCustPhysicalExamInfo WHERE ID_Team='{0}' AND SubScribDate BETWEEN '{1}' AND '{2}';\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ID_Register,RegisterName,FactPrice INTO #TEMPOnCustFee@RandNum FROM OnCustFee WHERE ID_Fee='{3}' AND EXISTS(SELECT ID_Customer FROM #TEMPOnCustPhysicalExamInfo@RandNum WHERE OnCustFee.ID_Customer=#TEMPOnCustPhysicalExamInfo@RandNum.ID_Customer);\r\nSELECT ID_Fee,RegisterName,FeeItemName,COUNT(ID_Fee) FeeNum,SUM(FactPrice) SumFactPrice FROM #TEMPOnCustFee@RandNum OnCustFee GROUP BY ID_Fee,RegisterName,FeeItemName;\r\n--获取收费项目明细\r\nSELECT TOC.ID_Fee,TOC.ID_Customer,OPE.CustomerName,OPE.TeamName,@ExamState ExamState,RegisterName,FactPrice FROM #TEMPOnCustFee@RandNum TOC \r\nINNER JOIN #TEMPOnCustPhysicalExamInfo@RandNum OPE ON TOC.ID_Customer=OPE.ID_Customer;\r\nDROP TABLE #TEMPOnCustPhysicalExamInfo@RandNum;\r\nDROP TABLE #TEMPOnCustFee@RandNum;", new object[]
					{
						@int,
						text3,
						text4,
						int2
					});
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
					int num4 = 0;
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num4);
					DataSet dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
					dataTable = dataSet2.Tables[0];
					dataTable2 = dataSet2.Tables[1];
					if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
					{
						try
						{
							DataSet dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
							dataTable3 = dataSet3.Tables[0];
							dataTable4 = dataSet3.Tables[1];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取收费项目明细统计信息失败(GetCustFeeWorkLoad_Detail)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (num4 > 0)
					{
						arrayList = new ArrayList(num4);
						arrayList2 = new ArrayList(num4);
						int i = 1;
						while (i <= num4)
						{
							text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text6 != null)
							{
								if (!string.IsNullOrEmpty(text6))
								{
									try
									{
										dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
										arrayList.Add(dataSet.Tables[0]);
										arrayList2.Add(dataSet.Tables[1]);
										dataSet.Dispose();
									}
									catch (Exception ex)
									{
										Log4J.Instance.Error(string.Concat(new string[]
										{
											Public.GetClientIP(),
											",",
											this.LoginUserModel.UserName,
											",查询收费项目汇总信息和明细信息失败，失败原因为从",
											text6,
											"获取数据出现如下错误：",
											ex.Message
										}));
									}
								}
							}
							IL_47B:
							i++;
							continue;
							goto IL_47B;
						}
					}
					if (dataTable3 != null)
					{
						dataTable.Merge(dataTable3);
					}
					IEnumerator enumerator;
					if (arrayList != null)
					{
						enumerator = arrayList.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DataTable dataTable5 = (DataTable)enumerator.Current;
								if (dataTable5 != null)
								{
									dataTable.Merge(dataTable5);
								}
							}
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					if (dataTable4 != null)
					{
						dataTable2.Merge(dataTable4);
					}
					DataTable dataTable6 = dataTable.Clone();
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							DataRow[] array = dataTable6.Select(string.Concat(new object[]
							{
								"ID_Fee='",
								dataRow["ID_Fee"],
								"' AND RegisterName='",
								dataRow["RegisterName"],
								"'"
							}), "FeeItemName ASC");
							if (array.Length > 0)
							{
								DataRow[] array2 = array;
								for (int j = 0; j < array2.Length; j++)
								{
									DataRow dataRow2 = array2[j];
									dataRow2.BeginEdit();
									dataRow2["FeeNum"] = int.Parse(dataRow2["FeeNum"].ToString()) + int.Parse(dataRow["FeeNum"].ToString());
									dataRow2["SumFactPrice"] = (decimal.Parse(dataRow2["SumFactPrice"].ToString()) + decimal.Parse(dataRow["SumFactPrice"].ToString())).ToString("f2");
									dataRow2.EndEdit();
								}
							}
							else
							{
								dataRow.BeginEdit();
								dataRow["SumFactPrice"] = decimal.Parse(dataRow["SumFactPrice"].ToString()).ToString("f2");
								dataRow.EndEdit();
								dataTable6.ImportRow(dataRow);
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					dataTable = dataTable6;
					//Queue<string> queue = new Queue<string>(dataTable.Rows.Count);
                    //Queue<string> strs = new Queue<string>(dataTable.Rows.Count);
                    DataSet dataSet4 = new DataSet();
					DataTable dataTable7 = new DataTable();
					dataTable7.Columns.Add("OrderIndex");
					dataTable7.Columns.Add("ID_Fee");
					dataTable7.Columns.Add("FeeItemName");
					dataTable7.Columns.Add("FeeItemNum");
					dataTable7.Columns.Add("RegisterName");
					dataTable7.Columns.Add("SumFactPrice");
					dataTable7.Columns.Add("CustomerName");
					dataTable7.Columns.Add("ID_Customer");
					dataTable7.Columns.Add("ExamState");
					dataTable7.Columns.Add("IsParent");
					dataTable7.Columns.Add("TeamName");
					dataSet4.Tables.Add(dataTable7);
					string text7 = "收费项目统计";
					if (!string.IsNullOrEmpty(text))
					{
						text7 = text + text7;
					}
					dataTable7.TableName = str + "-" + str2 + text7;
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						DataRow dataRow3 = dataTable.Rows[i];
						DataRow dataRow4 = dataTable7.NewRow();
						dataRow4["RegisterName"] = dataRow3["RegisterName"];
						dataRow4["SumFactPrice"] = dataRow3["SumFactPrice"];
						dataRow4["ID_Fee"] = dataRow3["ID_Fee"];
						dataRow4["FeeItemName"] = dataRow3["FeeItemName"];
						dataRow4["FeeItemNum"] = dataRow3["FeeNum"];
						dataRow4["CustomerName"] = string.Empty;
						dataRow4["ID_Customer"] = string.Empty;
						dataRow4["ExamState"] = string.Empty;
						dataRow4["IsParent"] = 1;
						dataRow4["OrderIndex"] = num;
						dataTable7.Rows.Add(dataRow4);
						num++;
						DataRow[] array3 = dataTable2.Select(string.Concat(new object[]
						{
							"ID_Fee='",
							dataRow3["ID_Fee"],
							"' AND RegisterName='",
							dataRow3["RegisterName"],
							"'"
						}));
						if (array3.Length > 0)
						{
							DataRow[] array2 = array3;
							for (int j = 0; j < array2.Length; j++)
							{
								DataRow dataRow5 = array2[j];
								dataRow4 = dataTable7.NewRow();
								dataRow4["ID_Fee"] = dataRow5["ID_Fee"];
								dataRow4["FeeItemName"] = string.Empty;
								dataRow4["FeeItemNum"] = 0;
								dataRow4["ID_Customer"] = dataRow5["ID_Customer"];
								dataRow4["CustomerName"] = dataRow5["CustomerName"];
								dataRow4["TeamName"] = dataRow5["TeamName"];
								dataRow4["RegisterName"] = dataRow5["RegisterName"];
								dataRow4["SumFactPrice"] = dataRow5["FactPrice"];
								int.TryParse(dataRow5["ExamState"].ToString(), out num3);
								if (num3 == 0)
								{
									dataRow4["ExamState"] = "在线库";
								}
								else if (num3 == 1)
								{
									dataRow4["ExamState"] = "离线库";
								}
								else if (num3 > 1)
								{
									dataRow4["ExamState"] = "归档库" + (num3 - 1);
								}
								dataRow4["OrderIndex"] = num2;
								dataRow4["FeeItemNum"] = num2;
								dataTable7.Rows.Add(dataRow4);
								num2++;
							}
						}
						dataTable.Rows.Remove(dataRow3);
						i--;
					}
					string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text8))
					{
						Directory.CreateDirectory(text8);
					}
					string str3 = text7 + ".xls";
					string text9 = text8 + "\\" + text7 + ".xls";
					if (File.Exists(text9))
					{
						File.Delete(text9);
					}
					Excel.DataSetToExcel(dataSet4, text9);
					DataTable dataTable8 = new DataTable();
					dataTable8.Columns.Add("FilePath");
					DataRow dataRow6 = dataTable8.NewRow();
					dataRow6["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
					dataTable8.Rows.Add(dataRow6);
					dataSet4.Tables.Add(dataTable8);
					string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet4);
					this.OutPutMessage(msg);
					if (dataTable8 != null)
					{
						dataTable8.Dispose();
					}
					if (dataTable2 != null)
					{
						dataTable2.Dispose();
						dataTable2 = null;
					}
					if (dataTable4 != null)
					{
						dataTable4.Dispose();
						dataTable4 = null;
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
						dataTable3 = null;
					}
					if (dataTable7 != null)
					{
						dataTable7.Dispose();
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
					arrayList = null;
					arrayList2 = null;
				}
			}
		}

		public void GetCustomerWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("IsSearchDate", -1);
				string text = base.Server.HtmlDecode(base.GetString("dataTypeName"));
				int int2 = base.GetInt("ID_Team", -1);
				string text2 = base.Server.HtmlDecode(base.GetString("IDCard"));
				string text3 = base.Server.HtmlDecode(base.GetString("CustomerName"));
				string text4 = base.Server.HtmlDecode(base.GetString("TeamName"));
				string text5 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text6 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text5.Replace("-", string.Empty);
				string str2 = text6.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text5))
				{
					text5 += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text6))
				{
					text6 += " 23:59:59";
				}
				string text7 = string.Empty;
				string text8 = string.Format("SELECT @ExamState ExamState,TOAC.ID_ArcCustomer,CustomerName,TOAC.ID_Customer,GenderName,MarriageName,IDCard,MobileNo,TOAC.TeamName,TOAC.Department DepartmentX,TOAC.DepartSubA,TOAC.DepartSubB,TOAC.DepartSubC,TOAC.SubScribDate\r\n,TOAC.ExamTypeName,ExamPlaceName,Is_FinalFinished,FinalDoctor,FinalDate,TOAC.Is_Checked,TOAC.Checker,TOAC.CheckedDate\r\n,Is_ReportPrinted,ID_ReportPrinter,ReportPrinter,ReportPrintedDate,Is_Informed,Informer,InformedDate,\r\nIs_InformReturned,Is_ReportReceipted,Is_SelfReceipted,ReportReceiptor,ReportReceiptedDate\r\nFROM(SELECT ORPE.ID_ArcCustomer,OPE.* FROM (SELECT CustomerName,GenderName,MarriageName,IDCard,MobileNo,ID_Customer,CONVERT(varchar(20),OperateDate,120) SubScribDate,TeamName,Department,DepartSubA,DepartSubB,DepartSubC,ExamTypeName,ExamPlaceName,Is_FinalFinished,FinalDoctor,FinalDate,Is_Checked,Checker,CheckedDate FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere)OPE\r\nINNER JOIN OnCustRelationCustPEInfo ORPE ON OPE.ID_Customer=ORPE.ID_Customer) TOAC\r\nLEFT JOIN OnCustReportManage OCRM ON TOAC.ID_Customer=OCRM.ID_Customer;", new object[0]);
				if (int2 > -1)
				{
					text7 += string.Format(" AND ID_Team='{0}'", int2);
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (text == "ID_Customer")
					{
						text7 += string.Format(" AND (" + text + " LIKE '%{0}%')", text2);
					}
					else if (!string.IsNullOrEmpty(text2))
					{
						text7 += string.Format(" AND (" + text + " LIKE '%{0}%')", text2);
					}
				}
				if (!string.IsNullOrEmpty(text3))
				{
					text7 += string.Format(" AND (CustomerName LIKE '%{0}%')", text3);
				}
				if (@int == 1)
				{
					text7 += string.Format(" AND (OperateDate>='{0}' AND OperateDate<='{1}')", text5, text6);
				}
				text8 = text8.Replace("@OnCustPhysicalExamInfoWhere", text7);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				ArrayList arrayList = null;
				DataSet dataSet2 = null;
				string text9 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text8.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text8.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet3.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取客户体检名单信息失败(GetCustomerWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text9 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text9 != null)
						{
							if (!string.IsNullOrEmpty(text9))
							{
								try
								{
									dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(text9, text8.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet2.Tables[0]);
									dataSet2.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询收费项目汇总信息和明细信息失败，失败原因为从",
										text9,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_489:
						i++;
						continue;
						goto IL_489;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable4 in arrayList)
					{
						if (dataTable4 != null)
						{
							dataTable2.Merge(dataTable4);
						}
					}
				}
				DataSet dataSet4 = dataSet;
				string text10 = "客户状态";
				if (!string.IsNullOrEmpty(text4))
				{
					text10 = text4 + text10;
				}
				dataTable2.TableName = str + "-" + str2 + text10;
				string text11 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text11))
				{
					Directory.CreateDirectory(text11);
				}
				string str3 = text10 + ".xls";
				string text12 = text11 + "\\" + text10 + ".xls";
				if (File.Exists(text12))
				{
					File.Delete(text12);
				}
				Excel.DataSetToExcel(dataSet4, text12);
				DataTable dataTable5 = new DataTable();
				dataTable5.Columns.Add("FilePath");
				DataRow dataRow = dataTable5.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable5.Rows.Add(dataRow);
				dataSet4.Tables.Add(dataTable5);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet4);
				this.OutPutMessage(msg);
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet4 != null)
				{
					dataSet4.Dispose();
				}
			}
		}

		public void GetPositiveSummaryWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string empty = string.Empty;
						string text5 = string.Format("\r\nSELECT ID_Customer INTO #TempTeamTaskGroupCust_YXJG@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}';\r\n--将团体任务下的所有体检者科室的阳性结果写入中间表\r\nSELECT ID_Customer,PositiveSummary INTO #TempPositiveSummaryDT_YXJG@RandNum FROM(\r\nSELECT ID_Customer,STUFF((SELECT '['+ SectionName +'] '+CONVERT(VARCHAR(8000),PositiveSummary)+';' FROM (SELECT A.* FROM(SELECT ID_Customer,ID_Section,SectionName,SectionSummary,PositiveSummary FROM OnCustExamSection WHERE CONVERT(nvarchar,PositiveSummary)!='' AND ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_YXJG@RandNum))A\r\nINNER JOIN SYSSection B ON A.ID_Section=B.ID_Section) T2 WHERE T2.ID_Customer=T1.ID_Customer FOR XML PATH('')),1,0,'') PositiveSummary\r\nFROM (SELECT A.* FROM(SELECT ID_Customer,ID_Section,SectionName,SectionSummary,PositiveSummary FROM OnCustExamSection WHERE CONVERT(nvarchar,PositiveSummary)!='' AND ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_YXJG@RandNum))A\r\nINNER JOIN SYSSection B ON A.ID_Section=B.ID_Section) T1 GROUP BY ID_Customer) TEMPSYSSection;\r\n--查询团体任务下的所有客户信息\r\nSELECT '@ExamState' ExamState,#TempTeamTaskGroupCust_YXJG@RandNum.ID_Customer,CustomerName,GenderName,MarriageName,FLOOR(DATEDIFF(DY,BirthDay,(CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END))/365.25)Age,TeamName,Department,DepartSubA,DepartSubB,DepartSubC,CONVERT(varchar(20),OnCustPhysicalExamInfo.OperateDate,120) SubScribDate,#TempPositiveSummaryDT_YXJG@RandNum.PositiveSummary,MainDiagnose,SecondaryDiagnose,IndicatorDiagnose,NormalDiagnose,OtherDiagnose,ResultCompare FROM #TempTeamTaskGroupCust_YXJG@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere)OnCustPhysicalExamInfo ON #TempTeamTaskGroupCust_YXJG@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nINNER JOIN(SELECT * FROM OnCustRelationCustPEInfo WHERE ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_YXJG@RandNum) \r\n)OnCustRelationCustPEInfo ON OnCustPhysicalExamInfo.ID_Customer=OnCustRelationCustPEInfo.ID_Customer\r\nLEFT JOIN #TempPositiveSummaryDT_YXJG@RandNum ON #TempTeamTaskGroupCust_YXJG@RandNum.ID_Customer=#TempPositiveSummaryDT_YXJG@RandNum.ID_Customer;\r\n--删除中间表\r\nDROP TABLE #TempPositiveSummaryDT_YXJG@RandNum;\r\nDROP TABLE #TempTeamTaskGroupCust_YXJG@RandNum;", int2);
						text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", empty);
						text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet2 = null;
						string text6 = string.Empty;
						int num = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet3.Tables[0];
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取阳性结果信息失败(GetPositiveSummaryWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet4.Tables[0];
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取阳性结果信息失败(GetPositiveSummaryWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num > 0)
						{
							arrayList = new ArrayList(num);
							int i = 1;
							while (i <= num)
							{
								text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text6 != null)
								{
									if (!string.IsNullOrEmpty(text6))
									{
										try
										{
											dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet2.Tables[0]);
											dataSet2.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text6,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_49F:
								i++;
								continue;
								goto IL_49F;
							}
						}
						if (dataTable3 != null)
						{
							dataTable2.Merge(dataTable3);
						}
						if (dataTable4 != null)
						{
							dataTable2.Merge(dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									dataTable2.Merge(dataTable5);
								}
							}
						}
						DataSet dataSet5 = dataSet;
						string text7 = "阳性结果统计";
						if (!string.IsNullOrEmpty(text2))
						{
							text7 = text2 + text7;
						}
						dataTable2.TableName = str + "-" + str2 + text7;
						string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text8))
						{
							Directory.CreateDirectory(text8);
						}
						string str3 = text7 + ".xls";
						string text9 = text8 + "\\" + text7 + ".xls";
						if (File.Exists(text9))
						{
							File.Delete(text9);
						}
						Excel.DataSetToExcel(dataSet5, text9);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text10 = text10.Replace("\\r\\n", "");
						this.OutPutMessage(text10);
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
					}
				}
			}
		}

		public void GetCompeleteWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string empty = string.Empty;
						string text5 = string.Format("\r\n--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_WCQK@RandNum\r\nSELECT Department,DepartSubA,DepartSubB,DepartSubC,ID_Customer INTO #TempTeamTaskGroupCust_WCQK@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}';\r\n\r\n--将团体任务下的所有体检者未完成的收费项目写入中间表 #TempOnCustFeeDT_WCQK@RandNum\r\nSELECT ID_Customer,NotCompeleteFeeItemName INTO #TempOnCustFeeDT_WCQK@RandNum FROM(\r\nSELECT ID_Customer,STUFF((SELECT FeeItemName+';' FROM \r\n(SELECT A.* FROM(SELECT ID_Customer,FeeItemName FROM OnCustFee WHERE ISNULL(Is_Examined,0)!=1 AND ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_WCQK@RandNum))A) T2 WHERE T2.ID_Customer=T1.ID_Customer FOR XML PATH('')),1,0,'') NotCompeleteFeeItemName\r\nFROM (SELECT A.* FROM(SELECT ID_Customer,FeeItemName FROM OnCustFee WHERE ISNULL(Is_Examined,0)!=1 AND ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_WCQK@RandNum))A) T1 GROUP BY ID_Customer) TEMPSYSSection;\r\n\r\n--将团体任务下的所有体检者已完成的收费项目写入中间表 #TempompeleteOnCustFeeDT_WCQK@RandNum\r\nSELECT ID_Customer,CompeleteFeeItemName INTO #TempompeleteOnCustFeeDT_WCQK@RandNum FROM(\r\nSELECT ID_Customer,STUFF((SELECT FeeItemName+';' FROM \r\n(SELECT A.* FROM(SELECT ID_Customer,FeeItemName FROM OnCustFee WHERE Is_Examined=1 AND ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_WCQK@RandNum))A) T2 WHERE T2.ID_Customer=T1.ID_Customer FOR XML PATH('')),1,0,'') CompeleteFeeItemName\r\nFROM (SELECT A.* FROM(SELECT ID_Customer,FeeItemName FROM OnCustFee WHERE Is_Examined=1 AND ID_Customer IN(SELECT ID_Customer FROM #TempTeamTaskGroupCust_WCQK@RandNum))A) T1 GROUP BY ID_Customer) TEMPSYSSection;\r\n\r\n--查询团体任务下的所有客户信息\r\nSELECT '@ExamState' ExamState,ID_Customer,CustomerName,GenderName,MarriageName,Age,TeamName,SubScribDate,ExamStateName,NotCompeleteFeeItemName,CompeleteFeeItemName,Department,DepartSubA,DepartSubB,DepartSubC,Note FROM(SELECT CustomerName,GenderName,DATEDIFF(YEAR,BirthDay,GETDATE())Age,MarriageName,TeamName,#TempTeamTaskGroupCust_WCQK@RandNum.*,CONVERT(varchar(20),OnCustPhysicalExamInfo.OperateDate,120) SubScribDate\r\n,CASE WHEN (ISNULL(CompeleteFeeItemName,'')='' AND ISNULL(NotCompeleteFeeItemName,'')!='')  THEN '未检' \r\nWHEN (ISNULL(CompeleteFeeItemName,'')!='' AND ISNULL(NotCompeleteFeeItemName,'')!='') THEN '部分检查' \r\nWHEN (ISNULL(CompeleteFeeItemName,'')!='' AND ISNULL(NotCompeleteFeeItemName,'')='') THEN '全部已检' \r\nEND ExamStateName\r\n,ISNULL(#TempOnCustFeeDT_WCQK@RandNum.NotCompeleteFeeItemName,'') NotCompeleteFeeItemName,ISNULL(#TempompeleteOnCustFeeDT_WCQK@RandNum.CompeleteFeeItemName,'')CompeleteFeeItemName,Note FROM #TempTeamTaskGroupCust_WCQK@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere)OnCustPhysicalExamInfo ON #TempTeamTaskGroupCust_WCQK@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nLEFT JOIN #TempOnCustFeeDT_WCQK@RandNum ON #TempTeamTaskGroupCust_WCQK@RandNum.ID_Customer=#TempOnCustFeeDT_WCQK@RandNum.ID_Customer\r\nLEFT JOIN #TempompeleteOnCustFeeDT_WCQK@RandNum ON #TempTeamTaskGroupCust_WCQK@RandNum.ID_Customer=#TempompeleteOnCustFeeDT_WCQK@RandNum.ID_Customer)C;\r\n--删除中间表\r\nDROP TABLE #TempOnCustFeeDT_WCQK@RandNum;\r\nDROP TABLE #TempompeleteOnCustFeeDT_WCQK@RandNum;\r\nDROP TABLE #TempTeamTaskGroupCust_WCQK@RandNum;", int2);
						text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", empty);
						text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataSet dataSet2 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet3 = null;
						string text6 = string.Empty;
						int num = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet4.Tables[0];
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取完成情况信息失败(GetCompeleteWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet2.Tables[0];
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从废弃库中获取完成情况信息失败(GetCompeleteWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num > 0)
						{
							arrayList = new ArrayList(num);
							int i = 1;
							while (i <= num)
							{
								text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text6 != null)
								{
									if (!string.IsNullOrEmpty(text6))
									{
										try
										{
											dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet3.Tables[0]);
											dataSet3.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text6,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_49F:
								i++;
								continue;
								goto IL_49F;
							}
						}
						if (dataTable3 != null)
						{
							dataTable2.Merge(dataTable3);
						}
						if (dataTable4 != null)
						{
							dataTable2.Merge(dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									dataTable2.Merge(dataTable5);
								}
							}
						}
						DataSet dataSet5 = dataSet;
						string text7 = "完成情况统计";
						if (!string.IsNullOrEmpty(text2))
						{
							text7 = text2 + text7;
						}
						dataTable2.TableName = str + "-" + str2 + text7;
						string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text8))
						{
							Directory.CreateDirectory(text8);
						}
						string str3 = text7 + ".xls";
						string text9 = text8 + "\\" + text7 + ".xls";
						if (File.Exists(text9))
						{
							File.Delete(text9);
						}
						Excel.DataSetToExcel(dataSet5, text9);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text10 = text10.Replace("\\r\\n", "");
						this.OutPutMessage(text10);
						if (dataSet3 != null)
						{
							dataSet3.Dispose();
							dataSet3 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
						if (dataTable4 != null)
						{
							dataTable4.Dispose();
							dataTable4 = null;
						}
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
					}
				}
			}
		}

		public void GetSubScribDateWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string newValue = " AND Is_GuideSheetPrinted=1";
						string text5 = string.Format("\r\n--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_RQFB@RandNum\r\nSELECT Department,DepartSubA,DepartSubB,DepartSubC,ID_Customer INTO #TempTeamTaskGroupCust_RQFB@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}'\r\nAND ID_Customer IN(SELECT ID_Customer FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere);\r\n--查询团体任务下的所有客户信息\r\n SELECT CONVERT(varchar(10),OnCustPhysicalExamInfo.OperateDate,120) OperateDate,ID_Gender,COUNT(ID_Gender) Gender INTO #TempSubScribDateDT_RQFB@RandNum\r\n FROM #TempTeamTaskGroupCust_RQFB@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere)OnCustPhysicalExamInfo ON #TempTeamTaskGroupCust_RQFB@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nGROUP BY CONVERT(varchar(10),OnCustPhysicalExamInfo.OperateDate,120),ID_Gender ORDER BY OperateDate DESC;\r\n--SELECT * FROM #TempSubScribDateDT_RQFB@RandNum;\r\nSELECT OperateDate,SUM(Gender)SumGender INTO #SumGenderDT_RQFB@RandNum FROM #TempSubScribDateDT_RQFB@RandNum GROUP BY OperateDate;\r\nSELECT A.OperateDate SubScribDate,ISNULL(B.Gender,0) Male,ISNULL(A.SumGender,0)-ISNULL(B.Gender,0) FeMale,A.SumGender FROM #SumGenderDT_RQFB@RandNum A \r\nLEFT JOIN (SELECT * FROM #TempSubScribDateDT_RQFB@RandNum WHERE ID_Gender='1') B ON A.OperateDate=B.OperateDate ORDER BY A.OperateDate DESC;\r\n--获取总数\r\nDECLARE @AllCount DECIMAL;\r\nSELECT @AllCount=SUM(SumGender) FROM #SumGenderDT_RQFB@RandNum;\r\n\r\n--获取男女总人数\r\nSELECT ISNULL(@AllCount,0) AllCount;\r\n\r\n--删除中间表\r\nDROP TABLE #SumGenderDT_RQFB@RandNum;\r\nDROP TABLE #TempSubScribDateDT_RQFB@RandNum;\r\nDROP TABLE #TempTeamTaskGroupCust_RQFB@RandNum;", int2);
						text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", newValue);
						text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataSet dataSet2 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet3 = null;
						decimal d = 0m;
						string text6 = string.Empty;
						int num = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						d += int.Parse(dataSet.Tables[1].Rows[0]["AllCount"].ToString());
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet4.Tables[0];
								d += int.Parse(dataSet4.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取日期分布信息失败(GetSubScribDateWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet2.Tables[0];
								d += int.Parse(dataSet2.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从废弃库中获取日期分布信息失败(GetSubScribDateWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num > 0)
						{
							arrayList = new ArrayList(num);
							int i = 1;
							while (i <= num)
							{
								text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text6 != null)
								{
									if (!string.IsNullOrEmpty(text6))
									{
										try
										{
											dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet3.Tables[0]);
											d += int.Parse(dataSet3.Tables[1].Rows[0]["AllCount"].ToString());
											dataSet3.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text6,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_58F:
								i++;
								continue;
								goto IL_58F;
							}
						}
						if (dataTable3 != null)
						{
							this.MergeSubScribDateWorkLoad(dataTable2, dataTable3);
						}
						if (dataTable4 != null)
						{
							this.MergeSubScribDateWorkLoad(dataTable2, dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									this.MergeSubScribDateWorkLoad(dataTable2, dataTable5);
								}
							}
						}
						DataSet dataSet5 = dataSet;
						string text7 = "日期分布统计";
						if (!string.IsNullOrEmpty(text2))
						{
							text7 = text2 + text7;
						}
						dataTable2.TableName = str + "-" + str2 + text7;
						string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text8))
						{
							Directory.CreateDirectory(text8);
						}
						string str3 = text7 + ".xls";
						string text9 = text8 + "\\" + text7 + ".xls";
						if (File.Exists(text9))
						{
							File.Delete(text9);
						}
						Excel.DataSetToExcel(dataSet5, text9);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text10 = text10.Replace("\\r\\n", "").Replace(" ", "");
						this.OutPutMessage(text10);
						if (dataSet3 != null)
						{
							dataSet3.Dispose();
							dataSet3 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
						if (dataTable4 != null)
						{
							dataTable4.Dispose();
							dataTable4 = null;
						}
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
					}
				}
			}
		}

		private void MergeSubScribDateWorkLoad(DataTable SourceDT, DataTable FromDT)
		{
			if (FromDT != null)
			{
				if (FromDT.Rows.Count > 0)
				{
					if (SourceDT.Rows.Count > 0)
					{
						foreach (DataRow dataRow in SourceDT.Rows)
						{
							DataRow[] array = FromDT.Select("SubScribDate='" + dataRow["SubScribDate"] + "'");
							if (array.Length > 0)
							{
								DataRow[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									DataRow dataRow2 = array2[i];
									dataRow.BeginEdit();
									dataRow["Male"] = int.Parse(dataRow["Male"].ToString()) + int.Parse(dataRow2["Male"].ToString());
									dataRow["FeMale"] = int.Parse(dataRow["FeMale"].ToString()) + int.Parse(dataRow2["FeMale"].ToString());
									dataRow["SumGender"] = int.Parse(dataRow["SumGender"].ToString()) + int.Parse(dataRow2["SumGender"].ToString());
									dataRow.EndEdit();
								}
							}
						}
					}
					else if (FromDT.Rows.Count > 0)
					{
						foreach (DataRow dataRow in FromDT.Rows)
						{
							SourceDT.ImportRow(dataRow);
						}
					}
				}
			}
		}

		public void GetAgeWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string text5 = string.Empty;
						string text6 = string.Format("\r\n--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_NNFB@RandNum\r\nSELECT Department,DepartSubA,DepartSubB,DepartSubC,ID_Customer INTO #TempTeamTaskGroupCust_NNFB@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}'\r\nAND ID_Customer IN(SELECT ID_Customer FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere);\r\n\r\n--查询团体任务下的所有客户信息\r\n SELECT Age AgeName,ID_Gender,COUNT(ID_Gender) Gender INTO #TempSubScribDateDT_NNFB@RandNum FROM(\r\n SELECT CASE \r\n\t\tWHEN Age>=0 AND Age<=4 THEN '0~4'\r\n\t\tWHEN Age>=5 AND Age<=9 THEN '5~9'\r\n\t\tWHEN Age>=10 AND Age<=14 THEN '10~14'\r\n\t\tWHEN Age>=15 AND Age<=19 THEN '15~19'\r\n\t\tWHEN Age>=20 AND Age<=24 THEN '20~24'\r\n\t\tWHEN Age>=25 AND Age<=29 THEN '25~29'\r\n\t\tWHEN Age>=30 AND Age<=34 THEN '30~34'\r\n\t\tWHEN Age>=35 AND Age<=39 THEN '35~39'\r\n\t\tWHEN Age>=40 AND Age<=44 THEN '40~44'\r\n\t\tWHEN Age>=45 AND Age<=49 THEN '45~49'\r\n\t\tWHEN Age>=50 AND Age<=54 THEN '50~54'\r\n\t\tWHEN Age>=55 AND Age<=59 THEN '55~59'\r\n\t\tWHEN Age>=60 AND Age<=64 THEN '60~64'\r\n\t\tWHEN Age>=65 AND Age<=69 THEN '65~69'\r\n\t\tWHEN Age>=70 AND Age<=74 THEN '70~74'\r\n\t\tWHEN Age>=75 AND Age<=79 THEN '75~79'\r\n\t\tWHEN Age>=80 AND Age<=84 THEN '80~84'\r\n\t\tWHEN Age>=85 AND Age<=89 THEN '85~89'\r\n\t\tWHEN Age>=90 AND Age<=94 THEN '90~94'\r\n\t\tWHEN Age>=95 AND Age<=99 THEN '95~99'\r\n\t\tWHEN Age>=100 AND Age<=104 THEN '100~104'\r\n\t\tWHEN Age>=105 AND Age<=109 THEN '105~109'\r\n\t\tWHEN Age>=110 THEN '110~'\r\n\t\tELSE '~0'\r\n      END Age,ID_Gender FROM(SELECT ID_Gender,DATEDIFF(YEAR,BirthDay,GETDATE())Age\r\n FROM #TempTeamTaskGroupCust_NNFB@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere)OnCustPhysicalExamInfo ON #TempTeamTaskGroupCust_NNFB@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer)A\r\n)B\r\nGROUP BY Age,ID_Gender ORDER BY Age DESC;\r\nSELECT AgeName,SUM(Gender) SumGender INTO #SumGenderDT_NNFB@RandNum FROM #TempSubScribDateDT_NNFB@RandNum GROUP BY AgeName;\r\nDECLARE @AllCount DECIMAL;\r\nSELECT @AllCount=SUM(SumGender) FROM #SumGenderDT_NNFB@RandNum;\r\nSELECT A.AgeName,ISNULL(B.Gender,0) Male,ISNULL(A.SumGender,0)-ISNULL(B.Gender,0) FeMale,A.SumGender,CONVERT(DECIMAL(18,2),ISNULL(B.Gender,0)*100/@AllCount) MalePer,CONVERT(DECIMAL(18,2),(A.SumGender-ISNULL(B.Gender,0))*100/@AllCount) FeMalePer,CONVERT(DECIMAL(18,2),A.SumGender*100/@AllCount) SumPer FROM #SumGenderDT_NNFB@RandNum A \r\nLEFT JOIN (SELECT * FROM #TempSubScribDateDT_NNFB@RandNum WHERE ID_Gender='1') B ON A.AgeName=B.AgeName ORDER BY AgeName ASC;\r\n--获取男女总人数\r\nSELECT ISNULL(@AllCount,0) AllCount;\r\n----删除中间表\r\nDROP TABLE #SumGenderDT_NNFB@RandNum;\r\nDROP TABLE #TempSubScribDateDT_NNFB@RandNum;\r\nDROP TABLE #TempTeamTaskGroupCust_NNFB@RandNum;", int2);
						text5 += " AND Is_GuideSheetPrinted=1";
						text6 = text6.Replace("@OnCustPhysicalExamInfoWhere", text5);
						text6 = text6.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataSet dataSet2 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet3 = null;
						decimal num = 0m;
						string text7 = string.Empty;
						int num2 = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num2);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text6.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						num += int.Parse(dataSet.Tables[1].Rows[0]["AllCount"].ToString());
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text6.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet4.Tables[0];
								num += int.Parse(dataSet4.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取年龄分布信息失败(GetAgeWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text6.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet2.Tables[0];
								num += int.Parse(dataSet2.Tables[1].Rows[0]["AllCount"].ToString());
								Log4J.Instance.Info(string.Concat(new object[]
								{
									"--------从废弃库中获取收费项目统计信息[toWasteConnectionString:",
									this.toWasteConnectionString,
									",WasteCustomerDT:",
									dataTable4.Rows.Count,
									",AllCount:",
									num,
									"]"
								}));
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从废弃库中中获取年龄分布信息失败(GetAgeWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num2 > 0)
						{
							arrayList = new ArrayList(num2);
							int i = 1;
							while (i <= num2)
							{
								text7 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text7 != null)
								{
									if (!string.IsNullOrEmpty(text7))
									{
										try
										{
											dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text7, text6.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet3.Tables[0]);
											num += int.Parse(dataSet3.Tables[1].Rows[0]["AllCount"].ToString());
											dataSet3.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text7,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_605:
								i++;
								continue;
								goto IL_605;
							}
						}
						if (dataTable3 != null)
						{
							this.MergeAgeWorkLoadOfTeam(dataTable2, dataTable3);
						}
						if (dataTable4 != null)
						{
							this.MergeAgeWorkLoadOfTeam(dataTable2, dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									this.MergeAgeWorkLoadOfTeam(dataTable2, dataTable5);
								}
							}
						}
						this.MergeAllAgeWorkLoadOfTeam(dataTable2, num);
						DataSet dataSet5 = dataSet;
						string text8 = "年龄分布统计";
						if (!string.IsNullOrEmpty(text2))
						{
							text8 = text2 + text8;
						}
						dataTable2.TableName = str + "-" + str2 + text8;
						string text9 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text9))
						{
							Directory.CreateDirectory(text9);
						}
						string str3 = text8 + ".xls";
						string text10 = text9 + "\\" + text8 + ".xls";
						if (File.Exists(text10))
						{
							File.Delete(text10);
						}
						Excel.DataSetToExcel(dataSet5, text10);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text11 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text11 = text11.Replace("\\r\\n", "").Replace(" ", "");
						this.OutPutMessage(text11);
						if (dataSet3 != null)
						{
							dataSet3.Dispose();
							dataSet3 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
						if (dataTable4 != null)
						{
							dataTable4.Dispose();
							dataTable4 = null;
						}
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
					}
				}
			}
		}

		private void MergeAllAgeWorkLoadOfTeam(DataTable SourceDT, decimal AllCount)
		{
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						dataRow.BeginEdit();
						dataRow["MalePer"] = (decimal.Parse(dataRow["Male"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow["FeMalePer"] = (decimal.Parse(dataRow["FeMale"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow["SumPer"] = (decimal.Parse(dataRow["SumGender"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow.EndEdit();
					}
				}
			}
		}

		private void MergeAgeWorkLoadOfTeam(DataTable SourceDT, DataTable FromDT)
		{
			if (FromDT != null)
			{
				if (FromDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = FromDT.Select("AgeName='" + dataRow["AgeName"] + "'");
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow.BeginEdit();
								dataRow["Male"] = int.Parse(dataRow["Male"].ToString()) + int.Parse(dataRow2["Male"].ToString());
								dataRow["FeMale"] = int.Parse(dataRow["FeMale"].ToString()) + int.Parse(dataRow2["FeMale"].ToString());
								dataRow["SumGender"] = decimal.Parse(dataRow["SumGender"].ToString()) + decimal.Parse(dataRow2["SumGender"].ToString());
								dataRow.EndEdit();
							}
						}
					}
				}
			}
		}

		public void GetCustFeeWorkLoadOfTeam()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string empty = string.Empty;
						string text5 = string.Format("SELECT * FROM TeamTaskGroupFee WHERE ID_TeamTaskGroup IN(SELECT ID_TeamTaskGroup FROM TeamTaskGroup WHERE ID_TeamTask='{0}');", int2);
						string text6 = string.Empty;
						DataTable dataTable = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout)).Tables[0];
						foreach (DataRow dataRow in dataTable.Rows)
						{
							text6 = text6 + dataRow["ID_Fee"].ToString() + ",";
						}
						text6 = text6.TrimEnd(new char[]
						{
							','
						});
						if (!string.IsNullOrEmpty(text6))
						{
							string text7 = string.Format("--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_CJQK@RandNum\r\nSELECT Department,DepartSubA,DepartSubB,DepartSubC,ID_Customer INTO #TempTeamTaskGroupCust_CJQK@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}'\r\nAND ID_Customer IN(SELECT ID_Customer FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere);\r\n\r\n--查询团体任务下所有的客户名单\r\nSELECT * INTO #TempOnCustFee_CJQK@RandNum FROM OnCustFee A WHERE EXISTS(SELECT ID_Customer FROM #TempTeamTaskGroupCust_CJQK@RandNum B WHERE A.ID_Customer=B.ID_Customer);\r\n \r\n--查询团体任务下所有的收费项目ID\r\n--SELECT * INTO TempTeamTaskGroupFee FROM TeamTaskGroupFee A WHERE EXISTS(SELECT ID_TeamTaskGroup FROM TeamTaskGroup B WHERE A.ID_TeamTaskGroup=B.ID_TeamTaskGroup AND B.ID_TeamTask='167');\r\n\r\n--查询只包含在团体备单项目中的客户收费项目\r\nSELECT C.*,B.ID_Section,B.SectionName INTO #TempSYSSectionCustFee_CJQK@RandNum FROM(\r\nSELECT ID_Customer,ID_Fee,FeeItemName,ISNULL(Is_Examined,0) Is_Examined FROM #TempOnCustFee_CJQK@RandNum A WHERE ID_Fee IN({1})) C\r\nINNER JOIN BusFee B ON C.ID_Fee=B.ID_Fee\r\n\r\n--查询团体任务下的所有客户信息\r\n SELECT #TempSYSSectionCustFee_CJQK@RandNum.ID_Section,#TempSYSSectionCustFee_CJQK@RandNum.ID_Fee,ID_Gender,COUNT(ID_Gender) Gender INTO #TempCustFeeOfTeamDT_CJQK@RandNum\r\n FROM #TempSYSSectionCustFee_CJQK@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1)OnCustPhysicalExamInfo ON #TempSYSSectionCustFee_CJQK@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nGROUP BY #TempSYSSectionCustFee_CJQK@RandNum.ID_Section,#TempSYSSectionCustFee_CJQK@RandNum.ID_Fee,ID_Gender;\r\n\r\n--SELECT * FROM #TempCustFeeOfTeamDT_CJQK@RandNum;\r\n\r\n--查询团体任务下已参检的所有客户信息\r\nSELECT #TempSYSSectionCustFee_CJQK@RandNum.ID_Section,#TempSYSSectionCustFee_CJQK@RandNum.ID_Fee,ID_Gender,COUNT(ID_Gender) Gender INTO #TempCompleteCustFeeOfTeamDT_CJQK@RandNum\r\nFROM (SELECT * FROM #TempSYSSectionCustFee_CJQK@RandNum WHERE Is_Examined=0)#TempSYSSectionCustFee_CJQK@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1)OnCustPhysicalExamInfo ON #TempSYSSectionCustFee_CJQK@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nGROUP BY #TempSYSSectionCustFee_CJQK@RandNum.ID_Section,#TempSYSSectionCustFee_CJQK@RandNum.ID_Fee,ID_Gender;\r\n\r\n\r\n--获取全部数量\r\nDECLARE @AllCount DECIMAL;\r\nSELECT @AllCount=COUNT(0) FROM #TempTeamTaskGroupCust_CJQK@RandNum; \r\n\r\n--计算已参检的科室信息(对应科室，对应收费项目参检信息)\r\nSELECT A.ID_Section,A.ID_Fee,A.ID_Gender,A.Gender-ISNULL(B.Gender,0) Gender INTO #TempSumCompleteCustFeeOfTeamDT_CJQK@RandNum FROM #TempCustFeeOfTeamDT_CJQK@RandNum A\r\nLEFT JOIN #TempCompleteCustFeeOfTeamDT_CJQK@RandNum B ON A.ID_Section=B.ID_Section AND A.ID_Fee=B.ID_Fee AND A.ID_Gender=B.ID_Gender;\r\n\r\n--获取全部参检信息\r\nSELECT C.ID_Section,C.SectionName,D.FeeName,ISNULL(B.Gender,0) MaleCustFee,ISNULL(A.SumGender,0)-ISNULL(B.Gender,0) FeMaleCustFee,A.SumGender SumGenderCustFee\r\n,CONVERT(DECIMAL(18,2),ISNULL(B.Gender,0)*100/@AllCount) MalePerCustFee,CONVERT(DECIMAL(18,2),(A.SumGender-ISNULL(B.Gender,0))*100/@AllCount) FeMalePerCustFee,CONVERT(DECIMAL(18,2),A.SumGender*100/@AllCount) SumPerCustFee\r\n FROM(SELECT ID_Section,ID_Fee,SUM(Gender) SumGender FROM #TempSumCompleteCustFeeOfTeamDT_CJQK@RandNum GROUP BY ID_Section,ID_Fee)A\r\nLEFT JOIN(SELECT * FROM #TempSumCompleteCustFeeOfTeamDT_CJQK@RandNum WHERE ID_Gender='1')B ON A.ID_Section=B.ID_Section AND A.ID_Fee=B.ID_Fee\r\n INNER JOIN SYSSection C ON A.ID_Section=C.ID_Section \r\nINNER JOIN BusFee D ON A.ID_Fee=D.ID_Fee ORDER BY C.DispOrder ASC;\r\n\r\n--查询男女总数\r\nSELECT ISNULL(@AllCount,0) AllCount;\r\n--删除中间表\r\nDROP TABLE #TempTeamTaskGroupCust_CJQK@RandNum;\r\nDROP TABLE #TempOnCustFee_CJQK@RandNum;\r\n--DROP TABLE TempTeamTaskGroupFee;\r\nDROP TABLE #TempSYSSectionCustFee_CJQK@RandNum;\r\nDROP TABLE #TempCustFeeOfTeamDT_CJQK@RandNum;\r\nDROP TABLE #TempCompleteCustFeeOfTeamDT_CJQK@RandNum;\r\nDROP TABLE #TempSumCompleteCustFeeOfTeamDT_CJQK@RandNum;", int2, text6);
							text7 = text7.Replace("@OnCustPhysicalExamInfoWhere", empty);
							text7 = text7.Replace("@RandNum", Public.GetGuid("-", string.Empty));
							DataTable dataTable2 = null;
							DataSet dataSet = null;
							DataTable dataTable3 = null;
							DataTable dataTable4 = null;
							DataSet dataSet2 = null;
							DataTable dataTable5 = null;
							ArrayList arrayList = null;
							DataSet dataSet3 = null;
							decimal num = 0m;
							string text8 = string.Empty;
							int num2 = 0;
							int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num2);
							dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text7.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
							dataTable3 = dataSet.Tables[0];
							num += int.Parse(dataSet.Tables[1].Rows[0]["AllCount"].ToString());
							if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
							{
								try
								{
									DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text7.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
									dataTable4 = dataSet4.Tables[0];
									num += int.Parse(dataSet4.Tables[1].Rows[0]["AllCount"].ToString());
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",从离线库中获取参检情况信息失败(GetCustFeeWorkLoadOfTeam)：",
										ex.Message,
										" ",
										Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
									}));
								}
							}
							if (!string.IsNullOrEmpty(this.toWasteConnectionString))
							{
								try
								{
									dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text7.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
									dataTable5 = dataSet2.Tables[0];
									num += int.Parse(dataSet2.Tables[1].Rows[0]["AllCount"].ToString());
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",从废弃库中获取参检情况信息失败(GetCustFeeWorkLoadOfTeam)：",
										ex.Message,
										" ",
										Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
									}));
								}
							}
							if (num2 > 0)
							{
								arrayList = new ArrayList(num2);
								int i = 1;
								while (i <= num2)
								{
									text8 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
									if (text8 != null)
									{
										if (!string.IsNullOrEmpty(text8))
										{
											try
											{
												dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text8, text7.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
												arrayList.Add(dataSet3.Tables[0]);
												num += int.Parse(dataSet3.Tables[1].Rows[0]["AllCount"].ToString());
												dataSet3.Dispose();
											}
											catch (Exception ex)
											{
												Log4J.Instance.Error(string.Concat(new object[]
												{
													Public.GetClientIP(),
													",",
													this.LoginUserModel.UserName,
													",查询团体任务[",
													int2,
													"]阳性结果明细信息失败，失败原因为从",
													text8,
													"获取数据出现如下错误：",
													ex.Message
												}));
											}
										}
									}
									IL_679:
									i++;
									continue;
									goto IL_679;
								}
							}
							if (dataTable4 != null)
							{
								this.MergeCustFeeWorkLoad(dataTable3, dataTable4);
							}
							if (dataTable5 != null)
							{
								this.MergeCustFeeWorkLoad(dataTable3, dataTable5);
							}
							if (arrayList != null)
							{
								foreach (DataTable dataTable6 in arrayList)
								{
									if (dataTable6 != null)
									{
										this.MergeCustFeeWorkLoad(dataTable3, dataTable6);
									}
								}
							}
							this.MergeAllCustFeeWorkLoad(dataTable3, num);
							DataSet dataSet5 = dataSet;
							string text9 = "参检分析统计";
							if (!string.IsNullOrEmpty(text2))
							{
								text9 = text2 + text9;
							}
							dataTable3.TableName = str + "-" + str2 + text9;
							string text10 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
							if (!Directory.Exists(text10))
							{
								Directory.CreateDirectory(text10);
							}
							string str3 = text9 + ".xls";
							string text11 = text10 + "\\" + text9 + ".xls";
							if (File.Exists(text11))
							{
								File.Delete(text11);
							}
							Excel.DataSetToExcel(dataSet5, text11);
							DataTable dataTable7 = new DataTable();
							dataTable7.Columns.Add("FilePath");
							DataRow dataRow2 = dataTable7.NewRow();
							dataRow2["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
							dataTable7.Rows.Add(dataRow2);
							dataSet5.Tables.Add(dataTable7);
							string text12 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
							text12 = text12.Replace("\\r\\n", "").Replace(" ", "");
							this.OutPutMessage(text12);
							if (dataSet3 != null)
							{
								dataSet3.Dispose();
								dataSet3 = null;
							}
							if (dataTable4 != null)
							{
								dataTable4.Dispose();
								dataTable4 = null;
							}
							if (dataSet != null)
							{
								dataSet.Dispose();
								dataSet = null;
							}
							if (dataTable3 != null)
							{
								dataTable3.Dispose();
								dataTable3 = null;
							}
							if (dataSet != null)
							{
								dataSet.Dispose();
								dataSet = null;
							}
							if (arrayList != null)
							{
								arrayList.Clear();
								arrayList = null;
							}
							if (dataTable2 != null)
							{
								dataTable2.Clear();
								dataTable2 = null;
							}
							if (dataSet5 != null)
							{
								dataSet5.Dispose();
							}
							if (dataTable5 != null)
							{
								dataTable5.Dispose();
								dataTable5 = null;
							}
							if (dataSet2 != null)
							{
								dataSet2.Dispose();
								dataSet2 = null;
							}
						}
					}
				}
			}
		}

		private void MergeAllCustFeeWorkLoad(DataTable SourceDT, decimal AllCount)
		{
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						dataRow.BeginEdit();
						dataRow["MalePerCustFee"] = (decimal.Parse(dataRow["MaleCustFee"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow["FeMalePerCustFee"] = (decimal.Parse(dataRow["FeMaleCustFee"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow["SumPerCustFee"] = (decimal.Parse(dataRow["SumGenderCustFee"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow.EndEdit();
					}
				}
			}
		}

		private void MergeCustFeeWorkLoad(DataTable SourceDT, DataTable FromDT)
		{
			if (FromDT != null)
			{
				if (FromDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = FromDT.Select(string.Concat(new object[]
						{
							"ID_Section='",
							dataRow["ID_Section"],
							"' AND FeeName='",
							dataRow["FeeName"],
							"'"
						}));
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow.BeginEdit();
								dataRow["MaleCustFee"] = int.Parse(dataRow["MaleCustFee"].ToString()) + int.Parse(dataRow2["MaleCustFee"].ToString());
								dataRow["FeMaleCustFee"] = int.Parse(dataRow["FeMaleCustFee"].ToString()) + int.Parse(dataRow2["FeMaleCustFee"].ToString());
								dataRow["SumGenderCustFee"] = int.Parse(dataRow["SumGenderCustFee"].ToString()) + int.Parse(dataRow2["SumGenderCustFee"].ToString());
								dataRow.EndEdit();
							}
						}
					}
				}
			}
		}

		public void GetCustomerFormOfTeam()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string empty = string.Empty;
						string text5 = string.Format("\r\n--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_RYGC@RandNum\r\nSELECT Department,DepartSubA,DepartSubB,DepartSubC,ID_Customer INTO #TempTeamTaskGroupCust_RYGC@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}'\r\nAND ID_Customer IN(SELECT ID_Customer FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere);\r\n\r\n--获取备单的所有客户\r\nSELECT OnCustPhysicalExamInfo.ID_Customer,ISNULL(OnCustPhysicalExamInfo.Is_ExamStarted,0)Is_ExamStarted,ID_Gender INTO #ALLTempOnCustPhysicalExamInfo_RYGC@RandNum\r\nFROM #TempTeamTaskGroupCust_RYGC@RandNum\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1)OnCustPhysicalExamInfo ON #TempTeamTaskGroupCust_RYGC@RandNum.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\n\r\nDECLARE @MaleCount DECIMAL;--定义计划男性数量\r\nDECLARE @FMaleCount DECIMAL;--定义计划女性数量\r\nDECLARE @ALLCount DECIMAL;--定义计划总数量\r\n\r\nDECLARE @ExamStartedMaleCount DECIMAL;--定义参检男性数量\r\nDECLARE @ExamStartedFMaleCount DECIMAL;--定义参检女性数量\r\nDECLARE @ALLExamStartedCount DECIMAL;--定义参检总数量\r\n\r\nDECLARE @NotExamStartedMaleCount DECIMAL;--定义未参检男性数量\r\nDECLARE @NotExamStartedFMaleCount DECIMAL;--定义未参检女性数量\r\nDECLARE @ALLNotExamStartedCount DECIMAL;--定义未参检数量\r\n\r\n--设置计划男女值\r\nSET @MaleCount=(SELECT COUNT(ID_Gender) FROM #ALLTempOnCustPhysicalExamInfo_RYGC@RandNum WHERE ID_Gender=1);\r\nSET @FMaleCount=(SELECT COUNT(ID_Gender) FROM #ALLTempOnCustPhysicalExamInfo_RYGC@RandNum WHERE ID_Gender!=1);\r\nSET @ALLCount=@MaleCount+@FMaleCount;\r\n\r\nIF @ALLCount!=0 \r\nBEGIN\r\n--设置参检男女值\r\nSET @ExamStartedMaleCount=(SELECT COUNT(ID_Gender) FROM #ALLTempOnCustPhysicalExamInfo_RYGC@RandNum WHERE ID_Gender=1 AND Is_ExamStarted=1);\r\nSET @ExamStartedFMaleCount=(SELECT COUNT(ID_Gender) FROM #ALLTempOnCustPhysicalExamInfo_RYGC@RandNum WHERE ID_Gender=2 AND Is_ExamStarted=1);\r\nSET @ALLExamStartedCount=@ExamStartedMaleCount+@ExamStartedFMaleCount;\r\n\r\n\r\n--设置未检男女值\r\nSET @NotExamStartedMaleCount=@MaleCount-@ExamStartedMaleCount;\r\nSET @NotExamStartedFMaleCount=@FMaleCount-@ExamStartedFMaleCount;\r\nSET @ALLNotExamStartedCount=@NotExamStartedMaleCount+@NotExamStartedFMaleCount;\r\n--获取计划男女比列\r\nSELECT '计划' ExamType, @MaleCount Male,@FMaleCount FMale,@ALLCount SumGender,CONVERT(DECIMAL(18,2),ISNULL(@MaleCount,0)*100/@AllCount) MealePer,CONVERT(DECIMAL(18,2),ISNULL(@FMaleCount,0)*100/@AllCount) FMalePer,CONVERT(DECIMAL(18,2),@ALLCount*100/@AllCount) SumPer\r\nUNION ALL\r\nSELECT '参检' ExamType,@ExamStartedMaleCount Male,@ExamStartedFMaleCount FMale,@ALLExamStartedCount SumGender,CONVERT(DECIMAL(18,2),ISNULL(@ExamStartedMaleCount,0)*100/@AllCount) MealePer,CONVERT(DECIMAL(18,2),ISNULL(@ExamStartedFMaleCount,0)*100/@AllCount) FMalePer,CONVERT(DECIMAL(18,2),@ALLExamStartedCount*100/@AllCount) SumPer\r\nUNION ALL\r\nSELECT '未检' ExamType,@NotExamStartedMaleCount Male,@NotExamStartedFMaleCount FMale,@ALLNotExamStartedCount SumGender,CONVERT(DECIMAL(18,2),ISNULL(@NotExamStartedMaleCount,0)*100/@AllCount) MealePer,CONVERT(DECIMAL(18,2),ISNULL(@NotExamStartedFMaleCount,0)*100/@AllCount) FMalePer,CONVERT(DECIMAL(18,2),@ALLNotExamStartedCount*100/@AllCount) SumPer\r\nEND\r\nELSE \r\nBEGIN\r\n    SELECT '计划' ExamType,0 Male,0 FMale,0 SumGender,0 MealePer,0 FMalePer,0 SumPer\r\n    UNION ALL SELECT '参检' ExamType,0 Male,0 FMale,0 SumGender,0 MealePer,0 FMalePer,0 SumPer\r\n    UNION ALL SELECT '未检' ExamType,0 Male,0 FMale,0 SumGender,0 MealePer,0 FMalePer,0 SumPer\r\nEND\r\n--获取男女总人数\r\nSELECT ISNULL(@AllCount,0) AllCount;\r\n\r\n--删除中间表\r\nDROP TABLE #TempTeamTaskGroupCust_RYGC@RandNum;\r\nDROP TABLE #ALLTempOnCustPhysicalExamInfo_RYGC@RandNum;", int2);
						text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", empty);
						text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataSet dataSet2 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet3 = null;
						decimal num = 0m;
						string text6 = string.Empty;
						int num2 = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num2);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						num += int.Parse(dataSet.Tables[1].Rows[0]["AllCount"].ToString());
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet4.Tables[0];
								num += int.Parse(dataSet4.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取人员构成信息失败(GetCustomerFormOfTeam)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet2.Tables[0];
								num += int.Parse(dataSet2.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从废弃库中获取人员构成信息失败(GetCustomerFormOfTeam)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num2 > 0)
						{
							arrayList = new ArrayList(num2);
							int i = 1;
							while (i <= num2)
							{
								text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text6 != null)
								{
									if (!string.IsNullOrEmpty(text6))
									{
										try
										{
											dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet3.Tables[0]);
											num += int.Parse(dataSet3.Tables[1].Rows[0]["AllCount"].ToString());
											dataSet3.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text6,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_58F:
								i++;
								continue;
								goto IL_58F;
							}
						}
						if (dataTable3 != null)
						{
							this.MergeCustomerFormOfTeam(dataTable2, dataTable3);
						}
						if (dataTable4 != null)
						{
							this.MergeCustomerFormOfTeam(dataTable2, dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									this.MergeCustomerFormOfTeam(dataTable2, dataTable5);
								}
							}
						}
						this.MergeAllCustomerFormOfTeam(dataTable2, num);
						DataSet dataSet5 = dataSet;
						string text7 = "人员构成";
						if (!string.IsNullOrEmpty(text2))
						{
							text7 = text2 + text7;
						}
						dataTable2.TableName = str + "-" + str2 + text7;
						string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text8))
						{
							Directory.CreateDirectory(text8);
						}
						string str3 = text7 + ".xls";
						string text9 = text8 + "\\" + text7 + ".xls";
						if (File.Exists(text9))
						{
							File.Delete(text9);
						}
						Excel.DataSetToExcel(dataSet5, text9);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text10 = text10.Replace("\\r\\n", "").Replace(" ", "");
						this.OutPutMessage(text10);
						if (dataSet3 != null)
						{
							dataSet3.Dispose();
							dataSet3 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
						if (dataTable4 != null)
						{
							dataTable4.Dispose();
							dataTable4 = null;
						}
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
					}
				}
			}
		}

		private void MergeAllCustomerFormOfTeam(DataTable SourceDT, decimal AllCount)
		{
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						if (AllCount != 0m)
						{
							dataRow.BeginEdit();
							dataRow["MealePer"] = (decimal.Parse(dataRow["Male"].ToString()) * 100m / AllCount).ToString("f2");
							dataRow["FMalePer"] = (decimal.Parse(dataRow["FMale"].ToString()) * 100m / AllCount).ToString("f2");
							dataRow["SumPer"] = (decimal.Parse(dataRow["SumGender"].ToString()) * 100m / AllCount).ToString("f2");
							dataRow.EndEdit();
						}
					}
				}
			}
		}

		private void MergeCustomerFormOfTeam(DataTable SourceDT, DataTable FromDT)
		{
			if (FromDT != null)
			{
				if (FromDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = FromDT.Select("ExamType='" + dataRow["ExamType"] + "'");
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow.BeginEdit();
								dataRow["Male"] = int.Parse(dataRow["Male"].ToString()) + int.Parse(dataRow2["Male"].ToString());
								dataRow["FMale"] = int.Parse(dataRow["FMale"].ToString()) + int.Parse(dataRow2["FMale"].ToString());
								dataRow["SumGender"] = int.Parse(dataRow["SumGender"].ToString()) + int.Parse(dataRow2["SumGender"].ToString());
								dataRow.EndEdit();
							}
						}
					}
				}
			}
		}

		public void GetConclusionWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string empty = string.Empty;
						string text5 = string.Format("\r\n--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_JCTJ@RandNum\r\nSELECT Department,DepartSubA,DepartSubB,DepartSubC,ID_Customer INTO #TempTeamTaskGroupCust_JCTJ@RandNum FROM TeamTaskGroupCust WHERE ID_TeamTask='{0}'\r\nAND ID_Customer IN(SELECT ID_Customer FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere);\r\n\r\n--查询检出团体结论词的所有男女客户\r\nSELECT ISNULL(D.TeamConclusionName,'') TeamConclusionName,C.ID_Customer,ID_Gender INTO #ALLTempOnCustPhysicalExamInfo_JCTJ@RandNum FROM(SELECT A.* FROM OnCustConclusion A WHERE EXISTS(SELECT ID_Customer FROM #TempTeamTaskGroupCust_JCTJ@RandNum B WHERE A.ID_Customer=B.ID_Customer))C\r\nINNER JOIN BusConclusion D ON C.ID_Conclusion=D.ID_Conclusion\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo A WHERE EXISTS(SELECT ID_Customer FROM #TempTeamTaskGroupCust_JCTJ@RandNum B WHERE A.ID_Customer=B.ID_Customer))OnCustRelationCustPEInfo ON C.ID_Customer=OnCustRelationCustPEInfo.ID_Customer\r\n--SELECT * FROM #ALLTempOnCustPhysicalExamInfo_JCTJ@RandNum\r\nDECLARE @ALLCount INT;--定义计划总数量\r\nSET @ALLCount=(SELECT COUNT(0) FROM #ALLTempOnCustPhysicalExamInfo_JCTJ@RandNum);\r\nIF @AllCount>0\r\nBEGIN\r\nDECLARE @CheckOutMaleCount DECIMAL;--定义检出团体结论词男性数量\r\nDECLARE @CheckOutFMaleCount DECIMAL;--定义检出团体结论词女性数量\r\nDECLARE @CheckOutALLCount DECIMAL--定义检出团体结论词总数量\r\n--按照团体结论词合并男女\r\nSELECT TeamConclusionName,ID_Gender,COUNT(ID_Gender) SumGender INTO SumTeamConclusionName FROM #ALLTempOnCustPhysicalExamInfo_JCTJ@RandNum GROUP BY TeamConclusionName,ID_Gender\r\n\r\nSET @CheckOutMaleCount=(SELECT COUNT(0) FROM SumTeamConclusionName WHERE ID_Gender=1);\r\nSET @CheckOutFMaleCount=(SELECT COUNT(0) FROM SumTeamConclusionName WHERE ID_Gender!=1);\r\nSET @CheckOutALLCount=@CheckOutMaleCount+@CheckOutFMaleCount;\r\n--print @ALLCount;\r\n--PRINT @CheckOutMaleCount;\r\n--PRINT @CheckOutFMaleCount;\r\n--PRINT @CheckOutALLCount;\r\n\r\n--获取全部参检信息\r\nSELECT A.TeamConclusionName,ISNULL(B.SumGender,0) CheckOutMale,ISNULL(A.SumGender,0)-ISNULL(B.SumGender,0) CheckOutFMale,A.SumGender SumCheckOutCount\r\n,CONVERT(DECIMAL(18,2),ISNULL(B.SumGender,0)*100/@AllCount) CheckOutMalePer\r\n,CONVERT(DECIMAL(18,2),(ISNULL(A.SumGender,0)-ISNULL(B.SumGender,0))*100/@AllCount) CheckOutFMalePer,\r\nCONVERT(DECIMAL(18,2),A.SumGender*100/@AllCount) SumPer\r\nFROM (SELECT TeamConclusionName,SUM(SumGender) SumGender FROM SumTeamConclusionName GROUP BY TeamConclusionName) A \r\nLEFT JOIN (SELECT * FROM SumTeamConclusionName WHERE ID_Gender='1') B ON A.TeamConclusionName=B.TeamConclusionName;\r\n\r\nDROP TABLE SumTeamConclusionName;\r\nEND\r\nELSE \r\nBEGIN\r\nSELECT 'Empty' TeamConclusionName,0 CheckOutMale,0 CheckOutFMale,0 SumCheckOutCount\r\n,0.0 CheckOutMalePer\r\n,0.0 CheckOutFMalePer,\r\n0.0 SumPer\r\nEND\r\nSELECT ISNULL(@ALLCount,0) ALLCount;\r\n--删除中间表\r\nDROP TABLE #TempTeamTaskGroupCust_JCTJ@RandNum;\r\nDROP TABLE #ALLTempOnCustPhysicalExamInfo_JCTJ@RandNum;", int2);
						text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", empty);
						text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataSet dataSet2 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet3 = null;
						decimal num = 0m;
						string text6 = string.Empty;
						int num2 = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num2);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						num += int.Parse(dataSet.Tables[1].Rows[0]["AllCount"].ToString());
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet4.Tables[0];
								num += int.Parse(dataSet4.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取检出统计信息失败(GetConclusionWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet2.Tables[0];
								num += int.Parse(dataSet2.Tables[1].Rows[0]["AllCount"].ToString());
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从废弃库中获取检出统计信息失败(GetConclusionWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num2 > 0)
						{
							arrayList = new ArrayList(num2);
							int i = 1;
							while (i <= num2)
							{
								text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text6 != null)
								{
									if (!string.IsNullOrEmpty(text6))
									{
										try
										{
											dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet3.Tables[0]);
											num += int.Parse(dataSet3.Tables[1].Rows[0]["AllCount"].ToString());
											dataSet3.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text6,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_58F:
								i++;
								continue;
								goto IL_58F;
							}
						}
						if (dataTable3 != null)
						{
							dataTable2.Merge(dataTable3);
						}
						if (dataTable4 != null)
						{
							dataTable2.Merge(dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									dataTable2.Merge(dataTable5);
								}
							}
						}
						DataRow[] array = dataTable2.Select("TeamConclusionName='Empty'");
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int j = 0; j < array2.Length; j++)
							{
								DataRow row = array2[j];
								dataTable2.Rows.Remove(row);
							}
						}
						dataTable2 = this.MergeSameRowOfTeamConclusion(dataTable2);
						this.MergeAllConclusionWorkLoadOfTeam(dataTable2, num);
						dataSet.Tables.Clear();
						dataSet.Tables.Add(dataTable2);
						DataSet dataSet5 = dataSet;
						string text7 = "检出统计";
						if (!string.IsNullOrEmpty(text2))
						{
							text7 = text2 + text7;
						}
						dataTable2.TableName = str + "-" + str2 + text7;
						string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text8))
						{
							Directory.CreateDirectory(text8);
						}
						string str3 = text7 + ".xls";
						string text9 = text8 + "\\" + text7 + ".xls";
						if (File.Exists(text9))
						{
							File.Delete(text9);
						}
						Excel.DataSetToExcel(dataSet5, text9);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text10 = text10.Replace("\\r\\n", "").Replace(" ", "");
						this.OutPutMessage(text10);
						if (dataSet3 != null)
						{
							dataSet3.Dispose();
							dataSet3 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
						if (dataTable4 != null)
						{
							dataTable4.Dispose();
							dataTable4 = null;
						}
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
					}
				}
			}
		}

		private void MergeAllConclusionWorkLoadOfTeam(DataTable SourceDT, decimal AllCount)
		{
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						dataRow.BeginEdit();
						dataRow["CheckOutMalePer"] = (decimal.Parse(dataRow["CheckOutMale"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow["CheckOutFMalePer"] = (decimal.Parse(dataRow["CheckOutFMale"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow["SumPer"] = (decimal.Parse(dataRow["SumCheckOutCount"].ToString()) * 100m / AllCount).ToString("f2");
						dataRow.EndEdit();
					}
				}
			}
		}

		private void MergeConclusionWorkLoadOfTeam(DataTable SourceDT, DataTable FromDT)
		{
			if (FromDT != null)
			{
				if (FromDT.Rows.Count > 0)
				{
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = FromDT.Select("TeamConclusionName='" + dataRow["TeamConclusionName"] + "'");
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow.BeginEdit();
								dataRow["CheckOutMale"] = int.Parse(dataRow["CheckOutMale"].ToString()) + int.Parse(dataRow2["CheckOutMale"].ToString());
								dataRow["CheckOutFMale"] = int.Parse(dataRow["CheckOutFMale"].ToString()) + int.Parse(dataRow2["CheckOutFMale"].ToString());
								dataRow["SumCheckOutCount"] = int.Parse(dataRow["SumCheckOutCount"].ToString()) + int.Parse(dataRow2["SumCheckOutCount"].ToString());
								dataRow.EndEdit();
							}
						}
					}
				}
			}
		}

		public void GetAllConclusionWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Team", -1);
				if (@int != -1)
				{
					int int2 = base.GetInt("ID_TeamTask", -1);
					if (int2 != -1)
					{
						string text = base.Server.HtmlDecode(base.GetString("CustomerName"));
						string text2 = base.Server.HtmlDecode(base.GetString("TeamName"));
						string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
						string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
						string str = text3.Replace("-", string.Empty);
						string str2 = text4.Replace("-", string.Empty);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 += " 00:00:00";
						}
						if (!string.IsNullOrEmpty(text4))
						{
							text4 += " 23:59:59";
						}
						string empty = string.Empty;
						string text5 = string.Format("IF EXISTS(SELECT TOP 1 * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('#TempTeamTaskGroupCust_JLHZ@RandNum'))\r\nBEGIN\r\n\tDROP TABLE #TempTeamTaskGroupCust_JLHZ@RandNum;\r\nEND\r\n--将团体任务下的所有体检信息放入中间表#TempTeamTaskGroupCust_JLHZ@RandNum\r\nSELECT TeamTaskGroupCust.*,OnCustPhysicalExamInfo.TeamName INTO #TempTeamTaskGroupCust_JLHZ@RandNum FROM(SELECT ID_Customer FROM TeamTaskGroupCust\r\n WHERE ID_TeamTask='{0}') TeamTaskGroupCust\r\nINNER JOIN (SELECT ID_Customer,TeamName FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere)OnCustPhysicalExamInfo ON TeamTaskGroupCust.ID_Customer=OnCustPhysicalExamInfo.ID_Customer;\r\n\r\n--从结论词信息表中获取包含团体结论词的体检信息\r\nSELECT CASE WHEN C.DiagnoseType=10 THEN '主要诊断' WHEN C.DiagnoseType=20 THEN '次要诊断' WHEN C.DiagnoseType=30 THEN '指标异常'\r\nWHEN C.DiagnoseType=40 THEN '正常' WHEN C.DiagnoseType=50 THEN '其它' ELSE '' END DiagnoseType, D.TeamConclusionName,C.ConclusionName,C.ID_Customer,CustomerName,GenderName,FLOOR(DATEDIFF(DY,BirthDay,(CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END))/365.25)Age,Department,DepartSubA,DepartSubB,DepartSubC FROM(SELECT * FROM OnCustConclusion A WHERE EXISTS(SELECT ID_Customer FROM #TempTeamTaskGroupCust_JLHZ@RandNum B WHERE A.ID_Customer=B.ID_Customer))C\r\nINNER JOIN BusConclusion D ON C.ID_Conclusion=D.ID_Conclusion\r\nINNER JOIN #TempTeamTaskGroupCust_JLHZ@RandNum E ON C.ID_Customer=E.ID_Customer\r\nINNER JOIN (SELECT * FROM OnCustPhysicalExamInfo WHERE 1=1)OnCustPhysicalExamInfo ON C.ID_Customer=OnCustPhysicalExamInfo.ID_Customer\r\nORDER BY D.TeamConclusionName;\r\n\r\n--删除中间表\r\nDROP TABLE #TempTeamTaskGroupCust_JLHZ@RandNum;", int2);
						text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", empty);
						text5 = text5.Replace("@RandNum", Public.GetGuid("-", string.Empty));
						DataTable dataTable = null;
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						DataTable dataTable3 = null;
						DataSet dataSet2 = null;
						DataTable dataTable4 = null;
						ArrayList arrayList = null;
						DataSet dataSet3 = null;
						string text6 = string.Empty;
						int num = 0;
						int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
						dataTable2 = dataSet.Tables[0];
						if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
						{
							try
							{
								DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
								dataTable3 = dataSet4.Tables[0];
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从离线库中获取结论词统计信息失败(GetAllConclusionWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (!string.IsNullOrEmpty(this.toWasteConnectionString))
						{
							try
							{
								dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
								dataTable4 = dataSet2.Tables[0];
							}
							catch (Exception ex)
							{
								Log4J.Instance.Error(string.Concat(new string[]
								{
									Public.GetClientIP(),
									",",
									this.LoginUserModel.UserName,
									",从废弃库中获取结论词统计信息失败(GetAllConclusionWorkLoad)：",
									ex.Message,
									" ",
									Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
								}));
							}
						}
						if (num > 0)
						{
							arrayList = new ArrayList(num);
							int i = 1;
							while (i <= num)
							{
								text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
								if (text6 != null)
								{
									if (!string.IsNullOrEmpty(text6))
									{
										try
										{
											dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
											arrayList.Add(dataSet3.Tables[0]);
											dataSet3.Dispose();
										}
										catch (Exception ex)
										{
											Log4J.Instance.Error(string.Concat(new object[]
											{
												Public.GetClientIP(),
												",",
												this.LoginUserModel.UserName,
												",查询团体任务[",
												int2,
												"]阳性结果明细信息失败，失败原因为从",
												text6,
												"获取数据出现如下错误：",
												ex.Message
											}));
										}
									}
								}
								IL_49F:
								i++;
								continue;
								goto IL_49F;
							}
						}
						if (dataTable3 != null)
						{
							dataTable2.Merge(dataTable3);
						}
						if (dataTable4 != null)
						{
							dataTable2.Merge(dataTable4);
						}
						if (arrayList != null)
						{
							foreach (DataTable dataTable5 in arrayList)
							{
								if (dataTable5 != null)
								{
									dataTable2.Merge(dataTable5);
								}
							}
						}
						this.MergeSameRowOfAllTeamConclusion(dataTable2);
						DataSet dataSet5 = dataSet;
						string text7 = "结论汇总";
						if (!string.IsNullOrEmpty(text2))
						{
							text7 = text2 + text7;
						}
						dataTable2.TableName = str + "-" + str2 + text7;
						string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text8))
						{
							Directory.CreateDirectory(text8);
						}
						string str3 = text7 + ".xls";
						string text9 = text8 + "\\" + text7 + ".xls";
						if (File.Exists(text9))
						{
							File.Delete(text9);
						}
						Excel.DataSetToExcel(dataSet5, text9);
						DataTable dataTable6 = new DataTable();
						dataTable6.Columns.Add("FilePath");
						DataRow dataRow = dataTable6.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
						dataTable6.Rows.Add(dataRow);
						dataSet5.Tables.Add(dataTable6);
						string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
						text10 = text10.Replace("\\r\\n", "").Replace(" ", "");
						this.OutPutMessage(text10);
						if (dataSet3 != null)
						{
							dataSet3.Dispose();
							dataSet3 = null;
						}
						if (dataTable3 != null)
						{
							dataTable3.Dispose();
							dataTable3 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (arrayList != null)
						{
							arrayList.Clear();
							arrayList = null;
						}
						if (dataTable != null)
						{
							dataTable.Clear();
							dataTable = null;
						}
						if (dataSet5 != null)
						{
							dataSet5.Dispose();
						}
						if (dataTable4 != null)
						{
							dataTable4.Dispose();
							dataTable4 = null;
						}
						if (dataSet2 != null)
						{
							dataSet2.Dispose();
							dataSet2 = null;
						}
					}
				}
			}
		}

		public DataTable MergeSameRowOfTeamConclusion(DataTable SourceDT)
		{
			DataTable dataTable = SourceDT.Clone();
			foreach (DataRow dataRow in SourceDT.Rows)
			{
				DataRow[] array = dataTable.Select("TeamConclusionName='" + dataRow["TeamConclusionName"].ToString() + "'");
				if (array.Length > 0)
				{
					array[0]["CheckOutMale"] = int.Parse(array[0]["CheckOutMale"].ToString()) + int.Parse(dataRow["CheckOutMale"].ToString());
					array[0]["CheckOutFMale"] = int.Parse(array[0]["CheckOutFMale"].ToString()) + int.Parse(dataRow["CheckOutFMale"].ToString());
					array[0]["SumCheckOutCount"] = int.Parse(array[0]["SumCheckOutCount"].ToString()) + int.Parse(dataRow["SumCheckOutCount"].ToString());
				}
				else
				{
					dataTable.ImportRow(dataRow);
				}
			}
			return dataTable;
		}

		public DataTable MergeSameRowOfAllTeamConclusion(DataTable SourceDT)
		{
			DataTable dataTable = SourceDT.Clone();
			foreach (DataRow dataRow in SourceDT.Rows)
			{
				DataRow[] array = dataTable.Select("TeamConclusionName='" + dataRow["TeamConclusionName"].ToString() + "'");
				if (array.Length <= 0)
				{
					dataTable.ImportRow(dataRow);
				}
			}
			return dataTable;
		}

		public void GetInvoiceSearchWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("ID_Customer"));
				string text2 = base.Server.HtmlDecode(base.GetString("Invoice")).TrimEnd(new char[]
				{
					','
				});
				string text3 = string.Empty;
				string text4 = string.Empty;
				if (!string.IsNullOrEmpty(text2))
				{
					text4 += string.Format(" AND Invoice LIKE '%{0}%'", text2);
				}
				if (!string.IsNullOrEmpty(text))
				{
					text4 += string.Format(" AND ID_Customer='{0}'", text);
					text3 += string.Format(" AND ID_Customer='{0}'", text);
				}
				string text5 = "SELECT OPEI.ID_Customer,CustomerName,GenderName,MarriageName,IDCard,MobileNo,Convert(varchar(10),OperateDate,120)OperateDate,TeamName,ExamTypeName,ExamPlaceName,GuideNurse,DictFeeWay.FeeWayName\r\n,OCF.FeeCharger,Invoice,Convert(varchar(10),OCF.FeeChargeDate,120)FeeChargeDate,'@ExamState' ExamState FROM\r\n(\r\nSELECT ID_Customer,ID_FeeType,FeeCharger,FeeChargeDate FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=1 @OnCustFeeWhere GROUP BY ID_Customer,ID_FeeType,FeeCharger,FeeChargeDate\r\n) OCF \r\nINNER JOIN DictFeeWay ON OCF.ID_FeeType=DictFeeWay.FeeWayID AND FeeWayName='现金'\r\nINNER JOIN (SELECT ID_Customer,CustomerName,GenderName,MarriageName,IDCard,MobileNo,OperateDate,TeamName,ExamTypeName,FeeWayName,ExamPlaceName,Invoice,GuideNurse FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere) OPEI\r\nON OPEI.ID_Customer=OCF.ID_Customer\r\nORDER BY ID_Customer,FeeChargeDate";
				text5 = text5.Replace("@OnCustFeeWhere", text3);
				text5 = text5.Replace("@OnCustPhysicalExamInfoWhere", text4);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text6 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text5.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text5.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取发票统计信息失败(GetInvoiceSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text5.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取发票统计信息失败(GetInvoiceSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text6 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text6 != null)
						{
							if (!string.IsNullOrEmpty(text6))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text6, text5.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text6,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_40D:
						i++;
						continue;
						goto IL_40D;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text7 = "发票查询报表";
				dataTable2.TableName = text7;
				string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text8))
				{
					Directory.CreateDirectory(text8);
				}
				string str = text7 + ".xls";
				string text9 = text8 + "\\" + text7 + ".xls";
				if (File.Exists(text9))
				{
					File.Delete(text9);
				}
				Excel.DataSetToExcel(dataSet5, text9);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string text10 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				text10 = text10.Replace("\\r\\n", "");
				this.OutPutMessage(text10);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		public void GetInvoiceWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("Invoice")).TrimEnd(new char[]
				{
					','
				});
				string text2 = base.Server.HtmlDecode(base.GetString("CustomerName"));
				int @int = base.GetInt("IsTeam", -1);
				int int2 = base.GetInt("IsHasInvoice", -1);
				string text3 = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text4 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text3.Replace("-", string.Empty);
				string str2 = text4.Replace("-", string.Empty);
				string text5 = base.Server.HtmlDecode(base.GetString("CardNo"));
				int int3 = base.GetInt("ISCustomerExamNo", -1);
				int int4 = base.GetInt("IsSearchDate", 0);
				string text6 = string.Empty;
				string text7 = string.Empty;
				string empty = string.Empty;
				if (!string.IsNullOrEmpty(text2))
				{
					text7 += string.Format(" AND CustomerName LIKE '%{0}%'", text2);
				}
				if (int2 != -1)
				{
					text7 += " AND ISNULL(Invoice,'')!=''";
				}
				if (!string.IsNullOrEmpty(text))
				{
					text7 += string.Format(" AND Invoice LIKE '%{0}%'", text);
				}
				if (@int != -1)
				{
					text7 += string.Format(" AND ISNULL(Is_Team,0)='{0}'", @int);
				}
				if (int3 == 1)
				{
					if (!string.IsNullOrEmpty(text5))
					{
						text7 += string.Format(" AND ID_Customer='{0}'", text5);
						text6 += string.Format(" AND ID_Customer='{0}'", text5);
					}
				}
				else
				{
					text7 += string.Format(" AND IDCard LIKE '%{0}%'", text5);
				}
				if (int4 == 1)
				{
					if (!string.IsNullOrEmpty(text3))
					{
						text3 += " 00:00:00";
						text7 += string.Format(" AND SubScribDate>='{0}'", text3);
					}
					if (!string.IsNullOrEmpty(text4))
					{
						text4 += " 23:59:59";
						text7 += string.Format(" AND SubScribDate<='{0}'", text4);
					}
				}
				string text8 = "SELECT OPEI.ID_Customer,CustomerName,Invoice,GenderName,FLOOR(DATEDIFF(DY,BirthDay,(CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END))/365.25)Age,IDCard,MobileNo,TeamName,Convert(varchar(10),OperateDate,120)OperateDate,ExamTypeName,ExamPlaceName,GuideNurse,FeeWayName\r\n,OCF.FeeCharger,Invoice,Convert(varchar(10),OCF.FeeChargeDate,120)FeeChargeDate,'@ExamState' ExamState FROM\r\n(\r\nSELECT ID_Customer,ID_FeeType,FeeCharger,FeeChargeDate FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=1 @OnCustFeeWhere GROUP BY ID_Customer,ID_FeeType,FeeCharger,FeeChargeDate\r\n) OCF \r\nINNER JOIN (SELECT Is_GuideSheetPrinted,ID_Customer,CustomerName,GenderName,MarriageName,IDCard,MobileNo,BirthDay,OperateDate,TeamName,ExamTypeName,FeeWayName,ExamPlaceName,Invoice,GuideNurse FROM OnCustPhysicalExamInfo WHERE 1=1 @OnCustPhysicalExamInfoWhere) OPEI\r\nON OPEI.ID_Customer=OCF.ID_Customer\r\nORDER BY ID_Customer,FeeChargeDate";
				text8 = text8.Replace("@OnCustFeeWhere", text6);
				text8 = text8.Replace("@OnCustPhysicalExamInfoWhere", text7);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text9 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text8.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text8.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取发票查询信息失败(GetInvoiceWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text8.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取发票查询信息失败(GetInvoiceWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text9 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text9 != null)
						{
							if (!string.IsNullOrEmpty(text9))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text9, text8.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询发票信息失败，失败原因为从",
										text9,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_5C3:
						i++;
						continue;
						goto IL_5C3;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				DataSet dataSet5 = dataSet;
				string text10 = "发票统计";
				dataTable2.TableName = str + "-" + str2 + text10;
				string text11 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text11))
				{
					Directory.CreateDirectory(text11);
				}
				string str3 = text10 + ".xls";
				string text12 = text11 + "\\" + text10 + ".xls";
				if (File.Exists(text12))
				{
					File.Delete(text12);
				}
				Excel.DataSetToExcel(dataSet5, text12);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string text13 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				text13 = text13.Replace("\\r\\n", "").Replace(" ", "");
				this.OutPutMessage(text13);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		public void GetCloneCustFee()
		{
			DataSet dataSet = null;
			DataTable dataTable = null;
			DataSet dataSet2 = null;
			DataTable dataTable2 = null;
			ArrayList arrayList = null;
			string text = base.GetString("ID_Customer").Trim();
			string text2 = string.Empty;
			string empty = string.Empty;
			text2 = string.Format("SELECT DISTINCT ISNULL(BusFee.Is_Banned,0) Is_Banned,OnCustFee.ID_CustFee,'@ExamState' ExamState,-1 CustFeeChargeState,BusSetFeeDetail.PEPackageID,'@userName' userName,'@date' date,BusFee.ID_Section,BusFee.SectionName,BusFee.ID_Fee,BusFee.FeeName,BusFee.InputCode,CONVERT(decimal(18,2),BusFee.Price) Price,@Discount Discount,CONVERT(decimal(18,2),BusFee.Price*@Discount/100) FactPrice,Forsex\r\nFROM (SELECT ID_CustFee,ID_Fee FROM OnCustFee WHERE ID_Customer='{0}') OnCustFee\r\nINNER JOIN BusFee ON OnCustFee.ID_Fee=BusFee.ID_Fee\r\nINNER JOIN SYSSection on BusFee.ID_Section=SYSSection.ID_Section \r\nLEFT JOIN BusSetFeeDetail on BusFee.ID_Fee=BusSetFeeDetail.ID_DtlSetFee\r\nORDER BY OnCustFee.ID_CustFee", text);
			text2 = text2.Replace("@where", empty);
			text2 = text2.Replace("@userName", this.UserName);
			text2 = text2.Replace("@date", DateTime.Now.ToString("yyyy-MM-dd"));
			text2 = text2.Replace("@Discount", this.Discount.ToString());
			string text3 = string.Empty;
			int num = 0;
			int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
			DataSet dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text2.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
			DataTable dataTable3 = dataSet3.Tables[0];
			if (dataTable3.Rows.Count > 0)
			{
				this.OutPutMessage(JsonHelperFont.Instance.DataTableToJSON(dataTable3, "dataList"));
				dataTable3.Dispose();
				dataSet3.Dispose();
			}
			else
			{
				try
				{
					dataSet = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text2.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
					dataTable = dataSet.Tables[0];
				}
				catch (Exception ex)
				{
					Log4J.Instance.Error(string.Concat(new string[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",从离线库中克隆客户体检信息失败(GetCloneCustFee)：",
						ex.Message,
						" ",
						Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
					}));
				}
				if (dataTable.Rows.Count > 0)
				{
					this.OutPutMessage(JsonHelperFont.Instance.DataTableToJSON(dataTable, "dataList"));
					dataTable.Dispose();
					dataTable = null;
					dataSet.Dispose();
					dataSet = null;
				}
				else
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text2.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable2 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中克隆客户体检信息失败(GetCloneCustFee)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
					if (dataTable2 != null)
					{
						if (dataTable2.Rows.Count > 0)
						{
							this.OutPutMessage(JsonHelperFont.Instance.DataTableToJSON(dataTable2, "dataList"));
							dataTable2.Dispose();
							dataTable2 = null;
							dataSet2.Dispose();
							dataSet2 = null;
							return;
						}
					}
					if (num > 0)
					{
						arrayList = new ArrayList(num);
						int i = 1;
						while (i <= num)
						{
							text3 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text3 != null)
							{
								try
								{
									DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(text3, text2.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet4.Tables[0]);
									dataSet4.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询体检号[",
										text,
										"]收费项目明细信息失败，失败原因为从",
										text3,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
							IL_466:
							i++;
							continue;
							goto IL_466;
						}
					}
					if (arrayList != null)
					{
						foreach (DataTable dataTable4 in arrayList)
						{
							if (dataTable4 != null)
							{
								if (dataTable4.Rows.Count > 0)
								{
									this.OutPutMessage(JsonHelperFont.Instance.DataTableToJSON(dataTable4, "dataList"));
									break;
								}
							}
						}
					}
					if (arrayList != null)
					{
						arrayList.Clear();
						arrayList = null;
					}
				}
			}
		}

		public void GetCustFeeDetail()
		{
			string text = base.GetString("ID_Customer").Trim();
			if (!string.IsNullOrEmpty(text))
			{
				string sql = string.Format("SELECT ISNULL(ExamState,0)ExamState,ID_Customer,ID_ArcCustomer,IDCardNo,Is_CompletePhysical FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}'", text);
				string sql2 = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY SectionDispOrder,BusFeeDispOrder ASC) FeeItemNum,SectionName,OCF.FeeItemName\r\n,OCF.FeeChargeStaute,CASE OCF.FeeChargeStaute WHEN '已退' THEN -OCF.FactPrice ELSE OCF.FactPrice END FactPrice\r\n FROM (SELECT BF.ID_Section,BF.SectionName,NS.DispOrder SectionDispOrder,BF.DispOrder BusFeeDispOrder,OCF.FeeItemName,CASE WHEN OCF.Is_FeeRefund=1 THEN '已退' WHEN OCF.Is_FeeCharged=1 THEN '已收' ELSE '未收' END FeeChargeStaute,OCF.FactPrice,OCF.ID_CustFee FROM\r\n(SELECT * FROM OnCustFee WHERE Is_FeeCharged=1 AND ISNULL(Is_FeeRefund,0)=0 AND ID_Customer='{0}') OCF\r\nINNER JOIN BusFee BF ON OCF.ID_Fee=BF.ID_Fee\r\nINNER JOIN SYSSection NS ON BF.ID_SECTION=NS.ID_Section)OCF", text);
				DataSet dataSet = PEIS.BLL.CommonStatistics.Instance.Query(sql, new int?(this.CommandTimeout));
				if (dataSet.Tables[0].Rows.Count != 0)
				{
					string currConnectionString = string.Empty;
					string configName = string.Empty;
					int num = int.Parse(dataSet.Tables[0].Rows[0]["ExamState"].ToString());
					if (num == 0)
					{
						configName = "ConnectionString";
					}
					else if (num == 1)
					{
						configName = "ToOffLineConnectionString";
					}
					else if (num == -1)
					{
						configName = "FYH_Waste";
					}
					else if (num > 1)
					{
						configName = "FYH_HisFile" + (num - 1).ToString().PadLeft(3, '0');
					}
					currConnectionString = PubConstant.GetConnectionString(configName);
					try
					{
						dataSet = PEIS.BLL.CommonStatistics.Instance.Query(currConnectionString, sql2, new int?(this.CommandTimeout));
						DataTable dataTable = dataSet.Tables[0];
						string text2 = "收费项目明细";
						dataTable.TableName = text2;
						string text3 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
						if (!Directory.Exists(text3))
						{
							Directory.CreateDirectory(text3);
						}
						string str = text2 + ".xls";
						string text4 = text3 + "\\" + text2 + ".xls";
						if (File.Exists(text4))
						{
							File.Delete(text4);
						}
						Excel.DataSetToExcel(dataSet, text4);
						DataTable dataTable2 = new DataTable();
						dataTable2.Columns.Add("FilePath");
						DataRow dataRow = dataTable2.NewRow();
						dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str;
						dataTable2.Rows.Add(dataRow);
						dataSet.Tables.Add(dataTable2);
						string text5 = JsonHelperFont.Instance.DataSetToJSON(dataSet);
						text5 = text5.Replace("\\r\\n", "").Replace(" ", "");
						this.OutPutMessage(text5);
					}
					catch (Exception ex)
					{
						Log4J.Instance.Info("--------从在线库中获取体检号[" + text + "]对应的收费项目明细失败。Error：" + ex.Message);
					}
				}
			}
		}

		public void GetRegisteWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Register", -1);
				if (@int != -1)
				{
					string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
					string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
					string str = text.Replace("-", string.Empty);
					string str2 = text2.Replace("-", string.Empty);
					if (!string.IsNullOrEmpty(text))
					{
						text += " 00:00:00";
					}
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " 23:59:59";
					}
					string newValue = string.Empty;
					newValue = string.Format(" AND (FeeChargeDate BETWEEN '{0}' AND '{1}')", text, text2);
					string text3 = "SELECT '@ExamState' ExamState,RegisterName,FeeItemName,COUNT(0) RegistCount,SUM(FactPrice) SumFactPrice FROM \r\n(SELECT RegisterName,FeeItemName,FactPrice,ID_Register,RegistDate FROM OnCustFee WHERE ID_FeeType IN (1,2) AND ISNULL(Is_FeeRefund,0)!=1 @OnCustFeeWhere)OnCustFee GROUP BY RegisterName,FeeItemName ORDER BY FeeItemName ";
					text3 = text3.Replace("@ID_Register", @int.ToString());
					text3 = text3.Replace("@OnCustFeeWhere", newValue);
					DataTable dataTable = null;
					DataSet dataSet = null;
					DataTable dataTable2 = null;
					DataTable dataTable3 = null;
					DataSet dataSet2 = null;
					DataTable dataTable4 = null;
					ArrayList arrayList = null;
					DataSet dataSet3 = null;
					string text4 = string.Empty;
					int num = 0;
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
					dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
					dataTable2 = dataSet.Tables[0];
					if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
					{
						try
						{
							DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
							dataTable3 = dataSet4.Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取登记工作量信息失败(GetRegisteWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (!string.IsNullOrEmpty(this.toWasteConnectionString))
					{
						try
						{
							dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
							dataTable4 = dataSet2.Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从废弃库中获取登记工作量信息失败(GetRegisteWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (num > 0)
					{
						arrayList = new ArrayList(num);
						int i = 1;
						while (i <= num)
						{
							text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text4 != null)
							{
								if (!string.IsNullOrEmpty(text4))
								{
									try
									{
										dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
										arrayList.Add(dataSet3.Tables[0]);
										dataSet3.Dispose();
									}
									catch (Exception ex)
									{
										Log4J.Instance.Error(string.Concat(new string[]
										{
											Public.GetClientIP(),
											",",
											this.LoginUserModel.UserName,
											",查询个人登记统计信息失败，失败原因为从",
											text4,
											"获取数据出现如下错误：",
											ex.Message
										}));
									}
								}
							}
							IL_42D:
							i++;
							continue;
							goto IL_42D;
						}
					}
					if (dataTable3 != null)
					{
						dataTable2.Merge(dataTable3);
					}
					if (dataTable4 != null)
					{
						dataTable2.Merge(dataTable4);
					}
					if (arrayList != null)
					{
						foreach (DataTable dataTable5 in arrayList)
						{
							if (dataTable5 != null)
							{
								dataTable2.Merge(dataTable5);
							}
						}
					}
					dataTable2 = this.MergeAllRegiste(dataTable2);
					dataSet.Tables.Clear();
					dataSet.Tables.Add(dataTable2);
					DataSet dataSet5 = dataSet;
					string text5 = "个人登记统计";
					dataTable2.TableName = str + "-" + str2 + text5;
					string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text6))
					{
						Directory.CreateDirectory(text6);
					}
					string str3 = text5 + ".xls";
					string text7 = text6 + "\\" + text5 + ".xls";
					if (File.Exists(text7))
					{
						File.Delete(text7);
					}
					Excel.DataSetToExcel(dataSet5, text7);
					DataTable dataTable6 = new DataTable();
					dataTable6.Columns.Add("FilePath");
					DataRow dataRow = dataTable6.NewRow();
					dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
					dataTable6.Rows.Add(dataRow);
					dataSet5.Tables.Add(dataTable6);
					string text8 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
					text8 = text8.Replace("\\r\\n", "").Replace(" ", "");
					this.OutPutMessage(text8);
					if (dataSet3 != null)
					{
						dataSet3.Dispose();
						dataSet3 = null;
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
						dataTable3 = null;
					}
					if (dataSet != null)
					{
						dataSet.Dispose();
						dataSet = null;
					}
					if (dataTable2 != null)
					{
						dataTable2.Dispose();
						dataTable2 = null;
					}
					if (dataSet != null)
					{
						dataSet.Dispose();
						dataSet = null;
					}
					if (arrayList != null)
					{
						arrayList.Clear();
						arrayList = null;
					}
					if (dataTable != null)
					{
						dataTable.Clear();
						dataTable = null;
					}
					if (dataSet5 != null)
					{
						dataSet5.Dispose();
					}
					if (dataTable4 != null)
					{
						dataTable4.Dispose();
						dataTable4 = null;
					}
					if (dataSet2 != null)
					{
						dataSet2.Dispose();
						dataSet2 = null;
					}
				}
			}
		}

		private DataTable MergeAllRegiste(DataTable SourceDT)
		{
			DataTable result;
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					DataTable dataTable = SourceDT.Clone();
					string empty = string.Empty;
					string empty2 = string.Empty;
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = dataTable.Select(string.Concat(new object[]
						{
							"RegisterName='",
							dataRow["RegisterName"],
							"' AND FeeItemName='",
							dataRow["FeeItemName"],
							"'"
						}));
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow2.BeginEdit();
								dataRow2["RegistCount"] = int.Parse(dataRow["RegistCount"].ToString()) + int.Parse(dataRow2["RegistCount"].ToString());
								dataRow2["SumFactPrice"] = (decimal.Parse(dataRow["SumFactPrice"].ToString()) + decimal.Parse(dataRow2["SumFactPrice"].ToString())).ToString("f2");
								dataRow2.EndEdit();
							}
						}
						else
						{
							dataRow.BeginEdit();
							dataRow["SumFactPrice"] = decimal.Parse(dataRow["SumFactPrice"].ToString()).ToString("f2");
							dataRow.EndEdit();
							dataTable.ImportRow(dataRow);
						}
					}
					result = dataTable;
					return result;
				}
			}
			result = SourceDT;
			return result;
		}

		public void GetCustFeeOfDayWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("IsMonth", 0);
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
					string str = text.Replace("-", string.Empty);
					string str2 = text2.Replace("-", string.Empty);
					if (!string.IsNullOrEmpty(text))
					{
						text += " 00:00:00";
					}
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " 23:59:59";
					}
					string newValue = string.Empty;
					newValue = string.Format(" AND (FeeChargeDate BETWEEN '{0}' AND '{1}')", text, text2);
					string text3 = "SELECT OPEI.ExamPlaceName,OAC.ID_ArcCustomer,OPEI.ID_Customer,OAC.CustomerName,OAC.GenderName,DATEDIFF(YEAR,OAC.BirthDay,GETDATE()) Age\r\n,OPEI.FeeWayName,OPEI.TeamName,OCF.FeeCharger,CASE WHEN ISNULL(OCF.FeeCharger,'')!='' THEN '已收' ELSE '未收' END FeeChargeState,OCF.FeeChargeDate,OCF.SumFactPrice,'@ExamState' ExamState FROM\r\n(\r\nSELECT ID_Customer,FeeCharger,SUM(FACTPRICE) SumFactPrice,FeeChargeDate FROM OnCustFee WHERE ISNULL(Is_FeeCharged,0)=1 AND ISNULL(Is_FeeRefund,0)!=1 @OnCustFeeWhere GROUP BY ID_Customer,FeeCharger,FeeChargeDate\r\n) OCF \r\nINNER JOIN OnCustPhysicalExamInfo OPEI\r\nON OPEI.ID_Customer=OCF.ID_Customer\r\nINNER JOIN OnCustRelationCustPEInfo OCPE ON OPEI.ID_Customer=OCPE.ID_Customer\r\nINNER JOIN OnArcCust OAC ON OCPE.ID_ArcCustomer=OAC.ID_ArcCustomer\r\nORDER BY OCF.ID_Customer,FeeCharger";
					text3 = text3.Replace("@OnCustFeeWhere", newValue);
					DataTable dataTable = null;
					DataSet dataSet = null;
					DataTable dataTable2 = null;
					DataTable dataTable3 = null;
					DataSet dataSet2 = null;
					DataTable dataTable4 = null;
					ArrayList arrayList = null;
					DataSet dataSet3 = null;
					string text4 = string.Empty;
					int num = 0;
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
					dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
					dataTable2 = dataSet.Tables[0];
					if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
					{
						try
						{
							DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
							dataTable3 = dataSet4.Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取收费日报信息失败(GetCustFeeOfDayWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (!string.IsNullOrEmpty(this.toWasteConnectionString))
					{
						try
						{
							dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
							dataTable4 = dataSet2.Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从废弃库中获取收费日报信息失败(GetCustFeeOfDayWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (num > 0)
					{
						arrayList = new ArrayList(num);
						int i = 1;
						while (i <= num)
						{
							text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text4 != null)
							{
								if (!string.IsNullOrEmpty(text4))
								{
									try
									{
										dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
										arrayList.Add(dataSet3.Tables[0]);
										dataSet3.Dispose();
									}
									catch (Exception ex)
									{
										Log4J.Instance.Error(string.Concat(new string[]
										{
											Public.GetClientIP(),
											",",
											this.LoginUserModel.UserName,
											",查询个人登记统计信息失败，失败原因为从",
											text4,
											"获取数据出现如下错误：",
											ex.Message
										}));
									}
								}
							}
							IL_41A:
							i++;
							continue;
							goto IL_41A;
						}
					}
					if (dataTable3 != null)
					{
						dataTable2.Merge(dataTable3);
					}
					if (dataTable4 != null)
					{
						dataTable2.Merge(dataTable4);
					}
					if (arrayList != null)
					{
						foreach (DataTable dataTable5 in arrayList)
						{
							if (dataTable5 != null)
							{
								dataTable2.Merge(dataTable5);
							}
						}
					}
					dataSet.Tables.Clear();
					dataSet.Tables.Add(dataTable2);
					DataSet dataSet5 = dataSet;
					string text5 = "收费日报统计";
					dataTable2.TableName = str + "-" + str2 + text5;
					string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text6))
					{
						Directory.CreateDirectory(text6);
					}
					string str3 = text5 + ".xls";
					string text7 = text6 + "\\" + text5 + ".xls";
					if (File.Exists(text7))
					{
						File.Delete(text7);
					}
					Excel.DataSetToExcel(dataSet5, text7);
					DataTable dataTable6 = new DataTable();
					dataTable6.Columns.Add("FilePath");
					DataRow dataRow = dataTable6.NewRow();
					dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
					dataTable6.Rows.Add(dataRow);
					dataSet5.Tables.Add(dataTable6);
					string text8 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
					text8 = text8.Replace("\\r\\n", "");
					this.OutPutMessage(text8);
					if (dataSet3 != null)
					{
						dataSet3.Dispose();
						dataSet3 = null;
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
						dataTable3 = null;
					}
					if (dataSet != null)
					{
						dataSet.Dispose();
						dataSet = null;
					}
					if (dataTable2 != null)
					{
						dataTable2.Dispose();
						dataTable2 = null;
					}
					if (dataSet != null)
					{
						dataSet.Dispose();
						dataSet = null;
					}
					if (arrayList != null)
					{
						arrayList.Clear();
						arrayList = null;
					}
					if (dataTable != null)
					{
						dataTable.Clear();
						dataTable = null;
					}
					if (dataSet5 != null)
					{
						dataSet5.Dispose();
					}
					if (dataTable4 != null)
					{
						dataTable4.Dispose();
						dataTable4 = null;
					}
					if (dataSet2 != null)
					{
						dataSet2.Dispose();
						dataSet2 = null;
					}
				}
			}
		}

		public void GetBusSetWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string newValue = string.Empty;
				newValue = string.Format(" AND (SubScribDate BETWEEN '{0}' AND '{1}')", text, text2);
				string text3 = "SELECT '@ExamState' ExamState,PEPackageID,PEPackageName,COUNT(0) SetCount FROM OnCustPhysicalExamInfo\r\nWHERE Is_GuideSheetPrinted=1 AND PEPackageID IS NOT NULL @OnCustPhysicalExamInfoWhere GROUP BY PEPackageID,PEPackageName ORDER BY PEPackageID";
				text3 = text3.Replace("@OnCustPhysicalExamInfoWhere", newValue);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取套餐使用统计信息失败(GetBusSetWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取套餐使用统计信息失败(GetBusSetWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_3F7:
						i++;
						continue;
						goto IL_3F7;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataTable2 = this.MergeAllBusSet(dataTable2);
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "套餐使用统计";
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string text8 = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				text8 = text8.Replace("\\r\\n", "").Replace(" ", "");
				this.OutPutMessage(text8);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		private DataTable MergeAllBusSet(DataTable SourceDT)
		{
			DataTable result;
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					DataTable dataTable = SourceDT.Clone();
					string empty = string.Empty;
					string empty2 = string.Empty;
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = dataTable.Select("PEPackageID='" + dataRow["PEPackageID"] + "'");
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow2.BeginEdit();
								dataRow2["SetCount"] = int.Parse(dataRow["SetCount"].ToString()) + int.Parse(dataRow2["SetCount"].ToString());
								dataRow2.EndEdit();
							}
						}
						else
						{
							dataTable.ImportRow(dataRow);
						}
					}
					result = dataTable;
					return result;
				}
			}
			result = SourceDT;
			return result;
		}

		public void GetSectionOncustFeeWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Section", -1);
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Format("SELECT '@ExamState' ExamState,OnCustFee.ID_Customer,CustomerName,ID_Fee,FeeItemName,SectionName,ExamDoctorName,ExamDate FROM OnCustFee\r\nINNER JOIN (SELECT ID_Customer,SectionName,CustomerName FROM OnCustExamSection WHERE ISNULL(IS_giveup,0)=0 AND Is_Check=1 AND (SectionSummaryDate>='{0}' AND SectionSummaryDate<='{1}') @WHERE)OnCustExamSection ON OnCustFee.ID_Customer=OnCustExamSection.ID_Customer\r\nWHERE EXISTS(SELECT ID_Fee FROM BusFee WHERE ID_Section='{2}' AND OnCustFee.ID_Fee=BusFee.ID_Fee)", text, text2, @int);
				if (@int > -1)
				{
					text3 = text3.Replace("@WHERE", " AND ID_Section='" + @int + "'");
					DataTable dataTable = null;
					DataSet dataSet = null;
					DataTable dataTable2 = null;
					DataTable dataTable3 = null;
					DataSet dataSet2 = null;
					DataTable dataTable4 = null;
					ArrayList arrayList = null;
					DataSet dataSet3 = null;
					string text4 = string.Empty;
					int num = 0;
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
					dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
					dataTable2 = dataSet.Tables[0];
					if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
					{
						try
						{
							DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
							dataTable3 = dataSet4.Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从离线库中获取结算报表统计信息失败(GetSectionOncustFeeWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (!string.IsNullOrEmpty(this.toWasteConnectionString))
					{
						try
						{
							dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
							dataTable4 = dataSet2.Tables[0];
						}
						catch (Exception ex)
						{
							Log4J.Instance.Error(string.Concat(new string[]
							{
								Public.GetClientIP(),
								",",
								this.LoginUserModel.UserName,
								",从废弃库中获取结算报表统计信息失败(GetSectionOncustFeeWorkLoad)：",
								ex.Message,
								" ",
								Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
							}));
						}
					}
					if (num > 0)
					{
						arrayList = new ArrayList(num);
						int i = 1;
						while (i <= num)
						{
							text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
							if (text4 != null)
							{
								if (!string.IsNullOrEmpty(text4))
								{
									try
									{
										dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
										arrayList.Add(dataSet3.Tables[0]);
										dataSet3.Dispose();
									}
									catch (Exception ex)
									{
										Log4J.Instance.Error(string.Concat(new string[]
										{
											Public.GetClientIP(),
											",",
											this.LoginUserModel.UserName,
											",查询个人登记统计信息失败，失败原因为从",
											text4,
											"获取数据出现如下错误：",
											ex.Message
										}));
									}
								}
							}
							IL_427:
							i++;
							continue;
							goto IL_427;
						}
					}
					if (dataTable3 != null)
					{
						dataTable2.Merge(dataTable3);
					}
					if (dataTable4 != null)
					{
						dataTable2.Merge(dataTable4);
					}
					if (arrayList != null)
					{
						foreach (DataTable dataTable5 in arrayList)
						{
							if (dataTable5 != null)
							{
								dataTable2.Merge(dataTable5);
							}
						}
					}
					dataSet.Tables.Clear();
					dataSet.Tables.Add(dataTable2);
					DataSet dataSet5 = dataSet;
					string text5 = "结算报表统计";
					dataTable2.TableName = str + "-" + str2 + text5;
					string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text6))
					{
						Directory.CreateDirectory(text6);
					}
					string str3 = text5 + ".xls";
					string text7 = text6 + "\\" + text5 + ".xls";
					if (File.Exists(text7))
					{
						File.Delete(text7);
					}
					Excel.DataSetToExcel(dataSet5, text7);
					DataTable dataTable6 = new DataTable();
					dataTable6.Columns.Add("FilePath");
					DataRow dataRow = dataTable6.NewRow();
					dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
					dataTable6.Rows.Add(dataRow);
					dataSet5.Tables.Add(dataTable6);
					string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
					this.OutPutMessage(msg);
					if (dataSet3 != null)
					{
						dataSet3.Dispose();
						dataSet3 = null;
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
						dataTable3 = null;
					}
					if (dataSet != null)
					{
						dataSet.Dispose();
						dataSet = null;
					}
					if (dataTable2 != null)
					{
						dataTable2.Dispose();
						dataTable2 = null;
					}
					if (dataSet != null)
					{
						dataSet.Dispose();
						dataSet = null;
					}
					if (arrayList != null)
					{
						arrayList.Clear();
						arrayList = null;
					}
					if (dataTable != null)
					{
						dataTable.Clear();
						dataTable = null;
					}
					if (dataSet5 != null)
					{
						dataSet5.Dispose();
					}
					if (dataTable4 != null)
					{
						dataTable4.Dispose();
						dataTable4 = null;
					}
					if (dataSet2 != null)
					{
						dataSet2.Dispose();
						dataSet2 = null;
					}
				}
			}
		}

		public void GetDiscountSearchWorkLoad()
		{
			lock ("1")
			{
				this.BeginDate = DateTime.Now;
				int @int = base.GetInt("flag", -1);
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				if (@int == 0)
				{
					text3 = string.Format("SELECT OCPE.ExamPlaceName,OCF.ID_Customer,FeeItemName,RegisterName,FeeChargeDate,OriginalPrice,Discount,FactPrice,DiscounterName,FeeWayName,'@ExamState' ExamState FROM\r\n(SELECT * FROM OnCustFee WHERE ISNULL(Discount,0)<10 AND ISNULL(Is_FeeCharged,0)=1 AND ISNULL(Is_FeeRefund,0)=0 AND (FeeChargeDate BETWEEN '{0}' AND '{1}')) OCF\r\nINNER JOIN OnCustPhysicalExamInfo OCPE ON OCF.ID_Customer=OCPE.ID_Customer\r\nORDER BY OCF.ID_Customer,FeeChargeDate", text, text2);
				}
				else if (@int == 1)
				{
					text3 = string.Format("SELECT OCPE.ExamPlaceName,OCF.ID_Customer,FeeItemName,RegisterName,FeeChargeDate,OriginalPrice,Discount,FactPrice,DiscounterName,FW.FeeWayName,'@ExamState' ExamState FROM(SELECT ID_FeeType,ID_Customer,FeeItemName,RegisterName,FeeChargeDate,OriginalPrice,Discount,FactPrice,DiscounterName FROM OnCustFee WHERE \r\nISNULL(Is_FeeCharged,0)=1 AND ISNULL(Is_FeeRefund,0)=0 AND (FeeChargeDate BETWEEN '{0}' AND '{1}')) OCF\r\nINNER JOIN (SELECT ID_FeeWay,FeeWayName FROM DictFeeWay WHERE FeeWayName='记账') FW ON OCF.ID_FeeType=FW.ID_FeeWay\r\nINNER JOIN OnCustPhysicalExamInfo OCPE ON OCF.ID_Customer=OCPE.ID_Customer\r\nORDER BY ID_Customer,FeeChargeDate", text, text2);
				}
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取折扣/记账明细信息失败(GetDiscountSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取折扣/记账明细信息失败(GetDiscountSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_427:
						i++;
						continue;
						goto IL_427;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				Log4J.Instance.Info(string.Concat(new string[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",记账查询/折扣查询,耗时:",
					Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
				}));
				string text5 = string.Empty;
				if (@int == 0)
				{
					text5 = "折扣查询";
				}
				else if (@int == 1)
				{
					text5 = "记账明细";
				}
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
				Log4J.Instance.Info(string.Concat(new string[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",记账查询/折扣查询,耗时:",
					Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
				}));
			}
		}

		public void GetGuideNurseSearchWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				text3 = string.Format("SELECT B.GuideNurse AS GuideNurse,COUNT(distinct(b.ID_Customer))AS GuideNum,SUM(A.FactPrice)AS GuidePrice\r\nFROM OnCustFee A,OnCustPhysicalExamInfo B\r\nWHERE A.ID_Customer=B.ID_Customer AND isnull(A.Is_FeeRefund ,0)!=1 \r\n---AND b.Is_GuideSheetPrinted=1 \r\n---AND  b.GuideSheetReturnedDate  between '{0}' AND '{1}' \r\nAND  a.ID_Customer in(SELECT ID_Customer FROM OnCustPhysicalExamInfo\r\nwhere  Is_GuideSheetPrinted=1 AND GuideSheetReturnedDate BETWEEN '{0}' AND '{1}'  AND GuideNurse is not null)\r\ngroup by GuideNurse\r\nORDER BY GuideNurse", text, text2);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取导检信息失败(GetGuideNurseSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取导检信息失败(GetGuideNurseSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_3E0:
						i++;
						continue;
						goto IL_3E0;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataTable2 = this.MergeAllGuideNurse(dataTable2);
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "导检查询";
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		private DataTable MergeAllGuideNurse(DataTable SourceDT)
		{
			DataTable result;
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					DataTable dataTable = SourceDT.Clone();
					string empty = string.Empty;
					string empty2 = string.Empty;
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = dataTable.Select("GuideNurse='" + dataRow["GuideNurse"] + "'");
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow2.BeginEdit();
								dataRow2["GuideNum"] = int.Parse(dataRow["GuideNum"].ToString()) + int.Parse(dataRow2["GuideNum"].ToString());
								dataRow2.EndEdit();
							}
						}
						else
						{
							dataTable.ImportRow(dataRow);
						}
					}
					result = dataTable;
					return result;
				}
			}
			result = SourceDT;
			return result;
		}

		public void GetGuideSheetReturnedWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				text3 = string.Format("SELECT GuideSheetReturnedby,COUNT(0)GuideSheetReturnedNum,TeamName,'@ExamState' ExamState FROM OnCustPhysicalExamInfo\r\n                    WHERE ISNULL(GuideSheetReturnedby,'')!='' AND (GuideSheetReturnedDate BETWEEN '{0}' AND '{1}') \r\n                    GROUP BY GuideSheetReturnedby,TeamName\r\n                    ORDER BY GuideSheetReturnedby", text, text2);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取指引单回收统计信息失败(GetGuideSheetReturnedWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取指引单回收统计信息失败(GetGuideSheetReturnedWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_3E0:
						i++;
						continue;
						goto IL_3E0;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataTable2 = this.MergeAllGuideSheetReturnedWorkLoad(dataTable2);
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "指引单回收统计";
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		private DataTable MergeAllGuideSheetReturnedWorkLoad(DataTable SourceDT)
		{
			DataTable result;
			if (SourceDT != null)
			{
				if (SourceDT.Rows.Count > 0)
				{
					DataTable dataTable = SourceDT.Clone();
					string empty = string.Empty;
					string empty2 = string.Empty;
					foreach (DataRow dataRow in SourceDT.Rows)
					{
						DataRow[] array = dataTable.Select(string.Concat(new object[]
						{
							"GuideSheetReturnedby='",
							dataRow["GuideSheetReturnedby"],
							"' AND TeamName='",
							dataRow["TeamName"],
							"'"
						}));
						if (array.Length > 0)
						{
							DataRow[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								DataRow dataRow2 = array2[i];
								dataRow2.BeginEdit();
								dataRow2["GuideSheetReturnedNum"] = int.Parse(dataRow["GuideSheetReturnedNum"].ToString()) + int.Parse(dataRow2["GuideSheetReturnedNum"].ToString());
								dataRow2.EndEdit();
							}
						}
						else
						{
							dataTable.ImportRow(dataRow);
						}
					}
					result = dataTable;
					return result;
				}
			}
			result = SourceDT;
			return result;
		}

		public void GetGuideSheetReturnedSearchWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				text3 = string.Format("SELECT ID_Customer,GuideSheetReturnedby,TeamName,GuideSheetReturnedDate,'@ExamState' ExamState FROM OnCustPhysicalExamInfo WHERE ISNULL(GuideSheetReturnedby,'')!='' AND (GuideSheetReturnedDate BETWEEN '{0}' AND '{1}')  \r\n                    ORDER BY GuideSheetReturnedDate", text, text2);
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取指引单回收明细信息失败(GetGuideSheetReturnedSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取指引单回收明细信息失败(GetGuideSheetReturnedSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_3E0:
						i++;
						continue;
						goto IL_3E0;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "指引单回收明细";
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		public void GetCustomerOfEveryDayWorkLoad()
		{
			lock ("1")
			{
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				text3 = string.Format("SELECT ID_Customer,SUM(FactPrice) SumFactPrice INTO #OCPE@RandNum FROM OnCustFee WHERE ISNULL(Is_FeeRefund ,0)!=1 AND Is_FeeCharged=1 AND FeeChargeDate BETWEEN '{0}' AND '{1}' GROUP BY ID_Customer,ID_FeeType\r\nSELECT OCPE.ExamPlaceName,OCPE.ID_Customer,CustomerName,GenderName,FLOOR(DATEDIFF(DY,BirthDay,(CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END))/365.25)Age,GuideNurse,TeamName,FeeWayName,SumFactPrice,OperateDate FROM OnCustPhysicalExamInfo OCPE\r\nINNER JOIN #OCPE@RandNum OCF ON OCPE.ID_Customer=OCF.ID_Customer\r\nORDER BY OperateDate\r\nDROP TABLE #OCPE@RandNum;", text, text2);
				text3 = text3.Replace("@RandNum", Public.GetGuid("-", string.Empty));
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取每日体检客户分类信息失败(GetCustomerOfEveryDayWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取每日体检客户分类信息失败(GetCustomerOfEveryDayWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_3FD:
						i++;
						continue;
						goto IL_3FD;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "每日体检客户分类";
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		public void GetSectionSearchWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("ID_Section", -1);
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				if (@int > -1)
				{
					text3 = string.Format("SELECT [ID_CustExamSection],[ID_Customer],[ID_Section],SectionName,SectionSummaryDate,SummaryDoctorName,TypistName,TypistDate,CheckDate,SectionSummary \r\n        INTO #OnCustExamSection@RandNum FROM [OnCustExamSection] WHERE (SectionSummaryDate BETWEEN '{0}' AND '{1}') AND ISNULL(IS_Refund,0) = 0 AND ISNULL([IS_giveup],0) = 0 AND ID_Section='{2}' \r\n        SELECT TEMP.ID_Customer,OnArcCust.CustomerName,OnArcCust.GenderName,DATEDIFF(YEAR,OnArcCust.BirthDay,GETDATE()) Age,SectionName,SectionSummaryDate,SummaryDoctorName,TypistName,TypistDate,CheckDate,SectionSummary FROM #OnCustExamSection@RandNum TEMP\r\n        LEFT JOIN (\r\n        SELECT ID_ArcCustomer,ID_Customer FROM OnCustRelationCustPEInfo WHERE ID_Customer IN \r\n        (\r\n        SELECT DISTINCT ID_Customer FROM #OnCustExamSection@RandNum\r\n        ) ) OCRCPEI ON OCRCPEI.ID_Customer = TEMP.ID_Customer\r\n        LEFT JOIN OnArcCust ON OnArcCust.ID_ArcCustomer = OCRCPEI.ID_ArcCustomer\r\n        DROP TABLE #OnCustExamSection@RandNum;", text, text2, @int);
				}
				else
				{
					text3 = string.Format("SELECT [ID_CustExamSection],[ID_Customer],[ID_Section],SectionName,SectionSummaryDate,SummaryDoctorName,TypistName,TypistDate,CheckDate,SectionSummary \r\nINTO #OnCustExamSection@RandNum FROM [OnCustExamSection] WHERE (SectionSummaryDate BETWEEN '{0}' AND '{1}') AND ISNULL(IS_Refund,0) = 0 AND ISNULL([IS_giveup],0) = 0\r\nSELECT TEMP.ID_Customer,OnArcCust.CustomerName,OnArcCust.GenderName,DATEDIFF(YEAR,OnArcCust.BirthDay,GETDATE()) Age,SectionName,SectionSummaryDate,SummaryDoctorName,TypistName,TypistDate,CheckDate,SectionSummary FROM #OnCustExamSection@RandNum TEMP\r\nLEFT JOIN (\r\nSELECT ID_ArcCustomer,ID_Customer FROM OnCustRelationCustPEInfo WHERE ID_Customer IN \r\n(\r\nSELECT DISTINCT ID_Customer FROM #OnCustExamSection@RandNum\r\n) ) OCRCPEI ON OCRCPEI.ID_Customer = TEMP.ID_Customer\r\nLEFT JOIN OnArcCust ON OnArcCust.ID_ArcCustomer = OCRCPEI.ID_ArcCustomer\r\nDROP TABLE #OnCustExamSection@RandNum;", text, text2, @int);
				}
				text3 = text3.Replace("@RandNum", Public.GetGuid("-", string.Empty));
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取分科明细统计信息失败(GetSectionSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取分科明细统计信息失败(GetSectionSearchWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_438:
						i++;
						continue;
						goto IL_438;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "分科明细统计";
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		public void GetDiseaseLevelWorkLoad()
		{
			lock ("1")
			{
				int @int = base.GetInt("flag", -1);
				int int2 = base.GetInt("IsInformed", -1);
				int int3 = base.GetInt("MinLevel", -1);
				int int4 = base.GetInt("MaxLevel", -1);
				string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
				string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
				string str = text.Replace("-", string.Empty);
				string str2 = text2.Replace("-", string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					text += " 00:00:00";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 += " 23:59:59";
				}
				string text3 = string.Empty;
				if (int2 == 1)
				{
					text3 = string.Format("SELECT ID_CustExamItem,ID_CustFee,ID_Fee,ID_ExamItem,ExamItemName,DiseaseLevel,ResultSummary,ExamItemSummaryDate,SummaryDoctorName\r\nINTO #TempOnCustExamItem@RandNum FROM OnCustExamItem\r\nWHERE DiseaseLevel>='{0}' AND DiseaseLevel<='{1}'\r\nAND ExamItemSummaryDate BETWEEN '{2}' and '{3}' \r\nSELECT OCPEI.ExamPlaceName,OnCustFee.ID_Customer,OCPEI.CustomerName,OCPEI.GenderName,SectionName,FeeItemName,ExamItemName,ResultSummary,ExamItemSummaryDate,TEMP.DiseaseLevel,SummaryDoctorName,CASE WHEN Is_Informed=1 THEN '已通知' ELSE '未通知' END Is_Informed,Informer,InformedDate\r\nFROM #TempOnCustExamItem@RandNum TEMP \r\n-- 关联客户收费项目表(查收费项目名称及体检号)\r\nLEFT JOIN (SELECT ID_Customer,ID_Fee,FeeItemName,ID_CustFee FROM OnCustFee WHERE EXISTS \r\n(SELECT ID_CustFee FROM #TempOnCustExamItem@RandNum WHERE OnCustFee.ID_CustFee=#TempOnCustExamItem@RandNum.ID_CustFee)\r\n)OnCustFee ON TEMP.ID_CustFee=OnCustFee.ID_CustFee\r\n-- 关联收费项目表(查科室名称)\r\nLEFT JOIN ( SELECT ID_Section,SectionName,ID_Fee FROM BusFee WHERE EXISTS \r\n(SELECT ID_Fee FROM #TempOnCustExamItem@RandNum WHERE BusFee.ID_Fee=#TempOnCustExamItem@RandNum.ID_Fee)\r\n)BusFee ON TEMP.ID_Fee = BusFee.ID_Fee\r\n-- 关联体检基本信息表(查客户姓名)\r\nLEFT JOIN OnCustPhysicalExamInfo OCPEI ON OCPEI.ID_Customer = OnCustFee.ID_Customer\r\n-- 关联报告管理表(查通知状态，通知人，通知时间)\r\nLEFT JOIN OnCustReportManage OCRM ON OCRM.ID_Customer = OnCustFee.ID_Customer\r\nWHERE OCRM.Is_Informed=1;\r\nDROP TABLE #TempOnCustExamItem@RandNum;", new object[]
					{
						int3,
						int4,
						text,
						text2
					});
				}
				else if (int2 == 0)
				{
					text3 = string.Format("SELECT ID_CustExamItem,ID_CustFee,ID_Fee,ID_ExamItem,ExamItemName,DiseaseLevel,ResultSummary,ExamItemSummaryDate,SummaryDoctorName\r\nINTO #TempOnCustExamItem@RandNum FROM OnCustExamItem\r\nWHERE DiseaseLevel>='{0}' AND DiseaseLevel<='{1}'\r\nAND ExamItemSummaryDate BETWEEN '{2}' and '{3}' \r\nSELECT OCPEI.ExamPlaceName,OnCustFee.ID_Customer,OCPEI.CustomerName,OCPEI.GenderName,SectionName,FeeItemName,ExamItemName,ResultSummary,ExamItemSummaryDate,TEMP.DiseaseLevel,SummaryDoctorName,CASE WHEN Is_Informed=1 THEN '已通知' ELSE '未通知' END Is_Informed,Informer,InformedDate\r\nFROM #TempOnCustExamItem@RandNum TEMP \r\n-- 关联客户收费项目表(查收费项目名称及体检号)\r\nLEFT JOIN (SELECT ID_Customer,ID_Fee,FeeItemName,ID_CustFee FROM OnCustFee WHERE EXISTS \r\n(SELECT ID_CustFee FROM #TempOnCustExamItem@RandNum WHERE OnCustFee.ID_CustFee=#TempOnCustExamItem@RandNum.ID_CustFee)\r\n)OnCustFee ON TEMP.ID_CustFee=OnCustFee.ID_CustFee\r\n-- 关联收费项目表(查科室名称)\r\nLEFT JOIN ( SELECT ID_Section,SectionName,ID_Fee FROM BusFee WHERE EXISTS \r\n(SELECT ID_Fee FROM #TempOnCustExamItem@RandNum WHERE BusFee.ID_Fee=#TempOnCustExamItem@RandNum.ID_Fee)\r\n)BusFee ON TEMP.ID_Fee = BusFee.ID_Fee\r\n-- 关联体检基本信息表(查客户姓名)\r\nLEFT JOIN OnCustPhysicalExamInfo OCPEI ON OCPEI.ID_Customer = OnCustFee.ID_Customer\r\n-- 关联报告管理表(查通知状态，通知人，通知时间)\r\nLEFT JOIN OnCustReportManage OCRM ON OCRM.ID_Customer = OnCustFee.ID_Customer\r\nWHERE ISNULL(OCRM.Is_Informed,0)=0;\r\nDROP TABLE #TempOnCustExamItem@RandNum;", new object[]
					{
						int3,
						int4,
						text,
						text2
					});
				}
				else
				{
					text3 = string.Format("SELECT ID_CustExamItem,ID_CustFee,ID_Fee,ID_ExamItem,ExamItemName,DiseaseLevel,ResultSummary,ExamItemSummaryDate,SummaryDoctorName\r\nINTO #TempOnCustExamItem@RandNum FROM OnCustExamItem\r\nWHERE DiseaseLevel>='{0}' AND DiseaseLevel<='{1}'\r\nAND ExamItemSummaryDate BETWEEN '{2}' and '{3}' \r\nSELECT OCPEI.ExamPlaceName,OnCustFee.ID_Customer,OCPEI.CustomerName,OCPEI.GenderName,SectionName,FeeItemName,ExamItemName,ResultSummary,ExamItemSummaryDate,TEMP.DiseaseLevel,SummaryDoctorName,CASE WHEN Is_Informed=1 THEN '已通知' ELSE '未通知' END Is_Informed,Informer,InformedDate\r\nFROM #TempOnCustExamItem@RandNum TEMP \r\n-- 关联客户收费项目表(查收费项目名称及体检号)\r\nLEFT JOIN (SELECT ID_Customer,ID_Fee,FeeItemName,ID_CustFee FROM OnCustFee WHERE EXISTS \r\n(SELECT ID_CustFee FROM #TempOnCustExamItem@RandNum WHERE OnCustFee.ID_CustFee=#TempOnCustExamItem@RandNum.ID_CustFee)\r\n)OnCustFee ON TEMP.ID_CustFee=OnCustFee.ID_CustFee\r\n-- 关联收费项目表(查科室名称)\r\nLEFT JOIN ( SELECT ID_Section,SectionName,ID_Fee FROM BusFee WHERE EXISTS \r\n(SELECT ID_Fee FROM #TempOnCustExamItem@RandNum WHERE BusFee.ID_Fee=#TempOnCustExamItem@RandNum.ID_Fee)\r\n)BusFee ON TEMP.ID_Fee = BusFee.ID_Fee\r\n-- 关联体检基本信息表(查客户姓名)\r\nLEFT JOIN OnCustPhysicalExamInfo OCPEI ON OCPEI.ID_Customer = OnCustFee.ID_Customer\r\n-- 关联报告管理表(查通知状态，通知人，通知时间)\r\nLEFT JOIN OnCustReportManage OCRM ON OCRM.ID_Customer = OnCustFee.ID_Customer\r\nDROP TABLE #TempOnCustExamItem@RandNum;", new object[]
					{
						int3,
						int4,
						text,
						text2
					});
				}
				text3 = text3.Replace("@RandNum", Public.GetGuid("-", string.Empty));
				DataTable dataTable = null;
				DataSet dataSet = null;
				DataTable dataTable2 = null;
				DataTable dataTable3 = null;
				DataSet dataSet2 = null;
				DataTable dataTable4 = null;
				ArrayList arrayList = null;
				DataSet dataSet3 = null;
				string text4 = string.Empty;
				int num = 0;
				int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text3.Replace("@ExamState", "0"), new int?(this.CommandTimeout));
				dataTable2 = dataSet.Tables[0];
				if (!string.IsNullOrEmpty(this.toOffLineConnectionString))
				{
					try
					{
						DataSet dataSet4 = PEIS.BLL.CommonStatistics.Instance.Query(this.toOffLineConnectionString, text3.Replace("@ExamState", "1"), new int?(this.CommandTimeout));
						dataTable3 = dataSet4.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从离线库中获取病症级别统计信息失败(GetDiseaseLevelWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (!string.IsNullOrEmpty(this.toWasteConnectionString))
				{
					try
					{
						dataSet2 = PEIS.BLL.CommonStatistics.Instance.Query(this.toWasteConnectionString, text3.Replace("@ExamState", "-1"), new int?(this.CommandTimeout));
						dataTable4 = dataSet2.Tables[0];
					}
					catch (Exception ex)
					{
						Log4J.Instance.Error(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",从废弃库中获取病症级别统计信息失败(GetDiseaseLevelWorkLoad)：",
							ex.Message,
							" ",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
				if (num > 0)
				{
					arrayList = new ArrayList(num);
					int i = 1;
					while (i <= num)
					{
						text4 = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
						if (text4 != null)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								try
								{
									dataSet3 = PEIS.BLL.CommonStatistics.Instance.Query(text4, text3.Replace("@ExamState", (i + 1).ToString()), new int?(this.CommandTimeout));
									arrayList.Add(dataSet3.Tables[0]);
									dataSet3.Dispose();
								}
								catch (Exception ex)
								{
									Log4J.Instance.Error(string.Concat(new string[]
									{
										Public.GetClientIP(),
										",",
										this.LoginUserModel.UserName,
										",查询个人登记统计信息失败，失败原因为从",
										text4,
										"获取数据出现如下错误：",
										ex.Message
									}));
								}
							}
						}
						IL_4F5:
						i++;
						continue;
						goto IL_4F5;
					}
				}
				if (dataTable3 != null)
				{
					dataTable2.Merge(dataTable3);
				}
				if (dataTable4 != null)
				{
					dataTable2.Merge(dataTable4);
				}
				if (arrayList != null)
				{
					foreach (DataTable dataTable5 in arrayList)
					{
						if (dataTable5 != null)
						{
							dataTable2.Merge(dataTable5);
						}
					}
				}
				dataSet.Tables.Clear();
				dataSet.Tables.Add(dataTable2);
				DataSet dataSet5 = dataSet;
				string text5 = "病症级别";
				if (@int == 0)
				{
					text5 = "病症级别查询";
				}
				else if (@int == 1)
				{
					text5 = "病症级别统计";
				}
				dataTable2.TableName = str + "-" + str2 + text5;
				string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
				if (!Directory.Exists(text6))
				{
					Directory.CreateDirectory(text6);
				}
				string str3 = text5 + ".xls";
				string text7 = text6 + "\\" + text5 + ".xls";
				if (File.Exists(text7))
				{
					File.Delete(text7);
				}
				Excel.DataSetToExcel(dataSet5, text7);
				DataTable dataTable6 = new DataTable();
				dataTable6.Columns.Add("FilePath");
				DataRow dataRow = dataTable6.NewRow();
				dataRow["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
				dataTable6.Rows.Add(dataRow);
				dataSet5.Tables.Add(dataTable6);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet5);
				this.OutPutMessage(msg);
				if (dataSet3 != null)
				{
					dataSet3.Dispose();
					dataSet3 = null;
				}
				if (dataTable3 != null)
				{
					dataTable3.Dispose();
					dataTable3 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (dataTable2 != null)
				{
					dataTable2.Dispose();
					dataTable2 = null;
				}
				if (dataSet != null)
				{
					dataSet.Dispose();
					dataSet = null;
				}
				if (arrayList != null)
				{
					arrayList.Clear();
					arrayList = null;
				}
				if (dataTable != null)
				{
					dataTable.Clear();
					dataTable = null;
				}
				if (dataSet5 != null)
				{
					dataSet5.Dispose();
				}
				if (dataTable4 != null)
				{
					dataTable4.Dispose();
					dataTable4 = null;
				}
				if (dataSet2 != null)
				{
					dataSet2.Dispose();
					dataSet2 = null;
				}
			}
		}

		public void GetCustomerPhysicalExamInfo()
		{
			lock ("1")
			{
				this.BeginDate = DateTime.Now;
				string text = base.GetString("IDCard").Trim().ToLower();
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = string.Empty;
					text2 = string.Format("SELECT ID_ArcCustomer,IDCardNo,ExamCardNo,ID_Customer,Is_CompletePhysical,ISNULL(ExamState,0)ExamState FROM OnCustRelationCustPEInfo WHERE ID_ArcCustomer in(select ID_ArcCustomer from OnArcCust where IDCard='{0}');", text);
					string empty = string.Empty;
					string text3 = string.Empty;
					DataTable dataTable = PEIS.BLL.CommonStatistics.Instance.Query(text2, null).Tables[0];
					if (dataTable.Rows.Count != 0)
					{
						Hashtable hashtable = new Hashtable(dataTable.Rows.Count);
						foreach (DataRow dataRow in dataTable.Rows)
						{
							int num = int.Parse(dataRow["ExamState"].ToString().Trim());
							if (num == -1)
							{
								if (!hashtable.Contains(this.toWasteConnectionString))
								{
									hashtable.Add(this.toWasteConnectionString, this.toWasteConnectionString);
								}
							}
							else if (num == 0)
							{
								if (!hashtable.Contains(this.toOnLineConnectionString))
								{
									hashtable.Add(this.toOnLineConnectionString, this.toOnLineConnectionString);
								}
							}
							else if (num == 1)
							{
								if (!hashtable.Contains(this.toOffLineConnectionString))
								{
									hashtable.Add(this.toOffLineConnectionString, this.toOffLineConnectionString);
								}
							}
							else if (num > 1)
							{
								text3 = PubConstant.GetConnectionString("FYH_HisFile" + (num - 1).ToString().PadLeft(3, '0'));
								if (!hashtable.Contains(text3))
								{
									hashtable.Add(text3, text3);
								}
							}
						}
						DataSet dataSet = null;
						DataTable dataTable2 = null;
						text2 = string.Format("SELECT ID_Customer,ID_ArcCustomer,IDCardNo INTO #TT@RandNum FROM OnCustRelationCustPEInfo WHERE ID_ArcCustomer in(SELECT ID_ArcCustomer FROM OnArcCust WHERE IDCard='{0}');\r\nSELECT OAC.*,OCPEI.ID_Customer,ExamTypeName,Operator,OperateDate,GuideSheetReturnedby,GuideSheetReturnedDate,FinalDoctor,FinalDate,Checker,CheckedDate,ReportPrinter,ReportPrintedDate,ReportReceiptor,ReportReceiptedDate,OCPEI.TeamName,Department,DepartSubA,DepartSubB,DepartSubC FROM(SELECT A.ID_ArcCustomer,A.IDCardNo,B.* FROM #TT@RandNum A LEFT JOIN OnCustPhysicalExamInfo B ON A.ID_Customer=B.ID_Customer)OCPEI\r\nINNER JOIN(SELECT ID_ArcCustomer,CustomerName,GenderName,CONVERT(varchar(10),BirthDay,120) BirthDay,MarriageName,IDCarD,Photo FROM OnArcCust WHERE IDCard='{0}') OAC ON OCPEI.ID_ArcCustomer=OAC.ID_ArcCustomer\r\nLEFT JOIN TeamTaskGroupCust TTGC ON OCPEI.ID_Customer=TTGC.ID_Customer\r\nLEFT JOIN (SELECT * FROM OnCustReportManage WHERE ID_Customer IN(SELECT ID_Customer FROM #TT@RandNum))OCRM ON OCRM.ID_Customer=OCPEI.ID_Customer\r\nDROP TABLE #TT@RandNum;", text);
						foreach (DictionaryEntry dictionaryEntry in hashtable)
						{
							text2 = text2.Replace("@RandNum", Public.GetGuid("-", string.Empty));
							dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text2, null);
							if (dataTable2 == null)
							{
								dataTable2 = dataSet.Tables[0];
							}
							else
							{
								dataTable2.Merge(dataSet.Tables[0]);
							}
						}
						if (dataTable2.Columns.Contains("Photo"))
						{
							dataTable2.Columns.Add("Base64Photo");
							foreach (DataRow dataRow in dataTable2.Rows)
							{
								if (!Convert.IsDBNull(dataRow["Photo"]))
								{
									dataRow["Base64Photo"] = Convert.ToBase64String((byte[])dataRow["Photo"]);
								}
							}
						}
						string msg = JsonHelperFont.Instance.DataTableToJSON(dataTable2, "dataList");
						this.OutPutMessage(msg);
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						if (dataTable2 != null)
						{
							dataTable2.Dispose();
							dataTable2 = null;
						}
						if (dataSet != null)
						{
							dataSet.Dispose();
							dataSet = null;
						}
						Log4J.Instance.Info(string.Concat(new string[]
						{
							Public.GetClientIP(),
							",",
							this.LoginUserModel.UserName,
							",客户体检信息查询,耗时:",
							Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
						}));
					}
				}
			}
		}

		public string GetTableInfo(DataTable DT)
		{
			string text = string.Empty;
			if (DT != null)
			{
				text = "表名：" + DT.TableName + "\r\n";
				foreach (DataRow dataRow in DT.Rows)
				{
					foreach (DataColumn column in DT.Columns)
					{
						text = text + dataRow[column].ToString() + ",";
					}
					text += "\r\n";
				}
			}
			return text;
		}

		public string GetRandNum(int MinValue, int MaxValue)
		{
			Random random = new Random();
			int num = 1;
			string text = DateTime.Now.ToString("HHmmss");
			num = random.Next(MinValue, MaxValue);
			try
			{
				if (this.Session.SessionID != string.Empty)
				{
					text = this.Session.SessionID + text;
				}
			}
			catch
			{
			}
			return text + num.ToString();
		}

		private DataTable GetBackWorkFlow()
		{
			DataTable dataTable = new DataTable();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this._backLogPath);
			XmlElement documentElement = xmlDocument.DocumentElement;
			XmlNode xmlNode = documentElement.SelectSingleNode("BackLogWorkLoad");
			DataSet dataSet = new DataSet();
			if (documentElement != null)
			{
				string value = documentElement.Attributes["TABLEKEY"].Value;
				string value2 = documentElement.Attributes["COLUMNS"].Value;
				string[] array = value2.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (!string.IsNullOrEmpty(text))
					{
						dataTable.Columns.Add(text);
					}
				}
				XmlNodeList xmlNodeList = documentElement.SelectNodes("WorkFlowItem");
				DataRow dataRow = null;
				foreach (XmlNode xmlNode2 in xmlNodeList)
				{
					dataRow = dataTable.NewRow();
					foreach (DataColumn dataColumn in dataTable.Columns)
					{
						dataRow[dataColumn] = xmlNode2.Attributes[dataColumn.ColumnName].Value;
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			dataTable.ReadXml(this._backLogPath);
			return dataTable;
		}

		public void SearchBackLog_old()
		{
			string str = base.GetString("prevWorkFlow").Trim().ToLower();
			int @int = base.GetInt("delayDays", -1);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this._backLogPath);
			XmlElement documentElement = xmlDocument.DocumentElement;
			string str2 = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			DataTable dataTable = null;
			if (documentElement != null)
			{
				str2 = documentElement.Attributes["TABLEKEY"].Value;
			}
			XmlNode xmlNode = documentElement.SelectSingleNode("//WorkFlowItem [@ID='" + str + "']");
			if (xmlNode != null)
			{
				text4 = xmlNode.Attributes["COLUMNSKEY"].Value.ToString();
				dataTable = new DataTable();
				dataTable.Columns.Add("columns");
				DataRow dataRow = dataTable.NewRow();
				dataRow["columns"] = text4;
				dataTable.Rows.Add(dataRow);
				text2 = xmlNode.Attributes["TEXT"].Value.ToString();
				text3 = xmlNode.Attributes["SQL"].Value.ToString();
				string text5 = xmlNode.Attributes["TRUESTATEKEY"].Value.ToString();
				string text6 = xmlNode.Attributes["FALSESTATEKEY"].Value.ToString();
				if (string.IsNullOrEmpty(text3))
				{
					if (!string.IsNullOrEmpty(text5))
					{
						text = string.Format(str2 + " WHERE 1=1 AND " + text5, @int);
					}
					if (!string.IsNullOrEmpty(text6))
					{
						text += string.Format(" AND " + text6, new object[0]);
					}
				}
				else
				{
					text = string.Format(text3, @int);
				}
			}
			DataSet dataSet = new DataSet();
			DataTable dataTable2 = new DataTable();
			try
			{
				dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text, null);
				Log4J.Instance.Error(string.Concat(new string[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",从在线库中查询积案(SearchBackLog)[",
					text2,
					"]成功",
					Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
				}));
			}
			catch (Exception ex)
			{
				dataSet.Tables.Add(new DataTable());
				Log4J.Instance.Error(string.Concat(new string[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",从在线库中查询积案(SearchBackLog)[",
					text2,
					"]失败，失败原因：",
					ex.Message,
					" ",
					Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
				}));
			}
			finally
			{
				if (dataSet != null)
				{
					string text7 = DateTime.Now.ToString("yyyyMMDDHHssmm") + "-" + text2.Replace("->", "_");
					DataTable dataTable3 = new DataTable();
					dataTable3.TableName = text7;
					string text8 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text8))
					{
						Directory.CreateDirectory(text8);
					}
					string str3 = text7 + ".xls";
					string text9 = text8 + "\\" + text7 + ".xls";
					if (File.Exists(text9))
					{
						File.Delete(text9);
					}
					DataSet dataSet2 = dataSet.Copy();
					if (dataSet2.Tables.Count > 0)
					{
						if (!string.IsNullOrEmpty(text4))
						{
							string[] array = text4.Split(new char[]
							{
								','
							});
							if (array.Length > 3)
							{
								if (dataSet2.Tables[0].Columns.Count > 0)
								{
									dataSet2.Tables[0].Columns["PrevOper"].ColumnName = array[0];
									dataSet2.Tables[0].Columns["PrevOperDate"].ColumnName = array[1];
									dataSet2.Tables[0].Columns["NextOper"].ColumnName = array[2];
									dataSet2.Tables[0].Columns["NextOperDate"].ColumnName = array[3];
								}
							}
						}
					}
					Excel.DataSetToExcel(dataSet2, text9);
					dataSet2.Dispose();
					DataTable dataTable4 = new DataTable();
					dataTable4.Columns.Add("FilePath");
					DataRow dataRow2 = dataTable4.NewRow();
					dataRow2["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str3;
					dataTable4.Rows.Add(dataRow2);
					dataSet.Tables.Add(dataTable4);
					dataSet.Tables.Add(dataTable);
					string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet);
					this.OutPutMessage(msg);
					if (dataTable != null)
					{
						dataTable.Dispose();
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
					}
					dataSet.Dispose();
				}
			}
		}

		public void SearchBackLog()
		{
			string str = base.GetString("prevWorkFlow").Trim().ToLower();
			string text = base.GetString("prevWorkFlowText").Trim().ToLower();
			int @int = base.GetInt("delayDays", -1);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this._backLogPath);
			XmlElement documentElement = xmlDocument.DocumentElement;
			string text2 = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			string text3 = string.Empty;
			DataTable dataTable = null;
			if (documentElement != null)
			{
			}
			XmlNode xmlNode = documentElement.SelectSingleNode("//WorkFlowItem [@ID='" + str + "']");
			if (xmlNode != null)
			{
				text3 = xmlNode.Attributes["COLUMNSKEY"].Value.ToString();
				dataTable = new DataTable();
				dataTable.Columns.Add("columns");
				DataRow dataRow = dataTable.NewRow();
				dataRow["columns"] = text3;
				dataTable.Rows.Add(dataRow);
				string text4 = xmlNode.Attributes["BACKTYPE"].Value.ToString();
				string[] array = text4.Split(new char[]
				{
					','
				});
				if (array.Length > 1)
				{
					object arg = array[0];
					object arg2 = array[1];
					text2 = string.Format("SELECT OCPE.ID_Customer,CustomerName,GenderName,MarriageName,CONVERT(varchar(10),BirthDay,120) BirthDay,MobileNo,PrevOper,PrevOperDate,NextOper,NextOperDate FROM OnCustPhysicalExamInfo OCPE\r\nINNER JOIN (SELECT ID_CUSTOMER,ID_BackLogType,Operator PrevOper,CONVERT(varchar(20),OperateDate,120) PrevOperDate,'' NextOper,'' NextOperDate\r\nFROM OnCustBackLog AS A WHERE A.Is_Finished=1 AND DATEDIFF(DY,A.OperateDate,getdate())>={0} AND A.ID_BackLogType in({1}) AND NOT EXISTS(SELECT B.ID_CUSTOMER FROM OnCustBackLog AS B WHERE A.ID_CUSTOMER = B.ID_CUSTOMER AND B.Is_Finished=1 AND B.ID_BackLogType in({2})))OCBL\r\nON OCPE.ID_Customer=OCBL.ID_Customer", @int, arg, arg2);
				}
			}
			DataSet dataSet = new DataSet();
			DataTable dataTable2 = new DataTable();
			try
			{
				if (text2.Length > 0)
				{
					dataSet = PEIS.BLL.CommonStatistics.Instance.Query(text2, null);
					Log4J.Instance.Error(string.Concat(new string[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",从在线库中查询积案(SearchBackLog)[",
						empty,
						"]成功",
						Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
					}));
				}
				else
				{
					Log4J.Instance.Error(string.Concat(new string[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",未执行查询(SearchBackLog)[",
						empty,
						"] 原因为:未创建查询语句",
						Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
					}));
				}
			}
			catch (Exception ex)
			{
				dataSet.Tables.Add(new DataTable());
				Log4J.Instance.Error(string.Concat(new string[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",从在线库中查询积案(SearchBackLog)[",
					empty,
					"]失败，失败原因：",
					ex.Message,
					" ",
					Public.GetDateDiff(string.Empty, this.BeginDate, DateTime.Now)
				}));
			}
			finally
			{
				if (dataSet != null)
				{
					string text5 = DateTime.Now.ToString("yyyyMMDDHHssmm") + "-" + empty.Replace("->", "_");
					DataTable dataTable3 = new DataTable();
					dataTable3.TableName = text5;
					string text6 = this.FilePath + "\\OutToExcel\\" + this.Session.SessionID;
					if (!Directory.Exists(text6))
					{
						Directory.CreateDirectory(text6);
					}
					string str2 = text5 + ".xls";
					string text7 = text6 + "\\" + text5 + ".xls";
					if (File.Exists(text7))
					{
						File.Delete(text7);
					}
					DataSet dataSet2 = dataSet.Copy();
					if (dataSet2.Tables.Count > 0)
					{
						if (!string.IsNullOrEmpty(text3))
						{
							string[] array2 = text3.Split(new char[]
							{
								','
							});
							if (array2.Length > 3)
							{
								if (dataSet2.Tables[0].Columns.Count > 0)
								{
									dataSet2.Tables[0].Columns["PrevOper"].ColumnName = array2[0];
									dataSet2.Tables[0].Columns["PrevOperDate"].ColumnName = array2[1];
									dataSet2.Tables[0].Columns["NextOper"].ColumnName = array2[2];
									dataSet2.Tables[0].Columns["NextOperDate"].ColumnName = array2[3];
								}
							}
						}
					}
					Excel.DataSetToExcel(dataSet2, text7);
					dataSet2.Dispose();
					DataTable dataTable4 = new DataTable();
					dataTable4.Columns.Add("FilePath");
					DataRow dataRow2 = dataTable4.NewRow();
					dataRow2["FilePath"] = "/OutToExcel/" + this.Session.SessionID + "/" + str2;
					dataTable4.Rows.Add(dataRow2);
					dataSet.Tables.Add(dataTable4);
					dataSet.Tables.Add(dataTable);
					string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet);
					this.OutPutMessage(msg);
					if (dataTable != null)
					{
						dataTable.Dispose();
					}
					if (dataTable3 != null)
					{
						dataTable3.Dispose();
					}
					dataSet.Dispose();
				}
			}
		}
	}
}
