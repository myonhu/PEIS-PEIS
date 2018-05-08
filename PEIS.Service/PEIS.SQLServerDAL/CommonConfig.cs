using PEIS.IDAL;
using PEIS.Model;
using PEIS.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace PEIS.SQLServerDAL
{
	public class CommonConfig : CommonBase, ICommonConfig
	{
		protected string[] QueryPagesSectionListParam = new string[]
		{
            "SectionID",
			"*",
            "  ( SELECT [SectionID],[SectionName],[FunctionType],[DisplayMenu],[IsDel],[InterfaceType],[IsNotSamePage],[IsNonPrintSectSummary],[IsOwnInterface],[IsAutoApprove],[ImageType],[PicPrintSetup],[PacsInterfaceFlag],[InputCode],[DispOrder],[SummaryName],[DefaultSummary],[SepBetweenExamItems],[SepBetweenSymptoms],[TerminalSymbol],[SepExamAndValue],[NoBetweenExamItems],[NoBetweenSympotms],[Note],[IsNoEntryFinalSummary],[IsNonPrintInReport],[IsPrintBarCode] FROM [SCPEIS].[dbo].[SYSSection] )  ",
            " ORDER BY DispOrder ASC "
        };

		protected string[] QueryPagesSectionListParamByName = new string[]
		{
            "SectionID",
			"*",
            "  ( SELECT [SectionID],[SectionName],[FunctionType],[DisplayMenu],[IsDel],[InterfaceType],[IsNotSamePage],[IsNonPrintSectSummary],[IsOwnInterface],[IsAutoApprove],[ImageType],[PicPrintSetup],[PacsInterfaceFlag],[InputCode],[DispOrder],[SummaryName],[DefaultSummary],[SepBetweenExamItems],[SepBetweenSymptoms],[TerminalSymbol],[SepExamAndValue],[NoBetweenExamItems],[NoBetweenSympotms],[Note],[IsNoEntryFinalSummary],[IsNonPrintInReport],[IsPrintBarCode]  FROM [SYSSection] WHERE [SectionName] LIKE @SectionName OR [InputCode] LIKE @SectionName\r\n                                                          ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QuerySingleSectionItem_Param = new string[]
		{
            "SELECT [SectionID],[SectionName],[FunctionType],[DisplayMenu],[IsDel],[InterfaceType],[IsNotSamePage],[IsNonPrintSectSummary],[IsOwnInterface],[IsAutoApprove],[ImageType],[PicPrintSetup],[PacsInterfaceFlag],[InputCode],[DispOrder],[SummaryName],[DefaultSummary],[SepBetweenExamItems],[SepBetweenSymptoms],[TerminalSymbol],[SepExamAndValue],[NoBetweenExamItems],[NoBetweenSympotms],[Note],[IsNoEntryFinalSummary],[IsNonPrintInReport],[IsPrintBarCode]   FROM [SYSSection]  \r\n         WHERE SectionID = @SectionID ;\r\n          "
        };

		protected string[] QueryPagesFeeList_Param = new string[]
		{
			"ID_Fee",
			"*",
            " (SELECT  Top 100000 ID_Fee\r\n              ,ID_Section\r\n              ,SectionName\r\n              ,ID_Specimen\r\n              ,SpecimenName\r\n              ,FeeName\r\n              ,ForGender\r\n              ,ReportFeeName\r\n              ,FeeCode\r\n              ,cast(round(Price,2) as numeric(10,2)) Price\r\n              ,BusFee.InputCode\r\n              ,WorkGroupCode\r\n              ,WorkStationCode\r\n              ,WorkBenchCode\r\n              ,CreateDate\r\n              ,Is_Banned\r\n              ,ID_BanOpr\r\n              ,BanDescribe\r\n              ,DispOrder\r\n              ,Note\r\n              ,BreakfastOrder\r\n          FROM BusFee\r\n          ORDER BY DispOrder ASC) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesFeeListParamBySection = new string[]
		{
			"ID_Fee",
			"*",
            " (SELECT Top 100000 ID_Fee\r\n              ,ID_Section\r\n              ,SectionName\r\n              ,ID_Specimen\r\n              ,SpecimenName\r\n              ,FeeName\r\n              ,ForGender\r\n              ,ReportFeeName\r\n              ,FeeCode\r\n              ,cast(round(Price,2) as numeric(10,2)) Price\r\n              ,BusFee.InputCode\r\n              ,WorkGroupCode\r\n              ,WorkStationCode\r\n              ,WorkBenchCode\r\n              ,CreateDate\r\n              ,Is_Banned\r\n              ,ID_BanOpr\r\n              ,BanDescribe\r\n              ,DispOrder\r\n              ,Note\r\n              ,BreakfastOrder\r\n          FROM BusFee\r\n          WHERE ID_Section = @SectionID\r\n          ORDER BY DispOrder ASC ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesSectionFeeListParamByName = new string[]
		{
			"ID_Fee",
			"*",
            " (SELECT Top 100000 ID_Fee\r\n              ,ID_Section\r\n              ,SectionName\r\n              ,ID_Specimen\r\n              ,SpecimenName\r\n              ,FeeName\r\n              ,ForGender\r\n              ,ReportFeeName\r\n              ,FeeCode\r\n              ,cast(round(Price,2) as numeric(10,2)) Price\r\n              ,BusFee.InputCode\r\n              ,WorkGroupCode\r\n              ,WorkStationCode\r\n              ,WorkBenchCode\r\n              ,CreateDate\r\n              ,Is_Banned\r\n              ,ID_BanOpr\r\n              ,BanDescribe\r\n              ,DispOrder\r\n              ,Note\r\n              ,BreakfastOrder\r\n          FROM BusFee\r\n          WHERE InputCode LIKE @FeeName OR  FeeName LIKE @FeeName \r\n                 ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesFeeListParamByNameAndSection = new string[]
		{
			"ID_Fee",
			"*",
            " (SELECT Top 100000 ID_Fee\r\n              ,ID_Section\r\n              ,SectionName\r\n              ,ID_Specimen\r\n              ,SpecimenName\r\n              ,FeeName\r\n              ,ForGender\r\n              ,ReportFeeName\r\n              ,FeeCode\r\n              ,cast(round(Price,2) as numeric(10,2)) Price\r\n              ,BusFee.InputCode\r\n              ,WorkGroupCode\r\n              ,WorkStationCode\r\n              ,WorkBenchCode\r\n              ,CreateDate\r\n              ,Is_Banned\r\n              ,ID_BanOpr\r\n              ,BanDescribe\r\n              ,DispOrder\r\n              ,Note\r\n              ,BreakfastOrder\r\n          FROM BusFee\r\n          WHERE ID_Section = @SectionID AND (InputCode LIKE @FeeName OR  FeeName LIKE @FeeName)  \r\n          ORDER BY DispOrder ASC\r\n          ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryQuickFeeList_Param = new string[]
		{
            "SELECT ID_Fee\r\n              ,ID_Section\r\n              ,SectionName\r\n              ,InputCode\r\n              ,ID_Specimen\r\n              ,SpecimenName\r\n              ,FeeName\r\n              ,ForGender\r\n              ,ReportFeeName\r\n              ,FeeCode\r\n              ,DispOrder\r\n              ,cast(round(Price,2) as numeric(10,2)) Price\r\n      FROM BusFee\r\n  ORDER BY InputCode ASC;\r\n          "
        };

		protected string[] QueryPagesFeeReportList_Param = new string[]
		{
			"ID_Fee",
			"*",
			" (SELECT BFR.*,BF.ID_Section,BF.SectionName,BF.DispOrder\r\n                  FROM BusFee BF,BusFeeReport BFR\r\n                  WHERE BF.ID_Fee = BFR.ID_Fee ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesFeeReportListParamBySection = new string[]
		{
			"ID_Fee",
			"*",
			" (SELECT BFR.*,BF.ID_Section,BF.SectionName,BF.DispOrder\r\n                  FROM BusFee BF,BusFeeReport BFR\r\n                  WHERE BF.ID_Fee = BFR.ID_Fee AND ID_Section = @ID_Section ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesSectionFeeReportListParamByName = new string[]
		{
			"ID_Fee",
			"*",
			" (SELECT BFR.*,BF.ID_Section,BF.SectionName,BF.DispOrder\r\n                  FROM BusFee BF,BusFeeReport BFR\r\n                  WHERE BF.ID_Fee = BFR.ID_Fee AND ( InputCode LIKE @FeeName OR  BF.FeeName LIKE @FeeName OR  BFR.FeeName LIKE @FeeName )\r\n          ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesFeeReportListParamByNameAndSection = new string[]
		{
			"ID_Fee",
			"*",
			" (SELECT BFR.*,BF.ID_Section,BF.SectionName,BF.DispOrder\r\n                  FROM BusFee BF,BusFeeReport BFR\r\n                  WHERE BF.ID_Fee = BFR.ID_Fee AND ID_Section = @ID_Section AND (InputCode LIKE @FeeName OR  BF.FeeName LIKE @FeeName OR  BFR.FeeName LIKE @FeeName)  \r\n         \r\n          ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryQuickFeeReportList_Param = new string[]
		{
			"SELECT BFR.*,BF.ID_Section,BF.SectionName,BF.DispOrder\r\n                  FROM BusFee BF,BusFeeReport BFR\r\n                  WHERE BF.ID_Fee = BFR.ID_Fee\r\n              ORDER BY BF.InputCode ASC;\r\n          "
		};

		protected string[] QueryPagesExamList_Param = new string[]
		{
            "ExamItemID",
			"*",
            " (SELECT ExamItemID\r\n                    ,ExamItemName\r\n                    ,GetResultWay\r\n                    ,ExamItemCode\r\n                    ,BusExamItem.SectionID\r\n                    ,SYSSection.SectionName\r\n                    ,IsLisValueNull\r\n                    ,IsEntrySectSum\r\n                    ,EntrySectSumLevel\r\n                    ,IsAutoCalc\r\n                    ,CalcExpression\r\n                    ,SymCols\r\n                    ,TextboxRows\r\n                    ,IsSameRow\r\n                    ,ExamItemUnit\r\n                    ,MaleHiLimit\r\n                    ,MaleLoLimit\r\n                    ,FemaleHiLimit\r\n                    ,FemaleLoLimit\r\n                    ,IsSymMultiValue\r\n                    ,IsExamItemNonPrintInReport\r\n                    ,BusExamItem.InputCode\r\n                    ,BusExamItem.DispOrder\r\n                    ,BusExamItem.Note\r\n                    ,BusExamItem.AbbrExamName\r\n                    ,ForGender\r\n                FROM BusExamItem,SYSSection\r\n                WHERE BusExamItem.SectionID = SYSSection.SectionID \r\n                ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesExamListParamBySection = new string[]
		{
            "ExamItemID",
			"*",
            " (SELECT ExamItemID\r\n                    ,ExamItemName\r\n                    ,GetResultWay\r\n                    ,ExamItemCode\r\n                    ,BusExamItem.SectionID\r\n                    ,SYSSection.SectionName\r\n                    ,IsLisValueNull\r\n                    ,IsEntrySectSum\r\n                    ,EntrySectSumLevel\r\n                    ,IsAutoCalc\r\n                    ,CalcExpression\r\n                    ,SymCols\r\n                    ,TextboxRows\r\n                    ,IsSameRow\r\n                    ,ExamItemUnit\r\n                    ,MaleHiLimit\r\n                    ,MaleLoLimit\r\n                    ,FemaleHiLimit\r\n                    ,FemaleLoLimit\r\n                    ,IsSymMultiValue\r\n                    ,IsExamItemNonPrintInReport\r\n                    ,BusExamItem.InputCode\r\n                    ,BusExamItem.DispOrder\r\n                    ,BusExamItem.Note\r\n                    ,BusExamItem.AbbrExamName\r\n                    ,ForGender\r\n                FROM BusExamItem,SYSSection\r\n                WHERE BusExamItem.SectionID = SYSSection.SectionID AND BusExamItem.SectionID = @SectionID\r\n                ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesSectionExamListParamByName = new string[]
		{
            "ExamItemID",
			"*",
            " (SELECT ExamItemID\r\n                    ,ExamItemName\r\n                    ,GetResultWay\r\n                    ,ExamItemCode\r\n                    ,BusExamItem.SectionID\r\n                    ,SYSSection.SectionName\r\n                    ,IsLisValueNull\r\n                    ,IsEntrySectSum\r\n                    ,EntrySectSumLevel\r\n                    ,IsAutoCalc\r\n                    ,CalcExpression\r\n                    ,SymCols\r\n                    ,TextboxRows\r\n                    ,IsSameRow\r\n                    ,ExamItemUnit\r\n                    ,MaleHiLimit\r\n                    ,MaleLoLimit\r\n                    ,FemaleHiLimit\r\n                    ,FemaleLoLimit\r\n                    ,IsSymMultiValue\r\n                    ,IsExamItemNonPrintInReport\r\n                    ,BusExamItem.InputCode\r\n                    ,BusExamItem.DispOrder\r\n                    ,BusExamItem.Note\r\n                    ,BusExamItem.AbbrExamName\r\n                    ,ForGender\r\n                FROM BusExamItem,SYSSection\r\n                WHERE BusExamItem.SectionID = SYSSection.SectionID \r\n                AND (ExamItemName LIKE @ExamItemName OR BusExamItem.InputCode LIKE @ExamItemName) \r\n                ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesExamListParamByNameAndSection = new string[]
		{
            "ExamItemID",
			"*",
            " (SELECT ExamItemID\r\n                    ,ExamItemName\r\n                    ,GetResultWay\r\n                    ,ExamItemCode\r\n                    ,BusExamItem.SectionID\r\n                    ,SYSSection.SectionName\r\n                    ,IsLisValueNull\r\n                    ,IsEntrySectSum\r\n                    ,EntrySectSumLevel\r\n                    ,IsAutoCalc\r\n                    ,CalcExpression\r\n                    ,SymCols\r\n                    ,TextboxRows\r\n                    ,IsSameRow\r\n                    ,ExamItemUnit\r\n                    ,MaleHiLimit\r\n                    ,MaleLoLimit\r\n                    ,FemaleHiLimit\r\n                    ,FemaleLoLimit\r\n                    ,IsSymMultiValue\r\n                    ,IsExamItemNonPrintInReport\r\n                    ,BusExamItem.InputCode\r\n                    ,BusExamItem.DispOrder\r\n                    ,BusExamItem.Note\r\n                    ,BusExamItem.AbbrExamName\r\n                    ,ForGender\r\n                FROM BusExamItem,SYSSection\r\n                WHERE BusExamItem.SectionID = SYSSection.SectionID \r\n                AND BusExamItem.SectionID = @SectionID\r\n                AND (ExamItemName LIKE @ExamItemName OR BusExamItem.InputCode LIKE @ExamItemName) \r\n                ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesSetListParamByName = new string[]
		{
			"PEPackageID",
			"*",
            " (SELECT [PEPackageID],[PEPackageName],[Forsex],[CreatorID],[CreateDate],[isBanned],[IDBanOpr],[BanDate],[BanDescribe],[InputCode],[DispOrder],[Note]  FROM  [BusPEPackage]  WHERE [PEPackageName] LIKE @PEPackageName OR [InputCode] LIKE @PEPackageName \r\n                ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesSetList_Param = new string[]
		{
			"PEPackageID",
			"*",
            " (SELECT [PEPackageID],[PEPackageName],[Forsex],[CreatorID],[CreateDate],[isBanned],[IDBanOpr],[BanDate],[BanDescribe],[InputCode],[DispOrder],[Note]  FROM [BusPEPackage]            ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesRoleListParamByName = new string[]
		{
			"RoleID",
			"*",
            " (SELECT [RoleID],[RoleName],[DispOrder],[Is_Locked],[Remark],[CreateDate],[OperatorID],[Is_DefaultRole] FROM [SysRole]  WHERE [RoleName] LIKE @RoleName \r\n                ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesRoleList_Param = new string[]
		{
            "RoleID",
			"*",
            " (SELECT [RoleID],[RoleName],[DispOrder],[Is_Locked],[Remark],[CreateDate],[OperatorID],[Is_DefaultRole] FROM [SysRole] ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryRoleNameIsExis_Param = new string[]
		{
            "SELECT [RoleID]\r\n          FROM [SYSRole]\r\n          WHERE RoleName = @RoleName; \r\n             "
        };

		protected string[] QueryPagesExamItemGroupList_Param = new string[]
		{
			"ID_ExamItemGroup",
			"*",
			" (SELECT * FROM BusExamItemGroup ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesExamItemGroupListParamByName = new string[]
		{
			"ID_ExamItemGroup",
			"*",
			" (SELECT * FROM BusExamItemGroup \r\n                  WHERE InputCode LIKE @ExamItemGroupName OR  ExamItemGroupName LIKE @ExamItemGroupName \r\n          ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryQuickExamItemGroupList_Param = new string[]
		{
			"SELECT * FROM BusExamItemGroup\r\n                  ORDER BY DispOrder ASC;\r\n          "
		};

		protected string[] QueryUserNameIsExis_Param = new string[]
		{
			"SELECT [UserID]\r\n          FROM [SYSOpUser]\r\n          WHERE UserName = @UserName; \r\n             "
		};

		protected string[] QueryUserLoginNameIsExis_Param = new string[]
		{
            "SELECT [UserID]\r\n          FROM [SYSOpUser]\r\n          WHERE LoginName = @LoginName; \r\n             "
        };

		protected string[] QuerySectionNameIsExis_Param = new string[]
		{
			"SELECT [SectionID]\r\n          FROM [SYSSection]\r\n          WHERE SectionName = @SectionName; \r\n             "
		};

		protected string[] QueryFeeNameIsExis_Param = new string[]
		{
			"SELECT [ID_Fee]\r\n          FROM [BusFee]\r\n          WHERE FeeName = @FeeName and Is_Banned = 0; \r\n             "
		};

		protected string[] QueryExamItemIsExis_Param = new string[]
		{
			"SELECT ID_ExamItem\r\n              FROM BusExamItem\r\n              WHERE ExamItemName = @ExamItemName\r\n              AND ID_Section = @ID_Section\r\n              AND ExamItemCode = @ExamItemCode\r\n              AND Note = @Note\r\n             "
		};

		protected string[] QueryExamItemIsExis_NoNote_Param = new string[]
		{
			"SELECT ID_ExamItem\r\n              FROM BusExamItem\r\n              WHERE ExamItemName = @ExamItemName\r\n              AND ID_Section = @ID_Section\r\n              AND ExamItemCode = @ExamItemCode\r\n              AND ISNULL(Note,'') = ''\r\n             "
		};

		protected string[] QueryExamItemIsExis_NoExamCode_NoNote_Param = new string[]
		{
			"SELECT ID_ExamItem\r\n              FROM BusExamItem\r\n              WHERE ExamItemName = @ExamItemName\r\n              AND ID_Section = @ID_Section\r\n              AND ISNULL(ExamItemCode,'') = ''\r\n              AND ISNULL(Note,'') = ''\r\n             "
		};

		protected string[] QueryExamItemIsExis_NoExamCode_Param = new string[]
		{
			"SELECT ID_ExamItem\r\n              FROM BusExamItem\r\n              WHERE ExamItemName = @ExamItemName\r\n              AND ID_Section = @ID_Section\r\n              AND ISNULL(ExamItemCode,'') = ''\r\n              AND Note = @Note\r\n             "
		};

		protected string[] QueryExamDetailListByFee_Param = new string[]
		{
			"SELECT [ID_ExamItem]\r\n                  ,[ExamItemName]\r\n                  ,[GetResultWay]\r\n                  ,[ExamItemCode]\r\n                  ,[ID_Section]\r\n                  ,[Is_LisValueNull]\r\n                  ,[Is_EntrySectSum]\r\n                  ,[EntrySectSumLevel]\r\n                  ,[Is_AutoCalc]\r\n                  ,[CalcExpression]\r\n                  ,[SymCols]\r\n                  ,[TextboxRows]\r\n                  ,[Is_SameRow]\r\n                  ,[ExamItemUnit]\r\n                  ,[MaleHiLimit]\r\n                  ,[MaleLoLimit]\r\n                  ,[FemaleHiLimit]\r\n                  ,[FemaleLoLimit]\r\n                  ,[Is_SymMultiValue]\r\n                  ,[InputCode]\r\n                  ,[DispOrder]\r\n                  ,[Note]\r\n                  ,[Forsex]\r\n              FROM [BusExamItem]\r\n             WHERE ID_ExamItem IN (\r\n            SELECT ID_ExamItem FROM BusFeeDetail WHERE ID_Fee = @ID_Fee ) "
		};

		protected string[] QuerySingleExamItem_Param = new string[]
		{
			"SELECT ID_ExamItem\r\n                    ,ExamItemName\r\n                    ,GetResultWay\r\n                    ,ExamItemCode\r\n                    ,BusExamItem.ID_Section\r\n                    ,SYSSection.SectionName\r\n                    ,Is_LisValueNull\r\n                    ,Is_EntrySectSum\r\n                    ,EntrySectSumLevel\r\n                    ,Is_AutoCalc\r\n                    ,CalcExpression\r\n                    ,SymCols\r\n                    ,TextboxRows\r\n                    ,Is_SameRow\r\n                    ,ExamItemUnit\r\n                    ,MaleHiLimit\r\n                    ,MaleLoLimit\r\n                    ,FemaleHiLimit\r\n                    ,FemaleLoLimit\r\n                    ,Is_SymMultiValue\r\n                    ,Is_ExamItemNonPrintInReport\r\n                    ,BusExamItem.InputCode\r\n                    ,BusExamItem.DispOrder\r\n                    ,BusExamItem.Note\r\n                    ,BusExamItem.AbbrExamName\r\n                    ,Forsex\r\n                FROM BusExamItem,SYSSection\r\n               WHERE BusExamItem.ID_Section = SYSSection.ID_Section \r\n                 AND ID_ExamItem = @ID_ExamItem ;\r\n          "
		};

		protected string[] QuerySingleFeeItem_Param = new string[]
		{
			"SELECT ID_Fee\r\n              ,ID_Section\r\n              ,SectionName\r\n              ,ID_Specimen\r\n              ,SpecimenName\r\n              ,FeeName\r\n              ,Forsex\r\n              ,ReportFeeName\r\n              ,FeeCode\r\n              ,cast(round(Price,2) as numeric(10,2)) Price\r\n              ,BusFee.InputCode\r\n              ,WorkGroupCode\r\n              ,WorkStationCode\r\n              ,WorkBenchCode\r\n              ,CreateDate\r\n              ,Is_Banned\r\n              ,ID_BanOpr\r\n              ,BanDescribe\r\n              ,DispOrder\r\n              ,Note\r\n              ,BreakfastOrder\r\n          FROM BusFee\r\n         WHERE ID_Fee = @ID_Fee ;\r\n          "
		};

		protected string[] QueryExamDetailListByExamItemGroup_Param = new string[]
		{
			"SELECT [ID_ExamItem]\r\n                  ,[ExamItemName]\r\n                  ,[GetResultWay]\r\n                  ,[ExamItemCode]\r\n                  ,[ID_Section]\r\n                  ,[Is_LisValueNull]\r\n                  ,[Is_EntrySectSum]\r\n                  ,[EntrySectSumLevel]\r\n                  ,[Is_AutoCalc]\r\n                  ,[CalcExpression]\r\n                  ,[SymCols]\r\n                  ,[TextboxRows]\r\n                  ,[Is_SameRow]\r\n                  ,[ExamItemUnit]\r\n                  ,[MaleHiLimit]\r\n                  ,[MaleLoLimit]\r\n                  ,[FemaleHiLimit]\r\n                  ,[FemaleLoLimit]\r\n                  ,[Is_SymMultiValue]\r\n                  ,[InputCode]\r\n                  ,[DispOrder]\r\n                  ,[Note]\r\n                  ,[Forsex]\r\n              FROM [BusExamItem]\r\n             WHERE ID_ExamItemGroup = @ID_ExamItemGroup "
		};

		protected string[] GetArcCustomerInfoByIDs_Param = new string[]
		{
			"SELECT [ID_ArcCustomer]\r\n              ,[ID_Gender]\r\n              ,[ID_Marriage]\r\n              ,[NationID]\r\n              ,[CultrulID]\r\n              ,[VocationID]\r\n              ,[CustomerName]\r\n              ,[IDCard]\r\n              ,[ExamCard]\r\n              ,[Photo]\r\n              ,convert(varchar(20),BirthDay,23) BirthDay \r\n              ,[GenderName]\r\n              ,[MarriageName]\r\n              ,[NationName]\r\n              ,[Address]\r\n              ,[MobileNo]\r\n              ,[Email]\r\n              ,[CultrulName]\r\n              ,[VocationName]\r\n              ,ISNULL(FinishedNum,0) FinishedNum\r\n              ,ISNULL(UnfinishedNum,0) UnfinishedNum\r\n              ,ISNULL(FinishedNum,0) + ISNULL(UnfinishedNum,0) TotalNum\r\n              ,convert(varchar(20),FirstDatePE,23) FirstDatePE \r\n              ,convert(varchar(20),BirthDay,23) LatestDatePE \r\n              FROM OnArcCust\r\n              WHERE ID_ArcCustomer in (@ID_ArcCustomers) ;\r\n\r\n            SELECT [ID_CustRelation]\r\n                ,[ID_ArcCustomer]\r\n                ,[IDCardNo]\r\n                ,[ExamCardNo]\r\n                ,[ID_Customer]\r\n                ,[Is_CompletePhysical]\r\n                ,[ExamState]\r\n            FROM OnCustRelationCustPEInfo\r\n            WHERE ID_ArcCustomer in (@ID_ArcCustomers) \r\n\r\n            "
		};

		protected string[] SearchArcCustomerList_Param = new string[]
		{
			"SELECT [ID_ArcCustomer]\r\n              ,[ID_Gender]\r\n              ,[ID_Marriage]\r\n              ,[NationID]\r\n              ,[CultrulID]\r\n              ,[VocationID]\r\n              ,[CustomerName]\r\n              ,[IDCard]\r\n              ,[ExamCard]\r\n              ,[Photo]\r\n              ,convert(varchar(20),BirthDay,23) BirthDay \r\n              ,[GenderName]\r\n              ,[MarriageName]\r\n              ,[NationName]\r\n              ,[Address]\r\n              ,[MobileNo]\r\n              ,[Email]\r\n              ,[CultrulName]\r\n              ,[VocationName]\r\n              ,ISNULL(FinishedNum,0) FinishedNum\r\n              ,ISNULL(UnfinishedNum,0) UnfinishedNum\r\n              ,ISNULL(FinishedNum,0) + ISNULL(UnfinishedNum,0) TotalNum\r\n              ,convert(varchar(20),FirstDatePE,23) FirstDatePE \r\n              ,convert(varchar(20),BirthDay,23) LatestDatePE \r\n              FROM OnArcCust\r\n              WHERE ID_Gender = @ID_Gender \r\n            AND CustomerName = @CustomerName\r\n             "
		};

		protected string[] SearchArcCustomerList_BirthDay_Param = new string[]
		{
			"SELECT [ID_ArcCustomer]\r\n              ,[ID_Gender]\r\n              ,[ID_Marriage]\r\n              ,[NationID]\r\n              ,[CultrulID]\r\n              ,[VocationID]\r\n              ,[CustomerName]\r\n              ,[IDCard]\r\n              ,[ExamCard]\r\n              ,[Photo]\r\n              ,convert(varchar(20),BirthDay,23) BirthDay \r\n              ,[GenderName]\r\n              ,[MarriageName]\r\n              ,[NationName]\r\n              ,[Address]\r\n              ,[MobileNo]\r\n              ,[Email]\r\n              ,[CultrulName]\r\n              ,[VocationName]\r\n              ,ISNULL(FinishedNum,0) FinishedNum\r\n              ,ISNULL(UnfinishedNum,0) UnfinishedNum\r\n              ,ISNULL(FinishedNum,0) + ISNULL(UnfinishedNum,0) TotalNum\r\n              ,convert(varchar(20),FirstDatePE,23) FirstDatePE \r\n              ,convert(varchar(20),BirthDay,23) LatestDatePE \r\n              FROM OnArcCust\r\n              WHERE ID_Gender = @ID_Gender \r\n                AND CustomerName = @CustomerName\r\n                AND BirthDay = @BirthDay;\r\n             "
		};

		protected string[] SearchArcCustomerList_IDCard_Param = new string[]
		{
			"SELECT [ID_ArcCustomer]\r\n              ,[ID_Gender]\r\n              ,[ID_Marriage]\r\n              ,[NationID]\r\n              ,[CultrulID]\r\n              ,[VocationID]\r\n              ,[CustomerName]\r\n              ,[IDCard]\r\n              ,[ExamCard]\r\n              ,[Photo]\r\n              ,convert(varchar(20),BirthDay,23) BirthDay \r\n              ,[GenderName]\r\n              ,[MarriageName]\r\n              ,[NationName]\r\n              ,[Address]\r\n              ,[MobileNo]\r\n              ,[Email]\r\n              ,[CultrulName]\r\n              ,[VocationName]\r\n              ,ISNULL(FinishedNum,0) FinishedNum\r\n              ,ISNULL(UnfinishedNum,0) UnfinishedNum\r\n              ,ISNULL(FinishedNum,0) + ISNULL(UnfinishedNum,0) TotalNum\r\n              ,convert(varchar(20),FirstDatePE,23) FirstDatePE \r\n              ,convert(varchar(20),BirthDay,23) LatestDatePE \r\n              FROM OnArcCust\r\n              WHERE ID_Gender = @ID_Gender \r\n                AND CustomerName = @CustomerName\r\n                AND IDCard = @IDCard;\r\n             "
		};

		protected string[] SearchArcCustomerList_BirthDay_IDCard_Param = new string[]
		{
			"SELECT [ID_ArcCustomer]\r\n              ,[ID_Gender]\r\n              ,[ID_Marriage]\r\n              ,[NationID]\r\n              ,[CultrulID]\r\n              ,[VocationID]\r\n              ,[CustomerName]\r\n              ,[IDCard]\r\n              ,[ExamCard]\r\n              ,[Photo]\r\n              ,convert(varchar(20),BirthDay,23) BirthDay \r\n              ,[GenderName]\r\n              ,[MarriageName]\r\n              ,[NationName]\r\n              ,[Address]\r\n              ,[MobileNo]\r\n              ,[Email]\r\n              ,[CultrulName]\r\n              ,[VocationName]\r\n              ,ISNULL(FinishedNum,0) FinishedNum\r\n              ,ISNULL(UnfinishedNum,0) UnfinishedNum\r\n              ,ISNULL(FinishedNum,0) + ISNULL(UnfinishedNum,0) TotalNum\r\n              ,convert(varchar(20),FirstDatePE,23) FirstDatePE \r\n              ,convert(varchar(20),BirthDay,23) LatestDatePE \r\n              FROM OnArcCust\r\n              WHERE ID_Gender = @ID_Gender \r\n                AND CustomerName = @CustomerName\r\n                AND BirthDay = @BirthDay\r\n                AND IDCard = @IDCard;\r\n             "
		};

		protected string[] QuerySymptonDetailListByExamID_Param = new string[]
		{
			"SELECT BusConclusion.ConclusionName,BusSymptom.* FROM (\r\n            SELECT ID_Symptom\r\n                  ,ID_ExamItem\r\n                  ,ID_Conclusion\r\n                  ,SymptomName\r\n                  ,SymptomDescribe\r\n                  ,DiseaseLevel\r\n                  ,Is_Default\r\n                  ,NumOperSign\r\n                  ,NumMale\r\n                  ,NumFemale\r\n                  ,InputCode\r\n                  ,DispOrder,BusSymptom.Is_Banned,BusSymptom.BanDate,BusSymptom.BanOperator\r\n              FROM BusSymptom\r\n              WHERE ID_ExamItem = @ID_ExamItem ) \r\n              BusSymptom LEFT JOIN BusConclusion \r\n              ON BusSymptom.ID_Conclusion = BusConclusion.ID_Conclusion \r\n            ORDER BY DispOrder ASC ; \r\n             "
		};

		protected string[] QuerySymptomNameIsExis_Param = new string[]
		{
			"SELECT ID_Symptom\r\n              FROM BusSymptom\r\n              WHERE ID_ExamItem = @ID_ExamItem\r\n              AND SymptomName = @SymptomName;\r\n          "
		};

		protected string[] QueryQuickSpecimenList_Param = new string[]
		{
			"SELECT [ID_Specimen]\r\n              ,[SpecimenName]\r\n              ,[InputCode]\r\n              ,[DispOrder]\r\n          FROM [BusSpecimen]\r\n      ORDER BY [DispOrder] ASC;\r\n          "
		};

		protected string[] QueryQuickFeeReportMergerList_Param = new string[]
		{
			"SELECT [ID_Fee]\r\n              ,[FeeName]\r\n              ,[ReportFeeName] FeeReportMergerName\r\n              ,[ID_Fee] ID_FeeReportMerger\r\n              ,[InputCode]\r\n              ,[DispOrder]\r\n          FROM [BusFee]\r\n         WHERE ID_Fee = ID_FeeReportMerger\r\n      ORDER BY [DispOrder] ASC;\r\n          "
		};

		protected string[] QueryQuickConclusionTypeList_Param = new string[]
		{
			"SELECT [ID_ConclusionType]\r\n              ,[ConclusionTypeName]\r\n              ,[InputCode], ISNULL(Is_Banned,0) Is_Banned\r\n          FROM [BusConclusionType]\r\n      ORDER BY [ConclusionTypeName] ASC;\r\n          "
		};

		protected string[] QueryQuickFinalConclusionTypeList_Param = new string[]
		{
			"SELECT [ID_FinalConclusionType]\r\n              ,[FinalConclusionTypeName]\r\n              ,[FinalConclusionSignCode]\r\n              ,[InputCode]\r\n              ,[DispOrder]\r\n              ,ISNULL(Is_Banned,0) Is_Banned\r\n          FROM [DctFinalConclusionType]\r\n      ORDER BY [DispOrder] ASC,[FinalConclusionTypeName] ASC;\r\n          "
		};

		protected string[] QueryQuickICD10List_Param = new string[]
		{
			"SELECT [ID_ICD]\r\n          ,[ICDCNName]\r\n          ,[ICDENName]\r\n          ,[Code]\r\n          ,[Codea]\r\n          ,[ID_Creator]\r\n          ,[Creator]\r\n          ,[CreateDate]\r\n          ,[Is_Banned]\r\n          ,[ID_BanOpr]\r\n          ,[BanOperator]\r\n          ,[BanDate]\r\n          ,[BanDescribe]\r\n          ,[LevelA]\r\n          ,[LevelB]\r\n          ,[LevelC]\r\n          ,[LevelD]\r\n          ,[LevelE]\r\n          ,[LevelTree]\r\n          ,[Class]\r\n          ,[Tag]\r\n          ,[ICDtoSection]\r\n          ,[Note]\r\n          ,[InputCode], ISNULL(Is_Banned,0) Is_Banned\r\n          FROM [DctICDTen]\r\n      ORDER BY [ICDCNName] ASC;\r\n          "
		};

		protected string[] QueryQuickConclusionList_Param = new string[]
		{
			"SELECT [ID_Conclusion]\r\n              ,[ConclusionName]\r\n              ,[InputCode]\r\n              ,[DispOrder]\r\n          FROM [BusConclusion]\r\n      ORDER BY [DispOrder] ASC;\r\n          "
		};

		protected string[] QueryQuickExamItemList_Param = new string[]
		{
			"SELECT ID_ExamItem\r\n          ,ExamItemName\r\n          ,BusExamItem.InputCode\r\n          ,BusExamItem.DispOrder\r\n          ,BusExamItem.ExamItemCode\r\n          ,BusExamItem.ID_Section\r\n          ,REPLACE(BusExamItem.Note,'''','‘') Note\r\n          ,SYSSection.SectionName\r\n      FROM BusExamItem,SYSSection \r\n      WHERE BusExamItem.ID_Section = SYSSection.ID_Section\r\n  ORDER BY BusExamItem.DispOrder ASC;\r\n          "
		};

		protected string[] QueryQuickNationList_Param = new string[]
		{
			"SELECT [NationID]\r\n              ,[NationName]\r\n              ,[InputCode]\r\n          FROM [DictNation]\r\n      ORDER BY [NationID] ASC;\r\n          "
		};

		protected string[] QueryQuickExamTypeList_Param = new string[]
		{
            "SELECT [ExamTypeID]\r\n              ,[ExamTypeName]\r\n              ,[InputCode]\r\n          FROM [DictExamType]\r\n      ORDER BY [ExamTypeID] ASC;\r\n          "
        };

		protected string[] QueryExamTypeNameIsExis_Param = new string[]
		{
            "SELECT [ExamTypeID]\r\n          FROM [DictExamType]\r\n          WHERE ExamTypeName = @ExamTypeName; \r\n             "
        };

		protected string[] QueryPagesExamTypeList_Param = new string[]
		{
            "ExamTypeID",
			"*",
            " ( SELECT * FROM  [DictExamType]  ) ",
			" "
		};

		protected string[] QueryPagesExamTypeListByName_Param = new string[]
		{
            "ExamTypeID",
			"*",
            " ( SELECT *\r\n            FROM  [DictExamType] \r\n            WHERE ExamTypeName LIKE @ExamTypeName OR [InputCode] LIKE @ExamTypeName\r\n            ) ",
			"  "
		};

		protected string[] QuerySingleExamTypeInfo_Param = new string[]
		{
            "SELECT *\r\n            FROM [DictExamType]\r\n           WHERE ExamTypeID = @ExamTypeID; \r\n             "
        };

		protected string[] QueryQuickSetList_Param = new string[]
		{
            "SELECT [PEPackageID]\r\n              ,[PEPackageName]\r\n              ,[InputCode]\r\n              ,[Forsex]\r\n              ,[DispOrder]\r\n          FROM [BusPEPackage]\r\n         WHERE [isBanned] = 0 or [isBanned] is null \r\n      ORDER BY [DispOrder] ASC;\r\n          "
        };

		protected string[] QueryFeeDetailListBySet_Param = new string[]
		{
            "SELECT *\r\n              FROM [BusFee]\r\n             WHERE ID_Fee IN (\r\n            SELECT ID_FeeItem FROM BusSetFeeDetail WHERE ID_Set = @PEPackageID ) "
        };

		protected string[] QueryPEPackageNameIsExis_Param = new string[]
		{
            "SELECT [PEPackageID] \r\n          FROM [BusPEPackage] \r\n          WHERE PEPackageName = @PEPackageName;  "
        };

		protected string[] QuerySingleBusSet_Param = new string[]
		{
            "SELECT *\r\n          FROM BusPEPackage \r\n         WHERE PEPackageID = @PEPackageID ;\r\n          "
        };

		protected string[] QueryPagesSpecimenList_Param = new string[]
		{
			"ID_Specimen",
			"*",
			" ( SELECT * FROM  [BusSpecimen] ) ",
			" ORDER BY DispOrder ASC "
		};

		protected string[] QueryPagesSpecimenListByName_Param = new string[]
		{
			"ID_Specimen",
			"*",
			" ( SELECT *\r\n            FROM  [BusSpecimen] \r\n            WHERE SpecimenName LIKE @SpecimenName OR [InputCode] LIKE @SpecimenName \r\n            ) ",
			"  ORDER BY DispOrder ASC "
		};

		protected string[] QuerySpecimenNameIsExis_Param = new string[]
		{
			"SELECT [ID_Specimen]\r\n          FROM [BusSpecimen]\r\n          WHERE SpecimenName = @SpecimenName; \r\n             "
		};

		protected string[] QuerySingleSpecimenInfo_Param = new string[]
		{
			"SELECT *\r\n            FROM [BusSpecimen]\r\n           WHERE ID_Specimen = @ID_Specimen; \r\n             "
		};

		protected string[] QueryPagesExamPlaceList_Param = new string[]
		{
			"ExamPlaceID",
			"*",
			" ( SELECT * FROM  [DictExamPlace]  ) ",
			" "
		};

		protected string[] QueryPagesExamPlaceListByName_Param = new string[]
		{
			"ExamPlaceID",
			"*",
			" ( SELECT *\r\n            FROM  [DictExamPlace] \r\n            WHERE ExamPlaceName LIKE @ExamPlaceName OR [InputCode] LIKE @ExamPlaceName\r\n            ) ",
			"  "
		};

		protected string[] QueryExamPlaceNameIsExis_Param = new string[]
		{
			"SELECT [ExamPlaceID]\r\n          FROM [DictExamPlace]\r\n          WHERE ExamPlaceName = @ExamPlaceName; \r\n             "
		};

		protected string[] QuerySingleExamPlaceInfo_Param = new string[]
		{
			"SELECT *\r\n            FROM [DictExamPlace]\r\n           WHERE ExamPlaceID = @ExamPlaceID; \r\n             "
		};

		protected string[] QueryPagesConclusionTypeList_Param = new string[]
		{
			"ID_ConclusionType",
			"*",
			" ( SELECT [ID_ConclusionType]\r\n                          ,[ConclusionTypeName]\r\n                          ,[InputCode]\r\n                          ,[Is_Banned]\r\n                          ,[ID_BanOpr]\r\n                          ,[BanOperator]\r\n                          ,[BanDate]\r\n                          ,[BanDescribe] FROM  [BusConclusionType]  ) ",
			" "
		};

		protected string[] QueryPagesConclusionTypeListByName_Param = new string[]
		{
			"ID_ConclusionType",
			"*",
			" ( SELECT [ID_ConclusionType]\r\n                          ,[ConclusionTypeName]\r\n                          ,[InputCode]\r\n                          ,[Is_Banned]\r\n                          ,[ID_BanOpr]\r\n                          ,[BanOperator]\r\n                          ,[BanDate]\r\n                          ,[BanDescribe] \r\n            FROM  [BusConclusionType] \r\n            WHERE ConclusionTypeName LIKE @ConclusionTypeName OR [InputCode] LIKE @ConclusionTypeName\r\n            ) ",
			"  "
		};

		protected string[] QueryConclusionTypeNameIsExis_Param = new string[]
		{
			"SELECT [ID_ConclusionType]\r\n          FROM [BusConclusionType]\r\n          WHERE ConclusionTypeName = @ConclusionTypeName; \r\n             "
		};

		protected string[] QuerySingleConclusionTypeInfo_Param = new string[]
		{
			"SELECT [ID_ConclusionType]\r\n                ,[ConclusionTypeName]\r\n                ,[InputCode]\r\n                ,[Is_Banned]\r\n                ,[ID_BanOpr]\r\n                ,[BanOperator]\r\n                ,[BanDate]\r\n                ,[BanDescribe] \r\n            FROM [BusConclusionType]\r\n           WHERE ID_ConclusionType = @ID_ConclusionType; \r\n             "
		};

		protected string[] QueryPagesFinalConclusionTypeList_Param = new string[]
		{
			"ID_FinalConclusionType",
			"*",
			" ( SELECT [ID_FinalConclusionType]\r\n                          ,[FinalConclusionTypeName]\r\n                          ,[InputCode]\r\n                          ,[DispOrder]\r\n                          ,[Is_Banned]\r\n                          ,[ID_BanOpr]\r\n                          ,[BanOperator]\r\n                          ,[BanDate]\r\n                          ,[BanDescribe]\r\n                          ,[FinalConclusionSignCode] FROM  [DctFinalConclusionType]  ) ",
			" ORDER BY [DispOrder] ASC "
		};

		protected string[] QueryPagesFinalConclusionTypeListByName_Param = new string[]
		{
			"ID_FinalConclusionType",
			"*",
			" ( SELECT [ID_FinalConclusionType]\r\n                          ,[FinalConclusionTypeName]\r\n                          ,[InputCode]\r\n                          ,[DispOrder]\r\n                          ,[Is_Banned]\r\n                          ,[ID_BanOpr]\r\n                          ,[BanOperator]\r\n                          ,[BanDate]\r\n                          ,[BanDescribe] \r\n                          ,[FinalConclusionSignCode]\r\n            FROM  [DctFinalConclusionType] \r\n            WHERE FinalConclusionTypeName LIKE @FinalConclusionTypeName OR [InputCode] LIKE @FinalConclusionTypeName\r\n            ) ",
			"  "
		};

		protected string[] QueryFinalConclusionTypeNameIsExis_Param = new string[]
		{
			"SELECT [ID_FinalConclusionType]\r\n          FROM [DctFinalConclusionType]\r\n          WHERE FinalConclusionTypeName = @FinalConclusionTypeName; \r\n             "
		};

		protected string[] QuerySingleFinalConclusionTypeInfo_Param = new string[]
		{
			"SELECT [ID_FinalConclusionType]\r\n                ,[FinalConclusionTypeName]\r\n                ,[InputCode]\r\n                ,[DispOrder]\r\n                ,[Is_Banned]\r\n                ,[ID_BanOpr]\r\n                ,[BanOperator]\r\n                ,[BanDate]\r\n                ,[BanDescribe] \r\n                ,[FinalConclusionSignCode]\r\n            FROM [DctFinalConclusionType]\r\n           WHERE ID_FinalConclusionType = @ID_FinalConclusionType; \r\n             "
		};

		protected string[] QueryICDCNNameIsExis_Param = new string[]
		{
			"SELECT [ID_ICD]\r\n          FROM [DctICDTen]\r\n          WHERE ICDCNName = @ICDCNName; \r\n             "
		};

		protected string[] QueryPagesICDList_Param = new string[]
		{
			"ID_ICD",
			"*",
			" ( SELECT [ID_ICD]\r\n      ,[ICDCNName]\r\n      ,[ICDENName]\r\n      ,[Code]\r\n      ,[Codea]\r\n      ,[ID_Creator]\r\n      ,[Creator]\r\n      ,[CreateDate]\r\n      ,[Is_Banned]\r\n      ,[ID_BanOpr]\r\n      ,[BanOperator]\r\n      ,[BanDate]\r\n      ,[BanDescribe]\r\n      ,[LevelA]\r\n      ,[LevelB]\r\n      ,[LevelC]\r\n      ,[LevelD]\r\n      ,[LevelE]\r\n      ,[LevelTree]\r\n      ,[Class]\r\n      ,[Tag]\r\n      ,[ICDtoSection]\r\n      ,[Note]\r\n      ,[InputCode] FROM  [DctICDTen] ) ",
			" "
		};

		protected string[] QueryPagesICDListByName_Param = new string[]
		{
			"ID_ICD",
			"*",
			" ( SELECT [ID_ICD]\r\n      ,[ICDCNName]\r\n      ,[ICDENName]\r\n      ,[Code]\r\n      ,[Codea]\r\n      ,[ID_Creator]\r\n      ,[Creator]\r\n      ,[CreateDate]\r\n      ,[Is_Banned]\r\n      ,[ID_BanOpr]\r\n      ,[BanOperator]\r\n      ,[BanDate]\r\n      ,[BanDescribe]\r\n      ,[LevelA]\r\n      ,[LevelB]\r\n      ,[LevelC]\r\n      ,[LevelD]\r\n      ,[LevelE]\r\n      ,[LevelTree]\r\n      ,[Class]\r\n      ,[Tag]\r\n      ,[ICDtoSection]\r\n      ,[Note]\r\n      ,[InputCode] FROM  [DctICDTen] \r\n            WHERE ICDCNName LIKE @ICDCNName OR ICDENName LIKE @ICDCNName OR [InputCode] LIKE @ICDCNName OR [Code] LIKE @ICDCNName OR [Codea] LIKE @ICDCNName\r\n            ) ",
			"  "
		};

		protected string[] QuerySingleICDInfo_Param = new string[]
		{
			"SELECT [ID_ICD]\r\n      ,[ICDCNName]\r\n      ,[ICDENName]\r\n      ,[Code]\r\n      ,[Codea]\r\n      ,[ID_Creator]\r\n      ,[Creator]\r\n      ,[CreateDate]\r\n      ,[Is_Banned]\r\n      ,[ID_BanOpr]\r\n      ,[BanOperator]\r\n      ,[BanDate]\r\n      ,[BanDescribe]\r\n      ,[LevelA]\r\n      ,[LevelB]\r\n      ,[LevelC]\r\n      ,[LevelD]\r\n      ,[LevelE]\r\n      ,[LevelTree]\r\n      ,[Class]\r\n      ,[Tag]\r\n      ,[ICDtoSection]\r\n      ,[Note]\r\n      ,[InputCode]\r\n            FROM [DctICDTen]\r\n           WHERE ID_ICD = @ID_ICD; \r\n             "
		};

		protected string[] QuerySingleNatRole_Param = new string[]
		{
            "SELECT *\r\n            FROM [SYSRole]\r\n           WHERE RoleID = @RoleID; \r\n             "
        };

		protected string[] QuerySingleFeeReportInfo_Param = new string[]
		{
			"SELECT BFR.*,BF.ID_Section,BF.SectionName,BF.DispOrder\r\n                  FROM BusFee BF,BusFeeReport BFR\r\n                  WHERE BF.ID_Fee = BFR.ID_Fee\r\n                  AND ID_FeeReport = @ID_FeeReport ;\r\n          "
		};

		protected string[] QueryFeeIsExis_Param = new string[]
		{
			"SELECT [ID_FeeReport]\r\n          FROM [BusFeeReport]\r\n          WHERE ID_Fee = @ID_Fee; \r\n             "
		};

		protected string[] QuerySingleExamItemGroupInfo_Param = new string[]
		{
			"SELECT * FROM BusExamItemGroup\r\n                  WHERE ID_ExamItemGroup = @ID_ExamItemGroup ;\r\n          "
		};

		protected string[] QueryExamItemGroupIsExis_Param = new string[]
		{
			"SELECT [ID_ExamItemGroup]\r\n          FROM [BusExamItemGroup]\r\n          WHERE ExamItemGroupName = @ExamItemGroupName; \r\n             "
		};

		DataTable ICommonConfig.GetPage(string pageCode, int pageIndex, int pageSize, out int recordCount, out int pageCount, params SqlConditionInfo[] conditions)
		{
			return base.GetPage(pageCode, pageIndex, pageSize, out recordCount, out pageCount, conditions);
		}

		DataSet ICommonConfig.ExcuteQuerySql(string QuerySqlCode, params SqlConditionInfo[] conditions)
		{
			return base.ExcuteQuerySql(QuerySqlCode, conditions);
		}

		int ICommonConfig.SaveFeeItem(PEIS.Model.BusFee busFeeModel)
		{
			int result;
			if (busFeeModel.ID_Fee > 0)
			{
				result = this.BusFeeUpdate(busFeeModel);
			}
			else
			{
				result = this.BusFeeAdd(busFeeModel);
			}
			return result;
		}

		private int BusFeeAdd(PEIS.Model.BusFee model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusFee(");
			stringBuilder.Append("ID_Section,ID_Specimen,FeeName,Forsex,ReportFeeName,FeeCode,Price,InputCode,SectionName,SpecimenName,WorkGroupCode,WorkStationCode,WorkBenchCode,CreateDate,Is_Banned,ID_BanOpr,BanDescribe,DispOrder,Note,BreakfastOrder,Is_FeeNonPrintInReport,InterfaceName,IS_FeeReportMerger,ID_FeeReportMerger,OperationalDate)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Section,@ID_Specimen,@FeeName,@Forsex,@ReportFeeName,@FeeCode,@Price,@InputCode,@SectionName,@SpecimenName,@WorkGroupCode,@WorkStationCode,@WorkBenchCode,@CreateDate,@Is_Banned,@ID_BanOpr,@BanDescribe,@DispOrder,@Note,@BreakfastOrder,@Is_FeeNonPrintInReport,@InterfaceName,@IS_FeeReportMerger,@ID_FeeReportMerger,@OperationalDate)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@ID_Specimen", SqlDbType.Int, 4),
				new SqlParameter("@FeeName", SqlDbType.VarChar, 50),
				new SqlParameter("@Forsex", SqlDbType.Int, 4),
				new SqlParameter("@ReportFeeName", SqlDbType.VarChar, 50),
				new SqlParameter("@FeeCode", SqlDbType.VarChar, 50),
				new SqlParameter("@Price", SqlDbType.Money, 8),
				new SqlParameter("@InputCode", SqlDbType.VarChar, 30),
				new SqlParameter("@SectionName", SqlDbType.VarChar, 20),
				new SqlParameter("@SpecimenName", SqlDbType.VarChar, 50),
				new SqlParameter("@WorkGroupCode", SqlDbType.VarChar, 20),
				new SqlParameter("@WorkStationCode", SqlDbType.VarChar, 20),
				new SqlParameter("@WorkBenchCode", SqlDbType.VarChar, 20),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar, 50),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar, 50),
				new SqlParameter("@BreakfastOrder", SqlDbType.Int, 4),
				new SqlParameter("@Is_FeeNonPrintInReport", SqlDbType.Bit, 1),
				new SqlParameter("@InterfaceName", SqlDbType.VarChar, 50),
				new SqlParameter("@IS_FeeReportMerger", SqlDbType.Bit, 1),
				new SqlParameter("@ID_FeeReportMerger", SqlDbType.Int, 4),
				new SqlParameter("@OperationalDate", SqlDbType.VarChar, 500)
			};
			array[0].Value = model.ID_Section;
			array[1].Value = model.ID_Specimen;
			array[2].Value = model.FeeName;
			array[3].Value = model.ForGender;
			array[4].Value = model.ReportFeeName;
			array[5].Value = model.FeeCode;
			array[6].Value = model.Price;
			array[7].Value = model.InputCode;
			array[8].Value = model.SectionName;
			array[9].Value = model.SpecimenName;
			array[10].Value = model.WorkGroupCode;
			array[11].Value = model.WorkStationCode;
			array[12].Value = model.WorkBenchCode;
			array[13].Value = model.CreateDate;
			array[14].Value = model.Is_Banned;
			array[15].Value = model.ID_BanOpr;
			array[16].Value = model.BanDescribe;
			array[17].Value = model.DispOrder;
			array[18].Value = model.Note;
			array[19].Value = model.BreakfastOrder;
			array[20].Value = model.Is_FeeNonPrintInReport;
			array[21].Value = model.InterfaceName;
			array[22].Value = model.IS_FeeReportMerger;
			array[23].Value = model.ID_FeeReportMerger;
			array[24].Value = model.OperationalDate;
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

		private int BusFeeUpdate(PEIS.Model.BusFee model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusFee set ");
			stringBuilder.Append("ID_Section=@ID_Section,");
			stringBuilder.Append("ID_Specimen=@ID_Specimen,");
			stringBuilder.Append("FeeName=@FeeName,");
			stringBuilder.Append("ForGender=@ForGender,");
			stringBuilder.Append("ReportFeeName=@ReportFeeName,");
			stringBuilder.Append("FeeCode=@FeeCode,");
			stringBuilder.Append("Price=@Price,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("SectionName=@SectionName,");
			stringBuilder.Append("SpecimenName=@SpecimenName,");
			stringBuilder.Append("WorkGroupCode=@WorkGroupCode,");
			stringBuilder.Append("WorkStationCode=@WorkStationCode,");
			stringBuilder.Append("WorkBenchCode=@WorkBenchCode,");
			stringBuilder.Append("CreateDate=@CreateDate,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_BanOpr=@ID_BanOpr,");
			stringBuilder.Append("BanDescribe=@BanDescribe,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("BreakfastOrder=@BreakfastOrder,");
			stringBuilder.Append("Is_FeeNonPrintInReport=@Is_FeeNonPrintInReport,");
			stringBuilder.Append("InterfaceName=@InterfaceName,");
			stringBuilder.Append("IS_FeeReportMerger=@IS_FeeReportMerger,");
			stringBuilder.Append("ID_FeeReportMerger=@ID_FeeReportMerger,");
			stringBuilder.Append("OperationalDate=@OperationalDate");
			stringBuilder.Append(" where ID_Fee=@ID_Fee");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@ID_Specimen", SqlDbType.Int, 4),
				new SqlParameter("@FeeName", SqlDbType.VarChar, 50),
				new SqlParameter("@ForGender", SqlDbType.Int, 4),
				new SqlParameter("@ReportFeeName", SqlDbType.VarChar, 50),
				new SqlParameter("@FeeCode", SqlDbType.VarChar, 50),
				new SqlParameter("@Price", SqlDbType.Money, 8),
				new SqlParameter("@InputCode", SqlDbType.VarChar, 30),
				new SqlParameter("@SectionName", SqlDbType.VarChar, 20),
				new SqlParameter("@SpecimenName", SqlDbType.VarChar, 50),
				new SqlParameter("@WorkGroupCode", SqlDbType.VarChar, 20),
				new SqlParameter("@WorkStationCode", SqlDbType.VarChar, 20),
				new SqlParameter("@WorkBenchCode", SqlDbType.VarChar, 20),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar, 50),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar, 50),
				new SqlParameter("@BreakfastOrder", SqlDbType.Int, 4),
				new SqlParameter("@Is_FeeNonPrintInReport", SqlDbType.Bit, 1),
				new SqlParameter("@InterfaceName", SqlDbType.VarChar, 50),
				new SqlParameter("@IS_FeeReportMerger", SqlDbType.Bit, 1),
				new SqlParameter("@ID_FeeReportMerger", SqlDbType.Int, 4),
				new SqlParameter("@OperationalDate", SqlDbType.VarChar, 500),
				new SqlParameter("@ID_Fee", SqlDbType.Int, 4)
			};
			array[0].Value = model.ID_Section;
			array[1].Value = model.ID_Specimen;
			array[2].Value = model.FeeName;
			array[3].Value = model.ForGender;
			array[4].Value = model.ReportFeeName;
			array[5].Value = model.FeeCode;
			array[6].Value = model.Price;
			array[7].Value = model.InputCode;
			array[8].Value = model.SectionName;
			array[9].Value = model.SpecimenName;
			array[10].Value = model.WorkGroupCode;
			array[11].Value = model.WorkStationCode;
			array[12].Value = model.WorkBenchCode;
			array[13].Value = model.CreateDate;
			array[14].Value = model.Is_Banned;
			array[15].Value = model.ID_BanOpr;
			array[16].Value = model.BanDescribe;
			array[17].Value = model.DispOrder;
			array[18].Value = model.Note;
			array[19].Value = model.BreakfastOrder;
			array[20].Value = model.Is_FeeNonPrintInReport;
			array[21].Value = model.InterfaceName;
			array[22].Value = model.IS_FeeReportMerger;
			array[23].Value = model.ID_FeeReportMerger;
			array[24].Value = model.OperationalDate;
			array[25].Value = model.ID_Fee;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveSection(PEIS.Model.SYSSection SectionModel)
		{
			int result;
			if (SectionModel.SectionID > 0)
			{
				result = this.SYSSectionUpdate(SectionModel);
			}
			else
			{
				result = this.SYSSectionAdd(SectionModel);
			}
			return result;
		}

		private int SYSSectionAdd(PEIS.Model.SYSSection model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SYSSection(");
            strSql.Append("SectionName,FunctionType,DisplayMenu,IsDel,InterfaceType,IsNotSamePage,IsNonPrintSectSummary,IsOwnInterface,IsAutoApprove,ImageType,PicPrintSetup,PacsInterfaceFlag,InputCode,DispOrder,SummaryName,DefaultSummary,SepBetweenExamItems,SepBetweenSymptoms,TerminalSymbol,SepExamAndValue,NoBetweenExamItems,NoBetweenSympotms,Note,IsNoEntryFinalSummary,IsNonPrintInReport,IsPrintBarCode)");
            strSql.Append(" values (");
            strSql.Append("@SectionName,@FunctionType,@DisplayMenu,@IsDel,@InterfaceType,@IsNotSamePage,@IsNonPrintSectSummary,@IsOwnInterface,@IsAutoApprove,@ImageType,@PicPrintSetup,@PacsInterfaceFlag,@InputCode,@DispOrder,@SummaryName,@DefaultSummary,@SepBetweenExamItems,@SepBetweenSymptoms,@TerminalSymbol,@SepExamAndValue,@NoBetweenExamItems,@NoBetweenSympotms,@Note,@IsNoEntryFinalSummary,@IsNonPrintInReport,@IsPrintBarCode)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SectionName", SqlDbType.VarChar,20),
                    new SqlParameter("@FunctionType", SqlDbType.Bit,1),
                    new SqlParameter("@DisplayMenu", SqlDbType.VarChar,80),
                    new SqlParameter("@IsDel", SqlDbType.Bit,1),
                    new SqlParameter("@InterfaceType", SqlDbType.VarChar,8),
                    new SqlParameter("@IsNotSamePage", SqlDbType.Bit,1),
                    new SqlParameter("@IsNonPrintSectSummary", SqlDbType.Bit,1),
                    new SqlParameter("@IsOwnInterface", SqlDbType.Bit,1),
                    new SqlParameter("@IsAutoApprove", SqlDbType.Bit,1),
                    new SqlParameter("@ImageType", SqlDbType.VarChar,8),
                    new SqlParameter("@PicPrintSetup", SqlDbType.VarChar,8),
                    new SqlParameter("@PacsInterfaceFlag", SqlDbType.VarChar,8),
                    new SqlParameter("@InputCode", SqlDbType.VarChar,10),
                    new SqlParameter("@DispOrder", SqlDbType.Int,4),
                    new SqlParameter("@SummaryName", SqlDbType.VarChar,20),
                    new SqlParameter("@DefaultSummary", SqlDbType.Text),
                    new SqlParameter("@SepBetweenExamItems", SqlDbType.VarChar,20),
                    new SqlParameter("@SepBetweenSymptoms", SqlDbType.VarChar,20),
                    new SqlParameter("@TerminalSymbol", SqlDbType.VarChar,20),
                    new SqlParameter("@SepExamAndValue", SqlDbType.VarChar,20),
                    new SqlParameter("@NoBetweenExamItems", SqlDbType.VarChar,20),
                    new SqlParameter("@NoBetweenSympotms", SqlDbType.VarChar,20),
                    new SqlParameter("@Note", SqlDbType.VarChar,50),
                    new SqlParameter("@IsNoEntryFinalSummary", SqlDbType.Bit,1),
                    new SqlParameter("@IsNonPrintInReport", SqlDbType.Bit,1),
                    new SqlParameter("@IsPrintBarCode", SqlDbType.Int,4)};
            parameters[0].Value = model.SectionName;
            parameters[1].Value = model.FunctionType;
            parameters[2].Value = model.DisplayMenu;
            parameters[3].Value = model.IsDel;
            parameters[4].Value = model.InterfaceType;
            parameters[5].Value = model.IsNotSamePage;
            parameters[6].Value = model.IsNonPrintSectSummary;
            parameters[7].Value = model.IsOwnInterface;
            parameters[8].Value = model.IsAutoApprove;
            parameters[9].Value = model.ImageType;
            parameters[10].Value = model.PicPrintSetup;
            parameters[11].Value = model.PacsInterfaceFlag;
            parameters[12].Value = model.InputCode;
            parameters[13].Value = model.DispOrder;
            parameters[14].Value = model.SummaryName;
            parameters[15].Value = model.DefaultSummary;
            parameters[16].Value = model.SepBetweenExamItems;
            parameters[17].Value = model.SepBetweenSymptoms;
            parameters[18].Value = model.TerminalSymbol;
            parameters[19].Value = model.SepExamAndValue;
            parameters[20].Value = model.NoBetweenExamItems;
            parameters[21].Value = model.NoBetweenSympotms;
            parameters[22].Value = model.Note;
            parameters[23].Value = model.IsNoEntryFinalSummary;
            parameters[24].Value = model.IsNonPrintInReport;
            parameters[25].Value = model.IsPrintBarCode;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

		private int SYSSectionUpdate(PEIS.Model.SYSSection model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SYSSection set ");
            strSql.Append("SectionName=@SectionName,");
            strSql.Append("FunctionType=@FunctionType,");
            strSql.Append("DisplayMenu=@DisplayMenu,");
            strSql.Append("IsDel=@IsDel,");
            strSql.Append("InterfaceType=@InterfaceType,");
            strSql.Append("IsNotSamePage=@IsNotSamePage,");
            strSql.Append("IsNonPrintSectSummary=@IsNonPrintSectSummary,");
            strSql.Append("IsOwnInterface=@IsOwnInterface,");
            strSql.Append("IsAutoApprove=@IsAutoApprove,");
            strSql.Append("ImageType=@ImageType,");
            strSql.Append("PicPrintSetup=@PicPrintSetup,");
            strSql.Append("PacsInterfaceFlag=@PacsInterfaceFlag,");
            strSql.Append("InputCode=@InputCode,");
            strSql.Append("DispOrder=@DispOrder,");
            strSql.Append("SummaryName=@SummaryName,");
            strSql.Append("DefaultSummary=@DefaultSummary,");
            strSql.Append("SepBetweenExamItems=@SepBetweenExamItems,");
            strSql.Append("SepBetweenSymptoms=@SepBetweenSymptoms,");
            strSql.Append("TerminalSymbol=@TerminalSymbol,");
            strSql.Append("SepExamAndValue=@SepExamAndValue,");
            strSql.Append("NoBetweenExamItems=@NoBetweenExamItems,");
            strSql.Append("NoBetweenSympotms=@NoBetweenSympotms,");
            strSql.Append("Note=@Note,");
            strSql.Append("IsNoEntryFinalSummary=@IsNoEntryFinalSummary,");
            strSql.Append("IsNonPrintInReport=@IsNonPrintInReport,");
            strSql.Append("IsPrintBarCode=@IsPrintBarCode");
            strSql.Append(" where SectionID=@SectionID");
            SqlParameter[] parameters = {
                    new SqlParameter("@SectionName", SqlDbType.VarChar,20),
                    new SqlParameter("@FunctionType", SqlDbType.Bit,1),
                    new SqlParameter("@DisplayMenu", SqlDbType.VarChar,80),
                    new SqlParameter("@IsDel", SqlDbType.Bit,1),
                    new SqlParameter("@InterfaceType", SqlDbType.VarChar,8),
                    new SqlParameter("@IsNotSamePage", SqlDbType.Bit,1),
                    new SqlParameter("@IsNonPrintSectSummary", SqlDbType.Bit,1),
                    new SqlParameter("@IsOwnInterface", SqlDbType.Bit,1),
                    new SqlParameter("@IsAutoApprove", SqlDbType.Bit,1),
                    new SqlParameter("@ImageType", SqlDbType.VarChar,8),
                    new SqlParameter("@PicPrintSetup", SqlDbType.VarChar,8),
                    new SqlParameter("@PacsInterfaceFlag", SqlDbType.VarChar,8),
                    new SqlParameter("@InputCode", SqlDbType.VarChar,10),
                    new SqlParameter("@DispOrder", SqlDbType.Int,4),
                    new SqlParameter("@SummaryName", SqlDbType.VarChar,20),
                    new SqlParameter("@DefaultSummary", SqlDbType.Text),
                    new SqlParameter("@SepBetweenExamItems", SqlDbType.VarChar,20),
                    new SqlParameter("@SepBetweenSymptoms", SqlDbType.VarChar,20),
                    new SqlParameter("@TerminalSymbol", SqlDbType.VarChar,20),
                    new SqlParameter("@SepExamAndValue", SqlDbType.VarChar,20),
                    new SqlParameter("@NoBetweenExamItems", SqlDbType.VarChar,20),
                    new SqlParameter("@NoBetweenSympotms", SqlDbType.VarChar,20),
                    new SqlParameter("@Note", SqlDbType.VarChar,50),
                    new SqlParameter("@IsNoEntryFinalSummary", SqlDbType.Bit,1),
                    new SqlParameter("@IsNonPrintInReport", SqlDbType.Bit,1),
                    new SqlParameter("@IsPrintBarCode", SqlDbType.Int,4),
                    new SqlParameter("@SectionID", SqlDbType.Int,4)};
            parameters[0].Value = model.SectionName;
            parameters[1].Value = model.FunctionType;
            parameters[2].Value = model.DisplayMenu;
            parameters[3].Value = model.IsDel;
            parameters[4].Value = model.InterfaceType;
            parameters[5].Value = model.IsNotSamePage;
            parameters[6].Value = model.IsNonPrintSectSummary;
            parameters[7].Value = model.IsOwnInterface;
            parameters[8].Value = model.IsAutoApprove;
            parameters[9].Value = model.ImageType;
            parameters[10].Value = model.PicPrintSetup;
            parameters[11].Value = model.PacsInterfaceFlag;
            parameters[12].Value = model.InputCode;
            parameters[13].Value = model.DispOrder;
            parameters[14].Value = model.SummaryName;
            parameters[15].Value = model.DefaultSummary;
            parameters[16].Value = model.SepBetweenExamItems;
            parameters[17].Value = model.SepBetweenSymptoms;
            parameters[18].Value = model.TerminalSymbol;
            parameters[19].Value = model.SepExamAndValue;
            parameters[20].Value = model.NoBetweenExamItems;
            parameters[21].Value = model.NoBetweenSympotms;
            parameters[22].Value = model.Note;
            parameters[23].Value = model.IsNoEntryFinalSummary;
            parameters[24].Value = model.IsNonPrintInReport;
            parameters[25].Value = model.IsPrintBarCode;
            parameters[26].Value = model.SectionID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

		int ICommonConfig.SaveRole(PEIS.Model.SysRole RoleModel)
		{
			int result;
			if (RoleModel.RoleID > 0)
			{
				result = this.SysRoleUpdate(RoleModel);
			}
			else
			{
				result = this.SysRoleAdd(RoleModel);
			}
			return result;
		}

		private int SysRoleAdd(PEIS.Model.SysRole model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SysRole(");
            strSql.Append("RoleID,RoleName,DispOrder,Is_Locked,Remark,CreateDate,OperatorID,Is_DefaultRole)");
            strSql.Append(" values (");
            strSql.Append("@RoleID,@RoleName,@DispOrder,@Is_Locked,@Remark,@CreateDate,@OperatorID,@Is_DefaultRole)");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.Int,4),
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,30),
                    new SqlParameter("@DispOrder", SqlDbType.Int,4),
                    new SqlParameter("@Is_Locked", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.VarChar,200),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@OperatorID", SqlDbType.Int,4),
                    new SqlParameter("@Is_DefaultRole", SqlDbType.Int,4)};
            parameters[0].Value = model.RoleID;
            parameters[1].Value = model.RoleName;
            parameters[2].Value = model.DispOrder;
            parameters[3].Value = model.Is_Locked;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.OperatorID;
            parameters[7].Value = model.Is_DefaultRole;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

		private int SysRoleUpdate(PEIS.Model.SysRole model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysRole set ");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("DispOrder=@DispOrder,");
            strSql.Append("Is_Locked=@Is_Locked,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("CreateDate=@CreateDate,");
            strSql.Append("OperatorID=@OperatorID,");
            strSql.Append("Is_DefaultRole=@Is_DefaultRole");
            strSql.Append(" where RoleID=@RoleID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,30),
                    new SqlParameter("@DispOrder", SqlDbType.Int,4),
                    new SqlParameter("@Is_Locked", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.VarChar,200),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@OperatorID", SqlDbType.Int,4),
                    new SqlParameter("@Is_DefaultRole", SqlDbType.Int,4),
                    new SqlParameter("@RoleID", SqlDbType.Int,4)};
            parameters[0].Value = model.RoleName;
            parameters[1].Value = model.DispOrder;
            parameters[2].Value = model.Is_Locked;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.OperatorID;
            parameters[6].Value = model.Is_DefaultRole;
            parameters[7].Value = model.RoleID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

		int ICommonConfig.SaveExamPlace(PEIS.Model.DictExamPlace ExamPlaceModel)
		{
			int result;
			if (ExamPlaceModel.ExamPlaceID > 0)
			{
				result = this.DictExamPlaceUpdate(ExamPlaceModel);
			}
			else
			{
				result = this.DictExamPlaceAdd(ExamPlaceModel);
			}
			return result;
		}

		private int DictExamPlaceAdd(PEIS.Model.DictExamPlace model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into DictExamPlace(");
			stringBuilder.Append("ExamPlaceName,Default,InputCode)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ExamPlaceName,@Default,@InputCode)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamPlaceName", SqlDbType.VarChar),
				new SqlParameter("@Default", SqlDbType.Bit, 1),
				new SqlParameter("@InputCode", SqlDbType.VarChar)
			};
			array[0].Value = model.ExamPlaceName;
			array[1].Value = model.Default;
			array[2].Value = model.InputCode;
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

		private int DictExamPlaceUpdate(PEIS.Model.DictExamPlace model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update DictExamPlace set ");
			stringBuilder.Append("ExamPlaceName=@ExamPlaceName,");
			stringBuilder.Append("Default=@Default,");
			stringBuilder.Append("InputCode=@InputCode");
			stringBuilder.Append(" where ExamPlaceID=@ExamPlaceID");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamPlaceName", SqlDbType.VarChar),
				new SqlParameter("@Default", SqlDbType.Bit, 1),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@ExamPlaceID", SqlDbType.Int, 4)
			};
			array[0].Value = model.ExamPlaceName;
			array[1].Value = model.Default;
			array[2].Value = model.InputCode;
			array[3].Value = model.ExamPlaceID;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveSpecimen(PEIS.Model.BusSpecimen SpecimenModel)
		{
			int result;
			if (SpecimenModel.ID_Specimen > 0)
			{
				result = this.BusSpecimenUpdate(SpecimenModel);
			}
			else
			{
				result = this.BusSpecimenAdd(SpecimenModel);
			}
			return result;
		}

		private int BusSpecimenAdd(PEIS.Model.BusSpecimen model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusSpecimen(");
			stringBuilder.Append("SpecimenName,InputCode,DispOrder,LisSpecimenName)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@SpecimenName,@InputCode,@DispOrder,@LisSpecimenName)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@SpecimenName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@LisSpecimenName", SqlDbType.VarChar)
			};
			array[0].Value = model.SpecimenName;
			array[1].Value = model.InputCode;
			array[2].Value = model.DispOrder;
			array[3].Value = model.LisSpecimenName;
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

		private int BusSpecimenUpdate(PEIS.Model.BusSpecimen model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusSpecimen set ");
			stringBuilder.Append("SpecimenName=@SpecimenName,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("LisSpecimenName=@LisSpecimenName");
			stringBuilder.Append(" where ID_Specimen=@ID_Specimen");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@SpecimenName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@LisSpecimenName", SqlDbType.VarChar),
				new SqlParameter("@ID_Specimen", SqlDbType.Int, 4)
			};
			array[0].Value = model.SpecimenName;
			array[1].Value = model.InputCode;
			array[2].Value = model.DispOrder;
			array[3].Value = model.LisSpecimenName;
			array[4].Value = model.ID_Specimen;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveDictExamType(PEIS.Model.DictExamType ExamTypeModel)
		{
			int result;
			if (ExamTypeModel.ExamTypeID > 0)
			{
				result = this.ExamTypeUpdate(ExamTypeModel);
			}
			else
			{
				result = this.ExamTypeAdd(ExamTypeModel);
			}
			return result;
		}

		private int ExamTypeAdd(PEIS.Model.DictExamType model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into DictExamType(");
            strSql.Append("ExamTypeName,InputCode,DocumentID)");
            strSql.Append(" values (");
            strSql.Append("@ExamTypeName,@InputCode,@DocumentID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@ExamTypeName", SqlDbType.VarChar,50),
                    new SqlParameter("@InputCode", SqlDbType.VarChar,10),
                    new SqlParameter("@DocumentID", SqlDbType.Int,4)};
            parameters[0].Value = model.ExamTypeName;
            parameters[1].Value = model.InputCode;
            parameters[2].Value = model.DocumentID;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

		private int ExamTypeUpdate(PEIS.Model.DictExamType model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update DictExamType set ");
			stringBuilder.Append("ExamTypeName=@ExamTypeName,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("DocumentID=@DocumentID");
			stringBuilder.Append(" where ExamTypeID=@ExamTypeID");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamTypeName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DocumentID", SqlDbType.Int, 4),
				new SqlParameter("@ExamTypeID", SqlDbType.Int, 4)
			};
			array[0].Value = model.ExamTypeName;
			array[1].Value = model.InputCode;
			array[2].Value = model.DocumentID;
			array[3].Value = model.ExamTypeID;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveFeeExamRel(int ID_Fee, string newExamItemIDStrs)
		{
			int result;
			if (ID_Fee <= 0)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (string.IsNullOrEmpty(newExamItemIDStrs))
				{
					stringBuilder.Append(" DELETE FROM BusFeeDetail ");
					stringBuilder.Append(" WHERE ID_Fee = " + ID_Fee.ToString() + ";");
				}
				else
				{
					stringBuilder.Append(" DELETE FROM BusFeeDetail ");
					stringBuilder.Append(" WHERE ID_Fee = " + ID_Fee.ToString());
					stringBuilder.Append(" AND ID_ExamItem NOT IN  ( " + newExamItemIDStrs + "); ");
					stringBuilder.Append(" INSERT INTO BusFeeDetail  ");
					stringBuilder.Append(" SELECT " + ID_Fee.ToString() + " ID_Fee, ID_ExamItem ");
					stringBuilder.Append(" FROM BusExamItem  ");
					stringBuilder.Append(string.Concat(new string[]
					{
						" WHERE ID_ExamItem NOT IN ( SELECT ID_ExamItem FROM BusFeeDetail WHERE ID_Fee = ",
						ID_Fee.ToString(),
						" AND ID_ExamItem  IN (",
						newExamItemIDStrs,
						") ) "
					}));
					stringBuilder.Append(" AND ID_ExamItem IN (" + newExamItemIDStrs + "); ");
				}
				int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString());
				result = num;
			}
			return result;
		}

		int ICommonConfig.SaveExamItem(PEIS.Model.BusExamItem ExamItemModel)
		{
			int result;
			if (ExamItemModel.ID_ExamItem > 0)
			{
				result = this.BusExamItemUpdate(ExamItemModel);
			}
			else
			{
				result = this.BusExamItemAdd(ExamItemModel);
			}
			return result;
		}

		private int BusExamItemAdd(PEIS.Model.BusExamItem model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusExamItem(");
			stringBuilder.Append("ExamItemName,GetResultWay,ExamItemCode,ID_Section,Is_LisValueNull,Is_EntrySectSum,EntrySectSumLevel,Is_AutoCalc,CalcExpression,SymCols,TextboxRows,Is_SameRow,ExamItemUnit,MaleHiLimit,MaleLoLimit,FemaleHiLimit,FemaleLoLimit,Is_SymMultiValue,InputCode,DispOrder,Note,Forsex,Is_ExamItemNonPrintInReport,ID_ExamItemGroup,AbbrExamName)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ExamItemName,@GetResultWay,@ExamItemCode,@ID_Section,@Is_LisValueNull,@Is_EntrySectSum,@EntrySectSumLevel,@Is_AutoCalc,@CalcExpression,@SymCols,@TextboxRows,@Is_SameRow,@ExamItemUnit,@MaleHiLimit,@MaleLoLimit,@FemaleHiLimit,@FemaleLoLimit,@Is_SymMultiValue,@InputCode,@DispOrder,@Note,@Forsex,@Is_ExamItemNonPrintInReport,@ID_ExamItemGroup,@AbbrExamName)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamItemName", SqlDbType.VarChar, 50),
				new SqlParameter("@GetResultWay", SqlDbType.VarChar, 2),
				new SqlParameter("@ExamItemCode", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@Is_LisValueNull", SqlDbType.Bit, 1),
				new SqlParameter("@Is_EntrySectSum", SqlDbType.Bit, 1),
				new SqlParameter("@EntrySectSumLevel", SqlDbType.Int, 4),
				new SqlParameter("@Is_AutoCalc", SqlDbType.Bit, 1),
				new SqlParameter("@CalcExpression", SqlDbType.Text),
				new SqlParameter("@SymCols", SqlDbType.Int, 4),
				new SqlParameter("@TextboxRows", SqlDbType.Int, 4),
				new SqlParameter("@Is_SameRow", SqlDbType.Bit, 1),
				new SqlParameter("@ExamItemUnit", SqlDbType.VarChar, 20),
				new SqlParameter("@MaleHiLimit", SqlDbType.Float, 8),
				new SqlParameter("@MaleLoLimit", SqlDbType.Float, 8),
				new SqlParameter("@FemaleHiLimit", SqlDbType.Float, 8),
				new SqlParameter("@FemaleLoLimit", SqlDbType.Float, 8),
				new SqlParameter("@Is_SymMultiValue", SqlDbType.Bit, 1),
				new SqlParameter("@InputCode", SqlDbType.VarChar, 30),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar, 20),
				new SqlParameter("@Forsex", SqlDbType.Int, 4),
				new SqlParameter("@Is_ExamItemNonPrintInReport", SqlDbType.Bit, 1),
				new SqlParameter("@ID_ExamItemGroup", SqlDbType.Int, 4),
				new SqlParameter("@AbbrExamName", SqlDbType.VarChar, 60)
			};
			array[0].Value = model.ExamItemName;
			array[1].Value = model.GetResultWay;
			array[2].Value = (model.ExamItemCode);
			array[3].Value = model.ID_Section;
			array[4].Value = model.Is_LisValueNull;
			array[5].Value = model.Is_EntrySectSum;
			array[6].Value = model.EntrySectSumLevel;
			array[7].Value = model.Is_AutoCalc;
			array[8].Value = (model.CalcExpression);
			array[9].Value = model.SymCols;
			array[10].Value = model.TextboxRows;
			array[11].Value = model.Is_SameRow;
			array[12].Value = model.ExamItemUnit;
			array[13].Value = (model.MaleLoLimit);
			array[14].Value = (model.MaleHiLimit);
			array[15].Value = (model.FemaleLoLimit);
			array[16].Value = (model.FemaleHiLimit);
			array[17].Value = model.Is_SymMultiValue;
			array[18].Value = model.InputCode;
			array[19].Value = model.DispOrder;
			array[20].Value = model.Note;
			array[21].Value = model.Forsex;
			array[22].Value = model.Is_ExamItemNonPrintInReport;
			array[23].Value = model.ID_ExamItemGroup;
			array[24].Value = model.AbbrExamName;
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

		private int BusExamItemUpdate(PEIS.Model.BusExamItem model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusExamItem set ");
			stringBuilder.Append("ExamItemName=@ExamItemName,");
			stringBuilder.Append("GetResultWay=@GetResultWay,");
			stringBuilder.Append("ExamItemCode=@ExamItemCode,");
			stringBuilder.Append("ID_Section=@ID_Section,");
			stringBuilder.Append("Is_LisValueNull=@Is_LisValueNull,");
			stringBuilder.Append("Is_EntrySectSum=@Is_EntrySectSum,");
			stringBuilder.Append("EntrySectSumLevel=@EntrySectSumLevel,");
			stringBuilder.Append("Is_AutoCalc=@Is_AutoCalc,");
			stringBuilder.Append("CalcExpression=@CalcExpression,");
			stringBuilder.Append("SymCols=@SymCols,");
			stringBuilder.Append("TextboxRows=@TextboxRows,");
			stringBuilder.Append("Is_SameRow=@Is_SameRow,");
			stringBuilder.Append("ExamItemUnit=@ExamItemUnit,");
			stringBuilder.Append("MaleHiLimit=@MaleHiLimit,");
			stringBuilder.Append("MaleLoLimit=@MaleLoLimit,");
			stringBuilder.Append("FemaleHiLimit=@FemaleHiLimit,");
			stringBuilder.Append("FemaleLoLimit=@FemaleLoLimit,");
			stringBuilder.Append("Is_SymMultiValue=@Is_SymMultiValue,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("Forsex=@Forsex,");
			stringBuilder.Append("Is_ExamItemNonPrintInReport=@Is_ExamItemNonPrintInReport,");
			stringBuilder.Append("ID_ExamItemGroup=@ID_ExamItemGroup,");
			stringBuilder.Append("AbbrExamName=@AbbrExamName");
			stringBuilder.Append(" where ID_ExamItem=@ID_ExamItem");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamItemName", SqlDbType.VarChar, 50),
				new SqlParameter("@GetResultWay", SqlDbType.VarChar, 2),
				new SqlParameter("@ExamItemCode", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@Is_LisValueNull", SqlDbType.Bit, 1),
				new SqlParameter("@Is_EntrySectSum", SqlDbType.Bit, 1),
				new SqlParameter("@EntrySectSumLevel", SqlDbType.Int, 4),
				new SqlParameter("@Is_AutoCalc", SqlDbType.Bit, 1),
				new SqlParameter("@CalcExpression", SqlDbType.Text),
				new SqlParameter("@SymCols", SqlDbType.Int, 4),
				new SqlParameter("@TextboxRows", SqlDbType.Int, 4),
				new SqlParameter("@Is_SameRow", SqlDbType.Bit, 1),
				new SqlParameter("@ExamItemUnit", SqlDbType.VarChar, 20),
				new SqlParameter("@MaleHiLimit", SqlDbType.Float, 8),
				new SqlParameter("@MaleLoLimit", SqlDbType.Float, 8),
				new SqlParameter("@FemaleHiLimit", SqlDbType.Float, 8),
				new SqlParameter("@FemaleLoLimit", SqlDbType.Float, 8),
				new SqlParameter("@Is_SymMultiValue", SqlDbType.Bit, 1),
				new SqlParameter("@InputCode", SqlDbType.VarChar, 30),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar, 20),
				new SqlParameter("@Forsex", SqlDbType.Int, 4),
				new SqlParameter("@Is_ExamItemNonPrintInReport", SqlDbType.Bit, 1),
				new SqlParameter("@ID_ExamItemGroup", SqlDbType.Int, 4),
				new SqlParameter("@AbbrExamName", SqlDbType.VarChar, 60),
				new SqlParameter("@ID_ExamItem", SqlDbType.Int, 4)
			};
			array[0].Value = model.ExamItemName;
			array[1].Value = model.GetResultWay;
			array[2].Value = (model.ExamItemCode);
			array[3].Value = model.ID_Section;
			array[4].Value = model.Is_LisValueNull;
			array[5].Value = model.Is_EntrySectSum;
			array[6].Value = model.EntrySectSumLevel;
			array[7].Value = model.Is_AutoCalc;
			array[8].Value = (model.CalcExpression);
			array[9].Value = model.SymCols;
			array[10].Value = model.TextboxRows;
			array[11].Value = model.Is_SameRow;
			array[12].Value = model.ExamItemUnit;
			array[13].Value = (model.MaleLoLimit);
			array[14].Value = (model.MaleHiLimit);
			array[15].Value = (model.FemaleLoLimit);
			array[16].Value = (model.FemaleHiLimit);
			array[17].Value = model.Is_SymMultiValue;
			array[18].Value = model.InputCode;
			array[19].Value = model.DispOrder;
			array[20].Value = model.Note;
			array[21].Value = model.Forsex;
			array[22].Value = model.Is_ExamItemNonPrintInReport;
			array[23].Value = model.ID_ExamItemGroup;
			array[24].Value = model.AbbrExamName;
			array[25].Value = model.ID_ExamItem;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveSymptom(PEIS.Model.BusSymptom BusSymptomModel)
		{
			int result;
			if (BusSymptomModel.ID_Symptom > 0)
			{
				result = this.BusSymptomUpdate(BusSymptomModel);
			}
			else
			{
				result = this.BusSymptomAdd(BusSymptomModel);
			}
			return result;
		}

		private int BusSymptomAdd(PEIS.Model.BusSymptom model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusSymptom(");
			stringBuilder.Append("ID_ExamItem,ID_Conclusion,SymptomName,SymptomDescribe,DiseaseLevel,Is_Default,NumOperSign,NumMale,NumFemale,InputCode,DispOrder,Is_Banned,ID_BanOpr,BanOperator,BanDate)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_ExamItem,@ID_Conclusion,@SymptomName,@SymptomDescribe,@DiseaseLevel,@Is_Default,@NumOperSign,@NumMale,@NumFemale,@InputCode,@DispOrder,@Is_Banned,@ID_BanOpr,@BanOperator,@BanDate)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_ExamItem", SqlDbType.Int, 4),
				new SqlParameter("@ID_Conclusion", SqlDbType.Int, 4),
				new SqlParameter("@SymptomName", SqlDbType.VarChar),
				new SqlParameter("@SymptomDescribe", SqlDbType.Text),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@Is_Default", SqlDbType.Bit, 1),
				new SqlParameter("@NumOperSign", SqlDbType.VarChar),
				new SqlParameter("@NumMale", SqlDbType.Float, 8),
				new SqlParameter("@NumFemale", SqlDbType.Float, 8),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime)
			};
			array[0].Value = model.ID_ExamItem;
			array[1].Value = model.ID_Conclusion;
			array[2].Value = model.SymptomName;
			array[3].Value = model.SymptomDescribe;
			array[4].Value = model.DiseaseLevel;
			array[5].Value = model.Is_Default;
			array[6].Value = model.NumOperSign;
			array[7].Value = (model.NumMale);
			array[8].Value = (model.NumFemale);
			array[9].Value = model.InputCode;
			array[10].Value = model.DispOrder;
			array[11].Value = model.Is_Banned;
			array[12].Value = model.ID_BanOpr;
			array[13].Value = model.BanOperator;
			array[14].Value = model.BanDate;
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

		private int BusSymptomUpdate(PEIS.Model.BusSymptom model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusSymptom set ");
			stringBuilder.Append("ID_ExamItem=@ID_ExamItem,");
			stringBuilder.Append("ID_Conclusion=@ID_Conclusion,");
			stringBuilder.Append("SymptomName=@SymptomName,");
			stringBuilder.Append("SymptomDescribe=@SymptomDescribe,");
			stringBuilder.Append("DiseaseLevel=@DiseaseLevel,");
			stringBuilder.Append("Is_Default=@Is_Default,");
			stringBuilder.Append("NumOperSign=@NumOperSign,");
			stringBuilder.Append("NumMale=@NumMale,");
			stringBuilder.Append("NumFemale=@NumFemale,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_BanOpr=@ID_BanOpr,");
			stringBuilder.Append("BanOperator=@BanOperator,");
			stringBuilder.Append("BanDate=@BanDate");
			stringBuilder.Append(" where ID_Symptom=@ID_Symptom");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_ExamItem", SqlDbType.Int, 4),
				new SqlParameter("@ID_Conclusion", SqlDbType.Int, 4),
				new SqlParameter("@SymptomName", SqlDbType.VarChar),
				new SqlParameter("@SymptomDescribe", SqlDbType.Text),
				new SqlParameter("@DiseaseLevel", SqlDbType.Int, 4),
				new SqlParameter("@Is_Default", SqlDbType.Bit, 1),
				new SqlParameter("@NumOperSign", SqlDbType.VarChar),
				new SqlParameter("@NumMale", SqlDbType.Float, 8),
				new SqlParameter("@NumFemale", SqlDbType.Float, 8),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@ID_Symptom", SqlDbType.Int, 4)
			};
			array[0].Value = model.ID_ExamItem;
			array[1].Value = model.ID_Conclusion;
			array[2].Value = model.SymptomName;
			array[3].Value = model.SymptomDescribe;
			array[4].Value = model.DiseaseLevel;
			array[5].Value = model.Is_Default;
			array[6].Value = model.NumOperSign;
			array[7].Value = (model.NumMale);
			array[8].Value = (model.NumFemale);
			array[9].Value = model.InputCode;
			array[10].Value = model.DispOrder;
			array[11].Value = model.Is_Banned;
			array[12].Value = model.ID_BanOpr;
			array[13].Value = model.BanOperator;
			array[14].Value = model.BanDate;
			array[15].Value = model.ID_Symptom;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveUser(PEIS.Model.SYSOpUser UserModel)
		{
			int result;
			if (UserModel.UserID > 0)
			{
				result = this.SysUserUpdate(UserModel);
			}
			else
			{
				result = this.SysUserAdd(UserModel);
			}
			return result;
		}

		private int SysUserAdd(PEIS.Model.SYSOpUser model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into SYSOpUser(");
			stringBuilder.Append("[SectionID],[UserName],[LoginName],[PassWord],[LastLoginTime],[Note],[DisDate],[Signature],[DisCountRate],[Sex],[Is_Del],[VocationType],[OperateLevel],[TotalNum],[FailCount],[CreateTime])");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Section,@UserName,@LoginName,@PassWord,@LastLoginTime,@Note,@DisDate,@Signature,@DisCountRate,@Sex,@Is_Del,@VocationType,@OperateLevel,@TotalNum,@FailCount,@CreateTime)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Section", SqlDbType.Int, 4),
				new SqlParameter("@UserName", SqlDbType.VarChar),
				new SqlParameter("@LoginName", SqlDbType.VarChar),
				new SqlParameter("@PassWord", SqlDbType.VarChar),				
				new SqlParameter("@CreateTime", SqlDbType.DateTime),
				new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
				new SqlParameter("@TotalNum", SqlDbType.Int, 4),
				new SqlParameter("@FailCount", SqlDbType.Int, 4),
				new SqlParameter("@OperateLevel", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar),
				new SqlParameter("@DisDate", SqlDbType.DateTime),
				new SqlParameter("@Signature", SqlDbType.Image),
				new SqlParameter("@DisCountRate", SqlDbType.Int, 4),
				new SqlParameter("@sex", SqlDbType.Int, 4),
				new SqlParameter("@Is_Del", SqlDbType.Int, 4),
				new SqlParameter("@VocationType", SqlDbType.Int, 4)
			};
			array[0].Value = model.SectionID;
			array[1].Value = model.UserName;
			array[2].Value = model.LoginName;
			array[3].Value = model.PassWord;			
			array[4].Value = model.CreateTime;
			array[5].Value = model.LastLoginTime;
			array[6].Value = model.TotalNum;
			array[7].Value = model.FailCount;
			array[8].Value = model.OperateLevel;
			array[9].Value = model.Note;
			array[10].Value = model.DisDate;
			array[11].Value = model.Signature;
			array[12].Value = model.DisCountRate;
			array[13].Value = model.Sex;
			array[14].Value = model.Is_Del;
			array[15].Value = model.VocationType;
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

		private int SysUserUpdate(PEIS.Model.SYSOpUser model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update SYSOpUser set ");
			stringBuilder.Append("SectionID=@SectionID,");
			stringBuilder.Append("UserName=@UserName,");
			stringBuilder.Append("LoginName=@LoginName,");
			stringBuilder.Append("PassWord=@PassWord,");		
			stringBuilder.Append("CreateTime=@CreateTime,");
			stringBuilder.Append("LastLoginTime=@LastLoginTime,");
			stringBuilder.Append("TotalNum=@TotalNum,");
			stringBuilder.Append("FailCount=@FailCount,");
			stringBuilder.Append("OperateLevel=@OperateLevel,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("DisDate=@DisDate,");
			stringBuilder.Append("Signature=@Signature,");
			stringBuilder.Append("DisCountRate=@DisCountRate,");
			stringBuilder.Append("sex=@sex,");
			stringBuilder.Append("Is_Del=@Is_Del,");
			stringBuilder.Append("VocationType=@VocationType");
			stringBuilder.Append(" where UserID=@UserID");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@SectionID", SqlDbType.Int, 4),
				new SqlParameter("@UserName", SqlDbType.VarChar),
				new SqlParameter("@LoginName", SqlDbType.VarChar),
				new SqlParameter("@PassWord", SqlDbType.VarChar),				
				new SqlParameter("@CreateTime", SqlDbType.DateTime),
				new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
				new SqlParameter("@TotalNum", SqlDbType.Int, 4),
				new SqlParameter("@FailCount", SqlDbType.Int, 4),
				new SqlParameter("@OperateLevel", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar),
				new SqlParameter("@DisDate", SqlDbType.DateTime),
				new SqlParameter("@Signature", SqlDbType.Image),
				new SqlParameter("@DisCountRate", SqlDbType.Int, 4),
				new SqlParameter("@sex", SqlDbType.Int, 4),
				new SqlParameter("@Is_Del", SqlDbType.Int, 4),
				new SqlParameter("@VocationType", SqlDbType.Int, 4),
				new SqlParameter("@UserID", SqlDbType.Int, 4)
			};
			array[0].Value = model.SectionID;
			array[1].Value = model.UserName;
			array[2].Value = model.LoginName;
			array[3].Value = model.PassWord;		
			array[4].Value = model.CreateTime;
			array[5].Value = model.LastLoginTime;
			array[6].Value = model.TotalNum;
			array[7].Value = model.FailCount;
			array[8].Value = model.OperateLevel;
			array[9].Value = model.Note;
			array[10].Value = model.DisDate;
			array[11].Value = model.Signature;
			array[12].Value = model.DisCountRate;
			array[13].Value = model.Sex;
			array[14].Value = model.Is_Del;
			array[15].Value = model.VocationType;
			array[16].Value = model.UserID;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.ClearUserLoginCount(int UserID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update SYSOpUser set ");
			stringBuilder.Append("FailCount=0");
			stringBuilder.Append(" where UserID=@UserID");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
			};
			array[0].Value = UserID;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.ResetUserPassword(int UserID, string DefaultPassword)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update SYSOpUser set ");
			stringBuilder.Append("PassWord=@PassWord");
			stringBuilder.Append(" where UserID=@UserID");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@PassWord", SqlDbType.VarChar),
				new SqlParameter("@UserID", SqlDbType.Int, 4)
			};
			array[0].Value = DefaultPassword;
			array[1].Value = UserID;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveBusPEPackage(PEIS.Model.BusPEPackage busSetModel)
		{
			int result;
			if (busSetModel.PEPackageID > 0)
			{
				result = this.PEPackageUpdate(busSetModel);
			}
			else
			{
				result = this.PEPackageAdd(busSetModel);
			}
			return result;
		}

		private int PEPackageAdd(PEIS.Model.BusPEPackage model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusPEPackage(");
            strSql.Append("PEPackageID,PEPackageName,Forsex,CreatorID,CreateDate,isBanned,IDBanOpr,BanDate,BanDescribe,InputCode,DispOrder,Note)");
            strSql.Append(" values (");
            strSql.Append("@PEPackageID,@PEPackageName,@Forsex,@CreatorID,@CreateDate,@isBanned,@IDBanOpr,@BanDate,@BanDescribe,@InputCode,@DispOrder,@Note)");
            SqlParameter[] parameters = {
                    new SqlParameter("@PEPackageID", SqlDbType.Int,4),
                    new SqlParameter("@PEPackageName", SqlDbType.VarChar,50),
                    new SqlParameter("@Forsex", SqlDbType.Int,4),
                    new SqlParameter("@CreatorID", SqlDbType.Int,4),
                    new SqlParameter("@CreateDate", SqlDbType.SmallDateTime),
                    new SqlParameter("@isBanned", SqlDbType.Bit,1),
                    new SqlParameter("@IDBanOpr", SqlDbType.Int,4),
                    new SqlParameter("@BanDate", SqlDbType.SmallDateTime),
                    new SqlParameter("@BanDescribe", SqlDbType.VarChar,50),
                    new SqlParameter("@InputCode", SqlDbType.VarChar,30),
                    new SqlParameter("@DispOrder", SqlDbType.Int,4),
                    new SqlParameter("@Note", SqlDbType.VarChar,50)};
            parameters[0].Value = model.PEPackageID;
            parameters[1].Value = model.PEPackageName;
            parameters[2].Value = model.Forsex;
            parameters[3].Value = model.CreatorID;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.isBanned;
            parameters[6].Value = model.IDBanOpr;
            parameters[7].Value = model.BanDate;
            parameters[8].Value = model.BanDescribe;
            parameters[9].Value = model.InputCode;
            parameters[10].Value = model.DispOrder;
            parameters[11].Value = model.Note;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

		private int PEPackageUpdate(PEIS.Model.BusPEPackage model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BusPEPackage set ");
            strSql.Append("PEPackageName=@PEPackageName,");
            strSql.Append("Forsex=@Forsex,");
            strSql.Append("CreatorID=@CreatorID,");
            strSql.Append("CreateDate=@CreateDate,");
            strSql.Append("isBanned=@isBanned,");
            strSql.Append("IDBanOpr=@IDBanOpr,");
            strSql.Append("BanDate=@BanDate,");
            strSql.Append("BanDescribe=@BanDescribe,");
            strSql.Append("InputCode=@InputCode,");
            strSql.Append("DispOrder=@DispOrder,");
            strSql.Append("Note=@Note");
            strSql.Append(" where PEPackageID=@PEPackageID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PEPackageName", SqlDbType.VarChar,50),
                    new SqlParameter("@Forsex", SqlDbType.Int,4),
                    new SqlParameter("@CreatorID", SqlDbType.Int,4),
                    new SqlParameter("@CreateDate", SqlDbType.SmallDateTime),
                    new SqlParameter("@isBanned", SqlDbType.Bit,1),
                    new SqlParameter("@IDBanOpr", SqlDbType.Int,4),
                    new SqlParameter("@BanDate", SqlDbType.SmallDateTime),
                    new SqlParameter("@BanDescribe", SqlDbType.VarChar,50),
                    new SqlParameter("@InputCode", SqlDbType.VarChar,30),
                    new SqlParameter("@DispOrder", SqlDbType.Int,4),
                    new SqlParameter("@Note", SqlDbType.VarChar,50),
                    new SqlParameter("@PEPackageID", SqlDbType.Int,4)};
            parameters[0].Value = model.PEPackageName;
            parameters[1].Value = model.Forsex;
            parameters[2].Value = model.CreatorID;
            parameters[3].Value = model.CreateDate;
            parameters[4].Value = model.isBanned;
            parameters[5].Value = model.IDBanOpr;
            parameters[6].Value = model.BanDate;
            parameters[7].Value = model.BanDescribe;
            parameters[8].Value = model.InputCode;
            parameters[9].Value = model.DispOrder;
            parameters[10].Value = model.Note;
            parameters[11].Value = model.PEPackageID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

		int ICommonConfig.SaveSetFeeRel(int PEPackageID, string newFeeIDStrs)
		{
			int result;
			if (PEPackageID <= 0)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (string.IsNullOrEmpty(newFeeIDStrs))
				{
					stringBuilder.Append(" DELETE FROM BusSetFeeDetail ");
					stringBuilder.Append(" WHERE PEPackageID = " + PEPackageID.ToString() + ";");
				}
				else
				{
					stringBuilder.Append(" DELETE FROM BusSetFeeDetail ");
					stringBuilder.Append(" WHERE PEPackageID = " + PEPackageID.ToString());
					stringBuilder.Append(" AND ID_FeeItem NOT IN  ( " + newFeeIDStrs + "); ");
					stringBuilder.Append(" INSERT INTO BusSetFeeDetail  ");
					stringBuilder.Append(" SELECT " + PEPackageID.ToString() + " PEPackageID, ID_Fee ");
					stringBuilder.Append(" FROM BusFee  ");
					stringBuilder.Append(string.Concat(new string[]
					{
						" WHERE ID_Fee NOT IN ( SELECT ID_FeeItem FROM BusSetFeeDetail WHERE PEPackageID = ",
						PEPackageID.ToString(),
						" AND ID_FeeItem  IN (",
						newFeeIDStrs,
						") ) "
					}));
					stringBuilder.Append(" AND ID_Fee IN (" + newFeeIDStrs + "); ");
				}
				int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString());
				result = num;
			}
			return result;
		}

		int ICommonConfig.MergeCustExamInfo(string MergerID_01, string MergerID_02, string[] ConnectionStringArray)
		{
			int num = 0;
			string text = MergerID_01 + "," + MergerID_02;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				" \r\n\r\n            DECLARE @ID_Gender INT\t\t     --性别ID\r\n            DECLARE @GenderName varchar(8)   --性别\r\n            DECLARE @ID_Marriage INT\t\t --婚姻ID\r\n            DECLARE @MarriageName varchar(8) --婚姻\r\n            DECLARE @NationID INT\t\t     --民族ID\r\n            DECLARE @NationName varchar(8)   --民族\r\n            DECLARE @CultrulID INT\t\t     --文化层度ID\r\n            DECLARE @CultrulName varchar(10) --文化层度\r\n            DECLARE @VocationID INT\t\t --职业ID\r\n            DECLARE @VocationName varchar(10)--职业\r\n            DECLARE @IDCard varchar(30)\t\t --证件号码\r\n            DECLARE @ExamCard varchar(30)\t --一卡通\r\n            DECLARE @BirthDay datetime\t     --出生日期\r\n            DECLARE @Address varchar(120)\t --联系地址\r\n            DECLARE @MobileNo varchar(120)\t --联系电话\r\n            DECLARE @Email varchar(30)\t     --Email\r\n            DECLARE @CustomerName varchar(30)--Email\r\n            DECLARE @MinCurID_CustRelation int--最小关系ID\r\n            DECLARE @MaxCurID_CustRelation int--最小关系ID\r\n\r\n            declare @MergerID_01 int;\r\n            declare @MergerID_02 int;\r\n            set @MergerID_01 = ",
				MergerID_01.ToString(),
				";\r\n            set @MergerID_02 = ",
				MergerID_02.ToString(),
				";\r\n\r\n            --获取基本信息\r\n            SELECT @CustomerName=CustomerName,@ID_Gender=ID_Gender,@GenderName=GenderName,@ID_Marriage=ID_Marriage,@MarriageName=MarriageName,@NationID=NationID,@NationName=NationName\r\n            ,@CultrulID=CultrulID,@CultrulName=CultrulName,@VocationID=VocationID,@VocationName=VocationName,@IDCard=IDCard,@ExamCard=ExamCard\r\n            ,@BirthDay=BirthDay,@Address=Address,@MobileNo=MobileNo,@Email=Email\r\n            FROM OnArcCust WHERE ID_ArcCustomer=@MergerID_01;\r\n\r\n\r\n            --修改体检信息表中客户基本信息\r\n            UPDATE OnCustPhysicalExamInfo SET ID_Gender=@ID_Gender,GenderName=@GenderName,ID_Marriage=@ID_Marriage,MarriageName=@MarriageName,NationID=@NationID\r\n            ,NationName=@NationName,CultrulID=@CultrulID,CultrulName=@CultrulName,VocationID=@VocationID,VocationName=@VocationName,IDCard=@IDCard,ExamCard=@ExamCard\r\n            ,BirthDay=@BirthDay,Address=@Address,MobileNo=@MobileNo,Email=@Email\r\n            ,Photo=(\r\n            SELECT Photo FROM [OnArcCust] where  [ID_ArcCustomer] =  @MergerID_01 )\r\n            where ID_Customer in (\r\n            SELECT ID_Customer FROM OnCustRelationCustPEInfo where  [ID_ArcCustomer] =  @MergerID_02 )  \r\n                          "
			}));
			stringBuilder.Append(string.Concat(new string[]
			{
				" update OnCustRelationCustPEInfo set ID_ArcCustomer = ",
				MergerID_01,
				",IDCardNo=(SELECT [IDCard] FROM [OnArcCust] where  [ID_ArcCustomer] =  '",
				MergerID_01,
				"'),ExamCardNo=(SELECT [ExamCard] FROM [OnArcCust] where  [ID_ArcCustomer] =  '",
				MergerID_01,
				"') where ID_ArcCustomer = ",
				MergerID_02,
				";"
			}));
			stringBuilder.Append(string.Concat(new string[]
			{
				" update OnArcCust set \r\n                            FinishedNum = (select SUM(ISNULL(FinishedNum,0)) FinishedNum from OnArcCust where ID_ArcCustomer in (",
				text,
				")) ,\r\n                            UnfinishedNum = (select SUM(ISNULL(UnfinishedNum,0)) UnfinishedNum from OnArcCust where ID_ArcCustomer in (",
				text,
				")) \r\n                            where ID_ArcCustomer = ",
				MergerID_01,
				";"
			}));
			stringBuilder.Append(" update OnArcCust set \r\n                            FinishedNum = 0 ,\r\n                            UnfinishedNum = 0 \r\n                            where ID_ArcCustomer = " + MergerID_02 + ";");
			for (int i = 0; i < ConnectionStringArray.Length; i++)
			{
				if (ConnectionStringArray[i] != null)
				{
					if (!string.IsNullOrEmpty(ConnectionStringArray[i].ToString()))
					{
						num += DbHelperSQL.ExecuteSql_Ex(ConnectionStringArray[i].ToString(), stringBuilder.ToString());
					}
				}
			}
			return num;
		}

		int ICommonConfig.SaveConclusionType(PEIS.Model.BusConclusionType ConclusionTypeModel)
		{
			int result;
			if (ConclusionTypeModel.ID_ConclusionType > 0)
			{
				result = this.BusConclusionTypeUpdate(ConclusionTypeModel);
			}
			else
			{
				result = this.BusConclusionTypeAdd(ConclusionTypeModel);
			}
			return result;
		}

		private int BusConclusionTypeAdd(PEIS.Model.BusConclusionType model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusConclusionType(");
			stringBuilder.Append("ConclusionTypeName,InputCode,Is_Banned,ID_BanOpr,BanOperator,BanDate,BanDescribe)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ConclusionTypeName,@InputCode,@Is_Banned,@ID_BanOpr,@BanOperator,@BanDate,@BanDescribe)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ConclusionTypeName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar)
			};
			array[0].Value = model.ConclusionTypeName;
			array[1].Value = model.InputCode;
			array[2].Value = model.Is_Banned;
			array[3].Value = model.ID_BanOpr;
			array[4].Value = model.BanOperator;
			array[5].Value = model.BanDate;
			array[6].Value = model.BanDescribe;
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

		private int BusConclusionTypeUpdate(PEIS.Model.BusConclusionType model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusConclusionType set ");
			stringBuilder.Append("ConclusionTypeName=@ConclusionTypeName,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_BanOpr=@ID_BanOpr,");
			stringBuilder.Append("BanOperator=@BanOperator,");
			stringBuilder.Append("BanDate=@BanDate,");
			stringBuilder.Append("BanDescribe=@BanDescribe");
			stringBuilder.Append(" where ID_ConclusionType=@ID_ConclusionType");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ConclusionTypeName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@ID_ConclusionType", SqlDbType.Int, 4)
			};
			array[0].Value = model.ConclusionTypeName;
			array[1].Value = model.InputCode;
			array[2].Value = model.Is_Banned;
			array[3].Value = model.ID_BanOpr;
			array[4].Value = model.BanOperator;
			array[5].Value = model.BanDate;
			array[6].Value = model.BanDescribe;
			array[7].Value = model.ID_ConclusionType;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveFinalConclusionType(PEIS.Model.DctFinalConclusionType FinalConclusionTypeModel)
		{
			int result;
			if (FinalConclusionTypeModel.ID_FinalConclusionType > 0)
			{
				result = this.DctFinalConclusionTypeUpdate(FinalConclusionTypeModel);
			}
			else
			{
				result = this.DctFinalConclusionTypeAdd(FinalConclusionTypeModel);
			}
			return result;
		}

		private int DctFinalConclusionTypeAdd(PEIS.Model.DctFinalConclusionType model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into DctFinalConclusionType(");
			stringBuilder.Append("FinalConclusionTypeName,InputCode,Note,ID_Creator,CreateDate,Is_Banned,ID_BanOpr,BanDescribe,DispOrder,BanDate,BanOperator,FinalConclusionSignCode)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@FinalConclusionTypeName,@InputCode,@Note,@ID_Creator,@CreateDate,@Is_Banned,@ID_BanOpr,@BanDescribe,@DispOrder,@BanDate,@BanOperator,@FinalConclusionSignCode)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@FinalConclusionTypeName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@Note", SqlDbType.VarChar),
				new SqlParameter("@ID_Creator", SqlDbType.Int, 4),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@FinalConclusionSignCode", SqlDbType.VarChar)
			};
			array[0].Value = model.FinalConclusionTypeName;
			array[1].Value = model.InputCode;
			array[2].Value = model.Note;
			array[3].Value = model.ID_Creator;
			array[4].Value = model.CreateDate;
			array[5].Value = model.Is_Banned;
			array[6].Value = model.ID_BanOpr;
			array[7].Value = model.BanDescribe;
			array[8].Value = model.DispOrder;
			array[9].Value = model.BanDate;
			array[10].Value = model.BanOperator;
			array[11].Value = model.FinalConclusionSignCode;
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

		private int DctFinalConclusionTypeUpdate(PEIS.Model.DctFinalConclusionType model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update DctFinalConclusionType set ");
			stringBuilder.Append("FinalConclusionTypeName=@FinalConclusionTypeName,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("ID_Creator=@ID_Creator,");
			stringBuilder.Append("CreateDate=@CreateDate,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_BanOpr=@ID_BanOpr,");
			stringBuilder.Append("BanDescribe=@BanDescribe,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("BanDate=@BanDate,");
			stringBuilder.Append("BanOperator=@BanOperator,");
			stringBuilder.Append("FinalConclusionSignCode=@FinalConclusionSignCode");
			stringBuilder.Append(" where ID_FinalConclusionType=@ID_FinalConclusionType");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@FinalConclusionTypeName", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@Note", SqlDbType.VarChar),
				new SqlParameter("@ID_Creator", SqlDbType.Int, 4),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@FinalConclusionSignCode", SqlDbType.VarChar),
				new SqlParameter("@ID_FinalConclusionType", SqlDbType.Int, 4)
			};
			array[0].Value = model.FinalConclusionTypeName;
			array[1].Value = model.InputCode;
			array[2].Value = model.Note;
			array[3].Value = model.ID_Creator;
			array[4].Value = model.CreateDate;
			array[5].Value = model.Is_Banned;
			array[6].Value = model.ID_BanOpr;
			array[7].Value = model.BanDescribe;
			array[8].Value = model.DispOrder;
			array[9].Value = model.BanDate;
			array[10].Value = model.BanOperator;
			array[11].Value = model.FinalConclusionSignCode;
			array[12].Value = model.ID_FinalConclusionType;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveICD(PEIS.Model.DctICDTen ICDTenModel)
		{
			int result;
			if (ICDTenModel.ID_ICD > 0)
			{
				result = this.DctICDTenUpdate(ICDTenModel);
			}
			else
			{
				result = this.DctICDTenAdd(ICDTenModel);
			}
			return result;
		}

		private int DctICDTenAdd(PEIS.Model.DctICDTen ICDTenModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into DctICDTen(");
			stringBuilder.Append("ICDCNName,ICDENName,Code,Codea,ID_Creator,Creator,CreateDate,Is_Banned,ID_BanOpr,BanOperator,BanDate,BanDescribe,LevelA,LevelB,LevelC,LevelD,LevelE,LevelTree,Class,Tag,ICDtoSection,Note,InputCode)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ICDCNName,@ICDENName,@Code,@Codea,@ID_Creator,@Creator,@CreateDate,@Is_Banned,@ID_BanOpr,@BanOperator,@BanDate,@BanDescribe,@LevelA,@LevelB,@LevelC,@LevelD,@LevelE,@LevelTree,@Class,@Tag,@ICDtoSection,@Note,@InputCode)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ICDCNName", SqlDbType.VarChar),
				new SqlParameter("@ICDENName", SqlDbType.VarChar),
				new SqlParameter("@Code", SqlDbType.VarChar),
				new SqlParameter("@Codea", SqlDbType.VarChar),
				new SqlParameter("@ID_Creator", SqlDbType.Int, 4),
				new SqlParameter("@Creator", SqlDbType.VarChar),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@LevelA", SqlDbType.Int, 4),
				new SqlParameter("@LevelB", SqlDbType.Int, 4),
				new SqlParameter("@LevelC", SqlDbType.Int, 4),
				new SqlParameter("@LevelD", SqlDbType.Int, 4),
				new SqlParameter("@LevelE", SqlDbType.Int, 4),
				new SqlParameter("@LevelTree", SqlDbType.Int, 4),
				new SqlParameter("@Class", SqlDbType.VarChar),
				new SqlParameter("@Tag", SqlDbType.VarChar),
				new SqlParameter("@ICDtoSection", SqlDbType.VarChar),
				new SqlParameter("@Note", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar)
			};
			array[0].Value = ICDTenModel.ICDCNName;
			array[1].Value = ICDTenModel.ICDENName;
			array[2].Value = ICDTenModel.Code;
			array[3].Value = ICDTenModel.Codea;
			array[4].Value = ICDTenModel.ID_Creator;
			array[5].Value = ICDTenModel.Creator;
			array[6].Value = ICDTenModel.CreateDate;
			array[7].Value = ICDTenModel.Is_Banned;
			array[8].Value = ICDTenModel.ID_BanOpr;
			array[9].Value = ICDTenModel.BanOperator;
			array[10].Value = ICDTenModel.BanDate;
			array[11].Value = ICDTenModel.BanDescribe;
			array[12].Value = ICDTenModel.LevelA;
			array[13].Value = ICDTenModel.LevelB;
			array[14].Value = ICDTenModel.LevelC;
			array[15].Value = ICDTenModel.LevelD;
			array[16].Value = ICDTenModel.LevelE;
			array[17].Value = ICDTenModel.LevelTree;
			array[18].Value = ICDTenModel.Class;
			array[19].Value = ICDTenModel.Tag;
			array[20].Value = ICDTenModel.ICDtoSection;
			array[21].Value = ICDTenModel.Note;
			array[22].Value = ICDTenModel.InputCode;
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

		private int DctICDTenUpdate(PEIS.Model.DctICDTen ICDTenModel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update DctICDTen set ");
			stringBuilder.Append("ICDCNName=@ICDCNName,");
			stringBuilder.Append("ICDENName=@ICDENName,");
			stringBuilder.Append("Code=@Code,");
			stringBuilder.Append("Codea=@Codea,");
			stringBuilder.Append("ID_Creator=@ID_Creator,");
			stringBuilder.Append("Creator=@Creator,");
			stringBuilder.Append("CreateDate=@CreateDate,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_BanOpr=@ID_BanOpr,");
			stringBuilder.Append("BanOperator=@BanOperator,");
			stringBuilder.Append("BanDate=@BanDate,");
			stringBuilder.Append("BanDescribe=@BanDescribe,");
			stringBuilder.Append("LevelA=@LevelA,");
			stringBuilder.Append("LevelB=@LevelB,");
			stringBuilder.Append("LevelC=@LevelC,");
			stringBuilder.Append("LevelD=@LevelD,");
			stringBuilder.Append("LevelE=@LevelE,");
			stringBuilder.Append("LevelTree=@LevelTree,");
			stringBuilder.Append("Class=@Class,");
			stringBuilder.Append("Tag=@Tag,");
			stringBuilder.Append("ICDtoSection=@ICDtoSection,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("InputCode=@InputCode");
			stringBuilder.Append(" where ID_ICD=@ID_ICD");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ICDCNName", SqlDbType.VarChar),
				new SqlParameter("@ICDENName", SqlDbType.VarChar),
				new SqlParameter("@Code", SqlDbType.VarChar),
				new SqlParameter("@Codea", SqlDbType.VarChar),
				new SqlParameter("@ID_Creator", SqlDbType.Int, 4),
				new SqlParameter("@Creator", SqlDbType.VarChar),
				new SqlParameter("@CreateDate", SqlDbType.DateTime),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_BanOpr", SqlDbType.Int, 4),
				new SqlParameter("@BanOperator", SqlDbType.VarChar),
				new SqlParameter("@BanDate", SqlDbType.DateTime),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar),
				new SqlParameter("@LevelA", SqlDbType.Int, 4),
				new SqlParameter("@LevelB", SqlDbType.Int, 4),
				new SqlParameter("@LevelC", SqlDbType.Int, 4),
				new SqlParameter("@LevelD", SqlDbType.Int, 4),
				new SqlParameter("@LevelE", SqlDbType.Int, 4),
				new SqlParameter("@LevelTree", SqlDbType.Int, 4),
				new SqlParameter("@Class", SqlDbType.VarChar),
				new SqlParameter("@Tag", SqlDbType.VarChar),
				new SqlParameter("@ICDtoSection", SqlDbType.VarChar),
				new SqlParameter("@Note", SqlDbType.VarChar),
				new SqlParameter("@InputCode", SqlDbType.VarChar),
				new SqlParameter("@ID_ICD", SqlDbType.Int, 4)
			};
			array[0].Value = ICDTenModel.ICDCNName;
			array[1].Value = ICDTenModel.ICDENName;
			array[2].Value = ICDTenModel.Code;
			array[3].Value = ICDTenModel.Codea;
			array[4].Value = ICDTenModel.ID_Creator;
			array[5].Value = ICDTenModel.Creator;
			array[6].Value = ICDTenModel.CreateDate;
			array[7].Value = ICDTenModel.Is_Banned;
			array[8].Value = ICDTenModel.ID_BanOpr;
			array[9].Value = ICDTenModel.BanOperator;
			array[10].Value = ICDTenModel.BanDate;
			array[11].Value = ICDTenModel.BanDescribe;
			array[12].Value = ICDTenModel.LevelA;
			array[13].Value = ICDTenModel.LevelB;
			array[14].Value = ICDTenModel.LevelC;
			array[15].Value = ICDTenModel.LevelD;
			array[16].Value = ICDTenModel.LevelE;
			array[17].Value = ICDTenModel.LevelTree;
			array[18].Value = ICDTenModel.Class;
			array[19].Value = ICDTenModel.Tag;
			array[20].Value = ICDTenModel.ICDtoSection;
			array[21].Value = ICDTenModel.Note;
			array[22].Value = ICDTenModel.InputCode;
			array[23].Value = ICDTenModel.ID_ICD;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveFeeReport(PEIS.Model.BusFeeReport FeeReportModel)
		{
			int result;
			if (FeeReportModel.ID_FeeReport > 0)
			{
				result = this.FeeReportUpdate(FeeReportModel);
			}
			else
			{
				result = this.FeeReportAdd(FeeReportModel);
			}
			return result;
		}

		public int FeeReportAdd(PEIS.Model.BusFeeReport model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusFeeReport(");
			stringBuilder.Append("ID_Fee,FeeName,ReportKey,ImageUrl,Note,Is_Banned,ID_Operator,Operator,OperateDate,OperateType,BanDescribe)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ID_Fee,@FeeName,@ReportKey,@ImageUrl,@Note,@Is_Banned,@ID_Operator,@Operator,@OperateDate,@OperateType,@BanDescribe)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Fee", SqlDbType.Int, 4),
				new SqlParameter("@FeeName", SqlDbType.VarChar, 50),
				new SqlParameter("@ReportKey", SqlDbType.VarChar, 50),
				new SqlParameter("@ImageUrl", SqlDbType.VarChar, 256),
				new SqlParameter("@Note", SqlDbType.VarChar, 2000),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Operator", SqlDbType.Int, 4),
				new SqlParameter("@Operator", SqlDbType.VarChar, 30),
				new SqlParameter("@OperateDate", SqlDbType.DateTime),
				new SqlParameter("@OperateType", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar, 50)
			};
			array[0].Value = model.ID_Fee;
			array[1].Value = model.FeeName;
			array[2].Value = model.ReportKey;
			array[3].Value = model.ImageUrl;
			array[4].Value = model.Note;
			array[5].Value = model.Is_Banned;
			array[6].Value = model.ID_Operator;
			array[7].Value = model.Operator;
			array[8].Value = model.OperateDate;
			array[9].Value = model.OperateType;
			array[10].Value = model.BanDescribe;
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

		public int FeeReportUpdate(PEIS.Model.BusFeeReport model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusFeeReport set ");
			stringBuilder.Append("ID_Fee=@ID_Fee,");
			stringBuilder.Append("FeeName=@FeeName,");
			stringBuilder.Append("ReportKey=@ReportKey,");
			stringBuilder.Append("ImageUrl=@ImageUrl,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_Operator=@ID_Operator,");
			stringBuilder.Append("Operator=@Operator,");
			stringBuilder.Append("OperateDate=@OperateDate,");
			stringBuilder.Append("OperateType=@OperateType,");
			stringBuilder.Append("BanDescribe=@BanDescribe");
			stringBuilder.Append(" where ID_FeeReport=@ID_FeeReport");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ID_Fee", SqlDbType.Int, 4),
				new SqlParameter("@FeeName", SqlDbType.VarChar, 50),
				new SqlParameter("@ReportKey", SqlDbType.VarChar, 50),
				new SqlParameter("@ImageUrl", SqlDbType.VarChar, 256),
				new SqlParameter("@Note", SqlDbType.VarChar, 2000),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Operator", SqlDbType.Int, 4),
				new SqlParameter("@Operator", SqlDbType.VarChar, 30),
				new SqlParameter("@OperateDate", SqlDbType.DateTime),
				new SqlParameter("@OperateType", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_FeeReport", SqlDbType.Int, 4)
			};
			array[0].Value = model.ID_Fee;
			array[1].Value = model.FeeName;
			array[2].Value = model.ReportKey;
			array[3].Value = model.ImageUrl;
			array[4].Value = model.Note;
			array[5].Value = model.Is_Banned;
			array[6].Value = model.ID_Operator;
			array[7].Value = model.Operator;
			array[8].Value = model.OperateDate;
			array[9].Value = model.OperateType;
			array[10].Value = model.BanDescribe;
			array[11].Value = model.ID_FeeReport;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveExamItemGroup(PEIS.Model.BusExamItemGroup ExamItemGroupModel)
		{
			int result;
			if (ExamItemGroupModel.ID_ExamItemGroup > 0)
			{
				result = this.ExamItemGroupUpdate(ExamItemGroupModel);
			}
			else
			{
				result = this.ExamItemGroupAdd(ExamItemGroupModel);
			}
			return result;
		}

		public int ExamItemGroupAdd(PEIS.Model.BusExamItemGroup model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into BusExamItemGroup(");
			stringBuilder.Append("ExamItemGroupName,InputCode,DispOrder,Note,Is_Banned,ID_Operator,Operator,OperateDate,OperateType,BanDescribe)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ExamItemGroupName,@InputCode,@DispOrder,@Note,@Is_Banned,@ID_Operator,@Operator,@OperateDate,@OperateType,@BanDescribe)");
			stringBuilder.Append(";select @@IDENTITY");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamItemGroupName", SqlDbType.VarChar, 120),
				new SqlParameter("@InputCode", SqlDbType.VarChar, 20),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar, 200),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Operator", SqlDbType.Int, 4),
				new SqlParameter("@Operator", SqlDbType.VarChar, 30),
				new SqlParameter("@OperateDate", SqlDbType.DateTime),
				new SqlParameter("@OperateType", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar, 50)
			};
			array[0].Value = model.ExamItemGroupName;
			array[1].Value = model.InputCode;
			array[2].Value = model.DispOrder;
			array[3].Value = model.Note;
			array[4].Value = model.Is_Banned;
			array[5].Value = model.ID_Operator;
			array[6].Value = model.Operator;
			array[7].Value = model.OperateDate;
			array[8].Value = model.OperateType;
			array[9].Value = model.BanDescribe;
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

		public int ExamItemGroupUpdate(PEIS.Model.BusExamItemGroup model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update BusExamItemGroup set ");
			stringBuilder.Append("ExamItemGroupName=@ExamItemGroupName,");
			stringBuilder.Append("InputCode=@InputCode,");
			stringBuilder.Append("DispOrder=@DispOrder,");
			stringBuilder.Append("Note=@Note,");
			stringBuilder.Append("Is_Banned=@Is_Banned,");
			stringBuilder.Append("ID_Operator=@ID_Operator,");
			stringBuilder.Append("Operator=@Operator,");
			stringBuilder.Append("OperateDate=@OperateDate,");
			stringBuilder.Append("OperateType=@OperateType,");
			stringBuilder.Append("BanDescribe=@BanDescribe");
			stringBuilder.Append(" where ID_ExamItemGroup=@ID_ExamItemGroup");
			SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("@ExamItemGroupName", SqlDbType.VarChar, 120),
				new SqlParameter("@InputCode", SqlDbType.VarChar, 20),
				new SqlParameter("@DispOrder", SqlDbType.Int, 4),
				new SqlParameter("@Note", SqlDbType.VarChar, 200),
				new SqlParameter("@Is_Banned", SqlDbType.Bit, 1),
				new SqlParameter("@ID_Operator", SqlDbType.Int, 4),
				new SqlParameter("@Operator", SqlDbType.VarChar, 30),
				new SqlParameter("@OperateDate", SqlDbType.DateTime),
				new SqlParameter("@OperateType", SqlDbType.Int, 4),
				new SqlParameter("@BanDescribe", SqlDbType.VarChar, 50),
				new SqlParameter("@ID_ExamItemGroup", SqlDbType.Int, 4)
			};
			array[0].Value = model.ExamItemGroupName;
			array[1].Value = model.InputCode;
			array[2].Value = model.DispOrder;
			array[3].Value = model.Note;
			array[4].Value = model.Is_Banned;
			array[5].Value = model.ID_Operator;
			array[6].Value = model.Operator;
			array[7].Value = model.OperateDate;
			array[8].Value = model.OperateType;
			array[9].Value = model.BanDescribe;
			array[10].Value = model.ID_ExamItemGroup;
			return DbHelperSQL.ExecuteSql(stringBuilder.ToString(), array);
		}

		int ICommonConfig.SaveExamItemGroupExamRel(int ID_ExamItemGroup, string newExamItemIDStrs)
		{
			int result;
			if (ID_ExamItemGroup <= 0)
			{
				result = 0;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(" UPDATE [BusExamItem] SET [ID_ExamItemGroup] = null WHERE  [ID_ExamItemGroup] = " + ID_ExamItemGroup.ToString() + ";");
				if (!string.IsNullOrEmpty(newExamItemIDStrs))
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						" UPDATE [BusExamItem] SET [ID_ExamItemGroup] = ",
						ID_ExamItemGroup.ToString(),
						" WHERE  [ID_ExamItem] in ( ",
						newExamItemIDStrs,
						"); "
					}));
				}
				int num = DbHelperSQL.ExecuteSql(stringBuilder.ToString());
				result = num;
			}
			return result;
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
	}
}
