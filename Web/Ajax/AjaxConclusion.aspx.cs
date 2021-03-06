using PEIS.Base;
using PEIS.Common;
using PEIS.BLL;
using PEIS.Model;
using Maticsoft.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace PEIS.Web.Ajax
{
	public class AjaxConclusion : BasePage
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

		public void GetAllConclusionList()
		{
			string keyword = "";
			DataTable conclusionByKeyWord = CommonConclusion.Instance.GetConclusionByKeyWord(keyword);
			string msg = JsonHelperFont.Instance.DataTableToJSON(conclusionByKeyWord, conclusionByKeyWord.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void GetConclusionByKeyWords()
		{
			DateTime now = DateTime.Now;
			string text = base.GetString("InputCode");
			text = Input.URLDecode(text).Trim();
			DataTable conclusionByKeyWord = CommonConclusion.Instance.GetConclusionByKeyWord(text);
			string msg = JsonHelperFont.Instance.DataTableToJSON(conclusionByKeyWord, conclusionByKeyWord.Rows.Count);
			DateTime now2 = DateTime.Now;
			string dateDiff = Public.GetDateDiff("用输入码【" + text + "】查询结论词", now, now2);
			Log4J.Instance.Debug(string.Concat(new string[]
			{
				Public.GetClientIP(),
				",",
				this.LoginUserModel.UserName,
				",",
				dateDiff
			}));
			this.OutPutMessage(msg);
		}

		public void GetConclusionAllContentByIDs()
		{
			DateTime now = DateTime.Now;
			string @string = base.GetString("IDList");
			string string2 = base.GetString("ExistIDList");
			DataTable conclusionAllContentByIDs = CommonConclusion.Instance.GetConclusionAllContentByIDs(@string, string2);
			string msg = JsonHelperFont.Instance.DataTableToJSON(conclusionAllContentByIDs, conclusionAllContentByIDs.Rows.Count);
			DateTime now2 = DateTime.Now;
			string dateDiff = Public.GetDateDiff("获取结论词内容", now, now2);
			Log4J.Instance.Debug(string.Concat(new string[]
			{
				Public.GetClientIP(),
				",",
				this.LoginUserModel.UserName,
				",",
				dateDiff
			}));
			this.OutPutMessage(msg);
		}

		public void GetCustExamSectionItem()
		{
			string @string = base.GetString("type");
			long @int = base.GetInt64("CustomerID", 0L);
			string text = string.Empty;
			if (@string.ToLower() == "printview")
			{
				string sql = string.Format("SELECT ID_ArcCustomer,IDCardNo,ExamCardNo,ID_Customer,Is_CompletePhysical,ExamState FROM OnCustRelationCustPEInfo WHERE ID_Customer='{0}';", @int);
				DataTable dataTable = CommonExcuteSql.Instance.ExcuteSql(sql).Tables[0];
				int count = dataTable.Rows.Count;
				int num = -1;
				if (count > 0)
				{
					int.TryParse(dataTable.Rows[0]["ExamState"].ToString(), out num);
				}
				if (num == 0)
				{
					text = "ConnectionString";
				}
				else
				{
					if (num == -1)
					{
					}
					if (num == 1)
					{
						text = "ToOffLineConnectionString";
					}
					else
					{
						text = "FYH_HisFile" + (num - 1).ToString().PadLeft(3, '0');
					}
				}
			}
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64)
			};
			string querySqlCode = "QueryCust_ExamSectionList_Param";
			if (@int > 0L)
			{
				DataSet dataSet = null;
				if (string.IsNullOrEmpty(text))
				{
					dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				}
				else
				{
					dataSet = CommonConclusion.Instance.ExcuteQuerySqlX(text, querySqlCode, conditions);
				}
				foreach (DataRow dataRow in dataSet.Tables[0].Rows)
				{
					if (dataRow["SectionSummaryDate"] != null && !string.IsNullOrEmpty(dataRow["SectionSummaryDate"].ToString()))
					{
						dataRow["SectionSummaryDate"] = Convert.ToDateTime(dataRow["SectionSummaryDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
					}
				}
				dataSet.AcceptChanges();
				string text2 = JsonHelperFont.Instance.DataSetToJSON(dataSet);
				text2 = text2.Replace("\\n", "<br/>");
				this.OutPutMessage(text2);
			}
			else
			{
				this.OutPutMessage("");
			}
		}

		public void UpdateFinalConclusionSectionLock()
		{
			long @int = base.GetInt64("CustomerID", 0L);
			int int2 = base.GetInt("IsSectionLock", 0);
			if (int2 == 0)
			{
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64)
				};
				string querySqlCode = "QueryCustomerOCPEI_Param";
				if (@int > 0L)
				{
					DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
					if (dataSet.Tables[0].Rows.Count > 0)
					{
						if (dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString().ToLower() == "true")
						{
							this.OutPutMessage("-4");
							return;
						}
						if (dataSet.Tables[0].Rows[0]["Is_Checked"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_Checked"].ToString().ToLower() == "true")
						{
							this.OutPutMessage("-3");
							return;
						}
						if (dataSet.Tables[0].Rows[0]["Is_SectionLock"].ToString() != "1" && dataSet.Tables[0].Rows[0]["Is_SectionLock"].ToString().ToLower() != "true")
						{
							this.OutPutMessage("-2");
							return;
						}
					}
					else
					{
						DataSet custRelationCustPEInfo = CommonCustExam.Instance.GetCustRelationCustPEInfo(@int, "", "");
						if (custRelationCustPEInfo == null || 0 >= custRelationCustPEInfo.Tables[0].Rows.Count)
						{
							this.OutPutMessage("-1");
							return;
						}
						List<PEIS.Model.OnCustRelationCustPEInfo> list = PEIS.BLL.OnCustRelationCustPEInfo.Instance.DataTableToList(custRelationCustPEInfo.Tables[0]);
						if (list == null || list.Count <= 0)
						{
							this.OutPutMessage("-1");
							return;
						}
						if (list[0].ExamState != 0)
						{
							this.OutPutMessage("-101");
							return;
						}
					}
				}
			}
			base.ClearCache_CustRelationCustPEInfo(@int);
			PEIS.Model.OnCustPhysicalExamInfo onCustPhysicalExamInfo = new PEIS.Model.OnCustPhysicalExamInfo();
			onCustPhysicalExamInfo.ID_Customer = @int;
			onCustPhysicalExamInfo.Is_SectionLock = new bool?(int2 == 1);
			int num = CommonConclusion.Instance.UpdateFinalConclusionSectionLock(onCustPhysicalExamInfo);
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",修改分科锁定标志(IsSectionLock) 体检号：",
					onCustPhysicalExamInfo.ID_Customer,
					",锁定标志：",
					int2
				}));
			}
			else if (num == 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",分科锁定标志(IsSectionLock)已经是:",
					int2,
					",不需要修改，体检号：",
					onCustPhysicalExamInfo.ID_Customer,
					",锁定标志：",
					int2
				}));
			}
			else
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",修改分科锁定标志(IsSectionLock)失败 体检号：",
					onCustPhysicalExamInfo.ID_Customer,
					",锁定标志：",
					int2
				}));
			}
			this.OutPutMessage(num.ToString());
		}

		private int GetCustomerMaxDiseaseLevel(long ID_Customer)
		{
			int result = 0;
			try
			{
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@ID_Customer", ID_Customer, TypeCode.Int64)
				};
				string querySqlCode = "QueryCustomerDiseaseLevel_Param";
				if (ID_Customer > 0L)
				{
					DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
					if (dataSet.Tables[0].Rows.Count > 0)
					{
						string text = dataSet.Tables[0].Rows[0]["MaxDiseaseLevel"].ToString();
						if (!string.IsNullOrEmpty(text))
						{
							result = int.Parse(text);
						}
					}
				}
			}
			catch (Exception var_5_AB)
			{
			}
			return result;
		}

		public void GetSymptomConnectConclusion()
		{
			DateTime now = DateTime.Now;
			long @int = base.GetInt64("CustomerID", 0L);
			try
			{
				SqlConditionInfo[] conditions = new SqlConditionInfo[]
				{
					new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64)
				};
				string querySqlCode = "QuerySymptomConnectConclusion_Param";
				if (@int > 0L)
				{
					DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
					string msg = JsonHelperFont.Instance.DataTableToJSON(dataSet.Tables[0], dataSet.Tables[0].Rows.Count);
					DateTime now2 = DateTime.Now;
					string dateDiff = Public.GetDateDiff("根据体征词自动关联结论词", now, now2);
					Log4J.Instance.Debug(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",",
						@int,
						",",
						dateDiff
					}));
					this.OutPutMessage(msg);
				}
			}
			catch (Exception ex)
			{
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					@int,
					",根据体征词自动关联结论词:",
					ex.Message
				}));
			}
		}

		public void SaveCustomerFinalConclusion()
		{
			DateTime now = DateTime.Now;
			int @int = base.GetInt("Is_FinalFinished", 0);
			long int2 = base.GetInt64("CustomerID", 0L);
			string @string = base.GetString("txtConclusionFinalConclusion");
			string string2 = base.GetString("txtConclusionFinalSportsGuide");
			string string3 = base.GetString("txtConclusionFinalDietGuide");
			string string4 = base.GetString("txtConclusionFinalHealthKnowlage");
			string string5 = base.GetString("txtConclusionMainDiagnose");
			string string6 = base.GetString("txtConclusionSecondaryDiagnose");
			string string7 = base.GetString("txtConclusionIndicatorDiagnose");
			string string8 = base.GetString("txtConclusionNormalDiagnose");
			string string9 = base.GetString("txtConclusionOtherDiagnose");
			string string10 = base.GetString("txtConclusionResultCompare");
			string string11 = base.GetString("txtConclusionFinalOverView");
			string string12 = base.GetString("FinalSelectedConclusionDataParams");
			PEIS.Model.OnCustPhysicalExamInfo onCustPhysicalExamInfo = new PEIS.Model.OnCustPhysicalExamInfo();
			onCustPhysicalExamInfo.ID_FinalDoctor = new int?(this.LoginUserModel.UserID);
			onCustPhysicalExamInfo.FinalDoctor = this.LoginUserModel.UserName;
			onCustPhysicalExamInfo.FinalDate = new DateTime?(DateTime.Now);
			onCustPhysicalExamInfo.ID_Customer = int2;
			onCustPhysicalExamInfo.DiseaseLevel = new int?(this.GetCustomerMaxDiseaseLevel(int2));
			onCustPhysicalExamInfo.FinalConclusion = Input.URLDecode(@string);
			onCustPhysicalExamInfo.ResultCompare = Input.URLDecode(string10);
			onCustPhysicalExamInfo.MainDiagnose = Input.URLDecode(string5);
			onCustPhysicalExamInfo.SecondaryDiagnose = Input.URLDecode(string6);
			onCustPhysicalExamInfo.IndicatorDiagnose = Input.URLDecode(string7);
			onCustPhysicalExamInfo.NormalDiagnose = Input.URLDecode(string8);
			onCustPhysicalExamInfo.OtherDiagnose = Input.URLDecode(string9);
			onCustPhysicalExamInfo.FinalDietGuide = Input.URLDecode(string3);
			onCustPhysicalExamInfo.FinalSportGuide = Input.URLDecode(string2);
			onCustPhysicalExamInfo.FinalHealthKnowlage = Input.URLDecode(string4);
			onCustPhysicalExamInfo.FinalOverView = Input.URLDecode(string11);
			onCustPhysicalExamInfo.FinalConclusion = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.FinalConclusion);
			onCustPhysicalExamInfo.ResultCompare = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.ResultCompare);
			onCustPhysicalExamInfo.MainDiagnose = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.MainDiagnose);
			onCustPhysicalExamInfo.SecondaryDiagnose = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.SecondaryDiagnose);
			onCustPhysicalExamInfo.IndicatorDiagnose = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.IndicatorDiagnose);
			onCustPhysicalExamInfo.NormalDiagnose = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.NormalDiagnose);
			onCustPhysicalExamInfo.OtherDiagnose = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.OtherDiagnose);
			onCustPhysicalExamInfo.FinalDietGuide = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.FinalDietGuide);
			onCustPhysicalExamInfo.FinalSportGuide = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.FinalSportGuide);
			onCustPhysicalExamInfo.FinalHealthKnowlage = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.FinalHealthKnowlage);
			onCustPhysicalExamInfo.FinalOverView = Secret.AES.EncryptPrefix(onCustPhysicalExamInfo.FinalOverView);
			onCustPhysicalExamInfo.Is_FinalFinished = new bool?(@int == 1);
			onCustPhysicalExamInfo.Is_SectionLock = new bool?(true);
			List<PEIS.Model.OnCustConclusion> list = null;
			if (!string.IsNullOrEmpty(string12))
			{
				string[] array = string12.Split(new char[]
				{
					'|'
				});
				if (array.Length > 0)
				{
					list = new List<PEIS.Model.OnCustConclusion>();
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split(new char[]
						{
							'、'
						});
						if (array2.Length >= 9)
						{
							PEIS.Model.OnCustConclusion onCustConclusion = new PEIS.Model.OnCustConclusion();
							if (!string.IsNullOrEmpty(array2[0].ToString()))
							{
								onCustConclusion.ID_CustConclusion = int.Parse(array2[0].ToString());
							}
							onCustConclusion.ID_Conclusion = new int?(int.Parse(array2[1].ToString()));
							onCustConclusion.Is_NoEntrySuggestion = new bool?(array2[2].ToString() == "1");
							if (!string.IsNullOrEmpty(array2[3].ToString()))
							{
								onCustConclusion.DispOrder = new int?(int.Parse(array2[3].ToString()));
							}
							onCustConclusion.ConclusionTypeName = Input.URLDecode(array2[5].ToString());
							onCustConclusion.ConclusionName = Input.URLDecode(array2[4].ToString());
							onCustConclusion.Explanation = Input.URLDecode(array2[6].ToString());
							onCustConclusion.Suggestion = Input.URLDecode(array2[7].ToString());
							onCustConclusion.DietGuide = Input.URLDecode(array2[8].ToString());
							onCustConclusion.SportGuide = Input.URLDecode(array2[9].ToString());
							onCustConclusion.HealthKnowledge = Input.URLDecode(array2[10].ToString());
							onCustConclusion.DiagnoseType = new int?(int.Parse(array2[11].ToString()));
							onCustConclusion.ConclusionName = Secret.AES.EncryptPrefix(onCustConclusion.ConclusionName);
							onCustConclusion.Explanation = Secret.AES.EncryptPrefix(onCustConclusion.Explanation);
							onCustConclusion.Suggestion = Secret.AES.EncryptPrefix(onCustConclusion.Suggestion);
							onCustConclusion.DietGuide = Secret.AES.EncryptPrefix(onCustConclusion.DietGuide);
							onCustConclusion.SportGuide = Secret.AES.EncryptPrefix(onCustConclusion.SportGuide);
							onCustConclusion.HealthKnowledge = Secret.AES.EncryptPrefix(onCustConclusion.HealthKnowledge);
							onCustConclusion.ID_Customer = new long?(int2);
							onCustConclusion.ID_Doctor = new int?(this.LoginUserModel.UserID);
							onCustConclusion.DoctorName = this.LoginUserModel.UserName;
							onCustConclusion.ConclusionDate = DateTime.Now;
							list.Add(onCustConclusion);
						}
					}
				}
			}
			DateTime now2 = DateTime.Now;
			string dateDiff = Public.GetDateDiff(" Start 保存总检信息", now, now2);
			Log4J.Instance.Debug(string.Concat(new object[]
			{
				Public.GetClientIP(),
				",",
				this.LoginUserModel.UserName,
				",体检号：",
				int2,
				",",
				dateDiff
			}));
			try
			{
				int num = CommonConclusion.Instance.SaveFinalConclusionData(onCustPhysicalExamInfo, list);
				now2 = DateTime.Now;
				dateDiff = Public.GetDateDiff("保存总检信息", now, now2);
				if (num > 0)
				{
					if (@int == 1)
					{
						base.AddOrUpdateByBackLogType(int2, EnumBusBackLogType.总检, true, null);
					}
					base.ClearCache_CustRelationCustPEInfo(int2);
					Log4J.Instance.Info(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",体检号：",
						onCustPhysicalExamInfo.ID_Customer,
						"成功 ",
						dateDiff,
						"，结论汇总：",
						onCustPhysicalExamInfo.FinalConclusion
					}));
				}
				else
				{
					Log4J.Instance.Error(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",体检号：",
						onCustPhysicalExamInfo.ID_Customer,
						"失败 ",
						dateDiff,
						"，结论汇总：",
						onCustPhysicalExamInfo.FinalConclusion
					}));
				}
				this.OutPutMessage(num.ToString());
			}
			catch (Exception ex)
			{
				now2 = DateTime.Now;
				dateDiff = Public.GetDateDiff("保存总检出错了，", now, now2);
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",体检号：",
					int2,
					",",
					dateDiff,
					",错误提示：",
					ex.Message
				}));
				this.OutPutMessage("-1");
			}
		}

		public void SaveCustomerFinalCheck()
		{
			DateTime now = DateTime.Now;
			long @int = base.GetInt64("CustomerID", 0L);
			string @string = base.GetString("CustomerName");
			int int2 = base.GetInt("IsChecked", 0);
			string string2 = base.GetString("RefuseReason");
			string string3 = base.GetString("FinalDateDetail");
			string string4 = base.GetString("FinalDoctor");
			int int3 = base.GetInt("ID_FinalDoctor", 0);
			PEIS.Model.OnFianlCheck onFianlCheck = new PEIS.Model.OnFianlCheck();
			onFianlCheck.ID_Customer = new long?(@int);
			onFianlCheck.CustomerName = Input.URLDecode(@string);
			onFianlCheck.Is_Pass = new bool?(int2 == 1);
			onFianlCheck.ID_FinalDoctor = new int?(int3);
			onFianlCheck.FinalDoctor = Input.URLDecode(string4);
			onFianlCheck.SubmitDate = new DateTime?((string3 == "") ? DateTime.Now : DateTime.Parse(Input.URLDecode(string3)));
			onFianlCheck.ID_FinalCheckDoctor = new int?(this.LoginUserModel.UserID);
			onFianlCheck.FinalCheckDoctor = this.LoginUserModel.UserName;
			onFianlCheck.FinaleCheckDate = new DateTime?(DateTime.Now);
			onFianlCheck.RefuseReason = ((int2 == 0) ? Input.URLDecode(string2) : "");
			try
			{
				int num = CommonConclusion.Instance.SaveCustomerFinalCheck(onFianlCheck);
				DateTime now2 = DateTime.Now;
				string dateDiff = Public.GetDateDiff("保存总审结果", now, now2);
				if (num > 0)
				{
					base.AddOrUpdateByBackLogType(@int, EnumBusBackLogType.质量控制, true, null);
					Log4J.Instance.Info(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",体检号：",
						onFianlCheck.ID_Customer,
						",是否通过总审：",
						int2,
						",成功",
						dateDiff
					}));
				}
				else
				{
					Log4J.Instance.Error(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",体检号：",
						onFianlCheck.ID_Customer,
						",是否通过总审：",
						int2,
						",失败",
						dateDiff
					}));
				}
				this.OutPutMessage(num.ToString());
			}
			catch (Exception ex)
			{
				DateTime now2 = DateTime.Now;
				string dateDiff = Public.GetDateDiff("保存总审出错了，", now, now2);
				Log4J.Instance.Error(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",体检号：",
					@int,
					",",
					dateDiff,
					",错误提示：",
					ex.Message
				}));
				this.OutPutMessage("-1");
			}
		}

		public void GetConclusionInfoByCustomerID()
		{
			long @int = base.GetInt64("CustomerID", 0L);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64)
			};
			string querySqlCode = "QueryCustomerOCPEI_Param";
			if (@int > 0L)
			{
				DataSet ds = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
		}

		public void UpdateOnCustFinalUnCheck()
		{
			DateTime now = DateTime.Now;
			long @int = base.GetInt64("CustomerID", 0L);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64)
			};
			string querySqlCode = "QueryCustomerOCPEI_Param";
			if (@int > 0L)
			{
				DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					if (dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString().ToLower() == "true")
					{
						this.OutPutMessage("-3");
						return;
					}
					if (!(dataSet.Tables[0].Rows[0]["Is_Checked"].ToString() == "1") && !(dataSet.Tables[0].Rows[0]["Is_Checked"].ToString().ToLower() == "true"))
					{
						this.OutPutMessage("-2");
						return;
					}
				}
				else
				{
					DataSet custRelationCustPEInfo = CommonCustExam.Instance.GetCustRelationCustPEInfo(@int, "", "");
					if (custRelationCustPEInfo == null || 0 >= custRelationCustPEInfo.Tables[0].Rows.Count)
					{
						this.OutPutMessage("-1");
						return;
					}
					List<PEIS.Model.OnCustRelationCustPEInfo> list = PEIS.BLL.OnCustRelationCustPEInfo.Instance.DataTableToList(custRelationCustPEInfo.Tables[0]);
					if (list == null || list.Count <= 0)
					{
						this.OutPutMessage("-1");
						return;
					}
					if (list[0].ExamState != 0)
					{
						this.OutPutMessage("-101");
						return;
					}
				}
			}
			PEIS.Model.OnCustPhysicalExamInfo model = PEIS.BLL.OnCustPhysicalExamInfo.Instance.GetModel(@int);
			if (model != null)
			{
				PEIS.Model.OnFianlCheck onFianlCheck = new PEIS.Model.OnFianlCheck();
				onFianlCheck.ID_Customer = new long?(@int);
				onFianlCheck.CustomerName = model.CustomerName;
				onFianlCheck.Is_Pass = new bool?(false);
				onFianlCheck.ID_FinalDoctor = model.ID_FinalDoctor;
				onFianlCheck.FinalDoctor = model.FinalDoctor;
				onFianlCheck.SubmitDate = model.FinalDate;
				onFianlCheck.ID_FinalCheckDoctor = new int?(this.LoginUserModel.UserID);
				onFianlCheck.FinalCheckDoctor = this.LoginUserModel.UserName;
				onFianlCheck.FinaleCheckDate = new DateTime?(DateTime.Now);
				onFianlCheck.RefuseReason = "解除总审";
				try
				{
					int num = CommonConclusion.Instance.SaveCustomerFinaUnCheck(onFianlCheck);
					DateTime now2 = DateTime.Now;
					string dateDiff = Public.GetDateDiff("解除总审，", now, now2);
					Log4J.Instance.Info(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",体检号：",
						@int,
						",",
						dateDiff
					}));
					this.OutPutMessage(num.ToString());
				}
				catch (Exception ex)
				{
					DateTime now2 = DateTime.Now;
					string dateDiff = Public.GetDateDiff("解除总审出错了，", now, now2);
					Log4J.Instance.Error(string.Concat(new object[]
					{
						Public.GetClientIP(),
						",",
						this.LoginUserModel.UserName,
						",体检号：",
						@int,
						",",
						dateDiff,
						",错误提示：",
						ex.Message
					}));
					this.OutPutMessage("0");
				}
			}
			else
			{
				this.OutPutMessage("-1");
			}
		}

		public void UpdateCustomerNotFinalFinished()
		{
			long @int = base.GetInt64("CustomerID", 0L);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64)
			};
			string querySqlCode = "QueryCustomerOCPEI_Param";
			if (@int > 0L)
			{
				DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					if (dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString().ToLower() == "true")
					{
						this.OutPutMessage("-4");
						return;
					}
					if (dataSet.Tables[0].Rows[0]["Is_Checked"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_Checked"].ToString().ToLower() == "true")
					{
						this.OutPutMessage("-3");
						return;
					}
					if (!(dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString() == "1") && !(dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString().ToLower() == "true"))
					{
						this.OutPutMessage("-2");
						return;
					}
				}
				else
				{
					DataSet custRelationCustPEInfo = CommonCustExam.Instance.GetCustRelationCustPEInfo(@int, "", "");
					if (custRelationCustPEInfo == null || 0 >= custRelationCustPEInfo.Tables[0].Rows.Count)
					{
						this.OutPutMessage("-1");
						return;
					}
					List<PEIS.Model.OnCustRelationCustPEInfo> list = PEIS.BLL.OnCustRelationCustPEInfo.Instance.DataTableToList(custRelationCustPEInfo.Tables[0]);
					if (list == null || list.Count <= 0)
					{
						this.OutPutMessage("-1");
						return;
					}
					if (list[0].ExamState != 0)
					{
						this.OutPutMessage("-101");
						return;
					}
				}
			}
			PEIS.Model.OnCustPhysicalExamInfo onCustPhysicalExamInfo = new PEIS.Model.OnCustPhysicalExamInfo();
			onCustPhysicalExamInfo.ID_Customer = @int;
			onCustPhysicalExamInfo.Is_FinalFinished = new bool?(false);
			onCustPhysicalExamInfo.Is_Checked = new bool?(false);
			this.OutPutMessage(CommonConclusion.Instance.UpdateCustomerNotFinalFinished(onCustPhysicalExamInfo).ToString());
		}

		public void GetCustomerFinalExamList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
			string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
			int int3 = base.GetInt("FinalChecked", -1);
			int int4 = base.GetInt("FinalFinished", -1);
			int totalCount = 0;
			int num = 0;
			string pageCode = "QueryPagesFinalExamListParam";
			if (!string.IsNullOrEmpty(text))
			{
				text += " 00:00:00";
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text2 += " 23:59:59";
			}
			int int5 = base.GetInt("OnlyMySelf", -1);
			SqlConditionInfo[] array = new SqlConditionInfo[5];
			if (int5 >= 0)
			{
				array[0] = new SqlConditionInfo("@ID_FinalDoctor", this.LoginUserModel.UserID, TypeCode.Int32);
			}
			if (int4 >= 0)
			{
				if (!string.IsNullOrEmpty(text) && Input.IsDate(text))
				{
					array[1] = new SqlConditionInfo("@BeginDate", text, TypeCode.DateTime);
					array[1].Place = 2;
				}
				if (!string.IsNullOrEmpty(text2) && Input.IsDate(text2))
				{
					array[2] = new SqlConditionInfo("@EndDate", text2, TypeCode.DateTime);
					array[2].Place = 2;
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(text) && Input.IsDate(text))
				{
					array[1] = new SqlConditionInfo("@FinalDate", text, TypeCode.DateTime);
					array[1].ParamOper = ">=";
				}
				if (!string.IsNullOrEmpty(text2) && Input.IsDate(text2))
				{
					array[2] = new SqlConditionInfo("@FinalDate", text2, TypeCode.DateTime);
					array[2].ParamOper = "<=";
				}
			}
			if (int3 >= 0)
			{
				array[3] = new SqlConditionInfo("@Is_Checked", int3, TypeCode.Int32);
			}
			if (int4 >= 0)
			{
				pageCode = "QueryPagesFinalExamList_NoFinalFinished_Param";
				array[4] = new SqlConditionInfo("@Is_FinalFinished", int4, TypeCode.Int32);
				array[4].Place = 2;
			}
			DataTable page = CommonConclusion.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			foreach (DataRow dataRow in page.Rows)
			{
				if (dataRow["CheckedFormatDate"] != null && !string.IsNullOrEmpty(dataRow["CheckedFormatDate"].ToString()))
				{
					dataRow["CheckedFormatDate"] = Convert.ToDateTime(dataRow["CheckedFormatDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
				}
				if (dataRow["FinalFormatDate"] != null && !string.IsNullOrEmpty(dataRow["FinalFormatDate"].ToString()))
				{
					dataRow["FinalFormatDate"] = Convert.ToDateTime(dataRow["FinalFormatDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
				}
			}
			page.AcceptChanges();
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void GetCustomerFinalCheckList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			string text = base.Server.HtmlDecode(base.GetString("BeginExamDate"));
			string text2 = base.Server.HtmlDecode(base.GetString("EndExamDate"));
			int totalCount = 0;
			int num = 0;
			string pageCode = "QueryPagesFinalCheckListParam";
			if (!string.IsNullOrEmpty(text))
			{
				text += " 00:00:00";
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text2 += " 23:59:59";
			}
			int int3 = base.GetInt("OnlyMySelf", -1);
			SqlConditionInfo[] array = new SqlConditionInfo[3];
			if (int3 >= 0)
			{
				array[0] = new SqlConditionInfo("@ID_FinalDoctor", this.LoginUserModel.UserID, TypeCode.Int32);
			}
			if (!string.IsNullOrEmpty(text) && Input.IsDate(text))
			{
				array[1] = new SqlConditionInfo("@CheckedDate", text, TypeCode.DateTime);
				array[1].ParamOper = ">=";
			}
			if (!string.IsNullOrEmpty(text) && Input.IsDate(text))
			{
				array[2] = new SqlConditionInfo("@CheckedDate", text2, TypeCode.DateTime);
				array[2].ParamOper = "<=";
			}
			DataTable page = CommonConclusion.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			foreach (DataRow dataRow in page.Rows)
			{
				if (dataRow["CheckedFormatDate"] != null && !string.IsNullOrEmpty(dataRow["CheckedFormatDate"].ToString()))
				{
					dataRow["CheckedFormatDate"] = Convert.ToDateTime(dataRow["CheckedFormatDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
				}
				if (dataRow["FinalFormatDate"] != null && !string.IsNullOrEmpty(dataRow["FinalFormatDate"].ToString()))
				{
					dataRow["FinalFormatDate"] = Convert.ToDateTime(dataRow["FinalFormatDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
				}
			}
			page.AcceptChanges();
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void GetDoctorFinalNotCheckedList()
		{
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_FinalDoctor", this.LoginUserModel.UserID, TypeCode.Int32)
			};
			string querySqlCode = "QueryDoctorFinalNotCheckedList_Param";
			DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
			string msg = "";
			if (dataSet != null && dataSet.Tables.Count > 0)
			{
				msg = JsonHelperFont.Instance.DataTableToJSON(dataSet.Tables[0], dataSet.Tables[0].Rows.Count);
			}
			this.OutPutMessage(msg);
		}

		public void SearchConclusionList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchConclusionKeyword").Trim();
			int int3 = base.GetInt("SelectedConclusioTypeID", 0);
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesConclusionList_Param";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesConclusionListByName_Param";
				array[0] = new SqlConditionInfo("@ConclusionName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			if (int3 > 0 && string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesConclusionListByType_Param";
				array[0] = new SqlConditionInfo("@ID_ConclusionType", int3, TypeCode.Int32);
				array[0].Place = 2;
			}
			if (int3 > 0 && !string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[2];
				pageCode = "QueryPagesConclusionListByTypeAndName_Param";
				array[0] = new SqlConditionInfo("@ConclusionName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
				array[1] = new SqlConditionInfo("@ID_ConclusionType", int3, TypeCode.Int32);
				array[1].Place = 2;
			}
			DataTable page = CommonConclusion.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void SaveConclusionInfo()
		{
			int @int = base.GetInt("ID_Conclusion", 0);
			string text = base.GetString("ConclusionName");
			text = Input.URLDecode(text);
			string text2 = base.GetString("TeamConclusionName");
			text2 = Input.URLDecode(text2);
			int? iD_ConclusionType = new int?(base.GetInt("ID_ConclusionType", 0));
			string input = base.GetString("ConclusionTypeName");
			input = Input.URLDecode(input);
			int? iD_FinalConclusionType = new int?(base.GetInt("ID_FinalConclusionType", 0));
			int int2 = base.GetInt("DispOrder", 500);
			int int3 = base.GetInt("Forsex", 0);
			string text3 = base.GetString("Explanation");
			string text4 = base.GetString("Suggestion");
			string text5 = base.GetString("DietGuide");
			string text6 = base.GetString("SportsGuide");
			string text7 = base.GetString("HealthKnowledge");
			text3 = Input.URLDecode(text3);
			text4 = Input.URLDecode(text4);
			text5 = Input.URLDecode(text5);
			text6 = Input.URLDecode(text6);
			text7 = Input.URLDecode(text7);
			int? iD_ICD = new int?(base.GetInt("ID_ICD", 0));
			bool flag = base.GetInt("Is_Banned", 0) == 1;
			string text8 = base.GetString("BanDescribe");
			text8 = Input.URLDecode(text8);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ConclusionName", text, TypeCode.String)
			};
			string querySqlCode = "QueryConclusionNameIsExis_Param";
			try
			{
				DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
				{
					if (@int <= 0 || dataSet.Tables[0].Rows.Count != 1 || int.Parse(dataSet.Tables[0].Rows[0][0].ToString()) != @int)
					{
						this.OutPutMessage("-1".ToString());						
					}
				}
			}
			catch (Exception ex)
			{
				return;
			}
			PEIS.Model.BusConclusion busConclusion;
			if (@int <= 0)
			{
				busConclusion = new PEIS.Model.BusConclusion();
				busConclusion.ID_Createopr = new int?(this.LoginUserModel.UserID);
				busConclusion.CreateOperator = this.LoginUserModel.UserName;
				busConclusion.CreateDate = new DateTime?(DateTime.Now);
			}
			else
			{
				busConclusion = PEIS.BLL.BusConclusion.Instance.GetModel(@int);
			}
			busConclusion.ID_Conclusion = @int;
			busConclusion.ConclusionName = Secret.AES.EncryptPrefix(text);
			busConclusion.ID_ConclusionType = iD_ConclusionType;
			busConclusion.DispOrder = new int?(int2);
			busConclusion.ForGender = new int?(int3);
			busConclusion.Explanation = Secret.AES.EncryptPrefix(text3);
			busConclusion.Suggestion = Secret.AES.EncryptPrefix(text4);
			busConclusion.DietGuide = Secret.AES.EncryptPrefix(text5);
			busConclusion.SportsGuide = Secret.AES.EncryptPrefix(text6);
			busConclusion.HealthKnowledge = Secret.AES.EncryptPrefix(text7);
			busConclusion.TeamConclusionName = Secret.AES.EncryptPrefix(text2);
			busConclusion.ID_FinalConclusionType = iD_FinalConclusionType;
			busConclusion.ID_ICD = iD_ICD;
			if (busConclusion.Is_Banned != flag && flag)
			{
				busConclusion.ID_BanOpr = new int?(this.LoginUserModel.UserID);
				busConclusion.BanOperator = this.LoginUserModel.UserName;
				busConclusion.BanDate = new DateTime?(DateTime.Now);
			}
			busConclusion.Is_Banned = new bool?(flag);
			if (flag)
			{
				busConclusion.BanDescribe = text8;
			}
			else
			{
				busConclusion.BanDescribe = "";
			}
			busConclusion.InputCode = Input.GetStringSpellCode(text);
			if (busConclusion.InputCode.Length > 20)
			{
				busConclusion.InputCode = busConclusion.InputCode.Substring(0, 20);
			}
			int num = CommonConclusion.Instance.SaveConclusion(busConclusion);
			int num2 = (@int > 0) ? @int : num;
			string text9 = (@int > 0) ? "修改" : "新增";
			if (num > 0)
			{
				Log4J.Instance.Info(string.Concat(new object[]
				{
					Public.GetClientIP(),
					",",
					this.LoginUserModel.UserName,
					",",
					text9,
					"结论词 名称：",
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
					text9,
					"结论词失败 名称：",
					text,
					",编号：",
					num2
				}));
			}
			this.OutPutMessage(num.ToString());
			base.ClearCache_AllConclusion();
		}

		public void GetSingleConclusionInfo()
		{
			int @int = base.GetInt("ID_Conclusion", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Conclusion", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleConclusionInfo_Param";
			try
			{
				DataSet ds = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}

		public void GetCustomerFinalCompareData()
		{
			long @int = base.GetInt64("ID_Customer", 0L);
			int int2 = base.GetInt("CustomerExamState", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64),
				new SqlConditionInfo("@CustomerExamState", int2, TypeCode.Int32)
			};
			string querySqlCode = "QueryCustomerFinalCompareDataList_Param";
			if (@int > 0L)
			{
				DataSet ds = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string text = JsonHelperFont.Instance.DataSetToJSON(ds);
				text = text.Replace("\\n", "<br/>");
				this.OutPutMessage(text);
			}
			else
			{
				this.OutPutMessage("");
			}
		}

		public void GetFinalExamCompareItemList()
		{
			long @int = base.GetInt64("ID_Customer01", 0L);
			long int2 = base.GetInt64("ID_Customer02", 0L);
			int int3 = base.GetInt("CustomerExamState01", 0);
			int int4 = base.GetInt("CustomerExamState02", 0);
			string querySqlCode = "QueryFinalExamCompareItemList_Param";
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", @int, TypeCode.Int64),
				new SqlConditionInfo("@CustomerExamState", int3, TypeCode.Int32)
			};
			SqlConditionInfo[] conditions2 = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@ID_Customer", int2, TypeCode.Int64),
				new SqlConditionInfo("@CustomerExamState", int4, TypeCode.Int32)
			};
			DataSet dataSet = new DataSet();
			if (@int > 0L && int2 > 0L)
			{
				DataSet dataSet2 = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions);
				DataSet dataSet3 = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, conditions2);
				DataSet dataSet4 = this.GetAllExamSectionList().Copy();
				dataSet4 = this.SelectCompareSectionItem(dataSet4, dataSet2, dataSet3);
				dataSet4.Tables[0].TableName = "CompareFeeItemSectionDT";
				dataSet.Merge(dataSet4);
				DataSet dataSet5 = this.GetAllCompareFeeItemList().Copy();
				dataSet5 = this.SelectCompareFeeItem(dataSet5, dataSet2, dataSet3);
				dataSet5.Tables[0].TableName = "CompareFeeItemDT";
				dataSet.Merge(dataSet5);
				dataSet = this.SelectCompareExamItem(dataSet, dataSet2, dataSet3);
				dataSet = this.SelectCompareFinalConclusion(dataSet, dataSet2, dataSet3);
				string msg = JsonHelperFont.Instance.DataSetToJSON(dataSet);
				this.OutPutMessage(msg);
			}
			else
			{
				this.OutPutMessage("");
			}
		}

		private DataSet GetAllExamSectionList()
		{
			string querySqlCode = "QueryAllExamSectionList_Param";
			string cacheKey = "AllExamSectionList";
			object obj = DataCache.GetCache(cacheKey);
			if (obj == null)
			{
				try
				{
					DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, null);
					obj = dataSet;
					if (obj != null)
					{
						int configInt = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(cacheKey, obj, DateTime.Now.AddMinutes((double)configInt), TimeSpan.Zero);
					}
				}
				catch
				{
				}
			}
			return (DataSet)obj;
		}

		private DataSet SelectCompareSectionItem(DataSet CompareSectionItemDS, DataSet CustomerSectionItemDS01, DataSet CustomerSectionItemDS02)
		{
			CompareSectionItemDS.Tables[0].Columns.Add("DiseaseLevel01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("SectionSummary01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("PositiveSummary01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("SectionSummaryDate01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("SummaryDoctorName01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("IS_giveup01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("CheckDate01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("CheckerName01", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("DiseaseLevel02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("SectionSummary02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("PositiveSummary02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("SectionSummaryDate02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("SummaryDoctorName02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("IS_giveup02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("CheckDate02", typeof(string));
			CompareSectionItemDS.Tables[0].Columns.Add("CheckerName02", typeof(string));
			foreach (DataRow dataRow in CustomerSectionItemDS01.Tables[2].Rows)
			{
				DataRow[] array = CompareSectionItemDS.Tables[0].Select(" ID_Section = " + dataRow["ID_Section"].ToString());
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow2 = array2[i];
					dataRow2["IsShowCompareInfo"] = 1;
					dataRow2["DiseaseLevel01"] = dataRow["DiseaseLevel"];
					dataRow2["SectionSummary01"] = dataRow["SectionSummary"];
					dataRow2["PositiveSummary01"] = dataRow["PositiveSummary"];
					dataRow2["SectionSummaryDate01"] = dataRow["SectionSummaryDate"];
					dataRow2["SummaryDoctorName01"] = dataRow["SummaryDoctorName"];
					dataRow2["IS_giveup01"] = dataRow["IS_giveup"];
					dataRow2["CheckDate01"] = dataRow["CheckDate"];
					dataRow2["CheckerName01"] = dataRow["CheckerName"];
				}
			}
			foreach (DataRow dataRow in CustomerSectionItemDS02.Tables[2].Rows)
			{
				DataRow[] array = CompareSectionItemDS.Tables[0].Select(" ID_Section = " + dataRow["ID_Section"].ToString());
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow2 = array2[i];
					dataRow2["IsShowCompareInfo"] = 1;
					dataRow2["DiseaseLevel02"] = dataRow["DiseaseLevel"];
					dataRow2["SectionSummary02"] = dataRow["SectionSummary"];
					dataRow2["PositiveSummary02"] = dataRow["PositiveSummary"];
					dataRow2["SectionSummaryDate02"] = dataRow["SectionSummaryDate"];
					dataRow2["SummaryDoctorName02"] = dataRow["SummaryDoctorName"];
					dataRow2["IS_giveup02"] = dataRow["IS_giveup"];
					dataRow2["CheckDate02"] = dataRow["CheckDate"];
					dataRow2["CheckerName02"] = dataRow["CheckerName"];
				}
			}
			foreach (DataRow dataRow in CompareSectionItemDS.Tables[0].Rows)
			{
				DataRow[] array = CompareSectionItemDS.Tables[0].Select(" IsShowCompareInfo = 0 ");
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow2 = array2[i];
					dataRow2.Delete();
				}
			}
			CompareSectionItemDS.AcceptChanges();
			return CompareSectionItemDS;
		}

		private DataSet GetAllCompareFeeItemList()
		{
			string querySqlCode = "QueryAllCompareFeeItemList_Param";
			string cacheKey = "AllCompareFeeItemList";
			object obj = DataCache.GetCache(cacheKey);
			if (obj == null)
			{
				try
				{
					DataSet dataSet = CommonConclusion.Instance.ExcuteQuerySql(querySqlCode, null);
					obj = dataSet;
					if (obj != null)
					{
						int configInt = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(cacheKey, obj, DateTime.Now.AddMinutes((double)configInt), TimeSpan.Zero);
					}
				}
				catch
				{
				}
			}
			return (DataSet)obj;
		}

		private DataSet SelectCompareFeeItem(DataSet CompareExamItemDS, DataSet CustomerFinalExamItemDS01, DataSet CustomerFinalExamItemDS02)
		{
			foreach (DataRow dataRow in CustomerFinalExamItemDS01.Tables[1].Rows)
			{
				DataRow[] array = CompareExamItemDS.Tables[0].Select(" IsShowCompareInfo = 0 and ID_Fee = " + dataRow["ID_Fee"].ToString());
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow2 = array2[i];
					dataRow2["IsShowCompareInfo"] = 1;
				}
			}
			foreach (DataRow dataRow in CustomerFinalExamItemDS02.Tables[1].Rows)
			{
				DataRow[] array = CompareExamItemDS.Tables[0].Select(" IsShowCompareInfo = 0 and ID_Fee = " + dataRow["ID_Fee"].ToString());
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow2 = array2[i];
					dataRow2["IsShowCompareInfo"] = 1;
				}
			}
			foreach (DataRow dataRow in CompareExamItemDS.Tables[0].Rows)
			{
				DataRow[] array = CompareExamItemDS.Tables[0].Select(" IsShowCompareInfo = 0 ");
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow2 = array2[i];
					dataRow2.Delete();
				}
			}
			CompareExamItemDS.AcceptChanges();
			return CompareExamItemDS;
		}

		private DataSet SelectCompareExamItem(DataSet CompareFinalExamItemDS, DataSet CustomerFinalExamItemDS01, DataSet CustomerFinalExamItemDS02)
		{
			DataTable dataTable = new DataTable("CompareExamItem");
			dataTable.Columns.Add("ID_Section", typeof(string));
			dataTable.Columns.Add("ID_Fee", typeof(string));
			dataTable.Columns.Add("ID_ExamItem", typeof(string));
			dataTable.Columns.Add("ExamItemName", typeof(string));
			dataTable.Columns.Add("ResultLabValues01", typeof(string));
			dataTable.Columns.Add("ResultLabUnit01", typeof(string));
			dataTable.Columns.Add("ResultLabMark01", typeof(string));
			dataTable.Columns.Add("ResultLabRange01", typeof(string));
			dataTable.Columns.Add("ResultSummary01", typeof(string));
			dataTable.Columns.Add("Is_FeeRefund01", typeof(string));
			dataTable.Columns.Add("Is_Examined01", typeof(string));
			dataTable.Columns.Add("ResultLabValues02", typeof(string));
			dataTable.Columns.Add("ResultLabUnit02", typeof(string));
			dataTable.Columns.Add("ResultLabMark02", typeof(string));
			dataTable.Columns.Add("ResultLabRange02", typeof(string));
			dataTable.Columns.Add("ResultSummary02", typeof(string));
			dataTable.Columns.Add("Is_FeeRefund02", typeof(string));
			dataTable.Columns.Add("Is_Examined02", typeof(string));
			dataTable.Columns.Add("IsSameExamSummary", typeof(string));
			foreach (DataRow dataRow in CustomerFinalExamItemDS01.Tables[0].Rows)
			{
				DataRow[] array = dataTable.Select(" ID_ExamItem = " + dataRow["ID_ExamItem"].ToString());
				if (array != null && array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow2 = array2[i];
						dataRow2["ResultLabValues01"] = dataRow["ResultLabValues"];
						dataRow2["ResultLabUnit01"] = dataRow["ResultLabUnit"];
						dataRow2["ResultLabMark01"] = dataRow["ResultLabMark"];
						dataRow2["ResultLabRange01"] = dataRow["ResultLabRange"];
						dataRow2["ResultSummary01"] = dataRow["ResultSummary"];
						dataRow2["Is_FeeRefund01"] = dataRow["Is_FeeRefund"];
						dataRow2["Is_Examined01"] = dataRow["Is_Examined"];
						if (dataRow2["ResultSummary01"].ToString() == dataRow2["ResultSummary02"].ToString())
						{
							dataRow2["IsSameExamSummary"] = "1";
						}
					}
				}
				else
				{
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["ID_Section"] = dataRow["ID_Section"];
					dataRow2["ID_Fee"] = dataRow["ID_Fee"];
					dataRow2["ID_ExamItem"] = dataRow["ID_ExamItem"];
					dataRow2["ExamItemName"] = dataRow["ExamItemName"];
					dataRow2["ResultLabValues01"] = dataRow["ResultLabValues"];
					dataRow2["ResultLabUnit01"] = dataRow["ResultLabUnit"];
					dataRow2["ResultLabMark01"] = dataRow["ResultLabMark"];
					dataRow2["ResultLabRange01"] = dataRow["ResultLabRange"];
					dataRow2["ResultSummary01"] = dataRow["ResultSummary"];
					dataRow2["Is_FeeRefund01"] = dataRow["Is_FeeRefund"];
					dataRow2["Is_Examined01"] = dataRow["Is_Examined"];
					dataRow2["IsSameExamSummary"] = "0";
					dataTable.Rows.Add(dataRow2);
				}
			}
			foreach (DataRow dataRow in CustomerFinalExamItemDS02.Tables[0].Rows)
			{
				DataRow[] array = dataTable.Select(" ID_ExamItem = " + dataRow["ID_ExamItem"].ToString());
				if (array != null && array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow2 = array2[i];
						dataRow2["ResultLabValues02"] = dataRow["ResultLabValues"];
						dataRow2["ResultLabUnit02"] = dataRow["ResultLabUnit"];
						dataRow2["ResultLabMark02"] = dataRow["ResultLabMark"];
						dataRow2["ResultLabRange02"] = dataRow["ResultLabRange"];
						dataRow2["ResultSummary02"] = dataRow["ResultSummary"];
						dataRow2["Is_FeeRefund02"] = dataRow["Is_FeeRefund"];
						dataRow2["Is_Examined02"] = dataRow["Is_Examined"];
						if (dataRow2["ResultSummary01"].ToString() == dataRow2["ResultSummary02"].ToString())
						{
							dataRow2["IsSameExamSummary"] = "1";
						}
					}
				}
				else
				{
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["ID_Section"] = dataRow["ID_Section"];
					dataRow2["ID_Fee"] = dataRow["ID_Fee"];
					dataRow2["ID_ExamItem"] = dataRow["ID_ExamItem"];
					dataRow2["ExamItemName"] = dataRow["ExamItemName"];
					dataRow2["ResultLabValues02"] = dataRow["ResultLabValues"];
					dataRow2["ResultLabUnit02"] = dataRow["ResultLabUnit"];
					dataRow2["ResultLabMark02"] = dataRow["ResultLabMark"];
					dataRow2["ResultLabRange02"] = dataRow["ResultLabRange"];
					dataRow2["ResultSummary02"] = dataRow["ResultSummary"];
					dataRow2["Is_FeeRefund02"] = dataRow["Is_FeeRefund"];
					dataRow2["Is_Examined02"] = dataRow["Is_Examined"];
					dataRow2["IsSameExamSummary"] = "0";
					dataTable.Rows.Add(dataRow2);
				}
			}
			dataTable.AcceptChanges();
			CompareFinalExamItemDS.Merge(dataTable);
			return CompareFinalExamItemDS;
		}

		private DataSet SelectCompareFinalConclusion(DataSet CompareFinalExamItemDS, DataSet CustomerFinalExamItemDS01, DataSet CustomerFinalExamItemDS02)
		{
			DataTable dataTable = new DataTable("CompareFinalConclusion");
			dataTable.Columns.Add("IDCard", typeof(string));
			dataTable.Columns.Add("CustomerName", typeof(string));
			dataTable.Columns.Add("FinalOverView01", typeof(string));
			dataTable.Columns.Add("FinalConclusion01", typeof(string));
			dataTable.Columns.Add("ResultCompare01", typeof(string));
			dataTable.Columns.Add("MainDiagnose01", typeof(string));
			dataTable.Columns.Add("FinalDietGuide01", typeof(string));
			dataTable.Columns.Add("FinalSportGuide01", typeof(string));
			dataTable.Columns.Add("FinalHealthKnowlage01", typeof(string));
			dataTable.Columns.Add("IndicatorDiagnose01", typeof(string));
			dataTable.Columns.Add("OtherDiagnose01", typeof(string));
			dataTable.Columns.Add("NormalDiagnose01", typeof(string));
			dataTable.Columns.Add("SecondaryDiagnose01", typeof(string));
			dataTable.Columns.Add("FinalOverView02", typeof(string));
			dataTable.Columns.Add("FinalConclusion02", typeof(string));
			dataTable.Columns.Add("ResultCompare02", typeof(string));
			dataTable.Columns.Add("MainDiagnose02", typeof(string));
			dataTable.Columns.Add("FinalDietGuide02", typeof(string));
			dataTable.Columns.Add("FinalSportGuide02", typeof(string));
			dataTable.Columns.Add("FinalHealthKnowlage02", typeof(string));
			dataTable.Columns.Add("IndicatorDiagnose02", typeof(string));
			dataTable.Columns.Add("OtherDiagnose02", typeof(string));
			dataTable.Columns.Add("NormalDiagnose02", typeof(string));
			dataTable.Columns.Add("SecondaryDiagnose02", typeof(string));
			foreach (DataRow dataRow in CustomerFinalExamItemDS01.Tables[3].Rows)
			{
				DataRow[] array = dataTable.Select();
				if (array != null && array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow2 = array2[i];
						dataRow2["FinalOverView01"] = dataRow["FinalOverView"];
						dataRow2["FinalConclusion01"] = dataRow["FinalConclusion"];
						dataRow2["ResultCompare01"] = dataRow["ResultCompare"];
						dataRow2["MainDiagnose01"] = dataRow["MainDiagnose"];
						dataRow2["FinalDietGuide01"] = dataRow["FinalDietGuide"];
						dataRow2["FinalSportGuide01"] = dataRow["FinalSportGuide"];
						dataRow2["FinalHealthKnowlage01"] = dataRow["FinalHealthKnowlage"];
						dataRow2["IndicatorDiagnose01"] = dataRow["IndicatorDiagnose"];
						dataRow2["OtherDiagnose01"] = dataRow["OtherDiagnose"];
						dataRow2["NormalDiagnose01"] = dataRow["NormalDiagnose"];
						dataRow2["SecondaryDiagnose01"] = dataRow["SecondaryDiagnose"];
					}
				}
				else
				{
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["IDCard"] = dataRow["IDCard"];
					dataRow2["CustomerName"] = dataRow["CustomerName"];
					dataRow2["FinalOverView01"] = dataRow["FinalOverView"];
					dataRow2["FinalConclusion01"] = dataRow["FinalConclusion"];
					dataRow2["ResultCompare01"] = dataRow["ResultCompare"];
					dataRow2["MainDiagnose01"] = dataRow["MainDiagnose"];
					dataRow2["FinalDietGuide01"] = dataRow["FinalDietGuide"];
					dataRow2["FinalSportGuide01"] = dataRow["FinalSportGuide"];
					dataRow2["FinalHealthKnowlage01"] = dataRow["FinalHealthKnowlage"];
					dataRow2["IndicatorDiagnose01"] = dataRow["IndicatorDiagnose"];
					dataRow2["OtherDiagnose01"] = dataRow["OtherDiagnose"];
					dataRow2["NormalDiagnose01"] = dataRow["NormalDiagnose"];
					dataRow2["SecondaryDiagnose01"] = dataRow["SecondaryDiagnose"];
					dataTable.Rows.Add(dataRow2);
				}
			}
			foreach (DataRow dataRow in CustomerFinalExamItemDS02.Tables[3].Rows)
			{
				DataRow[] array = dataTable.Select();
				if (array != null && array.Length > 0)
				{
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow2 = array2[i];
						dataRow2["FinalOverView02"] = dataRow["FinalOverView"];
						dataRow2["FinalConclusion02"] = dataRow["FinalConclusion"];
						dataRow2["ResultCompare02"] = dataRow["ResultCompare"];
						dataRow2["MainDiagnose02"] = dataRow["MainDiagnose"];
						dataRow2["FinalDietGuide02"] = dataRow["FinalDietGuide"];
						dataRow2["FinalSportGuide02"] = dataRow["FinalSportGuide"];
						dataRow2["FinalHealthKnowlage02"] = dataRow["FinalHealthKnowlage"];
						dataRow2["IndicatorDiagnose02"] = dataRow["IndicatorDiagnose"];
						dataRow2["OtherDiagnose02"] = dataRow["OtherDiagnose"];
						dataRow2["NormalDiagnose02"] = dataRow["NormalDiagnose"];
						dataRow2["SecondaryDiagnose02"] = dataRow["SecondaryDiagnose"];
					}
				}
				else
				{
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["IDCard"] = dataRow["IDCard"];
					dataRow2["CustomerName"] = dataRow["CustomerName"];
					dataRow2["FinalOverView02"] = dataRow["FinalOverView"];
					dataRow2["FinalConclusion02"] = dataRow["FinalConclusion"];
					dataRow2["ResultCompare02"] = dataRow["ResultCompare"];
					dataRow2["MainDiagnose02"] = dataRow["MainDiagnose"];
					dataRow2["FinalDietGuide02"] = dataRow["FinalDietGuide"];
					dataRow2["FinalSportGuide02"] = dataRow["FinalSportGuide"];
					dataRow2["FinalHealthKnowlage02"] = dataRow["FinalHealthKnowlage"];
					dataRow2["IndicatorDiagnose02"] = dataRow["IndicatorDiagnose"];
					dataRow2["OtherDiagnose02"] = dataRow["OtherDiagnose"];
					dataRow2["NormalDiagnose02"] = dataRow["NormalDiagnose"];
					dataRow2["SecondaryDiagnose02"] = dataRow["SecondaryDiagnose"];
					dataTable.Rows.Add(dataRow2);
				}
			}
			dataTable.AcceptChanges();
			CompareFinalExamItemDS.Merge(dataTable);
			return CompareFinalExamItemDS;
		}
	}
}
