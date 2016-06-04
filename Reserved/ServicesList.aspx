<%@ Page Title="Забронировать" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ServicesList.aspx.cs" Inherits="Reserved.ServicesList" %>

<%@ Register TagPrefix="cc1" Namespace="ServicesList" Assembly="ServicesList" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    
    
    <link href="/Libs/Metro-UI-CSS-master/build/css/metro.css" rel="stylesheet" />
    <script src="//code.jquery.com/jquery-1.12.0.min.js"></script>
    <script src="/Libs/Metro-UI-CSS-master/build/js/metro.js"></script>
    
    <link href="/Scripts/Wizard/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
	<link href="/Scripts/Wizard/prettify.css" rel="stylesheet" />
    
    <script src="/Scripts/Wizard/bootstrap/js/bootstrap.min.js"></script>
	<script src="/Scripts/Wizard/jquery.bootstrap.wizard.js"></script>
	<script src="/Scripts/Wizard/prettify.js"></script>
    
    
    <style>
        .label-deskription {
            font-family:Lobster, cursive;
            font-size:10px;
            margin-bottom: 1px;
            -webkit-user-select: none; 
            -moz-user-select: none; 
            -ms-user-select: none; 
            -o-user-select: none; 
            user-select: none;
        }
        .boxShadow {
            width: 90%;
            height: 90%;
            margin: 1.5em auto 1em;
            border: 1px solid #ccc;
            box-shadow: 10px -5px 3px 3px rgba(0, 0, 0, .2);
        }
    </style>
    

    <cc1:ServicesList runat="server" ID="servicesList"/>
   

    <%--Скрипт для раскрывающегося списка "Библиотека Metro"--%>
    <script>
        $(function () {
            $("#accordion").accordion();
        });
    </script>

</asp:Content>

