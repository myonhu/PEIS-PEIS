/**
 * Created by gjq on 2017/2/23.
 */
 function logout() {
    //clearThisLoginCookies();
   // clearAllCookies();
    window.open("/Login.aspx", "_top");//直接
}
 function closeWindows() {
     // document.getElementById("iframe").contentWindow.closeWindow();
    if (confirm("您确定要关闭本页吗？")) {
        // clearAllCookies();
        clearThisLoginCookies();
        var userAgent = navigator.userAgent;
        if (userAgent.indexOf("Firefox") != -1 || userAgent.indexOf("Chrome") != -1) {
           // window.external.ExitWindow();
            window.open("about:blank", "_self").close();//退出当前页chrome无法关闭窗口
        } else {
            window.opener = null;
            window.open(" ", "_self");
            window.close();
        }
    }
};

//$(function () {
//    if (strPage.indexOf("index.htm")>=0) {
//        getIndexListData();
//    }
//    alert(getCookies("UserLoginName"));
//    alert(getCookies("UserName"));
//    alert(getCookies("VocationType"));
   

//    //主页菜单操作
   

//});



//退出系统重新登陆
function quitAndLogin(tipes,doClass) {
    //如果不写提示的话直接跳到登陆界面
    if (tipes) {
        if (confirm(tipes)) {
            doClass();
        }
    } else {
        doClass();
    }
}


//清除本次登陆的cookies
function clearThisLoginCookies() {
    delCookies("ASP.NET_SessionId");
    delCookies("FYHUserPassPort");
    delCookies("Idea-8e799da1");
    delCookies("UserID");
    delCookies("UserLoginName");
    delCookies("UserName");
    delCookies("UserOperateLevel");
    delCookies("UserSessionID");
    delCookies("VocationType");
}




//时间选择控件统一加class  dateselect
$(function () {
    //民族控件加selectMinZu  class
    var minzukongjian = $(".selectMinZu");
    if (minzukongjian) {
        var minzu = "";
        for (var i = 0; i < 56; i++) {
            minzu += '<option value="'+allMinZuValue[i]+'">'+allMinZu[i]+'</option>';
        }

        $(".selectMinZu").append(minzu);
    }
    
})

//民族下拉框数组  以selectMinZu  class添加选择控件

var allMinZu = ['汉族', '蒙古族', '回族', '藏族', '维吾尔族', '苗族', '彝族', '壮族', '布依族', '朝鲜族', '满族', '侗族', '瑶族', '白族', '土家族', '哈尼族', '哈萨克族', '傣族', '黎族', '傈僳族', '佤族', '畲族', '高山族', '拉祜族', '水族', '东乡族', '纳西族', '景颇族', '柯尔克孜族', '土族', '达翰尔族', '仫佬族', '羌族', '布朗族', '撒拉族', '毛南族', '仡佬族', '锡伯族', '阿昌族', '普米族', '塔吉克族', '怒族', '乌孜别克族', '俄罗斯族', '鄂温克族', '德昂族', '保安族', '裕固族', '京族', '塔塔尔族', '独龙族', '鄂伦春族', '赫哲族', '门巴族', '珞巴族', '基诺族'];
var allMinZuValue = ['HANZ', 'MENGGUZ', 'HUIZ', 'WEIWUERZ', 'MIAOZZ', 'YIZ', 'ZHUANGZ', 'BUYIZ', 'CHAOXIANZ', 'MANZ', 'DONGZ', 'YAOZ', 'BAIZ', 'TUJIAZ', 'HANIZ', 'HASAKAZ', 'DAIZ', 'LIZ', 'LISUZ', 'WAZ', 'SHEZ', 'GAOSHANZ', 'LAHUZ', 'SHUIZ', 'DONGXIANGZ', 'NAXIZ', 'JINGPOZ', 'KEERKEZIZ', 'TUZ', 'DAHANERZ', 'MULAOZ', 'QIANGZ', 'BULANGZ', 'SALAZ', 'MAONANZ', 'KUALAOZ', 'XIBOZ', 'ACHANGZ', 'PUMIZ', 'TAJIKEZ', 'NUZ', 'WUZIBIEKEZ', 'ELUOSIZ', 'EWENKEZ', 'DEANGZ', 'BAOANZ', 'YUGUZ', 'JINGZ', 'TATAERZ', 'DULONGZ', 'ELUNCHUNZ', 'HEZHEZ', 'MENBAZ', 'LUOBAZ', 'JINUOZ'];
//监听dom变化
//var bread = $('.bread');//the element I want to monitor 
//bread.bind('DOMNodeInserted', function (e) {
//    alert('element now contains: ' + $(e.target).html());
//});


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
        document.getElementById("ShowHideDialogbg").style.height = document.body.scrollHeight + "px";
        document.getElementById("ShowHideDialogbg").style.width = document.body.scrollWidth + "px";

        document.getElementById("ShowHideDialogbg").style.display = "block";
    } else {
        document.body.style.overflow = "auto";
        jQuery("#" + DialogID).hide();
        document.getElementById("ShowHideDialogbg").style.display = 'none';
    }
}

//jQuery(document).ready(function () {

//    // 获取用户的菜单列表,权限列表，分检科室列表 (用于显示菜单项，判断用户权限)
//    GetUserMenuRightSectionList_New();

//    //SetSideFloatXY(); // 计算侧边栏的坐标

//    var IsLogin = jQuery("#CookieIsLogin").val();
//    var SessionID = jQuery("#CookieSessionID").val();
//    var LoginName = jQuery("#CookieLoginName").val();
//    var UserName = jQuery("#CookieUserName").val();
//    if (LoginName != "") {
//        if (IsLogin == 1)//表示用户是来之登录界面，向其他站内成员发送上线消息
//        {
//            Wait(LoginName, UserName, "LOGIN", "INFO"); //发送上线通知消息
//        }
//        else//表示用户来之首页,开启站内消息等待
//        {
//            Wait(LoginName, UserName, "-1", "INFO"); //开启消息接收
//        }
//    }
//    // SetContentLoadFormHeight(); // 设置加载区域的高度
//    setInterval(function () { jQuery(".CurrentTime").html(GetCurrentTime) }, 3000); // 显示当前时间

//    // 检测是否其它用户登录，或登录超时
//    // setInterval("CheckUserIsLoginState()", 60 * 1000); // 显示当前时间

//    document.onmousewheel = function () {
//        if (event.ctrlKey) {
//            return false;
//        }
//    }

//});
//var IsShowDelayMessage = false;
//var gLoadUrl = "";
///// <summary>
///// 加载子页面
///// </summary>

///// <summary>
///// 加载子页面
///// </summary>
//function DoLoadSubPage(href, parentContainer) {
//    SetUserRedirectUrl(href); // 每次加载前，先保存当前地址，用于在刷新页面时，页面进行自动加载 
//    ShowSubTitleName(); // 显示二级菜单的名称

//    if (href == "" || href == undefined) {
//        href = "/System/Welcome.aspx";
//    }
//    if (jQuery("#LastLoadSubPage").val() == href && href == "/System/Welcome.aspx") {
//        jQuery(".j-autoHeight").autoHeight(); // 自适应高度
//        return;
//    }
//    // 点击时，隐藏二级菜单
//    if (href != "") {
//        jQuery(".LoginUserMenuClassSubItem").hide();
//    }
//    jQuery("#LastLoadSubPage").val(href); // 记录最近一次加载的子页面地址

//    if (parentContainer == '' || parentContainer == undefined)
//        parentContainer = 'loadForm';

//    // 隐藏侧边栏按钮和连接 
//    if (parentContainer == "loadForm") {
//        jQuery("#divSectionExamedCount").hide();    // 隐藏已检查科室链接
//        jQuery("#divHistoryExamCount").hide();      // 隐藏历史检查次数链接
//        jQuery("#divSectionExamFloat").hide();      // 隐藏切换客户科室链接
//        jQuery("#divFinalExamCompare").hide();      // 隐藏总检对比链接
//        jQuery("#divCloneRightFloat").hide();       // 隐藏克隆链接  添加 

//        jQuery("#listSectionExamedCount").hide();   // 隐藏已检查科室列表
//        jQuery("#listSectionExamFloat").hide();     // 隐藏客户科室列表
//        jQuery("#listHistoryExamCount").hide();     // 隐藏历史体检号对比列表
//        jQuery("#listFinalExamCompare").hide();     // 隐藏总检对比列表
//        jQuery("#listCloneRightFloat").hide();      // 隐藏克隆链接

//        SetSideFloatXY(); // 计算侧边栏的坐标
//    }


//    var date = GetQueryString('date');
//    if (date == null || date == "" || date == undefined) {
//        if (href.indexOf("?") >= 0) {
//            href = href + "&date=" + encodeURIComponent(new Date());
//        }
//        else {
//            href = href + "?date=" + encodeURIComponent(new Date());
//        }
//    }

//    DisposeCamera(); //在加载页面之前判断是否开启视频资源，有则关闭
//    DisposeVideoCapture(); //在加载页面之前判断是否开启采集卡资源，有则关闭
//    jQuery("#" + parentContainer).empty();

//    // SwitchHeader(1); // 先回到正常头部显示状态

//    jQuery("#" + parentContainer).load(href, { limit: 200, cache: false }, function () {
//        //ShowSystemDialog("The last 25 entries in the feed have been loaded");

//        // SetContentLoadFormHeight(); // 设置加载区域的高度
//    });
//    jQuery("#" + parentContainer).ajaxSend(function () {
//        ShowLoadingProcessBarDefault(1);
//    });

//    jQuery("#" + parentContainer).ajaxStart(function () {
//        ShowLoadingProcessBarDefault(1);
//    });

//    jQuery("#" + parentContainer).ajaxComplete(function () {
//        ShowLoadingProcessBarDefault(0);
//    });


//    // GetCustomerInfo();
//}

//function GetCustomerInfo() {
//    jQuery('title').val('身份证：' + jQuery("#txtSFZ").val());
//}

//function ShowAllLoginUserMenu(ClassID) {
//    jQuery("#LoginUserMenuFrame_" + ClassID).attr("style", "background-color:White;overflow:hidden; width:984px; ");
//    jQuery("#ShowAllLoginUserMenu_" + ClassID).hide();
//    jQuery("#HideLoginUserMoreMenu_" + ClassID).show();
//}

//function HideLoginUserMoreMenu(ClassID) {
//    jQuery("#LoginUserMenuFrame_" + ClassID).attr("style", "max-height:108px; overflow:auto; background-color:White; ");
//    jQuery("#ShowAllLoginUserMenu_" + ClassID).show();
//    jQuery("#HideLoginUserMoreMenu_" + ClassID).hide();
//}




//function ShowModifyPasswordMsg(msg) {
//    if (msg != "") {
//        jQuery("#modifyPasswordMsg").html(msg);
//        jQuery("#modifyPasswordMsg").show();
//    } else {
//        jQuery("#modifyPasswordMsg").hide();
//    }
//}

///// <summary>
///// 打开或关闭功能组 
///// </summary>
///// <param name="clickObj">点击的功能组元素</param>
///// <param name="showObj">需要显示或隐藏的元素ID</param>
//function OpenOrClose(clickObj, showObjID) {
//    if (clickObj == null || clickObj == undefined)
//        return false;
//    if (jQuery("#" + showObjID).is(":visible")) {
//        jQuery(clickObj).find("img").attr("src", "/template/blue2/images/arrow-bottom.png");
//        jQuery(clickObj).attr("title", "点击显示");
//        jQuery("#" + showObjID).hide();

//    }
//    else {
//        jQuery(clickObj).find("img").attr("src", "/template/blue2/images/arrow-top.png");
//        jQuery(clickObj).attr("title", "点击隐藏");
//        jQuery("#" + showObjID).show();
//    }
//}
///// <summary>
///// 打开或关闭功能组(无提示文字) wtang 2014-04-02
///// </summary>
///// <param name="clickObj">点击的功能组元素</param>
///// <param name="showObj">需要显示或隐藏的元素ID</param>
//function OpenOrCloseNoTips(clickObj, showObjID) {
//    if (clickObj == null || clickObj == undefined)
//        return false;

//    if (jQuery("#" + showObjID).is(":visible")) {
//        jQuery("#" + showObjID).hide();
//    }
//    else {
//        jQuery("#" + showObjID).show();
//    }
//}


////分科信息、历史对比TAB
//function dxqh(n) {
//    var tag = document.getElementById("fkls").getElementsByTagName("li");
//    var taglength = tag.length;

//    for (i = 1; i <= taglength; i++) {
//        document.getElementById("ExamInfoTabLi" + i).className = "";
//        document.getElementById("ExamInfoTabDetail" + i).style.display = 'none';
//    }
//    document.getElementById("ExamInfoTabLi" + n).className = "fkls-on";
//    document.getElementById("ExamInfoTabDetail" + n).style.display = '';
//}
//var DelayMessage = false;
//// 检测是否其它用户登录，或登录超时
//function CheckUserIsLoginState() {
//    // 判断标记是否已经显示登录框
//    if (isShowLoginDialog == 1) { return; }

//    // 判断是否任然是当前进行的登录
//    var retCurrentUserMsg = JudgeISCurrentUserLogin();
//    if (retCurrentUserMsg == "-9999" || retCurrentUserMsg == "-9998") {
//        isShowLoginDialog = 1; // 标记是否已经显示登录框 
//        ReLoginConfirm();
//    }
//    //else if (retCurrentUserMsg.indexOf("服务器许可") > -1 && DelayMessage == false) {
//    //    DelayMessage = true;
//    //    ShowSystemDialog(retCurrentUserMsg);
//    //}
//}
function iframeLoad(url) {
    $("#iframe").attr("src", url);
}