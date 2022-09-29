<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMainMenu.aspx.cs" Inherits="ProjectPASS.FrmMainMenu" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <title>Policy Admin Satellite System - PASS</title>
    <link href="css/menu_1003.css" rel="stylesheet" />
    <link href="css/Layoutstyles.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />

    <!--[if lt IE 9]>
        <script src="js/html5shiv.js"></script>
    <![endif]-->
    <script>
  function preventBack(){window.history.forward();}
  setTimeout("preventBack()", 0);
  window.onunload=function(){null};
</script>
</head>
<body>
    <div class="header">
        <div class="container">
            <div id="divLogo" style="position: absolute; top: 9px; left: 2px">
                <img src="Images/logo.jpg" class="img-polaroid" style="height: 70px; width: 230px">
            </div>
        </div>
    </div>
    <form id="form1" runat="server">

        <div style="overflow-x:hidden">
            <asp:Menu ID="rdRibMenu" Width="100%" runat="server" Orientation="Horizontal" StaticDisplayLevels="1" MaximumDynamicDisplayLevels="3" SkipLinkText="">
                <LevelSubMenuStyles>
                    <asp:SubMenuStyle BackColor="#054271" Font-Underline="False" BorderColor="White"/>
                    <asp:SubMenuStyle BackColor="#ff0000" Font-Underline="False" BorderColor="White" BorderWidth="1px"/>
                    <asp:SubMenuStyle BackColor="#054271" Font-Underline="False" BorderColor="White" BorderWidth="1px"/>
                    <asp:SubMenuStyle BackColor="#ff0000" Font-Underline="False" BorderColor="White" BorderWidth="1px"/>
                    <asp:SubMenuStyle BackColor="#054271" Font-Underline="False" BorderColor="White" BorderWidth="1px"/>
                </LevelSubMenuStyles>

            </asp:Menu>
        </div>

        <div class="content">

            <div class="main">
                <div>
                    <asp:Label ID="lbluser" runat="server" Text="Logged In User :" Width="150px" Font-Bold="true" style="font-size:13px;"></asp:Label>
                    <asp:Label ID="lblusername" runat="server" Text="" Width="300px" ForeColor="Black" style="font-size:13px;display:inline;"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblrole" runat="server" Text="User Role :" Width="150px" Font-Bold="true" style="font-size:13px;"></asp:Label>
                    <asp:Label ID="lblrolename" runat="server" Text="" Width="300px" ForeColor="Black" style="font-size:13px;display:inline;"></asp:Label>
                </div>
            </div>
            <div style="position: fixed; top: 5%; left: 90%">
                <a href="FrmLogout.aspx" class="button" style="background-color: #054271">Logout</a>
            </div>
            <div style="position: fixed; top: 5%; left: 75%">
                <a href="FrmChangePassword.aspx" class="button" style="background-color: #054271">Change Password</a>
            </div>
        </div>


        <asp:ScriptManager ID="RadScriptManager1" AsyncPostBackTimeout="360000" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>
