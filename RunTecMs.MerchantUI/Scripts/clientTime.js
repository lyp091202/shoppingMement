(function () {
    function showTime() {
        var showDay = new Array('星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六');
        var time = new Date();
        var year = time.getFullYear();
        var month = parseInt(time.getMonth());
        var date = time.getDate();
        var day = time.getDay();
        var hour = time.getHours();
        var minutes = time.getMinutes();
        var second = time.getSeconds();
        month = parseInt(month) + 1;
        month < 10 ? month = '0' + month : month;
        hour < 10 ? hour = '0' + hour : hour;
        minutes < 10 ? minutes = '0' + minutes : minutes;
        second < 10 ? second = '0' + second : second;
        var nowTime = +year + '年' + month + '月' + date + '日' + ' ' + showDay[day] + ' ' + hour + ':' + minutes + ':' + second;
        return nowTime;
    }
    var dom = document.getElementById("nowTime");
    setInterval(function () { $("#nowTime").text(showTime()) }, 1000);
} ()); 
