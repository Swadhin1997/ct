﻿<%@ Page Title="" Language="C#"  AutoEventWireup="true"  MasterPageFile="~/PASS.Master" CodeBehind="FrmMobileRenewalUpload.aspx.cs" Inherits="ProjectPASS.FrmMobileRenewalUpload" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
   


    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Mobile Renewal Policy Upload</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">

                            <div style="position: absolute; top: 21%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Select File" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 10%; width:40%; margin-top: 10px; margin-left: 10px">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="100%" />
                            </div>
                            <div style="position: absolute; top: 21%; left: 1%; width: 90%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblError" runat="server" Width="100%" ForeColor="Black" />
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width:15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnImport" runat="server" Text="Import Data" Width="100%" OnClick="Upload" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width:15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExport" runat="server" Text="Get Template" Width="100%" OnClick="btnExport_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width:15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click" />
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" />
                            </div>

                        </asp:Panel>
                    </div>
                    <%--<div class="section colm colm12">
                        <asp:Panel ID="Panel3" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left:40%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGoBack" OnClick="btnGoBack_Click" runat="server" Text="Go Back" Font-Bold="true" Font-Size="Large" Width="100%"></obout:OboutButton>
                            </div>
                        </asp:Panel>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
