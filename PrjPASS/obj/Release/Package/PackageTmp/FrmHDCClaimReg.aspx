<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCClaimReg.aspx.cs" Inherits="PrjPASS.FrmHDCClaimReg" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">



    <script type="text/javascript">

        $(function () {
            ApplyDatePicker();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
        });

        function ApplyDatePicker() {

            $("[id$=txtClaimIntimationDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerClaimIntimation").click(function () {
                $("[id$=txtClaimIntimationDate]").datepicker("show");
            });


            $("#datepickerAdmissionDate").click(function () {
                $("[id$=txtDateOfAdmission]").datepicker("show");
            });

            $("[id$=txtDateOfAdmission]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });


            $("#datepickerDischargeDate").click(function () {
                $("[id$=txtDateOfDischarge]").datepicker("show");
            });

            $("[id$=txtDateOfDischarge]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });






            $("[id$=txtToDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

          

        }

        function ClearDate() {
            document.getElementById("<%=txtDateOfDischarge.ClientID%>").value = '';
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
                        <asp:Panel ID="Panel1" runat="server" Height="550px" BorderColor="#3399ff" BorderWidth="2">



                            <div id="tab1" role="tabpanel" class="tab-pane">
                                <div style="position: absolute; top: 12%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblCertificateNumber" runat="server" Text="Certificate Number" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 12%; left: 11%; width: 25%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="40%" runat="server" MaxLength="30"></obout:OboutTextBox>
                                </div>

                                <div style="position: absolute; top: 12%; left: 22%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblLoanAccNo" runat="server" Text="     Or           Loan Acc Number" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 12%; left: 33%; width: 25%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtLoanAccNo" Style="text-transform: uppercase" Width="40%" runat="server" MaxLength="30"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 12%; left: 45%; width: 20%; margin-top: 10px; margin-left: 10px">

                                    <obout:OboutButton ID="btnSearchCertificate" Width="25%" runat="server" Text="Search" OnClick="btnSearchCertificate_Click"></obout:OboutButton>

                                </div>

                                <div style="position: absolute; top: 12%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblCustomerName1" runat="server" Text="Customer Name" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 12%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtCustomerName" Style="text-transform: uppercase" Width="80%" runat="server" MaxLength="30" ReadOnly="true"></obout:OboutTextBox>
                                </div>

                                <div style="position: absolute; top: 17%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblCustomerType" runat="server" Text="Customer Type<font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 17%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtCustomerType" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30" ReadOnly="true"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 17%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblCustomerMobile" runat="server" Text="Customer Mobile Number<font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 17%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtCustomerMobile" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 22%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblMasterPolicyNumber" runat="server" Text="Master Policy Number <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 22%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtMasterPolicyNumber" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 22%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblMasterPolicyHolder" runat="server" Text="Master Policy Holder Name<font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 22%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtMasterPolicyHolder" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 27%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business (LOB)<font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 27%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtLineOfBusiness" Style="text-transform: uppercase" Width="100%" runat="server" Text="Health" ReadOnly="true"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 27%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblProductName" runat="server" Text="Product Name <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 27%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtProductName" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true" Text="Kotak Group Smart Cash"></obout:OboutTextBox>
                                </div>

                                <div style="position: absolute; top: 32%; left: 55%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblPolicyEndDate" runat="server" Text="Policy End Date <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 32%; left: 71%; width: 35%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtPolicyEndDate" Style="text-transform: uppercase" Width="57%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 32%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblPolicyStartDate" runat="server" Text="Policy Start Date <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 32%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtPolicyStartDate" Style="text-transform: uppercase;" Width="57%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                                </div>

                                <div style="position: absolute; top: 37%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblClaimIntimationDate" runat="server" Text="Claim Intimation Date <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 37%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtClaimIntimationDate" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 37%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <%--<obout:Calendar ID="clnClaimIntimationDate" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtClaimIntimationDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                    <img src="images/calendar.png" alt="" id="datepickerClaimIntimation" />
                                </div>

                                <div style="position: absolute; top: 37%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblClaimRegistrationDate" runat="server" Text="Claim Registration Date<font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 37%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtClaimRegistrationDate" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                                </div>

                                <div style="position: absolute; top: 42%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblClaimIntimationBy" runat="server" Text="Claim Intimation By <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 42%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">

                                    <obout:OboutDropDownList runat="server" ID="DrpClaimIntimationBy">
                                        <asp:ListItem Text="Select" Value="Select" />
                                        <asp:ListItem Text="Applicant" Value="Applicant" />
                                        <asp:ListItem Text="Co-Applicant" Value="Co-Applicant" />
                                        <asp:ListItem Text="Master Policy Holder" Value="Master Policy Holder" />
                                    </obout:OboutDropDownList>


                                </div>


                                <div style="position: absolute; top: 42%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblCallerNumber" runat="server" Text="Caller Contact Number" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 42%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtCallerNumber" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                                </div>

                                <div style="position: absolute; top: 47%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblDateOfAdmission" runat="server" Text="Date of Admission <font style='color:red'>*</font>" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 47%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtDateOfAdmission" runat="server" class="input" Width="100%" />
                                </div>
                                <div style="position: absolute; top: 47%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <%--<obout:Calendar ID="clnDateOfAdmission" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfAdmission" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                    <img src="images/calendar.png" alt="" id="datepickerAdmissionDate" />
                                </div>

                                <div style="position: absolute; top: 47%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblDateOfDischarge" runat="server" Text="Date Of Discharge" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 47%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtDateOfDischarge" Style="text-transform: uppercase" Width="100%" runat="server"> </obout:OboutTextBox>
                                </div>
                                <div style="position: absolute; top: 47%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <%--<obout:Calendar ID="clnDateOfDischarge" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                    <img src="images/calendar.png" alt="" id="datepickerDischargeDate" />
                                    <a href="javascript:ClearDate()" runat="server" onclick="ClearDate" id="hlClearDischargeDate">Clear</a>
                                </div>

                                <div style="position: absolute; top: 52%; left: 1%; width: 18%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblCustomerName" runat="server" Text="Member Name &lt;font style='color:red'&gt;*&lt;/font&gt; " Width="100%"></asp:Label>

                                </div>
                                <div style="position: absolute; top: 52%; left: 18%; width: 40%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutDropDownList ID="DrpCustomerName0" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DrpCustomerName_SelectedIndexChanged" Width="50%">
                                    </obout:OboutDropDownList>
                                </div>

                                <div style="position: absolute; top: 52%; left: 55%; width: 18%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblAccNumber" runat="server" Text="Account Number <font style='color:red'>*</font>" Width="100%"></asp:Label>

                                </div>
                                <div style="position: absolute; top: 52%; left: 71%; width: 40%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtAccountNumber" Style="text-transform: uppercase" Width="50%" runat="server" Enabled="false"> </obout:OboutTextBox>
                                </div>


                                <div style="position: absolute; top: 57%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblRelation" runat="server" Text="Relation with proposer&lt;font style='color:red'&gt;*&lt;/font&gt;" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 57%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutDropDownList ID="drpRelation0" runat="server" Width="100%">
                                    </obout:OboutDropDownList>
                                </div>
                                <div style="position: absolute; top: 57%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                </div>

                                <div style="position: absolute; top: 57%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblClaimedAmount" runat="server" Text="Claimed Amount <font style='color:red'>*</font>" Width="100%"></asp:Label>

                                </div>
                                <div style="position: absolute; top: 57%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtClaimedAmount" runat="server" MaxLength="10" class="input" Width="100%" />

                                </div>
                                <div style="position: absolute; top: 57%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <%--<obout:Calendar ID="Calendar2" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                </div>




                                <div style="position: absolute; top: 62%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblRemark" runat="server" Text="Remarks (if any)" Width="100%"></asp:Label>
                                </div>
                                <div style="position: absolute; top: 62%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    <obout:OboutTextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="5" Columns="5" class="input" Width="100%" MaxLength="4000" />
                                </div>
                                <div style="position: absolute; top: 62%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                </div>

                                <div style="position: absolute; top: 62%; left: 55%; width: 12%; margin-top: 10px; margin-left: 10px">
                                    <asp:Label ID="lblClaimForm" runat="server" Text="Upload Document" Width="100%"></asp:Label>

                                </div>
                                <div style="position: absolute; top: 62%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">

                                    <asp:FileUpload ID="fulClaimForm" runat="server" Width="100%" />
                                    <asp:Label ID="lblfulMessage" Style="overflow: hidden; font-size: small; color: red" runat="server" Text="Allow file with extensions .zip,.pdf,.jpeg,.xls,.xlsx,.doc,.docx,.7z,.rar upto 4 MB size."></asp:Label>

                                </div>
                                <div style="position: absolute; top: 62%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                </div>







                                <div class="section colm colm12" style="position: absolute; top: 86%">
                                    <asp:Panel ID="Panel2" runat="server" Height="60px">
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

