<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmPartner.aspx.cs" Inherits="PrjPASS.FrmPartner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divMain").css('position', 'relative');
            $("#divMain").css('left', '0%');
            $("#divMain").css('width', '100%');
            $("#divMain").css('top', '0%');
            $("#divSmartContainer").css('margin-top', '-30px');
            $("#divSmartContainer").css('margin-bottom', '16px');
            $("#divLogo").css('top', '-3px');
            $("#decorative2").css('height', '97px');
        });
    </script>

    <style type="text/css">
        #table1 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 2px;
        }

        #table3 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table13 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table4 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table5 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table6 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table7 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table8 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        #table9 td {
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 4px;
        }

        .tdbkg {
            background-color: rgba(51, 153, 255, 0.27);
            font-size: 11px;
        }

        .textbox {
            border: 1px solid rgba(0, 116, 128, 0.45);
            padding: 3px;
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MstCntFormContent" runat="server">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4" id="divSmartContainer" style="margin-top: -32px; margin-bottom: 16px">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Partner Integration</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue" style="padding-top: 16px;">
                <div class="frm-row">
                    <div class="section colm colm12">

                        <table id="table1" border="1" style="table-layout">
                            <tr>
                                <td>USER ID:</td>
                                <td>
                                    <asp:TextBox ID="txtUserId" Text="GC0014" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>PASSWORD</td>
                                <td>
                                    <asp:TextBox ID="txtPassword" Text="cmc123" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Product Type</td>
                                <td>
                                    <asp:TextBox ID="txtProductType" Text="Private Car - p" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Transaction Type
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactionType" Text="Renewal" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Transaction Id</td>
                                <td>
                                    <asp:TextBox ID="txtTransaction" Text="T1000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Policy Number
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPolicyNumber" Text="P1000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Proposal Date</td>
                                <td>
                                    <asp:TextBox ID="txtProposalDate" Text="2016-08-12" runat="server" CssClass="textbox"></asp:TextBox> YYYY-MM-DD
                                </td>
                                <td>Proposal Time
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProposalTime" Text="15:00:00" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Policy Start Date</td>
                                <td>
                                    <asp:TextBox ID="txtPolicyStartDate" Text="2002-05-30" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Policy End Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPolicyEndDate" Text="2003-05-29" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>ProposalType</td>
                                <td>
                                    <asp:TextBox ID="txtProposalType" Text="I" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>CustomerName
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerName" Text="Hasmukh" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>EngineNumber</td>
                                <td>
                                    <asp:TextBox ID="txtEngineNumber" Text="E0001" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Chessis Number
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChessisNumber" Text="C0001" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>Make</td>
                                <td>
                                    <asp:TextBox ID="txtMake" Text="Audi" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Model
                                </td>
                                <td>
                                    <asp:TextBox ID="txtModel" Text="A4" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Variant</td>
                                <td>
                                    <asp:TextBox ID="txtVariant" Text="V" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>IDV
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIDV" Text="50000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>Total A
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalA" Text="30000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Total B
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalB" Text="20000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Gross Premium
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGWP" Text="50000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Service Or Sales Tax
                                </td>
                                <td>
                                    <asp:TextBox ID="txtServiceSalesTax" Text="500" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>Net Premium
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNetPremium" Text="49500" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Transaction Reference
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactioReference" Text="12345678912345678912" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Transaction Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactionDate" Text="2002-05-30" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Transaction Amount
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactionAmount" Text="50000" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>Prefix
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrefix" Text="CWP" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>VehicleRegistrationNumber
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVehicleRegistrationNumber" Text="xyz" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>BiFuelKit
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBiFuelKit" Text="abc" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>BiFuelKitIDV
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBiFuelKitIDV" Text="123" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>NCB
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNCB" Text="321" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>LegalLiabilityPaidDriver
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLegalLiabilityPaidDriver" Text="cdef" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>ZeroDepreciation
                                </td>
                                <td>
                                    <asp:TextBox ID="txtZeroDepreciation" Text="hij" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>EngineProtector
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEngineProtector" Text="jkl" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>ReturnToInvoice
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReturnToInvoice" Text="mno" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>PAUnnamedPassengerSI
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPAUnnamedPassengerSI" Text="pqr" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Consumables_Addon
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsumables_Addon" Text="stu" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>RoadsideAssistance
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRoadsideAssistance" Text="vzx" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>

                                <td>IMTNos
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIMTNos" Text="zzzz" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                                <td>Additional_Info
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdditional_Info" Text="extra" runat="server" CssClass="textbox"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnSubmit" Width="200px" Height="50px" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="btn btn-primary" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
