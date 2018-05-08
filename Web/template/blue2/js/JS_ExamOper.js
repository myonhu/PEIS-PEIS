﻿


        /// <summary>
        /// 初始化采集卡
        /// </summary>
        function InitCapture() {

            var VideoCaptureContent = "";
            if (VideoCaptureContent == "") {
                VideoCaptureContent = '<iframe width="768" height="576" id="FrameVideoCapture" frameBorder=0 marginHeight=0 marginWidth=0 style="position:absolute; visibility:inherit; top:0px; left:0px; z-index:1; filter=\'progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)\';" src="/system/exam/VideoCapture.aspx"></iframe>';
            }
            jQuery("#ShowVideoCapture").html(VideoCaptureContent);

            InitCapptureImageList(); // 初始化采集卡的图片
        }

        //        function DisposeVideoCapture() {
        //            var IsVerifyCapture = jQuery("#IsVerifyCapture").val();
        //            // -1：表示还未验证是否注册
        //            if (IsVerifyCapture == "-1") {
        //                return;
        //            }
        //            // 0：还未验证通过，则提示：采集卡还未注册，注册后才能采集图像！
        //            if (IsVerifyCapture == "0") {
        //                return;
        //            }

        //            if (VideoCapture == null || VideoCapture == undefined) {
        //                return;
        //            }
        //            try {
        //                VideoCapture.StopVideoDisplay();
        //                VideoCapture.CloseVideoDisplay();
        //            } catch (e) { }
        //        }

        var VideoCapture = null;
        /// <summary>
        /// 采集单张图像
        /// </summary>
        function SaveVideoCaptureImage() {

            jQuery("#showCaptureMsg").html("");
            var IsVerifyCapture = jQuery("#IsVerifyCapture").val();
            // -1：表示还未验证是否注册
            if (IsVerifyCapture == "-1") {
                IsVerifyCapture = VideoCapture.VerifyCaptureRegist();
                jQuery("#IsVerifyCapture").val(IsVerifyCapture);
            }
            // 0：还未验证通过，则提示：采集卡还未注册，注册后才能采集图像！
            if (IsVerifyCapture == "0") {
                var showmsg = "采集卡还未注册，注册后才能采集图像！";
                jQuery("#showCaptureMsg").html(showmsg);
                return;
            }
            //            var GetImageState = VideoCapture.GetSignleImageParam(jQuery.trim(jQuery('#txtCustomerID').val()), jQuery('#txtSectionID').val());
            //            if (GetImageState != "1") { return; }

            //            return;

            var ReturnSingleImage = VideoCapture.GetSignleImageParam(jQuery.trim(jQuery('#txtCustomerID').val()), jQuery('#txtSectionID').val());
            if (ReturnSingleImage == "-1") {
                jQuery("#showCaptureMsg").html("获取图片错误,请重新获取！");
                return;
            }

            if (ReturnSingleImage == "-2") {
                jQuery("#showCaptureMsg").html("采集图片过多，请先进行保存后再采集！");
                return;
            }

            if (ReturnSingleImage == "-3") {
                jQuery("#showCaptureMsg").html("没有视频信号，不能进行图像的采集！");
                return;
            }

            if (ReturnSingleImage == "-4") {
                jQuery("#showCaptureMsg").html("获取图片错误,请重新获取！");
                return;
            }

            if (ReturnSingleImage != "-1" && ReturnSingleImage != "-2" && ReturnSingleImage != "-3" && ReturnSingleImage != "-4") {
                var arrayReturnSingleImage = ReturnSingleImage.split('|');
                if (arrayReturnSingleImage.length > 1) {

                    var CaptureImageItemTempleteContent = jQuery('#CaptureImageItemTemplete').html();         //采集图片模版

                    if (jQuery("#waitingupload_" + arrayReturnSingleImage[0]) != undefined) {
                        jQuery("#waitingupload_" + arrayReturnSingleImage[0]).remove();
                    }

                    jQuery("#WaitingUploadFileList").append("<li id='waitingupload_" + arrayReturnSingleImage[0] + "'>" + arrayReturnSingleImage[2] + "</li>");

                    var CaptureImageItem =
                             CaptureImageItemTempleteContent.replace(/@Base64Code/gi, "data:image/gif;base64," + arrayReturnSingleImage[1])
                            .replace(/@ImageFileName/gi, arrayReturnSingleImage[0])
                            .replace(/@checked/gi, "")
                            .replace(/@uploadname/gi, "")
                            .replace(/@isupload/gi, "0")
                            .replace(/@used/gi, "1")
                            .replace(/@CaptureImageFile/gi, "CaptureImageFile")
                            ;
                    // 将新新采集的图片，追加到第一个位置
                    jQuery("#SmallImageArea").prepend(CaptureImageItem);
                    jQuery("#Used_" + arrayReturnSingleImage[0]).show();               // 显示是否在报告中显示的标记
                }
            }
        }

        /// <summary>
        /// 显示该体检号已经采集的视频图像（显示OCX中还未上传的图片）
        /// </summary>
        function ShowCaptureImageList() {

            if (VideoCapture == null || VideoCapture == undefined) {
                try {
                    var IFrameVideoCapture = window.top.document.getElementById("FrameVideoCapture").contentWindow;
                    VideoCapture = IFrameVideoCapture.document.getElementById("VideoCapture");                     //获取采集卡插件
                } catch (e) {
                    VideoCapture = null;
                }

            }
            if (VideoCapture == null || VideoCapture == undefined) {
                setTimeout(ShowCaptureImageList, 500);
                return;
            }
            // 如果已经获取了图片，不进行下面的操作
            var IsGetCaptureImageList = jQuery("#IsGetCaptureImageList").val();
            if (IsGetCaptureImageList == "1") { return; }
            jQuery("#IsGetCaptureImageList").val("1");


            var IsVerifyCapture = jQuery("#IsVerifyCapture").val();
            // -1：表示还未验证是否注册
            if (IsVerifyCapture == "-1") {
                IsVerifyCapture = VideoCapture.VerifyCaptureRegist();
                jQuery("#IsVerifyCapture").val(IsVerifyCapture);
            }
            // 0：还未验证通过，则提示：采集卡还未注册，请注册后再使用！
            if (IsVerifyCapture == "0") {
                jQuery("#showCaptureMsg").html("采集卡还未注册，请注册后再使用！");
                // ShowSystemDialog("采集卡还未注册，请注册后再使用！");

                jQuery("#btnSaveImg").attr("disabled", "disabled");
                jQuery("#btnUploadImg").attr("disabled", "disabled");
                CloseVideoDisplay(); // 关闭视频采集卡
                return;
            }

            var ReturnImageList = VideoCapture.GetImageFileList(jQuery.trim(jQuery('#txtCustomerID').val()), jQuery('#txtSectionID').val());
            if (ReturnImageList == "-1" || ReturnImageList == "-2" || ReturnImageList == "-3") {

                // 开始显示视频
                StartVideoDisplay();

                return;
            }
            var arrayReturnImageList = ReturnImageList.split("－");
            if (arrayReturnImageList.length > 0) {
                var arrayReturnSingleImage;
                var CaptureImageItem;
                var CaptureImageItemTempleteContent = jQuery('#CaptureImageItemTemplete').html();         //采集图片模版
                for (var i = 0; i < arrayReturnImageList.length; i++) {

                    arrayReturnSingleImage = arrayReturnImageList[i].split('|');
                    if (arrayReturnSingleImage.length > 1) {

                        if (jQuery("#waitingupload_" + arrayReturnSingleImage[0]) != undefined) {
                            jQuery("#waitingupload_" + arrayReturnSingleImage[0]).remove();
                        }
                        jQuery("#WaitingUploadFileList").append("<li id='waitingupload_" + arrayReturnSingleImage[0] + "'>" + arrayReturnSingleImage[2] + "</li>");

                        CaptureImageItem =
                             CaptureImageItemTempleteContent.replace(/@Base64Code/gi, "data:image/gif;base64," + arrayReturnSingleImage[1])
                            .replace(/@ImageFileName/gi, arrayReturnSingleImage[0])
                            .replace(/@checked/gi, "")
                            .replace(/@used/gi, "1")
                            .replace(/@isupload/gi, "0")
                            .replace(/@uploadname/gi, "")
                            .replace(/@CaptureImageFile/gi, "CaptureImageFile")
                            .replace(/@IsUploaded/gi, "0")
                            ;
                        // 将新新采集的图片，追加到第一个位置
                        jQuery("#SmallImageArea").prepend(CaptureImageItem);
                        jQuery("#Used_" + arrayReturnSingleImage[0]).show();               // 显示是否在报告中显示的标记
                    }
                }
            }

            // 开始显示视频
            StartVideoDisplay();

        }

        /// <summary>
        /// 显示图像采集卡图像
        /// </summary>
        function StartVideoDisplay() {

            if (VideoCapture == null || VideoCapture == undefined) {
                try {
                    var IFrameVideoCapture = window.top.document.getElementById("FrameVideoCapture").contentWindow;
                    VideoCapture = IFrameVideoCapture.document.getElementById("VideoCapture");                     //获取采集卡插件
                } catch (e) {
                    VideoCapture = null;
                }
            }
            if (VideoCapture == null || VideoCapture == undefined) {
                setTimeout(StartVideoDisplay, 500);
                return;
            }
            // 如果已经开启了视频，则不进行下面的操作
            var IsShowCaptureImage = jQuery("#IsShowCaptureImage").val();
            if (IsShowCaptureImage == "1") { return; }
            jQuery("#IsShowCaptureImage").val("1");

            VideoCapture.StartVideoDisplay();
        }

        /// <summary>
        /// 关闭采集卡
        /// </summary>
        function CloseVideoDisplay() {
            //var VideoCapture = document.getElementById("VideoCapture");                     //获取采集卡插件
            VideoCapture.CloseVideoDisplay();

            jQuery("#btnCloseVideoDisplay").attr("disabled", "disabled");
        }

        /// <summary>
        /// 显示大图
        /// </summary>
        function ShowCaptureBigImage(id) {

            jQuery("#FrameVideoCapture").hide();
            jQuery("#CaptureBigImage").attr("src", jQuery("#CImage_" + id).attr("src"));
            jQuery("#ShowCaptureBigImage").show();

            if (jQuery("#btnUploadImg").attr("disabled") == "disabled") {
                return;
            }
            jQuery("#ShowReport_" + id).show(); // 在报告中显示（按钮）
            jQuery("#Del_" + id).show();        // 删除（按钮）
            jQuery("#Up_" + id).show();         // 向上移动（按钮）
            jQuery("#Down_" + id).show();       // 向下移动（按钮）

        }

        /// <summary>
        /// 隐藏大图
        /// </summary>
        function HideCaptureBigImage(id) {
            jQuery("#FrameVideoCapture").show();
            jQuery("#ShowCaptureBigImage").hide();
            jQuery("#ShowReport_" + id).hide(); // 在报告中显示（按钮）
            jQuery("#Del_" + id).hide();        // 删除（按钮）
            jQuery("#Up_" + id).hide();         // 向上移动（按钮）
            jQuery("#Down_" + id).hide();       // 向下移动（按钮）
        }


        /// <summary>
        /// 向上移动
        /// </summary>
        function UpCaptureImage(id, flag) {

            var UpImageFileName = null;   // 上面一个元素 文件名称
            var ImageFileName = null;     // 文件名称
            jQuery("input[name='CaptureImageFile']").each(function () {

                UpImageFileName = ImageFileName;
                ImageFileName = jQuery(this).val();     // 获取文件名称值

                if (ImageFileName == id) {
                    return false; // break;
                }

            });
            if (UpImageFileName == null) {
                UpImageFileName = ImageFileName;
            }

            if (UpImageFileName == ImageFileName) { return; } // 如果是第一个元素，直接退出

            var outHtml_ImageFileName = jQuery("#li_" + ImageFileName).outerHTML();
            var outHtml_UpImageFileName = jQuery("#li_" + UpImageFileName).outerHTML();

            var replaceReg01 = new RegExp("_" + UpImageFileName, "gi"); // 替换上面一个ID为特殊字符

            var NewOutHtml_UpImageFileName = outHtml_UpImageFileName.replace(replaceReg01, "_" + UpImageFileName + "_replace");

            jQuery("#li_" + UpImageFileName).outerHTML(NewOutHtml_UpImageFileName); // 先把上面一个替换成有特殊字符的

            jQuery("#li_" + ImageFileName).outerHTML(outHtml_UpImageFileName);
            jQuery("#li_" + UpImageFileName + "_replace").outerHTML(outHtml_ImageFileName);

        }


        /// <summary>
        /// 向下移动
        /// </summary>
        function DownCaptureImage(id, flag) {


            var UpImageFileName = null;   // 下面一个元素 文件名称
            var ImageFileName = null;     // 文件名称
            jQuery("input[name='CaptureImageFile']").each(function () {
                if (UpImageFileName == null) {
                    ImageFileName = jQuery(this).val();     // 获取文件名称值
                    if (ImageFileName == id) {
                        UpImageFileName = ImageFileName;
                        return true; // continue;
                    }
                } else {
                    UpImageFileName = jQuery(this).val();     // 获取文件名称值
                    return false; // break;
                }
            });

            if (UpImageFileName == ImageFileName) { return; } // 如果是最后一个元素，直接退出

            var outHtml_ImageFileName = jQuery("#li_" + ImageFileName).outerHTML();
            var outHtml_UpImageFileName = jQuery("#li_" + UpImageFileName).outerHTML();

            var replaceReg01 = new RegExp("_" + UpImageFileName, "gi"); // 替换第二个ID为特殊字符

            var NewOutHtml_UpImageFileName = outHtml_UpImageFileName.replace(replaceReg01, "_" + UpImageFileName + "_replace");

            jQuery("#li_" + UpImageFileName).outerHTML(NewOutHtml_UpImageFileName); // 先把第二个替换成有特殊字符的

            jQuery("#li_" + ImageFileName).outerHTML(outHtml_UpImageFileName);
            jQuery("#li_" + UpImageFileName + "_replace").outerHTML(outHtml_ImageFileName);

        }

        /// <summary>
        /// 清除科室图片目录下面的图片
        /// </summary>
        function ClearSectionImageDir() {

            if (jQuery("#CurrImageFileUploadUrl").val() == "") {

                var showmsg = "图片服务器地址配置错误，请检查相关配置！";
                jQuery("#showCaptureMsg").html(showmsg);
                return;
            }

            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());    // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var ID_Section = jQuery('#txtSectionID').val();

            var WebServiceUrl = encodeURIComponent(jQuery("#CurrImageFileUploadUrl").val() + "/UploadImages.aspx");

            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxBase.aspx",
                contentType: "application/x-www-form-urlencoded;Content-length=1024000",
                data: {
                    WebServiceUrl: WebServiceUrl,
                    action: "ClearSectionImageDir",
                    ID_Customer: ID_Customer,
                    ID_Section: ID_Section,
                    currenttime: encodeURIComponent(new Date())
                },
                cache: false,
                dataType: "text",
                success: function (xmlmsg) {
                    
                    if (parseInt(xmlmsg) < 0) {
                        ShowSystemDialog("图片删除失败，请联系管理人员!");
                    } else {

                        ShowSystemDialog("清除成功！");
                        // InitCustomExamItemPage_IsSetDefaultValue(gPageCustomExamDataMsg, 0);
                        GetCustomerSectionExamInfo(); // 清除成功后，重新调用查询，进行页面刷新 20140821 by wtang 
                    }
                }
            });
        }

        /// <summary>
        /// 测试获取图片服务器的当前时间
        /// </summary>
        function GetUploadImageServerSystemTime() {

            if (jQuery("#CurrImageFileUploadUrl").val() == "") {

                var showmsg = "图片服务器地址配置错误，请检查相关配置！";
                jQuery("#showCaptureMsg").html(showmsg);
                return;
            }

            var WebServiceUrl = encodeURIComponent(jQuery("#CurrImageFileUploadUrl").val() + "/UploadImages.aspx");

            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxBase.aspx",
                contentType: "application/x-www-form-urlencoded;Content-length=1024000",
                data: {
                    WebServiceUrl: WebServiceUrl,
                    action: "GetUploadImageServerSystemTime",
                    currenttime: encodeURIComponent(new Date())
                },
                cache: false,
                dataType: "text",
                success: function (xmlmsg) {
                    //jQuery("string", xmlmsg).each(function () {
                    var sNewImageFileName = xmlmsg; // jQuery(this).text();
                    //alert(sNewImageFileName);
                    //});
                }
            });
        }

        /// <summary>
        /// 上传图像
        /// </summary>
        function UploadVideoCaptureImage() {


            var IsVerifyCapture = jQuery("#IsVerifyCapture").val();
            // -1：表示还未验证是否注册
            if (IsVerifyCapture == "-1") {
                IsVerifyCapture = VideoCapture.VerifyCaptureRegist();
                jQuery("#IsVerifyCapture").val(IsVerifyCapture);
            }
            // 0：还未验证通过，则提示：采集卡还未注册，注册后才能上传图像！
            if (IsVerifyCapture == "0") {
                var showmsg = "采集卡还未注册，注册后才能上传图像！";
                jQuery("#showCaptureMsg").html(showmsg);
                return;
            }

            if (jQuery("#CurrImageFileUploadUrl").val() == "") {

                var showmsg = "图片服务器地址配置错误，请检查相关配置！";
                jQuery("#showCaptureMsg").html(showmsg);
                return;
            }

            var ImageFileName = ""; // 文件名称
            var ImageIsUpload = ""; // 标记是否已经上传
            var ImageID = "";       // 图片标记ID
            var ImageBase64String = ""; // 图片64位字符串
            var ImageIsShow = 0;


            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());    // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var ID_CustExamSection = jQuery("#ID_CustExamSection").val();           // 科室小结编号
            var ID_Section = jQuery('#txtSectionID').val();

            //var VideoCapture = document.getElementById("VideoCapture");                     //获取采集卡插件
            var nTotalFileNum = 0;      // 记录总文件数量
            var nUploadedFileNum = 0;   // 记录已经上传的文件数量
            var IsUploadFinished = 1;   // 标记是否已经上传完成
            jQuery("input[name='CaptureImageFile']").each(function () {
                nTotalFileNum++;
                ImageFileName = jQuery(this).val();     // 获取文件名称值
                ImageIsUpload = jQuery(this).attr("isupload");          // 该图片是否已经上传
                if (ImageIsUpload == "1") {
                    nUploadedFileNum++;
                    return true;
                }             // continue  1:已经上传 2：正在上传...
                if (ImageIsUpload == "2") { return true; }             // continue  1:已经上传 2：正在上传... 3：已经删除
                if (ImageIsUpload == "3") { return true; }             // continue  1:已经上传 2：正在上传... 3：已经删除
                IsUploadFinished = 0; // 表示还未上传完成

                ImageID = "CImage_" + ImageFileName;    // 图片ID
                ImageBase64String = jQuery("#waitingupload_" + ImageFileName).html();  // 图片64位字符串
                //ImageBase64String = encodeURIComponent(ImageBase64String);
                ImageIsShow = jQuery("#" + ImageID).attr("checked");    // 图片是否在报告中出现

                jQuery(this).attr("isupload", "2"); // 标记为正在上传...
                jQuery("#Message_" + ImageFileName).html("正在上传...");  // 显示正在上传中
                jQuery("#Message_" + ImageFileName).show();              // 显示

                //var WebServiceUrl = encodeURIComponent(jQuery("#CurrImageFileUploadUrl").val() + "/UploadFileAPI.asmx?op=UploadImageEx");
                var WebServiceUrl = encodeURIComponent(jQuery("#CurrImageFileUploadUrl").val() + "/UploadImages.aspx");

                jQuery.ajax({
                    type: "POST",
                    url: "/Ajax/AjaxBase.aspx",
                    contentType: "application/x-www-form-urlencoded;Content-length=1024000",
                    data: { ID_Customer: ID_Customer,
                        ID_Section: ID_Section,
                        ImageBase64String: ImageBase64String,
                        WebServiceUrl: WebServiceUrl,
                        OrgImageFileName: ImageFileName,
                        action: "WebServiceUploadImage",
                        currenttime: encodeURIComponent(new Date())
                    },
                    cache: false,
                    dataType: "text",
                    success: function (xmlmsg) {

                        // jQuery("string", xmlmsg).each(function () {
                        var sNewImageFileName = xmlmsg; // jQuery(this).text();

                        if (sNewImageFileName != "") {
                            var sFileName = sNewImageFileName.split("|");
                            if (sFileName.length > 1) {

                                // 上传成功后，图片读取上传成功后的图片（上传的图片，清晰度更高）
                                jQuery("#CImage_" + sFileName[0]).attr("src", GetUserImageFilesDir() + sFileName[1]);

                                jQuery("#Capture_" + sFileName[0]).attr("isupload", "1");               // 标记为已上传
                                jQuery("#Capture_" + sFileName[0]).attr("uploadname", sFileName[1]);    // 上传后的文件名称

                                jQuery("#Message_" + sFileName[0]).html("上传成功");                    // 显示上传成功

                                //                                    // 上传成功调用图片删除函数
                                //                                    VideoCapture.DeleteSingleImage(
                                //                                        jQuery.trim(jQuery('#txtCustomerID').val()), 
                                //                                        jQuery('#txtSectionID').val(),
                                //                                        sFileName[0]+".jpg"
                                //                                    );

                            }
                        }
                        // });

                        UploadVideoCaptureImage(); // 继续下一个文件的传输

                    }
                });

                return false; // 跳出循环
            });

            if (IsUploadFinished == 1 && nTotalFileNum == nUploadedFileNum) {
                //                // 上传完成,删除本地图片
                //                alert("上传完成,删除本地图片");
                // 上传成功调用图片删除函数
                VideoCapture.DeleteCustomerImage(
                    jQuery.trim(jQuery('#txtCustomerID').val()),
                    jQuery('#txtSectionID').val()
                );
            }
            else if (IsUploadFinished == 1) {
                // 这里，由于没有全部上传成功，只删除成功上传了的那些文件
                jQuery("input[name='CaptureImageFile']").each(function () {
                    nTotalFileNum++;
                    ImageFileName = jQuery(this).val();     // 获取文件名称值
                    ImageIsUpload = jQuery(this).attr("isupload");          // 该图片是否已经上传
                    ImageUploadName = jQuery(this).attr("uploadname");          // 该图片是否已经上传 
                    if (ImageIsUpload == "1" && ImageFileName != "ImageUploadName") {
                        // 上传成功调用图片删除函数
                        ret = VideoCapture.DeleteSingleImage(
                        jQuery.trim(jQuery('#txtCustomerID').val()),
                        jQuery('#txtSectionID').val(),
                        ImageFileName + ".jpg"
                        );
                    }
                });
            }
            if (IsUploadFinished == 1) {
                SaveUploadImageFileResult();
            }
        }

        /// <summary>
        /// 单个图片的删除
        /// </summary>
        function DeleteSingleImage(ImageFileName, IsUploaded) {

            //            ShowCaptureBigImage(ImageFileName);

            //            var tipscontent = "您确定要删除这张图片吗？";
            //            art.dialog({
            //                id: 'artDialogID',
            //                content: tipscontent,
            //                lock: true,
            //                fixed: true,
            //                opacity: 0.3,
            //                button: [{
            //                    name: '确定删除',
            //                    title: '提示',
            //                    callback: function () {

            var ret = 0;
            if (IsUploaded == 0) {
                //var VideoCapture = document.getElementById("VideoCapture");                     //获取采集卡插件
                // 上传成功调用图片删除函数
                ret = VideoCapture.DeleteSingleImage(
                            jQuery.trim(jQuery('#txtCustomerID').val()),
                            jQuery('#txtSectionID').val(),
                            ImageFileName + ".jpg"
                            );
            }
            else {
                ret = "1"; // 如果已经上传，则直接设置为删除成功标记，这里暂时不删除远程服务器上的问题。 
            }
            if (ret == "1") {
                jQuery("#li_" + ImageFileName).remove();

                // ShowSystemDialog("删除成功！");
            } else {
                // ShowSystemDialog("删除失败！");

                var showmsg = "删除失败！";
                jQuery("#showCaptureMsg").html(showmsg);
            }

            HideCaptureBigImage(ImageFileName);

            //                        HideCaptureBigImage(ImageFileName);
            //                        return true;
            //                    }, focus: true
            //                }, {
            //                    name: '取消'
            //                }]
            //            });
            //            
        }
        /// <summary>
        /// 修改图像是否在报告中显示的标记
        /// </summary>
        function IsImageShowInReport(ImageFileName) {

            var ImageIsUsed = "";       // 标记单个图片是否在报告中使用
            ImageIsUsed = jQuery("#Capture_" + ImageFileName).attr("used");                 // 该图片是否在报告中使用
            if (ImageIsUsed == "0") {
                jQuery("#Capture_" + ImageFileName).attr("used", "1");
                jQuery("#Used_" + ImageFileName).show();               // 显示是否在报告中显示的标记
            } else {
                jQuery("#Capture_" + ImageFileName).attr("used", "0");
                jQuery("#Used_" + ImageFileName).hide();               // 显示是否在报告中显示的标记
            }

        }
        /// <summary>
        /// 保存已经上传的图片到数据库中
        /// </summary>
        function SaveUploadImageFileResult() {

            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());    // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var ID_CustExamSection = jQuery("#ID_CustExamSection").val();           // 科室小结编号
            var ID_Section = jQuery('#txtSectionID').val();

            var nTotalFileNum = 0;
            var ImageFileName = "";     // 文件名称
            var IsUploadFinished = 1;   // 标记是否已经上传完成
            var ImageUploadName = "";   // 上传后的图像文件名
            var ImageIsUpload = "";     // 标记单个图片是否已经上传
            var ImageIsUsed = "";       // 标记单个图片是否在报告中使用
            var SaveUploadImageFileNames = "";  //
            jQuery("input[name='CaptureImageFile']").each(function () {
                nTotalFileNum++;
                ImageFileName = jQuery(this).val();     // 获取文件名称值
                ImageIsUpload = jQuery(this).attr("isupload");          // 该图片是否已经上传
                ImageIsUsed = jQuery(this).attr("used");                // 该图片是否在报告中使用
                ImageUploadName = jQuery(this).attr("uploadname");      // 该图片是否已经上传 
                if (ImageUploadName != "") {
                    // 文件名称 ，是否在报告中显示 
                    SaveUploadImageFileNames = ImageUploadName + "," + ImageIsUsed + "|" + SaveUploadImageFileNames;
                }
            });

            if (SaveUploadImageFileNames != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Ajax/AjaxCustExam.aspx",
                    data: { CustomerID: ID_Customer,
                        SectionID: ID_Section,
                        ID_CustExamSection: ID_CustExamSection,
                        SaveUploadImageFileNames: SaveUploadImageFileNames,
                        action: 'SaveUploadImageFileResult',
                        currenttime: encodeURIComponent(new Date())
                    },
                    cache: false,
                    dataType: "json",
                    success: function (jsonmsg) {
                        if (parseInt(jsonmsg) > 0) {
                            var showmsg = "保存&上传图像成功！";
                            jQuery("#showCaptureMsg").html(showmsg);
                        }
                        else {
                            var showmsg = "保存&上传图像失败，请与信息中心联系!";
                            jQuery("#showCaptureMsg").html(showmsg);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 显示已经上传到服务器的图片路径
        /// </summary>
        function ShowUploadedImageFile(ImageUrl) {
            var Is_ShowUploadImageFile = jQuery("#Is_ShowUploadImageFile").val();           // 标记是否已经读取并显示过文件了
            if (Is_ShowUploadImageFile == "1") { return; }
            jQuery("#Is_ShowUploadImageFile").val("1");           // 已经读取并显示过文件了。

            var ImageFileDir = GetUserImageFilesDir();
            var CaptureImageItemTempleteContent = jQuery('#CaptureImageItemTemplete').html();         //采集图片模版

            if (ImageUrl != "") {
                var SingleFileStr = ImageUrl.split("|");
                if (SingleFileStr.length > 1) {
                    for (var i = 0; i < SingleFileStr.length; i++) {
                        if (SingleFileStr[i] == "") { return true; } // continue ;

                        var sFileName = SingleFileStr[i].split(",");
                        if (sFileName.length > 0) {

                            if (sFileName[0] == "@uploadname") { return true; } // continue ;
                            var CaptureImageItem =
                             CaptureImageItemTempleteContent.replace(/@Base64Code/gi, ImageFileDir + sFileName[0])
                            .replace(/@ImageFileName/gi, sFileName[0].substr(0, 6))
                            .replace(/@checked/gi, "")
                            .replace(/@uploadname/gi, sFileName[0])
                            .replace(/@used/gi, sFileName[1])
                            .replace(/@isupload/gi, "1")
                            .replace(/@CaptureImageFile/gi, "CaptureImageFile")
                            ;
                            // 将新新采集的图片，追加到第一个位置
                            jQuery("#SmallImageArea").prepend(CaptureImageItem);

                            jQuery("#Uploaded_" + sFileName[0].substr(0, 6)).show();               // 显示已上传标记
                            if (sFileName[1] == "1") {
                                jQuery("#Used_" + sFileName[0].substr(0, 6)).show();               // 显示是否在报告中显示的标记
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示已经上传到服务器的图片(显示设备上传上来的图像列表 20140617 )
        /// </summary>
        function ShowDeviceResultImagesList(ImageUrl) {
            var Is_ShowDeviceResultImagesList = jQuery("#Is_ShowDeviceResultImagesList").val();           // 标记是否已经读取并显示过文件了
            if (Is_ShowDeviceResultImagesList != "1") { return; }

            var ImageFileDir = GetUserImageFilesDir();

            if (ImageUrl != "") {
                var SingleFileStr = ImageUrl.split("|");
                if (SingleFileStr.length > 1) {
                    for (var i = 0; i < SingleFileStr.length; i++) {
                        if (SingleFileStr[i] == "") { return true; } // continue ;

                        var sFileName = SingleFileStr[i].split(",");
                        if (sFileName.length > 0) {

                            if (sFileName[0] == "") { return true; } // continue ;
                            var ImageItem = '<p><img src="' + ImageFileDir + sFileName[0] + '" alt="" width="800px" /></p>';

                            // 显示设备上的图片 或 报告图片
                            jQuery("#DeviceResultImagesList").append(ImageItem);
                            jQuery("#ExamInfoTabLi5").show();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取客户图像的存放目录
        /// </summary>
        function GetUserImageFilesDir() {

            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());    // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var ID_CustExamSection = jQuery("#ID_CustExamSection").val();           // 科室小结编号
            var ID_Section = jQuery('#txtSectionID').val();

            if (ID_Customer == "" || ID_Section == "") {
                return;
            }

            return jQuery("#CurrImageFileUploadUrl").val() + "/Images/" + ID_Customer.substr(6, 4) + ID_Customer.substr(2, 2) + "/" + ID_Customer.substr(4, 2) + "/" + ID_Customer + "/" + ID_Section + "/";
        }
        
        /// <summary>
        /// 初始化采集卡的图片
        /// </summary>
        function InitCapptureImageList() {
            jQuery(".tuxcj-1-right-tu li")
		    .mouseover(function () {
		        if (jQuery(this).hasClass("selected")) return false;
		        jQuery(this).find(".tuxcj-1-right-guan").addClass("show");

		        ShowTestErrorMsg("调用 .tuxcj-1-right-tu li mouseover InitCapptureImageList() 函数（examoper.htm）");

		    })
		    .mouseout(function () {
		        jQuery(this).find(".tuxcj-1-right-guan").removeClass("show");
		        ShowTestErrorMsg("调用 .tuxcj-1-right-tu li mouseout InitCapptureImageList() 函数（examoper.htm）");
		    })
		    .click(function (e) {
		        e.stopPropagation();
		        jQuery(this).toggleClass("selected");
		        var gun = jQuery(this).find(".tuxcj-1-right-tu-tan-img-check");
		        if (gun.length) gun.remove();
		        else jQuery(this).append('<div class="tuxcj-1-right-tu-tan-img-check">');
		    });
            jQuery(".tuxcj-1-right-guan-x").click(function (e) {
                e.stopPropagation();
                var jQueryli = jQuery(this).parents(".tuxcj-1-right-tu-tan");
                jQueryli.fadeOut(300);
                setTimeout(function () {
                    jQueryli.remove();
                    wMove.reset();
                }, 500);
                return false;
            });
            var wMove = jQuery(".tuxcj-1-right-tu ul").wheelMove({
                items: ">li",
                show: 5,
                loop: false,
                step: 5,
                blank: true,
                min: 6,
                prev: ".tuxcj-1-right-tu-list-top",
                next: ".tuxcj-1-right-tu-list-bottom"
            });
        }


        var IsCanShowDetailInfo = false; // 控制是否显示详细信息
        var SepBetweenSymptoms_SplitID = "、";  // 用于分割体征词ID

        var SepBetweenExamItems = "，";   //项目分隔符        如：、
        var SepBetweenSymptoms = "、";    //体征词分隔符      如：、
        var TerminalSymbol = "。";        //项目终结符        如：。
        var SepExamAndValue = ":";        //项目小结分隔符    如：：
        var NoBetweenExamItems = "(1)";   //项目序号          如：(1)
        var NoBetweenSympotms = "①";     //体征词序号        如： ①
        var InterfaceType = "";           //接口类型  LAB PACS 等


        // 保存--读取到是体检数据，以重置页面时使用
        var gPageCustomExamDataMsg = "";
        // 保存--读取到上次的数据
        var gPageCustomExamLastSaveDataMsg = "";
        jQuery(document).ready(function () {

            jQuery(".btn_recvcomdata").hide(); // 隐藏com接口读取按钮
            SwitchHeader(2); // 显示分科的头部
            jQuery("#TipsArea").autoHeight(); // 自适应高度(提示信息区域)
            // 隐藏标签打印按钮
            jQuery("#spanPrintSectionBarCode").hide();
            // 清空结果信息中的数据
            jQuery('#CustExamResult').hide();

            // 显示科室名称
            jQuery("#HeaderSectionTitle").html(jQuery("#SectionName").val());

            //            jQuery(window).resize(
            //                            function () {
            //                                var IsVerifyCapture = jQuery("#IsVerifyCapture").val();
            //                                // -1：表示还未验证是否注册
            //                                if (IsVerifyCapture == "-1") {
            //                                    return;
            //                                }
            //                                // 0：还未验证通过，则提示：采集卡还未注册，注册后才能采集图像！
            //                                if (IsVerifyCapture == "0") {
            //                                    return;
            //                                }

            //                                if (VideoCapture == null || VideoCapture == undefined) {
            //                                    return;
            //                                }

            //                                try {
            //                                    VideoCapture.VideoCaptureMove();
            //                                } catch (e) {

            //                                }

            //                            }
            //                        );

            //            jQuery(window).scroll(
            //                function () {
            //                    var IsVerifyCapture = jQuery("#IsVerifyCapture").val();
            //                    // -1：表示还未验证是否注册
            //                    if (IsVerifyCapture == "-1") {
            //                        return;
            //                    }
            //                    // 0：还未验证通过，则提示：采集卡还未注册，注册后才能采集图像！
            //                    if (IsVerifyCapture == "0") {
            //                        return;
            //                    }

            //                    if (VideoCapture == null || VideoCapture == undefined) {
            //                        return;
            //                    }
            //                    try {
            //                        VideoCapture.VideoCaptureMove();
            //                    } catch (e) {

            //                    }
            //                }
            //            );


            // 查询客户的基本信息，并显示
            SearchCustomerBaseInfo(0, 0); //原型：SearchCustomerBaseInfo(IsShowMsg, IsLoadCustomerInfo)

            // 查询客户的体检项目等详细信息
            QueryExamDetailData();

            SepBetweenExamItems = jQuery("#SepBetweenExamItems").val();   //项目分隔符        如：、
            SepBetweenSymptoms = jQuery("#SepBetweenSymptoms").val();     //体征词分隔符      如：、
            TerminalSymbol = jQuery("#TerminalSymbol").val();             //项目终结符        如：。
            SepExamAndValue = jQuery("#SepExamAndValue").val();           //项目小结分隔符    如：：
            NoBetweenExamItems = jQuery("#NoBetweenExamItems").val();     //项目序号          如：(1)
            NoBetweenSympotms = jQuery("#NoBetweenSympotms").val();       //体征词序号        如： ①
            InterfaceType = jQuery("#InterfaceType").val();               // 接口类型

            if (SepBetweenExamItems == undefined) { SepBetweenExamItems = ""; }
            if (SepBetweenSymptoms == undefined) { SepBetweenSymptoms = ""; }
            if (TerminalSymbol == undefined) { TerminalSymbol = ""; }
            if (SepExamAndValue == undefined) { SepExamAndValue = ""; }
            if (NoBetweenExamItems == undefined) { NoBetweenExamItems = ""; }
            if (NoBetweenSympotms == undefined) { NoBetweenSympotms = ""; }
            if (InterfaceType == undefined) { InterfaceType = ""; }

            var IsSectionShowVideoCapture = jQuery("#IsSectionShowVideoCapture").val();               // 判断当前科室是否需要显示采集卡控件  20140213 by wtang
            if (IsSectionShowVideoCapture == "1") {

                if (IsCanShowDetailInfo == false) { return; }

                // 暂时不显示 图片上传控件
                jQuery("#ExamInfoTabLi4").show(); // 显示图片上传控件
                jQuery("#PACSImageUploadAreaContent").show(); // 显示图片上传控件
                InitCapture();              // 初始化采集卡
                ShowCaptureImageList();     // 显示已经采集的图片

                //                // 暂时不显示 图片上传控件
                //                                jQuery("#PACSImageUploadAreaContent").show(); // 显示图片上传控件
                //                                // 加载图片批量上传控件 Start
                //                                var params = {
                //                                    uploadServerUrl: "/SNS/Group/Upload.aspx", //上传响应页面(必须设置)
                //                                    jsFunction: "uploadImageListSuccess", 		//上传成功后回调JS
                //                                    filter: "*.jpg;*.png;*.gif;*.bmp;*.jpeg", 		//上传文件类型限制
                //                                    wmode: "transparent"
                //                                }
                //                                swfobject.embedSWF("$!{templatepath}/js/multiupload/uploadImage.swf", "PACSImageUploadArea", "960", "170", "10.0.0", "$!{templatepath}/multiupload/expressInstall.swf", params, { wmode: "transparent" });
                //                                // 加载图片批量上传控件 End

            } else {
                jQuery("#ExamInfoTabLi4").hide(); // 隐藏图片上传控件
                jQuery("#PACSImageUploadAreaContent").hide(); // 隐藏图片上传控件
            }
        });


        /// <summary>
        /// 根据体检号，查询客户的个人信息及体检项目
        /// </summary>
        function GetCustomerSectionExamInfo() {
        
            jQuery('#IDCardNoText').html("&nbsp;");
            jQuery('#CustomerNameText').html("&nbsp;");
            jQuery('#GenderNameText').html("&nbsp;");
            jQuery('#MarriageNameText').html("&nbsp;");
            jQuery('#MobileNoText').html("&nbsp;");

            jQuery('#fkls').hide();
            jQuery('#fklscen').hide();
            jQuery('#TipsArea').hide();
            jQuery('#ExamInfoTabLi1').hide();
            jQuery('#ExamInfoTabLi2').hide();
            jQuery('#ExamInfoTabLi3').hide();
            jQuery('#ExamInfoTabLi4').hide();
            jQuery("#ExamInfoTabDetail4").hide(); // 暂时不显示 图片上传控件
            jQuery('#btnCustomerSimpleInfo').hide();
            jQuery('#CustomerSectionQuickSwitch').hide();
            jQuery('#CustExamResult').hide();

            //显示等待信息
            var ExamItemWaitingTempleteContent = jQuery('#ExamItemWaitingTemplete').html(); //读取数据等待信息模版
            jQuery('#TipsMessage').html(ExamItemWaitingTempleteContent);                   //显示等待信息
            jQuery('#TipsArea').show();

            var customerid = jQuery.trim(jQuery('#txtCustomerID').val());
            var TipsMessageTempleteContent = jQuery('#TipsMessageTemplete').html();     //读取提示信息模版
            if (customerid == "") {
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "请输入客户的体检号！"));   //显示空信息
                return;
            }
            if (!isCustomerExamNo(customerid)) {

                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "体检号格式错误，请检核对后重新输入！"));   //显示空信息
                return;
            }

            var ExamUrl = "/System/Exam/ExamOper.aspx?txtSectionID=" + jQuery('#txtSectionID').val() + "&txtCustomerID=" + customerid;
            DoLoad(ExamUrl, '');
        }

        var oldQueryCustomerID = ""; // 记录上次查询的体检号
        /// <summary>
        /// 根据输入情况，自动获取客户的个人信息及体检项目 （当输入数据达到体检号的数据长度时，自动读取）
        /// </summary>
        function AutoGetCustomerSectionExamInfo() {

            var curEvent = window.event || e;
            if (curEvent.keyCode == 13) {
                var customerid = jQuery.trim(jQuery('#txtCustomerID').val());
                if (oldQueryCustomerID == customerid) { return; } // 如果与上次一样，则退出
                if (isCustomerExamNo(customerid)) {
                    oldQueryCustomerID = customerid;
                    var ExamUrl = "/System/Exam/ExamOper.aspx?txtSectionID=" + jQuery('#txtSectionID').val() + "&txtCustomerID=" + customerid;
                    DoLoad(ExamUrl, '');
                }
            }
        }

        /// <summary>
        /// 清除各个体检项目的默认值，及操作者输入的或选择的值
        /// </summary>
        function ClearCustomerDefaultValue() {

        }
        /// <summary>
        /// 清空客户信息（清空客户的所有信息，便于下一个客户数据的读取）
        /// </summary>
        function ClearCustomerInfo() {

            // 查询数据前，先隐藏客户基本信息区域
            jQuery("#divCustomerInfoArea").hide();

            var ExamItemEmptyTempleteContent = jQuery('#ExamItemEmptyTemplete').html();     //读取空数据信息模版
            jQuery('#TipsMessage').html(ExamItemEmptyTempleteContent);                     //显示空信息
            jQuery('#TipsArea').show();

            jQuery('#IDCardNoText').html("&nbsp;");
            jQuery('#CustomerNameText').html("&nbsp;");
            jQuery('#GenderNameText').html("&nbsp;");
            jQuery('#MarriageNameText').html("&nbsp;");
            jQuery('#MobileNoText').html("&nbsp;");

            jQuery('#txtCustomerID').val("");  //清空体检号

        }
        /// <summary>
        /// 查询客户的体检项目等详细信息
        /// </summary>
        function QueryExamDetailData() {

            jQuery("#txtCustomerID").focus();  // 设置选中文本框中的文字，并获取光标

            jQuery('#fkls').hide();
            jQuery('#fklscen').hide();
            jQuery('#TipsArea').hide();
            jQuery('#ExamInfoTabLi1').hide();
            jQuery('#ExamInfoTabLi2').hide();
            jQuery('#ExamInfoTabLi3').hide();
            jQuery('#ExamInfoTabLi4').hide();
            jQuery("#ExamInfoTabDetail4").hide(); // 暂时不显示 图片上传控件
            jQuery('#btnCustomerSimpleInfo').hide();
            jQuery('#CustomerSectionQuickSwitch').hide();

            IsCanShowDetailInfo = false;
            // 查询数据前，先隐藏客户基本信息区域
            jQuery("#divCustomerInfoArea").hide();
            var ExamState = jQuery('#ExamState').val();                         //体检状态,当次体检信息的状态：0表示在线，1表示归档，2表示在一号分库，3表示在二号分库…
            var Is_FeeSettled = jQuery('#Is_FeeSettled').val();                 //是否完成缴费
            var Is_GuideSheetPrinted = jQuery('#Is_GuideSheetPrinted').val();   //指引单是否打印
            var Is_SectionLock = jQuery('#Is_SectionLock').val();               //分科锁定
            var TipsMessageTempleteContent = jQuery('#TipsMessageTemplete').html();     //读取提示信息模版
            var Is_ReportReceipted = jQuery('#Is_ReportReceipted').val();               //报告已领取

            var CustomerSecurityLevel = jQuery("#CustomerSecurityLevel").val(); // 客户  操作密级
            var OperateLevel = jQuery("#OperateLevel").val();                   // 操作员操作密级
            var Is_Paused = jQuery('#Is_Paused').val();                         // 0表示处于正常体检状态；1表示处于禁检状态（若客户处于禁检状态时，只有解除禁检后方能进行体检）

            //体检状态,当次体检信息的状态：空表示不存在，0表示在线，1表示归档，2表示在一号分库，3表示在二号分库…

            jQuery(".j-autoHeight").autoHeight(); // 自适应高度

            if (ExamState == "") {
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "请输入你要操作的体检号！"));   //显示提示信息
                jQuery('#TipsArea').show();
                return;
            }

            if (ExamState != "0") {
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "该客户已归档，不能进行分科检查！"));   //显示提示信息
                jQuery('#TipsArea').show();
                return;
            }
            if (Is_Paused == "True" || Is_Paused == "1") {
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "该客户已被禁检，不能进行分科检查！"));   //显示提示信息
                jQuery('#TipsArea').show();
                return;
            }

            // 检查操作密级
            if (Is_ReportReceipted == "True" && parseInt(CustomerSecurityLevel) > parseInt(OperateLevel)) {
                jQuery("#divCustomerInfoArea").hide();  // 如果没有权限，则客户基本信息页不允许查看
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "对不起，你没有权限对该客户进行体检！"));   //显示提示信息
                jQuery('#TipsArea').show();
                return;
            }
            if (Is_FeeSettled != "True") {
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "该客户还未完成缴费，请先缴费后，再进行分科检查！"));   //显示提示信息
                jQuery('#TipsArea').show();
                return;
            }

            if (Is_GuideSheetPrinted != "True") {
                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "该客户还未打印指引单，请先打印指引单后，再进行分科检查！"));   //显示提示信息

                jQuery('#TipsArea').show();
                return;
            }

            IsCanShowDetailInfo = true;

            // 分科锁定后，修改为任然可以正常查看数据，但是不能进行修改等操作。
            //            if (Is_SectionLock == "True") {
            //                jQuery('#TipsMessage').html(TipsMessageTempleteContent.replace(/@TipsMessage/gi, "该客户分科检查已被锁定，请解除分科锁定后，再进行分科检查！"));   //显示提示信息
            //                return;
            //            }

            var NoFeeItemTempleteContent = jQuery('#NoFeeItemTemplete').html(); //暂无交费项目或体检项目模版
            //显示等待信息
            var ExamItemWaitingTempleteContent = jQuery('#ExamItemWaitingTemplete').html(); //读取数据等待信息模版
            jQuery('#TipsMessage').html(ExamItemWaitingTempleteContent);                   //显示等待信息
            jQuery('#TipsArea').show();

            var CustomerID = jQuery.trim(jQuery('#txtCustomerID').val());
            var SectionID = jQuery('#txtSectionID').val();
            var ID_Gender = jQuery('#txtGenderID').val();
            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxCustExam.aspx",
                data: { CustomerID: CustomerID,
                    SectionID: SectionID,
                    ID_Gender: ID_Gender,
                    action: 'GetCustomerExamDetailDataList',
                    currenttime: encodeURIComponent(new Date())
                },
                cache: false,
                dataType: "json",
                success: function (jsonmsg) {

                    gPageCustomExamDataMsg = jsonmsg;
                    InitCustomExamItemPage(jsonmsg);


                    if (jsonmsg != null && jsonmsg != "" && parseInt(jsonmsg.totalCount) > 0) {
                        //jQuery("#divSectionExamFloat").show();      // 显示客户其它科室列表链接
                        //jQuery("#divSectionExamedCount").show();      // 显示已检查科室链接    不采用侧边栏的方式 20140402
                        // 当体检次数大于1次时，才能进行分科对比
                        if (parseInt(jQuery("#totalExamNumber").val()) > 1) {
                            // jQuery("#divHistoryExamCount").show();   // 显示历史检查次数链接  不采用侧边栏的方式 20140402
                            jQuery("#ExamInfoTabLi2").show();           // 分科对比 显示
                        } else {
                            jQuery("#ExamInfoTabLi2").hide();           // 分科对比 隐藏
                        }
                        SetSideFloatXY();

                    }
                }
            });

        }

        /// <summary>
        /// 查询上次保存的数据
        /// </summary>
        function GetCustomerExamLastSaveData() {
            if (gPageCustomExamLastSaveDataMsg == "") {
                var CustomerID = jQuery.trim(jQuery('#txtCustomerID').val());
                var SectionID = jQuery('#txtSectionID').val();

                jQuery.ajax({
                    type: "POST",
                    url: "/Ajax/AjaxCustExam.aspx",
                    data: { CustomerID: CustomerID,
                        SectionID: SectionID,
                        action: 'GetCustomerExamLastSaveDataList',
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

                        gPageCustomExamLastSaveDataMsg = jsonmsg;
                        // 加载小结的内容,及各个检查项目的值
                        InitCustomExamLastSaveData();
                    }
                });
            } else {
                // 加载小结的内容,及各个检查项目的值
                InitCustomExamLastSaveData();
            }
        }

        /// <summary>
        /// 根据获得的数据，初始化页面显示的内容
        /// </summary>
        /// <param name="msg">该客户的收费项目，体检项目，及对应的特征词数据</param>
        function InitCustomExamItemPage(msg) {
            // 默认需要初始化默认值  0：不绑定默认值 1：绑定默认值（已经小结的情况下，加载小结后的信息） 2：仅绑定默认值
            InitCustomExamItemPage_IsSetDefaultValue(msg, 1);
        }


        /// <summary>
        /// 清除该客户的检查数据
        /// </summary>
        function ClearCustomerSectionExamInfo() {
            var tipscontent = "您正在执行清除客户检查数据，请确认是否继续！";
            art.dialog({
                id: 'artDialogID',
                content: tipscontent,
                lock: true,
                zIndex: 500,
                fixed: true,
                opacity: 0.3,
                button: [{
                    name: '确定清除',
                    title: '提示',
                    callback: function () {
                        InitCustomExamItemPage_IsSetDefaultValue(gPageCustomExamDataMsg, 0); // 清除该客户的检查数据 
                        return true;
                    }, focus: true
                }, {
                    name: '取消'
                }]
            });
        }

        /// <summary>
        /// 根据获得的数据，初始化页面显示的内容
        /// </summary>
        /// <param name="msg">该客户的收费项目，体检项目，及对应的特征词数据</param>
        /// <param name="_IsSetDefaultValue">用于标记是否加载默认选项的信息ID 0：不绑定默认值 1：绑定默认值（已经小结的情况下，加载小结后的信息） 2：仅绑定默认值 </param>
        function InitCustomExamItemPage_IsSetDefaultValue(msg, _IsSetDefaultValue) {

            var NoFeeItemTempleteContent = jQuery('#NoFeeItemTemplete').html(); //暂无交费项目或体检项目模版

            // 将是否加载默认选项的值设置到隐藏变量中
            jQuery("#IsSetDefaultValue").val(_IsSetDefaultValue);

            var FeeItemCount = 0;  // 记录收费项目的个数
            if (msg == null || msg == "" || parseInt(msg.totalCount) <= 0) {

                jQuery('#TipsMessage').html(NoFeeItemTempleteContent);             //显示没有收费项目的提示信息
                jQuery('#TipsArea').show();
                IsCanShowDetailInfo = false;
                if (IsCanShowDetailInfo == false) {
                    jQuery("#ExamInfoTabDetail4").hide(); // 隐藏图片上传控件
                    jQuery("#PACSImageUploadAreaContent").hide(); // 隐藏图片上传控件
                }
            }
            else if (msg != null && msg != "" && parseInt(msg.totalCount) > 0) {
                IsCanShowDetailInfo = true;
                var newcontent = "";
                // 从模版中读取数据加载列表
                var feeTemplateContent = jQuery('#FeeDataListTemplete').html();         //收费项目模版
                var examTemplateContent = jQuery('#ExamDataListTemplete').html();       //体检项目模版

                var InputTextTempleteContent = jQuery('#InputTextTemplete').html();     //文本输入框模版
                var RadioTempleteContent = jQuery('#RadioTemplete').html();             //单选输入模版
                var CheckboxTempleteContent = jQuery('#CheckboxTemplete').html();       //多选输入模版
                var AutoCalculTempleteContent = jQuery('#AutoCalculTemplete').html();   //自动计算相关数据模版

                var ExamItemResultTextTemplete01Content = jQuery('#ExamItemResultTextTemplete01').html();       //单行体检项目结果显示列表
                var ExamItemResultTextTemplete02Content = jQuery('#ExamItemResultTextTemplete02').html();       //多行体检项目结果显示列表
                var ExamItemSymptomResultSameRowRateTempleteContent = jQuery('#ExamItemSymptomResultSameRowRateTemplete').html(); //SymCols与TextboxRows 在同一行
                var ExamItemSymptomResultDiffRowRateTempleteContent = jQuery('#ExamItemSymptomResultDiffRowRateTemplete').html(); //SymCols与TextboxRows 在不同行

                var ExamItemResultSameRowRateTempleteContent = jQuery('#ExamItemResultSameRowRateTemplete').html(); //SymCols与TextboxRows 在同一行 体检项目结果值模版
                var SectionSummaryTextTempleteContent = jQuery('#SectionSummaryTextTemplete').html();               //科室小结模版
                var ExamDoctorSelectTempleteContent = jQuery('#ExamDoctorSelectTemplete').html();                   //检查医生模版
                var ExamOperatorButtonTempleteContent = jQuery('#ExamOperatorButtonTemplete').html();               //操作按钮模版

                var CurrFeeItemID = 0; //在加载检查项目使用
                var CurrExamItemID = 0; //在加载体征词时使用
                // 加载收费项目
                jQuery(msg.dataList0).each(function (i, feeitem) {

                    FeeItemCount++;  // 记录收费项目的个数
                    if (feeTemplateContent == undefined) { return false; }

                    newcontent +=
                             feeTemplateContent.replace(/@FeeName/gi, feeitem.FeeName)
                            .replace(/@ID_Fee/gi, feeitem.ID_Fee)
                            .replace(/@ID_CustFee/gi, feeitem.ID_CustFee)
                            ;
                    CurrFeeItemID = 0; //在加载检查项目使用
                    // 加载检查项目列表
                    var tmpBGStyleNo = 1; // 背景样式 20140321 by wtang
                    jQuery(msg.dataList1).each(function (j, examitem) {

                        // 这里可以判断出是否已经加载完成，如果已经加载完，则继续下一条
                        if (CurrFeeItemID > 0 && CurrFeeItemID != examitem.ID_Fee) {
                            return true;
                        }

                        if (feeitem.ID_Fee == examitem.ID_Fee) {

                            CurrFeeItemID = examitem.ID_Fee; //如果进入了这个循环
                            var IsReadOnly = "";
                            var IsShowThisText = "";
                            var ExamItemSymptom = "";

                            if (examitem.GetResultWay == "N") {
                                IsReadOnly = ""; // " readonly = readonly ";
                                IsShowThisText = " style='display:none;' ";
                                ExamItemSymptom =
                                                InputTextTempleteContent.replace(/@InputTextID/gi, examitem.ID_Fee + "_" + examitem.ID_ExamItem)
                                                .replace(/@ExamItemID/gi, examitem.ID_ExamItem)
                                                .replace(/@InputTextExamItemUnit/gi, examitem.ExamItemUnit == 'NULL' ? '' : examitem.ExamItemUnit)
                                                .replace(/@CalcExpression/gi, examitem.Is_AutoCalc == "True" ? " onblur='" + examitem.CalcExpression + "' " : "")
                                                ;
                                if (examitem.Is_AutoCalc == "True") {
                                    // 加载体征词列表
                                    jQuery(msg.dataList2).each(function (k, symptonitem) {
                                        // 判断是否同一个检查项目
                                        if (symptonitem.ID_ExamItem == examitem.ID_ExamItem && symptonitem.ID_Fee == examitem.ID_Fee) {
                                            // 20131031 添加病症级别
                                            ExamItemSymptom += AutoCalculTempleteContent.replace(/@AutoCalculID/gi, symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "_" + symptonitem.ID_Symptom)
                                                    .replace(/@NumOperSign/gi, symptonitem.NumOperSign)
                                                    .replace(/@AutoCalculValue/gi, symptonitem.ID_Symptom)
                                                    .replace(/@AutoCalculNum/gi, jQuery('#lblSex').html() == "男" ? symptonitem.NumMale : symptonitem.NumFemale)
                                                    .replace(/@SymptomName/gi, symptonitem.SymptomName)
                                                    .replace(/@Is_Checked/gi, symptonitem.Is_Default == "True" ? " checked='checked' " : "")
                                                    .replace(/@Is_Default/gi, symptonitem.Is_Default == "True" ? "True" : "False")
                                                    .replace(/@InputTextID/gi, symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem)
                                                    .replace(/@DiseaseLevel/gi, symptonitem.DiseaseLevel)
                                                    ;

                                            // NumMale 134 3821 9417
                                        }
                                    });
                                    ExamItemSymptom += " <span id='spanShowTxt_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem + "'>&nbsp;</span>";
                                }
                            } else {

                                IsReadOnly = " ";
                                IsShowThisText = " ";
                                CurrExamItemID = 0; //在加载体征词时使用

                                // 加载体征词列表
                                jQuery(msg.dataList2).each(function (k, symptonitem) {

                                    // 这里可以判断出是否已经加载完成，如果已经加载完，则跳出循环
                                    if (CurrExamItemID > 0 && CurrExamItemID != symptonitem.ID_ExamItem) {
                                        return false;
                                    }

                                    if (symptonitem.ID_ExamItem == examitem.ID_ExamItem && symptonitem.ID_Fee == examitem.ID_Fee) {

                                        CurrExamItemID = symptonitem.ID_ExamItem;

                                        // 先判断是否是文本输入框
                                        if (examitem.GetResultWay == "C") {
                                            if (symptonitem.IS_Banned == "True") {
                                                var hiddendivid = jQuery("#HiddenSymptomDivID").val();
                                                if (hiddendivid == "") {
                                                    hiddendivid = symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "_" + symptonitem.ID_Symptom;
                                                } else {
                                                    hiddendivid = hiddendivid + "," + symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "_" + symptonitem.ID_Symptom;
                                                }

                                                jQuery("#HiddenSymptomDivID").val(hiddendivid); // 将要隐藏的体征词ID记录到隐藏域中
                                            }

                                            // 判断是否是多选输入框
                                            if (examitem.Is_SymMultiValue == "True") {
                                                ExamItemSymptom +=
                                                    CheckboxTempleteContent.replace(/@SymptomCheckboxParentID/gi, symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem)
                                                    .replace(/@SymptomCheckboxID/gi, symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "_" + symptonitem.ID_Symptom)
                                                    .replace(/@CheckboxValue/gi, symptonitem.ID_Symptom)
                                                    .replace(/@SymptomCheckboxShowLabel/gi, symptonitem.SymptomName)
                                                    .replace(/@SymptomDescribe/gi, jQuery.trim(symptonitem.SymptomDescribe))
                                                    .replace(/@DiseaseLevel/gi, symptonitem.DiseaseLevel)
                                                    ;
                                            }
                                            else  // 否则视为单选输入框
                                            {
                                                ExamItemSymptom +=
                                                    RadioTempleteContent.replace(/@SymptomRadioParentID/gi, symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem)
                                                    .replace(/@SymptomRadioID/gi, symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "_" + symptonitem.ID_Symptom)
                                                    .replace(/@RadioValue/gi, symptonitem.ID_Symptom)
                                                    .replace(/@SymptomRadioShowLabel/gi, symptonitem.SymptomName)
                                                    .replace(/@SymptomDescribe/gi, jQuery.trim(symptonitem.SymptomDescribe))
                                                    .replace(/@DiseaseLevel/gi, symptonitem.DiseaseLevel)
                                                    ;
                                            }
                                        }
                                    }
                                });  // 加载体征词 end
                            }
                            // 体检项目结果框显示 同行
                            if (examitem.Is_SameRow == "True") {
                                newcontent +=
                                        examTemplateContent.replace(/@ExamItemSymptomAndResultDivInfo/gi, ExamItemSymptomResultSameRowRateTempleteContent)
                                        .replace(/@BGStyleNo/gi, tmpBGStyleNo);

                                newcontent = newcontent.replace(/@ExamItemResultTempleteDivInfo/gi,
                                        ExamItemResultSameRowRateTempleteContent
                                        );
                            } else {         // 不同行
                                newcontent +=
                                        examTemplateContent.replace(/@ExamItemSymptomAndResultDivInfo/gi,
                                        ExamItemSymptomResultDiffRowRateTempleteContent)
                                        .replace(/@BGStyleNo/gi, tmpBGStyleNo);
                            }
                            tmpBGStyleNo = tmpBGStyleNo == 1 ? 2 : 1; // 背景样式 20140321 by wtang
                            // 结果框显示 多行 或 单行
                            if (examitem.TextboxRows == "1") {
                                newcontent = newcontent.replace(/@ExamItemResultDivInfo/gi,
                                                    ExamItemResultTextTemplete01Content.replace(/@ExamItemResultText01/gi, examitem.ID_Fee + "_" + examitem.ID_ExamItem)
                                                    .replace(/@IsReadOnly/gi, IsReadOnly).replace(/@IsShowThisText/gi, IsShowThisText))
                                                    ;
                            } else {
                                newcontent = newcontent.replace(/@ExamItemResultDivInfo/gi,
                                                    ExamItemResultTextTemplete02Content.replace(/@ExamItemResultText02/gi, examitem.ID_Fee + "_" + examitem.ID_ExamItem))
                                                    .replace(/@ExamItemResultRows/gi, examitem.TextboxRows)
                                                    ;
                            }

                            newcontent =
                                    newcontent.replace(/@ExamItemSymptomDivInfo/gi, ExamItemSymptom)
                                    .replace(/@ExamItemName/gi, examitem.ExamItemName)
                                    .replace(/@ID_Fee/gi, examitem.ID_Fee)
                                    .replace(/@Is_EntrySectSum/gi, examitem.Is_EntrySectSum)
                                    .replace(/@ID_ExamItem/gi, examitem.ID_Fee + "_" + examitem.ID_ExamItem)
                                    .replace(/@TextboxRows/gi, examitem.TextboxRows)
                                    .replace(/@SymCols/gi, examitem.SymCols)
                                    .replace(/@GetResultWay/gi, examitem.GetResultWay)
                                    ;

                        }
                    }); // 加载检查项目 end
                    if (feeitem.ID_ExamDoctor != "") {
                        // 读取检查医生模版（每个体检项目对应一个体检医生）
                        newcontent += ExamDoctorSelectTempleteContent.replace(/@FeeItemID/gi, feeitem.ID_Fee)
                                 .replace(/@ID_Fee/gi, feeitem.ID_Fee)
                                 .replace(/@ExamDoctorName/gi, feeitem.ExamDoctorName)
                                 .replace(/@ExamDoctorID/gi, feeitem.ID_ExamDoctor);
                    } else {
                        // 读取检查医生模版（每个体检项目对应一个体检医生）
                        newcontent += ExamDoctorSelectTempleteContent.replace(/@FeeItemID/gi, feeitem.ID_Fee)
                                 .replace(/@ID_Fee/gi, feeitem.ID_Fee)
                                 .replace(/@ExamDoctorName/gi, "--")
                                 .replace(/@ExamDoctorID/gi, "0");
                    }

                });                             // 加载收费项目 end
                var examresult = "";  // 检查结果
                if (FeeItemCount > 0) {
                    if (SectionSummaryTextTempleteContent == null || SectionSummaryTextTempleteContent == undefined) {
                        return;
                    }
                    // 读取科室小结模版
                    examresult += SectionSummaryTextTempleteContent.replace(/@SectionSummaryArea/gi, "SectionSummary").replace(/@PositiveSummaryArea/gi, "PositiveSummary")
                            .replace(/@ButDefault/gi, "ButDefault")
                            .replace(/@ButCollect/gi, "ButCollect")
                            .replace(/@ButClear/gi, "ButClear")
                            .replace(/@ButSave/gi, "ButSave")
                            .replace(/@ButCheck/gi, "ButCheck")
                            .replace(/@ButUnCheck/gi, "ButUnCheck")
                            .replace(/@tempSelectedUserID/gi, "tempSelectedUserID")
                            .replace(/@spanUser/gi, "spanUser")
                            .replace(/@spanSelectUser/gi, "spanSelectUser")
                            .replace(/@txtUserInputCode/gi, "txtUserInputCode")
                            .replace(/@idSelectUser/gi, "idSelectUser")
                            .replace(/@nameSelectUser/gi, "nameSelectUser")
                            .replace(/@QuickQueryUserTable/gi, "QuickQueryUserTable")
                            .replace(/@QuickQueryUserTableData/gi, "QuickQueryUserTableData");


                    //                    // 读取操作按钮模版
                    //                    newcontent += ExamOperatorButtonTempleteContent.replace(/@ButDefault/gi, "ButDefault")
                    //                            .replace(/@ButCollect/gi, "ButCollect")
                    //                            .replace(/@ButClear/gi, "ButClear")
                    //                            .replace(/@ButSave/gi, "ButSave")
                    //                            .replace(/@ButCheck/gi, "ButCheck")
                    //                            .replace(/@ButUnCheck/gi, "ButUnCheck");

                } else {
                    jQuery('#TipsMessage').html(NoFeeItemTempleteContent);             //显示没有收费项目的提示信息
                    jQuery('#TipsArea').show();

                    IsCanShowDetailInfo = false;
                    if (IsCanShowDetailInfo == false) {
                        jQuery("#ExamInfoTabDetail4").hide(); // 隐藏图片上传控件
                        jQuery("#PACSImageUploadAreaContent").hide(); // 隐藏图片上传控件
                    }
                }

                if (newcontent != '') {


                    jQuery('#TipsArea').hide();
                    jQuery('#CustExamList').html(newcontent);
                    jQuery('#CustExamResult').html(examresult);
                    jQuery('#CustExamResult').show()

                    // 隐藏，显示打印条码的按钮
                    ShowHideIsPrintSectionBarCode();

                    jQuery(".ClassSymtomTextInput").attr("maxlength", "60");

                    jQuery(".j-autoHeight").autoHeight(); // 自适应高度
                    jQuery(".j-hiddenAway").hiddenAway(); // 隐藏显示

                    // 隐藏被禁用的体征词
                    HiddenBannedSymptom();

                    // 读取科室小结的信息（小结的时间，和小结的编号）
                    jQuery(msg.dataList3).each(function (m, sectionsummaryitem) {

                        jQuery("#ID_CustExamSection").val(sectionsummaryitem.ID_CustExamSection);

                        jQuery("#SectionSummaryDate").val(sectionsummaryitem.SectionSummaryDate);   //    小结时间
                        jQuery("#ID_SummaryDoctor").val(sectionsummaryitem.ID_SummaryDoctor);       //    医生编号
                        jQuery("#SummaryDoctorName").val(sectionsummaryitem.SummaryDoctorName);     //    医生姓名
                        jQuery("#ID_Typist").val(sectionsummaryitem.ID_Typist);     //    录入人编号
                        jQuery("#TypistName").val(sectionsummaryitem.TypistName);   //    录入人姓名
                        // 防止在点缺省的时候，重置了已经获取到的TypistDate,从而导致在缺省后，不能正常进行保存。 20150507 by wtang
                        if (jQuery("#TypistDate").val() == "") {
                            jQuery("#TypistDate").val(sectionsummaryitem.TypistDate);   //    录入时间
                        }

                        jQuery("#Is_GiveUp").val(sectionsummaryitem.IS_giveup);     //    是否弃检

                        // 如果录入人员不为空的情况
                        if (sectionsummaryitem.TypistName != "" && jQuery("#SummaryDoctorName").val() != "") {
                            var tmpTypistDate = sectionsummaryitem.TypistDate.split(" ");
                            jQuery("#spanTypistName").html(jQuery("#TypistName").val());
                            jQuery("#spanTypistDate").html(tmpTypistDate[0]);
                        }

                        var tmpDate = sectionsummaryitem.SectionSummaryDate.split(" ");
                        if (tmpDate[0] != undefined && tmpDate[0] != "") {
                            jQuery("[name='Lab_ExamDate']").html(tmpDate[0]);    //如果已经进行了分科检查，则显示小结的时间
                        }
                        if (sectionsummaryitem.SummaryDoctorName != "") {
                            SetDefaultExamDoctor(sectionsummaryitem.ID_SummaryDoctor, sectionsummaryitem.SummaryDoctorName); // 如果已经保存过，则加载保存后的检查医生
                        } else if (jQuery("#HiddenVocationType").val() == "1") { // 如果是医生，则自动选项为默认选项
                            SetDefaultExamDoctor(jQuery("#HiddenUserID").val(), jQuery("#HiddenUserName").val()); // 设置默认医生
                        }

                        //                        jQuery("[name='txtExamDoctor']").val(sectionsummaryitem.ID_SummaryDoctor);      // 医生ID
                        //                        jQuery("[name='spanExamDoctor']").html(sectionsummaryitem.SummaryDoctorName);   // 医生姓名
                        //                        jQuery("[name='txtExamDoctor']").attr("ExamDoctor", sectionsummaryitem.SummaryDoctorName); // 医生姓名


                        // 暂时不做指定操作人员的绑定，可以由具有该科室操作权限的所有人员进行操作。 20130729 by WTang
                        //                        if (sectionsummaryitem.SummaryDoctorName == "" && sectionsummaryitem.TypistName != "") {
                        //                            if (sectionsummaryitem.ID_Typist != jQuery("#HiddenUserID").val()) {
                        //                                CtrlButtonDisabled(3, false); //禁用所有操作按钮
                        //                                art.dialog({
                        //                                    content: '【提示】该客户已被(' + sectionsummaryitem.TypistName + ')绑定，只能由(' + sectionsummaryitem.TypistName + ')进行操作！<br/>【备注】如果，需要由其他人员操作，请先解除绑定！',
                        //                                    button: [{
                        //                                        name: '确定'
                        //                                    }]
                        //                                });
                        //                                return;
                        //                            }
                        //                        }

                        //                        if (sectionsummaryitem.SummaryDoctorName != "" && sectionsummaryitem.TypistName != "") {
                        //                            if (sectionsummaryitem.ID_Typist != jQuery("#HiddenUserID").val()) {
                        //                                CtrlButtonDisabled(3, false); //禁用所有操作按钮
                        //                                art.dialog({
                        //                                    content: '【提示】该客户已由(' + sectionsummaryitem.TypistName + ')做了分检，只能由(' + sectionsummaryitem.TypistName + ')进行操作！<br/>【备注】如果，需要由其他人员操作，请先解除绑定！',
                        //                                    button: [{
                        //                                        name: '确定'
                        //                                    }]
                        //                                });
                        //                                return;
                        //                            }
                        //                        }

                        // 在首次进入分科检查的情况下，进行绑定
                        if (sectionsummaryitem.SummaryDoctorName == "" && sectionsummaryitem.TypistName == "") {
                            // 绑定用户，绑定后只能由该医生或护士进行体检，如果需要其他人员操作，需要进行解除绑定操作 (绑定)
                            BandingCustomerSectionExamInfo();
                        }
                    });

                    // 根据获得的数据，初始化页面隐藏域中的默认值 , 用于对比是否全部是默认小结
                    SetCustomExamHiddenDefaultText(msg);

                    // 初始化 体征词相关事件 文本变化事件（change,keyup） 多选框点击事件 (click) 单选框点击事件(click)
                    InitSymptomEvent(msg);



                    // 显示医生下拉框中的默认值  这里需要 初始化下拉列表框 后再赋值
                    jQuery(msg.dataList3).each(function(m, sectionsummaryitem) {
                        // 如果检查医生名字不为空时，加载检查医生信息
                        if (sectionsummaryitem.SummaryDoctorName != "") {
                            if (sectionsummaryitem.ID_SummaryDoctor != "") {
                                ShowQuickSelectUser(sectionsummaryitem.ID_SummaryDoctor, sectionsummaryitem.SummaryDoctorName);

                                jQuery("#idLastSaveExamDoctor").val(sectionsummaryitem.ID_SummaryDoctor); // 加载数据时，将数据库保存的检查医生ID赋值给“上次保存的检查医生”，用于判断医生是否发生了变化 20141120 by wtang 
                            }
                            //                            jQuery("#ResultSummaryDoctor").find("option").removeAttr("selected");
                            //                            jQuery("#ResultSummaryDoctor").attr("value", sectionsummaryitem.ID_SummaryDoctor); // 医生编号

                            //                            jQuery("#s2id_ResultSummaryDoctor .select2-choice span").text(sectionsummaryitem.SummaryDoctorName); //显示选中项的文本
                        } else {

                            var exp = new Date(); // 设置当前时间，只有当天才有效 （在取出时，先判断时间是否一致。）
                            var SectionID = jQuery('#txtSectionID').val();
                            var OperatorID = jQuery('#CookieUserID').val();
                            var CurrentDate = exp.getFullYear() + "-" + exp.getMonth() + '-' + exp.getDay();
                            var CookedSaveCurrentDate = GetCookie('LastExamDoctorSaveDate');
                            var CookedSaveCurrentSection = GetCookie('LastExamDoctorSection');
                            var CookedSaveCurrentOperatorID = GetCookie('LastExamDoctorSaveOperatorID');

                            if (CookedSaveCurrentDate != CurrentDate || OperatorID != CookedSaveCurrentOperatorID) {
                                return;
                            }

                            SetCookie('LastExamDoctorSaveDate', CurrentDate);
                            SetCookie("LastExamDoctorSection", SectionID);
                            SetCookie('LastExamDoctorSaveOperatorID', OperatorID);

                            var tmpLastExamDoctorID = GetCookie("LastExamDoctorID_" + SectionID);
                            var tmpLastExamDoctorName = GetCookie("LastExamDoctorName_" + SectionID);
                            //alert(tmpLastExamDoctorID + " -- " + tmpLastExamDoctorName);
                            if (tmpLastExamDoctorID != null && tmpLastExamDoctorID != undefined && tmpLastExamDoctorID != "" && tmpLastExamDoctorID != "0" &&
                                tmpLastExamDoctorName != null && tmpLastExamDoctorName != undefined && tmpLastExamDoctorName != "") {
                                ShowQuickSelectUser(tmpLastExamDoctorID, tmpLastExamDoctorName); // 显示上次选择的医生
                                SetSelectedUserCallBack(); // 设置收费项目的检查医生信息
                            }
                        }

                    });

                    try {
                        // 初始化下拉列表框
                        //jQuery('.content select').select2();
                    } catch (e) { }

                    // 初始化按钮是否可以使用 , （在页面元素加载完成后，调用一次按钮控制函数）
                    InitButtomDisabled();
                }
            } else {
                jQuery('#TipsMessage').html("");
                jQuery('#TipsArea').hide();
            }


        }

        /// <summary>
        /// 隐藏被禁用的体征词
        /// </summary>
        function HiddenBannedSymptom() {
            var hiddendivid = jQuery("#HiddenSymptomDivID").val(); // 取出要隐藏的体征词ID
            if (hiddendivid == "") { return; }
            var hiddenDivIDArray = hiddendivid.split(",");
            for (var i = 0; i < hiddenDivIDArray.length; i++) {
                if (hiddenDivIDArray[i] != "") {
                    jQuery("#SymptomDiv_" + hiddenDivIDArray[i]).hide();
                }
            }
        }


        /// <summary>
        /// 初始化按钮是否可以使用
        /// </summary>
        function InitButtomDisabled() {

            if (jQuery("#IS_InitButtom").val() == "True") { return; }

            var isInitButton = 0; // 是否已经初始化按钮


            // 如果分科检查已经提交，则禁用其他按钮
            if (jQuery("#Is_SectionCheck").val() == "True") {
                CtrlButtonDisabled(1, false);
                isInitButton = 1; // 是否已经初始化按钮
            }

            // 分科锁定
            if (jQuery('#Is_SectionLock').val() == "True") {

                CtrlButtonDisabled(3, false); //禁用所有操作按钮
                isInitButton = 1; // 是否已经初始化按钮
                art.dialog({
                    id: 'artDialogID',
                    lock: true,
                    fixed: true,
                    zIndex: 500,
                    opacity: 0.3,
                    content: '【提示】该客户已被分科锁定，如果需修改，请先解除分科锁定！',
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
            }
            // 分科弃检
            else if (jQuery('#Is_GiveUp').val() == "True") {

                CtrlButtonDisabled(3, false); //禁用所有操作按钮
                isInitButton = 1; // 是否已经初始化按钮
                art.dialog({
                    id: 'artDialogID',
                    lock: true,
                    fixed: true,
                    zIndex: 500,
                    opacity: 0.3,
                    content: '【提示】该客户已弃检，请恢复该科室的检查后再做分科检查！',
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
            }
            if (isInitButton == 1) {
                // 设置为已经初始化按钮
                jQuery("#IS_InitButtom").val("True");
            }
        }

        /// <summary>
        /// 初始化体征词事件
        /// </summary>
        function InitSymptomEvent(msg) {
            // 将体征所在输入框中的值，显示到检查项的结果输入文本框中。
            jQuery('.ClassSymtomTextInput').change(function () {
                SyncShowSymptomInputData(this);
            });
            jQuery('.ClassSymtomTextInput').keyup(function () {
                SyncShowSymptomInputData(this);
            });

            // 将体征复选框中的值，显示到检查项的结果输入文本框中。
            jQuery('.ClassSymptomCheckBox').change(function () {
                SyncShowSymptomCheckboxData(this);
            });

            // 将体征单选框中的值，显示到检查项的结果输入文本框中。
            jQuery('.ClassSymptomRadio').change(function () {
                SyncShowSymptomRadioData(this);
            });

            // 默认设置为不是点击清除按钮进入的 20130813 by WTang
            jQuery("#IS_NotUseLastSaveData").val("0");
            // 0：不绑定默认值（点击清除按钮） 1：绑定默认值（已经小结的情况下，加载小结后的信息） 2：仅绑定默认值
            if (jQuery("#IsSetDefaultValue").val() == 1) {

                if (jQuery("#SummaryDoctorName").val() == "" || jQuery("#SummaryDoctorName").val() == "NULL") {
                    // 加载体征词默认选项 
                    // SetSymptomDefaultValue(msg.dataList2); // 经过讨论后，确定在页面初始化时，先不加载默认选项。 20130729 by WTang

                    // 如果是医生，则自动选项为默认选项
                    if (jQuery("#HiddenVocationType").val() == "1") {
                        SetDefaultExamDoctor(jQuery("#HiddenUserID").val(), jQuery("#HiddenUserName").val()); // 设置默认医生
                    }
                    AutoFinishSaveSummary(); // 调用自动保存小结结论函数（在函数内部判断哪些科室需要自动保存小结--目前只有营养科） 20130927 by WTang

                } else {
                    // 20131231 调用 GetCustomerExamLastSaveData 移除到判断外面
                }

                // 读取并加载小结的内容,及各个检查项目的值
                GetCustomerExamLastSaveData();

            } else if (jQuery("#IsSetDefaultValue").val() == 2) {
                // 加载体征词默认选项
                jQuery("#IS_NotUseLastSaveData").val("1");  // 标志如果是修改的情况下，没有使用已保存的数据（由于操作人员点击了缺省按钮操作）则后台需要将已保存的检查项目，体征词等进行删除。
                SetSymptomDefaultValue(msg.dataList2);
            }
            // 在清除操作时，传入的是0  20130813 by WTang
            else if (jQuery("#IsSetDefaultValue").val() == 0) {
                jQuery("#IS_NotUseLastSaveData").val("1");  // 标志如果是修改的情况下，没有使用已保存的数据（由于操作人员进行了清除操作）则后台需要将已保存的检查项目，体征词等进行删除。
            }


        }


        function AutoFinishSaveSummary() {

            // 弃检
            if (jQuery('#Is_GiveUp').val() == "True") {
                return;
            }
            // 分科锁定
            if (jQuery('#Is_SectionLock').val() == "True") {
                return;
            }

            // 如果正在执行自动加载函数，则退出本次调用
            if (jQuery("#IsAutoFinishSaveAndSubmit").val() == "1") {
                return;
            }

            var SectionID = jQuery('#txtSectionID').val();

            jQuery("#IsAutoFinishSaveAndSubmit").val("0"); // 先标记为不自动完成小结 
            // 只有营养科进行自动小结
            if (SectionID != "431") {
                return;
            }
            // 在还没有小结的情况下，自动汇总小结，并进行提交
            if (jQuery("#IsSetDefaultValue").val() == 1) {
                if (jQuery("#SummaryDoctorName").val() == "" || jQuery("#SummaryDoctorName").val() == "NULL") {

                    jQuery("#IsAutoFinishSaveAndSubmit").val("1"); // 标记为自动完成小结 

                    InitCustomExamItemPage_IsSetDefaultValue(gPageCustomExamDataMsg, 2);
                    //                    GetCustomerExamSectionSummaryValue();
                    //                    SaveCustomerSectionSummaryConfirm();
                    //                    UpdateSectionSummaryCheckState(1);

                }

            }
        }

        /// <summary>
        /// 根据获得的数据，初始化页面隐藏域中的默认值
        /// </summary>
        /// <param name="msg">该客户的收费项目，体检项目，及对应的特征词数据</param>
        function SetCustomExamHiddenDefaultText(msg) {

            // 遍历体征词列表
            jQuery(msg.dataList2).each(function (k, symptonitem) {
                if (symptonitem.Is_Default == "True") {
                    jQuery("#txtDefaultValueResult_" + symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem).val(symptonitem.SymptomName);
                    // ShowSystemDialog(jQuery("#txtDefaultValueResult_" + symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem).val());
                }
            });
        }


        /// <summary>
        /// 将体征所在输入框中的值，显示到检查项的结果输入文本框中。
        /// </summary>
        /// <param name="obg">操作的文本框对象</param>
        function SyncShowSymptomInputData(obg) {

            var resultvalue = "";
            if (jQuery("#" + obg.name).val() != "") {
                resultvalue = jQuery("#" + obg.name).val() + jQuery("#" + obg.name.replace(/txtSym/gi, "txtSymUnit")).html();
            }
            jQuery("#" + obg.name.replace(/txtSym/gi, "txtResult")).val(resultvalue);
        }


        /// <summary>
        /// 将体征单选框中的值，显示到检查项的结果输入文本框中。
        /// </summary>
        /// <param name="obj">操作的单选框对象</param>
        function SyncShowSymptomRadioData(obj) {

            var symRadioHidValue = "";
            var symRadioShowText = "";
            var symchkMaxDiseaseLevel = 0;   //当前的最大病症级别

            jQuery("input[name='" + obj.name + "']:radio:checked").each(function () {
                symRadioHidValue = jQuery(this).val(); //获取ID值
                symRadioShowText = jQuery("#" + this.name.replace(/radioSym/gi, "lab") + "_" + jQuery(this).val()).attr("SymptomName"); // +SepBetweenExamItems; //获取显示的文本

                // 判断描述是否为空，不为空时，显示描述信息 20130926 by WTang 
                var tmpSymptomDescribe = jQuery("#" + this.name.replace(/radioSym/gi, "lab") + "_" + jQuery(this).val()).attr("SymptomDescribe");
                
                if (tmpSymptomDescribe != undefined && tmpSymptomDescribe != "" && tmpSymptomDescribe != "　") {
                    symRadioShowText = tmpSymptomDescribe;
                }
                symchkMaxDiseaseLevel = jQuery("#" + this.name.replace(/radioSym/gi, "lab") + "_" + jQuery(this).val()).attr("DiseaseLevel");
            });

            jQuery("#" + obj.name.replace(/radioSym/gi, "txtHidValueResult")).val(SepBetweenSymptoms_SplitID + symRadioHidValue + SepBetweenSymptoms_SplitID);
            jQuery("#" + obj.name.replace(/radioSym/gi, "txtResult")).val(symRadioShowText);
            jQuery("#" + obj.name.replace(/radioSym/gi, "DiseaseLevel")).val(symchkMaxDiseaseLevel);
        }

        /// <summary>
        /// 将体征复选框中的值，显示到检查项的结果输入文本框中。
        /// </summary>
        /// <param name="obj">操作的复选框对象</param>
        function SyncShowSymptomCheckboxData(obj) {

            var symchkHidValueList = "";
            var symchkShowTextList = "";
            var symchkPositiveTextList = ""; // 该检查项目的阳性结果
            var symchkMaxDiseaseLevel = 0;   //当前的最大病症级别

            symchkHidValueList = jQuery("#" + obj.name.replace(/ckbSym/gi, "txtHidValueResult")).val();
            if (symchkHidValueList == "") symchkHidValueList += SepBetweenSymptoms_SplitID;
            if (jQuery(obj).attr("checked")) {
                // 判断不进行重复添加体征词ID 20130926 by WTang 
                var singleValue = SepBetweenSymptoms_SplitID + jQuery(obj).val() + SepBetweenSymptoms_SplitID;
                if (symchkHidValueList.indexOf(singleValue) < 0) {
                    symchkHidValueList = symchkHidValueList + jQuery(obj).val() + SepBetweenSymptoms_SplitID;   // 获取ID值
                }
            }
            else {
                var replaceReg01 = new RegExp(SepBetweenSymptoms_SplitID + jQuery(obj).val() + SepBetweenSymptoms_SplitID, "gi"); // 移除value中对应的选项
                //symchkHidValueList = "0" + SepBetweenSymptoms_SplitID + symchkHidValueList;
                symchkHidValueList = symchkHidValueList.replace(replaceReg01, SepBetweenSymptoms_SplitID);
            }

            // 根据体征词ID，读取体征词对应的文本，并用指定的分隔符进行分割。(小结内容)
            symchkShowTextList = GetSymptomCheckboxTextByIDList(obj, symchkHidValueList, 1);
            // (阳性结果) 如果有阳性结果，则返回1，阳性结果取值与小结内容一样【Modify by WTang 20130531】
            symchkPositiveTextList = GetSymptomCheckboxTextByIDList(obj, symchkHidValueList, 2);
            // 根据体征词ID，读取体征词中最大病症级别。 20130814 by WTang
            symchkMaxDiseaseLevel = GetExamItemMaxDiseaseLevel(obj, symchkHidValueList);

            jQuery("#" + obj.name.replace(/ckbSym/gi, "txtHidValueResult")).val(symchkHidValueList);
            jQuery("#" + obj.name.replace(/ckbSym/gi, "txtPositiveResult")).val(symchkPositiveTextList);
            jQuery("#" + obj.name.replace(/ckbSym/gi, "txtResult")).val(symchkShowTextList);
            jQuery("#" + obj.name.replace(/ckbSym/gi, "DiseaseLevel")).val(symchkMaxDiseaseLevel);
        }

        /// <summary>
        /// 根据体征词ID，读取体征词对应的文本，并用指定的分隔符进行分割。
        /// </summary>
        /// <param name="obj">操作的复选框对象</param>
        /// <param name="symchkHidValueList">现在的复选框对应的ID字符串</param>
        /// <param name="type">1：小结内容 2：阳性结果(如果有阳性结果，则返回1，整个结果取值与小结内容一样【Modify by WTang 20130531】)</param>
        function GetSymptomCheckboxTextByIDList(obj, symchkHidValueList, type) {
            var SymptomChkText = "";
            var SymptomChkArray = new Array();
            SymptomChkArray = symchkHidValueList.split(SepBetweenSymptoms_SplitID);
            var arraylength = 0;
            if (SymptomChkArray.length > 0) {
                arraylength = SymptomChkArray.length - 2;
            }
            var chklableid = "";
            var currNumber = 0; // 记录当前的编号
            for (var i = 0; i <= arraylength; i++) {

                if (SymptomChkArray[i] != "" && SymptomChkArray[i] != null) {

                    chklableid = obj.name.replace(/ckbSym/gi, "lab") + "_" + SymptomChkArray[i];

                    // 获取 阳性结果 时，根据 病症级别 DiseaseLevel 进行判断的。
                    if (type == 2) {
                        if (jQuery("#" + chklableid).attr("DiseaseLevel") == "0") {
                            continue;
                        }
                        if (jQuery("#" + chklableid).attr("SymptomName") == "" || jQuery("#" + chklableid).attr("SymptomName") == "NULL") {
                            continue;
                        }
                        // 如果这里存在阳性结果，则回1，表示改检查项目存在阳性结果，可以直接取该体检项目小结作为阳性结果(Modify by WTang 20130531)
                        return "1";
                    }

                    var tmpSymptomName = jQuery("#" + chklableid).attr("SymptomDescribe");
                    if (tmpSymptomName == undefined || tmpSymptomName == "" || tmpSymptomName == "　") {
                        tmpSymptomName = jQuery("#" + chklableid).attr("SymptomName");
                    }

                    if (tmpSymptomName != undefined && jQuery.trim(tmpSymptomName) != "") {
                        currNumber++; // 当前编号 ( +1 )
                        SymptomChkText += GetCountNumberBetweenItem(NoBetweenSympotms, currNumber, arraylength) + tmpSymptomName; //获取显示的文本

                        // 拼接分割符号
                        if (i < arraylength) {
                            SymptomChkText += SepBetweenSymptoms;
                        }
                        else {
                            // SymptomChkText += SepBetweenExamItems;
                        }
                    }
                }
            }
            return SymptomChkText;
        }


        /// <summary>
        /// 根据体征词ID，读取体征词中最大病症级别。 20130814 by WTang
        /// </summary>
        /// <param name="obj">操作的复选框对象</param>
        /// <param name="symchkHidValueList">现在的复选框对应的ID字符串</param>
        function GetExamItemMaxDiseaseLevel(obj, symchkHidValueList) {
            var SymptomChkText = "";
            var SymptomChkArray = new Array();
            SymptomChkArray = symchkHidValueList.split(SepBetweenSymptoms_SplitID);
            var arraylength = 0;
            if (SymptomChkArray.length > 0) {
                arraylength = SymptomChkArray.length - 2;
            }
            var chklableid = "";
            var currNumber = 0; // 记录当前的编号
            var currMaxDiseaseLevel = 0; //当前的最大病症级别
            var tempDiseaseLevel = 0; //临时记录病症级别
            for (var i = 0; i <= arraylength; i++) {

                if (SymptomChkArray[i] != "" && SymptomChkArray[i] != null) {

                    chklableid = obj.name.replace(/ckbSym/gi, "lab") + "_" + SymptomChkArray[i];

                    // 获取病症级别 DiseaseLevel 进行判断的。
                    tempDiseaseLevel = jQuery("#" + chklableid).attr("DiseaseLevel");
                    if (tempDiseaseLevel != "" && parseInt(tempDiseaseLevel) > currMaxDiseaseLevel) {
                        currMaxDiseaseLevel = tempDiseaseLevel;
                        continue;
                    }
                }
            }
            return currMaxDiseaseLevel;
        }

        /// <summary>
        /// 根据初始编号，获取指定数字的变化 
        /// <summary>
        function GetCountNumberBetweenItem(NoBetweenItems, currcount, totalcount) {

            // 如果编号为空，则直接返回空字符
            if (NoBetweenItems == "") {
                return "";
            }

            // 如果只有一个，则不需要编号
            if (totalcount <= 1) {
                return "";
            }
            // 定义的第一中编号
            var SpecialNumber01 = "①";
            var SpecialNumber01Array = ["", "①", "②", "③", "④", "⑤", "⑥", "⑦", "⑧", "⑨", "⑩", "⑪", "⑫", "⑬", "⑭", "⑮", "⑯", "⑰", "⑱", "⑲", "⑳"];

            var tempNumberBetweenItem = "";

            if (NoBetweenItems == SpecialNumber01) {
                if (currcount <= 20) {
                    return SpecialNumber01Array[currcount];
                } else {
                    return "" + currcount + ")"; // 大于20，用 21) 这种方式代替
                }
            }
            else {
                return NoBetweenItems.replace("1", currcount); // 直接替换掉编号中的数字
            }
        }

        /// <summary>
        /// 加载体征词默认选项
        /// <summary>
        function SetSymptomDefaultValue(SymptomDataList) {
            // 加载体征词列表
            jQuery(SymptomDataList).each(function (k, symptonitem) {
                if (symptonitem.Is_Default == "True") {
                    // 先判断是否是文本输入框
                    if (symptonitem.GetResultWay == "C") {
                        // 判断是否是多选输入框
                        if (symptonitem.Is_SymMultiValue == "True") {

                            jQuery("#CustExamList input[name='ckbSym_" + symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "']:checkbox").each(function () {

                                if (jQuery(this).val() == symptonitem.ID_Symptom) {
                                    jQuery(this).attr("checked", true);
                                    jQuery(this).change();
                                }
                            });

                        }
                        else  // 否则视为单选输入框
                        {
                            jQuery("#CustExamList input[name='radioSym_" + symptonitem.ID_Fee + "_" + symptonitem.ID_ExamItem + "']:radio").each(function () {
                                if (jQuery(this).val() == symptonitem.ID_Symptom) {
                                    jQuery(this).attr("checked", true);
                                    jQuery(this).change();
                                }
                            });
                        }
                    }
                }
            });

            // 如果自动完成小结 20130927 by WTang
            if (jQuery("#IsAutoFinishSaveAndSubmit").val() == "1") {
                GetCustomerExamSectionSummaryValue();
            }

        }

        /// <summary>
        /// 读取并加载小结的内容,及各个检查项目的值
        /// <summary>
        function InitCustomExamLastSaveData() {
            
            // 初始化 放大编辑
            InitBigEditer();

            var CurrCustExamItemID = 0;      //在遍历体征词时使用
            var symchkHidValueList = "";     //隐藏的的选择ID列表值
            var symchkPositiveTextList = ""; //阳性结果
            var symchkShowTextList = "";
            var SymptomObj = null;           //体征词对象
            var symchkMaxDiseaseLevel = 0;   //当前的最大病症级别

            // 读取科室小结的信息
            jQuery(gPageCustomExamLastSaveDataMsg.dataList2).each(function (m, sectionsummaryitem) {
                jQuery("#SectionSummary").val(sectionsummaryitem.SectionSummary);           // 科室小结
                jQuery("#Last_SectionSummary").val(sectionsummaryitem.SectionSummary);      // 科室小结 同时保存到备用文本中，用于对比是否进行了改动

                jQuery("#PositiveSummary").val(sectionsummaryitem.PositiveSummary);         // 阳性结果
                jQuery("#Last_PositiveSummary").val(sectionsummaryitem.PositiveSummary);    // 阳性结果 同时保存到备用文本中，用于对比是否进行了改动

                jQuery("#Is_SectionCheck").val(sectionsummaryitem.Is_Check);

                ShowUploadedImageFile(sectionsummaryitem.ImageUrl);


                ShowDeviceResultImagesList(sectionsummaryitem.ImageUrl);

                //                else {
                //                    jQuery("#spanTypistName").html(jQuery("#HiddenUserName").val());
                //                    jQuery("#spanTypistDate").html(jQuery("#DateToday").val()); 
                //                }



            });         // end dataList2

            // 加载体征词列表
            jQuery(gPageCustomExamLastSaveDataMsg.dataList0).each(function (i, lastfeeitem) {
                if (lastfeeitem.ResultSummary == 'undefined') {
                    lastfeeitem.ResultSummary = "";
                }
                // 用于检查项目的小结 用于对比是否改动
                jQuery("#txtLastResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(lastfeeitem.ResultSummary);         // 记录检查项目的小结文本信息(用于对比是否改动)
                jQuery("#txtCustExamItemID_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(lastfeeitem.ID_CustExamItem);   // 记录检查项目的小结的ID，便于进行修改操作

                CurrCustExamItemID = 0;
                symchkHidValueList = "";
                // 循环检查项目下的体征词，如果有，则置为选中状态
                jQuery(gPageCustomExamLastSaveDataMsg.dataList1).each(function (j, lastexamitem) {

                    // 判断是否是同一个检查项目
                    if (lastfeeitem.ID_CustExamItem != lastexamitem.ID_CustExamItem) {
                        return true;
                    }
                    // 这里可以判断出是否已经加载完成，如果已经加载完，则跳出循环
                    if (CurrCustExamItemID > 0 && CurrCustExamItemID != lastexamitem.ID_CustExamItem) {
                        return false;
                    }

                    CurrCustExamItemID = lastexamitem.ID_CustExamItem;


                    // 如果是自动计算的，则需要输出选择的ID到隐藏结果值中。 20131206 by WTang 
                    if (lastfeeitem.GetResultWay == "N") {

                        jQuery("#txtSym_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(lastfeeitem.ResultNumber);
                        jQuery("#DiseaseLevel_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(lastfeeitem.DiseaseLevel);

                        if (lastfeeitem.Is_AutoCalc == "True") {

                            jQuery("#txtHidValueResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val("、" + lastexamitem.ID_Symptom + "、");

                        }
                    }

                    // 先判断是否是选择框
                    if (lastfeeitem.GetResultWay == "C") {
                        // 判断是否是多选输入框

                        if (lastfeeitem.Is_SymMultiValue == "True") {


                            jQuery("#CustExamList input[name='ckbSym_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem + "']:checkbox").each(function () {
                                if (jQuery(this).val() == lastexamitem.ID_Symptom) {

                                    jQuery(this).attr("checked", true);


                                    if (symchkHidValueList == "") symchkHidValueList += SepBetweenSymptoms_SplitID;
                                    // 判断不进行重复添加体征词ID 20130926 by WTang 
                                    var singleValue = SepBetweenSymptoms_SplitID + lastexamitem.ID_Symptom + SepBetweenSymptoms_SplitID;
                                    if (symchkHidValueList.indexOf(singleValue) < 0) {
                                        symchkHidValueList = symchkHidValueList + lastexamitem.ID_Symptom + SepBetweenSymptoms_SplitID;   // 获取ID值
                                    }


                                    symchkShowTextList = GetSymptomCheckboxTextByIDList(this, symchkHidValueList, 1);
                                    symchkPositiveTextList = GetSymptomCheckboxTextByIDList(this, symchkHidValueList, 2);
                                    // 根据体征词ID，读取体征词中最大病症级别。 20130814 by WTang
                                    symchkMaxDiseaseLevel = GetExamItemMaxDiseaseLevel(this, symchkHidValueList);

                                    jQuery("#txtHidValueResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(symchkHidValueList);
                                    jQuery("#txtPositiveResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(symchkPositiveTextList);
                                    jQuery("#txtResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(symchkShowTextList);
                                    jQuery("#DiseaseLevel_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(symchkMaxDiseaseLevel);

                                    return false;
                                }
                            });

                        }
                        else  // 否则视为单选输入框
                        {
                            var radioid = "radioSym_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem + "_" + lastexamitem.ID_Symptom;
                            jQuery("#" + radioid).attr("checked", true);
                            jQuery("#" + radioid).change();

                            //                            jQuery("#CustExamList input[name='radioSym_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem + "']:radio").each(function () {
                            //                                if (jQuery(this).val() == lastexamitem.ID_Symptom) {
                            //                                    jQuery(this).attr("checked", true);
                            //                                    jQuery(this).change();
                            //                                }
                            //                            });
                        }

                        // 20131216 by wtang 增加了对禁用了体征词的控制
                        jQuery("#SymptomDiv_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem + "_" + lastexamitem.ID_Symptom).show(); // 绑定值的时候，如果已经禁用，任然需要显示出来

                    }   // end if (lastfeeitem.GetResultWay == "C")

                }); // end dataList1

                // 给一般科室中的文本输入框赋值，值保存在 ResultNumber 中。 20130729 by WTang 
                if (lastfeeitem.GetResultWay == "N") {
                    if (lastfeeitem.ResultNumber != "") {
                        jQuery("#txtSym_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(lastfeeitem.ResultNumber);
                    } else {
                        jQuery("#txtSym_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(lastfeeitem.ResultLabValues);
                    }
                }
                // 将检查项目的默认值，赋结果值
                // ShowSystemDialog(jQuery("#txtLastResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val());

                jQuery("#txtResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val(jQuery("#txtLastResult_" + lastfeeitem.ID_Fee + "_" + lastfeeitem.ID_ExamItem).val());

            });                           // end dataList0

            // 获取客户的病症级别，并判断是否进行相应的提示 20140326 by wtang 
            QueryCustomerExamItemDiseaseLevelTips();

            // 初始化按钮是否可以使用 , （在页面上次保存的数据加载完成后，调用一次按钮控制函数）
            InitButtomDisabled();

            jQuery('#fklscen').show();
            // 显示分科信息 和 科室小结 Tab 选项
            jQuery('#fkls').show();
            jQuery('#ExamInfoTabLi1').show();
            jQuery('#ExamInfoTabLi3').show();
            jQuery('#btnCustomerSimpleInfo').show();
            jQuery('#CustomerSectionQuickSwitch').show();
        }

        /// <summary>
        /// 控制按钮是否能操作
        /// </summary>
        /// <param name="type">0：解除审核 1：审核 2：保存 3:全部禁用 </param>
        /// <param name="flag">0：解除审核 1：审核 </param>
        function CtrlButtonDisabled(type, flag) {

            if (flag == false) {
                switch (type) {
                    case 0:
                        jQuery("#ButDefault").removeAttr("disabled");
                        jQuery("#ButClear").removeAttr("disabled");
                        jQuery("#ButCollect").removeAttr("disabled");
                        jQuery("#ButSave").removeAttr("disabled");
                        jQuery("#ButCheck").removeAttr("disabled");
                        jQuery("#ButUnCheck").removeAttr("disabled");
                        jQuery("#btnSaveImg").removeAttr("disabled");
                        jQuery("#btnUploadImg").removeAttr("disabled");
                        jQuery(".btn_recvcomdata").removeAttr("disabled");
                        break;
                    case 1:
                        jQuery("#ButDefault").attr("disabled", "disabled");
                        jQuery("#ButClear").attr("disabled", "disabled");
                        jQuery("#ButCollect").attr("disabled", "disabled");
                        jQuery("#ButSave").attr("disabled", "disabled");
                        jQuery("#ButCheck").attr("disabled", "disabled");
                        jQuery("#ButUnCheck").removeAttr("disabled");
                        jQuery("#btnSaveImg").attr("disabled", "disabled");
                        jQuery("#btnUploadImg").attr("disabled", "disabled");
                        break;
                    case 2:
                        jQuery("#ButDefault").attr("disabled", "disabled");
                        jQuery("#ButClear").attr("disabled", "disabled");
                        jQuery("#ButCollect").attr("disabled", "disabled");
                        jQuery("#ButSave").attr("disabled", "disabled");
                        jQuery("#ButCheck").removeAttr("disabled");
                        jQuery("#ButUnCheck").removeAttr("disabled");
                        jQuery("#btnSaveImg").removeAttr("disabled");
                        jQuery("#btnUploadImg").removeAttr("disabled");
                        break;
                    case 3:
                        jQuery("#ButDefault").attr("disabled", "disabled");
                        jQuery("#ButClear").attr("disabled", "disabled");
                        jQuery("#ButCollect").attr("disabled", "disabled");
                        jQuery("#ButSave").attr("disabled", "disabled");
                        jQuery("#ButUnCheck").attr("disabled", "disabled");
                        jQuery("#ButCheck").attr("disabled", "disabled");
                        jQuery("#btnSaveImg").attr("disabled", "disabled");
                        jQuery("#btnUploadImg").attr("disabled", "disabled");


                        jQuery(".btn_recvcomdata").attr("disabled", "disabled");
                        break;
                }

            } else {
                jQuery("#ButDefault").removeAttr("disabled");
                jQuery("#ButClear").removeAttr("disabled");
                jQuery("#ButCollect").removeAttr("disabled");
                jQuery("#ButSave").removeAttr("disabled");
                jQuery("#ButCheck").removeAttr("disabled");
                jQuery("#ButUnCheck").removeAttr("disabled");
                jQuery("#btnSaveImg").removeAttr("disabled");
                jQuery("#btnUploadImg").removeAttr("disabled");
            }
        }

        /// <summary>
        /// 获取科室小结 (汇总)
        /// </summary>
        function GetCustomerExamSectionSummaryValue() {


            var WrapCharacter = "";             //显示的换行符号   20130923 by WTang 去掉换行符号 "\n"; 

            //            var SepBetwFeeWrapCharacter = "\n"; //显示的换行符号  （用于收费项目之间换行）
            //            var SepEndFeeNameWrapCharacter = "："; //显示冒号  （用于收费项目名称后面）

            var Is_AutoAddWrapCharacter = 1; //是否自动添加换行符号

            var CustomerExamSectionSummaryText = "";    // 小结文本信息
            var CustomerExamSectionSummaryValue = "";   // 小结Value
            var CustomerExamPositiveSummaryText = "";   // 阳性结果
            var tempFeeItemNameText = "";               // 临时记录收费项目的名称
            var tempFeeItemSummaryText = "";            // 临时记录收费项目的小结文本信息
            var tempFeeItemSummaryValue = "";           // 临时记录收费项目的小结Value
            var tempExamItemSummaryText = "";           // 临时记录检查项目的小结文本信息
            var tempExamItemPositiveSummaryText = "";   // 临时记录检查项目的阳性结果文本信息
            var tempExamItemDefaultSummaryText = "";    // 临时记录检查项目的默认小结文本信息
            var tempExamItemDiseaseLevel = "";          // 临时记录检查项目的所选体征词的最大病症级别

            var CurrFeeItemID = 0;      //在遍历检查项目使用
            var PreFeeItemID = 0;       //记录已经拼接到项目小结中的收费项目ID
            var CurrExamItemID = 0;     //在遍历体征词时使用
            var FeeItemCount = 0;       //记录收费项目的个数
            var CurrExamItemCount = 0;  //当前检查项目编号
            var ExamItemIsEntrySectSum = ""; //是否允许进入科室小结
            var Is_AllDefaultSummary = 1;    //标记是否是所有检查项目都是默认小结内容
            // 遍历检查项目列表
            jQuery(gPageCustomExamDataMsg.dataList1).each(function (j, examitem) {
                tempExamItemSummaryText = jQuery("#txtResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();
                tempExamItemDefaultSummaryText = jQuery("#txtDefaultValueResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();

                if (tempExamItemSummaryText != tempExamItemDefaultSummaryText) {
                    Is_AllDefaultSummary = 0; //检查到非默认小结信息
                    return false; // 跳出循环（ break ）
                }

            });
            if (Is_AllDefaultSummary == 0) {

                // 1、小结
                // 遍历收费项目 汇总小结
                jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                    tempFeeItemNameText = jQuery("#lab_" + feeitem.ID_Fee).attr("FeeName"); // 临时记录收费项目的名称

                    // 当前检查项目编号 从0开始计数 
                    // 20130926 by WTang 不显示收费项目名称 ,同时多个收费项目的检查项目统一进行编号，不再向以前单个收费项目独立编号
                    // CurrExamItemCount = 0;  

                    CurrFeeItemID = 0; //在遍历检查项目使用
                    // 遍历检查项目列表
                    jQuery(gPageCustomExamDataMsg.dataList1).each(function (j, examitem) {


                        // 这里可以判断出是否已经遍历完成，如果已经遍历完，则跳出循环 （最后一个项目不会）
                        if (CurrFeeItemID > 0 && CurrFeeItemID != examitem.ID_Fee) {

                            // 如果收费项目中只有一个检查项目，则去掉项目编号
                            tempFeeItemSummaryText = ReplaceFistCountNumber(tempFeeItemSummaryText, CurrExamItemCount);
                            if (tempFeeItemSummaryText != "") {
                                CustomerExamSectionSummaryText += tempFeeItemSummaryText + TerminalSymbol;  //项目分隔符
                            }
                            tempFeeItemSummaryText = "";   // 清空临时变量数据
                            PreFeeItemID = CurrFeeItemID;  // 记录已经拼接到项目小结中的收费项目ID
                            return false;
                        }
                        if (feeitem.ID_Fee == examitem.ID_Fee) {
                            CurrFeeItemID = examitem.ID_Fee; //如果进入了这个循环

                            tempExamItemSummaryText = jQuery("#txtResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();
                            tempExamItemDefaultSummaryText = jQuery("#txtDefaultValueResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();
                            tempExamItemDiseaseLevel = jQuery("#DiseaseLevel_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();

                            if (tempExamItemSummaryText == tempExamItemDefaultSummaryText) {

                                return true; // 继续下一个检查项目循环（ continue ）
                            }
                            // 如果改检查项目的病症级别为0，则不进入小结
                            if (tempExamItemDiseaseLevel == undefined || tempExamItemDiseaseLevel == "0") {

                                return true; // 继续下一个检查项目循环（ continue ）
                            }

                            ExamItemIsEntrySectSum = jQuery("#lab_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).attr("IsEntrySectSum"); //是否允许进入科室小结
                            if (ExamItemIsEntrySectSum == "True" && tempExamItemSummaryText != "" && tempExamItemSummaryText != "NULL") {

                                if (CurrExamItemCount > 0 && Is_AutoAddWrapCharacter == 1) {
                                    tempFeeItemSummaryText += WrapCharacter;
                                }
                                else {
                                    if (FeeItemCount > 0 && Is_AutoAddWrapCharacter == 1) {
                                        tempFeeItemSummaryText += WrapCharacter; // 如果不是第一个收费项目，则添加换行符号
                                    }

                                    //  20130711 （改） 显示小结的时候，不显示收费项目名称 
                                    //  20130923 by WTang 改为显示收费项目
                                    //  20130926 by WTang 不显示收费项目名称 ,同时多个收费项目的检查项目统一进行编号，不再向以前单个收费项目独立编号
                                    //  tempFeeItemSummaryText += tempFeeItemNameText + SepEndFeeNameWrapCharacter;
                                    if (Is_AutoAddWrapCharacter == 1) {
                                        tempFeeItemSummaryText += WrapCharacter; // 如果自动换行，则收费项目名称单独作为一行显示
                                    }

                                    FeeItemCount++;  // 记录有进入小结信息的收费项目的个数
                                }

                                CurrExamItemCount++;  //当前检查项目编号 ( +1 )
                                tempFeeItemSummaryText += GetCountNumberBetweenItem(NoBetweenExamItems, CurrExamItemCount, 10) + jQuery("#lab_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).attr("ExamItemName") + SepExamAndValue + tempExamItemSummaryText + SepBetweenExamItems; //添加项目终结符
                                tempFeeItemSummaryValue += jQuery("#txtHidValueResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();
                            }
                        }
                    });
                    // 如果最后一个收费项目还未拼接，则在这里进行拼接。
                    if (PreFeeItemID != CurrFeeItemID) {
                        // 如果收费项目中只有一个检查项目，则去掉项目编号
                        tempFeeItemSummaryText = ReplaceFistCountNumber(tempFeeItemSummaryText, CurrExamItemCount);
                        //如果该收费项目小结有内容
                        if (tempFeeItemSummaryText != "") {
                            // 加上最后一个收费项目的小结。
                            CustomerExamSectionSummaryText += tempFeeItemSummaryText + TerminalSymbol;          //项目分隔符
                        }
                        tempFeeItemSummaryText = "";   // 清空临时变量数据
                    }
                });

                // 替换科室终结中的重复标点符号
                CustomerExamSectionSummaryText = ReplaceFlagInSectionSummaryText(CustomerExamSectionSummaryText);
                // 小结 end

                // 2、阳性结果 
                // 遍历收费项目 汇总 阳性结果
                CustomerExamPositiveSummaryText = "";
                FeeItemCount = 0;       // 记录收费项目的个数
                CurrExamItemCount = 0;  // 多个收费项目的检查项目统一进行编号，不再向以前单个收费项目独立编号 (但是，阳性结果从1开始编号 20131009 by WTang )
                jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                    tempFeeItemNameText = jQuery("#lab_" + feeitem.ID_Fee).attr("FeeName"); // 临时记录收费项目的名称
                    // 当前检查项目编号 从0开始计数 
                    // 20130926 by WTang 不显示收费项目名称 ,同时多个收费项目的检查项目统一进行编号，不再向以前单个收费项目独立编号
                    // CurrExamItemCount = 0;  
                    CurrFeeItemID = 0;      //在遍历检查项目使用
                    PreFeeItemID = 0;       //记录已经拼接到项目小结中的收费项目ID
                    tempFeeItemSummaryText = "";
                    // 遍历检查项目列表
                    jQuery(gPageCustomExamDataMsg.dataList1).each(function (j, examitem) {

                        // 这里可以判断出是否已经遍历完成，如果已经遍历完，则跳出循环 （最后一个项目不会）
                        if (CurrFeeItemID > 0 && CurrFeeItemID != examitem.ID_Fee) {

                            // 如果收费项目中只有一个检查项目，则去掉项目编号
                            tempFeeItemSummaryText = ReplaceFistCountNumber(tempFeeItemSummaryText, CurrExamItemCount);
                            if (tempFeeItemSummaryText != "") {
                                CustomerExamPositiveSummaryText += tempFeeItemSummaryText + TerminalSymbol;  //项目分隔符
                            }
                            tempFeeItemSummaryText = "";   // 清空临时变量数据
                            PreFeeItemID = CurrFeeItemID;  // 记录已经拼接到项目小结中的收费项目ID
                            return false;
                        }

                        if (feeitem.ID_Fee == examitem.ID_Fee) {
                            CurrFeeItemID = examitem.ID_Fee; //如果进入了这个循环

                            tempExamItemPositiveSummaryText = jQuery("#txtPositiveResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();
                            // 如果阳性结果的输入框中是1，表示该检查项目存在阳性结果，则阳性结果值 = 检查项目小结内容
                            if (tempExamItemPositiveSummaryText == "1") {
                                tempExamItemPositiveSummaryText = jQuery("#txtResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();
                            }

                            if (tempExamItemPositiveSummaryText != "") {

                                if (CurrExamItemCount > 0 && Is_AutoAddWrapCharacter == 1) {
                                    tempFeeItemSummaryText += WrapCharacter;
                                }
                                else {
                                    if (FeeItemCount > 0 && Is_AutoAddWrapCharacter == 1) {
                                        tempFeeItemSummaryText += WrapCharacter; // 如果不是第一个收费项目，则添加换行符号
                                    }

                                    //  20130711 （改） 显示小结的时候，不显示收费项目名称 
                                    //  20130923 by WTang 改为显示收费项目
                                    //  20130926 by WTang 不显示收费项目名称 ,同时多个收费项目的检查项目统一进行编号，不再向以前单个收费项目独立编号
                                    // tempFeeItemSummaryText += tempFeeItemNameText + SepEndFeeNameWrapCharacter;
                                    if (Is_AutoAddWrapCharacter == 1) {
                                        tempFeeItemSummaryText += WrapCharacter; // 如果自动换行，则收费项目名称单独作为一行显示
                                    }

                                    FeeItemCount++;  // 记录有进入小结信息的收费项目的个数
                                }
                                CurrExamItemCount++;  //当前检查项目编号 ( +1 )
                                tempFeeItemSummaryText += GetCountNumberBetweenItem(NoBetweenExamItems, CurrExamItemCount, 10) + jQuery("#lab_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).attr("ExamItemName") + SepExamAndValue + tempExamItemPositiveSummaryText + SepBetweenExamItems; //添加项目终结符

                            }
                        }
                    });

                    // 如果最后一个收费项目还未拼接，则在这里进行拼接。
                    if (PreFeeItemID != CurrFeeItemID) {
                        // 如果收费项目中只有一个检查项目，则去掉项目编号
                        tempFeeItemSummaryText = ReplaceFistCountNumber(tempFeeItemSummaryText, CurrExamItemCount);
                        //如果该收费项目 阳性结果 有内容
                        if (tempFeeItemSummaryText != "") {
                            // 加上最后一个收费项目的 阳性结果。
                            CustomerExamPositiveSummaryText += tempFeeItemSummaryText + TerminalSymbol;          //项目分隔符
                        }
                        tempFeeItemSummaryText = "";   // 清空临时变量数据
                    }
                });

                // 替换科室 阳性结果 中的重复标点符号
                CustomerExamPositiveSummaryText = ReplaceFlagInSectionSummaryText(CustomerExamPositiveSummaryText);
                // 阳性结果 end

                //  20130923 By WTang 如果结果为空，则显示科室默认小结
                if (CustomerExamSectionSummaryText == undefined || jQuery.trim(CustomerExamSectionSummaryText) == "") {
                    CustomerExamSectionSummaryText = jQuery("#SectionDefaultSummary").val();
                }

                //                //  20130923 By WTang 修改为小结中也不出现重症级别为0的 即，小结内容与阳性结果一样
                //                CustomerExamSectionSummaryText = CustomerExamPositiveSummaryText;

            }
            else {
                CustomerExamSectionSummaryText = jQuery("#SectionDefaultSummary").val();
                CustomerExamPositiveSummaryText = ""; // 阳性结果为空
            }
            // 输出科室小结
            jQuery("#SectionSummary").val(CustomerExamSectionSummaryText);  //  20130923 修改为小结中也不出现重症级别为0的 20130925 还原为原来小结内容
            // 输出科室阳性结果
            jQuery("#PositiveSummary").val(CustomerExamPositiveSummaryText);


            // 如果自动完成小结 20130927 by WTang
            if (jQuery("#IsAutoFinishSaveAndSubmit").val() == "1") {
                SaveCustomerSectionSummaryConfirm();
            }


        }

        /// <summary>
        /// 替换科室小结中的重复标点符号（小结）
        /// </summary>
        function ReplaceFlagInSectionSummaryText(SummaryMsg) {
            var ReturnSummaryMsg = SummaryMsg; // 保存返回的数据
            var replaceReg01 = new RegExp(SepBetweenSymptoms + SepBetweenExamItems, "gi"); //  替换掉多余的 SepBetweenSymptoms
            var replaceReg02 = new RegExp(SepBetweenExamItems + TerminalSymbol, "gi"); //  替换掉多余的 SepBetweenSymptoms

            if (SepBetweenSymptoms != "" && SepBetweenExamItems != "")
                ReturnSummaryMsg = ReturnSummaryMsg.replace(replaceReg01, SepBetweenExamItems);
            if (SepBetweenExamItems != "" && TerminalSymbol != "")
                ReturnSummaryMsg = ReturnSummaryMsg.replace(replaceReg02, TerminalSymbol);

            return ReturnSummaryMsg;

            //            var SepBetweenExamItems = "，";   //项目分隔符        如：、
            //            var SepBetweenSymptoms = "、";    //体征词分隔符      如：、
            //            var TerminalSymbol = "。";        //项目终结符        如：。
            //            var SepExamAndValue = ":";       //项目小结分隔符     如：：
            //            var NoBetweenExamItems = "(1)";    //项目序号         如：(1)
            //            var NoBetweenSympotms = "①";     //体征词序号        如： ①
        }
        /// <summary>
        /// 如果收费项目中只有一个检查项目，则去掉项目编号
        /// </summary>
        function ReplaceFistCountNumber(TheOneFeeItemSummaryText, totalcount) {

            if (totalcount <= 1) {
                return TheOneFeeItemSummaryText.replace(NoBetweenExamItems, "");
            } else {
                return TheOneFeeItemSummaryText;
            }
        }

        /// <summary>
        /// 取回确认操作 
        /// </summary>
        /// <param name="state">1：审核 0：解除审核</param>
        function UpdateSectionSummaryCheckStateConfirm(state) {
            
            var Is_SectionCheck = jQuery("#Is_SectionCheck").val();       // 是否提交（及是否审核）
            var ID_Typist = jQuery("#ID_Typist").val();         // 上次录入人员ID
            var ID_UserLogin = jQuery("#HiddenUserID").val();   // 当前登录人员ID
            if (Is_SectionCheck == "False") {
                ShowSystemDialog("当前体检信息还没有【提交】，不需要取回，请修改后，直接保存然后提交即可！");
            }
            // 在下面这种情况下，需要确认操作,1、上次的录入人员与当前录入人员不一致。
            else if (jQuery("#SummaryDoctorName").val() != "" && ID_Typist != "" && ID_Typist != "0" && ID_Typist != ID_UserLogin) {
                var tipscontent = "您确定要【取回】当前检查信息吗？<br/>取回后，再次提交，您将作为最后的录入人员,请确认是否继续?";
                art.dialog({
                    id: 'artDialogID',
                    content: tipscontent,
                    lock: true,
                    zIndex: 500,
                    fixed: true,
                    title: '变更录入者提示',
                    opacity: 0.3,
                    button: [{
                        name: '确定取回',
                        callback: function () {
                            UpdateSectionSummaryCheckState(state);
                            return true;
                        }, focus: true
                    }, {
                        name: '取消'
                    }]
                });
            }
            //  || ID_Typist == "" || ID_Typist == "0"
            //如果检查医生ID为空并且检查医生名字也为空则提示 xmhuang 20140409
//            else if (jQuery.trim(jQuery("#SummaryDoctorName").val()) == "" && jQuery.trim(jQuery("#ID_SummaryDoctor").val())) {
//                ShowSystemDialog("当前体检信息还没有【保存】，不需要进行取回！");
//            }
            else {
                // 
                UpdateSectionSummaryCheckState(state);
            }
        }

        /// <summary>
        /// 保存科室小结（审核） 
        /// </summary>
        /// <param name="state">1：审核 0：解除审核</param>
        function UpdateSectionSummaryCheckState_CompareLastSummary() {
            var state = 1;
            var tmpLast_SectionSummary = jQuery("#Last_SectionSummary").val();  // 上次结果
            var tmpSectionSummary = jQuery("#SectionSummary").val();            // 本次结果

            if (tmpSectionSummary == "" || jQuery.trim(tmpSectionSummary) == "") {
                ShowSystemDialog("科室小结不能为空，请填写小结后再进行保存！");
                return;
            }

            // 点击保存时，再次对所有数据进行一次提交，而不仅仅是修改审核相关标记 （20141119 by wtang , 经过20141118 讨论后，确定进行这样的改动 ）

            var ID_SummaryDoctor = jQuery("#idSelectUser").val();    // 检查医生ID (LAB类科室没有这个值)
            var idLastSaveExamDoctor = jQuery("#idLastSaveExamDoctor").val();    // 上次保存的 检查医生ID
            if ((idLastSaveExamDoctor == ID_SummaryDoctor || ID_SummaryDoctor == "" || ID_SummaryDoctor == undefined) 
                &&  tmpLast_SectionSummary == tmpSectionSummary) {
                UpdateSectionSummaryCheckState(state);
            }
            else {

                // 如果两次的值不一致，及做了修改。则首先进行保存，然后再进行提交
                jQuery("#IsAutoFinishSaveAndSubmit").val("2");
                SaveCustomerSectionSummaryConfirm(); // 
            }
        }
        /// <summary>
        /// 保存科室小结（审核） 
        /// </summary>
        /// <param name="state">1：审核 0：解除审核</param>
        function UpdateSectionSummaryCheckState(state) {

            // 将按钮设置为不可操作
            CtrlButtonDisabled(state, false);
            var ID_CustExamSection = jQuery("#ID_CustExamSection").val();   // 科室小结编号
            var statemsg = "提交";
            if (state == 0) { statemsg = "取回"; }

            // 如果是取回，则当前的录入人员变更为当前登录人员信息，录入时间，变更为当前日期 20130730 by  WTang
            if (state == 0) {
                jQuery("#spanTypistName").html(jQuery("#HiddenUserName").val());
                jQuery("#spanTypistDate").html(jQuery("#DateToday").val());
            }

            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxCustExam.aspx",
                data: { ID_CustExamSection: ID_CustExamSection,
                    SummaryCheckState: state,
                    action: 'UpdateSectionSummaryCheckState',
                    currenttime: encodeURIComponent(new Date())
                },
                cache: false,
                dataType: "text",
                success: function (jsonmsg) {
                    // 检查Ajax返回数据的状态等 20140430 by wtang 
                    jsonmsg = CheckAjaxReturnDataInfo(jsonmsg);
                    if (jsonmsg == null || jsonmsg == "") {
                        return;
                    }

                    if (jsonmsg == "-1") {
                        ShowSystemDialog("该分科信息还未保存，不能进行提交!");
                        CtrlButtonDisabled(0, true);
                    }
                    else if (jsonmsg == "0") { ShowSystemDialog(statemsg + "小结信息失败，请与技术人员联系!") }

                    else {

                        jQuery("#TypistDate").val(jsonmsg); // 录入时间 20150421 by wtang 

                        // 如果自动完成小结 20130927 by WTang
                        if (jQuery("#IsAutoFinishSaveAndSubmit").val() == "1") {
                            // 如果是自动保存，不进行提示
                            // 清空体检号，同时体检号输入框获取焦点 
                            jQuery("#txtCustomerID").val("");
                            jQuery("#txtCustomerID").focus();
                        } else {
                            ShowSystemDialogAutoClose(statemsg + "小结信息成功!", jQuery("#txtCustomerID"));
                        }

                        jQuery("#Is_SectionCheck").val(state == 1 ? "True" : "False"); // 提交或取回后，修改对应的标记 20130730 by WTang
                        jQuery("#ID_Typist").val(jQuery("#HiddenUserID").val());
                        jQuery("#TypistName").val(jQuery("#HiddenUserName").val());


                        //                        // 将按钮设置为不可操作
                        //                        CtrlButtonDisabled(0, false);
                    }
                }
            });
        }



        /// <summary>
        ///  保存科室小结（保存） 确认操作  20130730 by WTang
        /// </summary>
        function SaveCustomerSectionSummaryConfirm() {
            var ID_Typist = jQuery("#ID_Typist").val();         // 上次录入人员ID
            var ID_UserLogin = jQuery("#HiddenUserID").val();   // 当前登录人员ID


            var SectionSummary = jQuery("#SectionSummary").val();           // 科室小结
            if (SectionSummary == "" || jQuery.trim(SectionSummary) == "") {
                ShowSystemDialog("科室小结不能为空，请填写小结后再进行保存！");
                return;
            }

            // 将检查医生的提示，移动到弹出提示之前。 20130813 by WTang 
            var tempExamDoctorID = "0";
            // 遍历收费项目
            jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {
                // 获取收费项目对应的医生ID
                tempExamDoctorID = jQuery("#ExamDoctor_" + feeitem.ID_Fee).val();
            });
            if (tempExamDoctorID == "" || tempExamDoctorID == "0") {
                ShowSystemDialog("请选择检查医生！");
                return false;
            }

            // 在下面这种情况下，需要确认操作,1、上次的录入人员与当前录入人员不一致。   
            if (ID_Typist != "" && ID_Typist != "0" && ID_Typist != ID_UserLogin) {
                var tipscontent = "您确定【保存】当前体检信息吗？<br/>保存后，您将作为最后的录入人员,请确认是否继续?";
                art.dialog({
                    id: 'artDialogID',
                    content: tipscontent,
                    lock: true,
                    zIndex: 500,
                    fixed: true,
                    title: '变更录入者提示',
                    opacity: 0.3,
                    button: [{
                        name: '确定保存',
                        callback: function () {

                            // 更新显示的录入人员
                            jQuery("#spanTypistName").html(jQuery("#HiddenUserName").val());
                            jQuery("#spanTypistDate").html(jQuery("#DateToday").val());

                            SaveCustomerSectionSummary();  // 确定保存后，进行数据的保存操作 
                            return true;
                        }, focus: true
                    }, {
                        name: '取消'
                    }]
                });
            }
            else {
                //不需要确认提示的情况:1、首次保存；
                //                    2、录入人员与上次一致。  20130730 by WTang
                SaveCustomerSectionSummary();
            }
        }

        /// <summary>
        /// 保存科室小结（保存） 
        /// </summary>
        function SaveCustomerSectionSummary() {

            // 20130819 by WTang （这里改成，保存前禁用所有按钮，保存完成后，不进行提示，然后恢复按钮的使用）
            CtrlButtonDisabled(3, false); //禁用所有操作按钮 （在数据提交的过程中）

            //            // 在保存前，自动进行一次汇总数据，防止忘记点击汇总造成的数据不统一的情况。 
            //            // 暂时不进行自动汇总，考虑到有可能会在小结中进行直接编辑。20130724 by WTang
            //            GetCustomerExamSectionSummaryValue();

            // 获取保存时提交的参数。 (参数提取)
            var SectionSummaryParams = GetSaveCustomerSectionSummaryParams();

            LastTimeSectionExamedCount = null; // 开始为空(比对客户已检科室数据是否需要重新读取)
            jQuery("#iptCurrShowSectionCustomerID").val("");     // 将上次查询的 科室+身份证 清空 (侧边栏，分科对比)

            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxCustExam.aspx",
                data: SectionSummaryParams,         // 科室小结参数
                cache: false,
                dataType: "text",
                success: function (jsonmsg) {
                    // 检查Ajax返回数据的状态等 20140430 by wtang 
                    jsonmsg = CheckAjaxReturnDataInfo(jsonmsg);
                    if (jsonmsg == null || jsonmsg == "") {
                        CtrlButtonDisabled(0, true); //全部按钮可以使用  20140430 by wtang 
                        return;
                    }
                    if (parseInt(jsonmsg) > 0) {

                        jQuery("#ID_Typist").val(jQuery("#HiddenUserID").val()); // 将录入人员ID置为当前登陆人员ID，便于取回按钮的判断。
                        jQuery("#idLastSaveExamDoctor").val(jQuery("#idSelectUser").val()); // 保存成功后，设置当前选择的医生ID赋值给“上次保存的检查医生”，用于判断医生是否发生了变化 20141120 by wtang
                        // ShowSystemDialog("保存小结信息成功!");

                        // 由于保存后，没有禁用按钮，所以需要在保存成功后，重新加载数据 20130826 by WTang
                        // 清空保存的小结数据信息
                        gPageCustomExamLastSaveDataMsg = "";
                        // 重新获取新的数据，并绑定到页面上
                        GetCustomerExamLastSaveData();
                        /*** 
                        
                        备注：保存完成后，将按钮置为不可用状态，如果后期需要开放为可用状态，则启用该段代码

                        // 重新初始化页面(清空页面上的值，然后重新获取)
                        InitCustomExamItemPage_IsSetDefaultValue(gPageCustomExamDataMsg, 0);

                        ***/

                        // CtrlButtonDisabled(0, true); // 将按钮置为可以使用 20130819 by WTang （这里改成，保存前禁用所有按钮，保存完成后，不进行提示，然后恢复按钮的使用）

                        // 将按钮设置为不可操作 保存后不进行提示，但是保存按钮不能再次点击，需要刷新页面后才能重新点击保存 20130904 by WTang
                        CtrlButtonDisabled(2, false);


                        // 如果自动完成小结 20130927 by WTang
                        if (jQuery("#IsAutoFinishSaveAndSubmit").val() == "1") {
                            jQuery("#IsAutoFinishSaveAndSubmit").val("0");
                            UpdateSectionSummaryCheckState(1);
                        }

                        // 等于2表示提交时，监测到有未保存的项目，先进行了数据的保持，然后再进行数据的提交
                        if (jQuery("#IsAutoFinishSaveAndSubmit").val() == "2") {
                            jQuery("#IsAutoFinishSaveAndSubmit").val("0");
                            UpdateSectionSummaryCheckState(1);
                        }
                    }
                    else if (jsonmsg == "-1") {
                        ShowSystemDialog("分检结果与上次一致，请改动后再保存!"); // 实际上是没有数据改动，没有入库操作 20130729 by WTang
                        CtrlButtonDisabled(0, true); //全部按钮可以使用  20130729 by WTang
                    }
                    else if (jsonmsg == "0") {
                        CtrlButtonDisabled(0, true); //全部按钮可以使用  20131204 by WTang
                        ShowSystemDialog("保存小结信息失败，请与技术人员联系!")
                    }
                    else {
                        ShowSystemDialog(jsonmsg); // 提示两人同时操作时，提示先操作的人的姓名，及操作时间 20150420 by WTang
                        CtrlButtonDisabled(3, false); //全部按钮不可以使用
                    }
                }
            });
        }

        /// <summary>
        /// 设置检查医生 ，保存到cookie 
        /// </summary>
        function SetExamDoctorInfo(ExamDoctorID, ExamDoctorName) {
            if (ExamDoctorName == "") { return; }

            var exp = new Date(); // 设置当前时间，只有当天才有效 （在取出时，先判断时间是否一致。）
            var SectionID = jQuery('#txtSectionID').val();
            var OperatorID = jQuery('#CookieUserID').val();
            var CurrentDate = exp.getFullYear() + "-" + exp.getMonth() + '-' + exp.getDay();

            SetCookie('LastExamDoctorSaveDate', CurrentDate);
            SetCookie("LastExamDoctorSection", SectionID);
            SetCookie("LastExamDoctorID_" + SectionID, ExamDoctorID);
            SetCookie("LastExamDoctorName_" + SectionID, ExamDoctorName);
            SetCookie('LastExamDoctorSaveOperatorID', OperatorID);

            //alert(ExamDoctorName+" -- "+ExamDoctorID + "GET:"+ GetCookie("LastExamDoctorName"));

        }

        /// <summary>
        /// 获取保存时提交的参数。 (参数提取)
        /// </summary>
        /// 说明： 
        ///     1、如果是第一次保存，或修改（点击了清除按钮，或缺省按钮）,则向后出传入检查项目小结不为空的所有的检查项目及对应体征词。
        ///     2、如果是修改（没有点击清除按钮，缺省按钮），则只向后台传入改动过的数据。
        function GetSaveCustomerSectionSummaryParams() {
            var ID_Customer = jQuery("#txtHiddenCustomerID").val();         // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var ID_CustExamSection = jQuery("#ID_CustExamSection").val();   // 科室小结编号
            var SectionSummary = jQuery("#SectionSummary").val();           // 科室小结
            SectionSummary = encodeURIComponent(SectionSummary);            // 编码处理
            var PositiveSummary = jQuery("#PositiveSummary").val();         // 阳性结果
            PositiveSummary = encodeURIComponent(PositiveSummary);          // 编码处理
            var SummaryDoctorName = jQuery("#SummaryDoctorName").val();     // 小结医生姓名
            var SectionSummaryDate = jQuery("#SectionSummaryDate").val();   // 小结时间 (修改字段含义) 20130730 改为用于保持检查时间 by WTang 
            SectionSummaryDate = encodeURIComponent(SectionSummaryDate);    // 检查时间 编码处理
            var TypistDate = jQuery("#TypistDate").val();                   // 录入时间 20150421 by WTang (用于比对加载数据后，在保存前这段时间期间，是否有其它人进行了录入，如果有，则做出相应的提示。)
            TypistDate = encodeURIComponent(TypistDate);                    // 录入时间 编码处理

            var IS_NotUseLastSaveData = jQuery("#IS_NotUseLastSaveData").val();         // 标记是否是没有加载上次保存的数据，可能是：1、点击了清除按钮，2、点击了缺省按钮  20130813 by WTang 

            var CustExamDataInfoListStr = "";       // 记录所有检查项目拼接成为指定的特殊字符串后的信息（后台接收后，按照特殊的分割符进行分割并保存）
            var tempExamItemSummaryText = "";       // 临时记录检查项目的小结文本信息
            var tempExamItemSummaryValue = "";      // 临时记录检查项目的小结Value
            var tempMaxDiseaseLevel = 0;            // 临时记录检查项目的的最大病症级别 Add by WTang 20130814
            var tempResultNumber = "";              // 检查项目数值
            var SymptomStrList = "";
            var CustFeeDataInfoListStr = "";        // 记录所有收费项目拼接成为指定的特殊字符串后的信息
            var tempLastExamItemSummaryText = "";   // 临时记录检查项目的小结文本信息（上次保存的数据）
            var tempCustExamItemID = "0";           // 临时记录项目结论编号，只有修改的时候才会有该值
            var tempExamDoctorID = "0";             // 临时记录项各个收费项目的体检医生ID
            var tempExamDoctorName = "";            // 临时记录项各个收费项目的体检医生姓名


            var SectionMaxDiseaseLevel = 0; // 检查科室中对应的最大病症级别  Add by WTang 20130814
            var tempSectionMaxDiseaseLevel = 0; // 临时记录检查科室中对应的最大病症级别

            // 每个检查项目内部使用 、进行分割   检查项目与检查项目之间使用 | 进行分割。 
            // 所有数据在拼接之前必须采用url编码后，再进行拼接。
            //             一般方式    收费项目ID、客户收费ID、  检查项目ID、  检查项目名称、  检查结果值(体征词名称) 、检查值（体征词ID）     、客户检查项目ID   、医生ID       、医生姓名        、当前检查项目最大病症级别、@结果获取途径、 @检查项目数值
            var CustExamDataTemplete = "@ID_Fee、@ID_CustFee、@ID_ExamItem、@ExamItemName、@ExamItemResultSummary、@ExamItemSymptomValues、@ID_CustExamItem、@ID_SummaryDoctor、@SummaryDoctorName、@MaxDiseaseLevel、@GetResultWay、@ResultNumber";
            // url编码后，再进行拼接。
            //                        收费项目ID、客户收费ID、医生ID            、医生姓名          、当前检查项目最大病症级别 
            var CustFeeDataTemplete = "@ID_Fee、@ID_CustFee、@ID_SummaryDoctor、@SummaryDoctorName、@MaxDiseaseLevel";

            // 遍历收费项目 获取收费项目ID及检查医生
            jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                // 获取收费项目对应的医生ID，和 姓名
                tempExamDoctorID = jQuery("#ExamDoctor_" + feeitem.ID_Fee).val();
                tempExamDoctorName = jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName");
                jQuery("#SummaryDoctorName").val(tempExamDoctorName); // 检查医生
                CustFeeDataInfoListStr +=
                CustFeeDataTemplete.replace(/@ID_Fee/gi, encodeURIComponent(feeitem.ID_Fee))
                .replace(/@ID_CustFee/gi, encodeURIComponent(feeitem.ID_CustFee))
                .replace(/@ID_SummaryDoctor/gi, encodeURIComponent(tempExamDoctorID))
                .replace(/@SummaryDoctorName/gi, encodeURIComponent(tempExamDoctorName))
                + "|";
            });

            // 设置检查医生 ，保存到cookie 20140507 by wtang 
            SetExamDoctorInfo(tempExamDoctorID, tempExamDoctorName);

            // 遍历检查项目列表  获取科室最大病症级别 Add by WTang 20130814
            jQuery(gPageCustomExamDataMsg.dataList1).each(function (j, examitem) {
                tempSectionMaxDiseaseLevel = jQuery("#DiseaseLevel_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();             // 临时记录检查项目的的最大病症级别
                if (tempSectionMaxDiseaseLevel != "" && parseInt(tempSectionMaxDiseaseLevel) > parseInt(SectionMaxDiseaseLevel)) {
                    SectionMaxDiseaseLevel = tempSectionMaxDiseaseLevel;
                }
            });

            // 判断是否是第一次进行小结 （不是第一次进行小结, 且没有点击 1、清除按钮，2、缺省按钮 这两种情况）
            if (SummaryDoctorName != "" && IS_NotUseLastSaveData == "0") {
                // 遍历收费项目
                jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                    // 获取收费项目对应的医生ID，和 姓名
                    tempExamDoctorID = jQuery("#ExamDoctor_" + feeitem.ID_Fee).val();
                    tempExamDoctorName = jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName");

                    //                    if (jQuery("#LoginVocationType").val() == "1") {
                    //                        tempExamDoctorName = jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName");
                    //                    } else {
                    //                        tempExamDoctorName = jQuery("#ExamDoctor_" + feeitem.ID_Fee).find("option:selected").text();
                    //                    }

                    // 遍历检查项目列表  
                    jQuery(gPageCustomExamDataMsg.dataList1).each(function (j, examitem) {
                        if (feeitem.ID_Fee != examitem.ID_Fee) { return true; }

                        // 检查项目的小结 用于对比是否改动
                        tempExamItemSummaryText = jQuery("#txtResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();           // 临时记录检查项目的小结文本信息
                        if (tempExamItemSummaryText == "undefined") {
                            tempExamItemSummaryText = "";
                        }
                        tempExamItemSummaryValue = jQuery("#txtHidValueResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();  // 临时记录检查项目的小结Value
                        tempCustExamItemID = jQuery("#txtCustExamItemID_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();        // 临时记录检查项目结论编号，只有修改的时候才会有该值

                        tempMaxDiseaseLevel = jQuery("#DiseaseLevel_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();             // 临时记录检查项目的的最大病症级别 Add by WTang 20130814
                        tempResultNumber = jQuery("#txtSym_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();                     // 临时记录检查项目的检查项目数值
                        tempLastExamItemSummaryText = jQuery("#txtLastResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();   // 临时记录检查项目的小结文本信息（上次保存的数据）
                        //如果数据有改动，则后台先删除该体检项目分组下的体征词，再重新进行Insert
                        if (tempExamItemSummaryText != tempLastExamItemSummaryText) {
                            // 注意，在修改的时候，如果体检项目的小结为空的话，任然需要传到后台，以便进行上次数据的清空。
                            CustExamDataInfoListStr +=
                            CustExamDataTemplete.replace(/@ID_Fee/gi, encodeURIComponent(examitem.ID_Fee))
                            .replace(/@ID_CustFee/gi, encodeURIComponent(examitem.ID_CustFee))
                            .replace(/@ID_ExamItem/gi, encodeURIComponent(examitem.ID_ExamItem))
                            .replace(/@ExamItemName/gi, encodeURIComponent(examitem.ExamItemName))
                            .replace(/@ExamItemResultSummary/gi, encodeURIComponent(tempExamItemSummaryText))
                            .replace(/@ExamItemSymptomValues/gi, encodeURIComponent(tempExamItemSummaryValue))
                            .replace(/@ID_CustExamItem/gi, encodeURIComponent(tempCustExamItemID))
                            .replace(/@ID_SummaryDoctor/gi, encodeURIComponent(tempExamDoctorID))
                            .replace(/@SummaryDoctorName/gi, encodeURIComponent(tempExamDoctorName))
                            .replace(/@GetResultWay/gi, encodeURIComponent(examitem.GetResultWay))
                            .replace(/@ResultNumber/gi, encodeURIComponent(tempResultNumber))
                            .replace(/@MaxDiseaseLevel/gi, encodeURIComponent(tempMaxDiseaseLevel))
                            + "|";


                        }
                    });
                });

            }
            else //（是第一次进行小结）
            {

                // 遍历收费项目
                jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                    // 获取收费项目对应的医生ID，和 姓名
                    tempExamDoctorID = jQuery("#ExamDoctor_" + feeitem.ID_Fee).val();
                    tempExamDoctorName = jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName");

                    // 遍历检查项目列表  
                    jQuery(gPageCustomExamDataMsg.dataList1).each(function (j, examitem) {
                        if (feeitem.ID_Fee != examitem.ID_Fee) { return true; }
                        tempExamItemSummaryText = jQuery("#txtResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();           // 临时记录检查项目的小结文本信息
                        if (tempExamItemSummaryText == "undefined") {
                            tempExamItemSummaryText = "";
                        }
                        tempExamItemSummaryValue = jQuery("#txtHidValueResult_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();  // 临时记录检查项目的小结Value

                        tempMaxDiseaseLevel = jQuery("#DiseaseLevel_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();             // 临时记录检查项目的的最大病症级别 Add by WTang 20130814
                        tempResultNumber = jQuery("#txtSym_" + examitem.ID_Fee + "_" + examitem.ID_ExamItem).val();                     // 临时记录检查项目的检查项目数值 

                        // 如果是第一次保存，如果体检项目的小结数据为空的话，则无需传到后台进行数据的保存。
                        if (tempExamItemSummaryText != "") {
                            CustExamDataInfoListStr +=
                        CustExamDataTemplete.replace(/@ID_Fee/gi, encodeURIComponent(examitem.ID_Fee))
                        .replace(/@ID_CustFee/gi, encodeURIComponent(examitem.ID_CustFee))
                        .replace(/@ID_ExamItem/gi, encodeURIComponent(examitem.ID_ExamItem))
                        .replace(/@ExamItemName/gi, encodeURIComponent(examitem.ExamItemName))
                        .replace(/@ExamItemResultSummary/gi, encodeURIComponent(tempExamItemSummaryText))
                        .replace(/@ExamItemSymptomValues/gi, encodeURIComponent(tempExamItemSummaryValue))
                        .replace(/@ID_SummaryDoctor/gi, encodeURIComponent(tempExamDoctorID))
                        .replace(/@SummaryDoctorName/gi, encodeURIComponent(tempExamDoctorName))
                        .replace(/@ID_CustExamItem/gi, "0")
                        .replace(/@GetResultWay/gi, encodeURIComponent(examitem.GetResultWay))
                        .replace(/@ResultNumber/gi, encodeURIComponent(tempResultNumber))
                        .replace(/@MaxDiseaseLevel/gi, encodeURIComponent(tempMaxDiseaseLevel))
                        + "|";
                        }

                    }); // end dataList1

                }); // end dataList0
            }  // end 第一次进行小结 参数提取

            var SectionID = jQuery('#txtSectionID').val();
            var SectionSummaryParams = { ID_CustExamSection: ID_CustExamSection,
                SummaryDoctorName: SummaryDoctorName,
                ID_Customer: ID_Customer,
                SectionID: SectionID,
                SectionSummaryDate: SectionSummaryDate,
                SectionSummary: SectionSummary,
                PositiveSummary: PositiveSummary,
                TypistDate: TypistDate,
                CustExamDataInfoListStr: CustExamDataInfoListStr,
                CustFeeDataInfoListStr: CustFeeDataInfoListStr,
                IS_NotUseLastSaveData: IS_NotUseLastSaveData,
                SectionMaxDiseaseLevel: SectionMaxDiseaseLevel,
                action: 'SaveCustomerSectionSummaryInfo',
                currenttime: encodeURIComponent(new Date())
            };

            return SectionSummaryParams; // 返回拼接后的参数
        }

        /// <summary>
        /// 清除科室小结 及 删除检查明细值 20131112 by wtang
        /// </summary>
        function DeleteCustomerExamItemConfirm() {
            var tipscontent = "您确定要【清除】当前的检查信息吗？<br/>清除后，检查明细数据将无法恢复！！";
            art.dialog({
                id: 'artDialogID',
                content: tipscontent,
                lock: true,
                zIndex: 500,
                fixed: true,
                title: '清除检查信息提示',
                opacity: 0.3,
                button: [{
                    name: '确定清除',
                    callback: function () {
                        DeleteCustomerExamItem();
                        return true;
                    }, focus: true
                }, {
                    name: '取消'
                }]
            });
        }

        /// <summary>
        /// 清除科室小结 及 删除检查明细值 20131112 by wtang
        /// </summary>
        function DeleteCustomerExamItem() {

            var CustomerID = jQuery.trim(jQuery('#txtHiddenCustomerID').val());
            var SectionID = jQuery('#txtSectionID').val();

            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxCustExam.aspx",
                data: { CustomerID: CustomerID,
                    SectionID: SectionID,
                    InterfaceType: "",
                    action: 'DeleteCustomerExamItem',
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
                    jQuery('#txtCustomerID').val(jQuery('#txtHiddenCustomerID').val());
                    if (jsonmsg != "" && parseInt(jsonmsg) >= 0) {
                        ClearSectionImageDir();
                    } else if (jsonmsg == "-2") {

                        ShowSystemDialog("分科数据清除成功，但接口中的状态重置失败，请联系开发人员！");
                    } else {
                        ShowSystemDialog("清除失败，请联系开发人员！");
                    }
                }
            });
        }


        /// <summary>
        /// 自动计算体重指数函数
        /// </summary>
        /// <param name="obj">体重指数框对象（this）</param>
        /// <param name="param1">第一个框的数据库编号（用数据库编号，转换成为文本框的ID）</param>
        /// <param name="param2">第二个框的数据库编号（用数据库编号，转换成为文本框的ID）</param>
        function AutoCalculWeightPoint(obj, param1, param2) {

            var CurrTextID = jQuery(obj).attr("id");
            if (jQuery.trim(jQuery("#" + CurrTextID).val()) != "") {
                var ret = jQuery.trim(jQuery("#" + CurrTextID).val());
                var rettext = GetWeightPointText(CurrTextID.replace(/txtSym_/gi, "AutoCalcul_"), ret);
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtResult_")).val(rettext); // 给结果文本赋值
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "spanShowTxt_")).html(rettext); // 给结果文本赋值
            } else {
                var replaceReg01 = new RegExp(jQuery(obj).attr("ExamItemID"), "gi"); // 需要替换的检查项目ID
                var ID_Param1 = CurrTextID.replace(replaceReg01, param1);
                var ID_Param2 = CurrTextID.replace(replaceReg01, param2);
                var value_param1 = jQuery("#" + ID_Param1).val();
                var value_param2 = jQuery("#" + ID_Param2).val();
                if (value_param1 == "") return false;
                if (value_param2 == "") return false;
                if (parseFloat(value_param1) > 0) {
                    var ret = parseFloat(value_param2) * 10000 / parseFloat(value_param1) / parseFloat(value_param1);
                    ret = Math.round(ret * 10) / 10;
                    jQuery("#" + CurrTextID).val(ret);
                    var rettext = GetWeightPointText(CurrTextID.replace(/txtSym_/gi, "AutoCalcul_"), ret);
                    jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtResult_")).val(rettext); // 给结果文本赋值
                    jQuery("#" + CurrTextID.replace(/txtSym_/gi, "spanShowTxt_")).html(rettext); // 给结果文本赋值
                }
            }
        }


        /// <summary>
        /// 根据身高，体重计数体重指数结果
        /// </summary>
        function GetWeightPointText(AutoCalculName, CalculResult) {
            var RetText = "";
            var AutoCheckValue = "";
            var tmpIsDefault = "";
            var DefaultCheckedObjID = null;
            var DiseaseLevel = 0; //病症级别 20131203 by wtang
            jQuery("input[name='" + AutoCalculName + "']").each(function () {
                tmpIsDefault = jQuery(this).attr("Is_Default");     // 是否默认值
                if (tmpIsDefault == 'True') {
                    jQuery(this).attr("checked", true);
                    RetText = jQuery(this).attr("SymptomName");     // 获取选中的体征词名称
                    AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                    DefaultCheckedObjID = jQuery(this).attr("id"); // 获取选中体征词的ID
                } else {
                    jQuery(this).removeAttr("checked");
                }
            });
            var AutoCalculNum = 0;
            var NumOperSign = "";
            var lastcalculnum = "";
            var lastcalculsign = "";
            var ncount = 0; // 计数

            jQuery("input[name='" + AutoCalculName + "']:radio").each(function () {

                if (ncount == 0 && RetText == "") { RetText = jQuery(this).attr("SymptomName"); } // 如果没有设置默认值，则先默认为第一个

                AutoCalculNum = jQuery.trim(jQuery(this).attr("AutoCalculNum")); // 获取取值
                NumOperSign = jQuery.trim(jQuery(this).attr("NumOperSign")); // 获取运算符号

                if (AutoCalculNum != "") {
                    if (NumOperSign == "<" || NumOperSign == "<=") {
                        if (lastcalculsign == "" || parseFloat(AutoCalculNum) < parseFloat(lastcalculnum))
                            if (NumOperSign == "<") {
                                if (parseFloat(CalculResult) < parseFloat(AutoCalculNum)) {
                                    RetText = jQuery(this).attr("SymptomName");
                                    AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                    jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                    jQuery(this).attr("checked", true);
                                }
                            }
                        if (NumOperSign == "<=") {
                            if (parseFloat(CalculResult) <= parseFloat(AutoCalculNum)) {
                                RetText = jQuery(this).attr("SymptomName");
                                AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                jQuery(this).attr("checked", true);
                            }
                        }
                        lastcalculnum = AutoCalculNum;
                        lastcalculsign = NumOperSign;
                    }
                    else if (NumOperSign == ">" || NumOperSign == ">=") {

                        if (lastcalculsign == "" || parseFloat(AutoCalculNum) > parseFloat(lastcalculnum))
                            if (NumOperSign == ">") {
                                if (parseFloat(CalculResult) > parseFloat(AutoCalculNum)) {
                                    RetText = jQuery(this).attr("SymptomName");
                                    AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                    jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                    jQuery(this).attr("checked", true);
                                }
                            }
                        if (NumOperSign == ">=") {
                            if (parseFloat(CalculResult) >= parseFloat(AutoCalculNum)) {
                                RetText = jQuery(this).attr("SymptomName");
                                AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                jQuery(this).attr("checked", true);
                            }
                        }
                        lastcalculnum = AutoCalculNum;
                        lastcalculsign = NumOperSign;
                    }
                }
            });
            if (RetText != "") {

                jQuery("#" + AutoCalculName.replace(/AutoCalcul_/gi, "txtHidValueResult_")).val("、" + AutoCheckValue + "、"); // 将自动匹配的ID写入到隐藏域中。
                jQuery("#" + AutoCalculName.replace(/AutoCalcul_/gi, "DiseaseLevel_")).val(DiseaseLevel); // 给小结级别赋值
                return RetText + "(" + CalculResult + ")";
            }
            else {
                jQuery("#" + AutoCalculName.replace(/AutoCalcul_/gi, "txtHidValueResult_")).val(""); // 将自动匹配的ID写入到隐藏域中。
                jQuery("#" + AutoCalculName.replace(/AutoCalcul_/gi, "DiseaseLevel_")).val(DiseaseLevel); // 给小结级别赋值
                return "";
            }
        }


        /// <summary>
        /// 计算心率结果（20131218 腰围也调用该方法，计算腰围的值）
        /// </summary>
        function AutoCalculHeartRate(obj) {
            var DiseaseLevel = 0; //病症级别 20131031 by wtang
            var AutoCalculName = "";
            var CalculResult = "";
            var SymptomNameUnit = "";
            var CurrTextID = jQuery(obj).attr("id");
            if (jQuery.trim(jQuery("#" + CurrTextID).val()) != "") {
                CalculResult = jQuery.trim(jQuery("#" + CurrTextID).val());
                AutoCalculName = CurrTextID.replace(/txtSym_/gi, "AutoCalcul_");
                SymptomNameUnit = jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtSymUnit_")).html();
            } else {
                return;
            }

            var RetText = "";
            var AutoCheckValue = "";
            var tmpIsDefault = "";
            var DefaultCheckedObjID = null;
            jQuery("input[name='" + AutoCalculName + "']").each(function () {
                tmpIsDefault = jQuery(this).attr("Is_Default");     // 是否默认值
                if (tmpIsDefault == 'True') {
                    jQuery(this).attr("checked", true);
                    RetText = jQuery(this).attr("SymptomName");     // 获取选中的体征词名称
                    AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                    DefaultCheckedObjID = jQuery(this).attr("id"); // 获取选中体征词的ID
                } else {
                    jQuery(this).removeAttr("checked");
                }
            });

            var AutoCalculNum = 0;
            var NumOperSign = "";
            var lastcalculnum = "";
            var lastcalculsign = "";
            var ncount = 0; // 计数

            jQuery("input[name='" + AutoCalculName + "']:radio").each(function () {

                if (ncount == 0 && RetText == "") {
                    RetText = jQuery(this).attr("SymptomName");
                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                } // 如果没有设置默认值，则先默认为第一个

                AutoCalculNum = jQuery.trim(jQuery(this).attr("AutoCalculNum")); // 获取取值
                NumOperSign = jQuery.trim(jQuery(this).attr("NumOperSign")); // 获取运算符号

                if (AutoCalculNum != "") {
                    if (NumOperSign == "<" || NumOperSign == "<=") {
                        if (lastcalculsign == "" || parseFloat(AutoCalculNum) < parseFloat(lastcalculnum))
                            if (NumOperSign == "<") {
                                if (parseFloat(CalculResult) < parseFloat(AutoCalculNum)) {
                                    RetText = jQuery(this).attr("SymptomName");
                                    AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                    jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                    jQuery(this).attr("checked", true);
                                }
                            }
                        if (NumOperSign == "<=") {
                            if (parseFloat(CalculResult) <= parseFloat(AutoCalculNum)) {
                                RetText = jQuery(this).attr("SymptomName");
                                AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                jQuery(this).attr("checked", true);
                            }
                        }
                        lastcalculnum = AutoCalculNum;
                        lastcalculsign = NumOperSign;
                    }
                    else if (NumOperSign == ">" || NumOperSign == ">=") {

                        if (lastcalculsign == "" || parseFloat(AutoCalculNum) > parseFloat(lastcalculnum))
                            if (NumOperSign == ">") {
                                if (parseFloat(CalculResult) > parseFloat(AutoCalculNum)) {
                                    RetText = jQuery(this).attr("SymptomName");
                                    AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                    DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                    jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                    jQuery(this).attr("checked", true);
                                }
                            }
                        if (NumOperSign == ">=") {
                            if (parseFloat(CalculResult) >= parseFloat(AutoCalculNum)) {
                                RetText = jQuery(this).attr("SymptomName");
                                AutoCheckValue = jQuery(this).attr("value");    // 获取选中项的value
                                DiseaseLevel = jQuery(this).attr("DiseaseLevel");
                                jQuery('#' + DefaultCheckedObjID).removeAttr("checked");
                                jQuery(this).attr("checked", true);
                            }
                        }
                        lastcalculnum = AutoCalculNum;
                        lastcalculsign = NumOperSign;
                    }
                }
            });

            if (RetText != "") {
                RetText = RetText + "(" + CalculResult + " " + SymptomNameUnit + ")";
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtHidValueResult_")).val("、" + AutoCheckValue + "、"); // 将自动匹配的ID写入到隐藏域中。
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtResult_")).val(RetText); // 给结果文本赋值
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "spanShowTxt_")).html(RetText); // 给结果文本赋值
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "DiseaseLevel_")).val(DiseaseLevel); // 给小结级别赋值
            } else {
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtHidValueResult_")).val(""); // 将自动匹配的ID写入到隐藏域中。
            }


        }



        /// <summary>
        /// 自动计算腰臀围函数
        /// </summary>
        /// <param name="obj">腰臀围框对象（this）</param>
        /// <param name="param1">第一个框的数据库编号（用数据库编号，转换成为文本框的ID）</param>
        /// <param name="param2">第二个框的数据库编号（用数据库编号，转换成为文本框的ID）</param>
        function AutoCalculWaistButtock(obj, param1, param2) {

            var CurrTextID = jQuery(obj).attr("id");
            if (jQuery.trim(jQuery("#" + CurrTextID).val()) != "") {
                var ret = jQuery.trim(jQuery("#" + CurrTextID).val());
                var rettext = ret;
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtResult_")).val(rettext); // 给结果文本赋值
                jQuery("#" + CurrTextID.replace(/txtSym_/gi, "spanShowTxt_")).html(rettext); // 给结果文本赋值
            } else {
                var replaceReg01 = new RegExp(jQuery(obj).attr("ExamItemID"), "gi"); // 需要替换的检查项目ID
                var ID_Param1 = CurrTextID.replace(replaceReg01, param1);
                var ID_Param2 = CurrTextID.replace(replaceReg01, param2);
                var value_param1 = jQuery("#" + ID_Param1).val();
                var value_param2 = jQuery("#" + ID_Param2).val();
                if (value_param1 == "") return false;
                if (value_param2 == "") return false;
                if (parseFloat(value_param1) > 0) {
                    var ret = parseFloat(value_param1) / parseFloat(value_param2);
                    ret = Math.round(ret * 100) / 100;
                    jQuery("#" + CurrTextID).val(ret);
                    var rettext = ret;
                    jQuery("#" + CurrTextID.replace(/txtSym_/gi, "txtResult_")).val(rettext); // 给结果文本赋值
                    jQuery("#" + CurrTextID.replace(/txtSym_/gi, "spanShowTxt_")).html(rettext); // 给结果文本赋值
                }
            }
        }




        /// <summary>
        /// 绑定用户，绑定后只能由该医生或护士进行体检，如果需要其他人员操作，需要进行解除绑定操作 (绑定)
        /// </summary>
        function BandingCustomerSectionExamInfo() {

            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());         // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var ID_CustExamSection = jQuery("#ID_CustExamSection").val();   // 科室小结编号
            var SectionID = jQuery('#txtSectionID').val();

            jQuery.ajax({
                type: "POST",
                url: "/Ajax/AjaxCustExam.aspx",
                data: { ID_CustExamSection: ID_CustExamSection,
                    ID_Customer: ID_Customer,
                    SectionID: SectionID,
                    action: 'BandingCustomerSectionExamInfo',
                    currenttime: encodeURIComponent(new Date())
                },
                cache: false,
                dataType: "text",
                success: function (jsonmsg) {
                    // 检查Ajax返回数据的状态等 20150421 by wtang
                    jsonmsg = CheckAjaxReturnDataInfo(jsonmsg);
                    if (jsonmsg == null || jsonmsg == "") {
                        return;
                    }

                    // 返回值是录入时间或为空
                    jQuery("#TypistDate").val(jsonmsg);   //    录入时间

                    // 更新后，不用做任何的页面提示
                }
            });
        }

        /// <summary>
        /// 设置医生下拉框中的默认值 
        /// </summary>
        function SetDefaultExamDoctor(ID_Doctor, DoctorName) {

            // 显示医生下拉框中的默认值 
            jQuery("#ResultSummaryDoctor").find("option").removeAttr("selected");
            jQuery("#ResultSummaryDoctor").attr("value", ID_Doctor); // 医生编号
            jQuery("#s2id_ResultSummaryDoctor .select2-choice span").text(DoctorName); //显示选中项的文本

            jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                // 设置收费项目对应的医生ID，和 姓名
                jQuery("#ExamDoctor_" + feeitem.ID_Fee).val(ID_Doctor);
                jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName", DoctorName);
                jQuery("#Lab_ExamDoctor_" + feeitem.ID_Fee).html(DoctorName);

            });

            //            jQuery("[name='txtExamDoctor']").val(ID_Doctor);      // 医生ID
            //            jQuery("[name='spanExamDoctor']").html(DoctorName);   // 医生姓名
            //            jQuery("[name='txtExamDoctor']").attr("ExamDoctor", DoctorName);    // 医生姓名
            //            jQuery("[name='Lab_ExamDate']").html(jQuery("#DateToday").val());   // 显示当前时间

        }

        /// <summary>
        /// 控制隐藏，显示打印条码的按钮  Add by WTang 20130922 
        /// </summary>
        function ShowHideIsPrintSectionBarCode() {
            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());         // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var SectionID = jQuery('#txtSectionID').val();
            var Is_PrintBarCode = jQuery.trim(jQuery("#Is_PrintBarCode").val());
            if ((Is_PrintBarCode == "0" || Is_PrintBarCode == "1") && ID_Customer != undefined && ID_Customer != "") {
                jQuery("#spanPrintSectionBarCode").show();
            } else {
                jQuery("#spanPrintSectionBarCode").hide();
            }

            var IsSectionShowComRead = jQuery("#IsSectionShowComRead").val();               // 判断当前科室是否需要显示Com数据读取控件  20140213 by wtang
            if (IsSectionShowComRead == "1") {
                InitComRead();              // 初始化Com数据读取控件
                jQuery(".btn_recvcomdata").show(); // com接口读取按钮是否显示
            }
            else {
                jQuery(".btn_recvcomdata").hide(); // 隐藏com接口读取按钮
            }

            var IsShowWriteCustomerFileSectionBtn = jQuery("#IsShowWriteCustomerFileSectionBtn").val();               // 是否显示 生成本地体检号对应文件的按钮
            if (IsShowWriteCustomerFileSectionBtn == "1") {
                jQuery(".Btn_IsShowWriteCustomerFileSection").show(); // 显示生成本地体检号对应文件的按钮
            }
        }

        /// <summary>
        /// 打印条码 
        /// </summary>
        function PrintSectionBarCode() {

            var ID_Customer = jQuery.trim(jQuery("#txtHiddenCustomerID").val());            // 体检号，该值从隐藏域中获取，防止页面上进行了修改。
            var CurrSectionID = jQuery.trim(jQuery('#txtSectionID').val());                 // 当前科室ID
            var TagPrintRelSectionID = jQuery.trim(jQuery('#TagPrintRelSectionID').val());  // 被打印的科室
            var IsPrintCurrSectionTag = jQuery.trim(jQuery('#IsPrintCurrSectionTag').val()); // 当打印科室 与 被打印科室 不一致是，标记是否打印当前科室的标签
            var TagPrintTemplete = jQuery.trim(jQuery('#TagPrintTemplete').val());          // 标签模版

            if (TagPrintTemplete == '') {
                ShowSystemDialog("配置错误，标签模版不能为空！");
                return;
            }

            // 打印标签  直接从配置文件中读取，打印的科室，及打印调用的模版
            FastReport.GenerateSectionBarCode(ID_Customer, TagPrintRelSectionID, TagPrintTemplete);
            // 是否打印当前科室
            if (TagPrintRelSectionID != CurrSectionID && IsPrintCurrSectionTag == 'True') {
                FastReport.GenerateSectionBarCode(ID_Customer, CurrSectionID, TagPrintTemplete);
            }

            //            // 打印科室条码  心电图室（401） 打印带条码的标签 
            //            if (TagPrintSectionID == 401) {
            //                FastReport.GenerateSectionBarCode(ID_Customer, TagPrintSectionID, "SectionBarCodeX_B.frx");
            //            } // 打印科室条码  专项检查室（445） 打印两页（不带条码） 
            //            else if (TagPrintSectionID == 445) {
            //                FastReport.GenerateSectionBarCode(ID_Customer, TagPrintSectionID, "NoBarCodeOfC13.frx");
            //            } else {
            //                // 妇科打印病理科的条码
            //                FastReport.GenerateSectionBarCode(ID_Customer, TagPrintSectionID, "SectionNoBarCode.frx");

            //                // 修改病理科打印的时候，同时需要打印妇科的一个标签 by WTang 
            //                if (TagPrintSectionID != "" && TagPrintSectionID != CurrSectionID) {
            //                    FastReport.GenerateSectionBarCode(ID_Customer, CurrSectionID, "SectionNoBarCode.frx");
            //                }
            //            }
        }

        function ShowHideFeeExamItem(ID_Fee) {
            if (jQuery("#ShowHideFee_" + ID_Fee).attr("openclosestate") == "open") {
                jQuery("#ShowHideFee_" + ID_Fee).attr("openclosestate", "close");
                jQuery("#ShowHideFee_" + ID_Fee).removeClass("j-fenkTips");
                jQuery("#ShowHideFee_" + ID_Fee).addClass("j-fenkTips-h");
                jQuery("#CustExamList tr[name='trExamFee_" + ID_Fee + "']").each(function () {
                    jQuery(this).show();
                });
            } else {
                jQuery("#ShowHideFee_" + ID_Fee).attr("openclosestate", "open");
                jQuery("#ShowHideFee_" + ID_Fee).addClass("j-fenkTips");
                jQuery("#ShowHideFee_" + ID_Fee).removeClass("j-fenkTips-h");
                jQuery("#CustExamList tr[name='trExamFee_" + ID_Fee + "']").each(function () {
                    jQuery(this).hide();
                });
            }

        }


                function ResultSummaryDoctorChange(obj) {

                }

                // 选择用户后的回调函数
                function SetSelectedUserCallBack() {

                    var SelectValue = jQuery("#idSelectUser").val();
                    var SelectName = jQuery("#nameSelectUser").val(); 

//                  var SelectName = jQuery("#" + obj.name).find("option:selected").text();
//                  var SelectValue = jQuery("#" + obj.name).val();

                    //                    var tmpDate = jQuery("#SectionSummaryDate").val().split(" "); // 小结时间
                    // 遍历收费项目
                    jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                        // 设置收费项目对应的医生ID，和 姓名
                        jQuery("#ExamDoctor_" + feeitem.ID_Fee).val(SelectValue);
                        jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName", SelectName);
                        jQuery("#Lab_ExamDoctor_" + feeitem.ID_Fee).html(SelectName);

                        // jQuery("[name='Lab_ExamDate']").html(jQuery("#DateToday").val());    //检查时间，不随着检查医生的改变而改变，任然保留为第一次的时间

                        //                        if (SelectName == jQuery("#SummaryDoctorName").val()) {
                        //                            jQuery("[name='Lab_ExamDate']").html(tmpDate[0]);    //如果医生是上次保存的医生，则还是保存上次的时间
                        //                        }
                    });
                }


                // 移除选择的医生
                function RemoveSelectedExamDoctor() {

                    var SelectValue = "0";
                    var SelectName = "--";

                    // 遍历收费项目
                    jQuery(gPageCustomExamDataMsg.dataList0).each(function (i, feeitem) {

                        // 设置收费项目对应的医生ID，和 姓名
                        jQuery("#ExamDoctor_" + feeitem.ID_Fee).val(SelectValue);
                        jQuery("#ExamDoctor_" + feeitem.ID_Fee).attr("DoctorName", SelectName);
                        jQuery("#Lab_ExamDoctor_" + feeitem.ID_Fee).html(SelectName);

                    });
                    jQuery('#txtUserInputCode').focus();
                }
