<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmPlanMasterNew.aspx.cs" Inherits="ProjectPASS.FrmPlanMasterNew" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <script type="text/javascript">
        window.onload = function () {
            gvSubDetails.convertToExcel(
                ['ReadOnly', 'ReadOnly', 'TextBox', 'ComboBox','Action'],
                '<%=Grid1ExcelData.ClientID %>',
                '<%=Grid1ExcelDeletedIds.ClientID %>'
                );
        }
    </script>
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Master Setup(Create Plan)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="250px" BorderColor="#3399ff" BorderWidth="2">
                            
                            <div style="position: absolute; top: 1%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblProdCode" runat="server" Text="Product Code" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 1%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:ComboBox ID="cmbProduct" runat="server" Width="40%" OnSelectedIndexChanged="cmbProduct_SelectedIndexChanged" AutoPostBack="true" AllowEdit="false"></obout:ComboBox>
                            </div>
                             <div style="position: absolute; top: 1%; left: 30%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblProdName" runat="server" Text="" Width="100%"></asp:Label>
                            </div>

                             <div style="position: absolute; top: 1%; left: 55%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblProposal" runat="server" Text="Proposal Type" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 1%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                  <%--<obout:OboutDropDownList runat="server" ID="ddl1">            
                                                <asp:ListItem Text="--Select--" Value="Select" />                                    
                                                <asp:ListItem Text="Credit Linked" Value="CL" />
                                                <asp:ListItem Text="Non Credit Linked" Value="NCL" />                                               
                                            </obout:OboutDropDownList>--%>
                                <obout:ComboBox runat="server" ID="cmb1" AllowEdit="false">                                    
                                    <obout:ComboBoxItem Text="Credit Linked" Value="CL" />
                                    <obout:ComboBoxItem Text="Non Credit Linked" Value="NCL"/>
                                </obout:ComboBox>
                            </div>

                            <div style="position: absolute; top: 11%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Plan Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 11%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPlanDesc" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 11%; left: 55%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label2" runat="server" Text="Plan SI" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 11%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPlanSI" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 21%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label4" runat="server" Text="Stamp Duty" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtStampDuty" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 21%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label5" runat="server" Text="Section A Premium" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtSecAPrem" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 31%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label6" runat="server" Text="Extension To Section A Premium" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 31%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtExtToSecAPrem" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 31%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label7" runat="server" Text="Section B Premium" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 31%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtSecBPrem" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                            </div>

                            <div style="position: absolute; top: 41%; left: 55%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblSIBasis" runat="server" Text="Sum Insured Basis" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 41%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                              <%--  <obout:OboutDropDownList runat="server" ID="ddlSIBasis">        
                                    <asp:ListItem Text="--Select--" Value="Select" />                                                 
                                                <asp:ListItem Text="Fixed" Value="Fixed" />
                                                <asp:ListItem Text="Reducing" Value="Reducing" />                                               
                                            </obout:OboutDropDownList>--%>
                                <obout:ComboBox runat="server" ID="cmbSIBasis" AllowEdit="false">
                                    <obout:ComboBoxItem Text="Fixed" Value="Fixed"/>
                                    <obout:ComboBoxItem Text="Reducing" Value="Reducing"/>
                                </obout:ComboBox>
                            </div>

                             <div style="position: absolute; top: 41%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblFinancer" runat="server" Text="Financer Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 41%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtFinancer" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>

                               <div style="position: absolute; top: 51%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMasterPolicy" runat="server" Text="Master Policy No." Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 51%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMasterPolicy" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>

                              <div style="position: absolute; top: 51%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMasterPolicyDt" runat="server" Text="Master Policy Issue Date" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 51%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtFromDate" runat="server" class="input" Width="120px" ReadOnly="true"/>
                                <obout:Calendar ID="Calendar1" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtFromDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>

                             <div style="position: absolute; top: 61%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMsterPolicyLoc" runat="server" Text="Master Policy Location" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 61%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMsterPolicyLoc" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>


                             <div style="position: absolute; top: 61%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblLoanType" runat="server" Text="Loan Type" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 61%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                               <obout:OboutTextBox ID="txtLoanType" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>

                            <div style="position: absolute; top: 71%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMinTenure" runat="server" Text="Min Policy Tenure" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 71%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMinTenure" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="1" OnTextChanged="txtMinTenure_TextChanged" AutoPostBack="true" ></obout:OboutTextBox>
                            </div>

                             <div style="position: absolute; top: 71%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMaxTenure" runat="server" Text="Max Policy Tenure" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 71%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                               <obout:OboutTextBox ID="txtMaxTenure" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="1" OnTextChanged="txtMaxTenure_TextChanged" AutoPostBack="true"></obout:OboutTextBox>
                            </div>

                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 35%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Add" Width="100%"  OnClientClick="return gvSubDetails.saveExcelChanges()"  OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel3" runat="server" Height="400px">
                            <obout:Grid runat="server" ID="gvSubDetails" CallbackMode="false" Serialize="false" AllowAddingRecords="true" AllowMultiRecordAdding="true"
                                AllowRecordSelection="false" AutoGenerateColumns="false" AllowDataAccessOnServer="true"
                                ShowFooter="true" AllowSorting="false" PageSize="-1" ShowLoadingMessage="false" AllowPageSizeSelection="false" AllowPaging="false"
                                FolderStyle="Grid/styles/grand_gray">
                                <PagingSettings ShowRecordsCount="false" />
                               <TemplateSettings NewRecord_TemplateId="ButtonsTemplate" MultiRecordSaveCancel_TemplateId="ButtonsTemplate2" />
                                <Columns>
                                    <obout:Column ID="Column1" DataField="vCoverCode" ReadOnly="true" HeaderText="Cover Code" Width="150" runat="server">
                                        <TemplateSettings TemplateId="ReadOnlyTemplate" />
                                    </obout:Column>
                                    <obout:Column ID="Column2" DataField="vCoverDesc" HeaderText="Cover Name" Width="400" runat="server">
                                        <TemplateSettings TemplateId="ReadOnlyTemplate" />
                                    </obout:Column>
                                    <obout:Column ID="Column3" DataField="nCoverSI" HeaderText="Cover SI" Width="150" runat="server">
                                        <TemplateSettings TemplateId="TextBoxEditTemplate" />
                                    </obout:Column>
                                    <obout:Column ID="Column5" DataField="vCoverSIText" HeaderText="Cover SI Text" Width="150" runat="server">
                                        <TemplateSettings TemplateId="TextBoxEditTemplate" />
                                    </obout:Column>
                                    <obout:Column ID="Column4" DataField="bIsActive" HeaderText="Is Active" Visible="true" Width="200" runat="server">
                                        <TemplateSettings TemplateId="ComboBoxEditTemplate" />
                                    </obout:Column>
                                     <obout:Column ID="Column6" DataField="vProductName" ReadOnly="true" HeaderText="Product Name" Width="150" runat="server">
                                        <TemplateSettings TemplateId="ReadOnlyTemplate" />
                                    </obout:Column>
                                </Columns>
                                <Templates>
                                    <obout:GridTemplate runat="server" ID="ReadOnlyTemplate">
                                        <Template>
                                            <input type="text" name="TextBox1" class="excel-textbox" value='<%# Container.Value %>' readonly="readonly" />
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="TextBoxEditTemplate">
                                        <Template>
                                            <input type="text" name="TextBox1" class="excel-textbox" value='<%# Container.Value %>' readonly="readonly"
                                                onfocus="gvSubDetails.editWithTextBox(this)" />
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="ComboBoxEditTemplate">
                                        <Template>
                                            <input type="text" name="TextBox1" class="excel-textbox" value='<%# Container.Value %>' readonly="readonly"
                                                onfocus="gvSubDetails.editWithComboBox(this)" />
                                        </Template>
                                    </obout:GridTemplate>
                                    <obout:GridTemplate runat="server" ID="ButtonsTemplate">
                                        <Template>
                                            <obout:OboutButton ID="OboutButton2" runat="server" Text="Save Changes" OnClientClick="return gvSubDetails.saveExcelChanges()" OnClick="btnSave_Click" />
                                            <obout:OboutButton ID="OboutButton3" runat="server" Text="Cancel Changes" />
                                        </Template>
                                    </obout:GridTemplate>
                                     <obout:GridTemplate runat="server" ID="ButtonsTemplate2">
                                        <Template>
                                        </Template>
                                    </obout:GridTemplate>
                                </Templates>
                            </obout:Grid>
                            <div style="display: none;" id="FieldEditorsContainer">
                                <div id="TextBoxEditorContainer" style="width: 100%">
                                    <obout:OboutTextBox runat="server" ID="TextBoxEditor" FolderStyle="~/Interface/styles/premiere_blue/OboutTextBox" Width="100%" AutoCompleteType="None">
                                        <ClientSideEvents OnKeyDown="navigateThroughCells" />
                                    </obout:OboutTextBox>
                                </div>
                                <div id="ComboBoxEditorContainer" style="width: 100%">
                                    <obout:ComboBox ID="ComboBoxEditor" FolderStyle="~/ComboBox/styles/premiere_blue"
                                        Width="50%" Height="150" MenuWidth="175" runat="server" OpenOnFocus="false">  
                                        <obout:ComboBoxItem Text="Yes" Value="Y" />
                                        <obout:ComboBoxItem Text="No" Value="N" />
                                    </obout:ComboBox>
                                </div>
                                 <div id="CheckBoxEditorContainer" style="width: 100%">
                                    <obout:OboutCheckBox runat="server" ID="CheckBoxEditor" FolderStyle="~/Interface/styles/premiere_blue/OboutCheckBox">
                                        <ClientSideEvents OnBlur="persistFieldValue" />
                                    </obout:OboutCheckBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>
</asp:Content>
