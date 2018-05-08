/**
 * Created by gjq on 2017/2/24.
 */
function cleanUp(domid) {
    $(domid).text("");
};
function removedomid(domid) {
    $(domid).remove();
}
function removedomclass(domid, classname) {
    $(domid).removeClass(classname)
}
function adddomClass(domid, classname) {
    $(domid).addClass(classname);
}
/*
 * 获取url中带的参数
 * var url = window.location.href
 * url 是thispage的url
 */
function getUrlParam(thisPageUrl, name) {
    var pattern = new RegExp("[?&]" + name + "\=([^&]+)", "g");
    var matcher = pattern.exec(thisPageUrl);
    var items = null;
    if (matcher != null) {
        try {
            items = decodeURIComponent(decodeURIComponent(matcher[1]));
        } catch (e) {
            try {
                items = decodeURIComponent(matcher[1]);
            } catch (e) {
                items = matcher[1];
            }
        }
    }
    return items;
};

// 设置cookie
function SetCookies(name, value) {
    var exp, y, m, d;
    exp = new Date();
    exp.setHours(exp.getHours() + 4);//定期保存cookies
    document.cookie = name + "=" + escape(value) + "; expires=" + exp.toGMTString() + "; path=/";
}
//读取cookies
function getCookies(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");//正则匹配
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}


//清除cookies
function delCookies(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookies(name);
    if (cval != null) {
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    }
}


//清除所有cookies
function clearAllCookies() {
    var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
    if (keys) {
        for (var i = keys.length; i--;)
            document.cookie = keys[i] + '=0;expires=' + new Date(0).toGMTString()
    }
}




/**
 * 拍照并上传
 * <video id="video" width="640" height="480" autoplay></video>
 * <button id="snap">Snap Photo</button>
 *<canvas id="canvas" width="640" height="480"></canvas>
 */
//function takepicture() { 
//window.addEventListener("DOMContentLoaded",
    function takePicture() {
    try { document.createElement("canvas").getContext("2d"); } catch (e) { alert("not support canvas!") }
    var video = document.getElementById("video"),
        canvas = document.getElementById("canvas"),
        context = canvas.getContext("2d");
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;

    if (navigator.getUserMedia)
        navigator.getUserMedia(
            { "video": true },
            function (stream) {
                if (video.mozSrcObject !== undefined) video.mozSrcObject = stream;
                else video.src = ((window.URL || window.webkitURL || window.mozURL || window.msURL) && window.URL.createObjectURL(stream)) || stream;
                video.play();
            },
            function (error) {
                //if(error.PERMISSION_DENIED)console.log("用户拒绝了浏览器请求媒体的权限",error.code);
                //if(error.NOT_SUPPORTED_ERROR)console.log("当前浏览器不支持拍照功能",error.code);
                //if(error.MANDATORY_UNSATISFIED_ERROR)console.log("指定的媒体类型未接收到媒体流",error.code);
                alert("Video capture error: " + error.code);
            }
        );
    else alert("Native device media streaming (getUserMedia) not supported in this browser");
   $("#snap").bind("click", function () {
        context.drawImage(video, 60, 0, 160, 120);//照片大小
        //context.drawImage(video, 0, 0, canvas.width = video.videoWidth, canvas.height = video.videoHeight);
         base64img = canvas.toDataURL("image/png");
        //return base64img;
        //$.post('/home/index', { "img": base64img }, function (data, status) {
        //    alert(status != "success" ? "图片处理出错！" : data == "yes" ? "图片上传完成！" : data);
        //}, "text");
    });
}
//, false);
//}
//检测是否含有子节点
function haschild(domname, son) {
    if (son === 1) {
        var child = $(domname, parent.document).children();
        if (child.length > 0) {
            return 1
        } else {
            return 0
        }
    }else if(son===0){
    var child = $(domname).children();
    if (child.length>0) {
        return 1
    } else {
        return 0
    }
    }
}
//获取xx天以前的日期
//获取今天的日期，但是这个数据的格式不标准  
//也就是把 Fri Jan 06 2017 10:43:07 GMT+0800 转化成 2017-01-06 这种格式  
function beforeDay(beforeDayNum) {
    var d = new Date();
    var endDate = dateToString(d);
    //返回日期的原始值，也就是自xxx年xx月xx日 到今天的日期相差的毫秒数  
    d = d.valueOf();
    d = d - beforeDayNum * 24 * 60 * 60 * 1000;
    d = new Date(d);
    var startDate = dateToString(d);
    return startDate;
}
function dateToString(d) {
    var y = d.getFullYear();
    var m = d.getMonth() + 1;
    var d = d.getDate();

    //把日期2017-1-6 格式化为标准的 2017-01-06  
    //判断数字的长度是否是1，如果是1那么前面加上字符0  
    if (m.toString().length == 1) m = "0" + m;
    if (d.toString().length == 1) d = "0" + d;

    return y + "-" + m + "-" + d;
}
//比较两个字符串的相似度
function compare(x, y) {
    var z = 0;
    var s = x.length + y.length;;

    //x.sort();
    //y.sort();
    var a = x.substring(z, z+1);
    var b = y.substring(z, z+1);

    while (a !== undefined && b !== undefined) {
        if (a === b) {
            z++;
            a = x.substring(z, z + 1);
            b = y.substring(z, z + 1);
        } else {
            return 0;
        }
    }
    return z / s * 200;
}

//console.log(compare("hello", "hello"))
//console.log(compare(['1234', '中文', 'hello'], ['123', '中文', 'hello'].sort()))
//console.log(compare(['123', '中文', 'hello'], ['123', '中文', 'hello'].reverse()))
//console.log(compare(['123', '中文', 'hello', '中2文'], ['12', '中2文', '123', '中文3']))
//console.log(compare(['123', '中文', 'hello'], ['中文', 'world', '456']))
//console.log(compare(['123', '中3文', 'hello'], ['中文', 'world', '汉字']))
/*
 string 字符串;
 str 指定字符;
 split(),用于把一个字符串分割成字符串数组;
 split(str)[0],读取数组中索引为0的值（第一个值）,所有数组索引默认从0开始;
 */
function getStr(string, str) {
    var str_before = string.split(str)[0];
    var str_after = string.split(str)[2];
    alert('前：' + str_before + ' - 后：' + str_after);
}
//getStr("stringringwerwe", "r");