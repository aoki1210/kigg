<%@ Page Language="C#" AutoEventWireup="true"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <title>Access Denied</title>
    <style type="text/css">
        body
        {
            background:#eb4c07 url(http://farm4.static.flickr.com/3203/3079286331_191d6dce7c_o.png) no-repeat fixed 100% 100%;
        }
        .backSoon
        {
            margin-top:100px;
            margin-left:20px;
            color:#fff;
            font-family:Trebuchet MS, Tahoma, Arial, sans-serif;
            width:300px;
        }
    </style>
</head>
<body>
    <div class="backSoon">
        <h2>Access Denied!</h2>
        Your Ip address <strong><%=Request.UserHostAddress%></strong> is currently blocked due to malicious usage of this site. If you think we have incorrectly
        blocked your Ip address, we recommend to contact us as soon as possible.
        <br/>
        <br/>
        DotNetShoutout Team
        <br/>
        support@dotnetshoutout.com
    </div>
    <script type="text/javascript" src="http://www.google-analytics.com/ga.js"></script>
    <script type="text/javascript">
    //<![CDATA[
        _gat._getTracker('UA-5894416-2')._trackPageview();
    //]]>
    </script>
</body>
</html>