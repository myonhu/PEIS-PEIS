﻿
var IsDisplayOrHide = 0; 
var IsCommon = ""; 
var curFixed = 2; 
var curYJ = 0; 
var curZK = 0; 
var curSJ = 0; 
var defalutImagSrc = "/template/blue2/images/avtar.png"; 
var allBusSet = ''; 
var allBusFee = ''; 
var allFeeWay = ''; 
var allHiddFeeWay = ''; 
var choiceBusSetText = "--请选择套餐--";
var choiceCountryText = "--请选择国家--";
var choiceCultrulText = "--请选择教育背景--";
var choiceNationText = "--请选择民族--";
var choiceGuideNurseText = "--请选择导检护士--";
var choiceMarriedText = "请选择婚姻状态";
var type = "";
var IsTeam = "";  
var InputRigth = ""; 
var TargetID_Customer = ""; 
var TargetID_ArcCustomer = "";
var SynCardOcx1 = ""; 
var CVR_IDCard = ""; //银安身份证读卡插件
var FastReport = ""; //获取报表插件
var TakePhoto = ""; //获取拍照插件
var str = ""; //保存身份证读取信息
var IsSaved = true; //保存是否可用保存登记信息，默认可保存，在新增项目时将此标记变为false，保存成功后重置为true，此标记用于补打、保存
var SecurityLevelHtml = ""; //记录客户所有的操作密级
//定义56个民族
var NationArray = ['汉族', '蒙古族', '回族', '藏族', '维吾尔族', '苗族', '彝族', '壮族', '布依族', '朝鲜族', '满族', '侗族', '瑶族', '白族', '土家族', '哈尼族', '哈萨克族', '傣族', '黎族', '傈僳族', '佤族', '畲族', '高山族', '拉祜族', '水族', '东乡族', '纳西族', '景颇族', '柯尔克孜族', '土族', '达翰尔族', '仫佬族', '羌族', '布朗族', '撒拉族', '毛南族', '仡佬族', '锡伯族', '阿昌族', '普米族', '塔吉克族', '怒族', '乌孜别克族', '俄罗斯族', '鄂温克族', '德昂族', '保安族', '裕固族', '京族', '塔塔尔族', '独龙族', '鄂伦春族', '赫哲族', '门巴族', '珞巴族', '基诺族'];
//var IPParameters = { "本部": { "BeginIP": "192.170.1.1", "EndIP": "192.170.1.255", "Address": "川港康复中心大楼1楼", "ExamPlaceName": "本部体检中心（成都市一环路西二段32号）" },
//    "二部": { "BeginIP": "192.192.124.1", "EndIP": "192.192.124.255", "Address": "草堂", "ExamPlaceName": "草堂体检中心（成都市大石西路62号）" }
//};
//alert("IPParameters.本部.BeginIP:" + convertIP(IPParameters.本部.BeginIP));
//alert("IPParameters.本部.EndIP:" + convertIP(IPParameters.本部.EndIP));
//alert("IPParameters.二部.BeginIP:" + convertIP(IPParameters.二部.BeginIP));
//alert("IPParameters.二部.EndIP:" + convertIP(IPParameters.二部.EndIP));
/***********************变量定义 End******************************/

/*************页面初始化 Begin***********************/
jQuery(document).ready(function () {
    SwitchHeader(5); // 隐藏头部
    //jQuery("#divtblList").attr("data-left", (165 + jQuery("#ShowUserMenuDiv").height()));
    jQuery(".j-hiddenAway").hiddenAway();
    jQuery(".j-autoHeight").autoHeight();

    if (jQuery("#divCloneDataListShowDialog").length > 0) {
        jQuery("#divCloneDataListShowDialog").drag({ handler: jQuery(".copydiv-title") }); //克隆拖动
    }

    if (IsDisplayOrHide == 1) {
        jQuery(".DisplayOrHide").hide();
    }
    else {
        jQuery(".DisplayOrHide").show();
    }

    InputRigth = jQuery("#InputRigth").val();               //获取用户录入权限，该参数用于控制用户是否可用手动录入身份证信息
    TargetID_Customer = jQuery("#ID_Customer").val();       //跳转过来的体检号
    TargetID_ArcCustomer = jQuery("#ID_ArcCustomer").val(); //跳转过来的存档号
    SecurityLevelHtml = jQuery("#slOperateLevel").html();   //获取用户操作密级数据源 
    CVR_IDCard = document.getElementById("CVR_IDCard");     //获取身份证插件
    FastReport = document.getElementById("FastReport");     //获取报表插件
    TakePhoto = document.getElementById("TakePhoto");       //获取拍照插件
    type = jQuery("#type").val();                           //获取操作类型参数
    IsTeam = jQuery("#IsTeam").val();                       //获取是否团体参数
    IsCommon = jQuery("#IsCommon").val();                   //获取是否通用参数
    //如果是共用界面,则支持身份证和体检号共用检索功能 
    if (IsCommon == 1) {
        jQuery("#lblSearchTitle").html("&nbsp;&nbsp;证件号/体检号(F2)&nbsp;");
        jQuery("#lblPageTitle").text("通用登记");
    }
    else {
        if (IsTeam == 1) {
            jQuery("#lblSearchTitle").html("&nbsp;&nbsp;证件号/体检号(F2)&nbsp;");
            jQuery("#lblPageTitle").text("团体登记");
        }
        else {
            jQuery("#lblSearchTitle").html("&nbsp;&nbsp;体检号(F2)&nbsp;");
            //判断是预约还是登记
            if (jQuery('#modelName').val().toLowerCase() == "regist") {
                jQuery("#lblPageTitle").text("个人预约");
            }
            else if (jQuery('#modelName').val().toLowerCase() == "sign") {
                jQuery("#lblPageTitle").text("个人登记");
            }
            else if (jQuery('#modelName').val().toLowerCase() == "signandregiste") {
                jQuery("#lblPageTitle").text("预约登记");
            }
        }
    }
    jQuery("#divCloneRightFloat").show();                  // 显示克隆

    jQuery("#btnPrintCust1").data("data", data);           //设置打印参数和是否预约参数
    jQuery("#txtSearchX", parent.document).focus();                         //设置默认选中扫描文本框

    if (Base64PhtoStr == "") {
        jQuery("#HeadImg").attr("src", defalutImagSrc);
        jQuery("#divBox").attr("src", defalutImagSrc);  
        jQuery("#HeadImg").attr("Base64Photo", "");
    }
    else {
        jQuery("#HeadImg").attr("src", "data:image/gif;base64," + Base64PhtoStr);
        jQuery("#divBox").attr("src", "data:image/gif;base64," + Base64PhtoStr); 
        jQuery("#HeadImg").attr("Base64Photo", Base64PhtoStr);
    }
    //设置团体显示信息
    if (teamID == "") {
        jQuery("#didTeam").html("");
        jQuery("#divDepart span").text("");                //设置部门显示信息为空
        jQuery("#divRole span").text("");                  //设置角色显示信息为空 
    }

    ShowElementInfo();
    //设置预约还是登记显示 End

    jQuery("#showBusFee").hide();
    jQuery("#txtSearchX", parent.document).focus();

    allFeeWay = jQuery("#slFeeWay").html();                //获取所有的付费方式源数据
    allHiddFeeWay = jQuery("#hidslFeeWay");

    //设置性别变动套餐
    jQuery("#slSex").change(function () {
        RemoveSelectedSet(); // 清空套餐相应的数据
    });
    jQuery("#slSex").change(); //触发slSex change事件

    //体检类型变动事件
    jQuery("#idSelectSet").change(function () {
        if (jQuery("#idSelectSet").val() == "" && (jQuery("#idSelectSet").data("PEID") == undefined || jQuery("#idSelectSet").data("PEID") == "")) { return; } // 如果选择的体检类型为空值 ，则不进行下面的操作
        var checked = document.getElementById("chcAll").checked;
        var checkedStr = checked == true ? 'checked="checked"' : '';
        var curValue = jQuery(this).val();
        var data = {};
        var optionsLength = document.getElementById("slFeeWay").options.length;
        //移除未保存的项目，只显示当前套餐项目和已经保存的项目
        jQuery("#tblList tbody tr[exist='0']").remove();                            //移除未保存的项目
        if (curValue != "-1" || jQuery("#idSelectSet").data("PEPackageID") != undefined) {
            var PEPackageID = jQuery("#idSelectSet").val(); //获取套餐
            if (jQuery("#idSelectSet").data("PEPackageID") != undefined && jQuery("#idSelectSet").data("PEPackageID") != "") {
                data = { PEID: jQuery("#idSelectSet").data("PEPackageID"), action: 'GetBusFeeByCustomerID' };
            }
            else {
                data = { PEID: TargetID_Customer, PEPackageID: PEPackageID, action: 'GetBusFeeBySetID' };
            }
            jQuery.ajax({
                type: "GET",
                url: "/Ajax/AjaxRegiste.aspx",
                data: data,
                cache: false,
                dataType: "json",
                success: function (msg) {
                    // 检查Ajax返回数据的状态等 
                    msg = CheckAjaxReturnDataInfo(msg);
                    if (msg == null || msg == "") {
                        return;
                    }
                    allBusFee = msg;
                    var FeeWay = "", FeeWayName = "", PEID = "", newContent = "", exist = 0, CustCssStyle = "CustFeeChargeState";
                    var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("TemplateRegistCustFee");
                    var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
                    var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
                    var RowNum = jQuery("#tblList tbody tr[id!='loading']").length;
                    var XFeeChargeStaute = "", ItemCheckbox = "";
                    var oldtitle = ""; //设置tr鼠标提示值
                    jQuery(msg.dataList).each(function (i, item) {
                        //如果没有行或者没有当前ID的非退费项目则允许添加到列表中
                        if (jQuery("#tblList tbody tr[id!='loading'][id_fee='" + item.ID_Fee + "'][custfeechargestate!='2']").length == 0)//判断是否包含当前ID的非退费项目，不包含则新增
                        {
                            XFeeChargeStaute = "";
                            ItemCheckbox = "ItemCheckbox";
                            RowNum++;
                            if (jQuery.trim(item.PEID) != "") {
                                exist = 1;
                                PEID = jQuery("#idSelectSet").data("PEID");
                                FeeWay = item.FeeTypeID;
                                FeeWayName = item.FeeWayName;
                            }
                            else {
                                CustCssStyle = "NewXM";
                                if (document.getElementById("slFeeWay") != null) {
                                    FeeWay = document.getElementById("slFeeWay").value;
                                    FeeWayName = jQuery(allHiddFeeWay).find("option[value='" + document.getElementById("slFeeWay").value + "']").text();
                                }
                            }
                            //如果是团体项目则不允许删除，隐藏checkbox
                            if (item.ID_TeamTaskGroup != "") {
                                exist = 1;
                                CustCssStyle = "Yellow";
                                //判断收费状态
                                if (item.FeeChargeStaute == "2") {
                                    ItemCheckbox = "UnItemCheckbox";
                                    CustCssStyle = "Yellow";
                                    item.FeeChargeStaute = "√";
                                    XFeeChargeStaute = "√";
                                }
                                //判断是否是已检项目，已检项目不允许退费Is_Examined
                                else if (item.FeeChargeStaute == "1") {
                                    ItemCheckbox = "UnItemCheckbox";
                                    item.FeeChargeStaute = "√";
                                    CustCssStyle = "Green";
                                }
                                else if (item.FeeChargeStaute == "0") {
                                    ItemCheckbox = "ItemCheckbox";              // 团体未收费显示复选框
                                    CustCssStyle = "Red";                       // 团体未收费显示复选框
                                    item.FeeChargeStaute = "";
                                }
                            }
                            else {
                                //判断收费状态
                                if (item.FeeChargeStaute == "2") {
                                    ItemCheckbox = "UnItemCheckbox";
                                    CustCssStyle = "Yellow";
                                    item.FeeChargeStaute = "√";
                                    XFeeChargeStaute = "√";
                                }
                                //判断是否是已检项目，已检项目不允许退费Is_Examined
                                else if (item.FeeChargeStaute == "1") {
                                    item.FeeChargeStaute = "√";
                                    ItemCheckbox = "UnItemCheckbox";
                                    CustCssStyle = "Green";
                                }
                                else if (item.FeeChargeStaute == "0") {
                                    ItemCheckbox = "ItemCheckbox";
                                    item.FeeChargeStaute = "";
                                    CustCssStyle = "Red";
                                }
                            }
                            //设置禁用样式
                            oldtitle = "";
                            if (item.Is_Banned == "True" || item.Is_Banned == 1) {
                                CustCssStyle = CustCssStyle + " Banned";
                                oldtitle = "该收费项目已禁用";
                            }
                            //如果收费项目ID不存在，则表示此项目为新增但未保存状态
                            //未保持收费项目        黑色
                            //已保存未收费          蓝色
                            //已保存未退费          绿色
                            //已保存已退费          灰色
                            //禁用项目              红色
                            if (item.ID_CustFee == "") {
                                CustCssStyle = "NewXM";
                            }
                            //由于在从套餐中检索时，是没有折扣人、注册人字段，这里需要判断
                            if (teamTaskGroupFeeListBodyTempleteContent != null) {
                                newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/class=@CustCssStyle/gi, 'class="@CustCssStyle"')
                            .replace(/@type="text"/gi, "")
                            .replace(/@ItemCheckbox/gi, ItemCheckbox)
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                             .replace(/@exist/gi, exist)
                             .replace(/@Is_Org/gi, 0)
                            .replace(/@PEID/gi, PEID)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)
                            .replace(/@FeeChargeStaute/gi, item.FeeChargeStaute)
                            .replace(/@Price/gi, parseFloat(item.Price).toFixed(2))
                            .replace(/@FactPrice/gi, item.FactPrice)
                             .replace(/@Is_FeeRefund/gi, item.Is_FeeRefund)
                            .replace(/@Is_FeeCharged/gi, item.Is_FeeCharged)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                            .replace(/@date/gi, item.date)
                            .replace(/@FeeType/gi, FeeWay)
                            .replace(/@FeeWayName/gi, FeeWayName)
                            .replace(/@RowNum/gi, RowNum)
                            .replace(/@CustCssStyle/gi, CustCssStyle)
                            .replace(/@ID_CustFee/gi, item.ID_CustFee)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@Is_Printed/gi, item.Is_Printed)

                            .replace(/@Discount/gi, item.Discount)
                            .replace(/@ID_Discounter/gi, item.ID_Discounter)
                            .replace(/@XDiscounterName/gi, item.DiscounterName)
                            .replace(/@ID_Register/gi, item.ID_Register)
                            .replace(/@RegisterName/gi, item.RegisterName)
                            .replace(/@RegistDate/gi, item.RegistDate)
                            .replace(/@XFeeChargeStaute/gi, XFeeChargeStaute)
                            .replace(/@OperationalDate/gi, item.OperationalDate)
                            .replace(/@Is_Banned/gi, item.Is_Banned)    //新增是否禁用绑定值
                            .replace(/@tilte/gi, '"' + oldtitle + '"')//如果是默认属性，其key value没有用双引号括起来，所以这里需要特殊处理下 
                                //.replace(/@tilte/gi, oldtitle);    // 新增是否禁用绑定值
                            }
                        }
                    });
                    if (newContent != '') {
                        //jQuery("#tblList thead").html(teamTaskGroupFeeListTheadTempleteContent);
                        jQuery("#tblList tbody").append(newContent);
                        newContent = "";
                        DoScrollToBottom();
                        BindFeeWay();
                        BindKeyup();
                        SetTableEvenOddRowStyle();//设置奇偶项样式 
                        //SumJG();
                        //设置固定表头 
                        //$('#tblList').tablefix({ height: 200, width: 400, fixRows: 1, fixCols: 2 });
                    }
                    jQuery("#idSelectSet").removeData("ID_Customer");       //移除数据项
                    SumJG();                                                //计算合计

                    // 判断表格是否存在滚动条,并设置相应的样式
                    //JudgeTableIsExistScroll();
                }
            });
        }
    });

    //折扣变动事件
    jQuery("#txtXMZK").keyup(function () {
        SetDisscount();
        SumJG(); //计算合计
    });

    //绑定统一付费方式
    jQuery("#slFeeWay").html(jQuery(allHiddFeeWay).html());
    jQuery("#slFeeWay").change(function () {
        var id = '';
        var curSelectedValue = jQuery(this).val();
        jQuery("[name='ItemCheckbox']").each(function () {
            if (jQuery(this).attr("checked")) {
                jQuery(this).parent().parent().find("[name = 'sffs']").attr("feetype", curSelectedValue);
                jQuery(this).parent().parent().find("[name = 'sffs']").text(jQuery(allHiddFeeWay).find("option[value='" + curSelectedValue + "']").text());
            }
        }); SumJG();
    });

    //判断当前操作类型
    BindEditElement();
    //DoScrollToBottom();
    SetShowElement();
    HideAllQuickTableEvent();                                           // 通过注册焦点事件，隐藏弹出框
    ShowMe();                                                           //设置显示或隐藏列表

    //如果是是共用模块，不隐藏新增按钮和自定义证件按钮
    if (IsCommon == 1) {

    } else {
        if (IsTeam == 1) {
            jQuery("[name='btnAdd']").parent().hide();
            jQuery("#btnReadFromMachine").parent().hide();
            jQuery("#btnGenerate").hide();
        }
    }

    //如果是已经总检则隐藏保存按钮
    if (Is_Checked == 1 || Is_Checked == "True") {
        jQuery("[name='btnComplete']").parent().hide();                          //隐藏保存按钮
        jQuery("#divAddCustFee").hide();                                //隐藏新增收费项目操作组
        jQuery("#imgCustomer").hide();                                  //隐藏获取头像按钮
    }
    else {
        jQuery("[name='btnComplete']").parent().show();                          //显示保存按钮
        jQuery("#divAddCustFee").show();                                //显示新增收费项目操作组
        jQuery("#imgCustomer").show();                                  //显示获取头像按钮
    }
    DisposeCamera();

    //通过跳转过来的体检号判断是否隐藏团体显示信息
    var TypeCode = "";
    if (TargetID_Customer != undefined) {
        var TypeCode = TargetID_Customer.substring(1, 2);
    }
    if (TypeCode != 9) {

        //设置团体关联label为隐藏状态 

        jQuery(".sapnTeamTitle").hide();
        //重置团体信息
        jQuery("#spanTeamName").text("");
        //重置部门信息 
        jQuery("#spanDepart").text("");
        //重置角色信息 
        jQuery("#spanRoleName").text("");
    }
   
    ShowBigPic();

    //if (Is_Paused == 1)//已禁检的客户
    //{
    //    jQuery("[name='btnComplete']").parent().hide();
    //    ShowSystemDialog("对不起，该客户已禁检，不允许进行体检！");
    //    //return false;
    //}
    //通过跳转过来的体检号判断是否隐藏团体显示信息

    //通过IP设置默认体检地点
    //if (type == "Add" || type == "add") {
    //    var curIP = convertIP(jQuery("#ClientIP").val());
    //    if (parseFloat(convertIP(IPParameters.二部.BeginIP)) <= parseFloat(curIP) && parseFloat(curIP) <= parseFloat(convertIP(IPParameters.二部.EndIP))) {
    //        //这里设置二部默认地址 ，默认名称为“草堂分部”
    //        //alert("设置默认地点：" + IPParameters.二部.Address); 
    //        SetSelectByText(document.getElementById("slExamPlace"), IPParameters.二部.Address, false);
    //    }
    //    else {
    //        //alert(IPParameters.二部.Address);  
    //    }
    //    if (parseFloat(convertIP(IPParameters.本部.BeginIP)) <= parseFloat(curIP) && parseFloat(curIP) <= parseFloat(convertIP(IPParameters.本部.EndIP))) {
    //        //这里设置本部默认地址 
    //        //alert("设置默认地点：" + IPParameters.本部.Address);
    //    }
    //    else {
    //        //alert(IPParameters.本部.Address);
    //    }
    //}
});
//通过text内容设置下拉选中项
function SetSelectByText(objElement, text, IsFullEqual) {
    for (var c = 0; c < objElement.options.length; c++) {
        if (IsFullEqual) {
            if (objElement.options[c].innerHTML.toString() == text) {
                objElement.options[c].selected = true;
                break;
            }
        }
        else {
            if (objElement.options[c].innerHTML.toString().indexOf(text) > -1) {
                objElement.options[c].selected = true;
                break;
            }
        }
    }
}
/*************页面初始化 End***********************/

/// <summary>
/// 读取团体任务分组模版并返回thead部分和tbody部分html内容
///TemplateTeamTaskGroupID:模版ID
/// </summary>
function ReadTemplateTeamTaskGroup(TemplateTeamTaskGroupID) {
    //默认是读取tblTemplateTeamTaskList模版填充到tblTeamTask中显示
    if (TemplateTeamTaskGroupID == "" || TemplateTeamTaskGroupID == undefined) {
        TemplateTeamTaskGroupID = "TemplateRegistCustFee";
    }
    var teamTaskGroupListContent = ""; //团体任务分组table内容
    var teamTaskGroupListTheadTempleteContent = jQuery('#' + TemplateTeamTaskGroupID + ' thead').html();    //团体任务模版Thead部分
    var teamTaskListBodyTempleteContent = jQuery('#' + TemplateTeamTaskGroupID + ' tbody').html();          //团体任务模版body部分
    this.teamTaskGroupListTheadTempleteContent = teamTaskGroupListTheadTempleteContent;
    this.teamTaskListBodyTempleteContent = teamTaskListBodyTempleteContent;
    return this;
}

/*************Form表单按键操作 Begin**************/
function DoParentLoad() {
    DoLoad('/System/Customer/RegistList.aspx?type=' + type + '&modelName=' + jQuery('#modelName').val() + "&IsTeam=" + jQuery('#IsTeam').val(), '');
}
function OnFormKeyUp(e) {
    var curEvent = window.event || e;
    var id = document.activeElement.id;
    if (curEvent.keyCode == 13)//回车事件
    {
        //如果是在搜索中
        if (id == "txtSearch") {
            //SureAdd();
        }
        else {

            if (id == "txtSFZ")//如果光标在身份证号内，则自动通过体检身份证号检索信息
            {
                jQuery("#btnSearch").click(); return;
                //Search(id, "IDCard");
            }
            else if (id == "txtSearchX") //如果光标在扫描区则自动触发检索事件
            {
                jQuery("#btnSearch").click(); return;
            }
            else if (id == "txtTJH") //如果光标在体检号内，则自动通过体检号检索信息
            {
                Search(id, "ID_Customer"); return;
            }
            else if (id == "txtYKT")//如果光标在一卡通号内，则自动通过一卡通号检索信息
            {
                Search(id, "ExamCard"); return;
            }
            else if (id == "txtExamCard")//如果光标在扫描的体检号内，则将发送信息到HIS去
            {
                jQuery("#txtExamCard").select();
                SendExamInfoToHis();
            }
            else if (id == "txtUserLoginName" || id == "txtUserPassword")//如果更换折扣人
            {
                ChangeUserDiscount();
            }

        }
    }
    if (id == "txtSearch" && (curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40)) {
        keyMove(document.getElementById(id), curEvent); return;
    }
    return false;
}
/*************Form表单按键操作 End**************/



/*************折扣变动 Begin*******************/
function BindKeyup() {
    $("#tblList tr[CustFeeChargeState='0'] td label[name='zk']").keydown(function () {
        return (/[\d.]/.test(String.fromCharCode(event.keyCode)));
    }).keyup(function () {
        var curZK = jQuery.trim(jQuery(this).val());
        if (curZK == '')
            jQuery(this).val(DisCountRate);
        if (parseFloat(curZK) < DisCountRate || parseFloat(curZK) >= 10)
            jQuery(this).val(DisCountRate);
        if (DisCountRate == 0)
            jQuery(this).val(10);
        curYJ = jQuery(this).parent().parent().find('[name="yj"]').val();
        curZK = jQuery(this).val() * 10;
        var FactPrice = parseFloat(curYJ) * parseFloat(curZK) / 100;
        jQuery(this).parent().parent().find('[name="sj"]').val(parseFloat(FactPrice).toFixed(curFixed));
    }).change(function () {
        SumJG();
    });
}
/*************折扣变动 End*******************/

/**********绑定收费方式 Begin**********/
function BindFeeWay() {

    //jQuery("#tblList select[name='fffs']").select2();
    var FeeType = document.getElementById("slFeeWay").value;
    jQuery("#tblList tr td select[name='fffs']").each(function () {
        if (jQuery(this).children("option").length <= 0) {
            if (jQuery(this).attr("FeeType") != undefined) {
                FeeType = jQuery(this).attr("FeeType")
            }
            jQuery(this).empty();
            jQuery(this).append(jQuery("#slFeeWay").html()); //获取所有付费方式;
            jQuery(this).find("option").removeAttr("selected");
            jQuery(this).find("option[value='" + FeeType + "'").attr("selected", "selected");
        }
    });
}
/**********绑定收费方式 End**********/

/**********绑定页面元素设置显示值 Begin***********************/
function BindEditElement() {
    SetControlEnable();
    SetControlDefalut();
}
/**********绑定页面元素设置显示值 End***********************/

/**********设置选中项 Begin******************************/
function SetControlSelect() {
    jQuery("#slSex [forgender='" + selectedGender + "']").attr("selected", true);
    jQuery("#slMarried [value='" + selectedMarriage + "']").attr("selected", true);
    jQuery("#slReportWay [value='" + selectedReportWay + "']").attr("selected", true);
    jQuery("#slFeeWay option[value='" + selectedFeeWay + "']").attr("selected", true);
    //如果用户的操作密级大于了100，则证明该客户已经加密
    if (selectedSecurityLevel > 100) {
        jQuery("#slOperateLevel").html("<option value='" + selectedSecurityLevel + "'>已加密</option>");
    }
    else {
        jQuery("#slOperateLevel [value='" + selectedSecurityLevel + "']").attr("selected", true);
    }
    jQuery("#slCultrul [value='" + selectedCultrul + "']").attr("selected", true);
    jQuery("#slExamPlace [value='" + selectedExamPlace + "']").attr("selected", true);
    jQuery("#slOperateLevel [value='" + selectedOperateLevel + "']").attr("selected", true);

    jQuery("#idSelectSet").data("ID_Customer", jQuery("#txtTJH").val());

    var dddd = jQuery("#idSelectSet").data("ID_Customer");
    ShowQuickSelectUser(selectedGuideNurse, selectedGuideNurseName);    // 设置导检护士的已选人员
    ShowQuickSelectNation(selectedNation, selectedNationName);          // 设置民族的已选项
    ShowQuickSelectExamType(selectedExamType, selectedExamTypeName);    // 设置体检类型的已选项
    ShowQuickSelectSet(selectedSet, selectedPEPackageName, true);                   // 设置套餐的已选项
    //设置用户性别
    jQuery("#lblHidSex").text(document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].text);
}

//在存在体检号时需要隐藏年龄、体检时间
function SetHidOrShowItem() {
    var curCustomerID = jQuery("#lblHidCustomer").text();
    if (curCustomerID != "") {
        jQuery(".txtShow").hide();
        jQuery("#txtSubScribDate").hide();
        jQuery(".lblHid").show();
    }
    else {
        jQuery(".txtShow").show();
        jQuery("#txtSubScribDate").show();
        jQuery(".lblHid").hide();
    }
}
/**********设置选中项 End******************************/
//修改内容：如果是修改客户登记、预约、团体登记预约信息，则体检号、性别、出生日期为label显示
function SetShowElement() {
    //    jQuery(".txtShow").hide();
    //    jQuery(".lblHid").show();
    //    //return false;
    if (type.toLowerCase() == "edit") {
        jQuery(".txtShow").hide();
        jQuery(".lblHid").show();
        //获取性别
        jQuery("[name='lblHidSex']").text(document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].text);
        //jQuery("#btnReadFromMachine").hide();  //隐藏从设备读取按钮
    }
    else if (type.toLowerCase() == "add" || IsSearch == 1) {
        jQuery(".txtShow").show();
        jQuery(".lblHid").hide();
        //jQuery("#btnReadFromMachine").show(); //显示从设备读取按钮
    }
}

function ShowElementInfo() {
    /*设置显示值*/
    if (type.toLowerCase() == "edit") {
        if (jQuery('#modelName').val().toLowerCase() == "regist") {
            jQuery("#lblRegiste").text("体检日期");
            jQuery("[name='btnComplete']").val(" 完 成(F9) ");   // 完成预约
            jQuery("[name='btnAdd']").val(" 申 请 ");        // 申请新客户预约
            jQuery("[name='btnComplete']").attr("title", " 完成预约 ");     // 完成预约
            jQuery("[name='btnAdd']").attr("title", " 申请新客户预约 ");    // 申请新客户预约
        }
        else if (jQuery('#modelName').val().toLowerCase() == "sign") {
            //jQuery("#lblRegiste").text("登记时间");//xmhuang 2013-11-23 注释掉，统一显示为体检日期
            jQuery("#lblRegiste").text("体检日期");


            jQuery("[name='btnComplete']").val(" 完 成(F9) ");   // 完成登记
            jQuery("[name='btnAdd']").val(" 申 请 ");        // 申请新客户登记
            jQuery("[name='btnComplete']").attr("title", " 完成登记 ");  // 完成登记
            jQuery("[name='btnAdd']").attr("title", " 申请新客户登记 "); // 申请新客户登记
        }
    }
    else {
        if (jQuery('#modelName').val().toLowerCase() == "regist") {


        }
        else if (jQuery('#modelName').val().toLowerCase() == "sign") {


        }
        //jQuery("#btnReadFromMachine").show(); //显示从设备读取按钮，此按钮只能在新增状态下可见
        jQuery("#lblAge").text(""); //xmhuang 2013-12-04 置空年龄
    }

    /*设置显示*/
}
/*********设置页面下拉元素默认选中项 Begin********************/
function SetControlDefalut() {
    if (jQuery("#slCultrul").find("option[value='-1']").length > 0) {

    }
    else {
        jQuery("#slCultrul").prepend('<option code="qxz" value="-1">' + choiceCultrulText + '</option>'); //为select在第一个位置插入option
    }
    if (type.toLowerCase() == "add") {

        // 导检护士（如果是新增，则清空导检护士相应的数据）
        RemoveSelectedUser();
        // 体检类型（如果是新增，则清空体检类型相应的数据）
        RemoveSelectedExamType();
        // 套餐（如果是新增，则清空套餐相应的数据）
        RemoveSelectedSet();
        //        // 新增时，默认为汉族 
        //        ShowQuickSelectNation(1, "汉族");          // 设置民族的已选项

        jQuery("#slCultrul").find("option:selected").attr("selected", false);
        jQuery("#slCultrul").find("option[value='-1']").attr("selected", true);
        jQuery("#s2id_slCultrul .select2-choice span").text(choiceCultrulText);
        ShowQuickSelectExamType(1, "健康体检");    // 设置体检类型的已选项

        jQuery("#btnGenerate").show(); //显示无证件按钮
        jQuery("[name='btnAdd']").hide(); //隐藏新增按钮
        jQuery("#btnGenerate").removeAttr("disabled");
    }
    else if (type.toLowerCase() == "edit") {
        SetControlSelect();
        jQuery("#btnGenerate").hide();
        if (type.toLowerCase() == "edit")//如果是编辑状态不允许修改登记时间
        {
            jQuery("#txtSubScribDate").hide();
            //            jQuery("#txtSubScribDate").addClass("InputNoBorder"); //设置文本无边框显示
            //            jQuery("#txtSubScribDate").removeClass("Wdate"); //移除登记时间样式
        }
    }
}
/*********设置页面下拉元素默认选中项 End********************/

/*********设置不可编辑项 Begin*******************/
function SetControlEnable() {
    //隐藏检索按钮
    jQuery("#btnSearchX",parent.document).hide();
    if (type.toLowerCase() == "add") {
        //jQuery("#txtSFZ").removeAttr("readOnly");
        jQuery("#txtYKT").removeAttr("readOnly");

    }
    else if (type.toLowerCase() == "edit") {

        //jQuery("#txtSFZ").attr("readOnly", 'true');
        jQuery("#txtYKT").attr("readOnly", 'true');
        jQuery("#txtTJH").attr("readOnly", 'true');

    }
}
/// <summary>
/// 禁用保存、申请、注册按钮
/// 修改人：xmhuang
/// 修改日期：2013-11-14
function SetControlDisabled() {
    jQuery("#btnSaveData").attr("disabled", "disabled");
    jQuery("#btnSave").attr("disabled", "disabled");
    jQuery("#btnSave1").attr("disabled", "disabled");

    jQuery("#btnRegiste").attr("disabled", "disabled");
    jQuery("#btnRegiste1").attr("disabled", "disabled");

    jQuery("#btnSubScribe").attr("disabled", "disabled");
    jQuery("#btnSubScribe1").attr("disabled", "disabled");

}
/// <summary>
/// 重新启用保存、申请、注册按钮
/// 修改人：xmhuang
/// 修改日期：2013-11-14
function ReSetControlDisabled() {
    jQuery("#btnSaveData").removeAttr("disabled");
    jQuery("#btnSave").removeAttr("disabled");
    jQuery("#btnSave1").removeAttr("disabled");

    jQuery("#btnRegiste").removeAttr("disabled");
    jQuery("#btnRegiste1").removeAttr("disabled");

    jQuery("#btnSubScribe").removeAttr("disabled");
    jQuery("#btnSubScribe1").removeAttr("disabled");
}
/*********设置不可编辑项 End*******************/

/*********获取开设日期 Begin*******************/
function GetOperationalDate(OperationalDate) {
    var curWeekIndex = GetWeekIndex(CurWeek);
    var allWeekDayStr = "";
    //按星期开设
    if (OperationalDate.indexOf("week") > -1) {
        var weekArray = OperationalDate.split(':');
        if (weekArray.length > 1) {
            var weekDays = weekArray[1];
            if (weekDays != "") {
                if (weekDays.indexOf(curWeekIndex) < 0) {
                    var allWeekDayArray = weekDays.split(',');

                    for (var i = 0; i < allWeekDayArray.length; i++) {
                        if (allWeekDayArray[i] != "") {
                            curWeekStr = GetCHWeekName(allWeekDayArray[i]);
                            if (curWeekStr != "") {
                                allWeekDayStr += GetCHWeekName(allWeekDayArray[i]) + ",";
                            }
                        }
                    }
                    if (allWeekDayStr != "") {
                        allWeekDayStr = allWeekDayStr.substr(0, allWeekDayStr.length - 1);
                    }
                }
            }
        }
    }
    //按天开设
    else if (OperationalDate.indexOf("day") > -1) {

    }
    return allWeekDayStr;
}
/*********获取开设日期 End*******************/

/*********汇总费用 Begin*******************/

/// <summary>
/// 修改人：xmhuang
/// 修改日期：2013-11-22
///修改内容：修改个人登记费用统计显示
///原价：所有收费项目(不包含7已退项目)原始价格的总和
function SumJG() {
    var xmzj = jQuery("#tblList tbody tr").length; //项目合计个数
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
    var curWeekIndex = GetWeekIndex(CurWeek);
    var feeItemNameObj; //项目名称lable对象
    var curWeekStr = "";
    var rowNum = 0;
    jQuery("#tblList tbody tr td [name='yj']").each(function () {
        //将重置序号的方法一并在此处计算，以免多次循环table数据 Begin xmhuang 2014-04-11
        rowNum++;
        jQuery(this).parent().parent().find("[name='lblRowNum']").text(rowNum);
        //将重置序号的方法一并在此处计算，以免多次循环table数据 End xmhuang 2014-04-11
        curZK = jQuery.trim(jQuery(this).parent().parent().find("[name = 'zk']").text()); //获取当前折扣
        curSJ = parseFloat(jQuery.trim(jQuery(this).text())) * parseFloat(curZK) / 10; //计算折扣后价格
        feeTypeName = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sffs']").text()); //获取当前收费方式名称
        feeItemNameObj = jQuery(this).parent().parent().find("[name = 'xmmc']");
        isFeecharged = jQuery(this).parent().parent().attr("is_feecharged"); //获取是否付费标记
        isFeerefund = jQuery(this).parent().parent().attr("is_feerefund"); //获取是否退费标记

        //计算当前项目当天是否可以做 xmhuang 2014-03-25
        if (isFeecharged == 0 || isFeecharged == "False") {
            var OperationalDate = jQuery(this).parent().parent().attr("OperationalDate"); //获取是否开设标记
            var allWeekDayStr = GetOperationalDate(OperationalDate.toLowerCase());
            if (allWeekDayStr != "") {
                jQuery(this).parent().parent().attr("title", "该项目只在" + allWeekDayStr + "开设");
                jQuery(this).parent().parent().addClass("OperationalDate");
                jQuery(feeItemNameObj).attr("title", jQuery(feeItemNameObj).attr("oldtitle") + "[该项目只在" + allWeekDayStr + "开设]");
            }
        }
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
        jQuery(this).parent().parent().find("[name = 'sj']").text(parseFloat(curSJ).toFixed(curFixed)); //设置折扣后价格显示值
    });
    sumYJFY = parseFloat(sumYJFY).toFixed(curFixed); //汇总未付费
    sumUnPrintyjfy = parseFloat(sumUnPrintyjfy).toFixed(curFixed); //汇总未付费
    YJ = parseFloat(YJ).toFixed(curFixed); //汇总原价
    ZH = parseFloat(ZH).toFixed(curFixed); //汇总折后
    JZ = parseFloat(JZ).toFixed(curFixed); //汇总记账
    TF = parseFloat(TF).toFixed(curFixed); //汇总退费
    YSFY = parseFloat(YSFY).toFixed(curFixed); //汇总应收费用
    YSJE = parseFloat(YSJE).toFixed(curFixed); //汇总已收金额
    DS = parseFloat(DS).toFixed(curFixed); //汇总待收
    //var curHtml = '<td>' + xmzj + '个</td><td>￥' + YJ.toString() + '</td><td>￥' + ZH.toString() + '</td><td>￥' + JZ.toString() + '</td><td>￥' + TF.toString() + '</td><td class="red-x">￥' + YSFY.toString() + '</td><td class="red-x">￥' + YSJE.toString() + '</td><td class="red-x">￥' + DS.toString() + '</td>';
    var curHtml = '<span>项目总数：</span><span class="come_loose_center_red">' + xmzj + '个</span>';
    curHtml += '<span>原价：</span><span class="come_loose_center_red">￥' + YJ.toString() + '</span>';
    curHtml += '<span>折后：</span><span class="come_loose_center_red">￥' + ZH.toString() + '</span>';
    curHtml += '<span>记账：</span><span class="come_loose_center_red">￥' + JZ.toString() + '</span>';
    curHtml += '<span>退费：</span><span class="come_loose_center_red">￥' + TF.toString() + '</span>';
    curHtml += '<span>应收：</span><span class="come_loose_center_red">￥' + YSFY.toString() + '</span>';
    curHtml += '<span>已收：</span><span class="come_loose_center_red">￥' + YSJE.toString() + '</span>';
    curHtml += '<span>待收：</span><span class="come_loose_center_red">￥' + DS.toString() + '</span>';
    var waitChargeText = " 待收：￥" + DS.toString();
    jQuery("#lblWaitCharge").text(waitChargeText);
    var data = { YJ: YJ, ZH: ZH, JZ: JZ, TF: TF, YSFY: YSFY, YSJE: YSJE, DS: DS, yjfy: sumYJFY, sumUnPrintyjfy: sumUnPrintyjfy };
    jQuery("#divSumHeader").data("sumData", data);
    jQuery("#divSumHeader").html(curHtml);
    jQuery("#lblSumHeaderX").html("本次应交费用：" + sumYJFY + "元");

    /* 修改人：xmhuang 修改日期：2013-08-13 修改内容：设置客户收费项目显示内容 Begin*/
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
    /* 修改人：xmhuang 修改日期：2013-08-13 修改内容：设置客户收费项目显示内容 End*/

    // 判断表格是否存在滚动条,并设置相应的样式 xmhuang 20150311 在汇总时进行统一的表头设置
    JudgeTableIsExistScroll();
}

function SumJG_Old() {
    var xmzj = jQuery("#tblList tbody tr").length; //项目合计个数
    var sumYJ = 0; //项目原价总合计
    var sumZK = 0; //项目实收合计
    var sumSJ = 0; //项目实际合计
    var curZK = DisCountRate;
    var curSJ = 0;
    var sumXJJE = 0; //现金金额
    var sumJZJE = 0; //记账金额
    var sumYJFY = 0; //本次应交费用
    var sumUnPrintyjfy = 0; //本次未打印收费项目的应交费用
    var feeTypeName = '', feeChargeStaute = "";
    var ParentIsPrinted = "";
    jQuery("#tblList tbody tr td [name='yj']").each(function () {
        feeChargeStaute = jQuery(this).parent().parent().attr("feechargestaute");
        sumYJ += parseFloat(jQuery.trim(jQuery(this).text()));
        curZK = jQuery.trim(jQuery(this).parent().parent().find("[name = 'zk']").text());
        curSJ = parseFloat(jQuery.trim(jQuery(this).text())) * parseFloat(curZK) / 10;
        feeTypeName = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sffs']").text());
        jQuery(this).parent().parent().find("[name = 'sj']").text(parseFloat(curSJ).toFixed(curFixed));
        if (feeTypeName == "现金") {
            sumXJJE += curSJ;
            if (feeChargeStaute == "") {
                sumYJFY += curSJ;
            }
        }
        else if (feeTypeName == "记账") {
            sumJZJE += curSJ;
        }
        //sumSJ += curSJ;

        /*xmhuang 2013-10-15 记录未打印、未收费的现金项目的应交费用 Begin*/

        ParentIsPrinted = jQuery(this).parent().parent().attr("is_printed");
        if (ParentIsPrinted == undefined || ParentIsPrinted == 0 || ParentIsPrinted == 2)//如果是未打印
        {
            if (feeTypeName == "现金")//如果是现金项目
            {
                if (feeChargeStaute == "") //如果是未收费
                {
                    sumUnPrintyjfy += curSJ;
                }
            }
        }
        /*xmhuang 2013-10-15 记录未打印、未收费的现金项目的应交费用 End*/
    });
    sumXJJE = parseFloat(sumXJJE).toFixed(curFixed);
    sumYJ = parseFloat(sumYJ).toFixed(curFixed);
    sumSJ = parseFloat(sumSJ).toFixed(curFixed);
    sumYJFY = parseFloat(sumYJFY).toFixed(curFixed);
    sumSJ = parseFloat(sumYJ - sumXJJE).toFixed(curFixed);
    sumUnPrintyjfy = parseFloat(sumUnPrintyjfy).toFixed(curFixed);

    var curHtml = '<td>' + xmzj + '个</td><td>￥' + YJ.toString() + '</td><td>￥' + ZH.toString() + '</td><td>￥' + JZ.toString() + '</td><td>￥' + TF.toString() + '</td><td class="red-x">￥' + YSFY.toString() + '</td><td class="red-x">￥' + YSJE.toString() + '</td><td class="red-x">￥' + DS.toString() + '</td>';
    var data = { YJ: YJ, ZH: ZH, JZ: JZ, TF: TF, YSFY: YSFY, YSJE: YSJE, DS: DS, yjfy: sumYJFY, sumUnPrintyjfy: sumUnPrintyjfy };
    jQuery("#divSumHeader").data("sumData", data);
    jQuery("#divSumHeader").html(curHtml);

    /* 修改人：xmhuang 修改日期：2013-08-13 修改内容：设置客户收费项目显示内容 Begin*/
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
    /* 修改人：xmhuang 修改日期：2013-08-13 修改内容：设置客户收费项目显示内容 End*/
}
/*********汇总费用 End*******************/



//通过身份证检索用户基本信息 

function ReBindCustomerBusFee() {
    var data = { ID_Customer: jQuery.trim(jQuery("#txtTJH").val()), action: 'GetBusFeeByCustomerID' };
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: data,
        cache: false,
        dataType: "json",
        success: function (msg) {            
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            var FeeWay = "", FeeWayName = "", ID_Customer = "", newContent = "", exist = 0, CustCssStyle = "CustFeeChargeState", ID_Customer = "";
            var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("TemplateRegistCustFee");
            var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
            var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
            var RowNum = 0;
            var XFeeChargeStaute = "";
            jQuery(msg.dataList).each(function (i, item) {
                //如果没有行或者没有指定id的行则新增
                //today
                XFeeChargeStaute = ""
                ItemCheckbox = "ItemCheckbox";
                RowNum++;
                //在绑定用户收费项目时候，收费ID和收费名称都存放在数据源中，直接获取绑定值即可
                FeeWay = item.ID_FeeType;
                FeeWayName = item.FeeWayName;
                if (jQuery("#idSelectSet").data("ID_Customer") != undefined) {
                    exist = 1;
                    ID_Customer = jQuery("#idSelectSet").data("ID_Customer");
                }
                else {
                    CustCssStyle = "NewXM";
                    //                    FeeWay = document.getElementById("slFeeWay").value;
                    //                    FeeWayName = jQuery(allHiddFeeWay).find("option[value='" + document.getElementById("slFeeWay").value + "']").text();
                }
                //如果是团体项目则不允许删除，隐藏checkbox
                if (item.ID_TeamTaskGroup != "") {
                    exist = 1;
                    //ItemCheckbox = "UnItemCheckbox";//xmhuang 2013-11-09 团体和个人一样完成交费后不可操作
                    CustCssStyle = "Yellow";
                    //判断收费状态
                    if (item.FeeChargeStaute == "2") {
                        ItemCheckbox = "UnItemCheckbox";
                        CustCssStyle = "Yellow";
                        item.FeeChargeStaute = "√";
                        XFeeChargeStaute = "√";
                    }
                    //判断是否是已检项目，已检项目不允许退费Is_Examined
                    else if (item.FeeChargeStaute == "1") {
                        ItemCheckbox = "UnItemCheckbox";
                        item.FeeChargeStaute = "√";
                        CustCssStyle = "Green";
                    }
                    else if (item.FeeChargeStaute == "0") {
                        ItemCheckbox = "ItemCheckbox"; //xmhuang 2013-11-09 团体未收费显示复选框
                        CustCssStyle = "Red"; //xmhuang 2013-11-09 团体未收费显示复选框
                        //ItemCheckbox = "UnItemCheckbox";
                        item.FeeChargeStaute = "";
                        //CustCssStyle = "TeamRed";
                    }
                }
                else {
                    //判断收费状态
                    if (item.FeeChargeStaute == "2") {
                        ItemCheckbox = "UnItemCheckbox";
                        CustCssStyle = "Yellow";
                        item.FeeChargeStaute = "√";
                        XFeeChargeStaute = "√";
                    }
                    //判断是否是已检项目，已检项目不允许退费Is_Examined
                    else if (item.FeeChargeStaute == "1") {
                        item.FeeChargeStaute = "√";
                        ItemCheckbox = "UnItemCheckbox";
                        CustCssStyle = "Green";
                    }
                    else if (item.FeeChargeStaute == "0") {
                        ItemCheckbox = "ItemCheckbox";
                        item.FeeChargeStaute = "";
                        CustCssStyle = "Red";
                    }
                }
                //设置禁用样式
                oldtitle = "";
                if (item.Is_Banned == "True" || item.Is_Banned == 1) {
                    CustCssStyle = CustCssStyle + " Banned";
                    oldtitle = "该收费项目已禁用";
                }
                if (teamTaskGroupFeeListBodyTempleteContent != null) {
                    newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@type=text/gi, "")
                            .replace(/class=@CustCssStyle/gi, 'class="@CustCssStyle"')
                            .replace(/@type="text"/gi, "")
                            .replace(/@ItemCheckbox/gi, ItemCheckbox)
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                             .replace(/@exist/gi, 1)
                             .replace(/@Is_Org/gi, 0)
                            .replace(/@ID_Customer/gi, ID_Customer)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)
                            .replace(/@FeeChargeStaute/gi, item.FeeChargeStaute)
                            .replace(/@Price/gi, parseFloat(item.Price).toFixed(2))
                            .replace(/@FactPrice/gi, item.FactPrice)
                             .replace(/@Is_FeeRefund/gi, item.Is_FeeRefund)
                            .replace(/@Is_FeeCharged/gi, item.Is_FeeCharged)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                            .replace(/@date/gi, item.date)
                            .replace(/@FeeType/gi, FeeWay)
                            .replace(/@FeeWayName/gi, FeeWayName)
                            .replace(/@RowNum/gi, RowNum)
                            .replace(/@CustCssStyle/gi, CustCssStyle)
                            .replace(/@ID_CustFee/gi, item.ID_CustFee)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@Is_Printed/gi, item.Is_Printed)

                            .replace(/@Discount/gi, item.Discount)
                            .replace(/@ID_Discounter/gi, item.ID_Discounter)
                            .replace(/@XDiscounterName/gi, item.DiscounterName)
                            .replace(/@ID_Register/gi, item.ID_Register)
                            .replace(/@RegisterName/gi, item.RegisterName)
                            .replace(/@RegistDate/gi, item.RegistDate)
                            .replace(/@XFeeChargeStaute/gi, XFeeChargeStaute)
                            .replace(/@OperationalDate/gi, item.OperationalDate)
                            .replace(/@Is_Banned/gi, item.Is_Banned)    //xmhuang 20140801 新增是否禁用绑定值
                            .replace(/@tilte/gi, '"' + oldtitle + '"'); //如果是默认属性，其key value没有用双引号括起来，所以这里需要特殊处理下 xmhuang 20140924
                }
            });
            if (newContent != '') {
                jQuery("#tblList thead").html(teamTaskGroupFeeListTheadTempleteContent);
                //如果套餐为非当前套餐，且未付款和未体检项目则一并移除
                //如果是新增则默认移除所有套餐显示当前套餐
                if (type.toLowerCase() == "add") {
                    jQuery("#tblList tbody").html(newContent);

                }
                //如果是修改，则只能移除尚未保存的套餐
                else {
                    jQuery("#tblList tbody").html(newContent);
                }
                //SetTableEvenOddRowStyle();
                newContent = "";
                DoScrollToBottom();
                BindFeeWay();
                BindKeyup();
                SetTableEvenOddRowStyle();                          //设置奇偶项样式 xmhuang 2014-04-11
            }

            SumJG(); //计算合计

        }
    });
}
function DoDeleteCustFeeRow() {
    //xmhuang 2013-09-10 End
    var timer = 1000;
    var removeObj;
    var length = jQuery("[name='ItemCheckbox']:checked").length;
    if (length > 10) //当次连续删除10条数据时,采用删除效果
    {
        jQuery("[name='ItemCheckbox']:checked").each(function () {
            if (jQuery(this).attr('checked')) {
                removeObj = jQuery(this).parent().parent();
                jQuery(removeObj).fadeOut(timer, function () {
                    jQuery(this).remove();
                });
                timer += 30;
            }
        });
        setTimeout("DeleteComplete()", timer); //数据删除完后重新汇总收费信息 xmhuang 2014-04-11
    }
    else {
        jQuery("[name='ItemCheckbox']:checked").each(function () {
            if (jQuery(this).prop('checked')) {
                jQuery(this).parent().parent().remove();
            }
        });
        SumJG();
    }
}
/*********删除选中项目 Begin*********/
function DoDel() {
    var rowCount = jQuery("[name='ItemCheckbox']:checked").length;
    if (rowCount == 0) {
        return false;
    }
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '温馨提示',
        content: "您确认要删除吗？",
        button: [{
            name: '取消',
            callback: function () {
                return true;
            }
        }, {
            name: '确定',
            callback: function () {
                //判断是否有选中项目
                var CustFeeID = '';
                jQuery("[name='ItemCheckbox']:checked").each(function () {
                    if (jQuery(this).prop('checked')) {
                        if (jQuery(this).parent().parent().attr('id_custfee') != "") {
                            CustFeeID += "'" + jQuery(this).parent().parent().attr('id_custfee') + "',";
                        }
                    }
                });

                var ID_Customer = jQuery("#txtTJH").val();
                //xmhuang 2013-09-10 Begin
                var Forsex = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].value; //性别
                Forsex = (Forsex == 1 ? Forsex : 0); //0：女性，1男性，2：共用
                if (CustFeeID != '') {
                    jQuery.ajax({
                        type: "POST",
                        url: "/Ajax/AjaxRegiste.aspx",
                        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
                        data: { Forsex: Forsex, CustFeeID: CustFeeID, ID_Customer: ID_Customer, action: 'DeleteCustFee' },
                        cache: false,
                        dataType: "json",
                        success: function (msg) {
                            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
                            msg = CheckAjaxReturnDataInfo(msg);
                            //msg == null || msg == ""时表示收费项目未保存进行删除 msg.success==1表示已保存的收费项目删除
                            if (msg == null || msg == "" || msg.success == 1) {
                                DoDeleteCustFeeRow();
                                return;
                            }
                            //计算合计
                            SumJG();
                            if (msg != null) {
                                ShowSystemDialog(msg.Message);
                            }
                        }
                    });
                }
                else {
                    DoDeleteCustFeeRow();
                }
                return true;

            }, focus: true
        }]

    }).lock();

}
function DeleteComplete() {
    SumJG();
}

/*********删除选中项目 End*********/

/*
//验证数据项目
//通过元素已经设置的required属性来进行数据项为空验证，元素必须设置required类、emptyMessage、errorMessage信息
//如果需要验证Email、Phone需要设置元素vailType为Email、Phone Begin*/
function vailData(objElement, IsSave) {
    var curObj = '';
    for (i = 0; i < document.forms.length; i++) {
        var elements = document.forms[i].elements;
        for (j = 0; j < elements.length; j++) {
            //undefined elements[j].attributes["requi"]
            curObj = elements[j].value;
            if (elements[j].attributes["required"] != null) {
                //判断是否为空
                if (elements[j].type == "text" || elements[j].type == "textarea") {

                    if (elements[j].value == "") {
                        if (elements[j].attributes["emptyMessage"] != null) {
                            if (elements[j].attributes["emptyMessage"].value != "") {
                                ShowSystemDialog(elements[j].attributes["emptyMessage"].value);
                            }
                            if (jQuery(elements[j]).is(":visible") == true) {
                                elements[j].focus();
                                elements[j].select();
                            }
                            return false;
                        }
                    }
                    else {
                        if (elements[j].attributes["vailType"] != null) {
                            var flag = true;
                            var vailType = elements[j].attributes["vailType"].value;
                            if (vailType == "Email") {
                                flag = isEmail(elements[j].value);
                            }
                            else if (vailType == "Mobil") {
                                flag = isMobil(elements[j].value);
                            }
                            else if (vailType == "IDCard") {
                                flag = isIDCardNo(elements[j].value);
                            }
                            if (!flag) {
                                if (elements[j].attributes["errorMessage"] != null) {
                                    ShowSystemDialog(elements[j].attributes["errorMessage"].value);
                                }
                                else {
                                    ShowSystemDialog("请输入正确的格式！");
                                }
                                if (jQuery(elements[j]).is(":visible") == true) {
                                    elements[j].focus();
                                    elements[j].select();
                                }
                                return false;
                            }
                        }
                    }
                }
                else if (elements[j].type == "select-one") {
                    if (jQuery(elements[j]).val() == "" || jQuery(elements[j]).val() == "-1") {
                        if (elements[j].attributes["errorMessage"] != null) {
                            ShowSystemDialog(elements[j].attributes["errorMessage"].value);
                        }
                        if (jQuery(elements[j]).is(":visible") == true) {
                            elements[j].focus();
                            //elements[j].select();
                        }
                        return false;
                    }

                }
            }
            else {
                if (elements[j].value != "") {
                    if (elements[j].attributes["vailType"] != null) {
                        flag = true;
                        vailType = elements[j].attributes["vailType"].value;
                        if (vailType == "Email") {
                            flag = isEmail(elements[j].value);
                        }
                        else if (vailType == "Mobil") {
                            flag = isMobil(elements[j].value);
                        } else if (vailType == "IDCard") {
                            flag = isIDCardNo(elements[j].value);
                        }
                        if (!flag) {
                            if (elements[j].attributes["errorMessage"] != null) {
                                ShowSystemDialog(elements[j].attributes["errorMessage"].value);
                            }
                            else {
                                ShowSystemDialog("请输入正确的格式！");
                            }
                            if (jQuery(elements[j]).is(":visible") == true) {
                                elements[j].focus();
                                elements[j].select();
                            }
                            return false;
                        }
                    }
                }
            }
        }
    }
    //如果体检号不存在在验证预约时间不得小于或等于当前时间
    var TempID_Customer = document.getElementById("txtTJH").value; //体检号
    if (TempID_Customer == "") {
        var TempDate = jQuery("#txtSubScribDate").val();
        if (TempDate == "") {
            ShowSystemDialog("体检时间不允许为空！");
            return false;
        }
        if (IsCommon == 1)//如果是共用功能模块则不验证体检时间
        { }
        else {
            if (jQuery('#modelName').val().toLowerCase() == "regist") {
                if (TempDate <= CurDate) {
                    ShowSystemDialog("预约体检时间不得小于或等于当天！");
                    //jQuery("#txtSubScribDate").select();
                    return false;
                }
            }
            else if (jQuery('#modelName').val().toLowerCase() == "sign") {
                if (TempDate != CurDate) {
                    ShowSystemDialog("登记体检时间必须选择当天！");
                    //jQuery("#txtSubScribDate").select();
                    return false;
                }
            }
        }
    }

    //1.	现场登记时不可检索已禁用收费项目；
    //2.	存在已收费的禁用收费项目，可以完成登记；
    //3.	团体成员一旦备单，即使收费项目禁用，无论是否收费，也可以正常完成体检；
    //4.	个人用户只要存在未交费的禁用收费项目，则无法完成登记。
    var ID_Customer = jQuery.trim(document.getElementById("txtTJH").value); //体检号
    //获取预约/登记标记
    var TypeCode = "";
    if (ID_Customer != "") {
        TypeCode = ID_Customer.substring(1, 2); //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
    }
    if (IsSave == 1) {
        //判断是否有未收费且禁用的收费项目，有则不允许完成登记
        if (TypeCode != 9)                                          //如果是个人,只要存在未收费的禁用项目则不能完成登记
        {
            if (jQuery(".Banned[is_feecharged!='True']").length > 0)//查找未收费、禁用的收费项目
            {
                ShowSystemDialog("存在禁用的收费项目不允许保存！");
                return false;
            }
        }
        else if (TypeCode == 9)                                     //如果是团体客户，但是存在新增的未收费的禁用项目，则无法完成登记
        {
            if (jQuery(".Banned[is_feecharged!='True'][id_custfee='']").length > 0)//查找未收费、新增、禁用的收费项目
            {
                ShowSystemDialog("存在禁用的收费项目不允许保存！");
                return false;
            }
        }
        if (jQuery(".CustomerSex[is_feecharged!='True'][id_custfee='']").length > 0)//查找未保存的性别不符的收费项目 xmhuang20150323
        {
            ShowSystemDialog("存在与客户性别不符的收费项目不允许保存！");
            return false;
        }
        //仅保存
        JustSaveData(objElement);
    }
    else {
        //判断是否有未收费且禁用的收费项目，有则不允许完成登记
        if (TypeCode != 9)                                          //如果是个人,只要存在未收费的禁用项目则不能完成登记
        {
            if (jQuery(".Banned[is_feecharged!='True']").length > 0)//查找未收费、禁用的收费项目
            {
                ShowSystemDialog("存在禁用的收费项目不允许保存！");
                return false;
            }
        }
        else if (TypeCode == 9)                                     //如果是团体客户，但是存在新增的未收费的禁用项目，则无法完成登记
        {
            if (jQuery(".Banned[is_feecharged!='True'][id_custfee='']").length > 0)//查找未收费、新增、禁用的收费项目
            {
                ShowSystemDialog("存在禁用的收费项目不允许保存！");
                return false;
            }
        }
        if (jQuery(".CustomerSex[is_feecharged!='True'][id_custfee='']").length > 0)//查找未保存的性别不符的收费项目 xmhuang20150323
        {
            ShowSystemDialog("存在与客户性别不符的收费项目不允许保存！");
            return false;
        }
        SaveData(objElement);
    }
}

function CheckCustomerInfo() {
    var IDCard = jQuery("#txtSFZ").val(); //身份证
    var ExamCard = jQuery("#txtYKT").val(); //一卡通
    var modelName = jQuery("#modelName").val();
    var IsExist = 0;
    //存储大数据请设置Content-length值
    jQuery.ajax({
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: { action: 'GetCustomerInfo',
            modelName: modelName,
            IDCard: IDCard,
            ExamCard: ExamCard
        },
        cache: false,
        dataType: "json",
        success: function (msg) {
            IsExist = msg.Exist;
        }
    });
    return IsExist;
}
/*
//验证数据项目
//通过元素已经设置的required属性来进行数据项为空验证，元素必须设置required类、emptyMessage、errorMessage信息
//如果需要验证Email、Phone需要设置元素vailType为Email、Phone End*/

/*保存：只进行数据保存，不进行其它业务处理 Begin*/
function JustSaveData(objElement) {
    //DisCountRate = OldDisCountRate; //完成登记后重置客户折扣
    jQuery("#txtXMZK").text(20);
    var obj = jQuery("#tblList tr[name='busList']");
    if (obj.length == 0) {
        ShowSystemDialog("请您添加体检项目！");
        return false;
    }
    SetControlDisabled(); //禁用完成按钮，防止多次写入
    var ID_ArcCustomer = jQuery("#ID_ArcCustomer") == undefined ? "" : jQuery("#ID_ArcCustomer").val();
    ID_ArcCustomer = jQuery.trim(ID_ArcCustomer);
    var IDCard = jQuery("#lblSFZ").text(); //身份证号码
    IDCard = jQuery.trim(IDCard);
    var CustomerName = jQuery("#spanCustomerName").val(); //客户名称
    var ID_Customer = jQuery.trim(document.getElementById("txtTJH").value); //体检号
    //var ExamCard = jQuery.trim(document.getElementById("txtYKT").value); //一卡通
    var Gender = jQuery("#slSex").find("option:selected").val(); //性别
    Gender = jQuery.trim(Gender);
    var GenderName = jQuery("#slSex").find("option:selected").text(); //性别
    GenderName = jQuery.trim(GenderName);
    var BirthDay = document.getElementById("txtBirthDay").value; //出生年月
    BirthDay = jQuery.trim(BirthDay);
    var Married = jQuery("#slMarried").find("option:selected").val(); //婚姻状况
    Married = jQuery.trim(Married);
    var MarriageName = jQuery("#slMarried").find("option:selected").text(); //婚姻状况
    MarriageName = jQuery.trim(MarriageName);
    var MobileNo = document.getElementById("txtMobil").value; //手机号码
    MobileNo = jQuery.trim(MobileNo);
    var GuideNurse = jQuery("#idSelectUser").val();             //导检护士ID
    GuideNurse = jQuery.trim(GuideNurse);
    var GuideNurseName = jQuery("#nameSelectUser").val();       //导检护士姓名
    GuideNurseName = jQuery.trim(GuideNurseName);
    var Nation = jQuery("#idSelectNation").val();               //民族ID
    Nation = jQuery.trim(Nation);
    var NationName = jQuery("#nameSelectNation").val();         //民族
    NationName = jQuery.trim(NationName);
    var ExamType = jQuery("#idSelectExamType").val();        //体检类型ID
    ExamType = jQuery.trim(ExamType);
    var ExamTypeName = jQuery("#nameSelectExamType").val();  //体检类型名称
    ExamTypeName = jQuery.trim(ExamTypeName);
    var BusSet = jQuery("#idSelectSet").val();                  //套餐类型ID
    BusSet = jQuery.trim(BusSet);
    var BusPEPackageName = jQuery('#idSelectSet').attr("title");      //套餐类型名称 xmhuang 由于value已经截断，只能从title中取值 xmhuang 2014-04-06
    BusPEPackageName = jQuery.trim(BusPEPackageName);
    var ReportWay = $("#slReportWay").find("option:selected").val(); //报告领取方式
    ReportWay = jQuery.trim(ReportWay);
    var ReportWayName = $("#slReportWay").find("option:selected").text(); //报告领取方式
    ReportWayName = jQuery.trim(ReportWayName);
    var FeeWay = $("#slFeeWay").find("option:selected").val(); //付费方式
    FeeWay = jQuery.trim(FeeWay);
    var FeeWayName = $("#slFeeWay").find("option:selected").text(); //付费方式
    FeeWayName = jQuery.trim(FeeWayName);
   // var OperateLevel = document.getElementById("slOperateLevel").options[document.getElementById("slOperateLevel").selectedIndex].value; //操作密级
   // OperateLevel = jQuery.trim(OperateLevel);
   // var OperateLevelName = document.getElementById("slOperateLevel").options[document.getElementById("slOperateLevel").selectedIndex].text; //操作密级
    //OperateLevelName = jQuery.trim(OperateLevelName);
   // var Email = document.getElementById("txtEmail").value; //Email
   // Email = jQuery.trim(Email);
    var ExamPlace = document.getElementById("slExamPlace").selectedIndex == -1 ? -1 : document.getElementById("slExamPlace").options[document.getElementById("slExamPlace").selectedIndex].value; //体检地址
    ExamPlace = jQuery.trim(ExamPlace);
    //    if (ExamPlace == -1) {
    //        ShowSystemDialog("请您选择体检地址！"); ReSetControlDisabled();
    //        return false;
    //    }
    var ExamPlaceName = document.getElementById("slExamPlace").selectedIndex == -1 ? -1 : document.getElementById("slExamPlace").options[document.getElementById("slExamPlace").selectedIndex].text; //体检地址
    ExamPlaceName = jQuery.trim(ExamPlaceName);
    var Note = document.getElementById("txtNote").value; //备注
    Note = jQuery.trim(Note);
    var Country = document.getElementById("slCountry").selectedIndex >= 0 ? document.getElementById("slCountry").options[document.getElementById("slCountry").selectedIndex].value : null;   //国家ID
    Country = jQuery.trim(Country);
    var CountryName = document.getElementById("slCountry").selectedIndex >= 0 ? document.getElementById("slCountry").options[document.getElementById("slCountry").selectedIndex].text : null; //国家名称
    CountryName = jQuery.trim(CountryName);
    var Cultrul = document.getElementById("slCultrul").selectedIndex == -1 ? -1 : document.getElementById("slCultrul").options[document.getElementById("slCultrul").selectedIndex].value; //教育背景
    Cultrul = jQuery.trim(Cultrul);
    var CultrulName = document.getElementById("slCultrul").selectedIndex == -1 ? -1 : document.getElementById("slCultrul").options[document.getElementById("slCultrul").selectedIndex].text; //教育背景
    CultrulName = jQuery.trim(CultrulName);
    var SubScribDate = document.getElementById("txtSubScribDate") == null ? "" : document.getElementById("txtSubScribDate").value; //体检时间
    SubScribDate = jQuery.trim(SubScribDate);
    var Base64Photo = jQuery("#HeadImg").attr("Base64Photo");
    var modelName = "";

    //如果选取体检时间大于当天或参数为预约则为预约，如果体检时间为当天或参数为登记则为登记。如果是团体则为登记 Begin;
    if (IsCommon == 1)//如果共用功能模块则已体检号是否存在为标准，如果不存在则已体检时间为准判断是预约还是登记，如果体检时间大于当前时间认为是预约、等于当天则认为是登记
    {
        //如果体检号为空
        if (ID_Customer == "") {
            //如果体检时间大于当前时间则认为是预约
            if (SubScribDate > CurDate) {
                modelName = "Regist"; //预约
            }
            else if (SubScribDate == CurDate) {
                modelName = "Sign"; //登记
            }
        }
        else {
            //获取预约/登记标记
            var ErrorMsg = "";
            var TypeCode = ID_Customer.substring(1, 2); //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
            if (TypeCode == 3)//个人预约
            {
                modelName = "Sign"; //预约
            }
            else if (TypeCode == 6)//个人登记
            {
                modelName = "Sign"; //登记
            }
            else if (TypeCode == 9)//团体登记
            {
                modelName = "Sign"; //登记
            }
            else if (TypeCode == 5)//网上预约登记
            {
                modelName = "Sign"; //登记
            }
        }
    }
    else {
        var configModelName = jQuery('#modelName').val().toLowerCase(); //菜单中配置的模块名称
        if (configModelName == "regist") {
            modelName = "Regist"; //预约
        }
        else if (configModelName == "sign") {
            modelName = "Sign"; //登记
        }
        else if (configModelName == "" || configModelName == "signandregiste") {
            //如果体检号为空
            if (ID_Customer == "") {
                //如果体检时间大于当前时间则认为是预约
                if (SubScribDate > CurDate) {
                    modelName = "Regist"; //预约
                }
                else if (SubScribDate == CurDate) {
                    modelName = "Sign"; //登记
                }
            }
            else {
                //获取预约/登记标记
                var ErrorMsg = "";
                var TypeCode = ID_Customer.substring(1, 2); //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
                if (TypeCode == 3)//个人预约
                {
                    modelName = "Sign"; //预约
                }
                else if (TypeCode == 6)//个人登记
                {
                    modelName = "Sign"; //登记
                }
                else if (TypeCode == 9)//团体登记
                {
                    modelName = "Sign"; //登记
                } else if (TypeCode == 5)//网上预约登记
                {
                    modelName = "Sign"; //登记
                }
            }
        }
    }

    //如果选取体检时间大于当天或参数为预约则为预约，如果体检时间为当天或参数为登记则为登记。如果是团体则为登记 End

    var BusFeeItems = ''; //记录所有的套餐
    var AllIDFee = ""; //所有收费项目ID
    var IsUpdate = 0, oldid_discounter, oldfeetype, oldis_feecharged, oldis_feerefund, id_custfee, id_customer, id, id_discounter, discountername, exist, ID_Section, SectionName, xmmc, yj, zk, sj, sffs, sffsName, itemID, curItem;
    jQuery("#tblList tbody tr td [name='yj']").each(function () {
        if (jQuery(this).parent().parent().find("td input[name='ItemCheckbox']").length > 0) {
            id_custfee = jQuery.trim(jQuery(this).parent().parent().attr("id_custfee")); //收费项目ID 新增用于规避出现双项目
            id_customer = jQuery.trim(jQuery(this).parent().parent().attr("id_customer")); //收费项目所对应的体检号
            id = jQuery.trim(jQuery(this).parent().parent().attr("id_fee")); //收费项目ID
            id_discounter = jQuery.trim(jQuery(this).parent().parent().attr("id_discounter")); //折扣人ID
            discountername = jQuery.trim(jQuery(this).parent().parent().attr("discountername")); //折扣人名称
            exist = jQuery.trim(jQuery(this).parent().parent().attr('exist')); //是否存在
            ID_Section = jQuery.trim(jQuery(this).parent().parent().attr('id_section')); //科室ID
            SectionName = jQuery.trim(jQuery(this).parent().parent().attr('sectionname')); //科室名称
            xmmc = jQuery.trim(jQuery(this).parent().parent().find("[name = 'xmmc']").text()); //收费项目名称
            yj = jQuery.trim(jQuery(this).parent().parent().find("[name = 'yj']").text()); //原价
            zk = jQuery.trim(jQuery(this).parent().parent().find("[name = 'zk']").text()); //折扣
            sj = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sj']").text()); //实际价格
            sffs = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sffs']").attr("feetype")); //收费方式ID
            sffsName = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sffs']").text()); //收费方式名称
            itemID = id;
            AllIDFee += itemID + ",";
            curItem = itemID + "_" + xmmc + "_" + yj + "_" + zk + "_" + sj + "_" + sffs + "_" + sffsName + "_" + itemID + "_" + ID_Section + "_" + SectionName + "_" + id_discounter + "_" + discountername + "_" + id_customer + "_" + id_custfee + "_" + exist;
            BusFeeItems += curItem + "|";
        }
    });
    //新增IsLoadCustomer参数用于更新客户头像信息
    var qustData = { action: 'JustSaveData',
        modelName: modelName,
        type: type,
        ID_ArcCustomer: ID_ArcCustomer,
        CardNum: IDCard,
        CustomerName: encodeURIComponent(CustomerName),
        ID_Customer: ID_Customer,
        //ExamCard: ExamCard,
        Gender: Gender,
        GenderName: encodeURIComponent(GenderName),
        BirthDay: encodeURIComponent(BirthDay),
        Married: Married,
        MarriageName: encodeURIComponent(MarriageName),
        MobileNo: MobileNo,
        RegisteDate: encodeURIComponent(RegisteDate),
        Register: Register,
        BusSet: BusSet,
        BusPEPackageName: encodeURIComponent(BusPEPackageName),
        ReportWay: ReportWay,
        ReportWayName: encodeURIComponent(ReportWayName),
        ExamType: ExamType,
        ExamTypeName: encodeURIComponent(ExamTypeName),
        FeeWay: FeeWay,
        FeeWayName: encodeURIComponent(FeeWayName),
        GuideNurse: GuideNurse,
        GuideNurseName: encodeURIComponent(GuideNurseName),
        //OperateLevel: OperateLevel,
        ExamPlace: ExamPlace,
        ExamPlaceName: encodeURIComponent(ExamPlaceName),
        Country: Country,
        CountryName: encodeURIComponent(CountryName),
        //Email: encodeURIComponent(Email),
        Note: encodeURIComponent(Note),
        Nation: Nation,
        NationName: encodeURIComponent(NationName),
        Cultrul: Cultrul,
        CultrulName: encodeURIComponent(CultrulName),
        SubScribDate: encodeURIComponent(SubScribDate),
        BusFeeItems: encodeURIComponent(BusFeeItems),
        Base64Photo: jQuery("#HeadImg").attr("Base64Photo")
    };
    //存储大数据请设置Content-length值
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxRegiste.aspx",
        data: qustData,
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (jsonMsg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            jsonMsg = CheckAjaxReturnDataInfo(jsonMsg);
            if (jsonMsg == null || jsonMsg == "") {
                return;
            }
            jQuery(jsonMsg.dataList).each(function (i, msg) {
                if (msg.success == "1") {
                    jQuery("#tblList tbody tr").attr("exist", "1");   //设置套餐为存在项exist=1
                    //这里判断是否有未完成的非团体的现金项目，如果有需要打印缴费通知单
                    var Is_Subscribed = msg.Is_Subscribed;
                    var Is_GuideSheetPrinted = msg.Is_GuideSheetPrinted;
                    var CustomerSubScribDate = item.SubScribDate;
                    var data = { SubScribDate: CustomerSubScribDate, Is_Subscribed: Is_Subscribed, Is_GuideSheetPrinted: Is_GuideSheetPrinted };
                    jQuery("#btnPrintCust1").data("data", data);
                    jQuery("#lblHidCustomer").text(msg.ID_Customer); jQuery("#txtTJH").val(msg.ID_Customer);  //由于新增时，体检号是从后台生成的，这里需要设置体检号
                    SetHidOrShowItem(); //xmhuang 20150126
                    jQuery("#lblCode128c").text(msg.Code128c);
                    var Base64Photo = jQuery("#HeadImg").attr("Base64Photo");
                    AddCustomerQueue(msg.ID_Customer, CustomerName, GenderName, BirthDay, IDCard, Base64Photo);
                    ShowSystemDialog(msg.Message);  //设置成功完成后的提示信息

                    //重新绑定收费项目 xmhuang 20140513
                    ReBindCustomerBusFee();
                }
                else {
                    ShowSystemDialog(msg.Message);
                }
            });
        },
        complete: function () {
            ReSetControlDisabled();
        }
    });

}

/*保存数据项 Begin*/
function SaveData(objElement) {
    //DisCountRate = OldDisCountRate; //xmhuang 20140504 完成登记后重置客户折扣
    jQuery("#txtXMZK").val(10);
    var obj = jQuery("#tblList tr[name='busList']");
    if (obj.length == 0) {
        ShowSystemDialog("请您添加体检项目！");
        return false;
    }
    SetControlDisabled(); //xmhuang 2013-11-14 禁用完成按钮，防止多次写入
    var ID_ArcCustomer = jQuery("#ID_ArcCustomer") == undefined ? "" : jQuery("#ID_ArcCustomer").val();
    ID_ArcCustomer = jQuery.trim(ID_ArcCustomer);
    var IDCard = document.getElementById("lblSFZ").value; //身份证号码
    IDCard = jQuery.trim(IDCard);
    var CustomerName = jQuery.trim(jQuery("#spanCustomerName").text()); //客户名称
    var ID_Customer = jQuery.trim(document.getElementById("txtTJH").value); //体检号
   //var ExamCard = jQuery.trim(document.getElementById("txtYKT").value); //一卡通
   
    var Gender = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].value; //性别
    Gender = jQuery.trim(Gender);
    var GenderName = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].text; //性别
    GenderName = jQuery.trim(GenderName);
    var BirthDay = document.getElementById("txtBirthDay").value; //出生年月
    BirthDay = jQuery.trim(BirthDay);
    var Married = document.getElementById("slMarried").options[document.getElementById("slMarried").selectedIndex].value; //婚姻状况
    Married = jQuery.trim(Married);
    var MarriageName = document.getElementById("slMarried").options[document.getElementById("slMarried").selectedIndex].text; //婚姻状况
    MarriageName = jQuery.trim(MarriageName);
    var MobileNo = document.getElementById("txtMobil").value; //手机号码
    MobileNo = jQuery.trim(MobileNo);
    var GuideNurse = jQuery("#idSelectUser").val();             //导检护士ID
    GuideNurse = jQuery.trim(GuideNurse);
    var GuideNurseName = jQuery("#nameSelectUser").val();       //导检护士姓名
    GuideNurseName = jQuery.trim(GuideNurseName);
    var Nation = jQuery("#idSelectNation").val();               //民族ID
    Nation = jQuery.trim(Nation);
    Nation = Nation > 0 ? Nation : -1;                          //如果民族ID小于1，证明不存在民族 xmhuang 20140527

    var NationName = jQuery("#nameSelectNation").val();         //民族
    NationName = jQuery.trim(NationName);
    if (Nation == -1)                                           //如果民族ID小于1，证明不存在民族名称 xmhuang 20140527
    {
        NationName = "";
    }

    var ExamType = jQuery("#idSelectExamType").val();        //体检类型ID
    ExamType = jQuery.trim(ExamType);
    var ExamTypeName = jQuery("#nameSelectExamType").val();  //体检类型名称
    ExamTypeName = jQuery.trim(ExamTypeName);
    var BusSet = jQuery("#idSelectSet").val();                  //套餐类型ID
    BusSet = jQuery.trim(BusSet);
    var BusPEPackageName = jQuery('#idSelectSet').attr("title");      //套餐类型名称 xmhuang 由于value已经截断，只能从title中取值 xmhuang 2014-04-06
    BusPEPackageName = jQuery.trim(BusPEPackageName);
    var ReportWay = document.getElementById("slReportWay").options[document.getElementById("slReportWay").selectedIndex].value; //报告领取方式
    ReportWay = jQuery.trim(ReportWay);
    var ReportWayName = document.getElementById("slReportWay").options[document.getElementById("slReportWay").selectedIndex].text; //报告领取方式
    ReportWayName = jQuery.trim(ReportWayName);
    var FeeWay = document.getElementById("slFeeWay").options[document.getElementById("slFeeWay").selectedIndex].value; //付费方式
    FeeWay = jQuery.trim(FeeWay);
    var FeeWayName = document.getElementById("slFeeWay").options[document.getElementById("slFeeWay").selectedIndex].text; //付费方式
    FeeWayName = jQuery.trim(FeeWayName);
    var OperateLevel = document.getElementById("slOperateLevel").options[document.getElementById("slOperateLevel").selectedIndex].value; //操作密级
    OperateLevel = jQuery.trim(OperateLevel);
    var OperateLevelName = document.getElementById("slOperateLevel").options[document.getElementById("slOperateLevel").selectedIndex].text; //操作密级
    OperateLevelName = jQuery.trim(OperateLevelName);
    var Email = document.getElementById("txtEmail").value; //Email
    Email = jQuery.trim(Email);
    var ExamPlace = document.getElementById("slExamPlace").selectedIndex == -1 ? -1 : document.getElementById("slExamPlace").options[document.getElementById("slExamPlace").selectedIndex].value; //体检地址
    ExamPlace = jQuery.trim(ExamPlace);
    //    if (ExamPlace == -1) {
    //        ShowSystemDialog("请您选择体检地址！"); ReSetControlDisabled();
    //        return false;
    //    }
    var ExamPlaceName = document.getElementById("slExamPlace").selectedIndex == -1 ? -1 : document.getElementById("slExamPlace").options[document.getElementById("slExamPlace").selectedIndex].text; //体检地址
    ExamPlaceName = jQuery.trim(ExamPlaceName);
    var Note = document.getElementById("txtNote").value; //备注
    Note = jQuery.trim(Note);
    var Country = document.getElementById("slCountry").selectedIndex == -1 ? -1 : document.getElementById("slCountry").options[document.getElementById("slCountry").selectedIndex].value;   //国家ID
    Country = jQuery.trim(Country);
    var CountryName = document.getElementById("slCountry").selectedIndex == -1 ? -1 : document.getElementById("slCountry").options[document.getElementById("slCountry").selectedIndex].text; //国家名称
    CountryName = jQuery.trim(CountryName);
    var Cultrul = document.getElementById("slCultrul").selectedIndex == -1 ? -1 : document.getElementById("slCultrul").options[document.getElementById("slCultrul").selectedIndex].value; //教育背景
    Cultrul = jQuery.trim(Cultrul);
    var CultrulName = document.getElementById("slCultrul").selectedIndex == -1 ? -1 : document.getElementById("slCultrul").options[document.getElementById("slCultrul").selectedIndex].text; //教育背景
    CultrulName = jQuery.trim(CultrulName);
    var SubScribDate = document.getElementById("txtSubScribDate") == null ? "" : document.getElementById("txtSubScribDate").value; //体检时间
    SubScribDate = jQuery.trim(SubScribDate);
    var Base64Photo = jQuery("#HeadImg").attr("Base64Photo");
    var modelName = "";

    //xmhuang 2014-03-18 如果选取体检时间大于当天或参数为预约则为预约，如果体检时间为当天或参数为登记则为登记。如果是团体则为登记 Begin;
    if (IsCommon == 1)//如果共用功能模块则已体检号是否存在为标准，如果不存在则已体检时间为准判断是预约还是登记，如果体检时间大于当前时间认为是预约、等于当天则认为是登记
    {
        //如果体检号为空
        if (ID_Customer == "") {
            //如果体检时间大于当前时间则认为是预约
            if (SubScribDate > CurDate) {
                modelName = "Regist"; //预约
            }
            else if (SubScribDate == CurDate) {
                modelName = "Sign"; //登记
            }
        }
        else {
            //获取预约/登记标记
            var ErrorMsg = "";
            var TypeCode = ID_Customer.substring(1, 2); //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
            if (TypeCode == 3)//个人预约
            {
                modelName = "Sign"; //预约
            }
            else if (TypeCode == 6)//个人登记
            {
                modelName = "Sign"; //登记
            }
            else if (TypeCode == 9)//团体登记
            {
                modelName = "Sign"; //登记
            } else if (TypeCode == 5)//网上预约登记
            {
                modelName = "Sign"; //登记
            }
        }
    }
    else {
        var configModelName = jQuery('#modelName').val().toLowerCase(); //菜单中配置的模块名称
        if (configModelName == "regist") {
            modelName = "Regist"; //预约
        }
        else if (configModelName == "sign") {
            modelName = "Sign"; //登记
        }
        else if (configModelName == "" || configModelName == "signandregiste") {
            //如果体检号为空
            if (ID_Customer == "") {
                //如果体检时间大于当前时间则认为是预约
                if (SubScribDate > CurDate) {
                    modelName = "Regist"; //预约
                }
                else if (SubScribDate == CurDate) {
                    modelName = "Sign"; //登记
                }
            }
            else {
                //获取预约/登记标记
                var ErrorMsg = "";
                var TypeCode = ID_Customer.substring(1, 2); //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
                if (TypeCode == 3)//个人预约
                {
                    modelName = "Sign"; //预约
                }
                else if (TypeCode == 6)//个人登记
                {
                    modelName = "Sign"; //登记
                }
                else if (TypeCode == 9)//团体登记
                {
                    modelName = "Sign"; //登记
                } else if (TypeCode == 5)//网上预约登记
                {
                    modelName = "Sign"; //登记
                }
            }
        }
    }

    //xmhuang 2014-03-18 如果选取体检时间大于当天或参数为预约则为预约，如果体检时间为当天或参数为登记则为登记。如果是团体则为登记 End

    var BusFeeItems = ''; //记录所有的套餐
    var AllIDFee = ""; //所有收费项目ID xmhuang 2013-10-30
    var IsUpdate = 0, oldid_discounter, oldfeetype, oldis_feecharged, oldis_feerefund, id_custfee, id_customer, id, id_discounter, discountername, exist, ID_Section, SectionName, xmmc, yj, zk, sj, sffs, sffsName, itemID, curItem;
    jQuery("#tblList tbody tr td [name='yj']").each(function () {
        if (jQuery(this).parent().parent().find("td input[name='ItemCheckbox']").length > 0) {
            id_custfee = jQuery.trim(jQuery(this).parent().parent().attr("id_custfee")); //收费项目ID xmhuang 2013-09-25 新增用于规避出现双项目
            id_customer = jQuery.trim(jQuery(this).parent().parent().attr("id_customer")); //收费项目所对应的体检号
            id = jQuery.trim(jQuery(this).parent().parent().attr("id_fee")); //收费项目ID
            id_discounter = jQuery.trim(jQuery(this).parent().parent().attr("id_discounter")); //折扣人ID
            discountername = jQuery.trim(jQuery(this).parent().parent().attr("discountername")); //折扣人名称
            exist = jQuery.trim(jQuery(this).parent().parent().attr('exist')); //是否存在
            ID_Section = jQuery.trim(jQuery(this).parent().parent().attr('id_section')); //科室ID
            SectionName = jQuery.trim(jQuery(this).parent().parent().attr('sectionname')); //科室名称
            xmmc = jQuery.trim(jQuery(this).parent().parent().find("[name = 'xmmc']").text()); //收费项目名称
            yj = jQuery.trim(jQuery(this).parent().parent().find("[name = 'yj']").text()); //原价
            zk = jQuery.trim(jQuery(this).parent().parent().find("[name = 'zk']").text()); //折扣
            sj = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sj']").text()); //实际价格
            sffs = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sffs']").attr("feetype")); //收费方式ID
            sffsName = jQuery.trim(jQuery(this).parent().parent().find("[name = 'sffs']").text()); //收费方式名称

            /*oldid_discounter = jQuery(this).parent().parent().attr("oldid_discounter"); //原折扣人ID xmhuang 2013-12-03
            oldfeetype = jQuery(this).parent().parent().attr("oldfeetype"); //原收费方式方式 xmhuang 2013-12-03
            oldis_feecharged = jQuery(this).parent().parent().attr("oldis_feecharged"); //原付费状态 xmhuang 2013-12-03
            oldis_feerefund = jQuery(this).parent().parent().attr("oldis_feerefund"); //原退费状态 xmhuang 2013-12-03
            //判断当前行是否有改动
            IsUpdate = 0; //重置改动标记未未改动
            if (oldid_discounter != id_discounter || oldfeetype != sffs) {
            IsUpdate = 1;
            }*/
            itemID = id;
            AllIDFee += itemID + ",";
            curItem = itemID + "_" + xmmc + "_" + yj + "_" + zk + "_" + sj + "_" + sffs + "_" + sffsName + "_" + itemID + "_" + ID_Section + "_" + SectionName + "_" + id_discounter + "_" + discountername + "_" + id_customer + "_" + id_custfee + "_" + exist;
            BusFeeItems += curItem + "|";
        }
    });
    //xmhuang 2013-11-22 新增IsLoadCustomer参数用于更新客户头像信息
    var qustData = { action: 'SaveData',
        modelName: modelName,
        type: type,
        ID_ArcCustomer: ID_ArcCustomer,
        CardNum: IDCard,
        CustomerName: encodeURIComponent(CustomerName),
        ID_Customer: ID_Customer,
        ExamCard: ExamCard,
        Gender: Gender,
        GenderName: encodeURIComponent(GenderName),
        BirthDay: encodeURIComponent(BirthDay),
        Married: Married,
        MarriageName: encodeURIComponent(MarriageName),
        MobileNo: MobileNo,
        RegisteDate: encodeURIComponent(RegisteDate),
        Register: Register,
        BusSet: BusSet,
        BusPEPackageName: encodeURIComponent(BusPEPackageName),
        ReportWay: ReportWay,
        ReportWayName: encodeURIComponent(ReportWayName),
        ExamType: ExamType,
        ExamTypeName: encodeURIComponent(ExamTypeName),
        FeeWay: FeeWay,
        FeeWayName: encodeURIComponent(FeeWayName),
        GuideNurse: GuideNurse,
        GuideNurseName: encodeURIComponent(GuideNurseName),
        OperateLevel: OperateLevel,
        ExamPlace: ExamPlace,
        ExamPlaceName: encodeURIComponent(ExamPlaceName),
        Country: Country,
        CountryName: encodeURIComponent(CountryName),
        Email: encodeURIComponent(Email),
        Note: encodeURIComponent(Note),
        Nation: Nation,
        NationName: encodeURIComponent(NationName),
        Cultrul: Cultrul,
        CultrulName: encodeURIComponent(CultrulName),
        SubScribDate: encodeURIComponent(SubScribDate),
        BusFeeItems: encodeURIComponent(BusFeeItems),
        Base64Photo: jQuery("#HeadImg").attr("Base64Photo")
    };
    //存储大数据请设置Content-length值
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxRegiste.aspx",
        data: qustData,
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (jsonMsg) {

            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            jsonMsg = CheckAjaxReturnDataInfo(jsonMsg);
            if (jsonMsg == null || jsonMsg == "") {
                return;
            }


            //设置无证按钮可见
            jQuery("#btnGenerate").hide();
            //jQuery("[name='btnAdd']").show();
            jQuery("[name='btnAdd']").removeAttr("disabled");
            jQuery(jsonMsg.dataList).each(function (i, msg) {
                if (msg.success == "1") {
                    //设置套餐为存在项exist=1
                    jQuery("#tblList tbody tr").attr("exist", "1");


                    IsSaved = true;

                    //20130625,按照和xiaoqiu的讨论结果如下方式进行指引单打印
                    //这里判断是否有未完成的非团体的现金项目，如果有需要打印缴费通知单

                    var Is_Subscribed = msg.Is_Subscribed;
                    var Is_GuideSheetPrinted = msg.Is_GuideSheetPrinted;
                    var CustomerSubScribDate = item.SubScribDate;
                    var data = { SubScribDate: CustomerSubScribDate, Is_Subscribed: Is_Subscribed, Is_GuideSheetPrinted: Is_GuideSheetPrinted };
                    jQuery("#btnPrintCust1").data("data", data);

                    jQuery("#lblHidCustomer").text(msg.ID_Customer); jQuery("#txtTJH").val(msg.ID_Customer);  //由于新增时，体检号是从后台生成的，这里需要设置体检号
                    SetHidOrShowItem(); //xmhuang 20150126
                    jQuery("#lblCode128c").text(msg.Code128c);
                    //设置成功完成后的提示信息
                    //XMHuang 20130820 如果当前打印报表则不提示此信息
                    var ShowMsg = "";
                    if (Is_Subscribed == 1) {
                        ShowMsg = "客户[" + jQuery("#spanCustomerName").text() + "]成功完成预约！";
                    }
                    else {
                        ShowMsg = "客户[" + jQuery("#spanCustomerName").text() + "]成功完成登记！";
                    }
                    jQuery("[name='btnAdd']").data("ShowMsg", ShowMsg);
                    var Base64Photo = jQuery("#HeadImg").attr("Base64Photo");
                    AddCustomerQueue(msg.ID_Customer, CustomerName, GenderName, BirthDay, IDCard, Base64Photo);
                    //如果全部已经完成缴费，则直接打印指引单
                    PrintCust(1, "", msg, AllIDFee);

                    //重新绑定收费项目
                    //ReBindCustomerBusFee();//xmhuang 2013-09-25 注释，由于业务调整，此处不再绑定客户收费项目
                }
                else {
                    ShowSystemDialog(msg.Message);
                }
            });
        },
        complete: function () {
            IsSaved = true;
            ReSetControlDisabled();
        }
    });

}
function ReBindData(msg) {
    //保存成功后，这里重新绑定数据，采用调整方式进行 /System/Customer/RegistOper.aspx?type=Edit&modelName=Regist&ID_ArcCustomer=170&ID_Customer=11160620130016&IsPrint=1
    DoLoad('/System/Customer/RegistList.aspx?type=Edit&modelName=' + jQuery('#modelName').val() + 'ID_ArcCustomer=' + msg.ID_ArcCustomer + '&ID_Customer=' + msg.ID_Customer + '&IsPrint=1', '');
}
/**********全选***********/
function checkAllChildren(obj) {
    jQuery("[name='ItemCheckboxX']").each(function () {
        //判断是否隐藏
        jQuery(this).attr('checked', obj.checked);
    })
    
}
function checkself(obj) {
    $("#"+obj.id+"").attr('checked', obj.checked);
}
function checkAll(obj) {

    jQuery("[name='ItemCheckbox']").each(function () {
        //判断是否隐藏
        jQuery(this).attr('checked', obj.checked);
        //        if (obj.checked == true) {
        //            jQuery(this).parent().parent().addClass("externSelect");
        //        }
        //        else {
        //            jQuery(this).parent().parent().removeClass("externSelect");
        //        }
    })
}


/**************************用于套餐外项目添加 Begin***********************************/


/*重新绑定套餐内容 Begin*/
function DoAddX(obj) {

    jQuery("#txtSearch").val(''); //设置关键字为空
    var HasData = true;
    //jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:blue;text-align:center;">正在加载，请稍候...</td></tr>');
    var Gender = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].value; //性别
    Gender = (Gender == 1 ? Gender : 0); //0：女性，1男性，2：共用
    var GenderName = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].text; //性别
    var CustFeeID = ''; //获取当前所有套餐ID
    //查找非退费项目的收费项目ID
    jQuery("#tblList tr[name='busList'][custfeechargestate!='2']").each(function (i, item) {
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
                    newContent += '<tr OperationalDate="' + item.OperationalDate + '" name="trExternItem" class="trExternItem" CustFeeChargeState="0" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="10" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">' +
                        '<td><input onkeydown="keyMove(this, event)" onclick="checkself(this);" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox" parentid="1909"></td>' +
                        '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;background-color:white" value="' + item.FeeName + '"  id="xmmc_' + item.ID_Fee + '"></td>' +
                        '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;background-color:white" value="' + item.InputCode + '"  id="inputCode_' + item.ID_Fee + '"></td>' +
                    '</tr>';
                });
            }
            else {
                jQuery("#showBusFeeItem").html('<tr OperationalDate="' + item.OperationalDate + '" name="busList" exist="1"><td colspan="5" style="color:red;text-align:center;">对不起，没有找到适合的收费项目！</td></tr>');
                jQuery("#showBusFee").show();
            }
        },
        complete: function () {
            if (newContent == "") {
                jQuery("#showBusFeeItem").html('<tr OperationalDate="' + item.OperationalDate + '" name="busList" exist="1"><td colspan="5" style="color:red;text-align:center;">对不起，没有找到适合的收费项目！</td></tr>');
                jQuery("#showBusFee").show();
            }
            else {
                jQuery("#showBusFeeItem").append(newContent);
                BindSelect();

            }
            newContent = "";
            jQuery("#showBusFee").show();
            // 判断表格是否存在滚动条,并设置相应的样式
            JudgeTableIsExistScroll();
            //滚动到页面底部
            ScollToCustFeeBottom();
            jQuery("#txtSearch").focus();
        }
    });
}
function OnKeyUp() {
    SearchBusFee();
}
/***********关键字搜索
备注：将搜索到的内容放到最前面显示 由模糊匹配改成精确匹配
******************/
function SearchBusFee() {

    var curEvent = window.event || e;
    if (curEvent.keyCode == 37 || curEvent.keyCode == 38 || curEvent.keyCode == 39 || curEvent.keyCode == 40) {
        return false;
    }
    if (curEvent.keyCode == 13) {
        if (jQuery("#externTBlList tbody tr[id!='loading'] td input:checked").length > 0) {
            SureAdd();
        }
    }

    //jQuery("#showBusFeeItem").html('<tr name="busList" exist="1"><td colspan="3" style="color:blue;text-align:center;">正在加载，请稍候...</td></tr>');
    var Gender = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].value; //性别
    if (Gender != -1)//如果为-1，则表示男女适用
    {
        Gender = (Gender == 1 ? Gender : 0); //0：女性，1男性，2：共用
    }
    var GenderName = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].text; //性别
    var CustFeeID = "", SelectedFee = "", InputCode = jQuery.trim(jQuery("#txtSearch").val()); ; //获取当前所有套餐ID
    //查找非退费项目的收费项目ID
    //    jQuery("#tblList tr[name='busList'][feechargestaute!='已退']").each(function (i, item) {
    //        CustFeeID += "'" + jQuery(this).attr("id_fee") + "',";
    //    });
    jQuery("#tblList tr[name='busList'][custfeechargestate!='2']").each(function (i, item) {
        CustFeeID += "'" + jQuery(this).attr("id_fee") + "',";
    });

    //获取选中的收费项目
    jQuery("#showBusFeeItem tr td input:checked").each(function () {
        SelectedFee += "'" + jQuery(this).parent().parent().attr("id_fee") + "',";
    });
    CustFeeID = CustFeeID.replace(/undefined/gi, "");
    SelectedFee = SelectedFee.replace(/undefined/gi, "");

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
            SelectedFee: SelectedFee
        },
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等 20140504 by xmhuang 
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            BindCutomerBusFee(msg, GenderName);
        }
    });
}
function BindCutomerBusFee(msg, GenderName) {
    var newContent = "";
    jQuery("#showBusFeeItem").data('ExternList', msg); //缓存数据项到divExternList
    jQuery("#showBusFeeItem").empty(''); //清除显示项目
    if (msg.dataList.length > 0) {
        //由于在点击新增的时候已经过滤掉存在的CustFeeID，这里毋须再次过滤
        jQuery(msg.dataList).each(function (i, item) {
            if (item.IsChecked == 2)//以InputCode开头的
            {
                newContent += '<tr OperationalDate="' + item.OperationalDate + '" name="trExternItem" class="externCanFocus" CustFeeChargeState="0" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="' + DisCountRate + '" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">';
                newContent += '<td><input onkeydown="keyMove(this, event)" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox"></td>';

            }
            else {
                newContent += '<tr OperationalDate="' + item.OperationalDate + '" name="trExternItem" class="trExternItem" CustFeeChargeState="0" code="' + item.InputCode + '" id="' + item.ID_Fee + '" userName="' + item.userName + '" Price="' + item.Price + '" Discount="' + DisCountRate + '" FactPrice="' + item.FactPrice + '" date="' + item.date + '" FeeName="' + item.FeeName + '" name="busList" exist="0" ID_Section="' + item.ID_Section + '" SectionName="' + item.SectionName + '">';
                newContent += '<td><input onkeydown="keyMove(this, event)" id="Checkbox_' + item.ID_Fee + '" name="ItemCheckboxX" type="checkbox"></td>';
            }
            newContent += '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;" value="' + item.FeeName + '"  id="xmmc_' + item.ID_Fee + '"></td>';
            newContent += '<td><input name="textExternItem" onkeydown="keyMove(this, event)" type="text"  readonly="readonly" style="border:0px; width:90%;" value="' + item.InputCode + '"  id="inputCode_' + item.ID_Fee + '"></td>';
            newContent += '</tr>';
        });
    }
    else {
        jQuery("#showBusFeeItem").html('<tr OperationalDate="' + item.OperationalDate + '" name="busList" exist="1"><td colspan="5" style="color:red;text-align:center;">没有找到适合的收费项目！</td></tr>');
        jQuery("#showBusFee").show();
    }
    if (newContent == "") {
        jQuery("#showBusFeeItem").html('<tr OperationalDate="' + item.OperationalDate + '" name="busList" exist="1"><td colspan="5" style="color:red;text-align:center;">没有找到适合的收费项目！</td></tr>');
        jQuery("#showBusFee").show();
    }
    else {
        jQuery("#showBusFeeItem").html(newContent);
        //xmhuang 2013-09-27 经讨论，屏蔽默认选中第一个的功能
        //        if (jQuery(".externCanFocus").first().length > 0) {
        //            jQuery(".externCanFocus td input[name='textExternItem']").first().focus(); //设置以InputCode开始的项光标
        //            jQuery(".externCanFocus td input[name='textExternItem']").first().select(); //设置以InputCode开始的项为选中项
        //        }
        newContent = "";
        BindSelect();
    }

}
/***********确认新增********************/
function SureAddCurrentRow(obj) {
    var RowNum = jQuery("#tblList tbody tr[id!='loading']").length + 1, newContent = '', checked = false, ID_Fee = '', userName = '', date = '', FeeName = '', Price = 0, Discount = 0, FactPrice = 0, FeeType = document.getElementById("slFeeWay").value, ID_Section = '', SectionName = '';
    var OperationalDate = "";
    ID_Fee = jQuery(obj).parent().parent().attr("id");
    userName = jQuery(obj).parent().parent().attr("userName");
    date = jQuery(obj).parent().parent().attr("date");
    FeeName = jQuery(obj).parent().parent().attr("FeeName");
    Price = jQuery(obj).parent().parent().attr("Price");
    //Discount = jQuery(obj).parent().parent().attr("Discount");
    FactPrice = jQuery(obj).parent().parent().attr("FactPrice");
    CustFeeChargeState = jQuery(obj).parent().parent().attr("CustFeeChargeState");
    ID_Section = jQuery(obj).parent().parent().attr("ID_Section");
    SectionName = jQuery(obj).parent().parent().attr("SectionName");
    OperationalDate = jQuery(obj).parent().parent().attr("OperationalDate");
    TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("TemplateRegistCustFee");
    teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
    teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
    newContent = "";
    var title = '""';
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
                            .replace(/@Price/gi, parseFloat(Price).toFixed(2))
                            .replace(/@FactPrice/gi, FactPrice)
                            .replace(/@Is_FeeRefund/gi, 0)
                            .replace(/@Is_FeeCharged/gi, 0)
                            .replace(/@Discount/gi, 10)

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

                            .replace(/@ID_Discounter/gi, UserID)
                            .replace(/@XDiscounterName/gi, UserName)
                            .replace(/@ID_Register/gi, UserID)
                            .replace(/@RegisterName/gi, UserName)
                            .replace(/@RegistDate/gi, CurDate)
                            .replace(/@XFeeChargeStaute/gi, "")
                            .replace(/@OperationalDate/gi, OperationalDate)
                            .replace(/@tilte/gi, title);

    }
    jQuery(obj).parent().parent().remove();
    if (newContent != '') {
        jQuery("#tblList tbody").append(newContent);
        //SetTableEvenOddRowStyle();
        newContent = "";
        DoScrollToBottom();
        SumJG(); //计算合计
        SetTableEvenOddRowStyle();                          //设置奇偶项样式 xmhuang 2014-04-11
    }
    ResetSearch();
    //滚动到页面底部
    ScollToCustFeeBottom();
}
function SureAdd() {
    var newContent = '', checked = false, ID_Fee = '', userName = '', date = '', FeeName = '', Price = 0, Discount = 0, FactPrice = 0, FeeType = document.getElementById("slFeeWay").value, ID_Section = '', SectionName = '';
    var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("TemplateRegistCustFee");
    var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
    var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
    var RowNum = jQuery("#tblList tbody tr[id!='loading']").length;
    var OperationalDate = "";
    var title = '""';
    jQuery("#showBusFeeItem tr[id!='loading'] td input[name='ItemCheckboxX']").each(function (i,item) {
        checked = jQuery(this).attr("checked");
        if (checked) {
            RowNum++;
            ID_Fee = jQuery(this).parent().parent().attr("id");
            userName = jQuery(this).parent().parent().attr("userName");
            date = jQuery(this).parent().parent().attr("date");
            FeeName = jQuery(this).parent().parent().attr("FeeName");
            Price = jQuery(this).parent().parent().attr("Price");
            //Discount = jQuery(this).parent().parent().attr("Discount");
            FactPrice = jQuery(this).parent().parent().attr("FactPrice");
            CustFeeChargeState = jQuery(this).parent().parent().attr("CustFeeChargeState");
            ID_Section = jQuery(this).parent().parent().attr("ID_Section");
            SectionName = jQuery(this).parent().parent().attr("SectionName");
            OperationalDate = jQuery(this).parent().parent().attr("OperationalDate");
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
                            .replace(/@Price/gi, parseFloat(Price).toFixed(2))
                            .replace(/@FactPrice/gi, FactPrice)
                            .replace(/@Is_FeeRefund/gi, 0)
                            .replace(/@Is_FeeCharged/gi, 0)
                            .replace(/@Discount/gi, 10)

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
                            .replace(/@OperationalDate/gi, OperationalDate)
                            .replace(/@tilte/gi, title)
                            ;
            }
            jQuery(this).parent().parent().remove();
            //设置下一个元素选中

        }
    });
    if (newContent != '') {
        IsSaved = false;
        jQuery("#tblList tbody").append(newContent);
        //SetTableEvenOddRowStyle();
        newContent = "";
        DoScrollToBottom();
        BindFeeWay();
        BindKeyup();
        ResetSearch();
        DoClose();
        SetTableEvenOddRowStyle();                          //设置奇偶项样式 xmhuang 2014-04-11
        //设置固定表头 xmhuang 2014-04-01
        //$('#tblList').tablefix({ height: 200, width: 400, fixRows: 1, fixCols: 2 });
    }
    SumJG(); //计算合计
    //滚动到页面底部
    ScollToCustFeeBottom();
    return false;
}
//绑定搜索项获取光标即选中
function BindSelect() {
    jQuery("#showBusFeeItem input[type='text']").focus(function () {
        this.select();
    });

}

function DoClose() {
    jQuery("#showBusFeeItem").empty();
    jQuery("#showBusFee").hide(); // 判断表格是否存在滚动条,并设置相应的样式
    JudgeTableIsExistScroll();
}
function DoSelectAll() {
    jQuery("#txtSearch").select();
}

/**************************用于套餐外项目添加 End***********************************/
function DoScrollToWindowBottom(elementID, height) {
    window.scrollTo(0, document.body.scrollHeight - 100);
}
function DoScrollToBottom() {
    ScollToCustFeeBottom();
    // window.scrollTo(0, document.body.scrollHeight - 100);
}
/// <summary>
/// 滚动指定高度
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
function DoScrollValueToBottom(scrollHeight) {
    // window.scrollTo(0, document.body.scrollHeight - 100);
    var top = (document.documentElement.scrollTop + document.documentElement.clientHeight + scrollHeight);
    //var left = document.body.scrollWidth; //(document.documentElement.scrollLeft + document.documentElement.clientWidth - 50);
    window.scrollTo(0, top);
}


/*************键盘操作事件：主要为上下左右键，适用于table******************/
function keyMove(item, event) {

    var elementID = "externTBlList";
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
    if (objTable.rows[x].cells[y] != undefined) {
        if (objTable.rows[x].cells[y].children[0] != undefined) {

            //回车默认选中当前行
            if (event.keyCode == 13) {
                objTable.rows[x].cells[0].children[0].checked = !objTable.rows[x].cells[0].children[0].checked;
                if (objTable.rows[x].cells[0].children[0].name == "checkAllX") {
                    checkAllChildren(objTable.rows[x].cells[0].children[0]);
                }
            }
            if (objTable.rows[x].cells[y].children[0].type != "button") {
                objTable.rows[x].cells[y].children[0].blur();
                objTable.rows[x].cells[y].children[0].focus();
                if (objTable.rows[x].cells[y].children[0].id != "txtSearch") {
                    objTable.rows[x].cells[y].children[0].select();
                    //这里自动新增选中行数据
                    if (event.keyCode == 13) {
                        //判断是不是全选行
                        if (objTable.rows[x].cells[y].children[0].name != "checkAllX") {
                            SureAddCurrentRow(objTable.rows[x].cells[y].children[0]);
                            //滚动
                            DoScrollValueToBottom(10);
                            if (objTable.rows[x] != undefined) {
                                //                            objTable.rows[x].cells[y].children[0].focus();
                                //                            objTable.rows[x].cells[y].children[0].select();
                                //                        }
                                //                        else {

                            }
                            else {
                                //ResetSearch();
                            }

                        }
                        //ResetSearch();
                    }
                }
            }
            else {

            }
        }
    }
    return;
}

function ResetSearch() {
    jQuery("#chbAll1").attr("checked", false);
    if (jQuery("#txtSearch").is(":visible") == true) {
        document.getElementById("txtSearch").focus();
        document.getElementById("txtSearch").select();
    }
    //document.getElementById("txtSearch").value = " ";
}
/*********获取浏览器代理**************/
function GetCLientAgent() {

    if (navigator.userAgent.indexOf("MSIE") > -1) {
        return "MSIE";
    }
    else if (navigator.userAgent.indexOf("Firefox") > -1) {
        return "Firefox";
    }
}


//编辑页面或者新增页面新增预约登记
function AddNewRegistOrSign(IsCheck) {
    //判断是否有没有保存的项
    if (IsCheck == 1) {
        var length = jQuery("#tblList tbody tr[id!='loading'][id_customer='undefined']").length;
        if (length > 0) {
            var msgContent = "列表中存在未保存的项，您需要保存吗？";
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
                        jQuery("#btnRegiste1").click();
                        return true;

                    }, focus: true
                }]

            }).lock();
        }
    }
    //重新设置当前操作类型为新增：Add
    type = "add";
    //    if (operType != undefined) {
    //        operType.SetID(type);
    //    }
    SetShowElement(); //设置体检号、出生日期、性别显示方式
    IsSaved = false;

    ResetAllCustomerInfo();
    //设置头像
    jQuery("#HeadImg").attr("src", defalutImagSrc);
    jQuery("#divBox").attr("src", defalutImagSrc);  
    jQuery("#HeadImg").attr("Base64Photo", "");
    document.getElementById("txtSFZ").value = "";
    document.getElementById("txtYKT").value = "";
    document.getElementById("txtTJH").value = "";
    jQuery("#spanCustomerName").text("");

    document.getElementById("txtEmail").value = "";
    document.getElementById("txtBirthDay").value = "";
    document.getElementById("txtMobil").value = "";
    document.getElementById("txtNote").value = "";
    if (document.getElementById("txtSubScribDate") != null) {
        document.getElementById("txtSubScribDate").value = "";
    }
    //清除套餐
    jQuery("#tblList tbody").html("");
    jQuery("#slSex").change(); //触发slSex change事件

    ResetSearch(); //设置光标
    //设置无证按钮可见
    if (IsCommon == 1 || IsTeam != 1) {
        jQuery("#btnGenerate").show();
    }
    jQuery("[name='btnAdd']").hide();

    jQuery("#slOperateLevel").html(SecurityLevelHtml);              //设置用户操作密级
    jQuery("#txtSubScribDate").show();                              //显示体检时间
    jQuery("#txtSubScribDate").val(CurDate);                        //设置体检时间默认为当天
    jQuery("#txtSearchX", parent.document).val(""); //置空检索值
    jQuery("#txtSearchX", parent.document).focus();
    jQuery("#divAddCustFee").show(); //显示新增收费项目操作组

    jQuery("#lblHidCustomer").text(""); //置空体检号的值
    jQuery("#txtTJH").val(""); //置空体检号的值 


    jQuery("#imgCustomer").show(); //显示获取图片按钮

    jQuery("#lblAge").text(""); //重置年龄显示
}
/**************通过身份证号码获取用户基本信息**********************/
/// <summary>
/// 更新客户基本信息并返回
///修改内容：新增IsUpdateUserInfo参数，用户设置是否在采集图像时更新客户信息
///由于在采集图像就更新客户信息时容易造成重档，这里暂时设置为不更新客户信息,即参数IsUpdateUserInfo：0
/// </summary>
function PostImg(Base64Photo, objID, Key, IsLoadCustomer, IsUpdateUserInfo) {
    if (IsUpdateUserInfo == undefined || IsUpdateUserInfo == null) {
        IsUpdateUserInfo = 0;
    }
    var modelName = jQuery("#modelName").val();
    var KeyValue = jQuery.trim(jQuery("#" + objID).val());
    //var IDCard = jQuery.trim(jQuery("#txtSFZ").text()); //jQuery("#txtSearchX", parent.document).val("");
    var IDCard = $('#txtSFZ').val(); //document.getElementById("txtSFZ").value; //身份证号码 ：$("#txt").attr("value")；
    var CustomerName = jQuery("#spanCustomerName").text(); //客户名称 这里只传证件号进行检索以便查看是否具有多条体检信息 IIIIII
    var Gender = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].value; //性别
    var GenderName = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].text; //性别
    var BirthDay = document.getElementById("txtBirthDay").value; //出生年月
    var Nation = jQuery("#idSelectNation").val();               //民族ID
    var NationName = jQuery("#nameSelectNation").val();
    //    if (Nation == "") {
    //        Nation = 1;
    //        NationName = NationArray[Nation - 1];
    //    }
    var tempBase64Photo = Base64Photo;
    //var IDCard = jQuery.trim(jQuery("#txtSFZ").val());
    if (IDCard != "") {
        var Is_Org = 0;
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxRegiste.aspx",
            contentType: "application/x-www-form-urlencoded;Content-length=1024000",
            data: { action: 'SaveCustomerInfo',
                IDCard: IDCard,
                CustomerName: CustomerName,
                Gender: Gender,
                GenderName: GenderName,
                BirthDay: BirthDay,
                Nation: Nation,
                NationName: NationName,
                Base64Photo: tempBase64Photo,
                Key: Key,
                KeyValue: KeyValue,
                type: type,
                modelName: modelName,
                IsLoadCustomer: IsLoadCustomer,
                IsUpdateUserInfo: IsUpdateUserInfo
            },
            cache: false,
            dataType: "json",
            success: function (msg) {
                // 检查Ajax返回数据的状态等
                msg = CheckAjaxReturnDataInfo(msg);
                if (msg == null || msg == "") {
                    return;
                }
                //IsLoadCustomer==0表示不加载用户信息
                if (IsLoadCustomer == 2) {
                    //只检索基本信息
                    if (msg.dataList != undefined) {
                        var item = msg.dataList[0];
                        if (IDCard == item.IDCard && CustomerName == item.CustomerName)  //如果检索出来的客户证件号和姓名都相同则直接使用，否则使用从证件上读取的信息为准
                        {
                            jQuery("#txtSFZ").text(item.IDCard);
                            jQuery("#lblSFZ").text(item.IDCard);
                            jQuery("#spanCustomerName").text(item.CustomerName);
                            jQuery("#txtYKT").val(item.ExamCard);
                            jQuery("#txtBirthDay").val(item.date);
                            jQuery("#lblHidBirthDay").text(item.date); //设置出生日期
                            CaltAgeByBirthDay();
                            jQuery("#txtMobil").val(item.MobileNo);
                            jQuery("#txtEmail").val(item.Email);
                            jQuery("#lblHidSex").text(item.GenderName); //设置性别
                            jQuery("#slSex [value='" + parseInt(item.ID_Gender) + "']").attr("selected", true);
                            jQuery("#slSex").change();
                            jQuery("#slNation [value='" + parseInt(item.NationID) + "']").attr("selected", true);
                            jQuery("#slMarried [value='" + item.ID_Marriage + "']").attr("selected", true);
                            jQuery("#slCultrul [value='" + item.CultrulID + "']").attr("selected", true);
                        }
                    }
                }
                else if (IsLoadCustomer == 1) {
                    if (msg.dataList != undefined) {
                        //mark:此处验证为什么扫描身份证没有绑定客户民族和身份证信息
                        var msgLength = msg.dataList.length;

                        //存在用户基本信息
                        //如果客户信息大于两条则提示操作者选择客户信息后绑定其基本信息
                        if (msgLength > 0) {
                            //只存在一个客户信息
                            if (msgLength == 1) {
                                var IsLoadCustomerInfo = 1; //默认设置检索
                                //如果是证件号检索并且是团体客户这里需要通过证件号查询客户是否存在多个未打印指引单的体检信息
                                if (IsTeam == 1)//如果当前模块是团体登记模块
                                {
                                    if (Key == "IDCard")//如果是通过证件号检索
                                    {
                                        //查询该证件号对应的所有体检信息，如果存在多个则提示，只存在一个则直接进行设置
                                        var allCustomerExamInfoDT = GetCustPhysicalExamInfoByIDCard(msg.dataList[0].IDCard, msg.dataList[0].CustomerName, IsTeam);
                                        var dataList = allCustomerExamInfoDT.dataList;
                                        //如果只存在单个体检信息，则直接绑定    
                                        if (dataList.length == 1) {
                                            var oldValue = jQuery("#txtSearchX", parent.document).val();
                                            jQuery("#txtSearchX", parent.document).val(dataList[0].ID_Customer);     //设置检索区域值为当前体检号
                                            RequestCustomerInfo("txtSearchX", "ID_Customer", 1);    //通过检索区域值检索客户体检信息
                                            jQuery("#txtSearchX", parent.document).val(oldValue);
                                        }
                                        else if (dataList.length > 1) {
                                            ChooseCustomerPhysicalExamInfo(dataList);
                                        }
                                        else {
                                            SetCustomerInfo(msg.dataList, '', IsLoadCustomerInfo);
                                        }
                                        return false;
                                    }
                                }
                                SetCustomerInfo(msg.dataList, '', IsLoadCustomerInfo);
                                //不管新增还是编辑检索身份证都将检索客户收费信息
                                //                                if (type.toLowerCase() == "add")//新增状态下不检索客户体检信息
                                //                                {
                                //                                    IsLoadCustomerInfo = 0; //设置不检索
                                //                                }
                                //                                SetCustomerInfo(msg.dataList, '', IsLoadCustomerInfo); //设置客户基本信息，并设置是否检索可体检信息
                            }
                            //存在相同身份证多个客户的情况
                            else {
                                ChooseCustomerInfo(msg.dataList);
                            }
                        }
                    }
                }
            }
        });
    }
}
function ReadBustFeeData(Is_Org, dataList) {
    //非团队
    //    if (Is_Org == 0 || Is_Org.toLowerCase() == "False".toLowerCase())
    //        return false;
    if (dataList != undefined) {
        if (dataList.length > 0) {

            //这里读取模版
            var TemplateTeamTaskGroupFeeContent = ReadTemplateTeamTaskGroup("TemplateRegistCustFee");
            var teamTaskGroupFeeListTheadTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskGroupListTheadTempleteContent;
            var teamTaskGroupFeeListBodyTempleteContent = TemplateTeamTaskGroupFeeContent.teamTaskListBodyTempleteContent;
            //绑定套餐信息
            var newContent = '', CustCssStyle = '', RowNum = 0, exist = 0, ItemCheckbox = "ItemCheckbox";
            var XFeeChargeStaute = "";
            var oldtitle = "";
            //清除套餐
            jQuery("#tblList tbody").html("");
            jQuery(dataList).each(function (i, item) {
                if (jQuery("#tblList tr[id!='" + item.ID_Fee + "']").length > 0) {
                    RowNum++;
                    XFeeChargeStaute = "";
                    //如果是团体项目则不允许删除，隐藏checkbox
                    if (item.ID_TeamTaskGroup != "") {
                        exist = 1;
                        //ItemCheckbox = "UnItemCheckbox";//团体和个人一样完成交费后不可操作
                        CustCssStyle = "Yellow";

                        //判断收费状态
                        if (item.FeeChargeStaute == "2") {
                            ItemCheckbox = "UnItemCheckbox";
                            CustCssStyle = "Yellow";
                            item.FeeChargeStaute = "√";
                            XFeeChargeStaute = "√";
                        }
                        //判断是否是已检项目，已检项目不允许退费Is_Examined
                        else if (item.FeeChargeStaute == "1") {
                            ItemCheckbox = "UnItemCheckbox";
                            item.FeeChargeStaute = "√";
                            CustCssStyle = "Green";
                        }
                        else if (item.FeeChargeStaute == "0") {
                            ItemCheckbox = "ItemCheckbox"; //团体未收费显示复选框
                            CustCssStyle = "Red"; //团体未收费显示复选框
                            //ItemCheckbox = "UnItemCheckbox";
                            item.FeeChargeStaute = "";
                            // CustCssStyle = "TeamRed";
                        }
                    }
                    else {
                        //判断收费状态
                        if (item.FeeChargeStaute == "2") {
                            ItemCheckbox = "UnItemCheckbox";
                            CustCssStyle = "Yellow";
                            item.FeeChargeStaute = "√";
                            XFeeChargeStaute = "√";
                        }
                        //判断是否是已检项目，已检项目不允许退费Is_Examined
                        else if (item.FeeChargeStaute == "1") {

                            item.FeeChargeStaute = "√";
                            ItemCheckbox = "UnItemCheckbox";
                            CustCssStyle = "Green";
                        }
                        else if (item.FeeChargeStaute == "0") {
                            item.FeeChargeStaute = "";
                            CustCssStyle = "Red";
                        }
                    }

                    //设置禁用样式
                    oldtitle = "";
                    if (item.Is_Banned == "True" || item.Is_Banned == 1) {
                        CustCssStyle = CustCssStyle + " Banned";
                        oldtitle = "该收费项目已禁用";
                    }
                    //判断是修改还是新增
                    if (type.toLowerCase() == "edit")
                    { exist = 1; }
                    if (item.ID_Customer != "") {
                        exist = 1;
                    }
                    if (teamTaskGroupFeeListBodyTempleteContent != null) {
                        newContent += teamTaskGroupFeeListBodyTempleteContent.replace(/@ItemCheckbox/gi, ItemCheckbox)
                            .replace(/@ID_TeamTaskGroup/gi, item.ID_TeamTaskGroup)
                            .replace(/@exist/gi, exist)
                            .replace(/@Is_Org/gi, Is_Org)
                            .replace(/@ID_Customer/gi, item.ID_Customer)
                            .replace(/@ID_Fee/gi, item.ID_Fee)
                            .replace(/@FeeName/gi, item.FeeName)
                            .replace(/@FeeChargeStaute/gi, item.FeeChargeStaute)
                            .replace(/@Price/gi, parseFloat(item.Price).toFixed(2))
                            .replace(/@FactPrice/gi, item.FactPrice)
                            .replace(/@Is_FeeRefund/gi, item.Is_FeeRefund)
                            .replace(/@Is_FeeCharged/gi, item.Is_FeeCharged)
                            .replace(/@Discount/gi, item.Discount)

                            .replace(/@XDiscounterName/gi, item.DiscounterName)
                            .replace(/@ID_Section/gi, item.ID_Section)
                            .replace(/@SectionName/gi, item.SectionName)
                            .replace(/@date/gi, item.date)
                            .replace(/@FeeType/gi, item.ID_FeeType)
                            .replace(/@FeeWayName/gi, jQuery(allHiddFeeWay).find("option[value='" + item.ID_FeeType + "']").text())
                            .replace(/@RowNum/gi, RowNum)
                            .replace(/@CustCssStyle/gi, CustCssStyle)
                            .replace(/@ID_CustFee/gi, item.ID_CustFee)
                            .replace(/@CustFeeChargeState/gi, item.CustFeeChargeState)
                            .replace(/@Is_Printed/gi, item.Is_Printed)
                            .replace(/@XFeeChargeStaute/gi, XFeeChargeStaute)


                            .replace(/@Discount/gi, item.Discount)
                            .replace(/@ID_Discounter/gi, item.ID_Discounter)
                            .replace(/@XDiscounterName/gi, item.DiscounterName)
                            .replace(/@ID_Register/gi, item.ID_Register)
                            .replace(/@RegisterName/gi, item.RegisterName)
                            .replace(/@RegistDate/gi, item.RegistDate)
                            .replace(/@OperationalDate/gi, item.OperationalDate)

                            .replace(/@Is_Banned/gi, item.Is_Banned)    //新增是否禁用绑定值
                            .replace(/@tilte/gi, '"' + oldtitle + '"')//如果是默认属性，其key value没有用双引号括起来，所以这里需要特殊处理下
                        // .replace(/@tilte/gi, oldtitle);    //新增是否禁用绑定值
                        ;
                    }
                }
            });
            if (newContent != '') {
                jQuery("#tblList tbody").empty();
                jQuery("#tblList tbody").append(newContent);
                //SetTableEvenOddRowStyle();
                newContent = "";
                DoScrollToBottom();
                BindFeeWay();
                BindKeyup();
                SetTableEvenOddRowStyle();                          //设置奇偶项样式
            }
            jQuery("#idSelectSet").removeData("ID_Customer"); //移除数据项
            SumJG(); //计算合计
            jQuery("#lblErrorMessage").text("");
        }
    }
}
//选择用户基本信息,调用此方法时，用户身份证信息是>1条的
function ChooseCustomerInfo(dataList) {
    //ID_ArcCustomer,IDCard,CustomerName,CustomerName,ExamCard,ID_Gender
    var content = jQuery("#RegisteCustumerDialog").html();
    var newContent = "";
    var imgSrc = "";
    jQuery(dataList).each(function (i, item) {
        if (item.Base64Photo == "") {
            imgSrc = defalutImagSrc;
        }
        else {
            imgSrc = "data:image/gif;base64," + item.Base64Photo;
        }
        newContent += content.replace(/@ID_ArcCustomer/gi, item.ID_ArcCustomer)
                         .replace(/@IDCard/gi, item.IDCard)
                         .replace(/@CustomerName/gi, item.CustomerName)
                         .replace(/@ID_Gender/gi, item.ID_Gender)
                         .replace(/@GenderName/gi, item.GenderName)
                         .replace(/@CustomerName/gi, item.CustomerName)
                         .replace(/@imgSrc/gi, imgSrc)
                         .replace(/@date/gi, item.date)
    });

    content = '<div class="RegisteCustumerDialog">' + newContent + '</div>';
    var dialog = art.dialog({
        lock: true, fixed: true, opacity: 0.3,
        padding: 0,
        id: "dialog1",
        //        title: '请选择客户信息(点击图片即可选择)',
        title: '请核实以下客户信息',
        content: content
        //follow: document.getElementById('txtSFZ')
    }).lock();
    jQuery(window).data("dataList", dataList);
}
function SureChoose(obj, dataList) {
    ResetAllCustomerInfo();
    var ID_ArcCustomer = jQuery(obj).attr("ID_ArcCustomer");
    var CurdataList = jQuery(window).data("dataList");
    SetCustomerInfo(CurdataList, ID_ArcCustomer, 1);
    CloseDialog("dialog1");
}
function CloseDialog(dialogID) {
    var list = art.dialog.list;
    for (var i in list) {
        if (i == dialogID) {
            list[i].close();
        }
    };
}
/// <summary>
/// 设置用户基本信息
/// <param name="dataList">数据集，此数据集包含客户的基本信息</param>
/// <param name="selectedID_ArcCustomer">当前选中的客户存档ID，一般在身份证重合才会为其赋值，默认情况为空</param>
/// <param name="IsLoadCustomerInfo">是否加载客户体检号，本方法一般通过客户身份证号码、体检号、一卡通号码进行检索获取客户基本信息。
///但是在新增状态下，值允许设置客户基本信息，而不允许设置客户体检号，客户收费项目信息等。此标记用于区分新增或修改操作
///</param>
/// </summary>
/// <returns></returns>
function SetCustomerInfo(dataList, selectedID_ArcCustomer, IsLoadCustomerInfo) {

    if (dataList == null || dataList == undefined)
        return false;
    var item;
    for (var c = 0; c < dataList.length; c++) {
        item = dataList[c];
        if (selectedID_ArcCustomer != "" && selectedID_ArcCustomer != item.ID_ArcCustomer) {
            continue;
        }
        if (jQuery("#HeadImg").attr("IsExist") == 1)//用于区分是从卡片读取或者是拍照时，不再设置头像,此设置已经在读卡和拍照时完成
        {

        }
        else {
            //设置头像
            if (item.Base64Photo == "") {
                jQuery("#HeadImg").attr("src", defalutImagSrc);
                jQuery("#divBox").attr("src", defalutImagSrc); 
            }
            else {
                jQuery("#HeadImg").attr("src", "data:image/gif;base64," + item.Base64Photo + "");
                jQuery("#divBox").attr("src", "data:image/gif;base64," + item.Base64Photo + ""); 
                jQuery("#HeadImg").attr("Base64Photo", item.Base64Photo);

            }
        }
        jQuery("#HeadImg").removeAttr("IsExist"); // 使用后移除存在标记

        //绑定用户基本信息
        jQuery("#txtSFZ").val(item.IDCard);
        jQuery("#lblSFZ").text(item.IDCard);
        jQuery("#spanCustomerName").text(item.CustomerName);
        jQuery("#txtYKT").val(item.ExamCard);
        jQuery("#txtBirthDay").val(item.date);
        jQuery("#lblHidBirthDay").text(item.date); //设置出生日期
        CaltAgeByBirthDay();
        jQuery("#txtMobil").val(item.MobileNo);
        jQuery("#txtEmail").val(item.Email);
        jQuery("#lblHidSex").text(item.GenderName); //设置性别
        jQuery("#slSex [value='" + parseInt(item.ID_Gender) + "']").attr("selected", true);
        jQuery("#slSex").change();
        //jQuery("#slNation [value='" + parseInt(item.NationID) + "']").attr("selected", true);
        jQuery("#slMarried [value='" + item.ID_Marriage + "']").attr("selected", true);
        jQuery("#slCultrul [value='" + item.CultrulID + "']").attr("selected", true);
        if (item.NationID != "" && item.NationID != -1) {

            ShowQuickSelectNation(parseInt(item.NationID), NationArray[parseInt(item.NationID) - 1]);          // 设置民族的已选项获取客户基本信息，需要使用此方法设置民族
        }
        if (IsLoadCustomerInfo == 0)//不加载客户体检号信息和客户收费项目信息
        {
            return false;
        }
        else {
            //判断ID_Customer是否存在，如果存在，则直接使用体检号进行检索
            if (item.ID_Customer != undefined) {
                jQuery("#txtTJH").val(item.ID_Customer); //设置体检号
                jQuery("#lblHidCustomer").text(item.ID_Customer); //设置体检号
                SetHidOrShowItem(); 
                GetCustomerExamPhysicalInfo("", item.ID_Customer, IsLoadCustomerInfo); //通过存档ID获取用户体检信息
            }
            else {
                GetCustomerExamPhysicalInfo(item.ID_ArcCustomer, "", IsLoadCustomerInfo); //通过存档ID获取用户体检信息
            }
        }
    }
    jQuery("#lblErrorMessage").text("");
}
/// <summary>
/// 获取用户体检信息和设置套餐内容
///新增IsTeam参数，用于针对团体检索只检索最近一次此客户对应团体的最近一次体检信息
/// </summary>
/// <returns></returns>
function GetCustomerExamPhysicalInfo(ID_ArcCustomer, ID_Customer, IsLoadCustomerInfo) {
    //如果是通用功能模块，则是否团体查询条件不起作用
    var curIsTeam = IsTeam;
    if (IsCommon == 1) {
        curIsTeam = -1;
    }
    if (ID_ArcCustomer != "" || ID_Customer != "") {
        var Is_Org = 0;
        jQuery.ajax({
            type: "POST",
            url: "/Ajax/AjaxRegiste.aspx",
            data: { action: 'GetCustomerExamPhysicalInfo',
                ID_ArcCustomer: ID_ArcCustomer,
                ID_Customer: ID_Customer,
                IsTeam: curIsTeam
            },
            cache: false,
            dataType: "json",
            success: function (msg) {
                // 检查Ajax返回数据的状态等
                msg = CheckAjaxReturnDataInfo(msg);
                if (msg == null || msg == "") {
                    return;
                }
                if (msg.dataList0.length > 0) {
                    jQuery(msg.dataList0).each(function (i, item) {
                        jQuery("#spanCustomerName").text(item.CustomerName);
                        jQuery("#lblHidOperateDate").text(item.OperateDate); //绑定指引单打印时间
                        //绑定操作密级
                        if (item.SecurityLevel != "") {
                            jQuery("#slOperateLevel [value='" + item.SecurityLevel + "']").attr("selected", true);
                        }
                        if (item.ID_ReportWay != "") {
                            jQuery("#slReportWay [value='" + item.ID_ReportWay + "']").attr("selected", true);
                        }
                        if (item.ExamPlaceID != "") {
                            jQuery("#slExamPlace [value='" + item.ExamPlaceID + "']").attr("selected", true);
                        }
                        //判断用户体检号是否已加密
                        if (item.SecurityLevel > 100)//已加密客户不允许查看
                        {
                            jQuery("#divCustomerInfo").parent().next().hide();
                            ResetAllCustomerInfo();
                            ShowSystemDialog("对不起，该客户已加密，不允许查看！");
                            return false;
                        }
                        if (item.Is_Paused == 1)//已禁检的客户
                        {
                            jQuery("[name='btnComplete']").hide();
                            ShowSystemDialog("对不起，该客户已禁检，不允许进行体检！");
                            //return false;
                        }
                        else if (item.Is_Checked == 1 || item.Is_Checked == "True")//已完成总检不允许修改即保存
                        {
                            jQuery("[name='btnComplete']").hide(); //隐藏保存按钮
                            jQuery("#divAddCustFee").hide(); //隐藏新增收费项目操作组
                            jQuery("#imgCustomer").hide(); //隐藏获取头像按钮
                        }
                        else {
                            jQuery("[name='btnComplete']").show(); //显示保存按钮
                            jQuery("#divAddCustFee").show(); //显示新增收费项目操作组
                            jQuery("#imgCustomer").show(); //显示获取头像按钮
                            jQuery("#divCustomerInfo").parent().next().show();
                        }

                        /************************设置团体和非团体显示信息和体检信息  Begin*****************************/
                        if (item.Is_Team == "True")//如果是团体
                        {
                            Is_Org = "1";
                            item.RoleName = item.RoleName == undefined ? "" : item.RoleName;

                            //设置团体信息
                            //jQuery("#spanTeamNameTitle").text("单位：");
                            jQuery("#spanTeamName").text(item.TeamName);

                            //设置部门信息
                            //jQuery("#spanDepartTitle").text("部门：");
                            jQuery("#spanDepart").text(item.Department + '-' + item.DepartSubA + '-' + item.DepartSubB + '-' + item.DepartSubC);

                            //设置角色信息
                            //jQuery("#spanRoleNameTitle").text("角色：");
                            jQuery("#spanRoleName").text(item.RoleName);
                            //设置团体关联label为显示状态

                            jQuery(".sapnTeamTitle").show();

                        }
                        else {
                            Is_Org = "0";

                            //设置团体关联label为隐藏状态
                            jQuery(".sapnTeamTitle").hide();

                            //重置团体信息
                            //jQuery("#spanTeamNameTitle").text("");
                            jQuery("#spanTeamName").text("");

                            //重置部门信息
                            //jQuery("#spanDepartTitle").text("");
                            jQuery("#spanDepart").text("");

                            //重置角色信息
                            //jQuery("#spanRoleNameTitle").text("");
                            jQuery("#spanRoleName").text("");


                        }

                        ShowQuickSelectExamType(item.ID_ExamType, item.ExamTypeName);    // 设置体检类型的已选项
                        /************************设置团体和非团体显示信息和体检信息  End*****************************/
                        if (type.toLowerCase() == "add" && IsSearchIDCustomer == 0 && item.Is_Team != "True")//新增，并且检索的是身份证号
                        {
                            jQuery("#txtSubScribDate").val(CurDate); jQuery("#lblSubScribDate").text(CurDate);
                        }
                        else {

                            SetCustomerSubscribedInfo(item.Is_Subscribed); //设置客户预约还是登记信息
                            jQuery("#lblCode128c").text(item.Code128c); //设置Code128c码，用于单据打印.
                            jQuery("#idSelectSet").data("ID_Customer", item.ID_Customer);
                            //xmhuang 2013-09-28 由于从设备读取身份证信息只检索客户基本信息，不可用设置体检号和收费项目信息
                            var ReadCustomerFromCard = jQuery("#btnReadFromMachine").data("ReadCustomerFromCard");
                            if (ReadCustomerFromCard == 1) {
                                RemoveSelectedUser();    // 设置导检护士的已选人员
                                RemoveSelectedSet();    // 设置体检类型的已选项
                                jQuery("#lblHidCustomer").text(""); //重置体检号
                                jQuery("#txtTJH").val(""); //重置体检号
                                jQuery("#txtSubScribDate").val(CurDate); //设置登记时间为当天
                                jQuery("#lblSubScribDate").text(CurDate); //设置登记时间为当天
                                //jQuery("#btnReadFromMachine").data("ReadCustomerFromCard", 0); //重置来源于设备读取标记

                            }
                            //由于从设备读取身份证信息只检索客户基本信息，不可用设置体检号和收费项目信息
                            else {
                                ShowQuickSelectUser(item.ID_GuideNurse, item.GuideNurse);    // 设置导检护士的已选人员
                                if (item.NationName == "" || item.NationID == -1)
                                { }
                                else {

                                    ShowQuickSelectNation(item.NationID, item.NationName);          // 设置民族的已选项
                                }
                                jQuery("#lblHidCustomer").text(item.ID_Customer); jQuery("#txtTJH").val(item.ID_Customer); //设置体检号
                                SetHidOrShowItem(); 
                                jQuery("#txtSubScribDate").val(item.SubScribDateX); jQuery("#lblSubScribDate").text(item.SubScribDateX);
                                jQuery("#txtNote").val(item.Note); //设置备注信息
                                //如果是团体，这里不可用自动触发套餐变动事件，需要直接绑定其对应套餐信息
                                //if (item.Is_Team == "True") {
                                if (IsSearch == 1)//如果是检索身份证获取是体检号或者是一卡通，则需要通过体检号绑定客户收费项目信息
                                {
                                    ShowQuickSelectSet(item.PEPackageID, item.PEPackageName, true); //绑定套餐并绑定其收费项目
                                }
                                else { ShowQuickSelectSet(item.PEPackageID, item.PEPackageName, false); }
                                IsSearch = 0;
                                //设置收费类型
                                if (item.ID_FeeWay != "") {
                                    jQuery("#slFeeWay [value='" + item.ID_FeeWay + "']").attr("selected", true);
                                }

                            }
                            //如果是团体新增登记或者查看登记内容都需要绑定其套餐等内容
                            //设置选中套餐
                        }

                        IsSearchIDCustomer = -1;
                    }

                    );
                }
                else {
                    //未检索到此人的团体信息
                    //ShowSystemDialog("对不起,此客户为个人用户，请到个人登记处");
                    //修改提示信息
                    ShowSystemDialog("对不起,该客户没有 [未完成登记的团队备单信息]！<br/>(1).请通过体检号查询。<br/>(2).到个人登记处登记。");
                    return false;
                }
                //设置预约、登记时间
                if (jQuery.trim(jQuery("#txtSubScribDate").val()) == "") {
                    jQuery("#txtSubScribDate").val(CurDate);
                }

                //个人新增登记、预约毋须绑定收费项目
                if (jQuery("#didTeam").data("ID_Team") != undefined) {
                    if (msg.dataList1.length > 0) {
                        return false; ReadBustFeeData(Is_Org, msg.dataList1);
                    }
                }
            }
        });
    }
}

function RequestCustomerInfo(objID, Key, IsLoadCustomerInfo) {

    /********判断是否是从自定义身份证读取,如果是则需要后天通过身份证和姓名同时检索Begin**********/
    var IsGenerateCustomerCard = 0;
    var data = jQuery("#txtSearchX", parent.document).data("data");
    if (data != null) {
        IsGenerateCustomerCard = data.IsGenerateCustomerCard;
    }
    /********判断是否是从自定义身份证读取,如果是则需要后天通过身份证和姓名同时检索End**********/
    var CustomerName = jQuery.trim(jQuery("#spanCustomerName").text());
    var modelName = jQuery("#modelName").val();
    var KeyValue = jQuery.trim(jQuery("#" + objID).val());
    //组建请求参数
    var Is_Org = 0;
    var data = { action: "GetCustomerByIDCardX", IsGenerateCustomerCard: IsGenerateCustomerCard, Key: Key, KeyValue: KeyValue, CustomerName: encodeURIComponent(CustomerName), type: type, modelName: modelName };
    jQuery.ajax({
       // async: false,
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: data,
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }

            jQuery("#lblErrorMessage").text("获取到用户信息,正在整理数据...");
            //屏蔽此处，由于检索后是在获取到客户体检信息时才触发套餐变动事件
            //SetControlDefalut();
            //设置头像
            //            jQuery("#HeadImg").attr("src", defalutImagSrc);
            //            jQuery("#divBox").attr("src", defalutImagSrc); 
            //            jQuery("#HeadImg").attr("Base64Photo", "");

            //绑定用户基本信息
            jQuery("#spanCustomerName").text("");
            jQuery("#txtYKT").val("");
            jQuery("#slSex").val("");
            jQuery("#slMarried").val("");
            jQuery("#txtBirthDay").val("");
            jQuery("#txtMobil").val("");
            jQuery("#txtEmail").val("");
            var msgLength = msg.dataList.length;

            //存在用户基本信息
            //如果客户信息大于两条则提示操作者选择客户信息后绑定其基本信息
            if (msgLength > 0) {
                //只存在一个客户信息
                if (msgLength == 1) {
                    //如果是证件号检索并且是团体客户这里需要通过证件号查询客户是否存在多个未打印指引单的体检信息
                    if (IsTeam == 1)//如果当前模块是团体登记模块
                    {
                        if (Key == "IDCard")//如果是通过证件号检索
                        {
                            //查询该证件号对应的所有体检信息，如果存在多个则提示，只存在一个则直接进行设置
                            var allCustomerExamInfoDT = GetCustPhysicalExamInfoByIDCard(msg.dataList[0].IDCard, msg.dataList[0].CustomerName, IsTeam);
                            var dataList = allCustomerExamInfoDT.dataList;
                            //如果只存在单个体检信息，则直接绑定    
                            if (dataList.length == 1) {
                                var oldValue = jQuery("#txtSearchX", parent.document).val();
                                jQuery("#txtSearchX", parent.document).val(dataList[0].ID_Customer);     //设置检索区域值为当前体检号
                                RequestCustomerInfo("txtSearchX", "ID_Customer", 1);    //通过检索区域值检索客户体检信息
                                jQuery("#txtSearchX", parent.document).val(oldValue);
                            }
                            else if (dataList.length > 1) {
                                ChooseCustomerPhysicalExamInfo(dataList);
                            }
                            else {
                                SetCustomerInfo(msg.dataList, '', IsLoadCustomerInfo);
                            }
                            return false;
                        }
                    }
                    SetCustomerInfo(msg.dataList, '', IsLoadCustomerInfo);

                }
                //存在相同身份证多个客户的情况
                else {
                    ChooseCustomerInfo(msg.dataList);
                }
            }
            else {
                var ExamState = "";
                //进行归档验证
                if (Key == "ID_Customer") {
                    var data = { action: "GetCustomerByIDCustomerOfOnline", IsLoadCustomerInfo: 0, ID_Customer: KeyValue, currenttime: encodeURIComponent(new Date()) };
                    jQuery.ajax({
                        type: "GET",
                        async: false,
                        url: "/Ajax/AjaxRegiste.aspx",
                        data: data,
                        cache: false,
                        dataType: "json",
                        success: function (ExamStateMsg) {
                            if (ExamStateMsg.ExamState != undefined) {
                                ExamState = ExamStateMsg.ExamState;
                            }
                            else if (ExamStateMsg.dataList0 != undefined) {
                                ExamState = ExamStateMsg.dataList0.ExamState;
                            }
                        }
                    });
                }
                //进行归档验证      

                if (ExamState == "-1") {
                    ShowSystemDialog("对不起，该信息不存在！");
                } else if (ExamState != 0 && ExamState != "")//状态不为0则表明此数据不在生产库上
                {
                    ShowSystemDialog("对不起，该数据已归档！");
                }
                else {
                    if (IsFromGenerateCustomerCard == 0) {
                        ShowSystemDialog("对不起,未检索到客户体检信息");
                    }
                }
                SetGenerateCustomerCard();
               
            }
            //个人新增或者登记毋须绑定收费项目信息

            if (msg.dataList1 != null) {
                if (msg.dataList1.length > 0) {
                    ReadBustFeeData(Is_Org, msg.dataList1);
                }
            }
            jQuery("#lblErrorMessage").text("");
            jQuery("#txtSearchX", parent.document).removeData("data"); //移除自定义身份证生成标记
            return false;
        }
    });

    return false;

}

/****************缴费通知单、记账通知单 Begin********************/

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

///
///feeTypeName:收费类型：收费通知、记账通知
///operType：操作类型:View或者Edit
///ID_Customer：体检号
///AllIDFee：体检号对应的所有收费项目ID
function PrintCust(feeTypeName, operType, jsonMsg, AllIDFee) {
    //如果客户基本信息不存在，不能进行补打
    //判断是否存在客户名称
    var CustomerName = jQuery.trim(jQuery("#spanCustomerName").text());
    if (CustomerName == "") {
        ShowSystemDialog("对不起，客户基本信息不存在，请先维护！");
        return false;
    }
    //如果没有项目则不允许打印
    var objBusList = jQuery("#tblList tr[name='busList']");
    if (objBusList.length == 0) {
        ShowSystemDialog("请您添加体检项目！");
        return false;
    }
    if (!IsSaved) {
        ShowSystemDialog("请完成登记后再进行补打！");
        return false;
    }
    var Is_FeeSettled = "";
    if (jsonMsg != undefined) {
        if (jsonMsg.Is_FeeSettled != undefined) {
            Is_FeeSettled = jsonMsg.Is_FeeSettled;
        }
    }
    var SubScribDate = jQuery.trim(jQuery("#txtSubScribDate").val()); //客户预约、登记时间
    var ID_Customer = jQuery.trim(jQuery("#txtTJH").val());
    var ID_CustomerCode128 = jQuery.trim(jQuery("#lblCode128c").text());
    var sumData = jQuery("#divSumHeader").data("sumData");
    var customerName = jQuery.trim(jQuery("#spanCustomerName").text());
    var totalCustFee = 0;

    if (ID_Customer == "") {
        ShowSystemDialog("请完成登记后再进行补打！");
        return false;
    }
    //如果是正常打印，则只打印未打印的应交费用 ，补打则直接打印未缴费的所有应交费用
    if (feeTypeName == 0) //补打
    {
        totalCustFee = sumData.yjfy; //sumData.xjzf;

    }
    else//正常打印
    {
        totalCustFee = sumData.sumUnPrintyjfy; //调整成只打印未打印的应交费用
    }
    var upperWrite = chineseNumber(totalCustFee); //转换为大写
    var departmentName = "健康管理中心";
    var Date = CurDate; //页面公共变量中存放着当前服务器日期
    var Gender = document.getElementById("slSex").options[document.getElementById("slSex").selectedIndex].value; //性别
    //是否是预约
    var TemplateName = "CustomerCharges.frx";

    var IsCanShowSaveMsg = true; //是否可以弹出保存信息，如果没有打印报表或者没有其它提示的情况下显示保存信息jQuery("[name='btnAdd']").data("ShowMsg", ShowMsg);

    var para = {
        TemplateName: TemplateName,
        customerName: customerName,
        totalCustFee: totalCustFee,
        upperWrite: upperWrite,
        departmentName: departmentName,
        Date: Date,
        SubScribDate: SubScribDate
    };

    //获取打印数据
    var Data = IsCanPrint(ID_Customer).dataList[0];
    //判断是否是团体
    var Is_Subscribed = Data.Is_Subscribed;             //是否预约 1：是，0：否
    var ID_Team = Data.ID_Team;                         //团体ID
    var CustomerSubScribDate = Data.SubScribDate;       //客户体检时间
    var TaskExamStartDate = Data.TaskExamStartDate; //团体任务起始时间
    var TaskExamEndDate = Data.TaskExamEndDate;     //团体任务起始时间
    //如果已经完成收费，并且没有打印过指引单，则直接打印 xmhuang 2013-09-29
    if (Is_FeeSettled == "True") {
        if (ID_Team != "")                                  //如果是团体成员，则判断时间是否在时间段内
        {
            if (Date >= TaskExamStartDate && Date <= TaskExamEndDate)// 判断当前时间是否在备单时间段内
            {
                if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                    UpdateCustomerSubscribDateOfTeam_Ajax(ID_Customer, Date); //如果是团体客户在任务时间范围内完成登记，则需要修改实际体检时间为登记时间
                    IsCanShowSaveMsg = false; FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                    SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); ////团体完成登记后不修改预约体检时间，报告上所有的体检时间都以指引单打印时间为准 打印完指引单后向接口发送信息                   
                }
            }
            else {
                IsCanShowSaveMsg = false; ShowSystemDialog("客户所在团体预约体检时间为：[开始：" + TaskExamStartDate + "，结束：" + TaskExamEndDate + "]目前不能打印指引单！");
            }
        }
        else {
            //判断是登记还是预约
            if (Is_Subscribed == 1)                         //如果是预约，则需要判断时间是否为预约体检时间
            {
                if (Date == CustomerSubScribDate)           //判断当前时间是否和预约体检时间相等
                {
                    //该客户已经进行过登记或预约，有为收费项目则需要先打印收费单，没有收费项目则直接打印增项指引单
                    //判断是否存在非团体为收费的增项项目
                    if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                        //ShowSystemDialog("正在打印指引单，请稍候...");
                        IsCanShowSaveMsg = false; FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                        SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); ////团体完成登记后不修改预约体检时间，报告上所有的体检时间都以指引单打印时间为准 打印完指引单后向接口发送信息  
                    }
                }
                else {
                    IsCanShowSaveMsg = false; ShowSystemDialog("客户预约体检时间为:[" + CustomerSubScribDate + "]目前不能打印指引单！");
                }
            }
            else if (Is_Subscribed == 0)//如果是登记，则判断登记时间和当前时间是否相同，相同则可用直接打印指引单
            {
                if (Date == CustomerSubScribDate) {
                    //该客户已经进行过登记或预约，有为收费项目则需要先打印收费单，没有收费项目则直接打印增项指引单
                    //判断是否存在非团体为收费的增项项目
                    if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                        IsCanShowSaveMsg = false; FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                        SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); //团体完成登记后不修改预约体检时间，报告上所有的体检时间都以指引单打印时间为准 打印完指引单后向接口发送信息 
                    }
                }
                else {
                    IsCanShowSaveMsg = false; ShowSystemDialog("客户登记体检时间为:[" + CustomerSubScribDate + "]目前不能打印指引单！");
                }
            }
        }
    }
    //如果已经完成收费，并且没有打印过指引单，则直接打印
    else {
        //打印收费单
        if (feeTypeName == 0) {
            //判断如果未打印指引单，且交费金额为0
            if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                IsCanShowSaveMsg = false;
                if (para.totalCustFee > 0) {

                    //新增扩展报告内容（体检地点、尊称）
                    var curIP = convertIP(jQuery("#ClientIP").val());
                    var ExamPlaceName = IPParameters.本部.ExamPlaceName;
                    if (parseFloat(convertIP(IPParameters.二部.BeginIP)) <= parseFloat(curIP) && parseFloat(curIP) <= parseFloat(convertIP(IPParameters.二部.EndIP))) {
                        ExamPlaceName = IPParameters.二部.ExamPlaceName;
                    }
                    var sex = jQuery('#lblHidSex').text();
                    var GenderName = "同志";
                    if (sex == "男") {
                        GenderName = "先生";
                    }
                    else if (sex == "女") {
                        GenderName = "女士";
                    }
                    var detailXml = "<CustomerInfo>";
                    detailXml += "<ID_Customer>" + CheckXmlChar(ID_Customer) + "</ID_Customer>";
                    detailXml += "<ID_CustomerCode128>" + CheckXmlChar(ID_CustomerCode128) + "</ID_CustomerCode128>";
                    detailXml += "<TemplateName>" + CheckXmlChar(para.TemplateName) + "</TemplateName>";
                    detailXml += "<FeeTypeName>" + "收费" + "</FeeTypeName>";
                    detailXml += "<CustomerName>" + CheckXmlChar(para.customerName) + "</CustomerName>";
                    detailXml += "<TotalCustFee>" + para.totalCustFee + "</TotalCustFee>";
                    detailXml += "<UpperWrite>" + CheckXmlChar(para.upperWrite) + "</UpperWrite>";
                    detailXml += "<SubScribDate>" + CheckXmlChar(para.SubScribDate) + "</SubScribDate>";
                    detailXml += "<Operator>" + CheckXmlChar(UserName) + "</Operator>";
                    detailXml += "<ServerDate>" + CheckXmlChar(CurDate) + "</ServerDate>";
                    detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
                    detailXml += "<ExamPlaceName>" + CheckXmlChar(ExamPlaceName) + "</ExamPlaceName>";
                    detailXml += "</CustomerInfo>";
                    FastReport.GenerateCustomerCharges(ID_Customer, ID_CustomerCode128, para.TemplateName, "收费", para.customerName, para.totalCustFee, para.upperWrite, para.SubScribDate, UserName, CurDate, detailXml);
                }
                else {
                    IsCanShowSaveMsg = true;
                }
                UpdateCustFeePrintFlag(ID_Customer, AllIDFee); //新增打印完交费通知单后修改收费项目打印标记为已打，已保证分批打印的正确性

            }
            else {
                //            if (para.totalCustFee > 0) {
                IsCanShowSaveMsg = false;
                if (para.totalCustFee > 0) {

                    //新增扩展报告内容（体检地点、尊称）
                    var curIP = convertIP(jQuery("#ClientIP").val());
                    var ExamPlaceName = IPParameters.本部.ExamPlaceName;
                    if (parseFloat(convertIP(IPParameters.二部.BeginIP)) <= parseFloat(curIP) && parseFloat(curIP) <= parseFloat(convertIP(IPParameters.二部.EndIP))) {
                        ExamPlaceName = IPParameters.二部.ExamPlaceName;
                    }
                    var sex = jQuery('#lblHidSex').text();
                    var GenderName = "同志";
                    if (sex == "男") {
                        GenderName = "先生";
                    }
                    else if (sex == "女") {
                        GenderName = "女士";
                    }
                    var detailXml = "<CustomerInfo>";
                    detailXml += "<ID_Customer>" + CheckXmlChar(ID_Customer) + "</ID_Customer>";
                    detailXml += "<ID_CustomerCode128>" + CheckXmlChar(ID_CustomerCode128) + "</ID_CustomerCode128>";
                    detailXml += "<TemplateName>" + CheckXmlChar(para.TemplateName) + "</TemplateName>";
                    detailXml += "<FeeTypeName>" + "收费" + "</FeeTypeName>";
                    detailXml += "<CustomerName>" + CheckXmlChar(para.customerName) + "</CustomerName>";
                    detailXml += "<TotalCustFee>" + para.totalCustFee + "</TotalCustFee>";
                    detailXml += "<UpperWrite>" + CheckXmlChar(para.upperWrite) + "</UpperWrite>";
                    detailXml += "<SubScribDate>" + CheckXmlChar(para.SubScribDate) + "</SubScribDate>";
                    detailXml += "<Operator>" + CheckXmlChar(UserName) + "</Operator>";
                    detailXml += "<ServerDate>" + CheckXmlChar(CurDate) + "</ServerDate>";
                    detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
                    detailXml += "<ExamPlaceName>" + CheckXmlChar(ExamPlaceName) + "</ExamPlaceName>";
                    detailXml += "</CustomerInfo>";
                    FastReport.GenerateCustomerCharges(ID_Customer, ID_CustomerCode128, para.TemplateName, "收费", para.customerName, para.totalCustFee, para.upperWrite, para.SubScribDate, UserName, CurDate);
                }
                UpdateCustFeePrintFlag(ID_Customer, AllIDFee); //新增打印完交费通知单后修改收费项目打印标记为已打，已保证分批打印的正确性
                // }
            }
            //feeTypeName = "收费";
            //ShowDialogX("/template/blue/System/Report/RP_Registe_CustFee.htm?operType=" + operType + "&feeTypeName=" + feeTypeName + "&customerName=" + customerName + "&totalCustFee=" + totalCustFee + "&upperWrite=" + upperWrite + "&departmentName=" + departmentName + "&ID_Customer=" + ID_Customer + "&Date=" + Date, "我是弹出窗口", 650, 350, '我是弹出窗口');
        }
        else if (feeTypeName == 1) {
            feeTypeName = "收费";
            //var data = jQuery("#btnPrintCust1").data("data");
            //        var Is_GuideSheetPrinted = data.Is_GuideSheetPrinted;
            //        var Is_Subscribed = data.Is_Subscribed;
            //        var yyDate = SubScribDate; //  jQuery.trim(jQuery("#txtSubScribDate").val());
            //        var todayDate = CurDate; //  RegisteDateX; //客户登记是注册的预约、登记时间，只有到了这天方能打印指引单
            if (jQuery("#tblList tbody tr[id_teamtaskgroup=''][feechargestaute='']").length > 0) {
                IsCanShowSaveMsg = false;
                if (para.totalCustFee > 0) {

                    //新增扩展报告内容（体检地点、尊称）
                    var curIP = convertIP(jQuery("#ClientIP").val());
                    var ExamPlaceName = IPParameters.本部.ExamPlaceName;
                    if (parseFloat(convertIP(IPParameters.二部.BeginIP)) <= parseFloat(curIP) && parseFloat(curIP) <= parseFloat(convertIP(IPParameters.二部.EndIP))) {
                        ExamPlaceName = IPParameters.二部.ExamPlaceName;
                    }
                    var sex = jQuery('#lblHidSex').text();
                    var GenderName = "同志";
                    if (sex == "男") {
                        GenderName = "先生";
                    }
                    else if (sex == "女") {
                        GenderName = "女士";
                    }
                    var detailXml = "<CustomerInfo>";
                    detailXml += "<ID_Customer>" + CheckXmlChar(ID_Customer) + "</ID_Customer>";
                    detailXml += "<ID_CustomerCode128>" + CheckXmlChar(ID_CustomerCode128) + "</ID_CustomerCode128>";
                    detailXml += "<TemplateName>" + CheckXmlChar(para.TemplateName) + "</TemplateName>";
                    detailXml += "<FeeTypeName>" + "收费" + "</FeeTypeName>";
                    detailXml += "<CustomerName>" + CheckXmlChar(para.customerName) + "</CustomerName>";
                    detailXml += "<TotalCustFee>" + para.totalCustFee + "</TotalCustFee>";
                    detailXml += "<UpperWrite>" + CheckXmlChar(para.upperWrite) + "</UpperWrite>";
                    detailXml += "<SubScribDate>" + CheckXmlChar(para.SubScribDate) + "</SubScribDate>";
                    detailXml += "<Operator>" + CheckXmlChar(UserName) + "</Operator>";
                    detailXml += "<ServerDate>" + CheckXmlChar(CurDate) + "</ServerDate>";
                    detailXml += "<GenderName>" + CheckXmlChar(GenderName) + "</GenderName>";
                    detailXml += "<ExamPlaceName>" + CheckXmlChar(ExamPlaceName) + "</ExamPlaceName>";
                    detailXml += "</CustomerInfo>";
                    FastReport.GenerateCustomerCharges(ID_Customer, ID_CustomerCode128, para.TemplateName, "收费", para.customerName, para.totalCustFee, para.upperWrite, para.SubScribDate, UserName, CurDate, detailXml);
                }
                else {
                    IsCanShowSaveMsg = true;
                }
                UpdateCustFeePrintFlag(ID_Customer, AllIDFee); //新增打印完交费通知单后修改收费项目打印标记为已打，已保证分批打印的正确性
                // ShowDialogX("/template/blue/System/Report/RP_Registe_CustFee.htm?operType=" + operType + "&feeTypeName=" + feeTypeName + "&customerName=" + customerName + "&totalCustFee=" + totalCustFee + "&upperWrite=" + upperWrite + "&departmentName=" + departmentName + "&ID_Customer=" + ID_Customer + "&Date=" + Date, "我是弹出窗口", 650, 350, '我是弹出窗口');
            }
            else {
                if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {

                    if (ID_Team != "")                                  //如果是团体成员，则判断时间是否在时间段内
                    {

                        if (Date >= TaskExamStartDate && Date <= TaskExamEndDate)// 判断当前时间是否在备单时间段内
                        {
                            if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                                UpdateCustomerSubscribDateOfTeam_Ajax(ID_Customer, Date); //如果是团体客户在任务时间范围内完成登记，则需要修改实际体检时间为登记时间
                                IsCanShowSaveMsg = false; FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                                SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); ////团体完成登记后不修改预约体检时间，报告上所有的体检时间都以指引单打印时间为准 打印完指引单后向接口发送信息  
                            }
                        }
                        else {
                            IsCanShowSaveMsg = false; ShowSystemDialog("客户所在团体预约体检时间为：[开始：" + TaskExamStartDate + "，结束：" + TaskExamEndDate + "]目前不能打印指引单！");
                        }
                    }
                    else {
                        //判断是登记还是预约
                        if (Is_Subscribed == 1)                         //如果是预约，则需要判断时间是否为预约体检时间
                        {
                            if (Date == CustomerSubScribDate)           //判断当前时间是否和预约体检时间相等
                            {
                                //该客户已经进行过登记或预约，有为收费项目则需要先打印收费单，没有收费项目则直接打印增项指引单
                                //判断是否存在非团体为收费的增项项目
                                if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                                    //ShowSystemDialog("正在打印指引单，请稍候...");
                                    IsCanShowSaveMsg = false; FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                                    SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); ////团体完成登记后不修改预约体检时间，报告上所有的体检时间都以指引单打印时间为准 打印完指引单后向接口发送信息  
                                }
                            }
                            else {
                                IsCanShowSaveMsg = false; ShowSystemDialog("客户预约体检时间为:[" + CustomerSubScribDate + "]目前不能打印指引单！");
                            }
                        }
                        else if (Is_Subscribed == 0)//如果是登记，则判断登记时间和当前时间是否相同，相同则可用直接打印指引单
                        {
                            if (Date == CustomerSubScribDate) {
                                //该客户已经进行过登记或预约，有为收费项目则需要先打印收费单，没有收费项目则直接打印增项指引单
                                //判断是否存在非团体为收费的增项项目
                                if (jQuery("#tblList tbody tr[is_printed!='1']").length > 0) {
                                    IsCanShowSaveMsg = false; FastReport.GenerateCustomerGuide(UserID, UserName, ID_Customer, "guidesheet.frx", 0, 1);
                                    SendWaitToInterfaceByClient_Ajax(Gender, ID_Customer); ////团体完成登记后不修改预约体检时间，报告上所有的体检时间都以指引单打印时间为准 打印完指引单后向接口发送信息  
                                }
                            }
                            else {
                                IsCanShowSaveMsg = false; ShowSystemDialog("客户登记体检时间为:[" + CustomerSubScribDate + "]目前不能打印指引单！");
                            }
                        }
                    }
                }
                else {
                    //ShowSystemDialog("对不起，该用户已打印指引单，当前不能打印指引单...");
                }
            }
        }
    }
    if (IsCanShowSaveMsg) {
        ShowSystemDialog(jQuery("[name='btnAdd']").data("ShowMsg"));
    }
    //由于调整登记操作，这里完成登记后继续进行下一个申请登记操作

    if (type.toLocaleLowerCase() == "add") {
        if (operType == "Edit") {

        }
        else {
            AddNewRegistOrSign(0);
        }
    }
    else if (type.toLocaleLowerCase() == "edit") {
        ReBindCustomerBusFee();
    }
}

/****************缴费通知单、记账通知单 End********************/

function ChangeReadAndWrite(obj) {
    var checked = obj.checked;
    if (!checked) {
        jQuery("#txtSFZ").removeAttr("readonly");

        jQuery("#txtTJH").removeAttr("readonly");
        jQuery("#txtYKT").removeAttr("readonly");
    }
    else {
        jQuery("#txtSFZ").attr("readonly", "readonly");

        jQuery("#txtTJH").attr("readonly", "readonly");
        jQuery("#txtYKT").attr("readonly", "readonly");
    }
}
//设置身份证图片保存格式为jepg
try {
    SynCardOcx1.SetPhotoType(1);
} catch (e) { }

/*********************拍照 Begin**************************/
var photoContent = "";
var photoArt = "";
function StartCamera() {

    //    var content = '<div><object id="TakePhoto" classid="clsid:ea33a66e-f937-4d0d-aa91-8f6c917d0748" width="200"' +
    //            'height="220" codebase="http://192.172.0.120/ActiveX/FYHTakePhotoSetup.msi">' +
    //        '</object></div>';
    //    if (photoContent == "") {
    //        photoContent = jQuery("#divPhoto")[0].outerHTML;
    //        if (document.getElementById("TakePhoto") != undefined || document.getElementById("TakePhoto") != null) {
    //            document.getElementById("TakePhoto").CloseCam();
    //        }
    //        jQuery("#divPhoto").remove();
    //    }
    if (photoContent == "") {
        photoContent = '<div><object style="display:none;"id="TakePhoto" classid="clsid:ea33a66e-f937-4d0d-aa91-8f6c917d0748" width="200"' +
                    'height="220" codebase="http://192.172.0.120/ActiveX/FYHTakePhotoSetup.msi">' +
                '</object></div>';
    }

    var left = jQuery("#HeadImg").offset().left + jQuery("#HeadImg").width();
    var top = jQuery("#HeadImg").top; //  + document.documentElement.scrollTop;
    //var form = ShowDialogX("/takephoto/index.html", "我是弹出窗口", 650, 350, '我是弹出窗口');
    //photoArt = ""; //设置此值为空，以保证每次能够重新执行init中的代码保证正常启动视频
    if (photoArt == "") {
        photoArt = art.dialog({
            lock: true, opacity: 0.3, drag: false,
            //follow: document.getElementById("takePhoteFrame"),
            title: '视频拍照',
            content: photoContent,
            opacity: 0.3,
            init: function () {
                jQuery("#TakePhoto").hide();
                TakePhoto = document.getElementById("TakePhoto");
                TakePhoto.StartCamera();
                jQuery("#TakePhoto").show();
            },
            ok: function () {
                return DoTakePhoto();

            },

            okVal: "保存",
            cancelVal: '关闭',
            cancel: function () {
                //每次使用视频资源后就释放掉
                //                if (document.getElementById("TakePhoto") != undefined || document.getElementById("TakePhoto") != null) {
                //                    document.getElementById("TakePhoto").CloseCam();
                //                }
                //每次使用视频资源后就释放掉
                this.hide(); return false;
            },
            close: function () {
                //每次使用视频资源后就释放掉
                //                if (document.getElementById("TakePhoto") != undefined || document.getElementById("TakePhoto") != null) {
                //                    document.getElementById("TakePhoto").CloseCam();
                //                }
                //每次使用视频资源后就释放掉
                this.hide(); return false;
            }
        });
    }
    else {
        photoArt.show();
    }
}

function CloseCam() {

    //    try {
    //        TakePhoto.Exit();
    //    }
    //    catch (e) {
    //        
    //    }
}

function DoTakePhoto() {
    var CurBase64Photo = TakePhoto.TakePhoto();

    if (document.getElementById("HeadImg") == undefined) {
        return false;
    }
    if (CurBase64Photo == "") {
        ShowSystemDialog("获取图像失败,请确保您已安装视频驱动并且视频已正常连接到电脑!");
        return true;
    }
    jQuery("#HeadImg").attr("src", "data:image/gif;base64," + CurBase64Photo + "");
    jQuery("#divBox").attr("src", "data:image/gif;base64," + CurBase64Photo + "");
    jQuery("#HeadImg").attr("Base64Photo", CurBase64Photo);
    jQuery("#HeadImg").attr("IsExist", 1); //标记该头像已经存在，不用再重新设置    
    return true;
}
/*********************拍照 End**************************/



function JudgeIsHideQuickBox() {
    if (document.activeElement.id != "idSelectExamType") {
        ShowHideQuickQuerySetTable(false, "");      // 套餐
        ShowHideQuickQueryExamTypeTable(false, ""); // 体检类型
        ShowHideQuickQueryNationTable(false, "");   // 民族
        ShowHideQuickQueryUserTable(false, "");     // 导检护士
    }
}


// 隐藏所有的弹出窗口(通过注册焦点事件来隐藏弹出框)
function HideAllQuickTableEvent() {

    //input 获取焦点事件
    jQuery(".content input").focus(function () {
        var tempClass = jQuery("#" + document.activeElement.id).attr("class");

        if (tempClass != undefined) {
            if (tempClass != "QuickQueryShowBox" && tempClass.indexOf("btnQuickSure") < 0) {
                HideAllQuickTable();
            }
        } else {
            HideAllQuickTable();
        }
    });

    //select 获取焦点事件
    jQuery(".content select").focus(function () {
        var tempClass = jQuery("#" + document.activeElement.id).attr("class");
        // ShowSystemDialog(tempClass);
        if (tempClass != "QuickQueryShowBox") {
            HideAllQuickTable();
        }
    });

}

function HideAllQuickTable() {
    ShowHideQuickQuerySetTable(false, "");      // 套餐
    ShowHideQuickQueryExamTypeTable(false, ""); // 体检类型
    ShowHideQuickQueryNationTable(false, "");   // 民族
    ShowHideQuickQueryUserTable(false, "");     // 导检护士
}

/***************************扫描区域功能  Begin************************************/
/// <summary>
///重置收费项目汇总信息
/// </summary>
function ResetSumZJInfo() {
    //todaymark
    SumJG();

    /* 设置客户收费项目显示内容 Begin*/
    if (jQuery('#modelName').val().toLowerCase() == "regist") {
        jQuery("#divSumHeader label[name='kytjxm'").text("客户预约项目");
    }
    else if (jQuery('#modelName').val().toLowerCase() == "sign") {
        jQuery("#divSumHeader label[name='kytjxm'").text("客户登记项目");
    }
}
/// <summary>
///重置所有客户信息
/// </summary>
function ResetAllCustomerInfo() {
    /***************重置基础信息   Begin***************************/


    jQuery("#txtSFZ").val(""); //置空身份证
    jQuery("#lblSFZ").text(""); //置空身份证
    jQuery("#spanCustomerName").val(""); //置空姓名


    if (type.toLowerCase() != "add")//如果是新增状态扫描身份证，不能置空体检号
    {
        jQuery("#txtTJH").val(""); //置空体检号
    }
    jQuery("#txtYKT").val(""); //置空一卡通
    jQuery("#lblHidCustomer").text(""); //重置编辑状态下的体检号
    jQuery("#lblHidBirthDay").text(""); //重置编辑状态下出生日期
    jQuery("#slSex").val(-1); //重置编辑状态性别
    //    RemoveSelectedUser(); // 重置导检护士（如果是新增，则清空导检护士相应的数据）

    RemoveSelectedSet(); // 重置套餐（如果是新增，则清空套餐相应的数据）
    //ShowQuickSelectNation(1, "汉族"); // 新增时，默认为汉族 设置民族的已选项
    jQuery("#slCultrul").find("option:selected").attr("selected", false);
    jQuery("#slCultrul").find("option[value='-1']").attr("selected", true);
    jQuery("#s2id_slCultrul .select2-choice span").text(choiceCultrulText);
    /***************重置基础信息   End***************************/

    /***************重置收费项目信息   Begin***************************/
    jQuery("#tblList tbody tr[id!='loading']").remove(); //重置收费项目
    ShowQuickSelectExamType(1, "健康体检");    // 设置体检类型的已选项
    ResetSumZJInfo();

    jQuery("#txtMobil").val(""); //重置联系方式
    jQuery("[name='btnComplete']").show(); //已完成总检的数据，不允许保存，这里需要重置

    jQuery("#lblHidCustomer").text(""); //置空体检号的值
    jQuery("#txtTJH").val(""); //置空体检号的值
    jQuery("#didTeam").html(""); //置空团体信息值

    RemoveSelectedUser();    // 设置导检护士的已选人员
    RemoveSelectedNation();          // 设置民族的已选项
    RemoveSelectedSet();    // 设置体检类型的已选项
    RemoveSelectedExamType(); //设置体检类型
    ShowQuickSelectExamType(1, "健康体检");    // 设置体检类型的已选项
    jQuery("#txtSetInputCode").val(""); //重置输入的套餐名称
    jQuery("#txtUserInputCode").val(""); //重置输入的导检护士名称
    jQuery("#txtExamTypeInputCode").val(""); //重置输入的体检类型名称
    jQuery("#txtNationInputCode").val(""); //重置输入的所属民族名称

    jQuery("#slMarried [value='-1']").attr("selected", true); //设置婚姻为请选择
    jQuery("#slSex [value='-1']").attr("selected", true); //x设置性别为请选择
    jQuery("#slFeeWay [value='1']").attr("selected", true); //设置付费方式为现金
    jQuery("#slOperateLevel [value='1']").attr("selected", true); //设置用户密级为一级

    jQuery("#txtBirthDay").val(""); //重置出生日期
    jQuery("#HeadImg").attr("src", defalutImagSrc); //重置头像 
    jQuery("#divBox").attr("src", defalutImagSrc);  
    jQuery("#HeadImg").attr("Base64Photo", ""); //重置头像Base64Photo属性
    jQuery("#lblAge").text(""); //重置年龄显示

    //设置团体关联label为隐藏状态

    jQuery(".sapnTeamTitle").hide();

    jQuery("#spanTeamName").text(""); //置空单位名称
    jQuery("#spanDepart").text("");  //置空部门名称
    jQuery("#spanRoleName").text(""); //置空角色名称

    //设置预约登记日期可见
    jQuery("#txtSubScribDate").show();
    jQuery("#lblSubScribDate").hide();
    /***************重置收费项目信息   End***************************/
}
/// <summary>
///设置客户预约还是登记显示信息
/// </summary>
function SetCustomerSubscribedInfo(Is_Subscribed) {
    if (Is_Subscribed == 1)//预约客户
    {
        //jQuery("#lblRegiste").text("预约时间");//注释掉，统一显示为体检日期
        jQuery("#lblRegiste").text("体检日期");

        jQuery("[name='btnComplete']").val(" 完 成(F9) ");   // 完成预约
        jQuery("[name='btnAdd']").val(" 申 请 ");        // 申请新客户预约
        jQuery("[name='btnComplete']").attr("title", " 完成预约 ");     // 完成预约
        jQuery("[name='btnAdd']").attr("title", " 申请新客户预约 ");    // 申请新客户预约
    }
    else if (Is_Subscribed == 0)//登记客户
    {
        //jQuery("#lblRegiste").text("登记时间");//注释掉，统一显示为体检日期
        jQuery("#lblRegiste").text("体检日期");


        jQuery("[name='btnComplete']").val(" 完 成(F9) ");   // 完成登记
        jQuery("[name='btnAdd']").val(" 申 请 ");        // 申请新客户登记
        jQuery("[name='btnComplete']").attr("title", "完 成(F9)");  // 完成登记
        jQuery("[name='btnAdd']").attr("title", " 申请新客户登记 "); // 申请新客户登记

    }
}

function GetCustomerInfo(ID_Customer) {
    var allISDeleteDT = "";
    var flag = false;
    jQuery.ajax({
        type: "GET",
        async: false,
        url: "/Ajax/AjaxRegiste.aspx",
        data: { action: "GetCustomerInfo", ID_Customer: ID_Customer },
        cache: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            if (msg.dataList.length > 0) {
                allISDeleteDT = msg.dataList[0];
            }
        }
    });
    return allISDeleteDT;
}

var IsSearch = 0;
var IsSearchIDCustomer = 0;
/*
如果是团体通过证件号检索体检信息存在多个体检号需要提示 
*/
function SearchCardAndCustomer() {
    var card = jQuery.trim(jQuery("#txtSearchX", parent.document).val());                    //获取体检号或证件号
    IsFromGenerateCustomerCard = 0;
    jQuery("#btnReadFromMachine").data("ReadCustomerFromCard", 0);          //重置来源于设备读取标记
    if (isCustomerExamNo(card))//如果是体检号
    {
        jQuery("#HeadImg").attr("IsExist", 0); //设置图片标记为0以便从服务器检索图片绑定，如果为1则表示是读取证件和拍照，不再从服务器读取数据
        var ErrorMsg = "";
        var TypeCode = card.substring(1, 2);                                //TypeCode:3 个人预约 ,6个人登记 ,9团体登记
        if (IsCommon == 1)//如果是通用功能，则不验证是个人预约、个人登记、团体登记
        {
            var allISDeleteDT = GetCustomerInfo(card);
            //判断是否是对应模块的客户(是个人还是团体)
            if (allISDeleteDT == "" || allISDeleteDT.length == 0) {
                ShowSystemDialog("对不起,系统未找到体检号[" + card + "]对应的信息!");
                return false;
            }
            else {
                IsSearch = 1;
                IsSearchIDCustomer = 1;
                ResetAllCustomerInfo(); //xmhuang 20140424 注释此方法在后面已执行，此处不单独调用
                jQuery("#idSelectSet").data("ID_Customer", card); //绑定体检号
                jQuery("#tblList tbody").empty(); //重置收费项目
                //SearchCustomerInfo("txtSearchX", "ID_Customer");
            }
        }
        else {
            if (TypeCode == 3)//个人预约
            {
                if (IsTeam == 1)//如果当前模块是团体登记模块
                {
                    ErrorMsg = "对不起，体检号[" + card + "]为个人预约客户，请到个人登记处";
                }
                else {

                }
            }
            else if (TypeCode == 6)//个人登记
            {
                if (IsTeam == 1)//如果当前模块是团体登记模块
                {
                    ErrorMsg = "对不起，体检号[" + card + "]为个人登记客户，请到个人登记处";
                }
                else {
                    if (jQuery('#modelName').val().toLowerCase() == "regist") {
                        ErrorMsg = "对不起，体检号[" + card + "]为个人登记客户，请到个人登记处";
                    }
                    else if (jQuery('#modelName').val().toLowerCase() == "sign") {
                    }
                }
            }
            else if (TypeCode == 9)//团体登记
            {
                if (IsTeam == 1)//如果当前模块是团体登记模块
                { }
                else {
                    ErrorMsg = "对不起，体检号[" + card + "]为团体客户，请到团体登记处";
                }
            }
            else {
                var allISDeleteDT = GetCustomerInfo(card);
                //判断是否是对应模块的客户(是个人还是团体)
                if (allISDeleteDT == "" || allISDeleteDT.length == 0) {
                    ShowSystemDialog("对不起,系统未找到体检号[" + card + "]对应的信息!");
                    return false;
                }
                else {
                    ErrorMsg = ""; //充值错误信息                         
                    //如果是团体客户
                    if (allISDeleteDT.Is_Team == "True") {
                        if (IsTeam == 1)//如果当前模块是团体登记模块
                        { }
                        else {
                            ErrorMsg = "对不起，体检号[" + card + "]为团体客户，请到团体登记处";
                        }
                    }
                    else {
                        if (IsTeam == 1)//如果当前模块是团体登记模块
                        {
                            ErrorMsg = "对不起，体检号[" + card + "]为个人客户，请到个人登记处";
                        }
                        else {

                        }
                    }
                    if (ErrorMsg != "") {
                        jQuery("#txtSearchX", parent.document).val(""); //清空身份证号码
                        jQuery("#txtSearchX", parent.document).focus();
                        jQuery("#txtSearchX", parent.document).select();
                        ShowSystemDialog(ErrorMsg);
                        ErrorMsg = "";
                        return false;
                    }
                    else {
                        IsSearch = 1;
                        IsSearchIDCustomer = 1;
                        ResetAllCustomerInfo();
                        jQuery("#idSelectSet").data("ID_Customer", card); //绑定体检号
                        jQuery("#tblList tbody").empty(); //重置收费项目
                        SearchCustomerInfo("txtSearchX", "ID_Customer");
                    }
                }
            }
        }
        if (ErrorMsg != "") {
            jQuery("#txtSearchX", parent.document).val(""); //清空身份证号码
            jQuery("#txtSearchX", parent.document).focus();
            jQuery("#txtSearchX", parent.document).select();
            ShowSystemDialog(ErrorMsg);
            ErrorMsg = "";
            return false;
        }
        else {
            IsSearch = 1;
            IsSearchIDCustomer = 1;
            ResetAllCustomerInfo();
            jQuery("#idSelectSet").data("ID_Customer", card); //绑定体检号
            jQuery("#tblList tbody").empty(); //重置收费项目
            SearchCustomerInfo("txtSearchX", "ID_Customer");
        }

    }
    else if (isIDCardNo(card)) //如果身份证
    {
        //个人禁用身份证检索功能 讨论决定个人禁用身份证检索功能，值提供体检号检索功能，如果需要使用身份证检索功能只能在列表中进行检索
        if (IsTeam == 1) {
            IsSearch = 1;
            IsSearchIDCustomer = 1; //身份证和体检号都要检索最近一次体检信息
            ResetAllCustomerInfo();
            jQuery("#HeadImg").attr("IsExist", 0); //设置图片标记为0以便从服务器检索图片绑定，如果为1则表示是读取证件和拍照，不再从服务器读取数据
            SearchCustomerInfo("txtSearchX", "IDCard");
        }
        else {
            //个人登记、预约禁用身份证检索功能
        }
    }
    else {
        if (card == "") {
            //个人禁用身份证检索功能 讨论决定个人禁用身份证检索功能，值提供体检号检索功能，如果需要使用身份证检索功能只能在列表中进行检索
            if (IsTeam == 1) {
                //ShowSystemDialog("请您输入需要查询的体检号或证件信息！");
                IsSearch = 1;
                IsSearchIDCustomer = 1; // 身份证和体检号都要检索最近一次体检信息
                ResetAllCustomerInfo();
                SearchCustomerInfo("txtSearchX", "IDCard"); //从证件读取数据
            }
        }
        else {
            //体检号或证据号为空验证
            var curMsg = "";
            if (IsCommon == 1)//如果是通用功能，则不验证是个人预约、个人登记、团体登记
            {
                curMsg = "请输入正确的证件号或体检号进行查询!";
            }
            else {
                if (IsTeam == 1)//如果当前模块是团体登记模块
                {
                    curMsg = "请输入正确的证件号或体检号进行查询";
                }
                else {
                    curMsg = "请输入正确的体检号进行查询";
                }
            }
            ShowCallBackSystemDialog(curMsg, function () {
                jQuery("#txtSearchX", parent.document).focus();
                jQuery("#txtSearchX", parent.document).select();
            });
            return false;
        }
    }
    //    jQuery("#txtSearchX", parent.document).val(""); //清空身份证号码
    //    jQuery("#txtSearchX", parent.document).focus();
    //    jQuery("#txtSearchX", parent.document).select();
}

//修改内容：屏蔽当前操作为add时才能检索客户登记信息
function SearchCustomerInfo(objID, Key) {
    jQuery("#lblErrorMessage").text("");
    //判断是身份证检索还是体检号还是一卡通检索
    var KeyValue = jQuery.trim(jQuery("#" + objID).val());
    if (Key == "ID_Customer")//体检号检索
    {
        RequestCustomerInfo(objID, Key, 1);
    }
    else if (Key == "IDCard")//身份证检索
    {
        if (isIDCardNo(KeyValue))//如果身份证号满足要求，则直接通过身份证检索信息，其信息为客户最近一次登记（预约）信息，并且包含体检项目信息，此功能一般只允许在编辑状态下使用
        {
            if (type.toLowerCase() == "edit")//如果是修改状态则直接通过身份证号获取客户体检信息
            {
                RequestCustomerInfo(objID, Key, 1);
            }
            else//如果是新增状态,则直接从客户卡片上读取身份证信息
            {
                //FindReader(objID, Key); //从客户卡片上读取身份证，如果读取不到信息则从数据库中读取客户信息
                if (IsTeam == 1)//如果是团体成员则直接通过身份证检索其最近体检信息
                {
                    RequestCustomerInfo(objID, Key, 1); //由于当前身份证号存在，则不用从客户卡片上读取信息，直接进行检索，检索客户体检项目信息
                }
                else {
                    RequestCustomerInfo(objID, Key, 1); //调整身份证检索默认都检索最后一次体检信息 由于当前身份证号存在，则不用从客户卡片上读取信息，直接进行检索，但不检索客户体检项目信息
                }
            }
        }
        else {
            if (KeyValue == "") {
                FindReader(objID, Key, 1); //从客户卡片上读取身份证，如果读取不到信息则从数据库中读取客户信息
            }
        }
    }
    else if (Key == "ExamCard") //一卡通检索
    {
        if (KeyValue.length == 19) {
            RequestCustomerInfo(objID, Key, 1);
        }
        else {
            jQuery("#lblErrorMessage").text("请您输入正确的一卡通号！");
            jQuery("#" + objID).focus();
            jQuery("#" + objID).select();
        }
    }
}
var IsFromGenerateCustomerCard = 0;
/// <summary>
/// 生成自定义证件号
/// </summary>
function OpenUserNoIDCard() {
    ResetAllCustomerInfo();
    var tipscontent = '<ul>' +
'<div class="form-group" style="width: 100%"><div class="lable" style="width: 38%;float: left;">客户姓名：</div><div class="field" style="width: 60%;float: right;"><input maxlength="10" name="txtCustomerNameX" id="txtCustomerNameX" class="input" style="ime-mode:atuo;"/> &nbsp;</div></div>' +
'<div class="form-group" style="width: 100%"><div class="lable" style="width: 38%;float: left;">出生日期：</div><div class="field" style="width: 60%;float: right;"><input maxlength="10" name="txtCustomerBirthdayX" id="txtCustomerBirthdayX" class="input dateselect"/> &nbsp;</div></div>' +
'<div class="form-group" style="width: 100%"><div class="lable" style="width: 38%;float: left;">客户性别：</div><div class="field" style="width: 60%;float: right;"><select id="slSexx" class="input"><option value="1">男</option><option value="2">女</option></select></div></div>' +
'</ul>';
    art.dialog({
        id: 'OperWindowFrame',
        content: tipscontent,
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '生成自定义证件号',
        init: function () {
            jQuery("#txtCustomerNameX").focus();
        },
        button: [{
            name: '确定',
            callback: function () {
                IsFromGenerateCustomerCard = 1;
                var ISGenerateCustomerCard = GenerateCustomerCard(); //生成自定义证件号 
                //无证件确定后不允许修改出生日期
                jQuery("#txtBirthDay").hide();
                jQuery("#lblHidBirthDay").show();
                //jQuery("#lblHidBirthDay").html("asdsasafdsa");
                return ISGenerateCustomerCard;
            }, focus: true
        }, {
            name: '取消'
        }]
    });
    $(".dateselect").flatpickr();
}
/// <summary>
///生成自定义客户证件号
/// </summary>
function GenerateCustomerCard() {

    var CustomerName = jQuery("#txtCustomerNameX").val();
    var Birthday = jQuery("#txtCustomerBirthdayX").val();
    var Gender = document.getElementById("slSexx").options[document.getElementById("slSexx").selectedIndex].value; //性别
    var GenderName = document.getElementById("slSexx").options[document.getElementById("slSexx").selectedIndex].text; //性别
    if (CustomerName == "") {
        ShowSystemDialog("请您填写客户姓名！");
        jQuery("#txtCustomerNameX").focus();
        return false;
    }
    else if (Birthday == "") {
        ShowSystemDialog("请您填写出生日期！");
        //jQuery("#txtCustomerBirthdayX").select();
        return false;
    }
    //格式：111111(6位)+出生年月(8位)+"11"+性别+"1"
    var twoRandStr = "11";
    var oneRandStr = "1";
    //生成性别补位
    var Sex = 0;
    if (GenderName == "男") { Sex = 1; }
    else { Sex = 2; }
    var cardStr = "111111" + "" + Birthday.replace(/-/gi, "") + "" + twoRandStr + "" + Sex + "" + oneRandStr;
    var data = { IsGenerateCustomerCard: 1, CustomerName: CustomerName, IDCard: cardStr, GenderName: GenderName, Sex: Sex, Birthday: Birthday };
    jQuery("#txtSearchX", parent.document).data("data", data);
    jQuery("#txtSearchX", parent.document).val(cardStr);
    jQuery("#lblSFZ").text(cardStr);
    jQuery("#txtSFZ").text(cardStr); //设置身份信息
    jQuery("#spanCustomerName").val(CustomerName); //设置客户姓名
    RequestCustomerInfo("txtSearchX", "IDCard", 0); //从后台检索是否存在同身份证的客户,0：不加载客户体检号信息和客户收费项目信息 1：相反
    jQuery("#txtSearchX", parent.document).val(""); //清空身份证号码
    jQuery("#txtSearchX", parent.document).focus();
    jQuery("#txtSearchX", parent.document).select();
}
/// <summary>
///设置自定义客户证件信息
/// </summary>
function SetGenerateCustomerCard() {
    
    if (jQuery("#txtSearchX", parent.document).data("data") != undefined) {
        var data = jQuery("#txtSearchX", parent.document).data("data");
        var CustomerName = data.CustomerName;
        var IDCard = data.IDCard;
        var Sex = data.Sex;
        var GenderName = data.GenderName;
        var Birthday = data.Birthday;
        jQuery("#txtSearchX", parent.document).val("");
        jQuery("#lblSFZ").text(IDCard);
        jQuery("#txtSFZ").val(IDCard);       //设置身份信息
        jQuery("#slSex [forgender='" + Sex + "']").attr("selected", true);
        jQuery("#lblHidSex").val(GenderName);                                                                  //设置当前性别  
        jQuery("#spanCustomerName").val(CustomerName);                                                         //设置客户姓名     
        jQuery("#txtBirthDay").val(Birthday); jQuery("#lblHidBirthDay").text(Birthday);                         //设置出生日期
        CaltAgeByBirthDay();
        jQuery("#txtSearchX", parent.document).removeData("data");                                                               //移除数据项
    } else {
        console.log(123213213);
    }
}
/// <summary>
///扫描一卡通
/// </summary>
function InputExamCard() {
    var tipscontent = '<table class="ModifyPassword">' +
'<tbody>' +
'    <tr><td class="left">一卡通号：</td><td><input name="txtExamCard" id="txtExamCard" onkeyup="OnFormKeyUp();"/> &nbsp;</td></tr>' +
'</tbody>' +
'</table>';
    art.dialog({
        id: 'OperWindowFrame',
        content: tipscontent,
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '扫描一卡通',
        init: function () {
            document.getElementById("txtExamCard").focus();
            document.getElementById("txtExamCard").select();
        },
        button: [{
            id: "btnExamOK",
            name: '确定',
            callback: function () {
                var ISSend = SendExamInfoToHis(); //生成自定义证件号
                return ISSend;
            }, focus: true
        }, {
            name: '取消'
        }]
    });
}
function SetDisscount() {
    var curZK = jQuery.trim(jQuery("#txtXMZK").text());
    if (curZK != "") {
        curZK = parseFloat(curZK);
        if (curZK < DisCountRate) {
            curZK = DisCountRate;
        }
        if (curZK == 0) {
            curZK = 10;
        }
        if (curZK > 10) {
            curZK = 10;
        }
        jQuery("#txtXMZK").text(curZK);
        //遍历所有勾选项设置统一折扣
        jQuery("[name='ItemCheckbox']").each(function () {
            //                if (jQuery(this).parent().parent().attr('CustFeeChargeState') != "0") {
            if (jQuery(this).attr("checked")) {
                jQuery(this).parent().parent().find("[name = 'zk']").text(curZK); //设置折扣后价格
                jQuery(this).parent().parent().find("[name = 'zkr']").text(DisUserName); //设置折扣人
                jQuery(this).parent().parent().attr("id_discounter", DisUserID); //为tr保存折扣人ID
                jQuery(this).parent().parent().attr("discountername", DisUserName); //为tr保存折扣人名称
                //                    jQuery(this).parent().parent().find("[name = 'zkr']").text(UserName); //设置折扣人
                //                    jQuery(this).parent().parent().attr("id_discounter", UserID); //为tr保存折扣人ID
                //                    jQuery(this).parent().parent().attr("discountername", UserName); //为tr保存折扣人名称

            }
            //}
        });

    }
}
function OpenChangeUser() {
    var tipscontent = '<table class="ModifyPassword">' +
'<tbody>' +
'    <tr><td class="left">用户名：</td><td><input onkeyup="OnFormKeyUp();" style="width:120px;" maxlength="10" name="txtUserLoginName" id="txtUserLoginName"/> &nbsp;</td></tr>' +
'    <tr><td class="left">密码：</td><td><input type="password" style="width:120px;" onkeyup="OnFormKeyUp();" maxlength="20" name="txtUserPassword" id="txtUserPassword"/> &nbsp;</td></tr>' +
'<tr><td colspan="2" align="center"><span id="divUserLoginTips" style="color:red;">&nbsp;</span></td></tr>' +
'</tbody>' +
'</table>';
    art.dialog({
        id: 'OperWindowFrame',
        content: tipscontent,
        lock: true,
        fixed: true,
        opacity: 0.3,
        title: '获取用户折扣',
        init: function () {
            document.getElementById("txtUserLoginName").value = LastUserLoginName;
            document.getElementById("txtUserPassword").value = LastUserPassword;
            document.getElementById("txtUserLoginName").focus();
            document.getElementById("txtUserLoginName").select();
        },
        button: [{
            name: '确定',
            callback: function () {
                return ChangeUserDiscount();
            }, focus: true
        }, {
            name: '取消'
        }]
    });
}
var LastUserLoginName = "";
var LastUserPassword = "";
/// <summary>
///选择折扣人，设置折扣
/// </summary>
function ChangeUserDiscount() {


    //ChangeUser
    jQuery("#divUserLoginTips").html("");
    var flag = false;
    var UserLoginName = jQuery.trim(jQuery("#txtUserLoginName").val()); //用户名
    var UserPassword = jQuery.trim(jQuery("#txtUserPassword").val()); //密码
    if (UserLoginName == "") {
        jQuery("#divUserLoginTips").html("请输入用户名!");
        jQuery("#txtUserLoginName").focus();
        return false;
    }
    else if (UserPassword == "") {
        jQuery("#divUserLoginTips").html("请输入密码！");
        jQuery("#txtUserPassword").focus();
        return false;
    }
    jQuery("#divUserLoginTips").html("");
    var VerifyCode = ""; //验证码，暂时没有使用 $("#VerifyCode").val();
    jQuery.ajax({
        type: "GET",
        url: "/ajax/AjaxUser.aspx",
        data: { action: "ChangeUser", UserLoginName: UserLoginName, UserPassword: UserPassword, VerifyCode: VerifyCode, param: Math.random() },
        cache: false,
        async: false,
        dataType: "json",
        success: function (msg) {
            // 检查Ajax返回数据的状态等
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            if (msg.success == 0) {
                jQuery("#UserLoginName").focus();
                jQuery("#UserLoginName").select();
                jQuery("#UserPassword").val("");
                jQuery("#divUserLoginTips").html(msg.Message);
                jQuery("#txtUserLoginName").focus();
                jQuery("#txtUserLoginName").select();
                flag = false;
            }
            else if (msg.success == 1) {
                LastUserLoginName = UserLoginName;
                LastUserPassword = UserPassword;
                if (msg.DisCountRate == 0) {
                    msg.DisCountRate = 10;
                }
                DisCountRate = msg.DisCountRate;
                DisUserID = msg.UserID;
                DisUserName = msg.UserName;
                CloseDialogWindow();
                SetDisscount();
                flag = true;
            }
        }
    });
    return flag;
}

/// <summary>
///新增客户信息--原从设备读取
/// </summary>
function ReadCustomerFromCard() {
    jQuery("#btnReadFromMachine").data("ReadCustomerFromCard", 2);
    ResetAllCustomerInfo(); //重置
    FindReader('txtSearchX', 'IDCard', 2); //从设备读取并更新客户头像信息 0:不检索客户婚姻状况和联系电话信息 1：检索全部信息，包含体检信息 2：检索客户婚姻状况和联系电话信息
    jQuery("#txtSearchX", parent.document).val(""); //清空身份证号码
    jQuery("#txtSearchX", parent.document).focus();
    jQuery("#txtSearchX", parent.document).select();

    //读取身份证时不允许修改出生日期
    //    jQuery("#txtBirthDay").hide();
    //    jQuery("#lblHidBirthDay").show();
    jQuery("#txtSubScribDate").hide();
    if (jQuery("#lblHidCustomer").text() != "") {
        jQuery("#txtSubScribDate").hide(); jQuery("#lblSubScribDate").show();
    }
    else {
        jQuery("#txtSubScribDate").show(); jQuery("#lblSubScribDate").hide();
    }
}

/***************************扫描区域功能 ***********************************/
/// <summary>
/// 修改团体登记时间
/// </summary>
function UpdateCustomerSubscribDateOfTeam_Ajax(ID_Customer, SubScribDate) {
    return false; //无论是团体还是个人完成登记后不在修改预约体检时间
    //ajax请求：由于字符在get请求中超长，这里必须使用post方式提交请求
    //    jQuery.ajax({
    //        type: "POST",
    //        cache: false,
    //        url: "/Ajax/AjaxRegiste.aspx",
    //        data: { ID_Customer: ID_Customer, SubScribDate: SubScribDate, action: 'UpdateCustomerSubscribDateOfTeam' },
    //        dataType: "json",
    //        success: function (msg) {

    //        },
    //        complete: function () {

    //        }
    //    });
}
/// <summary>
/// 折扣变更后，超过10的重置为10
/// 修改内容：通过指定体检号获取其对应收费项目
function ChangeDiscount() {
    var Discount = $("#txtXMZK").text();
    if (!IsDecmal(Discount)) {
        Discount = 10;
    }
    if (Discount > 10) {
        Discount = 10;
        $("#txtXMZK").text(Discount);
    }
    //设置失去光标时自动计算折扣
    SetDisscount();
    SumJG(); //计算合计
}


function UpdateCustFeePrintFlag(ID_Customer, AllIDFee) {
    jQuery.ajax({
        type: "POST",
        url: "/Ajax/AjaxRegiste.aspx",
        data: { ID_Customer: ID_Customer, AllIDFee: AllIDFee, action: 'UpdateCustFeePrintFlag' },
        cache: false,
        contentType: "application/x-www-form-urlencoded;Content-length=1024000",
        dataType: "json",
        success: function (jsonMsg) {
        }
    });

}


/// 计算年龄
function CaltAgeByBirthDay() {
    var age = GetAgeByBirthDay(CurDate, jQuery("#txtBirthDay").val());
    jQuery("#lblAge").text(age + "岁");

}



/// 计算年龄 跳转到查询界面

function RedirectSearch() {
    var from = jQuery("#from").length == 0 ? "" : jQuery("#from").val();                       //获取是否团体参数
    from = decodeURIComponent(from);
    var modelName = jQuery("#modelName").val();                       //获取是否团体参数
    var IsTeam = jQuery("#IsTeam").val();                       //获取是否团体参数
    var IsCommon = jQuery("#IsCommon").val();                   //获取是否通用参数
    if (IsCommon == undefined)
    { IsCommon = 0; }
    var type = jQuery("#type").val();
    var url = "";
    if (from == "") {
        url = "/System/Customer/RegistList.aspx?modelName=@modelName&IsTeam=@IsTeam&IsCommon=@IsCommon&type=@type";
    }
    else {
        url = from + "?modelName=@modelName&IsTeam=@IsTeam&IsCommon=@IsCommon&type=@type";
    }
    url = url.replace(/@modelName/gi, modelName).replace(/@IsTeam/gi, IsTeam).replace(/@IsCommon/gi, IsCommon).replace(/@type/gi, type);
    DoLoad(url);
}

//鼠标显示缩略图
//移除登记界面鼠标显示图片功能
function ShowBigPic() {
    return false;
    var x = 10;
    var y = 10;
    jQuery(".box").mouseover(function (e) {

        ShowTestErrorMsg("调用 .box mouseover ShowBigPic() 函数（JS_RegistOper.js）");

        this.myTitle = this.title;
        this.title = "";
        var imgTitle;
        if (this.myTitle) {
            imgTitle = "<br />" + this.myTitle;
        }
        else {
            var CustomerName = jQuery("#spanCustomerName").text();
            var GenderName = jQuery("#lblHidSex").text();
            var MarriedName = "";
            var Age = jQuery("#lblAge").text();
            var Mobil = jQuery("#txtMobil").val();
            if (document.getElementById("slMarried").selectedIndex > 0) {
                MarriedName = document.getElementById("slMarried").options[document.getElementById("slMarried").selectedIndex].text;
            }
            imgTitle = CustomerName + " " + GenderName + " " + Age + " " + MarriedName;
            imgTitle += "<br/>" + Mobil;
         

        }
        var hdw = "<div id='two'><div><img width=140 height=160 src=" + this.src + " alt='hdw' /></div>" + imgTitle + "<\/div>";
        jQuery("body").append(hdw);
        jQuery("#two").css({
            "left": (e.pageX + x) + "px",
            "top": (e.pageY + y) + "px"
        }).show("fast");
    }).mouseout(function () {
        this.title = this.myTitle;
        jQuery("#two").remove();
        ShowTestErrorMsg("调用 .box mouseout ShowBigPic() 函数（JS_RegistOper.js）");
    }).mousemove(function (e) {
        ShowTestErrorMsg("调用 .box mousemove ShowBigPic() 函数（JS_RegistOper.js）");
        jQuery("#two").css({
            "left": (e.pageX + x) + "px",
            "top": (e.pageY + y) + "px"
        });
    });
}
/**
*         js中计算年龄 将生日转换成年龄
*/
function birthDayToAge() {
    var aDate = new Date();
    var thisYear = aDate.getFullYear();
    var thisMonth = aDate.getMonth() + 1;
    var thisDay = aDate.getDate();
    var currentDate = thisYear + "-" + thisMonth + "-" + thisDay;
    var oTable = document.getElementById('familyMember');
    var message = $("#message").html();
    if (message != null && message != "") {
        return null;
    }
    for (i = 1; i < oTable.rows.length; i++) {
        var cell = oTable.rows[i].cells[3].innerHTML;
        if (cell != "" && cell != null) {
            age = daysBetween(currentDate, cell.trim());
            oTable.rows[i].cells[3].innerHTML = age;
        } else {
            oTable.rows[i].cells[3].innerHTML = "";
        }
    }
}



/*
*滚动收费项目到底端
*/
function ScollToCustFeeBottom() {
   // jQuery("#divtblList").scrollTop(jQuery("#divtblList")[0].scrollHeight);
}

//读取客户证件，如果是通用登记，不仅要检索证件信息，并且要关联查询客户最近一次体检信息
function CommonReadCustomerCard() {
    if (IsCommon == 1) {
        jQuery("#btnSearch").click();
    }
    else {
        ReadCustomerFromCard();
    }
}
function OpenCustomerCloneDialog() {
    //判断是否存在客户名称
    var CustomerName = jQuery.trim(jQuery("#spanCustomerName").text());
    if (CustomerName == "") {
        ShowSystemDialog("对不起，客户基本信息不存在，请先维护！");
        return false;
    }
    var startState = 0;
    var dialog = art.dialog({
        id: 'artDialogIDRegisterDate',
        lock: true,
        fixed: true,
        opacity: 0.7,
        width: 580,
        padding: 0,
        title: '克隆收费项目',
        content: jQuery("#divCloneDataListShowDialog").html(),
        init: function () {
            //折叠基本信息栏
            if (jQuery("#box").is(":visible")) {
                jQuery(".basic").attr("disabled", true); //禁用元素编辑性以便保证鼠标不穿透
                jQuery("#baseInfo").click();
                startState = 1;
            }
        },
        close: function () {
            //折叠基本信息栏
            if (startState == 1) {
                jQuery(".basic").removeAttr("disabled"); //启用元素编辑性以便保证正常使用
                jQuery("#baseInfo").click();
            }
            return true;
        }
    }).lock();
}


/*****************选择团体用户体检信息 Begin(通过身份证查询客户体检信息存在多个为打印指引单的体检信息时)*/

//通过证件信息获取客户的体检信息
//此方法必须使用同步调用方法，直到此方法返回调用结果后方可执行下一步操作
function GetCustPhysicalExamInfoByIDCard(IDCard, CustomerName, Is_Team) {
    var allCustomerExamInfoDT = ""; //记录该证件号、客户姓名对应的所有体检信息
    var data = { Is_Team: Is_Team, IDCard: IDCard, CustomerName: CustomerName, action: "GetCustPhysicalExamInfoByIDCard" }
    jQuery.ajax({
        async: false,
        type: "GET",
        url: "/Ajax/AjaxRegiste.aspx",
        data: data,
        cache: false,
        dataType: "json",
        success: function (msg) {           
            msg = CheckAjaxReturnDataInfo(msg);
            if (msg == null || msg == "") {
                return;
            }
            allCustomerExamInfoDT = msg;
        }
    });
    return allCustomerExamInfoDT;
}

//选择客户体检信息
function ChooseCustomerPhysicalExamInfo(dataList) {
    var content = jQuery("#RegistePhysicalExamInfoDialog").html();
    var newContent = "";
    var imgSrc = "";
    jQuery(dataList).each(function (i, item) {
        if (item.Base64Photo == "") {
            imgSrc = defalutImagSrc;
        }
        else {
            imgSrc = "data:image/gif;base64," + item.Base64Photo;
        }
        newContent += content.replace(/@IDCard/gi, item.IDCard)
                      .replace(/@ID_Customer/gi, item.ID_Customer)
                      .replace(/@CustomerID/gi, item.ID_Customer == "" ? "" : "<b>体检号：</b>" + item.ID_Customer)
                      .replace(/@CustomerName/gi, item.CustomerName)
                      .replace(/@ID_Gender/gi, item.ID_Gender)
                      .replace(/@GenderName/gi, item.GenderName)
                      .replace(/@CustomerName/gi, item.CustomerName)
                      .replace(/@imgSrc/gi, imgSrc)
                      .replace(/@TeamName/gi, item.TeamName == "" ? "" : "<b>团体名称：</b>" + item.TeamName)
                      .replace(/@Department/gi, item.Department == "" ? "" : "<b>一级部门：</b>" + item.DepartSubA)
                      .replace(/@DepartSubA/gi, item.DepartSubA == "" ? "" : "<br/><b>二级部门：</b>" + item.DepartSubA)
                      .replace(/@DepartSubB/gi, item.DepartSubB == "" ? "" : "<br/><b>三级部门：</b>" + item.DepartSubB)
                      .replace(/@DepartSubC/gi, item.DepartSubC == "" ? "" : "<br/><b>四级部门：</b>" + item.DepartSubC)
                      .replace(/@RoleName/gi, item.RoleName == "" ? "角色：" : "<b>角色：</b>" + item.RoleName)
    });

    content = '<div class="RegisteCustumerDialog">' + newContent + '</div>';
    var dialog = art.dialog({
        lock: true, fixed: true, opacity: 0.3,
        padding: 0,
        id: "dialog1",
        title: '请选择客户体检信息(点击图片即可选择)',
        content: content
        //follow: document.getElementById('txtSFZ')
    }).lock();
}
//点击选中的客户体检信息
function SureChoosePhysicalExamInfo(obj, dataList) {
    var oldValue = jQuery("#txtSearchX", parent.document).val();
    ResetAllCustomerInfo();
    var ID_Customer = jQuery(obj).attr("ID_Customer");      //获取当前选中体检号
    jQuery("#txtSearchX", parent.document).val(ID_Customer);                 //设置检索区域值为当前体检号
    RequestCustomerInfo("txtSearchX", "ID_Customer", 1);    //通过检索区域值检索客户体检信息
    CloseDialog("dialog1");
}
/*****************选择团体用户体检信息(通过身份证查询客户体检信息存在多个为打印指引单的体检信息时)*/
