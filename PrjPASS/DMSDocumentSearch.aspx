<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DMSDocumentSearch.aspx.cs" Inherits="PrjPASS.DMSDocumentSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnSearchDocumentDMS" Text="Search Document DMS" runat="server" OnClick="btnSearchDocumentDMS_Click" />
        <asp:Button ID="btnCreateDocumentDMS" Text="Create Document DMS" runat="server" OnClick="btnCreateDocumentDMS_Click" />
        <asp:Button ID="btnDownloadDocumentDMS" Text="Download Document DMS" runat="server" OnClick="btnDownloadDocumentDMS_Click" />
    </div>
    </form>
</body>
</html>
