<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmGPAUploadConfiguration.aspx.cs" Inherits="ProjectPASS.FrmGPAUploadConfiguration" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Configuration Management(Create Upload Template)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 21%; left: 1%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Configuration Id" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 12%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtConfigurationId" Style="text-transform: uppercase" Width="100%" runat="server" Enabled="false"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 21%; left: 48%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label2" runat="server" Text="Configuration Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 60%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtConfigurationName" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Add" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Save" OnClick="btnUpdate_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="OboutButton1" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblstatus" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:GridView ID="gvSubDetails" runat="server" DataKeyNames="vTemplateId"
                            AutoGenerateColumns="false" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="Template Id" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="txtvTemplateId" runat="server" Text='<%#Eval("vTemplateId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SourceColumn Index" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvSourceColumnIndex" Text='<%#Eval("vSourceColumnIndex")%>' runat="server">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SourceColumn Name" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtvSourceColumnName" Text='<%#Eval("vSourceColumnName")%>' runat="server">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DestinationColumn Index" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="txtvDestinationColumnIndex" Text='<%#Eval("vDestinationColumnIndex")%>' runat="server">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DestinationColumn Name" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="txtvDestinationColumnName" Text='<%#Eval("vDestinationColumnName")%>' runat="server">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DestinationColumn Type" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="txtvDestinationType" Text='<%#Eval("vDestinationType")%>' runat="server">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DestinationColumn Length" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="txtvDestinationLenght" Text='<%#Eval("vDestinationLenght")%>' runat="server">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exclude From Policy Upload" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpbExcludeForPolicyUpload" runat="server">
                                            <Items>
                                                <asp:ListItem Text="Yes" Value="Y" />
                                                <asp:ListItem Text="No" Value="N" Selected="True" />
                                            </Items>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exclude From Claims Upload" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpbExcludeForClaimsUpload" runat="server">
                                            <Items>
                                                <asp:ListItem Text="Yes" Value="Y" />
                                                <asp:ListItem Text="No" Value="N" Selected="True" />
                                            </Items>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exclude From Endorsement" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpbExcludeForEndorseUpload" runat="server">
                                            <Items>
                                                <asp:ListItem Text="Yes" Value="Y" />
                                                <asp:ListItem Text="No" Value="N" Selected="True" />
                                            </Items>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mandatory For Policy" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpbMandatoryForPolicy" runat="server">
                                            <Items>
                                                <asp:ListItem Text="Yes" Value="Y" />
                                                <asp:ListItem Text="No" Value="N" Selected="True" />
                                            </Items>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mandatory For Claim" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpbMandatoryForClaims" runat="server">
                                            <Items>
                                                <asp:ListItem Text="Yes" Value="Y" />
                                                <asp:ListItem Text="No" Value="N" Selected="True" />
                                            </Items>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mandatory For Endorsement" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpbMandatoryForEndorse" runat="server">
                                            <Items>
                                                <asp:ListItem Text="Yes" Value="Y" />
                                                <asp:ListItem Text="No" Value="N" Selected="True" />
                                            </Items>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
