<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCPaymentProcessUpload.aspx.cs" Inherits="PrjPASS.FrmHDCPaymentProcessUpload" %>

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
            font-size: 11px;
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
    </style>






    <script type="text/javascript">

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
            $("[id$=txtFromDate]").datepicker({
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
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });


        }

    </script>

    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Payment Process Upload</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="400px" BorderColor="#3399ff" BorderWidth="2">
                            <div id="d3" runat="server" style="position: absolute; top: 7%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblmessage" runat="server" Text="Select valid Excel file to upload" Width="173%"></asp:Label>
                            </div>
                            <div id="d4" runat="server" style="position: absolute; top: 7%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <asp:FileUpload ID="fuPaymentDetail" runat="server" />
                            </div>

                            <div id="d5" runat="server" style="position: absolute; top: 7%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnUploadFile" runat="server" Text="Upload Data" OnClick="btnUploadFile_Click" />
                            </div>

                            <div id="d6" runat="server" style="position: absolute; top: 7%; left: 85%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnDownloadTemplate" runat="server" Text="DownLoad Template" OnClick="btnDownloadTemplate_Click" />
                            </div>
                            <div id="dvGridView" runat="server" visible="false" style="display: block; position: relative; top: 27%;height:300px; width: 100%; overflow-x: auto; overflow-y: auto;">
                                <asp:GridView CssClass="mydatagrid" ID="gvdtExcel" runat="server" AutoGenerateColumns="true"
                                    PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                                    <RowStyle Height="20px" />
                                </asp:GridView>
                                <br />
                                <div style="position: relative;height:25px; left: 55%;margin-top:10px">
                                    <obout:OboutButton ID="btnSubmit" runat="server" Text="Submit Data" OnClick="btnSubmit_Click" />
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="section colm colm12" style="display:block;margin-top: 50px">
                            <asp:Panel ID="Panel2" runat="server" Height="60px"  BorderColor="#3399ff" BorderWidth="2" >
                                <div style="position: absolute; left: 45%; width:15%; margin-left: 10px;margin-top:10px"> 
                                    <obout:OboutButton ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"></obout:OboutButton>
                                </div>
                                
                                <div style="position: absolute; left: 65%; width:15%; margin-left: 10px;margin-top:10px">
                                    <obout:OboutButton ID="btnExit" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <%--    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>--%>
</asp:Content>

