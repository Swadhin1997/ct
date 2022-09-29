<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentTagging.aspx.cs" Inherits="PrjPASS.PaymentTagging" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnAllocation" Text="Payment Allocation" runat="server" OnClick="btnAllocation_Click" />
    <asp:Button ID="btn" Text="Payment Tagging" runat="server" OnClick="btn_Click" />
    </div>
    </form>
</body>
</html>
