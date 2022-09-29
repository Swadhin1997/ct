<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCClaimProcess.aspx.cs" Inherits="PrjPASS.FrmHDCClaimProcess" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>




<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">





    <style type="text/css">
        .mydatagrid {
            width: 100%;
            border: solid 1px black;
            font-size: 16px;
        }

            .mydatagrid a /** FOR THE PAGING ICONS  **/ {
                background-color: Transparent;
                padding: 5px 5px 5px 5px;
                color: navy;
                text-decoration: none;
                font-weight: bold;
            }

                .mydatagrid a:hover /** FOR THE PAGING ICONS  HOVER STYLES**/ {
                    color: navy;
                }

            .mydatagrid span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
                color: #000;
                padding: 5px 5px 5px 5px;
            }


            .mydatagrid td {
                padding: 5px;
                border: 1px solid black;
                text-align: center;
            }

            .mydatagrid th {
                padding: 3px;
                border: 1px solid black;
                text-align: center;
            }

        .pager {
            background-color: #c1c4d0;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .header {
            background-color: #c1c4d0;
            font-family: Arial;
            color: black;
            border: none 0px transparent;
            height: 20px;
            text-align: center;
            font-size: 12px;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 12px;
            color: #000;
            min-height: 25px;
            text-align: left;
            border: none 0px transparent;
        }

            .rows:hover {
                background-color: #c1c4d0;
                font-family: Arial;
                color: black;
                text-align: left;
            }

        .selectedrow {
            background-color: #ff8000;
            font-family: Arial;
            color: #fff;
            font-weight: bold;
            text-align: left;
        }


        #tblPaymentProcess {
            border-collapse: collapse;
        }

        #tblPaymentProcess, th, td {
            border: 1px solid black;
        }
    </style>

    <script type="text/javascript">

        $(function () {
            ApplyDatePicker();
            CalculateRow1();
            CalculateRow2();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalculateRow1);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalculateRow2);
        });

        function CalculateRow1() {
            FinalApprovedAmt1 = parseInt($("#<%=txtFinalApprovedAmt.ClientID%>").val());
            var IGST1 = parseInt($("#<%=txtIGST.ClientID%>").val());
            var CGST1 = parseInt($("#<%=txtCGST.ClientID%>").val());
            var SGST1 = parseInt($("#<%=txtSGST.ClientID%>").val());
            var UGST1 = parseInt($("#<%=txtUGST.ClientID%>").val());
            var TDSAmt1 = parseInt($("#<%=txtTDSAmount.ClientID%>").val());
            var FinalPayableAmt1 = parseInt(0);
            FinalPayableAmt1 = FinalApprovedAmt1 + IGST1 + CGST1 + SGST1 + UGST1;
            FinalPayableAmt1 = FinalPayableAmt1 - TDSAmt1;
            $("#<%=txtFinalPayableAmt.ClientID%>").val(FinalPayableAmt1);
            $("#<%=hdnFinalPayableAmt1.ClientID%>").val(FinalPayableAmt1);
        }


        function CalculateRow2() {
            FinalApprovedAmt2 = parseInt($("#<%=txtFinalApprovedAmt2.ClientID%>").val());
            var IGST2 = parseInt($("#<%=txtIGST2.ClientID%>").val());
            var CGST2 = parseInt($("#<%=txtCGST2.ClientID%>").val());
            var SGST2 = parseInt($("#<%=txtSGST2.ClientID%>").val());
            var UGST2 = parseInt($("#<%=txtUGST2.ClientID%>").val());
            var TDSAmt2 = parseInt($("#<%=txtTDSAmount2.ClientID%>").val());
            var FinalPayableAmt2 = parseInt(0);
            FinalPayableAmt2 = FinalApprovedAmt2 + IGST2 + CGST2 + SGST2 + UGST2;
            FinalPayableAmt2 = FinalPayableAmt2 - TDSAmt2;
            $("#<%=txtFinalPayableAmt2.ClientID%>").val(FinalPayableAmt2);
            $("#<%=hdnFinalPayableAmt2.ClientID%>").val(FinalPayableAmt2);
        }


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


        }


    </script>

    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claim Payment Details</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="380px" BorderColor="#3399ff" BorderWidth="2">

                            <div id="d58" runat="server" style="position: absolute; top: 1%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCertificateNumber" runat="server" Text="Claim Number<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d59" runat="server" style="position: absolute; top: 1%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="50%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div id="d60" runat="server" style="position: absolute; top: 1%; left: 33%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSearchClaim" Width="50%" runat="server" Text="Search" OnClick="btnSearchClaim_Click"></obout:OboutButton>
                            </div>

                            <div id="d1" runat="server" style="position: absolute; top: 1%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px; display: none;">
                                <asp:Label ID="lblCustomerName" runat="server" Text="Certificate Number<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d2" runat="server" style="position: absolute; top: 1%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px; display: none;">
                                <obout:OboutTextBox ID="txtClaimNumber" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true" OnTextChanged="txtCustomerName_TextChanged"></obout:OboutTextBox>
                            </div>

                            <div id="d3" runat="server" style="position: absolute; top: 7%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerType" runat="server" Text="Certificate Number <span style='color:red'>*</span>" Width="173%"></asp:Label>
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
                            <div id="d61" runat="server" style="position: absolute; top: 7%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px; display: none">
                                <obout:Calendar ID="clnDateOfAdmission" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfAdmission" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>


                            <div id="d7" runat="server" style="position: absolute; top: 13%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateOfDischarge" runat="server" Text="Date Of Discharge<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d8" runat="server" style="position: absolute; top: 13%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfDischarge" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                            </div>
                            <div id="d64" runat="server" style="position: absolute; top: 13%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px; display: none">
                                <obout:Calendar ID="clnDateofDischarge" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>

                            <div id="d9" runat="server" style="position: absolute; top: 13%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblSettlementType" runat="server" Text="Settlement Type<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d10" runat="server" style="position: absolute; top: 13%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="DrpSettelmentType" runat="server" Width="100%">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="Fully Settled">Fully Settled</asp:ListItem>
                                    <asp:ListItem Value="Claim Withdrawn/CWP">Claim Withdrawn/CWP</asp:ListItem>
                                    <asp:ListItem Value="Claim Rejected/Repudiated">Claim Rejected/Repudiated</asp:ListItem>
                                    <asp:ListItem Value="Claim Reopen Paid">Claim Reopen Paid</asp:ListItem>
                                </obout:OboutDropDownList>
                            </div>



                            <div id="d11" runat="server" style="position: absolute; top: 19%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimType" runat="server" Text="Claim Type<span style='color:red'>*</span>" Width="100%"></asp:Label>
                            </div>
                            <div id="d12" runat="server" style="position: absolute; top: 19%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimType" Text="Reimbursement" Style="text-transform: uppercase" Width="58%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                            </div>


                            <div id="d13" runat="server" style="position: absolute; top: 28%; width: 100%; left: 10px;overflow-y: auto;height:150px">
                                <table style="display: block; overflow-x: auto; white-space: nowrap;" border="1" width="97%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvPrevPaymentDetails" runat="server" AutoGenerateColumns="true" CssClass="mydatagrid"
                                                PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Caption="<b> Previous Payment Details </b>" CaptionAlign="Top">
                                                <EmptyDataTemplate>
                                                    No Records available
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>

                                </table>
                            </div>


                            <div id="d14" runat="server" style="position: absolute; top: 75%; width: 100%; left: 10px">
                                <table id="tblPaymentProcess" cellspacing="2" cellpadding="2" runat="server" style="display: block; overflow-x: auto; white-space: nowrap;" border="1" width="97%">
                                    <tr>
                                        <th></th>
                                        <th>Payee Name<span style="color: red">*</span>	</th>
                                        <th>Payee Account Number<span style="color: red">*</span> </th>
                                        <th>Payee IFSC Code<span style="color: red">*</span>	</th>
                                        <th>Payment Type<span style="color: red">*</span></th>
                                        <th>Payment Mode</th>
                                        <th>DD Location</th>
                                        <th>PAN Number</th>
                                        <th>GST Number</th>
                                        <th>Invoice Number</th>
                                        <th>Invoice Date</th>
                                        <th>Final Approved Amount<span style="color: red">*</span></th>
                                        <th>IGST</th>
                                        <th>CGST</th>
                                        <th>SGST</th>
                                        <th>UGST</th>
                                        <th>TDS Amount</th>
                                        <th>Final Payable Amount</th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:OboutCheckBox ID="chkp1" name="chkp1" Checked="true" OnCheckedChanged="chkp1_CheckedChanged" AutoPostBack="true" runat="server"></obout:OboutCheckBox></td>
                                        <td>
                                            <div style="min-width: 150px">
                                                <obout:OboutTextBox ID="txtPayeeName" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtPayeeAccNo" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtIfscCode" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength="11"> </obout:OboutTextBox>
                                        </td>
                                        <td>

                                            <obout:OboutDropDownList ID="DrpPaymentType" AutoPostBack="true" OnSelectedIndexChanged="DrpPaymentType_SelectedIndexChanged" runat="server">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="Indemnity">Indemnity</asp:ListItem>
                                                <asp:ListItem Value="Expense">Expense</asp:ListItem>
                                            </obout:OboutDropDownList>

                                        </td>
                                        <td>
                                            <obout:OboutDropDownList ID="DrpPaymentMode" runat="server">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                                <asp:ListItem Value="NEFT">NEFT</asp:ListItem>
                                                <asp:ListItem Value="DD">DD</asp:ListItem>
                                            </obout:OboutDropDownList>

                                        </td>
                                        <td>
                                            <div style="min-width: 100px">
                                                <obout:OboutTextBox ID="txtDDLocation" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 100px">
                                                <obout:OboutTextBox ID="txtPANNumber" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength="10"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 100px">
                                                <obout:OboutTextBox ID="txtGSTNumber" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength ="20"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 150px">
                                                <obout:OboutTextBox ID="txtInvoiceNumber" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength ="10"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 100px">
                                                <obout:OboutTextBox ID="txtInvoiceDate" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtFinalApprovedAmt" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow1();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <div style="min-width: 60px">
                                                <obout:OboutTextBox ID="txtIGST" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow1();" Text="0"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 60px">
                                                <obout:OboutTextBox ID="txtCGST" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow1();" Text="0"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 60px">
                                                <obout:OboutTextBox ID="txtSGST" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow1();" Text="0"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 60px">
                                                <obout:OboutTextBox ID="txtUGST" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow1();" Text="0"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="min-width: 60px">
                                                <obout:OboutTextBox ID="txtTDSAmount" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow1();" Text="0"> </obout:OboutTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtFinalPayableAmt" runat="server" Style="text-transform: uppercase" Width="100%" Text="0" Enabled="false"> </obout:OboutTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <obout:OboutCheckBox ID="chkp2" name="chkp2" Checked="true" OnCheckedChanged="chkp2_CheckedChanged" AutoPostBack="true" runat="server"></obout:OboutCheckBox></td>
                                        <td>
                                            <obout:OboutTextBox ID="txtPayeeName2" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>

                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtPayeeAccNo2" runat="server" Style="text-transform: uppercase" Width="100%" > </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtIfscCode2" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength="11"> </obout:OboutTextBox>
                                        </td>
                                        <td>

                                            <obout:OboutDropDownList ID="DrpPaymentType2" AutoPostBack="true" OnSelectedIndexChanged="DrpPaymentType2_SelectedIndexChanged" runat="server">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="Indemnity">Indemnity</asp:ListItem>
                                                <asp:ListItem Value="Expense">Expense</asp:ListItem>
                                            </obout:OboutDropDownList>

                                        </td>
                                        <td>
                                            <obout:OboutDropDownList ID="DrpPaymentMode2" runat="server">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                                <asp:ListItem Value="NEFT">NEFT</asp:ListItem>
                                                <asp:ListItem Value="DD">DD</asp:ListItem>
                                            </obout:OboutDropDownList>

                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtDDLocation2" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtPANNumber2" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength="10"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtGSTNumber2" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength="20" > </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtInvoiceNumber2" runat="server" Style="text-transform: uppercase" Width="100%" MaxLength="10" > </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtInvoiceDate2" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtFinalApprovedAmt2" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow2();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtIGST2" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow2();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtCGST2" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow2();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtSGST2" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow2();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtUGST2" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow2();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtTDSAmount2" runat="server" Style="text-transform: uppercase" Width="100%" onkeyup="javascript:CalculateRow2();" Text="0"> </obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtFinalPayableAmt2" runat="server" Style="text-transform: uppercase" Width="100%" Text="0" Enabled="false"> </obout:OboutTextBox>
                                        </td>
                                    </tr>

                                </table>
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
    <asp:HiddenField runat="server" ID="hdnFinalPayableAmt1" />
    <asp:HiddenField runat="server" ID="hdnFinalPayableAmt2" />
    <%--    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>--%>
</asp:Content>

