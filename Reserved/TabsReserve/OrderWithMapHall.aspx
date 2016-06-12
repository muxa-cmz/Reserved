<%@ Page Title="Забронировать" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="OrderWithMapHall.aspx.cs" Inherits="Reserved.TabsReserve.OrderWithMapHall" %>

<%@ Register TagPrefix="cc1" Namespace="HallList" Assembly="HallList" %>
<%@ Register TagPrefix="cc1" Namespace="HallMap" Assembly="HallMap" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <style>
        .radios-as-buttons div {
            float: left;
        }
        .radios-as-buttons input {
            position: absolute;
            left: -9999px;
        }
        .radios-as-buttons label {
            display: block;
            font-size: 10px;    /**/
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif; /**/
            margin: 0 0 0 -1px;
            padding: 5px 5px;
            border: 1px solid #BBBBBB;
            /*background: linear-gradient(to bottom, rgba(3,74,243, 0.8) 0%, rgba(229,229,229,1) 100%);*/
            background: white;
            /*background: #59cde2;*/
            box-shadow: 0 2px 5px rgba(0, 0, 0, .12);
            cursor: pointer;

            display: inline-block;
            -webkit-user-select: none; 
            -moz-user-select: none; 
            -ms-user-select: none; 
            -o-user-select: none; 
            user-select: none;
        }
        .radios-as-buttons input:checked + label {
            /*background: white;*/
            background: #59cde2;
            box-shadow: inset 0 3px 6px rgba(0, 0, 0, .2);
        }

        .radios-as-buttons input:disabled + label {
            background: #cccccc;
        }
        
        .radios-as-buttons div:nth-child(even) label {
            margin-left: 5px;
            margin-top: 5px;
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        .radios-as-buttons div:nth-child(odd) label {
            margin-left: 5px;
            margin-top: 5px;
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }
    </style>
    
    <script src="//code.jquery.com/jquery-1.12.0.min.js"></script>
    <link href="/Libs/Metro-UI-CSS-master/build/css/metro.css" rel="stylesheet" />
    <script src="/Libs/Metro-UI-CSS-master/build/js/metro.js"></script>
    <link href="/Libs/Metro-UI-CSS-master/build/css/metro-icons.css" rel="stylesheet" />
    
    <link href="../Scripts/ClockPicker/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Scripts/ClockPicker/dist/bootstrap-clockpicker.min.css" rel="stylesheet" />
    <link href="../Scripts/ClockPicker/assets/css/github.min.css" rel="stylesheet" />
    <script src="../Scripts/ClockPicker/assets/js/bootstrap.min.js"></script>
    <script src="../Scripts/ClockPicker/dist/bootstrap-clockpicker.min.js"></script>

    <style>
        .h-content {
            float: right;
            position: relative;
            padding: 5px 0 0 5px;
        }
        .h-sidebar {
            float: left;
            width: 20%;
            padding: 5px 0 0 5px;
            position: relative;
        }
        .h-wrap {
            width: 60%;
            margin: 0 auto;
            background: #d9d9d9;
            position: relative;
        }
        ul {
            padding: 0;
        }
        li {
            list-style-type: none; /* Убираем маркеры */
            margin: 5px;
        }
        .customLi {
            border: 1px solid #333; /* Рамка */
            border-radius: 5px;
            border-color: #ccc;
            display: inline-block;
            padding: 5px 15px; /* Поля */
            text-decoration: none; /* Убираем подчёркивание */
            color: #000; /* Цвет текста */
            background-color: #f8f8f8;
         }
        .customLi:hover {
            box-shadow: 0 0 5px rgba(0,0,0,0.3); /* Тень */
            background: linear-gradient(to bottom, #f8f8f8, #f8f8f8); /* Градиент */
            /*color: #a00;*/
         }
    </style>
    
    <style>
        .wrapper {
             margin:0 auto;
             width:500px;
             position:relative;
             /*background-image: url(/Image/2.jpg);*/
         }
        .map {
            float:left;
            clear:both;
            width:500px;
            height:340px;
        }
        .point {
            position:absolute;
            display:none;
            padding:10px 15px;
            background:#7BB9F0;
            font-size:14px;
            font-weight:bold;
            -moz-border-radius:8px;
            -webkit-border-radius:8px;
            border-radius:8px;
        }
        .point .close {
            display:block;
            position:absolute;
            top:-10px;
            right:-10px;
            width:24px;
            height:24px;
            text-indent:-9999px;
            outline:none;
            background:url(/Image/close.png) no-repeat;
        }
        .point img {
            vertical-align:middle;
            margin-right:10px;
        }
        a img {
            border:none;
        }
    </style>
    
    
    <script src="../Scripts/Raphael/raphael.js" type="text/javascript"></script>
    <script src="../Scripts/Raphael/init.js"></script>
    <script src="../Scripts/Raphael/paths.js"></script>
    

    <div class="h-wrap">
        <div class="h-content">
            <cc1:HallMap runat="server" ID="hallMap"/>
        </div>
        <div class="h-sidebar">
            <div class="h-section" style="margin: 5px;">
                <label>1. Выберите дату</label>
                <div class="input-control text" 
                        data-week-start="1" 
                        data-start-mode="month"  
                        id="datepicker">
                    <input id="inputDate" type="text"/>
                    <button class="button"><span class="mif-calendar" ></span></button>
                </div>
            </div>
            <div class="h-section" style="margin: 5px;">
                <label>2. Выберите время</label>
                <div class="input-group clockpicker">
                    <input id="inputTime" type="text" class="form-control" value="09:30"/>
	                <span class="input-group-addon">
		                <span class="glyphicon glyphicon-time"></span>
	                </span>
                </div>
            </div>
            <div class="h-section" style="margin: 5px;">
                <label>3. Выберите зал</label>
                <div>
                    <cc1:HallList runat="server" ID="hallList"/>
                </div>
            </div>
        </div>
    </div>
    
    <script>
        function setCookie(name, value) {
            document.cookie = name + "=" + value;
        }

        function viewHall(value) {
            var len = document.getElementsByClassName('wrapper').length;
            for (var i = 1; i <= len; i++) {
                if (i === value) {
                    document.getElementById("desc_hall" + i).style = "";
                    // вызвать метод для поиска свободных столов в зале
                    var date = $('#inputDate').val();
                    var time = $('#inputTime').val();
                    viewFreeTablesHall(value, date, time);
                    PaintTable();
                }
                else {
                    document.getElementById("desc_hall" + i).style = "display: none;";
                }
            }
            setCookie("hall", value);
        }

        function viewFreeTablesHall(value, date, time) {
            $.ajax({    //Передаём введённые данные на сервер и получаем ответ
                type: "POST",
                url: window.location.href + '/ViewFreeTablesHall', //url: адрес текущей страницы / имя статического метода на стороне сервера
                data: "{'hallId': '" + value + "', 'date': '" + date + "', 'time': '" + time + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true, cache: false,
                success: function (data, textStatus, XHR) {
                    var arrayTable = data.d.toString();
                    setCookie("freeBox", arrayTable);
                    //var jObject = JSON.parse(data.d.toString());
                    //var elements = jObject["array"];
                    //for (var i = 0; i < elements.length; i++) {
                    //    var id = elements[i].interval;
                    //    var flag = elements[i].flag;
                    //    document.getElementById('radio' + id).disabled = !flag;
                    //}
                }
            });
            return false;
        }
    </script>

    <script>
        var myDate = new Date();
        var dayOfMonth = myDate.getDate();
        myDate.setDate(dayOfMonth - 1);
        $("#datepicker").datepicker({
            locale: "ru",
            format: "dd.mm.yyyy",
            position: "bottom",
            minDate: myDate
        });
    </script>
    
    <script type="text/javascript">
        $('.clockpicker').clockpicker({
            donetext: "Ok"
        });
    </script>
    

</asp:Content>

