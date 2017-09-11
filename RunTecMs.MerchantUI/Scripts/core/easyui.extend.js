$.fn.contextMenus = function () {
    var $tabs = $(this);

    var temphtml = []; temphtml.push('<div id="tabs-contextmenuparent" style="display:none;"><div id="tabs-contextmenu" class="easyui-menu" style="width:150px">');
    temphtml.push('<div id="mm-tabrefresh">刷新</div>');
    temphtml.push('<div class="menu-sep"></div>');
    temphtml.push('<div id="mm-tabclose">关闭</div>');
    temphtml.push('<div id="mm-tabcloseall">关闭全部</div>');
    temphtml.push('<div id="mm-tabcloseother">关闭其他</div>');
    temphtml.push('<div class="menu-sep"></div>');
    temphtml.push('<div id="mm-tabcloseright">关闭右侧标签</div>');
    temphtml.push('<div id="mm-tabcloseleft">关闭左侧标签</div>');
    temphtml.push('</div></div>');

    $("body").append(temphtml.join(''));
    $.parser.parse($("#tabs-contextmenuparent"));
    var $menus = $("#tabs-contextmenu");

    $(document).on("dblclick", ".tabs-inner", function () {
        var currtabTitle = $(this).children("span").text();
        var $link = $(".tabs-title:contains(" + currtabTitle + ")", $tabs);
        if ($link.is(".tabs-closable")) {
            $tabs.tabs("close", currtabTitle);
        }
    });

    $(document).on("contextmenu", ".tabs-inner", function (e) {
        $menus.menu("show", {
            left: e.pageX,
            top: e.pageY
        });
        var subtitle = $(this).children("span").text();
        $menus.data("currtab", subtitle);
        return false;
    });

    //刷新当前
    $("#mm-tabrefresh", $menus).click(function () {
        var currTab = $tabs.tabs("getSelected");
        var url = $(currTab.panel("options").content).attr("src");
        $tabs.tabs("update", {
            tab: currTab,
            options: {
                content: '<iframe src=' + url + ' frameborder="0" border="0"  style="width: 100%; overflow: hidden;  height: 98%;"/>'
            }
        });
    });

    //关闭当前
    $("#mm-tabclose", $menus).click(function () {
        var currtabTitle = $menus.data("currtab");
        var $link = $(".tabs-title:contains(" + currtabTitle + ")", $tabs);
        if ($link.is(".tabs-closable")) {
            $tabs.tabs("close", currtabTitle);
        }
    });

    //全部关闭
    $("#mm-tabcloseall", $menus).click(function () {
        $(".tabs-inner span", $tabs).each(function (i, n) {
            if ($(this).is(".tabs-closable")) {
                var t = $(n).text();
                $tabs.tabs("close", t);
            }
        });
    });

    //关闭除当前之外的TAB
    $("#mm-tabcloseother", $menus).click(function () {
        var currtabTitle = $menus.data("currtab");
        $(".tabs-inner span").each(function (i, n) {
            if ($(this).is(".tabs-closable")) {
                var t = $(n).text();
                if (t !== currtabTitle) {
                    $tabs.tabs("close", t);
                }
            }
        });
    });

    //关闭当前右侧的TAB
    $("#mm-tabcloseright").click(function () {
        var currtabTitle = $menus.data("currtab");
        var $li = $(".tabs-title:contains(" + currtabTitle + ")", $tabs).parent().parent();
        var nextall = $li.nextAll();
        nextall.each(function (i, n) {
            if ($("a.tabs-close", $(n)).length > 0) {
                var t = $("a:eq(0) span", $(n)).text();
                $tabs.tabs("close", t);
            }
        });
        $tabs.tabs("select", currtabTitle);
        return false;
    });

    //关闭当前左侧的TAB
    $("#mm-tabcloseleft").click(function () {
        var currtabTitle = $menus.data("currtab");
        var $li = $(".tabs-title:contains(" + currtabTitle + ")", $tabs).parent().parent();
        var prevall = $li.prevAll();
        prevall.each(function (i, n) {
            if ($("a.tabs-close", $(n)).length > 0) {
                var t = $("a:eq(0) span", $(n)).text();
                $tabs.tabs("close", t);
            }

        });
        $tabs.tabs("select", currtabTitle);
        return false;
    });
};