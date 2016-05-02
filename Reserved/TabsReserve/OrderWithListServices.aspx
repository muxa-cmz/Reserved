<%@ Page Title="Забронировать" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="~/TabsReserve/OrderWithListServices.aspx.cs" Inherits="Reserved.TabsReserve.OrderWithListServices" %>

<%@ Register Assembly="TileWithCheckBox" Namespace="TileWithCheckBox" TagPrefix="cc1" %>

<%@ Register Assembly="DropDownList" Namespace="DropDownList" TagPrefix="cc1" %>




<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <style>
        /* Cначала обозначаем стили для IE8 и более старых версий
        т.е. здесь мы немного облагораживаем стандартный чекбокс. */
        .checkbox {
            vertical-align: top;
            margin: 10px 0 0 0;
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
        .label {
            -webkit-user-select: none; 
            -moz-user-select: none; 
            -ms-user-select: none; 
            -o-user-select: none; 
            user-select: none;
        }

        .label_image {
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
    
    
    <link href="../Libs/Metro-UI-CSS-master/build/css/metro.css" rel="stylesheet" />
    <script src="//code.jquery.com/jquery-1.12.0.min.js"></script>
    <script src="../Libs/Metro-UI-CSS-master/build/js/metro.js"></script>
      

    <%--<table>
        <tr>
            <td style="align-content: center">
                <cc1:TileWithCheckBox ID="TileWithCheckBox1" 
                          Title="Бесконтактная" 
                          Image="../Image/38.jpg"
                          CheckBoxName="checkbox1"
                          ValueInput="ch1" runat="server" Enabled="False"/>
            </td>
            <td style="align-content: center">
                <cc1:TileWithCheckBox ID="TileWithCheckBox3" 
                            Title="Бесконтактная" 
                            Image="../Image/39.jpg"
                            CheckBoxName="checkbox2"
                            ValueInput="ch2" runat="server"/>
            </td>
        </tr>
    </table>--%>

    <%--<div class="accordion" id="accordion">        
        <div class="frame">
            <div class="heading">Кузов</div>
            <div class="content">
                <table>
                    <tr>
                        <td class="disable">
                            <div class="outer">
                                <label for="checkbox1" class="label">Бесконтактная мойка</label>
                                <br style="margin: 0 0 5px 0"/>
                                <label for="checkbox1" class="label_image" style="background-image: url(Image/38.jpg);"></label>
                                <br style="margin: 0 0 7px 0"/>
                                <input type="checkbox" class="checkbox" id="checkbox1" name="checkbox1" value="cb1"/>
                                <label for="checkbox1"></label>
                            </div>
                        </td>
                        <td>
                            <div class="outer">
                                <label for="checkbox2" class="label">Полировка воском</label>
                                <br style="margin: 0 0 5px 0"/>
                                <label for="checkbox2" class="label_image" style="background-image: url(Image/39.jpg);"></label>
                                <br style="margin: 0 0 7px 0"/>
                                <input type="checkbox" class="checkbox" id="checkbox2" name="checkbox2" value="cb2"/>
                                <label for="checkbox2"></label>
                            </div>
                        </td>
                        <td>
                            <div class="outer">
                                <label for="checkbox5" class="label">Бесконтактная мойка</label>
                                <br style="margin: 0 0 5px 0"/>
                                <label for="checkbox5" class="label_image" style="background-image: url(Image/38.jpg);"></label>
                                <br style="margin: 0 0 7px 0"/>
                                <input type="checkbox" class="checkbox" id="checkbox5" name="checkbox5" value="cb5"/>
                                <label for="checkbox5"></label>
                            </div>
                        </td>
                        <td>
                            <div class="outer">
                                <label for="checkbox6" class="label">Бесконтактная мойка</label>
                                <br style="margin: 0 0 5px 0"/>
                                <label for="checkbox6" class="label_image" style="background-image: url(Image/38.jpg);"></label>
                                <br style="margin: 0 0 7px 0"/>
                                <input type="checkbox" class="checkbox" id="checkbox6" name="checkbox6" value="cb6"/>
                                <label for="checkbox6"></label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="outer">
                                <label for="checkbox3" class="label">Полировка воском</label>
                                <br style="margin: 0 0 5px 0"/>
                                <label for="checkbox3" class="label_image" style="background-image: url(Image/40.jpg);"></label>
                                <br style="margin: 0 0 7px 0"/>
                                <input type="checkbox" class="checkbox" id="checkbox3" name="checkbox3" value="cb3"/>
                                <label for="checkbox3"></label>
                            </div>
                        </td>
                        <td>
                            <div class="outer">
                                <label for="checkbox4" class="label">Полировка воском</label>
                                <br style="margin: 0 0 5px 0"/>
                                <label for="checkbox4" class="label_image" style="background-image: url(Image/41.jpg);"></label>
                                <br style="margin: 0 0 7px 0"/>
                                <input type="checkbox" class="checkbox" id="checkbox4" name="checkbox4" value="cb4"/>
                                <label for="checkbox4"></label>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        
        <div class="frame">
            <div class="heading">Frame heading</div>
            <div class="content">Frame content</div>
        </div>
    </div>--%>
    
    <input type="button" id="btnStatus" value="Забронировать"/>
    <script>
        $(function () {
            $("#accordion").accordion();
            $("#accordionPanel").accordion();
        });
    </script>
    <script src="../Scripts/init.js"></script>
    <script type="text/javascript">
        document.getElementById("btnStatus").onclick = function() {
            var checked = [];
            var count = 5; // Решить проблему с тем что число должно соответствовать количеству "плиток"
            var i = 0;
            while (++i <= count) {
                //debugger;
                if (document.getElementById("checkbox" + i).checked) {
                    checked.push(document.getElementById("checkbox" + i).value);
                }
            }
            setCookie("checked_services", checked);
            alert("Выбраны: " + checked);
        };
    </script>

</asp:Content>

