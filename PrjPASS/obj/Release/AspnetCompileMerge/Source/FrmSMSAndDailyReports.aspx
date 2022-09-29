<%@ Page Title="Resend SMS - Connect One" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmSMSAndDailyReports.aspx.cs" Inherits="PrjPASS.FrmSMSAndDailyReports" %>


<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_NET" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divMain").css('position', 'relative');
            $("#divMain").css('left', '0%');
            $("#divMain").css('width', '100%');
            $("#divMain").css('top', '0%');

            $("#divSmartContainer").css('margin-bottom', '16px');
        });

    </script>

    <style type="text/css">
        #table1 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }

        #table21 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }

        #table22 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }

        #table24 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }

        #table3 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color: black;
        }

        #table23 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color: black;
        }

        #table13 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table4 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color: black;
        }

        #table5 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color: black;
        }

        #table6 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table7 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table8 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table9 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        .tdbkg {
            background-color: lightgray;
            font-size: 11px;
        }

        .tdbkgHead {
            background-color: darkgray;
            font-size: 11px;
        }

        .tdbkg2 {
            /*background-color: rgba(51, 180, 255, 0.22);*/
            font-size: 11px;
        }



        .validationsummary {
            border: 1px solid rgba(51, 153, 255, 0.2);
            font-size: 11px;
            padding: 0px 0px 13px 0px;
        }

        .validationheader {
            left: 0px;
            position: relative;
            font-size: 12px;
            background-color: rgba(51, 153, 255, 0.27);
            color: black;
            height: 12px;
            border-bottom: 1px solid rgba(51, 153, 255, 0.2);
            padding-top: 3px;
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 0px 0px 13px 0px;
        }


        .validationsummary ul li {
            padding: 2px 0px 0px 15px;
            background-image: url(Images/error4.png);
            background-size: 10px 10px;
            background-position: 0px 3px;
            background-repeat: no-repeat;
        }

        .validationsummary ul {
            padding-top: 5px;
            padding-left: 10px;
            list-style: none;
            font-size: 11px;
            color: #ff0000;
        }

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

        /* Absolute Center Spinner */
        #resultLoading {
            position: fixed;
            z-index: 999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            #resultLoading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            #resultLoading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                #resultLoading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        .mydatagrid {
            width: 100%;
            border: solid 1px black;
            font-size: 8px;
        }

        .header {
            background-color: #c1c4d0;
            font-family: Arial;
            color: black;
            border: none 0px transparent;
            height: 25px;
            text-align: center;
            font-size: 14px;
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

        .pager {
            background-color: #c1c4d0;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .mydatagrid td {
            padding: 5px;
            border: 1px solid black;
        }

        .mydatagrid th {
            padding: 5px;
            border: 1px solid black;
        }
    </style>

    <style type="text/css">
        .modalBackgroundStatus {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopupStatus {
            background-color: #FFFFFF;
            width: 500px;
            border: 2px solid #18B5F0;
            border-radius: 8px;
            padding: 0;
        }

            .modalPopupStatus .headerStatus {
                background-color: #0087bb;
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: left;
                font-weight: bold;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
                padding-left: 5px;
                font-size: small;
            }

            .modalPopupStatus .bodyStatus {
                min-height: 50px;
                line-height: 20px;
                font-weight: bold;
                font-size: small;
                padding: 6px;
                text-align: left;
                color: red;
            }

            .modalPopupStatus .footerStatus {
                padding: 6px;
            }

            .modalPopupStatus .yesStatus, .modalPopupStatus .noStatus {
                color: White;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
                border-radius: 4px;
            }

            .modalPopupStatus .yesStatus {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
                font-size: small;
                padding: 5px;
            }

            .modalPopupStatus .noStatus {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }


        .modalPopupPB {
            background-color: #FFFFFF;
            width: 70%;
            height: 590px;
            border: 2px solid #18B5F0;
            border-radius: 8px;
            padding: 0;
        }

            .modalPopupPB .headerPB {
                background-color: #0087bb;
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: left;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
                padding-left: 5px;
                font-size: small;
            }

            .modalPopupPB .bodyPB {
                min-height: 50px;
                font-size: small;
                padding: 4px;
                text-align: left;
            }

            .modalPopupPB .footerPB {
                padding: 6px;
            }

            .modalPopupPB .yesPB, .modalPopupPB .noPB {
                color: White;
                text-align: center;
                cursor: pointer;
                border-radius: 4px;
            }

            .modalPopupPB .yesPB {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
                font-size: small;
                padding: 5px;
            }

                .modalPopupPB .yesPB:hover {
                    background-color: dodgerblue;
                }

            .modalPopupPB .DownloadPB {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
                font-size: small;
                padding: 5px;
                color: White;
                text-align: center;
                cursor: pointer;
                border-radius: 4px;
                margin-right: 10px;
            }

                .modalPopupPB .DownloadPB:hover {
                    background-color: dodgerblue;
                }


            .modalPopupPB .noPB {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }

        .accordionHeaderSelected {
            border: 1px solid #2E4d7B;
            color: white;
            background-color: rgba(51, 51, 51, 0.69);
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            padding-left: 30px;
            margin-top: 5px;
            cursor: pointer;
            background-image: url(Images/bullet_toggle_minus.png);
            background-repeat: no-repeat;
            background-position: left;
        }

        .accordionHeader {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: rgba(51, 51, 51, 0.69);
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            padding-left: 30px;
            margin-top: 4px;
            cursor: pointer;
            background-image: url(Images/bullet_toggle_plus.png);
            background-repeat: no-repeat;
            background-position: left;
        }

        .accordionContent {
            border: 1px solid #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 6px;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            //SetAutoComplete();

            $("#table21 [id*=chkHeader]").click(function () {
                if ($(this).is(":checked")) {
                    $("#table21 [id*=chkRow]").prop("checked", true);
                } else {
                    $("#table21 [id*=chkRow]").prop("checked", false);
                }
            });
            $("#table21 [id*=chkRow]").click(function () {
                if ($("#table21 [id*=chkRow]").length == $("#table21 [id*=chkRow]:checked").length) {
                    $("#table21 [id*=chkHeader]").attr("checked", "checked");
                } else {
                    $("#table21 [id*=chkHeader]").removeAttr("checked");
                }
            });

        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
        });

        function SetAutoComplete() {

            $("[id$=txtFromDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#dtpFromDate").click(function () {
                $("[id$=txtFromDate]").datepicker("show");
            });

            $("[id$=txtToDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#dtpToDate").click(function () {
                $("[id$=txtToDate]").datepicker("show");
            });

        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MstCntFormContent" runat="server">

    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4" id="divSmartContainer" style="margin-top: -115px; margin-bottom: 16px">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>SMS And MISC Reports Status</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue" style="padding-top: 16px;">
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                <div class="frm-row">
                    <div class="section colm colm12">
                        <table id="table23" style="width: 100%;" cellspacing="0" cellpadding="2">
                            <tr>
                                <td class="tdbkgHead" colspan="6" style="font-size: 15px">Get Reports
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnDownloadMiniGist" runat="server" Text="Mini Gist Docs Dump Report"  Font-Size="15px"
                                        OnClick="btnDownloadMiniGist_Click"
                                        OnClientClick="return confirm('Mini Gist Report will be sent to the configured email ids, kindly do not click on this link again untill the operation is completed and it will take around 10 minutes to process the request, are you sure you want to proceed?');">
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                            <tr>
                                <td class="tdbkgHead" colspan="6" style="font-size: 15px">Search By
                                </td>
                            </tr>
                            <tr>
                                <td class="tdbkg">Message Type
                                </td>
                                <td>
                                    <obout:OboutRadioButton GroupName="mt" ID="rbtnClaimCount" Text="Claim Count" runat="server" Checked="true"></obout:OboutRadioButton>
                                    <obout:OboutRadioButton GroupName="mt" ID="rbtnQuoteCount" Text="Quote Count" runat="server"></obout:OboutRadioButton>
                                    <obout:OboutRadioButton GroupName="mt" ID="rbtnTotalPremium" Text="Total Daily Premium" runat="server"></obout:OboutRadioButton>
                                </td>

                                <td class="tdbkg">Delivery Status
                                </td>
                                <td>
                                    <obout:OboutRadioButton GroupName="ds" ID="rbtnTimeOut" Text="Time Out" runat="server" Checked="true"></obout:OboutRadioButton>
                                    <obout:OboutRadioButton GroupName="ds" ID="rbtnOtherThenDelivered" Text="Other Then Delivered" runat="server"></obout:OboutRadioButton>
                                </td>
                                <td class="tdbkg">Duration
                                </td>
                                <td>Last 
                                    <asp:DropDownList CssClass="drp" ID="drpDuration" runat="server">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                    Days
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td class="tdbkg">From
                                </td>
                                <td>
                                    <obout:OboutTextBox ID="txtFromDate" runat="server" ReadOnly="false"></obout:OboutTextBox>
                                    <img src="images/calendar.png" alt="" id="dtpFromDate" />
                                </td>
                                <td class="tdbkg">To
                                </td>
                                <td>

                                    <obout:OboutTextBox ID="txtToDate" runat="server" ReadOnly="false"></obout:OboutTextBox>
                                    <img src="images/calendar.png" alt="" id="dtpToDate" />

                                </td>
                            </tr>
                            <tr>

                                <td class="tdbkg" colspan="6" align="right">
                                    <obout:OboutButton ID="btnSearch" runat="server" Text="Search SMS" OnClick="btnSearch_Click"></obout:OboutButton>
                                </td>
                            </tr>

                        </table>
                        <br />
                        <div style="text-align: center">
                            <asp:Repeater ID="rptSMSData" runat="server">
                                <HeaderTemplate>
                                    <table cellspacing="0" rules="all" border="1" id="table21" style="border-collapse: collapse; width: 100%; text-align: center;">
                                        <tr class="header" style="text-align: center;">
                                            <th scope="col" style="width: 20px; text-align: center;">
                                                <asp:CheckBox ID="chkHeader" runat="server" />
                                            </th>
                                            <th scope="col" style="width: 100px; text-align: center;">Mobile
                                            </th>
                                            <th scope="col" style="width: 100px">Message
                                            </th>
                                            <th scope="col" style="width: 100px">MID
                                            </th>
                                            <th scope="col" style="width: 100px">Submit Time
                                            </th>
                                            <th scope="col" style="width: 10px">Status
                                            </th>
                                            <th scope="col" style="width: 100px">Delivery Status
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="rows">
                                        <td style="text-align: center;">
                                            <asp:CheckBox ID="chkRow" runat="server" />
                                            <asp:HiddenField ID="hdnsno" runat="server" Value='<%# Eval("sno") %>' />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="lbldest" runat="server" Text='<%# Eval("dest") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblmsg" runat="server" Text='<%# Eval("msg") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblmid" runat="server" Text='<%# Eval("mid") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsubmitTime" runat="server" Text='<%# Eval("submitTime") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldlr_status" runat="server" Text='<%# Eval("dlr_status") %>' />
                                        </td>

                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                            <br />
                            <div style="text-align: left; width: 100%">
                                <obout:OboutButton ID="btnSendSMS" runat="server" Text="SEND SMS" OnClick="btnSendSMS_Click" Visible="false"></obout:OboutButton>
                            </div>
                        </div>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" BorderColor="#727272" BorderWidth="2" Visible="true">


                            <div style="position: absolute; top: 40%; left: 42%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
