<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmQuoteReassignment.aspx.cs" Inherits="PrjPASS.FrmQuoteReassignment" %>

<%--<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>--%>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_NET" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divMain").css('position', 'relative');
            $("#divMain").css('left', '0%');
            $("#divMain").css('width', '100%');
            $("#divMain").css('top', '0%');
            ////$("#divSmartContainer").css('margin-top', '-30px');
            $("#divSmartContainer").css('margin-bottom', '16px');
            ManageHeaderCheckBox();
        });

        //Recheck Aspx Page

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

        .selected {
            background-color: #A1DCF2;
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
            SetAutoComplete();
            ShowHideCustomerRows();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ShowHideCustomerRows);
        });

        function openModal() {
            $('#modalSaveProposal').modal('hide');
            $('#myModal').modal('show');
        }

        function openModalSuccess() {
            $('#myModalSuccess').modal('show');
        }

        function openModalSaveProposal() {
            $('#modalSaveProposal').modal('show');
        }

        function CloseModalSaveProposal() {
            $('#modalSaveProposal').modal('hide');
        }

        function openModalSaveProposalSuccess() {
            $('#modalSaveProposalSuccess').modal('show');
        }

        function CloseModalSaveProposalSuccess() {
            $('#modalSaveProposalSuccess').modal('hide');
        }

        function openModalViewPremium() {
            $('#modalViewPremium').modal('show');
        }


        function CloseModalViewPremium() {
            $('#modalViewPremium').modal('hide');
        }


        function SetAutoComplete() {

            $("[id*=txtAssignedTo]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmQuoteReassignment.aspx/GetUsers") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                var strItems = item.split("~");
                                return {
                                    label: item,
                                    val: strItems[0]
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
                select: function (e, i) {
                    $("[id*=hdnAssignedTo]", $(e.target).closest("td")).val(i.item.val);
                },
                minLength: 3,
                autoFocus: true
            });


            $("[id$=txtPinCode]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmPremiumCalculatorMotor.aspx/GetPincode") %>',
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
                select: function (e, i) {
                    CloseModalSaveProposal();
                    var strItems2 = i.item.label.split("~");
                    $("[id$=txtPinCode]").val(strItems2[0]);
                    $("[id$=hdnPinCode]").val(strItems2[0]);
                    $("[id$=hdnPinCodeLocality]").val(i.item.val);
                    $("[id*=btnGetPincodeDetails]").click();
                },
                minLength: 3,
                autoFocus: true
            });


            $("[id$=txtDateofBirth]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#datepickerImagedob").click(function () {
                $("[id$=txtDateofBirth]").datepicker("show");
            });

            $("[id$=txtNomineeDOB]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#datepickerImagenomineedob").click(function () {
                $("[id$=txtNomineeDOB]").datepicker("show");
            });

            $("#datepickerImageFromDate").click(function () {
                $("[id$=txtFromDate]").datepicker("show");
            });

            $("[id$=txtFromDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#datepickerImageToDate").click(function () {
                $("[id$=txtToDate]").datepicker("show");
            });

            $("[id$=txtToDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });
        }

        function ShowHideCustomerRows() {
            var rbtIndividual = $('input[id*=rbtIndividual]').is(":checked");

            if (rbtIndividual) {
                $("#trCustomerRow1").show();
                $("#trCustomerRow2").show();
                $("#trCustomerRow3").show();
                $("#trCustomerRow4").hide();
                $("#trCustomerRow5").hide();
            }
            else {
                $("#trCustomerRow1").hide();
                $("#trCustomerRow2").hide();
                $("#trCustomerRow3").hide();
                $("#trCustomerRow4").show();
                $("#trCustomerRow5").show();
            }

            var rbtIsRollover = $('input[id*=rbtIsRollover]').is(":checked");

            if (rbtIsRollover) {
                $("#trPreviousInsurerRow").show();
            }
            else {
                $("#trPreviousInsurerRow").hide();
            }
        }

        debugger;
        function SelectAllCheckboxes(chk) {

            $('#<%=QuoteGridView.ClientID %>').find("input:checkbox").each(function () {
                if (this != chk) {
                    this.checked = chk.checked;
                }
            });
        }

        function UncheckHeaderCheckBox() {
            $("checkbox[id*=chkHeader]").attr("checked", false);
            $('table[id*=QuoteGridView] .chkHeader').find('input:checkbox').prop('checked', false);
        }

        function checkHeaderCheckBox() {
            //$('#chkHeader%>').find("input:checkbox").prop('checked', true);
            $("checkbox[id*=chkHeader]").attr("checked", true);
            $('table[id*=QuoteGridView] .chkHeader').find('input:checkbox').prop('checked', true);
        }

        debugger;
        function ManageHeaderCheckBox() {
            var length = $("table[id*=QuoteGridView] .chkRow").length;
            $("table[id*=QuoteGridView] .chkRow").find('input:checkbox').each(function () {
                if ($("table[id*=QuoteGridView] .chkRow").find('Checkbox:checked').length == length) {
                    checkHeaderCheckBox();
                }
                else {
                    UncheckHeaderCheckBox();
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MstCntFormContent" runat="server">




    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- Div for Quote Gridview --%>
            <div class="smart-wrap">
                <div class="smart-forms smart-container wrap-4" id="divSmartContainer" style="margin-top: -115px; margin-bottom: 16px">
                    <div class="form-header header-blue">
                        <h4><i class="fa fa-sign-in"></i>PASS - Re-assignment Quote</h4>
                        <div id="divLogo" class="LogoCSS">
                            <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                        </div>
                    </div>
                    <div class="form-body theme-blue" style="padding-top: 16px;">
                        <div class="frm-row">
                            <div class="section colm colm12">
                                <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td class="tdbkg">Quote Number: 
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtSearchQuoteNumber" runat="server"></obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutButton ID="btnSearchQuoteNumber" runat="server" Text="Search" OnClick="btnSearchQuoteNumber_Click"></obout:OboutButton>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <asp:GridView ID="QuoteGridView" runat="server" AutoGenerateColumns="false" ItemType="ProjectPASS.QuoteDetails"
                                    SelectMethod="QuoteGridView_GetData"
                                    DataKeyNames="QuoteNumber" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header"
                                    RowStyle-CssClass="rows" AllowPaging="true" OnPageIndexChanging="QuoteGridView_PageIndexChanging" PageSize="10"
                                    OnRowCommand="QuoteGridView_RowCommand" OnRowDataBound="QuoteGridView_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeader" CssClass="chkHeader" onclick="SelectAllCheckboxes(this)" runat="server" />
                                                <%--<asp:CheckBox ID="CheckBox1" CssClass="chkHeader" runat="server" />--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" CssClass="chkRow" name="chkRow" runat="server" onclick="ManageHeaderCheckBox()" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="QuoteNumber" HeaderText="Quote Number" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="QuoteVersion" HeaderText="Version" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="TotalPremium" HeaderText="Premium" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="QuoteDate" HeaderText="Quote Date" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="Make" HeaderText="Make" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="Model" HeaderText="Model" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="Variant" HeaderText="Variant" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="BusinessType" HeaderText="Business Type" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:BoundField DataField="CustomerType" HeaderText="Customer Type" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                        <asp:TemplateField HeaderText="Assigned To" SortExpression="Visible">
                                            <ItemTemplate>
                                                <obout:OboutTextBox ID="txtAssignedTo" runat="server" Text="" ValidationGroup="vgAssign"></obout:OboutTextBox>
                                                <asp:HiddenField ID="hdnAssignedTo" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remark" SortExpression="Visible">
                                            <ItemTemplate>
                                                <obout:OboutTextBox ID="txtRemark" runat="server" Text="" ValidationGroup="vgAssign"></obout:OboutTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Proposal Number" Visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ID="hypReviewConfirmLink" Text='<%#Eval("ProposalNumber") %>' NavigateUrl='<%#Eval("ReviewAndConfirmLink") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" Visible="false" />
                                        <asp:TemplateField HeaderText="Finalize Quote" Visible="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnProposalNumber" Value='<%#Eval("ProposalNumber") %>' runat="server" />
                                                <asp:HiddenField ID="hdnIsProposalExistsForQuoteNumber" Value='<%#Eval("IsProposalExistsForQuoteNumber") %>' runat="server" />
                                                <asp:LinkButton runat="server" ID="lnkRecalculate" CommandArgument='<%#Eval("QuoteNumber") %>'
                                                    OnClientClick="return confirm('final premium may not be same as existing total premium amount, Are you sure you want to re-calculate?');"
                                                    CommandName="recalculate" Visible="false">Finalize</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modify" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkModifyQuote" CommandArgument='<%#Eval("QuoteNumber") %>' CommandName="modifyquote">Modify</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <table id="table2" style="width: 100%;" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td>
                                            <obout:OboutButton ID="OboutAssignQuote" runat="server" Text="Assign" ValidationGroup="vgAssign" OnClick="OboutAssignQuote_Click"></obout:OboutButton>
                                        </td>
                                    </tr>
                                </table>
                                <br />
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
            <%-- Div for Quote Gridview end here--%>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
        <ProgressTemplate>
            <div id="resultLoading">
                <div>
                    <img alt="" src="Images/ajax-loader.gif"><div>Loading...Please Wait</div>
                </div>
                <div class="bg"></div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>


