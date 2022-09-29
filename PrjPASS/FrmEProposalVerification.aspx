<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmEProposalVerification.aspx.cs" Inherits="PrjPASS.FrmEProposalVerification" %>

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
            //$("#divLogo").css('top', '-3px');
            //$("#decorative2").css('height', '1px');





            $(document).on("input", "input:file", function (e) {
                let fileName = e.target.files[0].name;
                var ext = fileName.split('.').pop().toLowerCase();
                if ($.inArray(ext, ['pdf']) == -1) {
                    alert('Please upload only pdf format files.');
                    $("#<%=FileUploadBulkScanPhysicalForm.ClientID%>").val('');
                    return false;
                }
            });
        });


        //var formSubmitting = false;
        //var setFormSubmitting = function () { formSubmitting = true; };

        //var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);


        //window.onload = function () {
        //    window.addEventListener("beforeunload", function (e) {
        //        if (formSubmitting) {
        //            formSubmitting = false;
        //            return false;
        //        }

        //        if (isChrome) {
        //            var confirmationMessage = '';
        //        }
        //        else {
        //            var confirmationMessage = 'Are you sure you want to leave this page?';
        //        }

        //        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
        //        return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.

        //    });
        //};




       <%-- function Reset() {

            document.getElementById('<%=lblSystemIDV.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblFinalIDV.ClientID%>').innerText = '0.00';

            document.getElementById('<%=lblBasicTPPremium.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblOwnDamagePremium.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblConsumableCover.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblDepreciationCover.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblVoluntaryDeductionforDepWaiver.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblElectronicSI.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblNonElectronicSI.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblExternalBiFuelSI.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblEngineProtect.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblReturnToInvoice.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblRSA.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblLiabilityForBiFuel.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblPAForUnnamedPassengerSI.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblPAForNamedPassengerSI.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblPAToPaidDriverSI.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblPACoverForOwnerDriver.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblLegalLiabilityToPaidDriverNo.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblLLEOPDCC.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblVoluntaryDeduction.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblNCB.ClientID%>').innerText = '0.00';

            document.getElementById('<%=lblNetPremium.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblServiceTax.ClientID%>').innerText = '0.00';
            document.getElementById('<%=lblTotalPremium.ClientID%>').innerText = '0.00';

            var PolicyHolderType = $('#<%=drpPolicyHolderType.ClientID %> option:selected').text();
            var SelectedTenureOwnerDriver = $("#<%=drpTenureOwnerDriver.ClientID%> option:selected").val();
            if (SelectedTenureOwnerDriver == "0" && PolicyHolderType == "Individual Owner") {
                var IsCalculate = confirm("PA Cover For Owner Driver is a mandatory cover & can only be deleted if the Insured already has 24 hour PA cover of 15 Lakhs or above that coincides with the period of Insurance of this policy. Please confirm");
                if (IsCalculate) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }--%>
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

        #table23 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }

        #table3 td {
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

        #table25 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table26 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color: black;
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


        table > tbody > tr > th {
            height: 20px;
            font-weight: bold;
            background-color: #a5a8ab;
        }
    </style>



    <script type="text/javascript">

        //CR507 START
        var IsDisable3Plus3 = false;
        var d_CurrentDate = new Date();
        var d_LongTermProductDisableDate = new Date();
        var LongTermProductDisableDate = '08/01/2020'; //01-Aug-2020
        d_LongTermProductDisableDate = new Date(LongTermProductDisableDate);
        if (d_CurrentDate > d_LongTermProductDisableDate) {
            IsDisable3Plus3 = true; //01-Aug-2020 onwards 3+3 option should not be visible
        }
        //CR507 END

        $(function () {
            SetAutoComplete();


        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);

        });

        function openModal() {
            $('#myModal').modal('show');
        }

        function openModalSuccess() {
            $('#myModalSuccess').modal('show');
        }

        function openModalViewPremium() {
            $('#modalViewPremium').modal('show');
        }


        function CloseModalViewPremium() {
            $('#modalViewPremium').modal('hide');
        }



        function SetAutoComplete() {


            $("[id$=txtIntermediaryName]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmEProposalVerification.aspx/GetIntermediaryCode") %>',
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
                    var strItems2 = i.item.label.split("~");
                    $("[id$=txtIntermediaryName]").val(strItems2[0]);
                    $("[id$=hfIntermediaryCode]").val(i.item.val);
                    $("[id*=btnGetIntermediaryCode]").click();
                },
                minLength: 3,
                autoFocus: true
            });


        }



       <%-- function setbranchlocation(IMDCode) {
            debugger;
            //$("[id$=txtbranchlocation]").autocomplete({
            //    source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmEProposalVerification.aspx/GetBranchLocation") %>',
                        data: "{ 'prefix': '" + IMDCode+ "'}",
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
            //    },
            //    select: function (e, i) {
            //        var strItems2 = i.item.label.split("~");
            //        $("[id$=txtbranchlocation]").val(strItems2[0]);
            //        //$("[id$=hfIntermediaryCode]").val(i.item.val);
            //        ////$("[id*=btnGetIntermediaryCode]").click();

            //    },
            //    minLength: 3,
            //    autoFocus: true
            //});


        }--%>


        function ShowProgress() {
            document.getElementById('<% Response.Write(PageUpdateProgress.ClientID); %>').style.display = "block";
        }

        function HideProgress() {
            document.getElementById('<% Response.Write(PageUpdateProgress.ClientID); %>').style.display = "none";
            $('#myModalSuccess').modal('show');
        }

    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MstCntFormContent" runat="server">




    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:HiddenField ID="hdnQuoteNumber" runat="server" Value="" />
    <asp:HiddenField ID="hdnQuoteVersion" runat="server" Value="0" />

    <asp:HiddenField ID="hdnEnabledAddOnsName" runat="server" Value="" />
    <asp:HiddenField ID="hdnRTOZone" runat="server" Value="Zone-A" />

    <div class="modal fade" id="myModalSuccess" role="dialog" data-backdrop="static">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header alert alert-info fade in">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Status</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblStatusSuccess" runat="server" Style="color: red" />
                </div>
                <!-- Modal footer-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <div class="modal fade" id="myModal" role="dialog" data-backdrop="static">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header alert alert-info fade in">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Status</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblstatus" runat="server" Style="color: red" />
                </div>
                <!-- Modal footer-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4" id="divSmartContainer" style="margin-top: -115px; margin-bottom: 16px">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS  E-Proposal Verification</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                </div>
            </div>
            <div class="form-body theme-blue" style="padding-top: 16px;">
                <div class="frm-row">
                    <div class="section colm colm12">


                        <at1:Accordion
                            ID="MyAccordion"
                            runat="Server"
                            SelectedIndex="0"
                            HeaderCssClass="accordionHeader"
                            HeaderSelectedCssClass="accordionHeaderSelected"
                            ContentCssClass="accordionContent"
                            AutoSize="None"
                            FadeTransitions="true"
                            TransitionDuration="10"
                            FramesPerSecond="300"
                            RequireOpenedPane="false"
                            SuppressHeaderPostbacks="true">

                            <Panes>
                                <at1:AccordionPane ID="accVD" runat="server" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected"
                                    ContentCssClass="accordionContent">
                                    <Header>Proposal Details</Header>
                                    <Content>
                                        <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td class="tdbkg">Customer Name</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtClientName" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                </td>
                                                <td class="tdbkg">Customer Address</td>
                                                <td colspan="3">
                                                    <obout:OboutTextBox ID="txtaddress" runat="server" Text="" MaxLength="500"></obout:OboutTextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="tdbkg">Customer Mobile No</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtmobile" runat="server" Text="" MaxLength="10"></obout:OboutTextBox>
                                                </td>
                                                <td class="tdbkg">Customer Email ID</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtemail" runat="server" Text="" MaxLength="100"></obout:OboutTextBox>
                                                </td>
                                                <td class="tdbkg">Product</td>
                                                <td>
                                                    <asp:DropDownList CssClass="drp" ID="drpproduct" runat="server" AllowEdit="false">
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="tdbkg">Sum Insured Opted</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtsuminsured" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                </td>
                                                <td class="tdbkg">Intermediary ID</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtIntermediaryName" runat="server" Text=""></obout:OboutTextBox>
                                                    <asp:HiddenField ID="hfIntermediaryCode" runat="server" Value="" />
                                                    <asp:Button ID="btnGetIntermediaryCode" runat="server" OnClick="btnGetIntermediaryCode_Click" />
                                                </td>
                                                <td class="tdbkg">KGI Branch Location</td>
                                                <td>
                                                    <asp:DropDownList CssClass="drp" runat="server" ID="drpbranchlocation"></asp:DropDownList>

                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="tdbkg">Premium</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtpremiumamt" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                </td>
                                                <td class="tdbkg">Physical Proposal No</td>
                                                <td>
                                                    <obout:OboutTextBox ID="txtproposalno" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="tdbkg">Upload Scanned Physical Document</td>
                                                <td class="tdbkg">
                                                    <asp:FileUpload ID="FileUploadBulkScanPhysicalForm" runat="server" />
                                                    <asp:HiddenField ID="Hddnfilename" runat="server" Value="" />
                                                    <asp:HiddenField ID="Hddndirectorypath" runat="server" Value="" />



                                                </td>
                                                <td class="tdbkg" colspan="1" align="right">
                                                    <obout:OboutButton ID="btnCreateScanPhysicalForm" runat="server" Text="Upload Document" OnClientClick="return ShowProgress();" OnClick="btnCreatePhysicalDocument_Click"></obout:OboutButton>
                                                    <%--OnClick="btnCreateInvoiceBulkByScheduler_Click" --%>
                                                </td>
                                            </tr>
                                        </table>

                                    </Content>
                                </at1:AccordionPane>
                                <at1:AccordionPane ID="accIDV" runat="server" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected"
                                    ContentCssClass="accordionContent">
                                    <Header>E-Proposal Verification Details</Header>
                                    <Content>
                                        <table id="table8" style="width: 100%;" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td class="tdbkgHead" colspan="6" style="font-size: 15px">Search Reference No
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdbkg">Search Transaction
                                                            <obout:OboutTextBox ID="txtSearchReferenceNo" runat="server"></obout:OboutTextBox>
                                                    <obout:OboutButton ID="btnSearchReference" runat="server" Text="Search Reference" OnClick="btnSearchReference_Click"></obout:OboutButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdbkg">
                                                    <asp:GridView ID="EprposalGridView" runat="server" AutoGenerateColumns="false" SelectMethod="EprposalGridView_GetData" ItemType="ProjectPASS.EproposalDetails"
                                                        DataKeyNames="ReferenceNo" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows" AllowPaging="true" OnPageIndexChanging="EproposalGridView_PageIndexChanging" PageSize="10">
                                                        <Columns>

                                                            <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <%--HeaderStyle-Background-Color="#a5a8ab" />--%>
                                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="CustomerMobile" HeaderText="Customer Mobile No" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="CustomerEmail" HeaderText="Customer Email" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="Product" HeaderText="Product" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="SumInsuredAmt" HeaderText="Sum Insured Opted" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="IMDCODE" HeaderText="IMD Code" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="BranchLocationName" HeaderText="Branch Location Name" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="PremiumAmt" HeaderText="Premium Amount" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="ProposalNo" HeaderText="Proposal No" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="RowCreatedOn" HeaderText="Created On" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="RowCreatedBy" HeaderText="Created by" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                                            <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <a href='<%# Eval("FilePath")%>' target="_blank">Download</a>
<%--                                                                    <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FileName")%>' runat="server" OnClick="DownloadPDFFile" PostBackUrl="" ></asp:LinkButton>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>

                                    </Content>
                                </at1:AccordionPane>

                            </Panes>
                            <HeaderTemplate></HeaderTemplate>
                            <ContentTemplate></ContentTemplate>
                        </at1:Accordion>

                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" BorderColor="#727272" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 40%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnsubmit" runat="server" Text="Submit" Width="100%" OnClick="btnsubmit_Click" />
                            </div>

                            <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton><%----%>
                            </div>
                        </asp:Panel>
                    </div>



                </div>









            </div>
        </div>
    </div>
    <%--  </div>--%>
    <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsubmit" />
            <asp:PostBackTrigger ControlID="btnExit" />
            <asp:PostBackTrigger ControlID="btnCreateScanPhysicalForm" />
        </Triggers>
    </asp:UpdatePanel>--%>

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








