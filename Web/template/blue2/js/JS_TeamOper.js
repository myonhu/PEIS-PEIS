﻿/*
*该文件为团体备单界面公用脚本
*/

var defalutImagSrc = "/template/blue/images/icons/nohead.gif"; //默认头像
//修改人：黄兴茂
//修改时间：2013-07-26
//修改内容：通过页面参数，动态绑定团体、任务、任务分组联动功能
var type = ""; //为当前操作类型，有Add、Edit值
var modelName = ""; //参数：上一页面来源
var curTeam = ""; //参数:团体ID
var curTeamName = ""; //参数:团体名称
var curTeamTask = ""; //参数:任务ID
var curTeamGroup = ""; //参数:分组ID
var curTeamTaskName = ""; //参数:团体任务名称
var curFixed = 2; //有效数字
var choiceBusSetText = "----"; //下拉菜单默认选择内容
var allBusSet = '';  //获取所有的套餐内容
var editTitle = "点击查看该任务的分组信息"; //操作链接的提示信息
var IsFirstLoad = false;                  //是否第一次加载页面
var CurDate = jQuery("#CurDate").val()    //服务器当前日期
/// <summary>
/// DOM元素按键事件
/// </summary>
function OnFormKeyUp(e) {
    var curEvent = window.event || e;
    var id = document.activeElement.id;
    if (id == "txtSearch" && (curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40)) {
        keyMove(document.getElementById(id), curEvent); return;
    }
    else if (id == "txtCloneTeamTaskGroupID") //如果是在克隆文本框中按回车键，则自动触发克隆事件
    {
        if (curEvent.keyCode == 13)//回车事件
        {
            //            if (jQuery.trim(jQuery("#txtCloneTeamTaskGroupID").val()) != "") {
            //                DoCloneCustFee();
            //            }
        }
    }
    return false;
}

/// <summary>
/// 按键弹起事件
/// </summary>
function OnKeyUp() {
    SearchBusFee();
}
/// <summary>
/// 设置表横向滚动 
/// </summary>
function TableScrollByID(titleID, scrollID) {
    var $scrollControl = $("#" + scrollID);
    if ($scrollControl.length > 0) {
        var widthLeft = $scrollControl.width() - $scrollControl[0].clientWidth;
        if (widthLeft > 0) {
            var $scrollTitle = $("#" + titleID);
            $scrollTitle.css("width", $scrollTitle.width() + widthLeft);
        }
        $scrollControl.bind("scroll.j-control", function () {
            var left = $(this).scrollLeft();
            $("#" + titleID).css("margin-left", 0 - left);
        });
    }
}
/// <summary>
/// DOM加载完毕事件
/// 此脚本由团体维护、团体备单调用
///modelName:teamlist 表示团体维护调用 modelName：team团体备单调用
/// </summary>
jQuery(document).ready(function () {
    //团体、任务列表页面自适应高度计算
    if (jQuery("#QueryExamListData").length > 0) {
        jQuery("#QueryExamListData").attr("data-left", (269 + jQuery("#ShowUserMenuDiv").height()));
    }
    //团体任务名单自适应高度计算
    if (jQuery("#customerScrollControl").length > 0) {
        jQuery("#customerScrollControl").attr("data-left", (269 + jQuery("#ShowUserMenuDiv").height()));
    }
    if (jQuery("#teamTaskModifyDiv").length > 0) {
        jQuery("#teamTaskModifyDiv").drag({ handler: jQuery(".teamtaskTitle") });        //团体任务修改拖动
    }
    if (jQuery("#teamTaskAddDiv").length > 0) {
        jQuery("#teamTaskAddDiv").drag({ handler: jQuery(".teamtaskTitle") });           //团体任务新增拖动
    }
    if (jQuery("#showDialog").length > 0) {
        jQuery("#showDialog").drag({ handler: jQuery(".customerListtitle") });        //客户名单列表拖动
    }
    if (jQuery("#showDialogCustFee").length > 0) {
        jQuery("#showDialogCustFee").drag({ handler: jQuery(".custFeeTitle") });          //项目明细拖动
    }
    jQuery(".j-autoHeight").autoHeight(); // 自适应高度
    //jQuery(".j-scroll-control").TableScrollX();
    TableScrollByID("teamTaskGroupScrollTitle", "teamTaskGroupScrollControl");
    TableScrollByID("customerScrollTitle", "customerScrollControl");
    TableScrollByID("customerScrollTitleX", "customerScrollControlX");
    type = jQuery("#type").val(); //为当前操作类型，有Add、Edit值
    modelName = jQuery("#modelName").val() == undefined ? "" : jQuery("#modelName").val(); //参数：上一页面来源
    curTeam = jQuery("#curTeam").val(); //参数:团体ID
    curTeamName = decodeURI(jQuery("#curTeamName").val()); //参数:团体名称
    curTeamTask = jQuery("#curTeamTask").val() == undefined ? "" : jQuery("#curTeamTask").val(); //参数:任务ID
    curTeamTaskName = decodeURI(jQuery("#curTeamTaskName").val()); //参数:团体任务名称
    curTeamGroup = jQuery("#curTeamGroup").val() == undefined ? "" : jQuery("#curTeamGroup").val(); //参数:分组ID
    allBusSet = jQuery("#hidslBusSet").html(); //获取所有套餐内容
    //绑定选中团体的值

    if (modelName != undefined) {
        //当前模块名称如果是团体列表或者是团体操作，如果团体ID为为空则设置团体ID为******
        if (modelName.toLowerCase() == "teamlist" || modelName.toLowerCase() == "team") {
            if (ID_Team == "") {
                jQuery("#lblID_Team").text("******");
            }
            if (type != undefined) {
                //如果当前操作类型为add，则设置团体名称光标、并移除其只读属性，以便可以进行团体名称填写
                if (type.toLowerCase() == "add") {
                    document.getElementById("txtMyTeamName").focus();
                    jQuery("#txtMyTeamName").removeAttr("readOnly");
                }
                else if (type.toLowerCase() == "edit") {
                    jQuery("#txtMyTeamName").attr("readOnly", 'true');
                    jQuery("#txtMyTeamName").select();

                }
            }
        }
    }

    //绑定折扣变动事件
    if (jQuery("#txtXMZK").length > 0) {
        jQuery("#txtXMZK").keyup(function () {
            var curZK = jQuery.trim(jQuery("#txtXMZK").val());
            if (curZK != "") {
                if (parseFloat(curZK) < DisCountRate) {
                    curZK = DisCountRate;
                }
                if (curZK == 0) {
                    curZK = 10;
                }
                if (parseFloat(curZK) > 10) {
                    curZK = 10;
                }
                jQuery("#txtXMZK").val(curZK);
                //遍历所有勾选项设置统一折扣
                jQuery("[name='ItemCheckbox']").each(function () {
                    if (jQuery(this).attr("checked")) {
                        jQuery(this).parent().parent().find("[name = 'zk']").text(curZK);
                    }
                });
            }
            SumJG(); //计算总计
        });
    }


    //如果是从团体连接过来，这里需要默认加载团体任务
    if (modelName.toLowerCase() == "teamlist") {
        //设置选中项
        //        jQuery("#txtOperTeamNameX [value='" + curTeam + "']").attr("selected", true);
        //        jQuery("#txtOperTeamNameX").click();
        ShowQuickSelectTeam(curTeam, curTeamName);
        //绑定团体任务信息
        SearchTeamTask();
    }
    else if (modelName.toLowerCase() == "teamtask") {
        jQuery("#txtTeamNameX [value='" + curTeam + "']").attr("selected", true); //设置团体选中值
        jQuery("#txtTeamNameX").select2();
        ShowQuickSelectTeam(curTeam, curTeamName);
        ShowQuickSelectTeamTask(curTeamTask, curTeamTaskName);
        IsFirstLoad = true; //设置此值为true时默认勾选所有的分组信息
        TeamTaskCallBack();

        //修改人：黄兴茂 修改日期：2013-07-26 修改内容：绑定完数据后需要手动触发默认选中的团体任务[此内容适用于从任务中跳转链接的情况]
        //GetTeamTaskInfo();
    }
    else if (modelName.toLowerCase() == "dialogteamlist") //弹出框编辑团体信息
    {
        if (type.toLowerCase() == "add") {
            AddNewTeam();
        }
        else if (type.toLowerCase() == "edit") {
            var teamTask = new ReadTeamTaskTemplate("TeamTemplate_Edit", "TeamTaskEditList");
            var teamTaskListTheadTempleteContent = teamTask.teamTaskListTheadTempleteContent;
            var teamTaskListBodyTempleteContent = teamTask.teamTaskListBodyTempleteContent;
            jQuery("#TeamTaskEditList tbody").html(teamTaskListBodyTempleteContent);
            SetTableRowStyle();
        }
    }
    else if (modelName.toLowerCase() == "dialogteamtasklist") //弹出框编辑团体任务信息
    {
        if (type.toLowerCase() == "add") {
            AddNewTeamTask();
        }
        else if (type.toLowerCase() == "edit") {
            var teamTask = new ReadTeamTaskTemplate("TeamTaskTemplate_Edit", "TeamTaskEditList");
            var teamTaskListTheadTempleteContent = teamTask.teamTaskListTheadTempleteContent;
            var teamTaskListBodyTempleteContent = teamTask.teamTaskListBodyTempleteContent;
            jQuery("#TeamTaskEditList tbody").html(teamTaskListBodyTempleteContent);
            SetTableRowStyle();
        }
    }
    //调用select2
    if (jQuery("#tblRegistTableEdit select").length > 0) {
        jQuery("#tblRegistTableEdit select").select2();
    }
    ShowMe(); //设置显示或隐藏列表     
    if (document.getElementById("txtOperTeamNameX") != null) {
        jQuery("#txtOperTeamNameX").select2();
    }
});
//查询团体任务信息
function SearchTeamTask() {
    ResetSearchInfo("正在查询，请稍候...");
    //这里绑定团体任务信息
    var ID_Team = jQuery('#idSelectTeam').val();
    GetTeamTaskInfoByKeyWord("tblTeamTask", "ID_Team", ID_Team, 0);
}
var IsTeamTaskCallBack = false;
function TeamTaskCallBack() {
    ResetSearchInfo("正在查询，请稍候...");
    IsTeamTaskCallBack = true;
    GetTeamTaskGroupData_Ajax("tblTeamTaskGroupX", 0);

}
/// <summary>
/// 通过团体ID获取该团体任务信息
/// </summary>
function GetTeamTaskInfo() {
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "GetTeamTaskByTeam", ID_Team: curTeam },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            BindTeamTaskInfo(msg);
        }
    });
}

/****************************团体弹出任务框修改数据  Begin*************************************/

/// <summary>
/// 弹出团体任务编辑页面
///修改人：黄兴茂
///修改时间：2013-07-31
///修改内容：由于art.dialog.open方法在close时页面会空白，所以改用系统自带弹出页面,2013-07-31经大家讨论又重新启用该功能
/// </summary>
function OpenTeamTask(obj, title) {
    var url = jQuery(obj).attr("targeturl");
    if (title == "") {
        title = jQuery("#txtOperTeamNameX").find("option:selected").text() + "新增任务";
    }
    var selecteValue = jQuery("#txtOperTeamNameX").find("option:selected").val();

    if (selecteValue == -1) {
        ShowSystemDialog("请您选择团体名称！");
        return false;
    }
    url += "&curTeam=" + selecteValue + "&date=" + new Date();
    art.dialog.open(url,
    {
        width: 350,
        height: 300,
        drag: false,
        lock: true,
        id: 'OperWindowFrame',
        title: title,
        cache: false,
        init: function () {
            jQuery(".aui_close").hide(); //隐藏关闭按钮
        },
        close: function () {
            //重新绑定团体任务信息
            GetTeamTaskInfoByKeyWord("tblTeamTask", "ID_Team", jQuery('#idSelectTeam').val(), 0);
        }
    });
}
/****************************团体弹出任务框修改数据  End*************************************/

/// <summary>
/// 页面加载，调用Commom.js中页面加载原型
/// </summary>
function DoParentLoad() {
    DoLoad('/System/Customer/TeamList.aspx?type=' + type + '&modelName=modelName', '');
}

/// <summary>
/// 自定义页面加载
/// </summary>
function DoLoadX(obj) {
    if (jQuery(obj).attr("targeturl") != "") {
        DoLoad(jQuery(obj).attr("targeturl"), ''); //调用Commom.js中页面加载原型
    }
}

/// <summary>
/// 修改指定元素的样式为有边框，这里主要用于备单时新增客户双击文本可进行编辑功能
///此方法暂未使用
/// </summary>
function ChangeState(obj) {
    jQuery(obj).select();
    jQuery(obj).removeAttr("readonly");
    jQuery(obj).css("border", "1px solid blue");
    jQuery(obj).blur(function () {
        jQuery(obj).attr("readonly", "readonly");
        jQuery(obj).css("border", "0px solid");
    });
}

/// <summary>
///重置团体任务分组列表信息、重置客户名单列表信息
/// </summary>
function ResetTeamGroupChirldInfo() {
    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").remove();
    jQuery("#tblTeamTaskGroupFee tbody tr[id='loading']").show();
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id!='loading']").remove();
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id='loading']").show();
    ResetSumJG();
}

/// <summary>
///重置团体任务分组列表信息、重置客户名单列表信息、重置收费项目信息
/// </summary>
function ResetInfo() {
    jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").remove();
    jQuery("#tblTeamTaskGroupX tbody tr[id='loading']").show();
    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").remove();
    jQuery("#tblTeamTaskGroupFee tbody tr[id='loading']").show();
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id!='loading']").remove();
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id='loading']").show();
    jQuery("#chbAll1").attr("checked", false);
    ResetSumJG();
}

/// <summary>
///重置任务选中状态
/// </summary>
function ResetSearch() {
    jQuery("#chbAll1").attr("checked", false);
}

/// <summary>
/// 绑定团体列表元素变动事件，用于判断是否具有相同任务信息
/// </summary>
function BindTeamTaskChange(curObj) {
    //查找父元素中同辈元素中的所有和当前元素名称相同的团体任务分组
    var curTeamTaskGroupName = jQuery.trim(jQuery(curObj).val());
    var oldTeamTaskGroupName = jQuery(curObj).parent().attr("oldTeamTaskName");
    //如果新值和原始值不等，即元素值发生改变才判断
    if (curTeamTaskGroupName != oldTeamTaskGroupName) {
        jQuery(curObj).parent().attr("oldTeamTaskName", curTeamTaskGroupName);
    }
    if (curTeamTaskGroupName != "") {
        var siblingsCount = jQuery(curObj).parent().parent().siblings().find("td[oldTeamTaskName='" + curTeamTaskGroupName + "']").length;
        if (siblingsCount >= 1) {
            if (jQuery(curObj).parent().parent().attr("exist") == "1") {
                jQuery(curObj).val(oldTeamTaskGroupName);
            }
            else {
                jQuery(curObj).val("");
                jQuery(curObj).parent().attr("oldTeamTaskName", "");
            }
            ShowSystemDialog("已存在[" + curTeamTaskGroupName + "]的任务，请您修改");
            jQuery(curObj).focus();
            return false;
        }
    }
}

/// <summary>
/// 绑定团体任务信息
/// </summary>
function BindTeamTaskInfo(msg) {
    var choiceBusSetText = "-请选择任务--";
    var defaultoptions = '<option code="qzz" value="-1" selected="selected">' + choiceBusSetText + '</option>';
    var options = "";
    jQuery("#txtTeamTaskNameX").html(defaultoptions);
    if (msg.dataList != null) {
        if (msg.dataList.length > 0) {
            jQuery(msg.dataList).each(function (i, item) {
                options += '<option code="' + item.InputCode + '" value="' + item.ID_Team + '">' + item.TeamName + '</option>';
            });
            if (options != "") {
                options = defaultoptions + options;
            }
            else {
                options = defaultoptions;
            }
        }
    }
    jQuery("#txtTeamTaskNameX").html(options);
    jQuery("#txtTeamTaskNameX").find("option[value='-1']").attr("selected", true);
    jQuery("#s2id_txtTeamTaskNameX .select2-choice span").text(choiceBusSetText);
}

/****************************团体任务  Begin*******************************/

/// <summary>
/// 获取当前最大排序号，只是用于文本域
///ShowDataElementID:填充目标数据ID
///DispOrderName:排序号的name属性值
/// </summary>
function GetDispOrder(ShowDataElementID, DispOrderName) {
    var DispOrder = jQuery("#" + ShowDataElementID + " tbody tr").length == 0 ? 0 : jQuery("#" + ShowDataElementID + " tbody tr").length; //设置排序号
    jQuery("#" + ShowDataElementID + " tbody tr td input[name=" + DispOrderName + "]").each(function () {
        if (parseInt(jQuery(this).val()) > parseInt(DispOrder)) {
            DispOrder = parseInt(jQuery(this).val()) + 1;
        }
    });
    return (DispOrder == 0 ? 1 : DispOrder + 1);
}

/// <summary>
/// 新增团体任务
/// </summary>
function AddTeamTask(TempleteID, ShowDataElementID) {
    //判断是否选择了团体
    var selecteValue = jQuery("#txtOperTeamNameX").find("option:selected").val();
    if (selecteValue == -1) {
        ShowSystemDialog("请您选择团体名称！");
        return false;
    }
    var teamTask = new ReadTeamTaskTemplate(TempleteID, ShowDataElementID);
    var teamTaskListTheadTempleteContent = teamTask.teamTaskListTheadTempleteContent;
    var teamTaskListBodyTempleteContent = teamTask.teamTaskListBodyTempleteContent;
    if (teamTaskListBodyTempleteContent != "") {
        var txtContact = jQuery.trim(jQuery("#txtContact").val());
        var txtMobil = jQuery.trim(jQuery("#txtMobil").val());
        var DispOrder = GetDispOrder(ShowDataElementID, "txtDispOrder"); //获取排序号
        teamTaskListBodyTempleteContent = jQuery(teamTaskListBodyTempleteContent.replace(/@exist/gi, "0")
                            .replace(/@TeamTaskID/gi, "******")
                            .replace(/@OldTeamTaskName/gi, "")
                            .replace(/@Tel/gi, txtMobil)
                            .replace(/@TaskNumCount/gi, "0")
                            .replace(/@Contact/gi, txtContact)
                            .replace(/@DispOrder/gi, DispOrder)
                            .replace(/@TaskExamStartDate/gi, CreateDate)
                            .replace(/@TaskExamEndDate/gi, CreateDate)
                            .replace(/@InputCode/gi, ""));
    }


    //    jQuery(teamTaskListBodyTempleteContent).find("td input[name='TeamTaskName']").removeAttr("disabled");
    //    jQuery(teamTaskListBodyTempleteContent).find("td input[name='TaskExamStartDate']").removeAttr("disabled");
    //    jQuery(teamTaskListBodyTempleteContent).find("td input[name='TaskExamEndDate']").removeAttr("disabled");

    jQuery(teamTaskListBodyTempleteContent).find("td input[name='TeamTaskName']").val("");
    jQuery(teamTaskListBodyTempleteContent).find("td input[name='Tel']").val("");
    jQuery(teamTaskListBodyTempleteContent).find("td input[name='InputCode']").val("");
    teamTaskListBodyTempleteContent = jQuery(teamTaskListBodyTempleteContent)[0].outerHTML;
    if (jQuery('#' + ShowDataElementID + ' tbody').length == 0) {

        jQuery('#' + ShowDataElementID).append('<tbody>' + teamTaskListBodyTempleteContent + '</tbody>');
    }
    else {
        jQuery('#' + ShowDataElementID + ' tbody').prepend(teamTaskListBodyTempleteContent);
    }
    jQuery('input[name="TeamTaskName"]').first().focus();

}



/// <summary>
///绑定团体任务信息
/// </summary>
function BindData(ShowDataElementID, dataList) {
    jQuery('#' + ShowDataElementID + ' tbody tr[exist="1"]').html("");
    if (dataList != "") {
        var teamTask = new ReadTeamTaskTemplate("tblTemplateTeamTaskList", ShowDataElementID);
        var teamTaskListTheadTempleteContent = teamTask.teamTaskListTheadTempleteContent;
        var teamTaskListBodyTempleteContent = teamTask.teamTaskListBodyTempleteContent;
        var htmlContent = '';

        //        jQuery(teamTaskListBodyTempleteContent).find("td input[name='TeamTaskName']").val("");
        //        jQuery(teamTaskListBodyTempleteContent).find("td input[name='Tel']").val("");
        //        jQuery(teamTaskListBodyTempleteContent).find("td input[name='InputCode']").val("");
        jQuery(dataList).each(function (i, item) {
            htmlContent += teamTaskListBodyTempleteContent.replace(/@exist/gi, item.exist)
                            .replace(/@TeamTaskID/gi, item.ID_TeamTask)
                            .replace(/@OldTeamTaskName/gi, item.TeamTaskName)
                            .replace(/@Tel/gi, item.Tel)
                            .replace(/@TaskNumCount/gi, item.TaskNumCount)
                            .replace(/@Contact/gi, item.Contact)
                            .replace(/@DispOrder/gi, item.DispOrder)
                            .replace(/@TaskExamStartDate/gi, item.TaskExamStartDate)
                            .replace(/@TaskExamEndDate/gi, item.TaskExamEndDate)
                            .replace(/@InputCode/gi, item.InputCode)
                            .replace(/@CreateDate/gi, item.CreateDate)
                            .replace(/@editTitle/gi, editTitle)
                            .replace(/@ID_TeaM/gi, curTeam)
                            .replace(/@TeamTaskID/gi, item.ID_TeamTask)
                            ;

        });
        jQuery('#' + ShowDataElementID + ' tbody').html(htmlContent);
        SetTableRowStyle();
    }
}

/// <summary>
/// 判断指定团体包含的团体、任务、任务分组、分组名单、分组收费项目是否可用删除
/// </summary>
function ISCanDeleteTeamInfo(ID_Team) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanDeleteTeamInfo", ID_Team: ID_Team },
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}

/// <summary>
/// 判断指定团体任务是否可用删除
/// </summary>
function ISCanDeleteTeamTaskInfo(ID_TeamTask) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanDeleteTeamTaskInfo", ID_TeamTask: ID_TeamTask },
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}

/// <summary>
/// 判断指定团体任务分组信息是否可用删除
/// </summary>
function ISCanDeleteTeamTaskGroupInfo(ID_TeamTaskGroup) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanDeleteTeamTaskGroupInfo", ID_TeamTaskGroup: ID_TeamTaskGroup },
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}

/// <summary>
/// 判断指定团体任务名单信息是否可用删除
/// </summary>
function ISCanDeleteTeamTaskGroupCustomerInfo(ID_Customer) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanDeleteTeamTaskGroupCustomerInfo", ID_Customer: ID_Customer },
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}

/// <summary>
/// 通过团体和任务ID获取团体任务下的所有名单信息
///xmhuang 2014-01-08 使用异步调用
/// </summary>
function GetCustomerByTeamAndTask(ID_Team, ID_TeamTask) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "GET",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "GetCustomerByTeamAndTask", ID_Team: ID_Team, ID_TeamTask: ID_TeamTask },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}

/// <summary>
/// 通过团体和任务ID获取和团体任务分组ID获取所有客户名单
///xmhuang 2014-01-08 使用异步调用
/// </summary>
function GetTeamTaskGroupCustomerInfo(ID_Team, ID_TeamTask, ID_TeamTaskGroup) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "GET",
        async: true,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "GetTeamTaskGroupCustomerInfo", ID_Team: ID_Team, ID_TeamTask: ID_TeamTask, ID_TeamTaskGroup: ID_TeamTaskGroup },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
            BindCustomerCustInfo(msg.dataList);
        }
    });
    return allISDeleteDT;
}

/// <summary>
/// 删除团体任务
/// </summary>
function DeleteTeamTask() {
    //判断是否有选择，没有选择则直接return
    if (jQuery("#tblTeamTask tbody tr td input[type='checkbox'][name='ItemCheckbox']:checked").length == 0) {
        return false;
    }

    //获取所有的团体任务ID
    var CustTeamTaskID = "";
    jQuery("#tblTeamTask tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
        if (jQuery(this).parent().parent().attr('exist') == 1) {
            if (jQuery(this).attr('checked')) {
                CustTeamTaskID += "'" + jQuery(this).parent().parent().attr('id') + "',";
            }
        }
    });
    var curAllISDeleteDT = { "dataList": "" };
    if (CustTeamTaskID != "") {
        curAllISDeleteDT = ISCanDeleteTeamTaskInfo(CustTeamTaskID); //ISCanDeleteTeamInfo(jQuery('#idSelectTeam').val());
    }

    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/
    if (curAllISDeleteDT.success != undefined) {
        if (curAllISDeleteDT.success == -1) {
            ShowSystemDialog(curAllISDeleteDT.Message);
            return false;
        }
    }
    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/

    var CustTeamTaskID = '';
    var IsExist = false;
    var ErrorMessage = "";
    var CurID = "", TeamTaskName = "";
    //获取团体任务信息
    jQuery("#tblTeamTask tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
        if (jQuery(this).attr('checked')) {
            //判断当前团体任务是否可用用于删除
            IsExist = false;
            CurID = jQuery(this).parent().parent().attr('id');
            TeamTaskName = jQuery(this).parent().parent().find("td a[name='TeamTaskName']").text();
            jQuery(curAllISDeleteDT.dataList).each(function (i, item) {
                if (CurID == item.ID_TeamTask) {
                    ErrorMessage += TeamTaskName + ",";
                    IsExist = true;
                    return false;
                }
            });
            if (!IsExist) {
                if (jQuery(this).parent().parent().attr('exist') == 1) {
                    CustTeamTaskID += "'" + jQuery(this).parent().parent().attr('id') + "',";
                }
                //jQuery(this).parent().parent().remove();
            }
        }
    });
    if (ErrorMessage != "") {
        ErrorMessage = "团体任务[" + ErrorMessage.substring(0, ErrorMessage.length - 1) + "]已存在团体任务分组信息，不可删除,请重新选择！";
        ShowSystemDialog(ErrorMessage);
        return false;
    }

    var msgContent = "删除团体任务将无法修复！您确认要删除吗？";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                return true;
            }
        }, {
            name: '确定',
            callback: function () {
                jQuery("#tblTeamTask tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
                    if (jQuery(this).attr('checked')) {
                        jQuery(this).parent().parent().remove();
                        //CustTeamTaskID += jQuery(this).parent().parent().attr('id') + ",";
                    }
                });
                //判断是否有选中项目
                if (CustTeamTaskID != "") {
                    var qustData = { action: 'DeleteTeamTask',
                        modelName: modelName,
                        type: type,
                        CustTeamTaskID: CustTeamTaskID
                    }
                    jQuery.ajax({
                        type: "POST",
                        url: "/Ajax/AjaxTeamOper.aspx",
                        data: qustData,
                        cache: false,
                        dataType: "json",
                        success: function (msg) {
                            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
                            msg = CheckAjaxReturnDataInfo(msg);
                            if (msg == null || msg == "") {
                                return;
                            }
                            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
                            msg = CheckAjaxReturnDataInfo(msg);
                            if (msg == null || msg == "") {
                                return;
                            }
                            if (msg.success == 1) {
                                ShowSystemDialog(msg.Message);
                            } GetTeamTaskInfoByKeyWord("tblTeamTask", "ID_Team", jQuery('#idSelectTeam').val(), 0);

                        }
                    });
                }
                return true;

            }, focus: true
        }]

    }).lock();

}

/// <summary>
/// 绑定指定团体的任务信息
/// </summary>
/// <param name="ColumnName">需要查询的列名</param>
/// <param name="ColumnValue">需要匹配的值</param>
/// <param name="IsLike">是否启用模糊查询</param>
/// <returns></returns>
function GetTeamTaskInfoByKeyWord(ShowDataElementID, ColumnName, ColumnValue, IsLike) {
    var qustData = { action: 'GetTeamTaskInfoByKeyWord',
        modelName: modelName,
        type: type,
        ColumnName: ColumnName,
        ColumnValue: ColumnValue,
        IsLike: IsLike
    }
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        data: qustData,
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            //这里分页显示分组信息 xmhuang 2014-04-06
            if (msg != undefined) {
                pagerData = msg.dataList;
                tempOperPageCount = 0;
                QueryTeamTaskData(0);
                return false;
            }
            else {
                ResetSearchInfo("");
            }
            //BindData(ShowDataElementID, msg.dataList);
        }
    });

}

/****************************团体任务  End*******************************/

/****************************团体备单  Begin*******************************/
/// <summary>
/// 性别变动后修改当前行的套餐信息(name="slBusSet")
/// </summary>
function ChangeBusSet(obj) {
    //获取选中性别
    var Forsex = obj.options[obj.selectedIndex].value; //性别
    Forsex = (Forsex == 1 ? Forsex : 0); //0：女性，1男性，2：共用
    var GenderName = obj.options[obj.selectedIndex].text; //性别
    //找到当前行

    var ParentTr = jQuery(obj).parent().parent();
    //找到套餐对象
    var BusSet = jQuery(ParentTr).find("td select[name='slBusSet']");
    jQuery(BusSet).empty();
    var i = 0;
    //遍历套餐，隐藏不实用对象

    jQuery(allBusSet).each(function () {
        if (jQuery(this).attr("forgender") != null) {
            if (jQuery(this).attr("forgender") == Forsex || jQuery(this).attr("forgender") == "2") {
                if (i == 0) {
                    jQuery(BusSet).parent().find(".select2-choice span").text(choiceBusSetText);
                    jQuery(BusSet).prepend('<option code="qxz" value="-1">' + choiceBusSetText + '</option>').attr("selected", "selected"); //为select在第一个位置插入option
                    i = 1;
                }
                else {
                    jQuery(BusSet).prepend(jQuery(this)); //为select在第一个位置插入option
                }
            }
        }
    });
}
/// <summary>
/// 套餐实际变动
/// </summary>
function DoChangeBusSet(obj) {
    //找到套餐对象
    var BusSet = obj.options[obj.selectedIndex].value; //套餐ID
    if (BusSet == "-1") {
        return false;
    }

    var BusPEPackageName = obj.options[obj.selectedIndex].text; //套餐名称
    var ParentTr = jQuery(obj).parent().parent();
    var ID_TeamTaskGroup = jQuery(ParentTr).attr("id"); //任务分组ID
    //找到套餐对象
    var TeamTaskGroup = jQuery(ParentTr).find("td input[name='txtTeamTaskGroupName']");
    var TeamTaskGroupName = jQuery.trim(jQuery(TeamTaskGroup).val()); //任务分组名称
    //验证是否填写分组名称，且是否保存
    if (TeamTaskGroupName == "") {
        ShowSystemDialog("对不起，套餐变动之前必须保证任务分组存在且已经保存！");
        jQuery(TeamTaskGroup).focus();
        return false;
    }
    //判断是否有未保存的任务分组
    //    var teamTakGroupOfUnSaved = jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length;
    //    if (teamTakGroupOfUnSaved > 0) {
    //        ShowSystemDialog("对不起，有任务分组信息尚未保存，请先保存！");
    //        return false;
    //    }
    var FeeWay = jQuery.trim(jQuery(ParentTr).find("td select[name='slFeeWay']").find("option:selected").val());
    var FeeWayName = jQuery.trim(jQuery(ParentTr).find("td select[name='slFeeWay']").find("option:selected").text());
    var Gender = jQuery(ParentTr).find("td select[name='slSex']").find("option:selected").val(); //性别
    var GenderName = jQuery(ParentTr).find("td select[name='slSex']").find("option:selected").text(); //性别
    var AllFeeID = "", TeamTaskGroupID = "", AllItem = "";
    //    if (confirm("套餐变更后，系统将删除掉[分组为:" + TeamTaskGroupName + "]的所有收费项目，您确认要变更吗？")) {
    //遍历所有当前分组、当前套餐的收费项目，并删除
    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").each(function () {
        if (ID_TeamTaskGroup == "TaskGroup" || ID_TeamTaskGroup == "" || ID_TeamTaskGroup == undefined) {
            //表示还没有保存分组信息，这里按照名称来删除套餐信息
            if (jQuery.trim(jQuery(this).find("td label[name='fzmc']").text()) == TeamTaskGroupName) {
                jQuery(this).remove();
            }

        }
        else {
            if (jQuery(this).attr("id_teamtaskgroup") == ID_TeamTaskGroup) {
                TeamTaskGroupID = jQuery(this).attr('id_teamtaskgroup');
                AllFeeID = jQuery(this).attr('id_fee');
                AllItem += TeamTaskGroupID + "_" + AllFeeID + "|";
                jQuery(this).remove();
            }
        }
    });
    if (AllItem != "") {
        //执行后台删除
        DoDelete_Ajax(AllItem);
    }
    //加载套餐内项目
    DoAddTeamTaskGroupFee_Ajax("tblTeamTaskGroupFee", ID_TeamTaskGroup, TeamTaskGroupName, FeeWay, FeeWayName, BusSet, AllFeeID);
    //}
}

/// <summary>
/// Ajax重新绑定套餐内容：必须保存分组信息、必须勾选一项且只为一项的分组做为新增收费项目的所属项目
///ID_TeamGroup:分组ID
///BusSet:选择的套餐ID
///Gender:适用性别对象
/// </summary>
function DoAddTeamTaskGroupFee_Ajax(ShowDataElementID, ID_TeamGroup, TeamGroupName, FeeWay, FeeWayName, BusSet, AllFeeID) {
    jQuery("#" + ShowDataElementID + " tbody tr[id='loading'] td").text("正在加载，请稍候...");
    var data = { PEPackageID: BusSet, action: 'GetBusFeeBySetID' };
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: data,
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            var newContent = "";
            jQuery(msg.dataList).each(function (i, item) {
                var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("tblTemplateTeamTaskGroupFee");
                var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
                var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
                newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@class/gi, "NewXM")
                            .replace(/@exist/gi, "0")
                            .replace(/@ID_TeamTaskGroup/gi, ID_TeamGroup)
                            .replace(/@TeamTaskGroupName/gi, TeamGroupName)
                            .replace(/@FeeWayName/gi, FeeWayName)
                             .replace(/@FeeWay/gi, FeeWay)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)
                            .replace(/@Price/gi, item.Price)
                            .replace(/@Discount/gi, 10)
                            .replace(/@FactPrice/gi, item.FactPrice)
                            .replace(/@userName/gi, item.userName)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                             .replace(/@PEPackageID/gi, item.PEPackageID)
                            .replace(/@date/gi, item.date);
            });
            if (newContent != '') {

                jQuery("#" + ShowDataElementID + " tbody").append(newContent);
                newContent = "";
                DoScrollToBottom();
                SumJG(); //计算总计
            }
            else {
                //隐藏提示tr
                jQuery("#" + ShowDataElementID + " tbody tr[id='loading'] td").text("没有任何数据，请您维护！...");
            }
            jQuery("#" + ShowDataElementID + " tbody tr[id='loading']").hide();
        }
    });
}
/// <summary>
/// 判断指定团体任务收费项目是否可以删除
/// </summary>
function ISCanDeleteTeamTaskGroupFeeInfo(TeamTaskGroupID) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanDeleteTeamTaskGroupFeeInfo", TeamTaskGroupID: TeamTaskGroupID },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}
/// <summary>
/// 判断指定团体任务收费项目是否可以删除
/// </summary>
function IsCanSaveCustomer(TeamTaskGroupID) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "GET",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "IsCanSaveCustomer", TeamTaskGroupID: TeamTaskGroupID },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}
/// <summary>
/// 删除任务分组的收费项目
///修改人：xmhunag 
///修改时间:2013-10-23
///修改内容：删除收费项目时，现行验证该项目所在任务是否已经有名单存在，存在则不允许删除
/// </summary>
function DoDeleteTeamTaskGroupFee(ShowDataElementID) {
    //判断是否有选择
    if (jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']:checked").length == 0) {
        return false;
    }
    var msgContent = "删除收费项目后，将无法进行修复，您确认要删除吗？";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                return true;
            }
        }, {
            name: '确定',
            callback: function () {
                var ID_Team = jQuery('#txtTeamNameX').data("ID_Team");
                var curAllISDeleteDT = ""; // ISCanDeleteTeamInfo(ID_Team);
                var CustTeamTaskID = '';
                var IsExist = false;
                var ErrorMessage = "";
                var CurID = "", TeamTaskGroupName = "";
                var CurExists = "";
                var TeamTaskGroupID = "";
                var AllFeeID = "";
                var AllItem = "";
                jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
                    if (jQuery(this).attr('checked')) {
                        if (jQuery(this).parent().parent().attr('exist') == "1") {
                            if (jQuery(this).parent().parent().attr('id_fee') != undefined && jQuery(this).parent().parent().attr('id_teamtaskgroup') != undefined) {
                                TeamTaskGroupID = jQuery(this).parent().parent().attr('id_teamtaskgroup');
                                if (!IsExist) {
                                    if (jQuery(this).parent().parent().attr('id') != undefined && jQuery(this).parent().parent().attr('id_teamtaskgroup') != undefined) {
                                        CustTeamTaskID += "'" + jQuery(this).parent().parent().attr('id') + "',";
                                    }
                                }
                                AllFeeID = jQuery(this).parent().parent().attr('id_fee');
                                AllItem += TeamTaskGroupID + "_" + AllFeeID + "|";
                            }
                        }
                        jQuery(this).parent().parent().remove();
                    }
                });
                if (ErrorMessage != "") {
                    ErrorMessage = "团体任务分组[" + ErrorMessage.substring(0, ErrorMessage.length - 1) + "]下已存在团体成员，不可删除收费项目！";
                    ShowSystemDialog(ErrorMessage);
                    return false;
                }
                if (AllItem != "") {
                    //执行后台删除
                    DoDelete_Ajax(AllItem);
                    SumJG();
                }
                return true;

            }, focus: true
        }]

    }).lock();
}
/// <summary>
/// 执行后台删除收费数据
/// </summary>
function DoDelete_Ajax(AllItem) {
    //存储大数据请设置Content-length值
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "DeleteTeamTaskGroupFee", AllItem: AllItem },
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }

            //ShowSystemDialog(msg.Message);
            if (msg.success == "1") {
                //这里需要重新绑定分组任务
                //BindAllTeamAndTaskAndGroupAndCustInfo_Ajax();
            }
        },
        complete: function () {

        }
    });
}



/// <summary>
/// 新增团体任务分组
///TemplateTeamTaskGroupID:模版ID
///ShowDataElementID:需要加载模版内容的目标ID
/// </summary>
function AddTeamTaskGroup(TemplateTeamTaskGroupID, ShowDataElementID) {
    //判断团体任务是否存在
    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体任务分组信息尚未保存，请先保存！");
        return false;
    }
    var ID_Team = jQuery('#idSelectTeam').val();
    var TeamName = jQuery('#nameSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    var TeamTaskName = jQuery('#nameSelectTeamTask').val();
    ID_Team = encodeURIComponent(ID_Team);    //编码被查询医生
    if (ID_Team == "") {
        ShowSystemDialog("对不起，请您选择需要查看的团体!");
        return false;
    }
    if (ID_TeamTask == "") {
        ShowSystemDialog("对不起，请您选择需要查看的团体任务!");
        return false;
    }

    //    //判断任务分组是否保存
    //    if (jQuery("#txtTeamNameX").find("option:selected").val() == -1) {
    //        ShowSystemDialog("对不起，团体信息不允许为空，请您选择！");
    //        jQuery("#txtTeamNameX").select();
    //        return false;
    //    }
    //    //判断团体是否存在
    //    if (jQuery("#txtTeamTaskNameX").find("option:selected").val() == -1) {
    //        ShowSystemDialog("对不起，团体任务信息不允许为空，请您选择！");
    //        jQuery("#txtTeamTaskNameX").select();
    //        return false;
    //    }
    jQuery("#" + ShowDataElementID).find(".ParentMsg").remove(); //移除合并样式 xmhuang 2014-04-28
    jQuery("#" + ShowDataElementID + " tbody tr[id='loading']").hide();
    if (TemplateTeamTaskGroupID == "" || TemplateTeamTaskGroupID == undefined) {
        TemplateTeamTaskGroupID = "tblTemplateTeamTaskGroup";
    }
    var TemplateTeamTaskGroupContent = ReadTemplateTeamTaskGroup(TemplateTeamTaskGroupID);
    var teamTaskGroupListTheadTempleteContent = TemplateTeamTaskGroupContent.teamTaskGroupListTheadTempleteContent;
    var teamTaskGroupListBodyTempleteContent = TemplateTeamTaskGroupContent.teamTaskListBodyTempleteContent;
    teamTaskGroupListBodyTempleteContent = teamTaskGroupListBodyTempleteContent.replace(/@exist/gi, "0")
                            .replace(/@ID_Team/gi, "")
                            .replace(/@TeamTaskID/gi, "")
                            .replace(/@TeamTaskGroupName/gi, "")
                            .replace(/@MinAgeValue/gi, "")
                            .replace(/@MaxAgeValue/gi, "")
                            .replace(/@RoleName/gi, "")
                            .replace(/@oldTeamTaskGroupName/gi, "")
                             .replace(/@Is_GroupPaused/gi, "")
                              .replace(/@rowNum /gi, jQuery("#" + ShowDataElementID + " tbody tr").length)
                            ;
    //jQuery("#" + ShowDataElementID + " thead").html(teamTaskGroupListTheadTempleteContent);
    jQuery("#" + ShowDataElementID + " tbody").prepend(teamTaskGroupListBodyTempleteContent);
    jQuery("#" + ShowDataElementID + " tbody tr input[name='ItemCheckbox']").first().attr("checked", true);
    jQuery("#" + ShowDataElementID + " tbody tr input[name='ItemCheckbox']").first().change();
    jQuery("#" + ShowDataElementID + " tbody tr input[name='txtTeamTaskGroupName']").first().focus();
    //jQuery("#" + ShowDataElementID + " tbody tr td select[name='slBusSet']").select2(); //xmhuang 2014-04-04 移除select2调用方法
    jQuery("#" + ShowDataElementID + " tbody tr td select[name='slFeeWay'] option[value='3']").first().attr("selected", true);
    //重置文本值
    jQuery("#" + ShowDataElementID + " tbody tr td input[type='text']").each(function () {
        //jQuery(this).attr("maxLength", jQuery(this).attr("maxLength"));
        jQuery(this).val("");
    });
    ResetTeamGroupChirldInfo();
    SetTableRowStyle();
    CaltTeamTaskGroupOrder();



}
//汇总任务分组顺序 xmhuang 2014-04-01
function CaltTeamTaskGroupOrder() {
    var rowNum = 1;
    jQuery("#tblTeamTaskGroupX tbody tr td label[name='lblRowNum']").each(function () {
        jQuery(this).text(rowNum);
        rowNum++;
    });
    //设置固定表头 xmhuang 2014-04-01
    //$('#tblTeamTaskGroupX').tablefix({ height: 400, width: 950, fixRows: 2, fixCols: 3 });
}

function GetTeamTaskGroupID(TeamTaskGroupName) {
    var TeamTaskGroupID = -1;
    jQuery("#tblTeamTaskGroupX tbody tr td label[name='txtTeamTaskGroupName']").each(function () {
        if (TeamTaskGroupName == jQuery.trim(jQuery(this).text())) {
            TeamTaskGroupID = jQuery(this).parent().parent().attr("id");
            return false;
        }
    });
    return TeamTaskGroupID;
}
/// <summary>
/// 保存团体任务分组收费项目
/// </summary>
function SaveTeamTaskGroupFee(ShowDataElementID) {

    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").length == 0) {
        ShowSystemDialog("对不起，团体任务分组信息不能为空，请先维护！");
        return false;
    }
    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").length == 0) {
        ShowSystemDialog("对不起，团体收费项目不能为空，请先维护！");
        return false;
    }
    //判断任务分组是否保存
    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体任务分组信息尚未保存，请先保存！");
        return false;
    }
    DoSaveTeamTaskGroupFee(ShowDataElementID);
}
function DoSaveTeamTaskGroupFee(ShowDataElementID) {
    //AllItem
    //遍历所有的团体任务分组收费项目
    //获取当前分组ID()和收费ID
    var AllItem = "", TeamTaskGroupName = "", TeamTaskGroupID = "", TeampItem = "", exist = "", ID_TeamTaskGroup = "", ID_Fee = "", OriginalPrice = 0, Discount = 10, FactPrice = 0;
    var FeeWay, FeeWayName;
    jQuery("#" + ShowDataElementID + " tbody tr[id!='loading']").each(function () {
        TeamTaskGroupName = jQuery.trim(jQuery(this).find(" td label[name='fzmc']").text());
        exist = jQuery(this).attr("exist");
        //由于在新建团体任务分组时，团体任务分组ID是不存在的，需要动态通过名字获取
        if (ID_TeamTaskGroup == "") {
            ID_TeamTaskGroup = jQuery(this).attr("id_teamtaskgroup");
        }
        if (ID_TeamTaskGroup == "TaskGroup") {
            ID_TeamTaskGroup = GetTeamTaskGroupID(TeamTaskGroupName);
        }
        ID_Fee = jQuery(this).attr("id_fee");
        OriginalPrice = jQuery.trim(jQuery(this).find(" td label[name='yj']").text());
        Discount = jQuery.trim(jQuery(this).find(" td label[name='zk']").text());
        FactPrice = jQuery.trim(jQuery(this).find(" td label[name='sj']").text());

        FeeWay = jQuery.trim(jQuery(this).find(" td label[name='fffs']").attr("feeway"));  //付费方式ID  xmhuang 2014-04-13
        FeeWayName = jQuery.trim(jQuery(this).find(" td label[name='fffs']").text());  //付费方式名称  xmhuang 2014-04-13

        //如果没有获取到分组ID，这里需要从分组列表中动态获取
        if (ID_TeamTaskGroup == "" || ID_TeamTaskGroup == undefined || ID_TeamTaskGroup == "undefined") {
            TeamTaskGroupID = jQuery("#tblTeamTaskGroupX tbody tr td input[name='txtTeamTaskGroupName'][value='" + TeamTaskGroupName + "']").parent().parent().attr("id");
            ID_TeamTaskGroup = TeamTaskGroupID;
            jQuery(this).attr("id_teamtaskgroup", TeamTaskGroupID);
        }
        TeampItem = ID_TeamTaskGroup + "_" + ID_Fee + "_" + OriginalPrice + "_" + Discount + "_" + FactPrice + "_" + FeeWay + "_" + FeeWayName + "_" + exist;
        AllItem += TeampItem + "|";
    });
    if (AllItem != "") {
        //执行后台保存
        DoSaveTeamTaskGroupFee_Ajax(AllItem, ID_TeamTaskGroup);
    }
}
/// <summary>
/// 执行后台保存团体任务分组收费项目
/// </summary>
function DoSaveTeamTaskGroupFee_Ajax(AllItem, ID_TeamTaskGroup) {
    var qustData = { action: 'SaveTeamTaskGroupFee',
        AllTeamTaskGroupFeeItem: AllItem
    }
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        data: qustData,
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            //由于是事务保存，保存成功后，这里只需要更改客户端的存在标记即可
            jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist='0']").attr("id_teamtaskgroup", ID_TeamTaskGroup); //xmhuang 2013-12-11 绑定团体任务分组ID
            jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist='0']").attr("exist", "1");

            ShowSystemDialog(msg.Message);
            //            var AllTeamTaskGroupID = GetAllTeamTaskGroupID("tblTeamTaskGroupX");
            //            jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").remove();
            //            BindTeamTaskGroupFeeData_Ajax("tblTeamTaskGroupFee", AllTeamTaskGroupID);
        }
    });
}
function GetAllTeamTaskGroupID(ShowDataElementID) {
    var ID_TeamTaskGroup = "";
    jQuery("#" + ShowDataElementID + " tbody tr[id!='loading']").each(function () {
        ID_TeamTaskGroup += "'" + jQuery.trim(jQuery(this).attr("id")) + "',";
    });
    return ID_TeamTaskGroup;
}
/// <summary>
/// 滚动屏幕位置用于显示当前新增框
/// </summary>
function DoScrollToBottom() {
    //window.scrollTo(0, document.body.scrollHeight - 100);
    ScollToTbCustFeeBottom();
}

/// <summary>
/// 获取团体任务分组信息
/// </summary>
function GetTeamTaskGroupData_Ajax(ShowDataElementID, IsLike, TeamElementID) {
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    if ((ID_Team == "" || ID_TeamTask == "") || (ID_Team == undefined || ID_TeamTask == undefined)) {
        return false;
    }
    var qustData = { action: 'GetTeamTaskGroupInfoByTeamAndTask',
        modelName: modelName,
        type: type,
        ID_Team: ID_Team,
        ID_TeamTask: ID_TeamTask,
        IsLike: IsLike
    }
    jQuery.ajax({

        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        data: qustData,
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            BindTeamTaskGroupData(ShowDataElementID, msg.dataList);

        }
    });
}
/// <summary>
/// 绑定团体任务分组收费明细信息
///修改人：xmhuang
///修改时间:2013-10-26
///修改内容：经测试发现模板中如果设置class为空时，属性和值将被截断，无法正常其它属性，所以此处给class赋值为Empty
///ShowDataElementID:数据展示目标元素ID
///dataList:数据源
/// </summary>
function BindTeamTaskGroupFeeData_Ajax(ShowDataElementID, AllTeamTaskGroupID) {
    //判读是否有没有保存的数据，进行保存后重新绑定数据
    jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").attr("exist", "1");
    //这里保存成功了分组信息，如果存在收费项目没有保存，则自动保存收费项目
    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist='0']").length > 0) {
        //这里保存收费项目
        SaveTeamTaskGroupFee('tblTeamTaskGroupFee'); //自动保存未保存的收费项目 xmhuang 2014-03-28
    }
    else { ResetTeamGroupChirldInfo(); }
    //AllTeamTaskGroupFeeItem
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        data: { AllTeamTaskGroupID: AllTeamTaskGroupID, action: "GetTeamTaskGroupFeeDataByGroupID" },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            //这里绑定收费项目
            var newContent = "";
            var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("tblTemplateTeamTaskGroupFee");
            var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
            var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
            jQuery(msg.dataList).each(function (i, item) {
                newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@exist/gi, item.exist)
                             .replace(/@PEPackageID/gi, "")
                             .replace(/@class/gi, "Empty")
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                            .replace(/@TeamTaskGroupName/gi, item.TeamTaskGroupName)
                             .replace(/@FeeWayName/gi, item.FeeWayName)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)

                           .replace(/@FeeWay/gi, item.ID_FeeWay)
                            .replace(/@Price/gi, item.Price)
                            .replace(/@Discount/gi, item.Discount)
                            .replace(/@FactPrice/gi, item.FactPrice)
                            .replace(/@userName/gi, item.userName)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                // .replace(/@PEPackageID/gi, item.PEPackageID)
                            .replace(/@date/gi, item.date);
            });
            if (newContent != '') {
                //存在数据则移除掉客户新增行，重新绑定保存行
                jQuery("#" + ShowDataElementID + " tbody tr[id!='loading']").remove();
                jQuery("#" + ShowDataElementID + " tbody").append(newContent);
                newContent = "";
                //DoScrollToBottom();
                SumJG(); //计算总计
                //BindTeamAndTaskAndGroupAndCustInfo_Ajax();
                SetTableRowStyle();
            }
            else {
                //xmhuang 2013-10-23 此项操作将影响团体名单绑定，这里设置团体名单显示和隐藏不合理，由于收费项目不存在时也有可能存在团体成员
                ResetTeamGroupChirldInfo();
            }
            jQuery("#" + ShowDataElementID + " tbody tr[id='loading']").hide();
        }
    });

}
/// <summary>
/// 读取团体任务模版
///elementID：模版ID
///showDataElementID：填充目标数据ID
/// </summary>
function ReadTeamTaskTemplate(TempleteID, ShowDataElementID) {
    //默认是读取tblTemplateTeamTaskList模版填充到tblTeamTask中显示
    if (TempleteID == undefined) {
        TempleteID = "tblTemplateTeamTaskList";
    }
    if (ShowDataElementID == undefined) {
        ShowDataElementID = "tblTeamTask";
    }
    var teamTaskGroupListTheadTempleteContent = jQuery('#' + TempleteID + ' thead').html(); //团体任务模版Thead部分
    var teamTaskGroupListBodyTempleteContent = jQuery('#' + TempleteID + ' tbody').html(); //团体任务模版body部分
    this.teamTaskListTheadTempleteContent = teamTaskGroupListTheadTempleteContent;
    this.teamTaskListBodyTempleteContent = teamTaskGroupListBodyTempleteContent;
}
function GetSecurityLevel(SecurityLevel) {
    if (SecurityLevel == "") {
        return "";
    }
    else if (SecurityLevel == 1) {
        return "一级";
    }
    else if (SecurityLevel == 2) {
        return "二级";
    }
    else if (SecurityLevel == 3) {
        return "三级";
    }
    else if (SecurityLevel == 4) {
        return "四级";
    }
    else if (SecurityLevel == 5) {
        return "五级";
    }
    else if (SecurityLevel == 6) {
        return "六级";
    }
    else if (SecurityLevel == 7) {
        return "七级";
    }
    else if (SecurityLevel == 8) {
        return "八级";
    }
    else if (SecurityLevel == 9) {
        return "九级";
    }
    else if (SecurityLevel == 10) {
        return "十级";
    }
}
/// <summary>
/// 绑定团体任务分组信息
///ShowDataElementID:数据展示目标元素ID
///dataList:数据源
/// </summary>
function BindTeamTaskGroupData(ShowDataElementID, dataList) {
    //记录当前选中的分组
    jQuery('#' + ShowDataElementID + ' tbody tr[exist="1"]').remove();
    if (dataList != "") {
        var teamTaskGroup = new ReadTeamTaskTemplate("tblTemplateTeamTaskGroup", ShowDataElementID);
        var teamTaskGroupListTheadTempleteContent = teamTaskGroup.teamTaskGroupListTheadTempleteContent;
        var teamTaskListBodyTempleteContent = teamTaskGroup.teamTaskListBodyTempleteContent;
        var htmlContent = '';
        var tempContent = '';
        var AllTeamTaskGroupID = ""; //所有团体分组
        var RowNum = 1;
        jQuery(dataList).each(function (i, item) {
            //            if (item.Is_GroupPaused == 1) {
            //                item.Is_GroupPaused = "√"
            //            }
            //            else {
            //                item.Is_GroupPaused = "×"
            //            }
            AllTeamTaskGroupID += "'" + item.ID_TeamTaskGroup + "',";
            tempContent = jQuery(teamTaskListBodyTempleteContent.replace(/@exist/gi, item.exist)
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                            .replace(/@ID_Team/gi, item.ID_Team)
                            .replace(/@TeamTaskID/gi, item.ID_TeamTask)
                            .replace(/@TeamTaskGroupName/gi, item.TeamTaskGroupName)
                            .replace(/@oldTeamTaskGroupName/gi, item.TeamTaskGroupName)
                            .replace(/@ID_Gender/gi, item.ID_Gender)
                            .replace(/@Is_Married/gi, item.Is_Marriage)
                            .replace(/@MinAgeValue/gi, item.MinAgeValue)
                            .replace(/@MaxAgeValue/gi, item.MaxAgeValue)
                            .replace(/@ID_ExamType/gi, item.ID_ExamType)
                            .replace(/@PEPackageID/gi, item.PEPackageID)
                            .replace(/@RoleName/gi, item.RoleName)
                            .replace(/@ID_FeeWay/gi, item.ID_FeeWay)
                             .replace(/@GenderName/gi, item.GenderName)
                             .replace(/@MarriageName/gi, item.MarriageName)
                             .replace(/@ExamTypeName/gi, item.ExamTypeName)
                             .replace(/@PEPackageName/gi, item.PEPackageName)
                             .replace(/@FeeWayName/gi, item.FeeWayName)
                              .replace(/@FeeWay/gi, item.FeeWay)
                             .replace(/@Is_Marriage/gi, item.Is_Married)
                             .replace(/@RowNum/gi, RowNum)
                             .replace(/@SecurityLevel/gi, GetSecurityLevel(item.SecurityLevel))
                            );
            //设置选中项

            //jQuery(tempContent).find("td select[name='slMarried'] option[value='-1']")

            jQuery(tempContent).find("td select[name='slSex'] option[value='" + item.ID_Gender + "']").attr("selected", true);
            jQuery(tempContent).find("td select[name='slMarried'] option[value='" + item.Is_Married + "']").attr("selected", true);
            jQuery(tempContent).find("td select[name='slExamType'] option[value='" + item.ID_ExamType + "']").attr("selected", true);
            jQuery(tempContent).find("td select[name='slBusSet'] option[value='" + item.PEPackageID + "']").attr("selected", true);
            jQuery(tempContent).find("td select[name='slFeeWay'] option[value='" + item.ID_FeeWay + "']").attr("selected", true);
            //设置禁用按钮选中 xmhuang 2014-03-28
            if (item.Is_GroupPaused == 1) {
                jQuery(tempContent).find("td input[name='chcIsPause']").attr("checked", true);
            }
            htmlContent += jQuery(tempContent)[0].outerHTML;
            RowNum++;
        });
        jQuery('#' + ShowDataElementID + ' tbody').html(htmlContent);
        if (!IsHaveTeamTaskGoupPauseRight()) {
            jQuery('#' + ShowDataElementID + ' tbody tr[id!="loading"] input[type="checkbox"][name="chcIsPause"]').attr("disabled", true);  //设置禁用权限 xmhuang 2014-04-16
        }
        SetTableRowStyle();
        //设置固定表头 xmhuang 2014-04-01
        //$('#tblTeamTaskGroupX').tablefix({ height: 400, width: 950, fixRows: 2, fixCols: 3 });
        /*移除默认选中第一条并绑定数据 xmhuang 2013-12-03 
        //判断是否已经有选中项，如有则直接选中选中项，没有则默认选中第一个
        //importantmark xmhuang 2013-10-23 注释默认选中第一个分组和绑定对应信息的功能 Begin
        var curObj = jQuery("#" + ShowDataElementID + " tbody tr input[name='ItemCheckbox']:checked").first();
        if (jQuery(curObj).length > 0) {
        jQuery(curObj).attr("checked", true);
        ChangeGroupX(curObj);
        }
        else {
        jQuery("#" + ShowDataElementID + " tbody tr input[name='ItemCheckbox']").first().attr("checked", true);
        ChangeGroupX(jQuery("#" + ShowDataElementID + " tbody tr input[name='ItemCheckbox']").first());

        }
        //importantmark xmhuang 2013-10-23 注释默认选中第一个分组和绑定对应信息的功能 End
        */

        /*******************判断是否有未保存的收费项目提示保存 Begin xmhuang 2014-03-26***********************/

        if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist!='1']").length > 0) {
            DoSaveTeamTaskGroupFee("tblTeamTaskGroupFee");
        }
        /*******************判断是否有未保存的收费项目提示保存 End   xmhuang 2014-03-26***********************/
        //设置分组默认选中 xmhuang 2014-04-15
        if (IsTeamTaskCallBack || IsFirstLoad) {
            var checkbox3 = document.getElementById("Checkbox3");
            if (checkbox3 != null) {
                checkbox3.checked = true;
                CheckAllTeamTaskGroupDataX(checkbox3, 'tblTeamTaskGroupX');
                IsTeamTaskCallBack = false;
            }
        }
        //设置分组默认选中 xmhuang 2014-04-15
    }
    else {
        //没有分组信息则需要删除所有的收费项目
        jQuery('#tblTeamTaskGroupFee tbody tr[id!="loading"]').remove();
        ResetSearchInfo("");
    }
}
/// <summary>
/// 全选子元素
///obj:父元素
///ShowDataElementID: 需要设置元素选中状态的tableID
/// </summary>
function CheckAllTeamTaskGroupDataX(obj, ShowDataElementID) {

    jQuery("#" + ShowDataElementID + " tbody tr td input[name='ItemCheckbox']").each(function () {
        jQuery(this).attr('checked', obj.checked);
    })

}
/// <summary>
///重新绑定客户名单信息
/// </summary>
function ReBindTeamTaskGroupCustomerInfo() {
    if (jQuery("#tblTeamTaskGroupX tbody tr[exist='1'] td input:checked").length > 0) {
        var AllTeamTaskGroupID = "";
        jQuery("#tblTeamTaskGroupX tbody tr[exist='1'] td input:checked").each(function () {
            AllTeamTaskGroupID += jQuery(this).parent().parent().attr("id") + ",";
        });
        if (AllTeamTaskGroupID != "") {
            //mark绑定团体任务分组下的客户名单信息
            //mark绑定团体任务分组下的客户名单信息
            var ID_Team = jQuery('#idSelectTeam').val(); //获取团体ID
            var ID_TeamTask = jQuery('#idSelectTeamTask').val(); //获取团体任务ID
            var dataList = GetTeamTaskGroupCustomerInfo(ID_Team, ID_TeamTask, AllTeamTaskGroupID).dataList;
            //这里获取团体任务分组客户名单信息
            // BindCustomerCustInfo(dataList);//xmhuang 2014-01-08 注释
        }
    }
}
/// <summary>
/// 删除、保存团体任务分组后重新重新绑定收费项目
/// </summary>
function ChangeGroupX(obj) {

    // ResetTeamGroupChirldInfo();
    ShowOper();
    var AllTeamTaskGroupID = "TaskGroup";
    //如果当前选中，则移除其它选中项
    var checked = jQuery(obj).attr("checked");
    //checked = !checked;
    if (checked == "checked") {
        jQuery(obj).parent().parent().siblings().find("td input[type='checkbox']").removeAttr("checked");
        AllTeamTaskGroupID = jQuery(obj).parent().parent().attr("id");
    }
    else {
        //获取选中的团体任务分组ID
        jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
            if (jQuery(this).find("td input[type='checkbox']").attr("checked")) {
                AllTeamTaskGroupID = jQuery(this).attr("id");
            }
        });
    }
    if (AllTeamTaskGroupID == "TaskGroup") {
        jQuery('#tblTeamTaskGroupFee tbody tr[id!="loading"]').remove();
        jQuery('#tblTeamTaskGroupFee tbody tr[id="loading"]').show();
        ResetSumJG();
    }
    else {
        BindTeamTaskGroupFeeData_Ajax("tblTeamTaskGroupFee", AllTeamTaskGroupID);
        //mark绑定团体任务分组下的客户名单信息
        //mark绑定团体任务分组下的客户名单信息

        var ID_Team = jQuery('#idSelectTeam').val(); //获取团体ID
        var ID_TeamTask = jQuery('#idSelectTeamTask').val(); //获取团体任务ID
        var dataList = GetTeamTaskGroupCustomerInfo(ID_Team, ID_TeamTask, AllTeamTaskGroupID).dataList;
        //这里获取团体任务分组客户名单信息
        //BindCustomerCustInfo(dataList);//xmhuang 2014-01-08 注释
    }
}

/// <summary>
/// 绑定团体任务分组信息
/// </summary>
function ChangeGroup(obj) {
    //art.dialog({ id: "showInfo", lock: true, fixed: true, opacity: 0.3, content: "正在加载数据，请稍候..." }).title('正在加载数据，请稍候...').time(1);
    //ResetTeamGroupChirldInfo();
    ShowOper();
    var AllTeamTaskGroupID = "TaskGroup";
    //如果当前选中，则移除其它选中项
    var checked = obj.checked;
    //checked = !checked;
    if (checked) {
        jQuery(obj).parent().parent().siblings().find("td input[type='checkbox']").removeAttr("checked");
        AllTeamTaskGroupID = jQuery(obj).parent().parent().attr("id");
    }
    else {
        //获取选中的团体任务分组ID
        jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
            if (jQuery(this).find("td input[type='checkbox']").attr("checked")) {
                AllTeamTaskGroupID = jQuery(this).attr("id");
            }
        });
    }
    if (AllTeamTaskGroupID == "TaskGroup") {
        jQuery('#tblTeamTaskGroupFee tbody tr[id!="loading"]').remove();
        jQuery('#tblTeamTaskGroupFee tbody tr[id="loading"]').show();
        ResetSumJG();
    }
    else {
        //var myDialog = art.dialog({ id: "showInfo", lock: true, fixed: true, opacity: 0.3, content: "正在加载数据，请稍候..." }).show();


        //绑定团体任务分组的收费项目信息
        BindTeamTaskGroupFeeData_Ajax("tblTeamTaskGroupFee", AllTeamTaskGroupID);
        //mark绑定团体任务分组下的客户名单信息
        var ID_Team = jQuery('#idSelectTeam').val(); //获取团体ID
        var ID_TeamTask = jQuery('#idSelectTeamTask').val(); //获取团体任务ID
        //var dataList = GetTeamTaskGroupCustomerInfo(ID_Team, ID_TeamTask, AllTeamTaskGroupID).dataList;
        DoSearch(); //xmhuang 2014-01-09 分页绑定客户名单
        //这里获取团体任务分组客户名单信息
        //BindCustomerCustInfo(dataList);//xmhuang 2014-01-08 注释
    }
}

/*****获取任务名单信息 xmhuang 2014-03-28*****/

/// <summary>
/// 保存团体任务分组
///TemplateTeamTaskGroupID:模版ID
///ShowDataElementID:需要加载模版内容的目标ID
/// </summary>
function SaveTeamTaskGroup(ShowDataElementID) {
    //判断用户是否选择了团体和团体任务
    var ID_Team = jQuery('#idSelectTeam').val();
    var TeamName = jQuery('#nameSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    var TeamTaskName = jQuery('#nameSelectTeamTask').val();
    if (TeamName == "") {
        ShowSystemDialog("对不起，团体名称不允许为空，请您填写！");
        jQuery("#txtTeamNameX").focus();
        jQuery("#txtTeamNameX").select();
        return false;
    }
    if (ID_Team == "" || ID_Team == undefined) {
        ShowSystemDialog("对不起，团体名称在系统中不存在，请您重新填写！");
        jQuery("#txtTeamNameX").focus();
        jQuery("#txtTeamNameX").select();
        return false;
    }
    if (TeamTaskName == "") {
        ShowSystemDialog("对不起，团体任务名称不允许为空，请您填写！");
        jQuery("#txtTeamTaskNameX").focus();
        jQuery("#txtTeamTaskNameX").select();
        return false;
    }
    if (ID_TeamTask == "" || ID_TeamTask == undefined) {
        ShowSystemDialog("对不起，团体任务名称在系统中不存在，请您重新填写！");
        jQuery("#txtTeamTaskNameX").focus();
        jQuery("#txtTeamTaskNameX").select();
        return false;
    }
    if (jQuery("#" + ShowDataElementID + " tbody tr[id!='loading']").length == 0) {
        ShowSystemDialog("对不起，任务分组不存在，请您填写！");
        return false;
    }
    if (jQuery("#" + ShowDataElementID + " tbody tr[id!='loading'][exist='0']").length == 0) {
        ShowSystemDialog("对不起，没有需要保存的任务分组！");
        return false;
    }
    //遍历数据项table中的所有tr元素
    var IsPause = 0, exist = "", ID_TeamTaskGroup = "", BusFeeItems = "", IsCanSave = true, TeamTaskGroupName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", ExamType = "", ExamTypeName = "", BusSet = "", BusPEPackageName = "", FeeWay = "", FeeWayName = "", RoleName = "", SecurityLevel = "", SecurityLevelName = "";
    jQuery("#" + ShowDataElementID + " tbody tr[id!='loading'][exist='0']").each(function () {
        ID_TeamTaskGroup = jQuery.trim(jQuery(this).attr("id"));
        exist = jQuery.trim(jQuery(this).attr("exist")); //新增还是编辑
        TeamTaskGroupName = jQuery.trim(jQuery(this).find("td input[name='txtTeamTaskGroupName']").val());
        if (TeamTaskGroupName == "") {
            IsCanSave = false;
            ShowSystemDialog("对不起，任务分组名称不允许为空，请您填写！");
            jQuery(this).find("td input[name='txtTeamTaskGroupName']").focus();
            jQuery(this).find("td input[name='txtTeamTaskGroupName']").select();
            return false;
        }
        Gender = jQuery.trim(jQuery(this).find("td select[name='slSex']").find("option:selected").val());
        GenderName = jQuery.trim(jQuery(this).find("td select[name='slSex']").find("option:selected").text());
        Married = jQuery.trim(jQuery(this).find("td select[name='slMarried']").find("option:selected").val());
        MarriageName = jQuery.trim(jQuery(this).find("td select[name='slMarried']").find("option:selected").text());
        MinAge = jQuery.trim(jQuery.trim(jQuery(this).find("td input[name='txtMinAgeValue']").val()));
        MaxAge = jQuery.trim(jQuery.trim(jQuery(this).find("td input[name='txtMaxAgeValue']").val()));
        RoleName = jQuery.trim(jQuery.trim(jQuery(this).find("td input[name='txtRoleName']").val())); //角色名称
        ExamType = jQuery.trim(jQuery(this).find("td select[name='slExamType']").find("option:selected").val());
        ExamTypeName = jQuery.trim(jQuery(this).find("td select[name='slExamType']").find("option:selected").text());
        BusSet = jQuery.trim(jQuery(this).find("td select[name='slBusSet']").find("option:selected").val());
        BusPEPackageName = jQuery.trim(jQuery(this).find("td select[name='slBusSet']").find("option:selected").text());

        if (jQuery(this).find("td input[name='chcIsPause']").attr("checked") == true || jQuery(this).find("td input[name='chcIsPause']").attr("checked") == "checked") {
            IsPause = 1;
        }
        //经和CPQiu讨论套餐为非必选 20130627
        //        if (BusSet == "-1" || BusPEPackageName == "----") {
        //            IsCanSave = false;
        //            ShowSystemDialog("对不起,分组名称为[" + TeamTaskGroupName + "]的套餐类型不允许为空，请您选择！");
        //            jQuery(this).find("td select[name='slBusSet']").focus();
        //            jQuery(this).find("td select[name='slBusSet']").select();
        //            return false;
        //        }
        FeeWay = jQuery.trim(jQuery(this).find("td select[name='slFeeWay']").find("option:selected").val());
        FeeWayName = jQuery.trim(jQuery(this).find("td select[name='slFeeWay']").find("option:selected").text());

        SecurityLevel = jQuery.trim(jQuery(this).find("td select[name='SecurityLevel']").find("option:selected").val());
        SecurityLevelName = jQuery.trim(jQuery(this).find("td select[name='SecurityLevel']").find("option:selected").text());

        //判断是否有相同分组
        //不考虑婚姻
        if (Married == "-1" && Gender != "-1") {
            //不考虑年龄限制
            if (MinAge == "" && MaxAge == "") {
                //不考虑角色
                if (RoleName == "") {
                    //这里就只考虑性别
                    if (jQuery(this).siblings().find("td select[name='slSex'][value='" + Gender + "']").length > 0) {
                        IsCanSave = false;
                        ShowSystemDialog("对不起,分组名称为[" + TeamTaskGroupName + "]的分组和其它分组重复，请您修改！");
                        return false;
                    }
                }
                //考虑角色
                else {
                    //这里考虑性别、角色
                    if (jQuery(this).siblings().find("td select[name='slSex'][value='" + Gender + "']").length > 0) {
                        IsCanSave = false;
                        ShowSystemDialog("对不起,分组名称为[" + TeamTaskGroupName + "]的分组和其它分组重复，请您修改！");
                        return false;
                    }
                    if (jQuery(this).siblings().find("td input[name='txtRoleName'][value='" + RoleName + "']").length > 0) {
                        IsCanSave = false;
                        ShowSystemDialog("对不起,分组名称为[" + TeamTaskGroupName + "]的分组和其它分组重复，请您修改！");
                        return false;
                    }
                }

            }
            //如果下限和上限还有角色相同，那么认为分组重复
        }
        //不考虑性别
        if (Gender == "-1" && Married != "-1") {

        }
        //不考虑婚姻、性别
        if (Gender == "-1" && Married == "-1") {

        }
        /*********以下问题已经在绑定下拉选项时设置，这里毋须过滤 Begin**********/
        //        //这里为了跟设计对应上需要判断：0:女士适用 1：男士适用，2:男女均适用；婚姻状况,有0:未婚、1:已婚 2:视同已婚
        //        if (GenderName == "男") {
        //            Gender = 1;
        //        }
        //        else if (GenderName == "女") {
        //            Gender = 0;
        //        }
        //        else {
        //            Gender = 2;
        //        }
        //        if (MarriageName == "未婚") {
        //            Gender = Married;
        //        }
        //        else if (GenderName == "已婚") {
        //            Gender = 1;
        //        }
        //        else if (GenderName == "视为已婚") {
        //            Gender = 2;
        //        }
        /*********以下问题已经在绑定下拉选项时设置，这里毋须过滤 End**********/
        var curItem = TeamTaskGroupName + "_" + Gender + "_" + GenderName + "_" + Married + "_" + MarriageName + "_" + MinAge + "_" + MaxAge
         + "_" + ExamType + "_" + ExamTypeName + "_" + BusSet + "_" + BusPEPackageName + "_" + FeeWay + "_" + FeeWayName + "_" + RoleName + "_" + ID_TeamTaskGroup + "_" + SecurityLevel + "_" + IsPause + "_" + exist;
        BusFeeItems += curItem + "|";
    });

    if (IsCanSave) {

        var msgContent = "团体任务分组保存后将不能再进行修改，您确认要保存吗？";
        var dialog = art.dialog({
            id: 'artDialogIDRegisterDate',
            lock: true,
            fixed: true,
            opacity: 0.3,
            title: '温馨提示',
            content: msgContent,
            button: [{
                name: '取消',
                callback: function () {
                    return true;
                }
            }, {
                name: '确定',
                callback: function () {
                    IsCanSave = false;
                    //这里ajax提交数据
                    var qustData = { action: 'SaveTemaTaskGroup',
                        modelName: modelName,
                        type: type,
                        ID_Team: ID_Team,
                        TeamName: TeamName,
                        ID_TeamTask: ID_TeamTask,
                        TeamTaskName: TeamTaskName,
                        TeamTaskGroupItems: BusFeeItems
                    };
                    //存储大数据请设置Content-length值
                    jQuery.ajax({
                        type: "POST",
                        url: "/Ajax/AjaxTeamOper.aspx",
                        data: qustData,
                        cache: false,
                        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
                        dataType: "json",
                        success: function (msg) {
                            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
                            msg = CheckAjaxReturnDataInfo(msg);
                            if (msg == null || msg == "") {
                                return;
                            }
                            ShowSystemDialog(msg.Message);
                            if (msg.success == "1") {
                                GetTeamTaskGroupData_Ajax(ShowDataElementID, 0);
                            }
                            else {
                            }
                        },
                        complete: function () {
                        }
                    });
                    return true;

                }, focus: true
            }]

        }).lock();
    }
}


/// <summary>
/// 删除团体任务
/// </summary>
function DeleteTeamTaskGroup(ShowDataElementID) {
    //判断无数据时不允许删除
    if (jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']:checked").length == 0) {
        return false;
    }
    var CustTeamTaskID = "";
    //获取需要删除的团体任务分组ID
    jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
        if (jQuery(this).attr('checked')) {
            if (jQuery(this).parent().parent().attr('exist') == "1") {
                CustTeamTaskID += "'" + jQuery(this).parent().parent().attr('id') + "',";
            }
        }
    });

    //    var ID_Team = jQuery('#txtTeamNameX').data("ID_Team");

    var curAllISDeleteDT = { "dataList": "" };
    if (CustTeamTaskID != "") {
        curAllISDeleteDT = ISCanDeleteTeamTaskGroupInfo(CustTeamTaskID);
    }

    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/
    if (curAllISDeleteDT.success != undefined) {
        if (curAllISDeleteDT.success == -1) {
            ShowSystemDialog(curAllISDeleteDT.Message);
            return false;
        }
    }
    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/
    var CustTeamTaskID = '';
    var IsExist = false;
    var ErrorMessage = "";
    var CurID = "", TeamTaskName = "";
    //获取团体任务信息
    jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
        if (jQuery(this).attr('checked')) {

            //判断当前团体任务是否可用用于删除
            IsExist = false;
            CurID = jQuery(this).parent().parent().attr('id');
            //判断是新增行还是已经保存行
            if (jQuery(this).parent().parent().attr('exist') == 1) {
                TeamTaskName = jQuery.trim(jQuery(this).parent().parent().find("td label[name='txtTeamTaskGroupName']").text());
            }
            else {
                TeamTaskName = jQuery(this).parent().parent().find("td input[name='txtTeamTaskGroupName']").val();
            }

            jQuery(curAllISDeleteDT.dataList).each(function (i, item) {
                if (CurID == item.ID_TeamTaskGroup) {
                    ErrorMessage += TeamTaskName + ",";
                    IsExist = true;
                    return false;
                }
            });
            if (!IsExist) {
                if (jQuery(this).parent().parent().attr('id') != undefined && jQuery(this).parent().parent().attr('id_teamtaskgroup') != undefined) {
                    CustTeamTaskID += "'" + jQuery(this).parent().parent().attr('id') + "',";
                }
            }
        }
    });
    if (ErrorMessage != "") {
        ErrorMessage = "团体任务分组[" + ErrorMessage.substring(0, ErrorMessage.length - 1) + "]下已存在成员名单，不可删除,请重新选择！";
        ShowSystemDialog(ErrorMessage);
        return false;
    }
    //验证该团体是否可删除
    var msgContent = "删除团体任务分组数据将无法修复，您确认要删除团体任务分组数据吗？";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                return true;
            }
        }, {
            name: '确定',
            callback: function () {
                //判断是否有选中项目
                var CustTeamTaskID = '';
                jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
                    if (jQuery(this).attr('checked')) {
                        if (jQuery(this).parent().parent().attr('exist') == "1") {
                            CustTeamTaskID += "'" + jQuery(this).parent().parent().attr('id') + "',";
                        }
                        jQuery(this).parent().parent().remove();
                    }
                });
                if (CustTeamTaskID != "") {
                    var qustData = { action: 'DeleteTeamTaskGroup',
                        modelName: modelName,
                        type: type,
                        CustTeamTaskGroupID: CustTeamTaskID
                    }
                    jQuery.ajax({
                        type: "POST",
                        url: "/Ajax/AjaxTeamOper.aspx",
                        data: qustData,
                        cache: false,
                        dataType: "json",
                        success: function (msg) {
                            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
                            msg = CheckAjaxReturnDataInfo(msg);
                            if (msg == null || msg == "") {
                                return;
                            }
                            ShowSystemDialog(msg.Message);
                            //GetTeamTaskGroupData_Ajax(ShowDataElementID, 0);
                            //jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist!='0']").remove();
                            // BindAllTeamAndTaskAndGroupAndCustInfo_Ajax();
                            //重新设置汇总信息
                            ResetInfo();
                            GetTeamTaskGroupData_Ajax(ShowDataElementID, 0);
                        },
                        complete: function () {

                        }
                    });
                }
                return true;

            }, focus: true
        }]

    }).lock();

}
/// <summary>
/// 验证团体任务分组是否存在
/// </summary>
function CheckTeamTaskGroup(curObj) {
    //查找父元素中同辈元素中的所有和当前元素名称相同的团体任务分组
    var curTeamTaskGroupName = jQuery.trim(jQuery(curObj).val());
    var oldTeamTaskGroupName = jQuery(curObj).parent().attr("oldTeamTaskGroupName");
    //如果新值和原始值不等，即元素值发生改变才判断
    if (curTeamTaskGroupName != oldTeamTaskGroupName) {
        jQuery(curObj).parent().attr("oldTeamTaskGroupName", curTeamTaskGroupName)
    }
    if (curTeamTaskGroupName != "") {
        var siblingsCount = jQuery(curObj).parent().parent().siblings().find("td[oldTeamTaskGroupName='" + curTeamTaskGroupName + "']").length;
        if (siblingsCount >= 1) {
            if (jQuery(curObj).parent().parent().attr("exist") == "1") {
                jQuery(curObj).val(oldTeamTaskGroupName);
            }
            else {
                //jQuery(curObj).val("");
            }
            ShowSystemDialog("已存在[" + curTeamTaskGroupName + "]的任务，请您修改");
            jQuery(curObj).focus();
            jQuery(curObj).select();
            // return false;
        }
    }
}

/****************************套餐外项目新增 Begin*********************************/
/// <summary>
/// Ajax重新绑定套餐内容：必须保存分组信息、必须勾选一项且只为一项的分组做为新增收费项目的所属项目
///ID_TeamGroup:分组ID
///BusSet:选择的套餐ID
///Gender:适用性别对象
/// </summary>
function DoAddBusFee_Ajax(ShowDataElementID, ID_TeamGroup, TeamGroupName, FeeWayName, BusSet, Gender, GenderName, Action, InPutCode) {
    jQuery("#txtSearch").val(''); //设置关键字为空
    var HasData = true;
    jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:blue;text-align:center;">正在加载，请稍候...</td></tr>');
    var CustFeeID = ''; //获取当前所有套餐ID
    //查找非退费项目的收费项目ID
    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").each(function (i, item) {
        CustFeeID += "'" + jQuery(this).attr("id_fee") + "',";
    });
    var newContent = ''; //用于存放html
    //ajax请求：由于字符在get请求中超长，这里必须使用post方式提交请求
    jQuery.ajax({
        type: "POST",
        cache: false,
        url: "/Ajax/AjaxRegiste.aspx",
        data: { Gender: Gender, CustFeeID: CustFeeID, action: 'GetBusFeeNotINCustFeeID' },
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            jQuery("#showBusFeeItem").data('ExternList', msg); //缓存数据项到divExternList
            jQuery("#showBusFeeItem").empty(''); //清除显示项目
            if (msg.dataList.length > 0) {
                //由于在点击新增的时候已经过滤掉存在的CustFeeID，这里毋须再次过滤
                jQuery(msg.dataList).each(function (i, item) {
                    //过滤相同项
                    newContent += '<tr name="trExternItem" ID_TeamGroup="' + ID_TeamGroup + '" TeamGroupName="' + TeamGroupName + '" FeeWayName="' + FeeWayName + '" CustFeeChargeState="-1" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="' + DisCountRate + '" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">';
                    newContent += '<td><input onkeydown="keyMove(this, event)" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox" parentid="1909"></td>';
                    newContent += '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;" value="' + item.FeeName + '"  id="xmmc_' + item.ID_Fee + '"></td>' +
                                        '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;" value="' + item.InputCode + '"  id="inputCode_' + item.ID_Fee + '"></td>' +
                                    '</tr>';
                });
            }
            else {
                jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:red;text-align:center;">没有找到适合的收费项目！</td></tr>');
                jQuery("#showBusFee").show();
            }
        },
        complete: function () {
            if (newContent == "") {
                jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:red;text-align:center;">没有找到适合的收费项目！</td></tr>');
                jQuery("#showBusFee").show();
            }
            else {
                jQuery("#showBusFeeItem").append(newContent);
            }
            jQuery("#showBusFee").show(); DoScrollToBottom();
            newContent = "";
            jQuery("#txtSearch").focus();
        }
    });
}
/***********关键字搜索
备注：将搜索到的内容放到最前面显示 由模糊匹配改成精确匹配
******************/
function ResetSearch() {
    document.getElementById("txtSearch").focus();
    document.getElementById("txtSearch").select();
    document.getElementById("txtSearch").value = " ";
}
var SelectedCustFee = "", Tips = ""; //提示信息;

function BindCutomerBusFee(msg, ID_TeamGroup, TeamGroupName, FeeWayName, Gender, GenderName) {
    var newContent = "";
    jQuery("#showBusFeeItem").data('ExternList', msg); //缓存数据项到divExternList
    jQuery("#showBusFeeItem").empty(''); //清除显示项目
    var allItem = ",";
    if (msg.dataList.length > 0) {
        //由于在点击新增的时候已经过滤掉存在的CustFeeID，这里毋须再次过滤
        jQuery(msg.dataList).each(function (i, item) {
            if (allItem.search("," + item.ID_Fee + ",") == -1) {
                allItem += item.ID_Fee + ",";
                if (item.IsChecked == 1) {
                    newContent += '<tr name="trExternItem" class="externSelect" ID_TeamGroup="' + ID_TeamGroup + '" TeamGroupName="' + TeamGroupName + '" FeeWayName="' + FeeWayName + '" CustFeeChargeState="0" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="' + DisCountRate + '" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">';
                    newContent += '<td><input onkeydown="keyMove(this, event)" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox" checked="checked"></td>';
                }
                else if (item.IsChecked == 2)//以InputCode开头的
                {
                    newContent += '<tr name="trExternItem" class="externCanFocus" ID_TeamGroup="' + ID_TeamGroup + '" TeamGroupName="' + TeamGroupName + '" FeeWayName="' + FeeWayName + '" CustFeeChargeState="0" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="' + DisCountRate + '" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">';
                    newContent += '<td><input onkeydown="keyMove(this, event)" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox"></td>';
                }
                else {
                    newContent += '<tr name="trExternItem" class="trExternItem" ID_TeamGroup="' + ID_TeamGroup + '" TeamGroupName="' + TeamGroupName + '" FeeWayName="' + FeeWayName + '" CustFeeChargeState="0" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="' + DisCountRate + '" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">';
                    newContent += '<td><input onkeydown="keyMove(this, event)" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox"></td>';
                }
                newContent += '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;" value="' + item.FeeName + '"  id="xmmc_' + item.ID_Fee + '"></td>';
                newContent += '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;" value="' + item.InputCode + '"  id="inputCode_' + item.ID_Fee + '"></td>';
                newContent += '</tr>';
            }
        });
    }
    else {
        jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:red;text-align:center;">没有找到适合的收费项目！</td></tr>');
        jQuery("#showBusFee").show();
    }
    if (newContent == "") {
        jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:red;text-align:center;">没有找到适合的收费项目！</td></tr>');
        jQuery("#showBusFee").show();
    }
    else {
        jQuery("#showBusFeeItem").html(newContent);
        //        if (jQuery(".externCanFocus").first().length > 0) {
        //            jQuery(".externCanFocus td input[name='textExternItem']").first().focus(); //设置以InputCode开始的项光标
        //            jQuery(".externCanFocus td input[name='textExternItem']").first().select(); //设置以InputCode开始的项为选中项
        //        }
        newContent = "";
        BindSelect();
        SetTableRowStyle();
    }
}
function SureAddCurrentRow(obj) {
    var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("tblTemplateTeamTaskGroupFee");
    var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
    var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
    var newContent = '', ID_TeamGroup = "", TeamGroupName = "", FeeWayName = "", checked = false, ID_Fee = '', userName = '', date = '', FeeName = '', Price = 0, Discount = jQuery.trim(jQuery("#txtXMZK").val()), FactPrice = 0, FeeType = document.getElementById("slFeeWay").value, ID_Section = '', SectionName = '';
    ID_TeamGroup = jQuery(obj).parent().parent().attr("ID_TeamGroup");
    TeamGroupName = jQuery(obj).parent().parent().attr("TeamGroupName");
    //FeeWayName = jQuery(obj).parent().parent().attr("FeeWayName");
    var FeeWay = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].value; //付费方式 xmhuang 2014-04-13
    FeeWayName = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].text; //付费方式 xmhuang 2014-04-13

    ID_Fee = jQuery(obj).parent().parent().attr("id");
    userName = jQuery(obj).parent().parent().attr("userName");
    date = jQuery(obj).parent().parent().attr("date");
    FeeName = jQuery(obj).parent().parent().attr("FeeName");
    Price = jQuery(obj).parent().parent().attr("Price");
    Discount = (Discount == "" ? 10 : Discount);
    //Discount = jQuery(this).parent().parent().attr("Discount");
    FactPrice = jQuery(obj).parent().parent().attr("FactPrice");
    CustFeeChargeState = jQuery(obj).parent().parent().attr("CustFeeChargeState");
    ID_Section = jQuery(obj).parent().parent().attr("ID_Section");
    SectionName = jQuery(obj).parent().parent().attr("SectionName");
    newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@class/gi, "NewXM")
                            .replace(/@exist/gi, "0")
                            .replace(/@ID_TeamTaskGroup/gi, ID_TeamGroup)
                            .replace(/@TeamTaskGroupName/gi, TeamGroupName)
                            .replace(/@FeeWayName/gi, FeeWayName)
                            .replace(/@FeeWay/gi, FeeWay)
                            .replace(/@ID_Fee/gi, ID_Fee)
                            .replace(/@FeeName/gi, FeeName)
                            .replace(/@Price/gi, Price)
                            .replace(/@Discount/gi, Discount == 0 ? 10 : Discount)
                            .replace(/@FactPrice/gi, FactPrice)
                            .replace(/@userName/gi, userName)
                            .replace(/@CustFeeChargeState/gi, CustFeeChargeState)
                            .replace(/@ID_Section/gi, ID_Section)
                            .replace(/@SectionName/gi, SectionName)
    //                             .replace(/@PEPackageID/gi, PEPackageID)
                            .replace(/@date/gi, date);
    jQuery(obj).parent().parent().remove();
    if (newContent != '') {
        jQuery("#tblTeamTaskGroupFee tbody").append(newContent);
        newContent = "";
        //DoScrollToBottom();
        SumJG(); //计算总计
        ScollToTbCustFeeBottom();
        SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-18 
    }
    ResetSearch();
}
/// <summary>
/// 确认新增团体任务分组收费项目
///TemplateTeamTaskGroupID:模版ID
///ShowDataElementID:需要加载模版内容的目标ID
/// </summary>
function SureAddTeamTaskGroupFee(TemplateTeamTaskGroupFeeID, ShowDataElementID) {
    //清除选中项目
    SelectedCustFee = "", Tips = "";
    jQuery("#" + ShowDataElementID + " tbody tr[id='loading']").hide();
    if (TemplateTeamTaskGroupFeeID == "" || TemplateTeamTaskGroupFeeID == undefined) {
        TemplateTeamTaskGroupFeeID = "tblTemplateTeamTaskGroupFee";
    }
    if (ShowDataElementID == "" || ShowDataElementID == undefined) {
        ShowDataElementID = "tblTeamTaskGroupFee";
    }
    var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup(TemplateTeamTaskGroupFeeID);
    var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
    var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
    var newContent = '', ID_TeamGroup = "", TeamGroupName = "", FeeWayName = "", checked = false, ID_Fee = '', userName = '', date = '', FeeName = '', Price = 0, Discount = jQuery.trim(jQuery("#txtXMZK").val()), FactPrice = 0, FeeType = document.getElementById("slFeeWay").value, ID_Section = '', SectionName = '';
    var FeeWayName = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].text; //付费方式 xmhuang 2014-04-13
    var FeeWay = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].value; //付费方式 xmhuang 2014-04-13
    jQuery("#showBusFeeItem tr td input[name='ItemCheckboxX']").each(function () {
        checked = jQuery(this).attr("checked");
        if (checked) {
            //PEPackageID = jQuery(this).parent().parent().attr("PEPackageID");
            ID_TeamGroup = jQuery(this).parent().parent().attr("ID_TeamGroup");
            TeamGroupName = jQuery(this).parent().parent().attr("TeamGroupName");
            FeeWayName = FeeWayName; //jQuery(this).parent().parent().attr("FeeWayName");
            ID_Fee = jQuery(this).parent().parent().attr("id");
            userName = jQuery(this).parent().parent().attr("userName");
            date = jQuery(this).parent().parent().attr("date");
            FeeName = jQuery(this).parent().parent().attr("FeeName");
            Price = jQuery(this).parent().parent().attr("Price");
            Discount = (Discount == "" ? 10 : Discount);
            //Discount = jQuery(this).parent().parent().attr("Discount");
            FactPrice = jQuery(this).parent().parent().attr("FactPrice");
            CustFeeChargeState = jQuery(this).parent().parent().attr("CustFeeChargeState");
            ID_Section = jQuery(this).parent().parent().attr("ID_Section");
            SectionName = jQuery(this).parent().parent().attr("SectionName");
            newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@class/gi, "NewXM")
                            .replace(/@exist/gi, "0")
                            .replace(/@ID_TeamTaskGroup/gi, ID_TeamGroup)
                            .replace(/@TeamTaskGroupName/gi, TeamGroupName)
                            .replace(/@FeeWayName/gi, FeeWayName)
                            .replace(/@FeeWay/gi, FeeWay)
                            .replace(/@ID_Fee/gi, ID_Fee)
                            .replace(/@FeeName/gi, FeeName)
                            .replace(/@Price/gi, Price)
                            .replace(/@Discount/gi, Discount == 0 ? 10 : Discount)
                            .replace(/@FactPrice/gi, FactPrice)
                            .replace(/@userName/gi, userName)
                            .replace(/@CustFeeChargeState/gi, CustFeeChargeState)
                            .replace(/@ID_Section/gi, ID_Section)
                            .replace(/@SectionName/gi, SectionName)
            //                             .replace(/@PEPackageID/gi, PEPackageID)
                            .replace(/@date/gi, date);
            jQuery(this).parent().parent().remove();
            Tips += FeeName + ",";

        }
    });
    if (newContent != '') {
        IsSaved = false;
        jQuery("#" + ShowDataElementID + " tbody").append(newContent);

        newContent = "";
        SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-22       
    }
    //DoClose();
    SumJG(); //计算总计
    ResetSearch();
    ScollToTbCustFeeBottom();
    //显示提示信息
    if (Tips.length > 0) {
        art.dialog.tips("成功追加收费项目：" + Tips.substring(0, Tips.length - 1), 2);
    }
    DoClose();
}
//绑定搜索项获取光标即选中
function BindSelect() {
    jQuery("#showBusFeeItem input[type='text']").focus(function () {
        this.select();
    });
}

function DoClose() {
    jQuery("#showBusFeeItem").empty();
    jQuery("#showBusFee").hide();
}
function DoSelectAll() {
    jQuery("#txtSearch").select();
}
/**********全选团体任务分组收费项目***********/
function checkAllChildren(obj) {
    jQuery("[name='ItemCheckboxX']").each(function () {
        jQuery(this).attr('checked', obj.checked);
    })

}
/*************键盘操作事件：主要为上下左右键，适用于table******************/
function keyMove(item, event) {
    var elementID = "tblTeamListTableEdit";
    var maxX = document.getElementById(elementID).rows[0].cells.length;   //计算表格有列数
    var maxY = document.getElementById(elementID).rows.length;            //计算表格行数
    var objTable = document.getElementById(elementID); 						//获取table
    var c = item.parentNode.cellIndex; 										//获取当前列的下标，因为列中有文本框，取父级
    var row = item.parentNode; 											    //获取当前行的下标
    while (row.tagName != "TR") row = row.parentNode;
    var r = row.rowIndex; var x = r; var y = c;
    if (event.keyCode == 40) {
        if (r < maxY - 1) {
            x = r + 1;
            y = c;
        }
    }
    if (event.keyCode == 38) {
        if (r > 0) {
            x = r - 1;
            y = c;
        }
    }
    if (event.keyCode == 39) {
        if (c <= maxX - 2) {
            x = r;
            y = c + 1;
        }
    }
    if (event.keyCode == 37) {
        if (c > 0) {
            x = r;
            y = c - 1;
        }
    }
    if (objTable.rows[x].style.display == "none")
        return;
    if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 13) {
        if (objTable.rows[x].cells[y] != undefined) {
            if (objTable.rows[x].cells[y].children[0] != undefined) {
                //回车默认选中当前行
                if (event.keyCode == 13) //如果按键为回车，则设置当前行复选框选中
                {
                    //                    objTable.rows[x].cells[0].children[0].checked = !objTable.rows[x].cells[0].children[0].checked; //设置当前复选框为选中状态
                    //                    if (objTable.rows[x].cells[0].children[0].name == "checkAllX")//如果是全选按钮，则设置所有子行为选中状态
                    //                    {
                    //                        checkAllChildren(objTable.rows[x].cells[0].children[0]); //设置复选框选中
                    //                        SureAddCurrentRow(objTable.rows[x].cells[y].children[0]);
                    //                    }
                    //xmhuang 2013-10-16 调整新增项目采用个人登记方式
                    if (objTable.rows[x].cells[y].children[0].name != "checkAllX") {
                        SureAddCurrentRow(objTable.rows[x].cells[y].children[0]);
                    }
                    document.getElementById("txtSearch").focus(); //设置输入框焦点
                    document.getElementById("txtSearch").select(); //设置输入框内容为选中状态
                }
                else//如果按键为非回车，如上键、下键、左、右键，则设置当前行为选中状态
                {
                    if (objTable.rows[x].cells[y].children[0].type != "button") {
                        objTable.rows[x].cells[y].children[0].blur(); //设置当前行文本失去光标，此项为必须设置，否则无法兼容fiefox
                        objTable.rows[x].cells[y].children[0].focus(); //设置当前行文本元素光标
                        objTable.rows[x].cells[y].children[0].select(); //设置当前行文本内容为选中状态
                    }
                }
            }
        }
    }
}
//该方法只适用于团体维护功能
function keyMove_TeamOper(item, event) {

    var elementID = "tblTeamListTableEdit";
    var maxX = document.getElementById(elementID).rows[0].cells.length;   //计算表格有列数
    var maxY = document.getElementById(elementID).rows.length;            //计算表格行数
    var objTable = document.getElementById(elementID); 						//获取table
    var c = item.parentNode.cellIndex; 										//获取当前列的下标，因为列中有文本框，取父级
    var row = item.parentNode; 											    //获取当前行的下标
    while (row.tagName != "TR") row = row.parentNode;
    var r = row.rowIndex; var x = r; var y = c;
    if (event.keyCode == 40) {
        if (r < maxY - 1) {
            x = r + 1;
            y = c;
        }
    }
    if (event.keyCode == 38) {
        if (r > 0) {
            x = r - 1;
            y = c;
        }
    }
    if (event.keyCode == 39) {
        if (c <= maxX - 2) {
            x = r;
            y = c + 1;
        }
    }
    if (event.keyCode == 37) {
        if (c > 0) {
            x = r;
            y = c - 1;
        }
    }
    if (objTable.rows[x].style.display == "none")
        return;
    if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 13) {
        if (objTable.rows[x].cells[y] != undefined) {
            if (objTable.rows[x].cells[y].children[0] != undefined) {
                //回车默认选中当前行
                if (event.keyCode == 13) {
                    objTable.rows[x].cells[0].children[0].checked = !objTable.rows[x].cells[0].children[0].checked;
                    if (objTable.rows[x].cells[0].children[0].name == "checkAllX") {
                        checkAllChildren(objTable.rows[x].cells[0].children[0]);
                    }
                    document.getElementById("txtSearch").focus(); //设置检索焦点
                    document.getElementById("txtSearch").select(); //设置检索全选
                }
                else//如果按键为非回车，如上键、下键、左、右键，则设置当前行为选中状态
                {
                    if (objTable.rows[x].cells[y].children[0].type != "button") {
                        objTable.rows[x].cells[y].children[0].blur(); //设置当前行文本失去光标，此项为必须设置，否则无法兼容fiefox
                        objTable.rows[x].cells[y].children[0].focus(); //设置当前行文本元素光标
                        objTable.rows[x].cells[y].children[0].select(); //设置当前行文本内容为选中状态
                    }
                }
            }
        }
    }
    return false;
}
/****************************套餐外项目新增 End*********************************/


/****************************团体备单  End*******************************/


/****************************导入名单  Begin*****************************/


/// <summary>
/// 批量删除客户数据
/// </summary>
function DoDeleteCustomerInfo(ShowDataElementID) {
    //判断是否为空
    if (jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']:checked").length == 0) {
        return false;
    }
    //在没有打印指引单之前，客户备单允许删除
    //获取需要删除的客户体检号
    var CustTeamTaskID = "";
    jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
        if (jQuery(this).attr('checked')) {
            if (jQuery(this).parent().parent().attr('exist') == "1") {
                CustTeamTaskID += jQuery(this).parent().parent().attr('ID_Customer') + ",";
            }
        }
    });

    var curAllISDeleteDT = { "dataList": "" };
    //var ID_Team = jQuery('#txtTeamNameX').data("ID_Team");
    if (CustTeamTaskID != "") {
        curAllISDeleteDT = ISCanDeleteTeamTaskGroupCustomerInfo(CustTeamTaskID);
        //xmhuang 2013-12-01 从后台未检索到信息，不允许执行删除操作
        if (curAllISDeleteDT == "") {
            return false;
        }
    }

    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/
    if (curAllISDeleteDT.success != undefined) {
        if (curAllISDeleteDT.success == -1) {
            ShowSystemDialog(curAllISDeleteDT.Message);
            return false;
        }
    }
    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/

    var CustTeamTaskID = '';
    var IsExist = false;
    var ErrorMessage = "";
    var CurID = "", TeamTaskName = "";
    //获取团体任务信息
    jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
        if (jQuery(this).attr('checked')) {
            //判断当前团体任务是否可用用于删除
            IsExist = false;
            CurID = jQuery.trim(jQuery(this).parent().parent().find("td label[name='lblID_Customer']").text());
            TeamTaskName = jQuery.trim(jQuery(this).parent().parent().find("td input[name='txtCustomerName']").val());
            if (TeamTaskName == "") {
                TeamTaskName = jQuery.trim(jQuery(this).parent().parent().find("td label[name='txtCustomerName']").text());
            }
            jQuery(curAllISDeleteDT.dataList).each(function (i, item) {
                if (CurID == item.ID_Customer) {
                    ErrorMessage += TeamTaskName + ",";
                    IsExist = true;
                    return false;
                }
            });
            if (!IsExist) {
                if (jQuery(this).parent().parent().attr('exist') == "1") {
                    if (jQuery(this).parent().parent().attr('ID_Customer') != undefined) {
                        CustTeamTaskID += jQuery(this).parent().parent().attr('ID_Customer') + ",";
                    }
                }
            }
        }
    });
    if (ErrorMessage != "") {
        ErrorMessage = "客户[" + ErrorMessage.substring(0, ErrorMessage.length - 1) + "]已完成登记，不可删除！";
        ShowSystemDialog(ErrorMessage);
        return false;
    }

    var msgContent = "删除客户名单后，其收费项目信息、体检信息将无法修复，您确认要删除吗？";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                return true;
            }
        }, {
            name: '确定',
            callback: function () {
                //获取当前分组ID()和收费ID
                var ID_TeamTaskGroupCustomer = "";
                var AllItem = "";
                jQuery("#" + ShowDataElementID + " tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
                    if (jQuery(this).attr('checked')) {
                        if (jQuery(this).parent().parent().attr('exist') == "1") {
                            if (jQuery(this).parent().parent().attr('ID_Customer') != undefined) {

                                ID_TeamTaskGroupCustomer = jQuery(this).parent().parent().attr('ID_Customer');
                                AllItem += "'" + ID_TeamTaskGroupCustomer + "',";
                            }
                        }
                        jQuery(this).parent().parent().remove();
                    }
                });

                if (AllItem != "") {
                    //执行后台删除
                    DoDeleteCustomerInfo_Ajax(AllItem);
                }
                return true;

            }, focus: true
        }]

    }).lock();
}
/// <summary>
/// 执行后台删除收费数据
/// </summary>
function DoDeleteCustomerInfo_Ajax(AllItem) {
    //存储大数据请设置Content-length值
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "DoDeleteCustomerInfo", AllItem: AllItem },
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            //ShowSystemDialog(msg.Message);
            if (msg.success == "1") {
                DoSearch();
            }
        },
        complete: function () {

        }
    });
}
///重置序号
function ResetRowNum(elementID) {
    var RowNum = 1;
    jQuery("#" + elementID + " tbody tr td label[name='lblRowNum'").each(function () {
        jQuery(this).text(RowNum);
        RowNum++;
    });
}
//新增客户名单
function AddCustomerInfo(TemplateShowDataElementID, ShowDataElementID) {

    if (ShowDataElementID == undefined || ShowDataElementID == "") {
        ShowDataElementID = "tblTeamTaskGroupCustomerX";
    }
    //读取客户名单模版
    var xustomerTask = new ReadTeamTaskTemplate("tblTemplateTeamTaskGroupCustomer_Add", ShowDataElementID);
    var customerListTheadTempleteContent = xustomerTask.teamTaskListTheadTempleteContent;
    var customerListBodyTempleteContent = xustomerTask.teamTaskListBodyTempleteContent;
    var birthDate = new Date();
    var birthDay = birthDate.getFullYear() + "-" + birthDate.getMonth() + "-" + birthDate.getDay();
    var tempContent = "";

    tempContent = customerListBodyTempleteContent.replace(/@exist/gi, 2)
                            .replace(/@ID_TeamTaskGroup/gi, "")
                            .replace(/@TeamTaskGroupName/gi, "")
                            .replace(/@CustomerName/gi, "")
                            .replace(/@CustomerBirthDay/gi, "")
                            .replace(/@CustomerRoleName/gi, "")
                            .replace(/@IDCard/gi, "")
                            .replace(/@CustomerTel/gi, "")
                            .replace(/@DepartmentX/gi, "")
                            .replace(/@DepartmentA/gi, "")
                            .replace(/@DepartmentB/gi, "")
                            .replace(/@DepartmentC/gi, "")
                            .replace(/@ErorrMessage/gi, "--")
                            .replace(/@ID_Customer/gi, "");
    // jQuery("#" + ShowDataElementID + " thead").html(teamTaskGroupListTheadTempleteContent);
    jQuery("#" + ShowDataElementID + " tbody tr[id='loading']").hide();
    jQuery("#" + ShowDataElementID + " tbody").prepend(tempContent);
    jQuery("#" + ShowDataElementID + " tbody tr td input[name='txtCustomerName']").first().focus();

    jQuery("#" + ShowDataElementID + " tbody tr").first().find("td input[type='text']").each(function () {
        jQuery(this).val("");
    });

    //重置文本值
    //    jQuery("#" + ShowDataElementID + " tbody tr td input[type='text']").each(function () {
    //        jQuery(this).val("");
    //    });

    ResetRowNum("tblTeamTaskGroupCustomerX");
    SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-18 
}
/// <summary>
/// 绑定团体分组和分组名单
/// </summary>
function BindAllTeamAndTaskAndGroupAndCustInfo_Ajax() {
    //存储大数据请设置Content-length值
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    //获取所有分组ID
    var ID_TeamTaskGroupS = "";
    jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
        ID_TeamTaskGroupS += "'" + jQuery(this).attr("id") + "',";
    });
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "GetTeamTaskGroupCustInfo", ID_Team: ID_Team, ID_TeamTask: ID_TeamTask, ID_TeamTaskGroupS: ID_TeamTaskGroupS },
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            BindTeamTaskGroupData("tblTeamTaskGroupX", msg.dataList0); //这里已经绑定了收费项目
            //此项绑定已作废，后台只单项检索分组信息，绑定客户名单信息将在选择了分组后进行
            //BindCustomerCustInfo(msg.dataList1);
        },
        complete: function () {

        }
    });
}

function HideOper() {
    jQuery("#btnAdd").hide();
    jQuery("#btnDel").hide();
}
function ShowOper() {
    jQuery("#btnAdd").show();
    jQuery("#btnDel").show();
}
/// <summary>
/// 绑定客户名单
/// </summary>
function BindCustomerCustInfo(dataList) {
    var ShowDataElementID = "tblTeamTaskGroupCustomerX";
    //读取客户名单模版
    var xustomerTask = new ReadTeamTaskTemplate("tblTemplateTeamTaskGroupCustomer", ShowDataElementID);
    var customerListTheadTempleteContent = xustomerTask.teamTaskListTheadTempleteContent;
    var customerListBodyTempleteContent = jQuery("#tblTemplateTeamTaskGroupCustomer").html(); //  xustomerTask.teamTaskListBodyTempleteContent;


    var birthDate = new Date();
    var tempContent = "", htmlContent = "";
    var a = new Array(); //使用数组的join方法连接字符串效率更高
    var count = 0; var checkedme = "";
    jQuery(dataList).each(function (i, item) {
        checkedme = "";
        if (item.Is_Paused == 0 || item.Is_Paused == "") {
            item.Is_Paused = "";
        }
        else if (item.Is_Paused == 1) {
            checkedme = "checked=";
            item.Is_Paused = "checked";
        }
        a[count] = customerListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@exist/gi, item.exist)
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                            .replace(/@TeamTaskGroupName/gi, item.TeamTaskGroupName)
                            .replace(/@CustomerName/gi, item.CustomerName)
                            .replace(/@CustomerBirthDay/gi, item.CustomerBirthDay)
                            .replace(/@CustomerRoleName/gi, item.CustomerRoleName)
                            .replace(/@IDCard/gi, item.IDCard)
                            .replace(/@CustomerTel/gi, item.CustomerTel)//联系方式显示值只显示12位 xmhuang 20140419
        //.replace(/@TitleCustomerTel/gi, item.CustomerTel)//联系方式提示值 xmhuang 20140419
        //.replace(/@TitleDepartmentX/gi, item.DepartmentX)//部门鼠标提示值 xmhuang 20140419
                            .replace(/@DepartmentX/gi, item.DepartmentX)//部门显示值只显示12位 xmhuang 20140419
        //.replace(/@TitleDepartmentA/gi, item.DepartmentA)//一级部门显示值只显示12位 xmhuang 20140419
                            .replace(/@DepartmentA/gi, item.DepartmentA)//一级部门鼠标提示值 xmhuang 20140419
        //.replace(/@TitleDepartmentB/gi, item.DepartmentB)//二级部门鼠标提示值 xmhuang 20140419
                            .replace(/@DepartmentB/gi, item.DepartmentB)//二级部门鼠标提示值 xmhuang 20140419
        //.replace(/@TitleDepartmentC/gi, item.DepartmentC)
                            .replace(/@DepartmentC/gi, item.DepartmentC)//三级部门鼠标提示值 xmhuang 20140419
                            .replace(/@ErorrMessage/gi, "")
                            .replace(/@ID_Customer/gi, item.ID_Customer)
                            .replace(/@NationID/gi, item.NationID)
                            .replace(/@NationName/gi, item.NationName)
                            .replace(/@ID_Gender/gi, item.ID_Gender)
                            .replace(/@CustomerSex/gi, item.GenderName)
                            .replace(/@CustomerMarried/gi, item.MarriageName)
                            .replace(/@Is_Marriage/gi, item.ID_Marriage)
                            .replace(/@TitleNote/gi, item.Note)
                            .replace(/@Note/gi, item.Note)
                            .replace(/checkedme=/gi, checkedme)
                            .replace(/@Is_Paused/gi, item.Is_Paused);
        count++;
    });

    jQuery("#" + ShowDataElementID + " tbody tr[id='loading']").hide();
    jQuery("#" + ShowDataElementID + " tbody tr[id!='loading']").remove();
    jQuery("#" + ShowDataElementID + " tbody").prepend(a.join(""));
    jQuery('#' + ShowDataElementID + ' tbody tr[id!="loading"] input[type!="checkbox"]').css({ "border": "0px", "background-color": "transparent" });
    if (!IsHaveCustomerPauseRight()) {
        jQuery('#' + ShowDataElementID + ' tbody tr[id!="loading"] input[type="checkbox"][name="chcIsPause"]').attr("disabled", true);  //设置禁用权限 xmhuang 2014-04-16
    }
    if (dataList.length > 0) {
        HideOper();
    }
    else {
        ShowOper();
    }
    ResetRowNum("tblTeamTaskGroupCustomerX");
    SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-18 

}
/// <summary>
/// 保存客户名单
/// 修改人：xmhuang
/// 修改时间：2013-10-23
/// 修改内容：生成体检号时，验证所有分组是否都存在收费项目，不存在，则不允许生成
/// </summary>
function SaveCustomer() {
    art.dialog({ lock: true, fixed: true, opacity: 0.3,
        content: '正在进行数据分析，请稍候...',
        init: function () {
            var that = this;
            var fn = function () {
                if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").length == 0) {
                    ShowSystemDialog("对不起，团体任务分组信息不能为空，请先维护！");
                    that.close();
                    return false;
                }
                //    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").length == 0) {
                //        ShowSystemDialog("对不起，团体收费项目不能为空，请先维护！");
                //        return false;
                //    }
                //判断任务分组是否保存
                if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length > 0) {
                    ShowSystemDialog("对不起，团体任务分组信息尚未保存，请先保存！"); that.close();
                    return false;
                }
                //判断收费项目是否保存
                if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist='0']").length > 0) {
                    ShowSystemDialog("对不起，团体收费项目信息尚未保存，请先保存！"); that.close();
                    return false;
                }
                var IsCanSave = true;
                //重新分组
                var exist = "", NationName = "", CheckCustomerName = "", IDCard = "", CheckBirthDay = "", obj = "", CheckGenderName = "", CustomerTel = "", Base64Photo = "";
                jQuery("#tblTeamTaskGroupCustomerX tbody tr[id!='loading'][exist!='1']").each(function () {
                    exist = jQuery(this).attr("exist");
                    if (jQuery(this).find("td select[name='slCustomerSex']").find("option:selected").text().length > 0) {
                        CheckGenderName = jQuery.trim(jQuery(this).find("td select[name='slCustomerSex']").find("option:selected").text());
                        obj = jQuery(this).find("td input[name='txtIDCard']");
                        CheckBirthDay = jQuery.trim(jQuery(this).find("td input[name='txtCustomerBirthDay']").val());
                        CheckCustomerName = jQuery.trim(jQuery(this).find("td input[name='txtCustomerName']").val());
                        IDCard = jQuery.trim(jQuery(obj).val());
                        CustomerTel = jQuery.trim(jQuery(this).find("td input[name='txtCustomerTel']").val());
                    }
                    else {
                        CheckGenderName = jQuery.trim(jQuery(this).find("td label[name='slCustomerSex']").text());
                        obj = jQuery(this).find("td label[name='txtIDCard']");
                        CheckBirthDay = jQuery.trim(jQuery(this).find("td label[name='txtCustomerBirthDay']").text());
                        CheckCustomerName = jQuery.trim(jQuery(this).find("td label[name='txtCustomerName']").text());
                        IDCard = jQuery.trim(jQuery(obj).text());
                        CustomerTel = jQuery.trim(jQuery(this).find("td label[name='txtCustomerTel']").text());
                    }

                    //客户名称为空
                    if (CheckCustomerName == "") {
                        ShowSystemDialog("对不起，客户名称不允许为空！请您填写!");
                        //            if (exist == 0) {
                        jQuery(this).find("td input[name='txtCustomerName']").focus();
                        jQuery(this).find("td input[name='txtCustomerName']").select();
                        //            }
                        IsCanSave = false;
                        that.close();
                        return false;
                    }
                    //客户名称为空
                    if (IDCard == "") {
                        //            ShowSystemDialog("对不起，身份证号码不允许为空！请您填写!"); if (exist == 0) {
                        jQuery(this).find("td input[name='txtIDCard']").focus();
                        //            } 
                        IsCanSave = false;
                        that.close();
                        return false;
                    }
                    if (IDCard.length == 15 || IDCard.length == 18) {

                    }
                    else {
                        ShowSystemDialog("对不起，身份证号码不正确！请您填写!");
                        //             if (exist == 0) {
                        jQuery(this).find("td input[name='txtIDCard']").focus();
                        //            }
                        IsCanSave = false; that.close();
                        return false;
                    }
                    //NationName = jQuery.trim(jQuery(this).find("td select[name='slNation']").find("option:selected").text()); // jQuery.trim(jQuery(this).find("td input[name='txtNation']").val());

                    //        if (NationName == "") {
                    //            ShowSystemDialog("对不起，名族不允许为空！请您填写!");
                    //            jQuery(this).find("td input[name='txtNation']").focus(); IsCanSave = false;
                    //            return false;
                    //        }
                    //通过身份证判断出生年月和性别是否正确
                    //var customerInfo = new GetUserInfoByIDCard(obj);
                    //        if (customerInfo.birthday != CheckBirthDay) {
                    //            ShowSystemDialog("出生日期与身份证不符,请您确认身份证是否录入正确！");
                    //            jQuery(this).find("td input[name='txtIDCard']").focus();
                    //            jQuery(this).find("td input[name='txtIDCard']").select();
                    //            IsCanSave = false;
                    //            return false;
                    //        }
                    //        if (customerInfo.sexName != CheckGenderName) {
                    //            ShowSystemDialog("性别与身份证不符,请您确认身份证是否录入正确！");
                    //            jQuery(this).find("td input[name='txtIDCard']").focus();
                    //            jQuery(this).find("td input[name='txtIDCard']").select();
                    //            IsCanSave = false;
                    //            return false;
                    //        }
                    //重名
                    if (jQuery(this).siblings().find("td label[name='txtCustomerName'][text='" + CheckCustomerName + "']").length > 0) {
                        ShowSystemDialog("对不起，客户名称存在重名！请您重新填写!");
                        //             if (exist == 0) {
                        jQuery(this).find("td input[name='txtCustomerName']").focus();
                        //            }
                        IsCanSave = false; that.close();
                        return false;
                    }
                    //出生日期为空
                    if (CheckBirthDay == "") {
                        ShowSystemDialog("对不起，出生日期不允许为空！请您填写!");
                        //            if (exist == 0) {
                        jQuery(this).find("td input[name='txtCustomerBirthDay']").click();
                        //            }
                        IsCanSave = false; that.close();
                        return false;
                    }
                    //联系电话为空
                    if (CustomerTel == "") {
                        //            ShowSystemDialog("对不起，联系电话不允许为空！请您填写!");
                        //            jQuery(this).find("td input[name='txtCustomerTel']").focus();
                        //            IsCanSave = false;
                        //            return false;
                    }
                    else {
                        //联系电话不正确
                        //            if (!isMobil(CustomerTel)) {
                        //                ShowSystemDialog("对不起，联系电话输入不正确！请您重新填写!");
                        //                //                if (exist == 0) {
                        //                jQuery(this).find("td input[name='txtCustomerTel']").focus();
                        //                jQuery(this).find("td input[name='txtCustomerTel']").select();
                        //                //                }
                        //                IsCanSave = false;
                        //                return false;
                        //            }
                    }
                    //重新分组
                    //CheckCustomer(jQuery(this).find("td input[name='txtCustomerName']"));
                });
                if (!IsCanSave) {
                    return;
                }
                //判断是否有错误验证信息
                var ShowDataElementID = "tblTeamTaskGroupCustomerX";
                var AllItem = "";
                var CurItem = "";
                //jQuery("#" + ShowDataElementID + " tbody tr[id!='loading'][exist!='1'] td label[name='lblErorrMessage']").each(function () {
                //                    if (jQuery.trim(jQuery(this).text()) != "√") {
                //                        IsCanSave = false; that.close();
                //                        return;
                //                        //return false;
                //                    }
                //                });

                jQuery("#" + ShowDataElementID + " tbody tr td label[name='lblGroupName']").each(function () {
                    if (jQuery.trim(jQuery(this).text()) == "") {
                        IsCanSave = false;
                        return false;
                    }
                });

                if (jQuery("#" + ShowDataElementID + " tbody tr[class='ErrorMessageX'").length > 0) {
                    IsCanSave = false;
                }
                if (!IsCanSave) {
                    ShowSystemDialog("客户名单中存在错误验证信息或分组不正确，请您修改！");
                    IsCanSave = true;
                    that.close();
                    return false;
                }

                //xmhuang 2013-10-23 这里通过后台请求获取所有任务是否已经完成收费项目维护，如果都存在收费项目则可以进行体检号生成，如果其中任何一个没有维护收费项目信息，则必须维护后方可进行 Begin
                var AllTeamTaskGroupID = "";
                var AllErrorMsg = "";
                jQuery("#tblTeamTaskGroupX tbody tr[exist='1']").each(function () {
                    AllTeamTaskGroupID += jQuery(this).attr("id") + ",";
                });
                //判断是否可以保存
                if (AllTeamTaskGroupID != "") {
                    var AllCanotSaveCustomer = IsCanSaveCustomer(AllTeamTaskGroupID);
                    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/
                    if (AllCanotSaveCustomer.success != undefined) {
                        if (AllCanotSaveCustomer.success == -1) {
                            ShowSystemDialog(AllCanotSaveCustomer.Message); that.close();
                            return false;
                        }
                    }
                    /******如果出现异常则不允许执行下一步 xmhuang 2013-12-11*********/
                    if (AllCanotSaveCustomer.dataList.length > 0) {
                        jQuery(AllCanotSaveCustomer.dataList).each(function (i, item) {
                            AllErrorMsg += item.TeamTaskGroupName + ",";
                        });
                    }
                }
                if (AllErrorMsg != "") {
                    AllErrorMsg = "任务分组[" + AllErrorMsg.substring(0, AllErrorMsg.length - 1) + "]尚未维护收费项目，不允许生成体检号！";
                    ShowSystemDialog(AllErrorMsg); that.close();
                    return false;
                }
                //xmhuang 2013-10-23 这里通过后台请求获取所有任务是否已经完成收费项目维护，如果都存在收费项目则可以进行体检号生成，如果其中任何一个没有维护收费项目信息，则必须维护后方可进行 End

                var Is_Paused = 0, NationName = "", ID_Customer = "", ID_TeamTaskGroup = "", ID_TeamTask = "", CustomerName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", BirthDay = "", MobileNo = "", RoleName = "", Department = "", DepartSubA = "", DepartSubB = "", DepartSubC = "", Note = "";
                //由于已经生成体检号的项目不允许修改，这里只需要获取没有生成体检号的客户名单即可

                jQuery("#" + ShowDataElementID + " tbody tr[id!='loading'][exist!='1']").each(function () {
                    Base64Photo = "";
                    ID_Customer = jQuery(this).attr("ID_Customer");
                    exist = jQuery(this).attr("exist");
                    if (ID_Customer == "") {
                        ID_TeamTaskGroup = jQuery(this).attr("ID_TeamTaskGroup");
                        ID_TeamTask = jQuery(this).attr("ID_TeamTask");
                        if (exist == 0)///表示客户新增行数据，其使用的模版都为input类型
                        {
                            CustomerName = jQuery.trim(jQuery(this).find("td label[name='txtCustomerName']").text());
                            IDCard = jQuery.trim(jQuery(this).find("td label[name='txtIDCard']").text());
                            Gender = jQuery.trim(jQuery(this).find("td label[name='slCustomerSex']").attr("id_gender"));
                            GenderName = jQuery.trim(jQuery(this).find("td label[name='slCustomerSex']").text());
                            Married = jQuery.trim(jQuery(this).find("td label[name='slCustomerMarried']").attr("is_marriage"));
                            MarriageName = jQuery.trim(jQuery(this).find("td label[name='slCustomerMarried']").text());
                            BirthDay = jQuery.trim(jQuery(this).find("td label[name='txtCustomerBirthDay']").text());
                            MobileNo = jQuery.trim(jQuery(this).find("td label[name='txtCustomerTel']").text());
                            Department = jQuery.trim(jQuery(this).find("td label[name='txtDepartment']").text());
                            DepartSubA = jQuery.trim(jQuery(this).find("td label[name='txtDepartmentA']").text());
                            DepartSubB = jQuery.trim(jQuery(this).find("td label[name='txtDepartmentB']").text());
                            DepartSubC = jQuery.trim(jQuery(this).find("td label[name='txtDepartmentC']").text());
                            NationName = jQuery.trim(jQuery(this).find("td label[name='slNation']").text());
                            Note = jQuery.trim(jQuery(this).find("td label[name='txtNote']").text()); //获取备注信息 xmhuang 2013-10-26
                            Base64Photo = jQuery(this).find("td img[name='HeadImg']").attr("base64photo"); //xmhuang 2014-01-22
                        }

                        else if (exist == 2)//表示客户导入数据，其使用的模版都为Label类型
                        {
                            CustomerName = jQuery.trim(jQuery(this).find("td input[name='txtCustomerName']").val());
                            IDCard = jQuery.trim(jQuery(this).find("td input[name='txtIDCard']").val());
                            Gender = jQuery.trim(jQuery(this).find("td select[name='slCustomerSex']").find("option:selected").val());
                            GenderName = jQuery.trim(jQuery(this).find("td select[name='slCustomerSex']").find("option:selected").text());
                            Married = jQuery.trim(jQuery(this).find("td select[name='slCustomerMarried']").find("option:selected").val());
                            MarriageName = jQuery.trim(jQuery(this).find("td select[name='slCustomerMarried']").find("option:selected").text());
                            BirthDay = jQuery.trim(jQuery(this).find("td input[name='txtCustomerBirthDay']").val());
                            MobileNo = jQuery.trim(jQuery(this).find("td input[name='txtCustomerTel']").val());
                            Department = jQuery.trim(jQuery(this).find("td input[name='txtDepartment']").val());
                            DepartSubA = jQuery.trim(jQuery(this).find("td input[name='txtDepartmentA']").val());
                            DepartSubB = jQuery.trim(jQuery(this).find("td input[name='txtDepartmentB']").val());
                            DepartSubC = jQuery.trim(jQuery(this).find("td input[name='txtDepartmentC']").val());
                            NationName = jQuery.trim(jQuery(this).find("td select[name='slNation']").find("option:selected").text()); // jQuery.trim(jQuery(this).find("td input[name='txtNation']").val());
                            Note = jQuery.trim(jQuery(this).find("td input[name='txtNote']").val()); //获取备注信息 xmhuang 2013-10-26
                            Base64Photo = jQuery(this).find("td img[name='HeadImg']").attr("base64photo"); //xmhuang 2014-01-22
                        }
                        Is_Paused = jQuery(this).find("td input[name='chcIsPause']").attr("checked") == "checked" ? 1 : 0;
                        CurItem = ID_TeamTask + "_" + ID_TeamTaskGroup + "_" + Department + "_" + DepartSubA + "_" + DepartSubB
        + "_" + DepartSubC + "_" + CustomerName + "_" + IDCard + "_" + Gender + "_" + GenderName
        + "_" + Married + "_" + MarriageName + "_" + BirthDay + "_" + MobileNo + "_" + NationName + "_" + Note + "_" + Base64Photo + "_" + Is_Paused + "_" + ID_Customer + "|";
                        AllItem += CurItem;
                    }
                });
                that.close(); //关闭当前信息提示框
                //这里通过后台保存数据
                if (AllItem != "") {

                    //SaveCustomer_Ajax(AllItem);
                    art.dialog({
                        id: "OperWindowFrame",
                        lock: true, fixed: true, opacity: 0.3,
                        content: '正在生成体检号，请稍候...',
                        init: function () {
                            var that = this;
                            SaveCustomer_Ajax(AllItem);
                            //                            var fn = function () {
                            //                                
                            //                                SaveCustomer_Ajax(AllItem);
                            //                            };
                            //                            setTimeout(fn, 1);
                        },
                        close: function () {
                        }
                    }).show();
                }
                //that.close();
            };
            setTimeout(fn, 1);
        },
        close: function () {
        }
    }).show();
}

function CloseCurDialogWindow() {
    if (art.dialog.get('OperWindowFrame') != undefined) {
        art.dialog.get('OperWindowFrame').close();
    }
}
function SaveCustomer_Ajax(AllItem) {
    jQuery("#btnSaveCustomer").hide();
    //存储大数据请设置Content-length值
    var ID_Team = jQuery('#idSelectTeam').val();
    var TeamName = jQuery('#nameSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    //获取所有分组ID
    var ID_TeamTaskGroupS = "";
    jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
        ID_TeamTaskGroupS += "'" + jQuery(this).attr("id") + "',";
    });
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "SaveCustomerInfo", ID_Team: ID_Team, TeamName: TeamName, ID_TeamTask: ID_TeamTask, ID_TeamTaskGroupS: ID_TeamTaskGroupS, AllItem: AllItem },
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            if (msg.dataList0 != undefined) {
                ShowSystemDialog(msg.dataList0[0].Message);
                jQuery("#btnSaveCustomer").show();
            }

            if (msg.dataList1 != undefined) {
                DoSearch(); // BindCustomerCustInfo(msg.dataList1); xmhuang 2014-01-22 注释
            }
            //CloseCurDialogWindow();
        },
        complete: function () {
            CloseCurDialogWindow(); //关闭所有提示信息
            //ReBindTeamTaskGroupCustomerInfo();//重新绑定客户名单
            jQuery("#btnSaveCustomer").show();
        }
    });
}
/// <summary>
/// 清除名单(未生成体检号的客户名单)
/// </summary>
function ResetCustomer() {
    //判断是否为空
    if (jQuery("#tblTeamTaskGroupCustomerX tbody tr td input[type='checkbox'][name='ItemCheckbox']:checked").length == 0) {
        return false;
    }
    var msgContent = "您确认要清除所选的尚未生成体检号的客户名单吗？";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                return true;
            }
        }, {
            name: '确定',
            callback: function () {
                jQuery("#tblTeamTaskGroupCustomerX tbody tr td input[type='checkbox'][name='ItemCheckbox']").each(function () {
                    if (jQuery(this).attr('checked')) {
                        if (jQuery(this).parent().parent().attr('exist') != "1") {
                            jQuery(this).parent().parent().remove();
                        }
                    }
                });
                ResetRowNum("tblTeamTaskGroupCustomerX");
                return true;

            }, focus: true
        }]

    }).lock();
}
function DoChangeCustomerGroup() {
    var ShowDataElementID = "tblTeamTaskGroupCustomerX";
    var htmlContent = "", tempContent = "", TempGender = "", TempGenderName = "", TempMarried = "", TempMarriageName = "", TeampBirthDay = "", TeampRoleName = "", Age = "", ID_TeamTaskGroup = "", TeamTaskGroupName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", RoleName = "", groupInfo = "", CurID_TeamTaskGroup = "", CurTeamTaskGroupName = "";
    var obj = "", CustomerObj = "", IsCompar = false, ErrorMessage = "";
    jQuery("#" + ShowDataElementID + " tbody tr[id!='loading']").each(function (i, item) {
        obj = jQuery(this);
        IsCompar = false;
        ErrorMessage = "";
        ID_Customer = jQuery(obj).attr("ID_Customer");
        ID_TeamTask = jQuery(obj).attr("ID_TeamTask");
        TempGender = jQuery.trim(jQuery(obj).find("td select[name='slCustomerSex']").find("option:selected").val());
        TempGenderName = jQuery.trim(jQuery(obj).find("td select[name='slCustomerSex']").find("option:selected").text());
        TempMarried = jQuery.trim(jQuery(obj).find("td select[name='slCustomerMarried']").find("option:selected").val());
        TempMarriageName = jQuery.trim(jQuery(obj).find("td select[name='slCustomerMarried']").find("option:selected").text());
        TeampBirthDay = jQuery.trim(jQuery(obj).find("td input[name='txtCustomerBirthDay']").val());
        TeampRoleName = jQuery.trim(jQuery(obj).find("td input[name='txtCustomerRoleName']").val());
        if (TeampBirthDay != "") {
            Age = GetAgeByBirthDay(CurDate,TeampBirthDay);
        } else {
            Age = 0;
        }
        //先按照条件分组
        jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
            CustomerObj = jQuery(this);
            ID_TeamTaskGroup = jQuery.trim(jQuery(CustomerObj).attr("id"));
            TeamTaskGroupName = jQuery.trim(jQuery(CustomerObj).find("td input[name='txtTeamTaskGroupName']").val());
            Gender = jQuery.trim(jQuery(CustomerObj).find("td select[name='slSex']").find("option:selected").val());
            GenderName = jQuery.trim(jQuery(CustomerObj).find("td select[name='slSex']").find("option:selected").text());
            Married = jQuery.trim(jQuery(CustomerObj).find("td select[name='slMarried']").find("option:selected").val());
            MarriageName = jQuery.trim(jQuery(CustomerObj).find("td select[name='slMarried']").find("option:selected").text());
            MinAge = jQuery.trim(jQuery.trim(jQuery(CustomerObj).find("td input[name='txtMinAgeValue']").val()));
            MaxAge = jQuery.trim(jQuery.trim(jQuery(CustomerObj).find("td input[name='txtMaxAgeValue']").val()));
            RoleName = jQuery.trim(jQuery.trim(jQuery(CustomerObj).find("td input[name='txtRoleName']").val())); //角色名称
            if (MinAge == "" || MinAge == undefined) {
                MinAge = 0;
            }
            if (MaxAge == "" || MaxAge == undefined) {
                MaxAge = Age;
            }
            if (Gender == "-1") {
                TempGenderName = "----";
            }
            if (Married == "-1") {
                TempMarriageName = "----";
            }
            //计算年纪
            //判断客户属于哪个分组
            if (TempGenderName == GenderName && TempMarriageName == MarriageName && (Age >= MinAge && Age <= MaxAge) && TeampRoleName == RoleName) {
                IsCompar = true;
                jQuery(obj).attr("id_teamtaskgroup", ID_TeamTaskGroup);
                jQuery(obj).attr("groupname", TeamTaskGroupName);
                jQuery(obj).find("td label[name='lblErorrMessage']").text("");
                jQuery(obj).removeClass("ErrorMessage");
                return false;
            }
            else {
                if (TempGenderName != GenderName) {
                    ErrorMessage += "和分组[" + TeamTaskGroupName + "]性别匹配失败！";
                }
                if (TempMarriageName != MarriageName) {
                    ErrorMessage += "和分组[" + TeamTaskGroupName + "]婚姻匹配失败！";
                }
                if (Age < MinAge || Age > MaxAge) {
                    ErrorMessage += "和分组[" + TeamTaskGroupName + "]年龄限制匹配失败！";
                }
            }
        });
        if (TeampBirthDay == "") {
            IsCompar = false;
        }
        //判断有无匹配项
        if (!IsCompar) {
            jQuery(obj).addClass("ErrorMessage");
            //jQuery(this).find("td label[name='lblErorrMessage']").text(ErrorMessage);
            ErrorMessage = "";
        }
        else {
            jQuery(this).attr("title", "");
            jQuery(obj).removeClass("ErrorMessage");
        }
    });

}
var timer;
/// <summary>
/// 客户手动分组
/// </summary>
function ChangeCustomerGroup() {

    art.dialog({ lock: true, fixed: true, opacity: 0.3,
        content: '正在进行分组，请稍候...',
        init: function () {
            var that = this;
            var fn = function () {
                var ShowDataElementID = "tblTeamTaskGroupCustomerX";
                var Is_GroupPaused, htmlContent = "", tempContent = "", TempCustomerName = "", TempCustomerIDCard = "", TempGender = "", TempGenderName = "", TempMarried = "", TempMarriageName = "", TeampBirthDay = "", TeampRoleName = "", Age = "", ID_TeamTaskGroup = "", TeamTaskGroupName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", RoleName = "", groupInfo = "", CurID_TeamTaskGroup = "", CurTeamTaskGroupName = "";
                var obj = "", CustomerObj = "", IsCompar = false, ErrorMessage = "";

                jQuery("#" + ShowDataElementID + " tbody tr[id!='loading'][exist='0']").each(function (i, item) {
                    if (jQuery(this).attr("exist") != 1) {
                        obj = jQuery(this);
                        IsCompar = false;
                        ErrorMessage = "";
                        ID_Customer = jQuery(obj).attr("ID_Customer");
                        ID_TeamTask = jQuery(obj).attr("ID_TeamTask");
                        if (jQuery(obj).find("td label[name='slCustomerSex']").length > 0) {
                            TempCustomerName = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerName']").text()); //客户姓名
                            TempCustomerIDCard = jQuery.trim(jQuery(obj).find("td label[name='txtIDCard']").text()); //客户身份证
                            TempGender = jQuery.trim(jQuery(obj).find("td label[name='slCustomerSex']").attr("id_gender")); //jQuery.trim(jQuery(obj).find("td select[name='slCustomerSex']").find("option:selected").val());
                            TempGenderName = jQuery.trim(jQuery(obj).find("td label[name='slCustomerSex']").text()); //jQuery.trim(jQuery(obj).find("td select[name='slCustomerSex']").find("option:selected").text());
                            TempMarried = jQuery.trim(jQuery(obj).find("td label[name='slCustomerMarried']").attr("is_marriage")); //jQuery.trim(jQuery(obj).find("td select[name='slCustomerMarried']").find("option:selected").val());
                            TempMarriageName = jQuery.trim(jQuery(obj).find("td label[name='slCustomerMarried']").text()); //jQuery.trim(jQuery(obj).find("td select[name='slCustomerMarried']").find("option:selected").text());
                            TeampBirthDay = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerBirthDay']").text()); //jQuery.trim(jQuery(obj).find("td input[name='txtCustomerBirthDay']").val());
                            TeampRoleName = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerRoleName']").text()); //jQuery.trim(jQuery(obj).find("td input[name='txtCustomerRoleName']").val());

                        }
                        else {
                            TempCustomerName = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerName']").val()); //客户姓名
                            TempCustomerIDCard = jQuery.trim(jQuery(obj).find("td label[name='txtIDCard']").val()); //客户身份证
                            TempGender = jQuery.trim(jQuery(obj).find("td select[name='slCustomerSex']").find("option:selected").val());
                            TempGenderName = jQuery.trim(jQuery(obj).find("td select[name='slCustomerSex']").find("option:selected").text());
                            TempMarried = jQuery.trim(jQuery(obj).find("td select[name='slCustomerMarried']").find("option:selected").val());
                            TempMarriageName = jQuery.trim(jQuery(obj).find("td select[name='slCustomerMarried']").find("option:selected").text());
                            TeampBirthDay = jQuery.trim(jQuery(obj).find("td input[name='txtCustomerBirthDay']").val());
                            TeampRoleName = jQuery.trim(jQuery(obj).find("td input[name='txtCustomerRoleName']").val());

                        }
                        if (TeampBirthDay != "") {
                            TeampBirthDay = TeampBirthDay.replace("/", "-");
                            Age = GetAgeByBirthDay(CurDate, TeampBirthDay);
                        } else {
                            Age = 0;
                        }
                        if (!isDate(TeampBirthDay)) {
                            IsCompar = false;
                            ErrorMessage += "客户出生日期不正确,分组失败！";
                        }
                        else if (TempCustomerName == "" || TempCustomerIDCard == "") {
                            IsCompar = false;
                            ErrorMessage += "客户姓名或者客户身份证为空,分组失败！";
                        }
                        //xmhuang 2014-01-22 新增证件号长度验证
                        else if (TempCustomerIDCard.length != 15 && TempCustomerIDCard.length != 18) {
                            IsCompar = false;
                            ErrorMessage += "客户身份证格式不正确,分组失败！";
                        }
                        else {
                            /***********保存上一次验证原始值 Begin*************/
                            var CloneTempGenderName = TempGenderName; //存放需要验证的性别
                            var CloneTempMarriageName = TempMarriageName; //存放需要验证的性别
                            var CloneTeampRoleName = TeampRoleName; //存放需要验证的角色
                            var CloneMinAge = MinAge; //存放需要验证的最小年龄
                            var CloneMaxAge = MaxAge; //存放需要验证的最大年龄
                            /************保存上一次验证原始值 End************/
                            //先按照条件分组
                            jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
                                /*********重置原始值 Begin*****************/
                                TempGenderName = CloneTempGenderName;
                                TempMarriageName = CloneTempMarriageName;
                                TeampRoleName = CloneTeampRoleName;
                                MinAge = CloneMinAge;
                                MaxAge = CloneMaxAge;
                                /*********重置原始值 End*****************/

                                CustomerObj = jQuery(this);
                                ID_TeamTaskGroup = jQuery.trim(jQuery(CustomerObj).attr("id"));
                                TeamGroupName = jQuery.trim(jQuery(CustomerObj).find("td label[name='txtTeamTaskGroupName']").text());
                                FeeWayName = jQuery.trim(jQuery(CustomerObj).find("td label[name='slFeeWay']").text());
                                BusSet = jQuery.trim(jQuery(CustomerObj).find("td label[name='slBusSet']").attr("id_set"));
                                Gender = jQuery.trim(jQuery(CustomerObj).find("td label[name='slSex']").attr("id_gender"));
                                GenderName = jQuery.trim(jQuery(CustomerObj).find("td label[name='slSex']").text());
                                Married = jQuery.trim(jQuery(CustomerObj).find("td label[name='slMarried']").attr("is_marriage"));
                                MarriageName = jQuery.trim(jQuery(CustomerObj).find("td label[name='slMarried']").text());
                                MinAge = jQuery.trim(jQuery(CustomerObj).find("td label[name='txtMinAgeValue']").text());
                                MaxAge = jQuery.trim(jQuery(CustomerObj).find("td label[name='txtMaxAgeValue']").text());
                                RoleName = jQuery.trim(jQuery(CustomerObj).find("td label[name='txtRoleName']").text());
                                Is_GroupPaused = jQuery(CustomerObj).find("td input[name='chcIsPause']").attr("checked");
                                //如果最小年龄为空，则默认为0 ，如果最大年龄为空，则默认当前验证行的年龄为当前最大年龄，以保证全等验证
                                if (MinAge == "" || MinAge == undefined) {
                                    MinAge = 0;
                                }
                                if (MaxAge == "" || MaxAge == undefined) {
                                    MaxAge = Age;
                                }
                                if (Gender == "-1") {
                                    TempGenderName = GenderName; // "----"; //表示毋须验证性别,只需要把当前分组中的性别赋值给验证行即可执行全等验证
                                }
                                if (Married == "-1") {
                                    TempMarriageName = MarriageName; // "----"; //表示毋须验证婚姻,只需要把当前分组中的婚姻状况赋值给验证行即可执行全等验证
                                }
                                if (RoleName == "") {
                                    TeampRoleName = RoleName;
                                }
                                //计算年纪

                                //判断客户属于哪个分组
                                if (TempGenderName == GenderName && TempMarriageName == MarriageName && (Age >= MinAge && Age <= MaxAge) && TeampRoleName == RoleName) {

                                    IsCompar = true;
                                    jQuery(obj).attr("id_teamtaskgroup", ID_TeamTaskGroup);
                                    jQuery(obj).attr("groupname", TeamGroupName);
                                    jQuery(obj).find("td label[name='lblErorrMessage']").removeClass("ErrorMessage");
                                    jQuery(obj).find("td label[name='lblErorrMessage']").addClass("NoErrorMessage");
                                    jQuery(obj).find("td label[name='lblErorrMessage']").text("√");
                                    jQuery(obj).find("td div[name='displayGroupName']").text(TeamGroupName); //显示截断div值 xmhuang 20140425
                                    jQuery(obj).find("td label[name='lblGroupName']").text(TeamGroupName);
                                    jQuery(obj).find("td label[name='lblErorrMessage']").attr("title", TeamGroupName);

                                    if (Is_GroupPaused == true || Is_GroupPaused == "checked") {
                                        if (ID_Customer == "") { jQuery(obj).find("td input[name='chcIsPause']").attr("checked", Is_GroupPaused); }
                                    }
                                    return false;
                                }
                                else {
                                    if (TempGenderName != GenderName) {
                                        ErrorMessage += "和分组[" + TeamGroupName + "]性别匹配失败！";
                                    }
                                    if (TempMarriageName != MarriageName) {
                                        ErrorMessage += "和分组[" + TeamGroupName + "]婚姻匹配失败！";
                                    }
                                    if (Age < MinAge || Age > MaxAge) {
                                        ErrorMessage += "和分组[" + TeamGroupName + "]年龄限制匹配失败！";
                                    }
                                }
                            });
                        }
                        //判断有无匹配项
                        if (!IsCompar) {
                            jQuery(this).addClass("ErrorMessageX");
                            jQuery(this).attr("title", ErrorMessage);
                            jQuery(this).find("td label[name='lblErorrMessage']").removeClass("NoErrorMessage");
                            jQuery(this).find("td label[name='lblErorrMessage']").addClass("ErrorMessage");
                            jQuery(this).find("td label[name='lblErorrMessage']").text("×");
                            jQuery(this).find("td label[name='lblErorrMessage']").attr("title", ErrorMessage);
                        }
                        else {
                            jQuery(this).attr("title", "");
                            jQuery(this).removeClass("ErrorMessageX");
                        }
                    }
                });
                that.close();
            };
            setTimeout(fn, 1);
        },
        close: function () {
        }
    }).show();
}
/// <summary>
/// 读取客户名单模版并绑定值
/// </summary>
function ReadCustomerTemplate(msg) {
    if (msg == null || msg == undefined)
        return false;

    //读取当前任务的所有人员名单进行对比，是否已经包含
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    var AllCustomerInfo = GetCustomerByTeamAndTask(ID_Team, ID_TeamTask);
    var StrAllCustomerInfo = ","; //获取所有的身份证信息
    if (AllCustomerInfo.dataList != null) {
        jQuery(AllCustomerInfo.dataList).each(function (i, item) {
            StrAllCustomerInfo += jQuery.trim(item.IDCard) + ",";
        });
        //StrAllCustomerInfo = StrAllCustomerInfo.substring(0, StrAllCustomerInfo.length - 1);
    }

    //加上当前列表中的客户信息
    jQuery("#tblTeamTaskGroupCustomerX tbody tr").each(function () {

        if (jQuery(this).attr("exist") != 1) {
            if (jQuery(this).find("td input[name='txtIDCard'").length > 0) {
                StrAllCustomerInfo += jQuery.trim(jQuery(this).find("td input[name='txtIDCard'").val()) + ",";
            }
            else if (jQuery(this).find("td label[name='txtIDCard'").length > 0) {
                StrAllCustomerInfo += jQuery.trim(jQuery(this).find("td label[name='txtIDCard'").text()) + ",";
            }
        }
    });
    //        if (StrAllCustomerInfo != "") {
    //            StrAllCustomerInfo = StrAllCustomerInfo.substring(0, StrAllCustomerInfo.length - 1);
    //        }
    //此操作要求先保存分组信息
    //    if (!SaveTeamTaskGroup('tblTeamTaskGroupX')) {
    //        return false;
    //    }
    var ShowDataElementID = "tblTeamTaskGroupCustomerX";
    jQuery("#" + ShowDataElementID).data("data", msg.dataList0);
    //读取客户名单模版
    var xustomerTask = new ReadTeamTaskTemplate("tblTemplateTeamTaskGroupCustomer", ShowDataElementID);
    var customerListTheadTempleteContent = xustomerTask.teamTaskListTheadTempleteContent;
    var customerListBodyTempleteContent = jQuery("#tblTemplateTeamTaskGroupCustomer").html(); //  xustomerTask.teamTaskListBodyTempleteContent;
    var htmlContent = "", tempContent = "", TempGender = "", TempMarried = "", Age = "", ID_TeamTaskGroup = "", TeamTaskGroupName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", RoleName = "", groupInfo = "", CurID_TeamTaskGroup = "", CurTeamTaskGroupName = "", imgSrc = "";
    var IsCompar = false, CurCustomerInfo = "";
    var toolTipMsg = ""; //用于提示具有相同证件的客户导入信息方便备单人员进行核对
    if (customerListBodyTempleteContent != "") {
        jQuery(msg.dataList0).each(function (i, item) {
            //为空验证 Begin
            item.备注 = item.备注 == undefined ? "" : item.备注; //xmhuang 2013-10-28 excel新增备注字段导入功能
            item.身份证 = item.身份证 == undefined ? "" : item.身份证;
            item.出生日期 = item.出生日期 == undefined ? "" : item.出生日期;
            item.性别 = item.性别 == undefined ? "" : item.性别;
            item.婚姻 = item.婚姻 == undefined ? "" : item.婚姻;
            item.民族 = item.民族 == undefined ? "" : item.民族;
            item.联系电话 = item.联系电话 == undefined ? "" : item.联系电话;
            item.部门 = item.部门 == undefined ? "" : item.部门;
            item.二级部门 = item.二级部门 == undefined ? "" : item.二级部门;
            item.三级部门 = item.三级部门 == undefined ? "" : item.三级部门;
            item.四级部门 = item.四级部门 == undefined ? "" : item.四级部门;
            //为空验证 End

            //去除空格 Begin
            item.身份证 = jQuery.trim(item.身份证);
            item.出生日期 = jQuery.trim(item.出生日期);
            item.性别 = jQuery.trim(item.性别);
            item.婚姻 = jQuery.trim(item.婚姻);
            item.民族 = jQuery.trim(item.民族);
            item.联系电话 = jQuery.trim(item.联系电话);
            item.部门 = jQuery.trim(item.部门);
            item.二级部门 = jQuery.trim(item.二级部门);
            item.三级部门 = jQuery.trim(item.三级部门);
            item.四级部门 = jQuery.trim(item.四级部门);
            //去除空格 End

            //判断是否已经包含省份证信息了
            if (StrAllCustomerInfo.search("," + item.身份证 + ",") == -1) {
                StrAllCustomerInfo += item.身份证 + ",";
                item.出生日期 = item.出生日期.replace("/", "-").replace("/", "-").replace("/", "-").replace("年", "-").replace("月", "-").replace("0:00:00", "").replace("日", ""); //.substr(0, 10);
                item.出生日期 = GetFormatDate(item.出生日期);
                item.性别 = jQuery.trim(item.性别);
                item.婚姻 = jQuery.trim(item.婚姻);

                var CurCustomerInfo = new GetUserInfoByIDCard(item.身份证);
                //性别验证 Begin
                if (item.性别 == "男") {
                    TempGender = 1;
                }
                else if (item.性别 == "女") {
                    TempGender = 0;
                }
                else {
                    //没有填写性别则直接从身份证信息中读取性别
                    if (CurCustomerInfo.sex != -1) {
                        TempGender = CurCustomerInfo.sex;
                        item.性别 = CurCustomerInfo.sexName;
                    }
                    else {
                        TempGender = -1; //未能获取到客户性别，这种情况是有身份证号码填写不正确造成
                    }
                }
                //性别验证 End

                //出生日期 Begin
                if (item.出生日期 == "") {
                    item.出生日期 = CurCustomerInfo.birthday;
                }
                //出生日期 End

                if (item.婚姻 == "已婚") {
                    TempMarried = 1;
                }
                else if (item.婚姻 == "未婚") {
                    TempMarried = 0;
                }
                else {
                    TempMarried = -1;
                }
                if (item.头像 == undefined || item.头像 == "") {
                    imgSrc = defalutImagSrc;
                }
                else {
                    imgSrc = "data:image/gif;base64," + item.头像;
                }

                tempContent = jQuery(customerListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@exist/gi, "0")
                //.replace(/@flag/gi, item.flag)
                            .replace(/@ID_TeamTaskGroup/gi, "")
                            .replace(/@TeamTaskGroupName/gi, "")
                            .replace(/@CustomerName/gi, item.姓名 == undefined ? "" : item.姓名)
                            .replace(/@CustomerBirthDay/gi, item.出生日期 == undefined ? "" : item.出生日期)
                            .replace(/@CustomerRoleName/gi, item.角色 == undefined ? "" : item.角色)
                            .replace(/@IDCard/gi, item.身份证 == undefined ? "" : item.身份证)
                            .replace(/@CustomerTel/gi, item.联系电话 == undefined ? "" : item.联系电话)
                            .replace(/@DepartmentX/gi, item.部门 == undefined ? "" : item.部门)
                            .replace(/@DepartmentA/gi, item.二级部门 == undefined ? "" : item.二级部门)
                            .replace(/@DepartmentB/gi, item.三级部门 == undefined ? "" : item.三级部门)
                            .replace(/@DepartmentC/gi, item.四级部门 == undefined ? "" : item.四级部门)

                            .replace(/@TitleCustomerRoleName/gi, item.角色 == undefined ? "" : item.角色)
                            .replace(/@TitleCustomerTel/gi, item.联系电话 == undefined ? "" : item.联系电话)
                            .replace(/@TitleDepartmentX/gi, item.部门 == undefined ? "" : item.部门)
                            .replace(/@TitleDepartmentA/gi, item.二级部门 == undefined ? "" : item.二级部门)
                            .replace(/@TitleDepartmentB/gi, item.三级部门 == undefined ? "" : item.三级部门)
                            .replace(/@TitleDepartmentC/gi, item.四级部门 == undefined ? "" : item.四级部门)

                            .replace(/@ErorrMessage/gi, "--")
                            .replace(/@ID_Customer/gi, "")
                            .replace(/@NationName/gi, item.民族 == undefined ? "" : item.民族)
                            .replace(/@CustomerSex/gi, item.性别 == undefined ? "" : item.性别)
                            .replace(/@ID_Gender/gi, TempGender)
                            .replace(/@CustomerMarried/gi, item.婚姻 == undefined ? "" : item.婚姻)
                            .replace(/@Is_Marriage/gi, TempMarried)
                            .replace(/@Note/gi, item.备注)
                            .replace(/@Base64Photo/gi, item.头像 == undefined ? "" : item.头像)
                            .replace(/@imgSrc/gi, imgSrc)

                //由于新增或导入名单需要保全全部内容进行入库，这里暂不做截断处理
                //                            .replace(/@CustomerTel/gi, DisplayLitterLetter(item.联系电话, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
                //                            .replace(/@DepartmentX/gi, DisplayLitterLetter(item.部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
                //                            .replace(/@DepartmentA/gi, DisplayLitterLetter(item.二级部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
                //                            .replace(/@DepartmentB/gi, DisplayLitterLetter(item.三级部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
                //                            .replace(/@DepartmentC/gi, DisplayLitterLetter(item.四级部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
                //                            .replace(/@Note/gi, DisplayLitterLetter(item.备注, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
                            );
                //设置选中项
                jQuery(tempContent).find("td label[name='lblErorrMessage']").attr("title", "尚未分组！");
                jQuery(tempContent).find("td select[name='slCustomerSex'] option[value='" + TempGender + "']").attr("selected", true);
                jQuery(tempContent).find("td select[name='slCustomerMarried'] option[value='" + TempMarried + "']").attr("selected", true);

                //XMHuang 20130821 新增如果姓名和省份证为空则默认黄色标识改行数据
                if (item.身份证 == "" || item.姓名 == "") {
                    jQuery(tempContent).addClass("ErrorMessageX");
                    jQuery(tempContent).attr("title", "身份证或者姓名为空");
                }
                htmlContent += jQuery(tempContent)[0].outerHTML;
            }
            else {
                //提示重复身份证 xmhuang 20150312 
                toolTipMsg += item.姓名 + "[" + item.身份证 + "]</br>";
            }
        });
    }
    if (htmlContent != "") {
        jQuery('#' + ShowDataElementID + " tbody tr[id='loading']").hide();
        if (jQuery('#' + ShowDataElementID + ' tbody').length == 0) {
            jQuery('#' + ShowDataElementID).append('<tbody>' + htmlContent + '</tbody>');
        }
        else {
            jQuery('#' + ShowDataElementID + ' tbody').prepend(htmlContent);
        }
        htmlContent = "";
        jQuery('input[name="txtCustomerName"]').first().focus();
        ResetRowNum("tblTeamTaskGroupCustomerX");
        SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-18 
    }
    if (toolTipMsg != "") {
        ShowSystemDialog("以下证件号已存在于本次任务中,请核对</br>" + toolTipMsg);
    }
}
//获取指定格式额出生日期
function GetFormatDate(date) {
    if (date == undefined || date == "")
        return date;
    var arr = date.split('-');
    if (arr.length > 2) {
        date = arr[0] + "-" + arr[1] + "-" + arr[2];
        return date;
    }
}
function BindCustomerInfoFromExcel_Ajax(WorkSheetName, ExcelFilePath) {

    // var myDialog = art.dialog({ lock: true, fixed: true, opacity: 0.3, content: "正在加载数据，请稍候..." });
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { WorkSheetName: WorkSheetName, ExcelFilePath: ExcelFilePath, action: "ImportCustomerInfoFromExcel" },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            //这里遍历msg数据绑定所有通过或者没有通过的用户数据
            ReadCustomerTemplate(msg);
        },
        complete: function () {
            //myDialog.close();
        }
    });
}
//验证客户身份证号是否重复
function CheckCustomerIDCard(obj) {

    var CurIDCard = jQuery.trim(jQuery(obj).val());
    if (CurIDCard == "")
        return false;
    //读取当前任务的所有人员名单进行对比，是否已经包含
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();


    //从当前列表中判断是否有相同的身份证号
    var TempCurIDCard = "", Value = 0, flag = false;
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id!='loading']").each(function () {

        TempCurIDCard = "";
        if (jQuery(this).find("td input[name='txtIDCard'").length > 0) {
            TempCurIDCard = jQuery.trim(jQuery(this).find("td input[name='txtIDCard'").val());
            //StrAllCustomerInfo += "," + TempCurIDCard + ",";
        }
        else if (jQuery(this).find("td label[name='txtIDCard'").length > 0) {
            TempCurIDCard = jQuery.trim(jQuery(this).find("td label[name='txtIDCard'").text());
            //StrAllCustomerInfo += "," + TempCurIDCard + ",";
        }
        if (TempCurIDCard == CurIDCard) {
            Value++;
        }
        if (Value > 1) {
            flag = true;
            ShowSystemDialog("列表中已经包含[" + CurIDCard + "]的客户名单，请更改！");
            jQuery(obj).focus();
            jQuery(obj).select();
            return false;
        }
    });
    //表示在当前列表中不包含该身份证号码，则需要从数据库中检索是否存在相同的身份证
    if (!flag) {
        var AllCustomerInfo = GetCustomerByTeamAndTask(ID_Team, ID_TeamTask);
        var StrAllCustomerInfo = ","; //获取所有的身份证信息
        if (AllCustomerInfo.dataList != null) {
            jQuery(AllCustomerInfo.dataList).each(function (i, item) {
                StrAllCustomerInfo += item.IDCard + ",";
            });
            //StrAllCustomerInfo = StrAllCustomerInfo.substring(0, StrAllCustomerInfo.length - 1);
            if (StrAllCustomerInfo.search("," + CurIDCard + ",") > -1) {
                ShowSystemDialog("当前任务已经包含[" + CurIDCard + "]的客户名单，请更改！");
                jQuery(obj).focus();
                jQuery(obj).select();
                return false;
            }
        }
    }
}
//验证客户名单信息
function CheckCustomer(obj) {

    //验证是否同组重名
    //验证是否没有包含在分组中
    var IsCompar = false; Age = "", TempGender = "", TempMarried = "", Age = "", ID_TeamTaskGroup = "", TeamTaskGroupName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", RoleName = "", groupInfo = "", CurID_TeamTaskGroup = "", CurTeamTaskGroupName = "";
    var CurObj, CurCustomerName = "", CurGroupName = "", CurGender = "", CurGenderName = "", CurMarried = "", CurMarriageName = "", CurRoleName = "";
    var parentTr = jQuery(obj).parent().parent(); //获取元素所在行
    var ParentTbody = jQuery(obj).parent().parent().parent();
    var TempCustomerName = "", TempGroupName = "", TempGender = "", TempGenderName = "", TempMarried = "", TempMarriageName = "", TempRoleName = "", CurBirthDay = "";

    jQuery(parentTr).each(function () {
        CurObj = jQuery(this);
        CurGroupName = jQuery.trim(jQuery(this).find("td label[name='lblGroupName']").text()); //获取组名
        CurGender = jQuery.trim(jQuery(this).find("td select[name='slCustomerSex']").find("option:selected").val());
        CurGenderName = jQuery.trim(jQuery(this).find("td select[name='slCustomerSex']").find("option:selected").text());
        CurMarried = jQuery.trim(jQuery(this).find("td select[name='slCustomerMarried']").find("option:selected").val());
        CurMarriageName = jQuery.trim(jQuery(this).find("td select[name='slCustomerMarried']").find("option:selected").text());
        CurRoleName = jQuery.trim(jQuery(this).find("td input[name='txtCustomerRoleName']").val()); //角色名称
        CurCustomerName = jQuery.trim(jQuery(this).find("td input[name='txtCustomerName']").val()); //客户名称
        CurBirthDay = jQuery.trim(jQuery(this).find("td input[name='txtCustomerBirthDay']").val()); //客户名称
        //客户姓名不允许为空
        if (CurCustomerName == "") {
            //ShowSystemDialog("客户姓名不能为空，请您填写！");
            jQuery(this).find("td input[name='txtCustomerName']").focus();
            return false;
        }
        if (jQuery.trim(jQuery(this).find("td input[name='txtCustomerBirthDay']").val()) == "") {
            Age = 0;
        }
        else {
            Age = GetAgeByBirthDay(CurDate, jQuery.trim(jQuery(this).find("td input[name='txtCustomerBirthDay']").val()));
        }
        ID_TeamTaskGroup = "";
        TeamTaskGroupName = "";
        Gender = "";
        GenderName = "";
        Married = "";
        MarriageName = ""; ;
        MinAge = "";
        MaxAge = "";
        RoleName = "";
        jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
            IsCompar = false;
            ID_TeamTaskGroup = jQuery.trim(jQuery(this).attr("id"));
            TeamGroupName = jQuery.trim(jQuery(this).find("td label[name='txtTeamTaskGroupName']").text());
            FeeWayName = jQuery.trim(jQuery(this).find("td label[name='slFeeWay']").text());
            BusSet = jQuery.trim(jQuery(this).find("td label[name='slBusSet']").attr("id_set"));
            Gender = jQuery.trim(jQuery(this).find("td label[name='slSex']").attr("id_gender"));
            GenderName = jQuery.trim(jQuery(this).find("td label[name='slSex']").text());
            Married = jQuery.trim(jQuery(this).find("td label[name='slMarried']").attr("is_marriage"));
            MarriageName = jQuery.trim(jQuery(this).find("td label[name='slMarried']").text());
            MinAge = jQuery.trim(jQuery(this).find("td label[name='txtMinAgeValue']").text());
            MaxAge = jQuery.trim(jQuery(this).find("td label[name='txtMaxAgeValue']").text());
            RoleName = jQuery.trim(jQuery(this).find("td label[name='txtRoleName']").text());
            //            ID_TeamTaskGroup = jQuery.trim(jQuery(this).attr("id"));
            //            TeamTaskGroupName = jQuery.trim(jQuery(this).find("td input[name='txtTeamTaskGroupName']").val());
            //            Gender = jQuery.trim(jQuery(this).find("td select[name='slSex']").find("option:selected").val());
            //            GenderName = jQuery.trim(jQuery(this).find("td select[name='slSex']").find("option:selected").text());
            //            Married = jQuery.trim(jQuery(this).find("td select[name='slMarried']").find("option:selected").val());
            //            MarriageName = jQuery.trim(jQuery(this).find("td select[name='slMarried']").find("option:selected").text());
            //            MinAge = jQuery.trim(jQuery.trim(jQuery(this).find("td input[name='txtMinAgeValue']").val()));
            //            MaxAge = jQuery.trim(jQuery.trim(jQuery(this).find("td input[name='txtMaxAgeValue']").val()));
            //            RoleName = jQuery.trim(jQuery.trim(jQuery(this).find("td input[name='txtRoleName']").val())); //角色名称


            if (MinAge == "" || MinAge == undefined) {
                MinAge = 0;
            }
            if (MaxAge == "" || MaxAge == undefined) {
                MaxAge = Age;
            }

            //如果婚姻为-1，则不验证婚姻
            if (Married == "-1" && Gender != "-1") {
                //分组依据相同，设置其分组
                if (CurGender == Gender && (Age >= MinAge && Age <= MaxAge) && CurRoleName == RoleName) {
                    IsCompar = true;
                    return false; //退出循环
                }
            }
            //如果性别为-1，则不验证
            if (Married != "-1" && Gender == "-1") {
                //分组依据相同，设置其分组
                if (CurMarried == Married && (Age >= MinAge && Age <= MaxAge) && CurRoleName == RoleName) {
                    IsCompar = true;
                    return false; //退出循环
                }
            }
            if (Married == "-1" && Gender == "-1") {
                //分组依据相同，设置其分组
                if ((Age >= MinAge && Age <= MaxAge) && CurRoleName == RoleName) {
                    IsCompar = true;
                    return false; //退出循环
                }
            }
            if (Married != "-1" && Gender != "-1") {
                //分组依据相同，设置其分组
                if (CurMarried == Married && CurGender == Gender && (Age >= MinAge && Age <= MaxAge) && CurRoleName == RoleName) {
                    IsCompar = true;
                    return false; //退出循环
                }
            }
        });
        if (CurBirthDay == "") {
            IsCompar = false;
        }
        if (IsCompar) {
            //设置分组
            jQuery(this).removeClass("ErrorMessageX");
            jQuery(this).attr("id_teamtaskgroup", ID_TeamTaskGroup);
            jQuery(this).attr("groupname", TeamTaskGroupName);
            jQuery(this).attr("errormessage", "");
            jQuery(this).find("td label[name='lblErorrMessage']").text("√")
            jQuery(obj).find("td div[name='displayGroupName']").text(TeamGroupName); //显示截断div值 xmhuang 20140425
            jQuery(this).find("td label[name='lblGroupName']").text(TeamGroupName);
        }
        else {
            jQuery(this).addClass("ErrorMessageX");
            jQuery(this).attr("id_teamtaskgroup", "");
            jQuery(this).attr("groupname", "");
            jQuery(this).attr("errormessage", "");
            jQuery(this).find("td label[name='lblGroupName']").text("");
            jQuery(obj).find("td div[name='displayGroupName']").text(""); //显示截断div值 xmhuang 20140425
            jQuery(this).find("td label[name='lblErorrMessage']").text("×")
            jQuery(this).find("td label[name='lblErorrMessage']").attr("title", "分组失败！");
        }
    });
}

//从Excel报表导入客户名单
function ImportFromExcel() {

    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").length == 0) {
        ShowSystemDialog("对不起，团体任务分组信息不能为空，请先维护！");
        return false;
    }

    //判断任务分组是否保存
    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体任务分组信息尚未保存，请先保存！");
        return false;
    }
    //判断收费项目是否保存
    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体收费项目信息尚未保存，请先保存！");
        return false;
    }
    var TeamTaskID = jQuery('#idSelectTeamTask').val();
    this.ReturnValue = "";
    var aExcelFileInfo = new ExcelFileInfo("", "", "", "no");
    var title = "选择文件";
    var url = "/System/Customer/ShowDialog.aspx?Type=Excel&Page=ImportExcel.aspx&TeamTaskGroupID=" + TeamTaskID + "&Path=KHMD";
    art.dialog.open(url,
    {
        width: 350,
        height: 300,
        drag: true,
        lock: true,
        id: 'OperWindowFrame',
        title: title,
        cache: false,
        init: function () {

        },
        close: function () {
            aExcelFileInfo = art.dialog.data("aExcelFileInfo");
            if (aExcelFileInfo == undefined) {
                return true;
            }
            if (aExcelFileInfo.ReturnValue == "ok") {
                var ExcelFilePath = aExcelFileInfo.ExcelFilePath;
                var FileFormat = aExcelFileInfo.FileFormat;
                url = "/System/Customer/SelectExcelInfo.aspx?FilePath=" + escape(aExcelFileInfo.ExcelFilePath);
                //var CurWindow = window.showModalDialog("/System/Customer/SelectExcelInfo.aspx?FilePath=" + escape(aExcelFileInfo.ExcelFilePath), aExcelFileInfo, "dialogWidth:420px; dialogHeight:320px; scroll:yes; status:no; resizable:no");
                art.dialog.open(url,
                {
                    width: 350,
                    height: 300,
                    drag: false,
                    lock: true,
                    id: 'OperWindowFrame1',
                    title: title,
                    close: function () {
                        aExcelFileInfo = art.dialog.data("aExcelFileInfo");
                        if (aExcelFileInfo.WorkSheetName != '') {
                            //ShowProgressPage("正在导入客户名单，请稍后、、、");
                            //这里采用Ajax方式进行Excel数据验证并返回客户端呈现
                            var WorkSheetName = aExcelFileInfo.WorkSheetName;
                            var OperValue = "ImportFromExcel";
                            BindCustomerInfoFromExcel_Ajax(WorkSheetName, ExcelFilePath);
                        }
                    }
                });

            }
            else {
                return true;
            }
        }
    });
    return false;
    window.showModalDialog("/System/Customer/ShowDialog.aspx?Type=Excel&Page=ImportExcel.aspx&TeamTaskGroupID=" + TeamTaskGroupID + "&Path=KHMD", aExcelFileInfo, "dialogWidth:420px; dialogHeight:320px; scroll:yes; status:no; resizable:no");
    if (aExcelFileInfo.ReturnValue == "ok") {
        var ExcelFilePath = aExcelFileInfo.ExcelFilePath;
        var FileFormat = aExcelFileInfo.FileFormat;
        //等待用户选择表名
        var CurWindow = window.showModalDialog("/System/Customer/SelectExcelInfo.aspx?FilePath=" + escape(aExcelFileInfo.ExcelFilePath), aExcelFileInfo, "dialogWidth:420px; dialogHeight:320px; scroll:yes; status:no; resizable:no");

        if (aExcelFileInfo.ReturnValue == "ok") {
            if (aExcelFileInfo.WorkSheetName != '') {
                //ShowProgressPage("正在导入客户名单，请稍后、、、");
                //这里采用Ajax方式进行Excel数据验证并返回客户端呈现
                var WorkSheetName = aExcelFileInfo.WorkSheetName;
                var OperValue = "ImportFromExcel";
                BindCustomerInfoFromExcel_Ajax(WorkSheetName, ExcelFilePath);
            }
        }
    }
    else if (aExcelFileInfo.ReturnValue != "no" && aExcelFileInfo.ReturnValue != "cancel") {
        ShowSystemDialog(aExcelFileInfo.ReturnValue);
    }
}
function ExcelFileInfo(excelFilePath, workSheetName, fileFormat, returnValue) {
    this.ExcelFilePath = excelFilePath;
    this.ReturnValue = returnValue;
    this.WorkSheetName = workSheetName;
    this.FileFormat = fileFormat;
}
/****************************导入名单  End*****************************/


/**************************调整团体、团体任务编辑模式后的新方法 Begin*****************************************/
/// <summary>
/// 弹出框中新增团体信息
/// </summary>
function AddNewTeam() {
    type = "add";
    var teamTask = new ReadTeamTaskTemplate("TeamTemplate_Add", "TeamTaskEditList");
    var teamTaskListTheadTempleteContent = teamTask.teamTaskListTheadTempleteContent;
    var teamTaskListBodyTempleteContent = teamTask.teamTaskListBodyTempleteContent;
    jQuery("#TeamTaskEditList tbody").html(teamTaskListBodyTempleteContent);
    jQuery("#TeamTaskEditList tbody tr td input[name='txtTeamName']").focus();
    //    //jQuery("#lblID_Team").text("******");
    //    jQuery("#txtMyTeamName").val("");
    //    jQuery("#txtInputCode").val("");
    //    jQuery("#txtNote").val("");
    //    jQuery("#txtMyTeamName").removeAttr("readOnly");
    //    jQuery("#txtMyTeamName").focus();
}

/// <summary>
/// 弹出框中新增团体任务信息
/// </summary>
function AddNewTeamTask() {
    var teamTask = new ReadTeamTaskTemplate("TeamTaskTemplate_Add", "TeamTaskEditList");
    var teamTaskListTheadTempleteContent = teamTask.teamTaskListTheadTempleteContent;
    var teamTaskListBodyTempleteContent = teamTask.teamTaskListBodyTempleteContent;
    jQuery("#TeamTaskEditList tbody").html(teamTaskListBodyTempleteContent);
    jQuery("#TeamTaskEditList tbody tr td input[name='txtMyTeamTaskName']").focus();
    SetTableRowStyle();
}

/// <summary>
/// 保存团体任务
/// </summary>
function SaveTeamTask(IsClose) {
    if (type.toLowerCase() == "add") {
        if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtMyTeamTaskName']").val()) == "")//判断任务名称是否为空
        {
            IsCanSave = false;
            ShowSystemDialog("对不起，任务名称不允许为空，请您填写！");
            jQuery("#TeamTaskEditList [name='txtMyTeamTaskName']").focus();
            jQuery("#TeamTaskEditList [name='txtMyTeamTaskName']").select();
            return false;
        }
    }
    if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtTaskExamStartDate']").val()) == "")//判断开始日期是否为空
    {
        IsCanSave = false;
        ShowSystemDialog("对不起，开始日期不允许为空，请您填写！");
        //jQuery("#TeamTaskEditList [name='txtTaskExamStartDate']").focus();
        jQuery("#TeamTaskEditList [name='txtTaskExamStartDate']").select();
        return false;
    }
    if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtTaskExamEndDate']").val()) == "")//判断结束日期是否为空
    {
        IsCanSave = false;
        ShowSystemDialog("对不起，结束日期不允许为空，请您填写！");
        //jQuery("#TeamTaskEditList [name='txtTaskExamEndDate']").focus();
        jQuery("#TeamTaskEditList [name='txtTaskExamEndDate']").select();
        return false;
    }
    if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtTaskNumCount']").val()) == "") //判断体检人数是否为空
    {
        IsCanSave = false;
        ShowSystemDialog("对不起，体检人数不允许为空，请您填写！");
        jQuery("#TeamTaskEditList [name='txtTaskNumCount']").focus();
        jQuery("#TeamTaskEditList [name='txtTaskNumCount']").select();
        return false;
    }
    if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtContact']").val()) == "")//判断联系人是否为空
    {
        IsCanSave = false;
        ShowSystemDialog("对不起，联系人不允许为空，请您填写！");
        jQuery("#TeamTaskEditList [name='txtContact']").focus();
        jQuery("#TeamTaskEditList [name='txtContact']").select();
        return false;
    }
    if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtTel']").val()) == "")//判断联系电话是否为空
    {
        IsCanSave = false;
        ShowSystemDialog("对不起，联系电话不允许为空，请您填写！");
        jQuery("#TeamTaskEditList [name='txtTel']").focus();
        jQuery("#TeamTaskEditList [name='txtTel']").select();
        return false;
    }
    //    else if (!isMobil(jQuery.trim(jQuery("#TeamTaskEditList [name='txtTel']").val()))) {
    //        IsCanSave = false;
    //        ShowSystemDialog("请您输入正确的手机号码！");
    //        jQuery("#TeamTaskEditList [name='txtTel']").focus();
    //        jQuery("#TeamTaskEditList [name='txtTel']").select();
    //        return false; //退出循环
    //    }
    var StartDate = jQuery.trim(jQuery("#TeamTaskEditList [name='txtTaskExamStartDate']").val());
    var EndDate = jQuery.trim(jQuery("#TeamTaskEditList [name='txtTaskExamEndDate']").val());
    if (StartDate > EndDate)//判断日期是否满足要求
    {
        IsCanSave = false;
        ShowSystemDialog("对不起，开始日期不得大于结束日期，请您修改！");
        //jQuery("#TeamTaskEditList [name='txtTaskExamStartDate']").focus();
        jQuery("#TeamTaskEditList [name='txtTaskExamStartDate']").select();
        return false;
    }

    var IsCanSave = true, ID_TeamTask, exist, TeamTaskName, TargetDate, TaskExamStartDate, TaskExamEndDate, TaskNumCount, Contact, Tel, InputCode, DispOrder, TeamTaskItems = ''; //记录所有的团体任务
    if (type.toLowerCase() == "add") {
        exist = 0;
        TeamTaskName = jQuery.trim(jQuery("#TeamTaskEditList [name='txtMyTeamTaskName']").val());  //任务名称
    }
    else if (type.toLowerCase() == "edit") {
        exist = 1;
        TeamTaskName = jQuery.trim(jQuery("#TeamTaskEditList [name='txtMyTeamTaskName']").text());  //任务名称
    }
    ID_TeamTask = jQuery.trim(jQuery("#TeamTaskEditList [name='lblID_TeamTask']").text());

    TaskExamStartDate = StartDate; //预约开始日期
    TaskExamEndDate = EndDate; //预约结束日期
    TaskNumCount = jQuery.trim(jQuery("#TeamTaskEditList [name='txtTaskNumCount']").val()); //预约人数
    TaskNumCount == "" ? 0 : TaskNumCount;
    Contact = jQuery.trim(jQuery("#TeamTaskEditList [name='txtContact']").val()); //联系人
    Tel = jQuery.trim(jQuery("#TeamTaskEditList [name='txtTel']").val()); //联系电话
    InputCode = ""; //输入码
    DispOrder = 0; //排序号
    curItem = ID_TeamTask + "_" + TeamTaskName + "_" + TaskExamStartDate + "_" + TaskExamEndDate + "_" + TaskNumCount + "_" + Contact + "_" + Tel + "_" + DispOrder + "_" + InputCode + "_" + exist;
    TeamTaskItems += curItem + "|";
    if (IsCanSave) {
        var qustData = { action: 'SaveTeamTaskData',
            modelName: modelName,
            type: type,
            ID_Team: curTeam,
            TeamTaskItems: TeamTaskItems
        }
        //存储大数据请设置Content-length值
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxTeamOper.aspx",
            data: qustData,
            cache: false,
            contentType: "application/x-www-form-urlencoded;Content-length=1024000",
            dataType: "json",
            success: function (msg) {
                // 检查Ajax返回数据的状态等 20140504 by xmhuang 
                msg = CheckAjaxReturnDataInfo(msg);
                if (msg == null || msg == "") {
                    return;
                }
                ShowSystemDialog(msg.Message);
                if (msg.success == "1") {
                    //重新绑定数据
                    //GetTeamTaskInfoByKeyWord("tblTeamTask", "ID_Team", jQuery('#idSelectTeam').val(), 0);
                }
            },
            complete: function () {
                if (IsClose == 1) {
                    CloseDialogWindow();
                }
                else if (IsClose == 0) {
                    AddNewTeamTask();
                }
            }
        });
    }
}
/// <summary>
/// 保存团体信息
/// </summary>
function SaveTeam(IsClose) {
    var ID_Team = "", TeamName = "", InputCode = "", Note = "";
    if (type.toLowerCase() == "add") {
        if (jQuery.trim(jQuery("#TeamTaskEditList [name='txtTeamName']").val()) == "")//判断团体名称不允许为空
        {
            IsCanSave = false;
            ShowSystemDialog("对不起，团体名称不允许为空，请您填写！");
            jQuery("#TeamTaskEditList [name='txtTeamName']").focus();
            jQuery("#TeamTaskEditList [name='txtTeamName']").select();
            return false;
        }
        TeamName = jQuery.trim(jQuery("#TeamTaskEditList [name='txtTeamName']").val());  //团体名称
    }
    else if (type.toLowerCase() == "edit") {
        TeamName = jQuery.trim(jQuery("#TeamTaskEditList [name='txtTeamName']").text());  //团体名称
    }
    ID_Team = jQuery.trim(jQuery("#TeamTaskEditList [name='txtID_TeaM']").text());
    Note = jQuery.trim(jQuery("#TeamTaskEditList [name='txtNote']").val());
    var qustData = {
        action: 'SaveData',
        modelName: modelName,
        type: type,
        ID_Team: ID_Team,
        TeamName: TeamName,
        ID_Creator: ID_Creator,
        Creator: Creator,
        CreateDate: CreateDate,
        InputCode: InputCode,
        Note: Note
    };
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: qustData,
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            ShowSystemDialog(msg.Message);
            if (msg.success == "1") {
                jQuery("#lblID_Team").text(msg.ID_Team);
                jQuery("#txtInputCode").val(msg.InputCode);
                //SureColse();
            }
            else {
            }
        },
        complete: function () {
            if (IsClose == 1) {
                CloseDialogWindow();
            }
            else if (IsClose == 0) {
                AddNewTeam();
            }
        }
    });
}


/**************************调整团体、团体任务编辑模式后的新方法 End*****************************************/


/**************************克隆   Begin*******************************************************************/
/// <summary>
/// 展示克隆窗体
/// </summary>
function ShowCloneWin() {
    var data = jQuery("#tblTeamTaskGroupFee").data("data"); //获取当前选中任务信息
    if (data == undefined) {
        ShowSystemDialog("请您勾选一个需要进行克隆的任务分组！");
        return false;
    }
    var tipscontent = '<table class="ModifyPassword">' +
            '<tbody>' +
            '    <tr><td class="left">分组ID：</td><td><input style="width:120px;" onkeyup="OnFormKeyUp();" maxlength="20" name="txtCloneTeamTaskGroupID" id="txtCloneTeamTaskGroupID"/> &nbsp;</td></tr>' +
            '<tr><td colspan="2" align="center"><span id="divUserLoginTips" style="color:red;">&nbsp;</span></td></tr>' +
            '</tbody>' +
            '</table>';
    art.dialog({
        id: 'OperWindowFrame',
        content: tipscontent,
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '克隆收费项目',
        init: function () {
            document.getElementById("txtCloneTeamTaskGroupID").focus();
            document.getElementById("txtCloneTeamTaskGroupID").select();
        },
        button: [{
            name: '确定',
            callback: function () {
                return DoCloneCustFee();
            }, focus: true
        }, {
            name: '取消'
        }]
    });
}
/// <summary>
/// 执行克隆
/// </summary>
function DoCloneCustFee(ToTeamTaskGroupID) {
    var FromTeamTaskGroupID = jQuery.trim(jQuery("#txtCloneTeamTaskGroupID").val());
    var ToTeamTaskGroupName = "";
    var ToFeeWayName = "";
    if (FromTeamTaskGroupID != "") {
        if (!IsNum(FromTeamTaskGroupID)) {
            ShowCallBackSystemDialog("对不起，任务ID必须为整数！", function () {
                jQuery("#txtCloneTeamTaskGroupID").focus();
                jQuery("#txtCloneTeamTaskGroupID").select();
            });
            return false;
        }

        var ToTeamTaskGroupID = "";
        var ToTeamTaskGroupName;
        var ToFeeWayName;
        var data = jQuery("#tblTeamTaskGroupFee").data("data");
        if (data != undefined) {
            ToTeamTaskGroupID = data.ID_TeamTaskGroup;
            ToTeamTaskGroupName = data.TeamGroupName;
            ToFeeWayName = data.FeeWayName;
            ToFeeWay = data.FeeWay;
        } else {
            ShowSystemDialog("获取任务信息失败，请重试！");
            return false;
        }
        return CloneCustFee_Ajax(FromTeamTaskGroupID, ToTeamTaskGroupID, ToTeamTaskGroupName, ToFeeWayName);
    }
    else {
        ShowCallBackSystemDialog("请您输入分组ID！", function () {
            jQuery("#txtCloneTeamTaskGroupID").focus();
            jQuery("#txtCloneTeamTaskGroupID").select();
        });
        return false;
    }
}
/// <summary>
/// 通过Ajax请求，克隆指定团体任务ID的收费项目信息
/// </summary>
function CloneCustFee_Ajax(FromTeamTaskGroupID, ToTeamTaskGroupID, ToTeamTaskGroupName, ToFeeWayName) {
    //这里从下拉框中获取付费方式 Begin xmhuang 2014-04-13
    // var FeeWayName = ToFeeWayName;
    var FeeWay = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].value; //付费方式
    var FeeWayName = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].text; //付费方式
    //这里从下拉框中获取付费方式 End   xmhuang 2014-04-13
    var ID_TeamTaskGroup = ToTeamTaskGroupID;
    var TeamTaskGroupName = ToTeamTaskGroupName;

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "CloneCustFee", FromTeamTaskGroupID: FromTeamTaskGroupID, ToTeamTaskGroupID: ToTeamTaskGroupID },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            if (msg != null) {
                if (msg.dataList.length > 0) {
                    //这里绑定收费项目
                    var newContent = "";
                    var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("tblTemplateTeamTaskGroupFee");
                    var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
                    var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
                    jQuery(msg.dataList).each(function (i, item) {
                        //如果没有行或者没有当前ID的非退费项目则允许添加到列表中
                        if (jQuery("#tblTeamTaskGroupFee tbody tr[id_fee='" + item.ID_Fee + "']").length == 0)//判断是否包含某个项目 xmhuang 2013-11-08
                        {
                            newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@class/gi, "NewXM")
                            .replace(/@exist/gi, "0")
                            .replace(/@PEPackageID/gi, item.PEPackageID)
                            .replace(/@ID_TeamTaskGroup/gi, ID_TeamTaskGroup)
                            .replace(/@TeamTaskGroupName/gi, TeamTaskGroupName)
                            .replace(/@FeeWayName/gi, FeeWayName)
                             .replace(/@FeeWay/gi, FeeWay)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)
                            .replace(/@Price/gi, item.Price)
                            .replace(/@Discount/gi, item.Discount)
                            .replace(/@FactPrice/gi, item.FactPrice)
                            .replace(/@userName/gi, item.userName)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                            .replace(/@date/gi, item.date)
                        }
                    });
                    if (newContent != '') {
                        jQuery("#tblTeamTaskGroupFee tbody").append(newContent);
                        newContent = "";
                        SumJG(); //计算总计
                        SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-18 
                        ScollToTbCustFeeBottom();
                    }
                    //显示提示信息
                    //art.dialog.tips("成功克隆收费项目：" + Tips.substring(0, Tips.length - 1), 2);
                }
                else {
                    return ShowCallBackSystemDialog("对不起,您输入的任务ID不存在！", function () {
                        ShowCloneWin();
                        return false;
                    });

                }
            }
            else {
                return ShowCallBackSystemDialog("对不起,您输入的任务ID不存在！", function () {
                    ShowCloneWin();
                    return false;
                });

            }
        },
        complete: function () {
            CloseCurDialogWindow();
            return true;
        }
    });
}
/**************************克隆   End*******************************************************************/

/// <summary>
/// 主动勾选任务分组加载收费项目和客户信息，用于区分是自动绑定收费项目还是手动触发绑定收费项目和客户信息
///手动绑定收费项目需要重置收费项目区域和客户信息区域
///自动绑定时，不能重置以上区域，因为收费项目中可能存在未保存项，此时需要保存
/// </summary>
function ClickChangeGroup(obj) {

    //ResetTeamGroupChirldInfo(); return false;
    //ChangeGroup(obj); xmhuang 2014-03-28
}

/********************分页绑定客户名单 Begin***********************/
var tempOperPageCount = 0;
var tempOldtotalCount = 0;
var pagerCustomerData = null; //记录当前分页数据源
function pageselectCallback(page_index, jq) {
    if (tempOperPageCount > 0) {
        tempOperPageCount++;
        QueryPagesData(page_index);
    }
    tempOperPageCount++;
    return false;
}
function DoSearch() {
    tempOperPageCount = 0;
    QueryPagesData(0); //重新按照新输入的条件进行查询
}
/// <summary>
///通过团体、团体任务分页查询其对应的客户名单
///新增通过CustomerName查询客户体检信息功能 xmhuang 20140513
/// </summary>
function QueryPagesData(pageIndex) {
    var optInit = getOptionsFromForm06();
    var totalCount = 0;
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    var ID_TeamTaskGroup = ""; //遍历分组获取勾选的分组信息
    //    if (jQuery("#tblTeamTaskGroupX tbody tr[exist='1'] td input:checked").length > 0) {
    //        jQuery("#tblTeamTaskGroupX tbody tr[exist='1'] td input:checked").each(function () {
    //            ID_TeamTaskGroup += jQuery(this).parent().parent().attr("id") + ",";
    //        });
    //    }
    //获取选中的团体任务分组ID
    jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
        if (jQuery(this).find("td input[type='checkbox']").attr("checked")) {
            ID_TeamTaskGroup += jQuery(this).attr("id") + ",";
        }
    });

    var ID_Customer = jQuery.trim(jQuery('#txtSFZ').val());  //身份证或体检号
    var IDCard = ID_Customer;
    var IsCustomerID = 0;
    //判断是否是体检号
    if (isCustomerExamNo(ID_Customer)) {
        IsCustomerID = 1;
        IDCard = "";
    }
    else {
        ID_Customer = "";
    }
    var CustomerName = jQuery.trim(jQuery('#txtCustomerName').val()); //客户姓名
    if (ID_Team == -1) {
        ShowSystemDialog("对不起，请选择团体！");
        return false;
    }
    if (ID_TeamTask == -1) {
        ShowSystemDialog("对不起，请选择任务！");
        return false;
    }
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { IDCard: IDCard, ID_Customer: ID_Customer, CustomerName: CustomerName, ID_Team: ID_Team, ID_TeamTask: ID_TeamTask, ID_TeamTaskGroup: ID_TeamTaskGroup, pageIndex: pageIndex, pageSize: pagePagination06.items_per_page, action: 'GetCustomerPagesInfo' },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            jQuery("#Pagination").show();
            if (tempOperPageCount == 0) {
                jQuery("#Pagination ul").pagination(msg.totalCount, optInit);
                tempOldtotalCount = msg.totalCount;
            }
            else if (tempOldtotalCount != msg.totalCount) {
                jQuery("#Pagination ul").pagination(msg.totalCount, optInit);
            }
            BindCustomerCustInfo(msg.dataList);

        }
    });
}
/********************分页绑定客户名单 Begin***********************/

/********************新增客户名单   Begin*************************/

//从Excel报表导入客户名单
function AddTeamCustomer() {
    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").length == 0) {
        ShowSystemDialog("对不起，团体任务分组信息不能为空，请先维护！");
        return false;
    }
    //    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").length == 0) {
    //        ShowSystemDialog("对不起，团体收费项目不能为空，请先维护！");
    //        return false;
    //    }
    //判断任务分组是否保存
    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体任务分组信息尚未保存，请先保存！");
        return false;
    }
    //判断收费项目是否保存
    if (jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体收费项目信息尚未保存，请先保存！");
        return false;
    }
    var TeamTaskID = jQuery('#idSelectTeamTask').val();
    this.ReturnValue = "";
    var aExcelFileInfo;
    var title = "选择文件";
    var url = "/System/Customer/TeamCustomerManageOper.aspx?TeamTaskGroupID=" + TeamTaskID + "&Path=KHMD&Time=" + new Date();
    art.dialog.open(url,
    {
        width: 360,
        height: 420,
        drag: false,
        lock: true,
        id: 'OperWindowFrame',
        title: title,
        cache: false,
        init: function () {
            jQuery(".aui_close").hide();
        },
        close: function () {
            aExcelFileInfo = art.dialog.data("TeamCustomerInfo");
            if (aExcelFileInfo == undefined || aExcelFileInfo == null || aExcelFileInfo.取消 == 1) {
                return true;
            }
            BindCustomerInfo(aExcelFileInfo);
        }
    });
}

function BindCustomerInfo(jsonData) {
    var ShowDataElementID = "tblTeamTaskGroupCustomerX";
    //读取客户名单模版
    var xustomerTask = new ReadTeamTaskTemplate("tblTemplateTeamTaskGroupCustomer", ShowDataElementID);
    var customerListTheadTempleteContent = xustomerTask.teamTaskListTheadTempleteContent;
    var customerListBodyTempleteContent = jQuery("#tblTemplateTeamTaskGroupCustomer").html(); //  xustomerTask.teamTaskListBodyTempleteContent;
    var htmlContent = "", tempContent = "", TempGender = "", TempMarried = "", Age = "", ID_TeamTaskGroup = "", TeamTaskGroupName = "", Gender = "", GenderName = "", Married = "", MarriageName = "", MinAge = "", MaxAge = "", RoleName = "", groupInfo = "", CurID_TeamTaskGroup = "", CurTeamTaskGroupName = "", imgSrc = "";
    var IsCompar = false, CurCustomerInfo = "";
    var item = jsonData;
    //加上当前列表中的客户信息
    var StrAllCustomerInfo = ",";
    jQuery("#tblTeamTaskGroupCustomerX tbody tr").each(function () {
        if (jQuery(this).attr("exist") != 1) {
            if (jQuery(this).find("td input[name='txtIDCard'").length > 0) {
                StrAllCustomerInfo += jQuery.trim(jQuery(this).find("td input[name='txtIDCard'").val()) + ",";
            }
            else if (jQuery(this).find("td label[name='txtIDCard'").length > 0) {
                StrAllCustomerInfo += jQuery.trim(jQuery(this).find("td label[name='txtIDCard'").text()) + ",";
            }
        }
    });
    if (customerListBodyTempleteContent != "") {
        //为空验证 Begin
        item.备注 = item.备注 == undefined ? "" : item.备注; //xmhuang 2013-10-28 excel新增备注字段导入功能
        item.身份证 = item.身份证 == undefined ? "" : item.身份证;
        item.出生日期 = item.出生日期 == undefined ? "" : item.出生日期;
        item.性别 = item.性别 == undefined ? "" : item.性别;
        item.婚姻 = item.婚姻 == undefined ? "" : item.婚姻;
        item.民族 = item.民族 == undefined ? "" : item.民族;
        item.联系电话 = item.联系电话 == undefined ? "" : item.联系电话;
        item.部门 = item.部门 == undefined ? "" : item.部门;
        item.二级部门 = item.二级部门 == undefined ? "" : item.二级部门;
        item.三级部门 = item.三级部门 == undefined ? "" : item.三级部门;
        item.四级部门 = item.四级部门 == undefined ? "" : item.四级部门;
        //为空验证 End

        //去除空格 Begin
        item.身份证 = jQuery.trim(item.身份证);
        item.出生日期 = jQuery.trim(item.出生日期);
        item.性别 = jQuery.trim(item.性别);
        item.婚姻 = jQuery.trim(item.婚姻);
        item.民族 = jQuery.trim(item.民族);
        item.联系电话 = jQuery.trim(item.联系电话);
        item.部门 = jQuery.trim(item.部门);
        item.二级部门 = jQuery.trim(item.二级部门);
        item.三级部门 = jQuery.trim(item.三级部门);
        item.四级部门 = jQuery.trim(item.四级部门);
        item.婚姻编号 = jQuery.trim(item.婚姻编号);
        //去除空格 End

        //判断是否已经包含身份证信息了
        if (StrAllCustomerInfo.search("," + item.身份证 + ",") == -1) {
            StrAllCustomerInfo += item.身份证 + ",";
            item.出生日期 = item.出生日期.replace("/", "-").replace("/", "-").replace("/", "-").replace("年", "-").replace("月", "-").replace("0:00:00", "").replace("日", ""); //.substr(0, 10);
            item.出生日期 = GetFormatDate(item.出生日期);
            item.性别 = jQuery.trim(item.性别);
            item.婚姻 = jQuery.trim(item.婚姻);
            var CurCustomerInfo = new GetUserInfoByIDCard(item.身份证);
            //性别验证 Begin
            if (item.性别 == "男") {
                TempGender = 1;
            }
            else if (item.性别 == "女") {
                TempGender = 0;
            }
            else {
                //没有填写性别则直接从身份证信息中读取性别
                if (CurCustomerInfo.sex != -1) {
                    TempGender = CurCustomerInfo.sex;
                    item.性别 = CurCustomerInfo.sexName;
                }
                else {
                    TempGender = -1; //未能获取到客户性别，这种情况是有身份证号码填写不正确造成
                }
            }
            //性别验证 End

            //出生日期 Begin
            if (item.出生日期 == "") {
                item.出生日期 = CurCustomerInfo.birthday;
            }
            //出生日期 End

            //            if (item.婚姻 == "已婚") {
            //                TempMarried = 1;
            //            }
            //            else if (item.婚姻 == "未婚") {
            //                TempMarried = 0;
            //            }
            //            else {
            //                TempMarried = -1;
            //            }
            if (item.头像 == undefined || item.头像 == "") {
                imgSrc = defalutImagSrc;
            }
            else {
                imgSrc = "data:image/gif;base64," + item.头像;
            }
            tempContent = jQuery(customerListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/@type="text"/gi, "")
                            .replace(/@exist/gi, "0")
                            .replace(/@ID_TeamTaskGroup/gi, "")
                            .replace(/@TeamTaskGroupName/gi, "")
                            .replace(/@CustomerName/gi, item.姓名 == undefined ? "" : item.姓名)
                            .replace(/@CustomerBirthDay/gi, item.出生日期 == undefined ? "" : item.出生日期)
                            .replace(/@CustomerRoleName/gi, item.角色 == undefined ? "" : item.角色)
                            .replace(/@IDCard/gi, item.身份证 == undefined ? "" : item.身份证)
                            .replace(/@TitleCustomerTel/gi, item.联系电话 == undefined ? "" : item.联系电话)
                            .replace(/@CustomerTel/gi, item.联系电话 == undefined ? "" : item.联系电话)
                            .replace(/@TitleDepartmentX/gi, item.部门 == undefined ? "" : item.部门)
                            .replace(/@TitleDepartmentA/gi, item.二级部门 == undefined ? "" : item.二级部门)
                            .replace(/@TitleDepartmentB/gi, item.三级部门 == undefined ? "" : item.三级部门)
                            .replace(/@TitleDepartmentC/gi, item.四级部门 == undefined ? "" : item.四级部门)

                            .replace(/@DepartmentX/gi, item.部门 == undefined ? "" : item.部门)
                            .replace(/@DepartmentA/gi, item.二级部门 == undefined ? "" : item.二级部门)
                            .replace(/@DepartmentB/gi, item.三级部门 == undefined ? "" : item.三级部门)
                            .replace(/@DepartmentC/gi, item.四级部门 == undefined ? "" : item.四级部门)

                            .replace(/@ErorrMessage/gi, "--")
                            .replace(/@ID_Customer/gi, "")
                            .replace(/@NationID/gi, item.民族ID == undefined ? "" : item.民族ID)
                            .replace(/@NationName/gi, item.民族 == undefined ? "" : item.民族)

                            .replace(/@CustomerSex/gi, item.性别 == undefined ? "" : item.性别)
                            .replace(/@ID_Gender/gi, TempGender)
                            .replace(/@CustomerMarried/gi, item.婚姻 == undefined ? "" : item.婚姻)
            //                            .replace(/@Is_Marriage/gi, TempMarried)
                            .replace(/@Note/gi, item.备注 == undefined ? "" : item.备注)
                            .replace(/@TitleNote/gi, item.备注)
                            .replace(/@Base64Photo/gi, item.头像)
                            .replace(/@imgSrc/gi, imgSrc)
                            .replace(/@Is_Marriage/gi, item.婚姻编号)

            //.replace(/@CustomerTel/gi, DisplayLitterLetter(item.联系电话, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
            //.replace(/@DepartmentX/gi, DisplayLitterLetter(item.部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
            //.replace(/@DepartmentA/gi, DisplayLitterLetter(item.二级部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
            //.replace(/@DepartmentB/gi, DisplayLitterLetter(item.三级部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
            //.replace(/@DepartmentC/gi, DisplayLitterLetter(item.四级部门, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419
            //.replace(/@Note/gi, DisplayLitterLetter(item.备注, 15, "..."))//联系方式显示值只显示12位 xmhuang 20140419

                            );
            //设置选中项
            jQuery(tempContent).find("td label[name='lblErorrMessage']").attr("title", "尚未分组！");
            jQuery(tempContent).find("td select[name='slCustomerSex'] option[value='" + TempGender + "']").attr("selected", true);
            jQuery(tempContent).find("td select[name='slCustomerMarried'] option[value='" + TempMarried + "']").attr("selected", true);

            //XMHuang 20130821 新增如果姓名和省份证为空则默认黄色标识改行数据
            if (item.身份证 == "" || item.姓名 == "") {
                jQuery(tempContent).addClass("ErrorMessageX");
                jQuery(tempContent).attr("title", "身份证或者姓名为空");
            }
            htmlContent += jQuery(tempContent)[0].outerHTML;
        } else {
            ShowSystemDialog("对不起，身份证号[" + item.身份证 + "]已存在！");
            return true;
        }
    }
    if (htmlContent != "") {
        jQuery('#' + ShowDataElementID + " tbody tr[id='loading']").hide();
        if (jQuery('#' + ShowDataElementID + ' tbody').length == 0) {
            jQuery('#' + ShowDataElementID).append('<tbody>' + htmlContent + '</tbody>');
        }
        else {
            jQuery('#' + ShowDataElementID + ' tbody').prepend(htmlContent);
        }
        htmlContent = "";
        jQuery('input[name="txtCustomerName"]').first().focus();
        ResetRowNum("tblTeamTaskGroupCustomerX");
        SetTableRowStyle(); //设置隔行样式 xmhuang 2014-04-18 
    }
}
/// <summary>
/// 获取当前团体、当前团体任务下的所有客户名单信息,返回所有存在的证件号,形式为：",511124198901032**0,"
/// 这里主要是证件号信息，用于排除同一证件号重复生成体检号的验证 xmhuang 2014-01-22
/// </summary>
function GetCustomerIDCard(IDCard) {
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();
    var AllCustomerInfo = GetCustomerByTeamAndTask(ID_Team, ID_TeamTask);
    var StrAllCustomerInfo = ","; //获取所有的身份证信息
    if (AllCustomerInfo.dataList != null) {
        jQuery(AllCustomerInfo.dataList).each(function (i, item) {
            StrAllCustomerInfo += jQuery.trim(item.IDCard) + ",";
        });
    }
    //加上当前列表中的客户信息
    jQuery("#tblTeamTaskGroupCustomerX tbody tr").each(function () {

        if (jQuery(this).attr("exist") != 1) {
            if (jQuery(this).find("td input[name='txtIDCard'").length > 0) {
                StrAllCustomerInfo += jQuery.trim(jQuery(this).find("td input[name='txtIDCard'").val()) + ",";
            }
            else if (jQuery(this).find("td label[name='txtIDCard'").length > 0) {
                StrAllCustomerInfo += jQuery.trim(jQuery(this).find("td label[name='txtIDCard'").text()) + ",";
            }
        }
    });
    return StrAllCustomerInfo.search("," + IDCard + ",") >= 0 ? true : false;
}
/********************新增客户名单   End  *************************/
//搜索收费项目 xmhuang 2014-03-28
function SearchBusFee() {
    var curEvent = window.event || e;
    if (curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40 || curEvent.keyCode == 13) {
        return false;
    }
    SelectedCustFee = "";
    jQuery("#showBusFeeItem tr[id!='loading'] td input:checked").each(function () {
        SelectedCustFee += jQuery(this).parent().parent().attr("id") + ",";
    });
    //判断是否有未保存的任务分组
    var teamTakGroupOfUnSaved = jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length;
    if (teamTakGroupOfUnSaved > 0) {
        ShowSystemDialog("对不起，有任务分组信息尚未保存，请先保存！");
        return false;
    }
    var ID_TeamGroup; //分组ID
    var TeamGroupName;
    var FeeWayName;
    var BusSet;
    var Gender;
    var GenderName;
    var data = jQuery("#tblTeamTaskGroupFee").data("data");
    if (data != undefined) {
        ID_TeamGroup = data.ID_TeamTaskGroup;
        TeamGroupName = data.TeamGroupName;
        FeeWayName = data.FeeWayName;
        BusSet = data.BusSet;
        Gender = data.Gender;
        GenderName = data.GenderName;
    }
    //通过Ajax请求获取数据（需要性别、套餐ID两个参数）
    var CustFeeID = "", SelectedFee = "", InputCode = jQuery.trim(jQuery("#txtSearch").val()); ; //获取当前所有套餐ID
    //查找非退费项目的收费项目ID
    jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").each(function (i, item) {
        CustFeeID += "'" + jQuery(this).attr("id_fee") + "',";
    });

    //提交后台处理
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxRegiste.aspx",
        cache: false,
        data: { action: "SearchBusFeeByCustFeeID",
            Gender: Gender,
            CustFeeID: CustFeeID,
            SelectedFee: SelectedFee,
            InputCode: InputCode,
            SelectedFee: SelectedCustFee
        },
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            BindCutomerBusFee(msg, ID_TeamGroup, TeamGroupName, FeeWayName, Gender, GenderName);
        }
    });
}
/*绑定客户名单
新增通过客户名称进行查询
*/
function GetAllTeamTaskGroup() {
    //判断任务分组是否保存
    if (jQuery("#tblTeamTaskGroupX tbody tr[id!='loading'][exist='0']").length > 0) {
        ShowSystemDialog("对不起，团体任务分组信息尚未保存，请先保存！");
        return false;
    }
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id!='loading']").remove();
    var AllTeamTaskGroupID = "";
    //获取选中的团体任务分组ID
    jQuery("#tblTeamTaskGroupX tbody tr[id!='loading']").each(function () {
        if (jQuery(this).find("td input[type='checkbox']").attr("checked")) {
            AllTeamTaskGroupID += jQuery(this).attr("id") + ",";
        }
    });
    if (AllTeamTaskGroupID == "TaskGroup" || AllTeamTaskGroupID == "") {
        ShowSystemDialog("请您勾选需要查看名单的分组!");
        return false;

    }
    else {
        jQuery("#txtCustomerName").val(""); DoSearch(); CommonShowHideDialog('showDialog');
    }
}
function GetTaskGroupFee(ID_TeamTaskGroup, TeamGroupName, FeeWayName, BusSet, Gender, GenderName) {
    if (ID_TeamTaskGroup != "") {
        var data = { ID_TeamTaskGroup: ID_TeamTaskGroup
, TeamGroupName: TeamGroupName
, FeeWayName: FeeWayName
, BusSet: BusSet
, Gender: Gender
, GenderName: GenderName
        };
        jQuery("#tblTeamTaskGroupFee").data("data", data);
        //绑定团体任务分组的收费项目信息
        BindTeamTaskGroupFeeData_Ajax("tblTeamTaskGroupFee", ID_TeamTaskGroup); CommonShowHideDialog('showDialogCustFee');
    }
    else {
        ShowSystemDialog("对不起未获取到分组信息，请重试!");
        return false;
    }

}
/// <summary>
/// 新增团体任务分组收费项目：必须保存分组信息、必须勾选一项且只为一项的分组做为新增收费项目的所属项目
///TemplateTeamTaskGroupID:模版ID
///ShowDataElementID:需要加载模版内容的目标ID
/// </summary>
function AddTeamTaskGroupFee(TemplateTeamTaskGroupFeeID, ShowDataElementID) {
    var data = jQuery("#tblTeamTaskGroupFee").data("data");
    //这里从下拉框中获取付费方式 Begin xmhuang 2014-04-13
    var FeeWay = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].value; //付费方式
    var FeeWayName = document.getElementById("selFeeWay").options[document.getElementById("selFeeWay").selectedIndex].text; //付费方式
    //这里从下拉框中获取付费方式 End   xmhuang 2014-04-13
    if (data != undefined) {
        var ID_TeamGroup = data.ID_TeamTaskGroup;
        var TeamGroupName = data.TeamGroupName;
        var FeeWayName = FeeWayName; //  data.FeeWayName;
        var BusSet = data.BusSet;
        var Gender = data.Gender;
        var GenderName = data.GenderName;
        //通过Ajax请求获取数据（需要性别、套餐ID两个参数）
        jQuery("#txtSearch").val(''); //设置关键字为空
        DoAddBusFee_Ajax(ShowDataElementID, ID_TeamGroup, TeamGroupName, FeeWayName, BusSet, Gender, GenderName, "", "");
    }

}
/************禁用或启用指定任务分组 Begin**************************************/
function DoPauseTeamTaskGroup(ID_TeamTaskGroup, IsPaused) {
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { ID_TeamTaskGroup: ID_TeamTaskGroup
        , IsPaused: IsPaused
        , action: 'PauseTeamTaskGroup'
        },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            ShowSystemDialog(msg.Message);
        }
    });
}

//判断任务分组是否可以禁用 xmhuang 20140507
function ISCanPauseTeamTaskGroup(ID_TeamTaskGroup) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanPauseTeamTaskGroup", ID_TeamTaskGroup: ID_TeamTaskGroup },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}
/*禁用/启用体检号
xmhuang 20140507 针对任务分组的禁检必须是该任务分组都是未完成体检的状态
*/
function PauseTeamTaskGroup(obj, ID_TeamTaskGroup) {
    //判断体检号是否满足格式
    if (!IsNum(ID_TeamTaskGroup)) {
        ShowSystemDialog("分组不存在无法进行禁用操作!");
        return false;
    }

    //判断选中状态
    var checkStatue = jQuery(obj).attr("checked");
    var isPaused = 0;
    var msg = "";
    var unCheckedStute = checkStatue;
    if (checkStatue == true || checkStatue == "checked") { msg = "禁用"; isPaused = 1; unCheckedStute = false; }
    else { msg = "启用"; isPaused = 0; unCheckedStute = true; }
    var msgContent = "您确认要" + msg + "分组[" + ID_TeamTaskGroup + "]吗?";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                jQuery(obj).attr("checked", unCheckedStute);
                return true;
            }, focus: true
        }, {
            name: '确定',
            callback: function () {
                DoPauseTeamTaskGroup(ID_TeamTaskGroup, isPaused);
                return true;
            }
        }]

    }).lock();
}
/**************禁用或启用指定任务分组 End**************************************/


/************禁用或启用指定客户 Begin**************************************/
function DoPauseCustomer(ID_Customer, IsPaused) {
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { ID_Customer: ID_Customer
        , IsPaused: IsPaused
        , action: 'PauseCustomer'
        },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            ShowSystemDialog(msg.Message);
        }
    });
}

//判断名单是否可以禁用，如果客户已经开始体检了不允许禁用 xmhuang 20140507
function ISCanPauseCustomer(ID_Customer) {
    var allISDeleteDT = ""; //当前团体所有不可用于删除的团体ID，团体任务，团体任务分组，体检号
    var flag = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { action: "ISCanPauseCustomer", ID_Customer: ID_Customer },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allISDeleteDT = msg;
        }
    });
    return allISDeleteDT;
}

/*禁用/启用体检号*/
function PauseCustomer(obj, ID_Customer) {
    //判断体检号是否满足格式
    if (!isCustomerExamNo(ID_Customer)) {
        ShowSystemDialog("体检号不存在无法进行禁用操作!");
        return false;
    }
    //判断选中状态
    var checkStatue = jQuery(obj).attr("checked");
    var unCheckedStute = checkStatue;
    var isPaused = 0;
    var msg = "";
    if (checkStatue == true || checkStatue == "checked") { msg = "禁用"; isPaused = 1; unCheckedStute = false; }
    else { msg = "启用"; isPaused = 0; unCheckedStute = true; }

    //判断任务分组是否可以禁用 xmhuang 20140507
    if (checkStatue) {
        var allISDeleteDT = ISCanPauseCustomer(ID_Customer);
        if (allISDeleteDT.success != 1) {
            if (allISDeleteDT.Message != undefined) {
                jQuery(obj).attr("checked", unCheckedStute);
                ShowSystemDialog(allISDeleteDT.Message);
                return false;
            }
        }
    }

    var msgContent = "您确认要" + msg + "体检号[" + ID_Customer + "]的客户吗?";
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: msgContent,
        button: [{
            name: '取消',
            callback: function () {
                jQuery(obj).attr("checked", unCheckedStute);
                return true;
            }, focus: true
        }, {
            name: '确定',
            callback: function () {
                DoPauseCustomer(ID_Customer, isPaused);
                return true;

            }
        }]

    }).lock();
}
/**************禁用或启用指定客户 End**************************************/

function CloseDialogCustFee(objID) {
    //手动关闭窗口则认为用户放弃操作，这里重置收费项目为空，则不会自动保存未保存的收费项目
    //jQuery("#tblTeamTaskGroupFee tbody tr[id!='loading']").attr("exist", "1");
    CommonShowHideDialog(objID);
}

/*
*滚动收费项目到底端 xmhuang 2014-04-15
*/
function ScollToTbCustFeeBottom() {
    var obj = document.getElementById("divTeamTaskGroupFee");
    obj.scrollTop = obj.scrollHeight;
}

/*
*是否具有禁用权限 xmhuang 2014-04-16
*/
function IsHaveCustomerPauseRight() {
    return Is_LoginUserRight('B.D.01.05_TeamTaskGroupCustomerPause');
}
function IsHaveTeamTaskGoupPauseRight() {
    return Is_LoginUserRight('B.D.01.04_TeamTaskGroupPause');
}
//弹出新增客户名单窗口
function OpenNewCustomer() {
    ResetCustomerInfo(); //重置弹出信息 xmhuang 20140424
    var dialog = art.dialog({
        lock: true, fixed: true, opacity: 0.3,
        id: "OpenNewCustomer",
        title: '新增客户名单',
        content: jQuery("#TeamCustomerAddDiv").html()
    }).lock();
}
function SureChoose(obj, dataList) {
    CloseDialog("OpenNewCustomer");
}
/// <summary>
/// 重置检索无结果显示的信息
/// </summary>
function ResetSearchInfo(msgInfo) {

    if (msgInfo == "" || msgInfo == undefined) {
        msgInfo = "在您查询的条件内，没有找到任何信息！";
    }
    var html = "<tr class='ParentMsg'><td class='msg' colSpan='16'>" + msgInfo + "</td></tr>";
    jQuery('#Searchresult').html(html); //设置无数据检索时显示提示信息
    SetTableRowStyle();
    jQuery("#Pagination").hide(); //隐藏分页控件
}

/*
*滚动收费项目到底端 xmhuang 2014-04-14
*/
function ScollToCustFeeBottom() {
    jQuery("#tblTeamTaskGroupFee").scrollTop(jQuery("#tblTeamTaskGroupFee")[0].scrollHeight);
}
/*
*滚动收费项目到底端 xmhuang 2014-04-14
*/
function ScollToCustomerListBottom() {
    jQuery("#tblTeamTaskGroupCustomerX").scrollTop(jQuery("#tblTeamTaskGroupCustomerX")[0].scrollHeight);
}
//修改客户名单 xmhuang 2014-05-13
//obj:为当前点击的tr对象 
//CustomerID:为客户体检号
function OpenModifyCustomer(obj, CustomerID) {
    if (obj == undefined || obj == null) {
        return false;
    }
    //    if (jQuery.trim(CustomerID) == "") {
    //        ShowSystemDialog("对不起,该客户尚未保存，不能进行编辑！");
    //        return false;
    //    }
    //判断当前是否存在未保存的项，存在则不允许修改
    if (CustomerID != "") {
        var IsCanModify = true;
        var NotCanModifyMsg = "";
        jQuery("#tblTeamTaskGroupCustomerX td label[name='lblID_Customer']").each(function () {
            if (jQuery.trim(jQuery(this).text()) == "") {
                IsCanModify = false
                return false;
            }
        });
        if (!IsCanModify) {
            ShowSystemDialog("对不起,列表中存在尚未保存的客户名单，不能进行编辑！");
            return false;
        }
        jQuery(".lblTitle").show();
        jQuery(".lblText").hide();
    }
    else {
        //如果体检号不为空则表明为修改，则需要设置lblTitle显示
        jQuery(".lblTitle").hide();
        jQuery(".lblText").show();
    }
    //获取点击行的值
    var CustomerName, IDCard, BirthDay, Gender, GenderName, Married, MarriedName, MobileNo, RoleName, Nation, NationName, Note, DepartmentX, DepartmentA, DepartmentB, DepartmentC;   //获取客户名称
    CustomerName = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerName']").text());    //获取客户名称
    IDCard = jQuery.trim(jQuery(obj).find("td label[name='txtIDCard']").text());                //获取证件号
    Gender = jQuery.trim(jQuery(obj).find("td label[name='slCustomerSex']").attr("id_gender")); //客户性别
    GenderName = jQuery.trim(jQuery(obj).find("td label[name='slCustomerSex']").text());        //客户性别
    Married = jQuery.trim(jQuery(obj).find("td label[name='slCustomerMarried']").attr("is_marriage")); //客户婚姻
    MarriedName = jQuery.trim(jQuery(obj).find("td label[name='slCustomerMarried']").text());        //客户婚姻
    RoleName = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerRoleName']").text());    //获取客户角色
    BirthDay = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerBirthDay']").text());    //获取客户出生日期
    Nation = jQuery.trim(jQuery(obj).find("td label[name='slNation']").attr("NationID"));    //获取客户名族
    NationName = jQuery.trim(jQuery(obj).find("td label[name='slNation']").text());    //获取客户名族
    MobileNo = jQuery.trim(jQuery(obj).find("td label[name='txtCustomerTel']").text());    //获取客户联系电话
    Note = jQuery.trim(jQuery(obj).find("td label[name='txtNote']").text());    //获取备注信息
    DepartmentX = jQuery.trim(jQuery(obj).find("td label[name='txtDepartment']").text());    //获取客户部门信息
    DepartmentA = jQuery.trim(jQuery(obj).find("td label[name='txtDepartmentA']").text());    //获取客户二级部门信息
    DepartmentB = jQuery.trim(jQuery(obj).find("td label[name='txtDepartmentB']").text());    //获取客户三级部门信息
    DepartmentC = jQuery.trim(jQuery(obj).find("td label[name='txtDepartmentC']").text());    //获取客户四级部门信息

    var dialog = art.dialog({
        esc: false,
        lock: true, fixed: true, opacity: 0.3,
        id: "OpenNewCustomer",
        title: '修改客户[' + CustomerName + ']基本信息',
        content: jQuery("#TeamCustomerModifyDiv").html(),
        init: function () {
            jQuery("#txtModifyIDCard").val(IDCard);
            jQuery("#txtModifyCustomerName").val(CustomerName);
            jQuery("#txtModifyBirthDay").val(BirthDay);
            jQuery("#txtModifyMobileNo").val(MobileNo);
            jQuery("#txtModifyRole").val(RoleName);
            jQuery("#txtModifyDepartment").val(DepartmentX);
            jQuery("#txtModifyDepartSubA").val(DepartmentA);
            jQuery("#txtModifyDepartSubB").val(DepartmentB);
            jQuery("#txtModifyDepartSubC").val(DepartmentC);
            jQuery("#txtModifyNote").val(Note);

            jQuery("#lblModifyIDCard").text(IDCard);
            jQuery("#lblModifyIDCard").attr("title", IDCard);
            jQuery("#lblModifyCustomerName").text(CustomerName);
            jQuery("#lblModifyCustomerName").attr("title", CustomerName);
            jQuery("#lblModifyBirthDay").text(BirthDay);
            jQuery("#lblModifyBirthDay").attr("title", BirthDay);
            jQuery("#lblModifyMobileNo").text(MobileNo);
            jQuery("#lblModifyMobileNo").attr("title", MobileNo);
            jQuery("#lblModifyRole").text(RoleName);
            jQuery("#lblModifyRole").attr("title", RoleName);
            jQuery("#lblModifyDepartment").text(DepartmentX);
            jQuery("#lblModifyDepartment").attr("title", DepartmentX);
            jQuery("#lblModifyDepartSubA").text(DepartmentA);
            jQuery("#lblModifyDepartSubA").attr("title", DepartmentA);
            jQuery("#lblModifyDepartSubB").text(DepartmentB);
            jQuery("#lblModifyDepartSubB").attr("title", DepartmentB);
            jQuery("#lblModifyDepartSubC").text(DepartmentC);
            jQuery("#lblModifyDepartSubC").attr("title", DepartmentC);
            jQuery("#lblModifyNote").text(Note);
            jQuery("#lblModifyNote").attr("title", Note);
            jQuery("#lblModifyNation").text(NationName);
            jQuery("#lblModifyNation").attr("title", NationName);

            //设置性别
            if (GenderName == "男") {
                jQuery("[name='slModifyCustomerSex'] [value='1']").attr("selected", true);
            }
            else {
                jQuery("[name='slModifyCustomerSex'] [value='2']").attr("selected", true);
            }
            jQuery("#lblModifyCustomerSex").text(GenderName);
            //设置婚姻
            jQuery("#slModifyCustomerMarriage [value='" + Married + "']").attr("selected", true);
            jQuery("#nationModifyTemplate").html(jQuery("#nationParentTemplate").html());
            //设置名族
            ShowQuickSelectNation(parseFloat(Nation), NationArray[Nation - 1]);

            //设置用户名查询值,在点击保存后后默认查询此人的数据信息
            jQuery("#txtCustomerName").val(CustomerName);
            jQuery("#txtCustomerName").data("IsAdd", 1);
            var CustomerInfo = { obj: obj, CustomerID: CustomerID };         //设置当前原始编辑对象属性和值
            jQuery("#TeamCustomerModifyDiv").data("CustomerInfo", CustomerInfo);

            //设置空间的操作性
            SetControlDisabled(CustomerID == "" ? false : true);
        }
    }).lock();
}
function SetControlDisabled(Disabled) {
    if (Disabled) {
        jQuery("#txtModifyIDCard").attr("disabled", "disabled");
        jQuery("#txtModifyCustomerName").attr("disabled", "disabled");
        jQuery("#slModifyCustomerSex").attr("disabled", "disabled");
        jQuery("#slModifyCustomerMarriage").attr("disabled", "disabled");
        jQuery("#txtModifyBirthDay").attr("disabled", "disabled");
        jQuery("#txtModifyRole").attr("disabled", "disabled");
        //jQuery("#txtModifyMobileNo").attr("disabled", "disabled");
        jQuery("#txtModifyNote").attr("disabled", "disabled");

    }
    else {
        jQuery("#txtModifyIDCard").removeAttr("disabled");
        jQuery("#txtModifyCustomerName").removeAttr("disabled");
        jQuery("#slModifyCustomerSex").removeAttr("disabled");
        jQuery("#slModifyCustomerMarriage").removeAttr("disabled");
        jQuery("#txtModifyBirthDay").removeAttr("disabled");
        jQuery("#txtModifyRole").removeAttr("disabled");
        //jQuery("#txtModifyMobileNo").removeAttr("disabled");
        jQuery("#txtModifyNote").removeAttr("disabled");
    }
}
//验证客户身份证号是否重复 xmhuang 2014-05-14
function CheckModifyCustomerIDCard(obj, CurIDCard, CustomerID) {
    if (CurIDCard == "" || CurIDCard == null || CurIDCard == undefined)
        return false;
    //读取当前任务的所有人员名单进行对比，是否已经包含
    var ID_Team = jQuery('#idSelectTeam').val();
    var ID_TeamTask = jQuery('#idSelectTeamTask').val();

    //从当前列表中判断是否有相同的身份证号
    var TempCurIDCard = "", Value = 0, flag = false;
    jQuery("#tblTeamTaskGroupCustomerX tbody tr[id!='loading']").each(function () {
        TempCurIDCard = "";
        if (jQuery(this).find("td input[name='txtIDCard'").length > 0) {
            TempCurIDCard = jQuery.trim(jQuery(this).find("td input[name='txtIDCard'").val());
        }
        else if (jQuery(this).find("td label[name='txtIDCard'").length > 0) {
            TempCurIDCard = jQuery.trim(jQuery(this).find("td label[name='txtIDCard'").text());
        }
        //如果证件号相同，并且不是相同对象，则证明该证件号重号
        if (TempCurIDCard == CurIDCard && jQuery(this).attr("id") != jQuery(obj).attr("id")) {
            Value++;
        }
        if (Value > 0) {
            flag = true;
            ShowSystemDialog("列表中已经包含[" + CurIDCard + "]的客户名单，请更改！");
            //            jQuery(obj).focus();
            //            jQuery(obj).select();
            return false;
        }
    });
    //表示在当前列表中不包含该身份证号码，则需要从数据库中检索是否存在相同的身份证
    if (!flag) {
        var count = 0;
        var AllCustomerInfo = GetCustomerByTeamAndTask(ID_Team, ID_TeamTask);
        if (AllCustomerInfo.dataList != null) {
            jQuery(AllCustomerInfo.dataList).each(function (i, item) {
                if (item.IDCard == CurIDCard && item.ID_Customer != CustomerID) {
                    count++;
                }
            });
            if (count > 0) {
                ShowSystemDialog("当前任务已经包含[" + CurIDCard + "]的客户名单，请更改！");
                //                jQuery(obj).focus();
                //                jQuery(obj).select();
                return false;
            }
        }
    }
    return true;
}

//执行保存，并关闭
function SaveModifyCustomer(IsSave) {
    //获取客户基本信息,如果客户体检号不存在，则直接设置当前值，不进行重新重新
    if (IsSave == 1) {
        jQuery("#txtCustomerName").data("IsAdd", 0);
        var CustomerInfo = jQuery("#TeamCustomerModifyDiv").data("CustomerInfo");
        var ModifyObj = CustomerInfo.obj;
        var CustomerID = CustomerInfo.CustomerID;
        //绑定编辑后的值
        var ModifyIDCard = jQuery.trim(jQuery("#txtModifyIDCard").val());               //修改后的客户证件号码
        //判断是否是正确的证件号码
        if (!isIDCardNo(ModifyIDCard)) {
            ShowCallBackSystemDialog("对不起，请输入正确的证件号码！", function () {
                jQuery("#txtModifyIDCard").focus();
                jQuery("#txtModifyIDCard").select();
            });
            return false;
        }
        //判断证件号是否已经存在
        if (!CheckModifyCustomerIDCard(ModifyObj, ModifyIDCard, CustomerID)) {
            return false;
        }
        var ModifyCustomerName = jQuery.trim(jQuery("#txtModifyCustomerName").val());   //修改后的客户名称
        var ModifyCustomerSex = document.getElementById("slModifyCustomerSex").options[document.getElementById("slModifyCustomerSex").selectedIndex].value; //修改后的客户性别
        var ModifyCustomerSexName = document.getElementById("slModifyCustomerSex").options[document.getElementById("slModifyCustomerSex").selectedIndex].text; //修改后的客户性别
        var ModifyCustomerMarriage = document.getElementById("slModifyCustomerMarriage").options[document.getElementById("slModifyCustomerMarriage").selectedIndex].value; //修改后的客户婚姻
        var ModifyCustomerMarriageName = document.getElementById("slModifyCustomerMarriage").options[document.getElementById("slModifyCustomerMarriage").selectedIndex].text; //修改后的客户婚姻
        var ModifyNation = jQuery("#idSelectNation").val();               //民族ID
        ModifyNation = jQuery.trim(ModifyNation);
        var ModifyNationName = jQuery("#nameSelectNation").val();         //民族
        ModifyNationName = jQuery.trim(ModifyNationName);
        var ModifyBirthDay = jQuery.trim(jQuery("#txtModifyBirthDay").val());               //修改后的客户出生日期

        if (ModifyBirthDay == "") {
            ShowCallBackSystemDialog("对不起，客户出生日期不允许为空！", function () {
                jQuery("#txtModifyBirthDay").focus();
                jQuery("#txtModifyBirthDay").select();
            });
            return false;
        }
        //验证出生日期的有效性 xmhuang 2015-04-07
        if (!isDate(ModifyBirthDay)) {
            ShowCallBackSystemDialog("对不起，客户出生日期格式不正确!", jQuery("#txtModifyBirthDay"));
            return false;
        }
        var ModifyMobileNo = jQuery.trim(jQuery("#txtModifyMobileNo").val());               //修改后的客户联系电话
        if (ModifyMobileNo == "") {
            ShowCallBackSystemDialog("对不起，联系电话不允许为空！", function () {
                jQuery("#txtModifyMobileNo").focus();
                jQuery("#txtModifyMobileNo").select();
            });
            return false;
        }

        var ModifyRole = jQuery.trim(jQuery("#txtModifyRole").val());                       //修改后的客户角色
        var ModifyDepartment = jQuery.trim(jQuery("#txtModifyDepartment").val());           //修改后的客户部门
        var ModifyDepartSubA = jQuery.trim(jQuery("#txtModifyDepartSubA").val());           //修改后的客户二级部门
        var ModifyDepartSubB = jQuery.trim(jQuery("#txtModifyDepartSubB").val());           //修改后的客户三级部门
        var ModifyDepartSubC = jQuery.trim(jQuery("#txtModifyDepartSubC").val());           //修改后的客户四级部门
        var ModifyNote = jQuery.trim(jQuery("#txtModifyNote").val());                       //修改后的客户备注
        if (CustomerID == "") {
            //设置列表项目
            jQuery(ModifyObj).find("td label[name='txtCustomerName']").text(ModifyCustomerName);                          //设置客户名称
            jQuery(ModifyObj).find("td label[name='txtIDCard']").text(ModifyIDCard);                                      //设置客户证件号
            jQuery(ModifyObj).find("td label[name='slCustomerSex']").attr("id_gender", ModifyCustomerSex);                //设置客户性别
            jQuery(ModifyObj).find("td label[name='slCustomerSex']").text(ModifyCustomerSexName);                         //设置客户性别
            jQuery(ModifyObj).find("td label[name='slCustomerMarried']").attr("is_marriage", ModifyCustomerMarriage);     //设置客户婚姻
            jQuery(ModifyObj).find("td label[name='slCustomerMarried']").text(ModifyCustomerMarriageName);                //设置客户婚姻
            jQuery(ModifyObj).find("td label[name='slNation']").attr("NationID", ModifyNation);                          //设置客户名族
            jQuery(ModifyObj).find("td label[name='slNation']").text(ModifyNationName);                                   //设置客户名族
            jQuery(ModifyObj).find("td label[name='txtCustomerBirthDay']").text(ModifyBirthDay);                          //设置客户出生日期
            jQuery(ModifyObj).find("td label[name='txtCustomerTel']").text(ModifyMobileNo);                               //设置客户联系电话
            jQuery(ModifyObj).find("td label[name='txtCustomerRoleName']").text(ModifyRole);                              //设置客户角色
            jQuery(ModifyObj).find("td label[name='txtDepartment']").text(ModifyDepartment);                              //设置客户部门信息
            jQuery(ModifyObj).find("td label[name='txtDepartmentA']").text(ModifyDepartSubA);                             //设置客户二级部门信息
            jQuery(ModifyObj).find("td label[name='txtDepartmentB']").text(ModifyDepartSubB);                             //设置客户三级部门信息
            jQuery(ModifyObj).find("td label[name='txtDepartmentC']").text(ModifyDepartSubC);                             //设置客户四级部门信息
            jQuery(ModifyObj).find("td label[name='txtNote']").text(ModifyNote);                                          //设置客户备注信息

            //设置截断显示值
            jQuery(ModifyObj).find("td label[name='txtCustomerName']").prev("div[class='nowrap']").text(ModifyCustomerName);
            jQuery(ModifyObj).find("td label[name='txtCustomerName']").parent().attr("title", ModifyCustomerName);
            jQuery(ModifyObj).find("td label[name='txtCustomerTel']").prev("div[class='nowrap']").text(ModifyMobileNo);
            jQuery(ModifyObj).find("td label[name='txtCustomerTel']").parent().attr("title", ModifyMobileNo);
            jQuery(ModifyObj).find("td label[name='txtCustomerRoleName']").prev("div[class='nowrap']").text(ModifyRole);
            jQuery(ModifyObj).find("td label[name='txtCustomerRoleName']").parent().attr("title", ModifyRole);
            jQuery(ModifyObj).find("td label[name='txtDepartment']").prev("div[class='nowrap']").text(ModifyDepartment);
            jQuery(ModifyObj).find("td label[name='txtDepartment']").parent().attr("title", ModifyDepartment);
            jQuery(ModifyObj).find("td label[name='txtDepartmentA']").prev("div[class='nowrap']").text(ModifyDepartSubA);
            jQuery(ModifyObj).find("td label[name='txtDepartmentA']").parent().attr("title", ModifyDepartSubA);
            jQuery(ModifyObj).find("td label[name='txtDepartmentB']").prev("div[class='nowrap']").text(ModifyDepartSubB);
            jQuery(ModifyObj).find("td label[name='txtDepartmentB']").parent().attr("title", ModifyDepartSubB);
            jQuery(ModifyObj).find("td label[name='txtDepartmentC']").prev("div[class='nowrap']").text(ModifyDepartSubC);
            jQuery(ModifyObj).find("td label[name='txtDepartmentC']").parent().attr("title", ModifyDepartSubC);
            jQuery(ModifyObj).find("td label[name='txtNote']").prev("div[class='nowrap']").text(ModifyNote);
            jQuery(ModifyObj).find("td label[name='txtNote']").parent().attr("title", ModifyNote);
            jQuery("#TeamCustomerModifyDiv").data("CustomerInfo", "");                          //重置Data属性
            CloseDialog("OpenNewCustomer");
        }
        else {
            jQuery("#TeamCustomerModifyDiv").data("CustomerInfo", "");                          //重置Data属性
            CloseDialog("OpenNewCustomer");
            jQuery("#txtCustomerName").val(ModifyCustomerName);
            //这里执行保存
            var qustData = {
                CustomerID: CustomerID,
                ModifyIDCard: ModifyIDCard,
                ModifyCustomerName: ModifyCustomerName,
                ModifyCustomerSex: ModifyCustomerSex,
                ModifyCustomerSexName: ModifyCustomerSexName,
                ModifyCustomerMarriage: ModifyCustomerMarriage,
                ModifyCustomerMarriageName: ModifyCustomerMarriageName,
                ModifyNation: ModifyNation,
                ModifyNationName: ModifyNationName,
                ModifyBirthDay: ModifyBirthDay,
                ModifyMobileNo: ModifyMobileNo,
                ModifyRole: ModifyRole,
                ModifyDepartment: ModifyDepartment,
                ModifyDepartSubA: ModifyDepartSubA,
                ModifyDepartSubB: ModifyDepartSubB,
                ModifyDepartSubC: ModifyDepartSubC,
                ModifyNote: ModifyNote,
                action: "SaveModifyCustomer"
            };

            art.dialog({
                lock: true, fixed: true, opacity: 0.3,
                content: "您确定要保存吗？",
                icon: 'info',
                cancelVal: '取消',
                cancel: true, //为true等价于function(){}
                okVal: "保存", //为true等价于function(){}
                ok: function () {
                    //这里执行Ajax请求保存客户基本信息
                    //存储大数据请设置Content-length值
                    jQuery.ajax({
                        type: "POST",
                        url: "/Ajax/AjaxTeamOper.aspx",
                        data: qustData,
                        cache: false,
                        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
                        dataType: "json",
                        success: function (msg) {
                            if (msg.success == 0) {
                                ShowSystemDialog(msg.Message);
                                return false;
                            }
                            else if (msg.success == 1) {
                                DoSearch(); //查询客户信息
                                return true;
                            }
                        },
                        complete: function () {
                        }
                    });
                }
            });
        }
    }
    else {
        //取消
        if (jQuery("#txtCustomerName").data("IsAdd") == 1) {
            jQuery("#txtCustomerName").val("");
        }
        jQuery("#TeamCustomerModifyDiv").data("CustomerInfo", "");                          //重置Data属性
        CloseDialog("OpenNewCustomer");
    }
}