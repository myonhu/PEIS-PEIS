using PEIS.Base;
using NVelocity;
using System;

namespace PEIS.Web.System.Config.Set
{
	public class SetFeeRel : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.TemplateName = "blue2";
			this.ProcessRequest();
		}

		public override void ReplaceContent(ref VelocityContext vltContext)
		{
		}
	}
}
