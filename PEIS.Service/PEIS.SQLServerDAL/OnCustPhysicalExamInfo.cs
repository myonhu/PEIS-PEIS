using PEIS.IDAL;
using PEIS.Model;
using PEIS.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PEIS.SQLServerDAL
{
	public class OnCustPhysicalExamInfo : IOnCustPhysicalExamInfo
	{
		public bool Exists(long ID_Customer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select count(1) from OnCustPhysicalExamInfo");
			stringBuilder.Append(" where ID_Customer=@ID_Customer ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt)
			};
			array[0].Value = ID_Customer;
			return DbHelperSQL.Exists(stringBuilder.ToString(), array);
		}

		public void Add(PEIS.Model.OnCustPhysicalExamInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into OnCustPhysicalExamInfo(");
			stringBuilder.Append("ID_Customer,CustomerName,Is_Team,ID_Team,Is_Paused,Is_SectionLock,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,ExamPlaceID,ExamPlaceName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName,SecurityLevel,DiseaseLevel,Is_FeeSettled,Is_GuideSheetPrinted,Is_GuideSheetReturned,GuideSheetReturnedDate,GuideSheetReturnedby,Is_UseHideCode,Is_FinalFinished,ID_FinalDoctor,FinalDoctor,FinalDate,FinalOverView,FinalConclusion,ResultCompare,MainDiagnose,FinalDietGuide,FinalSportGuide,FinalHealthKnowlage,Is_Checked,ID_Checker,Checker,CheckedDate,Is_ReportReceipted,SubScribDate,OperateDate,ID_Operator,Operator,Note,Is_Subscribed,Invoice,ID_UserGuideSheetReturnedBy,ID_SubScriber,SubScriber,SubScriberOperDate,Is_ExamStarted,TeamName,IndicatorDiagnose,OtherDiagnose,ID_Gender,GenderName,ID_Marriage,MarriageName,NationID,NationName,CultrulID,CultrulName,VocationID,VocationName,IDCard,ExamCard,Photo,BirthDay,Address,MobileNo,Email,SecondaryDiagnose,NormalDiagnose)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Customer,@CustomerName,@Is_Team,@ID_Team,@Is_Paused,@Is_SectionLock,@ID_ExamType,@ExamTypeName,@PEPackageID,@PEPackageName,@ExamPlaceID,@ExamPlaceName,@ID_GuideNurse,@GuideNurse,@ID_ReportWay,@ReportWayName,@ID_FeeWay,@FeeWayName,@SecurityLevel,@DiseaseLevel,@Is_FeeSettled,@Is_GuideSheetPrinted,@Is_GuideSheetReturned,@GuideSheetReturnedDate,@GuideSheetReturnedby,@Is_UseHideCode,@Is_FinalFinished,@ID_FinalDoctor,@FinalDoctor,@FinalDate,@FinalOverView,@FinalConclusion,@ResultCompare,@MainDiagnose,@FinalDietGuide,@FinalSportGuide,@FinalHealthKnowlage,@Is_Checked,@ID_Checker,@Checker,@CheckedDate,@Is_ReportReceipted,@SubScribDate,@OperateDate,@ID_Operator,@Operator,@Note,@Is_Subscribed,@Invoice,@ID_UserGuideSheetReturnedBy,@ID_SubScriber,@SubScriber,@SubScriberOperDate,@Is_ExamStarted,@TeamName,@IndicatorDiagnose,@OtherDiagnose,@ID_Gender,@GenderName,@ID_Marriage,@MarriageName,@NationID,@NationName,@CultrulID,@CultrulName,@VocationID,@VocationName,@IDCard,@ExamCard,@Photo,@BirthDay,@Address,@MobileNo,@Email,@SecondaryDiagnose,@NormalDiagnose)");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
				new SqlParameter("@CustomerName", SqlDbType.VarChar, 30),
				new SqlParameter("@Is_Team", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Team", SqlDbType.Int, 4),
				new SqlParameter("@Is_Paused", SqlDbType.Int, 4),
				new SqlParameter("@Is_SectionLock", SqlDbType.Bit, 1),
				new SqlParameter("@ID_ExamType", SqlDbType.Int, 4),
				new SqlParameter("@ExamTypeName", SqlDbType.VarChar, 50),
				new SqlParameter("@PEPackageID", SqlDbType.Int, 4),
				new SqlParameter("@PEPackageName", SqlDbType.VarChar, 50),
				new SqlParameter("@ExamPlaceID", SqlDbType.Int, 4),
				new SqlParameter("@ExamPlaceName", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_GuideNurse", SqlDbType.Int, 4),
				new SqlParameter("@GuideNurse", SqlDbType.VarChar, 30),
				new SqlParameter("@ID_ReportWay", SqlDbType.Int, 4),
				new SqlParameter("@ReportWayName", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_FeeWay", SqlDbType.Int, 4),
				new SqlParameter("@FeeWayName", SqlDbType.VarChar, 50),
				new SqlParameter("@SecurityLevel", SqlDbType.Int, 4),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@Is_FeeSettled", SqlDbType.Bit, 1),
				new SqlParameter("@Is_GuideSheetPrinted", SqlDbType.Bit, 1),
				new SqlParameter("@Is_GuideSheetReturned", SqlDbType.Bit, 1),
				new SqlParameter("@GuideSheetReturnedDate", SqlDbType.DateTime),
				new SqlParameter("@GuideSheetReturnedby", SqlDbType.VarChar, 30),
				new SqlParameter("@Is_UseHideCode", SqlDbType.Bit, 1),
				new SqlParameter("@Is_FinalFinished", SqlDbType.Bit, 1),
				new SqlParameter("@ID_FinalDoctor", SqlDbType.Int, 4),
				new SqlParameter("@FinalDoctor", SqlDbType.VarChar, 30),
				new SqlParameter("@FinalDate", SqlDbType.DateTime),
				new SqlParameter("@FinalOverView", SqlDbType.Text),
				new SqlParameter("@FinalConclusion", SqlDbType.Text),
				new SqlParameter("@ResultCompare", SqlDbType.Text),
				new SqlParameter("@MainDiagnose", SqlDbType.Text),
				new SqlParameter("@FinalDietGuide", SqlDbType.Text),
				new SqlParameter("@FinalSportGuide", SqlDbType.Text),
				new SqlParameter("@FinalHealthKnowlage", SqlDbType.Text),
				new SqlParameter("@Is_Checked", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@Checker", SqlDbType.VarChar, 30),
				new SqlParameter("@CheckedDate", SqlDbType.DateTime),
				new SqlParameter("@Is_ReportReceipted", SqlDbType.Bit, 1),
				new SqlParameter("@SubScribDate", SqlDbType.DateTime),
				new SqlParameter("@OperateDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Operator", SqlDbType.Int, 4),
				new SqlParameter("@Operator", SqlDbType.VarChar, 30),
				new SqlParameter("@Note", SqlDbType.VarChar, 200),
				new SqlParameter("@Is_Subscribed", SqlDbType.Int, 4),
				new SqlParameter("@Invoice", SqlDbType.VarChar, 200),
				new SqlParameter("@ID_UserGuideSheetReturnedBy", SqlDbType.Int, 4),
				new SqlParameter("@ID_SubScriber", SqlDbType.Int, 4),
				new SqlParameter("@SubScriber", SqlDbType.VarChar, 30),
				new SqlParameter("@SubScriberOperDate", SqlDbType.DateTime),
				new SqlParameter("@Is_ExamStarted", SqlDbType.Bit, 1),
				new SqlParameter("@TeamName", SqlDbType.VarChar, 50),
				new SqlParameter("@IndicatorDiagnose", SqlDbType.Text),
				new SqlParameter("@OtherDiagnose", SqlDbType.Text),
				new SqlParameter("@ID_Gender", SqlDbType.Int, 4),
				new SqlParameter("@GenderName", SqlDbType.VarChar, 8),
				new SqlParameter("@ID_Marriage", SqlDbType.Int, 4),
				new SqlParameter("@MarriageName", SqlDbType.VarChar, 8),
				new SqlParameter("@NationID", SqlDbType.Int, 4),
				new SqlParameter("@NationName", SqlDbType.VarChar, 10),
				new SqlParameter("@CultrulID", SqlDbType.Int, 4),
				new SqlParameter("@CultrulName", SqlDbType.VarChar, 10),
				new SqlParameter("@VocationID", SqlDbType.Int, 4),
				new SqlParameter("@VocationName", SqlDbType.VarChar, 10),
				new SqlParameter("@IDCard", SqlDbType.VarChar, 30),
				new SqlParameter("@ExamCard", SqlDbType.VarChar, 30),
				new SqlParameter("@Photo", SqlDbType.Image),
				new SqlParameter("@BirthDay", SqlDbType.DateTime),
				new SqlParameter("@Address", SqlDbType.VarChar, 360),
				new SqlParameter("@MobileNo", SqlDbType.VarChar, 120),
				new SqlParameter("@Email", SqlDbType.VarChar, 240),
				new SqlParameter("@SecondaryDiagnose", SqlDbType.Text),
				new SqlParameter("@NormalDiagnose", SqlDbType.Text)
			};
			array[0].Value = model.ID_Customer;
			array[1].Value = model.CustomerName;
			array[2].Value = model.Is_Team;
			array[3].Value = model.ID_Team;
			array[4].Value = model.Is_Paused;
			array[5].Value = model.Is_SectionLock;
			array[6].Value = model.ID_ExamType;
			array[7].Value = model.ExamTypeName;
			array[8].Value = model.PEPackageID;
			array[9].Value = model.PEPackageName;
			array[10].Value = model.ExamPlaceID;
			array[11].Value = model.ExamPlaceName;
			array[12].Value = model.ID_GuideNurse;
			array[13].Value = model.GuideNurse;
			array[14].Value = model.ID_ReportWay;
			array[15].Value = model.ReportWayName;
			array[16].Value = model.ID_FeeWay;
			array[17].Value = model.FeeWayName;
			array[18].Value = model.SecurityLevel;
			array[19].Value = model.DiseaseLevel;
			array[20].Value = model.Is_FeeSettled;
			array[21].Value = model.Is_GuideSheetPrinted;
			array[22].Value = model.Is_GuideSheetReturned;
			array[23].Value = model.GuideSheetReturnedDate;
			array[24].Value = model.GuideSheetReturnedby;
			array[25].Value = model.Is_UseHideCode;
			array[26].Value = model.Is_FinalFinished;
			array[27].Value = model.ID_FinalDoctor;
			array[28].Value = model.FinalDoctor;
			array[29].Value = model.FinalDate;
			array[30].Value = model.FinalOverView;
			array[31].Value = model.FinalConclusion;
			array[32].Value = model.ResultCompare;
			array[33].Value = model.MainDiagnose;
			array[34].Value = model.FinalDietGuide;
			array[35].Value = model.FinalSportGuide;
			array[36].Value = model.FinalHealthKnowlage;
			array[37].Value = model.Is_Checked;
			array[38].Value = model.ID_Checker;
			array[39].Value = model.Checker;
			array[40].Value = model.CheckedDate;
			array[41].Value = model.Is_ReportReceipted;
			array[42].Value = model.SubScribDate;
			array[43].Value = model.OperateDate;
			array[44].Value = model.ID_Operator;
			array[45].Value = model.Operator;
			array[46].Value = model.Note;
			array[47].Value = model.Is_Subscribed;
			array[48].Value = model.Invoice;
			array[49].Value = model.ID_UserGuideSheetReturnedBy;
			array[50].Value = model.ID_SubScriber;
			array[51].Value = model.SubScriber;
			array[52].Value = model.SubScriberOperDate;
			array[53].Value = model.Is_ExamStarted;
			array[54].Value = model.TeamName;
			array[55].Value = model.IndicatorDiagnose;
			array[56].Value = model.OtherDiagnose;
			array[57].Value = model.ID_Gender;
			array[58].Value = model.GenderName;
			array[59].Value = model.ID_Marriage;
			array[60].Value = model.MarriageName;
			array[61].Value = model.NationID;
			array[62].Value = model.NationName;
			array[63].Value = model.CultrulID;
			array[64].Value = model.CultrulName;
			array[65].Value = model.VocationID;
			array[66].Value = model.VocationName;
			array[67].Value = model.IDCard;
			array[68].Value = model.ExamCard;
			array[69].Value = model.Photo;
			array[70].Value = model.BirthDay;
			array[71].Value = model.Address;
			array[72].Value = model.MobileNo;
			array[73].Value = model.Email;
			array[74].Value = model.SecondaryDiagnose;
			array[75].Value = model.NormalDiagnose;
			DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		public bool Update(PEIS.Model.OnCustPhysicalExamInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("CustomerName=@CustomerName,");
			stringBuilder.Append("Is_Team=@Is_Team,");
			stringBuilder.Append("ID_Team=@ID_Team,");
			stringBuilder.Append("Is_Paused=@Is_Paused,");
			stringBuilder.Append("Is_SectionLock=@Is_SectionLock,");
			stringBuilder.Append("ID_ExamType=@ID_ExamType,");
			stringBuilder.Append("ExamTypeName=@ExamTypeName,");
			stringBuilder.Append("PEPackageID=@PEPackageID,");
			stringBuilder.Append("PEPackageName=@PEPackageName,");
			stringBuilder.Append("ExamPlaceID=@ExamPlaceID,");
			stringBuilder.Append("ExamPlaceName=@ExamPlaceName,");
			stringBuilder.Append("ID_GuideNurse=@ID_GuideNurse,");
			stringBuilder.Append("GuideNurse=@GuideNurse,");
			stringBuilder.Append("ID_ReportWay=@ID_ReportWay,");
			stringBuilder.Append("ReportWayName=@ReportWayName,");
			stringBuilder.Append("ID_FeeWay=@ID_FeeWay,");
			stringBuilder.Append("FeeWayName=@FeeWayName,");
			stringBuilder.Append("SecurityLevel=@SecurityLevel,");
			stringBuilder.Append("DiseaseLevel=@DiseaseLevel,");
			stringBuilder.Append("Is_FeeSettled=@Is_FeeSettled,");
			stringBuilder.Append("Is_GuideSheetPrinted=@Is_GuideSheetPrinted,");
			stringBuilder.Append("Is_GuideSheetReturned=@Is_GuideSheetReturned,");
			stringBuilder.Append("GuideSheetReturnedDate=@GuideSheetReturnedDate,");
			stringBuilder.Append("GuideSheetReturnedby=@GuideSheetReturnedby,");
			stringBuilder.Append("Is_UseHideCode=@Is_UseHideCode,");
			stringBuilder.Append("Is_FinalFinished=@Is_FinalFinished,");
			stringBuilder.Append("ID_FinalDoctor=@ID_FinalDoctor,");
			stringBuilder.Append("FinalDoctor=@FinalDoctor,");
			stringBuilder.Append("FinalDate=@FinalDate,");
			stringBuilder.Append("FinalOverView=@FinalOverView,");
			stringBuilder.Append("FinalConclusion=@FinalConclusion,");
			stringBuilder.Append("ResultCompare=@ResultCompare,");
			stringBuilder.Append("MainDiagnose=@MainDiagnose,");
			stringBuilder.Append("FinalDietGuide=@FinalDietGuide,");
			stringBuilder.Append("FinalSportGuide=@FinalSportGuide,");
			stringBuilder.Append("FinalHealthKnowlage=@FinalHealthKnowlage,");
			stringBuilder.Append("Is_Checked=@Is_Checked,");
			stringBuilder.Append("ID_Checker=@ID_Checker,");
			stringBuilder.Append("Checker=@Checker,");
			stringBuilder.Append("CheckedDate=@CheckedDate,");
			stringBuilder.Append("Is_ReportReceipted=@Is_ReportReceipted,");
			stringBuilder.Append("SubScribDate=@SubScribDate,");
			stringBuilder.Append("OperateDate=@OperateDate,");
			stringBuilder.Append("ID_Operator=@ID_Operator,");
			stringBuilder.Append("Operator=@Operator,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("Is_Subscribed=@Is_Subscribed,");
			stringBuilder.Append("Invoice=@Invoice,");
			stringBuilder.Append("ID_UserGuideSheetReturnedBy=@ID_UserGuideSheetReturnedBy,");
			stringBuilder.Append("ID_SubScriber=@ID_SubScriber,");
			stringBuilder.Append("SubScriber=@SubScriber,");
			stringBuilder.Append("SubScriberOperDate=@SubScriberOperDate,");
			stringBuilder.Append("Is_ExamStarted=@Is_ExamStarted,");
			stringBuilder.Append("TeamName=@TeamName,");
			stringBuilder.Append("IndicatorDiagnose=@IndicatorDiagnose,");
			stringBuilder.Append("OtherDiagnose=@OtherDiagnose,");
			stringBuilder.Append("ID_Gender=@ID_Gender,");
			stringBuilder.Append("GenderName=@GenderName,");
			stringBuilder.Append("ID_Marriage=@ID_Marriage,");
			stringBuilder.Append("MarriageName=@MarriageName,");
			stringBuilder.Append("NationID=@NationID,");
			stringBuilder.Append("NationName=@NationName,");
			stringBuilder.Append("CultrulID=@CultrulID,");
			stringBuilder.Append("CultrulName=@CultrulName,");
			stringBuilder.Append("VocationID=@VocationID,");
			stringBuilder.Append("VocationName=@VocationName,");
			stringBuilder.Append("IDCard=@IDCard,");
			stringBuilder.Append("ExamCard=@ExamCard,");
			stringBuilder.Append("Photo=@Photo,");
			stringBuilder.Append("BirthDay=@BirthDay,");
			stringBuilder.Append("Address=@Address,");
			stringBuilder.Append("MobileNo=@MobileNo,");
			stringBuilder.Append("Email=@Email,");
			stringBuilder.Append("SecondaryDiagnose=@SecondaryDiagnose,");
			stringBuilder.Append("NormalDiagnose=@NormalDiagnose");
			stringBuilder.Append(" where ID_Customer=@ID_Customer ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@CustomerName", SqlDbType.VarChar, 30),
				new SqlParameter("@Is_Team", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Team", SqlDbType.Int, 4),
				new SqlParameter("@Is_Paused", SqlDbType.Int, 4),
				new SqlParameter("@Is_SectionLock", SqlDbType.Bit, 1),
				new SqlParameter("@ID_ExamType", SqlDbType.Int, 4),
				new SqlParameter("@ExamTypeName", SqlDbType.VarChar, 50),
				new SqlParameter("@PEPackageID", SqlDbType.Int, 4),
				new SqlParameter("@PEPackageName", SqlDbType.VarChar, 50),
				new SqlParameter("@ExamPlaceID", SqlDbType.Int, 4),
				new SqlParameter("@ExamPlaceName", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_GuideNurse", SqlDbType.Int, 4),
				new SqlParameter("@GuideNurse", SqlDbType.VarChar, 30),
				new SqlParameter("@ID_ReportWay", SqlDbType.Int, 4),
				new SqlParameter("@ReportWayName", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_FeeWay", SqlDbType.Int, 4),
				new SqlParameter("@FeeWayName", SqlDbType.VarChar, 50),
				new SqlParameter("@SecurityLevel", SqlDbType.Int, 4),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@Is_FeeSettled", SqlDbType.Bit, 1),
				new SqlParameter("@Is_GuideSheetPrinted", SqlDbType.Bit, 1),
				new SqlParameter("@Is_GuideSheetReturned", SqlDbType.Bit, 1),
				new SqlParameter("@GuideSheetReturnedDate", SqlDbType.DateTime),
				new SqlParameter("@GuideSheetReturnedby", SqlDbType.VarChar, 30),
				new SqlParameter("@Is_UseHideCode", SqlDbType.Bit, 1),
				new SqlParameter("@Is_FinalFinished", SqlDbType.Bit, 1),
				new SqlParameter("@ID_FinalDoctor", SqlDbType.Int, 4),
				new SqlParameter("@FinalDoctor", SqlDbType.VarChar, 30),
				new SqlParameter("@FinalDate", SqlDbType.DateTime),
				new SqlParameter("@FinalOverView", SqlDbType.Text),
				new SqlParameter("@FinalConclusion", SqlDbType.Text),
				new SqlParameter("@ResultCompare", SqlDbType.Text),
				new SqlParameter("@MainDiagnose", SqlDbType.Text),
				new SqlParameter("@FinalDietGuide", SqlDbType.Text),
				new SqlParameter("@FinalSportGuide", SqlDbType.Text),
				new SqlParameter("@FinalHealthKnowlage", SqlDbType.Text),
				new SqlParameter("@Is_Checked", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@Checker", SqlDbType.VarChar, 30),
				new SqlParameter("@CheckedDate", SqlDbType.DateTime),
				new SqlParameter("@Is_ReportReceipted", SqlDbType.Bit, 1),
				new SqlParameter("@SubScribDate", SqlDbType.DateTime),
				new SqlParameter("@OperateDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Operator", SqlDbType.Int, 4),
				new SqlParameter("@Operator", SqlDbType.VarChar, 30),
				new SqlParameter("@Note", SqlDbType.VarChar, 200),
				new SqlParameter("@Is_Subscribed", SqlDbType.Int, 4),
				new SqlParameter("@Invoice", SqlDbType.VarChar, 200),
				new SqlParameter("@ID_UserGuideSheetReturnedBy", SqlDbType.Int, 4),
				new SqlParameter("@ID_SubScriber", SqlDbType.Int, 4),
				new SqlParameter("@SubScriber", SqlDbType.VarChar, 30),
				new SqlParameter("@SubScriberOperDate", SqlDbType.DateTime),
				new SqlParameter("@Is_ExamStarted", SqlDbType.Bit, 1),
				new SqlParameter("@TeamName", SqlDbType.VarChar, 50),
				new SqlParameter("@IndicatorDiagnose", SqlDbType.Text),
				new SqlParameter("@OtherDiagnose", SqlDbType.Text),
				new SqlParameter("@ID_Gender", SqlDbType.Int, 4),
				new SqlParameter("@GenderName", SqlDbType.VarChar, 8),
				new SqlParameter("@ID_Marriage", SqlDbType.Int, 4),
				new SqlParameter("@MarriageName", SqlDbType.VarChar, 8),
				new SqlParameter("@NationID", SqlDbType.Int, 4),
				new SqlParameter("@NationName", SqlDbType.VarChar, 10),
				new SqlParameter("@CultrulID", SqlDbType.Int, 4),
				new SqlParameter("@CultrulName", SqlDbType.VarChar, 10),
				new SqlParameter("@VocationID", SqlDbType.Int, 4),
				new SqlParameter("@VocationName", SqlDbType.VarChar, 10),
				new SqlParameter("@IDCard", SqlDbType.VarChar, 30),
				new SqlParameter("@ExamCard", SqlDbType.VarChar, 30),
				new SqlParameter("@Photo", SqlDbType.Image),
				new SqlParameter("@BirthDay", SqlDbType.DateTime),
				new SqlParameter("@Address", SqlDbType.VarChar, 360),
				new SqlParameter("@MobileNo", SqlDbType.VarChar, 120),
				new SqlParameter("@Email", SqlDbType.VarChar, 240),
				new SqlParameter("@SecondaryDiagnose", SqlDbType.Text),
				new SqlParameter("@NormalDiagnose", SqlDbType.Text),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = model.CustomerName;
			array[1].Value = model.Is_Team;
			array[2].Value = model.ID_Team;
			array[3].Value = model.Is_Paused;
			array[4].Value = model.Is_SectionLock;
			array[5].Value = model.ID_ExamType;
			array[6].Value = model.ExamTypeName;
			array[7].Value = model.PEPackageID;
			array[8].Value = model.PEPackageName;
			array[9].Value = model.ExamPlaceID;
			array[10].Value = model.ExamPlaceName;
			array[11].Value = model.ID_GuideNurse;
			array[12].Value = model.GuideNurse;
			array[13].Value = model.ID_ReportWay;
			array[14].Value = model.ReportWayName;
			array[15].Value = model.ID_FeeWay;
			array[16].Value = model.FeeWayName;
			array[17].Value = model.SecurityLevel;
			array[18].Value = model.DiseaseLevel;
			array[19].Value = model.Is_FeeSettled;
			array[20].Value = model.Is_GuideSheetPrinted;
			array[21].Value = model.Is_GuideSheetReturned;
			array[22].Value = model.GuideSheetReturnedDate;
			array[23].Value = model.GuideSheetReturnedby;
			array[24].Value = model.Is_UseHideCode;
			array[25].Value = model.Is_FinalFinished;
			array[26].Value = model.ID_FinalDoctor;
			array[27].Value = model.FinalDoctor;
			array[28].Value = model.FinalDate;
			array[29].Value = model.FinalOverView;
			array[30].Value = model.FinalConclusion;
			array[31].Value = model.ResultCompare;
			array[32].Value = model.MainDiagnose;
			array[33].Value = model.FinalDietGuide;
			array[34].Value = model.FinalSportGuide;
			array[35].Value = model.FinalHealthKnowlage;
			array[36].Value = model.Is_Checked;
			array[37].Value = model.ID_Checker;
			array[38].Value = model.Checker;
			array[39].Value = model.CheckedDate;
			array[40].Value = model.Is_ReportReceipted;
			array[41].Value = model.SubScribDate;
			array[42].Value = model.OperateDate;
			array[43].Value = model.ID_Operator;
			array[44].Value = model.Operator;
			array[45].Value = model.Note;
			array[46].Value = model.Is_Subscribed;
			array[47].Value = model.Invoice;
			array[48].Value = model.ID_UserGuideSheetReturnedBy;
			array[49].Value = model.ID_SubScriber;
			array[50].Value = model.SubScriber;
			array[51].Value = model.SubScriberOperDate;
			array[52].Value = model.Is_ExamStarted;
			array[53].Value = model.TeamName;
			array[54].Value = model.IndicatorDiagnose;
			array[55].Value = model.OtherDiagnose;
			array[56].Value = model.ID_Gender;
			array[57].Value = model.GenderName;
			array[58].Value = model.ID_Marriage;
			array[59].Value = model.MarriageName;
			array[60].Value = model.NationID;
			array[61].Value = model.NationName;
			array[62].Value = model.CultrulID;
			array[63].Value = model.CultrulName;
			array[64].Value = model.VocationID;
			array[65].Value = model.VocationName;
			array[66].Value = model.IDCard;
			array[67].Value = model.ExamCard;
			array[68].Value = model.Photo;
			array[69].Value = model.BirthDay;
			array[70].Value = model.Address;
			array[71].Value = model.MobileNo;
			array[72].Value = model.Email;
			array[73].Value = model.SecondaryDiagnose;
			array[74].Value = model.NormalDiagnose;
			array[75].Value = model.ID_Customer;
			int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
			return num > 0;
		}

		public bool Delete(long ID_Customer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from OnCustPhysicalExamInfo ");
			stringBuilder.Append(" where ID_Customer=@ID_Customer ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt)
			};
			array[0].Value = ID_Customer;
			int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
			return num > 0;
		}

		public bool DeleteList(string ID_Customerlist)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from OnCustPhysicalExamInfo ");
			stringBuilder.Append(" where ID_Customer in (" + ID_Customerlist + ")  ");
			int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString());
			return num > 0;
		}

		public PEIS.Model.OnCustPhysicalExamInfo GetModel(long ID_Customer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select  top 1 ID_Customer,CustomerName,Is_Team,ID_Team,Is_Paused,Is_SectionLock,ID_ExamType,ExamTypeName,ID_Set,SetName,ID_ExamPlace,ExamPlaceName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName,SecurityLevel,DiseaseLevel,Is_FeeSettled,Is_GuideSheetPrinted,Is_GuideSheetReturned,GuideSheetReturnedDate,GuideSheetReturnedby,Is_UseHideCode,Is_FinalFinished,ID_FinalDoctor,FinalDoctor,FinalDate,FinalOverView,FinalConclusion,ResultCompare,MainDiagnose,FinalDietGuide,FinalSportGuide,FinalHealthKnowlage,Is_Checked,ID_Checker,Checker,CheckedDate,Is_ReportReceipted,SubScribDate,OperateDate,ID_Operator,Operator,Note,Is_Subscribed,Invoice,ID_UserGuideSheetReturnedBy,ID_SubScriber,SubScriber,SubScriberOperDate,Is_ExamStarted,TeamName,IndicatorDiagnose,OtherDiagnose,ID_Gender,GenderName,ID_Marriage,MarriageName,ID_Nation,NationName,ID_Cultrul,CultrulName,ID_Vocation,VocationName,IDCard,ExamCard,Photo,BirthDay,Address,MobileNo,Email,SecondaryDiagnose,NormalDiagnose from OnCustPhysicalExamInfo ");
			stringBuilder.Append(" where ID_Customer=@ID_Customer ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt)
			};
			array[0].Value = ID_Customer;
			PEIS.Model.OnCustPhysicalExamInfo onCustPhysicalExamInfo = new PEIS.Model.OnCustPhysicalExamInfo();
			DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
			PEIS.Model.OnCustPhysicalExamInfo result;
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				if (dataSet.Tables[0].Rows[0]["ID_Customer"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_Customer = long.Parse(dataSet.Tables[0].Rows[0]["ID_Customer"].ToString());
				}
				onCustPhysicalExamInfo.CustomerName = dataSet.Tables[0].Rows[0]["CustomerName"].ToString();
				if (dataSet.Tables[0].Rows[0]["Is_Team"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_Team"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_Team"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_Team = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_Team = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["ID_Team"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_Team = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Team"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["Is_Paused"].ToString() != "")
				{
					onCustPhysicalExamInfo.Is_Paused = new int?(int.Parse(dataSet.Tables[0].Rows[0]["Is_Paused"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["Is_SectionLock"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_SectionLock"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_SectionLock"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_SectionLock = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_SectionLock = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["ID_ExamType"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_ExamType = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_ExamType"].ToString()));
				}
				onCustPhysicalExamInfo.ExamTypeName = dataSet.Tables[0].Rows[0]["ExamTypeName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_Set"].ToString() != "")
				{
					onCustPhysicalExamInfo.PEPackageID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Set"].ToString()));
				}
				onCustPhysicalExamInfo.PEPackageName = dataSet.Tables[0].Rows[0]["SetName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_ExamPlace"].ToString() != "")
				{
					onCustPhysicalExamInfo.ExamPlaceID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_ExamPlace"].ToString()));
				}
				onCustPhysicalExamInfo.ExamPlaceName = dataSet.Tables[0].Rows[0]["ExamPlaceName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_GuideNurse"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_GuideNurse = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_GuideNurse"].ToString()));
				}
				onCustPhysicalExamInfo.GuideNurse = dataSet.Tables[0].Rows[0]["GuideNurse"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_ReportWay"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_ReportWay = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_ReportWay"].ToString()));
				}
				onCustPhysicalExamInfo.ReportWayName = dataSet.Tables[0].Rows[0]["ReportWayName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_FeeWay"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_FeeWay = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_FeeWay"].ToString()));
				}
				onCustPhysicalExamInfo.FeeWayName = dataSet.Tables[0].Rows[0]["FeeWayName"].ToString();
				if (dataSet.Tables[0].Rows[0]["SecurityLevel"].ToString() != "")
				{
					onCustPhysicalExamInfo.SecurityLevel = new int?(int.Parse(dataSet.Tables[0].Rows[0]["SecurityLevel"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["DiseaseLevel"].ToString() != "")
				{
					onCustPhysicalExamInfo.DiseaseLevel = new int?(int.Parse(dataSet.Tables[0].Rows[0]["DiseaseLevel"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["Is_FeeSettled"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_FeeSettled"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_FeeSettled"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_FeeSettled = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_FeeSettled = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["Is_GuideSheetPrinted"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_GuideSheetPrinted"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_GuideSheetPrinted"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_GuideSheetPrinted = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_GuideSheetPrinted = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["Is_GuideSheetReturned"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_GuideSheetReturned"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_GuideSheetReturned"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_GuideSheetReturned = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_GuideSheetReturned = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["GuideSheetReturnedDate"].ToString() != "")
				{
					onCustPhysicalExamInfo.GuideSheetReturnedDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["GuideSheetReturnedDate"].ToString()));
				}
				onCustPhysicalExamInfo.GuideSheetReturnedby = dataSet.Tables[0].Rows[0]["GuideSheetReturnedby"].ToString();
				if (dataSet.Tables[0].Rows[0]["Is_UseHideCode"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_UseHideCode"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_UseHideCode"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_UseHideCode = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_UseHideCode = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_FinalFinished"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_FinalFinished = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_FinalFinished = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["ID_FinalDoctor"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_FinalDoctor = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_FinalDoctor"].ToString()));
				}
				onCustPhysicalExamInfo.FinalDoctor = dataSet.Tables[0].Rows[0]["FinalDoctor"].ToString();
				if (dataSet.Tables[0].Rows[0]["FinalDate"].ToString() != "")
				{
					onCustPhysicalExamInfo.FinalDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["FinalDate"].ToString()));
				}
				onCustPhysicalExamInfo.FinalOverView = dataSet.Tables[0].Rows[0]["FinalOverView"].ToString();
				onCustPhysicalExamInfo.FinalConclusion = dataSet.Tables[0].Rows[0]["FinalConclusion"].ToString();
				onCustPhysicalExamInfo.ResultCompare = dataSet.Tables[0].Rows[0]["ResultCompare"].ToString();
				onCustPhysicalExamInfo.MainDiagnose = dataSet.Tables[0].Rows[0]["MainDiagnose"].ToString();
				onCustPhysicalExamInfo.FinalDietGuide = dataSet.Tables[0].Rows[0]["FinalDietGuide"].ToString();
				onCustPhysicalExamInfo.FinalSportGuide = dataSet.Tables[0].Rows[0]["FinalSportGuide"].ToString();
				onCustPhysicalExamInfo.FinalHealthKnowlage = dataSet.Tables[0].Rows[0]["FinalHealthKnowlage"].ToString();
				if (dataSet.Tables[0].Rows[0]["Is_Checked"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_Checked"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_Checked"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_Checked = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_Checked = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["ID_Checker"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_Checker = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Checker"].ToString()));
				}
				onCustPhysicalExamInfo.Checker = dataSet.Tables[0].Rows[0]["Checker"].ToString();
				if (dataSet.Tables[0].Rows[0]["CheckedDate"].ToString() != "")
				{
					onCustPhysicalExamInfo.CheckedDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["CheckedDate"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_ReportReceipted"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_ReportReceipted = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_ReportReceipted = new bool?(false);
					}
				}
				if (dataSet.Tables[0].Rows[0]["SubScribDate"].ToString() != "")
				{
					onCustPhysicalExamInfo.SubScribDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["SubScribDate"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["OperateDate"].ToString() != "")
				{
					onCustPhysicalExamInfo.OperateDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["OperateDate"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["ID_Operator"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_Operator = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Operator"].ToString()));
				}
				onCustPhysicalExamInfo.Operator = dataSet.Tables[0].Rows[0]["Operator"].ToString();
				onCustPhysicalExamInfo.Note = dataSet.Tables[0].Rows[0]["Note"].ToString();
				if (dataSet.Tables[0].Rows[0]["Is_Subscribed"].ToString() != "")
				{
					onCustPhysicalExamInfo.Is_Subscribed = new int?(int.Parse(dataSet.Tables[0].Rows[0]["Is_Subscribed"].ToString()));
				}
				onCustPhysicalExamInfo.Invoice = dataSet.Tables[0].Rows[0]["Invoice"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_UserGuideSheetReturnedBy"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_UserGuideSheetReturnedBy = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_UserGuideSheetReturnedBy"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["ID_SubScriber"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_SubScriber = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_SubScriber"].ToString()));
				}
				onCustPhysicalExamInfo.SubScriber = dataSet.Tables[0].Rows[0]["SubScriber"].ToString();
				if (dataSet.Tables[0].Rows[0]["SubScriberOperDate"].ToString() != "")
				{
					onCustPhysicalExamInfo.SubScriberOperDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["SubScriberOperDate"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["Is_ExamStarted"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_ExamStarted"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_ExamStarted"].ToString().ToLower() == "true")
					{
						onCustPhysicalExamInfo.Is_ExamStarted = new bool?(true);
					}
					else
					{
						onCustPhysicalExamInfo.Is_ExamStarted = new bool?(false);
					}
				}
				onCustPhysicalExamInfo.TeamName = dataSet.Tables[0].Rows[0]["TeamName"].ToString();
				onCustPhysicalExamInfo.IndicatorDiagnose = dataSet.Tables[0].Rows[0]["IndicatorDiagnose"].ToString();
				onCustPhysicalExamInfo.OtherDiagnose = dataSet.Tables[0].Rows[0]["OtherDiagnose"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_Gender"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_Gender = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Gender"].ToString()));
				}
				onCustPhysicalExamInfo.GenderName = dataSet.Tables[0].Rows[0]["GenderName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_Marriage"].ToString() != "")
				{
					onCustPhysicalExamInfo.ID_Marriage = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Marriage"].ToString()));
				}
				onCustPhysicalExamInfo.MarriageName = dataSet.Tables[0].Rows[0]["MarriageName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_Nation"].ToString() != "")
				{
					onCustPhysicalExamInfo.NationID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Nation"].ToString()));
				}
				onCustPhysicalExamInfo.NationName = dataSet.Tables[0].Rows[0]["NationName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_Cultrul"].ToString() != "")
				{
					onCustPhysicalExamInfo.CultrulID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Cultrul"].ToString()));
				}
				onCustPhysicalExamInfo.CultrulName = dataSet.Tables[0].Rows[0]["CultrulName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_Vocation"].ToString() != "")
				{
					onCustPhysicalExamInfo.VocationID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_Vocation"].ToString()));
				}
				onCustPhysicalExamInfo.VocationName = dataSet.Tables[0].Rows[0]["VocationName"].ToString();
				onCustPhysicalExamInfo.IDCard = dataSet.Tables[0].Rows[0]["IDCard"].ToString();
				onCustPhysicalExamInfo.ExamCard = dataSet.Tables[0].Rows[0]["ExamCard"].ToString();
				if (dataSet.Tables[0].Rows[0]["Photo"].ToString() != "")
				{
					onCustPhysicalExamInfo.Photo = (byte[])dataSet.Tables[0].Rows[0]["Photo"];
				}
				if (dataSet.Tables[0].Rows[0]["BirthDay"].ToString() != "")
				{
					onCustPhysicalExamInfo.BirthDay = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["BirthDay"].ToString()));
				}
				onCustPhysicalExamInfo.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
				onCustPhysicalExamInfo.MobileNo = dataSet.Tables[0].Rows[0]["MobileNo"].ToString();
				onCustPhysicalExamInfo.Email = dataSet.Tables[0].Rows[0]["Email"].ToString();
				onCustPhysicalExamInfo.SecondaryDiagnose = dataSet.Tables[0].Rows[0]["SecondaryDiagnose"].ToString();
				onCustPhysicalExamInfo.NormalDiagnose = dataSet.Tables[0].Rows[0]["NormalDiagnose"].ToString();
				result = onCustPhysicalExamInfo;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public DataSet GetList(string strWhere)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ID_Customer,CustomerName,Is_Team,ID_Team,Is_Paused,Is_SectionLock,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,ExamPlaceID,ExamPlaceName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName,SecurityLevel,DiseaseLevel,Is_FeeSettled,Is_GuideSheetPrinted,Is_GuideSheetReturned,GuideSheetReturnedDate,GuideSheetReturnedby,Is_UseHideCode,Is_FinalFinished,ID_FinalDoctor,FinalDoctor,FinalDate,FinalOverView,FinalConclusion,ResultCompare,MainDiagnose,FinalDietGuide,FinalSportGuide,FinalHealthKnowlage,Is_Checked,ID_Checker,Checker,CheckedDate,Is_ReportReceipted,SubScribDate,OperateDate,ID_Operator,Operator,Note,Is_Subscribed,Invoice,ID_UserGuideSheetReturnedBy,ID_SubScriber,SubScriber,SubScriberOperDate,Is_ExamStarted,TeamName,IndicatorDiagnose,OtherDiagnose,ID_Gender,GenderName,ID_Marriage,MarriageName,NationID,NationName,CultrulID,CultrulName,VocationID,VocationName,IDCard,ExamCard,Photo,BirthDay,Address,MobileNo,Email,SecondaryDiagnose,NormalDiagnose ");
			stringBuilder.Append(" FROM OnCustPhysicalExamInfo ");
			if (strWhere.Trim() != "")
			{
				stringBuilder.Append(" where " + strWhere);
			}
			return DbHelperSQL.Query(stringBuilder.ToString());
		}

		public DataSet GetList(int Top, string strWhere, string filedOrder)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ");
			if (Top > 0)
			{
				stringBuilder.Append(" top " + Top.ToString());
			}
			stringBuilder.Append(" ID_Customer,CustomerName,Is_Team,ID_Team,Is_Paused,Is_SectionLock,ID_ExamType,ExamTypeName,PEPackageID,PEPackageName,ExamPlaceID,ExamPlaceName,ID_GuideNurse,GuideNurse,ID_ReportWay,ReportWayName,ID_FeeWay,FeeWayName,SecurityLevel,DiseaseLevel,Is_FeeSettled,Is_GuideSheetPrinted,Is_GuideSheetReturned,GuideSheetReturnedDate,GuideSheetReturnedby,Is_UseHideCode,Is_FinalFinished,ID_FinalDoctor,FinalDoctor,FinalDate,FinalOverView,FinalConclusion,ResultCompare,MainDiagnose,FinalDietGuide,FinalSportGuide,FinalHealthKnowlage,Is_Checked,ID_Checker,Checker,CheckedDate,Is_ReportReceipted,SubScribDate,OperateDate,ID_Operator,Operator,Note,Is_Subscribed,Invoice,ID_UserGuideSheetReturnedBy,ID_SubScriber,SubScriber,SubScriberOperDate,Is_ExamStarted,TeamName,IndicatorDiagnose,OtherDiagnose,ID_Gender,GenderName,ID_Marriage,MarriageName,NationID,NationName,CultrulID,CultrulName,VocationID,VocationName,IDCard,ExamCard,Photo,BirthDay,Address,MobileNo,Email,SecondaryDiagnose,NormalDiagnose ");
			stringBuilder.Append(" FROM OnCustPhysicalExamInfo ");
			if (strWhere.Trim() != "")
			{
				stringBuilder.Append(" where " + strWhere);
			}
			stringBuilder.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(stringBuilder.ToString());
		}
	}
}
