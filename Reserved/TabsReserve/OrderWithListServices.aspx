﻿<%@ Page Title="Забронировать" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="~/TabsReserve/OrderWithListServices.aspx.cs" Inherits="Reserved.TabsReserve.OrderWithListServices" %>

<%--<%@ Register Assembly="TileWithCheckBox" Namespace="TileWithCheckBox" TagPrefix="cc1" %>--%>

<%@ Register Assembly="DropDownList" Namespace="DropDownList" TagPrefix="cc1" %>
<%@ Register TagPrefix="cc1" Namespace="RadioButtonsList" Assembly="RadioButtonsList" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <style>
        /* Cначала обозначаем стили для IE8 и более старых версий
        т.е. здесь мы немного облагораживаем стандартный чекбокс. */
        .checkbox {
            vertical-align: top;
            margin: 5px 0 0 0;
            width: 17px;
            height: 17px;
        }
        
        /* Это для всех браузеров, кроме совсем старых, которые не поддерживают
        селекторы с плюсом. Показываем, что label кликабелен. */
        .checkbox + label {
            cursor: pointer;
        }

        /* Далее идет оформление чекбокса в современных браузерах, а также IE9 и выше.
        Благодаря тому, что старые браузеры не поддерживают селекторы :not и :checked,
        в них все нижеследующие стили не сработают. */

        /* Прячем оригинальный чекбокс. */
        .checkbox:not(checked) {
            position: absolute;
            opacity: 0;
        }
        .checkbox:not(checked) + label {
            position: relative;  /*будем позиционировать псевдочекбокс относительно label */
            padding: 0 0 0 60px;  /*оставляем слева от label место под псевдочекбокс */
        }
       
        /* Оформление первой части чекбокса в выключенном состоянии (фон). */
        .checkbox:not(checked) + label:before {
            content: '';
            position: absolute;
            top: -4px;
            left: 0;
            width: 50px;
            height: 26px;
            border-radius: 13px;
            background: #CDD1DA;
            box-shadow: inset 0 2px 3px rgba(0,0,0,.2);
        }
        
        /* Оформление второй части чекбокса в выключенном состоянии (переключатель). */
        .checkbox:not(checked) + label:after {
            content: '';
            position: absolute;
            top: -2px;
            left: 2px;
            width: 22px;
            height: 22px;
            border-radius: 10px;
            background: #FFF;
            box-shadow: 0 2px 5px rgba(0,0,0,.3);
            transition: all .2s;  /* анимация, чтобы чекбокс переключался плавно */
        }
        
        /* Меняем фон чекбокса, когда он включен.*/
        .checkbox:checked + label:before {
            background: #9FD468;
        } 

        /* Сдвигаем переключатель чекбокса, когда он включен. */
        .checkbox:checked + label:after {
            left: 26px;
        }

        /* Показываем получение фокуса. 
        .checkbox:focus + label:before {
          box-shadow: 0 0 0 3px rgba(255,255,0,.5);
        }*/
        .outer {
            width: 90%;
            text-align: center;
            margin: 5px 5px 10px;
        }

        /*Отменить выделение текста в элементах Label*/
        .label-tile {
            color: black;
            font-size: 11px;
            -webkit-user-select: none; 
            -moz-user-select: none; 
            -ms-user-select: none; 
            -o-user-select: none; 
            user-select: none;
        }

        .label-image {
            width: 100px; height: 100px; 
            display: inline-block;
            -webkit-user-select: none; 
            -moz-user-select: none; 
            -ms-user-select: none; 
            -o-user-select: none; 
            user-select: none;
        }

        .blackAndWhite{
            filter: progid:DXImageTransform.Microsoft.BasicImage(grayscale=1);
            position:absolute;
        }

        .disable {
            position: relative;   
        }
        .disable:after {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.2);   
            content: "";
        }

        .img-overlay{
            width: 100%;
            height: 100%;
            background: rgba(169,169,169,0.3);
        }
    </style>
    
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
    
    
    <link href="../Libs/Metro-UI-CSS-master/build/css/metro.css" rel="stylesheet" />
    <script src="//code.jquery.com/jquery-1.12.0.min.js"></script>
    <script src="../Libs/Metro-UI-CSS-master/build/js/metro.js"></script>
    
    <link href="/Scripts/Wizard/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
	<link href="/Scripts/Wizard/prettify.css" rel="stylesheet"/>
    
    <script src="/Scripts/Wizard/bootstrap/js/bootstrap.min.js"></script>
	<script src="/Scripts/Wizard/jquery.bootstrap.wizard.js"></script>
	<script src="/Scripts/Wizard/prettify.js"></script>
    
    
    <div id="rootwizard">
	    <div class="navbar">
	      <div class="navbar-inner">
	        <div class="container">
	            <ul>
	  	            <li><a href="#tab1" data-toggle="tab">Выбор списка услуг</a></li>
		            <li><a href="#tab2" data-toggle="tab">Выбор даты и времени</a></li>
		            <li><a href="#tab3" data-toggle="tab">Персональные данные</a></li>
	            </ul>
	        </div>
	      </div>
	    </div>
	    <div class="tab-content">
            <%-- Выбор услуг --%>
	        <div class="tab-pane" id="tab1">
	            <cc1:DropDownList runat="server" ID="ServiceList"/>
            </div>
            <%-- ***************** --%>
             <%-- Календарь --%>
	        <div class="tab-pane" id="tab2">
	            <table>
	                <tr>
	                    <td>
	                        <div style="width: 250px">
                                <div class="calendar" 
                                     data-week-start="1" 
                                     data-start-mode="month" 
                                     id="cal-events"
                                     data-locale="ru"
                                     data-day-click="day_click"></div>
                                <div id="calendar-output2"></div>
                            </div>
	                    </td>
                        <td>
                            <cc1:RadioButtonsList runat="server" ID="radioButtonsList" Visible="True"/>
                        </td>
	                </tr>
	            </table>

	        </div>
            <%-- ***************** --%>
            <%-- Персональные данные --%>
		    <div class="tab-pane" id="tab3">
		        <%--<asp:Label ID="Label2" runat="server" Text="Список выбранных вами услуг"></asp:Label>
			    <asp:Label runat="server" Text="Фамилия"></asp:Label>
                <asp:TextBox runat="server" ID="lastname"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="Имя"></asp:Label>
                <asp:TextBox runat="server" ID="firstname"></asp:TextBox>--%>
	        </div>
            <%-- ***************** --%>
		    <ul class="pager wizard">
			    <li class="previous first" style="display:none;"><a href="#">Первая</a></li>
			    <li class="previous"><a href="#">Назад</a></li>
			    <li class="next last" style="display:none;"><a href="#">Последняя</a></li>
		  	    <li class="next"><a href="#">Далее</a></li>
		    </ul>
	    </div>	
    </div>

   <%--Скрипт для календаря "Библиотека Metro"--%>
    <script>
        $(function () {
            var myDate = new Date();
            var dayOfMonth = myDate.getDate();
            myDate.setDate(dayOfMonth - 1);
            var cal = $("#cal-events").calendar({
                multiSelect: false,
                format: 'yyyy-mm-dd',
                minDate: myDate
            });
        });

        function day_click(short, full) {
            $.ajax({    //Передаём введённые данные на сервер и получаем ответ
                type: "POST",
                url: window.location.href + '/GetTime', //url: адрес текущей страницы / имя статического метода на стороне сервера
                data: "{'date': '" + short + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true, cache: false,
                success: function (data, textStatus, XHR) {
                    var jObject = JSON.parse(data.d.toString());
                    var elements = jObject["array"];
                    for (var i = 0; i < elements.length; i++) {
                        var id = elements[i].interval;
                        var flag = elements[i].flag;
                        document.getElementById('radio' + id).disabled = !flag;
                    }
                }
            });
            return false;
        }
    </script>

    <%--Скрипт для multiple формы "Библиотека Bootstrap"--%>
    <script>
        $(document).ready(function () {
            $('#rootwizard').bootstrapWizard();
        });
    </script>

    <%--Скрипт для раскрывающегося списка "Библиотека Metro"--%>
    <script>
        $(function () {
            $("#accordion").accordion();
        });
    </script>
    
    <%--Скрипт для анализа активных плиток--%>
    <script type="text/javascript">
        var checked = [];
        //debugger;
        function setCookie(name, value) {
            document.cookie = name + "=" + value;
        }

        function clickService(box) {
            //debugger;
            if (box.checked) {
                checked.push(box.value);
            } else {
                checked.splice(checked.indexOf(box.value), 1);
            }
            setCookie("checked_services", checked);
        };

        //document.getElementById("btnStatus").onclick = function() {
        //    //var checked = [];
        //    var count = 5; // Решить проблему с тем что число должно соответствовать количеству "плиток"
        //    var i = 0;
        //    while (++i <= count) {
        //        //debugger;
        //        if (document.getElementById("checkbox" + i).checked) {
        //            checked.push(document.getElementById("checkbox" + i).value);
        //        }
        //    }
        //    setCookie("checked_services", checked);
        //    alert("Выбраны: " + checked);
        //};
    </script>

</asp:Content>

