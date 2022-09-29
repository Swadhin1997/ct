<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmResetPassword.aspx.cs" Inherits="ProjectPASS.FrmResetPassword" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        #table1 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 2px;
        }

        .tdbkg {
            background-color: rgba(158, 158, 158, 0.32);
            font-size: 11px;
            color:white;
            text-align:right;
        }
    </style>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Reset Password</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" BorderColor="#3399ff" BorderWidth="1">

                            <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td class="tdbkg">Login Id
                                    </td>
                                    <td>
                                        <obout:OboutTextBox ID="txtLoginId" Style="text-transform: uppercase"  runat="server" MaxLength="10"></obout:OboutTextBox>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" Display="Dynamic" ControlToValidate="txtLoginId" ErrorMessage="Please Enter LoginId" ValidationGroup="Blnk_chk" />
                                    </td>
                                   
                                </tr>
                              <tr>
                                    <td class="tdbkg">Registered Email Id
                                    </td>
                                    <td>
                                        <obout:OboutTextBox ID="txtEmailId" runat="server" MaxLength="100"></obout:OboutTextBox>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic" ControlToValidate="txtEmailId" ErrorMessage="Please Enter EmailId" ValidationGroup="Blnk_chk" />
                                    </td>
                              </tr>
                                <tr>
                                    <td class="tdbkg">New Password</td>
                                    <td>
                                        <obout:OboutTextBox ID="txtNewPassword" TextMode="Password" MaxLength="15" runat="server"></obout:OboutTextBox>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="txtNewPassword" ErrorMessage="Please Enter New Password" ValidationGroup="Blnk_chk" />
                                        

                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbkg">Confirm New Password</td>
                                    <td>
                                        <obout:OboutTextBox ID="txtConfirmNewPassword" TextMode="Password" MaxLength="15" runat="server"></obout:OboutTextBox>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="Dynamic" ControlToValidate="txtConfirmNewPassword" ErrorMessage="Please Enter Confirm New Password" ValidationGroup="Blnk_chk" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbkg">Security Question
                                        &nbsp;&nbsp; <asp:Label ID="lblQuestion" runat="server" Text="1+1 = "></asp:Label>
                                    </td>
                                    <td>
                                        
                                        <obout:OboutTextBox ID="txtAnswer" MaxLength="4" runat="server" TextMode="Number"></obout:OboutTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CompareValidator runat="server" id="cmpPassword" controltovalidate="txtNewPassword" controltocompare="txtConfirmNewPassword" operator="Equal" type="String" errormessage="New Password must match with confirm new password" />

                                        <asp:RegularExpressionValidator ID="Regex4" runat="server" Display="Dynamic" ControlToValidate="txtNewPassword"
                                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}"
                                        ErrorMessage="Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character" ForeColor="Red" ValidationGroup="Blnk_chk" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="1">
                            <div style="position: absolute; top: 40%; left: 35%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Update Password" Width="100%" OnClick="btnSave_Click" ValidationGroup="Blnk_chk"  />
                            </div>
                            <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblstatus" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                  
                </div>
            </div>
        </div>
    </div>
</asp:Content>
