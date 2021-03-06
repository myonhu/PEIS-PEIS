﻿
var curFixed = 2; //有效数字
var choiceBusSetText = "----";
var type = jQuery("#type").val(); //为当前操作类型，有Add、Edit值
var allBusSet = '';  //获取所有的套餐内容
var allHiddFeeWay;
var curChargeAction = "";
var defalutImagSrc = "/template/blue/images/icons/nohead.gif";
//模块名称
var modelName = jQuery("#modelName").val();
if (modelName.toLowerCase() == "GetCustomerCharges".toLowerCase()) {
    jQuery("#btnUnCharge").hide();
}
else if (modelName.toLowerCase() == "ReturnCustomerCharges".toLocaleLowerCase()) {
    jQuery("#btnCharge").hide();
}
else {
    jQuery("#btnCharge").show();
    jQuery("#btnCharge").show();
}
/// <summary>
/// 文档加载完成，为扫描框设置焦点，并注册鼠标悬停选中事件
/// </summary>
$(document).ready(function () {
    allBusSet = jQuery("#hidslBusSet").html();
    allHiddFeeWay = jQuery("#hidslFeeWay");
    //设置非扫描区域隐藏
    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").hide();
    jQuery("#tblSearch tbody tr[id!='loading']").hide();
    //设置默认选中
    jQuery("#txtID_Customer").focus();
    jQuery("#txtID_Customer").bind("mouseover", function () {
        jQuery(this).select();
    });

    //
    jQuery("#Checkbox2").change(function () {
        var checked = jQuery(this).attr("checked");
        jQuery("#tblTeamTaskGroupFee tbody tr td input[name='ItemCheckbox']").each(function () {
            jQuery(this).attr('checked', checked);
        })
    });
});
/// <summary>
/// 为页面注册按键事件，如果光标在扫描框中，且按键为回车时，将自动触发检索客户信息
/// </summary>
function OnFormKeyUp(e) {
    var curEvent = window.event || e;
    var id = document.activeElement.id;
    if (curEvent.keyCode == 13)//回车事件
    {
        //如果是在搜索中
        if (id == "txtID_Customer") {
            //这里绑定客户所有的收费项目、费用统计信息
            //判断是否是正常体检号
            DoSearchCustomerInfoAndCustFeeInfo();
        }
        else if (id == "txtInvoice")//扫描发票
        {
            curChargeAction == "Charge"
            DoChargeCallback();
            //CloseDialogWindow();
        }
        else if (id == "txtUnChargeInvoice")//退费扫描发票
        {
            DoUnChargeInvoice();
        }
    }
    if (id == "txtID_Customer" && (curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40)) {

    }
    return false;
}

function DoScrollToBottom() {
    window.scrollTo(0, document.body.scrollHeight - 100);
}

/// <summary>
///通过客户编号[体检号]检索客户基本信息和客户收费项目信息
/// </summary>
function DoSearchCustomerInfoAndCustFeeInfo() {

    var objID = "txtID_Customer";
    var Key = "ID_Customer";
    var modelName = jQuery("#modelName").val();
    var KeyValue = jQuery.trim(jQuery("#" + objID).val());
    if (KeyValue == "" || !isCustomerExamNo(KeyValue)) {
        return false;
    }

    //组建请求参数
    var Is_Org = 0;
    var data = { action: "GetCustomerByIDCustomer", ID_Customer: KeyValue, type: type, modelName: modelName };
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: data,
        cache: false,
        dataType: "json",
        success: function (msg) {
            SetCustomerInfo(msg, KeyValue);
            //这里绑定客户基本信息
        }
    });
    jQuery("#" + objID).blur();
    //jQuery("#" + objID).select();
    return false;
}
//设置用户基本信息
function SetCustomerInfo(msg, ID_Customer) {
    if (msg == null || msg == undefined)
        return false;
    var item;
    var dataList0 = msg.dataList0;
    var dataList1 = msg.dataList1;
    for (var c = 0; c < dataList0.length; c++) {
        item = dataList0[c];
        jQuery("#lblCustomerName").text(item.CustomerName);
        jQuery("#lblSex").text(item.GenderName);
        jQuery("#lblIDCard").text(item.IDCard);
        jQuery("#lblTel").text(item.MobileNo);
        jQuery("#lblMarriedName").text(item.MarriageName);
        //设置头像
        if (item.Base64Photo == "") {
            jQuery("#HeadImg").attr("src", defalutImagSrc);
        }
        else {
            jQuery("#HeadImg").attr("src", "data:image/gif;base64," + item.Base64Photo + "");
        }
        //绑定用户基本信息
        //jQuery("#lblAge").text(catcAge(item.date));
        jQuery("#lblAge").text(item.Age);
        //jQuery("#lblAge").text(item.Age); //xmhuang 2013-11-05 客户年纪计算已有后台返回，此处直接获取Age属性即可
        jQuery("#tblSearch tbody tr[id='loading']").hide();
        jQuery("#tblSearch tbody tr[id!='loading']").show();



    }
    for (var c = 0; c < dataList1.length; c++) {
        item = dataList1[c];
        //绑定用户基本信息
        jQuery("#lblID_Customer").text(item.ID_Customer);
        jQuery("#lblID_Customer").data("Code128c", item.Code128c);
        jQuery("#lblRegisterDate").text(item.SubScribDate);
        jQuery("#lblTeamName").text(item.TeamName);
        jQuery("#lblExamType").text(item.ExamTypeName);

        jQuery("#lblOperateDate").text(item.OperateDate);
        jQuery("#lblOperator").text(item.Operator);

        var data = { Is_GuideSheetPrinted: item.Is_GuideSheetPrinted, Is_Subscribed: item.Is_Subscribed };
        jQuery("#txtID_Customer").data("data", data);

        //设置补打预约凭证按钮的显示和隐藏 xmhuang2013-11-05 Begin
        var card = item.ID_Customer;
        if (isCustomerExamNo(card))//如果是体检号
        {
            var TypeCode = card.substring(1, 2); //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
            if (TypeCode == 3)//个人预约
            {
                jQuery("#btnReprintCustomerCredence").show();
            }
            else {
                jQuery("#btnReprintCustomerCredence").hide();
            }
        }
        //设置补打预约凭证按钮的显示和隐藏 xmhuang2013-11-05 End
    }
    if (dataList0.length == 0) {
        jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").remove();
        jQuery("#tblTeamTaskGroupFee tbody tr[id='loading']").show();
    }
    if (dataList1.length == 0) {
        jQuery("#tblSearch tbody tr[id='loading']").show();
        jQuery("#tblSearch tbody tr[id!='loading']").hide();
        //设置总体价格汇总
        jQuery("#lblSumHeaderX").html("");
        //var ysjg = 0, ssjg = 0, ysfy = 0, yjfy = 0;
        var curHtml = "收费项目(<label style='color:red;font-size:13px; text-decoration:underline;'>共：0个；原始价格：0元；应收价格：0元；应收费用：0元；应缴费用：0元</label>)";
        var data = { ysjg: 0, ssjg: 0, ysfy: 0, yjfy: 0 };
        jQuery("#divSumHeader").data("sumData", data);
        jQuery("#divSumHeader").html(curHtml);
        jQuery("#lblSumHeaderX").html("应收费用：0元");
    }
    else {
        GetCustomerExamPhysicalInfo(ID_Customer);
    }
    jQuery("#txtID_Customer").focus();
    jQuery("#txtID_Customer").select();
}
/// <summary>
/// 获取用户体检信息和套餐内容
/// </summary>
/// <returns></returns>
function GetCustomerExamPhysicalInfo(ID_Customer) {

    if (ID_Customer != "") {
        var Is_Org = 0;
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxRegiste.aspx",
            data: { action: 'GetCustomerExamPhysicalInfo',
                ID_ArcCustomer: "",
                ID_Customer: ID_Customer
            },
            cache: false,
            dataType: "json",
            success: function (msg) {
                if (msg.dataList0.length > 0) {
                    jQuery(msg.dataList0).each(function (i, item) {
                        if (item.Is_Team == "True") {
                            Is_Org = 1;
                            jQuery("#lblTeamName").text(item.TeamName);
                        }
                        else {
                            Is_Org = 0;
                            jQuery("#lblTeamName").text("");
                        }
                        //设置用户基本信息
                        jQuery("#lblID_Customer").text(item.ID_Customer);
                    });
                }
                if (msg.dataList1.length > 0) {
                    ReadBustFeeData(Is_Org, msg.dataList1);
                }
            }
        });
    }
}
function ReadBustFeeData(Is_Org, dataList) {
    if (dataList != undefined) {
        if (dataList.length > 0) {
            var CustCssStyle = "";
            var RowNum = 0;
            var ysjg = 0, ssjg = 0, ysfy = 0, yjfy = 0, ItemCheckbox = "ItemCheckbox";
            //绑定套餐信息
            var newContent = '', XFeeChargeStaute = "";
            var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("tblTemplateCustFee");
            var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
            var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
            var IsCanShowInvoiceButton = true; //是否可显示发票补录按钮[发票补录按钮显示前提是所有收费项目都已经完成收费]
            jQuery(dataList).each(function (i, item) {
                XFeeChargeStaute = "";
                ItemCheckbox = "ItemCheckbox"
                ysjg += parseFloat(item.Price);
                ssjg += parseFloat(item.FactPrice);
                RowNum++;

                if (item.ID_TeamTaskGroup != "") {
                    ItemCheckbox = "ItemCheckboxX";
                    CustCssStyle = "TeamYellow";
                    //判断收费状态
                    if (item.FeeChargeStaute == "2") {
                        CustCssStyle = "TeamYellow";
                        item.FeeChargeStaute = "√";
                        XFeeChargeStaute = "√";
                    }
                    //判断是否是已检项目，已检项目不允许退费Is_Examined
                    else if (item.FeeChargeStaute == "1") {
                        ysfy += parseFloat(item.FactPrice);
                        item.FeeChargeStaute = "√";
                        ItemCheckbox = "ItemCheckbox"//xmhuang 2013-11-09 团体收费项目允许退费
                        CustCssStyle = "Green"; //xmhuang 2013-11-09 团体收费项目允许退费
                        //CustCssStyle = "TeamGreen";//xmhuang 2013-11-09 团体收费项目允许退费

                    }
                    else if (item.FeeChargeStaute == "0") {
                        IsCanShowInvoiceButton = false;
                        if (item.FeeWayName == "现金") {
                            yjfy += parseFloat(item.FactPrice);
                        }
                        item.FeeChargeStaute = "";
                        ItemCheckbox = "ItemCheckbox"//xmhuang 2013-11-09 团体未收费需显示复选框
                        CustCssStyle = "Red"; //xmhuang 2013-11-09 团体未收费需显示复选框
                        //CustCssStyle = "TeamRed";//xmhuang 2013-11-09 团体未收费需显示复选框
                    }
                }
                else {
                    //判断收费状态
                    if (item.FeeChargeStaute == "2") {
                        ItemCheckbox = "ItemCheckboxX";
                        CustCssStyle = "Yellow";
                        item.FeeChargeStaute = "√";
                        XFeeChargeStaute = "√";
                    }
                    //判断是否是已检项目，已检项目不允许退费Is_Examined
                    else if (item.FeeChargeStaute == "1") {
                        ysfy += parseFloat(item.FactPrice);
                        item.FeeChargeStaute = "√";
                        CustCssStyle = "Green";
                    }
                    else if (item.FeeChargeStaute == "0") {
                        IsCanShowInvoiceButton = false;
                        yjfy += parseFloat(item.FactPrice);
                        item.FeeChargeStaute = "";
                        CustCssStyle = "Red";
                    }

                }
                if (item.Is_Examined == "True") {
                    CustCssStyle = "TeamYellow";
                    ItemCheckbox = "ItemCheckboxX";
                    item.Is_Examined = "已检";
                }
                else {
                    item.Is_Examined = "未检";
                }
                newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                            .replace(/@ID_Customer/gi, item.ID_Customer)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)
                            .replace(/@FeeChargeStaute/gi, item.FeeChargeStaute)
                            .replace(/@Is_FeeCharged/gi, item.Is_FeeCharged)
                            .replace(/@Is_FeeRefund/gi, item.Is_FeeRefund)
                            .replace(/@Price/gi, item.Price)
                            .replace(/@FactPrice/gi, item.FactPrice)
                            .replace(/@Discount/gi, (item.Discount == "10" ? "" : item.Discount))
                            .replace(/@DscctName/gi, item.Discount == "10" ? "" : item.DiscounterName)
                // .replace(/@CustFeeChargeState/gi, item.Is_FeeCharged)
                            .replace(/@FeeCharger/gi, item.FeeCharger)
                            .replace(/@FeeChargeDate/gi, item.FeeChargeDate)
                            .replace(/@FeeRefunderName/gi, item.FeeRefunder)
                             .replace(/@FeeRefunderDate/gi, item.FeeRefunderDate)
                            .replace(/@Is_Examined/gi, item.Is_Examined)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                            .replace(/@date/gi, item.date)
                            .replace(/@FeeWayName/gi, item.FeeWayName)
                            .replace(/@RowNum/gi, RowNum)
                            .replace(/@CustCssStyle/gi, CustCssStyle)
                            .replace(/@ItemCheckbox/gi, ItemCheckbox)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@Is_Printed/gi, item.Is_Printed)
                            .replace(/@XFeeChargeStaute/gi, XFeeChargeStaute)
                            ;
            });

            if (newContent != '') {

                /*设置补录按钮的显示隐藏 xmhuang 2013-10-16 Begin*/
                if (modelName.toLowerCase() == "GetCustomerCharges".toLowerCase()) {
                    if (IsCanShowInvoiceButton) {
                        jQuery("#btnInvoice").show();
                    }
                    else {
                        jQuery("#btnInvoice").hide();
                    }
                }
                /*设置补录按钮的显示隐藏 xmhuang 2013-10-16  End*/

                //清除套餐
                jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").remove();
                jQuery("#tblTeamTaskGroupFee tbody tr[id='loading']").hide();
                jQuery("#tblTeamTaskGroupFee tbody").append(newContent);
                newContent = "";
                //设置总体价格汇总
                jQuery("#lblSumHeaderX").html("");
                //var ysjg = 0, ssjg = 0, ysfy = 0, yjfy = 0;
                ysjg = parseFloat(ysjg).toFixed(curFixed);
                ssjg = parseFloat(ssjg).toFixed(curFixed);
                ysfy = parseFloat(ysfy).toFixed(curFixed);
                yjfy = parseFloat(yjfy).toFixed(curFixed);
                //                var curHtml = "收费项目(<label style='color:red;font-size:13px; text-decoration:underline;'>共：" + RowNum + "个；原始价格：" + ysjg.toString() + "元；应收价格：" + ssjg.toString() + "元；已收费用：" + ysfy.toString() + "元；应交费用：" + yjfy.toString() + "元</label>)";
                //                var data = { ysjg: ysjg, ssjg: ssjg, ysfy: ysfy, yjfy: yjfy };
                //                jQuery("#divSumHeader").data("sumData", data);
                //                jQuery("#divSumHeader").html(curHtml);
                //                jQuery("#lblSumHeaderX").html("应交费用：" + yjfy + "元");
                SumJG();
                if (modelName.toLowerCase() == "GetCustomerCharges".toLowerCase()) {
                    SetCheckAll();
                }
                //DoScrollToBottom();
            }
            else {
                jQuery("#tblTeamTaskGroupFee tbody tr[id='loading']").show();
            }

        }
    }
    else {
        jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").remove();
        jQuery("#tblTeamTaskGroupFee tbody tr[id='loading']").show();
    }

    //如果当前操作为收费操作，判断是否可以直接打印指引单
    if (curChargeAction == "Charge" && jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][id_teamtaskgroup=''][feechargestaute='']").length == 0) {
        DoPrintReport();
    }
}
/// <summary>
/// 修改人：xmhuang
/// 修改日期：2013-11-22
///修改内容：修改个人登记费用统计显示
///原价：所有收费项目(不包含7已退项目)原始价格的总和
function SumJG() {
    var xmzj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").length; //项目合计个数
    var YJ = 0; //原价
    var ZH = 0; //折后
    var JZ = 0; //记账
    var TF = 0; //退费
    var YSFY = 0; //应收费用
    var YSJE = 0; //已收金额
    var DS = 0; //待收
    var sumYJFY = 0; //应缴费用
    var sumUnPrintyjfy = 0; //分批应缴费用
    var ParentIsPrinted = "";
    var isFeecharged = false; //是否付费
    var isFeerefund = false; //是否退费
    var feeTypeName = ""; //收费方式名称
    var curZK = 0; //当前折扣
    var curSJ = 0; //当前折扣后价格

    jQuery("#tblTeamTaskGroupFee tbody tr td [name='yj']").each(function () {
        curZK = jQuery.trim(jQuery(this).parent().parent().find("[name = 'zk']").text()); //获取当前折扣
        if (curZK == "") {
            curZK = 10;
        }
        curSJ = parseFloat(jQuery.trim(jQuery(this).text())) * parseFloat(curZK) / 10; //计算折扣后价格
        feeTypeName = jQuery.trim(jQuery(this).parent().parent().find("[name = 'fffs']").text()); //获取当前收费方式名称

        isFeecharged = jQuery(this).parent().parent().attr("is_feecharged"); //获取是否付费标记
        isFeerefund = jQuery(this).parent().parent().attr("is_feerefund"); //获取是否退费标记
        if (isFeecharged == 0) {
            isFeecharged = "False"
        }
        if (isFeerefund == 0) {
            isFeerefund = "False"
        }

        if (isFeerefund == "True") //获取退项项目
        {
            if (feeTypeName == "现金")//获取现金项目
            {
                TF += parseFloat(curSJ);                       //汇总退费
            }
        }
        else if (isFeerefund == "False") //获取非退项项目
        {
            YJ += parseFloat(jQuery.trim(jQuery(this).text())); //汇总原价
            ZH += parseFloat(curSJ);                        //汇总折后
            if (feeTypeName == "记账")//获取记账项目
            {
                JZ += parseFloat(curSJ);                    //汇总记账
            }
            else//获取非记账项目
            {
                YSFY += parseFloat(curSJ);                  //汇总应收费用

                if (feeTypeName == "现金") {
                    if (isFeecharged == "True") //获取已收费的项目
                    {
                        YSJE += parseFloat(curSJ);         //汇总已收金额
                    }
                }
                else if (feeTypeName == "统一收费")//获取统一收费的项目
                {
                    YSJE += parseFloat(curSJ);            //汇总已收金额
                }
            }

        }


        if (isFeecharged == "False") //获取未收费的项目
        {
            if (feeTypeName == "现金")//获取现金项目
            {
                DS += parseFloat(curSJ);                        //汇总待收
            }
        }


        /*xmhuang 2013-10-15 记录未打印、未收费的现金项目的应交费用 Begin*/

        if (feeTypeName == "现金")//如果是现金项目
        {
            if (isFeecharged == "False") //如果是未收费
            {
                sumYJFY += parseFloat(curSJ);                   //汇总未付费总金额
                ParentIsPrinted = jQuery(this).parent().parent().attr("is_printed");
                if (ParentIsPrinted == undefined || ParentIsPrinted == 0)//如果是未打印
                {
                    sumUnPrintyjfy += parseFloat(curSJ);        //汇总分批未付费金额
                }
            }
        }
        /*xmhuang 2013-10-15 记录未打印、未收费的现金项目的应交费用 End*/

    });
    sumYJFY = parseFloat(sumYJFY).toFixed(curFixed); //汇总本次应交费用
    sumUnPrintyjfy = parseFloat(sumUnPrintyjfy).toFixed(curFixed); //汇总未付费
    YJ = parseFloat(YJ).toFixed(curFixed); //汇总原价
    ZH = parseFloat(ZH).toFixed(curFixed); //汇总折后
    JZ = parseFloat(JZ).toFixed(curFixed); //汇总记账
    TF = parseFloat(TF).toFixed(curFixed); //汇总退费
    YSFY = parseFloat(YSFY).toFixed(curFixed); //汇总应收费用
    YSJE = parseFloat(YSJE).toFixed(curFixed); //汇总已收金额
    DS = parseFloat(DS).toFixed(curFixed); //汇总待收

    //    var curHtml = "<label name='kytjxm'>客户预约项目</label>(<label style='color:red;font-size:13px; text-decoration:underline;'>共：" + xmzj + "个；原价合计：" + sumYJ.toString() + "元；折扣合计：" + sumSJ.toString() + "元；实收合计：" + sumXJJE.toString() + "元</label>)";
    //    var data = { YJ: YJ, yjzj: sumYJ, zkzj: sumSJ, zkhzj: sumSJ, xjzf: sumXJJE, jzzf: sumJZJE, yjfy: sumYJFY, sumUnPrintyjfy: sumUnPrintyjfy };
    var curHtml = "<label name='kytjxm'>客户预约项目</label>(<label style='color:red;font-size:13px; text-decoration:underline;'>共" + xmzj + "个；原价：" + YJ.toString() + "；折后：" + ZH.toString() + "；记账：" + JZ.toString() + "；退费：" + TF.toString() + "；应收：" + YSFY.toString() + "；已收：" + YSJE.toString() + "；待收：" + DS.toString() + "</label>)";
    var data = { YJ: YJ, ZH: ZH, JZ: JZ, TF: TF, YSFY: YSFY, YSJE: YSJE, DS: DS, yjfy: sumYJFY, sumUnPrintyjfy: sumUnPrintyjfy };
    jQuery("#divSumHeader").data("sumData", data);
    jQuery("#divSumHeader").html(curHtml);
    jQuery("#lblSumHeaderX").html("本次应交费用：" + sumYJFY + "元");

    /* 修改人：黄兴茂 修改日期：2013-08-13 修改内容：设置客户收费项目显示内容 Begin*/
    if (jQuery('#modelName').val() != undefined) {
        if (jQuery('#modelName').val().toLowerCase() == "regist") {
            jQuery("#divSumHeader label[name='kytjxm'").text("客户预约项目");
        }
        else if (jQuery('#modelName').val().toLowerCase() == "sign") {
            jQuery("#divSumHeader label[name='kytjxm'").text("客户登记项目");
        }
    }
    else {
        jQuery("#divSumHeader label[name='kytjxm'").text("客户体检项目");
    }
    /* 修改人：黄兴茂 修改日期：2013-08-13 修改内容：设置客户收费项目显示内容 End*/
}
function SetCheckAll() {
    if (modelName.toLowerCase() == "GetCustomerCharges".toLowerCase())//交费
    {
        if (jQuery("#tblTeamTaskGroupFee tbody tr[feechargestaute='']").length > 0) {
            jQuery("#tblTeamTaskGroupFee tbody tr td input[name='ItemCheckbox']").each(function () {
                if (jQuery(this).parent().parent().attr("feechargestaute") == "") {
                    jQuery(this).attr('checked', true);
                }
            });
        }
    }
    else if (modelName.toLowerCase() == "ReturnCustomerCharges".toLowerCase()) {
        if (jQuery("#tblTeamTaskGroupFee tbody tr[feechargestaute='√']").length > 0) {
            jQuery("#tblTeamTaskGroupFee tbody tr td input[name='ItemCheckbox']").each(function () {
                if (jQuery(this).parent().parent().attr("feechargestaute") == "√") {
                    jQuery(this).attr('checked', true);
                }
            });
        }
    }

}
/// <summary>
/// 全选子元素
///obj:父元素
///ShowDataElementID: 需要设置元素选中状态的tableID
/// </summary>
function CheckAllCustFee(obj) {
    jQuery("#tblTeamTaskGroupFee tbody tr td input[name='ItemCheckbox']").each(function () {
        jQuery(this).attr('checked', obj.checked);
    })
}
/// <summary>
/// 收费
/// </summary>
/// <returns></returns>
function Charge() {
    //判断有无选中的收费项目，并且只能是费的项目
    var checkedObj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'] td input:checked");
    var checkedObjCount = jQuery(checkedObj).length;
    if (checkedObjCount == 0) {
        art.dialog({
            lock: true, fixed: true, opacity: 0.3,
            content: '对不起，请您勾选需要收费的项目！',
            icon: 'info',
            ok: true
        });
        return false;
    }
    else {

        if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute!=''][feechargestaute!='已退'] td input:checked").length > 0) {
            art.dialog({
                lock: true, fixed: true, opacity: 0.3,
                content: '对不起，您勾选的项目包含了已收费的项目,是否自动过滤?',
                icon: 'error',
                ok: function () {
                    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute!=''] td input:checked").removeAttr("checked");
                    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute=''] td input:checked").length > 0) {
                        DoCharge("Charge");
                        this.close();
                    }
                    else {
                        art.dialog({
                            lock: true, fixed: true, opacity: 0.3,
                            content: '对不起，您勾选的项目中不包含任何费项目，不能进行收费操作！',
                            icon: 'info',
                            ok: true
                        });
                    }
                },
                cancel: true
            });
            return false;
        }
        else {
            DoCharge("Charge");
        }
    }

}
var InvoiceContent = jQuery("#tblInvoiceTemplate tbody").html(); //xmhuang 2013-10-15 获取退费发票table模板
var InvoiceListContent = jQuery("#InvoiceTemplate").html(); //xmhuang 2013-10-15 获取退费发票列表
function DeleteMe(obj) {
    if (jQuery(obj).parent().length > 0) {
        jQuery(obj).parent().remove();
    }
}

function CalteAllCustFee(ChargeAction) {
    var content = "", allFeeID = "", ysjg = 0, ssjg = 0, ysfy = 0, yjfy = 0, count = 0, obj = "";
    if (ChargeAction == "Charge") {
        obj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute=''] td input:checked");
    }
    else if (ChargeAction == "UnCharge") {
        obj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute='√'] td input:checked");
    }
    jQuery(obj).each(function () {
        allFeeID += jQuery(this).parent().parent().attr("id_fee") + ",";
        ysjg += parseFloat(jQuery.trim(jQuery(this).parent().parent().find("td label[name='yj']").text()));
        ssjg += parseFloat(jQuery.trim(jQuery(this).parent().parent().find("td label[name='sj']").text()));
        if (jQuery(this).parent().parent().attr("feechargestaute") == "√") {
            ysfy += parseFloat(jQuery.trim(jQuery(this).parent().parent().find("td label[name='sj']").text()));
        }
        else if (jQuery(this).parent().parent().attr("feechargestaute") == "") {
            yjfy += parseFloat(jQuery.trim(jQuery(this).parent().parent().find("td label[name='sj']").text()));
        }
        count++;
    });
    if (ChargeAction == "Charge") {

        //        content = "<div>共:" + count + "个项目;原始价格:" 
        //        + parseFloat(ysjg).toFixed(curFixed) + "元;应收价格:" + parseFloat(ssjg).toFixed(curFixed) + "元;已收费用:"
        //        + parseFloat(ysfy).toFixed(curFixed) + "元;应交费用:" 
        //        + parseFloat(yjfy).toFixed(curFixed) + "元</div>";
        //        content += "<br/><div><label>发票号: </label><input id='txtInvoice' onkeyup='OnFormKeyUp();'/></div>";


        content = '<table class="ModifyPassword">' +
            '<tbody>' +
             '    <tr><td class="left" colspan="2" style="text-align:center;font-size:16px; color:#2255a4; font-weight:bold;padding-bottom:10px;">收费发票扫描</td></tr>' +
            '    <tr><td class="left">收费项目：</td><td>' + count + ' &nbsp;个</td></tr>' +
            '    <tr><td class="left">原始价格：</td><td>' + parseFloat(ysjg).toFixed(curFixed) + '&nbsp;元 </td></tr>' +
        //            '    <tr><td class="left">应收价格：</td><td>' + parseFloat(ssjg).toFixed(curFixed) + '&nbsp;元 </td></tr>' +
        //            '    <tr><td class="left">已收费用：</td><td>' + parseFloat(ysfy).toFixed(curFixed) + '&nbsp;元 </td></tr>' +
            '    <tr><td class="left">应交费用：</td><td><span  style="font-size:16px; color:blue; font-weight:bold; float:left;">' + parseFloat(yjfy).toFixed(curFixed) + '&nbsp;元 </td></tr>' +
            '    <tr><td class="left">发票扫描：</td><td><input id="txtInvoice" onkeyup="OnFormKeyUp();"/></td></tr>' +
             '<tr><td><td></tr>' + InvoiceContent.replace("@InvoiceName", "收费发票号:") +
        '</tbody>' +
            '</table>';
    }
    else if (ChargeAction == "UnCharge") {
        content = "共:" + count + "个项目;总金额:" + parseFloat(ssjg).toFixed(curFixed) + "元";
        content = '<table class="ModifyPassword">' +
            '<tbody>' +
             '    <tr><td class="left" colspan="2" style="text-align:center;font-size:16px; color:#2255a4; font-weight:bold;padding-bottom:10px;">退费发票扫描</td></tr>' +
            '    <tr><td class="left">退费项目：</td><td>' + count + ' &nbsp;个</td></tr>' +
            '    <tr><td class="left">发票扫描：</td><td><input id="txtUnChargeInvoice" onkeyup="OnFormKeyUp();"/> </td></tr>' + InvoiceContent.replace("@InvoiceName", "退费发票号:") +
            '    <tr><td class="left">应退金额：</td><td><label id="hidYTJE" style="display:none;">' + parseFloat(ssjg).toFixed(curFixed) + '</label><span  style="font-size:16px; color:blue; font-weight:bold; float:left;">' + parseFloat(ssjg).toFixed(curFixed) + '&nbsp;元 </td></tr>' +
            '    <tr><td class="left">发票总金额：</td><td><input id="txtAllUnChargeInvoiceMoney"/> </td></tr>' + '<tr><td><td></tr>' +
            '    <tr><td class="left" colspan="2" style="text-align:left;font-size:12px; color:red;  padding-bottom:10px;">部分退费：指对已扫描发票总金额中部分金额进行退费；<br/> 全额退费：指对已扫描发票进行全额退费； <br/> 其中部分退费发票总金额必须大于应退金额。</td></tr>' +
        '</tbody>' +
            '</table>';
    }

    jQuery("#tblTeamTaskGroupFee").data("AllFeeID", allFeeID);
    return content;
}


function DoCharge(ChargeAction) {
    var allUnchargename = "";
    curChargeAction = ChargeAction;
    if (ChargeAction == undefined || ChargeAction == "")
        return false;
    var name = "确认交费";
    if (ChargeAction == "UnCharge") {
        name = "部分退费";
        allUnchargename = "全额退费";
    }
    var title = "";
    if (ChargeAction == "Charge") {
        title = "扫描发票";
    }
    else if (ChargeAction == "UnCharge") {
        title = "退费消息";
    }
    var content = CalteAllCustFee(ChargeAction);
    var button = "";
    if (allUnchargename == "") {
        button = [
        {
            name: name,
            callback: function () {
                //this.time(2);
                //这里执行交费
                return ChargeCallback();
            },
            focus: true
        },
        {
            name: '取消',
            callback: function () {
                this.close();
            }
        }];
    }
    else {
        button = [
        {
            name: name,
            callback: function () {
                //this.time(2);
                //这里执行交费
                return UnChargeAllCallback(0);
            },
            focus: true
        },
        {
            name: allUnchargename,
            callback: function () {
                //this.time(2);
                //这里执行交费
                return UnChargeAllCallback(1);
            }
        },
        {
            name: '取消',
            callback: function () {
                this.close();
            }
        }];
    }
    //计算所有收费项目总金额
    art.dialog({
        id: 'OperWindowFrame',
        title: title,
        lock: true, fixed: true, opacity: 0.3,
        content: content,
        init: function () {
            if (jQuery("#txtInvoice").length > 0) {
                jQuery("#txtInvoice").focus();
            }
            if (jQuery("#txtUnChargeInvoice").length > 0) {
                jQuery("#txtUnChargeInvoice").focus();
            }
        },
        button: button
    });
}
/// <summary>
/// 退费
/// </summary>
/// <returns></returns>
function UnCharge() {
    //判断有无选中的收费项目，并且只能是费的项目
    var checkedObj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'] td input:checked");
    var checkedObjCount = jQuery(checkedObj).length;
    if (checkedObjCount == 0) {
        art.dialog({
            lock: true, fixed: true, opacity: 0.3,
            content: '对不起，请您勾选需要退费的项目！',
            icon: 'info',
            ok: true
        });
        return false;
    }
    else {
        if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute!='√'][feechargestaute!='已退'] td input:checked").length > 0) {
            art.dialog({
                lock: true, fixed: true, opacity: 0.3,
                content: '对不起，您勾选的项目包含了未收费的项目,是否自动过滤?',
                icon: 'error',
                ok: function () {
                    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute!='√'] td input:checked").removeAttr("checked");
                    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][feechargestaute='√'] td input:checked").length > 0) {
                        DoCharge("UnCharge");
                        this.close();
                    }
                    else {
                        art.dialog({
                            lock: true, fixed: true, opacity: 0.3,
                            content: '对不起，您勾选的项目中不包含任何已退费项目，不能进行退费操作！',
                            icon: 'info',
                            ok: true
                        });
                    }
                },
                cancel: true
            });
            return false;
        }

        else {
            DoCharge("UnCharge");
        }
    }
}
///判断是否可以打印指引单
function IsCanPrint(ID_Customer) {
    //GetCustomerPrint
    if (ID_Customer == "")
        return { "dataList": "" };
    else {
        var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
        var flag = false;
        jQuery.ajax({
            type: "GET",
            async: false,
            url: "/Ajax/AjaxRegiste.aspx",
            data: { action: "GetCustomerPrint", ID_Customer: ID_Customer },
            cache: false,
            dataType: "json",
            success: function (msg) {
                allISDeleteDT = msg;
            }
        });
        return allISDeleteDT;
    }
}
/// <summary>
/// 打印指引单
/// </summary>
/// <returns></returns>
function DoPrintReport() {
    var Gender = jQuery.trim(jQuery("#lblSex").text()); //xmhuang 2013-10-12 用于发送接口时使用
    //判断项目是否都已经收费
    var checkedObj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][id_teamtaskgroup=''][feechargestaute='']");
    var checkedObjCount = jQuery(checkedObj).length;
    if (checkedObjCount > 0) {
        art.dialog({
            lock: true, fixed: true, opacity: 0.3,
            content: '对不起，您还有未交费的项目不能打印指引单！',
            icon: 'info',
            ok: true
        });
        return false;
    }
    else {
        //        var data = jQuery("#txtID_Customer").data("data");
        //        var Is_GuideSheetPrinted = data.Is_GuideSheetPrinted;
        //        var Is_Subscribed = data.Is_Subscribed; //lblRegisterDate
        if (jQuery("#tblTeamTaskGroupFee tbody tr[is_printed!='1']").length > 0) {
            var yyDate = jQuery.trim(jQuery("#lblRegisterDate").text());
            var todayDate = RegisteDate;
            var ID_Customer = jQuery.trim(jQuery("#txtID_Customer").val());
            //获取打印数据
            var Data = IsCanPrint(ID_Customer).dataList[0];
            //判断是否是团体
            var Is_Subscribed = Data.Is_Subscribed; //是否预约 1：是，0：否
            var ID_Team = Data.ID_Team; //团体ID
            if (ID_Team != "")//如果是团体成员，则判断时间是否在事件断内
            {
                var TaskExamStartDate = Data.TaskExamStartDate; //团体任务起始时间
                var TaskExamEndDate = Data.TaskExamEndDate; //团体任务起始时间
                if (todayDate >= TaskExamStartDate && todayDate <= TaskExamEndDate) {
                    if (jQuery("#tblTeamTaskGroupFee tbody tr[is_printed!='1']").length > 0) {
                        UpdateCustomerSubscribDateOfTeam_Ajax(ID_Customer, todayDate); //xmhuang 2013-10-12 如果是团体客户在任务时间范围内完成登记，则需要修改实际体检时间为登记时间
                        FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                        SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); //xmhuang 2013-10-12 打印完指引单后向接口发送信息  
                    }
                }
                else {
                    art.dialog({
                        lock: true, fixed: true, opacity: 0.3,
                        content: '客户所在团体预约体检时间为：[开始：' + TaskExamStartDate + '，结束：' + TaskExamEndDate + ']目前不能打印指引单！',
                        icon: 'info',
                        ok: true
                    });
                }
            }
            else {

                //判断是登记还是预约
                if (Is_Subscribed == 1)//如果是预约，则需要判断时间是否为预约时间
                {
                    var SubScribDate = Data.SubScribDate; //预约时间
                    var ID_Customer = jQuery.trim(jQuery("#lblID_Customer").text());
                    if (SubScribDate == todayDate) {
                        if (jQuery("#tblTeamTaskGroupFee tbody tr[is_printed!='1']").length > 0) {
                            FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                            SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); //xmhuang 2013-10-12 打印完指引单后向接口发送信息  
                        }
                    }
                    else {

                        //全部√费则，直接打印预约凭证
                        if (jQuery("#tblTeamTaskGroupFee tbody tr[is_printed!='1']").length >= 0) {
                            var ID_CustomerCode128 = jQuery("#lblID_Customer").data("Code128c"); //  jQuery.trim(jQuery("#lblCode128c").text());
                            var customerName = jQuery.trim(jQuery("#lblCustomerName").text());
                            var departmentName = "健康管理中心";
                            var GenderName = jQuery.trim(jQuery("#lblSex").text());
                            var OperateDate = jQuery.trim(jQuery("#lblOperateDate").text());
                            var Operator = jQuery.trim(jQuery("#lblOperator").text());
                            var SubScribDate = jQuery.trim(jQuery("#lblRegisterDate").text());
                            //是否是预约
                            var TemplateName = "CustomerCredence.frx";
                            var para = {
                                TemplateName: TemplateName,
                                CustomerName: customerName,
                                departmentName: departmentName,
                                SubScribDate: SubScribDate,
                                FeeTypeName: "收费",
                                GenderName: GenderName,
                                OperateDate: OperateDate,
                                Operator: Operator,
                                SubScribDate: SubScribDate
                            }; //直接打印预约凭证
                            
                            var sex = jQuery.trim(jQuery("#lblSex").text());
                            GenderName = "同志";
                            if (sex == "男") {
                                GenderName = "先生";
                            }
                            else if (sex == "女") {
                                GenderName = "女士";
                            }
                            var detailXml = "<CustomerInfo>";
                            detailXml += "<ID_Customer>" + ID_Customer + "</ID_Customer>";
                            detailXml += "<ID_CustomerCode128>" + CheckXmlChar(ID_CustomerCode128) + "</ID_CustomerCode128>";
                            detailXml += "<TemplateName>" + CheckXmlChar(para.TemplateName) + "</TemplateName>";
                            detailXml += "<FeeTypeName>" + CheckXmlChar(para.FeeTypeName) + "</FeeTypeName>";
                            detailXml += "<CustomerName>" + CheckXmlChar(para.CustomerName) + "</CustomerName>";
                            detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
                            detailXml += "<OperateDate>" + CheckXmlChar(para.OperateDate) + "</OperateDate>";
                            detailXml += "<Operator>" + CheckXmlChar(para.Operator) + "</Operator>";
                            detailXml += "<SubScribDate>" + CheckXmlChar(para.SubScribDate) + "</SubScribDate>";
                            detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
                            detailXml += "<ExamPlaceName>" + CheckXmlChar(ExamPlaceName) + "</ExamPlaceName>";
                            detailXml += "</CustomerInfo>";
                            
                            FastReport.GenerateCustomerCertificate(ID_Customer, ID_CustomerCode128, para.TemplateName, para.FeeTypeName, para.CustomerName, para.GenderName, para.OperateDate, para.Operator, para.SubScribDate);
                         }
                        //未到期，且未打印，即没有到打印时间，不得打印指引单
                        art.dialog({
                            lock: true, fixed: true, opacity: 0.3,
                            content: '客户预约体检时间为:[' + SubScribDate + ']目前不能打印指引单！',
                            icon: 'info',
                            ok: true
                        });

                    }
                }
                else if (Is_Subscribed == 0)//如果是登记，则判断登记时间和当前时间是否相同，相同则可用直接打印指引单
                {
                    if (yyDate == todayDate) {
                        //该客户已经进行过登记或预约，有为收费项目则需要先打印收费单，没有收费项目则直接打印增项指引单
                        //判断是否存在非团体为收费的增项项目
                        if (jQuery("#tblTeamTaskGroupFee tbody tr[is_printed!='1']").length > 0) {
                            FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                            SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); //xmhuang 2013-10-12 打印完指引单后向接口发送信息  
                        }
                    }
                    else {
                        art.dialog({
                            lock: true, fixed: true, opacity: 0.3,
                            content: '客户登记体检时间为:[' + yyDate + ']目前不能打印指引单！',
                            icon: 'info',
                            ok: true
                        });
                    }
                }
            }
        }
    }
}
/// <summary>
///补打指引单，这里只补打非退项的收费项目
/// </summary>
function ReDoPrint() {
    var checkedObj = jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][id_teamtaskgroup=''][feechargestaute='']");
    var checkedObjCount = jQuery(checkedObj).length;
    if (checkedObjCount > 0) {
        art.dialog({
            lock: true, fixed: true, opacity: 0.3,
            content: '对不起，您还有未交费的项目不能打印指引单！',
            icon: 'info',
            ok: true
        });
        return false;
    }
    else {
        //判断是否已经打印过指引单，如果没有打印过则不允许补打指引单 xmhuang 2013-11-05
        var Datas = jQuery("#txtID_Customer").data("data");
        if (Datas != null) {
            if (Datas.Is_GuideSheetPrinted == "True")//如果已经打印过指引单
            {
                var ID_Customer = jQuery.trim(jQuery("#txtID_Customer").val());
                if (isCustomerExamNo(ID_Customer)) {
                    FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 2, 0);
                }
                else {
                    art.dialog({
                        lock: true, fixed: true, opacity: 0.3,
                        content: '对不起，请您输入正确的体检号！',
                        icon: 'info',
                        ok: true
                    });
                    return false;
                }
            }
            else //没有打印过指引单
            {
                art.dialog({
                    lock: true, fixed: true, opacity: 0.3,
                    content: '对不起，该客户尚未打印过指引单，无法进行补打！',
                    icon: 'info',
                    ok: true
                });
                return false;
            }
        }
        else {
            art.dialog({
                lock: true, fixed: true, opacity: 0.3,
                content: '对不起，未获取到客户信息，请重新检索！',
                icon: 'info',
                ok: true
            });
            return false;
        }
        //判断是否已经打印过指引单，如果没有打印过则不允许补打指引单 xmhuang 2013-11-05
    }
}

/// <summary>
/// 修改团体登记时间 xmhuang 2013-10-12
/// </summary>
function UpdateCustomerSubscribDateOfTeam_Ajax(ID_Customer, SubScribDate) {
    //ajax请求：由于字符在get请求中超长，这里必须使用post方式提交请求
    jQuery.ajax({
        type: "POST",
        cache: false,
        url: "/Ajax/AjaxRegiste.aspx",
        data: { ID_Customer: ID_Customer, SubScribDate: SubScribDate, action: 'UpdateCustomerSubscribDateOfTeam' },
        dataType: "json",
        success: function (msg) {

        },
        complete: function () {

        }
    });
}
/// <summary>
/// 退费 全额退费 UnChargeType:0 表示部分退费 1：表示全额退费 xmhuang 2013-10-15
/// </summary>
function UnChargeAllCallback(UnChargeType) {
    var Forsex = jQuery.trim(jQuery("#lblSex").text()) == "男" ? 1 : 0;
    //这里执行交费
    var AllFeeID = jQuery("#tblTeamTaskGroupFee").data("AllFeeID");
    var ID_Customer = jQuery.trim(jQuery("#lblID_Customer").text());
    var Invoice = "";

    if (AllFeeID == "" || ID_Customer == "") {
        this.close();
        return false;
    }
    //获取所有的发票信息
    jQuery("#showInvoiceList li").each(function () {
        Invoice += "," + jQuery(this).attr("id");
    });

    if (Invoice.length > 0) {
        var pAllUnChargeInvoiceMoney = 0;
        var TotalCustFee = jQuery.trim(jQuery("#hidYTJE").text());
        var AllUnChargeInvoiceMoney = jQuery.trim(jQuery("#txtAllUnChargeInvoiceMoney").val()); //获取已扫描发票总金额 xmhuang2013-10-23
        //如果是部分退费，需要验证发票总金额不能为空，并且要大于或等于退费总金额
        if (UnChargeType == 0) {
            if (AllUnChargeInvoiceMoney == "") {
                ShowSystemDialog("对不起,发票总金额不能为空！");
                jQuery("#txtAllUnChargeInvoiceMoney").focus();
                return false;
            }
            else {
                pAllUnChargeInvoiceMoney = parseFloat(AllUnChargeInvoiceMoney).toFixed(curFixed)
                if (parseFloat(pAllUnChargeInvoiceMoney) <= parseFloat(TotalCustFee)) {
                    ShowSystemDialog("对不起,发票总金额必须大于应退金额！");
                    jQuery("#txtAllUnChargeInvoiceMoney").focus();
                    return false;
                }
                else {

                }
            }
        }
        var TemplateName = "CustomerCharges.frx"; // "CustomerUnCharges.frx";

        var CustomerName = jQuery.trim(jQuery("#lblCustomerName").text());
        var SubScribDate = jQuery.trim(jQuery("#lblRegisterDate").text());
        var UpperWrite = ""; //  chineseNumber(TotalCustFee); //转换为大写
        var ID_CustomerCode128 = jQuery("#lblID_Customer").data("Code128c");
        var para = { ID_Customer: ID_Customer, ID_CustomerCode128: ID_CustomerCode128, TemplateName: TemplateName
        , CustomerName: CustomerName, TotalCustFee: TotalCustFee, UpperWrite: UpperWrite, SubScribDate: SubScribDate
        };
        //提交Ajax请求
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxRegiste.aspx",
            data: { action: "UnChargeInvoice",
                AllFeeID: AllFeeID,
                ID_Customer: ID_Customer,
                Invoice: Invoice,
                Forsex: Forsex,
                UnChargeType: UnChargeType
            },
            cache: false,
            dataType: "json",
            success: function (msg) {
                if (msg.success == "1") {
                    //重新绑定收费项目
                    GetCustomerExamPhysicalInfo(ID_Customer);
                    //部分退费：打印退费通知单 xmhuang 2013-10-23  
                    if (UnChargeType == 0) {
                        TotalCustFee = pAllUnChargeInvoiceMoney - TotalCustFee; //获取需要交费的金额
                        if (TotalCustFee > 0) {
                            UpperWrite = chineseNumber(TotalCustFee); //转换为大写
                            para.TotalCustFee = TotalCustFee;
                            para.UpperWrite = UpperWrite;
                            
                            var sex = jQuery('#lblHidSex').text();
                            var GenderName = "同志";
                            if (sex == "男") {
                                GenderName = "先生";
                            }
                            else if (sex == "女") {
                                GenderName = "女士";
                            }
                            var detailXml = "<CustomerInfo>";
                            detailXml += "<ID_Customer>" + para.ID_Customer + "</ID_Customer>";
                            detailXml += "<ID_CustomerCode128>" + CheckXmlChar(para.ID_CustomerCode128) + "</ID_CustomerCode128>";
                            detailXml += "<TemplateName>" + CheckXmlChar(para.TemplateName) + "</TemplateName>";
                            detailXml += "<FeeTypeName>" + "收费" + "</FeeTypeName>";
                            detailXml += "<CustomerName>" + CheckXmlChar(para.CustomerName) + "</CustomerName>";
                            detailXml += "<TotalCustFee>" + CheckXmlChar(para.TotalCustFee) + "</TotalCustFee>";
                            detailXml += "<UpperWrite>" + CheckXmlChar(para.UpperWrite) + "</UpperWrite>";
                            detailXml += "<SubScribDate>" + CheckXmlChar(para.SubScribDate) + "</SubScribDate>";
                            detailXml += "<Operator>" + CheckXmlChar(UserName) + "</Operator>";
                            detailXml += "<ServerDate>" + CheckXmlChar(CurDate) + "</ServerDate>";
                            detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
                            detailXml += "<ExamPlaceName>" + CheckXmlChar(ExamPlaceName) + "</ExamPlaceName>";
                            detailXml += "</CustomerInfo>";
                            FastReport.GenerateCustomerCharges(para.ID_Customer, para.ID_CustomerCode128, para.TemplateName, "收费", para.CustomerName, para.TotalCustFee, para.UpperWrite, para.SubScribDate, UserName);
                        }
                    }
                }
            }
        });
    }
    else {
        jQuery("#txtUnChargeInvoice").focus();
        return false;
    }
    return true;
}
/// <summary>
/// 将扫描的退费发票放入退费发票列表 xmhuang 2013-10-15
/// </summary>
/// <returns></returns>
function DoUnChargeInvoice() {
    var InvoiceNo = jQuery.trim(jQuery("#txtUnChargeInvoice").val());
    if (InvoiceNo != "") {
        if (jQuery("#showInvoiceList li[id='" + InvoiceNo + "'").length == 0) {
            var NewContent = InvoiceListContent.replace(/@InvoiceNo/gi, InvoiceNo);
            jQuery("#showInvoiceList").prepend(NewContent);
        }
        jQuery("#txtUnChargeInvoice").val("");
        jQuery("#txtUnChargeInvoice").focus();
    }

}
/// <summary>
/// 将扫描的收费发票放入收费列表 xmhuang 2013-10-16
/// </summary>
/// <returns></returns>
function DoChargeCallback() {
    var InvoiceNo = jQuery.trim(jQuery("#txtInvoice").val());
    if (InvoiceNo != "") {
        if (jQuery("#showInvoiceList li[id='" + InvoiceNo + "'").length == 0) {
            var NewContent = InvoiceListContent.replace(/@InvoiceNo/gi, InvoiceNo);
            jQuery("#showInvoiceList").prepend(NewContent);
        }
        jQuery("#txtInvoice").val("");
        jQuery("#txtInvoice").focus();
    }
}
/// <summary>
/// 执行收费
/// </summary>
/// <returns></returns>
function ChargeCallback() {
    //xmhuang 2013-09-10 Begin
    var Forsex = jQuery.trim(jQuery("#lblSex").text()) == "男" ? 1 : 0;
    //xmhuang 2013-09-10 End

    //this.time(2);
    //这里执行交费
    var AllFeeID = jQuery("#tblTeamTaskGroupFee").data("AllFeeID");
    var ID_Customer = jQuery.trim(jQuery("#lblID_Customer").text());
    var Invoice = "";

    if (AllFeeID == "" || ID_Customer == "") {
        this.close();
        return false;
    }
    //获取所有的发票信息
    jQuery("#showInvoiceList li").each(function () {
        Invoice += "," + jQuery(this).attr("id");
    });
    if (Invoice.length > 0) {
        //提交Ajax请求
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxRegiste.aspx",
            data: { action: curChargeAction,
                AllFeeID: AllFeeID,
                ID_Customer: ID_Customer,
                Invoice: Invoice,
                Forsex: Forsex
            },
            cache: false,
            dataType: "json",
            success: function (msg) {
                if (msg.success == "1") {
                    //重新绑定收费项目
                    GetCustomerExamPhysicalInfo(ID_Customer);
                }
            }
        });
    }
    else {
        jQuery("#txtInvoice").val("");
        jQuery("#txtInvoice").focus();
        return false;
    }
    return true;
}

/// <summary>
/// 发票补录 xmhuang 2013-10-16
/// </summary>
/// <returns></returns>
function LoseInvoiceCharge() {
    var content = '<table class="ModifyPassword">' +
            '<tbody>' +
             '    <tr><td class="left" colspan="2" style="text-align:center;font-size:16px; color:#2255a4; font-weight:bold;padding-bottom:10px;">补录发票扫描</td></tr>' +
            '    <tr><td class="left">发票扫描：</td><td><input id="txtUnChargeInvoice" onkeyup="OnFormKeyUp();"/> </td></tr>' +
            '<tr><td><td></tr>' + InvoiceContent.replace("@InvoiceName", "补录发票号:") +
    '</tbody>' +
            '</table>';
    var title = "发票补录";
    var button = [
        {
            name: "确认补录",
            callback: function () {
                //this.time(2);
                //这里执行交费
                return DoLoseInvoiceCharge();
            },
            focus: true
        },
        {
            name: '取消',
            callback: function () {
                this.close();
            }
        }];

    //计算所有收费项目总金额
    art.dialog({
        id: 'OperWindowFrame',
        title: title,
        lock: true, fixed: true, opacity: 0.3,
        content: content,
        init: function () {
            if (jQuery("#txtUnChargeInvoice").length > 0) {
                jQuery("#txtUnChargeInvoice").focus();
            }
        },
        button: button
    });

}
/// <summary>
/// 确认发票补录 xmhuang 2013-10-16 只需要修改
/// </summary>
/// <returns></returns>
function DoLoseInvoiceCharge() {
    var ID_Customer = jQuery.trim(jQuery("#lblID_Customer").text());
    var Invoice = "";
    //获取所有的发票信息
    jQuery("#showInvoiceList li").each(function () {
        Invoice += "," + jQuery(this).attr("id");
    });
    if (Invoice.length > 0) {
        //提交Ajax请求
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxRegiste.aspx",
            data: { action: "LoseInvoiceCharge",
                ID_Customer: ID_Customer,
                Invoice: Invoice
            },
            cache: false,
            dataType: "json",
            success: function (msg) {
                if (msg.success == "1") {
                    //重新绑定收费项目
                    GetCustomerExamPhysicalInfo(ID_Customer);
                }
            }
        });
    }
}

/************************补打预约凭证 xmhuang  2013-11-05****************************/
/// <summary>
/// 补打预约凭证
/// </summary>
function ReprintCustomerCredence() {
    //全部√费则，直接打印预约凭证
    var Datas = jQuery("#txtID_Customer").data("data");
    if (Datas != null) {
        if (Datas.Is_GuideSheetPrinted != "True")//如果已经打印过指引单
        {
            //if (jQuery("#tblTeamTaskGroupFee tbody tr[is_printed!='1']").length >= 0) {
            var ID_Customer = jQuery.trim(jQuery("#lblID_Customer").text());
            var ID_CustomerCode128 = jQuery("#lblID_Customer").data("Code128c"); //  jQuery.trim(jQuery("#lblCode128c").text());
            var customerName = jQuery.trim(jQuery("#lblCustomerName").text());
            var departmentName = "健康管理中心";
            var GenderName = jQuery.trim(jQuery("#lblSex").text());
            var OperateDate = jQuery.trim(jQuery("#lblOperateDate").text());
            var Operator = jQuery.trim(jQuery("#lblOperator").text());
            var SubScribDate = jQuery.trim(jQuery("#lblRegisterDate").text());
            //是否是预约
            var TemplateName = "CustomerCredence.frx";
            var para = {
                TemplateName: TemplateName,
                CustomerName: customerName,
                departmentName: departmentName,
                SubScribDate: SubScribDate,
                FeeTypeName: "收费",
                GenderName: GenderName,
                OperateDate: OperateDate,
                Operator: Operator,
                SubScribDate: SubScribDate
            }; //直接打印预约凭证
            
            var sex = jQuery.trim(jQuery("#lblSex").text());
            GenderName = "同志";
            if (sex == "男") {
                GenderName = "先生";
            }
            else if (sex == "女") {
                GenderName = "女士";
            }
            var detailXml = "<CustomerInfo>";
            detailXml += "<ID_Customer>" + ID_Customer + "</ID_Customer>";
            detailXml += "<ID_CustomerCode128>" + CheckXmlChar(ID_CustomerCode128) + "</ID_CustomerCode128>";
            detailXml += "<TemplateName>" + CheckXmlChar(para.TemplateName) + "</TemplateName>";
            detailXml += "<FeeTypeName>" + CheckXmlChar(para.FeeTypeName) + "</FeeTypeName>";
            detailXml += "<CustomerName>" + CheckXmlChar(para.CustomerName) + "</CustomerName>";
            detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
            detailXml += "<OperateDate>" + CheckXmlChar(para.OperateDate) + "</OperateDate>";
            detailXml += "<Operator>" + CheckXmlChar(para.Operator) + "</Operator>";
            detailXml += "<SubScribDate>" + CheckXmlChar(para.SubScribDate) + "</SubScribDate>";
            detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
            detailXml += "<ExamPlaceName>" + CheckXmlChar(ExamPlaceName) + "</ExamPlaceName>";
            detailXml += "</CustomerInfo>";
            FastReport.GenerateCustomerCertificate(ID_Customer, ID_CustomerCode128, para.TemplateName, para.FeeTypeName, para.CustomerName, para.GenderName, para.OperateDate, para.Operator, para.SubScribDate);
        }
        else {
            art.dialog({
                lock: true, fixed: true, opacity: 0.3,
                content: '对不起，该客户已经打印指引单，不允许打印预约凭证，请核实！',
                icon: 'info',
                ok: true
            });
            return false;
        }
    }
    else {
        art.dialog({
            lock: true, fixed: true, opacity: 0.3,
            content: '对不起，未获取到客户信息，请重新检索！',
            icon: 'info',
            ok: true
        });
        return false;
    }
}
/************************补打预约凭证 xmhuang  2013-11-05****************************/