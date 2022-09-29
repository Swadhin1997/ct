<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmCoverMasterNew.aspx.cs" Inherits="ProjectPASS.FrmCoverMasterNew" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Master Setup(Create Cover)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="200px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 1%; left: 1%; width: 35%; margin-top: 10px; margin-left: 10px">                                 
                            <asp:Label ID="lblProduct" runat="server" Text="Product Name" Width="100%"></asp:Label>
                                </div>
                             <div style="position: absolute; top: 1%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">                                 
                               <obout:ComboBox ID="drpProduct" runat="server" Width="100%" AllowEdit="false" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_SelectedIndexChanged"></obout:ComboBox>
                            </div>

                            <div style="position: absolute; top: 1%; left: 48%; width: 58%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="lblProductName" Text=""></asp:Label>
                            </div>
                            <div style="position: absolute; top: 26%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Cover Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 26%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCoverDesc" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="90"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 26%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="Cover SI"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 26%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCoverSI" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 56%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" Text="Cover Field" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 56%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <%--<obout:ComboBox ID="drpCoverField" runat="server" Width="100%" AllowEdit="false" Height="200px"></obout:ComboBox>--%>
                                <asp:DropDownList ID="drplCoverField" runat="server" Width="100%"></asp:DropDownList>
                            </div>
                            <div style="position: absolute; top: 56%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label4" Text="CoverSI Field"></asp:Label>
                            </div>  
                            <div style="position: absolute; top: 56%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <%--<obout:ComboBox ID="drpCoverSIField" runat="server" Width="100%" AllowEdit="false" Height="200px"></obout:ComboBox>--%>
                                <asp:DropDownList ID="drplCoverSIField" runat="server" Width="100%"></asp:DropDownList>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
<%--                             <div style="position: absolute; top: 40%; left: 30%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnShow" runat="server" Text="Show Data" Width="100%" OnClick="btnShow_Click" />
                            </div>--%>
                            <div style="position: absolute; top: 40%; left: 50%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Add" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 70%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 58%; left: 50%; width: 8%; margin-top: 10px; margin-left: 10px">
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
                                    <obout:Column ID="Column1" DataField="vCoverCode" ReadOnly="true" HeaderText="Cover Code" Width="150" runat="server" />
                                    <obout:Column ID="Column2" DataField="vCoverDesc" HeaderText="Cover Name" Width="300" runat="server" />
                                    <obout:Column ID="Column3" DataField="nCoverSI" HeaderText="Cover SI" Width="100" runat="server" />
                                    <obout:Column ID="Column4" DataField="vCoverFieldInDB" HeaderText="Cover Field" Width="150" runat="server" />
                                    <obout:Column ID="Column5" DataField="vCoverSIFieldInDB" HeaderText="CoverSI Field" Width="150" runat="server" />
                                    <obout:Column ID="Column6" DataField="bIsActive" HeaderText="Is Active" Visible="true" Width="110" runat="server">                                    
                                        <TemplateSettings TemplateId="bIsActiveTemplate" EditTemplateId="EditbIsActiveTemplate" />
                                    </obout:Column>

                                    <obout:Column ID="Column7" DataField="vProductName" HeaderText="Product Name" Width="200" runat="server" ReadOnly="true" />

                                    <obout:Column ID="Column8" HeaderText="EDIT" Width="100" AllowEdit="true" AllowDelete="true" runat="server" />
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="bIsActiveTemplate">
                                        <Template>
                                            <%# Container.DataItem["bIsActive"].ToString() %>
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="EditbIsActiveTemplate" ControlID="drpbIsActive" ControlPropertyName="value">
                                        <Template>
                                            <obout:ComboBox ID="drpbIsActive" runat="server" Width="100">
                                                 <obout:ComboBoxItem Text="Yes" Value="Y"  />
                                                 <obout:ComboBoxItem Text="No" Value="N"  />
                                            </obout:ComboBox>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
