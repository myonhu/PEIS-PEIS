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
	public class CommonConclusion : CommonBase, ICommonConclusion
	{
		protected string[] QueryPagesFinalExamListParam = new string[]
		{
			"ID_Customer",
			"*",
			" (  SELECT TOP 99999999 [ID_Customer]\r\n                                                                                      ,[CustomerName]\r\n                                                                                      ,[ExamTypeName]\r\n                                                                                      ,[Is_FinalFinished]\r\n                                                                                      ,[ID_FinalDoctor]\r\n                                                                                      ,[FinalDoctor]\r\n                                                                                      ,[FinalDate]\r\n                                                                                      ,[Is_Checked]\r\n                                                                                      ,[ID_Checker]\r\n                                                                                      ,[Checker]\r\n                                                                                      ,[CheckedDate]\r\n                                                                                      ,[Is_ReportReceipted]\r\n                                                                                      ,convert(varchar(50),CheckedDate,20) CheckedFormatDate\r\n                                                                                      ,convert(varchar(50),FinalDate,20) FinalFormatDate\r\n                                                                                  FROM OnCustPhysicalExamInfo OCPEI ORDER BY OCPEI.ID_Customer ASC ) ",
			" ORDER BY ID_Customer ASC "
		};

		protected string[] QueryPagesFinalExamList_NoFinalFinished_Param = new string[]
		{
			"ID_Customer",
			"*",
			" (  SELECT TOP 99999999 [ID_Customer]\r\n                                                                                      ,[CustomerName]\r\n                                                                                      ,[ExamTypeName]\r\n                                                                                      ,[Is_FinalFinished]\r\n                                                                                      ,[ID_FinalDoctor]\r\n                                                                                      ,[FinalDoctor]\r\n                                                                                      ,[FinalDate]\r\n                                                                                      ,[Is_Checked]\r\n                                                                                      ,[ID_Checker]\r\n                                                                                      ,[Checker]\r\n                                                                                      ,[CheckedDate]\r\n                                                                                      ,[Is_ReportReceipted]\r\n                                                                                      ,convert(varchar(50),CheckedDate,20) CheckedFormatDate\r\n                                                                                      ,convert(varchar(50),FinalDate,20) FinalFormatDate\r\n                                                                                  FROM OnCustPhysicalExamInfo OCPEI \r\n                                                                                 WHERE ISNULL(Is_FinalFinished,0) = 0 AND ([SubScribDate] between @BeginDate and @EndDate)  AND ISNULL(Is_GuideSheetReturned,0) = 1  ORDER BY OCPEI.ID_Customer ASC ) ",
			" ORDER BY ID_Customer ASC "
		};

		protected string[] QueryPagesFinalCheckListParam = new string[]
		{
			"ID_Customer",
			"*",
			" (  SELECT TOP 99999999 [ID_Customer]\r\n                                                                                      ,[CustomerName]\r\n                                                                                      ,[ExamTypeName]\r\n                                                                                      ,[Is_FinalFinished]\r\n                                                                                      ,[ID_FinalDoctor]\r\n                                                                                      ,[FinalDoctor]\r\n                                                                                      ,[FinalDate]\r\n                                                                                      ,[Is_Checked]\r\n                                                                                      ,[ID_Checker]\r\n                                                                                      ,[Checker]\r\n                                                                                      ,[CheckedDate]\r\n                                                                                      ,[Is_ReportReceipted]\r\n                                                                                      ,convert(varchar(50),CheckedDate,20) CheckedFormatDate\r\n                                                                                      ,convert(varchar(50),FinalDate,20) FinalFormatDate\r\n                                                                                  FROM OnCustPhysicalExamInfo OCPEI ORDER BY OCPEI.ID_Customer ASC  ) ",
			" ORDER BY ID_Customer ASC "
		};

		protected string[] QueryPagesConclusionList_Param = new string[]
		{
			"ID_Conclusion",
			"*",
            " ( SELECT [ID_Conclusion]\r\n                ,BCT.[ID_ConclusionType]\r\n                ,BCT.ConclusionTypeName\r\n                ,[ConclusionName]\r\n                ,[TeamConclusionName]\r\n                ,replace(convert(nvarchar(4000),Explanation),'\u3000','') Explanation\r\n                ,[Suggestion]\r\n                ,[DietGuide]\r\n                ,[SportsGuide]\r\n                ,[HealthKnowledge]\r\n                ,[ForGender]\r\n                ,BC.[InputCode]\r\n                ,[DispOrder]\r\n                ,BC.[Is_Banned]\r\n                ,BC.[ID_BanOpr]\r\n                ,BC.[BanOperator]\r\n                ,BC.[BanDate]\r\n                ,BC.[BanDescribe]\r\n                ,BC.[ID_ICD]\r\n            FROM  [BusConclusion] BC LEFT JOIN [BusConclusionType] BCT \r\n            ON BC.ID_ConclusionType = BCT.ID_ConclusionType \r\n            ) ",
			" "
		};

		protected string[] QueryPagesConclusionListByType_Param = new string[]
		{
			"ID_Conclusion",
			"*",
            " ( SELECT [ID_Conclusion] ,BCT.[ID_ConclusionType]  ,BCT.ConclusionTypeName  ,[ConclusionName]  ,[TeamConclusionName] ,replace(convert(nvarchar(4000),Explanation),'\u3000','') Explanation ,[Suggestion]  ,[DietGuide]  ,[SportsGuide] ,[HealthKnowledge] ,[ForGender]  ,BC.[InputCode]  ,[DispOrder]\r\n            ,BC.[Is_Banned]\r\n            ,BC.[ID_BanOpr]\r\n            ,BC.[BanOperator]\r\n            ,BC.[BanDate]\r\n            ,BC.[BanDescribe]\r\n            ,BC.[ID_ICD]\r\n            FROM  [BusConclusion] BC  LEFT JOIN [BusConclusionType] BCT \r\n            ON BC.ID_ConclusionType = BCT.ID_ConclusionType\r\n            WHERE BC.ID_ConclusionType = @ID_ConclusionType \r\n            ) ",
			" "
		};

		protected string[] QueryPagesConclusionListByName_Param = new string[]
		{
			"ID_Conclusion",
			"*",
            " ( SELECT [ID_Conclusion]\r\n                ,BCT.[ID_ConclusionType]\r\n                ,BCT.ConclusionTypeName\r\n                ,[ConclusionName]\r\n                ,[TeamConclusionName]\r\n                ,replace(convert(nvarchar(4000),Explanation),'\u3000','') Explanation\r\n                ,[Suggestion]\r\n                ,[DietGuide]\r\n                ,[SportsGuide]\r\n                ,[HealthKnowledge]\r\n                ,[ForGender]\r\n                ,BC.[InputCode]\r\n                ,[DispOrder],BC.[Is_Banned]\r\n                ,BC.[ID_BanOpr]\r\n                ,BC.[BanOperator]\r\n                ,BC.[BanDate]\r\n                ,BC.[BanDescribe]\r\n                ,BC.[ID_ICD]\r\n            FROM [BusConclusion] BC  LEFT JOIN [BusConclusionType] BCT \r\n              ON BC.ID_ConclusionType = BCT.ID_ConclusionType \r\n           WHERE (BC.ConclusionName LIKE @ConclusionName OR BC.[InputCode] LIKE @ConclusionName)\r\n            ) ",
			"  "
		};

		protected string[] QueryPagesConclusionListByTypeAndName_Param = new string[]
		{
			"ID_Conclusion",
			"*",
            " ( SELECT [ID_Conclusion]\r\n                ,BCT.[ID_ConclusionType]\r\n                ,BCT.ConclusionTypeName\r\n                ,[ConclusionName]\r\n                ,[TeamConclusionName]\r\n                ,replace(convert(nvarchar(4000),Explanation),'\u3000','') Explanation\r\n                ,[Suggestion]\r\n                ,[DietGuide]\r\n                ,[SportsGuide]\r\n                ,[HealthKnowledge]\r\n                ,[ForGender]\r\n                ,BC.[InputCode]\r\n                ,[DispOrder],BC.[Is_Banned]\r\n                ,BC.[ID_BanOpr]\r\n                ,BC.[BanOperator]\r\n                ,BC.[BanDate]\r\n                ,BC.[BanDescribe]\r\n                ,BC.[ID_ICD]\r\n            FROM  [BusConclusion] BC  LEFT JOIN  [BusConclusionType] BCT \r\n               ON BC.ID_ConclusionType = BCT.ID_ConclusionType \r\n            WHERE (BC.ConclusionName LIKE @ConclusionName OR BC.[InputCode] LIKE @ConclusionName)\r\n              AND BC.ID_ConclusionType = @ID_ConclusionType \r\n            ) ",
			"  "
		};

		protected string[] QueryCustomerDiseaseLevel_Param = new string[]
		{
			" SELECT max(ISNULL([DiseaseLevel],0)) MaxDiseaseLevel\r\n               FROM [OnCustExamSection]\r\n              WHERE ID_Customer = @ID_Customer;\r\n          "
		};

		protected string[] QueryDoctorFinalNotCheckedList_Param = new string[]
		{
			" SELECT [ID_Customer]\r\n              ,[CustomerName]\r\n              ,[ExamTypeName]\r\n              ,[Is_FinalFinished]\r\n              ,[ID_FinalDoctor]\r\n              ,[FinalDoctor]\r\n              ,[FinalDate]\r\n              ,[Is_Checked]\r\n              ,[ID_Checker]\r\n              ,[Checker]\r\n              ,[CheckedDate]\r\n              ,[Is_ReportReceipted]\r\n              ,convert(varchar(20),FinalDate,23) FinalFormatDate\r\n           FROM OnCustPhysicalExamInfo OCPEI \r\n          WHERE Is_Checked = 0 \r\n            AND Is_FinalFinished = 0 \r\n            AND ID_FinalDoctor = @ID_FinalDoctor;\r\n          "
		};

		protected string[] QueryCustomerOCPEI_Param = new string[]
		{
			" SELECT [ID_Customer]\r\n              ,[CustomerName]\r\n              ,[ExamTypeName]\r\n              ,[Is_SectionLock]\r\n              ,[Is_FinalFinished]\r\n              ,[ID_FinalDoctor]\r\n              ,[FinalDoctor]\r\n              ,[FinalDate]\r\n              ,[Is_Checked]\r\n              ,[ID_Checker]\r\n              ,[Checker]\r\n              ,[CheckedDate]\r\n              ,[Is_ReportReceipted]\r\n              ,convert(varchar(20),FinalDate,23) FinalFormatDate\r\n           FROM OnCustPhysicalExamInfo OCPEI \r\n          WHERE ID_Customer = @ID_Customer;\r\n          "
		};

		protected string[] QueryQuickConclusionTypeList_Param = new string[]
		{
			"SELECT[ID_ConclusionType]\r\n          ,[ConclusionTypeName]\r\n          ,[InputCode]\r\n          FROM [BusConclusionType] ;\r\n          "
		};

		protected string[] QueryCust_ConclusionList_Param = new string[]
		{
			"SELECT [ID_Conclusion]\r\n              ,[BusConclusionType].ConclusionTypeName\r\n              ,[BusConclusionType].InputCode TypeInputCode\r\n              ,[BusConclusion].[ID_ConclusionType]\r\n              ,[ConclusionName]\r\n              ,[TeamConclusionName]\r\n              ,[Explanation]\r\n              ,[Suggestion]\r\n              ,[DietGuide]\r\n              ,[SportsGuide]\r\n              ,[HealthKnowledge]\r\n              ,[Forsex]\r\n              ,[FinalConclusionTypeName]\r\n              ,[FinalConclusionSignCode]\r\n              ,[BusConclusion].[InputCode]\r\n              ,[BusConclusion].[DispOrder],ISNULL(BusConclusion.IS_Banned ,0) Is_Banned\r\n          FROM [BusConclusion] LEFT JOIN [BusConclusionType] \r\n          ON [BusConclusion].ID_ConclusionType = [BusConclusionType].ID_ConclusionType\r\n          LEFT JOIN DctFinalConclusionType \r\n          ON [BusConclusion].ID_FinalConclusionType = DctFinalConclusionType.ID_FinalConclusionType\r\n          ORDER BY [BusConclusion].[DispOrder] ASC,[BusConclusion].[InputCode] ASC;\r\n                "
		};

		protected string[] QueryCust_ExamSectionList_Param = new string[]
		{
			"SELECT *,NS.[InterfaceType] FROM (\r\n\t\tSELECT [ID_CustExamSection]\r\n              ,[ID_Customer]\r\n              ,[ID_Section]\r\n              ,[CustomerName]\r\n              ,[SectionName]\r\n              ,[DiseaseLevel]\r\n              ,CONVERT(varchar(50),[SectionSummaryDate],120) [SectionSummaryDate]\r\n              ,[SectionSummary]\r\n              ,[PositiveSummary]\r\n              ,[ID_SummaryDoctor]\r\n              ,[SummaryDoctorName]\r\n              ,[ID_Typist]\r\n              ,[TypistName]\r\n              ,[TypistDate]\r\n              ,[Is_Check]\r\n              ,[CheckerName]\r\n              ,[CheckDate]\r\n              ,[ID_Checker]\r\n              ,[IS_giveup]\r\n              ,[IS_Refund]\r\n          FROM [OnCustExamSection]\r\n\t\t WHERE ID_Customer = @ID_Customer ) \r\n      OCES ,\r\n      SYSSection NS\r\n      WHERE OCES.ID_Section = NS.ID_Section \r\n      AND NS.Is_NonFunction = 0 \r\n      AND (OCES.IS_Refund = 0 OR IS_Refund is null ) \r\n ORDER BY ns.DispOrder ASC;\r\n\r\n\r\n          SELECT ID_CustConclusion\r\n                ,OCC.ID_Customer\r\n                ,BC.TeamConclusionName\r\n                ,BC.ConclusionName\r\n                ,OCC.ConclusionName ConclusionShowName\r\n                ,OCC.ConclusionTypeName\r\n                ,OCC.Is_NoEntrySuggestion\r\n                ,OCC.Explanation\r\n                ,OCC.Suggestion\r\n                ,OCC.DietGuide\r\n                ,OCC.SportGuide\r\n                ,OCC.HealthKnowledge\r\n                ,OCC.ID_Doctor\r\n                ,OCC.DoctorName\r\n                ,OCC.ConclusionDate\r\n                ,OCC.ID_Conclusion\r\n                ,OCC.DispOrder\r\n                ,OCC.DiagnoseType\r\n            FROM OnCustConclusion OCC,BusConclusion BC\r\n            WHERE OCC.ID_Conclusion = BC.ID_Conclusion\r\n            AND ID_Customer = @ID_Customer\r\n            ORDER BY OCC.DispOrder ASC,ID_CustConclusion ASC;\r\n            \r\n            SELECT Is_AutoCalc,GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,[OnCustExamItem].*,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY ID_Section ASC, BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n\r\n                "
		};

		protected string[] QueryCustomerFinalCompareDataList_Param = new string[]
		{
			"SELECT * FROM (\r\n\t\tSELECT [ID_CustExamSection]\r\n              ,[ID_Customer]\r\n              ,[ID_Section]\r\n              ,[CustomerName]\r\n              ,[SectionName]\r\n              ,[DiseaseLevel]\r\n              ,CONVERT(varchar(10),[SectionSummaryDate],120) [SectionSummaryDate]\r\n              ,[SectionSummary]\r\n              ,[PositiveSummary]\r\n              ,[ID_SummaryDoctor]\r\n              ,[SummaryDoctorName]\r\n              ,[ID_Typist]\r\n              ,[TypistName]\r\n              ,[TypistDate]\r\n              ,[Is_Check]\r\n              ,[CheckerName]\r\n              ,[CheckDate]\r\n              ,[ID_Checker]\r\n              ,[IS_giveup]\r\n              ,[IS_Refund]\r\n          FROM [OnCustExamSection]\r\n\t\t WHERE ID_Customer = @ID_Customer ) \r\n      OCES ,\r\n      SYSSection NS\r\n      WHERE OCES.ID_Section = NS.ID_Section \r\n      AND NS.Is_NonFunction = 0 \r\n      AND (OCES.IS_Refund = 0 OR IS_Refund is null );\r\n\r\n    SELECT [FinalConclusion]\r\n          ,[ResultCompare]\r\n          ,[MainDiagnose]\r\n          ,[FinalDietGuide]\r\n          ,[FinalSportGuide]\r\n          ,[FinalHealthKnowlage]\r\n      \r\n      FROM [OnCustPhysicalExamInfo]\r\n      where ID_Customer = @ID_Customer;\r\n\r\n                "
		};

		protected string[] QueryFinalExamCompareItemList_Param = new string[]
		{
			"SELECT [OnCustExamItem].ID_CustFee,[OnCustExamItem].ID_Fee,BusFee.FeeName,[OnCustExamItem].ID_ExamItem,[OnCustExamItem].ID_CustExamItem,[OnCustExamItem].ExamItemName,\r\n                GetResultWay,Is_SymMultiValue,BusFee.ID_Section,OnCustFee.ID_Customer,ResultLabValues,ResultLabUnit,ResultLabMark,ResultSummary,ResultLabRange,\r\n                OnCustFee.Is_FeeRefund,OnCustFee.Is_Examined,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder , \r\n                ISNULL(BusExamItem.DispOrder,500) as BusExamItemDispOrder \r\n                FROM [OnCustExamItem],BusExamItem,\r\n                BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee,\r\n                BusFeeDetail\r\n                WHERE \r\n                BusExamItem.ID_ExamItem = [OnCustExamItem].ID_ExamItem\r\n                AND [OnCustExamItem].ID_CustFee = OnCustFee.ID_CustFee\r\n                AND BusExamItem.ID_ExamItem = BusFeeDetail.ID_ExamItem\r\n                AND OnCustFee.ID_Fee = BusFee.ID_Fee\r\n                AND BusFeeDetail.ID_Fee = BusFee.ID_Fee\r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC, BusExamItemDispOrder ASC,BusExamItem.ID_ExamItem ASC;\r\n\r\n                SELECT BusFee.ID_Section,OnCustFee.ID_Fee,OnCustFee.ID_CustFee,OnCustFee.FeeItemName,OnCustFee.Is_FeeRefund,OnCustFee.Is_Examined,\r\n                convert(varchar(20),OnCustFee.ExamDate,23) FormatExamDate,\r\n                ISNULL(BusFee.DispOrder,500) as BusFeeDispOrder ,\r\n                InterfaceType, FeeImageUrl\r\n                FROM \r\n                BusFee,\r\n                (SELECT  * FROM OnCustFee WHERE  OnCustFee.ID_Customer = @ID_Customer ) \r\n                AS OnCustFee, SYSSection\r\n                WHERE OnCustFee.ID_Fee = BusFee.ID_Fee AND ISNULL(OnCustFee.Is_FeeRefund,0) = 0 \r\n                AND SYSSection.ID_Section = BusFee.ID_Section \r\n                AND SYSSection.Is_NonFunction = 0 \r\n                ORDER BY BusFeeDispOrder ASC, BusFee.ID_Fee ASC;\r\n\r\n                SELECT OCES.* ,NS.SummaryName\r\n                FROM OnCustExamSection OCES , SYSSection NS\r\n                WHERE OCES.ID_Section = NS.ID_Section \r\n                AND NS.Is_NonFunction = 0 \r\n                AND OCES.ID_Customer = @ID_Customer ;\r\n\r\n                SELECT ID_Customer,CustomerName,IDCard\r\n                      ,[FinalOverView]\r\n                      ,[FinalConclusion]\r\n                      ,[ResultCompare]\r\n                      ,[MainDiagnose]\r\n                      ,[FinalDietGuide]\r\n                      ,[FinalSportGuide]\r\n                      ,[FinalHealthKnowlage]\r\n                      ,[IndicatorDiagnose]\r\n                      ,[NormalDiagnose]\r\n                      ,[SecondaryDiagnose]\r\n                      ,[OtherDiagnose] FROM OnCustPhysicalExamInfo WHERE ID_Customer = @ID_Customer ;\r\n\r\n            \r\n                -- 将发送分单信息 和 已经录入（读取）结果合并为一个表\r\n                SELECT * FROM (  \r\n\t\t\t    -- 从接口表中读取发送的信息，包括申请单号，发送的对应收费项目编码 (分单使用)\r\n\t\t\t    SELECT * FROM [FYH_Interface].[dbo].[OnCustSendToInterfaceApplyList]\r\n\t\t\t\t    WHERE ID_Customer = @ID_Customer) OCSTIAL LEFT JOIN (\r\n\t\t\t\t\r\n                -- 从客户LAB申请单表中查询出已保存（已读取）的申请单信息 (分单使用)\r\n                SELECT ID_Apply,Is_TypistFinish,CheckDoctorName,ReportTime,ID_Typist,TypistName,TypistDate,DetectionDoctorName,ID_DetectionDoctor\r\n\t\t\t\t    FROM [OnCustApply]\r\n\t\t\t\t    WHERE ID_Customer = @ID_Customer ) OCA ON OCSTIAL.ApplyID = OCA.ID_Apply;\r\n\r\n                "
		};

		protected string[] QueryAllExamSectionList_Param = new string[]
		{
			"SELECT ID_Section\r\n                  ,SectionName\r\n                  ,InterfaceType\r\n                  ,Is_NonFunction\r\n                  ,DispOrder\r\n                  ,'0' IsShowCompareInfo\r\n              FROM [SYSSection]\r\n              WHERE ISNULL(Is_NonFunction,0) = 0\r\n              ORDER BY DispOrder ASC ,ID_Section ASC;"
		};

		protected string[] QueryAllCompareFeeItemList_Param = new string[]
		{
			"SELECT ID_Fee\r\n                  ,BusFee.ID_Section\r\n                  ,FeeName\r\n                  ,SYSSection.SectionName\r\n                  ,SpecimenName\r\n                  ,WorkGroupCode\r\n                  ,WorkStationCode\r\n                  ,WorkBenchCode\r\n                  ,[BusFee].Is_Banned\r\n                  ,[BusFee].DispOrder\r\n                  ,'0' IsShowCompareInfo\r\n                  ,SYSSection.InterfaceType\r\n              FROM [BusFee],[SYSSection] where [BusFee].ID_Section = [SYSSection].ID_Section\r\n              ORDER BY [SYSSection].DispOrder ASC, [SYSSection].ID_Section ASC, BusFee.DispOrder ASC, ID_Fee ASC;"
		};

		protected string[] QueryConclusionNameIsExis_Param = new string[]
		{
			"SELECT [ID_Conclusion]\r\n          FROM [BusConclusion]\r\n          WHERE ConclusionName = @ConclusionName; \r\n             "
		};

		protected string[] QuerySingleConclusionInfo_Param = new string[]
		{
			"SELECT [ID_Conclusion]\r\n                ,BCT.[ID_ConclusionType]\r\n                ,BCT.ConclusionTypeName\r\n                ,[TeamConclusionName]\r\n                ,[ConclusionName]\r\n                ,replace(convert(nvarchar(4000),Explanation),'\u3000','') Explanation\r\n                ,[Suggestion]\r\n                ,[DietGuide]\r\n                ,[SportsGuide]\r\n                ,[HealthKnowledge]\r\n                ,[Forsex]\r\n                ,BC.[InputCode]\r\n                ,[DispOrder]\r\n                ,BC.[Is_Banned]\r\n                ,BC.[ID_BanOpr]\r\n                ,BC.[BanOperator]\r\n                ,BC.[BanDate]\r\n                ,BC.[BanDescribe]\r\n                ,BC.[ID_ICD]\r\n            FROM [BusConclusion] BC LEFT JOIN [BusConclusionType] BCT \r\n              ON BC.ID_ConclusionType = BCT.ID_ConclusionType \r\n           WHERE ID_Conclusion = @ID_Conclusion; \r\n             "
		};

		protected string[] QuerySymptomConnectConclusion_Param = new string[]
		{
			"-- 客户体征词关联的结论词\r\n            SELECT BusConclusion.*,DctFinalConclusionType.FinalConclusionTypeName,DctFinalConclusionType.FinalConclusionSignCode FROM (\r\n\t            SELECT distinct ID_Conclusion FROM \r\n\t            (\r\n\t\t            -- 客户的体征词\r\n\t\t            SELECT ID_Symptom\r\n\t\t              FROM OnCustExamItemResult\r\n\t\t             WHERE ID_CustExamItem IN (\r\n\t\t\t            -- 客户的检查项目\r\n\t\t\t            SELECT ID_CustExamItem \r\n\t\t\t              FROM OnCustExamItem\r\n\t\t\t             WHERE ID_CustFee IN (\r\n\t\t\t\t            -- 客户的收费项目\r\n\t\t\t\t            SELECT ID_CustFee\r\n\t\t\t\t              FROM OnCustFee\r\n\t\t\t\t             WHERE ID_Customer= @ID_Customer\r\n\t\t\t             )\r\n\t\t            )\r\n\t            )\r\n\t            OnCustSymptom \r\n\t            JOIN \r\n\t            (\r\n\t\t            SELECT ID_Symptom,ID_Conclusion\r\n\t\t              FROM BusSymptom\r\n\t\t             WHERE ID_Conclusion IS NOT NULL AND ID_Conclusion > 0\r\n\t            )\r\n\t            BusSymptom\r\n\t            ON OnCustSymptom.ID_Symptom = BusSymptom.ID_Symptom \r\n\t            ) OnCustConclusion,\r\n\t            BusConclusion\r\n            LEFT JOIN DctFinalConclusionType ON DctFinalConclusionType.ID_FinalConclusionType = BusConclusion.ID_FinalConclusionType\r\n            WHERE OnCustConclusion.ID_Conclusion = BusConclusion.ID_Conclusion and ISNULL(BusConclusion.IS_Banned ,0) = 0\r\n            ORDER BY DispOrder ASC,ID_Conclusion ASC;\r\n             "
		};

		DataTable ICommonConclusion.GetPage(string pageCode, int pageIndex, int pageSize, out int recordCount, out int pageCount, params SqlConditionInfo[] conditions)
		{
			return base.GetPage(pageCode, pageIndex, pageSize, out recordCount, out pageCount, conditions);
		}

		DataSet ICommonConclusion.ExcuteQuerySql(string QuerySqlCode, params SqlConditionInfo[] conditions)
		{
			return base.ExcuteQuerySql(QuerySqlCode, conditions);
		}

		DataSet ICommonConclusion.ExcuteQuerySqlX(string AppSettingKey, string QuerySqlCode, params SqlConditionInfo[] conditions)
		{
			return base.ExcuteQuerySqlX(AppSettingKey, QuerySqlCode, conditions);
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

		int ICommonConclusion.SaveFinalConclusionData(PEIS.Model.OnCustPhysicalExamInfo OCPEIModel, List<PEIS.Model.OnCustConclusion> OnCustConclusionList)
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
						int num = this.UpdateOnCustFinalConclusion(sqlCommand, OCPEIModel);
						if (num > 0)
						{
							string text = "";
							if (OnCustConclusionList != null && OnCustConclusionList.Count > 0)
							{
								for (int i = 0; i < OnCustConclusionList.Count; i++)
								{
									if (OnCustConclusionList[i].ID_CustConclusion > 0)
									{
										if (string.IsNullOrEmpty(text))
										{
											text = OnCustConclusionList[i].ID_CustConclusion.ToString();
										}
										else
										{
											text = text + "," + OnCustConclusionList[i].ID_CustConclusion.ToString();
										}
									}
								}
								num += this.DeleteOnCustConclusionItem(sqlCommand, OCPEIModel.ID_Customer, text);
								for (int i = 0; i < OnCustConclusionList.Count; i++)
								{
									if (OnCustConclusionList[i].ID_CustConclusion > 0)
									{
										num += this.UpdateOnCustConclusionItem(sqlCommand, OnCustConclusionList[i]);
									}
									else
									{
										num += this.InsertOnCustConclusionItem(sqlCommand, OnCustConclusionList[i]);
									}
								}
							}
							else
							{
								num += this.DeleteOnCustConclusionItem(sqlCommand, OCPEIModel.ID_Customer, text);
							}
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

		private int UpdateOnCustFinalConclusion(SqlCommand cmd, PEIS.Model.OnCustPhysicalExamInfo OCPEIModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("Is_SectionLock=@Is_SectionLock,");
			stringBuilder.Append("Is_FinalFinished=@Is_FinalFinished,");
			stringBuilder.Append("ID_FinalDoctor=@ID_FinalDoctor,");
			stringBuilder.Append("FinalDoctor=@FinalDoctor,");
			stringBuilder.Append("FinalDate=@FinalDate,");
			stringBuilder.Append("FinalOverView=@FinalOverView,");
			stringBuilder.Append("FinalConclusion=@FinalConclusion,");
			stringBuilder.Append("ResultCompare=@ResultCompare,");
			stringBuilder.Append("MainDiagnose=@MainDiagnose,");
			stringBuilder.Append("IndicatorDiagnose=@IndicatorDiagnose,");
			stringBuilder.Append("OtherDiagnose=@OtherDiagnose,");
			stringBuilder.Append("FinalDietGuide=@FinalDietGuide,");
			stringBuilder.Append("FinalSportGuide=@FinalSportGuide,");
			stringBuilder.Append("FinalHealthKnowlage=@FinalHealthKnowlage,");
			stringBuilder.Append("SecondaryDiagnose=@SecondaryDiagnose,");
			stringBuilder.Append("NormalDiagnose=@NormalDiagnose,");
			stringBuilder.Append("DiseaseLevel=@DiseaseLevel");
			stringBuilder.Append(" where ID_Customer=@ID_Customer AND Is_GuideSheetReturned IS NOT NULL ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_SectionLock", SqlDbType.Bit, 1),
				new SqlParameter("@Is_FinalFinished", SqlDbType.Bit, 1),
				new SqlParameter("@ID_FinalDoctor", SqlDbType.Int, 4),
				new SqlParameter("@FinalDoctor", SqlDbType.VarChar),
				new SqlParameter("@FinalDate", SqlDbType.DateTime),
				new SqlParameter("@FinalOverView", SqlDbType.Text),
				new SqlParameter("@FinalConclusion", SqlDbType.Text),
				new SqlParameter("@ResultCompare", SqlDbType.Text),
				new SqlParameter("@MainDiagnose", SqlDbType.Text),
				new SqlParameter("@IndicatorDiagnose", SqlDbType.Text),
				new SqlParameter("@OtherDiagnose", SqlDbType.Text),
				new SqlParameter("@FinalDietGuide", SqlDbType.Text),
				new SqlParameter("@FinalSportGuide", SqlDbType.Text),
				new SqlParameter("@FinalHealthKnowlage", SqlDbType.Text),
				new SqlParameter("@SecondaryDiagnose", SqlDbType.Text),
				new SqlParameter("@NormalDiagnose", SqlDbType.Text),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCPEIModel.Is_SectionLock;
			array[1].Value = OCPEIModel.Is_FinalFinished;
			array[2].Value = OCPEIModel.ID_FinalDoctor;
			array[3].Value = OCPEIModel.FinalDoctor;
			array[4].Value = OCPEIModel.FinalDate;
			array[5].Value = OCPEIModel.FinalOverView;
			array[6].Value = OCPEIModel.FinalConclusion;
			array[7].Value = OCPEIModel.ResultCompare;
			array[8].Value = OCPEIModel.MainDiagnose;
			array[9].Value = OCPEIModel.IndicatorDiagnose;
			array[10].Value = OCPEIModel.OtherDiagnose;
			array[11].Value = OCPEIModel.FinalDietGuide;
			array[12].Value = OCPEIModel.FinalSportGuide;
			array[13].Value = OCPEIModel.FinalHealthKnowlage;
			array[14].Value = OCPEIModel.SecondaryDiagnose;
			array[15].Value = OCPEIModel.NormalDiagnose;
			array[16].Value = OCPEIModel.DiseaseLevel;
			array[17].Value = OCPEIModel.ID_Customer;
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		protected int InsertOnCustConclusionItem(SqlCommand cmd, PEIS.Model.OnCustConclusion OCCModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into OnCustConclusion(");
			stringBuilder.Append("ID_Customer,ConclusionName,ConclusionTypeName,Is_NoEntrySuggestion,Explanation,Suggestion,DietGuide,SportGuide,HealthKnowledge,ID_Doctor,DoctorName,ConclusionDate,ID_Conclusion,DispOrder,DiagnoseType)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Customer,@ConclusionName,@ConclusionTypeName,@Is_NoEntrySuggestion,@Explanation,@Suggestion,@DietGuide,@SportGuide,@HealthKnowledge,@ID_Doctor,@DoctorName,@ConclusionDate,@ID_Conclusion,@DispOrder,@DiagnoseType)");
			stringBuilder.Append(";select SCOPE_IDENTITY()");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
				new SqlParameter("@ConclusionName", SqlDbType.VarChar),
				new SqlParameter("@ConclusionTypeName", SqlDbType.VarChar),
				new SqlParameter("@Is_NoEntrySuggestion", SqlDbType.Bit, 1),
				new SqlParameter("@Explanation", SqlDbType.Text),
				new SqlParameter("@Suggestion", SqlDbType.Text),
				new SqlParameter("@DietGuide", SqlDbType.Text),
				new SqlParameter("@SportGuide", SqlDbType.Text),
				new SqlParameter("@HealthKnowledge", SqlDbType.Text),
				new SqlParameter("@ID_Doctor", SqlDbType.Int, 4),
				new SqlParameter("@DoctorName", SqlDbType.VarChar),
				new SqlParameter("@ConclusionDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Conclusion", SqlDbType.Int, 4),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@DiagnoseType", SqlDbType.Int, 4)
			};
			array[0].Value = OCCModel.ID_Customer;
			array[1].Value = OCCModel.ConclusionName;
			array[2].Value = OCCModel.ConclusionTypeName;
			array[3].Value = OCCModel.Is_NoEntrySuggestion;
			array[4].Value = OCCModel.Explanation;
			array[5].Value = OCCModel.Suggestion;
			array[6].Value = OCCModel.DietGuide;
			array[7].Value = OCCModel.SportGuide;
			array[8].Value = OCCModel.HealthKnowledge;
			array[9].Value = OCCModel.ID_Doctor;
			array[10].Value = OCCModel.DoctorName;
			array[11].Value = OCCModel.ConclusionDate;
			array[12].Value = OCCModel.ID_Conclusion;
			array[13].Value = (OCCModel.DispOrder);
			array[14].Value = OCCModel.DiagnoseType;
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		public int DeleteOnCustConclusionItem(SqlCommand cmd, long ID_Customer, string sExistConclusionStr)
		{
			int result;
			if (ID_Customer <= 0L)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("delete from OnCustConclusion ");
				if (!string.IsNullOrEmpty(sExistConclusionStr))
				{
					stringBuilder.Append(" where ID_CustConclusion not in (" + sExistConclusionStr + ") and ID_Customer=@ID_Customer");
				}
				else
				{
					stringBuilder.Append(" where ID_Customer=@ID_Customer");
				}
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
				};
				array[0].Value = ID_Customer;
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

		public int UpdateOnCustConclusionItem(SqlCommand cmd, PEIS.Model.OnCustConclusion OCCModel)
		{
			int result;
			if (OCCModel.ID_CustConclusion <= 0)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("update OnCustConclusion set ");
				stringBuilder.Append("ID_Customer=@ID_Customer,");
				stringBuilder.Append("ConclusionName=@ConclusionName,");
				stringBuilder.Append("ConclusionTypeName=@ConclusionTypeName,");
				stringBuilder.Append("Is_NoEntrySuggestion=@Is_NoEntrySuggestion,");
				stringBuilder.Append("Explanation=@Explanation,");
				stringBuilder.Append("Suggestion=@Suggestion,");
				stringBuilder.Append("DietGuide=@DietGuide,");
				stringBuilder.Append("SportGuide=@SportGuide,");
				stringBuilder.Append("HealthKnowledge=@HealthKnowledge,");
				stringBuilder.Append("ID_Doctor=@ID_Doctor,");
				stringBuilder.Append("DoctorName=@DoctorName,");
				stringBuilder.Append("ConclusionDate=@ConclusionDate,");
				stringBuilder.Append("ID_Conclusion=@ID_Conclusion,");
				stringBuilder.Append("DispOrder=@DispOrder,");
				stringBuilder.Append("DiagnoseType=@DiagnoseType");
				stringBuilder.Append(" where ID_CustConclusion=@ID_CustConclusion");
				SqlParameter[] array = new SqlParameter[]
				{
					new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
					new SqlParameter("@ConclusionName", SqlDbType.VarChar),
					new SqlParameter("@ConclusionTypeName", SqlDbType.VarChar),
					new SqlParameter("@Is_NoEntrySuggestion", SqlDbType.Bit, 1),
					new SqlParameter("@Explanation", SqlDbType.Text),
					new SqlParameter("@Suggestion", SqlDbType.Text),
					new SqlParameter("@DietGuide", SqlDbType.Text),
					new SqlParameter("@SportGuide", SqlDbType.Text),
					new SqlParameter("@HealthKnowledge", SqlDbType.Text),
					new SqlParameter("@ID_Doctor", SqlDbType.Int, 4),
					new SqlParameter("@DoctorName", SqlDbType.VarChar),
					new SqlParameter("@ConclusionDate", SqlDbType.DateTime),
					new SqlParameter("@ID_Conclusion", SqlDbType.Int, 4),
					new SqlParameter("@DispOrder", SqlDbType.Int, 4),
					new SqlParameter("@DiagnoseType", SqlDbType.Int, 4),
					new SqlParameter("@ID_CustConclusion", SqlDbType.Int, 4)
				};
				array[0].Value = OCCModel.ID_Customer;
				array[1].Value = OCCModel.ConclusionName;
				array[2].Value = OCCModel.ConclusionTypeName;
				array[3].Value = OCCModel.Is_NoEntrySuggestion;
				array[4].Value = OCCModel.Explanation;
				array[5].Value = OCCModel.Suggestion;
				array[6].Value = OCCModel.DietGuide;
				array[7].Value = OCCModel.SportGuide;
				array[8].Value = OCCModel.HealthKnowledge;
				array[9].Value = OCCModel.ID_Doctor;
				array[10].Value = OCCModel.DoctorName;
				array[11].Value = OCCModel.ConclusionDate;
				array[12].Value = OCCModel.ID_Conclusion;
				array[13].Value = (OCCModel.DispOrder);
				array[14].Value = OCCModel.DiagnoseType;
				array[15].Value = OCCModel.ID_CustConclusion;
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

		int ICommonConclusion.UpdateFinalConclusionSectionLock(PEIS.Model.OnCustPhysicalExamInfo OCPEIModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE OnCustPhysicalExamInfo SET ");
			stringBuilder.Append("Is_SectionLock=@Is_SectionLock");
			stringBuilder.Append(" WHERE Is_SectionLock=@Is_SectionLock_UN AND ID_Customer=@ID_Customer AND Is_GuideSheetReturned IS NOT NULL ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_SectionLock", SqlDbType.Bit, 1),
				new SqlParameter("@Is_SectionLock_UN", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCPEIModel.Is_SectionLock;
			array[1].Value = !(OCPEIModel.Is_SectionLock == true);
			array[2].Value = OCPEIModel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConclusion.SaveCustomerFinalCheck(PEIS.Model.OnFianlCheck OFCModel)
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
						int num = this.UpdatePhysicalExamFinalCheck(sqlCommand, OFCModel);
						if (num > 0)
						{
							num += this.InsertOnCustFianlCheck(sqlCommand, OFCModel);
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

		private int UpdatePhysicalExamFinalCheck(SqlCommand cmd, PEIS.Model.OnFianlCheck OFCModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("Is_FinalFinished=@Is_FinalFinished,");
			stringBuilder.Append("Is_Checked=@Is_Checked,");
			stringBuilder.Append("ID_Checker=@ID_Checker,");
			stringBuilder.Append("Checker=@Checker,");
			stringBuilder.Append("CheckedDate=@CheckedDate");
			stringBuilder.Append(" where ID_Customer=@ID_Customer");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_FinalFinished", SqlDbType.Bit, 1),
				new SqlParameter("@Is_Checked", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@Checker", SqlDbType.VarChar),
				new SqlParameter("@CheckedDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = (OFCModel.Is_Pass == true);
			array[1].Value = (OFCModel.Is_Pass == true);
			array[2].Value = OFCModel.ID_FinalCheckDoctor;
			array[3].Value = OFCModel.FinalCheckDoctor;
			array[4].Value = OFCModel.FinaleCheckDate;
			array[5].Value = OFCModel.ID_Customer;
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		int ICommonConclusion.SaveCustomerFinaUnCheck(PEIS.Model.OnFianlCheck OFCModel)
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
						int num = this.UpdatePhysicalExamFinalUnCheck(sqlCommand, OFCModel);
						if (num > 0)
						{
							num += this.InsertOnCustFianlCheck(sqlCommand, OFCModel);
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

		private int UpdatePhysicalExamFinalUnCheck(SqlCommand cmd, PEIS.Model.OnFianlCheck OFCModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append(" Is_Checked=@Is_Checked ");
			stringBuilder.Append(" where ID_Customer=@ID_Customer");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_Checked", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = (OFCModel.Is_Pass == true);
			array[1].Value = OFCModel.ID_Customer;
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		int ICommonConclusion.UpdateCustomerNotFinalFinished(PEIS.Model.OnCustPhysicalExamInfo OCPEIModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("Is_FinalFinished=@Is_FinalFinished");
			stringBuilder.Append(" where ID_Customer=@ID_Customer");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_FinalFinished", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCPEIModel.Is_FinalFinished;
			array[1].Value = OCPEIModel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		protected int InsertOnCustFianlCheck(SqlCommand cmd, PEIS.Model.OnFianlCheck OFCModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into OnFianlCheck(");
			stringBuilder.Append("ID_Customer,CustomerName,ID_FinalDoctor,FinalDoctor,SubmitDate,ID_FinalCheckDoctor,FinalCheckDoctor,FinaleCheckDate,Is_Pass,RefuseReason)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Customer,@CustomerName,@ID_FinalDoctor,@FinalDoctor,@SubmitDate,@ID_FinalCheckDoctor,@FinalCheckDoctor,@FinaleCheckDate,@Is_Pass,@RefuseReason)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8),
				new SqlParameter("@CustomerName", SqlDbType.VarChar),
				new SqlParameter("@ID_FinalDoctor", SqlDbType.Int, 4),
				new SqlParameter("@FinalDoctor", SqlDbType.VarChar),
				new SqlParameter("@SubmitDate", SqlDbType.DateTime),
				new SqlParameter("@ID_FinalCheckDoctor", SqlDbType.Int, 4),
				new SqlParameter("@FinalCheckDoctor", SqlDbType.VarChar),
				new SqlParameter("@FinaleCheckDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Pass", SqlDbType.Bit, 1),
				new SqlParameter("@RefuseReason", SqlDbType.Text)
			};
			array[0].Value = OFCModel.ID_Customer;
			array[1].Value = OFCModel.CustomerName;
			array[2].Value = OFCModel.ID_FinalDoctor;
			array[3].Value = OFCModel.FinalDoctor;
			array[4].Value = OFCModel.SubmitDate;
			array[5].Value = OFCModel.ID_FinalCheckDoctor;
			array[6].Value = OFCModel.FinalCheckDoctor;
			array[7].Value = OFCModel.FinaleCheckDate;
			array[8].Value = OFCModel.Is_Pass;
			array[9].Value = OFCModel.RefuseReason;
			cmd.Parameters.Clear();
			cmd.CommandText = stringBuilder.ToString();
			for (int i = 0; i < array.Length; i++)
			{
				cmd.Parameters.Add(array[i]);
			}
			return cmd.ExecuteNonQuery();
		}

		int ICommonConclusion.LockCustomerFinalCheck(PEIS.Model.OnCustPhysicalExamInfo OCPEIModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update OnCustPhysicalExamInfo set ");
			stringBuilder.Append("Is_FinalFinished=@Is_FinalFinished , ");
			stringBuilder.Append("Is_Checked=@Is_Checked , ");
			stringBuilder.Append("ID_Checker=@ID_Checker,");
			stringBuilder.Append("Checker=@Checker,");
			stringBuilder.Append("CheckedDate=@CheckedDate,");
			stringBuilder.Append(" where ID_Customer=@ID_Customer");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@Is_SectionLock", SqlDbType.Bit, 1),
				new SqlParameter("@Is_Checked", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Checker", SqlDbType.Int, 4),
				new SqlParameter("@Checker", SqlDbType.VarChar),
				new SqlParameter("@CheckedDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = OCPEIModel.Is_FinalFinished;
			array[1].Value = (OCPEIModel.Is_Checked);
			array[2].Value = OCPEIModel.ID_Checker;
			array[3].Value = OCPEIModel.Checker;
			array[4].Value = OCPEIModel.CheckedDate;
			array[5].Value = OCPEIModel.ID_Customer;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		PEIS.Model.OnFianlCheck ICommonConclusion.GetCustomerLastOnFianlCheck(long ID_Customer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select  top 1 ID_FinalCheck,ID_Customer,CustomerName,ID_FinalDoctor,FinalDoctor,SubmitDate,ID_FinalCheckDoctor,FinalCheckDoctor,FinaleCheckDate,Is_Pass,RefuseReason from OnFianlCheck ");
			stringBuilder.Append(" where ID_Customer=@ID_Customer order by ID_FinalCheck desc ");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Customer", SqlDbType.BigInt, 8)
			};
			array[0].Value = ID_Customer;
			PEIS.Model.OnFianlCheck onFianlCheck = new PEIS.Model.OnFianlCheck();
			DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString(), array);
			PEIS.Model.OnFianlCheck result;
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				if (dataSet.Tables[0].Rows[0]["ID_FinalCheck"].ToString() != "")
				{
					onFianlCheck.ID_FinalCheck = int.Parse(dataSet.Tables[0].Rows[0]["ID_FinalCheck"].ToString());
				}
				if (dataSet.Tables[0].Rows[0]["ID_Customer"].ToString() != "")
				{
					onFianlCheck.ID_Customer = new long?(long.Parse(dataSet.Tables[0].Rows[0]["ID_Customer"].ToString()));
				}
				onFianlCheck.CustomerName = dataSet.Tables[0].Rows[0]["CustomerName"].ToString();
				if (dataSet.Tables[0].Rows[0]["ID_FinalDoctor"].ToString() != "")
				{
					onFianlCheck.ID_FinalDoctor = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_FinalDoctor"].ToString()));
				}
				onFianlCheck.FinalDoctor = dataSet.Tables[0].Rows[0]["FinalDoctor"].ToString();
				if (dataSet.Tables[0].Rows[0]["SubmitDate"].ToString() != "")
				{
					onFianlCheck.SubmitDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["SubmitDate"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["ID_FinalCheckDoctor"].ToString() != "")
				{
					onFianlCheck.ID_FinalCheckDoctor = new int?(int.Parse(dataSet.Tables[0].Rows[0]["ID_FinalCheckDoctor"].ToString()));
				}
				onFianlCheck.FinalCheckDoctor = dataSet.Tables[0].Rows[0]["FinalCheckDoctor"].ToString();
				if (dataSet.Tables[0].Rows[0]["FinaleCheckDate"].ToString() != "")
				{
					onFianlCheck.FinaleCheckDate = new DateTime?(DateTime.Parse(dataSet.Tables[0].Rows[0]["FinaleCheckDate"].ToString()));
				}
				if (dataSet.Tables[0].Rows[0]["Is_Pass"].ToString() != "")
				{
					if (dataSet.Tables[0].Rows[0]["Is_Pass"].ToString() == "1" || dataSet.Tables[0].Rows[0]["Is_Pass"].ToString().ToLower() == "true")
					{
						onFianlCheck.Is_Pass = new bool?(true);
					}
					else
					{
						onFianlCheck.Is_Pass = new bool?(false);
					}
				}
				onFianlCheck.RefuseReason = dataSet.Tables[0].Rows[0]["RefuseReason"].ToString();
				result = onFianlCheck;
			}
			else
			{
				result = null;
			}
			return result;
		}

		int ICommonConclusion.SaveConclusion(PEIS.Model.BusConclusion ConclusionModel)
		{
			int result;
			if (ConclusionModel.ID_Conclusion > 0)
			{
				result = this.BusConclusionUpdate(ConclusionModel);
			}
			else
			{
				result = this.BusConclusionAdd(ConclusionModel);
			}
			return result;
		}

		private int BusConclusionAdd(PEIS.Model.BusConclusion model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusConclusion(");
			stringBuilder.Append("ID_ConclusionType,ConclusionName,Explanation,Suggestion,DietGuide,SportsGuide,HealthKnowledge,ForGender,InputCode,DispOrder,TeamConclusionName,ID_Createopr,CreateOperator,CreateDate,Is_Banned,ID_BanOpr,BanOperator,BanDate,BanDescribe,ID_ICD,ID_FinalConclusionType)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_ConclusionType,@ConclusionName,@Explanation,@Suggestion,@DietGuide,@SportsGuide,@HealthKnowledge,@ForGender,@InputCode,@DispOrder,@TeamConclusionName,@ID_Createopr,@CreateOperator,@CreateDate,@Is_Banned,@ID_BanOpr,@BanOperator,@BanDate,@BanDescribe,@ID_ICD,@ID_FinalConclusionType)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_ConclusionType", SqlDbType.Int, 4),
				new SqlParameter("@ConclusionName", SqlDbType.VarChar),
				new SqlParameter("@Explanation", SqlDbType.Text),
				new SqlParameter("@Suggestion", SqlDbType.Text),
				new SqlParameter("@DietGuide", SqlDbType.Text),
				new SqlParameter("@SportsGuide", SqlDbType.Text),
				new SqlParameter("@HealthKnowledge", SqlDbType.Text),
				new SqlParameter("@ForGender", SqlDbType.Int, 4),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@TeamConclusionName", SqlDbType.VarChar),
				new SqlParameter("@ID_Createopr", SqlDbType.Int, 4),
				new SqlParameter("@CreateOperator", SqlDbType.VarChar),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@ID_ICD", SqlDbType.Int, 4),
				new SqlParameter("@ID_FinalConclusionType", SqlDbType.Int, 4)
			};
			array[0].Value = (model.ID_ConclusionType);
			array[1].Value = model.ConclusionName;
			array[2].Value = model.Explanation;
			array[3].Value = model.Suggestion;
			array[4].Value = model.DietGuide;
			array[5].Value = model.SportsGuide;
			array[6].Value = model.HealthKnowledge;
			array[7].Value = model.ForGender;
			array[8].Value = model.InputCode;
			array[9].Value = model.DispOrder;
			array[10].Value = model.TeamConclusionName;
			array[11].Value = model.ID_Createopr;
			array[12].Value = model.CreateOperator;
			array[13].Value = model.CreateDate;
			array[14].Value = model.Is_Banned;
			array[15].Value = model.ID_BanOpr;
			array[16].Value = model.BanOperator;
			array[17].Value = model.BanDate;
			array[18].Value = model.BanDescribe;
			array[19].Value = (model.ID_ICD);
			array[20].Value = (model.ID_FinalConclusionType);
			object single = DbHelperSQL.GetSingle(stringBuilder.ToString(), array);
			int result;
			if (single == null)
			{
				result = 0;
			}
			else
			{
				result = Convert.ToInt32(single);
			}
			return result;
		}

		public int BusConclusionUpdate(PEIS.Model.BusConclusion model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusConclusion set ");
			stringBuilder.Append("ID_ConclusionType=@ID_ConclusionType,");
			stringBuilder.Append("ConclusionName=@ConclusionName,");
			stringBuilder.Append("Explanation=@Explanation,");
			stringBuilder.Append("Suggestion=@Suggestion,");
			stringBuilder.Append("DietGuide=@DietGuide,");
			stringBuilder.Append("SportsGuide=@SportsGuide,");
			stringBuilder.Append("HealthKnowledge=@HealthKnowledge,");
			stringBuilder.Append("ForGender=@ForGender,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("TeamConclusionName=@TeamConclusionName,");
			stringBuilder.Append("ID_Createopr=@ID_Createopr,");
			stringBuilder.Append("CreateOperator=@CreateOperator,");
			stringBuilder.Append("CreateDate=@CreateDate,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_BanOpr=@ID_BanOpr,");
			stringBuilder.Append("BanOperator=@BanOperator,");
			stringBuilder.Append("BanDate=@BanDate,");
			stringBuilder.Append("BanDescribe=@BanDescribe,");
			stringBuilder.Append("ID_ICD=@ID_ICD,");
			stringBuilder.Append("ID_FinalConclusionType=@ID_FinalConclusionType");
			stringBuilder.Append(" where ID_Conclusion=@ID_Conclusion");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_ConclusionType", SqlDbType.Int, 4),
				new SqlParameter("@ConclusionName", SqlDbType.VarChar),
				new SqlParameter("@Explanation", SqlDbType.Text),
				new SqlParameter("@Suggestion", SqlDbType.Text),
				new SqlParameter("@DietGuide", SqlDbType.Text),
				new SqlParameter("@SportsGuide", SqlDbType.Text),
				new SqlParameter("@HealthKnowledge", SqlDbType.Text),
				new SqlParameter("@ForGender", SqlDbType.Int, 4),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@TeamConclusionName", SqlDbType.VarChar),
				new SqlParameter("@ID_Createopr", SqlDbType.Int, 4),
				new SqlParameter("@CreateOperator", SqlDbType.VarChar),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@ID_ICD", SqlDbType.Int, 4),
				new SqlParameter("@ID_FinalConclusionType", SqlDbType.Int, 4),
				new SqlParameter("@ID_Conclusion", SqlDbType.Int, 4)
			};
			array[0].Value = (model.ID_ConclusionType);
			array[1].Value = model.ConclusionName;
			array[2].Value = model.Explanation;
			array[3].Value = model.Suggestion;
			array[4].Value = model.DietGuide;
			array[5].Value = model.SportsGuide;
			array[6].Value = model.HealthKnowledge;
			array[7].Value = model.ForGender;
			array[8].Value = model.InputCode;
			array[9].Value = model.DispOrder;
			array[10].Value = model.TeamConclusionName;
			array[11].Value = model.ID_Createopr;
			array[12].Value = model.CreateOperator;
			array[13].Value = model.CreateDate;
			array[14].Value = model.Is_Banned;
			array[15].Value = model.ID_BanOpr;
			array[16].Value = model.BanOperator;
			array[17].Value = model.BanDate;
			array[18].Value = model.BanDescribe;
			array[19].Value = (model.ID_ICD);
			array[20].Value = (model.ID_FinalConclusionType);
			array[21].Value = model.ID_Conclusion;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}
	}
}
