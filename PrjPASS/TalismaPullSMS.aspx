<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TalismaPullSMS.aspx.cs" Inherits="PrjPASS.TalismaPullSMS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function Validate()
        {
            document.getElementById('<%= lblstatus.ClientID %>').value = '-';
            var txtFromMobileNumber = document.getElementById('<%= txtFromMobileNumber.ClientID %>').value;
            var txtMessage = document.getElementById('<%= txtMessage.ClientID %>').value;
            
            var val = txtFromMobileNumber.trim();

            if (txtFromMobileNumber.trim().length <= 0) {
                alert('please enter 10 digit mobile number');
                return false;
            }
            else if (!/^\d{10}$/.test(val)) {
                alert("Invalid number; must be ten digits")
                return false;
            }
            else if (txtFromMobileNumber.trim().length != 10) {
                alert('please enter 10 digit mobile number');
                return false;
            }
            else if (txtMessage.trim().length <= 0) {
                alert('please enter Message');
                return false;
            }
            else {
                return true;
            }
        }
    </script>



    <style>
        table {
            border-collapse: collapse;
        }

        table, th, td {
            border: 1px solid black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <img src="Images/Logo.jpg" />
        <hr />
        <div style="width: 450px; margin: 0 auto;">
            <div style="text-align: center">
                <h2>
                    <u>Talisma Pull SMS</u></h2>
            </div>
            <hr />
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <b>From (Mobile No.): 
                        </b></td>
                    <td>
                        <asp:TextBox ID="txtFromMobileNumber" runat="server" Width="300px" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <b>Message: </b></td>
                    <td>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="300px" Height="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <b>To:
                        </b></td>
                    <td>1234
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Button ID="btnCreateInteraction" class="btn btn-primary" runat="server" Text="Create Talisma Interaction" OnClick="btnCreateInteraction_Click" OnClientClick="return Validate();" />
                    </td>
                </tr>

            </table>
        </div>
        Status: <asp:Label ID="lblstatus" runat="server" Text="-"></asp:Label>
    </form>
</body>
</html>
