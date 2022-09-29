<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmUserRightsToRoleMapping.aspx.cs" Inherits="ProjectPASS.FrmUserRightsToRoleMapping" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - User Mapping(Role To Rights)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 21%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Role Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:ComboBox  ID="drpUserRoleList"  Width="100%" runat="server"></obout:ComboBox>
                            </div>
                            <div style="position: absolute; top: 21%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" Text="Select Module" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:ComboBox ID="drpModule"  AutoPostBack="true" OnSelectedIndexChanged="drpModule_SelectedIndexChanged1" Width="100%" runat="server"></obout:ComboBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel3" runat="server" Height="400px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 5%; left:20%; width:15%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="Opt.Un-Mapped"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 15%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <asp:ListBox CssClass ="ListBoxClass" ID="lstMenuMaster" runat="server" Width="100%" Height="300px" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div style="position: absolute; top: 21%; left: 50%; width: 5%; margin-top: 10px; margin-left: 10px">
                                <obout:oboutButton type="submit" ID="btn1" OnClick="btn1_Click" runat="server" Text=">" Width="100%" />
                            </div>
                            <div style="position: absolute; top: 31%; left: 50%; width: 5%; margin-top: 10px; margin-left: 10px">
                                <obout:oboutButton type="submit" ID="btn2" OnClick="btn2_Click" runat="server" Text=">>" Width="100%"/>
                            </div>
                            <div style="position: absolute; top: 41%; left: 50%; width: 5%; margin-top: 10px; margin-left: 10px">
                                <obout:oboutButton type="submit" ID="btn3" OnClick="btn3_Click" runat="server" Text="<" Width="100%"/>
                            </div>
                            <div style="position: absolute; top: 51%; left: 50%; width: 5%; margin-top: 10px; margin-left: 10px">
                                <obout:oboutButton type="submit" ID="btn4" OnClick="btn4_Click" runat="server" Text="<<" Width="100%"/>
                            </div>
                            <div style="position: absolute; top: 5%; left: 70%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label4" Text="Opt.Mapped"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 15%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <asp:ListBox CssClass="ListBoxClass" ID="lstMenuRole" runat="server" Width="100%" Height="300px" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                        </asp:Panel>
                    </div>
                     <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Add" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnReset" Width="100%" runat="server" Text="Reset" OnClick="btnReset_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>