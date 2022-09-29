<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmUserToRoleMapping.aspx.cs" Inherits="ProjectPASS.FrmUserToRoleMapping" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .drp {
            width: 150px;
            height: 19px;
            border: 1px solid #5B768A;
            border-radius: 3px;
            background-color: #EAEAEA;
            background: linear-gradient(EAEAEA, white);
        }

            .drp:hover {
                cursor: pointer;
            }
    </style>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - User Mapping(User To Role)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 21%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="User Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <asp:DropDownList ID="drpUserList" runat="server" Width="100%" CssClass="drp"></asp:DropDownList>
                            </div>
                            <div style="position: absolute; top: 21%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="Role Name"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:ComboBox ID="drpUserRoleList" Width="100%" runat="server"></obout:ComboBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Add" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 55%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblstatus" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel3" runat="server" Height="400px">
                            <obout:Grid runat="server" ID="gvSubDetails" CallbackMode="true" Serialize="true"
                                EnableTypeValidation="false" AutoGenerateColumns="false" OnUpdateCommand="gvSubDetails_RowUpdating"
                                FolderStyle="Grid/styles/grand_gray">
                                <ScrollingSettings ScrollWidth="100%" NumberOfFixedColumns="3" FixedColumnsPosition="Left" />
                                <Columns>
                                    <obout:Column ID="Column1" DataField="vUserLoginId" ReadOnly="true" HeaderText="User Id" Width="150" runat="server" />
                                    <obout:Column ID="Column2" DataField="vUserLoginDesc" HeaderText="User Name" Width="300" runat="server" />
                                    <obout:Column ID="Column3" DataField="vRoleCode" ReadOnly="true" HeaderText="Role Code" Visible="false" Width="150" runat="server" />
                                    <obout:Column ID="Column4" DataField="vRoleDesc" HeaderText="Role Name" Width="300" runat="server" />
                                    <obout:Column ID="Column5" HeaderText="EDIT" Width="150" AllowEdit="true" AllowDelete="true" runat="server" />
                                </Columns>
                            </obout:Grid>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
