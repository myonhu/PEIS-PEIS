﻿parent.userIsLogin();
$(function () {
    if (typeof (quickSearchBoxData) != "undefined") {
        QuickSearchBox(quickSearchBoxData);
        $(".bread", parent.document).show();
        $(".leftnav", parent.document).css("top", "120px");
        $(".admin", parent.document).css("top", "120px");
    } else {
        //检查导航条状态决定隐藏还是显示
            $(".bread", parent.document).hide();
            $(".leftnav", parent.document).css("top", "70px");
            $(".admin", parent.document).css("top", "70px");
    }
    var biTianBiaoZhi = '<span class="biTian">*</span>';
    $(".biTianXiang").before(biTianBiaoZhi);
    //加时间控件
    $(".dateselect").flatpickr();
    $(".datemaxselect").flatpickr({ maxDate: "today" });
    $(".datemin40select").flatpickr({ minDate: beforeDay(40), maxDate: "today" });

    //var href = $("#iframe", parent.document).attr("src");
    //if (href.indexOf("&") > 0) {
    //    //不止有一个参数
    //     href = href.split("&")[0];
    //}
    //var sonSectionNum = $(".leftnav ul li a", parent.document).length;
    //var secondLi = $(".leftnav ul li a", parent.document);
    //for (var i = 0; i < sonSectionNum; i++) {
    //    var dizhi = secondLi[i].href.split(":")[2].substr(4);
    //    if (dizhi.indexOf("&") > 0) {
    //        //不止有一个参数
    //        var dizhishort = dizhi.split("&")[0];
    //        var isNum = compare(href, dizhishort)
    //    } else {
    //        var isNum = compare(href, dizhi)
    //    }
    //    if (isNum === 100) {
    //        $(".leftnav ul li a", parent.document).removeClass("on");
    //        $(".leftnav ul li a[dizhi=" + dizhi + "]", parent.document).addClass("on");
    //        break;
    //    }
    //}
})

//搜索栏生成操作
//quickSearchBoxData:[{type:(添加的元素的类型  带搜索按钮的为0，下拉框为1,按钮为2，单选框为3),inputtype:输入框类型，placeholder：placeholder，name：name，btnbgcolor：背景颜色，function：触发的方法，tip：输入提示,title：下拉框题目，classname：下拉框类，btnbgcolor：按钮背景色,icon:按钮图标}]
function QuickSearchBox(quickSearchBoxData) {
    var quickSearchBoxDom = "";
    //var quickSearchBoxDatalength = quickSearchBoxData.length
    $.each(quickSearchBoxData, function (n, items) {
        var a = items.type
        switch (a) {
            //搜索框 inputtype:
            case 0:
                quickSearchBoxDom += '<li><input id="' + items.inputid + '" type="' + items.inputtype + '" placeholder="' + items.placeholder + '" name="' + items.name + '" class="input searchbox"/><a id="' + items.btnid + '" href="javascript:void(0)" class="button button-little icon-search ' + items.btnbgcolor + '" onclick="' + items.function + '"> ' + items.tip + '</a></li>';
                break;
            case 1:
                items.title ? quickSearchBoxDom += '<li>&nbsp;&nbsp;' + items.title + '' : quickSearchBoxDom += '<li>';
                quickSearchBoxDom += ' <select name="' + items.name + '" class="input ' + items.classname + '" onchange="' + items.function + '"><option value="">' + items.tip + '</option>';
                for (var j = 0; j < items.option.length; j++) {
                    quickSearchBoxDom += '<option value="' + items.option[j].value + '">' + items.option[j].name + '</option>';
                }
                quickSearchBoxDom += ' </select></li>';
                break;
            case 2:
                quickSearchBoxDom += '<li class="searchbtn"><a id=" ' + items.btnid + '" href="javascript:void(0)" name=" ' + items.btnname + '" class="button button-little   ' + items.icon + ' ' + items.btnbgcolor + '" onclick="' + items.function + '"> ' + items.tip + '</a></li>';
                break;
            case 3:
                quickSearchBoxDom += '<input type="checkbox" name=" ' + items.name + '" value="" />';
                break;
        }
    })
    $(".bread", parent.document).html(quickSearchBoxDom);
}
//QuickSearchBox(quickSearchBoxData);

// 标记是否已经显示登录框
var isShowLoginDialog = 0;

function SetToolMenuXY() {
    // 弹出框的背景在浏览器缩小变大时自适应
    if (!jQuery("#ShowHideDialogbg").is(":hidden")) {
        document.getElementById("ShowHideDialogbg").style.height = document.body.scrollHeight + "px";
        document.getElementById("ShowHideDialogbg").style.width = document.body.scrollWidth + "px";
    }

    var clientWidth = document.documentElement.clientWidth;
    if (clientWidth > 1020) {
        document.getElementById("RightTool").style.right = ((document.body.offsetWidth - 980) / 2 - 5 - document.getElementById("RightTool").offsetWidth) + "px";
    } else {
        document.getElementById("RightTool").style.right = "0px";
    }
    document.getElementById("RightTool").style.top = (document.documentElement.clientHeight - document.getElementById("RightTool").offsetHeight) / 2 + "px";
}

//关闭隐藏弹出框
function ShowHideDialog() {
    if (jQuery("#divShowDialog").is(":hidden")) {
        document.body.style.overflow = "hidden";
        jQuery("#divShowDialog").show();
        document.getElementById("ShowHideDialogbg").style.height = document.body.scrollHeight + "px";
        document.getElementById("ShowHideDialogbg").style.width = document.body.scrollWidth + "px";
        document.getElementById("ShowHideDialogbg").style.display = "block";
    } else {
        document.body.style.overflow = "auto";
        jQuery("#divShowDialog").hide();
        document.getElementById("ShowHideDialogbg").style.display = 'none';
    }
}

//关闭隐藏弹出框
function CommonShowHideDialog(DialogID) {
    if (jQuery("#" + DialogID).is(":hidden")) {
        document.body.style.overflow = "hidden";
        SetDivToScreenCenter(DialogID);
        jQuery("#" + DialogID).show();
        //document.getElementById(DialogID).style.top = "10%";
        $("#ShowHideDialogbg").css({ "height": document.body.scrollHeight + "px" });
        $("#ShowHideDialogbg").css({"width":document.body.scrollWidth + "px"});

        $("#ShowHideDialogbg").css({ "display": "block" });
    } else {
        document.body.style.overflow = "auto";
        jQuery("#" + DialogID).hide();
        $("#ShowHideDialogbg").css({ "display": "none" });
    }
}

jQuery(document).ready(function () {

    // 获取用户的菜单列表,权限列表，分检科室列表 (用于显示菜单项，判断用户权限)
    GetUserMenuRightSectionList_New();

    //SetSideFloatXY(); // 计算侧边栏的坐标

    var IsLogin = jQuery("#CookieIsLogin").val();
    var SessionID = jQuery("#CookieSessionID").val();
    var LoginName = jQuery("#CookieLoginName").val();
    var UserName = jQuery("#CookieUserName").val();
    if (LoginName != "") {
        if (IsLogin == 1)//表示用户是来之登录界面，向其他站内成员发送上线消息
        {
            Wait(LoginName, UserName, "LOGIN", "INFO"); //发送上线通知消息
        }
        else//表示用户来之首页,开启站内消息等待
        {
            Wait(LoginName, UserName, "-1", "INFO"); //开启消息接收
        }
    }
    // SetContentLoadFormHeight(); // 设置加载区域的高度
    setInterval(function () { jQuery(".CurrentTime").html(GetCurrentTime) }, 3000); // 显示当前时间

    // 检测是否其它用户登录，或登录超时
   // setInterval("CheckUserIsLoginState()", 60 * 1000); // 显示当前时间

    document.onmousewheel = function () {
        if (event.ctrlKey) {
            return false;
        }
    }

});
var IsShowDelayMessage = false;
var gLoadUrl = "";
/// <summary>
/// 加载子页面
/// </summary>
function DoLoad(href, parentContainer) {
    // 页面调整前，先判断是否已经登录，如果未登录，则弹出提示框进行提示。 20140429
    gLoadUrl = href;
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxUser.aspx",
        data: { action: 'JudgeUserIsLogin',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        success: function (msg) {
            if (msg == "-9999") {
                ReLoginConfirm(); // 提示重新登录
            } else {

                // 判断是否任然是当前进行的登录
                var retCurrentUserMsg = JudgeISCurrentUserLogin();

                if (retCurrentUserMsg == "-9999" || retCurrentUserMsg == "-9998") {
                    ReLoginConfirm();
                }
                //else if (retCurrentUserMsg.indexOf("服务器许可") > -1 && IsShowDelayMessage == false) {
                //    IsShowDelayMessage = true;
                //    ShowSystemDialog(retCurrentUserMsg);
                //    DoLoadSubPage(gLoadUrl, parentContainer);
                //}
                //else {
                //    DoLoadSubPage(gLoadUrl, parentContainer);
                //}

            }
        }
    });
}
/// <summary>
/// 加载子页面
/// </summary>
function DoLoadSubPage(href, parentContainer) {
    SetUserRedirectUrl(href); // 每次加载前，先保存当前地址，用于在刷新页面时，页面进行自动加载 
    ShowSubTitleName(); // 显示二级菜单的名称

    if (href == "" || href == undefined) {
        href = "/System/Welcome.aspx";
    }
    if (jQuery("#LastLoadSubPage").val() == href && href == "/System/Welcome.aspx") {
        jQuery(".j-autoHeight").autoHeight(); // 自适应高度
        return;
    }
    // 点击时，隐藏二级菜单
    if (href != "") {
        jQuery(".LoginUserMenuClassSubItem").hide();
    }
    jQuery("#LastLoadSubPage").val(href); // 记录最近一次加载的子页面地址

    if (parentContainer == '' || parentContainer == undefined)
        parentContainer = 'loadForm';

    // 隐藏侧边栏按钮和连接 
    if (parentContainer == "loadForm") {
        jQuery("#divSectionExamedCount").hide();    // 隐藏已检查科室链接
        jQuery("#divHistoryExamCount").hide();      // 隐藏历史检查次数链接
        jQuery("#divSectionExamFloat").hide();      // 隐藏切换客户科室链接
        jQuery("#divFinalExamCompare").hide();      // 隐藏总检对比链接
        jQuery("#divCloneRightFloat").hide();       // 隐藏克隆链接  添加 

        jQuery("#listSectionExamedCount").hide();   // 隐藏已检查科室列表
        jQuery("#listSectionExamFloat").hide();     // 隐藏客户科室列表
        jQuery("#listHistoryExamCount").hide();     // 隐藏历史体检号对比列表
        jQuery("#listFinalExamCompare").hide();     // 隐藏总检对比列表
        jQuery("#listCloneRightFloat").hide();      // 隐藏克隆链接

        SetSideFloatXY(); // 计算侧边栏的坐标
    }


    var date = GetQueryString('date');
    if (date == null || date == "" || date == undefined) {
        if (href.indexOf("?") >= 0) {
            href = href + "&date=" + encodeURIComponent(new Date());
        }
        else {
            href = href + "?date=" + encodeURIComponent(new Date());
        }
    }

    DisposeCamera(); //在加载页面之前判断是否开启视频资源，有则关闭
    DisposeVideoCapture(); //在加载页面之前判断是否开启采集卡资源，有则关闭
    jQuery("#" + parentContainer).empty();

    // SwitchHeader(1); // 先回到正常头部显示状态

    jQuery("#" + parentContainer).load(href, { limit: 200, cache: false }, function () {
        //ShowSystemDialog("The last 25 entries in the feed have been loaded");

        // SetContentLoadFormHeight(); // 设置加载区域的高度
    });
    jQuery("#" + parentContainer).ajaxSend(function () {
        ShowLoadingProcessBarDefault(1);
    });

    jQuery("#" + parentContainer).ajaxStart(function () {
        ShowLoadingProcessBarDefault(1);
    });

    jQuery("#" + parentContainer).ajaxComplete(function () {
        ShowLoadingProcessBarDefault(0);
    });


    // GetCustomerInfo();
}

function GetCustomerInfo() {
    jQuery('title').val('身份证：' + jQuery("#txtSFZ").val());
}

function ShowAllLoginUserMenu(ClassID) {
    jQuery("#LoginUserMenuFrame_" + ClassID).attr("style", "background-color:White;overflow:hidden; width:984px; ");
    jQuery("#ShowAllLoginUserMenu_" + ClassID).hide();
    jQuery("#HideLoginUserMoreMenu_" + ClassID).show();
}

function HideLoginUserMoreMenu(ClassID) {
    jQuery("#LoginUserMenuFrame_" + ClassID).attr("style", "max-height:108px; overflow:auto; background-color:White; ");
    jQuery("#ShowAllLoginUserMenu_" + ClassID).show();
    jQuery("#HideLoginUserMoreMenu_" + ClassID).hide();
}



/// <summary>
/// 弹出用户密码修改页面
/// </summary>
function OpenUserPasswordModifyWindow() {

    var tipscontent = '<table class="ModifyPassword">' +
        '<tbody>' +
        '    <tr><td class="left">原密码：</td><td><input maxlength="30" type="password" name="txtOldPwd" id="txtOldPwd" /> &nbsp;</td></tr>' +
        '    <tr><td class="left">新密码：</td><td><input maxlength="30" type="password" name="txtNewPwd" id="txtNewPwd" /> &nbsp;</td></tr>' +
        '    <tr><td class="left">重复新密码：</td><td><input maxlength="30" type="password" name="txtRepeatOldPwd" id="txtRepeatOldPwd" /> &nbsp;</td></tr>' +
        '    <tr><td  style="text-align:center;" class="msg" colspan="2" id="modifyPasswordMsg">&nbsp;</td></tr>' +
        '</tbody>' +
        '</table>';

    art.dialog({
        id: 'OperWindowFrame',
        content: tipscontent,
        lock: true,
        fixed: true,
        opacity: 0.3,
        zIndex: 300,
        title: '用户口令修改',
        button: [{
            name: '取消'
        }, {
            name: '确定',
            callback: function () {
                var bModifyState = ModifyUserPasswordInfo(); // 修改用户的密码。
                return false;
            }, focus: true
        }]
    });

}


/// <summary>
/// 修改用户的密码。
/// </summary>
function ModifyUserPasswordInfo() {
    ShowModifyPasswordMsg("");
    var OldPassword = jQuery("#txtOldPwd").val();
    var NewPassword = jQuery("#txtNewPwd").val();
    var RepeatOldPassword = jQuery("#txtRepeatOldPwd").val();

    if (OldPassword == "") {
        jQuery("#txtOldPwd").focus();
        jQuery("#txtOldPwd").select();
        ShowModifyPasswordMsg("请输入原密码！");
        return false;
    }

    if (NewPassword == "") {
        jQuery("#txtNewPwd").focus();
        jQuery("#txtNewPwd").select();
        ShowModifyPasswordMsg("请输入新密码！");
        return false;
    }

    if (RepeatOldPassword == "") {
        jQuery("#txtRepeatOldPwd").focus();
        jQuery("#txtRepeatOldPwd").select();
        ShowModifyPasswordMsg("请再次输入新密码！");
        return false;
    }

    if (NewPassword != RepeatOldPassword) {

        jQuery("#txtRepeatOldPwd").focus();
        jQuery("#txtRepeatOldPwd").select();
        ShowModifyPasswordMsg("您输入的两次新密码不一致，请核对后重新再试！");
        return false;
    }


    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxConfig.aspx",
        data: { OldPassword: OldPassword,
            NewPassword: NewPassword,
            action: 'ModifyUserPasswordInfo',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            if (parseInt(jsonmsg) > 0) {
                ShowModifyPasswordMsg("修改密码成功！");
            }
            else if (parseInt(jsonmsg) == -1) {
                //OpenUserPasswordModifyWindow();
                ShowModifyPasswordMsg("原密码错误，请重新输入！");
            }
            else {

                ShowModifyPasswordMsg("修改密码失败，请与管理员员联系！");
            }
        }
    });

    return true;
}

function ShowModifyPasswordMsg(msg) {
    if (msg != "") {
        jQuery("#modifyPasswordMsg").html(msg);
        jQuery("#modifyPasswordMsg").show();
    } else {
        jQuery("#modifyPasswordMsg").hide();
    }
}

/// <summary>
/// 打开或关闭功能组 
/// </summary>
/// <param name="clickObj">点击的功能组元素</param>
/// <param name="showObj">需要显示或隐藏的元素ID</param>
function OpenOrClose(clickObj, showObjID) {
    if (clickObj == null || clickObj == undefined)
        return false;
    if (jQuery("#" + showObjID).is(":visible")) {
        jQuery(clickObj).find("img").attr("src", "/template/blue2/images/arrow-bottom.png");
        jQuery(clickObj).attr("title", "点击显示");
        jQuery("#" + showObjID).hide();

    }
    else {
        jQuery(clickObj).find("img").attr("src", "/template/blue2/images/arrow-top.png");
        jQuery(clickObj).attr("title", "点击隐藏");
        jQuery("#" + showObjID).show();
    }
}
/// <summary>
/// 打开或关闭功能组(无提示文字) wtang 2014-04-02
/// </summary>
/// <param name="clickObj">点击的功能组元素</param>
/// <param name="showObj">需要显示或隐藏的元素ID</param>
function OpenOrCloseNoTips(clickObj, showObjID) {
    if (clickObj == null || clickObj == undefined)
        return false;

    if (jQuery("#" + showObjID).is(":visible")) {
        jQuery("#" + showObjID).hide();
    }
    else {
        jQuery("#" + showObjID).show();
    }
}


//分科信息、历史对比TAB
function dxqh(n) {
    var tag = document.getElementById("fkls").getElementsByTagName("li");
    var taglength = tag.length;

    for (i = 1; i <= taglength; i++) {
        document.getElementById("ExamInfoTabLi" + i).className = "";
        document.getElementById("ExamInfoTabDetail" + i).style.display = 'none';
    }
    document.getElementById("ExamInfoTabLi" + n).className = "fkls-on";
    document.getElementById("ExamInfoTabDetail" + n).style.display = '';
}
var DelayMessage = false;
// 检测是否其它用户登录，或登录超时
function CheckUserIsLoginState() {
    // 判断标记是否已经显示登录框
    if (isShowLoginDialog == 1) { return; }

    // 判断是否任然是当前进行的登录
    var retCurrentUserMsg = JudgeISCurrentUserLogin();
    if (retCurrentUserMsg == "-9999" || retCurrentUserMsg == "-9998") {
        isShowLoginDialog = 1; // 标记是否已经显示登录框 
        ReLoginConfirm();
    }
    //else if (retCurrentUserMsg.indexOf("服务器许可") > -1 && DelayMessage == false) {
    //    DelayMessage = true;
    //    ShowSystemDialog(retCurrentUserMsg);
    //}
}