using PEIS.Base;
using PEIS.Common;
using PEIS.BLL;
using PEIS.Model;
using System;
using System.Data;
using System.Reflection;

namespace PEIS.Web.Ajax
{
	public class AjaxSection : BasePage
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

		public void GetQuickSectionList()
		{
			string @string = base.GetString("InputCode");
			DataTable quickSectionList = CommonSystemInfo.Instance.GetQuickSectionList(@string);
			string msg = JsonHelperFont.Instance.DataTableToJSON(quickSectionList, quickSectionList.Rows.Count);
			this.OutPutMessage(msg);
		}

		public void SearchSectionList()
		{
			int @int = base.GetInt("pageIndex", 0);
			int int2 = base.GetInt("pageSize", 10);
			int totalCount = 0;
			int num = 0;
			string text = base.GetString("SearchSectionKeyword").Trim();
			SqlConditionInfo[] array = null;
			string pageCode = "QueryPagesSectionListParam";
			if (!string.IsNullOrEmpty(text))
			{
				array = new SqlConditionInfo[1];
				pageCode = "QueryPagesSectionListParamByName";
				array[0] = new SqlConditionInfo("@SectionName", text, TypeCode.String);
				array[0].Blur = 3;
				array[0].Place = 2;
			}
			DataTable page = CommonConfig.Instance.GetPage(pageCode, @int, int2, out totalCount, out num, array);
			string msg = JsonHelperFont.Instance.DataTableToJSON(page, totalCount);
			this.OutPutMessage(msg);
		}

		public void GetSingleSectionItem()
		{
			int @int = base.GetInt("SectionID", 0);
			SqlConditionInfo[] conditions = new SqlConditionInfo[]
			{
				new SqlConditionInfo("@SectionID", @int, TypeCode.Int32)
			};
			string querySqlCode = "QuerySingleSectionItem_Param";
			try
			{
				DataSet ds = CommonConfig.Instance.ExcuteQuerySql(querySqlCode, conditions);
				string msg = JsonHelperFont.Instance.DataSetToJSON(ds);
				this.OutPutMessage(msg);
			}
			catch (Exception e)
			{
				this.OutPutMessage("");
			}
		}
	}
}
