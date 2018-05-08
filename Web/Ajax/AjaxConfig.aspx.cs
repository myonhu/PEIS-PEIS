using PEIS.Base;
using PEIS.Common;
using PEIS.BLL;
using PEIS.Model;
using PEIS.SQLServerDAL;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace PEIS.Web.Ajax
{
	public class AjaxConfig : BasePage
	{
		public string ErrorMessage = string.Empty;

		public void OutPutMessage(string msg)
		{
			base.Response.Write(msg);
		}

		public void TestOutMessage()
		{
			this.OutPutMessage("This is the Test Info ... ");
		}

		protected void Page_Load(object sender, EventArgs e)
		{
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

		public void SearchFeeList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchFeeKeyword").Trim();
			int int3 = base.GetInt("SelectedSectionID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesFeeList_Param";
			if (!string.IsNullOrEmpty(text) && int3 <= 0)
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesSectionFeeListParamByName";
				array[0] = new SqlConditionInfo("@FeeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			if (int3 > 0 && string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesFeeListParamBySection";
				array[0] = new SqlConditionInfo("@SectionID", int3, TypeCode.Int32);
				array[0].Place = 2;
			}
			if (int3 > 0 && !string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[2];
				pageCode = "QueryPagesFeeListParamByNameAndSection";
				array[0] = new SqlConditionInfo("@FeeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
				array[1] = new SqlConditionInfo("@SectionID", int3, TypeCode.Int32);
				array[1].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void GetSingleFeeItem()
		{
			int @int = base.GetInt("ID_Fee", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Fee", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleFeeItem_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SaveFeeItemInfo()
		{
			int @int = base.GetInt("ID_Fee", 0);
			string text = base.GetString("FeeName");
			text = Input.URLDecode(text);
			string text2 = base.GetString("ReportFeeName");
			text2 = Input.URLDecode(text2);
			string text3 = base.GetString("InterfaceName");
			text3 = Input.URLDecode(text3);
			string text4 = base.GetString("FeeCode");
			text4 = Input.URLDecode(text4);
			decimal @decimal = base.GetDecimal("Price", 0m);
			int int2 = base.GetInt("ID_Section", 0);
			int int3 = base.GetInt("ID_Specimen", 0);
			string @string = base.GetString("SectionName");
			string string2 = base.GetString("SpecimenName");
			int int4 = base.GetInt("DispOrder", 500);
			int int5 = base.GetInt("BreakfastOrder", 0);
			int int6 = base.GetInt("Forsex", 0);
			string text5 = base.GetString("WorkGroupCode");
			string text6 = base.GetString("WorkStationCode");
			string text7 = base.GetString("WorkBenchCode");
			text5 = Input.URLDecode(text5);
			text6 = Input.URLDecode(text6);
			text7 = Input.URLDecode(text7);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text8 = base.GetString("BanDescribe");
			text8 = Input.URLDecode(text8);
			string text9 = base.GetString("Note");
			text9 = Input.URLDecode(text9);
			int int7 = base.GetInt("Is_FeeNonPrintInReport", 0);
			int int8 = base.GetInt("IS_FeeReportMerger", 0);
			int int9 = base.GetInt("ID_FeeReportMerger", 0);
			string string3 = base.GetString("FeeReportMergerName");
			string text10 = base.GetString("OperationalDate");
			text10 = Input.URLDecode(text10);
			if (!flag)
			{
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@FeeName", text, TypeCode.String)
				};
				string querySqlCode = "QueryFeeNameIsExis_Param";
				try
				{
					DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
					if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
					{
						if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
						{
							this.OutPutMessage("-1".ToString());
							return;
						}
					}
				}
				catch (Exception e)
				{
                    Log4J.Instance.Error(string.Concat(new object[]
                     {
                        Public.GetClientIP(),
                        ",",
                        this.LoginUserModel.UserName,
                        e.Message                 
                        
                     }));

                    return;
				}
			}
			PEIS.Model.BusFee busFee;
			if (@int <= 0)
			{
				busFee = new PEIS.Model.BusFee();
			}
			else
			{
				busFee = PEIS.BLL.BusFee.Instance.GetModel(@int);
			}
			if (busFee == null)
			{
				this.OutPutMessage("-1");
			}
			else
			{
				busFee.ID_Fee = @int;
				busFee.FeeName = text;
				busFee.ReportFeeName = text2;
				busFee.InterfaceName = text3;
				busFee.FeeCode = text4;
				busFee.InputCode = Input.GetStringSpellCode(text);
				busFee.Price = new decimal?(@decimal);
				busFee.ID_Section = new int?(int2);
				busFee.ID_Specimen = new int?(int3);
				busFee.SectionName = @string;
				busFee.SpecimenName = string2;
				busFee.DispOrder = new int?(int4);
				busFee.BreakfastOrder = new int?(int5);
				busFee.ForGender = new int?(int6);
				busFee.WorkGroupCode = text5;
				busFee.WorkStationCode = text6;
				busFee.WorkBenchCode = text7;
				busFee.Is_FeeNonPrintInReport = new bool?(int7 == 1);
				busFee.OperationalDate = text10;
				if (busFee.Is_Banned != flag && flag)
				{
					busFee.ID_BanOpr = new int?(this.LoginUserModel.UserID);
				}
				busFee.Is_Banned = new bool?(flag);
				if (flag)
				{
					busFee.BanDescribe = text8;
				}
				else
				{
					busFee.BanDescribe = "";
				}
				busFee.Note = text9;
				if (int8 == 1)
				{
					if (busFee != null)
					{
						busFee.IS_FeeReportMerger = new bool?(true);
						busFee.ID_FeeReportMerger = new int?(int9);
						busFee.ReportFeeName = string3;
					}
				}
				if (@int > 0 && int8 == 2)
				{
					if (busFee != null)
					{
						busFee.IS_FeeReportMerger = new bool?(true);
						busFee.ID_FeeReportMerger = new int?(@int);
					}
				}
				int num = PEIS.BLL.CommonConfig.Instance.SaveFeeItem(busFee);
				int num2 = (@int > 0) ? @int : num;
				string text11 = (@int > 0) ? "修改" : "新增";
				if (num > 0)
				{
					if (@int <= 0)
					{
						if (int8 == 2)
						{
							busFee = PEIS.BLL.BusFee.Instance.GetModel(num);
							if (busFee != null)
							{
								busFee.IS_FeeReportMerger = new bool?(true);
								busFee.ID_FeeReportMerger = new int?(num);
							}
						}
					}
					base.ClearCache_AllFee();
					base.ClearCache_AllFeeReportMerger();
					Log4J.Instance.Info(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",",
						text11,
						"收费项目 名称：",
						text,
						",编号：",
						num2
					}));
				}
				else
				{
					Log4J.Instance.Error(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",",
						text11,
						"收费项目失败 名称：",
						text,
						",编号：",
						num2
					}));
				}
				this.OutPutMessage(num.ToString());
			}
		}

		public void SaveFeeExamRel()
		{
			int @int = base.GetInt("ID_Fee", 0);
			string @string = base.GetString("newExamItemIDStrs");
			int num = PEIS.BLL.CommonConfig.Instance.SaveFeeExamRel(@int, @string);
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",保存收费项目明细 ID_Fee：",
					@int,
					",检查项目编号：",
					@string
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",保存收费项目明细失败 ID_Fee：",
					@int,
					",检查项目编号：",
					@string
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void SearchExamList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchExamKeyword").Trim();
			int int3 = base.GetInt("SelectedSectionID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesExamList_Param";
			if (!string.IsNullOrEmpty(text) && int3 <= 0)
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesSectionExamListParamByName";
				array[0] = new SqlConditionInfo("@ExamItemName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			if (int3 > 0 && string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesExamListParamBySection";
				array[0] = new SqlConditionInfo("@ID_Section", int3, TypeCode.Int32);
				array[0].Place = 2;
			}
			if (int3 > 0 && !string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[2];
				pageCode = "QueryPagesExamListParamByNameAndSection";
				array[0] = new SqlConditionInfo("@ExamItemName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
				array[1] = new SqlConditionInfo("@ID_Section", int3, TypeCode.Int32);
				array[1].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void GetExamDetailListByFee()
		{
			int @int = base.GetInt("FeeID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Fee", @int, TypeCode.Int32)
			};
			string querySqlCode = "QueryExamDetailListByFee_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void GetQuickExamItemList()
		{
			string @string = base.GetString("InputCode");
			int @int = base.GetInt("ID_Section", 0);
			DataTable quickExamItemList = PEIS.BLL.CommonConfig.Instance.GetQuickExamItemList(@string, @int);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickExamItemList, quickExamItemList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickFeeList()
		{
			string @string = base.GetString("InputCode");
			int @int = base.GetInt("ID_Section", 0);
			DataTable quickFeeList = PEIS.BLL.CommonConfig.Instance.GetQuickFeeList(@string, @int);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickFeeList, quickFeeList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetSingleExamItem()
		{
			int @int = base.GetInt("ID_ExamItem", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ExamItem", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleExamItem_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SaveExamItemInfo()
		{
			int @int = base.GetInt("ID_ExamItem", 0);
			string text = base.GetString("ExamItemName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("ID_Section", 0);
			string input = base.GetString("SectionName");
			input = Input.URLDecode(input);
			string text2 = base.GetString("AbbrExamName");
			text2 = Input.URLDecode(text2);
			string @string = base.GetString("GetResultWay");
			string string2 = base.GetString("ExamItemCode");
			int int3 = base.GetInt("Is_LisValueNull", 0);
			int int4 = base.GetInt("Is_EntrySectSum", 0);
			int int5 = base.GetInt("EntrySectSumLevel", 0);
			int int6 = base.GetInt("Is_AutoCalc", 0);
			string text3 = base.GetString("CalcExpression");
			text3 = Input.URLDecode(text3);
			int int7 = base.GetInt("SymCols", 0);
			int int8 = base.GetInt("TextboxRows", 0);
			int int9 = base.GetInt("Is_SameRow", 0);
			string text4 = base.GetString("ExamItemUnit");
			text4 = Input.URLDecode(text4);
			decimal @decimal = base.GetDecimal("MaleLoLimit", -9999m);
			decimal decimal2 = base.GetDecimal("MaleHiLimit", -9999m);
			decimal decimal3 = base.GetDecimal("FemaleLoLimit", -9999m);
			decimal decimal4 = base.GetDecimal("FemaleHiLimit", -9999m);
			int int10 = base.GetInt("Is_SymMultiValue", 0);
			int int11 = base.GetInt("Forsex", 2);
			string text5 = base.GetString("Note");
			text5 = Input.URLDecode(text5);
			int int12 = base.GetInt("DispOrder", 500);
			int int13 = base.GetInt("Is_ExamItemNonPrintInReport", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ExamItemName", text, TypeCode.String),
				new SqlConditionInfo("@ID_Section", int2, TypeCode.Int32),
				new SqlConditionInfo("@ExamItemCode", string2, TypeCode.String),
				new SqlConditionInfo("@Note", text5, TypeCode.String)
			};
			string querySqlCode = "QueryExamItemIsExis_Param";
			if (string.IsNullOrEmpty(string2) && string.IsNullOrEmpty(text5))
			{
				querySqlCode = "QueryExamItemIsExis_NoExamCode_NoNote_Param";
			}
			else if (!string.IsNullOrEmpty(string2) && string.IsNullOrEmpty(text5))
			{
				querySqlCode = "QueryExamItemIsExis_NoNote_Param";
			}
			else if (string.IsNullOrEmpty(string2) && !string.IsNullOrEmpty(text5))
			{
				querySqlCode = "QueryExamItemIsExis_NoExamCode_Param";
			}
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					int num = 0;
					for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
					{
						if (@int > 0 && int.Parse(dataSet.Tables[0].Rows[i][0].ToString()) == @int)
						{
							num = 1;
						}
					}
					if (num <= 0)
					{
						this.OutPutMessage("-1".ToString());
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.BusExamItem busExamItem;
			if (@int <= 0)
			{
				busExamItem = new PEIS.Model.BusExamItem();
			}
			else
			{
				busExamItem = PEIS.BLL.BusExamItem.Instance.GetModel(@int);
			}
			busExamItem.ID_ExamItem = @int;
			busExamItem.ExamItemName = text;
			busExamItem.ID_Section = new int?(int2);
			busExamItem.GetResultWay = @string;
			busExamItem.ExamItemCode = string2;
			busExamItem.AbbrExamName = text2;
			busExamItem.Is_LisValueNull = new bool?(int3 == 1);
			busExamItem.Is_EntrySectSum = new bool?(int4 == 1);
			busExamItem.EntrySectSumLevel = new int?(int5);
			busExamItem.Is_AutoCalc = new bool?(int6 == 1);
			busExamItem.CalcExpression = text3;
			busExamItem.SymCols = new int?(int7);
			busExamItem.TextboxRows = new int?(int8);
			busExamItem.Is_SameRow = new bool?(int9 == 1);
			busExamItem.ExamItemUnit = text4;
			busExamItem.MaleLoLimit = new decimal?(@decimal);
			busExamItem.MaleHiLimit = new decimal?(decimal2);
			busExamItem.FemaleLoLimit = new decimal?(decimal3);
			busExamItem.FemaleHiLimit = new decimal?(decimal4);
			busExamItem.Is_SymMultiValue = new bool?(int10 == 1);
			busExamItem.Forsex = new int?(int11);
			busExamItem.Note = text5;
			busExamItem.DispOrder = new int?(int12);
			busExamItem.Is_ExamItemNonPrintInReport = new bool?(int13 == 1);
			busExamItem.InputCode = Input.GetStringSpellCode(text);
			int num2 = PEIS.BLL.CommonConfig.Instance.SaveExamItem(busExamItem);
			int num3 = (@int > 0) ? @int : num2;
			string text6 = (@int > 0) ? "修改" : "新增";
			if (num2 > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text6,
					"检查项目 名称：",
					text,
					",编号：",
					num3
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text6,
					"检查项目失败 名称：",
					text,
					",编号：",
					num3
				}));
			}
			this.OutPutMessage(num2.ToString());
			base.ClearCache_AllExamItem();
		}

		public void GetSymptomDetailListByExamID()
		{
			int @int = base.GetInt("ID_ExamItem", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ExamItem", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySymptonDetailListByExamID_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SaveSymptomInfo()
		{
			int @int = base.GetInt("ID_ExamItem", 0);
			string input = base.GetString("ExamItemName");
			input = Input.URLDecode(input);
			int int2 = base.GetInt("ID_Symptom", 0);
			string text = base.GetString("SymptomName");
			text = Input.URLDecode(text);
			int int3 = base.GetInt("ID_Conclusion", 0);
			string input2 = base.GetString("ConclusionName");
			input2 = Input.URLDecode(input2);
			int int4 = base.GetInt("DiseaseLevel", 0);
			string text2 = base.GetString("NumOperSign");
			text2 = Input.URLDecode(text2);
			decimal @decimal = base.GetDecimal("NumMale", -9999m);
			decimal decimal2 = base.GetDecimal("NumFemale", -9999m);
			int int5 = base.GetInt("Is_Default", 0);
			string text3 = base.GetString("SymptomDescribe");
			text3 = Input.URLDecode(text3);
			int int6 = base.GetInt("DispOrder", 500);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ExamItem", @int, TypeCode.Int16),
				new SqlConditionInfo("@SymptomName", text, TypeCode.String)
			};
			string querySqlCode = "QuerySymptomNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (int2 <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != int2)
					{
						this.OutPutMessage("-1");
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.BusSymptom busSymptom;
			if (int2 <= 0)
			{
				busSymptom = new PEIS.Model.BusSymptom();
			}
			else
			{
				busSymptom = PEIS.BLL.BusSymptom.Instance.GetModel(int2);
			}
			busSymptom.ID_Symptom = int2;
			busSymptom.ID_ExamItem = new int?(@int);
			busSymptom.SymptomName = text;
			busSymptom.ID_Conclusion = new int?(int3);
			busSymptom.DiseaseLevel = new int?(int4);
			busSymptom.NumOperSign = text2;
			busSymptom.NumMale = new decimal?(@decimal);
			busSymptom.NumFemale = new decimal?(decimal2);
			busSymptom.Is_Default = new bool?(int5 == 1);
			busSymptom.SymptomDescribe = text3;
			busSymptom.DispOrder = new int?(int6);
			busSymptom.InputCode = Input.GetStringSpellCode(text);
			if (busSymptom.Is_Banned != flag && flag)
			{
				busSymptom.ID_BanOpr = new int?(this.LoginUserModel.UserID);
				busSymptom.BanOperator = this.LoginUserModel.UserName;
				busSymptom.BanDate = new DateTime?(DateTime.Now);
			}
			busSymptom.Is_Banned = new bool?(flag);
			int num = PEIS.BLL.CommonConfig.Instance.SaveSymptom(busSymptom);
			int num2 = (int2 > 0) ? int2 : num;
			string text4 = (int2 > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text4,
					"体征词 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text4,
					"体征词失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void GetQuickSpecimenList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickSpecimenList = PEIS.BLL.CommonConfig.Instance.GetQuickSpecimenList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickSpecimenList, quickSpecimenList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickFeeReportMergerList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickFeeReportMergerList = PEIS.BLL.CommonConfig.Instance.GetQuickFeeReportMergerList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickFeeReportMergerList, quickFeeReportMergerList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickConclusionTypeList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickConclusionTypeList = PEIS.BLL.CommonConfig.Instance.GetQuickConclusionTypeList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickConclusionTypeList, quickConclusionTypeList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickFinalConclusionTypeList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickFinalConclusionTypeList = PEIS.BLL.CommonConfig.Instance.GetQuickFinalConclusionTypeList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickFinalConclusionTypeList, quickFinalConclusionTypeList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickICD10List()
		{
			string @string = base.GetString("InputCode");
			DataTable quickICD10List = PEIS.BLL.CommonConfig.Instance.GetQuickICD10List(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickICD10List, quickICD10List.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void SaveSectionInfo()
		{
			int @int = base.GetInt("ID_Section", 0);
			string text = base.GetString("SectionName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("FunctionType", 0);
			//int int3 = base.GetInt("Is_AutoApprove", 0);
			//int int4 = base.GetInt("Is_OwnInterface", 0);
			//string @string = base.GetString("InterfaceType");
			//string text2 = base.GetString("PacsInterfaceFlag");
			//text2 = Input.URLDecode(text2);
			//string string2 = base.GetString("ImageType");
			//int int5 = base.GetInt("Is_NonPrintSectSummary", 0);
			//int int6 = base.GetInt("Is_NotSamePage", 0);
			//string string3 = base.GetString("PicPrintSetup");
			//string text3 = base.GetString("SummaryName");
			//text3 = Input.URLDecode(text3);
			int int7 = base.GetInt("DispOrder", 500);
			//string text4 = base.GetString("DefaultSummary");
			//text4 = Input.URLDecode(text4);
			//string text5 = base.GetString("SepBetweenExamItems");
			//text5 = Input.URLDecode(text5);
			//string text6 = base.GetString("SepBetweenSymptoms");
			//text6 = Input.URLDecode(text6);
			//string text7 = base.GetString("TerminalSymbol");
			//text7 = Input.URLDecode(text7);
			//string text8 = base.GetString("SepExamAndValue");
			//text8 = Input.URLDecode(text8);
			//string text9 = base.GetString("NoBetweenExamItems");
			//text9 = Input.URLDecode(text9);
			//string text10 = base.GetString("NoBetweenSympotms");
			//text10 = Input.URLDecode(text10);
			//int int8 = base.GetInt("Is_NoEntryFinalSummary", 0);
			//int int9 = base.GetInt("Is_NonPrintInReport", 0);
			//string text11 = base.GetString("Note");
			//text11 = Input.URLDecode(text11);
			//bool value = base.GetInt("Is_Banned", 0) == 1;
			string text12 = base.GetString("DisplayMenu");
			text12 = Input.URLDecode(text12);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@SectionName", text, TypeCode.String)
			};
			string querySqlCode = "QuerySectionNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
						this.OutPutMessage("-1");
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.SYSSection sysSection;
			if (@int <= 0)
			{
                sysSection = new PEIS.Model.SYSSection();
			}
			else
			{
                sysSection = PEIS.BLL.SYSSection.Instance.GetModel(@int);
			}
            sysSection.SectionID = @int;
            sysSection.SectionName = text;
            sysSection.FunctionType = (int2 == 1 ? true : false);
            //sysSection.Is_AutoApprove = new bool?(int3 == 1);
            //sysSection.Is_OwnInterface = new bool?(int4 == 1);
            //sysSection.InterfaceType = @string;
            //sysSection.PacsInterfaceFlag = text2;
            //sysSection.ImageType = string2;
            //sysSection.Is_NonPrintSectSummary = new bool?(int5 == 1);
            //sysSection.Is_NotSamePage = new bool?(int6 == 1);
            //sysSection.PicPrintSetup = string3;
            //sysSection.SummaryName = text3;
            //sysSection.DispOrder = new int?(int7);
            //sysSection.DefaultSummary = text4;
            //sysSection.SepBetweenExamItems = text5;
            //sysSection.SepBetweenSymptoms = text6;
            //sysSection.TerminalSymbol = text7;
            //sysSection.SepExamAndValue = text8;
            //sysSection.NoBetweenExamItems = text9;
            //sysSection.NoBetweenSympotms = text10;
            //sysSection.Is_NoEntryFinalSummary = new bool?(int8 == 1);
            //sysSection.Is_NonPrintInReport = new bool?(int9 == 1);
            //sysSection.Is_Banned = new bool?(value);
            sysSection.DisplayMenu = text12;
            //sysSection.InputCode = Input.GetStringSpellCode(text);
            //sysSection.Note = text11;
			int num = PEIS.BLL.CommonConfig.Instance.SaveSection(sysSection);
			int num2 = (@int > 0) ? @int : num;
			string text13 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text13,
					"科室 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text13,
					"科室失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
			if (@int > 0)
			{
				base.ClearCache_Section(@int);
			}
			else if (num > 1)
			{
				base.ClearCache_Section(num);
			}
		}

		public void SaveUserInfo()
		{
			int @int = base.GetInt("UserID", 0);
			string text = base.GetString("UserName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("SectionID", 0);
			string input = base.GetString("SectionName");
			input = Input.URLDecode(input);
			int int3 = base.GetInt("OperateLevel", 0);
			int int4 = base.GetInt("DisCountRate", 0);
			int int5 = base.GetInt("VocationType", 0);
			int int6 = base.GetInt("GenderName", 0);
			int int7 = base.GetInt("IsDel", 0);
			string @string = base.GetString("Signature");
			string text2 = base.GetString("Note");
			text2 = Input.URLDecode(text2);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@UserName", text, TypeCode.String)
			};
			string querySqlCode = "QueryUserNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
						this.OutPutMessage("-1");
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.SYSOpUser sysopuser;
			if (@int <= 0)
			{
                sysopuser = new PEIS.Model.SYSOpUser();             
                sysopuser.CreateTime = DateTime.Now;
			}
			else
			{
                sysopuser = PEIS.BLL.SYSOpUser.Instance.GetModel(@int);
			}
            sysopuser.UserID = @int;
            sysopuser.UserName = text;
            sysopuser.SectionID = new int?(int2);
            sysopuser.OperateLevel = new int?(int3);
            sysopuser.DisCountRate = new int?(int4);
            sysopuser.VocationType = new int?(int5);
            sysopuser.Sex = new int?(int6);
            sysopuser.Is_Del = new int?(int7);
			if (@int <= 0)
			{
                sysopuser.PassWord = Secret.MD5.Encrypt("888888");
			}
			else if (string.IsNullOrEmpty(sysopuser.PassWord))
			{
                sysopuser.PassWord = Secret.MD5.Encrypt("888888");
			}
            sysopuser.LoginName = this.GetUserLoginName(@int, text);
			if (!string.IsNullOrEmpty(@string))
			{
				byte[] signature = Convert.FromBase64String(@string);
                sysopuser.Signature = signature;
			}
            sysopuser.Note = text2;
			int num = PEIS.BLL.CommonConfig.Instance.SaveUser(sysopuser);
			int num2 = (@int > 0) ? @int : num;
			string text3 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text3,
					"用户 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text3,
					"用户失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
			base.ClearCache_User(int2);
		}

		private string GetUserLoginName(int ID_User, string UserName)
		{
			string stringSpellCode = Input.GetStringSpellCode(UserName);
			int num = 0;
			string result;
			while (true)
			{
				string text = stringSpellCode;
				if (num > 0)
				{
					text = stringSpellCode + num.ToString();
				}
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@LoginName", text, TypeCode.String)
				};
				string querySqlCode = "QueryUserLoginNameIsExis_Param";
				try
				{
					DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
					if (dataSet == null || dataSet.Tables[0].Rows.Count <= 0)
					{
						result = text;
						break;
					}
					if (ID_User > 0 && dataSet.Tables[0].Rows.Count == 1 && int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) == ID_User)
					{
						result = text;
						break;
					}
					num++;
				}
				catch (Exception e)
				{
					result = "";
					break;
				}
			}
			return result;
		}

		public void ClearUserLoginCountInfo()
		{
			int @int = base.GetInt("UserID", 0);
			int num = PEIS.BLL.CommonConfig.Instance.ClearUserLoginCount(@int);
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
                    ",清除登录错误次数成功 UserID：",
					@int
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",清除登录错误次数失败 ID_User：",
					@int
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void ResetUserPasswordInfo()
		{
			int @int = base.GetInt("UserID", 0);
			string text = base.GetString("DefaultPassword");
			text = Secret.MD5.Encrypt(text);
			int num = PEIS.BLL.CommonConfig.Instance.ResetUserPassword(@int, text);
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
                    ",重置密码 UserID：",
					@int
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
                    ",重置密码失败 UserID：",
					@int
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void ModifyUserPasswordInfo()
		{
			int num = 0;
			string text = base.GetString("OldPassword");
			text = Secret.MD5.Encrypt(text);
			string text2 = base.GetString("NewPassword");
			text2 = Secret.MD5.Encrypt(text2);
			PEIS.Model.SYSOpUser model = PEIS.BLL.SYSOpUser.Instance.GetModel(this.LoginUserModel.UserID);
			if (model == null)
			{
				num = 0;
			}
			else if (model.PassWord != text)
			{
				num = -1;
			}
			else if (model.PassWord == text)
			{
				num = PEIS.BLL.CommonConfig.Instance.ResetUserPassword(this.LoginUserModel.UserID, text2);
			}
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
                    ",修改密码 UserID：",
					this.LoginUserModel.UserID
                }));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
                    ",修改密码失败 UserID：",
					this.LoginUserModel.UserID
                }));
			}
			this.OutPutMessage(num.ToString());
		}

		public void GetQuickNationList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickNationList = PEIS.BLL.CommonConfig.Instance.GetQuickNationList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickNationList, quickNationList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickExamTypeList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickExamTypeList = PEIS.BLL.CommonConfig.Instance.GetQuickExamTypeList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickExamTypeList, quickExamTypeList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetQuickSetList()
		{
			string @string = base.GetString("InputCode");
			int @int = base.GetInt("Sex", 0);
			DataTable quickSetList = PEIS.BLL.CommonConfig.Instance.GetQuickSetList(@string, @int);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickSetList, quickSetList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void SearchSetList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchSetKeyword").Trim();
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesSetList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesSetListParamByName";
				array[0] = new SqlConditionInfo("@PEPackageName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void GetFeeDetailListBySet()
		{
			int @int = base.GetInt("PEPackageID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@PEPackageID", @int, TypeCode.Int32)
			};
			string querySqlCode = "QueryFeeDetailListBySet_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SaveBusSetInfo()
		{
			int @int = base.GetInt("PEPackageID", 0);
			string text = base.GetString("PEPackageName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("DispOrder", 500);
			int int3 = base.GetInt("Forsex", 0);
			bool flag = base.GetInt("IsBanned", 0) == 1;
			string text2 = base.GetString("BanDescribe");
			text2 = Input.URLDecode(text2);
			string text3 = base.GetString("Note");
			text3 = Input.URLDecode(text3);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@PEPackageName", text, TypeCode.String)
			};
			string querySqlCode = "QueryPEPackageNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
						this.OutPutMessage("-1");
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.BusPEPackage pePackage;
			if (@int <= 0)
			{
                pePackage = new PEIS.Model.BusPEPackage();
			}
			else
			{
                pePackage = PEIS.BLL.BusPEPackage.Instance.GetModel(@int);
			}
            pePackage.PEPackageID = @int;
			if (@int <= 0)
			{
                pePackage.CreateDate = new DateTime?(DateTime.Now);
			}
            pePackage.PEPackageName = text;
            pePackage.InputCode = Input.GetStringSpellCode(text);
			if (pePackage.InputCode.Length > 30)
			{
                pePackage.InputCode = pePackage.InputCode.Substring(0, 30);
			}
            pePackage.DispOrder = new int?(int2);
            pePackage.Forsex = new int?(int3);
			if (pePackage.isBanned != flag && flag)
			{
                pePackage.IDBanOpr = new int?(this.LoginUserModel.UserID);
                pePackage.BanDate = new DateTime?(DateTime.Now);
			}
            pePackage.isBanned = flag;
			if (flag)
			{
                pePackage.BanDescribe = text2;
			}
			else
			{
                pePackage.BanDescribe = "";
			}
            pePackage.Note = text3;
			int num = PEIS.BLL.CommonConfig.Instance.SaveBusPEPackage(pePackage);
			int num2 = (@int > 0) ? @int : num;
			string text4 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				base.ClearCache_AllSet();
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text4,
					"套餐 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text4,
					"套餐失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleBusSet()
		{
			int @int = base.GetInt("PEPackageID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@PEPackageID", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleBusSet_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SaveSetFeeRel()
		{
			int @int = base.GetInt("PEPackageID", 0);
			string @string = base.GetString("newFeeIDStrs");
			int num = PEIS.BLL.CommonConfig.Instance.SaveSetFeeRel(@int, @string);
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",保存套餐明细 PEPackageID：",
					@int,
					",收费项目编号：",
					@string
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",保存套餐明细失败 PEPackageID：",
					@int,
					",收费项目编号：",
					@string
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void SearchSpecimenList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchSpecimenKeyword").Trim();
			int int3 = base.GetInt("SelectedConclusioTypeID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesSpecimenList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesSpecimenListByName_Param";
				array[0] = new SqlConditionInfo("@SpecimenName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveSpecimenInfo()
		{
			int @int = base.GetInt("ID_Specimen", 0);
			string text = base.GetString("SpecimenName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("DispOrder", 500);
			string text2 = base.GetString("LisSpecimenName");
			text2 = Input.URLDecode(text2);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@SpecimenName", text, TypeCode.String)
			};
			string querySqlCode = "QuerySpecimenNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
						this.OutPutMessage("-1");
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.BusSpecimen busSpecimen;
			if (@int <= 0)
			{
				busSpecimen = new PEIS.Model.BusSpecimen();
			}
			else
			{
				busSpecimen = PEIS.BLL.BusSpecimen.Instance.GetModel(@int);
			}
			busSpecimen.ID_Specimen = @int;
			busSpecimen.SpecimenName = text;
			busSpecimen.DispOrder = int2;
			busSpecimen.LisSpecimenName = text2;
			busSpecimen.InputCode = Input.GetStringSpellCode(text);
			if (busSpecimen.InputCode.Length > 10)
			{
				busSpecimen.InputCode = busSpecimen.InputCode.Substring(0, 10);
			}
			int num = PEIS.BLL.CommonConfig.Instance.SaveSpecimen(busSpecimen);
			int num2 = (@int > 0) ? @int : num;
			string text3 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text3,
					"样本 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text3,
					"样本失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			base.ClearCache_AllSpecimen();
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleSpecimenInfo()
		{
			int @int = base.GetInt("ID_Specimen", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Specimen", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleSpecimenInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SearchExamTypeList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchExamTypeKeyword").Trim();
			int int3 = base.GetInt("SelectedConclusioTypeID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesExamTypeList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesExamTypeListByName_Param";
				array[0] = new SqlConditionInfo("@ExamTypeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveExamTypeInfo()
		{
			int @int = base.GetInt("ExamTypeID", 0);
			string text = base.GetString("ExamTypeName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("DocumentID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ExamTypeName", text, TypeCode.String)
			};
			string querySqlCode = "QueryExamTypeNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
						this.OutPutMessage("-1");
						return;
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			PEIS.Model.DictExamType dictExamType;
			if (@int <= 0)
			{
                dictExamType = new PEIS.Model.DictExamType();
			}
			else
			{
                dictExamType = PEIS.BLL.DictExamType.Instance.GetModel(@int);
			}
            dictExamType.ExamTypeID = @int;
            dictExamType.ExamTypeName = text;
            dictExamType.DocumentID = new int?(int2);
            dictExamType.InputCode = Input.GetStringSpellCode(text);
			int num = PEIS.BLL.CommonConfig.Instance.SaveDictExamType(dictExamType);
			int num2 = (@int > 0) ? @int : num;
			string text2 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text2,
					"体检类型 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text2,
					"体检类型失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			base.ClearCache_AllExamType();
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleExamTypeInfo()
		{
			int @int = base.GetInt("ExamTypeID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ExamTypeID", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleExamTypeInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage(e.Message);
			}
		}

		public void SearchArcCustomerList()
		{
			int @int = base.GetInt("ID_Gender", 0);
			string text = base.GetString("CustomerName");
			string text2 = base.GetString("IDCard");
			string text3 = base.GetString("BirthDay");
			text = Input.URLDecode(text).Trim();
			text2 = Input.URLDecode(text2).Trim();
			text3 = Input.URLDecode(text3).Trim();
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Gender", @int, TypeCode.Int32),
				new SqlConditionInfo("@CustomerName", text, TypeCode.String),
				new SqlConditionInfo("@IDCard", text2, TypeCode.String),
				new SqlConditionInfo("@BirthDay", text3, TypeCode.String)
			};
			string querySqlCode = "SearchArcCustomerList_Param";
			if (string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				querySqlCode = "SearchArcCustomerList_BirthDay_IDCard_Param";
			}
			else if (!string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3))
			{
				querySqlCode = "SearchArcCustomerList_IDCard_Param";
			}
			else if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				querySqlCode = "SearchArcCustomerList_BirthDay_IDCard_Param";
			}
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void GetArcCustomerInfoByIDs()
		{
			string @string = base.GetString("CustomerIDs");
			if (!string.IsNullOrEmpty(@string))
			{
				try
				{
					SqlConditionInfo[] conditions = new SqlConditionInfo[]
					{
						new SqlConditionInfo("@ID_ArcCustomers", @string.Replace("_", ","), TypeCode.Object)
					};
					string querySqlCode = "GetArcCustomerInfoByIDs_Param";
					DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
					if (dataSet.Tables[0].Rows.Count > 0)
					{
						int count = dataSet.Tables[0].Columns.Count;
						if (dataSet.Tables[0].Columns.Contains("Photo"))
						{
							dataSet.Tables[0].Columns.Add("Base64Photo");
							dataSet.Tables[0].Columns.Add("FilePath");
							foreach (DataRow dataRow in dataSet.Tables[0].Rows)
							{
								if (!Convert.IsDBNull(dataRow["Photo"]))
								{
									dataRow["Base64Photo"] = Convert.ToBase64String((byte[])dataRow["Photo"]);
								}
							}
						}
					}
					string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet);
					this.OutPutMessage(msg);
				}
				catch (Exception e)
				{
					this.OutPutMessage("");
				}
			}
		}

		public void MergeCustExamInfo()
		{
			string @string = base.GetString("ID_ArcCustomer_01");
			string string2 = base.GetString("ID_ArcCustomer_02");
			int @int = base.GetInt("MergeFlag", 0);
			if (@int != 0)
			{
				string text = "";
				string text2 = "";
				if (@int == 1)
				{
					text = @string;
					text2 = string2;
				}
				else
				{
					text = string2;
					text2 = @string;
				}
				int num = 0;
				try
				{
					int.TryParse(ConfigurationManager.AppSettings["MaxHisFileCode"], out num);
				}
				catch (Exception e)
				{
					num = 0;
				}
				string[] array = new string[num + 2];
				array[0] = PubConstant.GetConnectionString("ConnectionString");
				array[1] = PubConstant.GetConnectionString("ToOffLineConnectionString");
				if (num > 0)
				{
					for (int i = 1; i <= num; i++)
					{
						array[i + 1] = PubConstant.GetConnectionString("FYH_HisFile" + i.ToString().PadLeft(3, '0'));
					}
				}
				int num2 = PEIS.BLL.CommonConfig.Instance.MergeCustExamInfo(text, text2, array);
				if (num2 > 0)
				{
					Log4J.Instance.Info(string.Concat(new string[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",合并信息关联成功，原存档ID:",
						text,
						" 合并后存档ID:",
						text2
					}));
				}
				else
				{
					Log4J.Instance.Info(string.Concat(new string[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",合并信息关联失败，原存档ID:",
						text,
						" 合并后存档ID:",
						text2
					}));
				}
				this.OutPutMessage(num2.ToString());
			}
		}

		public void SearchConclusionTypeList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchConclusionTypeKeyword").Trim();
			int int3 = base.GetInt("SelectedConclusioTypeID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesConclusionTypeList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesConclusionTypeListByName_Param";
				array[0] = new SqlConditionInfo("@ConclusionTypeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveConclusionTypeInfo()
		{
			int @int = base.GetInt("ID_ConclusionType", 0);
			string text = base.GetString("ConclusionTypeName");
			text = Input.URLDecode(text);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text2 = base.GetString("BanDescribe");
			text2 = Input.URLDecode(text2);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ConclusionTypeName", text, TypeCode.String)
			};
			string querySqlCode = "QueryConclusionTypeNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
                        this.OutPutMessage("-1");
                    }
				}
			}
			catch (Exception var_8_121)
			{
				return;
			}
			PEIS.Model.BusConclusionType busConclusionType;
			if (@int <= 0)
			{
				busConclusionType = new PEIS.Model.BusConclusionType();
			}
			else
			{
				busConclusionType = PEIS.BLL.BusConclusionType.Instance.GetModel(@int);
			}
			busConclusionType.ID_ConclusionType = @int;
			busConclusionType.ConclusionTypeName = text;
			if (busConclusionType.Is_Banned != flag && flag)
			{
				busConclusionType.ID_BanOpr = new int?(this.LoginUserModel.UserID);
				busConclusionType.BanOperator = this.LoginUserModel.UserName;
				busConclusionType.BanDate = new DateTime?(DateTime.Now);
			}
			busConclusionType.Is_Banned = new bool?(flag);
			if (flag)
			{
				busConclusionType.BanDescribe = text2;
			}
			else
			{
				busConclusionType.BanDescribe = "";
			}
			busConclusionType.InputCode = Input.GetStringSpellCode(text);
			if (busConclusionType.InputCode.Length > 10)
			{
				busConclusionType.InputCode = busConclusionType.InputCode.Substring(0, 10);
			}
			int num = PEIS.BLL.CommonConfig.Instance.SaveConclusionType(busConclusionType);
			int num2 = (@int > 0) ? @int : num;
			string text3 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text3,
					"结论词分类 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text3,
					"结论词分类失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			base.ClearCache_AllConclusionType();
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleConclusionTypeInfo()
		{
			int @int = base.GetInt("ID_ConclusionType", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ConclusionType", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleConclusionTypeInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SearchICDList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchICDKeyword").Trim();
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesICDList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesICDListByName_Param";
				array[0] = new SqlConditionInfo("@ICDCNName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveICDInfo()
		{
			int @int = base.GetInt("ID_ICD", 0);
			string text = base.GetString("ICDCNName");
			text = Input.URLDecode(text);
			string text2 = base.GetString("ICDENName");
			text2 = Input.URLDecode(text2);
			string text3 = base.GetString("Code");
			text3 = Input.URLDecode(text3);
			string text4 = base.GetString("Codea");
			text4 = Input.URLDecode(text4);
			int int2 = base.GetInt("LevelA", 0);
			int int3 = base.GetInt("LevelB", 0);
			int int4 = base.GetInt("LevelC", 0);
			int int5 = base.GetInt("LevelD", 0);
			int int6 = base.GetInt("LevelE", 0);
			int int7 = base.GetInt("LevelTree", 0);
			string text5 = base.GetString("Class");
			text5 = Input.URLDecode(text5);
			string text6 = base.GetString("Tag");
			text6 = Input.URLDecode(text6);
			string text7 = base.GetString("Note");
			text7 = Input.URLDecode(text7);
			string text8 = base.GetString("ICDtoSection");
			text8 = Input.URLDecode(text8);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text9 = base.GetString("BanDescribe");
			text9 = Input.URLDecode(text9);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ICDCNName", text, TypeCode.String)
			};
			string querySqlCode = "QueryICDNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
                        this.OutPutMessage("-1");
                    }
				}
			}
			catch (Exception var_21_20D)
			{
				return;
			}
			PEIS.Model.DctICDTen dctICDTen;
			if (@int <= 0)
			{
				dctICDTen = new PEIS.Model.DctICDTen();
			}
			else
			{
				dctICDTen = PEIS.BLL.DctICDTen.Instance.GetModel(@int);
			}
			dctICDTen.ID_ICD = @int;
			dctICDTen.ICDCNName = text;
			dctICDTen.ICDENName = text2;
			dctICDTen.Code = text3;
			dctICDTen.Codea = text4;
			dctICDTen.LevelA = new int?(int2);
			dctICDTen.LevelB = new int?(int3);
			dctICDTen.LevelC = new int?(int4);
			dctICDTen.LevelD = new int?(int5);
			dctICDTen.LevelE = new int?(int6);
			dctICDTen.LevelTree = new int?(int7);
			dctICDTen.Class = text5;
			dctICDTen.Tag = text6;
			dctICDTen.Note = text7;
			dctICDTen.ICDtoSection = text8;
			dctICDTen.InputCode = Input.GetStringSpellCode(text);
			if (dctICDTen.Is_Banned != flag && flag)
			{
				dctICDTen.ID_BanOpr = new int?(this.LoginUserModel.UserID);
				dctICDTen.BanOperator = this.LoginUserModel.UserName;
				dctICDTen.BanDate = new DateTime?(DateTime.Now);
			}
			dctICDTen.Is_Banned = new bool?(flag);
			if (flag)
			{
				dctICDTen.BanDescribe = text9;
			}
			else
			{
				dctICDTen.BanDescribe = "";
			}
			int num = PEIS.BLL.CommonConfig.Instance.SaveICD(dctICDTen);
			int num2 = (@int > 0) ? @int : num;
			string text10 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text10,
					"ICD 中文名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text10,
					"ICD失败 中文名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleICDInfo()
		{
			int @int = base.GetInt("ID_ICD", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ICD", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleICDInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SearchFinalConclusionTypeList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchFinalConclusionTypeKeyword").Trim();
			int int3 = base.GetInt("SelectedConclusioTypeID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesFinalConclusionTypeList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesFinalConclusionTypeListByName_Param";
				array[0] = new SqlConditionInfo("@FinalConclusionTypeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveFinalConclusionTypeInfo()
		{
			int @int = base.GetInt("ID_FinalConclusionType", 0);
			string text = base.GetString("FinalConclusionTypeName");
			text = Input.URLDecode(text);
			string text2 = base.GetString("FinalConclusionSignCode");
			text2 = Input.URLDecode(text2);
			int int2 = base.GetInt("DispOrder", 500);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text3 = base.GetString("BanDescribe");
			text3 = Input.URLDecode(text3);
			string text4 = base.GetString("Note");
			text4 = Input.URLDecode(text4);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@FinalConclusionTypeName", text, TypeCode.String)
			};
			string querySqlCode = "QueryFinalConclusionTypeNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
                        this.OutPutMessage("-1");
                    }
				}
			}
			catch (Exception var_11_15D)
			{
				return;
			}
			PEIS.Model.DctFinalConclusionType dctFinalConclusionType;
			if (@int <= 0)
			{
				dctFinalConclusionType = new PEIS.Model.DctFinalConclusionType();
				dctFinalConclusionType.ID_Creator = new int?(this.LoginUserModel.UserID);
				dctFinalConclusionType.CreateDate = new DateTime?(DateTime.Now);
			}
			else
			{
				dctFinalConclusionType = PEIS.BLL.DctFinalConclusionType.Instance.GetModel(@int);
			}
			dctFinalConclusionType.ID_FinalConclusionType = @int;
			dctFinalConclusionType.FinalConclusionTypeName = text;
			dctFinalConclusionType.FinalConclusionSignCode = text2;
			if (dctFinalConclusionType.Is_Banned != flag && flag)
			{
				dctFinalConclusionType.ID_BanOpr = new int?(this.LoginUserModel.UserID);
				dctFinalConclusionType.BanOperator = this.LoginUserModel.UserName;
				dctFinalConclusionType.BanDate = new DateTime?(DateTime.Now);
			}
			dctFinalConclusionType.Is_Banned = new bool?(flag);
			dctFinalConclusionType.Note = text4;
			dctFinalConclusionType.DispOrder = int2;
			if (flag)
			{
				dctFinalConclusionType.BanDescribe = text3;
			}
			else
			{
				dctFinalConclusionType.BanDescribe = "";
			}
			dctFinalConclusionType.InputCode = Input.GetStringSpellCode(text);
			int num = PEIS.BLL.CommonConfig.Instance.SaveFinalConclusionType(dctFinalConclusionType);
			int num2 = (@int > 0) ? @int : num;
			string text5 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text5,
					"结论类型 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text5,
					"结论类型失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			base.ClearCache_AllFinalConclusionType();
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleFinalConclusionTypeInfo()
		{
			int @int = base.GetInt("ID_FinalConclusionType", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_FinalConclusionType", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleFinalConclusionTypeInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SearchExamPlaceList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchExamPlaceKeyword").Trim();
			int int3 = base.GetInt("SelectedConclusioTypeID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesExamPlaceList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesExamPlaceListByName_Param";
				array[0] = new SqlConditionInfo("@ExamPlaceName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveExamPlaceInfo()
		{
			int @int = base.GetInt("ExamPlaceID", 0);
			string text = base.GetString("ExamPlaceName");
			text = Input.URLDecode(text);
			int int2 = base.GetInt("DispOrder", 500);
			int int3 = base.GetInt("Default", 0);
			string input = base.GetString("LisExamPlaceName");
			input = Input.URLDecode(input);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ExamPlaceName", text, TypeCode.String)
			};
			string querySqlCode = "QueryExamPlaceNameIsExis_Param";
			try
			{
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
                        this.OutPutMessage("-1");
                    }
				}
			}
			catch (Exception ex)
			{
				return;
			}
			PEIS.Model.DictExamPlace busExamPlace;
			if (@int <= 0)
			{
				busExamPlace = new PEIS.Model.DictExamPlace();
			}
			else
			{
				busExamPlace = PEIS.BLL.DictExamPlace.Instance.GetModel(@int);
			}
			busExamPlace.ExamPlaceID = @int;
			busExamPlace.ExamPlaceName = text;
			busExamPlace.Default =(int3 == 1 ? true : false);
            busExamPlace.InputCode = Input.GetStringSpellCode(text);
			if (busExamPlace.InputCode.Length > 10)
			{
				busExamPlace.InputCode = busExamPlace.InputCode.Substring(0, 10);
			}
			int num = PEIS.BLL.CommonConfig.Instance.SaveExamPlace(busExamPlace);
			int num2 = (@int > 0) ? @int : num;
			string text2 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text2,
					"体检地址 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text2,
					"体检地址失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			base.ClearCache_AllExamPlace();
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleExamPlaceInfo()
		{
			int @int = base.GetInt("ExamPlaceID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ExamPlaceID", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleExamPlaceInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SearchFeeReportList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchFeeKeyword").Trim();
			int int3 = base.GetInt("SelectedSectionID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesFeeReportList_Param";
			if (!string.IsNullOrEmpty(text) && int3 <= 0)
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesSectionFeeReportListParamByName";
				array[0] = new SqlConditionInfo("@FeeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			if (int3 > 0 && string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesFeeReportListParamBySection";
				array[0] = new SqlConditionInfo("@ID_Section", int3, TypeCode.Int32);
				array[0].Place = 2;
			}
			if (int3 > 0 && !string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[2];
				pageCode = "QueryPagesFeeReportListParamByNameAndSection";
				array[0] = new SqlConditionInfo("@FeeName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
				array[1] = new SqlConditionInfo("@ID_Section", int3, TypeCode.Int32);
				array[1].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveFeeReportInfo()
		{
			int @int = base.GetInt("ID_FeeReport", 0);
			int int2 = base.GetInt("ID_Fee", 0);
			string text = base.GetString("FeeName");
			text = Input.URLDecode(text);
			string text2 = base.GetString("ReportKey");
			text2 = Input.URLDecode(text2);
			string text3 = base.GetString("Note");
			text3 = Input.URLDecode(text3);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text4 = base.GetString("BanDescribe");
			text4 = Input.URLDecode(text4);
			string querySqlCode = "QueryFeeIsExis_Param";
			try
			{
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@ID_Fee", int2, TypeCode.Int32)
				};
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
                        this.OutPutMessage("-1");
                    }
				}
			}
			catch (Exception ex)
			{
				return;
			}
			string text5 = "";
			string @string = base.GetString("ImageUrl");
			if (!string.IsNullOrEmpty(@string))
			{
				try
				{
					byte[] bytes = Convert.FromBase64String(@string);
					Image image = Public.ReadImage(bytes);
					Bitmap bitmap = new Bitmap(image);
					image.Dispose();
					string physicalApplicationPath = base.Request.PhysicalApplicationPath;
					text5 = "Images\\" + DateTime.Now.ToString("yyyyMM") + "\\Report";
					string str = Secret.MD5.Encrypt(DateTime.Now.ToString()) + ".jpg";
					if (!Directory.Exists(physicalApplicationPath + text5))
					{
						Directory.CreateDirectory(physicalApplicationPath + text5);
					}
					text5 = text5 + "\\" + str;
					bitmap.Save(physicalApplicationPath + text5, ImageFormat.Jpeg);
					text5 = "\\" + text5;
					text5 = text5.Replace("\\", "/");
					bitmap.Dispose();
				}
				catch (Exception ex)
				{
					text5 = ex.Message;
				}
			}
			PEIS.Model.BusFeeReport busFeeReport;
			if (@int <= 0)
			{
				busFeeReport = new PEIS.Model.BusFeeReport();
				busFeeReport.OperateType = new int?(1);
			}
			else
			{
				busFeeReport = PEIS.BLL.BusFeeReport.Instance.GetModel(@int);
				busFeeReport.OperateType = new int?(2);
			}
			busFeeReport.ID_FeeReport = @int;
			busFeeReport.ID_Fee = new int?(int2);
			busFeeReport.FeeName = text;
			busFeeReport.ReportKey = text2;
			busFeeReport.Note = text3;
			if (!string.IsNullOrEmpty(text5))
			{
				busFeeReport.ImageUrl = text5;
			}
			busFeeReport.ID_Operator = new int?(this.LoginUserModel.UserID);
			busFeeReport.Operator = this.LoginUserModel.UserName;
			busFeeReport.OperateDate = new DateTime?(DateTime.Now);
			busFeeReport.Is_Banned = new bool?(flag);
			if (flag)
			{
				busFeeReport.BanDescribe = text4;
			}
			else
			{
				busFeeReport.BanDescribe = "";
			}
			int num = PEIS.BLL.CommonConfig.Instance.SaveFeeReport(busFeeReport);
			int num2 = (@int > 0) ? @int : num;
			string text6 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text6,
					"收费项目报告 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text6,
					"收费项目报告失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleFeeReportInfo()
		{
			int @int = base.GetInt("ID_FeeReport", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_FeeReport", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleFeeReportInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SearchExamItemGroupList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchExamItemGroupKeyword").Trim();
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesExamItemGroupList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesExamItemGroupListParamByName";
				array[0] = new SqlConditionInfo("@ExamItemGroupName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = PEIS.BLL.CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveExamItemGroupInfo()
		{
			int @int = base.GetInt("ID_ExamItemGroup", 0);
			int int2 = base.GetInt("ID_Fee", 0);
			string text = base.GetString("ExamItemGroupName");
			text = Input.URLDecode(text);
			string stringSpellCode = Input.GetStringSpellCode(text);
			int int3 = base.GetInt("DispOrder", 0);
			string text2 = base.GetString("Note");
			text2 = Input.URLDecode(text2);
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text3 = base.GetString("BanDescribe");
			text3 = Input.URLDecode(text3);
			string querySqlCode = "QueryFeeIsExis_Param";
			try
			{
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@ID_Fee", int2, TypeCode.Int32)
				};
				DataSet dataSet = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
                        this.OutPutMessage("-1");
                    }
				}
			}
			catch (Exception ex)
			{
				return;
			}
			PEIS.Model.BusExamItemGroup busExamItemGroup;
			if (@int <= 0)
			{
				busExamItemGroup = new PEIS.Model.BusExamItemGroup();
				busExamItemGroup.OperateType = new int?(1);
			}
			else
			{
				busExamItemGroup = PEIS.BLL.BusExamItemGroup.Instance.GetModel(@int);
				busExamItemGroup.OperateType = new int?(2);
			}
			busExamItemGroup.ID_ExamItemGroup = @int;
			busExamItemGroup.ExamItemGroupName = text;
			busExamItemGroup.InputCode = stringSpellCode;
			busExamItemGroup.DispOrder = int3;
			busExamItemGroup.Note = text2;
			busExamItemGroup.ID_Operator = new int?(this.LoginUserModel.UserID);
			busExamItemGroup.Operator = this.LoginUserModel.UserName;
			busExamItemGroup.OperateDate = new DateTime?(DateTime.Now);
			busExamItemGroup.Is_Banned = new bool?(flag);
			if (flag)
			{
				busExamItemGroup.BanDescribe = text3;
			}
			else
			{
				busExamItemGroup.BanDescribe = "";
			}
			int num = PEIS.BLL.CommonConfig.Instance.SaveExamItemGroup(busExamItemGroup);
			int num2 = (@int > 0) ? @int : num;
			string text4 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text4,
					"检查项目分组 名称：",
					text,
					",编号：",
					num2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text4,
					"检查项目分组失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		public void GetSingleExamItemGroupInfo()
		{
			int @int = base.GetInt("ID_ExamItemGroup", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ExamItemGroup", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleExamItemGroupInfo_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void GetExamDetailListByExamItemGroup()
		{
			int @int = base.GetInt("ExamItemGroupID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_ExamItemGroup", @int, TypeCode.Int32)
			};
			string querySqlCode = "QueryExamDetailListByExamItemGroup_Param";
			try
			{
				DataSet ds = PEIS.BLL.CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void SaveExamItemGroupExamRel()
		{
			int @int = base.GetInt("ID_ExamItemGroup", 0);
			string @string = base.GetString("newExamItemIDStrs");
			int num = PEIS.BLL.CommonConfig.Instance.SaveExamItemGroupExamRel(@int, @string);
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",保存检查项目分组明细 ID_ExamItemGroup：",
					@int,
					",检查项目编号：",
					@string
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",保存检查项目分组明细失败 ID_ExamItemGroup：",
					@int,
					",检查项目编号：",
					@string
				}));
			}
			this.OutPutMessage(num.ToString());
		}
	}
}
