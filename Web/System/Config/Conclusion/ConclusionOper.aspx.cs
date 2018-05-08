using PEIS.Base;
using PEIS.Common;
using PEIS.BLL;
using PEIS.Model;
using NVelocity;
using System;

namespace PEIS.Web.System.Config.Conclusion
{
	public class ConclusionOper : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.ProcessRequest();
		}

		public override void ReplaceContent(ref VelocityContext vltContext)
		{
			vltContext.Put("webName", this.SiteName);
			int @int = base.GetInt("ConclusionID", 0);
			vltContext.Put("pageTitle", "新增结论词");
			if (@int > 0)
			{
				vltContext.Put("pageTitle", "修改结论词");
				this.GetEditConclusionInfo(@int, ref vltContext);
			}
		}

		protected void GetEditConclusionInfo(int ID_Conclusion, ref VelocityContext vltContext)
		{
			if (ID_Conclusion > 0)
			{
				PEIS.Model.BusConclusion model = PEIS.BLL.BusConclusion.Instance.GetModel(ID_Conclusion);
				if (null != model)
				{
					vltContext.Put("ID_Conclusion", model.ID_Conclusion);
					vltContext.Put("ID_ConclusionType", model.ID_ConclusionType);
					vltContext.Put("ConclusionName", Secret.AES.DecryptPrefix(model.ConclusionName));
					vltContext.Put("DispOrder", model.DispOrder);
					vltContext.Put("ForGender", model.ForGender);
					vltContext.Put("InputCode", model.InputCode);
					vltContext.Put("Explanation", Secret.AES.DecryptPrefix(model.Explanation));
					vltContext.Put("Suggestion", Secret.AES.DecryptPrefix(model.Suggestion));
					vltContext.Put("DietGuide", Secret.AES.DecryptPrefix(model.DietGuide));
					vltContext.Put("SportsGuide", Secret.AES.DecryptPrefix(model.SportsGuide));
					vltContext.Put("Is_Banned", model.Is_Banned);
					vltContext.Put("BanOperator", model.BanOperator);
					vltContext.Put("ID_BanOpr", model.ID_BanOpr);
					vltContext.Put("BanDescribe", model.BanDescribe);
					vltContext.Put("BanDate", model.BanDate);
					vltContext.Put("ID_Createopr", model.ID_Createopr);
					vltContext.Put("CreateOperator", model.CreateOperator);
					vltContext.Put("CreateDate", model.CreateDate);
					vltContext.Put("HealthKnowledge", Secret.AES.DecryptPrefix(model.HealthKnowledge));
					vltContext.Put("TeamConclusionName", Secret.AES.DecryptPrefix(model.TeamConclusionName));
					if (model.ID_ConclusionType.HasValue)
					{
						string value = CommonConclusion.Instance.GetConclusionTypeName(int.Parse(model.ID_ConclusionType.ToString()));
						vltContext.Put("ConclusionTypeName", value);
					}
					vltContext.Put("ID_ICD", model.ID_ICD);
					if (model.ID_ICD.HasValue)
					{
						string value = CommonConfig.Instance.GetICDCNName(int.Parse(model.ID_ICD.ToString()));
						vltContext.Put("ICDCNName", value);
					}
					vltContext.Put("ID_FinalConclusionType", model.ID_FinalConclusionType);
					if (model.ID_FinalConclusionType.HasValue)
					{
						string value = CommonConfig.Instance.GetFinalConclusionTypeName(int.Parse(model.ID_FinalConclusionType.ToString()));
						vltContext.Put("FinalConclusionTypeName", value);
					}
				}
			}
		}
	}
}
