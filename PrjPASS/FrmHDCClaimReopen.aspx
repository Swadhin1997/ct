<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCClaimReopen.aspx.cs" Inherits="PrjPASS.FrmHDCClaimReopen" %>

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
        $(function () {
            ApplyDatePicker();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
        });

        //function SetAutoComplete() {
        //    debugger;
        //    $("input[name*='chkp1']").checked(true);

        //    // $('#chkp1').checked(true);

        //    $("input[name*='chkp1']").change(function () {
        //        debugger;
        //        if ($(this).is(":checked")) {
        //            debugger;
        //            $("#txtPayeeName").removeAttr('disabled');
        //            $("#txtPayeeAccNo").removeAttr('disabled');
        //            $("#txtIfscCode").removeAttr('disabled');
        //            $("#DrpPaymentType").removeAttr('disabled');
        //            $("#DrpPaymentMode").removeAttr('disabled');
        //            $("#txtDDLocation").removeAttr('disabled');
        //            $("#txtPANNumber").removeAttr('disabled');
        //            $("#txtGSTNumber").removeAttr('disabled');
        //            $("#txtInvoiceNumber").removeAttr('disabled');
        //            $("#txtInvoiceDate").removeAttr('disabled');
        //            $("#txtFinalApprovedAmt").removeAttr('disabled');
        //            $("#txtIGST").removeAttr('disabled');
        //            $("#txtCGST").removeAttr('disabled');
        //            $("#txtSGST").removeAttr('disabled');
        //            $("#txtUGST").removeAttr('disabled');
        //            $("#txtTDSAmount").removeAttr('disabled');
        //            $("#txtFinalPayableAmt").removeAttr('disabled');
        //        }
        //        else {
        //            debugger;
        //            $("#txtPayeeName").attr('disabled', 'disabled');
        //            $("#txtPayeeAccNo").attr('disabled', 'disabled');
        //            $("#txtIfscCode").attr('disabled', 'disabled');
        //            $("#DrpPaymentType").attr('disabled', 'disabled');
        //            $("#DrpPaymentMode").attr('disabled', 'disabled');
        //            $("#txtDDLocation").attr('disabled', 'disabled');
        //            $("#txtPANNumber").attr('disabled', 'disabled');
        //            $("#txtGSTNumber").attr('disabled', 'disabled');
        //            $("#txtInvoiceNumber").attr('disabled', 'disabled');
        //            $("#txtInvoiceDate").attr('disabled', 'disabled');
        //            $("#txtFinalApprovedAmt").attr('disabled', 'disabled');
        //            $("#txtIGST").attr('disabled', 'disabled');
        //            $("#txtCGST").attr('disabled', 'disabled');
        //            $("#txtSGST").attr('disabled', 'disabled');
        //            $("#txtUGST").attr('disabled', 'disabled');
        //            $("#txtTDSAmount").attr('disabled', 'disabled');
        //            $("#txtFinalPayableAmt").attr('disabled', 'disabled');
        //        }

        //    });

        //    $('#chkp1').click(function () {
        //        debugger;
        //        if (!$(this).is(':checked')) {
        //            return confirm("Are you sure?");
        //        }
        //    });



        //    $('#chkp2').checked(true);

        //    $('#chkp2').change(function () {
        //        debugger;
        //        if ($(this).is(":checked")) {
        //            $("#txtPayeeName2").removeAttr('disabled');
        //            $("#txtPayeeAccNo2").removeAttr('disabled');
        //            $("#txtIfscCode2").removeAttr('disabled');
        //            $("#DrpPaymentType2").removeAttr('disabled');
        //            $("#DrpPaymentMode2").removeAttr('disabled');
        //            $("#txtDDLocation2").removeAttr('disabled');
        //            $("#txtPANNumber2").removeAttr('disabled');
        //            $("#txtGSTNumber2").removeAttr('disabled');
        //            $("#txtInvoiceNumber2").removeAttr('disabled');
        //            $("#txtInvoiceDate2").removeAttr('disabled');
        //            $("#txtFinalApprovedAmt2").removeAttr('disabled');
        //            $("#txtIGST2").removeAttr('disabled');
        //            $("#txtCGST2").removeAttr('disabled');
        //            $("#txtSGST2").removeAttr('disabled');
        //            $("#txtUGST2").removeAttr('disabled');
        //            $("#txtTDSAmount2").removeAttr('disabled');
        //            $("#txtFinalPayableAmt2").removeAttr('disabled');
        //        }
        //        else {
        //            debugger;
        //            $("#txtPayeeName2").attr('disabled', 'disabled');
        //            $("#txtPayeeAccNo2").attr('disabled', 'disabled');
        //            $("#txtIfscCode2").attr('disabled', 'disabled');
        //            $("#DrpPaymentType2").attr('disabled', 'disabled');
        //            $("#DrpPaymentMode2").attr('disabled', 'disabled');
        //            $("#txtDDLocation2").attr('disabled', 'disabled');
        //            $("#txtPANNumber2").attr('disabled', 'disabled');
        //            $("#txtGSTNumber2").attr('disabled', 'disabled');
        //            $("#txtInvoiceNumber2").attr('disabled', 'disabled');
        //            $("#txtInvoiceDate2").attr('disabled', 'disabled');
        //            $("#txtFinalApprovedAmt2").attr('disabled', 'disabled');
        //            $("#txtIGST2").attr('disabled', 'disabled');
        //            $("#txtCGST2").attr('disabled', 'disabled');
        //            $("#txtSGST2").attr('disabled', 'disabled');
        //            $("#txtUGST2").attr('disabled', 'disabled');
        //            $("#txtTDSAmount2").attr('disabled', 'disabled');
        //            $("#txtFinalPayableAmt2").attr('disabled', 'disabled');
        //        }

        //    });

        //    $('#chkp2').click(function () {
        //        debugger;
        //        if (!$(this).is(':checked')) {
        //            return confirm("Are you sure?");
        //        }
        //    });
        //}

        function ApplyDatePicker() {
            $("[id$=txtInvoiceDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("[id$=txtInvoiceDate2]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });




            $("[id$=txtDateOfAdmission]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerAdmissionDate").click(function () {
                $("[id$=txtDateOfAdmission]").datepicker("show");
            });



            $("[id$=txtDateOfDischarge]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerfDischarge").click(function () {
                $("[id$=txtDateOfDischarge]").datepicker("show");
            });

        }


    </script>

    <style type="text/css">
        #tblPaymentProcess {
            display: block;
            overflow-x: auto;
            white-space: nowrap;
        }
    </style>
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claim Reopen</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="380px" BorderColor="#3399ff" BorderWidth="2">

                            <div id="d58" runat="server" style="position: absolute; top: 1%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCertificateNumber" runat="server" Text="Certificate/Claim Number<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d59" runat="server" style="position: absolute; top: 1%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="50%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div id="d60" runat="server" style="position: absolute; top: 1%; left: 33%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSearchClaim" Width="50%" runat="server" Text="Search" OnClick="btnSearchClaim_Click"></obout:OboutButton>
                            </div>

                            <div id="d1" runat="server" style="position: absolute; top: 1%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerName" runat="server" Text="Claim Number<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d2" runat="server" style="position: absolute; top: 1%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimNumber" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                            </div>

                            <div id="d3" runat="server" style="position: absolute; top: 7%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerType" runat="server" Text="Certificate Number<span style='color:red'>*</span>" Width="173%"></asp:Label>
                            </div>
                            <div id="d4" runat="server" style="position: absolute; top: 7%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertNumber" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30" ReadOnly="true"></obout:OboutTextBox>
                            </div>
                            <div id="d5" runat="server" style="position: absolute; top: 7%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateofAdmission" runat="server" Text="Date of Admission<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d6" runat="server" style="position: absolute; top: 7%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfAdmission" runat="server" Width="100%" />
                            </div>
                            <div id="d61" runat="server" style="position: absolute; top: 7%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                               <%-- <obout:Calendar ID="clnDateOfAdmission" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfAdmission" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                <img src="images/calendar.png" alt="" id="datepickerAdmissionDate" />
                            </div>


                            <div id="d7" runat="server" style="position: absolute; top: 13%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateOfDischarge" runat="server" Text="Date Of Discharge<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d8" runat="server" style="position: absolute; top: 13%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfDischarge" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                            </div>
                            <div id="d64" runat="server" style="position: absolute; top: 13%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                               <%-- <obout:Calendar ID="clnDateofDischarge" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                 <img src="images/calendar.png" alt="" id="datepickerfDischarge" />
                            </div>

                            <div id="d9" runat="server" style="position: absolute; top: 13%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblSettlementType" runat="server" Text="Date of Reopen<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d10" runat="server" style="position: absolute; top: 13%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtReopenDate" runat="server" Style="text-transform: uppercase" Width="100%" ReadOnly="true"> </obout:OboutTextBox>
                            </div>

                            <div id="d11" runat="server" style="position: absolute; top: 19%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblReasonofReopening" runat="server" Text="Reason of Reopening<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d12" runat="server" style="position: absolute; top: 19%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="DrpSettelmentType" runat="server" Width="100%">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="Policyholder Justifies right to be Indemnified">Policyholder Justifies right to be Indemnified</asp:ListItem>
                                    <asp:ListItem Value="Documents received after claim is closed">Documents received after claim is closed</asp:ListItem>
                                    <asp:ListItem Value="Consumer Forun">Consumer Forun</asp:ListItem>
                                    <asp:ListItem Value="Ombudsam">Ombudsam</asp:ListItem>
                                    <asp:ListItem Value="Grievance">Grievance</asp:ListItem>
                                    <asp:ListItem Value="Court">Court</asp:ListItem>
                                    <asp:ListItem Value="Wrongly Closure of Claim">Wrongly Closure of Claim</asp:ListItem>
                                </obout:OboutDropDownList>
                            </div>

                            <div id="d13" runat="server" style="position: absolute; top: 19%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimedAmt" runat="server" Text="Claimed Amount<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d14" runat="server" style="position: absolute; top: 19%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimedAmount" MaxLength="10" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                            </div>

                            <div id="d15" runat="server" style="position: absolute; top: 25%; left: 1%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblExpenseAmount" runat="server" Text="Expense Amount" Width="100%"></asp:Label>
                            </div>
                            <div id="d16" runat="server" style="position: absolute; top: 25%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtExpenseAmount" MaxLength="10" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                            </div>



                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Save" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click1"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnReset" Width="100%" runat="server" Text="Reset" OnClick="btnReset_Click"></obout:OboutButton>
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
    <asp:HiddenField runat="server" ID="hdnPinCodeLocality" />
    <asp:HiddenField runat="server" ID="hdnPinCode" />
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <asp:HiddenField runat="server" ID="hdnvTotalPolicySumInsured" />
    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>
</asp:Content>

