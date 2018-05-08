﻿/****************************************

1.	文件名称(File Name)：   各个页面 右边浮动信息的脚步文件
2.	功能描述(Description):  主要处理右边浮动信息，及浮动链接点击后加载的数据
3.	作者(Author)：			WTang
4.	日期(Create Date)：		2013.8.26

****************************************/

// -------------------------- 右边浮动层控制及鼠标事件 start -------------------------------
function SetSideFloatXY() {

    var top01 = 0; // 侧边栏，其它链接的Y坐标
    var top02 = 0; // Top链接的Y坐标
    var top03 = 0; // Bottom链接的Y坐标
    // 侧边栏，其它链接的位置
    if (document.getElementById("divRightSideFloat") != undefined && document.getElementById("divRightSideFloat") != null) {

        //jQuery("#divRightSideFloat").hide();
        var top = (document.documentElement.scrollTop + document.documentElement.clientHeight - jQuery("#divRightSideFloat").height() - 90);
        var left = document.body.scrollWidth; //(document.documentElement.scrollLeft + document.documentElement.clientWidth - 50);
        top01 = top;
        if (parseInt(left) > 1012) {
            left = (left - 952) / 2 + 952;
        } else {
            left = left - 30;
        }
        document.getElementById("divRightSideFloat").style.top = top + "px";
        document.getElementById("divRightSideFloat").style.left = left + "px";

        jQuery("#divRightSideFloat").show();
    }
    // TOP 的位置
    if (document.getElementById("divGotoTop") != undefined && document.getElementById("divGotoTop") != null) {

        jQuery("#divGotoTop").hide();
        var top = (document.documentElement.scrollTop + document.documentElement.clientHeight - jQuery("#divGotoTop").height() - 70);
        var left = document.body.scrollWidth;
        top02 = top;
        if (parseInt(left) > 1012) {
            left = (left - 952) / 2 + 952;
        } else {
            left = left - 30;
        }
        document.getElementById("divGotoTop").style.top = top + "px";
        document.getElementById("divGotoTop").style.left = left + "px";

        if (top < document.documentElement.clientHeight) {

            jQuery("#divGotoTop").hide();

        } else {
            if (top02 - top01 >= jQuery("#divRightSideFloat").height()) {
                jQuery("#divGotoTop").show();
            } else {
                jQuery("#divGotoTop").hide();
            }
        }
    }
    // Bottom 的位置
    if (document.getElementById("divGotoBottom") != undefined && document.getElementById("divGotoBottom") != null) {

        jQuery("#divGotoBottom").hide();
        var top = (document.documentElement.scrollTop + document.documentElement.clientHeight - jQuery("#divGotoBottom").height() - 30);
        var left = document.body.scrollWidth;
        top03 = top;
        if (parseInt(left) > 1012) {
            left = (left - 952) / 2 + 952;
        } else {
            left = left - 30;
        }
        document.getElementById("divGotoBottom").style.top = top + "px";
        document.getElementById("divGotoBottom").style.left = left + "px";

        if (top < document.body.scrollHeight - 100) {

            jQuery("#divGotoBottom").show();

        } else {
            //                    if (top02 - top01 >= jQuery("#divRightSideFloat").height()) {
            //                        jQuery("#divGotoBottom").show();
            //                    } else {
            jQuery("#divGotoBottom").hide();
            //                    }
        }
    }
}
window.onscroll = SetSideFloatXY;
window.onresize = SetSideFloatXY;

function GotoTop() {
    scroll(0, 0);
}

function GotoBottom() {
    window.scrollTo(0, document.body.scrollHeight - 100);
}

/// <summary>
// 隐藏浮动框内部的所有列表层
/// </summary>
function HideAllFloatList() {
    // 隐藏其他列表
    jQuery("#listHistoryExamCount").hide();     // 隐藏历史体检号对比列表
    jQuery("#listSectionExamedCount").hide();   // 隐藏已检查科室列表
    jQuery("#listSectionExamFloat").hide();     // 隐藏客户科室列表
    jQuery("#listFinalExamCompare").hide();     // 隐藏总检对比结果列表
}

/***************************克隆相关函数 Start ************************************/

/// <summary>
/// 隐藏显示已检查科室列表
/// </summary>
function ShowHideCloneRightFloatDiv() {
    var CustomerName = jQuery.trim(jQuery("#txtCustomer").val());
    if (CustomerName == "") {
        jQuery("#listCloneRightFloat").hide();
        ShowSystemDialog("客户姓名不存在,请先维护");
        jQuery("#slGenderName").select();
        return false;
    }
    if (jQuery("#listCloneRightFloat").is(":hidden")) {
        // 如果之前没有加载数据，加载克隆查询条件及刚做的客户信息
        GetCloneRightFloatList();
        // 列表的位置 = divRightSideFloat的高度 - listSectionExamedCount的高度
        document.getElementById("listCloneRightFloat").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listCloneRightFloat").height()) + "px";

        // 隐藏浮动框内部的所有列表层
        HideAllFloatList();

        jQuery("#listCloneRightFloat").show();
    } else {
        jQuery("#listCloneRightFloat").hide();
    }
}
/// <summary>
/// 从Cookie中获取数据
/// 修改人：黄兴茂
/// 修改日期：2013-10-21
/// </summary>
function GetCloneRightFloatList() {
    GetCustomerQueue();
}

/// <summary>
/// 查询需要克隆的客户信息
/// 修改人：黄兴茂
/// 修改日期：2013-10-21
/// </summary>
function SearchCloneCustomer() {
    var ID_Customer = jQuery.trim(jQuery("#cloneCustomerID").val()); //获取体检号
    var CustomerName = jQuery.trim(jQuery("#cloneCustomerName").val()); //获取客户性别
    var Gender = document.getElementById("slGenderName").options[document.getElementById("slGenderName").selectedIndex].value; //性别
    var GenderName = document.getElementById("slGenderName").options[document.getElementById("slGenderName").selectedIndex].text; //性别
    var BirthDay = jQuery.trim(jQuery("#selBirthDay").val()); //获取客户出生日期
    var IDCard = "";
    if (ID_Customer != "") {
        if (!isCustomerExamNo(ID_Customer)) {
            ShowSystemDialog("请您输入正确的体检号！");
            jQuery("#cloneCustomerID").focus();
            return false;
        }
    }
    else {
        if (CustomerName == "") {
            ShowSystemDialog("请您输入客户名称！");
            jQuery("#cloneCustomerName").focus();
            return false;
        }
        if (Gender == -1) {
            ShowSystemDialog("请您选择客户性别！");
            jQuery("#slGenderName").select();
            return false;
        }
    }
    //从列表中查找具有相同体检号的客户信息
    var NewElementHTML = "";
    var ID_CustomerElement = jQuery("#listCloneRightFloatData tr[ID_Customer='" + ID_Customer + "']");
    if (jQuery(ID_CustomerElement).length > 0) {
        jQuery(ID_CustomerElement).removeClass("history");
        NewElementHTML = jQuery(ID_CustomerElement)[0].outerHTML;
        jQuery(ID_CustomerElement).remove();
        jQuery("#listCloneRightFloatData").prepend(NewElementHTML);
        NewElementHTML = "";
        return false;
    }

    //从列表中查找具有相同客户名称的客户信息
    //    var CustomerNameElement = jQuery("#listCloneRightFloatData tr[CustomerName='" + CustomerName + "']");
    //    if (CustomerNameElement.length > 0) {
    //        jQuery(CustomerNameElement).each(function () {
    //            NewElementHTML += jQuery(this)[0].outerHTML;
    //            jQuery(this).remove();

    //        });
    //        if (NewElementHTML != "") {
    //            jQuery("#listCloneRightFloatData").prepend(NewElementHTML);
    //        }
    //        NewElementHTML = "";
    //        return false;
    //    }


    SearchCloneCustomer_Ajax(ID_Customer, CustomerName, GenderName, BirthDay, IDCard);
}
/// <summary>
///执行Ajax请求，查询需要克隆的客户信息
/// 修改人：黄兴茂
/// 修改日期：2013-10-21
/// </summary>
function SearchCloneCustomer_Ajax(ID_Customer, CustomerName, GenderName, BirthDay, IDCard) {
    jQuery("#listCloneRightFloatData tr[class!='history']").remove(); //移除查询结果
    //ajax请求：由于字符在get请求中超长，这里必须使用post方式提交请求
    jQuery.ajax({
        type: "POST",
        cache: false,
        url: "/Ajax/AjaxRegiste.aspx",
        data: { ID_Customer: ID_Customer,
            CustomerName: CustomerName,
            GenderName: GenderName,
            BirthDay: BirthDay,
            IDCard: IDCard,
            action: 'SearchCloneCustomer'
        },
        dataType: "json",
        success: function (msg) {
            if (msg == null || msg == undefined || msg.dataList == null || msg.dataList == undefined) return false;
            var NewContent = "";
            var RowNum = 1;
            var CloneCustomerTemplateBody = jQuery("#TemplateCloneCustomer tbody").html();
            jQuery(msg.dataList).each(function (i, item) {
                jQuery("#listCloneRightFloatData tr[ID_Customer='" + item.ID_Customer + "']").remove(); //移除已存在的项
                if (item.Base64Photo == "") {
                    item.Base64Photo = defalutImagSrc;
                }
                else {
                    if (item.Base64Photo.indexOf("nohead.gif") < 0) {
                        item.Base64Photo = "data:image/gif;base64," + item.Base64Photo;
                    }
                }
                NewContent += CloneCustomerTemplateBody.replace(/@RowNum/gi, RowNum)
                        .replace(/@ID_Customer/gi, item.ID_Customer)
                        .replace(/@CustomerName/gi, item.CustomerName)
                        .replace(/@GenderName/gi, item.GenderName)
                        .replace(/@BirthDay/gi, item.BirthDay)
                        .replace(/@Base64Photo/gi, item.Base64Photo)
                        .replace(/@class/gi, "Unhistory")
                        ;
                RowNum++;
            });
            if (NewContent != "") {
                jQuery("#listCloneRightFloatData").prepend(NewContent);
            }
        },
        complete: function () {

        }
    });
}
/// <summary>
/// 执行克隆
/// 修改人：黄兴茂
/// 修改日期：2013-10-21
/// 修改内容：从Cookie中获取登记信息
/// </summary>
function DoCustomerClone(ID_Customer) {
    //判断客户名称是否存在
    if (ID_Customer == null || ID_Customer == undefined) return false;
    ID_Customer = jQuery.trim(ID_Customer);
    if (!isCustomerExamNo(ID_Customer)) return false;
    if (confirm("克隆数据之前请确保您的数据已经保存，您确认要克隆[" + ID_Customer + "]所对应的数据吗?")) {
        GetCustomerCustFeeByID_Customer_Ajax(ID_Customer);
    }
}
/// <summary>
/// 通过指定体检号获取其对应收费项目
/// 修改人：黄兴茂
/// 修改日期：2013-10-20
/// 修改内容：通过指定体检号获取其对应收费项目
/// </summary>
function GetCustomerCustFeeByID_Customer_Ajax(ID_Customer) {
    jQuery.ajax({
        type: "POST",
        cache: false,
        url: "/Ajax/AjaxStatistics.aspx",
        data: { ID_Customer: ID_Customer, action: 'GetCloneCustFee' },
        dataType: "json",
        success: function (msg) {
            if (msg == null || msg == undefined || msg.dataList == null || msg.dataList == undefined) return false;

            var newContent = '', checked = false, ID_Fee = '', userName = '', date = '', FeeName = '', Price = 0, Discount = document.getElementById("txtXMZK").value, FactPrice = 0, FeeType = document.getElementById("slFeeWay").value, SectionID = '', SectionName = '';
            if (Discount > 10) {
                Discount = 10;
                document.getElementById("txtXMZK").value = Discount;
            }
            var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("TemplateRegistCustFee");
            var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
            var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
            var RowNum = jQuery("#tblList tbody tr[id!='loading']").length;
            jQuery(msg.dataList).each(function (i, item) {
                //如果没有行或者没有当前ID的非退费项目则允许添加到列表中
                if (jQuery("#tblList tbody tr[id!='loading'][id_fee='" + item.ID_Fee + "'][custfeechargestate!='2']").length == 0)//判断是否包含当前ID的非退费项目，不包含则新增
                {
                    RowNum++;
                    ID_Fee = item.ID_Fee;
                    userName = item.userName;
                    date = item.date;
                    FeeName = item.FeeName;
                    Price = item.Price;
                    //Discount = jQuery(this).parent().parent().attr("Discount");
                    FactPrice = item.FactPrice;
                    CustFeeChargeState = item.CustFeeChargeState;
                    ID_Section = item.ID_Section;
                    SectionName = item.SectionName;

                    if (teamTaskGroupFeeListBodyTempleteContent != null) {
                        newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@ItemCheckbox/gi, "ItemCheckbox")
                            .replace(/@ID_TeamTaskGroup/gi, "")
                             .replace(/@exist/gi, 0)
                            .replace(/@Is_Org/gi, 0)
                            .replace(/@ID_Customer/gi, item.ID_Customer)
                            .replace(/@ID_Fee/gi, ID_Fee)
                            .replace(/@FeeName/gi, FeeName)
                            .replace(/@FeeChargeStaute/gi, "")
                            .replace(/@Price/gi, Price)
                            .replace(/@FactPrice/gi, FactPrice)
                            .replace(/@Is_FeeRefund/gi, 0)
                            .replace(/@Is_FeeCharged/gi, 0)
                            .replace(/@Discount/gi, Discount)

                            .replace(/@ID_Section/gi, ID_Section)
                            .replace(/@SectionName/gi, SectionName)
                            .replace(/@date/gi, item.date)
                            .replace(/@FeeType/gi, FeeType)
                            .replace(/@FeeWayName/gi, jQuery(allHiddFeeWay).find("option[value='" + FeeType + "']").text())
                            .replace(/@RowNum/gi, RowNum)
                            .replace(/@CustCssStyle/gi, "NewXM")
                            .replace(/@ID_Section/gi, ID_Section)
                            .replace(/@SectionName/gi, SectionName)
                            .replace(/@ID_CustFee/gi, "")
                            .replace(/@CustFeeChargeState/gi, "0")
                            .replace(/@Is_Printed/gi, "0")

                            .replace(/@ID_Discounter/gi, DisUserID)
                            .replace(/@XDiscounterName/gi, DisUserName)
                            .replace(/@ID_Register/gi, UserID)
                            .replace(/@RegisterName/gi, UserName)
                            .replace(/@RegistDate/gi, CurDate)
                            .replace(/@XFeeChargeStaute/gi, "")
                            ;
                    }
                }
            });
            if (newContent != '') {
                IsSaved = false;
                jQuery("#tblList tbody").append(newContent);
                newContent = "";
                DoScrollToBottom();
                BindFeeWay();
                BindKeyup();
                ResetSearch();
                DoClose();
            }
            SumJG(); //计算合计
            return false;
        },
        complete: function () {
            jQuery("#listCloneRightFloat").hide();
        }
    });
}
var defalutImagSrc = "/template/blue/images/icons/nohead.gif"; //默认头像
/// <summary>
/// 添加数据到列表中
/// 修改人：黄兴茂
/// 修改日期：2013-10-21
/// 修改内容：从Cookie中获取登记信息
/// </summary>
function AddCustomerQueue(ID_Customer, CustomerName, GenderName, BirthDay, IDCard, Base64Photo) {
    if (Base64Photo == "") {
        Base64Photo = defalutImagSrc;
    }
    else {
        if (Base64Photo.indexOf("nohead.gif") < 0) {
            Base64Photo = "data:image/gif;base64," + Base64Photo;
        }
    }
    var NewContent = "";
    var RowNum = 1;
    var CloneCustomerTemplateBody = jQuery("#TemplateCloneCustomer tbody").html();
    var NewContent = CloneCustomerTemplateBody.replace(/@RowNum/gi, RowNum)
                        .replace(/@ID_Customer/gi, ID_Customer)
                        .replace(/@CustomerName/gi, CustomerName)
                        .replace(/@GenderName/gi, GenderName)
                        .replace(/@BirthDay/gi, BirthDay)
                        .replace(/@Base64Photo/gi, Base64Photo)
                        .replace(/@class/gi, "history")
                        ;
    RowNum++;
    jQuery("#listCloneRightFloatData").prepend(NewContent);
    var CloneCustomer = ID_Customer + ","; // ID_Customer + "@" + CustomerName + "@" + GenderName + "@" + BirthDay + "@" + IDCard + "@" + Base64Photo + "|";
    var CookieCustomerQueue = GetCookie("CustomerQueue");
    if (CookieCustomerQueue == null) {
        CookieCustomerQueue = CloneCustomer;
    }
    else {
        CookieCustomerQueue = CloneCustomer + CookieCustomerQueue;
    }
    SetCookie('CustomerQueue', CookieCustomerQueue); //将用户登记客户写入Cookie
}
/// <summary>
/// 从Cookie中获取登记信息
/// 修改人：黄兴茂
/// 修改日期：2013-10-21
/// 修改内容：从Cookie中获取登记信息
/// </summary>
function GetCustomerQueue() {
    //  CloneCustomer = ID_Customer + "," + CustomerName + "," + GenderName + "," + BirthDay + "," + IDCard + "," + jQuery("#HeadImg").attr("Base64Photo") + "|";
    jQuery("#listCloneRightFloatData").empty(); //清除列表数据
    var CustomerQueue = GetCookie("CustomerQueue");
    if (CustomerQueue != null) {
        jQuery.ajax({
            type: "POST",
            cache: true,
            url: "/Ajax/AjaxRegiste.aspx",
            data: { ID_Customer: CustomerQueue,
                action: 'GetCustomerQueue'
            },
            dataType: "json",
            success: function (msg) {
                if (msg == null || msg == undefined || msg.dataList == null || msg.dataList == undefined) return false;
                var NewContent = "";
                var RowNum = 1;
                var CloneCustomerTemplateBody = jQuery("#TemplateCloneCustomer tbody").html();
                jQuery(msg.dataList).each(function (i, item) {
                    jQuery("#listCloneRightFloatData tr[ID_Customer='" + item.ID_Customer + "']").remove(); //移除已存在的项
                    if (item.Base64Photo == "") {
                        item.Base64Photo = defalutImagSrc;
                    }
                    else {
                        if (item.Base64Photo.indexOf("nohead.gif") < 0) {
                            item.Base64Photo = "data:image/gif;base64," + item.Base64Photo;
                        }
                    }
                    NewContent += CloneCustomerTemplateBody.replace(/@RowNum/gi, RowNum)
                        .replace(/@ID_Customer/gi, item.ID_Customer)
                        .replace(/@CustomerName/gi, item.CustomerName)
                        .replace(/@GenderName/gi, item.GenderName)
                        .replace(/@BirthDay/gi, item.BirthDay)
                        .replace(/@Base64Photo/gi, item.Base64Photo)
                        .replace(/@class/gi, "history")
                        ;
                    RowNum++;
                });
                if (NewContent != "") {
                    jQuery("#listCloneRightFloatData").prepend(NewContent);
                }
            },
            complete: function () {

            }
        });
    }
    //    if (CustomerQueue != null) {
    //        var CustomerQueueArr = CustomerQueue.split("|");
    //        var CustomerQueueLength = CustomerQueueArr.length;
    //        if (CustomerQueueLength > 0) {
    //            var RowNum = 1;
    //            var NewContent = "";
    //            var DetailCustomerQueueArr = "";
    //            var CloneCustomerTemplateBody = jQuery("#TemplateCloneCustomer tbody").html();
    //            for (var c = CustomerQueueLength - 1; c >= 0; c--) {
    //                if (CustomerQueueArr[c] != "") {
    //                    DetailCustomerQueueArr = CustomerQueueArr[c].split("@");
    //                    if (DetailCustomerQueueArr.length > 0) {
    //                        if (DetailCustomerQueueArr[0] != "null") {
    //                            if (DetailCustomerQueueArr[5] == "") {
    //                                DetailCustomerQueueArr[5] = defalutImagSrc;
    //                            }
    //                            else {
    //                                if (DetailCustomerQueueArr[5].indexOf("nohead.gif") < 0) {
    //                                    DetailCustomerQueueArr[5] = DetailCustomerQueueArr[5];
    //                                }
    //                            }
    //                            NewContent += CloneCustomerTemplateBody.replace(/@RowNum/gi, RowNum)
    //                        .replace(/@ID_Customer/gi, DetailCustomerQueueArr[0])
    //                        .replace(/@CustomerName/gi, DetailCustomerQueueArr[1])
    //                        .replace(/@GenderName/gi, DetailCustomerQueueArr[2])
    //                        .replace(/@BirthDay/gi, DetailCustomerQueueArr[3])
    //                        .replace(/@Base64Photo/gi, DetailCustomerQueueArr[5])
    //                        .replace(/@class/gi, "history")
    //                        ;
    //                            RowNum++;
    //                        }
    //                    }
    //                }
    //            }
    //            if (NewContent != "") {
    //                jQuery("#listCloneRightFloatData").append(NewContent);
    //            }
    //        }
    //    }
}
/***************************克隆相关函数 End ************************************/


/// <summary>
/// 隐藏显示已检查科室列表
/// </summary>
function ShowHideSectionExamedCount() {
    if (jQuery("#listSectionExamedCount").is(":hidden")) {
        // 如果之前没有加载数据，则在这里先调用数据加载函数 20130721 by WTang
        GetSectionExamedCountList();
        // 列表的位置 = divRightSideFloat的高度 - listSectionExamedCount的高度
        document.getElementById("listSectionExamedCount").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listSectionExamedCount").height()) + "px";

        // 隐藏浮动框内部的所有列表层
        HideAllFloatList();

        jQuery("#listSectionExamedCount").show();
    } else {
        jQuery("#listSectionExamedCount").hide();
    }
}

/*******************************分科对比 start ************************************************/

var CustomerIDList = ""; // 客户所有体检号列表
/// <summary>
/// 隐藏显示历史体检号对比列表
/// </summary>
function ShowHideHistoryExamCount() {
    if (jQuery("#listHistoryExamCount").is(":hidden")) {
        // 如果之前没有加载数据，则在这里先调用数据加载函数 20130721 by WTang
        GetCustomerIDList();
        // 列表的位置 = divRightSideFloat的高度 - listHistoryExamCount的高度
        document.getElementById("listHistoryExamCount").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listHistoryExamCount").height()) + "px";

        // 隐藏浮动框内部的所有列表层
        HideAllFloatList();

        jQuery("#listHistoryExamCount").show();
    } else {
        jQuery("#listHistoryExamCount").hide();
    }
}

/// <summary>
/// 获取客户体检号列表
/// </summary>
function GetCustomerIDList() {
    if (CustomerIDList != "") {
        var CurrShowSectionCustomerID = jQuery("#txtSectionID").val() + "_" + jQuery('#HiddenIDCardNo').val();  // 科室+身份证
        if (CurrShowSectionCustomerID != jQuery("#iptCurrShowSectionCustomerID").val())// 上次查询的 科室+身份证
        {
            GetCustomerHistorySectionExamItemList();
        } else {
            return;
        }
    }
    var tempIDCardNo = jQuery("#HiddenIDCardNo").val();                 // 身份证号码
    if (tempIDCardNo == "" || tempIDCardNo == undefined) { return; }
    var tempCustomerName = jQuery("#HiddenCustomerName").val();         // 客户姓名
    var tempSectionID = jQuery('#txtSectionID').val();                  // 科室编号
    if (tempSectionID == "" || tempSectionID == undefined) { return; }

    jQuery("#ulHistoryExamCount").html('<tr><td colspan="3" style="height:120px; line-height:120px; text-align:center;">数据正在加载中...</td></tr>');
    jQuery("#ulHistoryExamCountFooter").html("");
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxCustExam.aspx",
        data: { IDCardNo: tempIDCardNo,
            CustomerName: tempCustomerName,
            action: 'GetCustomerIDList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {

            if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {
                CustomerIDList = jsonmsg; // 保存客户ID列表
                var dateStr = "";
                jQuery(jsonmsg.dataList0).each(function (i, custiditem) {

                    dateStr = custiditem.ID_Customer; /// custiditem.ID_Customer.substring(6, 10) + "年" + custiditem.ID_Customer.substring(2, 4) + "月" + custiditem.ID_Customer.substring(4, 6) + "日";

                    // 默认情况下，当此数据与上一次数据进行比对
                    if (i == 0) {
                        jQuery("#selCustomerID_1").append("<option state='" + custiditem.ExamState + "' selected=selected value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    } else {
                        jQuery("#selCustomerID_1").append("<option state='" + custiditem.ExamState + "' value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    }
                    if (i == 1) {
                        jQuery("#selCustomerID_2").append("<option state='" + custiditem.ExamState + "' selected=selected value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    } else {
                        jQuery("#selCustomerID_2").append("<option state='" + custiditem.ExamState + "' value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    }
                });

                GetCustomerHistorySectionExamItemList();

                // 身份证号 + 姓名
                jQuery("#iptIDCardNoHistoryExamCount").val(tempIDCardNo + tempCustomerName);
            } else {

                // 身份证号 + 姓名
                jQuery("#iptIDCardNoHistoryExamCount").val("");
            }
        }
    });

}

/// <summary>
/// 获取客户某科室检查项目及结果值(获取选择的两个体检号对应的数据)
/// </summary>
function GetCustomerHistorySectionExamItemList() {

    var CustomerID_1_state = jQuery("#selCustomerID_1 option:selected").attr("state");
    var CustomerID_2_state = jQuery("#selCustomerID_2 option:selected").attr("state");

    var CustomerID_1 = jQuery("#selCustomerID_1").val();  // 第一个下拉框的值
    var CustomerID_2 = jQuery("#selCustomerID_2").val();                 // 第二个下拉框的值
    var tempSectionID = jQuery('#txtSectionID').val();                   // 科室编号
    if (tempSectionID == "" || tempSectionID == undefined) { return; }

    if (CustomerID_1 != "" && CustomerID_1 == CustomerID_2) {
        ShowSystemDialog("请选择不同的体检号进行对比！");
        return;
    }
    if (CustomerID_1 == "" || CustomerID_2 == "") {
        ShowSystemDialog("请选择体检号，然后再进行对比！");
        return;
    }

    jQuery("#ulHistoryExamCount").html('<tr><td colspan="3" style="height:120px; line-height:120px; text-align:center;">数据正在加载中...</td></tr>');
    jQuery("#ulHistoryExamCountFooter").html("");
    jQuery("#iptCurrShowSectionCustomerID").val("");     // 将上次查询的 科室+身份证 清空
    // 获取科室中某体检号的检查信息
    GetHistoryDataAndShow(tempSectionID, CustomerID_1, CustomerID_1_state, 1);
    GetHistoryDataAndShow(tempSectionID, CustomerID_2, CustomerID_1_state, 2);

}


/// <summary>
/// 获取科室中某体检号的检查信息
/// </summary>
/// 参数：科室ID , 体检号 ，所在位置 1，2（在第一列，还是在第二列）
function GetHistoryDataAndShow(ID_Section, ID_Customer, state, index) {
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxCustExam.aspx",
        data: { ID_Customer: ID_Customer,
            ID_Section: ID_Section,
            CustomerExamState: state,
            action: 'GetHistoryCustomerIDSectionExamItem',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            var HistoryExamCountTemplateContent = jQuery('#HistoryExamCountTemplate').html(); // 对比列表的模版
            var HistoryResultSummarytTemplateContent = jQuery('#HistoryResultSummaryTemplate').html(); // 对比列表的模版
            var tempIDCardNo = jQuery("#HiddenIDCardNo").val(); // 身份证号码
            var ExamItemResult01 = ""; // 第一列的值
            var ExamItemResult02 = ""; // 第二列的值
            if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {

                var currSectionCustomerID = ID_Section + "_" + tempIDCardNo;                    // 本次查询的 科室+身份证
                var lastSectionCustomerID = jQuery("#iptCurrShowSectionCustomerID").val();   // 上次查询的 科室+身份证
                if (currSectionCustomerID != "" && lastSectionCustomerID != "" && currSectionCustomerID == lastSectionCustomerID) {
                    //
                } else {
                    jQuery("#ulHistoryExamCount").html("");
                    jQuery("#ulHistoryExamCountFooter").html("");
                    jQuery("#iptCurrShowSectionCustomerID").val(currSectionCustomerID);     // 将本次查询的 科室+身份证 记录下来
                }

                var newcontent = "";
                jQuery(jsonmsg.dataList0).each(function (i, examitem) {

                    var bandobj = jQuery("#ExamName_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_ExamItem).html();
                    if (bandobj == undefined || bandobj == "" || bandobj == null) {
                        if (index == 1) {
                            ExamItemResult01 = examitem.ResultSummary;
                            ExamItemResult02 = "";
                        } else {
                            ExamItemResult01 = "";
                            ExamItemResult02 = examitem.ResultSummary;
                        }
                        newcontent =
                                HistoryExamCountTemplateContent.replace(/@IDCardNo/gi, tempIDCardNo)
                                                            .replace(/@ID_ExamItem/gi, examitem.ID_ExamItem)
                                                            .replace(/@ExamItemName/gi, examitem.ExamItemName)
                                                            .replace(/@ExamItemResult01/gi, ExamItemResult01)
                                                            .replace(/@ExamItemResult02/gi, ExamItemResult02)
                                                            ;
                        jQuery("#ulHistoryExamCount").append(newcontent);
                    } else {
                        jQuery("#ExamItem_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_ExamItem + "_" + index).html(examitem.ResultSummary);
                    }

                    // 检测是否需要标注为不同的颜色
                    // 如果是检验科，则检查结果中是否包含"↓","↑",如果包含，则显示不同颜色
                    if (ID_Section == "101") {
                        if (examitem.ResultSummary.indexOf("↓") >= 0 || examitem.ResultSummary.indexOf("↑") >= 0) {
                            jQuery("#tr_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_ExamItem).attr("Class", "DiffResultSummary");
                        }
                    } else {
                        if (index == 2) {
                            var ResultSummary01 = jQuery("#ExamItem_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_ExamItem + "_1").html();
                            if (ResultSummary01 != "" && examitem.ResultSummary != "" && ResultSummary01 != examitem.ResultSummary) {
                                jQuery("#tr_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_ExamItem).attr("Class", "DiffResultSummary");
                            } else {
                                jQuery("#tr_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_ExamItem).attr("Class", "");
                            }
                        }
                    }
                });
                jQuery(jsonmsg.dataList1).each(function (i, examitem) {

                    var bandobj = jQuery("#ResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section).html();
                    if (bandobj == undefined || bandobj == "" || bandobj == null) {
                        if (index == 1) {
                            ExamItemResult01 = examitem.SectionSummary.replace(/\n/gi, "<br\/>");
                            ExamItemResult02 = "";
                        } else {
                            ExamItemResult01 = "";
                            ExamItemResult02 = examitem.SectionSummary.replace(/\n/gi, "<br\/>");
                        }
                        newcontent =
                                HistoryResultSummarytTemplateContent.replace(/@IDCardNo/gi, tempIDCardNo)
                                                        .replace(/@ID_ExamItem/gi, examitem.ID_Section)
                                                        .replace(/@ExamItemName/gi, examitem.SummaryName)
                                                        .replace(/@ExamItemResult01/gi, ExamItemResult01)
                                                        .replace(/@ExamItemResult02/gi, ExamItemResult02)
                                                        ;
                        // 将小结文本拼接到后面 
                        jQuery("#ulHistoryExamCountFooter").html(newcontent);

                    } else {
                        jQuery("#ResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section + "_" + index).html(examitem.SectionSummary.replace(/\n/gi, "<br\/>"));

                        //                                // 标注不同颜色
                        //                                if (index == 2) {
                        //                                    var ResultSummary01 = jQuery("#ExamItem_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section + "_1").html();
                        //                                    if (ResultSummary01 != "" && examitem.SectionSummary.replace(/\n/gi, "<br\/>") != "" && ResultSummary01 != examitem.SectionSummary.replace(/\n/gi, "<br\/>")) {
                        //                                        jQuery("#trSummary_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section).attr("Class", "DiffResultSummary");
                        //                                    } else {
                        //                                        jQuery("#trSummary_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section).attr("Class", "");
                        //                                    }
                        //                                }
                    }


                });
                // 列表的位置 = divRightSideFloat的高度 - listHistoryExamCount的高度
                document.getElementById("listHistoryExamCount").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listHistoryExamCount").height()) + "px";

            }

        }
    });
}

/*******************************分科对比 end************************************************/


/*******************************总检对比 start ************************************************/

/// <summary>
/// 隐藏显示总检科室列表
/// </summary>
function ShowHideFinalExamCompare() {
    if (jQuery("#listFinalExamCompare").is(":hidden")) {
        // 如果之前没有加载数据，则在这里先调用数据加载函数 20130721 by WTang
        GetFinalExamCompareList();
        // 列表的位置 = divRightSideFloat的高度 - listFinalExamCompare的高度
        document.getElementById("listFinalExamCompare").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listFinalExamCompare").height()) + "px";

        // 隐藏浮动框内部的所有列表层
        HideAllFloatList();

        jQuery("#listFinalExamCompare").show(); // 显示总检对比结果列表
    } else {
        jQuery("#listFinalExamCompare").hide();
    }
}
var FinalExamCustomerIDList = ""; // 记录该客户的体检号列表
/// <summary>
/// 获取客户体检号列表
/// </summary>
function GetFinalExamCompareList() {
    if (FinalExamCustomerIDList != "") {

        if (jQuery('#HiddenIDCardNo').val() != jQuery("#iptIDCardNoFinalExamCompare_CompareList").val())// 上次查询的 身份证 
        {
            GetCustomerFinalHistoryCompareList();
        } else {
            return;
        }
    }
    var tempIDCardNo = jQuery("#HiddenIDCardNo").val();                 // 身份证号码
    if (tempIDCardNo == "" || tempIDCardNo == undefined) { return; }
    var tempCustomerName = jQuery("#HiddenCustomerName").val();         // 客户姓名

    jQuery("#ulFinalExamCompare").html('<tr><td colspan="3" style="height:120px; line-height:120px; text-align:center;">数据正在加载中...</td></tr>');
    jQuery("#ulFinalExamCompareFooter").html("");
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxCustExam.aspx",
        data: { IDCardNo: tempIDCardNo,
            CustomerName: tempCustomerName,
            action: 'GetCustomerIDList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {

            if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {
                FinalExamCustomerIDList = jsonmsg; // 保存客户ID列表
                var dateStr = "";
                jQuery(jsonmsg.dataList0).each(function (i, custiditem) {

                    dateStr = custiditem.ID_Customer; // custiditem.ID_Customer.substring(6, 10) + "年" + custiditem.ID_Customer.substring(2, 4) + "月" + custiditem.ID_Customer.substring(4, 6) + "日";

                    // 默认情况下，当此数据与上一次数据进行比对
                    if (i == 0) {
                        jQuery("#selFinalCustomerID_1").append("<option state='" + custiditem.ExamState + "' selected=selected value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    } else {
                        jQuery("#selFinalCustomerID_1").append("<option state='" + custiditem.ExamState + "' value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    }
                    if (i == 1) {
                        jQuery("#selFinalCustomerID_2").append("<option state='" + custiditem.ExamState + "' selected=selected value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    } else {
                        jQuery("#selFinalCustomerID_2").append("<option state='" + custiditem.ExamState + "' value='" + custiditem.ID_Customer + "'>" + dateStr + "</option>");
                    }
                });

                GetCustomerFinalHistoryCompareList();

                // 身份证号 + 姓名
                jQuery("#iptIDCardNoFinalExamCompare_CustList").val(tempIDCardNo + tempCustomerName);
            } else {

                // 身份证号 + 姓名
                jQuery("#iptIDCardNoFinalExamCompare_CustList").val("");
            }
        }
    });

}

/// <summary>
/// 获取客户总检结果对比数据(获取选择的两个体检号对应的数据)
/// </summary>
function GetCustomerFinalHistoryCompareList() {

    var CustomerID_1_state = jQuery("#selFinalCustomerID_1 option:selected").attr("state");
    var CustomerID_2_state = jQuery("#selFinalCustomerID_2 option:selected").attr("state");

    var CustomerID_1 = jQuery("#selFinalCustomerID_1").val();                 // 第一个下拉框的值
    var CustomerID_2 = jQuery("#selFinalCustomerID_2").val();                 // 第二个下拉框的值

    if (CustomerID_1 != "" && CustomerID_1 == CustomerID_2) {
        ShowSystemDialog("请选择不同的体检号进行对比！");
        return;
    }
    if (CustomerID_1 == "" || CustomerID_2 == "") {
        ShowSystemDialog("请选择体检号，然后再进行对比！");
        return;
    }

    jQuery("#ulFinalExamCompare").html('<tr><td colspan="3" style="height:120px; line-height:120px; text-align:center;">数据正在加载中...</td></tr>');
    jQuery("#ulFinalExamCompareFooter").html("");
    jQuery("#iptIDCardNoFinalExamCompare_CompareList").val("");     // 将上次查询的 体检号 清空
    // 获取某体检号的检查信息
    GetHistoryFinalDataAndShow(CustomerID_1, CustomerID_1_state, 1);
    GetHistoryFinalDataAndShow(CustomerID_2, CustomerID_1_state, 2);

}



/// <summary>
/// 获取体检号各个科室的检查信息
/// </summary>
/// 参数：体检号 ，所在位置 1，2（在第一列，还是在第二列）
function GetHistoryFinalDataAndShow(ID_Customer, state, index) {
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxConclusion.aspx",
        data: { ID_Customer: ID_Customer,
            CustomerExamState: state,
            action: 'GetCustomerFinalCompareData',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            var HistoryExamCountTemplateContent = jQuery('#HistoryFinalExamTemplate').html(); // 对比列表的模版
            var HistoryResultSummarytTemplateContent = jQuery('#HistoryFinalResultSummaryTemplate').html(); // 对比列表的模版
            var tempIDCardNo = jQuery("#HiddenIDCardNo").val(); // 身份证号码
            var ExamItemResult01 = ""; // 第一列的值
            var ExamItemResult02 = ""; // 第二列的值
            if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {

                var currSectionCustomerID = tempIDCardNo;                    // 本次查询的 身份证
                var lastSectionCustomerID = jQuery("#iptIDCardNoFinalExamCompare_CompareList").val();   // 上次查询的 身份证
                if (currSectionCustomerID != "" && lastSectionCustomerID != "" && currSectionCustomerID == lastSectionCustomerID) {
                    //
                } else {
                    jQuery("#ulFinalExamCompare").html("");
                    jQuery("#ulFinalExamCompareFooter").html("");
                    jQuery("#iptIDCardNoFinalExamCompare_CompareList").val(currSectionCustomerID);     // 将本次查询的 科室+身份证 记录下来
                }

                var newcontent = "";
                jQuery(jsonmsg.dataList0).each(function (i, examitem) {

                    var bandobj = jQuery("#SectionName_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section).html();
                    if (bandobj == undefined || bandobj == "" || bandobj == null) {
                        if (index == 1) {
                            ExamItemResult01 = examitem.SectionSummary;
                            ExamItemResult02 = "";
                        } else {
                            ExamItemResult01 = "";
                            ExamItemResult02 = examitem.SectionSummary;
                        }
                        newcontent =
                                HistoryExamCountTemplateContent.replace(/@IDCardNo/gi, tempIDCardNo)
                                                            .replace(/@ID_Section/gi, examitem.ID_Section)
                                                            .replace(/@SectionName/gi, examitem.SectionName)
                                                            .replace(/@SectionSummaryResult01/gi, ExamItemResult01)
                                                            .replace(/@SectionSummaryResult02/gi, ExamItemResult02)
                                                            ;
                        jQuery("#ulFinalExamCompare").append(newcontent);
                    } else {
                        jQuery("#SectionSummary_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section + "_" + index).html(examitem.SectionSummary);
                    }

                    // 检测是否需要标注为不同的颜色
                    if (index == 2) {
                        var ResultSummary01 = jQuery("#SectionSummary_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section + "_1").html();
                        if (ResultSummary01 != "" && examitem.SectionSummary != "" && ResultSummary01 != examitem.SectionSummary) {
                            jQuery("#trSection_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section).attr("Class", "DiffResultSummary");
                        } else {
                            jQuery("#trSection_" + jQuery("#HiddenIDCardNo").val() + "_" + examitem.ID_Section).attr("Class", "");
                        }
                    }
                });

                jQuery(jsonmsg.dataList1).each(function (i, examitem) {

                    var FinalConclusion_01 = "";
                    var ResultCompare_01 = "";
                    var MainDiagnose_01 = "";
                    var FinalDietGuide_01 = "";
                    var FinalSportGuide_01 = "";
                    var FinalHealthKnowlage_01 = "";


                    var FinalConclusion_02 = "";
                    var ResultCompare_02 = "";
                    var MainDiagnose_02 = "";
                    var FinalDietGuide_02 = "";
                    var FinalSportGuide_02 = "";
                    var FinalHealthKnowlage_02 = "";

                    var bandobj = jQuery.trim(jQuery("#ulHistoryExamCountFooter").html());
                    if (bandobj == "" || bandobj == undefined || bandobj == null) {
                        if (index == 1) {

                            FinalConclusion_01 = examitem.FinalConclusion.replace(/\n/gi, "<br\/>");
                            ResultCompare_01 = examitem.ResultCompare.replace(/\n/gi, "<br\/>");
                            MainDiagnose_01 = examitem.MainDiagnose.replace(/\n/gi, "<br\/>");
                            FinalDietGuide_01 = examitem.FinalDietGuide.replace(/\n/gi, "<br\/>");
                            FinalSportGuide_01 = examitem.FinalSportGuide.replace(/\n/gi, "<br\/>");
                            FinalHealthKnowlage_01 = examitem.FinalHealthKnowlage.replace(/\n/gi, "<br\/>");

                            FinalConclusion_02 = "";
                            ResultCompare_02 = "";
                            MainDiagnose_02 = "";
                            FinalDietGuide_02 = "";
                            FinalSportGuide_02 = "";
                            FinalHealthKnowlage_02 = "";
                        } else {

                            FinalConclusion_01 = "";
                            ResultCompare_01 = "";
                            MainDiagnose_01 = "";
                            FinalDietGuide_01 = "";
                            FinalSportGuide_01 = "";
                            FinalHealthKnowlage_01 = "";

                            FinalConclusion_02 = examitem.FinalConclusion.replace(/\n/gi, "<br\/>");
                            ResultCompare_02 = examitem.ResultCompare.replace(/\n/gi, "<br\/>");
                            MainDiagnose_02 = examitem.MainDiagnose.replace(/\n/gi, "<br\/>");
                            FinalDietGuide_02 = examitem.FinalDietGuide.replace(/\n/gi, "<br\/>");
                            FinalSportGuide_02 = examitem.FinalSportGuide.replace(/\n/gi, "<br\/>");
                            FinalHealthKnowlage_02 = examitem.FinalHealthKnowlage.replace(/\n/gi, "<br\/>");
                        }
                        newcontent =
                                HistoryResultSummarytTemplateContent.replace(/@IDCardNo/gi, tempIDCardNo)
                                                        .replace(/@FinalResult_1_01/gi, FinalConclusion_01)
                                                        .replace(/@FinalResult_2_01/gi, ResultCompare_01)
                                                        .replace(/@FinalResult_3_01/gi, MainDiagnose_01)
                                                        .replace(/@FinalResult_4_01/gi, FinalDietGuide_01)
                                                        .replace(/@FinalResult_5_01/gi, FinalSportGuide_01)
                                                        .replace(/@FinalResult_6_01/gi, FinalHealthKnowlage_01)

                                                        .replace(/@FinalResult_1_02/gi, FinalConclusion_02)
                                                        .replace(/@FinalResult_2_02/gi, ResultCompare_02)
                                                        .replace(/@FinalResult_3_02/gi, MainDiagnose_02)
                                                        .replace(/@FinalResult_4_02/gi, FinalDietGuide_02)
                                                        .replace(/@FinalResult_5_02/gi, FinalSportGuide_02)
                                                        .replace(/@FinalResult_6_02/gi, FinalHealthKnowlage_02)
                                                        ;
                        // 将文本拼接到后面 
                        jQuery("#ulFinalExamCompareFooter").html(newcontent);

                    } else {
                        jQuery("#FinalResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_1_0" + index).html(examitem.FinalConclusion.replace(/\n/gi, "<br\/>"));
                        jQuery("#FinalResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_2_0" + index).html(examitem.ResultCompare.replace(/\n/gi, "<br\/>"));
                        jQuery("#FinalResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_3_0" + index).html(examitem.MainDiagnose.replace(/\n/gi, "<br\/>"));
                        jQuery("#FinalResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_4_0" + index).html(examitem.FinalDietGuide.replace(/\n/gi, "<br\/>"));
                        jQuery("#FinalResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_5_0" + index).html(examitem.FinalSportGuide.replace(/\n/gi, "<br\/>"));
                        jQuery("#FinalResultSummary_" + jQuery("#HiddenIDCardNo").val() + "_6_0" + index).html(examitem.FinalHealthKnowlage.replace(/\n/gi, "<br\/>"));

                    }
                });

                // 列表的位置 = divRightSideFloat的高度 - listFinalExamCompare的高度
                document.getElementById("listFinalExamCompare").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listFinalExamCompare").height()) + "px";

            }

        }
    });
}

/*******************************总检对比 end ************************************************/



/// <summary>
/// 隐藏显示客户科室列表
/// </summary>
function ShowHideCustSectionList() {
    if (jQuery("#listSectionExamFloat").is(":hidden")) {
        // 如果之前没有加载数据，则在这里先调用数据加载函数 20130721 by WTang
        GetCustExamSectionList();  // 获取科室列表
        // 列表的位置 = divRightSideFloat的高度 - listSectionExamFloat的高度
        document.getElementById("listSectionExamFloat").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listSectionExamFloat").height()) + "px";

        jQuery("#listSectionExamedCount").hide();   // 隐藏已检查科室列表
        jQuery("#listHistoryExamCount").hide();     // 隐藏历史体检号对比列表

        jQuery("#listSectionExamFloat").show();
    } else {
        jQuery("#listSectionExamFloat").hide();
    }
}
var LastTimeSectionExamedCount = null; // 开始为空(比对客户已检科室数据是否需要重新读取)
// 根据客户体检号，查询客户已检的科室列表信息
function GetSectionExamedCountList() {

    var CustomerID = jQuery.trim(jQuery('#HiddenCustomerID').val()); // 当前客户的体检号
    var CustomerIDSectionExamedCount = jQuery.trim(jQuery('#iptCustomerIDSectionExamedCount').val());    // 已经加载数据对应体检号

    // 如果体检号为空
    if (CustomerID == "") {

        jQuery('#iptCustomerIDSectionExamedCount').val(""); //  查询失败，清空体检号

        jQuery("#ulSectionExamedCount").html("<tbody><tr><td style='text-align:center;' colspan='6'>请输入客户的体检号！</td></tr></tbody>");

        // 列表的位置 = divRightSideFloat的高度 - listSectionExamedCount的高度
        document.getElementById("listSectionExamedCount").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listSectionExamedCount").height()) + "px";
        return;
    }

    // 如果体检号一致，则不需要从新加载数据
    if (CustomerIDSectionExamedCount != "" && CustomerID == CustomerIDSectionExamedCount) {
        if (LastTimeSectionExamedCount != null) {
            var nowtime = new Date(); // 当前时间
            // 如果是在60秒以内，则不用再次查询
            if ((1 + nowtime.getTime() - LastTimeSectionExamedCount.getTime()) / 1000 < 60) {
                return;
            }
        }
    }

    LastTimeSectionExamedCount = new Date();

    jQuery("#ulSectionExamedCount").html("<tbody><tr><td style='text-align:center;' colspan='6'>正在加载科室小结...</td></tr></tbody>");
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxCustExam.aspx",
        data: { CustomerID: CustomerID,
            action: 'GetCustomerExamSectionList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {

            var sectioncount = 0; // 记录已经做过科室小结的科室数量
            var newcontent = "";
            var ExamSectionTemplateContent = jQuery('#SectionExamedCountTemplate').html();               //科室列表模版

            if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {

                jQuery('#iptCustomerIDSectionExamedCount').val(CustomerID); //  记录当前显示的体检号

                // 遍历科室
                jQuery(jsonmsg.dataList0).each(function (i, sectionitem) {
                    if (sectionitem.SectionSummary == "") { return true; } // 还没有进行科室小结的，不显示

                    sectioncount++;

                    newcontent +=
                            ExamSectionTemplateContent.replace(/@SectionName/gi, sectionitem.SectionName)
                            .replace(/@ID_Section/gi, sectionitem.ID_Section)
                            .replace(/@CustomerID/gi, CustomerID)
                            .replace(/@SummaryDoctorName/gi, sectionitem.SummaryDoctorName)
                            .replace(/@SectionSummaryFormatDate/gi, sectionitem.SectionSummaryDate)
                            .replace(/@SectionSummary/gi, sectionitem.SectionSummary.replace(/\n/gi, "<br\/>"))
                            .replace(/@SummaryName/gi, sectionitem.SummaryName)
                            .replace(/@Number/gi, i + 1)
                            ;
                });

                if (newcontent == "") {
                    jQuery("#ulSectionExamedCount").html("<tbody><tr><td style='text-align:center; line-height:150px;height:150px;' colspan='6'>该客户还未进行体检！</td></tr></tbody>");
                }
                else {
                    jQuery("#ulSectionExamedCount").html(newcontent);
                }
                // 列表的位置 = divRightSideFloat的高度 - listSectionExamedCount的高度
                document.getElementById("listSectionExamedCount").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listSectionExamedCount").height()) + "px";


            } else {
                jQuery('#iptCustomerIDSectionExamedCount').val(""); //  查询失败，清空体检号

                jQuery("#ulSectionExamedCount").html("<tbody><tr><td style='text-align:center;' colspan='6'>该客户还未进行体检！</td></tr></tbody>");

                // 列表的位置 = divRightSideFloat的高度 - listSectionExamedCount的高度
                document.getElementById("listSectionExamedCount").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listSectionExamedCount").height()) + "px";

            }
        }
    });

}


// 根据客户体检号，查询客户相关的科室列表信息
function GetCustExamSectionList() {

    var CustomerID = jQuery.trim(jQuery('#HiddenCustomerID').val());
    var CustomerIDSectionExam = jQuery.trim(jQuery('#iptCustomerIDSectionExam').val());
    // 如果体检号一致，则不需要从新加载数据
    if (CustomerIDSectionExam != "" && CustomerID == CustomerIDSectionExam) {
        return;
    }

    jQuery("#ulSectionList").html("<li>正在加载科室...</li>");
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxCustExam.aspx",
        data: { CustomerID: CustomerID,
            action: 'GetCustomerExamSectionList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {

            var newcontent = "";
            var ExamSectionTemplateContent = jQuery('#ExamSectionTemplate').html();               //科室列表模版

            if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {

                jQuery('#iptCustomerIDSectionExam').val(CustomerID); //  记录当前显示的体检号
                var sectioncount = 0;
                // 遍历科室
                jQuery(jsonmsg.dataList0).each(function (i, sectionitem) {
                    if (Is_LoginUserSection(sectionitem.ID_Section) == false) {
                        return true; // continue
                    }
                    sectioncount++;
                    newcontent +=
                            ExamSectionTemplateContent.replace(/@SectionName/gi, sectionitem.SectionName)
                            .replace(/@ID_Section/gi, sectionitem.ID_Section)
                            .replace(/@CustomerID/gi, CustomerID)
                            .replace(/@Number/gi, sectioncount)
                            ;
                });

                jQuery("#ulSectionList").html(newcontent);

                // 列表的位置 = divRightSideFloat的高度 - listSectionExamFloat的高度
                document.getElementById("listSectionExamFloat").style.top = (jQuery("#divRightSideFloat").height() - jQuery("#listSectionExamFloat").height()) + "px";


            } else {
                jQuery('#iptCustomerIDSectionExam').val(""); //  查询失败，清空体检号

                jQuery("#ulSectionList").html("<li>未找到科室</li>");
            }
        }
    });

}



/// <summary>
/// 分科检查结果预览
/// </summary>
function OpenSectionExamViewWindow(sectionid, customerid) {

    var tmpUrl = "/System/Exam/ExamView.aspx?txtSectionID=" + sectionid + "&HiddenCustomerID=" + customerid + "&num=" + Math.random();
    art.dialog.open(tmpUrl,
            {
                width: 600,
                height: 380,
                lock: true,
                drag: false,
                id: 'OperWindowFrame',
                title: "分科检查结果预览"
            });
}


// -------------------------- 右边浮动层控制及鼠标事件 end -------------------------------


/// <summary>
/// 点击侧边栏浮动中的科室切换
/// </summary>
function ReLoadCustomerSection(href, ID_Section) {
    SetCookie('FYHSubSectionID', ID_Section);

    var FYHSubMenuID = GetCookie('FYHSubMenuID');       // 获取当前的子菜单ID
    var FYHSubSectionID = GetCookie('FYHSubSectionID'); // 获取当前的子菜单科室ID（如果不是分检，则该值为空）

    jQuery(".quick-actions li a").removeClass("selected");

    //            if (FYHSubMenuID != "" && FYHSubMenuID != null && FYHSubSectionID != "" && FYHSubSectionID != null) {
    //                jQuery("#menu_" + FYHSubMenuID + "_" + FYHSubSectionID).removeClass("selected");    // 设置之前为为未选择状态
    //            }
    if (FYHSubMenuID != "" && FYHSubMenuID != null) {
        jQuery("#menu_" + FYHSubMenuID + "_" + ID_Section).attr("class", "selected");       // 设置当前点击的菜单为选中状态
    }
    DoLoad(href, "");
}
