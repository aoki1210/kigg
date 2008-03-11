<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Json.aspx.cs" Inherits="Kigg.JsonView" %>
<%
    var response = Context.Response;
    response.Clear();
    response.ContentType = "application/json";
    response.Write(ViewData.ToJson());
%>