using PEIS.Base;
using PEIS.Common;
using PEIS.BLL;
using NVelocity;
using System;
using System.Data;

namespace PEIS.Web.System.Customer
{
	public class TeamOper : BasePage
	{
		private string type = string.Empty;

		private string UserID = string.Empty;

		private new string UserName = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.TemplateName = "blue2";
			this.UserID = this.LoginUserModel.UserID.ToString();
			this.UserName = this.LoginUserModel.UserName;
			this.ProcessRequest();
		}

		public override void ReplaceContent(ref VelocityContext vltContext)
		{
			vltContext.Put("type", base.GetString("type").ToLower());
			vltContext.Put("SecurityLevelDT", Public.GetSecurityLevelDataFromEnum());
			vltContext.Put("CurDate", DateTime.Now.ToString("yyyy年M月dd日"));
			vltContext.Put("pageTitle", "团体备单");
			vltContext.Put("RegisteDate", DateTime.Now.ToString("yyyy-MM-dd"));
			vltContext.Put("Register", this.UserName);
			vltContext.Put("DisCountRate", CommonExcuteSql.Instance.ExcuteSql(string.Format("select isnull(DisCountRate,10) DisCountRate from SYSOpUser where UserID='{0}';", this.UserID)).Tables[0].Rows[0][0].ToString());
			DataSet allDctData = CommonOnArcCust.Instance.GetAllDctData();
			for (int i = 0; i < allDctData.Tables.Count; i++)
			{
				vltContext.Put(CommonOnArcCust.Instance.tbName[i], allDctData.Tables[i]);
			}
			allDctData.Dispose();
			vltContext.Put("TeamDT", CommonTeam.Instance.GetTeamInfoByKeyWords(string.Empty));
			this.type = base.GetString("type").ToLower();
			if (this.type == "add")
			{
				vltContext.Put("CreateDate", DateTime.Now.ToString("yyyy-MM-dd"));
				vltContext.Put("Creator", this.UserName);
				vltContext.Put("ID_Creator", this.UserID);
				vltContext.Put("SecurityLevelDT", Public.GetSecurityLevelDataFromEnum());
			}
			if (this.type == "edit")
			{
			}
		}
	}
}
