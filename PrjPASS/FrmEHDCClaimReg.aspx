<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmEHDCClaimReg.aspx.cs" Inherits="PrjPASS.FrmEHDCClaimReg" %>

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
                ['ReadOnly', 'ReadOnly', 'TextBox', 'ComboBox', 'Action'],
                '<%=Grid1ExcelData.ClientID %>',
                '<%=Grid1ExcelDeletedIds.ClientID %>'
                );
        }
    </script>
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claims Registration</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="450px" BorderColor="#3399ff" BorderWidth="2">

                            <div style="position: absolute; top: 1%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCertificateNumber" runat="server" Text="Certificate Number <font style='color:red'>*</font>" Width="80%"></asp:Label>

                            </div>
                            <div style="position: absolute; top: 1%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="80%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <%--<div style="position: absolute; top: 1%; left: 33%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSearchCertificate" Width="50%" runat="server" Text="Search" OnClick="btnSearchCertificate_Click"></obout:OboutButton>
                            </div>--%>

                            <div style="position: absolute; top: 1%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name <font style='color:red'>*</font>" Width="100%"></asp:Label>

                            </div>
                            <div style="position: absolute; top: 1%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerName" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>




                            <div style="position: absolute; top: 7%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerType" runat="server" Text="Customer Type <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 7%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerType" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 7%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerMobile" runat="server" Text="Customer Mobile <font style='color:red'>*</font>" Width="100%"></asp:Label>
                               
                            </div>
                            <div style="position: absolute; top: 7%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerMobile" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>



                            <div style="position: absolute; top: 13%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMasterPolicyNumber" runat="server" Text="Master Policy Number <font style='color:red'>*</font>" Width="100%"></asp:Label>
                               
                            </div>
                            <div style="position: absolute; top: 13%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMasterPolicyNumber" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 13%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMasterPolicyHolder" runat="server" Text="Master Policy Holder <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 13%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMasterPolicyHolder" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>



                            <div style="position: absolute; top: 19%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line Of Business" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 19%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtLineOfBusiness" Style="text-transform: uppercase" Width="100%" runat="server" Text="Health" enable="false"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 19%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblProductName" runat="server" Text="Product Name <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 19%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtProductName" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>


                            <div style="position: absolute; top: 25%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblPolicyStartDate" runat="server" Text="Policy Start Date <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 25%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyStartDate" Width="100%" runat="server" class="input"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 25%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:Calendar ID="clnPolicyStartDate" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtPolicyStartDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>

                            <div style="position: absolute; top: 25%; left: 55%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblPolicyEndDate" runat="server" Text="Policy End Date <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 25%; left: 71%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyEndDate" class="input" Width="57%" runat="server"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 25%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:Calendar ID="clnPolicyEndDate" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtPolicyEndDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>





                            <div style="position: absolute; top: 31%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimIntimationDate" runat="server" Text="Claim Intimation Date <font style='color:red'>*</font> " Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 31%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimIntimationDate" Width="100%" runat="server" class="input"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 31%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:Calendar ID="clnClaimIntimationDate" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtClaimIntimationDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>

                            <div style="position: absolute; top: 31%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimRegistrationDate" runat="server" Text="Claim Reg Date <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 31%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimRegistrationDate" Width="100%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 31%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>




                            <div style="position: absolute; top: 37%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimIntimationBy" runat="server" Text="Claim Intimation By <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 37%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">

                                <obout:ComboBox runat="server" ID="cmbClaimIntimationBy" AllowEdit="false">
                                    <obout:ComboBoxItem Text="Applicant" Value="A" />
                                    <obout:ComboBoxItem Text="Co-Applicant" Value="C" />
                                    <obout:ComboBoxItem Text="Master Policy Holder" Value="M" />
                                </obout:ComboBox>
                            </div>
                            <div style="position: absolute; top: 37%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCallerNumber" runat="server" Text="Caller Number" Width="100%"></asp:Label>
                            </div>

                            <div style="position: absolute; top: 37%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCallerNumber" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>



                            <div style="position: absolute; top: 43%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateOfAdmission" runat="server" Text="Date of Admission <font style='color:red'>*</font> " Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 43%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfAdmission" runat="server" class="input" Width="100%" />
                            </div>
                            <div style="position: absolute; top: 43%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:Calendar ID="clnDateOfAdmission" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfAdmission" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>

                            <div style="position: absolute; top: 43%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateOfDischarge" runat="server" Text="Date Of Discharge" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 43%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfDischarge" Style="text-transform: uppercase" Width="100%" runat="server"> </obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 43%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:Calendar ID="clnDateOfDischarge" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                                 <a href="javascript:<%=clnDateOfDischarge.ClientID%>.Clear()">Clear</a> 
                            </div>



                            <div style="position: absolute; top: 49%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimAmount" runat="server" Text="Claimed Amount <font style='color:red'>*</font> " Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 49%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimAmount" Width="100%" runat="server" MaxLength="8" class="input"></obout:OboutTextBox>
                            </div>

                            <div style="position: absolute; top: 49%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblAccountNumber" runat="server" Text="Account Number <font style='color:red'>*</font> " Width="100%"></asp:Label>

                            </div>
                            <div style="position: absolute; top: 49%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtAccountNumber" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>




                            <div style="position: absolute; top: 55%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMemberID" runat="server" Text="Member ID <font style='color:red'>*</font> " Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 55%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMemberID" Width="100%" runat="server" class="input"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 55%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>

                            <div style="position: absolute; top: 55%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblMemberName" runat="server" Text="Member Name <font style='color:red'>*</font> " Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 55%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtMemberName" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>


                            <div style="position: absolute; top: 61%; left: 1%; width: 18%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimForm" runat="server" Text="Upload Document" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 61%; left: 18%; width: 40%; margin-top: 10px; margin-left: 10px">
                                <asp:FileUpload ID="fulClaimForm" runat="server" Width="100%" />
                                <br />
                                <asp:Label ID="lblfulMessage" Style="overflow: hidden; font-size: small; color: red" runat="server" Text="Allow file with extensions .zip,.pdf,.jpeg,.xls,.xlsx,.doc,.docx,.7z,.rar upto 4 MB size."></asp:Label>
                            </div>

                            <div style="position: absolute; top: 61%; left: 55%; width: 18%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Relation <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                
                            </div>
                            <div style="position: absolute; top: 61%; left: 71%; width: 40%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList runat="server" ID="drpRelation">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Self" Value="Self"></asp:ListItem>
                                    <asp:ListItem Text="Spouse" Value="Spouse"></asp:ListItem>
                                    <asp:ListItem Text="Mother" Value="Mother"></asp:ListItem>
                                    <asp:ListItem Text="Father" Value="Father"></asp:ListItem>
                                    <asp:ListItem Text="Son" Value="Son"></asp:ListItem>
                                    <asp:ListItem Text="Daughter" Value="Daughter"></asp:ListItem>
                                </obout:OboutDropDownList>
                            </div>



                            <div style="position: absolute; top: 70%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblRemark" runat="server" Text="Remark" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 70%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtRemark" Width="100%" runat="server" TextMode="MultiLine" Rows="5" class="input"></obout:OboutTextBox>
                            </div>

                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 35%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Save" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
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
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>
</asp:Content>

