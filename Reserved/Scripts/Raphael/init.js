function setCookie(name, value) {
    document.cookie = name + "=" + value;
}

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function Close() {
    debugger;
    var t = $(this),
        parent = t.parent(".point");

    parent.fadeOut(function () {
        parent.remove();
    });
    return false;
}

function makeOrder() {
    var name = document.getElementById("name").value;
    var phone = document.getElementById("phone").value;
    //debugger;
    var table = getCookie("table").substr(1, getCookie("table").length - 1);
    var time = document.getElementById("inputTime").value;
    time = time.substr(0, 2) + "." + time.substr(3, 2);
    var date = document.getElementById("inputDate").value;
    insertInformationOrder(name, phone, table, time, date);
}

function insertInformationOrder(name, phone, table, time, date) {
    debugger;
    var path = window.location.href.substr(0, window.location.href.length - 2);
    $.ajax({
        //Передаём введённые данные на сервер и получаем ответ
        type: "POST",
        //url: window.location.href.substr(0, window.location.href.length - 2) + "/InsertInformationOrder", //url: адрес текущей страницы / имя статического метода на стороне сервера
        url: path + "/InsertInformationOrder", //url: адрес текущей страницы / имя статического метода на стороне сервера
        data: "{'name': '" + name + "', 'phone': '" + phone + "', 'table': '" + table + "', 'time': '" + time + "', 'date': '" + date + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        cache: false,
        success: function (data, textStatus, XHR) {
            debugger;
            var idOrder = data.d.toString();
            alert("Информация о Вышей брони:\n  Стол №" + table + "\n  Время бронирования: " + time + "\n  Номер брони: " + idOrder);
            
        }
    });
    return false;
}

//$(function () {
function paintTable() {
    //debugger;
    var freeBox = getCookie("freeBox").split(" ");
    var hall = getCookie("hall");

    var r = Raphael("map" + hall, 500, 340),
        attributes = {
            fill: "#C5BFC1",
            stroke: "#3899E6",
            'stroke-width': 1,
            'stroke-linejoin': "round"
        },
        arr = new Array();

    //if (table in freeBox) alert("Элемент с ключём 3 присутствует в массиве");

    var path = paths[hall];

    for (var table in path) {
        if (path.hasOwnProperty(table)) {
            if ($.inArray(table, freeBox) > -1) {
                var obj = r.path(path[table].path);
                obj.attr(attributes);
                arr[obj.id] = table;
                obj
                    .hover(function() {
                        this.animate({
                            fill: "#E6F21B"
                        }, 300);
                    }, function() {
                        this.animate({
                            fill: attributes.fill
                        }, 300);
                    })
                    .click(function() {
                        document.location.hash = arr[this.id];

                        var point = this.getBBox(0);
                        $("#map" + hall).next(".point").remove();
                        $("#map" + hall).after($("<div />").addClass("point"));
                        setCookie("table", (path[arr[this.id]].name).toString());
                        document.getElementById("ss").innerHTML = path[arr[this.id]].name;

                        $(".point")
                            //.html("Стол " + path[arr[this.id]].name + " " + path[arr[this.id]].seats + "\n" + path[arr[this.id]].description)
                            .html("<label class=\"res-label\">Стол " + path[arr[this.id]].name + "</label>" +
                                  "<label class=\"res-label\">Количество мест " + path[arr[this.id]].seats + "</label>" +
                                  "<label class=\"res-label\">Описание " + path[arr[this.id]].description + "</label>" +
                                  "<input id=\"name\" class=\"res-input\" placeholder=\"Имя\"/><br/>" +
                                  "<input id=\"phone\" class=\"res-input\" placeholder=\"Телефон\"/>")
                            .prepend($("<a />").attr("href", "#").addClass("close").text("Закрыть"))
                            .css({
                                left: point.x + (point.width / 2) - 80,
                                top: point.y + (point.height / 2) - 20
                            })
                            .prepend($("<a />").attr("href", "#").addClass("complite").text("Подтвердить заказ"))
                            .css({
                                left: point.x + (point.width / 2) - 80,
                                top: point.y + (point.height / 2) - 20
                            })
                            //.prepend($("<input />").attr("placeholder", "Имя"))
                            .fadeIn();
                    }); 

                $(document).on("click", ".close", function () {
                    //debugger;
                    var t = $(this),
                        parent = t.parent(".point");
                    parent.fadeOut(function() {
                        parent.remove();
                    });
                    return false;
                });
                $(document).on("click", ".complite", function () {
                    //debugger;
                    var t = $(this),
                        parent = t.parent(".point");
                    parent.fadeOut(function () {
                        makeOrder();
                        parent.remove();
                    });
                    return false;
                });

            }
        }
    }
    //});
}

