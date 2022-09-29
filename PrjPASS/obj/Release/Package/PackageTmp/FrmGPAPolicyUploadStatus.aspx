<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmGPAPolicyUploadStatus.aspx.cs" Inherits="ProjectPASS.FrmGPAPolicyUploadStatus" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Policy Upload Dump(GPA)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="150px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 16%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="From Date" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 16%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtFromDate" runat="server" class="input" Width="120px"/>
                                <obout:Calendar ID="Calendar1" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtFromDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>
                            <div style="position: absolute; top: 16%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="To Date" Width="100%"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 16%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtToDate" runat="server" class="input" Width="120px"/>
                                <obout:Calendar ID="Calendar2" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtToDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>
                            <div style="position: absolute; top: 56%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label4" Text="Policy Number" Width="100%"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 56%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 56%; left:48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label5" Text="Upload Id" Width="100%"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 56%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtUploadId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left:25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGetPolicy" runat="server" Text="Download Dump" Width="100%" OnClick="btnGetPolicy_Click" />
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
