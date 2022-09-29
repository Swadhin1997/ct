<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmGPAGetPolicy_New.aspx.cs" Inherits="ProjectPASS.FrmGPAGetPolicy_New" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - View Policy Certificate(GPA)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="150px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 16%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Policy Number" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 16%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 16%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="Crn No" Width="100%"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 16%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 56%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label4" Text="Customer Name" Width="100%"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 56%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerName" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left:35%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGetPolicy_New" runat="server" Text="View Policy " Width="100%" OnClick="btnGetPolicy_New_Click" />
                            </div>
                            <div style="display:none; position: absolute; top: 40%; left:35%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGetPolicy" runat="server" Text="View Policy" Width="100%" OnClick="btnGetPolicy_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left:55%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"/>
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
