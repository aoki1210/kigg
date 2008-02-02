<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Json.aspx.cs" Inherits="Kigg.JsonView" %>
<%
    Response.Clear();
    Response.ContentType = "application/json";
    Response.Write(ViewData.ToJson());
%>