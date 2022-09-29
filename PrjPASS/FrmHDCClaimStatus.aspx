<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCClaimStatus.aspx.cs" Inherits="PrjPASS.FrmHDCClaimStatus" %>

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
        window.onload = function () {
            gvSubDetails.convertToExcel(
                ['ReadOnly', 'ReadOnly', 'TextBox', 'ComboBox', 'Action'],
                '<%=Grid1ExcelData.ClientID %>',
                '<%=Grid1ExcelDeletedIds.ClientID %>'
                );
        }
        $(function () {
            SetAutoComplete();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
        });



        function SetAutoComplete() {
            $("[id$=txtHospitalPinCode]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmHDCClaimDetails.aspx/GetPincode") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                var strItems = item.split("~");
                                return {
                                    label: item,
                                    val: strItems[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (event, ui) {
                    var strItems2 = ui.item.label.split("~");
                    $("[id$=txtHospitalPinCode]").val(strItems2[0]);
                    $("[id$=hdnPinCode]").val(strItems2[0]);
                    $("[id$=hdnPinCodeLocality]").val(ui.item.val);
                    $("[id*=btnGetPincodeDetails]").click();
                },
                minLength: 3,
                autoFocus: true
            });
        }


    </script>
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claims Details</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <asp:HiddenField ID="hfPolicyStartDate" Value="" runat="server" />
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="460px" BorderColor="#3399ff" BorderWidth="2">

                            <div id="d58" runat="server" style="position: absolute; top: 1%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCertificateNumber" runat="server" Text="Claim Number" Width="100%"></asp:Label>
                            </div>
                            <div id="d59" runat="server" style="position: absolute; top: 1%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="50%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div id="d60" runat="server" style="position: absolute; top: 1%; left: 33%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSearchClaim" Width="50%" runat="server" Text="Search" OnClick="btnSearchClaim_Click"></obout:OboutButton>
                            </div>
                            <div id="dvPaymentDetails" runat="server" style="position: absolute; top: 20%;left:20%">
                                <asp:GridView ID="gvPaymentDetails" runat="server" CssClass="mydatagrid" AutoGenerateColumns="true"
                                    PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                                    <EmptyDataTemplate>
                                        No Data Found.
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>
</asp:Content>

