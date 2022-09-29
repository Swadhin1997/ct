<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmDownloadRenewalNotice.aspx.cs" Inherits="PrjPASS.FrmDownloadRenewalNotice" %>

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
            font-size: 6px;
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
            text-align: center;
        }

        .mydatagrid th {
            padding: 3px;
            border: 1px solid black;
            text-align: center;
        }
    </style>



    <script type="text/javascript">

        $(function () {
            ApplyDatePicker();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
        });



        function ApplyDatePicker() {
            $("[id$=txtFromDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("[id$=txtToDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });



            $("#datepickerFromDate").click(function () {
                $("[id$=txtFromDate]").datepicker("show");
            });

            $("[id$=txtFromDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });


            $("#datepickerToDate").click(function () {
                $("[id$=txtToDate]").datepicker("show");
            });

            $("[id$=txtToDate]").datepicker({
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
                <h4><i class="fa fa-sign-in"></i>Policy Download</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="380px" BorderColor="#3399ff" BorderWidth="2">


                            <div id="d3" runat="server" style="position: absolute; top: 7%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number" Width="173%"></asp:Label>
                            </div>
                            <div id="d4" runat="server" style="position: absolute; top: 7%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyNumber" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="25"></obout:OboutTextBox>
                            </div>
                            <div id="d1" runat="server" style="position: absolute; top: 7%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>

                            <div id="d5" runat="server" style="position: absolute; top: 7%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCRNnumber" runat="server" Text="CRN Number" Width="100%"></asp:Label>
                            </div>
                            <div id="d6" runat="server" style="position: absolute; top: 7%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCRNnumber" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="15"></obout:OboutTextBox>
                            </div>
                            <div id="d61" runat="server" style="position: absolute; top: 7%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>


                            <div id="Div1" runat="server" style="position: absolute; top: 15%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblLANno" runat="server" Text="Loan Account Number" Width="173%"></asp:Label>
                            </div>
                            <div id="Div2" runat="server" style="position: absolute; top: 15%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtLANNo" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div id="Div3" runat="server" style="position: absolute; top: 15%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>


                            <div id="d7" runat="server" style="position: absolute; top: 23%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSearch" Width="100%" runat="server" Text="Search" OnClick="btnSearch_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 35%; left: 45px; text-align: center">
                                <center>
                                    <div style="text-align: center">
                                        <asp:GridView ID="gvPolicyData" CssClass="mydatagrid" runat="server" AutoGenerateColumns="false"
                                            DataKeyNames="TXT_POLICY_NO_CHAR,TXT_EMAIL,TXT_CUSTOMER_NAME" OnRowCommand="gvPolicyData_RowCommand"
                                            PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" OnSelectedIndexChanged="gvPolicyData_SelectedIndexChanged">
                                            <RowStyle Height="20px" HorizontalAlign="Center" Width="80%" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Policy Number" DataField="TXT_POLICY_NO_CHAR" />
                                                <asp:BoundField HeaderText="Insured Name" DataField="TXT_CUSTOMER_NAME" />
                                                <asp:BoundField HeaderText="Policy Start Date" DataField="DAT_POLICY_EFF_FROMDATE" />
                                                <asp:BoundField HeaderText="Policy End Date" DataField="DAT_POLICY_EFF_TODATE" />
                                                <asp:BoundField HeaderText="Product Name" DataField="PRODUCTNAME" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownload" Text="Download Notice" CommandArgument='<%# Eval("TXT_POLICY_NO_CHAR") %>' runat="server" Product-Code='<%# Eval("TXT_POLICY_NO_CHAR") %>' data-ID='<%# Eval("TXT_POLICY_NO_CHAR") %>' data-myData='<%# Eval("TXT_POLICY_NO_CHAR") %>' OnClick="lnkDownload_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                  <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkSendEmail" Text="Send Email" CommandArgument='<%# Eval("TXT_POLICY_NO_CHAR") %>' runat="server" Custmer-Name='<%# Eval("TXT_CUSTOMER_NAME") %>' data-myData='<%# Eval("TXT_EMAIL") %>' OnClick="lnkSendEmail_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div align="center" style="font-size: large; color: red; width: 900px">No records found.</div>
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </div>
                                </center>
                            </div>

                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">

                            <div style="position: absolute; top: 40%; left: 35%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click1"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
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
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <%--    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>--%>
</asp:Content>

