var menu = {};

//导航栏选择
menu.selectNav = function () {
     var asd= $('#nav').tabs('getTab', 0);
     if (asd ==null) {
        $.messager.alert({
            title: '提示',
            msg: '未给您分配角色，请联系管理员。',
            fn: function () {
                location.href = "/Account/LogOut";
            }
        });
    }
    $("#nav").tabs({
        selected: 0,
        onSelect: function (title, index) {
            var pp = $("#nav").tabs("getSelected");
            var tabid = pp.panel("options").id;
            menu.getLeftNavList(tabid);
        },
        onUnselect: function (title, index) {
            menu.clearLeftMenuList();
        }
    });
}

//获取已选择的菜单
menu.getSelectMenu = function() {
    this.selectNav();
    var pp = $("#nav").tabs("getSelected");
    var tabid = pp.panel("options").id;
    this.getLeftNavList(tabid);
}

//清空左边菜单
menu.clearLeftMenuList = function() {
    var $leftNav = $("#leftNav");
    var pp = $leftNav.accordion("panels");
    var index = 0;
    while (pp.length) {
        $leftNav.accordion("remove", index);
    }
}

//获取左边列表
menu.getLeftNavList = function (id) {
    var options = { url: "/Menu/GetMenuList/", data: { id: id }, fnSuccess: this.getLeftNavListSucc };
    utils.sendUrl(options);
}

//获取左边列表成功回调函数
menu.getLeftNavListSucc = function(data) {
    menu.getLeftAllMenuList(data);
}

//获取左边菜单列表
menu.getLeftAllMenuList = function (data) {
    var $leftNav = $("#leftNav"), content = "";
    $.parser.parse();
    $.each(data, function (i, n) {
        content = '<div class="navlist" id="mainMenu' + n.ModuleID + '" title="' + n.Name + '" style="padding:10px" title="" + n.Name + "" index="" + n.ModuleID + "" ></div>';
        $leftNav.accordion("add", {
            id: n.ModuleID,
            title: n.Name,
            iconCls: "",
            selected: 0,
            content: content
        });
        $("#mainMenu" + n.ModuleID).html('<ul id="childMenu' + n.ModuleID + '" style=""></ul>');

        $("#childMenu" + n.ModuleID).tree({
            url: "/Menu/GetAllChildMenuList/?id=" + n.ModuleID,
            loadFilter: function (rows) {
                var filter = utils.filterProperties(rows, ['ModuleID as id', 'Name as text', 'ParentModuleID as pid', 'Path as url']);
                var data = utils.toTreeData(filter, 'id', 'pid', "children");
                return data;
            },
            onClick: function (node) {
                menu.addPanel(node.url, node.text);
            },
            onLoadSuccess: function(node, data) {
                $(this).tree("collapseAll");
            }
        });
    });
    $leftNav.accordion("select", 0);
}

//导航到主页面
menu.addPanel = function (url, title) {
    if (!url) {
        return;
    }
    if (!$("#mainPanel").tabs("exists", title)) {
        $("#mainPanel").tabs("add", {
            title: title,
            content: '<iframe src=' + url + ' frameborder="0" border="0"  style="width: 100%; overflow: hidden;  height: 98%;"/>',
            closable: true
        });
    } else {
        $("#mainPanel").tabs("select", title);
    }
    $("#mainPanel").contextMenus();
}
