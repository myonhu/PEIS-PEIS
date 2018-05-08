﻿

String.prototype.startWith = function (str) {
    var reg = new RegExp("^" + str);
    return reg.test(this);
}

String.prototype.endWith = function (str) {
    var reg = new RegExp(str + "$");
    return reg.test(this);
}

var IsShowProcessBar = true;
//分页配置参数
var pagePagination = {
    prev_text: '上一页',
    next_text: '下一页',
    totalnumber_prev_text: "共 ",
    totalnumber_next_text: " 条记录",
    items_per_page: 50,
    num_display_entries: 10,
    num_edge_entries: 2
}

function getOptionsFromForm() {
    var opt = { callback: pageselectCallback };
    opt.prev_text = pagePagination.prev_text;
    opt.next_text = pagePagination.next_text;
    opt.totalnumber_prev_text = pagePagination.totalnumber_prev_text;
    opt.totalnumber_next_text = pagePagination.totalnumber_next_text;
    opt.items_per_page = pagePagination.items_per_page;
    opt.num_display_entries = pagePagination.num_display_entries;
    opt.num_edge_entries = pagePagination.num_edge_entries;
    return opt;
}
//分页配置参数
var pagePagination04 = {
    prev_text: '上一页',
    next_text: '下一页',
    totalnumber_prev_text: "共 ",
    totalnumber_next_text: " 条记录",
    items_per_page: 50,
    num_display_entries: 4,
    num_edge_entries: 1
}
//报告打印分页配置参数
var pagePagination05 = {
    prev_text: '上一页',
    next_text: '下一页',
    totalnumber_prev_text: "共 ",
    totalnumber_next_text: " 条记录",
    items_per_page: 100,
    num_display_entries: 4,
    num_edge_entries: 1
}
function getOptionsFromForm05() {
    var opt = { callback: pageselectCallback };
    opt.prev_text = pagePagination05.prev_text;
    opt.next_text = pagePagination05.next_text;
    opt.totalnumber_prev_text = pagePagination05.totalnumber_prev_text;
    opt.totalnumber_next_text = pagePagination05.totalnumber_next_text;
    opt.items_per_page = pagePagination05.items_per_page;
    opt.num_display_entries = pagePagination05.num_display_entries;
    opt.num_edge_entries = pagePagination05.num_edge_entries;
    return opt;
}
//团体备单客户名单分页配置参数 xmhuang 2014-04-15
var pagePagination06 = {
    prev_text: '上一页',
    next_text: '下一页',
    totalnumber_prev_text: "共 ",
    totalnumber_next_text: " 条记录",
    items_per_page: 15,
    num_display_entries: 4,
    num_edge_entries: 1
}
function getOptionsFromForm06() {
    var opt = { callback: pageselectCallback };
    opt.prev_text = pagePagination06.prev_text;
    opt.next_text = pagePagination06.next_text;
    opt.totalnumber_prev_text = pagePagination06.totalnumber_prev_text;
    opt.totalnumber_next_text = pagePagination06.totalnumber_next_text;
    opt.items_per_page = pagePagination06.items_per_page;
    opt.num_display_entries = pagePagination06.num_display_entries;
    opt.num_edge_entries = pagePagination06.num_edge_entries;
    return opt;
}
function getOptionsFromForm04() {
    var opt = { callback: pageselectCallback };
    opt.prev_text = pagePagination04.prev_text;
    opt.next_text = pagePagination04.next_text;
    opt.totalnumber_prev_text = pagePagination04.totalnumber_prev_text;
    opt.totalnumber_next_text = pagePagination04.totalnumber_next_text;
    opt.items_per_page = pagePagination04.items_per_page;
    opt.num_display_entries = pagePagination04.num_display_entries;
    opt.num_edge_entries = pagePagination04.num_edge_entries;
    return opt;
}
var regexEnum =
{
    intege: "^-?[1-9]\\d*$", 				//整数
    intege1: "^[1-9]\\d*$", 				//正整数
    intege2: "^-[1-9]\\d*$", 				//负整数
    num: "^([+-]?)\\d*\\.?\\d+$", 		//数字
    num1: "^[1-9]\\d*|0$", 				//正数（正整数 + 0）
    num2: "^-[1-9]\\d*|0$", 				//负数（负整数 + 0）
    decmal: "^([+-]?)\\d*\\.\\d+$", 		//浮点数
    decmal1: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*$", //正浮点数
    decmal2: "^-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*)$", //负浮点数
    decmal3: "^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$", //浮点数
    decmal4: "^[1-9]\\d*|^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$", //非负浮点数（正浮点数 + 0）
    decmal5: "^(-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*))|0?.0+|0$", //非正浮点数（负浮点数 + 0）
    email: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //邮件
    color: "^[a-fA-F0-9]{6}$", 			//颜色
    url: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$", //url
    chinese: "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$", 				//仅中文
    ascii: "^[\\x00-\\xFF]+$", 			//仅ACSII字符
    zipcode: "^\\d{6}$", 					//邮编
    mobile: "^13[0-9]{9}|15[012356789][0-9]{8}|18[0256789][0-9]{8}|147[0-9]{8}$", 			//手机
    ip4: "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$", //ip地址
    notempty: "^\\S+$", 					//非空
    picture: "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$", //图片
    rar: "(.*)\\.(rar|zip|7zip|tgz)$", 							//压缩文件
    date: "^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$", 				//日期
    qq: "^[1-9]*[1-9][0-9]*$", 			//QQ号码
    tel: "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$", //电话号码的函数(包括验证国内区号,国际区号,分机号)
    phone: "/^0?1(3|5|8|)\d{9}$/",
    username: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+|13[0-9]{9}|15[012356789][0-9]{8}|18[0256789][0-9]{8}|147[0-9]{8}|^[a-zA-Z][a-zA-Z0-9_]*$", //邮件 手机号码 字母开始的字母+数字的组合
    email: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //邮件 手机号码 
    letter: "^[A-Za-z]+$", 				//字母
    letter_u: "^[A-Z]+$", 				//大写字母
    letter_l: "^[a-z]+$", 				//小写字母
    idcard: "^[1-9]([0-9]{14}|[0-9]{17})$"	//身份证
}
function checkAll(obj) {
    jQuery("[name='ItemCheckbox']").each(function () {
        jQuery(this).attr('checked', obj.checked);
    })
}
//验证是否正浮点类型数据 xmhuang 2014-04-17
function IsDecmal(str) {
    var patrn = regexEnum.decmal1;
    if (!str.match(patrn)) {
        return false;
    }
    return true;
}
//验证是否为Email
function IsEmail(email) {


}
//判断是否为电话号码：包含手机和座机号码
function IsPhone(phoneNum) {

}
//判断是否是正整数 xmhuang 2013-10-23
function IsNum(s) {
    var patrn = regexEnum.intege1;
    if (!s.match(patrn)) {
        return false;
    }
    return true;
}

function isMobil(s) {
    var patrn = /^0?1(3|5|8|)\d{9}$/;
    if (!patrn.test(s)) {
        return false;
    }
    return true;
}
function isEmail(str) {
    //   var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    //   var r = str.match(reg);
    var patrn = regexEnum.email;
    if (!str.match(patrn)) {
        return false;
    }
    return true;

}
/// <summary>
/// 判断是否是体检号 （全数字 + 14位）
/// </summary>
function isCustomerExamNo(s) {
    var patrn = regexEnum.intege1;
    if (!s.match(patrn)) {
        return false;
    }
    if (s.length != 14) {
        return false;
    }
    return true;
}

/// <summary>
/// 判断是否是日期格式
/// 修改人：xmhuang 2013-12-12 
/// 描述:此方法验证正确日期包含格式和实际日期
/// </summary>
function isDate(str) {
    var date = str;
    var result = date.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (result == null) {
        return false;
    }
    var d = new Date(result[1], result[3] - 1, result[4]);
    return (d.getFullYear() == result[1] && (d.getMonth() + 1) == result[3] && d.getDate() == result[4]);

}
/// <summary>
/// 判断是否是身份证号码
/// </summary>
function isIDCardNo(s) {
    if (s.length == 15 || s.length == 18) {
        return true;
    }
    return false;
}
function IsIP(str) {
    var patrn = regexEnum.ip4;
    if (!str.match(patrn)) {
        return false;
    }
    return true;
}
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    }
    else {
        return "";
    }
}

// Html 编码获取 html 转义实体(换行符号需要转换回去 ，否则会显示<br>)
function htmlEncode(value) {
    var strEncode = jQuery('<div/>').text(value).html();
    return strEncode.replace(/&lt;br\/&gt;/g, '<br>');
}

// Html 解码获取 html 转义实体
function htmlDecode(value) {
    return jQuery('<div/>').html(value).text();
}

//打印预览
function DoPrintView(containerId) {
    var html = document.getElementById(containerId).innerHTML;
    window.document.body.innerHTML = html;
    window.print();
}
//function DoPrint(containerId) {
//    var html = document.getElementById(containerId).innerHTML;
//    window.document.body.innerHTML = html;
//    window.print();
//}
/*************通用弹出窗**********************/
/// <summary>
/// url:页面地址
///whichText：弹出窗口的显示标题
///whichForm
///
///
/// </summary>
function ShowDialogX(url, whichText, width, height, optValidata) {
    var modalForm = showModalDialog(url, window, "dialogTitle:" + whichText + ";dialogWidth:" + width + "px;dialogHeight:" + height + "px;help:no;scroll:on;status:off;resizable:on;");
    return modalForm;
}

/**************大小写转换通用 Begin*********************/

function regInput(reg) {
    var srcElem = event.srcElement
    var oSel = document.selection.createRange()
    oSel = oSel.duplicate()

    oSel.text = ""
    var srcRange = srcElem.createTextRange()
    oSel.setEndPoint("StartToStart", srcRange)
    var num = oSel.text + String.fromCharCode(event.keyCode) + srcRange.text.substr(oSel.text.length)
    event.returnvalue = reg.test(num)
}
function chineseNumber(num) {
    if (isNaN(num) || num > Math.pow(10, 12)) return ""
    var cn = "零壹贰叁肆伍陆柒捌玖"
    var unit = new Array("拾百千", "分角")
    var unit1 = new Array("万亿", "")
    var numArray = num.toString().split(".")
    var start = new Array(numArray[0].length - 1, 2)
    function toChinese(num, index) {
        var num = num.replace(/\d/g, function ($1) {
            return cn.charAt($1) + unit[index].charAt(start-- % 4 ? start % 4 : -1)
        })
        return num
    }
    for (var i = 0; i < numArray.length; i++) {
        var tmp = ""
        for (var j = 0; j * 4 < numArray[i].length; j++) {
            var strIndex = numArray[i].length - (j + 1) * 4
            var str = numArray[i].substring(strIndex, strIndex + 4)
            var start = i ? 2 : str.length - 1
            var tmp1 = toChinese(str, i)
            tmp1 = tmp1.replace(/(零.)+/g, "零").replace(/零+$/, "")
            tmp1 = tmp1.replace(/^壹拾/, "拾")
            tmp = (tmp1 + unit1[i].charAt(j - 1)) + tmp
        }
        numArray[i] = tmp
    }
    numArray[1] = numArray[1] ? numArray[1] : ""
    numArray[0] = numArray[0] ? numArray[0] + "元" : numArray[0], numArray[1] = numArray[1].replace(/^零+/, "")
    numArray[1] = numArray[1].match(/分/) ? numArray[1] : numArray[1]; // + "整"
    return numArray[0] + numArray[1] == "" ? "零元" : numArray[0] + numArray[1];
}
function aNumber(num) {
    var numArray = new Array()
    var unit = "亿万元$"
    for (var i = 0; i < unit.length; i++) {
        var re = eval("/" + (numArray[i - 1] ? unit.charAt(i - 1) : "") + "(.*)" + unit.charAt(i) + "/")
        if (num.match(re)) {
            numArray[i] = num.match(re)[1].replace(/^拾/, "壹拾")
            numArray[i] = numArray[i].replace(/[零壹贰叁肆伍陆柒捌玖]/g, function ($1) {
                return "零壹贰叁肆伍陆柒捌玖".indexOf($1)
            })
            numArray[i] = numArray[i].replace(/[分角拾百千]/g, function ($1) {
                return "*" + Math.pow(10, "分角 拾百千 ".indexOf($1) - 2) + "+"
            }).replace(/^\*|\+$/g, "").replace(/整/, "0")
            numArray[i] = "(" + numArray[i] + ")*" + Math.ceil(Math.pow(10, (2 - i) * 4))
        }
        else numArray[i] = 0
    }
    return eval(numArray.join("+"))
}

/**************大小写转换通用 End*********************/




/************************* 用户权限控制 start ***********************************/;

/// <summary>
///  判断用户是否具有相应的权限
/// </summary>
function GetUserPasswordRight() {
    var isHavePasswordChange_Right = Is_LoginUserRight('A.D.01_PasswordChange');    // 当前用户口令修改
    if (isHavePasswordChange_Right == true) { jQuery(".ChangePasswordUserRightSet").show(); }
    else { jQuery(".ChangePasswordUserRightSet").remove(); }
}

/************************* 用户权限控制 start ***********************************/




function ShowSystemWarningDialog(msg) {
    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            title: '警告',
            lock: true,
            fixed: true,
            opacity: 0.3,
            content: msg,
            button: [{
                name: '确定',
                callback: function () {
                    return true;
                }
            }]
        });
    }
}

function ShowSystemDialog(msg) {
    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            lock: true,
            fixed: true,
            opacity: 0.3,
            content: msg,
            button: [{
                name: '确定',
                callback: function () {
                    return true;
                }
            }],
            close: function () {
                return true;
            }
        });
    }
}

function ShowSystemDialogAutoClose(msg, canFocusObj) {
    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            lock: true,
            fixed: true,
            opacity: 0.3,
            time: 1,
            content: msg,
            button: [{
                name: '确定',
                callback: function () {
                    return true;
                }
            }],
            close: function () {
                if (canFocusObj) {
                    if ("function" === typeof canFocusObj) {
                        canFocusObj && canFocusObj.call(this);
                    }
                    else {
                        if (jQuery(canFocusObj).length > 0) {
                            jQuery(canFocusObj).focus();
                            jQuery(canFocusObj).select();
                        }
                    }
                    return true;
                }
            }
        });
    }
}

function ShowSystemDialogCloseDialog(msg, isClose) {

    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            lock: true,
            fixed: true,
            opacity: 0.3,
            content: msg,
            button: [{
                name: '确定',
                callback: function () {
                    // 如果是自动关闭弹出窗口
                    if (isClose == "True") {
                        CloseDialogWindow();
                    } else {
                        btnResetClick(); // 重置页面数据
                    }
                    return true;
                }
            }]
        });
    }
}
/// <summary>
/// 打开系统提示窗口 并设置指定对象焦点，并选中
/// </summary>
function ShowCallBackSystemDialog(msg, canFocusObj) {
    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            opacity: 0.3,
            content: msg,
            button: [{
                name: '确定',
                callback: function () {
                    return true;
                }
            }],
            close: function () {
                if (canFocusObj) {
                    if ("function" === typeof canFocusObj) {
                        canFocusObj && canFocusObj.call(this);
                    }
                    else {
                        if (jQuery(canFocusObj).length > 0) {
                            jQuery(canFocusObj).focus();
                            jQuery(canFocusObj).select();
                        }
                    }
                    return true;
                }
            }
        });
    }
}
/// <summary>
/// 病症级别的提示
/// </summary>
function ShowDiseaseLevelDialog(msg) {

    if (msg != "") {
        art.dialog({
            id: 'artDiseaseLevel',
            title: '警告',
            lock: true,
            fixed: true,
            opacity: 0.3,
            //   time:10,
            content: msg,
            close: function () {

                jQuery("#txtCustomerID").focus();

                return true;
            }
        });

        jQuery(".aui_content").css("padding-left", "0px");
        jQuery(".aui_content").css("padding-right", "0px");
        jQuery(".aui_content").css("padding-bottom", "0px");
        jQuery(".aui_dialog").css("margin-left", "-8px");
        jQuery(".aui_dialog").css("margin-right", "-8px");
        jQuery(".aui_dialog").css("margin-bottom", "0px");
    }
}
function btnResetClick() {

    jQuery("#btnReset").click();
}

// ================================ 登录用户的菜单列表,权限列表，分检科室列表 （新菜单 20150411 ） ==== start ==================================================



/// <summary>
/// 获取用户的菜单列表,权限列表，分检科室列表  (20150411)
/// </summary>
function GetUserMenuRightSectionList_New() {
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxRight.aspx",
        data: { action: 'GetUserMenuRightSectionList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            if (jsonmsg != null && jsonmsg != "") {
                // 显示菜单项，判断用户权限
                ShowUserMenuRightSectionList_New(jsonmsg);

                BandMuenClassDownListEvent_New(); // 计算并绑定下拉菜单的事件

                HideSubMenu();
            }
        }
    });
}


/// <summary>
///  用于显示菜单项，判断用户权限
/// </summary>
function ShowUserMenuRightSectionList_New(UserMRSList) {

    gLoginRightJsonData = UserMRSList.dataList1; // 登录用户的权限变量
    gLoginUserSection = UserMRSList.dataList2;   // 用户具有的科室
    gLoginUserMenuClass = UserMRSList.dataList3; // 用户菜单分组

    // 菜单显示模版
    var LoginUserMenuTempleteContent = jQuery("#LoginUserMenuTemplete").html();
    if (LoginUserMenuTempleteContent == undefined) { return }

    // 菜单分组显示模版（分组的标题栏）
    var LoginUserMenuClassTempleteContent = jQuery("#LoginUserMenuClassTemplete").html();
    if (LoginUserMenuClassTempleteContent == undefined) { return }
    var LoginUserMenuClassTempleteFlagContent = jQuery("#LoginUserMenuClassTempleteFlag").html();

    // 菜单分组显示模版（分组的详细信息）
    var LoginUserMenuClassSubItemTempleteContent = jQuery("#LoginUserMenuClassSubItemTemplete").html();
    if (LoginUserMenuClassSubItemTempleteContent == undefined) { return }


    var LoginUserMenuStrs = "";             // 用户菜单项内容字符串
    var LoginUserMenuClassStrs = "";        // 用户菜单分组内容字符串（分组的标题栏）
    var LoginUserMenuClassSubItemStrs = ""; // 用户菜单分组内容字符串（分组的详细信息）
    var tmpMenuCount = 0;                   // 记录菜单项的个数
    var tmpMenuClassCount = 0;              // 记录分组的个数
    var tmpFirstMeunClassID = 0;            // 记录第一个菜单分组的ID
    var strShowMoreBtnClassID = "";         // 记录需要显示“显示全部”的分组ID

    // dataList3 菜单分类项目
    var IsSelected = " class='selected' ";
    tmpMenuClassCount = 0;
    jQuery(UserMRSList.dataList3).each(function (k, menuclassitem) {
        tmpMenuClassCount++;
        if (tmpMenuClassCount > 1) {
            IsSelected = "";
        } else {
            tmpFirstMeunClassID = menuclassitem.MenuID;
        }

        IsSelected = "";

        LoginUserMenuClassStrs += LoginUserMenuClassTempleteContent
                            .replace(/@MenuClassName/gi, menuclassitem.MenuName)
                            .replace(/@MenuID/gi, menuclassitem.MenuID)
                            .replace(/@menu_icon_class/gi, GetMenuIconClass(menuclassitem.MenuName)) // 获取一级菜单的样式
                            .replace(/@IsSelected/gi, IsSelected)
                            .replace(/@ParentMenuUrl/gi, menuclassitem.Url);
    });
    LoginUserMenuClassStrs = LoginUserMenuClassStrs; // + LoginUserMenuClassTempleteFlagContent;
    // 显示菜单分组
    jQuery("#LoginUserMenuClass").append(LoginUserMenuClassStrs);
    if (tmpMenuClassCount > 1) {
        jQuery("#LoginUserMenuClassDiv").show();
        jQuery("#LoginUserMenuClass").show();
    }

    // 循环菜单分类 (dataList3)
    jQuery(UserMRSList.dataList3).each(function (k, menuclassitem) {

        LoginUserMenuStrs = ""; // 开始新的分类时，先清空先前的菜单数据
        tmpMenuCount = 0;       // 当前分类中菜单项的数量
        // dataList0 菜单项目
        jQuery(UserMRSList.dataList0).each(function (i, menuitem) {

            // 如果不是本类中的子菜单项，则跳过该条数据，继续判断下一条数据
            if (menuclassitem.MenuID != menuitem.ParentID) { return true; } // continue;

            if (menuitem.Is_CombineWithSection == "True") {

                // dataList2 用户科室
                jQuery(UserMRSList.dataList2).each(function (j, sectionitem) {

                    if (sectionitem.DisplayMenu == menuitem.MenuName) {
                        tmpMenuCount++;
                        LoginUserMenuStrs += LoginUserMenuTempleteContent
                                    .replace(/@MenuID/gi, i)
                                    .replace(/@MenuName/gi, sectionitem.SectionName)
                                    .replace(/@SectionID/gi, sectionitem.SectionID)
                                    .replace(/@Url/gi, menuitem.Url + "?txtSectionID=" + sectionitem.SectionID);

                        //                        jQuery("#LoginUserMenuClass_li1_" + menuitem.ID_ParentMenu).show();
                        //                        jQuery("#LoginUserMenuClass_li2_" + menuitem.ID_ParentMenu).show();

                    }

                });

            } else {

                //                jQuery("#LoginUserMenuClass_li1_" + menuitem.ID_ParentMenu).show();
                //                jQuery("#LoginUserMenuClass_li2_" + menuitem.ID_ParentMenu).show();

                tmpMenuCount++;
                LoginUserMenuStrs += LoginUserMenuTempleteContent
                                .replace(/@MenuID/gi, i)
                                .replace(/@SectionID/gi, "0")
                                .replace(/@MenuName/gi, menuitem.MenuName)
                                .replace(/@Url/gi, menuitem.Url);
            }
        });

        // 如果当前菜单数量大于18，则显示“显示全部”这个按钮。
        if (tmpMenuCount > 18) {
            if (strShowMoreBtnClassID == "") {
                strShowMoreBtnClassID = menuclassitem.MenuID + ",";
            } else {
                strShowMoreBtnClassID = strShowMoreBtnClassID + menuclassitem.MenuID + ",";
            }
        }


        tmpMenuClassCount++;
        LoginUserMenuClassSubItemStrs += LoginUserMenuClassSubItemTempleteContent
                                    .replace(/@MenuID/gi, menuclassitem.MenuID)
                                    .replace(/@LoginUserMenuClassSubItem/gi, LoginUserMenuStrs);
        //if (tmpMenuClassCount == 1) { tmpFirstMeunClassID = menuclassitem.MenuID; }
    });
    // 显示菜单项（显示所有分组后的详细信息）
    jQuery("#ShowUserMenuDiv").html(LoginUserMenuClassSubItemStrs);
    // 显示第一个分组 
    //jQuery("#LoginUserMenuClassSubItem_" + tmpFirstMeunClassID).show();

    // 判断是否有需要显示“显示全部”按钮的分组
    if (strShowMoreBtnClassID != "") {
        var showMoreBtnArray = strShowMoreBtnClassID.split(",");
        for (var i = 0; i < showMoreBtnArray.length; i++) {
            if (showMoreBtnArray[i] != "") {
                jQuery("#ShowAllLoginUserMenu_" + showMoreBtnArray[i]).show();
            }
        }
    }

    jQuery(".menu-actions li").click(function () {
        //remove掉同级元素样式
        jQuery(".menu-actions li").removeClass("selected");
        jQuery(this).addClass("selected");

        var fyhmenuid = jQuery(this).attr("menuid");
        var fyhsectionid = jQuery(this).attr("sectionid");
        SetCookie('SubMenuID', fyhmenuid);       // 点击菜单项，保存对应的menuid
        SetCookie('SubSectionID', fyhsectionid); // 点击菜单项，保存对应的sectionid （如果不是分检，则该值为空）
        ShowSubTitleName_Click(fyhmenuid, fyhsectionid);

        var targeturl = jQuery(this).attr("targeturl");
        if (targeturl == undefined) {
            targeturl = jQuery(this).parent().attr("targeturl");
        }
        DoLoad(targeturl, '');
    });


    // 获取用户相应的权限
    GetUserPasswordRight();

    // 获取当前跳转地址 20130803 by WTang
    GetUserRedirectUrl();

}



/// <summary> 
/// 计算并绑定下拉菜单的事件
/// </summary>
function BandMuenClassDownListEvent_New() {

    jQuery(".menu-actions li").unbind();
    jQuery(".menu-actions li").click(function () {
        //remove掉同级元素样式
        jQuery(".menu-actions li").removeClass("selected");
        jQuery(this).addClass("selected");

        var fyhmenuid = jQuery(this).attr("menuid");
        var fyhsectionid = jQuery(this).attr("sectionid");
        SetCookie('SubMenuID', fyhmenuid);       // 点击菜单项，保存对应的menuid
        SetCookie('SubSectionID', fyhsectionid); // 点击菜单项，保存对应的sectionid （如果不是分检，则该值为空）
        ShowSubTitleName_Click(fyhmenuid, fyhsectionid);

        var targeturl = jQuery(this).attr("targeturl");
        if (targeturl == undefined) {
            targeturl = jQuery(this).parent().attr("targeturl");
        }
        if (targeturl != undefined && targeturl != "") {
            SetSubMenuUrlCookie(targeturl);
        }

        DoLoad(targeturl, '');
    });

}








// ================================ 登录用户的菜单列表,权限列表，分检科室列表 （新菜单 2015 04 11 ） ==== end ==================================================



// ================================ 登录用户的菜单列表,权限列表，分检科室列表 ==== start ==================================================

/// <summary>
/// 获取用户的菜单列表,权限列表，分检科室列表
/// </summary>
function GetUserMenuRightSectionList() {
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxRight.aspx",
        data: { action: 'GetUserMenuRightSectionList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            if (jsonmsg != null && jsonmsg != "") {
                // 显示菜单项，判断用户权限
                ShowUserMenuRightSectionList(jsonmsg);

                BandMuenClassDownListEvent(); // 计算并绑定下拉菜单的事件 20150310 by wtang
            }
        }
    });
}

var gLoginRightJsonData = null; // 登录用户的权限变量
var gLoginUserMenuClass = null; // 用户菜单分组
var gLoginUserSection = null;   // 用户具有的科室

/// <summary>
///  判断登录用户是否具有某科室的操作权限
/// </summary>
function Is_LoginUserSection(currSectionID) {
    var tempCurrentResult = false;
    jQuery(gLoginUserSection).each(function (k, tempsectionitem) {
        if (tempsectionitem.SectionID == currSectionID) {
            tempCurrentResult = true;
            return false; // brerak
        }
    });
    return tempCurrentResult;
}
/// <summary>
///  判断登录用户是否具有当前权限
/// </summary>
function Is_LoginUserRight(currRightCode) {
    var tempCurrentResult = false;
    jQuery(gLoginRightJsonData).each(function (i, rightitem) {
        if (rightitem.RightCode == currRightCode) {
            tempCurrentResult = true;
            return false; // brerak
        }
    });
    return tempCurrentResult;
}

/// <summary>
///  用于显示菜单项，判断用户权限
/// </summary>
function ShowUserMenuRightSectionList(UserMRSList) {

    gLoginRightJsonData = UserMRSList.dataList1; // 登录用户的权限变量
    gLoginUserSection = UserMRSList.dataList2;   // 用户具有的科室
    gLoginUserMenuClass = UserMRSList.dataList3; // 用户菜单分组

    // 菜单显示模版
    var LoginUserMenuTempleteContent = jQuery("#LoginUserMenuTemplete").html();
    if (LoginUserMenuTempleteContent == undefined) { return }

    // 菜单分组显示模版（分组的标题栏）
    var LoginUserMenuClassTempleteContent = jQuery("#LoginUserMenuClassTemplete").html();
    if (LoginUserMenuClassTempleteContent == undefined) { return }
    var LoginUserMenuClassTempleteFlagContent = jQuery("#LoginUserMenuClassTempleteFlag").html();

    // 菜单分组显示模版（分组的详细信息）
    var LoginUserMenuClassSubItemTempleteContent = jQuery("#LoginUserMenuClassSubItemTemplete").html();
    if (LoginUserMenuClassSubItemTempleteContent == undefined) { return }


    var LoginUserMenuStrs = "";             // 用户菜单项内容字符串
    var LoginUserMenuClassStrs = "";        // 用户菜单分组内容字符串（分组的标题栏）
    var LoginUserMenuClassSubItemStrs = ""; // 用户菜单分组内容字符串（分组的详细信息）
    var tmpMenuCount = 0;                   // 记录菜单项的个数
    var tmpMenuClassCount = 0;              // 记录分组的个数
    var tmpFirstMeunClassID = 0;            // 记录第一个菜单分组的ID

    var strShowMoreBtnClassID = "";         // 记录需要显示“显示全部”的分组ID

    // dataList3 菜单分类项目
    var IsSelected = " class='selected' ";
    tmpMenuClassCount = 0;
    jQuery(UserMRSList.dataList3).each(function (k, menuclassitem) {
        tmpMenuClassCount++;
        if (tmpMenuClassCount > 1) {
            IsSelected = "";
            tmpFirstMeunClassID = menuclassitem.MenuID;
        }
        LoginUserMenuClassStrs += LoginUserMenuClassTempleteContent
                            .replace(/@MenuClassName/gi, menuclassitem.MenuName)
                            .replace(/@MenuID/gi, menuclassitem.MenuID)
                            .replace(/@IsSelected/gi, IsSelected)
                            .replace(/@ParentMenuUrl/gi, menuclassitem.MenuUrl);
    });
    LoginUserMenuClassStrs = LoginUserMenuClassStrs; // + LoginUserMenuClassTempleteFlagContent;
    // 显示菜单分组
    jQuery("#LoginUserMenuClass").append(LoginUserMenuClassStrs);
    if (tmpMenuClassCount > 1) {
        jQuery("#LoginUserMenuClassDiv").show();
        jQuery("#LoginUserMenuClass").show();
    }

    // 循环菜单分类 (dataList3)
    jQuery(UserMRSList.dataList3).each(function (k, menuclassitem) {

        LoginUserMenuStrs = ""; // 开始新的分类时，先清空先前的菜单数据
        tmpMenuCount = 0;       // 当前分类中菜单项的数量
        // dataList0 菜单项目
        jQuery(UserMRSList.dataList0).each(function (i, menuitem) {

            // 如果不是本类中的子菜单项，则跳过该条数据，继续判断下一条数据
            if (menuclassitem.MenuID != menuitem.ParentID) { return true; } // continue;

            if (menuitem.Is_CombineWithSection == "True") {

                // dataList2 用户科室
                jQuery(UserMRSList.dataList2).each(function (j, sectionitem) {

                    if (sectionitem.DisplayMenu == menuitem.MenuName) {
                        tmpMenuCount++;
                        LoginUserMenuStrs += LoginUserMenuTempleteContent
                                    .replace(/@MenuID/gi, i)
                                    .replace(/@MenuName/gi, sectionitem.SectionName)
                                    .replace(/@SectionID/gi, sectionitem.SectionID)
                                    .replace(/@Url/gi, menuitem.Url + "?txtSectionID=" + sectionitem.SectionID);

                        jQuery("#LoginUserMenuClass_li1_" + menuitem.ParentID).show();
                        jQuery("#LoginUserMenuClass_li2_" + menuitem.ParentID).show();

                    }

                });

            } else {

                jQuery("#LoginUserMenuClass_li1_" + menuitem.ParentID).show();
                jQuery("#LoginUserMenuClass_li2_" + menuitem.ParentID).show();

                tmpMenuCount++;
                LoginUserMenuStrs += LoginUserMenuTempleteContent
                                .replace(/@MenuID/gi, i)
                                .replace(/@SectionID/gi, "0")
                                .replace(/@MenuName/gi, menuitem.MenuName)
                                .replace(/@Url/gi, menuitem.Url);
            }
        });

        // 如果当前菜单数量大于18，则显示“显示全部”这个按钮。
        if (tmpMenuCount > 18) {
            if (strShowMoreBtnClassID == "") {
                strShowMoreBtnClassID = menuclassitem.MenuID + ",";
            } else {
                strShowMoreBtnClassID = strShowMoreBtnClassID + menuclassitem.MenuID + ",";
            }
        }


        tmpMenuClassCount++;
        LoginUserMenuClassSubItemStrs += LoginUserMenuClassSubItemTempleteContent
                                    .replace(/@MenuID/gi, menuclassitem.MenuID)
                                    .replace(/@LoginUserMenuClassSubItem/gi, LoginUserMenuStrs);
        // if (tmpMenuClassCount == 1) { tmpFirstMeunClassID = menuclassitem.ID_Menu; }


    });
    // 显示菜单项（显示所有分组后的详细信息）
    jQuery("#ShowUserMenuDiv").html(LoginUserMenuClassSubItemStrs);

    // 显示第一个分组 
    // jQuery("#LoginUserMenuClassSubItem_" + tmpFirstMeunClassID).show();

    // 判断是否有需要显示“显示全部”按钮的分组
    if (strShowMoreBtnClassID != "") {
        var showMoreBtnArray = strShowMoreBtnClassID.split(",");
        for (var i = 0; i < showMoreBtnArray.length; i++) {
            if (showMoreBtnArray[i] != "") {
                jQuery("#ShowAllLoginUserMenu_" + showMoreBtnArray[i]).show();
            }
        }
    }

    jQuery(".menu-actions li a").click(function () {
        //remove掉同级元素样式
        jQuery(".menu-actions li").removeClass("selected");
        jQuery(this).parent().addClass("selected");

        SetCookie('SubMenuID', jQuery(this).attr("menuid"));       // 点击菜单项，保存对应的menuid
        SetCookie('SubSectionID', jQuery(this).attr("sectionid")); // 点击菜单项，保存对应的sectionid （如果不是分检，则该值为空）

        DoLoad(jQuery(this).attr("targeturl"), '');
    });


    // 获取用户相应的权限
    GetUserPasswordRight();

    // 获取当前跳转地址 20130803 by WTang
    GetUserRedirectUrl();

}

/// <summary>
/// 切换分组信息
/// </summary>
function ShowUserMenuClass(CurrentMenuID) {

    if (GetCookie('SaveMenuID') != ""
    && GetCookie('SaveMenuID') != undefined
    && GetCookie('SaveMenuID_Old') != CurrentMenuID
    && GetCookie('SaveMenuID') != CurrentMenuID) {
        DoLoad("", ''); // 点击一级菜单后，默认显示首页信息

        // 清除二级菜单的选中项
        jQuery(".menu-actions li").removeClass("selected");
        SetCookie('SubMenuID', 0);       // 点击菜单项，保存对应的menuid
        SetCookie('SubSectionID', 0); // 点击菜单项，保存对应的sectionid （如果不是分检，则该值为空）

    }

    if (jQuery("#LoginUserMenuClassSubItem_" + CurrentMenuID).is(":visible") == true) {
        SetCookie('SaveMenuID_Old', CurrentMenuID); // 上次点击的一级菜单的ID
        SetCookie('SaveMenuID', "0"); // 隐藏当前点击的二级菜单
        jQuery("#LoginUserMenuClassTitle_" + CurrentMenuID).removeClass("selected");   // 选中当前点击的标题
        jQuery("#LoginUserMenuClassSubItem_" + CurrentMenuID).hide();               // 显示当前点击菜单子项
        return;
    } else {
        SetCookie('SaveMenuID_Old', "0"); // 上次点击的一级菜单的ID
        SetCookie('SaveMenuID', CurrentMenuID); // 将当前的MenuID保存到Cookie中。
    }

    // 循环菜单分类
    jQuery(gLoginUserMenuClass).each(function (k, menuclassitem) {
        jQuery("#LoginUserMenuClassTitle_" + menuclassitem.MenuID).removeClass("selected");    // 标题的选中效果取消
        jQuery("#LoginUserMenuClassSubItem_" + menuclassitem.MenuID).hide();                   // 隐藏对应的菜单子项
    });
    jQuery("#LoginUserMenuClassTitle_" + CurrentMenuID).addClass("selected");   // 选中当前点击的标题
    jQuery("#LoginUserMenuClassSubItem_" + CurrentMenuID).show();               // 显示当前点击菜单子项

    //jQuery("#loadForm").html("<div>&nbsp;</div>");        // 当切换分组的时候，清空页面内容区域加载的数据

    // 隐藏侧边栏的链接 20130822 by WTang 
    jQuery("#listSectionExamedCount").hide();   // 隐藏已检查科室列表
    jQuery("#listSectionExamFloat").hide();     // 隐藏客户科室列表
    jQuery("#listHistoryExamCount").hide();     // 隐藏历史体检号对比列表


    jQuery("#divGotoTop").hide();             // 隐藏 Top
    jQuery("#divSectionExamedCount").hide();  // 隐藏 结果
    jQuery("#divHistoryExamCount").hide();    // 隐藏 对比
    jQuery("#divSectionExamFloat").hide();    // 隐藏 换科室

    // // SetContentLoadFormHeight(); // 设置加载区域的高度


}

var menutimer, menustartTimer;
function HideSubMenu() {


    //     暂时不采用鼠标移开后隐藏二级菜单的功能。
    /*
    jQuery(".LoginUserMenuClassSubItem").unbind();
    var timer;

    $(".LoginUserMenuClassSubItem").bind("mouseover", function () {
    if (timer) {
    clearTimeout(timer);
    timer = undefined;
    }
    }).bind("mouseout", function (e) {
    e.stopPropagation();
    var $t = $(this);
    timer = setTimeout(function () {
    $t.hide();
    }, 300);
    return false;
    });
    */
}
// ================================ 登录用户的菜单列表,权限列表，分检科室列表 ==== end ==================================================



// ================================ 年龄计算 ==== start ==================================================

function calculateYear(dateText) {
    var aDate = dateText.split("-");
    var birthdayYear = parseInt(aDate[0]);
    var curDate = new Date();
    var curYear = parseInt(curDate.getFullYear());
    return curYear - birthdayYear;
}
function calculateMonth(dateText) {
    var month = 1;
    var aDate = dateText.split("-");
    if (aDate[1].substr(0, 1) == "0") {
        aDate[1] = aDate[1].substring(1);
    }
    var birthdayMonth = parseInt(aDate[1]);
    var curDate = new Date();
    var curMonth = parseInt(curDate.getMonth() + 1);
    month = curMonth - birthdayMonth;
    return month;
}
function catcAge(dateText) {
    var birthDay = new Date(dateText.replace(/-/g, "\/"));
    var d = new Date();
    var age = d.getFullYear() - birthDay.getFullYear() - ((d.getMonth() < birthDay.getMonth() ||
    d.getMonth() == birthDay.getMonth() && d.getDate() < birthDay.getDate()) ? 1 : 0);
    var month = calculateMonth(dateText);
    var year = calculateYear(dateText);
    if (year >= 0) {
        if (month <= 0 && year == 1) {
            return 0;
        }
        else {
            return year;
        }
    }
    else {
        return -1;
    }
}
/// <summary>
/// 计算年龄 按具体出生日期进行计算年龄
/// 修改人：xmhuang
/// 修改日期：2015-04-20
function GetAgeByBirthDay(DateOne, DateTwo) {
    var OneMonth = DateOne.substring(5, DateOne.lastIndexOf('-'));
    var OneDay = DateOne.substring(DateOne.length, DateOne.lastIndexOf('-') + 1);
    var OneYear = DateOne.substring(0, DateOne.indexOf('-'));

    var TwoMonth = DateTwo.substring(5, DateTwo.lastIndexOf('-'));
    var TwoDay = DateTwo.substring(DateTwo.length, DateTwo.lastIndexOf('-') + 1);
    var TwoYear = DateTwo.substring(0, DateTwo.indexOf('-'));

    var cha = ((Date.parse(OneMonth + '/' + OneDay + '/' + OneYear) - Date.parse(TwoMonth + '/' + TwoDay + '/' + TwoYear)) / 86400000 / 365.25);
    var age = cha + "."// parseInt(cha); // Math.abs(cha)+ "";
    var curAge = age.split(".")[0];

    if (curAge == "NaN" || curAge == NaN) {
        curAge = 0;
    }
    return parseInt(curAge); //必须转换为int类型，否则将认为是字符进行比较如 40>100的情况出现
}
// ================================ 年龄计算 ==== end ==================================================




// ================================ 用户头像获取 ==== start ==================================================

/// <summary>
///  通用方法，获取图片的64位编码
/// </summary>
function GetImageCodeBase64(path) {

    var FastReport = document.getElementById("FastReport");
    var SignatureCodeBase64 = FastReport.GetImageCodeBase64(path);
    return SignatureCodeBase64;
}

// ================================ 用户头像获取 ==== start ==================================================




// ================================ 用户快速选择函数区域 ==== start ==================================================

/// <summary>
/// 隐藏，显示快速查询用户列表
/// </summary>
function ShowHideQuickQueryUserTable(flag, InputCode) {
    if (flag == true) {
        jQuery("#QuickQueryUserTable").show();
    } else {
        jQuery("#QuickQueryUserTable").hide();
    }
}

var gAllUserDataList = "";    // 保存全部的用户列表，前台输入输入码后，在这个列表中进行过滤，然后显示即可。
var OldUserInputCode = "****";              // 记录上次输入的输入码
var gAllUserListContent = ""; // 保存查询条件为空时，显示的信息，避免每次去执行替换。
/// <summary>
/// 根据输入码查询用户（通过Ajax后台过滤）
/// VocationType 1:医生 2:护士 3:其他工作人员
/// </summary>
function QuickQueryUserTableData_Ajax(VocationType) {

    var curEvent = window.event || e;
    if (curEvent.keyCode == 13 || curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {

        jQuery("#chkUser_" + jQuery("#tempSelectedUserID").val() + "").attr('checked', true);
        jQuery("#chkUser_" + jQuery("#tempSelectedUserID").val() + "").focus();
    }

    var InputCode = jQuery('#txtUserInputCode').val();
    if (OldUserInputCode != InputCode) {
        OldUserInputCode = InputCode;
    }

    //    else {
    //        ShowHideQuickQueryUserTable(true, InputCode);
    //        return;
    //    }
    //    

    var SectionID = jQuery('#txtSectionID').val(); // 当前科室
    var IsShowCurrSectionDoctor = jQuery('#IsShowCurrSectionDoctor').val(); // 是否只显示当前科室的医生

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxUser.aspx",
        data: { InputCode: InputCode,
            SectionID: SectionID,
            IsShowCurrSectionDoctor: IsShowCurrSectionDoctor,
            VocationType: VocationType,        // 0:全部 1:医生 2:护士 3:其他工作人员
            action: 'GetQuickUserList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            // 显示查询到的用户
            ShowQuickQueryUserTableData_Ajax(jsonmsg, InputCode);
        }
    });

}


/// <summary>
/// 根据查询结果数据，显示用户（通过Ajax过滤）
/// </summary>
function ShowQuickQueryUserTableData_Ajax(Userlist) {
    if (Userlist == "" || Userlist.totalCount == 0) {

        ShowHideQuickQueryUserTable(true, jQuery('#txtUserInputCode').val());
        // 显示没有找到用户信息
        jQuery("#QuickQueryUserTableData").html(jQuery('#EmptyUserQuickQueryDataTemplete').html());
    }
    else {
        var conclusionContent = ""; //用户table内容

        var UserQuickQueryTableTempleteContent = jQuery('#UserQuickQueryTableTemplete').html();             //快速查询用户列表模版
        if (UserQuickQueryTableTempleteContent == undefined) { return; }
        var CurrQueryCount = 0; // 满足当前查询条件的数据条数。
        var currCodeIndex = 0;

        if (Userlist != "") {
            jQuery("#tempSelectedUserID").val("");
            jQuery(Userlist.dataList).each(function (j, Useritem) {
                // 如果字符串不包含这个输入码，则继续下一条数据的判断

                CurrQueryCount++;
                conclusionContent += UserQuickQueryTableTempleteContent
                            .replace(/@UserID/gi, Useritem.UserID)
                            .replace(/@UserName/gi, Useritem.UserName)
                            .replace(/@InputCode/gi, Useritem.LoginName)
                            .replace(/@chkUserQueryList/gi, "chkUserQueryList")
                            ;
                if (CurrQueryCount == 1) {
                    jQuery("#tempSelectedUserID").val(Useritem.UserID);
                }
            });
        }
        if (conclusionContent != "") {
            ShowHideQuickQueryUserTable(true, jQuery('#txtUserInputCode').val());
            jQuery("#QuickQueryUserTableData").html(conclusionContent);
        } else {
            ShowHideQuickQueryUserTable(false);
            jQuery("#QuickQueryUserTableData").html("");
        }
    }

}


/// <summary>
/// 点击确定按钮（确定选择用户）
/// </summary>
function SelectUserDataList() {

    jQuery("input[name='chkUserQueryList']:radio:checked").each(function () {
        ShowQuickSelectUser(jQuery(this).val(), jQuery(this).attr("Username"));
    });

    ShowHideQuickQueryUserTable(false);
    SetSelectedUserCallBack();
}

/// <summary>
/// 删除选择的用户
/// </summary>
function RemoveSelectedUser() {
    jQuery('#spanSelectUser').hide();
    jQuery('#spanUser').show();
    jQuery('#spanSelectUser').html('');
    jQuery('#idSelectUser').val('');
    jQuery('#nameSelectUser').val('');

}


/// <summary> 
/// 选择用户后的回调函数
/// </summary>
function SetSelectedUserCallBack() {

}

/// <summary> 
/// 点击选中对应用户的单选按钮（快速选择列表）
/// </summary>
function SetUserChecked(UserID) {
    jQuery("#chkUser_" + UserID).attr("checked", true);
    SelectQueryUser(UserID);

    ShowHideQuickQueryUserTable(false, '');
    SetSelectedUserCallBack();
}


/// <summary> 
/// 点击用户列（快速选择列表）
/// </summary>
function UserClick(UserID) {
    jQuery("#chkUser_" + UserID).attr("checked", true);
    jQuery("#chkUser_" + UserID).focus();
    UserClickBackground(UserID);
}

/// <summary> 
/// 点击用户列（快速选择列表）
/// </summary>
function UserClickBackground(UserID) {
    jQuery("#QuickQueryUserTableData tr").removeClass();

    jQuery("#trUser_" + UserID).addClass("SelectedItem");
}


/// <summary> 
/// 选择用户（快速选择）
/// </summary>
function SelectQueryUser(UserID) {

    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedUserLableTemplete').html();
    if (templateContent == undefined) { return; }
    var tempUserName = jQuery("#chkUser_" + UserID).attr("UserName");

    var newcontent = templateContent.replace(/@UserName/gi, tempUserName); // 替换模版中的关键字

    jQuery('#spanSelectUser').html(newcontent);   // 显示到页面
    jQuery('#spanUser').hide();   // 显示到页面
    jQuery('#spanSelectUser').show();   // 显示到页面
    jQuery('#idSelectUser').val(UserID);         // 选择的用户ID
    jQuery('#nameSelectUser').val(tempUserName);  // 选择的用户名称

}


/// <summary> 
/// 显示快速选择的用户名
/// </summary>
function ShowQuickSelectUser(UserID, UserName) {
    if (UserName == "") {
        RemoveSelectedUser();
        return;
    }
    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedUserLableTemplete').html();
    if (templateContent == undefined) { return; }

    var newcontent = templateContent.replace(/@UserName/gi, UserName); // 替换模版中的关键字

    jQuery('#spanSelectUser').html(newcontent);     // 显示到页面
    jQuery('#spanUser').hide();                     // 隐藏
    jQuery('#spanSelectUser').show();               // 显示到页面
    jQuery('#idSelectUser').val(UserID);           // 选择的用户ID  （隐藏值）
    jQuery('#nameSelectUser').val(UserName);        // 选择的用户名称（隐藏值）

}


// ================================ 用户快速选择函数区域 ==== end ==================================================



// ================================ 民族快速选择函数区域 ==== start ==================================================

/// <summary>
/// 隐藏，显示快速查询民族列表
/// </summary>
function ShowHideQuickQueryNationTable(flag, InputCode) {
    if (flag == true) {
        jQuery("#QuickQueryNationTable").show();
    } else {
        jQuery("#QuickQueryNationTable").hide();
    }
}

var gAllNationDataList = "";    // 保存全部的民族列表，前台输入输入码后，在这个列表中进行过滤，然后显示即可。
var OldNationInputCode = "****";              // 记录上次输入的输入码
var gAllNationListContent = ""; // 保存查询条件为空时，显示的信息，避免每次去执行替换。
/// <summary>
/// 根据输入码查询民族（通过Ajax后台过滤）
/// NationVocationType 1:医生 2:护士 3:其他工作人员
/// </summary>
function QuickQueryNationTableData_Ajax() {
    var curEvent = window.event || e;
    if (curEvent.keyCode == 13 || curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {

        jQuery("#tempSelectedNationID").val().attr('checked', true);
        jQuery("#tempSelectedNationID").val().focus();
    }

    var InputCode = jQuery('#txtNationInputCode').val();
    if (OldNationInputCode != InputCode) {
        OldNationInputCode = InputCode;
    } else {
        ShowHideQuickQueryNationTable(true, InputCode);
        return;
    }

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxConfig.aspx",
        data: { InputCode: InputCode,
            action: 'GetQuickNationList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            // 显示查询到的民族
            ShowQuickQueryNationTableData_Ajax(jsonmsg, InputCode);
        }
    });

}


/// <summary>
/// 根据查询结果数据，显示民族（通过Ajax过滤）
/// </summary>
function ShowQuickQueryNationTableData_Ajax(Nationlist) {
    if (Nationlist == "" || Nationlist.totalCount == 0) {

        ShowHideQuickQueryNationTable(true, jQuery('#txtNationInputCode').val());
        // 显示没有找到民族信息
        jQuery("#QuickQueryNationTableData").html(jQuery('#EmptyNationQuickQueryDataTemplete').html());
    }
    else {

        var conclusionContent = ""; //民族table内容

        var NationQuickQueryTableTempleteContent = jQuery('#NationQuickQueryTableTemplete').html();             //快速查询民族列表模版
        if (NationQuickQueryTableTempleteContent == undefined) { return; }
        var CurrQueryCount = 0; // 满足当前查询条件的数据条数。
        var currCodeIndex = 0;

        if (Nationlist != "") {
            jQuery("#tempSelectedNationID").val("");
            jQuery(Nationlist.dataList).each(function (j, Nationitem) {
                // 如果字符串不包含这个输入码，则继续下一条数据的判断

                CurrQueryCount++;
                conclusionContent += NationQuickQueryTableTempleteContent
                            .replace(/@NationID/gi, Nationitem.NationID)
                            .replace(/@NationName/gi, Nationitem.NationName)
                            .replace(/@InputCode/gi, Nationitem.InputCode)
                            .replace(/@chkNationQueryList/gi, "chkNationQueryList")
                            ;
                if (CurrQueryCount == 1) {
                    jQuery("#tempSelectedNationID").val(Nationitem.NationID);
                }
            });


        }
        if (conclusionContent != "") {
            ShowHideQuickQueryNationTable(true, jQuery('#txtNationInputCode').val());
            jQuery("#QuickQueryNationTableData").html(conclusionContent);
        } else {
            ShowHideQuickQueryNationTable(false);
            jQuery("#QuickQueryNationTableData").html("");
        }
    }

}


/// <summary>
/// 点击确定按钮（确定选择民族）
/// </summary>
function SelectNationDataList() {

    jQuery("input[name='chkNationQueryList']:radio:checked").each(function () {
        ShowQuickSelectNation(jQuery(this).val(), jQuery(this).attr("Nationname"));
    });

    ShowHideQuickQueryNationTable(false);
}

/// <summary>
/// 删除选择的民族
/// </summary>
function RemoveSelectedNation() {
    jQuery('#spanSelectNation').hide();
    jQuery('#spanNation').show();
    jQuery('#spanSelectNation').html('');
    jQuery('#idSelectNation').val('');
    jQuery('#nameSelectNation').val('');

}


/// <summary> 
/// 点击选中对应民族的单选按钮（快速选择列表）
/// </summary>
function SetNationChecked(NationID) {
    jQuery("#chkNation_" + NationID).attr("checked", true);
    SelectQueryNation(NationID);

    ShowHideQuickQueryNationTable(false, '');
}


/// <summary> 
/// 点击民族列（快速选择列表）
/// </summary>
function NationClick(NationID) {
    jQuery("#chkNation_" + NationID).attr("checked", true);
    jQuery("#chkNation_" + NationID).focus();
    NationClickBackground(NationID);
}

/// <summary> 
/// 点击民族列（快速选择列表）
/// </summary>
function NationClickBackground(NationID) {
    jQuery("#QuickQueryNationTableData tr").removeClass();

    jQuery("#trNation_" + NationID).addClass("SelectedItem");
}

/// <summary> 
/// 选择民族（快速选择）
/// </summary>
function SelectQueryNation(NationID) {

    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedNationLableTemplete').html();
    if (templateContent == undefined) { return; }
    var tempNationName = jQuery("#chkNation_" + NationID).attr("NationName");

    var newcontent = templateContent.replace(/@NationName/gi, tempNationName); // 替换模版中的关键字

    jQuery('#spanSelectNation').html(newcontent);   // 显示到页面
    jQuery('#spanNation').hide();   // 显示到页面
    jQuery('#spanSelectNation').show();   // 显示到页面
    jQuery('#idSelectNation').val(NationID);         // 选择的民族ID
    jQuery('#nameSelectNation').val(tempNationName);  // 选择的民族名称

}


/// <summary> 
/// 显示快速选择的民族名
/// </summary>
function ShowQuickSelectNation(NationID, NationName) {
    if (NationName == "" || NationName == undefined) { return; }
    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedNationLableTemplete').html();
    if (templateContent == undefined) { return; }

    var newcontent = templateContent.replace(/@NationName/gi, NationName); // 替换模版中的关键字

    jQuery('#spanSelectNation').html(newcontent);     // 显示到页面
    jQuery('#spanNation').hide();                     // 隐藏
    jQuery('#spanSelectNation').show();               // 显示到页面
    jQuery('#idSelectNation').val(NationID);           // 选择的民族ID  （隐藏值）
    jQuery('#nameSelectNation').val(NationName);        // 选择的民族名称（隐藏值）

}


// ================================ 民族快速选择函数区域 ==== end ==================================================


// ================================ 团体快速选择函数区域 ==== start ==================================================

/// <summary>
/// 隐藏，显示快速查询团体列表
/// </summary>
function ShowHideQuickQueryTeamTable(flag, InputCode) {
    if (flag == true) {
        jQuery("#QuickQueryTeamTable").show();
    } else {
        jQuery("#QuickQueryTeamTable").hide();
    }
}

var gAllTeamDataList = "";    // 保存全部的团体列表，前台输入输入码后，在这个列表中进行过滤，然后显示即可。
var OldTeamInputCode = "****";              // 记录上次输入的输入码
var gAllTeamListContent = ""; // 保存查询条件为空时，显示的信息，避免每次去执行替换。
/// <summary>
/// 根据输入码查询团体（通过Ajax后台过滤）
/// </summary>
function QuickQueryTeamTableData_Ajax() {

    var curEvent = window.event || e;
    if (curEvent.keyCode == 13 || curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {

        jQuery("#chkTeam_" + jQuery("#tempSelectedTeamID").val() + "").attr('checked', true);
        jQuery("#chkTeam_" + jQuery("#tempSelectedTeamID").val() + "").focus();
    }

    var InputCode = jQuery('#txtTeamInputCode').val();
    if (OldTeamInputCode != InputCode) {
        OldTeamInputCode = InputCode;
    } else {
        ShowHideQuickQueryTeamTable(true, InputCode);
        return;
    }

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { InputCode: InputCode,
            action: 'GetQuickTeamList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            // 显示查询到的团体
            ShowQuickQueryTeamTableData_Ajax(jsonmsg, InputCode);
        },
        
    });

}


/// <summary>
/// 根据查询结果数据，显示团体（通过Ajax过滤）
/// </summary>
function ShowQuickQueryTeamTableData_Ajax(Teamlist) {
    if (Teamlist == "" || Teamlist.totalCount == 0) {

        ShowHideQuickQueryTeamTable(true, jQuery('#txtTeamInputCode').val());
        // 显示没有找到团体信息
        jQuery("#QuickQueryTeamTableData").html(jQuery('#EmptyTeamQuickQueryDataTemplete').html());
    }
    else {

        var conclusionContent = ""; //团体table内容

        var TeamQuickQueryTableTempleteContent = jQuery('#TeamQuickQueryTableTemplete', parent.document).html();             //快速查询团体列表模版
        if (TeamQuickQueryTableTempleteContent == undefined) { return; }
        var CurrQueryCount = 0; // 满足当前查询条件的数据条数。
        var currCodeIndex = 0;

        if (Teamlist != "") {
            jQuery("#tempSelectedTeamID").val("");
            jQuery(Teamlist.dataList).each(function (j, Teamitem) {
                // 如果字符串不包含这个输入码，则继续下一条数据的判断

                CurrQueryCount++;
                conclusionContent += TeamQuickQueryTableTempleteContent
                            .replace(/@ID_Team/gi, Teamitem.ID_Team)
                            .replace(/@TeamName/gi, Teamitem.TeamName)
                            .replace(/@InputCode/gi, Teamitem.InputCode)
                            .replace(/@chkTeamQueryList/gi, "chkTeamQueryList")
                            ;
                if (CurrQueryCount == 1) {
                    jQuery("#tempSelectedTeamID").val(Teamitem.ID_Team);
                }
            });


        }
        if (conclusionContent != "") {
            ShowHideQuickQueryTeamTable(true, jQuery('#txtTeamInputCode').val());
            jQuery("#QuickQueryTeamTableData").html(conclusionContent);
        } else {
            ShowHideQuickQueryTeamTable(false);
            jQuery("#QuickQueryTeamTableData").html("");
        }
    }

}


/// <summary>
/// 点击确定按钮（确定选择团体）
/// </summary>
function SelectTeamDataList() {

    jQuery("input[name='chkTeamQueryList']:radio:checked").each(function () {
        ShowQuickSelectTeam(jQuery(this).val(), jQuery(this).attr("Teamname"));
    });

    ShowHideQuickQueryTeamTable(false);
}

/// <summary>
/// 删除选择的团体
/// </summary>
function RemoveSelectedTeam() {
    jQuery('#spanSelectTeam').hide();
    jQuery('#spanTeam').show();
    jQuery('#spanSelectTeam').html('');
    jQuery('#idSelectTeam').val('');
    jQuery('#nameSelectTeam').val('');
    RemoveSelectedTeamTask(); // 清空任务已选择的值

}


/// <summary> 
/// 点击选中对应团体的单选按钮（快速选择列表）
/// </summary>
function SetTeamChecked(ID_Team) {
    jQuery("#chkTeam_" + ID_Team).attr("checked", true);
    SelectQueryTeam(ID_Team);
    RemoveSelectedTeamTask(); // 清空任务已选择的值
    ShowHideQuickQueryTeamTable(false, '');
}


/// <summary> 
/// 点击团体列（快速选择列表）
/// </summary>
function TeamClick(ID_Team) {
    jQuery("#chkTeam_" + ID_Team).attr("checked", true);
    jQuery("#chkTeam_" + ID_Team).focus();
    TeamClickBackground(ID_Team);
}

/// <summary> 
/// 点击团体列（快速选择列表）
/// </summary>
function TeamClickBackground(ID_Team) {
    jQuery("#QuickQueryTeamTableData tr").removeClass();

    jQuery("#trTeam_" + ID_Team).addClass("SelectedItem");
}

/// <summary> 
/// 选择团体（快速选择）
/// </summary>
function SelectQueryTeam(ID_Team) {
    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedTeamLableTemplete ',parent.document).html();
    if (templateContent == undefined) { return; }
    var tempTeamName = jQuery("#chkTeam_" + ID_Team).attr("TeamName");
    jQuery('#idSelectTeam').val(ID_Team);         // 选择的团体ID
    jQuery('#nameSelectTeam').val(tempTeamName);  // 选择的团体名称
    var tempTitleTeamName = tempTeamName;
    if (tempTeamName.length > 7) {
        tempTeamName = tempTeamName.substr(tempTeamName.length - 8, 8);
    }
    var newcontent = templateContent.replace(/@TeamName/gi, tempTeamName).replace(/@TitleTeamName/gi, tempTitleTeamName); // 替换模版中的关键字

    jQuery('#spanSelectTeam').html(newcontent);   // 显示到页面
    jQuery('#spanTeam').hide();   // 显示到页面
    jQuery('#spanSelectTeam').show();   // 显示到页面

    TeamCallBack();


}


/// <summary> 
/// 显示快速选择的团体名
/// </summary>
function ShowQuickSelectTeam(ID_Team, TeamName) {
    if (TeamName == "") { return; }
    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedTeamLableTemplete',parent.document).html();
    if (templateContent == undefined) { return; }

    var newcontent = templateContent.replace(/@TeamName/gi, TeamName)
                    .replace(/@TitleTeamName/gi, TeamName); //替换模版中的Title关键字 xmhuang 2014-04-18

    jQuery('#spanSelectTeam').html(newcontent);     // 显示到页面
    jQuery('#spanTeam').hide();                     // 隐藏
    jQuery('#spanSelectTeam').show();               // 显示到页面
    jQuery('#idSelectTeam').val(ID_Team);           // 选择的团体ID  （隐藏值）
    jQuery('#nameSelectTeam').val(TeamName);        // 选择的团体名称（隐藏值）

}


// ================================ 团体快速选择函数区域 ==== end ==================================================





// ================================ 团体任务快速选择函数区域 ==== start ==================================================

/// <summary>
/// 隐藏，显示快速查询团体任务列表
/// </summary>
function ShowHideQuickQueryTeamTaskTable(flag, InputCode) {
    if (flag == true) {
        jQuery("#QuickQueryTeamTaskTable").show();
    } else {
        jQuery("#QuickQueryTeamTaskTable").hide();
    }
}

var gAllTeamTaskDataList = "";    // 保存全部的团体任务列表，前台输入输入码后，在这个列表中进行过滤，然后显示即可。
var OldTeamTaskInputCode = "****";              // 记录上次输入的输入码
var gAllTeamTaskListContent = ""; // 保存查询条件为空时，显示的信息，避免每次去执行替换。
var OldCurrSelectedTeamID = "****";             // 记录上次选择的团队ID
/// <summary>
/// 根据输入码查询团体任务（通过Ajax后台过滤）
/// </summary>
function QuickQueryTeamTaskTableData_Ajax() {

    var curEvent = window.event || e;
    if (curEvent.keyCode == 13 || curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {

        jQuery("#chkTeamTask_" + jQuery("#tempSelectedTeamTaskID").val() + "").attr('checked', true);
        jQuery("#chkTeamTask_" + jQuery("#tempSelectedTeamTaskID").val() + "").focus();
    }

    var InputCode = jQuery('#txtTeamTaskInputCode').val();
    var CurrSelectedTeamID = jQuery("#idSelectTeam").val(); // 当前选择的团队ID

    if (OldTeamTaskInputCode != InputCode || OldCurrSelectedTeamID != CurrSelectedTeamID) {
        OldTeamTaskInputCode = InputCode;
        OldCurrSelectedTeamID = CurrSelectedTeamID;
    } else {
        ShowHideQuickQueryTeamTaskTable(true, InputCode);
        return;
    }

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxTeamOper.aspx",
        data: { InputCode: InputCode,
            CurrSelectedTeamID: CurrSelectedTeamID,
            action: 'GetQuickTeamTaskList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            // 显示查询到的团体任务
            ShowQuickQueryTeamTaskTableData_Ajax(jsonmsg, InputCode);
        }
    });

}


/// <summary>
/// 根据查询结果数据，显示团体任务（通过Ajax过滤）
/// </summary>
function ShowQuickQueryTeamTaskTableData_Ajax(TeamTasklist) {
    if (TeamTasklist == "" || TeamTasklist.totalCount == 0) {

        ShowHideQuickQueryTeamTaskTable(true, jQuery('#txtTeamTaskInputCode').val());
        // 显示没有找到团体任务信息
        jQuery("#QuickQueryTeamTaskTableData").html(jQuery('#EmptyTeamTaskQuickQueryDataTemplete').html());
    }
    else {

        var conclusionContent = ""; //团体任务table内容

        var TeamTaskQuickQueryTableTempleteContent = jQuery('#TeamTaskQuickQueryTableTemplete',parent.document).html();             //快速查询团体任务列表模版
        if (TeamTaskQuickQueryTableTempleteContent == undefined) { return; }
        var CurrQueryCount = 0; // 满足当前查询条件的数据条数。
        var currCodeIndex = 0;

        if (TeamTasklist != "") {
            jQuery("#tempSelectedTeamTaskID").val("");
            jQuery(TeamTasklist.dataList).each(function (j, TeamTaskitem) {
                // 如果字符串不包含这个输入码，则继续下一条数据的判断

                CurrQueryCount++;
                conclusionContent += TeamTaskQuickQueryTableTempleteContent
                            .replace(/@ID_TeamTask/gi, TeamTaskitem.ID_TeamTask)
                            .replace(/@TeamTaskName/gi, TeamTaskitem.TeamTaskName)
                            .replace(/@InputCode/gi, TeamTaskitem.InputCode)
                            .replace(/@chkTeamTaskQueryList/gi, "chkTeamTaskQueryList")
                            ;
                if (CurrQueryCount == 1) {
                    jQuery("#tempSelectedTeamTaskID").val(TeamTaskitem.ID_TeamTask);
                }
            });


        }
        if (conclusionContent != "") {
            ShowHideQuickQueryTeamTaskTable(true, jQuery('#txtTeamTaskInputCode').val());
            jQuery("#QuickQueryTeamTaskTableData").html(conclusionContent);
        } else {
            ShowHideQuickQueryTeamTaskTable(false);
            jQuery("#QuickQueryTeamTaskTableData").html("");
        }
    }

}


/// <summary>
/// 点击确定按钮（确定选择团体任务）
/// </summary>
function SelectTeamTaskDataList() {

    jQuery("input[name='chkTeamTaskQueryList']:radio:checked").each(function () {
        ShowQuickSelectTeamTask(jQuery(this).val(), jQuery(this).attr("TeamTaskname"));
    });

    ShowHideQuickQueryTeamTaskTable(false);
}

/// <summary>
/// 删除选择的团体任务
/// </summary>
function RemoveSelectedTeamTask() {
    jQuery('#spanSelectTeamTask').hide();
    jQuery('#spanTeamTask').show();
    jQuery('#spanSelectTeamTask').html('');
    jQuery('#idSelectTeamTask').val('');
    jQuery('#nameSelectTeamTask').val('');

}


/// <summary> 
/// 点击选中对应团体任务的单选按钮（快速选择列表）
/// </summary>
function SetTeamTaskChecked(ID_TeamTask) {
    jQuery("#chkTeamTask_" + ID_TeamTask).attr("checked", true);
    SelectQueryTeamTask(ID_TeamTask);

    ShowHideQuickQueryTeamTaskTable(false, '');
}


/// <summary> 
/// 点击团体任务列（快速选择列表）
/// </summary>
function TeamTaskClick(TeamTaskID) {
    jQuery("#chkTeamTask_" + TeamTaskID).attr("checked", true);
    jQuery("#chkTeamTask_" + TeamTaskID).focus();
    TeamTaskClickBackground(TeamTaskID);
}

/// <summary> 
/// 点击团体任务列（快速选择列表）
/// </summary>
function TeamTaskClickBackground(ID_TeamTask) {
    jQuery("#QuickQueryTeamTaskTableData tr").removeClass();

    jQuery("#trTeamTask_" + ID_TeamTask).addClass("SelectedItem");
}

/// <summary> 
/// 选择团体任务（快速选择）
/// </summary>
function SelectQueryTeamTask(ID_TeamTask) {

    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedTeamTaskLableTemplete',parent.document).html();
    if (templateContent == undefined) { return; }
    var tempTeamTaskName = jQuery("#chkTeamTask_" + ID_TeamTask).attr("TeamTaskName");

    jQuery('#idSelectTeamTask').val(ID_TeamTask);         // 选择的团体任务ID
    jQuery('#nameSelectTeamTask').val(tempTeamTaskName);  // 选择的团体任务名称

    var tempTitleTeamTaskName = tempTeamTaskName;
    if (tempTeamTaskName.length > 7) {
        tempTeamTaskName = tempTeamTaskName.substr(tempTeamTaskName.length - 8, 8);
    }
    var newcontent = templateContent.replace(/@TeamTaskName/gi, tempTeamTaskName).replace(/@TitleTeamTaskName/gi, tempTitleTeamTaskName); // 替换模版中的关键字

    jQuery('#spanSelectTeamTask').html(newcontent);   // 显示到页面
    jQuery('#spanTeamTask').hide();   // 显示到页面
    jQuery('#spanSelectTeamTask').show();   // 显示到页面

    TeamTaskCallBack();
}
function TeamCallBack()
{ }
function TeamTaskCallBack() {

}

/// <summary> 
/// 显示快速选择的团体任务名
/// </summary>
function ShowQuickSelectTeamTask(ID_TeamTask, TeamTaskName) {
    if (TeamTaskName == "") { return; }
    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedTeamTaskLableTemplete', parent.document).html();
    if (templateContent == undefined) { return; }

    jQuery('#idSelectTeamTask').val(ID_TeamTask);           // 选择的团体任务ID  （隐藏值）
    jQuery('#nameSelectTeamTask').val(TeamTaskName);        // 选择的团体任务名称（隐藏值）

    var TitleTeamTaskName = TeamTaskName;
    if (TeamTaskName.length > 7) {
        TeamTaskName = TeamTaskName.substr(0, 8);
    }
    var newcontent = templateContent.replace(/@TeamTaskName/gi, TeamTaskName).replace(/@TitleTeamTaskName/gi, TitleTeamTaskName); // 替换模版中的关键字

    jQuery('#spanSelectTeamTask').html(newcontent);     // 显示到页面
    jQuery('#spanTeamTask').hide();                     // 隐藏
    jQuery('#spanSelectTeamTask').show();               // 显示到页面

}


// ================================ 团体任务快速选择函数区域 ==== end ==================================================




// ================================ 体检类型快速选择函数区域 ==== start ==================================================

/// <summary>
/// 隐藏，显示快速查询体检类型列表
/// </summary>
function ShowHideQuickQueryExamTypeTable(flag, InputCode) {
    if (flag == true) {
        jQuery("#QuickQueryExamTypeTable").show();
    } else {
        jQuery("#QuickQueryExamTypeTable").hide();
    }
}

var gAllExamTypeDataList = "";    // 保存全部的体检类型列表，前台输入输入码后，在这个列表中进行过滤，然后显示即可。
var OldExamTypeInputCode = "****";              // 记录上次输入的输入码
var gAllExamTypeListContent = ""; // 保存查询条件为空时，显示的信息，避免每次去执行替换。
/// <summary>
/// 根据输入码查询体检类型（通过Ajax后台过滤）
/// ExamTypeVocationType 1:医生 2:护士 3:其他工作人员
/// </summary>
function QuickQueryExamTypeTableData_Ajax() {

    var curEvent = window.event || e;
    if (curEvent.keyCode == 13 || curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {

        jQuery("#chkExamType_" + jQuery("#tempSelectedExamTypeID").val() + "").attr('checked', true);
        jQuery("#chkExamType_" + jQuery("#tempSelectedExamTypeID").val() + "").focus();
    }
    var InputCode = jQuery('#txtExamTypeInputCode').val();
    if (OldExamTypeInputCode != InputCode) {
        OldExamTypeInputCode = InputCode;
    } else {
        ShowHideQuickQueryExamTypeTable(true, InputCode);
        return;
    }

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxConfig.aspx",
        data: { InputCode: InputCode,
            action: 'GetQuickExamTypeList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            // 显示查询到的体检类型
            ShowQuickQueryExamTypeTableData_Ajax(jsonmsg, InputCode);
        }
    });

}


/// <summary>
/// 根据查询结果数据，显示体检类型（通过Ajax过滤）
/// </summary>
function ShowQuickQueryExamTypeTableData_Ajax(ExamTypelist) {
    if (ExamTypelist == "" || ExamTypelist.totalCount == 0) {

        ShowHideQuickQueryExamTypeTable(true, jQuery('#txtExamTypeInputCode').val());
        // 显示没有找到体检类型信息
        jQuery("#QuickQueryExamTypeTableData").html(jQuery('#EmptyExamTypeQuickQueryDataTemplete').html());
    }
    else {

        var conclusionContent = ""; //体检类型table内容

        var ExamTypeQuickQueryTableTempleteContent = jQuery('#ExamTypeQuickQueryTableTemplete').html();             //快速查询体检类型列表模版
        if (ExamTypeQuickQueryTableTempleteContent == undefined) { return; }
        var CurrQueryCount = 0; // 满足当前查询条件的数据条数。
        var currCodeIndex = 0;

        if (ExamTypelist != "") {

            jQuery("#tempSelectedExamTypeID").val("");
            jQuery(ExamTypelist.dataList).each(function (j, ExamTypeitem) {
                // 如果字符串不包含这个输入码，则继续下一条数据的判断

                CurrQueryCount++;
                conclusionContent += ExamTypeQuickQueryTableTempleteContent
                            .replace(/@ExamTypeID/gi, ExamTypeitem.ExamTypeID)
                            .replace(/@ExamTypeName/gi, ExamTypeitem.ExamTypeName)
                            .replace(/@InputCode/gi, ExamTypeitem.InputCode)
                            .replace(/@chkExamTypeQueryList/gi, "chkExamTypeQueryList")
                            ;
                if (CurrQueryCount == 1) {
                    jQuery("#tempSelectedExamTypeID").val(ExamTypeitem.ExamTypeID);
                }
            });
        }
        if (conclusionContent != "") {
            ShowHideQuickQueryExamTypeTable(true, jQuery('#txtExamTypeInputCode').val());
            jQuery("#QuickQueryExamTypeTableData").html(conclusionContent);
        } else {
            ShowHideQuickQueryExamTypeTable(false);
            jQuery("#QuickQueryExamTypeTableData").html("");
        }
    }

}


/// <summary>
/// 点击确定按钮（确定选择体检类型）
/// </summary>
function SelectExamTypeDataList() {
    jQuery("input[name='chkExamTypeQueryList']:radio:checked").each(function () {
        ShowQuickSelectExamType(jQuery(this).val(), jQuery(this).attr("ExamTypename"));
    });

    ShowHideQuickQueryExamTypeTable(false);
}

/// <summary>
/// 删除选择的体检类型
/// </summary>
function RemoveSelectedExamType() {
    jQuery('#spanSelectExamType').hide();
    jQuery('#spanExamType').show();
    jQuery('#spanSelectExamType').html('');
    jQuery('#idSelectExamType').val('');
    jQuery('#nameSelectExamType').val('');

}


/// <summary> 
/// 点击选中对应体检类型的单选按钮（快速选择列表）
/// </summary>
function SetExamTypeChecked(ID_ExamType) {
    jQuery("#chkExamType_" + ID_ExamType).attr("checked", true);
    SelectQueryExamType(ID_ExamType);

    ShowHideQuickQueryExamTypeTable(false, '');
}


/// <summary> 
/// 选择体检类型（快速选择）
/// </summary>
function SelectQueryExamType(ID_ExamType) {

    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedExamTypeLableTemplete').html();
    if (templateContent == undefined) { return; }
    var tempExamTypeName = jQuery("#chkExamType_" + ID_ExamType).attr("ExamTypeName");

    var newcontent = templateContent.replace(/@ExamTypeName/gi, tempExamTypeName); // 替换模版中的关键字

    jQuery('#spanSelectExamType').html(newcontent);   // 显示到页面
    jQuery('#spanExamType').hide();   // 显示到页面
    jQuery('#spanSelectExamType').show();   // 显示到页面
    jQuery('#idSelectExamType').val(ID_ExamType);         // 选择的体检类型ID
    jQuery('#nameSelectExamType').val(tempExamTypeName);  // 选择的体检类型名称

}

/// <summary> 
/// 点击体检类型列（快速选择列表）
/// </summary>
function ExamTypeClick(ExamTypeID) {
    jQuery("#chkExamType_" + ExamTypeID).attr("checked", true);
    jQuery("#chkExamType_" + ExamTypeID).focus();
    SetClickBackground(ExamTypeID);
}

/// <summary> 
/// 点击体检类型列（快速选择列表）
/// </summary>
function ExamTypeClickBackground(ExamTypeID) {
    jQuery("#QuickQueryExamTypeTableData tr").removeClass();

    jQuery("#trExamType_" + ExamTypeID).addClass("SelectedItem");
}


/// <summary> 
/// 显示快速选择的体检类型名
/// </summary>
function ShowQuickSelectExamType(ExamTypeID, ExamTypeName) {
    if (ExamTypeName == "") { return; }
    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedExamTypeLableTemplete').html();
    if (templateContent == undefined) { return; }

    var newcontent = templateContent.replace(/@ExamTypeName/gi, ExamTypeName); // 替换模版中的关键字

    jQuery('#spanSelectExamType').html(newcontent);     // 显示到页面
    jQuery('#spanExamType').hide();                     // 隐藏
    jQuery('#spanSelectExamType').show();               // 显示到页面
    jQuery('#idSelectExamType').val(ExamTypeID);           // 选择的体检类型ID  （隐藏值）
    jQuery('#nameSelectExamType').val(ExamTypeName);        // 选择的体检类型名称（隐藏值）

}


// ================================ 体检类型快速选择函数区域 ==== end ==================================================


// ================================ 套餐类型快速选择函数区域 ==== start ==================================================

/// <summary>
/// 隐藏，显示快速查询套餐类型列表
/// </summary>
function ShowHideQuickQuerySetTable(flag, InputCode) {
    if (flag == true) {
        jQuery("#QuickQuerySetTable").show();
    } else {
        jQuery("#QuickQuerySetTable").hide();
    }
}

var gAllSetDataList = "";    // 保存全部的套餐类型列表，前台输入输入码后，在这个列表中进行过滤，然后显示即可。
var OldSetInputCode = "****";              // 记录上次输入的输入码
var gAllSetListContent = ""; // 保存查询条件为空时，显示的信息，避免每次去执行替换。
var gSex = ""; // 保存上次查询的性别
/// <summary>
/// 根据输入码查询套餐类型（通过Ajax后台过滤）
/// SetVocationType 1:医生 2:护士 3:其他工作人员
/// </summary>
function QuickQuerySetTableData_Ajax() {

    var curEvent = window.event || e;
    if (curEvent.keyCode == 13 || curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {

        jQuery("#chkSet_" + jQuery("#tempSelectedSetID").val() + "").attr('checked', true);
        jQuery("#chkSet_" + jQuery("#tempSelectedSetID").val() + "").focus();
    }
    var InputCode = jQuery('#txtSetInputCode').val();
    var Sex = jQuery('#slSex').val();
    Sex = (Sex == 1 ? Sex : 0); //0：女性，1男性，2：共用

    if (gSex != Sex || OldSetInputCode != InputCode) {
        OldSetInputCode = InputCode;
        gSex = Sex;
    } else {
        ShowHideQuickQuerySetTable(true, InputCode);
        return;
    }

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxConfig.aspx",
        data: { InputCode: InputCode,
            Sex: Sex,
            action: 'GetQuickSetList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {
            // 显示查询到的套餐类型
            ShowQuickQuerySetTableData_Ajax(jsonmsg, InputCode);
        }
    });

}


/// <summary>
/// 根据查询结果数据，显示套餐类型（通过Ajax过滤）
/// </summary>
function ShowQuickQuerySetTableData_Ajax(Setlist) {
    if (Setlist == "" || Setlist.totalCount == 0) {

        ShowHideQuickQuerySetTable(true, jQuery('#txtSetInputCode').val());
        // 显示没有找到套餐类型信息
        jQuery("#QuickQuerySetTableData").html(jQuery('#EmptySetQuickQueryDataTemplete').html());
    }
    else {

        var conclusionContent = ""; //套餐类型table内容

        var SetQuickQueryTableTempleteContent = jQuery('#SetQuickQueryTableTemplete').html();             //快速查询套餐类型列表模版
        if (SetQuickQueryTableTempleteContent == undefined) { return; }
        var CurrQueryCount = 0; // 满足当前查询条件的数据条数。
        var currCodeIndex = 0;

        if (Setlist != "") {
            jQuery("#tempSelectedSetID").val("");
            jQuery(Setlist.dataList).each(function (j, Setitem) {
                // 如果字符串不包含这个输入码，则继续下一条数据的判断

                CurrQueryCount++;
                conclusionContent += SetQuickQueryTableTempleteContent
                            .replace(/@PEPackageID/gi, Setitem.PEPackageID)
                            .replace(/@PEPackageName/gi, Setitem.PEPackageName)
                            .replace(/@InputCode/gi, Setitem.InputCode)
                            .replace(/@chkSetQueryList/gi, "chkSetQueryList")
                            ;
                if (CurrQueryCount == 1) {
                    jQuery("#tempSelectedSetID").val(Setitem.PEPackageID);
                }
            });
        }
        if (conclusionContent != "") {
            ShowHideQuickQuerySetTable(true, jQuery('#txtSetInputCode').val());
            jQuery("#QuickQuerySetTableData").html(conclusionContent);
        } else {
            ShowHideQuickQuerySetTable(false);
            jQuery("#QuickQuerySetTableData").html("");
        }
    }

}


/// <summary>
/// 点击确定按钮（确定选择套餐类型）
/// </summary>
function SelectSetDataList() {
    jQuery("input[name='chkSetQueryList']:radio:checked").each(function () {
        ShowQuickSelectSet(jQuery(this).val(), jQuery(this).attr("PEPackageName"), true); // 套餐变动，重新读取数据
    });

    ShowHideQuickQuerySetTable(false);
}

/// <summary>
/// 删除选择的套餐类型
/// </summary>
function RemoveSelectedSet() {
    jQuery('#spanSelectSet').hide();
    jQuery('#spanSet').show();
    jQuery('#spanSelectSet').html('');
    jQuery('#idSelectSet').val('');
    jQuery('#nameSelectSet').val('');
}


/// <summary> 
/// 点击选中对应套餐类型的单选按钮（快速选择列表）
/// </summary>
function SetSetChecked(PEPackageID) {
    jQuery("#chkSet_" + PEPackageID).attr("checked", true);
    SelectQuerySet(PEPackageID);

    ShowHideQuickQuerySetTable(false, '');
}

/// <summary> 
/// 点击套餐类型列（快速选择列表）
/// </summary>
function SetClick(PEPackageID) {
    jQuery("#chkSet_" + PEPackageID).attr("checked", true);
    jQuery("#chkSet_" + PEPackageID).focus();
    SetClickBackground(PEPackageID);
}

/// <summary> 
/// 点击套餐类型列（快速选择列表）
/// </summary>
function SetClickBackground(PEPackageID) {
    jQuery("#QuickQuerySetTableData tr").removeClass();

    jQuery("#trSet_" + PEPackageID).addClass("SelectedItem");
}

/// <summary> 
/// 选择套餐类型（快速选择）
/// </summary>
function SelectQuerySet(PEPackageID) {

    // 从模版中读取数据加载列表
    var templateContent = jQuery('#SecectedSetLableTemplete').html();
    if (templateContent == undefined) { return; }
    var tempPEPackageName = jQuery("#chkSet_" + PEPackageID).attr("PEPackageName");

    ShowQuickSelectSet(PEPackageID, tempPEPackageName, true); // 套餐变动，重新读取数据

}


/// <summary> 
/// 显示快速选择的套餐类型名
/// 修改人：黄兴茂
///修改时间:2013-07-25
///修改内容：新增IsTeam参数以便特殊处理团体登记中套餐问题（由于团体备单时可用没有套餐，即使有套餐也有可用移除套餐中某些项目，这里不能直接单纯的触发套餐变动事件，需要特殊处理）
///修改时间：2014-03-25
///修改内容：新增ShowLength字段用于截取显示，以免造成样式显示不整齐问题
/// </summary>
function ShowQuickSelectSet(PEPackageID, PEPackageName, IsChange) {
    if (PEPackageName != "") {
        // 从模版中读取数据加载列表
        var templateContent = jQuery('#SecectedSetLableTemplete').html();
        if (templateContent == undefined) { return; }

        //判断套餐文字是否超过8个字符 Begin xmhuang 2014-03-25
        var fullPEPackageName = PEPackageName;                                        //套餐全名，用于title显示
        if (PEPackageName.length > 7) {
            PEPackageName = PEPackageName.substr(0, 8);
        }
        //判断套餐文字是否超过8个字符 End xmhuang 2014-03-25
        var newcontent = templateContent.replace(/@PEPackageName/gi, PEPackageName); // 替换模版中的关键字
        newcontent = newcontent.replace(/@fullPEPackageName/gi, fullPEPackageName); // 替换模版中Title显示的套餐全名

        jQuery('#spanSelectSet').html(newcontent);     // 显示到页面
        jQuery('#spanSet').hide();                     // 隐藏
        jQuery('#spanSelectSet').show();               // 显示到页面
        jQuery('#idSelectSet').val(PEPackageID);           // 选择的套餐类型ID  （隐藏值）
        jQuery('#idSelectSet').attr("title", fullPEPackageName); // 选择的套餐类型  （隐藏title值） xmhuang 2014-04-06
        jQuery('#nameSelectSet').val(PEPackageName);        // 选择的套餐类型名称（隐藏值）
    }
    if (IsChange == true) {
        jQuery("#idSelectSet").change(); // 套餐变动，调用数据读取函数
    }
}

// ================================ 套餐类型快速选择函数区域 ==== end ==================================================


// ========================== 查询客户基本信息 == start ====================================


var defalutImagSrc = "/template/blue/images/icons/nohead.gif";
/// <summary>
///通过客户编号[体检号]检索客户基本信息和客户收费项目信息
///新增归档验证 xmhuang 20150514
///IsShowMsg:1弹出提示 IsLoadCustomerInfo:加载客户基本信息
/// </summary>
function SearchCustomerBaseInfo(IsShowMsg, IsLoadCustomerInfo) {
    // 查询数据前，先隐藏客户基本信息区域
    jQuery("#divCustomerInfoArea").hide();
    var ID_Customer = jQuery('#txtCustomerID').val();
    if (jQuery('#HiddenCustomerID').val() != undefined) {
        jQuery('#HiddenCustomerID').val(jQuery('#txtCustomerID').val());
    }
    if (ID_Customer == "") {
        return false;
    }

    //组建请求参数
    var Is_Org = 0;
    var data = { action: "GetCustomerByIDCustomerOfOnline", IsLoadCustomerInfo: IsLoadCustomerInfo, ID_Customer: ID_Customer, currenttime: encodeURIComponent(new Date()) };
    // var data = { action: "GetCustomerByIDCustomer", ID_Customer: ID_Customer, currenttime: encodeURIComponent(new Date()) };
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: data,
        cache: false,
        dataType: "json",
        success: function (msg) {
            var ExamState = "";
            if (msg.ExamState != undefined) {
                ExamState = msg.ExamState;
            }
            else if (msg.dataList0 != undefined) {
                ExamState = msg.dataList0.ExamState;
            }
            if (ExamState != undefined && ExamState != 0 && ExamState != "")//状态不为0则表明此数据不在生产库上
            {
                if (IsShowMsg == 0)
                { }
                else {
                    ShowSystemDialog("对不起，该数据已归档！");
                }
            }
            if (IsLoadCustomerInfo == 1) {
                ShowCustomerBaseInfo(msg); //这里绑定客户基本信息 
            }
            else {
                ShowCustomerBaseInfo(msg); //这里绑定客户基本信息 
            }
        }
    });
    jQuery("#txtID_Customer").focus();
    jQuery("#txtID_Customer").select();
}
//设置用户基本信息
function ShowCustomerBaseInfo(msg) {
    if (msg == null || msg == undefined)
        return false;
    var item;
    var dataList0 = msg.dataList0; //存放客户基本信息
    if (dataList0 != undefined) {
        item = dataList0[0];
        // 判断item为空的情况，当归档后，这里客户基本信息为空。 20141118 by wtang 
        if (item == undefined || item == null || item == "") {
            return false;
        }
        jQuery("label[name='lblCustomerName']").text(item.CustomerName);
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
        jQuery("#lblAge").text(item.Age);
        jQuery("#tblSearch tbody tr[id='loading']").hide();
        jQuery("#tblSearch tbody tr[id!='loading']").show();



        //绑定用户基本信息
        jQuery("#lblID_Customer").text(item.ID_Customer);
        jQuery("#lblRegisterDate").text(item.OperateDate);
        jQuery("#lblTeamName").text(item.TeamName);
        jQuery("#lblExamType").text(item.ExamTypeName);

        var data = { Is_GuideSheetPrinted: item.Is_GuideSheetPrinted, Is_Subscribed: item.Is_Subscribed };
        jQuery("#txtCustomerID").data("data", data);
        //这里判断总审是否存在,存在则赋值
        if (jQuery("#lblIs_Checked") != undefined) {

            if (item.Is_Checked == "True") {
                item.Is_Checked = "√";
                //设置报告预览按钮可见 xmhuang 2013-10-15 注释掉，移除预览限制
                //jQuery("#btnReportPreview").show();
            }
            else {
                item.Is_Checked = "×";
                //xmhuang 2013-10-15 注释掉，移除预览限制
                //jQuery("#btnReportPreview").hide();
            }
            jQuery("#lblIs_Checked").text(item.Is_Checked);
            jQuery("#lblChecker").text(item.Checker);
            jQuery("#lblCheckedDate").text(item.CheckedDate);
            //判断分检状态
            if (item.Is_ExamStarted == "True") {
                item.Is_ExamStarted = "√";
            }
            else {
                item.Is_ExamStarted = "×";
            }
            //判断总检状态
            if (item.Is_FinalFinished == "True") {
                item.Is_FinalFinished = "√";
            }
            else {
                item.Is_FinalFinished = "×";
            }
            //jQuery("#lblIs_ExamStarted").text(item.Is_ExamStarted); //是否总检
            jQuery("#lblIs_FinalFinished").text(item.Is_FinalFinished); //是否总检
            jQuery("#lblFinalDoctor").text(item.FinalDoctor); //总检医生
            jQuery("#lblFinalDate").text(item.FinalDate); //总检日期

        }

        //判断报告打印状态
        if (item.Is_ReportPrinted == "True") {
            item.Is_ReportPrinted = "√";
        }
        else {
            item.Is_ReportPrinted = "×";
        }
        //判断报告通知状态
        if (item.Is_Informed == "True") {
            item.Is_Informed = "√";
        }
        else {
            item.Is_Informed = "×";
        }
        //判断报告领取状态
        if (item.Is_ReportReceipted == "True") {
            item.Is_ReportReceipted = "√";
        }
        else {
            item.Is_ReportReceipted = "×";
        }
        jQuery("#lblIs_ReportPrinted").text(item.Is_ReportPrinted); //是否打印
        jQuery("#lblReportPrinter").text(item.ReportPrinter); //打印人
        jQuery("#lblReportPrintedDate").text(item.ReportPrintedDate); //打印时间

        jQuery("#lblIs_Informed").text(item.Is_Informed); //是否通知
        jQuery("#lblInformer").text(item.Informer); //通知人
        jQuery("#lblInformedDate").text(item.InformedDate); //通知日期

        jQuery("#lblIs_ReportReceipted").text(item.Is_ReportReceipted); //是否领取

        //报告预览界面的报告领取人设置 xmhuang 2014-04-22

        jQuery("#lblReportReceiptorTitle").attr("title", item.ReportReceiptor);

        jQuery("#lblReportReceiptorTitle").text(DisplayLitterLetter(jQuery.trim(item.ReportReceiptor), 10).replace("...", ""));

        //报告预览界面的报告领取人设置 xmhuang 2014-04-22
        jQuery("#lblReportReceiptor").text(item.ReportReceiptor); //领取人
        jQuery("#lblReportReceiptedDate").text(item.ReportReceiptedDate); //领取日期

        jQuery("#lblSubScriber").text(item.SubScriber); //登记人
        jQuery("#lblSubScriberOperDate").text(item.SubScriberOperDate); //登记时间

        jQuery("#lblOperator").text(item.Operator); //指引单打印人
        jQuery("#lblOperateDate").text(item.OperateDate); //指引单打印时间

        jQuery("#lblGuideSheetReturnedby").text(item.GuideSheetReturnedby); //指引单回收人
        jQuery("#lblGuideSheetReturnedDate").text(item.GuideSheetReturnedDate); //指引单回收时间

        jQuery("#lblFinalDoctor").text(item.FinalDoctor); //总检医生
        jQuery("#lblFinalDate").text(item.FinalDate); //总检日期

        jQuery("#lblChecker").text(item.Checker); //总审医生
        jQuery("#lblCheckedDate").text(item.CheckedDate); //总审日期

        jQuery("#lblReportPrinter").text(item.ReportPrinter); //报告打印人
        jQuery("#lblReportPrintedDate").text(item.ReportPrintedDate); //报告打印日期

        jQuery("#lblInformer").text(item.Informer); //报告通知人
        jQuery("#lblInformedDate").text(item.InformedDate); //报告通知日期

        jQuery("#lblReportReceiptor").text(item.ReportReceiptor); //报告领取人
        jQuery("#lblReportReceiptedDate").text(item.ReportReceiptedDate); //报告领取日期

        jQuery("#lblReportChecker").text(item.ReportChecker); //报告审核人
        jQuery("#lblReportCheckDate").text(item.ReportCheckDate); //报告审核日期

        jQuery("#lblReportOffer").text(item.ReportOffer); //报告发放人
        jQuery("#lblReportOffDate").text(item.ReportOffDate); //报告发放日期
    }

    if (dataList0 == undefined || dataList0.length == 0) {
        jQuery("#tblSearch tbody tr[id!='loading']").hide();
        jQuery("#tblSearch tbody tr[id='loading']").show();

        // 如果是分科检查，隐藏分科内容区域的信息 by WTang 20130819
        try {
            if (jQuery("#ContentArea") != null) { jQuery("#ContentArea").hide(); }
        } catch (e) {

        }
    }

    var CustomerSecurityLevel = jQuery("#CustomerSecurityLevel").val(); // 客户  操作密级
    var OperateLevel = jQuery("#OperateLevel",parent.document).val();                   // 操作员操作密级

    // 检查操作密级
    if (CustomerSecurityLevel != undefined && parseInt(CustomerSecurityLevel) > parseInt(OperateLevel)) {
        jQuery("#divCustomerInfoArea").hide();  // 如果没有权限，则客户基本信息页不允许查看
    }
    else {
        // 如果查询到客户信息，则将客户信息进行显示
        jQuery("#divCustomerInfoArea").show();
    }

    jQuery("#txtCustomerID").focus();
    jQuery("#txtCustomerID").select();

}

// ========================== 查询客户基本信息 == end ====================================

/*******************************热键   Begin**********************************************/
//一个快捷键对象 
function KeyOne(id, keys, dom, isfun, fun, iskeydown) {
    this.id = id;
    this.keys = keys;
    this.dom = dom;
    this.isfun = isfun;
    this.fun = fun;
    this.isKeydown = iskeydown;
}

//快捷键管理类 
var KeyConlor = {};
KeyConlor.list = new Array();
//添加一个快捷键绑定焦点（当快捷键被激发时让焦点落在指定id对象上） 
//使用说明key的值如果是“c,50”则表示“ctrl”和键码为50的组合键 
// "a,50" 则表示“alt”和键码为50的组合键 
// "s,50" 则表示“shift”和键码为50的组合键 
// "50" 则表示键码为50的单键（建议使用组合键alt） 
//id指的是快捷键对应的焦点对象。 
//dom指的是id对象所在的document对象 
KeyConlor.addkeyfouse = function (id, key, dom, iskyedown) {
    var keyone = new KeyOne(id, key, dom, false, null, iskyedown);
    if (KeyConlor.KeyIsOK(keyone)) {
        KeyConlor.list.push(keyone);
    } else {
        ShowSystemDialog("快捷键" + keyone.keys + "已经被注册 不能重复注册了");
        return false;
    }
};

//快捷键绑定方法（当快捷键激发时触发方法） 
KeyConlor.addkeyfun = function (key, fun, iskeydown) {
    var keyone = new KeyOne("", key, "", true, fun, iskeydown);
    if (KeyConlor.KeyIsOK(keyone)) {
        KeyConlor.list.push(keyone)
    } else {
        ShowSystemDialog("快捷键:" + keyone.keys + ";已经被注册 .重复注册无效");
        return false;
    }
};

//--删除一个快捷键 
KeyConlor.removeFouseKey = function (id) {
    var keyone = new KeyOne(id, "");
    for (var i = 0; i < KeyConlor.list.length; i++) {
        if (keyone.id == KeyConlor.list[i].id) {
            KeyConlor.list[i] = null;
        }
    }
};

//--判断快捷键是不是重复注册 
KeyConlor.KeyIsOK = function (keyone) {
    for (var i = 0; i < KeyConlor.list.length; i++) {
        if (KeyConlor.list[i].keys == keyone.keys) {
            return false;
        }
    }
    return true;
};


document.onkeydown = function () {

    // 暂时不屏蔽用户手动输入 20130802 by WTang
    //    // 屏蔽掉鼠标输入体检号的功能 
    //    var ID = "txtCustomerID";
    //    if (jQuery('#' + ID) != undefined) {
    //        JudgeIsKeyBoardInputKeyDown();
    //    }

    var event = window.event || e;

    for (var i = 0; i < KeyConlor.list.length; i++) {

        var keyone = KeyConlor.list[i];
        if (!keyone.isKeydown) continue;
        var control = keyone.keys.split(",")[0];
        switch (control) {
            case 's':
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.shiftKey == true && event.keyCode == code) {
                    //获得焦点 
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;
            case 'c':
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.ctrlKey == true && event.keyCode == code) {
                    //获得焦点 
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;
            case 'a':
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.altKey == true && event.keyCode == code) {
                    //获得焦点 
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                event.keyCode = 0;
                break;

            default:
                //获得焦点 
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.keyCode == code && event.altKey == false && event.ctrlKey == false && event.shiftKey == false) {
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;
        }
    }
};
document.onkeyup = function () {

    // 暂时不屏蔽用户手动输入 20130802 by WTang
    //    // 屏蔽掉鼠标输入体检号的功能
    //    var ID = "txtCustomerID";
    //    if (jQuery('#' + ID) != undefined) {
    //        JudgeIsKeyBoardInputKeyUp();
    //    }

    for (var i = 0; i < KeyConlor.list.length; i++) {
        var keyone = KeyConlor.list[i];
        if (keyone.isKeydown) continue;
        var control = keyone.keys.split(",")[0];
        switch (control) {
            case 's':
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.shiftKey == true && event.keyCode == code) {
                    //获得焦点 
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;
            case 'c':
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.ctrlKey == true && event.keyCode == code) {
                    //获得焦点 
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;
            case 'a':
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.altKey == true && event.keyCode == code) {
                    //获得焦点 
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;

            default:
                //获得焦点 
                var code = keyone.keys.split(",").length > 1 ? keyone.keys.split(",")[1] : keyone.keys.split(",")[0];
                if (event.keyCode == code && event.altKey == false && event.ctrlKey == false && event.shiftKey == false) {
                    if (!keyone.isfun) {
                        keyone.dom.getElementById(keyone.id).focus();
                    } else {
                        keyone.fun();
                    }
                    event.keyCode = 0;
                    return false;
                }
                break;
        }
    }
};

//常用键盘码 
var keyCodeStr = {
    Alt: "a",
    Shift: "s",
    Ctrl: "c",
    Up: "38",
    Down: "40",
    Left: "37",
    Right: "39",
    Esc: "27",
    Enter: "13",
    Backspace: "8",
    Delete: "46",
    Tab: "9",
    CapsLK: "20",
    Space: "32",
    F1: "112",
    F2: "113",
    F3: "114",
    F4: "115",
    F5: "116",
    F6: "117",
    F7: "118",
    F8: "119",
    F9: "120",
    F10: "121",
    F11: "122",
    F12: "123",
    Save: "83",
    Add: "65",
    B: "66",
    Copy: "67",
    Delete: "68",
    CloseWin: "87",
    v: "86",
    Windowx: "91",
    Q: "81",
    PlusSign: "109",
    MinusSign: "107",
    PlusSign2: "189",
    MinusSign2: "187"
};
//Ctrl ， +
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.PlusSign,
function () {
    return false;
}
, true
);
//Ctrl ， -
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.MinusSign,
function () {
    return false;
}
, true
);
//Ctrl ， +
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.PlusSign2,
function () {
    return false;
}
, true
);
//Ctrl ， -
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.MinusSign2,
function () {
    return false;
}
, true
);
//为套餐搜索注册热键Ctrl+Enter,则用户按下Alt+s键时触发套餐选择的确定事件
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.Save,
function () {
    //判断是否显示状态
    if (document.getElementById("showBusFee") != undefined) {
        if (document.getElementById("showBusFee").style.display != "none") {
            if (document.getElementById("btnSure") != undefined) {
                document.getElementById("btnSure").click();
            }
        }
    }
}
, true
);
//为套餐搜索注册热键Ctrl+w,则用户按下Alt+w键关闭搜索框
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.CloseWin,
function () {
    if (document.getElementById("showBusFee") != undefined) {
        document.getElementById("showBusFee").style.display = "none";
    }
    //    if (document.getElementById("externTBlList") != undefined) {
    //        document.getElementById("showBusFee").style.display = "none";
    //    }
}
, true
);
//为批量操作注册热键Ctrl+d,则用户按下Alt+d键激发批量删除
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.Delete,
function () {
    if (document.getElementById("btnBatchDelete") != undefined) {
        document.getElementById("btnBatchDelete").click();
    }
}
, true
);
//为批量操作注册热键Ctrl+a,则用户按下Alt+a键激发批量新增
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.Q,
function () {
    if (document.getElementById("btnBatchAdd") != undefined) {
        document.getElementById("btnBatchAdd").click();
    }
}
, true
);

//为批量操作注册热键Esc,则用户按下Esc键激发批量新增
KeyConlor.addkeyfun(
keyCodeStr.Esc,
function () {
    //经讨论，系统将屏蔽原有的Esc系统功能，在使用Esc键时，只默认关闭收费项目列表
    if (jQuery("#showBusFeeItem").length > 0) {
        try {
            jQuery("#showBusFeeItem").empty();
            jQuery("#showBusFee").hide();
            if (jQuery("#TeamCustomerAddDiv").length > 0)//如果是团体备单则不执行重绘Table表头操作
            { }
            else {
                JudgeTableIsExistScroll();
            }
        }
        catch (e)
        { }
    }

    // 总检页面，分科详细弹出框
    if (jQuery(".SectionDetailInfo").length > 0) {
        jQuery('.SectionDetailInfo').fadeOut('slow').data("target", "");
    }

    // 总检页面，分科对比弹出框
    if (jQuery(".SectionCompareDetailInfo").length > 0) {
        jQuery('.SectionCompareDetailInfo').fadeOut('slow').data("target", "");
    }
    // 总检页面，分科详细列表
    if (jQuery(".ShowBigEditer").length > 0) {
        jQuery('.texttcc,.m-media').fadeOut('slow').data("target", "");
    }

}
, true
);

//注册到底部的快捷方式
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.Down,
function () {
    GotoBottom();
}
, true
);

//注册到顶部的快捷方式
KeyConlor.addkeyfun(
keyCodeStr.Ctrl + "," + keyCodeStr.Up,
function () {
    GotoTop();
}
, true
);

//热键 体检号/身份证 输入框获取焦点 F2
KeyConlor.addkeyfun(
keyCodeStr.F2,
function () {
    //体检号输入框获取焦点
    if (document.getElementById("txtCustomerID") != null) {
        if (jQuery("#txtCustomerID").attr("disabled") != "disabled") {
            jQuery("#txtCustomerID").focus();
            jQuery("#txtCustomerID").select();
        }
    }
    //体检号、身份证号获取焦点(个人登记列表页面的体检号文本框)
    if (document.getElementById("txtSFZ") != null) {
        if (jQuery("#txtSFZ").attr("disabled") != "disabled") {
            jQuery("#txtSFZ").focus();
            jQuery("#txtSFZ").select();
        }
    }
    //证件号(登记预约页面的证件号文本框)
    if (document.getElementById("txtSearchX") != null) {
        if (jQuery("#txtSearchX").attr("disabled") != "disabled") {
            jQuery("#txtSearchX").focus();
            jQuery("#txtSearchX").select();
        }
    }

    //体检号(收费页面体检号文本框)
    if (document.getElementById("txtID_Customer") != null) {
        if (jQuery("#txtID_Customer").attr("disabled") != "disabled") {
            jQuery("#txtID_Customer").focus();
            jQuery("#txtID_Customer").select();
        }
    }
}
, true
);

//热键 结论词输入框获取焦点 F4
//热键 查询按钮 F4
KeyConlor.addkeyfun(
keyCodeStr.F4,
function () {
    //查询按钮
    if (document.getElementById("btnSearch") != null) {
        if (jQuery("#btnSearch").is(":visible") && jQuery("#btnSearch").attr("disabled") != "disabled") {
            document.getElementById("btnSearch").click();
        }
    }
    //    //查询按钮（登记页面的检索按钮）
    //    if (document.getElementById("btnSearchX") != null) {
    //        if (jQuery("#btnSearchX").is(":visible") && jQuery("#btnSearchX").attr("disabled") != "disabled") {
    //            document.getElementById("btnSearchX").click();
    //        }
    //    }
    //查询按钮（收费退费页面的检索按钮）
    if (document.getElementById("btnSearchCustomer") != null) {
        if (jQuery("#btnSearchCustomer").is(":visible") && jQuery("#btnSearchCustomer").attr("disabled") != "disabled") {
            document.getElementById("btnSearchCustomer").click();
        }
    }
}
, true
);

//热键 刷新
KeyConlor.addkeyfun(
keyCodeStr.F5,
function () {
    DisposeVideoCapture(); //在加载页面之前判断是否开启采集卡资源，有则关闭

    location.replace(location.href);
}
, true
);

//热键 总检 汇总 
//热键 分检 汇总
KeyConlor.addkeyfun(
keyCodeStr.F6,
function () {
    //总检热键注册 (汇总)
    if (document.getElementById("ButCollect") != null) {
        if (jQuery("#ButCollect").attr("disabled") != "disabled") {
            document.getElementById("ButCollect").click();
        }
    }
    //    //分检热键注册 (汇总)
    //    if (document.getElementById("ButCollect") != null) {
    //        if (jQuery("#ButCollect").attr("disabled") != "disabled") {
    //            document.getElementById("ButCollect").click();
    //        }
    //    }
}
, true
);

//热键 分检 保存
//热键 登记列表 申请 
KeyConlor.addkeyfun(
keyCodeStr.F7,
function () {
    //分检热键注册 (保存)
    if (document.getElementById("ButSave") != null) {
        if (jQuery("#ButSave").attr("disabled") != "disabled") {
            document.getElementById("ButSave").click();
        }
    }
    //登记列表热键注册 (申请)
    if (document.getElementById("btnAdd") != null) {
        if (jQuery("#btnAdd").attr("disabled") != "disabled") {
            document.getElementById("btnAdd").click();
        }
    }

    //结论词输入框获取焦点
    if (document.getElementById("txtConclusionInputCode") != null) {
        if (jQuery("#txtConclusionInputCode").is(":visible") && jQuery("#txtConclusionInputCode").attr("disabled") != "disabled") {
            jQuery("#txtConclusionInputCode").focus();
            jQuery("#txtConclusionInputCode").select();
        }
    }

    //分检 采集图像 btnUploadImg
    if (jQuery("#btnSaveImg").is(":visible") && document.getElementById("btnSaveImg") != null) {
        if (jQuery("#btnSaveImg").attr("disabled") != "disabled") {
            document.getElementById("btnSaveImg").click();
        }
    }
}
, true
);

//热键 总检提交 
//热键 总审 审核通过
//热键 分检提交 
//热键 登记预约 读证件
KeyConlor.addkeyfun(
keyCodeStr.F8,
function () {
    //总检 热键注册 (提交)
    if (document.getElementById("ButSave") != null) {
        if (jQuery("#ButSave").is(":visible") && jQuery("#ButSave").attr("disabled") != "disabled") {
            // document.getElementById("ButSave").click();
            // 通过“分科解锁”按钮判断是否是总检页面
            if (document.getElementById("ButUnLock01") != null) {
                SaveCustomerFinalConclusionConfirm(1); // 总检提交确认操作
            }
        }
    }

    // 通过“解除总检”按钮判断是否是总审页面
    if (document.getElementById("ButUnLockFinalCheck1") != null) {
        //总审 热键注册 (审核通过)
        if (document.getElementById("ButChecked") != null) {
            if (jQuery("#ButChecked").is(":visible") && jQuery("#ButChecked").attr("disabled") != "disabled") {
                document.getElementById("ButChecked").click();
            }
        }
    }

    //分检 热键注册 (提交)
    if (document.getElementById("ButCheck") != null) {
        if (jQuery("#ButCheck").is(":visible") && jQuery("#ButCheck").attr("disabled") != "disabled") {
            document.getElementById("ButCheck").click();
        }
    }


    //分检 热键注册 (提交) [LAB 类]
    if (document.getElementById("ButCheck_Ex") != null) {
        if (jQuery("#ButCheck_Ex").is(":visible")) {
            document.getElementById("ButCheck_Ex").click();
        }
    }

    //登记预约 热键注册 (读证件)
    if (document.getElementById("btnReadFromMachine") != null) {
        if (jQuery("#btnReadFromMachine").is(":visible") && jQuery("#btnReadFromMachine").attr("disabled") != "disabled") {
            document.getElementById("btnReadFromMachine").click();
        }
    }
    //收费 热键注册 (收费)
    if (document.getElementById("btnCharge") != null) {
        if (jQuery("#btnCharge").is(":visible") && jQuery("#btnCharge").attr("disabled") != "disabled") {
            document.getElementById("btnCharge").click();
        }
    }
    //退费 热键注册 (退费)
    if (document.getElementById("btnUnCharge") != null) {
        if (jQuery("#btnUnCharge").is(":visible") && jQuery("#btnUnCharge").attr("disabled") != "disabled") {
            document.getElementById("btnUnCharge").click();
        }
    }
}
, true
);

//热键 总审 审核不通过
//热键 预约登记 完成
KeyConlor.addkeyfun(
keyCodeStr.F9,
function () {
    // 通过“解除总检”按钮判断是否是总审页面
    if (document.getElementById("ButUnLockFinalCheck1") != null) {
        //总审热键注册 (审核不通过)
        if (document.getElementById("ButUnCheck") != null) {
            if (jQuery("#ButUnCheck").attr("disabled") != "disabled") {
                document.getElementById("ButUnCheck").click();
            }
        }
    }
    //预约登记 (完成) xmhuang 2014-04-11 如果按钮为可编辑且必须为显示状态时方可触发完成事件
    if (document.getElementById("btnRegiste") != null) {
        if (jQuery("#btnRegiste").attr("disabled") != "disabled" && jQuery("#btnRegiste").is(":visible")) {
            document.getElementById("btnRegiste").click();
        }
    }
    //分检 上传图像 
    if (jQuery("#btnUploadImg").is(":visible") && document.getElementById("btnUploadImg") != null) {
        if (jQuery("#btnUploadImg").attr("disabled") != "disabled") {
            document.getElementById("btnUploadImg").click();
        }
    }

    // 是否是总检
    var Is_FinalConclusion = jQuery("#Is_FinalConclusion").val();
    if (Is_FinalConclusion == "True") {
        SetSelectConclusionItemFocus();
    }
}
, true
);


//热键 总检 预览
KeyConlor.addkeyfun(
keyCodeStr.F10,
function () {
    //总检热键注册 (预览)
    if (document.getElementById("ButReprotPreview") != null) {
        if (jQuery("#ButReprotPreview").attr("disabled") != "disabled") {
            document.getElementById("ButReprotPreview").click();
        }
    }
    //报告预览页面 (预览)
    if (jQuery("#btnReportPreview").is(":visible") && document.getElementById("btnReportPreview") != null) {
        if (jQuery("#btnReportPreview").attr("disabled") != "disabled") {
            document.getElementById("btnReportPreview").click();
        }
    }
    //分检 缺省
    if (jQuery("#ButDefault").is(":visible") && document.getElementById("ButDefault") != null) {
        if (jQuery("#ButDefault").attr("disabled") != "disabled") {
            document.getElementById("ButDefault").click();
        }
    }
}
, true
);

/*******************************热键   End**********************************************/


// 设置选中的结论词获取焦点
function SetSelectConclusionItemFocus() {
    // 等待总检页面重载这个函数
}

function ReplaceSelectText() {
    if (window.getSelection) {
        window.getSelection().deleteFromDocument();
    }
    else if (document.selection && document.selection.createRange) {
        document.selection.createRange().text = "";
    }
}

/// <summary>
/// 获取当前选中值
/// </summary>
function GetSelectionText() {
    if (window.getSelection) {
        return window.getSelection().toString();
    }
    else if (document.selection && document.selection.createRange) {
        return document.selection.createRange().text;
    }
}
/// <summary>
/// 设置当前选中值为指定值
/// </summary>
function SetSelectText(text) {
    if (window.getSelection) {
        window.getSelection().deleteFromDocument();
    }
    else if (document.selection && document.selection.createRange) {
        document.selection.createRange().text = text;
    }
}


/*******************************屏蔽掉鼠标输入体检号的功能 start **********************************************/
var lastInputTime = new Date();
var currInputTime = new Date();
// 判断是否是键盘输入的字符
function JudgeIsKeyBoardInputKeyDown() {
    lastInputTime = new Date();
}

function JudgeIsKeyBoardInputKeyUp() {

    var curEvent = window.event || e;
    try {
        if (curEvent.srcElement.id != "txtCustomerID") {
            return;
        }
    } catch (e) {
        return;
    }

    currInputTime = new Date();
    var time01 = parseInt(lastInputTime.getTime());
    var time02 = parseInt(currInputTime.getTime());
    var times = time02 - time01;

    if ((curEvent.ctrlKey) && (curEvent.keyCode == 86)) { // 如果是粘贴，则直接返回（不允许粘贴）

        jQuery("#" + curEvent.srcElement.id).val("");

        return false;
    }
    // 如果按键响应时间大于25毫秒，则认为是手动输入的，直接进行过滤掉。
    if (times > 25) {

        jQuery("#" + curEvent.srcElement.id).val("");

        return false;
    }

}


/*******************************屏蔽掉鼠标输入体检号的功能 end **********************************************/

/// <summary>
/// 关闭弹出窗口
/// </summary>
function CloseDialogWindow() {
    //parent.
        art.dialog.get('OperWindowFrame').close();
}

/// <summary>
/// 获取页面地址参数
/// </summary>
function GetQueryString(sProp) {
    var re = new RegExp(sProp + "=([^\\&;]*)", "i");
    var a = re.exec(document.location.search);
    if (a == null)
        return "";
    return a[1];
}

/// <summary>
/// 保存当前访问的页面的查询参数（主要针对查询列表页面）
/// </summary>
function SetUserCurrentQueryParams(PageTag, ParamsArgArray) {

    var exp = new Date(); // 设置当前时间，只有当天才有效 （在取出时，先判断时间是否一致。）
    var CurrentDate = exp.getFullYear() + "-" + exp.getMonth() + '-' + exp.getDay();
    // 拼接的字符串，最后一个为当前日期，在取回的时候，需要做验证，如果是今天设置的，则在页面加载时进行匹配，否则为系统默认配置
    var ParamsArgStrs = "";
    var argCount = 0;
    var ParamsArgArrayLength = 0;

    for (var i = 0; i < ParamsArgArray.length; i++) {
        ParamsArgStrs = ParamsArgStrs + ParamsArgArray[i] + "；";
    }
    ParamsArgStrs = ParamsArgStrs + CurrentDate;
    ParamsArgStrs = encodeURIComponent(ParamsArgStrs);
    SetCookie(PageTag, ParamsArgStrs);

}


/// <summary>
/// 保存当前访问的页面的查询参数（主要针对查询列表页面）
/// </summary>
function GetUserCurrentQueryParams(PageTag) {

    // 拼接的字符串，第一个字符为当前日期，在取回的时候，需要做验证，如果是今天设置的，则在页面加载时进行匹配，否则为系统默认配置
    var ParamsArgStrs = GetCookie(PageTag);
    ParamsArgStrs = decodeURIComponent(ParamsArgStrs);

    var ParamsArgArray = ParamsArgStrs.split("；");


    var exp = new Date(); // 设置当前时间，只有当天才有效 （在取出时，先判断时间是否一致。）
    var CurrentDate = exp.getFullYear() + "-" + exp.getMonth() + '-' + exp.getDay();
    var SaveDate = "";
    if (ParamsArgArray != null && ParamsArgArray != undefined) {
        if (ParamsArgArray.length > 0) {
            SaveDate = ParamsArgArray[ParamsArgArray.length - 1];
        }
    }
    if (SaveDate == CurrentDate) {
        return ParamsArgArray;
    } else {
        return null;
    }
}

/// <summary>
/// 设置菜单的二级标题名称
/// </summary>
function ShowSubTitleName_Name(tmpTitle) {
    if (tmpTitle == undefined || tmpTitle == "") {
        tmpTitle = GetCookie('MenuSubTitleName');
    }
    if (tmpTitle != undefined && tmpTitle != "") {
        jQuery(".Work_Title").text(tmpTitle);
        jQuery("#MenuSubTitleName").val(tmpTitle);

        SetCookie('SubMenuID', '');      // 清除 子菜单的MenuID
    }
    if (tmpTitle != undefined && tmpTitle != "") {
        SetCookie('MenuSubTitleName', tmpTitle);
    }
}


/// <summary>
/// 设置菜单的二级标题名称
/// </summary>
function ShowSubTitleName_Click(SubMenuID, SubSectionID) {
    if (SubMenuID == undefined || SubMenuID == "") {
        jQuery(".Work_Title").text(jQuery("#MenuSubTitleName").val());
        return;
    }
    var tmpTitle = jQuery("#menu_" + SubMenuID + "_" + SubSectionID + " a").text();
    if (tmpTitle != undefined && tmpTitle != "") {
        jQuery(".Work_Title").text(tmpTitle);
        jQuery("#MenuSubTitleName").val(tmpTitle);
        SetCookie('MenuSubTitleName', tmpTitle);
    }
}


/// <summary>
/// 设置菜单的二级标题名称
/// </summary>
function ShowSubTitleName() {
    var SubMenuID = GetCookie('SubMenuID');       // 获取当前的子菜单ID
    var SubSectionID = GetCookie('SubSectionID'); // 获取当前的子菜单科室ID（如果不是分检，则该值为空）

    var tmpTitle = jQuery("#menu_" + SubMenuID + "_" + SubSectionID + " a").text();
    if (tmpTitle != undefined && tmpTitle != "") {
        jQuery(".Work_Title").text(tmpTitle);
        SetCookie('MenuSubTitleName', tmpTitle);
    }

    if (SubMenuID == undefined || SubMenuID == "") {
        ShowSubTitleName_Name(jQuery("#MenuSubTitleName").val());
        return;
    }
}

/// <summary>
/// 保存当前访问的子页面地址
/// </summary>
function SetUserRedirectUrl(url) {

    SetCookie('CurrentUrl', url);
    var exp = new Date(); // 设置当前时间，只有当天才有效 （在取出时，先判断时间是否一致。）
    var CurrentDate = exp.getFullYear() + "-" + exp.getMonth() + '-' + exp.getDay();
    SetCookie('SaveDate', CurrentDate);
    SetCookie('UserID', jQuery("#CookieUserID",parent.document).val()); // 写入当前用户ID
}

/// <summary>
/// 获取当前页面地址，并进行加载
/// </summary>
function GetUserRedirectUrl() {

    var CurrentUrl = GetCookie('CurrentUrl'); // 获取Url
    var SaveDate = GetCookie('SaveDate');     // 获取当前日期
    var SaveMenuID = GetCookie('SaveMenuID'); // 获取当前选择的菜单分类ID

    var SubMenuID = GetCookie('SubMenuID');       // 获取当前的子菜单ID
    var SubSectionID = GetCookie('SubSectionID'); // 获取当前的子菜单科室ID（如果不是分检，则该值为空）

    var UserID = GetCookie('UserID');       // 获取上次保存的UserID
    // 如果上次保存的CookieUserID不为空，且两个值不一致的情况，则不用加载相应的连接。 20130905 by WTang
    if (UserID != null && UserID != undefined && UserID != "" && jQuery.trim(jQuery("#CookieUserID",parent.document).val()) != jQuery.trim(UserID)) {
        DoLoad("");
        return;
    }

    var exp = new Date(); // 设置当前时间，只有当天才有效 （在取出时，先判断时间是否一致。）
    var CurrentDate = exp.getFullYear() + "-" + exp.getMonth() + '-' + exp.getDay();

    // 1、如果cookie 设置的时间，与当前时间是同一天
    // 2、如果同时当前地址不为空，则加载当前地址。

    if (CurrentUrl != null && CurrentUrl != undefined && SaveDate != undefined && SaveDate != null) {
        if (CurrentUrl != "" && CurrentDate == SaveDate) {
            if (SaveMenuID != "" && SaveMenuID != null) {

                // 循环菜单分类
                jQuery(gLoginUserMenuClass).each(function (k, menuclassitem) {
                    jQuery("#LoginUserMenuClassTitle_" + menuclassitem.MenuID).removeClass("selected");    // 标题的选中效果取消
                    jQuery("#LoginUserMenuClassSubItem_" + menuclassitem.MenuID).hide();                   // 隐藏对应的菜单子项
                });
                jQuery("#LoginUserMenuClassTitle_" + SaveMenuID).addClass("selected");     // 选中当前点击的标题
                //jQuery("#LoginUserMenuClassSubItem_" + SaveMenuID).show();               // 显示当前点击菜单子项
                jQuery("#loadForm").html("<div style='height:500px;'>&nbsp;</div>");        // 当切换分组的时候，清空页面内容区域加载的数据

                jQuery("#menu_" + SubMenuID + "_" + SubSectionID).attr("class", "selected");   // 设置当前点击的菜单为选中状态
            }
            DoLoad(CurrentUrl);
            return;
        }
    }
    DoLoad("");
}

/// <summary>
/// 设置cookie
/// </summary>
function SetCookie(name, value) {
    var exp, y, m, d;
    exp = new Date();
    exp.setHours(exp.getHours() + 4);
    document.cookie = name + "=" + escape(value) + "; expires=" + exp.toGMTString() + "; path=/";
}

/// <summary>
/// 获取cookie值
/// </summary>
function GetCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]);
    return null;
}

/// <summary>
/// 清除所有的Cookie
/// </summary>
function ClearCookie() {

    SetCookie('CurrentUrl', '');     // 清除 当前Url地址
    SetCookie('SaveMenuID', '');     // 清除 当前选择的菜单分组
    SetCookie('SaveDate', '');       // 清除 当前保存的时间
    SetCookie('SubMenuID', '');      // 清除 子菜单的MenuID
    SetCookie('SubSectionID', '');   // 清除 子菜单的SectionID 
    SetCookie('UserID', '');         // 清除 UserID


    SetCookie('LastExamDoctorSaveDate', "");    // 清除 分科最后一次保存的时间
    SetCookie("LastExamDoctorSection", "");     // 清除 分科最后一次保存的科室
    SetCookie("LastExamDoctorID", "");          // 清除 分科最后一次保存的医生ID
    SetCookie("LastExamDoctorName", "");        // 清除 分科最后一次保存的医生姓名

    //    var keys = document.cookie.match(/[^=;]+(?=\=)/g);

    //    alert(keys);
    //    if (keys) {
    //        for (var i = keys.length; i--; ) {
    //            alert(keys[i]);
    //            //document.cookie = keys[i] + '=0;expires='+new Date(0).toUTCString();
    //        }
    //    }
}

/// <summary>
/// 显示或者隐藏列表区域
/// 该功能为指定name=ToggerMe的元素绑定点击事件，并显示和隐藏其下一个同级元素div
/// </summary>
function ShowMe() {
    jQuery("div [name='ToggerMe']").bind("click", function () {
        if ($(this).parent().next().is(":visible")) {
            $(this).parent().next().hide();
            $(this).attr("title", "点击显示列表");
        }
        else {
            $(this).parent().next().show();
            $(this).attr("title", "点击隐藏列表");
        }
    });

    jQuery("div [name='ToggerMe']").next().bind("click", function () {
        jQuery(this).prev().parent().next().show();
        jQuery(this).prev().parent().prev().find("[name='ToggerMe']").attr("title", "点击隐藏列表");
    });

    jQuery("div [name='ToggerMe']").css({ cursor: "pointer" }); //设置手型选择分组标题     
    jQuery("div [name='ToggerMe']").attr("title", "点击隐藏列表");
    jQuery("div [name='ToggerMe'][allowtogger='1']").attr("title", "点击显示列表");
    jQuery("div [name='ToggerMe'][allowtogger='1']").parent().next().hide(); //默认不显示列表
}

/// <summary>
/// 显示正在加载的进度条(默认文字)
/// </summary>
function ShowLoadingProcessBarDefault(flag) {
    ShowLoadingProcessBar(flag, "", "");
}
/// <summary>
/// 显示正在加载的进度条
/// </summary>
function ShowLoadingProcessBar(flag, title, info) {
    return; // 暂时不显示进度条提示信息 20140418 by wtang
    try {
        if (title == "") {
            title = "提示信息";
        }
        if (info == "") {
            info = "正在加载数据，请稍后...";
        }
        if (flag == 1) {
            if (IsShowProcessBar) {
                var top = (document.documentElement.scrollTop + window.screen.availHeight / 2 - 150) + "px";
                var left = (document.documentElement.scrollLeft + window.screen.availWidth / 2 - 100) + "px";
                document.getElementById("ProcessLoading").style.top = top;
                document.getElementById("ProcessLoading").style.left = left;
                jQuery("#ProcessLoading").show();
                //ShowCover(); // 显示遮罩层
            }
            else {
                jQuery("#ProcessLoading").hide();
            }
        } else {
            jQuery("#ProcessLoading").hide();
            //jQuery("#cover").hide(); // 隐藏遮罩层
        }
        //IsShowProcessBar = true;
    } catch (e) { }
}

/// <summary>
/// 显示遮罩层
/// </summary>
function ShowCover() {
    var cover = document.getElementById("cover");
    cover.style.width = document.documentElement.scrollWidth + "px";
    cover.style.heigh = document.documentElement.scrollHeigh + "px";
    cover.style.display = "block";
}
/// <summary>
/// 发送退出重新登录消息 xmhuang 2013-12-30
/// </summary>
function SendLogOutMessage() {
    var LoginName = jQuery("#CookieLoginName",parent.document).val();
    var UserName = jQuery("#CookieUserName",parent.document).val();
    return Wait(LoginName, UserName, "LOGOUT", "LOGOUT");
}
var ShowTime = 1; //消息显示时常 默认1分钟
var LogoutTime = 3; //退出系统消息显示时常，默认3分钟
/// <summary>
/// 发送退出消息 xmhuang 2013-12-31
/// </summary>
function SendExitMessage() {
    var LoginName = jQuery("#CookieLoginName",parent.document).val();
    var UserName = jQuery("#CookieUserName",parent.document).val();
    //return Wait(LoginName, UserName, "EXIT", "EXIT");
    var SessionID = jQuery("#CookieSessionID",parent.document).val();
    var LoginName = jQuery("#CookieLoginName",parent.document).val();
    var UserName = jQuery("#CookieUserName",parent.document).val();
    Wait(SessionID, LoginName, "EXIT", "EXIT");
    //SendMessage("EXIT", ShowTime, LogoutTime, SessionID, UserName, "SERVER", "EXIT");
}
/// <summary>
/// 关闭窗口（安全退出）
///IsFromLogin:是否从登录调用
/// </summary>
function CloseWindow(IsFromLogin) {
    var msg = "<span style='font-size:12px;'>你确定要退出系统吗？</span>";

    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            lock: true,
            fixed: true,
            title: '系统消息',
            opacity: 0.3,
            content: msg,
            button: [{
                name: '取消'
            }, {
                name: '确定',
                callback: function () {

                    try {
                        // 是否来至登录页面
                        if (IsFromLogin != 1) {
                            SendExitMessage();
                        }
                    } catch (e) { }

                    try {
                        DisposeVideoCapture(); //在加载页面之前判断是否开启采集卡资源，有则关闭
                    } catch (e) { }

                    // 调用 webBrowser 应用程序的退出函数
                    try {
                        window.external.ExitWindow();
                    } catch (e) { }

                    //                     调用IE的退出函数
                    try {
                        window.opener = null;
                        window.open(" ", "_self"); //IE7必须的
                        self.close();
                    } catch (e) { }

                    return true;

                }, focus: true
            }]
        });
    }

}
function MinimizeWindow() {
    // 调用 webBrowser 应用程序的最小化函数
    try {
        window.external.MinimizeWindow();
    } catch (e) { }
}

/// <summary>
/// 关闭窗口（安全退出）
/// </summary>
function LogoutSystem() {
    var msg = "你确定要注销登录吗？";

    if (msg != "") {
        art.dialog({
            id: 'artDialogID',
            lock: true,
            fixed: true,
            title: '系统消息',
            opacity: 0.3,
            content: msg,
            button: [{
                name: '取消'
            }, {
                name: '确定',
                callback: function () {
                    SendLogOutMessage();
                    DisposeVideoCapture(); //在加载页面之前判断是否开启采集卡资源，有则关闭
                    location.replace("/Logout.aspx");
                    return true;
                }, focus: true
            }]
        });
    }

}

/// <summary>
/// 从身份证件中读取用户信息  黄兴茂 2013-08-19
/// </summary>
function GetUserInfoByIDCard(IDCard) {
    this.year = '';
    this.month = "";
    this.day = "";
    this.birthday = '';
    this.age = 0;
    this.sex = -1;
    this.sexName = "";
    if (IDCard == undefined || IDCard == "")
        return false;
    var IDCardLegth = IDCard.length;
    var xb = 0;
    var d = new Date();
    if (IDCardLegth == 15) {
        this.year = "19" + IDCard.substr(6, 2);
        this.month = IDCard.substr(8, 2);
        this.day = IDCard.substr(10, 2);
        xb = IDCard.substr(14, 1);
        this.birthday = this.year + '-' + this.month + '-' + this.day;
        if (xb % 2 == 0) {
            this.sex = 2;
            this.sexName = "女";
        }
        else {
            this.sex = 1;
            this.sexName = "男";
        }
        this.age = d.getYear() - 1900 - this.year;
    }
    else if (IDCardLegth == 18) {
        this.year = IDCard.substr(6, 4);
        this.month = IDCard.substr(10, 2);
        this.day = IDCard.substr(12, 2);
        xb = IDCard.substr(16, 1);
        this.birthday = this.year + '-' + this.month + '-' + this.day;
        if (xb % 2 == 0) {
            this.sex = 0;
            this.sexName = "女";
        }
        else {
            this.sex = 1;
            this.sexName = "男";
        }
        this.age = d.getYear() - this.year;
    }
}

/// <summary>
/// 导出Excel通用方法，该方法主要是从新连接中打开需要导出的Excel文件
///XMHuang 2013-09-12 新增
/// </summary>
function OutToExcel() {
    var href = jQuery.trim(jQuery("#loadExcel").attr("href"));
    if (href != "") {
        window.open(href, null, "dialogWidth:420px; dialogHeight:320px; scroll:no; status:no; resizable:no");
    }
    return false;
}

/***************************字符串操作通用脚本 Begin xmhuang 2013-09-24************************************/
var TT = {

    /*
    * 获取光标位置
    * @Method getCursorPosition
    * @param t element
    * @return number
    */
    getCursorPosition: function (t) {
        if (document.selection) {
            t.focus();
            var ds = document.selection;
            var range = ds.createRange();
            var stored_range = range.duplicate();
            stored_range.moveToElementText(t);
            stored_range.setEndPoint("EndToEnd", range);
            t.selectionStart = stored_range.text.length - range.text.length;
            t.selectionEnd = t.selectionStart + range.text.length;
            return t.selectionStart;
        } else return t.selectionStart
    },


    /*
    * 设置光标位置
    * @Method setCursorPosition
    * @param t element
    * @param p number
    * @return
    */
    setCursorPosition: function (t, p) {
        this.sel(t, p, p);
    },

    /*
    * 插入到光标后面
    * @Method add
    * @param t element
    * @param txt String
    * @return
    */
    add: function (t, txt) {
        var val = t.value;
        if (document.selection) {
            t.focus()
            document.selection.createRange().text = txt;
        } else {
            var cp = t.selectionStart;
            var ubbLength = t.value.length;
            var s = t.scrollTop;
            // document.getElementById('aaa').innerHTML += s + '<br />';
            t.value = t.value.slice(0, t.selectionStart) + txt + t.value.slice(t.selectionStart, ubbLength);
            this.setCursorPosition(t, cp + txt.length);
            // document.getElementById('aaa').innerHTML += t.scrollTop + '<br />';
            firefox = navigator.userAgent.toLowerCase().match(/firefox\/([\d\.]+)/) && setTimeout(function () {
                if (t.scrollTop != s) t.scrollTop = s;
            }, 0)
        };
    },


    /*
    * 删除光标 前面或者后面的 n 个字符
    * @Method del
    * @param t element
    * @param n number  n>0 后面 n<0 前面
    * @return
    * 重新设置 value 的时候 scrollTop 的值会被清0
    */
    del: function (t, n) {
        var p = this.getCursorPosition(t);
        var s = t.scrollTop;
        var val = t.value;
        t.value = n > 0 ? val.slice(0, p - n) + val.slice(p) :
      val.slice(0, p) + val.slice(p - n);
        this.setCursorPosition(t, p - (n < 0 ? 0 : n));
        firefox = navigator.userAgent.toLowerCase().match(/firefox\/([\d\.]+)/) && setTimeout(function () {
            if (t.scrollTop != s) t.scrollTop = s;
        }, 10)
    },

    /*
    * 选中 s 到 z 位置的文字
    * @Method sel
    * @param t element
    * @param s number
    * @param z number
    * @return
    */
    sel: function (t, s, z) {
        if (document.selection) {
            var range = t.createTextRange();
            range.moveEnd('character', -t.value.length);
            range.moveEnd('character', z);
            range.moveStart('character', s);
            range.select();
        } else {
            t.setSelectionRange(s, z);
            t.focus();
        }
    },


    /*
    * 选中一个字符串
    * @Method sel
    * @param t element
    * @param s String
    * @return
    */
    selString: function (t, s) {
        var index = t.value.indexOf(s);
        index != -1 ? this.sel(t, index, index + s.length) : false;
    }
}
/***************************字符串操作通用脚本 End xmhuang 2013-09-24************************************/

/// 判断是否按下回车按键
function IsEnterKeyDown() {
    var curEvent = window.event || e;
    var id = document.activeElement.id;

    if (curEvent.keyCode == 13) {
        return true;
    }
    return false;
}

/// 判断是否按下ESC键
function IsEscKeyDown() {
    var curEvent = window.event || e;
    var id = document.activeElement.id;

    if (curEvent.keyCode == 27) {
        return true;
    }
    return false;
}


/***************************接口信息发送 Begin xmhuang 2013-10-12************************************/
/// <summary>
/// 发送体检号信息到LES，PACS接口 xmhuang 2013-10-12
/// </summary>
function SendWaitToInterfaceByClient_Ajax(Forsex, ID_Customer) {
    if (Forsex == 1 || Forsex == "男") {
        Forsex = 1;
    }
    else if (Forsex == 2 || Forsex == "女") {
        Forsex = 0;
    }

    //ajax请求：由于字符在get请求中超长，这里必须使用post方式提交请求
    jQuery.ajax({
        type: "POST",
        cache: false,
        url: "/Ajax/AjaxRegiste.aspx",
        data: { Forsex: Forsex, ID_Customer: ID_Customer, action: 'SendWaitToInterfaceByClient' },
        dataType: "json",
        success: function (msg) {

        },
        complete: function () {

        }
    });
}
/***************************接口信息发送 End xmhuang 2013-10-12************************************/

/***************************模拟队列类 Begin xmhuang 2013-10-20************************************/

var Qu = {}; //定义一个对象，用于
//队列构造函数 先进先出
Qu.Queue = function (len) {
    this.capacity = len; //设置队列最大容量
    this.list = new Array(); //队列数据
};
//添加入队方法
Qu.Queue.prototype.enqueue = function (data) {
    if (data == null) return;
    if (data == undefined) return;
    if (this.list.length >= this.capacity) {
        this.list.remove(0);
    }
    this.list.push(data);
};

//添加出队方法
Qu.Queue.prototype.dequeue = function (data) {
    if (this.list == null) return;
    if (this.list.length == 0) return;
    this.list.remove(0);
};

//添加获取队列长度方法
Qu.Queue.prototype.size = function () {
    if (this == null) return;
    return this.list.length;
};

//添加获取队列是否为空方法
Qu.Queue.prototype.isEmpty = function () {
    if (this == null || this.list == null) return false;
    return this.list.length > 0;
};

//对象数组扩展属性
Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) {
        return false;
    }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n + 1] = this[i];
        }
    }
    this.length -= 1;
};


/// <summary>
/// 获取元素自身的html代码 20140103 by wtang
/// </summary>
jQuery.fn.outerHTML = function (s) {
    return (s) ? this.before(s).remove() : jQuery("<p>").append(this.eq(0).clone()).html();
}


/***************************模拟队列类 End xmhuang 2013-10-20************************************/


/// <summary>
/// 刷新当前页面
/// </summary>
function RefreshCurrentPage() {
    DisposeVideoCapture();
    //location.replace('/System/Index.aspx'); 
    location.reload();
}

/// <summary>
/// 释放掉视频资源 xmhuang 2013-10-29
/// </summary>
function DisposeCamera() {
    if (document.getElementById("TakePhoto") != undefined || document.getElementById("TakePhoto") != null) {
        try {
            document.getElementById("TakePhoto").CloseCam();
            //document.getElementById("TakePhoto").Exit();
        }
        catch (e)
       { }
    }
}



/// <summary>
/// 释放掉采集卡资源 Wtang 2013-12-24
/// </summary>
function DisposeVideoCapture() {
    ComClose();
    var VideoCapture = null;

    if (VideoCapture == null || VideoCapture == undefined) {

        if (window.top.document.getElementById("FrameVideoCapture") == null) { return; }

        var IFrameVideoCapture = window.top.document.getElementById("FrameVideoCapture").contentWindow;
        VideoCapture = IFrameVideoCapture.document.getElementById("VideoCapture");                     //获取采集卡插件
    }

    if (VideoCapture != null && VideoCapture != undefined) {
        try {
            VideoCapture.StopVideoDisplay();
            VideoCapture.CloseVideoDisplay();
        } catch (e) { }
    }
}


/// <summary>
/// 获取服务器信息，目前只返回了服务器当前时间 xmhuang 2013-12-25
/// </summary>
function GetServerInfo() {
    var allServerInfo = "";
    jQuery.ajax({
        type: "POST",
        async: false,
        url: "/Ajax/AjaxRegiste.aspx",
        data: { action: "GetServerInfo" },
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        cache: false,
        dataType: "json",
        success: function (msg) {
            allServerInfo = msg;
        }
    });
    return allServerInfo;
}



function InitComRead() {
    //    var divComReadContent01 = "<object id='ComReadObject' classid='clsid:88D76990-883F-4678-9B08-CDF13021373B'></object>";
    //    var divComReadContent02 = "<script type='text/javascript' language='javascript'>function ComReadObject::SendComRecvDataEvent(x,y,z){ ShowComReadData(x,y,z); }<//script>";
    //    jQuery("#divComRead").html(divComReadContent01);
    //    jQuery("#divComReadScript").html(divComReadContent02);
    OpenRecvComData();
}

function ShowComReadData(type, data) {
    //ShowSystemWarningDialog(type + "_" + data);
    if (type == 1) {
        var dataArray = data.split("|");
        if (dataArray.length > 2 && dataArray[0] != "0" && dataArray[0] != "") {
            if (jQuery("#txtSym_2295_103") != undefined) {

                var feeid_symid = "2295_103";
                if (jQuery("#txtSym_" + feeid_symid).val() == "") {
                    jQuery("#txtSym_" + feeid_symid).val(dataArray[0]);
                    jQuery("#txtResult_" + feeid_symid).val(dataArray[0] + jQuery("#txtSymUnit_" + feeid_symid).html());
                    jQuery("#txtSym_" + feeid_symid).focus();
                }
                feeid_symid = "2295_104";
                if (jQuery("#txtSym_" + feeid_symid).val() == "") {
                    jQuery("#txtSym_" + feeid_symid).val(dataArray[1]);
                    jQuery("#txtResult_" + feeid_symid).val(dataArray[1] + jQuery("#txtSymUnit_" + feeid_symid).html());
                    jQuery("#txtSym_" + feeid_symid).focus();
                }
            }
        }
    }
    else if (type == 2) {
        var dataArray = data.split("|");
        if (dataArray.length > 2 && dataArray[0] != "0" && dataArray[0] != "") {
            if (jQuery("#txtSym_2295_103") != undefined) {
                var feeid_symid = "2295_6057";
                if (jQuery("#txtSym_" + feeid_symid).val() == "") {
                    jQuery("#txtSym_" + feeid_symid).val(dataArray[0]);
                    jQuery("#txtResult_" + feeid_symid).val(dataArray[0] + jQuery("#txtSymUnit_" + feeid_symid).html());
                    jQuery("#txtSym_" + feeid_symid).focus();
                }
                feeid_symid = "2295_101";
                if (jQuery("#txtSym_" + feeid_symid).val() == "") {
                    jQuery("#txtSym_" + feeid_symid).val(dataArray[1]);
                    jQuery("#txtResult_" + feeid_symid).val(dataArray[1] + jQuery("#txtSymUnit_" + feeid_symid).html());
                    jQuery("#txtSym_" + feeid_symid).focus();
                }
                feeid_symid = "2295_102";
                if (jQuery("#txtSym_" + feeid_symid).val() == "") {
                    jQuery("#txtSym_" + feeid_symid).val(dataArray[2]);
                    jQuery("#txtResult_" + feeid_symid).val(dataArray[2] + jQuery("#txtSymUnit_" + feeid_symid).html());
                    jQuery("#txtSym_" + feeid_symid).focus();
                }
            }
        }
    }
}

function OpenRecvComData() {
    var params01 = "";
    var params02 = "";
    try {
        params01 = ComReadObject.GetCOMConfigParams("1");
        params02 = ComReadObject.GetCOMConfigParams("2");
    } catch (e) {
        return;
    }

    var paramsArray01 = params01.split("|");
    var paramsArray02 = params02.split("|");

    var PortNumber01 = paramsArray01[0];          // 血压 端口号
    var Baudrate01 = paramsArray01[1];            // 血压 波特率
    var DataBit01 = paramsArray01[2];             // 血压 数据位
    var CheckBit01 = paramsArray01[3];            // 血压 校验位
    var StopBit01 = paramsArray01[4];             // 血压 停止位
    var DataAnalysisParams01 = paramsArray01[5];  // 血压 数据解析方式


    if (PortNumber01 == undefined || PortNumber01 == null || PortNumber01 == "") {
        art.dialog({
            id: 'artDialogID',
            lock: true,
            fixed: true,
            zIndex: 560,
            opacity: 0.3,
            content: '【提示】未设置串口参数或参数丢失。<br/>设置参数才能自动采集【血压计】和【身高体重】仪器的数据！',
            button: [{
                name: '确定',
                callback: function () {
                    jQuery("#txtCustomerID").focus();  // 设置选中文本框中的文字，并获取光标
                }
            }],
            close: function () {
                jQuery("#txtCustomerID").focus();  // 设置选中文本框中的文字，并获取光标
            }
        });

        return;
    }

    try {
        ComReadObject.SetComParam(PortNumber01, Baudrate01, DataBit01, CheckBit01, StopBit01, 1, DataAnalysisParams01, jQuery.trim(jQuery('#txtCustomerID').val()));

        ComReadObject.GetComRecvData();
        jQuery("#txtSym_2295_103").focus();
    } catch (e) {
        return;
    }
}
// 
function ChangeRecvComData(type) {

    var params01 = "";
    var params02 = "";
    try {
        params01 = ComReadObject.GetCOMConfigParams("1");
        params02 = ComReadObject.GetCOMConfigParams("2");
    } catch (e) {
        return;
    }


    var paramsArray01 = params01.split("|");
    var paramsArray02 = params02.split("|");

    var PortNumber01 = paramsArray01[0];          // 血压 端口号
    var Baudrate01 = paramsArray01[1];            // 血压 波特率
    var DataBit01 = paramsArray01[2];             // 血压 数据位
    var CheckBit01 = paramsArray01[3];            // 血压 校验位
    var StopBit01 = paramsArray01[4];             // 血压 停止位
    var DataAnalysisParams01 = paramsArray01[5];  // 血压 数据解析方式

    if (PortNumber01 == undefined || PortNumber01 == null || PortNumber01 == "") {
        ShowSystemDialog("未设置串口参数或参数丢失，设置参数才能自动采集【血压计】和【身高体重】仪器的数据！");
        return;
    }

    var PortNumber02 = paramsArray02[0];          // 身高体重 端口号
    var Baudrate02 = paramsArray02[1];            // 身高体重 波特率
    var DataBit02 = paramsArray02[2];             // 身高体重 数据位
    var CheckBit02 = paramsArray02[3];            // 身高体重 校验位
    var StopBit02 = paramsArray02[4];             // 身高体重 停止位
    var DataAnalysisParams02 = paramsArray02[5];  // 身高体重 数据解析方式
    try {
        if (type == "1") {          // 血压
            ComReadObject.SetComParam(PortNumber01, Baudrate01, DataBit01, CheckBit01, StopBit01, 1, DataAnalysisParams01, jQuery.trim(jQuery('#txtCustomerID').val()));
            ComReadObject.GetComRecvData();
            jQuery("#txtSym_2295_103").focus();
        } else if (type == "2") {   // 身高体重
            ComReadObject.SetComParam(PortNumber02, Baudrate02, DataBit02, CheckBit02, StopBit02, 2, DataAnalysisParams02, jQuery.trim(jQuery('#txtCustomerID').val()));
            ComReadObject.GetComRecvData();
            jQuery("#txtSym_2295_101").focus();
        }
    } catch (e) { }
}

/// <summary>
/// 关闭Com端口
/// </summary>
function ComClose() {
    try {
        var ComReadInterface = document.getElementById("ComReadObject");

        if (ComReadInterface != undefined && ComReadInterface != null) {
            ComReadInterface.ComClose();

        }
    } catch (e)
    { }
}



/// <summary>
/// 获取两个时间相差天数 xmhuang 2014-01-14 -1:开始时间小于结束时间 -2:结束日期格式不正确 -3:开始日期格式不正确
/// </summary>
function GetTimeSpanOfDay(BeginTime, EndTime) {
    //判断日期格式是否正确
    var newBeginTime = new Date(BeginTime.replace("-", "/")); // new Date(bYear, bMonth, bDay);
    if (isNaN(newBeginTime)) {
        return -3;
    }
    var newEndTime = new Date(EndTime.replace("-", "/")); //new Date(eYear, eMonth, eDay);
    if (isNaN(newEndTime)) {
        return -2;
    }
    var spanDays = newEndTime.getTime() - newBeginTime.getTime(); //Date.parse(newEndTime) - Date.parse(newBeginTime);
    if (spanDays < 0) {
        return -1;
    }
    return spanDays / (1000 * 60 * 60 * 24);
}
/// <summary>
/// 通用日期间隔判断调用接口 xmhuang 2014-01-14
/// </summary>
function CheckTime(BeginTime, EndTime) {
    var spanDays = GetTimeSpanOfDay(BeginTime, EndTime);
    if (spanDays == -1) {
        ShowSystemWarningDialog("开始时间不得小于结束时间！");
        return false;
    }
    else if (spanDays == -2) {
        ShowSystemWarningDialog("结束日期格式不正确！");
        return false;
    }
    else if (spanDays == -3) {
        ShowSystemWarningDialog("开始日期格式不正确！");
        return false;
    }
    else if (spanDays > 35) {
        ShowSystemWarningDialog("时间跨度不得大于35天！");
        return false;
    }
    return true;
}

/// <summary>
/// 表格奇数偶数行背景设置 20140320 by WTang
/// </summary>
function SetTableRowStyleFocusBg() {
    $(".stripe tr").unbind();
    $(".stripe tr").mouseover(function () {
        //如果鼠标移到class为stripe的表格的tr上时，执行函数   
        $(this).addClass("over");


       // ShowTestErrorMsg("调用 .stripe tr mouseover SetTableRowStyle()  函数（commom.js）");

    }).mouseout(function () {
        //给这行添加class值为over，并且当鼠标一出该行时执行函数   
        $(this).removeClass("over");


       // ShowTestErrorMsg("调用 .stripe tr mouseout SetTableRowStyle()  函数（commom.js）");

    }) //移除该行的class   
    $(".stripe tr:odd").removeClass("alt");
    $(".stripe tr:even").addClass("alt");
    //给class为stripe的表格的偶数行添加class值为alt
}

/// <summary>
/// 表格奇数偶数行背景设置 20140320 by WTang
/// </summary>
function SetTableRowStyle() {
    $(".stripe tr:odd").removeClass("alt");
    $(".stripe tr:even").addClass("alt");
    //给class为stripe的表格的偶数行添加class值为alt
}


/// <summary>
/// 表格奇数偶数行背景设置 20140320 by WTang
/// </summary>
function SetTableEvenOddRowStyle() {

    $(".stripe tr:odd").removeClass("alt");
    $(".stripe tr:even").addClass("alt");
    //给class为stripe的表格的偶数行添加class值为alt


    //ShowTestErrorMsg("调用 SetTableEvenOddRowStyle() 函数（commom.js）");

}

function SetTableRowStyleQT() { //这个就是传说的ready   
    $(".qttable tr").unbind();
    $(".qttable tr").mouseover(function () {
        //如果鼠标移到class为stripe的表格的tr上时，执行函数   
        $(this).addClass("over1");

        //ShowTestErrorMsg("调用 .qttable tr mouseover SetTableRowStyleQT() 函数（commom.js）");

    }).mouseout(function () {
        //给这行添加class值为over，并且当鼠标一出该行时执行函数   
        $(this).removeClass("over1");

        //ShowTestErrorMsg("调用 .qttable tr mouseout SetTableRowStyleQT() 函数（commom.js）");

    }) //移除该行的class   
    $(".qttable tr:even").addClass("alt1");
    //给class为stripe的表格的偶数行添加class值为alt
};

function SetTableEvenOddRowStyleQT() {

    $(".qttable tr:odd").removeClass("alt1");
    $(".qttable tr:even").addClass("alt1");
    //给class为stripe的表格的偶数行添加class值为alt
};

//中文星期数组 
var CHWeekArray = ["星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
//英文日期数组
var ENWeekArray = ["SunDay", "Monday", "TuesDay", "WednesDay", "Thursday", "Friday", "Saturday"];
function GetWeekIndex(WeekName) {
    for (var i = 0; i < CHWeekArray.length; i++) {
        if (CHWeekArray[i].toLowerCase() == WeekName.toLowerCase() || ENWeekArray[i].toLowerCase() == WeekName.toLowerCase()) {
            return i;
        }
    }
}
//获取指定索引中文星期名称
function GetCHWeekName(WeekIndex) {
    if (WeekIndex >= CHWeekArray.length)
        return ""
    if (CHWeekArray[WeekIndex] != undefined) {
        return CHWeekArray[WeekIndex];
    }
}
//获取指定索引英文星期名称
function GetENWeekName(WeekIndex) {
    if (ENWeekArray[WeekIndex] != undefined) {
        return ENWeekArray[WeekIndex];
    }
}


/// <summary>
/// 设置加载区域的高度
/// </summary>
function SetContentLoadFormHeight() {

    //    var ClientHeight = document.documentElement.clientHeight;   // 窗口高度
    //    var HeaderHeight = jQuery("#SysHeader").height();           // 头部高度
    //    var SysFooterHeight = 70;        // 底部高度
    //    var LoadFormHeight = jQuery("#loadForm").height();          // 内容区域的高度
    //    var tmpHeight = parseInt(ClientHeight) - parseInt(SysFooterHeight) - parseInt(HeaderHeight);
    //    if (tmpHeight > 0) {
    //        jQuery("#loadForm").css("min-height", tmpHeight);
    //    }

}


/// <summary>
/// 获取当前时间
/// </summary>
function GetCurrentTime() {
    try {
        var currDateTime = new Date();
        var Month = currDateTime.getMonth() + 1;
        var CurrDate = currDateTime.getDate();
        var Hours = currDateTime.getHours();
        var Minutes = currDateTime.getMinutes();
        var Seconds = currDateTime.getSeconds();

        var strTime = "当前时间：";
        strTime += currDateTime.getFullYear() + "年";
        strTime += (Month < 10 ? "0" + Month : Month) + "月";
        strTime += (CurrDate < 10 ? "0" + CurrDate : CurrDate) + "日 ";

        strTime += (Hours < 10 ? "0" + Hours : Hours) + ":";
        strTime += Minutes < 10 ? "0" + Minutes : Minutes;
        //        strTime += Seconds < 10 ? ":0" + Seconds : ":" + Seconds;
        return strTime;
    } catch (e) {
    }
}


/// <summary>
/// 查询出病症级别大于某个值的所有体征词,用于在系统中进行提示
/// </summary>
function QueryCustomerExamItemDiseaseLevelTips() {

    //    // 分科锁定
    //    if (jQuery('#Is_SectionLock').val() == "True") {
    //        return;
    //    }

    // 分科弃检
    if (jQuery('#Is_GiveUp').val() == "True") {
        return;
    }
    var ExamState = jQuery('#ExamState').val();                         //体检状态,当次体检信息的状态：0表示在线，1表示归档，2表示在一号分库，3表示在二号分库…
    //体检状态,当次体检信息的状态：0表示在线，1表示归档，2表示在一号分库，3表示在二号分库…
    if (ExamState != "0") {
        return; // 已归档
    }


    var CustomerID = jQuery.trim(jQuery('#txtCustomerID').val());
    var SectionID = jQuery('#txtSectionID').val();
    var MinDiseaseLevel = 2;

    // 病症级别提示开始值 及 各个等级对应颜色值 1:蓝色 2:黄色 3:橙 4:红色 严重程度依次表示 一般，较重，严重，特别严重
    var DiseaseLevelWarningString = jQuery('#DiseaseLevelWarning').val();

    var tempArray = DiseaseLevelWarningString.split("|");
    if (tempArray.length > 0) {
        MinDiseaseLevel = tempArray[0];
    }

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxCustExam.aspx",
        data: { CustomerID: CustomerID,
            SectionID: SectionID,
            MinDiseaseLevel: MinDiseaseLevel,
            action: 'QueryCustomerExamItemDiseaseLevelTipsList',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "json",
        success: function (jsonmsg) {

            if (jsonmsg == null || jsonmsg == "") {
                return;
            }

            //                    var DiseaseLevelTipsFrameTempleteContent = jQuery('#DiseaseLevelTipsFrameTemplete').html();     //读取框架模版
            //                    var DiseaseLevelTipsContentTempleteContent = jQuery('#DiseaseLevelTipsContentTemplete').html();     //读取内容模版
            //                    var tempContent = "";
            //                    // 病症级别列表
            //                    jQuery(jsonmsg.dataList0).each(function (i, examitem) {
            //                        tempContent +=
            //                             DiseaseLevelTipsContentTempleteContent.replace(/@ExamItemName/gi, examitem.ExamItemName)
            //                            .replace(/@ResultSummary/gi, examitem.ResultSummary)
            //                            .replace(/@ResultLabRange/gi, examitem.ResultLabRange);
            //                    });

            //                    if (tempContent != "") {
            //                        tempContent = DiseaseLevelTipsFrameTempleteContent.replace(/@DiseaseLevelTipsContentList/gi, tempContent);
            //                        ShowSystemDialog(tempContent);
            //                    }

            var DiseaseLevelTipsFrameTempleteContent = jQuery('#DiseaseLevelTipsFrameTemplete').html();     //读取框架模版
            var tempContent = "";
            var DiseaseLevelColor = "";
            // 病症级别列表
            jQuery(jsonmsg.dataList0).each(function (i, examitem) {
                // 根据病症级别的数值，返回对应的颜色值
                DiseaseLevelColor = GetDiseaseLevelColor(examitem.DiseaseLevel);
                tempContent = DiseaseLevelTipsFrameTempleteContent.replace(/@DiseaseLevel/gi, examitem.DiseaseLevel).replace(/@DivDiseaseLevel/gi, "DivDiseaseLevel");
            });

            if (tempContent != "") {
                ShowDiseaseLevelDialog(tempContent);
            }

        }
    });
}


/// <summary>
/// 根据病症级别的数值，返回对应的颜色值
/// </summary>
function GetDiseaseLevelColor(tempDiseaseLevel) {

    // 病症级别提示开始值 及 各个等级对应颜色值 1:蓝色 2:黄色 3:橙 4:红色 严重程度依次表示 一般，较重，严重，特别严重
    // DiseaseLevelWarning 中的value值配置格式如下：   1|1:Blue|2:Yellow|3:Orange|4:Red
    var DiseaseLevelWarningString = jQuery('#DiseaseLevelWarning').val();
    var DiseaseLevelColor = "";
    var tempArray = DiseaseLevelWarningString.split("|");
    if (tempArray.length > 1) {
        var tempColorArray = "";
        for (var k = 1; k < tempArray.length; k++) {
            tempColorArray = tempArray[k].split(":");
            if (tempColorArray.length > 1) {
                if (tempColorArray[0] == tempDiseaseLevel) {
                    DiseaseLevelColor = tempColorArray[1]; // 病症级别对应的颜色值
                }
            }
        }
    }
    return DiseaseLevelColor;
}


/// <summary>
/// 头部的切换
/// </summary>
function SwitchHeader(num) {


    jQuery("#ReturnListType").attr("listtype", num); // 设置列表类型 

    ShowSubTitleName(); // 显示二级菜单的名称
    switch (num) {
        case 1: // 一般的头部
            jQuery("#LoginUserMenuClassDivReturn").hide();         // 工作台头部
            jQuery("#LoginUserMenuClassDivFrame").show();
            DoLoad("", "");
            break;
        default: // 工作台头部
            jQuery("#LoginUserMenuClassDivFrame").hide();         // 一般的头部
            jQuery("#LoginUserMenuClassDivReturn").show();

            jQuery("#ReturnConsle").hide();
            jQuery("#ReturnListType").show();
            // 弃检管理      收退费
            if (num == 6 || num == 8) {
                jQuery("#ReturnListType").hide(); // 不显示 切换到 列表的按钮
            }
            break;
    }


    return; // 20150413 不切换工作台头部。 wtang 


    //    jQuery("#SysHeaderGeneral").hide();         // 一般的头部
    //    jQuery("#SysHeaderSection").hide();         // 分科的头部
    //    jQuery("#SysHeaderConclusion").hide();      // 总检头部
    //    jQuery("#SysHeaderConclusionCheck").hide(); // 总审头部

    //    switch (num) {
    //        case 1: // 一般的头部
    //            jQuery("#SysHeaderGeneral").show();
    //            break;
    //        case 2: // 分科的头部
    //            jQuery("#SysHeaderSection").show();
    //            break;
    //        case 3: // 总检头部
    //            jQuery("#SysHeaderConclusion").show();
    //            break;
    //        case 4: // 总审头部
    //            jQuery("#SysHeaderConclusionCheck").show();
    //            break;
    //        case 5: // 预约登记
    //            
    //            break;
    //    }

}
// 返回到列表
function ReturnToOperList() {

    jQuery("#ReturnListType").hide();
    jQuery("#ReturnConsle").show();
    var num = parseInt(jQuery("#ReturnListType").attr("listtype"));
    switch (num) {
        case 1: // 
            break;
        case 2: // 分科
            ShowSectionList();
            break;
        case 3: // 总检
            DoLoad('/System/ConclusionEx/ConclusionList.aspx', '');
            break;
        case 4: // 总审
            DoLoad('/System/ConclusionEx/ConclusionCheckList.aspx', '');
            break;
        case 5: // 预约登记
            RedirectSearch();
            break;
        case 6: // 弃检管理
            break;
        case 7: // 回收管理
            DoLoad('/System/GuideSheet/GuideSheetReturnList.aspx', '');
            break;
        case 8: // 收退费
            LoadDefaultPage();
            break;
    }
}

// 返回到工作台
function ReturnToOperConsle() {
    var url = GetSubMenuUrl();
    if (url != undefined && url != "") {

        jQuery("#ReturnConsle").hide();
        jQuery("#ReturnListType").show();

        DoLoad(url, '');
    }
}

function GetSubMenuUrl() {
    var url = jQuery("#hiddenSubMenuUrl").val();
    if (url == undefined || url == "") {
        url = GetCookie("SubMenuUrl");
    }
    return url;
}

function SetSubMenuUrlCookie(targeturl) {
    jQuery("#hiddenSubMenuUrl").val(targeturl);
    SetCookie('SubMenuUrl', targeturl);
}

// 返回到分科检查的列表页面  20140402 by wtang
function ShowSectionList() {
    var SectionID = jQuery("#txtSectionID").val();
    var href = "/System/Exam/ExamList.aspx?txtSectionID=" + SectionID;
    DoLoad(href, "");
}

/// <summary>
/// 高度自适应浏览器 
/// </summary>
function SetContentDivAutoHeight() {

    //    var otherInfoHeight = 350; // 剩余高度
    //    var minHeight = 300;

    //    var setPageOIHeight = jQuery("#otherInfoHeight").val(); // 获取页面配置的其余部分的高度
    //    var dataMinHeight = jQuery("#dataMinHeight").val(); // 最小高度
    //    if (parseInt(setPageOIHeight) > 0) {
    //        otherInfoHeight = setPageOIHeight;
    //    }
    //    // 最小高度
    //    if (parseInt(dataMinHeight) > 0) {
    //        minHeight = dataMinHeight;
    //    }
    //    var SetHeight = jQuery(window).height() - otherInfoHeight;
    //    if (SetHeight < minHeight) SetHeight = minHeight;

    //    if (jQuery("#fklscen") != undefined) {
    //        jQuery("#fklscen").css({ "height": SetHeight + "px", "overflow-y": "auto", "overflow-x": "hidden" });
    //    }
}

/// <summary>
/// 初始化 放大编辑 修改为委托的方式加载编辑器放大效果 20140423 by wtang , YLuo
/// </summary>
function InitBigEditer() {

    // 

    //    jQuery(".texttcc .text-title a").unbind();
    //    jQuery(".b-edit").dblclick(function () {
    //        var $t = jQuery(this), $box = jQuery(".texttcc"), top = jQuery(window).scrollTop(), h = $box.height(), w = $box.width(), sw = jQuery(window).width(), sh = jQuery(window).height();
    //        $box.data("textarea", $t);

    //        jQuery(".texttcc .Editboxdiv-title .Editboxdiv-title-btke strong").html(jQuery(this).attr("boxtitle") == undefined ? "编辑框放大" : jQuery(this).attr("boxtitle"));

    //        $box.css({ 'top': top + ((sh - h) / 2), 'left': (sw - w) / 2 });
    //        jQuery("body").append('<div class="m-media">');
    //        jQuery(".m-media").css("height", jQuery(document).height());
    //        $box.show().find(".text-edit").html('<textarea>' + $t.val() + '</textarea>');
    //        jQuery(".texttcc .text-edit textarea").focus();  // 获取焦点 
    //    });
    //    jQuery(".texttcc .text-title a").bind("click", function () {
    //        var txt = jQuery(this).html();
    //        jQuery(".texttcc .text-edit textarea").insertAtCaret(txt);
    //        return false;
    //    });
    //    jQuery(".texttcc .clexit").click(function (event) { jQuery('.texttcc,.m-media').fadeOut('slow').data("target", "") });
    //    jQuery(".texttcc .text-save").click(function () {
    //        var $text = jQuery(".texttcc").data("textarea"),
    //			 val = jQuery(".texttcc textarea").val();
    //        $text.val(val);
    //        jQuery('.texttcc,.m-media').fadeOut('slow').data("target", "");
    //    });

}


/// <summary> 
/// 将指定ID设置到屏幕中间
/// </summary>
function SetDivToScreenCenter(id) {
    try {
        var divid = document.getElementById(id);
        divid.style.left = (jQuery(window).width() - jQuery('#' + id).outerWidth()) / 2;
        divid.style.top = (jQuery(window).height() - jQuery('#' + id).outerHeight()) / 2 + jQuery(document).scrollTop();
        //    divid.style.top = (document.body.client.clientHeight / 2 - divid.clientHeight / 2)+"px"; //
        //   divid.style.top = (jQuery(window).height() - jQuery('#' + id).outerHeight()) / 2 + jQuery(document).scrollTop() / 2;

    } catch (e)
        { }
}


/// <summary> 
/// 获取科室快速切换列表
/// </summary>
function GetCustomerSectionQuickSwitchList() {

    // 判断是否已经加载了科室列表
    if (jQuery("#SectionQuickSwitchList").attr("isloaddata") == "1") {
        return;
    }

    var CustomerID = jQuery.trim(jQuery('#HiddenCustomerID').val());
    jQuery("#SectionQuickSwitchList").html("<li>正在加载科室...</li>");
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

            // 检查Ajax返回数据的状态等 20140430 by wtang 
            jsonmsg = CheckAjaxReturnDataInfo(jsonmsg);
            if (jsonmsg == null || jsonmsg == "") {
                return;
            }

            var newcontent = "<ul>";
            var endhtml = "";
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
                    if (endhtml != "") {
                        newcontent += endhtml;
                        endhtml = "";
                    }
                    newcontent +=
                            ExamSectionTemplateContent.replace(/@SectionName/gi, sectionitem.SectionName)
                            .replace(/@ID_Section/gi, sectionitem.ID_Section)
                            .replace(/@CustomerID/gi, CustomerID)
                            .replace(/@Number/gi, sectioncount)
                            ;

                    if (sectioncount % 10 == 0) {
                        endhtml = "</ul><ul>";
                    }
                });
                newcontent += "</ul>";
                endhtml = "";
                jQuery("#SectionQuickSwitchList").html(newcontent);
                jQuery("#SectionQuickSwitchList").attr("isloaddata", "1");
                ShowSectionQuickSwitch();

            } else {
                jQuery('#iptCustomerIDSectionExam').val(""); //  查询失败，清空体检号

                jQuery("#SectionQuickSwitchList").html("<li>未找到科室</li>");
            }
        }
    });


}

/// <summary> 
/// 显示科室快速切换列表
/// </summary>
function ShowSectionQuickSwitch() {

    jQuery(".KscenAroww").unbind();
    jQuery(".KscenCenCenter-center li a").unbind();
    jQuery(".KscenCen").unbind();

    jQuery(".KscenAroww").click(function (e) {
        var $t = $(this),
		    state = $t.data("state");
        if (!state) {
            $(".KscenCen").show();
            if (undefined === state) {
                var h = $(".KscenCenCenter-center").height();
                $(".KscenCenCenter-left,.KscenCenCenter-right").height(h);
            }
            $t.data("state", 1);
        }
        else {
            $(".KscenCen").hide();
            $t.data("state", 0);
        }
    });

    jQuery(".KscenCenCenter-center li a").click(function (e) {
        var $t = $(this);
        $t.parents(".KscenCenCenter-center").find("li").removeClass("active");
        $(this).parents("li").addClass("active");


        ShowSubTitleName_Name($(this).attr("name")); // 显示二级菜单名称

        return false;

        return false;
    });

    var timer;
    $(".KscenCen").bind("mouseover", function () {
        if (timer) {
            clearTimeout(timer);
            timer = undefined;
        }
    }).bind("mouseout", function (e) {
        e.stopPropagation();
        var $t = $(this);
        timer = setTimeout(function () {
            $t.hide();
            $(".KscenAroww").data("state", 0);
        }, 300);
        return false;
    });

    $(".KscenAroww").click();
}

function FormartFloatTo2Bits(val) {
    if (parseFloat(val) == val) {
        return parseFloat(val) * 100 / 100;
    } else {
        return val;
    }
}
// 滚动条滚动到底部
function ScrollToTop(id) {
    jQuery("#" + id).scrollTop(jQuery("#" + id)[0].scrollHeight);
}
//加载通用空白页面方法 xmhuang 2014-04-18
function LoadDefaultPage() {
    DoLoad("/System/Welcome.aspx", "");
    SwitchHeader(1);
}

//根据指定长度截断显示对象内容，并返回截断后的值 xmhuang 2014-04-19
//oldText:元字符，length：需要截断的长度，dispLayLitter：截断后末尾需要追加显示的字符
function DisplayLitterLetter(oldText, length, dispLayLitter) {
    if (oldText == null || oldText == undefined || oldText == "")   //过滤不存在的对象
    { return oldText; }
    oldText = jQuery.trim(oldText);                            //清空空格
    //获取字符实际长度
    var chinesRegex = /[^\x00-\xff]/ig;
    var curLength = oldText.replace(chinesRegex, "**").length;
    //    var curLength = oldText.length;                                 //获取字符长度
    if (curLength <= length) {
        return oldText;
    }
    else {
        var newText = "";
        var singleChar = "";
        var newLength = 0;
        for (var i = 0; i < curLength; i++) {
            singleChar = oldText.charAt(i).toString();
            if (singleChar.match(chinesRegex) != null) {
                newLength += 2;
            }
            else {
                newLength += 1;
            }
            if (newLength > length) {
                break;
            }
            newText += singleChar;
        }
        return newText + "...";
    }
}

//显示截断后的字符，不带...
function DisplayCutLitterLetter(oldText, length) {
    if (oldText == null || oldText == undefined || oldText == "")   //过滤不存在的对象
    { return oldText; }
    oldText = jQuery.trim(oldText);                            //清空空格
    //获取字符实际长度
    var chinesRegex = /[^\x00-\xff]/ig;
    var curLength = oldText.replace(chinesRegex, "**").length;
    //    var curLength = oldText.length;                                 //获取字符长度
    if (curLength <= length) {
        return oldText;
    }
    else {
        var newText = "";
        var singleChar = "";
        var newLength = 0;
        for (var i = 0; i < curLength; i++) {
            singleChar = oldText.charAt(i).toString();
            if (singleChar.match(chinesRegex) != null) {
                newLength += 2;
            }
            else {
                newLength += 1;
            }
            if (newLength > length) {
                break;
            }
            newText += singleChar;
        }
        return newText;
    }
}
// 在编辑修改数据后，
function ShowModifyTableListInfo(id, tmpValue) {
    if (tmpValue == "" || tmpValue == null || tmpValue == undefined) {
        tmpValue == "&nbsp;";
    }
    if (jQuery("#" + id + " div") != null && jQuery("#" + id + " div").html() != null && jQuery("#" + id + " div").html() != undefined) {
        jQuery("#" + id + " div").html(tmpValue);
        jQuery("#" + id).attr("title", tmpValue);
    } else {
        jQuery("#" + id).html(tmpValue);
    }
}

/// <summary>
/// 检测是否登录超时等情况 20140429 by wtang
/// </summary>
function CheckAjaxReturnDataInfo(retmsg) {
    if (retmsg == null || retmsg=="") {
        // 未获取到数据，或获取数据出错
        return "";
    } else if (retmsg == "-9999") {
        ReLoginConfirm();
        return "";
    } else {
        // 判断是否任然是当前进行的登录
        var retCurrentUserMsg = JudgeISCurrentUserLogin();

        if (retCurrentUserMsg == "-9999" || retCurrentUserMsg == "-9998") {
            ReLoginConfirm();
            return "";
        } else {
            return retmsg;
        }
    }
}


/// <summary>
/// 判断是否任然是当前进行的登录 20141030 by wtang
/// </summary>
function JudgeISCurrentUserLogin() {
    var retCurrentUserMsg = "";
    var LoginUserID = jQuery("#CookieUserID",parent.document).val();   // 已经登录的用户ID
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxUser.aspx",
        data: { LoginUserID: LoginUserID,
            action: 'JudgeISCurrentUserLogin',
            currenttime: encodeURIComponent(new Date())
        },
        async: false,
        cache: false,
        dataType: "text",
        success: function (retmsg) {
            retCurrentUserMsg = retmsg;
        }
    });
    return retCurrentUserMsg;
}
/// <summary>
/// 登录超时的处理 20140429 by wtang
/// </summary>
function ReLoginConfirm() {
    var CookieLoginName = jQuery("#CookieLoginName",parent.document).val();
    var tipscontent = '<table class="ModifyPassword" style="width:350px;">' +
        '<tbody>' +
        '    <tr><td class="left">用户名：</td><td><input maxlength="30" type="text" name="txtTempUserName" id="txtTempUserName" style="width:180px; height:20px; line-height:20px; background-color:#ccc;color:#ff6600;" value="' + CookieLoginName + '"  readonly="readonly"  /> &nbsp;</td></tr>' +
        '    <tr><td class="left">密　码：</td><td><input maxlength="30" type="password" name="txtTempPassword" id="txtTempPassword"  style="width:180px; height:20px; line-height:20px;" /> &nbsp;</td></tr>' +
        '    <tr><td  style="text-align:center;" class="msg" colspan="2" id="tempLoginMsg">&nbsp;</td></tr>' +
        '</tbody>' +
        '</table>';

    art.dialog({
        id: 'artDialogID',
        content: tipscontent,
        lock: true,
        title: '请重新登录',
        fixed: true,
        zIndex: 555,
        opacity: 0.3,
        button: [{
            name: '登录',
            callback: function () {
                isShowLoginDialog = 0; // 标记是否已经显示登录框 20141030 by wtang
                // location.replace('/Login.aspx?flag=logout');
                UserReLoginInfo();
                return false;
            }, focus: true
        }, {
            name: '取消',
            callback: function () {
                isShowLoginDialog = 0; // 标记是否已经显示登录框 20141030 by wtang
                return true;
            }
        }],
        close: function () {
            isShowLoginDialog = 0; // 标记是否已经显示登录框 20141030 by wtang
            return true;
        }
    });

}

function ShowUserReLoginMsg(msg) {
    if (msg != "") {
        jQuery("#tempLoginMsg").html(msg);
        jQuery("#tempLoginMsg").show();
    } else {
        jQuery("#tempLoginMsg").hide();
    }
}
/// <summary>
/// 用户重新登录
/// </summary>
function UserReLoginInfo() {
    ShowUserReLoginMsg("");
    var txtTempUserName = jQuery("#txtTempUserName").val();
    var txtTempPassword = jQuery("#txtTempPassword").val();

    if (txtTempUserName == "") {
        jQuery("#txtTempUserName").focus();
        jQuery("#txtTempUserName").select();
        ShowUserReLoginMsg("请输入用户名！");
        return false;
    }

    if (txtTempPassword == "") {
        jQuery("#txtTempPassword").focus();
        jQuery("#txtTempPassword").select();
        ShowUserReLoginMsg("请输入密码！");
        return false;
    }

    $.post("/ajax/AjaxUser.aspx?action=UserLogin", { UserLoginName: txtTempUserName, UserPassword: txtTempPassword,
        VerifyCode: "",
        param: Math.random()
    },
        function (data) {
            if (data != "" && data != null) {
                ShowUserReLoginMsg(data);

                if (data == "登录成功") {

                    // 获取登录用户信息 20140504 by wtang
                    GetUserLoginInfo();

                    // 登录成功后，关闭登录框
                    art.dialog({ id: 'artDialogID' }).close();
                }
                return false;
            }
        }
    )
    .complete(function () {
        $("#btnLogin").removeAttr("disabled");
    })
    .success(function () { })
    .error(function () {
        ShowUserReLoginMsg("登录失败，请与系统管理人员联系！");
    });


    return true;
}

/// <summary>
/// 获取登录用户信息 20140504 by wtang
/// </summary>
function GetUserLoginInfo() {

    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxUser.aspx",
        data: { action: 'GetUserLoginInfo',
            currenttime: encodeURIComponent(new Date())
        },
        cache: false,
        dataType: "text",
        success: function (msg) {

            if (msg != null && msg != "-9999" && msg != "") {
                var tmpUserInfoArray = msg.split('|');
                if (tmpUserInfoArray.length > 5) {
                    jQuery("#CookieUserID", parent.document).val(tmpUserInfoArray[0]); // 操作员ID
                    jQuery("#CookieLoginName",parent.document).val(tmpUserInfoArray[1]); // 操作员登录名
                    jQuery("#CookieUserName", parent.document).val(tmpUserInfoArray[2]); // 操作员姓名
                    jQuery("#OperateLevel", parent.document).val(tmpUserInfoArray[3]);   // 操作员等级
                    jQuery("#LoginVocationType", parent.document).val(tmpUserInfoArray[4]); // 操作员分类
                    jQuery("#CookieSessionID", parent.document).val(tmpUserInfoArray[5]); // SessionID

                    jQuery(".TopShowUserName", parent.document).html("用户：" + tmpUserInfoArray[2] + "&nbsp;&nbsp;"); // 操作员姓名
                    jQuery(".ShowUserName", parent.document).html(tmpUserInfoArray[2]); // 操作员姓名
                }
            }
        }
    });
}

/// <summary>
/// 登录超时的处理 20140429 by wtang
/// </summary>
function ReLoginConfirm_bak() {
    var tipscontent = "请重新登录后再操作！<br/>【确定】点击确定，立刻重新登录！<br/>【取消】点击取消，稍后再登录！";

    art.dialog({
        id: 'artDialogID',
        content: tipscontent,
        lock: true,
        fixed: true,
        zIndex: 555,
        opacity: 0.3,
        button: [{
            name: '确定',
            title: '提示',
            callback: function () {
                // location.replace('/Login.aspx?flag=logout');

                return true;
            }, focus: true
        }, {
            name: '取消',
            callback: function () {
                return true;
            }
        }],
        close: function () {
            return true;
        }
    });

}

/// <summary>
/// 判断浏览器状态
/// <summary>
function IsFullScreen() {
    if (window.outerHeigth == screen.heigth && window.outerWidth == screen.width) {
        alert("全屏");
    }
    else {
        alert("不是全屏");
    }
}


/*** JS 读写文件 start ***/
var gJudgeGetCustomerInfoIntervalID = 0;
/// <summary>
/// 创建文件
/// <summary>
function CreateCustomerSectionFile() {
    var SectionID = jQuery("#txtSectionID").val();          // 科室ID
    var CustomerID = jQuery("#txtHiddenCustomerID").val();  // 体检号
    var filePath = jQuery("#CustomerFileSavePath").val();
    if (filePath == "" || filePath == undefined) { ShowSystemDialog("请先配置本地文件保存目录！"); return false; }

    var fileName = CustomerID + "." + SectionID;
    if (!CreateFile(filePath, fileName, CustomerID)) {
        ShowSystemDialogAutoClose("创建体检号【" + CustomerID + "】对应文件失败，请检查相应的配置是否正确？", jQuery("#txtCustomerID"));
    } else {
        gJudgeGetCustomerInfoIntervalID = window.setInterval(function () {
            JudgeGetCustomerBaseInfoIsSuccess(CustomerID, SectionID);
        }, 300);
    }
}

/// <summary>
/// 判断是否返回结果
/// <summary>
function JudgeGetCustomerBaseInfoIsSuccess(CustomerID, SectionID) {
    JudgeGetCustomerBaseInfoResult(CustomerID, SectionID, "succ");
    JudgeGetCustomerBaseInfoResult(CustomerID, SectionID, "fail");
}

/// <summary>
/// 判断文件是否存在
/// <summary>
function JudgeGetCustomerBaseInfoResult(CustomerID, SectionID, flag) {

    var filePath = jQuery("#CustomerFileResultFlagSavePath").val();
    if (filePath == "" || filePath == undefined) { clearInterval(gJudgeGetCustomerInfoIntervalID); return false; }

    var fileName = CustomerID + "." + SectionID + "." + flag;
    if (JudgeFileIsExist(filePath, fileName)) {
        clearInterval(gJudgeGetCustomerInfoIntervalID);
        var fso, tf;
        // 文件完整路径
        var fileFullName = filePath + "\\" + fileName;
        // 创建文件
        fso = new ActiveXObject("Scripting.FileSystemObject"); // 获取对象
        // 判断文件是否存在
        if (fso.FileExists(fileFullName)) {
            tf = fso.GetFile(fileFullName);
            tf.Delete();
        }
        var tmpCustomerName = jQuery("#CustomerNameText").html();
        if (tmpCustomerName != "") {
            if (flag == "succ") {
                ShowSystemDialogAutoClose("发送客户【" + tmpCustomerName + "】的基本信息成功！", jQuery("#txtCustomerID"));
            } else {
                ShowSystemDialogAutoClose("发送客户【" + tmpCustomerName + "】的基本信息失败，请核对网络是否正常，或体检号是否正确！", jQuery("#txtCustomerID"));
            }
        }
        else {
            ShowSystemDialogAutoClose("发送体检号【" + CustomerID + "】的基本信息失败，请核对网络是否正常，或体检号是否正确！", jQuery("#txtCustomerID"));
        }
        return true;
    }
    else {
        return false;
    }
}

/// <summary>
/// 创建文件
/// <summary>
function CreateFile(filePath, fileName, content) {
    var fso, tf;
    // 文件完整路径
    var fileFullName = filePath + "\\" + fileName;
    // 创建文件
    fso = new ActiveXObject("Scripting.FileSystemObject"); // 获取对象
    // 判断文件是否存在
    if (fso.FileExists(fileFullName)) {
        tf = fso.GetFile(fileFullName);
        tf.Delete();
    }

    // 判断文件夹是否存在
    if (!fso.FolderExists(filePath)) {
        var astrPath = filePath.split("\\");
        var ulngPath = astrPath.length;
        var strTmpPath = "";
        for (var i = 0; i < ulngPath; i++) {
            strTmpPath = strTmpPath + astrPath[i] + "\\";
            if (!fso.FolderExists(strTmpPath)) {
                fso.CreateFolder(strTmpPath);
            }
        }
    }
    // 判断文件夹是否存在
    if (!fso.FolderExists(filePath)) {
        fso.CreateFolder(filePath);
    }
    // 创建一个文件
    tf = fso.CreateTextFile(fileFullName, true);
    if (content != "") {
        // 如果内容不为空，则写入到文件中。
        tf.WriteLine(content);
    }
    // 关闭文件
    tf.Close();

    // 判断文件是否已经生成，并返回文件是否创建成功的标记。
    if (fso.FileExists(fileFullName)) {
        return true;
    } else {
        return false;
    }
}

/// <summary>
/// 判断文件是否存在
/// <summary>
function JudgeFileIsExist(filePath, fileName) {

    var fso, tf;
    // 文件完整路径
    var fileFullName = filePath + "\\" + fileName;
    // 创建文件
    fso = new ActiveXObject("Scripting.FileSystemObject"); // 获取对象

    // 判断文件是否存在
    if (fso.FileExists(fileFullName)) {
        return true;
    } else {
        return false;
    }
}

/*** JS 读写文件 end ***/


/// <summary>
/// 判断表格是否存在滚动条,并设置相应的样式
/// <summary>
function JudgeTableIsExistScroll() {

    var bgclassname = "TableHeaderBg";
    jQuery(".j-autoHeight").each(function () {

        bgclassname = "TableHeaderBg";
        if (jQuery(this).prev().height() > 40) {
            bgclassname = "TableHeaderBg2";
        }

        if (jQuery(this).prev().height() == 39) {
            bgclassname = "TableHeaderBg3";
        }
        if (jQuery(this).prev().attr("name") != "notaddscrollheadarea") {
            if (jQuery(this).scrollTop() > 0) {
                jQuery(this).prev().addClass(bgclassname);
            }
            else {
                jQuery(this).scrollTop(1);
                if (jQuery(this).scrollTop() > 0) {
                    jQuery(this).prev().addClass(bgclassname);

                }
                else {
                    jQuery(this).prev().removeClass(bgclassname);
                }
                jQuery(this).scrollTop(0);
            }
        }
    });
    ResetShowText();

}
/// <summary>
/// 判断表格是否存在滚动条,并设置相应的样式
/// <summary>
function JudgeTableIsExistScrollByID(id) {
    jQuery("#" + id).scrollTop(1);
    if (jQuery("#" + id).scrollTop() > 0) {
        jQuery("#" + id).prev().addClass("TableHeaderBg");
    } else {
        jQuery("#" + id).prev().removeClass("TableHeaderBg");
    }
    jQuery("#" + id).scrollTop(0);
    ResetShowText();
}

var resize_flag = false;
jQuery(window).resize(function () {

    //    if (!resize_flag) {
    //        resize_flag = true;
    //        setTimeout(function () { resize_flag = false; }, 100);

    BandMuenClassDownListEvent_New(); // 计算并绑定下拉菜单的事件 20150310 by wtang

    ResetShowText();
    //由于在非全屏下存在横向滚动条拉动后，全屏时margin-left值将为负值，需要重置  xmhuang20150303
    if (jQuery(".j-control-title").length > 0) {
        jQuery(".j-control-title").css("margin-left", "0");
    }
    if (jQuery(".j-autoHeight").length > 0) {
        jQuery(".j-autoHeight").scrollLeft(jQuery(".j-autoHeight").scrollLeft() + 1);
    }
    if (jQuery("#QueryExamListData").length > 0 && jQuery("#ShowUserMenuDiv").length > 0) {
        jQuery("#QueryExamListData").attr("data-left", (269 + jQuery("#ShowUserMenuDiv").height()));
    }
    if (jQuery(".j-autoHeight").length > 0) {
        jQuery(".j-autoHeight").autoHeight();
    }
    // 判断表格是否存在滚动条,并设置相应的样式
    JudgeTableIsExistScroll();
    SetHomePageBackGroupImagSize();
});
//设置首页图片背景大小
function SetHomePageBackGroupImagSize() {
    if (jQuery("#homePageDiv").length > 0) {
        if (jQuery("#homePageDiv").width() < 1042 || jQuery("#homePageDiv").height() < 560) {
            if (jQuery("#homePageDiv").length > 0) {
                jQuery("#homePageDiv").addClass("HomePage_Small");
                jQuery("#homePageDiv").removeClass("HomePage_Big");

            }
        }
        else {
            if (jQuery("#homePageDiv").length > 0) {
                jQuery("#homePageDiv").addClass("HomePage_Big");
                jQuery("#homePageDiv").removeClass("HomePage_Small");

            }
        }
    }
}
// 重置多余文字的省略符号显示样式
function ResetShowText() {
    jQuery(".nowrap").hide();
    jQuery(".nowrap").each(function () {
        jQuery(this).css("width", (jQuery(this).parent().width() - 2) + "px");
    });
    jQuery(".nowrap").show();
}


/// <summary>
/// 计算总检结论框所占用的宽度。
/// <summary>
function CalcFinialExamConclusionsWidth() {
    jQuery(".zjxz-jlc-nr-right").hide();
    var rightWidth = 0;
    jQuery(".zjxz-jlc-nr-right").each(function () {
        if (rightWidth == 0) {
            rightWidth = jQuery(this).parent().width() - jQuery(this).prev().width() - jQuery(this).prev().prev().width();
            //alert(rightWidth);
        }
        jQuery(this).css("width", (rightWidth - 10) + "px");
    });
    jQuery(".zjxz-jlc-nr-right").show();
}



function MoveShowMoreMenuClass() {
    jQuery("#LoginUserMenuClass").append(jQuery("#MenuClassDownList").html());
    jQuery("#MenuClassDownList li").remove();
    var tmpCount = 0;
    var tmpHiddenCount = 0;
    var defaultOffSet = 0;
    var beginmove = false;
    jQuery("#LoginUserMenuClass li").each(function () {
        if (tmpCount == 0) {
            defaultOffSet = jQuery(this).offset().top;
        }
        tmpCount++;
        if (beginmove == false && jQuery(this).offset().top > defaultOffSet) {
            beginmove = true;
        }
        if (beginmove == true) {
            tmpHiddenCount++;
            jQuery("#MenuClassDownList").append(jQuery(this));
        }
    });
    if (tmpHiddenCount > 0) {
        jQuery(".NavDownListAroww").show();
    } else {
        jQuery(".NavDownListAroww").hide();
    }
    // alert(tmpCount + " --- " + tmpHiddenCount);
}

/// <summary> 
/// 计算并绑定下拉菜单的事件
/// </summary>
function BandMuenClassDownListEvent() {
    MoveShowMoreMenuClass();
    jQuery(".NavDownListAroww").unbind();
    jQuery(".NavDownListCenter-center li a").unbind();
    jQuery(".NavDownList").unbind();

    jQuery(".NavDownListAroww").click(function (e) {
        var $t = $(this),
		    state = $t.data("state");
        if (!state) {
            $(".NavDownList").show();
            if (undefined === state || 0 == state) {
                var h = $(".NavDownListCenter-center").height();
                $(".NavDownListCenter-left,.NavDownListCenter-right").height(h);
            }
            $t.data("state", 1);
        }
        else {
            $(".NavDownList").hide();
            $t.data("state", 0);
        }

        return false;
    });

    jQuery(".NavDownListCenter-center li a").click(function (e) {
        var $t = $(this);
        $t.parents(".NavDownListCenter-center").find("li").removeClass("nav-on");
        $(this).parents("li").addClass("nav-on");

        DoLoad("", ''); // 加载welcome页面
        return false;
    });

    var timer;
    $(".NavDownList").bind("mouseover", function () {
        if (timer) {
            clearTimeout(timer);
            timer = undefined;
        }
    }).bind("mouseout", function (e) {
        e.stopPropagation();
        var $t = $(this);
        timer = setTimeout(function () {
            $t.hide();
            $(".NavDownListAroww").data("state", 0);
        }, 300);
        return false;
    });

    jQuery(".menu-actions li a").unbind();
    jQuery(".menu-actions li a").click(function () {
        //remove掉同级元素样式
        jQuery(".menu-actions li").removeClass("selected");
        jQuery(this).parent().addClass("selected");

        SetCookie('SubMenuID', jQuery(this).attr("menuid"));       // 点击菜单项，保存对应的menuid
        SetCookie('SubSectionID', jQuery(this).attr("sectionid")); // 点击菜单项，保存对应的sectionid （如果不是分检，则该值为空）

        DoLoad(jQuery(this).attr("targeturl"), '');
    });

    //    $(".NavDownListAroww").click();
}

/**图片大图显示函数**/
function ShowCustomerBigPic() {
    jQuery(".box").each(function (e) {
        jQuery(this).poshytip({
            content: '<div class="DialogPhoto"><img width="98" height="118" src="' + jQuery(this).parent().attr("src") + '"/><div></div>' + jQuery(this).parent().attr("CustomerName") + '</div>',
            bgImageFrameSize: 9,
            offsetX: 5,
            offsetY: 35,
            allowTipHover: false,
            fade: false,
            slide: false
        });
    });
}
//将IP地址转换成数字
function convertIP(what) {
    var myArray = what.split(/\./);
    var ip = (256 * 256 * 256) * (myArray[0]) + (256 * 256) * (myArray[1]) + 256 * (myArray[2]) + 1 + 1 * myArray[3];
    return ip;
}



var EnumMenuIconName = [
    { "name": "登记系统", "icon": "menu_icon1" },
    { "name": "财务管理", "icon": "menu_icon2" },
    { "name": "常规检查", "icon": "menu_icon3" },
    { "name": "检验检查", "icon": "menu_icon4" },
    { "name": "影像检查", "icon": "menu_icon5" },
    { "name": "总检系统", "icon": "menu_icon6" },
    { "name": "报告管理", "icon": "menu_icon7" },
    { "name": "报表系统", "icon": "menu_icon8" },
    { "name": "系统设置", "icon": "menu_icon9"}];


// 根据菜单名称获取样式名称
function GetMenuIconClass(menuname) {
    var tempicon = "";
    jQuery(EnumMenuIconName).each(function (i, key) {
        if (menuname == key.name) {
            tempicon = key.icon;
            return false; // break;
        }
    });
    return tempicon;
}
//实际体检地点配置 xmhuang 20150508 为了个性化预约凭证、缴费通知单、报告领取凭证而设定的体检地址配置
var IPParameters = { "本部": { "BeginIP": "192.170.1.1", "EndIP": "192.170.1.255", "Address": "川港康复中心大楼1楼", "ExamPlaceName": "本部体检中心（成都市一环路西二段32号）" },
    "二部": { "BeginIP": "192.192.124.1", "EndIP": "192.192.124.255", "Address": "草堂分部", "ExamPlaceName": "草堂体检中心（成都市大石西路62号）" }
};
//处理xml字符中的特殊字符为实体类型&:&amp;<:&lt;>:&gt;':&apos;"":&quot;
function CheckXmlChar(xmlChar) {
    if (xmlChar == undefined || xmlChar == null)
    { return xmlChar; }
    else {
        try {
            xmlChar = xmlChar.replace(/&/gi, "&amp;").replace(/</gi, "&lt;").replace(/>/gi, "&gt;").replace(/'/gi, "&apos;").replace(/""/gi, "&quot;");
        } catch (e) {
            return xmlChar;
        }
    }
    return xmlChar;
}


function DoLoad(href, parentContainer) {
    // 页面调整前，先判断是否已经登录，如果未登录，则弹出提示框进行提示。 20140429
    gLoadUrl = href;
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxUser.aspx",
        data: {
            action: 'JudgeUserIsLogin',
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
                } else {
                    parent.iframeLoad(gLoadUrl);
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