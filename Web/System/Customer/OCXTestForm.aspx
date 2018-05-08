﻿<head>
    <style type="text/css">
        #txtCommand
        {
            width: 86px;
        }
    </style>
</head>
p<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OCXTestForm.aspx.cs" Inherits="PEIS.Web.System.Customer.OCXTestForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<body>
    --此区域为TakePhoto OCX插件测试 Begin--
    <div>
        <object id="TakePhoto" classid="clsid:ea33a66e-f937-4d0d-aa91-8f6c917d0748" width="341"
            height="221" codebase="http://192.172.0.120/ActiveX/FYHTakePhotoSetup.msi">
        </object>
        <br>
        <input type='button' onclick='TakePhoto.ShowMessage("Hello World!")' value='ShowMessage'>
        <input type='button' onclick='TakePhoto.StartPhoto();' value='StartPhoto'>
        <input type='button' id="btnSaveImage" onclick='SavePhoto();' value='显示图片'>
        <img id="testImage" src="data:image/gif;base64,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" />
    </div>
    <br />
    --此区域为TakePhoto OCX插件测试 End-- --此区域为FastReport OCX插件测试 Begin--<p>
        &nbsp;</p>
    &nbsp;<div>
        <object id="FastReport" classid="clsid:b4d9b4bf-d98d-4748-9018-73688e1fcb99" codebase="http://192.172.0.120/ActiveX/FYHTakePhotoSetup.msi"
            style="width: 0px; height: 0px;">
        </object>
        <br>
        <label>
            体检号：</label>
        <input type="text" value="11020620130030" id="txtCustomer" />
        <label>
            体检号：</label><input type="text" value="1" id="txtCommand" /><input type="text" value="1"
                id="txtIsPrint" />
        <input type='button' value="加载指引单" onclick='Generate();'>
        <input type='button' value="打开设计器" onclick='FastReport.LoadEmpty();'>
        <input type='button' value="体检报告[非合并]" onclick='GenerateCustomerExam();'>
        <input type='button' value="体检报告[合并]" onclick='GenerateCustomerExam_Merge();'>
        <input type='button' value="体检报告[通用]" onclick='GenerateCustomerExam_Common();'>
        <input type='button' value="体检报告[通用含标题]" onclick='GenerateCustomerExam_Common_Caption();'>
        <input type='button' value="体检报告预览[通用含标题]" onclick='GenerateCustomerExam_Common_Caption_View();'>
    </div>
    --此区域为FastReport OCX插件测试 End--
    <div>
        <input type="file" id="myFile" /><input type="button" value="显示图片" onclick="ShowImage()" />
        <image id="myImage"></image>
    </div>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    </div>
    </form>
</body>
</html>
<script>
    function ShowImage() {
        document.getElementById("myImage").src = "data:image/gif;base64," + FastReport.GetImageCodeBase64(document.getElementById("myFile").value);
    }
    function GenerateCustomerExam() {
        FastReport.GenerateCustomerExam(document.getElementById("txtCustomer").value, "ExamReport.frx", document.getElementById("txtIsPrint").value);
    }
    function GenerateCustomerExam_Merge() {
        FastReport.GenerateCustomerExam_Merge(document.getElementById("txtCustomer").value, "ExamReport_Merge.frx", "1", document.getElementById("txtIsPrint").value);
    }
    function GenerateCustomerExam_Common() {
        FastReport.GenerateCustomerExam_Merge(document.getElementById("txtCustomer").value, "ExamReport_Common.frx", document.getElementById("txtCommand").value, document.getElementById("txtIsPrint").value);
    }
    function GenerateCustomerExam_Common_Caption() {
        FastReport.GenerateCustomerExam_Merge(document.getElementById("txtCustomer").value, "ExamReport_Common_Caption.frx", document.getElementById("txtCommand").value, document.getElementById("txtIsPrint").value);
    }
    function GenerateCustomerExam_Common_Caption_View() {
        FastReport.ReportPreview(document.getElementById("txtCustomer").value,
        "ExamReport_Common_Caption.frx",
         document.getElementById("txtCommand").value,
          document.getElementById("txtIsPrint").value);
    }
    function Generate() {
        FastReport.GenerateCustomerGuide(document.getElementById("txtCustomer").value, "Guidesheet.frx", 0);
    }
    function StartPhoto() {
        TakePhoto.StartPhoto(2, 2, 308, 221);
        document.getElementById("btnPreview").click();
    }
    function SavePhoto() {
        var Base64Code = TakePhoto.SavePhoto("c:\\aa.jpg");
        document.getElementById("testImage").src = "data:image/gif;base64," + Base64Code + "";
    }
</script>
