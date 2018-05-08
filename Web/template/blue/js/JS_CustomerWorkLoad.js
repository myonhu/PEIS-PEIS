﻿/// <summary>
/// 页面初始化
/// </summary>
jQuery(document).ready(function () {
    jQuery("[name='Searchresult']").attr("title", "点击关闭详细");
    jQuery("#slTeam").select2();
    ResetSearchInfo("尚无任何数据...");
});

/// <summary>
/// 显示或者折叠详细信息
/// </summary>
function ShowMe(obj) {
    var name = jQuery(obj).attr("name");
    var nextObj = jQuery("[name='Searchresult_detail_" + name + "']");
    jQuery(nextObj).each(function () {
        if (jQuery(this).is(":visible")) {
            jQuery(this).hide();
            jQuery(obj).attr("title", "点击查看详细");
        }
        else {
            jQuery(this).show("slow");
            jQuery(obj).attr("title", "点击关闭详细");
        }
    });

}

/// <summary>
/// 重置检索无结果显示的信息
/// </summary>
function ResetSearchInfo(msgInfo) {
    if (msgInfo == "" || msgInfo == undefined) {
        msgInfo = "在您查询的时间段内，没有找到任何信息！";
    }
    var html = "<tr><td style='text-align: center; line-height: 100px;' colSpan='28'>" + msgInfo + "</td></tr>";
    jQuery('#Searchresult').html(html); //设置无数据检索时显示提示信息
    jQuery("#Pagination").hide(); //隐藏分页控件
}

/// <summary>
/// 获取收费项目工作量
/// </summary>
function GetCustomerWorkLoad() {
    jQuery('#Searchresult').html("");
    var CustomerName = jQuery.trim(jQuery('#txtCustomer').val());
    var ID_Team = document.getElementById("slTeam").options[document.getElementById("slTeam").selectedIndex].value; //获取被查询医生
    var TeamName = document.getElementById("slTeam").options[document.getElementById("slTeam").selectedIndex].text; //获取被查询医生
    var BeginExamDate = jQuery.trim(jQuery('#BeginExamDate').val());
    BeginExamDate = encodeURIComponent(BeginExamDate); //编码开始日期
    var EndExamDate = jQuery.trim(jQuery('#EndExamDate').val());
    EndExamDate = encodeURIComponent(EndExamDate);    //结束日期
    //判断时间差 Begin xmhuang 2014-01-14
    if (!CheckTime(BeginExamDate, EndExamDate)) {
        return false;
    }    
    var InputCode = jQuery.trim(jQuery("#txtInputCode").val());
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxStatistics.aspx",
        data: {
            ID_Team: ID_Team,
            TeamName: TeamName,
            CustomerName: CustomerName,
            InputCode: InputCode,
            BeginExamDate: BeginExamDate,
            EndExamDate: EndExamDate,
            action: 'GetCustomerWorkLoad'
        },
        cache: false,
        dataType: "json",
        success: function (msg) {
            pagerData = msg.dataList0;
            tempOperPageCount = 0;
            QueryPagesData(0);
            jQuery(msg.dataList1).each(function (i, item) {
                jQuery("#loadExcel").attr("href", item.FilePath);
            });
            return false;
            //            if (msg != undefined) {
            //                var index = 1;
            //                var newContent = '';
            //                var CustFeetemplateContent = jQuery("#SearchresultTeamplate").html();
            //                var CustFeeDetailtemplateContent = jQuery("#Searchresult_detail").html();
            //                if (msg.dataList0.length > 0) {
            //                    if (CustFeetemplateContent == undefined) { return false; }
            //                    //由于是单表返回，返回格式为父－子－父－子...父-子形式，其中IsParent=1为父 IsParent=0为其对应的子项
            //                    jQuery(msg.dataList0).each(function (i, item) {
            //                        if (CustFeetemplateContent != null) {
            //                            if (item.ExamState == 0) {
            //                                item.ExamState = "在线库";
            //                            }
            //                            else if (item.ExamState == 1) {
            //                                item.ExamState = "离线库";
            //                            }
            //                            else if (item.ExamState > 1) {
            //                                item.ExamState = "归档库" + (item.ExamState - 1);
            //                            }
            //                            if (item.Is_Checked == "True") {
            //                                item.Is_Checked = "√";
            //                            }
            //                            else {
            //                                item.Is_Checked = "×";
            //                            }

            //                            if (item.Is_ReportPrinted == "True") {
            //                                item.Is_ReportPrinted = "√";
            //                            }
            //                            else {
            //                                item.Is_ReportPrinted = "×";
            //                            }

            //                            if (item.Is_Informed == "True") {
            //                                item.Is_Informed = "√";
            //                            }
            //                            else {
            //                                item.Is_Informed = "×";
            //                            }

            //                            if (item.Is_ReportReceipted == "True") {
            //                                item.Is_ReportReceipted = "√";
            //                            }
            //                            else {
            //                                item.Is_ReportReceipted = "×";
            //                            }

            //                            if (item.ReportPrinter == undefined) {
            //                                item.ReportPrinter = "";
            //                            }
            //                            if (item.ReportPrintedDate == undefined) {
            //                                item.ReportPrintedDate = "";
            //                            }
            //                            if (item.Informer == undefined) {
            //                                item.Informer = "";
            //                            }
            //                            if (item.InformedDate == undefined) {
            //                                item.InformedDate = "";
            //                            }

            //                            if (item.ReportReceiptor == undefined) {
            //                                item.ReportReceiptor = "";
            //                            }
            //                            if (item.ReportReceiptedDate == undefined) {
            //                                item.ReportReceiptedDate = "";
            //                            }

            //                            //应LZhang需求反馈，新增体检地点、是否总检、总检人、总检时间四个显示字段  xmhuang 2013-10-23
            //                            //                        @ExamPlaceName
            //                            //                        @Is_FinalFinished
            //                            //                        @FinalDoctor
            //                            //                        @FinalDate
            //                            if (item.ExamPlaceName == undefined) {
            //                                item.ExamPlaceName = "";
            //                            }
            //                            if (item.FinalDoctor == undefined) {
            //                                item.FinalDoctor = "";
            //                            }
            //                            if (item.FinalDate == undefined) {
            //                                item.FinalDate = "";
            //                            }
            //                            if (item.Is_FinalFinished == "True") {
            //                                item.Is_FinalFinished = "√";
            //                            }
            //                            else {
            //                                item.Is_FinalFinished = "×";
            //                            }
            //                            newContent += CustFeetemplateContent.replace(/@ID_ArcCustomer/gi, item.ID_ArcCustomer)
            //                                    .replace(/@index/gi, index)
            //                                    .replace(/@ID_Customer/gi, item.ID_Customer)
            //                                    .replace(/@CustomerName/gi, item.CustomerName)
            //                                     .replace(/@GenderName/gi, item.GenderName)
            //                                     .replace(/@MarriageName/gi, item.MarriageName)
            //                                     .replace(/@IDCard/gi, item.IDCard)
            //                                     .replace(/@MobileNo/gi, item.MobileNo)
            //                                     .replace(/@TeamName/gi, item.TeamName)
            //                                     .replace(/@ExamState/gi, item.ExamState)
            //                                     .replace(/@SubScribDate/gi, item.SubScribDate)
            //                                     .replace(/@ExamTypeName/gi, item.ExamTypeName)
            //                                     .replace(/@Is_Checked/gi, item.Is_Checked)
            //                                     .replace(/@Checker/gi, item.Checker)
            //                                     .replace(/@CheckedDate/gi, item.CheckedDate)
            //                                     .replace(/@Is_ReportPrinted/gi, item.Is_ReportPrinted)
            //                                     .replace(/@ReportPrinter/gi, item.ReportPrinter)
            //                                     .replace(/@ReportPrintedDate/gi, item.ReportPrintedDate)
            //                                     .replace(/@Is_Informed/gi, item.Is_Informed)
            //                                     .replace(/@Informer/gi, item.Informer)
            //                                     .replace(/@InformedDate/gi, item.InformedDate)
            //                                     .replace(/@Is_ReportReceipted/gi, item.Is_ReportReceipted)
            //                                     .replace(/@ReportReceiptor/gi, item.ReportReceiptor)
            //                                     .replace(/@ReportReceiptedDate/gi, item.ReportReceiptedDate)

            //                                     .replace(/@ExamPlaceName/gi, item.ExamPlaceName)
            //                                     .replace(/@Is_FinalFinished/gi, item.Is_FinalFinished)
            //                                     .replace(/@FinalDoctor/gi, item.FinalDoctor)
            //                                     .replace(/@FinalDate/gi, item.FinalDate)
            //                                    ;
            //                            index++;
            //                        }
            //                    });
            //                    if (newContent != '') {
            //                        jQuery('#Searchresult').html(newContent); //将值填充到Searchresult中显示
            //                    } else {
            //                        ResetSearchInfo("");
            //                    }
            //                }

            //                jQuery(msg.dataList1).each(function (i, item) {
            //                    jQuery("#loadExcel").attr("href", item.FilePath);
            //                });
            //            }
            //            else {
            //                ResetSearchInfo("");
            //            }
        }
    });
}

function LoadCustomerReport(title, ID_Customer) {

    //    if (ID_Customer != undefined && ID_Customer != "") {
    //        var ExamUrl = "/System/ReportManage/ReportPreview.aspx?ID_Customer=" + ID_Customer;
    //        art.dialog.open(ExamUrl,
    //    {
    //        width: 350,
    //        height: 250,
    //        drag: false,
    //        lock: true,
    //        id: 'OperWindowFrame',
    //        title: title,
    //        cache: false,
    //        init: function () {
    //            jQuery(".aui_close").hide();
    //        },
    //        close: function () {

    //        }
    //    });
    //        // DoLoad(ExamUrl, '');
    //    }
}
/// <summary>
/// 加载客户报表
/// </summary>
function OpenCustomerReport(obj, title) {
    return false;
    var url = jQuery(obj).attr("targeturl");
    DoLoad(url, "");
    var width = 980;
    var height = 650;
    jQuery("#divLoadCustomerReport").load(url, { limit: 65 }, function () {
        jQuery("#divLoadCustomerReport").width(width);
        jQuery("#divLoadCustomerReport").height(height);
        var content = jQuery("#divLoadCustomerReport").html();
        jQuery("#divLoadCustomerReport").html("");
        art.dialog(
        {
            content: content,
            width: width,
            height: height,
            drag: false,
            lock: true,
            id: 'OperWindowFrame',
            title: title,
            init: function () {

            },
            close: function () {
                //jQuery("#divLoadCustomerReport").html("");
            }
        });
        jQuery("#OperWindowFrame").click();
    });
}
/*通用客户端分页脚本 xmhuang 2014-01-13  Begin*/
// 1、只有第1次才调用 jQuery("#Pagination").pagination
// 2、只有第2次及以后的操作才调用回调函数 pageselectCallback 中的 QueryPagesData(page_index );
var tempOperPageCount = 0;
var tempOldtotalCount = 0; //初始总页数，用于判断是否更新页码
var pagerData = null; //记录当前分页数据源
function pageselectCallback(page_index, jq) {
    if (tempOperPageCount > 0) {
        tempOperPageCount++;
        QueryPagesData(page_index);
    }
    tempOperPageCount++;
    return false;
}
function QueryPagesData(pageIndex) {
    if (pagerData != null) {
        if (pagerData.length == 0) {
            ResetSearchInfo("");
            jQuery("#Pagination").hide();
            return false;
        }
        var dataLength = pagerData.length;
        var flag = jQuery.trim(jQuery('#flag').val());
        var curPageSize = pagePagination.items_per_page;
        var optInit = getOptionsFromForm();                   //获取分页配置参数
        jQuery("#Pagination").show();
        if (tempOperPageCount == 0) {
            jQuery("#Pagination").pagination(dataLength, optInit);
        }
        else if (tempOldtotalCount != dataLength) {
            jQuery("#Pagination").pagination(dataLength, optInit);
        }
        tempOldtotalCount = dataLength;
        var item;
        var rowNum = curPageSize * pageIndex; //计算
        var newcontent = '';
        var templateContent = jQuery("#SearchresultTeamplate").html();
        if (templateContent == undefined) { return false; }
        for (var c = 0; c < curPageSize; c++) {
            if (rowNum + c > dataLength) {
                break;
            }
            item = pagerData[rowNum + c];
            if (item != undefined) {
                if (templateContent != null) {
                    if (item.ExamState == 0) {
                        item.ExamState = "在线库";
                    }
                    else if (item.ExamState == 1) {
                        item.ExamState = "离线库";
                    }
                    else if (item.ExamState > 1) {
                        item.ExamState = "归档库" + (item.ExamState - 1);
                    }
                    if (item.Is_Checked == "True") {
                        item.Is_Checked = "√";
                    }
                    else {
                        item.Is_Checked = "×";
                    }

                    if (item.Is_ReportPrinted == "True") {
                        item.Is_ReportPrinted = "√";
                    }
                    else {
                        item.Is_ReportPrinted = "×";
                    }

                    if (item.Is_Informed == "True") {
                        item.Is_Informed = "√";
                    }
                    else {
                        item.Is_Informed = "×";
                    }

                    if (item.Is_ReportReceipted == "True") {
                        item.Is_ReportReceipted = "√";
                    }
                    else {
                        item.Is_ReportReceipted = "×";
                    }

                    if (item.ReportPrinter == undefined) {
                        item.ReportPrinter = "";
                    }
                    if (item.ReportPrintedDate == undefined) {
                        item.ReportPrintedDate = "";
                    }
                    if (item.Informer == undefined) {
                        item.Informer = "";
                    }
                    if (item.InformedDate == undefined) {
                        item.InformedDate = "";
                    }

                    if (item.ReportReceiptor == undefined) {
                        item.ReportReceiptor = "";
                    }
                    if (item.ReportReceiptedDate == undefined) {
                        item.ReportReceiptedDate = "";
                    }

                    //应LZhang需求反馈，新增体检地点、是否总检、总检人、总检时间四个显示字段  xmhuang 2013-10-23
                    //                        @ExamPlaceName
                    //                        @Is_FinalFinished
                    //                        @FinalDoctor
                    //                        @FinalDate
                    if (item.ExamPlaceName == undefined) {
                        item.ExamPlaceName = "";
                    }
                    if (item.FinalDoctor == undefined) {
                        item.FinalDoctor = "";
                    }
                    if (item.FinalDate == undefined) {
                        item.FinalDate = "";
                    }
                    if (item.Is_FinalFinished == "True") {
                        item.Is_FinalFinished = "√";
                    }
                    else {
                        item.Is_FinalFinished = "×";
                    }
                    newcontent += templateContent.replace(/@ID_ArcCustomer/gi, item.ID_ArcCustomer)
                                    .replace(/@index/gi, rowNum + c + 1)
                                    .replace(/@ID_Customer/gi, item.ID_Customer)
                                    .replace(/@CustomerName/gi, item.CustomerName)
                                     .replace(/@GenderName/gi, item.GenderName)
                                     .replace(/@MarriageName/gi, item.MarriageName)
                                     .replace(/@IDCard/gi, item.IDCard)
                                     .replace(/@MobileNo/gi, item.MobileNo)
                                     .replace(/@TeamName/gi, item.TeamName)
                                     .replace(/@ExamState/gi, item.ExamState)
                                     .replace(/@SubScribDate/gi, item.SubScribDate)
                                     .replace(/@ExamTypeName/gi, item.ExamTypeName)
                                     .replace(/@Is_Checked/gi, item.Is_Checked)
                                     .replace(/@Checker/gi, item.Checker)
                                     .replace(/@CheckedDate/gi, item.CheckedDate)
                                     .replace(/@Is_ReportPrinted/gi, item.Is_ReportPrinted)
                                     .replace(/@ReportPrinter/gi, item.ReportPrinter)
                                     .replace(/@ReportPrintedDate/gi, item.ReportPrintedDate)
                                     .replace(/@Is_Informed/gi, item.Is_Informed)
                                     .replace(/@Informer/gi, item.Informer)
                                     .replace(/@InformedDate/gi, item.InformedDate)
                                     .replace(/@Is_ReportReceipted/gi, item.Is_ReportReceipted)
                                     .replace(/@ReportReceiptor/gi, item.ReportReceiptor)
                                     .replace(/@ReportReceiptedDate/gi, item.ReportReceiptedDate)

                                     .replace(/@ExamPlaceName/gi, item.ExamPlaceName)
                                     .replace(/@Is_FinalFinished/gi, item.Is_FinalFinished)
                                     .replace(/@FinalDoctor/gi, item.FinalDoctor)
                                     .replace(/@FinalDate/gi, item.FinalDate)
                }
            }
        }
        if (newcontent != '') {
            //flag:0:折扣查询  1：记账明细查询
            if (flag == 1) {
                jQuery('#tdFeeWayName').show();
            }
            else {
                jQuery('#tdFeeWayName').hide();
            }
            jQuery('#Searchresult').html(newcontent); //将值填充到Searchresult中显示
        }
        else {
            ResetSearchInfo("");
        }
    }
}
/*通用客户端分页脚本 xmhuang 2014-01-13  End*/