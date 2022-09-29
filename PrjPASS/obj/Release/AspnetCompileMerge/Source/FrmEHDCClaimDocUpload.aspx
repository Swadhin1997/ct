<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmEHDCClaimDocUpload.aspx.cs" Inherits="PrjPASS.FrmEHDCClaimDocUpload" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claim Document Upload</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">                            
                         
                            <%--<div style="position: absolute; top: 45%; left: 54%; width: 8%; margin-top: 10px; margin-left: 10px">                                
                            </div>
                            <div style="position: absolute; top: 45%; left: 65%; width: 40%; margin-top: 10px; margin-left: 10px">
                            </div>--%>
                           
                            <asp:Label ID="lblClaimNumber" runat="server" Text="Claim Number" Width="15%" style="position: absolute; top: 15%; left: 5%; width: 40%; margin-top: 10px; margin-left: 10px"></asp:Label>
                            
                            <div style="position: absolute; top: 15%; left: 14%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimNumber" runat="server" Width="75%"  />
                            </div>
                            <div style="position: absolute; top: 15%; left: 32%; width: 10%; margin-top: 10px; margin-left: 10px">
                            <obout:OboutButton ID="btnSearchClaim" runat="server" Text="Search" Width="100%" OnClick="btnSearchClaim_Click" />
                           </div>

                            <div id="divClaimLabel" style="position: absolute; top: 55%; left: 5%; width: 10%; margin-top: 10px; margin-left: 10px" runat="server" visible="false">
                                <asp:Label ID="lblClaimForm" runat="server" Text="Upload Document" Width="100%"></asp:Label>
                            </div>
                            <div id="divClaimUpload" style="position: absolute; top: 55%; left: 15%; width: 30%; margin-top: 10px; margin-left: 10px" runat="server" visible="false">
                                <asp:FileUpload ID="fulClaimForm" runat="server" Width="100%" />
                            </div>
                            <asp:HiddenField runat="server" ID="hdnCertificateNumber" Value="" />

                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnUpload" runat="server" Text="Upload Document" Width="100%" OnClick="btnUpload_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click" />
                            </div>
                           
                            
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>