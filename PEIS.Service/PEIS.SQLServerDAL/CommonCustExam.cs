using PEIS.IDAL;
using PEIS.Model;
using PEIS.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace PEIS.SQLServerDAL
{
	public class CommonCustExam : CommonBase, ICommonCustExam
	{
		protected string[] QueryPagesCustExamListParam = new string[]
		{
			"ID_Customer",
			" * ",
			" ( SELECT TOP 99999999 [ID_CustExamSection] ,[ID_Customer] ,[ID_Section] ,[CustomerName] ,[SectionName]  ,[DiseaseLevel]  ,[SectionSummaryDate] ,[ID_SummaryDoctor] ,[SummaryDoctorName] ,[SectionSummary] ,[ID_Typist] ,[TypistName] ,[TypistDate]  ,[CheckerName]   ,[CheckDate]  ,[ID_Checker]  ,convert(varchar(50),SectionSummaryDate,20) SectionSummaryFormatDate   ,convert(varchar(50),CheckDate,20) CheckFormatDate  ,ISNULL(OCES.Is_Check,0) AS Is_Check  ,ISNULL(OCES.IS_giveup,0) AS IS_giveup, 'True' Is_GuideSheetPrinted  FROM OnCustExamSection OCES WHERE ID_Section = @ID_Section ORDER BY OCES.ID_Customer ASC ) ",
			" ORDER BY ID_Customer ASC "
		};

		protected string[] QueryPagesCustExamListNotExamed_Param = new string[]
		{
			"ID_Customer",
			"*",
			"( \r\n        SELECT  TOP 99999999 OCES.*,OCPEI.Is_GuideSheetPrinted FROM (\r\n\t        \tSELECT * FROM (\r\n\t            SELECT [ID_CustExamSection]\r\n                ,[ID_Customer]\r\n                ,[ID_Section]\r\n                ,[CustomerName]\r\n                ,[SectionName]\r\n                ,[DiseaseLevel]\r\n                ,[SectionSummaryDate]\r\n                ,[ID_SummaryDoctor]\r\n                ,[SummaryDoctorName]\r\n                ,[SectionSummary]\r\n                ,[ID_Typist]\r\n                ,[TypistName]\r\n                ,[TypistDate]\r\n                ,[CheckerName]\r\n                ,[CheckDate]\r\n                ,[ID_Checker]\r\n                ,convert(varchar(50),SectionSummaryDate,20) SectionSummaryFormatDate \r\n                ,convert(varchar(50),CheckDate,20) CheckFormatDate\r\n                ,ISNULL(Is_Check,0) AS Is_Check \r\n                ,ISNULL(IS_giveup,0) AS IS_giveup\r\n                ,ISNULL(IS_Refund,0) AS IS_Refund\r\n\t          FROM [OnCustExamSection] \r\n\t          WHERE ID_Section = @ID_Section ) OnCustExamSection\r\n\t          WHERE ISNULL(IS_giveup,0) = 0\r\n              AND ISNULL(IS_Refund,0) = 0\r\n\t          AND ISNULL(SummaryDoctorName,'') = '' )  OCES\r\n          INNER JOIN (\r\n\t\t        SELECT [ID_Customer],Is_GuideSheetPrinted\r\n\t\t        FROM [OnCustPhysicalExamInfo] \r\n\t\t        WHERE  ID_Operator is not null \r\n\t\t        AND ([OperateDate] between @BeginDate and @EndDate)\r\n\t            ) OCPEI \r\n\t        ON OCPEI.ID_Customer = OCES.ID_Customer\r\n            ORDER BY OCES.ID_Customer ASC \r\n         ) ",
			" ORDER BY ID_Customer ASC "
		};

		protected string[] QueryGuideSheetReturnedPagesParam = new string[]
		{
			"ID_Customer",
			"*",
			"( SELECT TOP 99999999 ID_Customer,\r\n                CustomerName,\r\n                Is_GuideSheetPrinted,\r\n                OperateDate SubScribDate,\r\n                Is_GuideSheetReturned,\r\n                GuideSheetReturnedby,\r\n                ID_UserGuideSheetReturnedBy,\r\n                GuideSheetReturnedDate,\r\n                Is_Team\r\n                FROM OnCustPhysicalExamInfo WHERE ISNULL(Is_GuideSheetReturned,0) = @Is_GuideSheetReturned  ORDER BY ID_Customer ASC \r\n                ) ",
			"ORDER BY ID_Customer ASC "
		};

		protected string[] QueryCustDiseaseLevelPagesParam = new string[]
		{
			"ID_Customer",
			"*",
			"( SELECT [ID_Customer]\r\n      ,[CustomerName]\r\n      ,[Is_Paused]\r\n      ,[Is_SectionLock]\r\n      ,[SecurityLevel]\r\n      ,ISNULL([DiseaseLevel],0) DiseaseLevel\r\n      ,[Is_FeeSettled]\r\n      ,[Is_UseHideCode]\r\n      ,[Is_FinalFinished]\r\n      ,[ID_FinalDoctor]\r\n      ,[FinalDoctor]\r\n      ,[FinalDate]\r\n      ,[Is_Checked]\r\n      ,[ID_Checker]\r\n      ,[Checker]\r\n      ,[CheckedDate]\r\n      ,[Is_ReportReceipted]\r\n      ,[SubScribDate]\r\n      ,[OperateDate]\r\n      ,[ID_Operator]\r\n      ,[Operator]\r\n      ,[Note]\r\n      ,[Is_Subscribed]\r\n      ,[ID_SubScriber]\r\n      ,[SubScriber]\r\n      ,[SubScriberOperDate]\r\n      ,[TeamName]\r\n      ,[ID_Gender]\r\n      ,[GenderName]\r\n      ,[IDCard]\r\n      ,[BirthDay]\r\n      ,FLOOR(DATEDIFF(DY,BirthDay,OperateDate)/365.25) Age\r\n      ,[MobileNo]\r\n      ,ISNULL([Is_DiseaseLevelInformed],0) Is_DiseaseLevelInformed\r\n      ,[ID_DiseaseLevelInformer]\r\n      ,[DiseaseLevelInformer]\r\n      ,[DiseaseLevelInformedDate]\r\n      ,[DiseaseLevelInformNote]\r\n  FROM [OnCustPhysicalExamInfo] ) ",
			"ORDER BY ID_Customer Asc"
		};

		protected string[] QueryCust_OAC_OCPEI_Param = new string[]
		{
			" SELECT *,FLOOR(DATEDIFF(DY,BirthDay,OperateDate)/365.25) Age FROM (\r\n              SELECT ID_Customer,CustomerName,ID_Gender,GenderName,BirthDay,ID_Marriage,MarriageName,MobileNo,IDCard,ExamCard,CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END OperateDate\r\n              FROM OnCustPhysicalExamInfo \r\n              WHERE ID_Customer IN \r\n              (@ID_Customer_Str)) OCPEI ;\r\n          "
		};

		protected string[] QueryCust_OAC_OCPEI_Param_OLD = new string[]
		{
			"SELECT OCRCPEI.ID_Customer, \r\n          OAC.ID_ArcCustomer,OAC.CustomerName,OAC.ID_Gender,OAC.GenderName,OAC.BirthDay,OAC.ID_Marriage,OAC.MarriageName,OAC.MobileNo,OAC.IDCard,OAC.ExamCard,FLOOR(DATEDIFF(DY,BirthDay,OCPHY.OperateDate)/365.25) Age\r\n          FROM \r\n          (SELECT * FROM OnArcCust WHERE ID_ArcCustomer in\r\n          (SELECT ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer IN \r\n          (@ID_Customer_Str)\r\n          )) AS OAC  \r\n          ,\r\n          (SELECT ID_Customer,ID_ArcCustomer FROM OnCustRelationCustPEInfo WHERE ID_Customer IN \r\n          (@ID_Customer_Str)\r\n          ) AS OCRCPEI \r\n          ,(SELECT CASE WHEN Is_GuideSheetPrinted=1 THEN OperateDate ELSE GETDATE() END OperateDate FROM OnCustPhysicalExamInfo WHERE ID_Customer IN \r\n          (@ID_Customer_Str)) OCPHY\r\n          WHERE OAC.ID_ArcCustomer = OCRCPEI.ID_ArcCustomer ;\r\n  \r\n          "
		};

		protected string[] QueryCust_FeeList_ExamList_Param = new string[]
		{
			"SELECT  BusFee.*, OnCustFee.ID_CustFee,OnCustFee.ID_ExamDoctor,OnCustFee.ExamDoctorName,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder \r\n                FROM \r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee \r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee\r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2\r\n                \r\n                AND BusFee.ID_Section = @ID_Section AND ID_Customer = @ID_Customer\r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n\r\n            SELECT BusExamItem.*,BusFee.ID_Fee , OnCustFee.ID_CustFee,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM \r\n                BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee \r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2\r\n                AND (BusExamItem.Forsex = 2 OR BusExamItem.Forsex = @Forsex )\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee \r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Customer = @ID_Customer\r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC; \r\n            \r\n            SELECT BusSymptom.*  , BusFee.ID_Fee,BusExamItem.GetResultWay,BusExamItem.Is_SymMultiValue , \r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder , \r\n                ISNULL(BusSymptom.DispOrder,500) as BusSymptomDispOrder \r\n                FROM \r\n                BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail,\r\n                BusSymptom\r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee \r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2\r\n                AND (BusExamItem.Forsex = 2 OR BusExamItem.Forsex = @Forsex )\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee \r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem \r\n                AND BusSymptom.ID_ExamItem = BusExamItem.ID_ExamItem \r\n                AND OnCustFee.ID_Customer = @ID_Customer \r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC,BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC,BusSymptomDispOrder ASC,BusSymptom.ID_Symptom ASC ; \r\n                \r\n            SELECT ID_CustExamSection,SectionSummaryDate,ID_SummaryDoctor,SummaryDoctorName,ID_Typist, TypistName, TypistDate,IS_giveup \r\n                FROM OnCustExamSection \r\n                WHERE ID_Customer = @ID_Customer  AND ID_Section = @ID_Section;\r\n\r\n                "
		};

		protected string[] QueryCust_FeeList_ExamList_Lab_Param = new string[]
		{
			"SELECT  [BusFee].[ID_Fee] ,[BusFee].[ID_Section] ,[BusFee].[ID_Specimen] ,[BusFee].[FeeName] ,[BusFee].[Forsex]\r\n\t\t\t      ,[BusFee].[ReportFeeName] ,[BusFee].[FeeCode] ,[BusFee].[Price] ,[BusFee].[InputCode] ,[BusFee].[SectionName]\r\n\t\t\t      ,[BusFee].[SpecimenName] ,[BusFee].[WorkGroupCode] ,[BusFee].[WorkStationCode] ,[BusFee].[WorkBenchCode] ,[BusFee].[CreateDate]\r\n\t\t\t      ,[BusFee].[Is_Banned] ,[BusFee].[ID_BanOpr] ,[BusFee].[BanDescribe] ,[BusFee].[DispOrder] ,[BusFee].[Note] ,[BusFee].[BreakfastOrder]\r\n\t\t\t      ,[BusFee].[Is_FeeNonPrintInReport] ,[BusFee].[InterfaceName] ,[BusFee].[IS_FeeReportMerger]\r\n\t\t\t      ,isnull([BusFee].[ID_FeeReportMerger],[BusFee].[ID_Fee]) ID_FeeReportMerger ,[BusFee].[OperationalDate]\r\n\t\t\t      , OnCustFee.ID_CustFee,OnCustFee.ID_ExamDoctor,OnCustFee.ExamDoctorName,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder  , FeeImageUrl \r\n                FROM \r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee \r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee\r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2\r\n                \r\n                AND BusFee.ID_Section = @ID_Section AND ID_Customer = @ID_Customer\r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n\r\n            SELECT BusExamItem.*,BusFee.ID_Fee , OnCustFee.ID_CustFee,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee \r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2\r\n                AND (BusExamItem.Forsex = 2 OR BusExamItem.Forsex = @Forsex )\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee \r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Customer = @ID_Customer\r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC,  BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC; \r\n            \r\n            SELECT ID_CustExamSection,SectionSummaryDate,ID_SummaryDoctor,SummaryDoctorName,ID_Typist, TypistName, TypistDate \r\n                FROM OnCustExamSection \r\n                WHERE ID_Customer = @ID_Customer  AND ID_Section = @ID_Section;\r\n\r\n\t\t\t--  读取作为父级的收费项目 (分单使用)\r\n\t\t\tSELECT  [BusFee].[ID_Fee] ,[BusFee].[ID_Section] ,[BusFee].[ID_Specimen] ,[BusFee].[FeeName] ,[BusFee].[Forsex]\r\n\t\t\t  ,[BusFee].[ReportFeeName] ,[BusFee].[FeeCode] ,[BusFee].[Price] ,[BusFee].[InputCode] ,[BusFee].[SectionName]\r\n\t\t\t  ,[BusFee].[SpecimenName] ,[BusFee].[WorkGroupCode] ,[BusFee].[WorkStationCode] ,[BusFee].[WorkBenchCode] ,[BusFee].[CreateDate]\r\n\t\t\t  ,[BusFee].[Is_Banned] ,[BusFee].[ID_BanOpr] ,[BusFee].[BanDescribe] ,[BusFee].[DispOrder] ,[BusFee].[Note] ,[BusFee].[BreakfastOrder]\r\n\t\t\t  ,[BusFee].[Is_FeeNonPrintInReport] ,[BusFee].[InterfaceName] ,[BusFee].[IS_FeeReportMerger]\r\n\t\t\t  ,ISNULL([BusFee].[ID_FeeReportMerger],[BusFee].[ID_Fee]) ID_FeeReportMerger ,[BusFee].[OperationalDate]\r\n\t\t\t  ,OnCustFee.ID_CustFee,OnCustFee.ID_ExamDoctor,OnCustFee.ExamDoctorName,OnCustFee.ID_Customer,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder \r\n                FROM \r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee \r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee \r\n                --AND BusFee.ID_FeeReportMerger = BusFee.ID_Fee\r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2\r\n                AND BusFee.ID_Section = @ID_Section AND ID_Customer = @ID_Customer\r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n                \r\n\r\n            -- 将发送分单信息 和 已经录入（读取）结果合并为一个表\r\n            SELECT * FROM (  \r\n\t\t\t-- 从接口表中读取发送的信息，包括申请单号，发送的对应收费项目编码 (分单使用)\r\n\t\t\tSELECT * FROM [FYH_Interface].[dbo].[OnCustSendToInterfaceApplyList]\r\n\t\t\t\tWHERE ID_Customer = @ID_Customer AND ID_Section = @ID_Section) OCSTIAL LEFT JOIN (\r\n            -- 从客户LAB申请单表中查询出已保存（已读取）的申请单信息 (分单使用)\r\n            SELECT ID_Apply,Is_TypistFinish,CheckDoctorName,ReportTime,ID_Typist,TypistName,TypistDate,DetectionDoctorName,ID_DetectionDoctor\r\n\t\t\t\tFROM [OnCustApply]\r\n\t\t\t\tWHERE ID_Customer = @ID_Customer AND ID_Section = @ID_Section ) OCA ON OCSTIAL.ApplyID = OCA.ID_Apply\r\n\t\t\t\tUNION\r\n            -- 将发送分单信息 和 已经录入（读取）结果合并为一个表\r\n            SELECT * FROM (  \r\n\t\t\t-- 从接口表中读取发送的信息，包括申请单号，发送的对应收费项目编码 (分单使用)\r\n\t\t\tSELECT * FROM [FYH_Interface].[dbo].[OnCustSendToInterfaceApplyList]\r\n\t\t\t\tWHERE ID_Customer = @ID_Customer AND ID_Section = @ID_Section) OCSTIAL RIGHT JOIN (\r\n            -- 从客户LAB申请单表中查询出已保存（已读取）的申请单信息 (分单使用)\r\n            SELECT ID_Apply,Is_TypistFinish,CheckDoctorName,ReportTime,ID_Typist,TypistName,TypistDate,DetectionDoctorName,ID_DetectionDoctor\r\n\t\t\t\tFROM [OnCustApply]\r\n\t\t\t\tWHERE ID_Customer = @ID_Customer AND ID_Section = @ID_Section ) OCA ON OCSTIAL.ApplyID = OCA.ID_Apply;\r\n\r\n                "
		};

		protected string[] QueryCust_Exam_SaveData_Param = new string[]
		{
			"SELECT Is_AutoCalc,GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,[OnCustExamItem].*,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n\r\n                SELECT * FROM OnCustExamItemResult,\r\n\t            (SELECT [OnCustExamItem].ID_CustExamItem\r\n                FROM [OnCustExamItem], BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ) AS OnCustExamItem\r\n                WHERE OnCustExamItemResult.ID_CustExamItem = OnCustExamItem.ID_CustExamItem\r\n                ORDER BY OnCustExamItemResult.ID_CustExamItem ASC;\r\n            \r\n                SELECT * \r\n                FROM OnCustExamSection \r\n                WHERE ID_Customer = @ID_Customer  AND ID_Section = @ID_Section;\r\n                "
		};

		protected string[] QueryCust_GetSummaryExamList_Lab_Param = new string[]
		{
			"SELECT  BusFee.*, OnCustFee.ID_CustFee,OnCustFee.ID_ExamDoctor,OnCustFee.ExamDoctorName,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder \r\n                FROM \r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section ) \r\n                AS BusFee \r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee\r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2 \r\n                \r\n                AND BusFee.ID_Section = @ID_Section AND ID_Customer = @ID_Customer\r\n                order by BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n\r\n            SELECT BusFee.ID_Section,OnCustFee.ID_Customer,BusExamItem.ExamItemCode,GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,[OnCustExamItem].*,\r\n                convert(varchar(20),OnCustExamItem.ExamItemSummaryDate,23) FormatExamItemSummaryDate,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                ResultLabMark != '' \r\n                AND BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n                \r\n                SELECT * FROM SYSSection WHERE ID_Section = @ID_Section ;\r\n\r\n                SELECT [ID_CustExamSection]\r\n                      ,[ID_Customer]\r\n                      ,[ID_Section]\r\n                      ,isnull([Is_Check],0) Is_Check\r\n                      ,[CheckerName]\r\n                      ,[CheckDate]\r\n                      ,[ID_Checker]\r\n                      ,isnull([IS_giveup],0) IS_giveup\r\n                      ,isnull([IS_Refund],0) IS_Refund\r\n                  FROM [FYH].[dbo].[OnCustExamSection]\r\n                  where ID_Customer = @ID_Customer\r\n                  and ID_Section =  @ID_Section; "
		};

		protected string[] QueryCust_GetNotExamItemIDList_Lab_Param = new string[]
		{
			"\r\n        \t-- 查询未传回检查值的项目\r\n            SELECT BusExamItem.ID_ExamItem\r\n                FROM BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer  ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE [OnCustFee].ID_Fee = BusFee.ID_Fee \r\n                AND ISNULL(OnCustFee.CustFeeChargeState,0) <> 2 \r\n                AND (BusExamItem.Forsex = 2 OR BusExamItem.Forsex = @Forsex )\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee \r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Customer = @ID_Customer \r\n                AND ISNULL(Is_LisValueNull,0) = 0 \r\n                AND BusExamItem.ID_ExamItem not in (\r\n                \r\n\t\t\t-- 已经传回值的检查项目\r\n            SELECT [OnCustExamItem].ID_ExamItem\r\n                FROM [OnCustExamItem],BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer  ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                ResultLabValues != '' \r\n                AND BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ) "
		};

		protected string[] QueryCust_ExamSection_SaveData_Param = new string[]
		{
			"    SELECT * \r\n                FROM OnCustExamSection \r\n                WHERE ID_Customer = @ID_Customer  AND ID_Section = @ID_Section;\r\n                "
		};

		protected string[] QueryCust_DiseaseLevelExamResult_Param = new string[]
		{
			"SELECT Is_AutoCalc,GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,[OnCustExamItem].*,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem, BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee, BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                AND DiseaseLevel > 0\r\n                ORDER BY DiseaseLevel DESC, BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n                \r\n                "
		};

		protected string[] QueryCust_DiseaseLevelInform_Param = new string[]
		{
			"SELECT [Is_DiseaseLevelInformed]\r\n                  ,[ID_DiseaseLevelInformer]\r\n                  ,[DiseaseLevelInformer]\r\n                  ,[DiseaseLevelInformedDate]\r\n                  ,[DiseaseLevelInformNote]\r\n              FROM [OnCustPhysicalExamInfo]\r\n              WHERE ID_Customer = @ID_Customer; \r\n                "
		};

		protected string[] QueryCust_Exam_SaveData_Lab_Param = new string[]
		{
			"SELECT GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,[OnCustExamItem].*,\r\n                convert(varchar(20),OnCustExamItem.ExamItemSummaryDate,23) FormatExamItemSummaryDate,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n\r\n                SELECT BusFee.ID_Section,OnCustFee.*,convert(varchar(20),OnCustFee.ExamDate,23) FormatExamDate,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder \r\n                FROM \r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ExamDate is not null AND OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee\r\n                WHERE OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n\r\n                SELECT * \r\n                FROM OnCustExamSection \r\n                WHERE ID_Customer = @ID_Customer  AND ID_Section = @ID_Section;\r\n\r\n\r\n            -- 从客户LAB申请单表中查询出已保存（已读取）的申请单信息 (分单使用)\r\n            SELECT ID_Apply,Is_TypistFinish,CheckDoctorName,ReportTime,ID_Typist,TypistName,TypistDate,DetectionDoctorName,ID_DetectionDoctor\r\n\t\t\t\tFROM [OnCustApply]\r\n\t\t\t\tWHERE ID_Customer = @ID_Customer AND ID_Section = @ID_Section;\r\n\r\n                "
		};

		protected string[] CountCustomerID_Param = new string[]
		{
			"SELECT Count(ID_Customer)\r\n          FROM [OnCustPhysicalExamInfo]\r\n          WHERE ID_Customer = @ID_Customer; \r\n             "
		};

		protected string[] GetCustomerLisPacsFeeItem = new string[]
		{
			"SELECT OCF.ID_Fee\r\n                  ,NS.ID_Section\r\n                  ,FeeName\r\n                  ,Forsex\r\n                  ,FeeCode\r\n                  ,Price\r\n                  ,NS.SectionName\r\n                  ,SpecimenName\r\n                  ,InterfaceName\r\n                  ,InterfaceType\r\n                  ,NS.DispOrder SectionDispOrder\r\n                  ,BF.DispOrder FeeDispOrder\r\n              FROM BusFee BF\r\n            LEFT JOIN\r\n            OnCustFee OCF\r\n            ON BF.ID_Fee = OCF.ID_Fee \r\n            LEFT JOIN SYSSection  NS\r\n            ON NS.ID_Section = BF.ID_Section\r\n            WHERE InterfaceType IN ( 'LAB' ,'pacs') \r\n            AND ID_Customer = @ID_Customer\r\n            ORDER BY SectionDispOrder ASC,FeeDispOrder ASC;\r\n             "
		};

		protected string[] CustomerInfoByID_Param = new string[]
		{
			"SELECT SecurityLevel,ID_Customer,ISNULL(Is_Subscribed,0) Is_Subscribed\r\n          FROM [OnCustPhysicalExamInfo]\r\n          WHERE ID_Customer = @ID_Customer; \r\n             "
		};

		protected string[] QueryGuideSheetReturn_ExamSectionList_Param = new string[]
		{
            "SELECT ID_Customer, \r\n                [dbo].StrToCode128(ID_Customer) ID_CustomerCode128,\r\n                CustomerName,\r\n                Is_Team,\r\n                Is_GuideSheetPrinted,\r\n                Is_GuideSheetReturned,\r\n                GuideSheetReturnedby,\r\n                ID_UserGuideSheetReturnedBy,\r\n                GuideSheetReturnedDate,\r\n                Is_FinalFinished,\r\n                ID_FinalDoctor,\r\n                FinalDoctor,\r\n                FinalDate,\r\n                Is_Checked,\r\n                ID_Checker,\r\n                Checker,\r\n                CheckedDate,GenderName\r\n            FROM OnCustPhysicalExamInfo \r\n           WHERE ID_Customer =  @ID_Customer;\r\n\r\n            SELECT * FROM (\r\n\t\t        SELECT [ID_CustExamSection]\r\n                      ,[ID_Customer]\r\n                      ,[ID_Section]\r\n                      ,[CustomerName]\r\n                      ,[SectionName]\r\n                      ,[DiseaseLevel]\r\n                      ,[SectionSummaryDate]\r\n                      ,[ID_SummaryDoctor]\r\n                      ,[SummaryDoctorName]\r\n                      ,[ID_Typist]\r\n                      ,[TypistName]\r\n                      ,[TypistDate]\r\n                      ,[Is_Check]\r\n                      ,[CheckerName]\r\n                      ,[CheckDate]\r\n                      ,[ID_Checker]\r\n                      ,[IS_giveup]\r\n                      ,[IS_Refund]\r\n                  FROM [OnCustExamSection]\r\n\t\t         WHERE ID_Customer = @ID_Customer ) \r\n              OCES ,\r\n              SYSSection NS\r\n              WHERE OCES.ID_Section = NS.SectionID \r\n              AND ISNULL(OCES.IS_Refund,0) <> 1\r\n              AND NS.FunctionType = 0 ;\r\n          \r\n                "
        };

		protected string[] QueryCustomerExamSectionList_Param = new string[]
		{
			"SELECT * FROM (\r\n\t\tSELECT [ID_CustExamSection]\r\n              ,[ID_Customer]\r\n              ,[ID_Section]\r\n              ,[CustomerName]\r\n              ,[DiseaseLevel]\r\n              ,CONVERT(varchar(10),[SectionSummaryDate],120) [SectionSummaryDate]\r\n              ,[SectionSummary]\r\n              ,[PositiveSummary]\r\n              ,[ID_SummaryDoctor]\r\n              ,[SummaryDoctorName]\r\n              ,[ID_Typist]\r\n              ,[TypistName]\r\n              ,[TypistDate]\r\n              ,[Is_Check]\r\n              ,[CheckerName]\r\n              ,[CheckDate]\r\n              ,[ID_Checker]\r\n              ,[IS_giveup]\r\n              ,[IS_Refund]\r\n          FROM [OnCustExamSection]\r\n\t\t WHERE ID_Customer = @ID_Customer ) \r\n      OCES ,\r\n      SYSSection NS\r\n      WHERE OCES.ID_Section = NS.ID_Section \r\n      AND NS.Is_NonFunction = 0 \r\n      AND (OCES.IS_Refund = 0 OR IS_Refund is null )\r\n      ORDER BY ns.DispOrder ASC;\r\n\r\n                "
		};

		protected string[] QueryCustomerIDList_Param = new string[]
		{
			"SELECT [ID_CustRelation]\r\n          ,[ID_ArcCustomer]\r\n          ,[IDCardNo]\r\n          ,[ExamCardNo]\r\n          ,[ID_Customer]\r\n          ,[Is_CompletePhysical]\r\n          ,[ExamState]\r\n      FROM [OnCustRelationCustPEInfo]\r\n      where [ID_ArcCustomer] = (\r\n    SELECT [ID_ArcCustomer]\r\n      FROM [OnArcCust]\r\n      where [CustomerName] = @CustomerName \r\n          and [IDCard] = @IDCard );\r\n\r\n                "
		};

		protected string[] QueryCustomerExamItemListAndSummary_Param = new string[]
		{
			"SELECT GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,[OnCustExamItem].*,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n\r\n                SELECT OCES.* ,NS.SummaryName\r\n                FROM OnCustExamSection OCES , SYSSection NS\r\n                WHERE OCES.ID_Section = NS.ID_Section \r\n                AND NS.Is_NonFunction = 0 \r\n                AND OCES.ID_Customer = @ID_Customer  AND OCES.ID_Section = @ID_Section;\r\n                "
		};

		protected string[] QueryCustomerFeeItemExamItemListAndSummary_Param = new string[]
		{
			"SELECT [OnCustExamItem].ID_CustFee,[OnCustExamItem].ID_Fee,BusFee.FeeName,[OnCustExamItem].ID_ExamItem,[OnCustExamItem].ID_CustExamItem,[OnCustExamItem].ExamItemName,\r\n                GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,ResultLabValues,ResultLabUnit,ResultLabMark,ResultSummary,ResultLabRange,\r\n                OnCustFee.Is_FeeRefund,OnCustFee.Is_Examined,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                (SELECT * FROM BusFeeDetail WHERE BusFeeDetail.ID_Fee IN (SELECT  BusFee.ID_Fee FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) )\r\n                AS BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n\r\n                SELECT BusFee.ID_Section,OnCustFee.ID_Fee,OnCustFee.ID_CustFee,OnCustFee.FeeItemName,OnCustFee.Is_FeeRefund,OnCustFee.Is_Examined,\r\n                convert(varchar(20),OnCustFee.ExamDate,23) FormatExamDate,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder \r\n                FROM \r\n                (SELECT  BusFee.* FROM BusFee WHERE  BusFee.ID_Section = @ID_Section  ) \r\n                AS BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE  OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee\r\n                WHERE OnCustFee.ID_Fee = BusFee.ID_Fee AND ISNULL(OnCustFee.Is_FeeRefund,0) = 0\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n\r\n                SELECT OCES.* ,NS.SummaryName\r\n                FROM OnCustExamSection OCES , SYSSection NS\r\n                WHERE OCES.ID_Section = NS.ID_Section \r\n                AND NS.Is_NonFunction = 0 \r\n                AND OCES.ID_Customer = @ID_Customer  AND OCES.ID_Section = @ID_Section;\r\n                "
		};

		protected string[] QuerySectionFeeItemList_Param = new string[]
		{
			"SELECT ID_Fee\r\n                  ,ID_Section\r\n                  ,FeeName\r\n                  ,SectionName\r\n                  ,SpecimenName\r\n                  ,WorkGroupCode\r\n                  ,WorkStationCode\r\n                  ,WorkBenchCode\r\n                  ,Is_Banned\r\n                  ,DispOrder\r\n                  ,'0' IsShowCompareInfo\r\n              FROM [BusFee]\r\n              WHERE [ID_Section] = @ID_Section\r\n              ORDER BY DispOrder ASC ,ID_Fee ASC;"
		};

		protected string[] QueryCustomerExamItemDiseaseLevelTips_Param = new string[]
		{
			"SELECT top 1 [ID_CustExamItem]\r\n              ,[ID_CustFee]\r\n              ,[ID_Fee]\r\n              ,[ID_ExamItem]\r\n              ,[ExamItemName]\r\n              ,[DiseaseLevel]\r\n              ,[ResultSummary]\r\n              ,[SummaryDoctorName]\r\n              ,[ExamItemSummaryDate]\r\n              ,[ResultLabRange]\r\n          FROM [OnCustExamItem]\r\n          WHERE ID_CustFee IN ( \r\n          SELECT ID_CustFee FROM OnCustFee WHERE ID_Customer = @ID_Customer\r\n          )\r\n          AND DiseaseLevel >= @DiseaseLevel \r\n          ORDER BY\tDiseaseLevel DESC;"
		};

		DataTable ICommonCustExam.GetPage(string pageCode, int pageIndex, int pageSize, out int recordCount, out int pageCount, params SqlConditionInfo[] conditions)
		{
			return base.GetPage(pageCode, pageIndex, pageSize, out recordCount, out pageCount, conditions);
		}

		DataSet ICommonCustExam.ExcuteQuerySql(string QuerySqlCode, params SqlConditionInfo[] conditions)
		{
			return base.ExcuteQuerySql(QuerySqlCode, conditions);
		}

		protected new string[] GetSqlSentence(string PageName)
		{
			FieldInfo field = base.GetType().GetField(PageName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new Exception("没有找到SQL");
			}
			return (string[])field.GetValue(this);
		}

		public PEIS.Model.OnCustPhysicalExamInfo GetOCPEIModel(long ID_Customer, string ConfigName)
		{
			string connectionString = PEIS.DBUtility.PubConstant.GetConnectionString(ConfigName);
			PEIS.Model.OnCustPhysicalExamInfo result;
			if (string.IsNullOrEmpty(connectionString))
			{
				result = null;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("select  top 1 * from OnCustPhysicalExamInfo ");
				stringBuilder.Append(" where ID_Customer='" + ID_Customer + "';");
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID_Customer", SqlDbType.BigInt)
				};
				array[0].Value = ID_Customer;
				PEIS.Model.OnCustPhysicalExamInfo onCustPhysicalExamInfo = new PEIS.Model.OnCustPhysicalExamInfo();
				DataSet dataSet = DbHelperSQL.Query(connectionString, stringBuilder.ToString(), array);
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
					if (dataSet.Tables[0].Rows[0]["PEPackageID"].ToString() != "")
					{
						onCustPhysicalExamInfo.PEPackageID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["PEPackageID"].ToString()));
					}
					onCustPhysicalExamInfo.PEPackageName = dataSet.Tables[0].Rows[0]["PEPackageName"].ToString();
					if (dataSet.Tables[0].Rows[0]["ExamPlaceID"].ToString() != "")
					{
						onCustPhysicalExamInfo.ExamPlaceID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ExamPlaceID"].ToString()));
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
					if (dataSet.Tables[0].Rows[0]["NationID"].ToString() != "")
					{
						onCustPhysicalExamInfo.NationID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["NationID"].ToString()));
					}
					onCustPhysicalExamInfo.NationName = dataSet.Tables[0].Rows[0]["NationName"].ToString();
					if (dataSet.Tables[0].Rows[0]["CultrulID"].ToString() != "")
					{
						onCustPhysicalExamInfo.CultrulID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["CultrulID"].ToString()));
					}
					onCustPhysicalExamInfo.CultrulName = dataSet.Tables[0].Rows[0]["CultrulName"].ToString();
					if (dataSet.Tables[0].Rows[0]["VocationID"].ToString() != "")
					{
						onCustPhysicalExamInfo.VocationID = new int?(int.Parse(dataSet.Tables[0].Rows[0]["VocationID"].ToString()));
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
			}
			return result;
		}

		public DataSet GetCustRelationCustPEInfo(long ID_Customer, string IDCardNo, string ExamCardNo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT  top 1 * FROM OnCustRelationCustPEInfo ");
			SqlParameter[] array;
			DataSet result;
			if (ID_Customer > 0L)
			{
				stringBuilder.Append(" WHERE ID_Customer=@ID_Customer ");
				array = new SqlParameter[]
				{
					new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
				};
				array[0].Value = ID_Customer;
			}
			else if (!string.IsNullOrEmpty(IDCardNo))
			{
				stringBuilder.Append(" WHERE IDCardNo=@IDCardNo ");
				array = new SqlParameter[]
				{
					new SqlParameter("@IDCardNo", SqlDbType.VarChar)
				};
				array[0].Value = IDCardNo;
			}
			else
			{
				if (string.IsNullOrEmpty(ExamCardNo))
				{
					result = null;
					return result;
				}
				stringBuilder.Append(" WHERE ExamCardNo=@ExamCardNo ");
				array = new SqlParameter[]
				{
					new SqlParameter("@ExamCardNo", SqlDbType.VarChar)
				};
				array[0].Value = ExamCardNo;
			}
			DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				result = dataSet;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public DataSet GetArcCustomerInfo(string IDCard, string ExamCard)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT  top 1 ID_ArcCustomer,ID_Gender,ID_Marriage,NationID,CultrulID,VocationID,CustomerName,IDCard,ExamCard,Photo,BirthDay,GenderName,MarriageName,NationName,Address,MobileNo,Email,CultrulName,VocationName,FinishedNum,UnfinishedNum,FirstDatePE,LatestDatePE FROM OnArcCust ");
			SqlParameter[] array;
			DataSet result;
			if (!string.IsNullOrEmpty(IDCard))
			{
				stringBuilder.Append(" WHERE IDCard=@IDCard ");
				array = new SqlParameter[]
				{
					new SqlParameter("@IDCard", SqlDbType.VarChar)
				};
				array[0].Value = IDCard;
			}
			else
			{
				if (string.IsNullOrEmpty(ExamCard))
				{
					result = null;
					return result;
				}
				stringBuilder.Append(" WHERE ExamCard=@ExamCard ");
				array = new SqlParameter[]
				{
					new SqlParameter("@ExamCard", SqlDbType.VarChar)
				};
				array[0].Value = ExamCard;
			}
			DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				result = dataSet;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public DataSet GetOnArcCustomerAndPEInfo(long ID_Customer, string IDCard, string ExamCard)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT  top 1 * FROM OnArcCust ");
			SqlParameter[] array;
			DataSet result;
			if (!string.IsNullOrEmpty(IDCard))
			{
				stringBuilder.Append(" WHERE IDCard=@IDCard; ");
				array = new SqlParameter[2];
				array[0] = new SqlParameter("@IDCard", SqlDbType.VarChar);
				array[0].Value = IDCard;
			}
			else
			{
				if (string.IsNullOrEmpty(ExamCard))
				{
					result = null;
					return result;
				}
				stringBuilder.Append(" WHERE ExamCard=@ExamCard; ");
				array = new SqlParameter[2];
				array[0] = new SqlParameter("@ExamCard", SqlDbType.VarChar);
				array[0].Value = ExamCard;
			}
			if (array != null)
			{
				array[1] = new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8);
				array[1].Value = ID_Customer;
			}
			stringBuilder.Append(" SELECT * FROM OnCustPhysicalExamInfo WHERE ID_Customer=@ID_Customer; ");
			DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				result = dataSet;
			}
			else
			{
				result = null;
			}
			return result;
		}

		int ICommonCustExam.SaveCustomerSectionSummary(List<PEIS.Model.OnCustExamItem> OnCustExamItemModelList, List<PEIS.Model.OnCustFee> OnCustFeeModelList, PEIS.Model.OnCustExamSection OnCustExamSectionModel, PEIS.Model.OnCustApply OnCustApplyModel, int ID_FeeReportMerger, params SqlConditionInfo[] conditions)
		{
			string connectionString = PEIS.DBUtility.PubConstant.ConnectionString;
			int result;
			using (SqlConnection sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
				{
					SqlCommand sqlCommand = new SqlCommand();
					try
					{
						sqlCommand.Connection = sqlConnection;
						sqlCommand.Transaction = sqlTransaction;
						int num = this.UpdateSectionSummary(sqlCommand, OnCustExamSectionModel);
						if (num > 0)
						{
							num += this.UpdateOnCustFeeItem(sqlCommand, OnCustFeeModelList);
							num += this.WriteCustApplyItem(sqlCommand, OnCustApplyModel);
							if (conditions != null)
							{
								if (conditions[0].ParamName == "@Is_FirstSaveSummary")
								{
									if (conditions[0].ParamValue.ToString() == "0")
									{
										num += this.UpdateExamDoctor(sqlCommand, OnCustFeeModelList, OnCustExamSectionModel);
										if (conditions[1].ParamName == "@IS_NotUseLastSaveData" && conditions[1].ParamValue.ToString() == "1")
										{
											num += this.ClearCustExamItemResult(sqlCommand, OnCustFeeModelList);
										}
										else
										{
											num += this.DeleteCustExamItemResult(sqlCommand, OnCustExamItemModelList);
										}
									}
								}
							}
							num += this.WriteCustExamItemResult(sqlCommand, OnCustExamItemModelList);
						}
						if (num > 0)
						{
							sqlTransaction.Commit();
						}
						result = num;
					}
					catch (Exception ex)
					{
						sqlTransaction.Rollback();
						throw ex;
					}
				}
			}
			return result;
		}

		int ICommonCustExam.UpdateSectionSummaryCheckState(PEIS.Model.OnCustExamSection OCESModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("ID_Typist=@ID_Typist,");
			stringBuilder.Append("TypistName=@TypistName,");
			stringBuilder.Append("TypistDate=@TypistDate,");
			stringBuilder.Append("Is_Check=@Is_Check,");
			stringBuilder.Append("ID_Checker=@ID_Checker,");
			stringBuilder.Append("CheckerName=@CheckerName,");
			stringBuilder.Append("CheckDate=@CheckDate");
			stringBuilder.Append(" WHERE ID_CustExamSection = @ID_CustExamSection ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@TypistName", SqlDbType.VarChar),
				new SqlParameter("@TypistDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Check", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@CheckerName", SqlDbType.VarChar),
				new SqlParameter("@CheckDate", SqlDbType.DateTime),
				new SqlParameter("@ID_CustExamSection", SqlDbType.BigInt)
			};
			array[0].Value = OCESModel.ID_Typist;
			array[1].Value = OCESModel.TypistName;
			array[2].Value = OCESModel.TypistDate;
			array[3].Value = OCESModel.Is_Check;
			array[4].Value = OCESModel.ID_Checker;
			array[5].Value = OCESModel.CheckerName;
			array[6].Value = OCESModel.CheckDate;
			array[7].Value = OCESModel.ID_CustExamSection;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateSectionSummaryCheckState(PEIS.Model.SYSOpUser UserModel, int CheckState, List<int> IDList)
		{
			string value = string.Empty;
			for (int i = 0; i < IDList.Count; i++)
			{
				if (i == 0)
				{
					value = IDList[i].ToString();
				}
				else
				{
					value = "," + IDList[i].ToString();
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("ID_Typist=@ID_Typist,");
			stringBuilder.Append("TypistName=@TypistName,");
			stringBuilder.Append("TypistDate=@TypistDate,");
			stringBuilder.Append("Is_Check=@Is_Check,");
			stringBuilder.Append("ID_Checker=@ID_Checker,");
			stringBuilder.Append("CheckerName=@CheckerName,");
			stringBuilder.Append("CheckDate=@CheckDate");
			stringBuilder.Append(" WHERE ID_CustExamSection in ( ");
			stringBuilder.Append(value);
			stringBuilder.Append(" ) ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@TypistName", SqlDbType.VarChar),
				new SqlParameter("@TypistDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Check", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@CheckerName", SqlDbType.VarChar),
				new SqlParameter("@CheckDate", SqlDbType.DateTime)
			};
			array[0].Value = UserModel.UserID;
			array[1].Value = UserModel.UserName;
			array[2].Value = DateTime.Now;
			array[3].Value = CheckState;
			array[4].Value = UserModel.UserID;
			array[5].Value = UserModel.UserName;
			array[6].Value = DateTime.Now;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateSectionImageUrl(int ID_CustExamSection, string ImageUrl)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("SummaryDoctorName='--'");
			stringBuilder.Append(" WHERE ID_CustExamSection = @ID_CustExamSection and isnull(SummaryDoctorName,'') = '' ;");
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("ImageUrl=@ImageUrl");
			stringBuilder.Append(" WHERE ID_CustExamSection = @ID_CustExamSection");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ImageUrl", SqlDbType.VarChar),
				new SqlParameter("@ID_CustExamSection", SqlDbType.Int, 4)
			};
			array[0].Value = ImageUrl;
			array[1].Value = ID_CustExamSection;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateLisCustFeeExamItem(long ID_Customer, int ID_Section, PEIS.Model.SYSOpUser LoginUserModel)
		{
			string text = string.Format(" UPDATE OnCustFee SET Is_Examined = 1,ID_ExamDoctor={2},ExamDoctorName='{3}',ExamDate=GetDate() WHERE Is_Examined = 0 and [ID_CustFee] IN\r\n                                ( SELECT [ID_CustFee]\r\n                                  FROM [OnCustFee]\r\n                                  WHERE [ID_Customer] = {0}\r\n                                  AND [ID_Fee] IN (\r\n                                SELECT [ID_Fee] FROM [BusFee] \r\n                                  WHERE [ID_Section] = {1}\r\n                                  )\r\n                                ); ", new object[]
			{
				ID_Customer,
				ID_Section,
				LoginUserModel.UserID,
				LoginUserModel.UserName
			});
			return DbHelperSQL.ExecuteSql(text.ToString());
		}

		int ICommonCustExam.DeleteCustomerExamItem(long ID_Customer, int ID_Section, string InterFaceType)
		{
			string value;
			if (InterFaceType == "LAB")
			{
				value = string.Format("\r\n                DELETE FROM OnCustExamItem where ID_CustFee in (\r\n\t                SELECT [ID_CustFee]\r\n                    FROM [OnCustFee]\r\n                    where ID_Customer = {0} and ID_Fee in (SELECT id_fee from BusFee WHERE ID_Section = {1}) \r\n                ); ", ID_Customer, ID_Section);
			}
			else
			{
				value = string.Format("\r\n                DELETE FROM OnCustExamItemResult where ID_CustExamItem in (\r\n                SELECT ID_CustExamItem FROM OnCustExamItem where ID_CustFee in (\r\n\t                SELECT [ID_CustFee]\r\n                    FROM [OnCustFee]\r\n                    WHERE ID_Customer = {0} and ID_Fee in (SELECT id_fee FROM BusFee WHERE ID_Section = {1}) ) \r\n                );\r\n                DELETE FROM OnCustExamItem where ID_CustFee in (\r\n\t                SELECT [ID_CustFee]\r\n                    FROM [OnCustFee]\r\n                    where ID_Customer = {0} and ID_Fee in (SELECT id_fee from BusFee WHERE ID_Section = {1}) \r\n                ); ", ID_Customer, ID_Section);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("ImageUrl='',ExamProjectName='',");
			stringBuilder.Append("SectionSummaryDate=@SectionSummaryDate,");
			stringBuilder.Append("SectionSummary=@SectionSummary,");
			stringBuilder.Append("PositiveSummary=@PositiveSummary,");
			stringBuilder.Append("ID_SummaryDoctor=@ID_SummaryDoctor,");
			stringBuilder.Append("SummaryDoctorName=@SummaryDoctorName,");
			stringBuilder.Append("ID_Typist=@ID_Typist,");
			stringBuilder.Append("TypistName=@TypistName,");
			stringBuilder.Append("TypistDate=@TypistDate,");
			stringBuilder.Append("Is_Check=@Is_Check,");
			stringBuilder.Append("CheckerName=@CheckerName,");
			stringBuilder.Append("CheckDate=@CheckDate,");
			stringBuilder.Append("ID_Checker=@ID_Checker,");
			stringBuilder.Append("DiseaseLevel=@DiseaseLevel ");
			stringBuilder.Append(" WHERE ID_Section=@ID_Section AND ID_Customer=@ID_Customer;  ");
			stringBuilder.Append("update OnCustFee set ");
			stringBuilder.Append("Is_Examined=@Is_Examined,");
			stringBuilder.Append("ID_ExamDoctor=@ID_ExamDoctor,");
			stringBuilder.Append("ExamDoctorName=@ExamDoctorName,");
			stringBuilder.Append("FeeImageUrl='',");
			stringBuilder.Append("ExamDate=@ExamDate ");
			stringBuilder.Append(" WHERE  ID_Customer=@ID_Customer and ID_Fee in (select id_fee from BusFee where ID_Section = @ID_Section ); ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@SectionSummaryDate", SqlDbType.DateTime),
				new SqlParameter("@SectionSummary", SqlDbType.Text),
				new SqlParameter("@PositiveSummary", SqlDbType.Text),
				new SqlParameter("@ID_SummaryDoctor", SqlDbType.Int, 4),
				new SqlParameter("@SummaryDoctorName", SqlDbType.VarChar),
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@TypistName", SqlDbType.VarChar),
				new SqlParameter("@TypistDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Check", SqlDbType.Bit, 1),
				new SqlParameter("@CheckerName", SqlDbType.VarChar),
				new SqlParameter("@CheckDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
				new SqlParameter("@Is_Examined", SqlDbType.Bit, 1),
				new SqlParameter("@ID_ExamDoctor", SqlDbType.Int, 4),
				new SqlParameter("@ExamDoctorName", SqlDbType.VarChar),
				new SqlParameter("@ExamDate", SqlDbType.DateTime)
			};
			array[0].Value = DBNull.Value;
			array[1].Value = DBNull.Value;
			array[2].Value = DBNull.Value;
			array[3].Value = DBNull.Value;
			array[4].Value = DBNull.Value;
			array[5].Value = DBNull.Value;
			array[6].Value = DBNull.Value;
			array[7].Value = DBNull.Value;
			array[8].Value = DBNull.Value;
			array[9].Value = DBNull.Value;
			array[10].Value = DBNull.Value;
			array[11].Value = DBNull.Value;
			array[12].Value = 0;
			array[13].Value = ID_Section;
			array[14].Value = ID_Customer;
			array[15].Value = DBNull.Value;
			array[16].Value = DBNull.Value;
			array[17].Value = DBNull.Value;
			array[18].Value = DBNull.Value;
			stringBuilder.Append(value);
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.DeleteCustomerExamItem_LAB(long ID_Customer, int ID_Section, string ApplyID, int ID_MergerFee, string InterFaceType)
		{
			int result;
			if (InterFaceType != "LAB")
			{
				result = 0;
			}
			else
			{
				string value = string.Format("\r\n                DELETE FROM OnCustExamItem where ID_CustFee in (\r\n\t                SELECT [ID_CustFee]\r\n                    FROM [OnCustFee]\r\n                    where ID_Customer = {0} and ID_Fee in (SELECT ID_Fee FROM BusFee WHERE ID_Section = {1} AND (ID_FeeReportMerger={2} OR ID_Fee = {2} ) ) \r\n                ); ", ID_Customer, ID_Section, ID_MergerFee);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("update OnCustExamSection set ");
				stringBuilder.Append("ID_Typist=@ID_Typist,");
				stringBuilder.Append("TypistName=@TypistName,");
				stringBuilder.Append("TypistDate=@TypistDate,");
				stringBuilder.Append("Is_Check=@Is_Check,");
				stringBuilder.Append("CheckerName=@CheckerName,");
				stringBuilder.Append("CheckDate=@CheckDate,");
				stringBuilder.Append("ID_Checker=@ID_Checker");
				stringBuilder.Append(" WHERE ID_Section=@ID_Section AND ID_Customer=@ID_Customer;  ");
				stringBuilder.Append(string.Format(" DELETE FROM OnCustApply WHERE ID_Apply = '{0}'; ", ApplyID));
				stringBuilder.Append("update OnCustFee set ");
				stringBuilder.Append("Is_Examined=@Is_Examined,");
				stringBuilder.Append("ID_ExamDoctor=@ID_ExamDoctor,");
				stringBuilder.Append("ExamDoctorName=@ExamDoctorName,");
				stringBuilder.Append("FeeImageUrl='',");
				stringBuilder.Append("ExamDate=@ExamDate ");
				stringBuilder.Append(" WHERE  ID_Customer=@ID_Customer AND ID_Fee IN (SELECT ID_Fee FROM BusFee WHERE ID_Section = @ID_Section AND (ID_FeeReportMerger=@ID_MergerFee OR ID_Fee=@ID_MergerFee) ); ");
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
					new SqlParameter("@TypistName", SqlDbType.VarChar),
					new SqlParameter("@TypistDate", SqlDbType.DateTime),
					new SqlParameter("@Is_Check", SqlDbType.Bit, 1),
					new SqlParameter("@CheckerName", SqlDbType.VarChar),
					new SqlParameter("@CheckDate", SqlDbType.DateTime),
					new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
					new SqlParameter("@ID_Section", SqlDbType.Int, 4),
					new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
					new SqlParameter("@Is_Examined", SqlDbType.Bit, 1),
					new SqlParameter("@ID_ExamDoctor", SqlDbType.Int, 4),
					new SqlParameter("@ExamDoctorName", SqlDbType.VarChar),
					new SqlParameter("@ExamDate", SqlDbType.DateTime),
					new SqlParameter("@ID_MergerFee", SqlDbType.BigInt, 8)
				};
				array[0].Value = DBNull.Value;
				array[1].Value = DBNull.Value;
				array[2].Value = DBNull.Value;
				array[3].Value = DBNull.Value;
				array[4].Value = DBNull.Value;
				array[5].Value = DBNull.Value;
				array[6].Value = DBNull.Value;
				array[7].Value = ID_Section;
				array[8].Value = ID_Customer;
				array[9].Value = DBNull.Value;
				array[10].Value = DBNull.Value;
				array[11].Value = DBNull.Value;
				array[12].Value = DBNull.Value;
				array[13].Value = ID_MergerFee;
				stringBuilder.Append(value);
				int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
				result = num;
			}
			return result;
		}

		private int UpdateExamDoctor(SqlCommand cmd, List<PEIS.Model.OnCustFee> OnCustFeeModelList, PEIS.Model.OnCustExamSection OnCustExamSectionModel)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < OnCustFeeModelList.Count; i++)
			{
				if (num2 <= 0 || OnCustFeeModelList[i].ID_CustFee != num2)
				{
					num2 = OnCustFeeModelList[i].ID_CustFee;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("update OnCustExamItem set ");
					stringBuilder.Append("ID_SummaryDoctor=@ID_SummaryDoctor,");
					stringBuilder.Append("SummaryDoctorName=@SummaryDoctorName,");
					stringBuilder.Append("TypistName=@TypistName,");
					stringBuilder.Append("ID_Typist=@ID_Typist,");
					stringBuilder.Append("ExamItemSummaryDate=@ExamItemSummaryDate");
					stringBuilder.Append(" WHERE ID_CustFee=@ID_CustFee");
					SqlParameter[] array = new SqlParameter[]
					{
						new SqlParameter("@ID_SummaryDoctor", SqlDbType.Int, 4),
						new SqlParameter("@SummaryDoctorName", SqlDbType.VarChar),
						new SqlParameter("@TypistName", SqlDbType.VarChar),
						new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
						new SqlParameter("@ExamItemSummaryDate", SqlDbType.DateTime),
						new SqlParameter("@ID_CustFee", SqlDbType.Int, 4)
					};
					array[0].Value = OnCustExamSectionModel.ID_SummaryDoctor;
					array[1].Value = OnCustExamSectionModel.SummaryDoctorName;
					array[2].Value = OnCustExamSectionModel.TypistName;
					array[3].Value = OnCustExamSectionModel.ID_Typist;
					array[4].Value = OnCustExamSectionModel.SectionSummaryDate;
					array[5].Value = OnCustFeeModelList[i].ID_CustFee;
					cmd.Parameters.Clear();
					cmd.CommandText = stringBuilder.ToString();
					for (int j = 0; j < array.Length; j++)
					{
						cmd.Parameters.Add(array[j]);
					}
					num += cmd.ExecuteNonQuery();
				}
			}
			return num;
		}

		private int UpdateSectionSummary(SqlCommand cmd, PEIS.Model.OnCustExamSection OnCustExamSectionModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("SectionSummaryDate=@SectionSummaryDate,");
			stringBuilder.Append("SectionSummary=@SectionSummary,");
			stringBuilder.Append("PositiveSummary=@PositiveSummary,");
			stringBuilder.Append("ID_SummaryDoctor=@ID_SummaryDoctor,");
			stringBuilder.Append("SummaryDoctorName=@SummaryDoctorName,");
			stringBuilder.Append("ID_Typist=@ID_Typist,");
			stringBuilder.Append("TypistName=@TypistName,");
			stringBuilder.Append("TypistDate=@TypistDate,");
			stringBuilder.Append("DiseaseLevel=@DiseaseLevel ");
			stringBuilder.Append(" WHERE ID_CustExamSection=@ID_CustExamSection; ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@SectionSummaryDate", SqlDbType.DateTime),
				new SqlParameter("@SectionSummary", SqlDbType.Text),
				new SqlParameter("@PositiveSummary", SqlDbType.Text),
				new SqlParameter("@ID_SummaryDoctor", SqlDbType.Int, 4),
				new SqlParameter("@SummaryDoctorName", SqlDbType.VarChar),
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@TypistName", SqlDbType.VarChar),
				new SqlParameter("@TypistDate", SqlDbType.DateTime),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@ID_CustExamSection", SqlDbType.Int, 4)
			};
			array[0].Value = OnCustExamSectionModel.SectionSummaryDate;
			array[1].Value = OnCustExamSectionModel.SectionSummary;
			array[2].Value = OnCustExamSectionModel.PositiveSummary;
			array[3].Value = (OnCustExamSectionModel.ID_SummaryDoctor);
			array[4].Value = OnCustExamSectionModel.SummaryDoctorName;
			array[5].Value = OnCustExamSectionModel.ID_Typist;
			array[6].Value = OnCustExamSectionModel.TypistName;
			array[7].Value = OnCustExamSectionModel.TypistDate;
			array[8].Value = OnCustExamSectionModel.DiseaseLevel;
			array[9].Value = OnCustExamSectionModel.ID_CustExamSection;
			stringBuilder.Append(string.Format(" \r\n                IF EXISTS( SELECT Is_ExamStarted FROM (SELECT ISNULL(Is_ExamStarted,0) Is_ExamStarted FROM OnCustPhysicalExamInfo WHERE ID_Customer='{0}') OCPEI WHERE Is_ExamStarted=0 )\r\n                BEGIN\r\n\t                --如果没有开始体检，则修改开始体检的标记\r\n\t                 UPDATE OnCustPhysicalExamInfo SET Is_ExamStarted=1 WHERE ID_Customer='{0}';\r\n                END ", OnCustExamSectionModel.ID_Customer));
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		private int UpdateOnCustFeeItem(SqlCommand cmd, List<PEIS.Model.OnCustFee> OnCustFeeModelList)
		{
			int result;
			if (OnCustFeeModelList == null)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				for (int i = 0; i < OnCustFeeModelList.Count; i++)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("update OnCustFee set ");
					stringBuilder.Append("Is_Examined=@Is_Examined,");
					stringBuilder.Append("ID_ExamDoctor=@ID_ExamDoctor,");
					stringBuilder.Append("ExamDoctorName=@ExamDoctorName,");
					stringBuilder.Append("ExamDate=@ExamDate");
					stringBuilder.Append(" WHERE ID_CustFee=@ID_CustFee");
					SqlParameter[] array = new SqlParameter[]
					{
						new SqlParameter("@Is_Examined", SqlDbType.Bit, 1),
						new SqlParameter("@ID_ExamDoctor", SqlDbType.Int, 4),
						new SqlParameter("@ExamDoctorName", SqlDbType.VarChar),
						new SqlParameter("@ExamDate", SqlDbType.DateTime),
						new SqlParameter("@ID_CustFee", SqlDbType.Int, 4)
					};
					array[0].Value = OnCustFeeModelList[i].Is_Examined;
					array[1].Value = (OnCustFeeModelList[i].ID_ExamDoctor);
					array[2].Value = OnCustFeeModelList[i].ExamDoctorName;
					array[3].Value = OnCustFeeModelList[i].ExamDate;
					array[4].Value = OnCustFeeModelList[i].ID_CustFee;

                   

					cmd.Parameters.Clear();
					cmd.CommandText = stringBuilder.ToString();
					for (int j = 0; j < array.Length; j++)
					{
						cmd.Parameters.Add(array[j]);
					}
					num += cmd.ExecuteNonQuery();
				}
				result = num;
			}
			return result;
		}

		private int WriteCustApplyItem(SqlCommand cmd, PEIS.Model.OnCustApply OnCustApplyModel)
		{
			int result;
			if (OnCustApplyModel == null)
			{
				result = 0;
			}
			else
			{
				int num = this.UpdateCustApplyItem(cmd, OnCustApplyModel);
				if (num <= 0)
				{
					num = this.InsertCustApplyItem(cmd, OnCustApplyModel);
				}
				result = num;
			}
			return result;
		}

		public int InsertCustApplyItem(SqlCommand cmd, PEIS.Model.OnCustApply model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" IF NOT EXISTS(SELECT * FROM OnCustApply WHERE ID_Apply='{0}') BEGIN ", model.ID_Apply.ToString());
			stringBuilder.Append("insert into OnCustApply(");
			stringBuilder.Append("ID_Apply,ID_Section,ID_Customer,ApplyTitle,SpecimenName,BatchNumber,SectionName,DeptName,ExamNumber,AcquisitionTime,RecvTime,ReportTime,ApplyDoctorName,DetectionDoctorName,CheckDoctorName,CreateTime,ExamItems,SendProjectIDs,SendGroupParams,SpecimenNo,Is_TypistFinish,ID_Typist,TypistName,TypistDate,ID_DetectionDoctor)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Apply,@ID_Section,@ID_Customer,@ApplyTitle,@SpecimenName,@BatchNumber,@SectionName,@DeptName,@ExamNumber,@AcquisitionTime,@RecvTime,@ReportTime,@ApplyDoctorName,@DetectionDoctorName,@CheckDoctorName,@CreateTime,@ExamItems,@SendProjectIDs,@SendGroupParams,@SpecimenNo,@Is_TypistFinish,@ID_Typist,@TypistName,@TypistDate,@ID_DetectionDoctor)");
			stringBuilder.Append("; END ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Apply", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
				new SqlParameter("@ApplyTitle", SqlDbType.VarChar, 100),
				new SqlParameter("@SpecimenName", SqlDbType.VarChar, 50),
				new SqlParameter("@BatchNumber", SqlDbType.VarChar, 20),
				new SqlParameter("@SectionName", SqlDbType.VarChar, 50),
				new SqlParameter("@DeptName", SqlDbType.VarChar, 50),
				new SqlParameter("@ExamNumber", SqlDbType.VarChar, 50),
				new SqlParameter("@AcquisitionTime", SqlDbType.DateTime),
				new SqlParameter("@RecvTime", SqlDbType.DateTime),
				new SqlParameter("@ReportTime", SqlDbType.DateTime),
				new SqlParameter("@ApplyDoctorName", SqlDbType.VarChar, 20),
				new SqlParameter("@DetectionDoctorName", SqlDbType.VarChar, 20),
				new SqlParameter("@CheckDoctorName", SqlDbType.VarChar, 20),
				new SqlParameter("@CreateTime", SqlDbType.DateTime),
				new SqlParameter("@ExamItems", SqlDbType.VarChar, 200),
				new SqlParameter("@SendProjectIDs", SqlDbType.VarChar, 100),
				new SqlParameter("@SendGroupParams", SqlDbType.VarChar, 100),
				new SqlParameter("@SpecimenNo", SqlDbType.VarChar, 30),
				new SqlParameter("@Is_TypistFinish", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@TypistName", SqlDbType.VarChar, 30),
				new SqlParameter("@TypistDate", SqlDbType.DateTime),
				new SqlParameter("@ID_DetectionDoctor", SqlDbType.Int, 4)
			};
			array[0].Value = model.ID_Apply;
			array[1].Value = model.ID_Section;
			array[2].Value = model.ID_Customer;
			array[3].Value = (model.ApplyTitle);
			array[4].Value = (model.SpecimenName);
			array[5].Value = (model.BatchNumber);
			array[6].Value = (model.SectionName);
			array[7].Value = (model.DeptName );
			array[8].Value = (model.ExamNumber);
			array[9].Value = (model.AcquisitionTime);
			array[10].Value = (model.RecvTime);
			array[11].Value = (model.ReportTime);
			array[12].Value = (model.ApplyDoctorName);
			array[13].Value = (model.DetectionDoctorName);
			array[14].Value = (model.CheckDoctorName);
			array[15].Value = (model.CreateTime);
			array[16].Value = (model.ExamItems);
			array[17].Value = (model.SendProjectIDs);
			array[18].Value = (model.SendGroupParams);
			array[19].Value = (model.SpecimenNo);
			array[20].Value = (model.Is_TypistFinish);
			array[21].Value = (model.ID_Typist);
			array[22].Value = (model.TypistName);
			array[23].Value = (model.TypistDate);
			array[24].Value = (model.ID_DetectionDoctor);
			cmd.Parameters.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			cmd.CommandText = stringBuilder.ToString();
			return cmd.ExecuteNonQuery();
		}

		public int UpdateCustApplyItem(SqlCommand cmd, PEIS.Model.OnCustApply model)
		{
			int result;
			if (model == null)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("update OnCustApply set ");
				stringBuilder.Append("ID_Section=@ID_Section,");
				stringBuilder.Append("ID_Customer=@ID_Customer,");
				stringBuilder.Append("Is_TypistFinish=@Is_TypistFinish,");
				stringBuilder.Append("ID_Typist=@ID_Typist,");
				stringBuilder.Append("TypistName=@TypistName,");
				stringBuilder.Append("TypistDate=@TypistDate,");
				stringBuilder.Append("DetectionDoctorName=@DetectionDoctorName,");
				stringBuilder.Append("ID_DetectionDoctor=@ID_DetectionDoctor");
				stringBuilder.Append(" where ID_Apply=@ID_Apply ");
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID_Section", SqlDbType.Int, 4),
					new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
					new SqlParameter("@Is_TypistFinish", SqlDbType.Bit, 1),
					new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
					new SqlParameter("@TypistName", SqlDbType.VarChar, 30),
					new SqlParameter("@TypistDate", SqlDbType.DateTime),
					new SqlParameter("@DetectionDoctorName", SqlDbType.VarChar, 20),
					new SqlParameter("@ID_DetectionDoctor", SqlDbType.Int, 4),
					new SqlParameter("@ID_Apply", SqlDbType.VarChar, 50)
				};
				array[0].Value = model.ID_Section;
				array[1].Value = model.ID_Customer;
				array[2].Value = model.Is_TypistFinish;
				array[3].Value = model.ID_Typist;
				array[4].Value = model.TypistName;
				array[5].Value = model.TypistDate;
				array[6].Value = model.DetectionDoctorName;
				array[7].Value = (model.ID_DetectionDoctor);
				array[8].Value = model.ID_Apply;
				cmd.Parameters.Clear();
				cmd.CommandText = stringBuilder.ToString();
				for (int i = 0; i < array.Length; i++)
				{
					cmd.Parameters.Add(array[i]);
				}
				result = cmd.ExecuteNonQuery();
			}
			return result;
		}

		private int WriteCustExamItemResult(SqlCommand cmd, List<PEIS.Model.OnCustExamItem> OnCustExamItemModelList)
		{
			int result;
			if (OnCustExamItemModelList == null)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				if (OnCustExamItemModelList != null && OnCustExamItemModelList.Count > 0)
				{
					int i = 0;
					while (i < OnCustExamItemModelList.Count)
					{
						int num2 = OnCustExamItemModelList[i].ID_CustExamItem;
						if (num2 > 0)
						{
							num += this.UpdateCustExamItem(cmd, OnCustExamItemModelList[i]);
							goto IL_9F;
						}
						object obj = this.InsertCustExamItem(cmd, OnCustExamItemModelList[i], 0);
						if (obj != null)
						{
							num++;
							num2 = Convert.ToInt32(obj);
							goto IL_9F;
						}
						IL_14B:
						i++;
						continue;
						IL_9F:
						if (OnCustExamItemModelList[i].ExamItemResultList != null && OnCustExamItemModelList[i].ExamItemResultList.Count > 0)
						{
							for (int j = 0; j < OnCustExamItemModelList[i].ExamItemResultList.Count; j++)
							{
								OnCustExamItemModelList[i].ExamItemResultList[j].ID_CustExamItem = new int?(num2);
								int num3 = this.InsertCustExamResultItem(cmd, OnCustExamItemModelList[i].ExamItemResultList[j]);
								if (num3 <= 0)
								{
									result = num3;
									return result;
								}
								num++;
							}
						}
						goto IL_14B;
					}
				}
				result = num;
			}
			return result;
		}

		protected object InsertCustExamItem(SqlCommand cmd, PEIS.Model.OnCustExamItem CustExamModel, int OperatorType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(CustExamModel.ID_CustApply))
			{
				stringBuilder.AppendFormat(" IF NOT EXISTS(SELECT * FROM OnCustExamItem WHERE ID_CustFee={0} AND ID_ExamItem = {1} AND ID_CustApply = '{2}' ) BEGIN ", CustExamModel.ID_CustFee.ToString(), CustExamModel.ID_ExamItem.ToString(), CustExamModel.ID_CustApply.ToString());
			}
			else
			{
				stringBuilder.AppendFormat(" IF NOT EXISTS(SELECT * FROM OnCustExamItem WHERE ID_CustFee={0} AND ID_ExamItem = {1}) BEGIN ", CustExamModel.ID_CustFee.ToString(), CustExamModel.ID_ExamItem.ToString());
			}
			stringBuilder.Append("insert into OnCustExamItem(");
			stringBuilder.Append("ID_CustFee,ID_Fee,ID_ExamItem,ExamItemName,DiseaseLevel,ResultLabValues,ResultNumber,ResultLabLowLimit,ResultLabHighLimit,ResultLabUnit,ResultLabMark,ResultSummary,ID_SummaryDoctor,SummaryDoctorName,TypistName,ExamItemSummaryDate,ID_Typist,ResultLabRange,ID_Customer,AbbrExamName,DetectionMethod,SCO,ID_CustApply)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_CustFee,@ID_Fee,@ID_ExamItem,@ExamItemName,@DiseaseLevel,@ResultLabValues,@ResultNumber,@ResultLabLowLimit,@ResultLabHighLimit,@ResultLabUnit,@ResultLabMark,@ResultSummary,@ID_SummaryDoctor,@SummaryDoctorName,@TypistName,@ExamItemSummaryDate,@ID_Typist,@ResultLabRange,@ID_Customer,@AbbrExamName,@DetectionMethod,@SCO,@ID_CustApply)");
			stringBuilder.Append(";select @@IDENTITY; END ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_CustFee", SqlDbType.Int, 4),
				new SqlParameter("@ID_Fee", SqlDbType.Int, 4),
				new SqlParameter("@ID_ExamItem", SqlDbType.Int, 4),
				new SqlParameter("@ExamItemName", SqlDbType.VarChar, 50),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@ResultLabValues", SqlDbType.VarChar, 240),
				new SqlParameter("@ResultNumber", SqlDbType.Float, 8),
				new SqlParameter("@ResultLabLowLimit", SqlDbType.Float, 8),
				new SqlParameter("@ResultLabHighLimit", SqlDbType.Float, 8),
				new SqlParameter("@ResultLabUnit", SqlDbType.VarChar, 10),
				new SqlParameter("@ResultLabMark", SqlDbType.VarChar, 60),
				new SqlParameter("@ResultSummary", SqlDbType.Text),
				new SqlParameter("@ID_SummaryDoctor", SqlDbType.Int, 4),
				new SqlParameter("@SummaryDoctorName", SqlDbType.VarChar, 30),
				new SqlParameter("@TypistName", SqlDbType.VarChar, 30),
				new SqlParameter("@ExamItemSummaryDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@ResultLabRange", SqlDbType.VarChar, 100),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
				new SqlParameter("@AbbrExamName", SqlDbType.VarChar, 60),
				new SqlParameter("@DetectionMethod", SqlDbType.VarChar, 60),
				new SqlParameter("@SCO", SqlDbType.VarChar, 20),
				new SqlParameter("@ID_CustApply", SqlDbType.VarChar, 60)
			};
			array[0].Value = CustExamModel.ID_CustFee;
			array[1].Value = CustExamModel.ID_Fee;
			array[2].Value = CustExamModel.ID_ExamItem;
			array[3].Value = CustExamModel.ExamItemName;
			array[4].Value = CustExamModel.DiseaseLevel;
			array[5].Value = CustExamModel.ResultLabValues;
			array[6].Value = (CustExamModel.ResultNumber);
			array[7].Value = (CustExamModel.ResultLabLowLimit);
			array[8].Value = (CustExamModel.ResultLabHighLimit);
			array[9].Value = CustExamModel.ResultLabUnit;
			array[10].Value = CustExamModel.ResultLabMark;
			array[11].Value = CustExamModel.ResultSummary;
			array[12].Value = (CustExamModel.ID_SummaryDoctor);
			array[13].Value = CustExamModel.SummaryDoctorName;
			array[14].Value = CustExamModel.TypistName;
			array[15].Value = CustExamModel.ExamItemSummaryDate;
			array[16].Value = CustExamModel.ID_Typist;
			array[17].Value = CustExamModel.ResultLabRange;
			array[18].Value = CustExamModel.ID_Customer;
			array[19].Value = CustExamModel.AbbrExamName;
			array[20].Value = CustExamModel.DetectionMethod;
			array[21].Value = CustExamModel.SCO;
			array[22].Value = (CustExamModel.ID_CustApply);
			cmd.Parameters.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			cmd.CommandText = stringBuilder.ToString();
			return cmd.ExecuteScalar();
		}

		public int UpdateCustExamItem(SqlCommand cmd, PEIS.Model.OnCustExamItem CustExamModel)
		{
			int result;
			if (CustExamModel.ID_CustExamItem <= 0)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("update OnCustExamItem set ");
				stringBuilder.Append("ID_CustFee=@ID_CustFee,");
				stringBuilder.Append("ID_Fee=@ID_Fee,");
				stringBuilder.Append("ID_ExamItem=@ID_ExamItem,");
				stringBuilder.Append("ExamItemName=@ExamItemName,");
				stringBuilder.Append("DiseaseLevel=@DiseaseLevel,");
				stringBuilder.Append("ResultLabValues=@ResultLabValues,");
				stringBuilder.Append("ResultNumber=@ResultNumber,");
				stringBuilder.Append("ResultLabLowLimit=@ResultLabLowLimit,");
				stringBuilder.Append("ResultLabHighLimit=@ResultLabHighLimit,");
				stringBuilder.Append("ResultLabUnit=@ResultLabUnit,");
				stringBuilder.Append("ResultLabMark=@ResultLabMark,");
				stringBuilder.Append("ResultSummary=@ResultSummary,");
				stringBuilder.Append("ID_SummaryDoctor=@ID_SummaryDoctor,");
				stringBuilder.Append("SummaryDoctorName=@SummaryDoctorName,");
				stringBuilder.Append("TypistName=@TypistName,");
				stringBuilder.Append("ExamItemSummaryDate=@ExamItemSummaryDate,");
				stringBuilder.Append("ID_Typist=@ID_Typist,");
				stringBuilder.Append("ResultLabRange=@ResultLabRange,");
				stringBuilder.Append("ID_Customer=@ID_Customer,");
				stringBuilder.Append("AbbrExamName=@AbbrExamName,");
				stringBuilder.Append("DetectionMethod=@DetectionMethod,");
				stringBuilder.Append("SCO=@SCO,");
				stringBuilder.Append("ID_CustApply=@ID_CustApply");
				stringBuilder.Append(" where ID_CustExamItem=@ID_CustExamItem");
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID_CustFee", SqlDbType.Int, 4),
					new SqlParameter("@ID_Fee", SqlDbType.Int, 4),
					new SqlParameter("@ID_ExamItem", SqlDbType.Int, 4),
					new SqlParameter("@ExamItemName", SqlDbType.VarChar, 50),
					new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
					new SqlParameter("@ResultLabValues", SqlDbType.VarChar, 240),
					new SqlParameter("@ResultNumber", SqlDbType.Float, 8),
					new SqlParameter("@ResultLabLowLimit", SqlDbType.Float, 8),
					new SqlParameter("@ResultLabHighLimit", SqlDbType.Float, 8),
					new SqlParameter("@ResultLabUnit", SqlDbType.VarChar, 10),
					new SqlParameter("@ResultLabMark", SqlDbType.VarChar, 60),
					new SqlParameter("@ResultSummary", SqlDbType.Text),
					new SqlParameter("@ID_SummaryDoctor", SqlDbType.Int, 4),
					new SqlParameter("@SummaryDoctorName", SqlDbType.VarChar, 30),
					new SqlParameter("@TypistName", SqlDbType.VarChar, 30),
					new SqlParameter("@ExamItemSummaryDate", SqlDbType.DateTime),
					new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
					new SqlParameter("@ResultLabRange", SqlDbType.VarChar, 100),
					new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
					new SqlParameter("@AbbrExamName", SqlDbType.VarChar, 60),
					new SqlParameter("@DetectionMethod", SqlDbType.VarChar, 60),
					new SqlParameter("@SCO", SqlDbType.VarChar, 20),
					new SqlParameter("@ID_CustApply", SqlDbType.VarChar, 60),
					new SqlParameter("@ID_CustExamItem", SqlDbType.Int, 4)
				};
				array[0].Value = CustExamModel.ID_CustFee;
				array[1].Value = CustExamModel.ID_Fee;
				array[2].Value = CustExamModel.ID_ExamItem;
				array[3].Value = CustExamModel.ExamItemName;
				array[4].Value = CustExamModel.DiseaseLevel;
				array[5].Value = CustExamModel.ResultLabValues;
				array[6].Value = (CustExamModel.ResultNumber);
				array[7].Value = (CustExamModel.ResultLabLowLimit);
				array[8].Value = (CustExamModel.ResultLabHighLimit);
				array[9].Value = CustExamModel.ResultLabUnit;
				array[10].Value = CustExamModel.ResultLabMark;
				array[11].Value = CustExamModel.ResultSummary;
				array[12].Value = (CustExamModel.ID_SummaryDoctor);
				array[13].Value = CustExamModel.SummaryDoctorName;
				array[14].Value = CustExamModel.TypistName;
				array[15].Value = CustExamModel.ExamItemSummaryDate;
				array[16].Value = CustExamModel.ID_Typist;
				array[17].Value = CustExamModel.ResultLabRange;
				array[18].Value = CustExamModel.ID_Customer;
				array[19].Value = CustExamModel.AbbrExamName;
				array[20].Value = CustExamModel.DetectionMethod;
				array[21].Value = CustExamModel.SCO;
				array[22].Value = (CustExamModel.ID_CustApply);
				array[23].Value = CustExamModel.ID_CustExamItem;
				cmd.Parameters.Clear();
				cmd.CommandText = stringBuilder.ToString();
				for (int i = 0; i < array.Length; i++)
				{
					cmd.Parameters.Add(array[i]);
				}
				result = cmd.ExecuteNonQuery();
			}
			return result;
		}

		protected int InsertCustExamResultItem(SqlCommand cmd, PEIS.Model.OnCustExamItemResult CustExamResultModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into OnCustExamItemResult(");
			stringBuilder.Append("ID_CustExamItem,ID_Symptom)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_CustExamItem,@ID_Symptom);");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_CustExamItem", SqlDbType.Int, 4),
				new SqlParameter("@ID_Symptom", SqlDbType.Int, 4)
			};
			array[0].Value = CustExamResultModel.ID_CustExamItem;
			array[1].Value = CustExamResultModel.ID_Symptom;
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		private int DeleteCustExamItemResult(SqlCommand cmd, List<PEIS.Model.OnCustExamItem> OnCustExamItemModelList)
		{
			int result = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			if (OnCustExamItemModelList != null && OnCustExamItemModelList.Count > 0)
			{
				for (int i = 0; i < OnCustExamItemModelList.Count; i++)
				{
					if (OnCustExamItemModelList[i].ID_CustExamItem > 0)
					{
						if (string.IsNullOrEmpty(text))
						{
							text = OnCustExamItemModelList[i].ID_CustExamItem.ToString();
						}
						else
						{
							text = text + "," + OnCustExamItemModelList[i].ID_CustExamItem.ToString();
						}
						if (string.IsNullOrEmpty(OnCustExamItemModelList[i].ResultSummary))
						{
							if (string.IsNullOrEmpty(text2))
							{
								text2 = OnCustExamItemModelList[i].ID_CustExamItem.ToString();
							}
							else
							{
								text2 = text2 + "," + OnCustExamItemModelList[i].ID_CustExamItem.ToString();
							}
						}
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("delete FROM OnCustExamItemResult ");
					stringBuilder.Append(" WHERE ID_CustExamItem in (" + text + ");  ");
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append("delete FROM OnCustExamItem ");
						stringBuilder.Append(" WHERE ID_CustExamItem in (" + text2 + ");  ");
					}
					cmd.Parameters.Clear();
					cmd.CommandText = stringBuilder.ToString();
					result = cmd.ExecuteNonQuery();
				}
			}
			return result;
		}

		private int ClearCustExamItemResult(SqlCommand cmd, List<PEIS.Model.OnCustFee> OnCustFeeModelList)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string text = "";
			for (int i = 0; i < OnCustFeeModelList.Count; i++)
			{
				if (i == 0)
				{
					text = OnCustFeeModelList[i].ID_CustFee.ToString();
				}
				else
				{
					text = text + "," + OnCustFeeModelList[i].ID_Fee;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" DELETE FROM OnCustExamItemResult ");
			stringBuilder.Append(" WHERE ID_CustExamItem in ( SELECT ID_CustExamItem FROM [OnCustExamItem] WHERE ID_CustFee IN (" + text + ") );  ");
			stringBuilder.Append(" DELETE FROM OnCustExamItem ");
			stringBuilder.Append(" WHERE ID_CustExamItem in ( SELECT ID_CustExamItem FROM [OnCustExamItem] WHERE ID_CustFee IN (" + text + ") );  ");
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			return cmd.ExecuteNonQuery();
		}

		int ICommonCustExam.BandingCustomerSectionExamInfo(PEIS.Model.OnCustExamSection OnCustExamSectionModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("SectionSummaryDate=@SectionSummaryDate,");
			stringBuilder.Append("ID_SummaryDoctor=@ID_SummaryDoctor,");
			stringBuilder.Append("SummaryDoctorName=@SummaryDoctorName,");
			stringBuilder.Append("ID_Typist=@ID_Typist,");
			stringBuilder.Append("TypistName=@TypistName,");
			stringBuilder.Append("TypistDate=@TypistDate");
			stringBuilder.Append(" WHERE ID_CustExamSection=@ID_CustExamSection AND ID_Customer=@ID_Customer AND (ID_SummaryDoctor is null or ID_SummaryDoctor <> @ID_SummaryDoctor); ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@SectionSummaryDate", SqlDbType.DateTime),
				new SqlParameter("@ID_SummaryDoctor", SqlDbType.Int, 4),
				new SqlParameter("@SummaryDoctorName", SqlDbType.VarChar),
				new SqlParameter("@ID_Typist", SqlDbType.Int, 4),
				new SqlParameter("@TypistName", SqlDbType.VarChar),
				new SqlParameter("@TypistDate", SqlDbType.DateTime),
				new SqlParameter("@ID_CustExamSection", SqlDbType.Int, 4),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OnCustExamSectionModel.SectionSummaryDate;
			array[1].Value = OnCustExamSectionModel.ID_SummaryDoctor;
			array[2].Value = OnCustExamSectionModel.SummaryDoctorName;
			array[3].Value = OnCustExamSectionModel.ID_Typist;
			array[4].Value = OnCustExamSectionModel.TypistName;
			array[5].Value = OnCustExamSectionModel.TypistDate;
			array[6].Value = OnCustExamSectionModel.ID_CustExamSection;
			array[7].Value = OnCustExamSectionModel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateGuideSheetReturnState(PEIS.Model.OnCustPhysicalExamInfo OCPEIModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("Is_GuideSheetReturned=@Is_GuideSheetReturned,");
			stringBuilder.Append("GuideSheetReturnedDate=@GuideSheetReturnedDate,");
			stringBuilder.Append("GuideSheetReturnedby=@GuideSheetReturnedby,");
			stringBuilder.Append("ID_UserGuideSheetReturnedBy=@ID_UserGuideSheetReturnedBy");
			stringBuilder.Append(" where ID_Customer=@ID_Customer ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_GuideSheetReturned", SqlDbType.Bit, 1),
				new SqlParameter("@GuideSheetReturnedDate", SqlDbType.DateTime),
				new SqlParameter("@GuideSheetReturnedby", SqlDbType.VarChar),
				new SqlParameter("@ID_UserGuideSheetReturnedBy", SqlDbType.Int, 4),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCPEIModel.Is_GuideSheetReturned;
			array[1].Value = OCPEIModel.GuideSheetReturnedDate;
			array[2].Value = OCPEIModel.GuideSheetReturnedby;
			array[3].Value = OCPEIModel.ID_UserGuideSheetReturnedBy;
			array[4].Value = OCPEIModel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateDiseaselLevelInform(PEIS.Model.OnCustPhysicalExamInfo OCPEIModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("Is_DiseaseLevelInformed=@Is_DiseaseLevelInformed,");
			stringBuilder.Append("ID_DiseaseLevelInformer=@ID_DiseaseLevelInformer,");
			stringBuilder.Append("DiseaseLevelInformer=@DiseaseLevelInformer,");
			stringBuilder.Append("DiseaseLevelInformedDate=@DiseaseLevelInformedDate,");
			stringBuilder.Append("DiseaseLevelInformNote=@DiseaseLevelInformNote");
			stringBuilder.Append(" where ID_Customer=@ID_Customer ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_DiseaseLevelInformed", SqlDbType.Bit, 1),
				new SqlParameter("@ID_DiseaseLevelInformer", SqlDbType.Int, 4),
				new SqlParameter("@DiseaseLevelInformer", SqlDbType.VarChar, 30),
				new SqlParameter("@DiseaseLevelInformedDate", SqlDbType.DateTime),
				new SqlParameter("@DiseaseLevelInformNote", SqlDbType.VarChar, 500),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCPEIModel.Is_DiseaseLevelInformed;
			array[1].Value = OCPEIModel.ID_DiseaseLevelInformer;
			array[2].Value = OCPEIModel.DiseaseLevelInformer;
			array[3].Value = OCPEIModel.DiseaseLevelInformedDate;
			array[4].Value = OCPEIModel.DiseaseLevelInformNote;
			array[5].Value = OCPEIModel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateExamSectionGiveUpState(PEIS.Model.OnCustExamSection OCESmodel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("IS_giveup=@IS_giveup");
			stringBuilder.Append(" where ID_CustExamSection=@ID_CustExamSection ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@IS_giveup", SqlDbType.Bit, 1),
				new SqlParameter("@ID_CustExamSection", SqlDbType.Int, 4)
			};
			array[0].Value = OCESmodel.IS_giveup;
			array[1].Value = OCESmodel.ID_CustExamSection;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonCustExam.UpdateAllNotExamedSectionGiveUp(PEIS.Model.OnCustExamSection OCESmodel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustExamSection set ");
			stringBuilder.Append("IS_giveup=@IS_giveup");
			if (OCESmodel.IS_giveup == true)
			{
				stringBuilder.Append(" where ID_Section in ( SELECT [ID_Section] FROM [SYSSection] WHERE ISNULL(InterfaceType,'') = '' ) AND  ID_Customer=@ID_Customer and ( SummaryDoctorName is null or SummaryDoctorName = '' ) ");
			}
			else
			{
				stringBuilder.Append(" where ID_Section in ( SELECT [ID_Section] FROM [SYSSection] WHERE ISNULL(InterfaceType,'') = '' ) AND ID_Customer=@ID_Customer and ISNULL(SummaryDoctorName,'') <> '' ");
			}
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@IS_giveup", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCESmodel.IS_giveup;
			array[1].Value = OCESmodel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}
	}
}
