﻿/**
 * Created by gjq on 2017/2/28.
 */
var thisPageUrl = window.location.href;
var isLogin = getUrlParam(thisPageUrl, "IsLogin");
if (isLogin === "1") {
    userIsLogin();
   // quitAndLogin("请您重新登陆"); // 提示重新登录
    GetUserLoginInfo();
    //alert(getCookies("UserLoginName"));
    getIndexListData();
    checkBread();
    var href = '/System/Welcome.aspx?date='+ encodeURIComponent(new Date())+'';
    
    iframeLoad(href);
    //$("#iframe").attr("src", href);
}
function checkBread(){
    //检查导航条状态决定隐藏还是显示
    if (haschild(".bread",0) === 1) {
        $(".bread").show();
        $(".leftnav").css("top", "120px");
        $(".admin").css("top", "120px");
    } else {
        $(".bread").hide();
        $(".leftnav").css("top", "70px");
        $(".admin").css("top", "70px");
    }
}
function vailData(whichBtn) {
    if (whichBtn === 0 || whichBtn ===1) {
        //1保存 0完成
    var _this = $("#iframe").contents().find('.form-x');
    document.getElementById("iframe").contentWindow.vailData(_this, whichBtn);
    } else if (whichBtn === 2) {
        //读证件
        document.getElementById("iframe").contentWindow.ReadCustomerFromCard();
    } else if (whichBtn === 3) {
        //无证件
        document.getElementById("iframe").contentWindow.OpenUserNoIDCard();
    } else if (whichBtn === 4) {
        //补打
        document.getElementById("iframe").contentWindow.PrintCust('0', 'Edit');
    } else if (whichBtn === 5) {
        //搜索
        document.getElementById("iframe").contentWindow.SearchCardAndCustomer();
    } else if (whichBtn === 6) {
        //搜索
        document.getElementById("iframe").contentWindow.RedirectSearch();
    }
    else if (whichBtn === 7) {
        //搜索
        iframeLoad("/System/Customer/RegistOper.aspx?modelName=Sign&IsTeam=1&type=add");
        $(".leftnav ul li a").removeClass("on");
        $(".leftnav ul li a[dizhi='/System/Customer/RegistOper.aspx?modelName=Sign&IsTeam=1&type=add']").addClass("on");
    }
    else if (whichBtn === 8) {
        //搜索
        iframeLoad("/System/Customer/RegistOper.aspx?modelName=SignAndRegiste&type=add&IsTeam=0");
        $(".leftnav ul li a").removeClass("on");
        $(".leftnav ul li a[dizhi='/System/Customer/RegistOper.aspx?modelName=SignAndRegiste&type=add&IsTeam=0']").addClass("on");
    }
}
