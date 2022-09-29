<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmFinalizeQuotations.aspx.cs" Inherits="PrjPASS.FrmFinalizeQuotations" %>

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

            //$("#btnCloseSuccessProposalPopup").click(function () {

            //    //var optReviewConfirmLink = $('input[id*=optReviewConfirmLink]').is(":checked");
            //    //var optPayULink = $('input[id*=optPayULink]').is(":checked");
                
            //    var isConfirm = confirm('You have not sent the payment link or review confirm link on email yet, you can do this by clicking on the send button. If you close this window then Email will not be sent. Do you really want to close this window?');
            //    if (isConfirm) {
            //        $('#modalSaveProposalSuccess').modal('hide');
            //        return true;
            //    }
            //    else {
            //        return false;
            //    }
            //});
        });
        

        var formSubmitting = false;
        var setFormSubmitting = function () { formSubmitting = true; };

        var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
        

        window.onload = function () {
            window.addEventListener("beforeunload", function (e) {
                if (formSubmitting) {
                    formSubmitting = false;
                    return false;
                }

                if (isChrome) {
                    var confirmationMessage = '';
                }
                else {
                    var confirmationMessage = 'Are you sure you want to leave this page?';
                }

                (e || window.event).returnValue = confirmationMessage; //Gecko + IE
                return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.

            });
        };

        //function test() {
        //    var popup = $find('');
        //    popup.add_shown(SetzIndex);
        //}


        function Reset() {

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
        }

    </script>
<script language="Javascript" type="text/javascript">

        function AvoidSpace(event) {
    var k = event ? event.which : window.event.keyCode;
    if (k == 32) return false;
}
function allowAlphaNumericSpace(thisInput) {
  thisInput.value = thisInput.value.split(/[^0-9 ]/).join('');
  //thisInput.value = thisInput.value.split(/[^a-zA-Z0-9 ]/).join('');
  
}
function allowAlpha(thisInput) {
  thisInput.value = thisInput.value.split(/[^a-zA-Z ]/).join('');
  
}
    </script> 
    <style type="text/css">
        .ui-autocomplete{
                z-index: 1051;
        }
        .ui-autocomplete.ui-front{
                z-index: 1051;
        }

        .ui-autocomplete {
        z-index: 99999999 !important;
        }

        #table1 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table21 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table22 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table24 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table3 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

         #table23 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

        #table13 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table4 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

        #table5 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
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

        #table26 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color : black;
        }

        #table25 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color : black;
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
            text-align:center;
        }

        .mydatagrid th {
            padding: 3px;
            border: 1px solid black;
            text-align:center;
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
            chkFinancierChange();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ShowHideCustomerRows);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(chkFinancierChange);
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
         
            $("[id$=txtFinancierName]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmFinalizeQuotations.aspx/GetFinacier") %>',
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
                    $("[id$=txtFinancierName]").val(strItems2[1]);
                    $("[id$=hdnFinancierCode]").val(strItems2[0]);
                    $("[id$=hdnFinancierName]").val(strItems2[1]);
                    $("[id*=btnFinancierName]").click();
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

            var d_currDate = new Date();
            var d_minDate = new Date();
            var d_maxDate = new Date();
            d_minDate.setDate(d_currDate.getDate() - 30);
            d_maxDate.setDate(d_currDate.getDate() + 60);

            $("[id$=txtPolicyStartDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'images/calendar.png',
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: d_minDate,
                maxDate: d_maxDate
            });

            $("[id$=txtDateofBirth]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/1940',
                yearRange: "-78:-18"
            });

            $("#datepickerImagedob").click(function () {
                $("[id$=txtDateofBirth]").datepicker("show");
            });

            $("[id$=txtNomineeDOB]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/1940',
                yearRange: "1940:" + new Date().getFullYear().toString()
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

            $("[id*=QuoteGridView] [id*=lnkRecalculate]").click(function () {
                //Find the GridView Row using the LinkButton reference.
                var row = $(this).closest("tr");

                //Find the TextBox control.
                var txtPolicyStartDate = row.find("[id*=txtPolicyStartDate]");
                var lblQuoteDate = row.find("[id*=lblQuoteDate]");
               
                var array_QuoteDate = lblQuoteDate.text().split("/");
                var array_PolicyStartDate = txtPolicyStartDate.val().split("/");
                

                var d_QD = new Date(array_QuoteDate[2].split(" ")[0], parseInt(array_QuoteDate[1]) - 1, array_QuoteDate[0]);
                var d_PSD = new Date(array_PolicyStartDate[2], parseInt(array_PolicyStartDate[1]) - 1, array_PolicyStartDate[0]);
                

                //As discuss with sanjay sir / avni: Not Applying validation hence commmneted
                //if (d_PSD < d_QD) {
                //    alert("Policy Start Date Cannot Be Less Than Quote Created Date");
                //    return false;
                //}
                
                var r = confirm("final premium may not be same as existing total premium amount, Are you sure you want to re-calculate?");
                return r;
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

        function chkFinancierChange()
        {
            if ($('input[id*=chkFinancier]').is(':checked') == true)
            {
                $('input[id*=txtFinacierAgrrementType]').val('Hypothecation');
                $('input[id*=txtFinacierAgrrementType]').attr("disabled", "disabled");
            }
            else if ($(this).is(":not(:checked)") == false)
            {
                $('input[id*=txtFinacierAgrrementType]').val('');
                $('input[id*=txtFinancierName]').val('');
                $('input[id*=txtFinancierAddress]').val('');
                $('input[id*=txtLoanAccountNumber]').val('');
                $('input[id*=txtFileNumber]').val('');
                $('input[id*=txtFinacierAgrrementType]').removeAttr("disabled");
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MstCntFormContent" runat="server">

   
     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnQuoteVersion" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMaxQuoteVersion" runat="server" Value="0" />
            
            <asp:HiddenField ID="hdnCreditScoreId" Value="0" runat="server" />
            <asp:HiddenField ID="hdnIsFastlaneFlow" Value="0" runat="server" />

            <asp:HiddenField ID="hdnIRDAProductCode" Value="" runat="server" />
            <asp:HiddenField ID="hdnTenureOwnerDriver" Value="" runat="server" />

            <div class="modal fade" id="modalSaveProposalSuccess" role="dialog" data-backdrop="static" style="margin-top:-17px">
                        <div class="modal-dialog" style="width:80%;">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                                    <h4 class="modal-title">Save Proposal Successfull</h4>
                                </div>
                                <div class="modal-body">
                                    
                                  <div style="float:left;width: 600px;text-align: left;"> 
                                       <asp:Label ID="lblStatusSaveProposalSuccess" runat="server" style="color:#054271"></asp:Label>
                                    </div>
                                    <br />
                                    <table id="table24" style="width: 100%; text-align: left;" cellspacing="0" cellpadding="2" border="1" valign="top">
                                           <tr style="text-align: left; background-color: gray; color: white;">
                                                <td style="color:white" colspan="3">Customer Details</td>
                                           </tr>
                                            <tr style="background-color:rgba(128, 128, 128, 0.18);font-weight:bold;">
                                                <td>CustomerId</td>
                                                <td>Customer Full Name</td>
                                                <td>Customer Type</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCustomerId" runat="server">-</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustomerFullName" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustomerType" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        <tr style="text-align: left; background-color: gray; color: white;">
                                                <td style="color:white" colspan="3">Proposal Details</td>
                                           </tr>
                                            <tr style="background-color:rgba(128, 128, 128, 0.18);font-weight:bold;">
                                                <td>Quote Number</td>
                                                <td>Proposal Number</td>
                                                <td>Total Premium</td>
                                            </tr>
                                           <tr>
                                                <td>
                                                    <asp:Label ID="lblQuoteNum" runat="server">-</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblProposalNumber" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotalPremiumAmount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        <tr style="text-align: left; background-color: gray; color: white;">
                                                <td style="color:white" colspan="3">Vehicle Details</td>
                                           </tr>
                                            <tr style="background-color:rgba(128, 128, 128, 0.18);font-weight:bold;">
                                                <td>Make</td>
                                                <td>Model</td>
                                                <td>Variant</td>
                                            </tr>
                                           <tr>
                                                <td>
                                                    <asp:Label ID="lblMake2" runat="server">-</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblModel2" runat="server"></asp:Label>    
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVariant2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                         <tr style="background-color:rgba(128, 128, 128, 0.18);font-weight:bold;">
                                                <td>Ragistration Number</td>
                                                <td>Engine Number</td>
                                                <td>Chassis Number</td>
                                            </tr>
                                           <tr>
                                                <td>
                                                    <asp:Label ID="lblRegistrationNumber" runat="server">-</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEngineNumber" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblChassisNumber" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                         <tr style="text-align: left; background-color: gray; color: white;" id="trOptionButtonRowForLinkSendingOptions" runat="server">
                                                <td style="color:white" colspan="3">
                                                    <obout:OboutRadioButton ID="optReviewConfirmLink" runat="server" GroupName="grpTypeOfLinks" Text="Send Quote PDF and Review Confirm Link to the Customer" Checked="true">
                                                    </obout:OboutRadioButton>
                                                    <obout:OboutRadioButton ID="optPayULink" Visible="false"  runat="server" GroupName="grpTypeOfLinks" Text="Send Quote PDF and Digital Payment Link to the Customer">
                                                    </obout:OboutRadioButton>
                                                     <%--Removed this Radiobutton optPayULink As CR Number P1_124    --%>
                                                        
                                                </td>
                                           </tr>
                                        <tr id="trEmailIdAndRemarksRow" runat="server">
                                            <td colspan="3">
                                            <asp:TextBox ID="txtEmailId" placeholder="Please Enter Emailid" runat="server" Text="" BorderStyle="Solid" style="border:1px solid" Width="250px" Height="25px" Enabled="false"></asp:TextBox>
                                            <asp:TextBox ID="txtRemarks" placeholder="Enter Remarks If Any" runat="server" Text="" style="border:1px solid" Width="250px" Height="25px"></asp:TextBox>
                                            <asp:Button ID="btnSendPDF" class="btn btn-info" runat="server" Text="Send Link" OnClick="btnSendPDF_Click" />
                                            <button type="button" class="btn btn-primary" data-dismiss="modal" id="btnCloseSuccessProposalPopup" name="btnCloseSuccessProposalPopup">Don't Send Link</button>
                                                </td>
                                        </tr>
                                    </table>
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer" id="modalProposalSuccessFooter" runat="server" visible="false">
                                <asp:Label ID="lblWarningTextForProposalSuccessPopUp" runat="server" Text=""></asp:Label>    
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>

            <div class="modal fade" id="modalSaveProposal" role="dialog" data-backdrop="static" style="margin-top:-17px">
                        <div class="modal-dialog" style="width:80%;">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Save Proposal</h4>
                                </div>
                                <div class="modal-body">
                                    
                                  <at1:Accordion
                                    ID="Accordion1"
                                    runat="Server"
                                    SelectedIndex="0"
                                    HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected"
                                    ContentCssClass="accordionContent"
                                    AutoSize="None"
                                    FadeTransitions="true"
                                    TransitionDuration="10"
                                    FramesPerSecond="300"
                                    RequireOpenedPane="true"
                                    SuppressHeaderPostbacks="true">
                                    <Panes>
                                        <at1:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Customer Details</Header>
                                            <Content>

                                                <table id="table21" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                       <td class="tdbkg" colspan="6">
                                                           <obout:OboutRadioButton onclick="EnableDisableCustomerInputFields();" GroupName="customerType" ID="rbtNewCustomer" Text="New Customer" runat="server" Checked="true"></obout:OboutRadioButton>
                                                        </td>
                                                    </tr>
                                                      <tr>
                                                       <td class="tdbkg">Customer Type
                                                        </td>
                                                        <td>
                                                           <asp:Label ID="lblSelectedCustomerType" runat="server"></asp:Label>
                                                            <obout:OboutRadioButton onclick="ShowHideCustomerRows();" GroupName="cusType" ID="rbtIndividual" Text="Individual" runat="server" Enabled="false"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton onclick="ShowHideCustomerRows();" GroupName="cusType" ID="rbtOrganization" Text="Organization" runat="server" Enabled="false"></obout:OboutRadioButton>
                                                            <div style="display:none">
                                                                <obout:OboutRadioButton GroupName="BTType" ID="rbtIsRollover" Text="IsRollover" runat="server" Enabled="false"></obout:OboutRadioButton>
                                                            </div>
                                                        </td>

                                                         <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                          
                                                        </td>

                                                        
                                                         <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                           
                                                        </td>
                                                    </tr>
                                                     <tr id="trCustomerRow2">
                                                       

                                                         <td class="tdbkg">Title
                                                        </td>
                                                        <td>
                                                           <asp:DropDownList ID="drpCustomerTitle" runat="server">
                                                               <asp:ListItem Selected="True" Value="Mr">Mr</asp:ListItem>
                                                               <asp:ListItem Value="Mrs">Mrs</asp:ListItem>
                                                               <asp:ListItem Value="Miss">Miss</asp:ListItem>
                                                               <asp:ListItem Value="Major">Major</asp:ListItem>
                                                               <asp:ListItem Value="Dr">Dr</asp:ListItem>
                                                           </asp:DropDownList>
                                                        </td>

                                                        <td class="tdbkg">Gender
                                                        </td>
                                                        <td>
                                                           <obout:OboutRadioButton GroupName="genderType" ID="rbtMale" Text="Male" runat="server" Checked="true" Enabled="false"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton GroupName="genderType" ID="rbtFemale" Text="Female" runat="server" Enabled="false"></obout:OboutRadioButton>
                                                        </td>
                                                         <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                          
                                                        </td>
                                                    </tr>
                                                    <tr id="trCustomerRow1">
                                                       <td class="tdbkg">First Name
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtFirstName" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Middle Name
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtMiddleName" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>

                                                        
                                                         <td class="tdbkg">Last Name
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtLastName" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>
                                                   
                                                      <tr id="trCustomerRow3">
                                                       <td class="tdbkg">Date of Birth
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtDateofBirth" runat="server" ReadOnly="false"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagedob" />
                                                        </td>

                                                         <td class="tdbkg">Email Address
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtEmailAddress" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                        </td>

                                                        
                                                         <td class="tdbkg">Mobile Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtMobileNumber" runat="server" Text="" MaxLength="10"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>
                                                     <tr id="trCustomerRow4">
                                                       <td class="tdbkg">Contact Person
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtContactPerson" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Organization Name
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtOrganizationName" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                        </td>

                                                        
                                                         <td class="tdbkg">Mobile Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtMobileNumberOrz" runat="server" Text="" MaxLength="10"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>
                                                     <tr id="trCustomerRow5">
                                                       <td class="tdbkg">Email Address
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtEmailIdOrz" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                           
                                                        </td>

                                                        
                                                         <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                          
                                                        </td>
                                                    </tr>

                                                    <tr id="trCustomerRow6">
                                                       <td class="tdbkg">Pan Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtPanNumber" runat="server" Text="" MaxLength="10"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Pincode
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtPinCode" runat="server" MaxLength="10"></obout:OboutTextBox>
                                                             <asp:HiddenField ID="hdnPinCode" runat="server" Value="0" />
                                                             <asp:HiddenField ID="hdnPinCodeLocality" runat="server" Value="" />
                                                            <asp:Button ID="btnGetPincodeDetails" runat="server" OnClick="btnGetPincodeDetails_Click" />
                                                        </td>
                                                         <td class="tdbkg">PincodeLocality
                                                        </td>
                                                        <td>
                                                          <asp:Label ID="lblPincodeLocality" runat="server" Text="-"></asp:Label>
                                                        </td>
                                                    </tr>
                                                     <tr id="trCustomerRow7">
                                                         <td class="tdbkg">State
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblStateName" runat="server" Text="-"></asp:Label>
                                                            <asp:HiddenField ID="hdnStateId" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnCreditScoreStateId" runat="server" Value="0" />
                                                        </td>

                                                           <td class="tdbkg">City 
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCityName" runat="server" Text="-"></asp:Label>
                                                            <asp:HiddenField ID="hdnCityId" runat="server" Value="0" />
                                                        </td>
                                                        </td>

                                                          <td class="tdbkg">District
                                                        </td>
                                                        <td>
                                                           <asp:Label ID="lblDistrictName" runat="server" Text="-"></asp:Label>
                                                            <asp:HiddenField ID="hdnDistrictId" runat="server" Value="0" />
                                                        </td>
                                                     </tr>
                                                      <tr id="trCustomerRow8">
                                                          <td class="tdbkg">Address Line 1
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtAddressLine1" runat="server" Text="" MaxLength="100"></obout:OboutTextBox>
                                                        </td>
                                                          <td class="tdbkg">Address Line 2
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtAddressLine2" runat="server" Text="" MaxLength="100"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">Address Line 3
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtAddressLine3" runat="server" Text="" MaxLength="100"></obout:OboutTextBox>
                                                        </td>
                                                      </tr>
                                                      <tr id="trCustomerRow9" runat="server">
                                                          <td class="tdbkg">Marital Status
                                                        </td>
                                                        <td>
                                                            <obout:OboutDropDownList ID="ddlMaritalStatus" runat="server">
                                                                <asp:ListItem Value="--Select--" Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="Single" Text="Single"></asp:ListItem>
                                                                <asp:ListItem Value="Married" Text="Married"></asp:ListItem>
                                                                <asp:ListItem Value="Divorced" Text="Divorced"></asp:ListItem>
                                                                <asp:ListItem Value="Widowed" Text="Widowed"></asp:ListItem>
                                                                <asp:ListItem Value="Others" Text="Others"></asp:ListItem>
                                                            </obout:OboutDropDownList>
                                                        </td>
                                                          <td class="tdbkg" style="display:none;">Adhar UID
                                                        </td>
                                                        <td style="display:none;">
                                                            <obout:OboutTextBox ID="txtUID" runat="server"  MaxLength="12"></obout:OboutTextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                          <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                      </tr>

                                                     <tr style="display:none">
                                                       <td class="tdbkg" colspan="6">
                                                           <obout:OboutRadioButton onclick="EnableDisableCustomerInputFields();" GroupName="customerType" ID="rbtExistingCustomer" Text="Existing Customer" runat="server"></obout:OboutRadioButton>
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr style="display:none">
                                                        <td class="tdbkg">
                                                            Customer Id
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtCustomerId" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">
                                                            Customer Full Name
                                                        </td>
                                                        <td colspan="3">
                                                           <obout:OboutTextBox ID="txtCustomerFullName" runat="server" Text="" MaxLength="100"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>
                                        <at1:AccordionPane ID="AccordionPane2" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Vehiche Other Details</Header>
                                            <Content>

                                                <table id="table22" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                       <td class="tdbkg">Registration Number
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtRN1" runat="server" Text="" MaxLength="3" Width="45px" Enabled="false"></obout:OboutTextBox>
                                                            <obout:OboutTextBox ID="txtRN2" runat="server" Text="" MaxLength="3" Width="40px" Enabled="false"></obout:OboutTextBox>
                                                            <obout:OboutTextBox ID="txtRN3" runat="server" Text="" MaxLength="3" Width="40px" onkeyup="allowAlpha(this)" onkeypress="return AvoidSpace(event)"></obout:OboutTextBox>
                                                            <obout:OboutTextBox ID="txtRN4" runat="server" Text="" MaxLength="4" Width="60px" onkeyup="allowAlphaNumericSpace(this)" onkeypress="return AvoidSpace(event)"></obout:OboutTextBox>
                                                        </td>

                                                        <td class="tdbkg">Engine Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtEngineNumber" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Chassis Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtChassisNumber" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>

                                                       
                                                    </tr>
                                                    <tr>
                                                        <td class="tdbkg">Partner Application Number
                                                        </td>
                                                         <td>
                                                             <obout:OboutTextBox ID="txtPartnerApplicationNumber" runat="server" MaxLength="25"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">Type Of Transfer
                                                        </td>
                                                        <td>
                                                             <asp:DropDownList ID="drpTypeOfTransfer" runat="server" Width="200px" CssClass="drp">
                                                                <asp:ListItem Value="Internet">Internet</asp:ListItem>
                                                                <asp:ListItem Value="GENISYS">GENISYS</asp:ListItem>
                                                           </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trPreviousInsurerRow">
                                                       <td class="tdbkg">Previous Insurer Name
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpPreviousInsurerName" runat="server" Width="200px" CssClass="drp">
                                                                <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="LVGI">LIBERTY VIDEOCON GENERAL INSURANCE COMPANY LTD.</asp:ListItem>
                                                                <asp:ListItem Value="AMHI">Apollo Munich (AMHI)</asp:ListItem>
                                                                <asp:ListItem Value="GIF">Directorate of Insurance (GIF), Govt. of Maha</asp:ListItem>
                                                                <asp:ListItem Value="ENDURANCE">Endurance</asp:ListItem>
                                                                <asp:ListItem Value="GICL">General Insurance Corporation of India</asp:ListItem>
                                                                <asp:ListItem Value="L&T GICL">LT General Insurance Co. Ltd.</asp:ListItem>
                                                                <asp:ListItem Value="MAGMA HGICL">Magma HDI General Insurance Co Ltd</asp:ListItem>
                                                                <asp:ListItem Value="MITSUI">Mitsui Insurance</asp:ListItem>
                                                                <asp:ListItem Value="SBI GICL">SBI General Insurance Company Limited</asp:ListItem>
                                                                <asp:ListItem Value="GUJRAT INSURANCE">The Govt. Of Gujarat Insurance Fund</asp:ListItem>
                                                                <asp:ListItem Value="TATAAIG">TATA AIG GENERAL INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="EXPORT CREDIT">EXPORT CREDIT GUARANTEE CORPORATION OF INDIA LTD</asp:ListItem>
                                                                <asp:ListItem Value="USGI">UNIVERSAL SOMPO GENERAL INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="BAJAJ ALLIANZ">BAJAJ ALLIANZ GENERAL INSURANCE CO.LTD</asp:ListItem>
                                                                <asp:ListItem Value="ICICI">ICICI LOMBARD GENERAL INSURANCE CO. LTD.</asp:ListItem>
                                                                <asp:ListItem Value="IFFCO TOKYO">IFFCO TOKIO GENERAL INSURANCE CO. LTD.</asp:ListItem>
                                                                <asp:ListItem Value="OICL">THE ORIENTAL INSURANCE CO. LTD.</asp:ListItem>
                                                                <asp:ListItem Value="RGICL">RELIANCE GENERAL INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="RSAICL">ROYAL SUNDARAM ALLIANCE INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="UIIC">UNITED INDIA INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="CHOLA">CHOLAMANDALAM MS GENERAL INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="HDFC ERGO">HDFC ERGO GENERAL INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="SHAIC">STAR HEALTH AND ALLIED INSURANCE COMPANY LIMITED</asp:ListItem>
                                                                <asp:ListItem Value="ADIC">APOLLO MUNICH HEALTH INSURANCE COMPANY LIMITED</asp:ListItem>
                                                                <asp:ListItem Value="FUTURE GEN">FUTURE GENERALI INDIA INSURANCE COMPANY LIMITED</asp:ListItem>
                                                                <asp:ListItem Value="SGIC">SHRIRAM GENERAL INSURANCE COMPANY LIMITED</asp:ListItem>
                                                                <asp:ListItem Value="BHARTI AXA">BHARTI AXA GENERAL INSURANCE COMPANY LIMITED</asp:ListItem>
                                                                <asp:ListItem Value="RAHEJA">RAHEJA QBE GENERAL INSURANCE COMPANY LIMITED</asp:ListItem>
                                                                <asp:ListItem Value="NICL">NATIONAL INSURANCE CO.LTD.</asp:ListItem>
                                                                <asp:ListItem Value="TNIA">THE NEW INDIA ASSURANCE CO. LTD.</asp:ListItem>
                                                                <asp:ListItem Value="AICI">AGRICULTURE INSURANCE CO. OF INDIA LTD.</asp:ListItem>
                                                                <asp:ListItem Value="GO DIGIT">Go Digit General Insurance Limited</asp:ListItem>
                                                                <asp:ListItem Value="KOTAK">KOTAK GENERAL POLICY</asp:ListItem>
                                                           </asp:DropDownList>
                                                        </td>

                                                        <td class="tdbkg">Previous Policy Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtPreviousPolicyNumber" runat="server" Text="" MaxLength="30"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                           
                                                        </td>

                                                       
                                                    </tr>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>
                                        <at1:AccordionPane ID="AccordionPane3" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Nominee Details</Header>
                                            <Content>

                                                <table id="table23" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                       <td class="tdbkg">Nominee Name
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNomineeName" runat="server" MaxLength="20"></obout:OboutTextBox>
                                                        </td>

                                                        <td class="tdbkg">Nominee Date of Birth
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtNomineeDOB" runat="server" ReadOnly="false" MaxLength="20"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagenomineedob" />
                                                        </td>

                                                         <td class="tdbkg">Relationship
                                                        </td>
                                                        <td>
                                                           <asp:DropDownList CssClass="drp" ID="drpNomineeRelationship" runat="server">
                                                            <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Aunt" Value="Aunt"></asp:ListItem>
                                                            <asp:ListItem Text="Brother" Value="Brother"></asp:ListItem>
                                                            <asp:ListItem Text="Brother-In-law" Value="Brother-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="CHAUFFEUR" Value="CHAUFFEUR"></asp:ListItem>
                                                            <asp:ListItem Text="chaffeur" Value="chaffeur"></asp:ListItem>
                                                            <asp:ListItem Text="Daughter" Value="Daughter"></asp:ListItem>
                                                            <asp:ListItem Text="Daughter-In-law" Value="Daughter-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                                            <asp:ListItem Text="Employer" Value="Employer"></asp:ListItem>
                                                            <asp:ListItem Text="FRIEND" Value="FRIEND"></asp:ListItem>
                                                            <asp:ListItem Text="Father" Value="Father"></asp:ListItem>
                                                            <asp:ListItem Text="Father-In-law" Value="Father-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Fiance" Value="Fiance"></asp:ListItem>
                                                            <asp:ListItem Text="Friend" Value="Friend"></asp:ListItem>
                                                            <asp:ListItem Text="Granddaughter" Value="Granddaughter"></asp:ListItem>
                                                            <asp:ListItem Text="Grandfather" Value="Grandfather"></asp:ListItem>
                                                            <asp:ListItem Text="GrandMother" Value="GrandMother"></asp:ListItem>
                                                            <asp:ListItem Text="Grandson" Value="Grandson"></asp:ListItem>
                                                            <asp:ListItem Text="Husband" Value="Husband"></asp:ListItem>
                                                            <asp:ListItem Text="INSURED (SELF-DRIVING)" Value="INSURED (SELF-DRIVING)"></asp:ListItem>
                                                            <asp:ListItem Text="Insured" Value="Insured"></asp:ListItem>
                                                            <asp:ListItem Text="Insured Estate" Value="Insured Estate"></asp:ListItem>
                                                            <asp:ListItem Text="Mother" Value="Mother"></asp:ListItem>
                                                            <asp:ListItem Text="Mother-In-law" Value="Mother-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Nephew" Value="Nephew"></asp:ListItem>
                                                            <asp:ListItem Text="Niece" Value="Niece"></asp:ListItem>
                                                            <asp:ListItem Text="OTHERS" Value="OTHERS"></asp:ListItem>
                                                            <asp:ListItem Text="Owner" Value="Owner"></asp:ListItem>
                                                            <asp:ListItem Text="Partner" Value="Partner"></asp:ListItem>
                                                            <asp:ListItem Text="RELATIVE" Value="RELATIVE"></asp:ListItem>
                                                            <asp:ListItem Text="Relatives" Value="Relatives"></asp:ListItem>
                                                            <asp:ListItem Text="Self" Value="Self"></asp:ListItem>
                                                            <asp:ListItem Text="Sister" Value="Sister"></asp:ListItem>
                                                            <asp:ListItem Text="Sister-In-law" Value="Sister-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Son" Value="Son"></asp:ListItem>
                                                            <asp:ListItem Text="Son-In-law" Value="Son-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Spouse" Value="Spouse"></asp:ListItem>
                                                            <asp:ListItem Text="Uncle" Value="Uncle"></asp:ListItem>
                                                            <asp:ListItem Text="Wife" Value="Wife"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>

                                                       
                                                    </tr>
                                                     <tr>
                                                       <td class="tdbkg">Name of Appointee
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNameOfAppointee" runat="server" Text="" MaxLength="20"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Relationship with Appointee
                                                        </td>
                                                        <td>
                                                           <asp:DropDownList CssClass="drp" ID="drpRelationshipWithAppointee" runat="server">
                                                            <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Dependent Daughter" Value="Dependent Daughter"></asp:ListItem>
                                                            <asp:ListItem Text="Dependent Son" Value="Dependent Son"></asp:ListItem>
                                                            <asp:ListItem Text="Father" Value="Father"></asp:ListItem>
                                                            <asp:ListItem Text="Father-In-law" Value="Father-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Husband" Value="Husband"></asp:ListItem>
                                                            <asp:ListItem Text="Mother" Value="Mother"></asp:ListItem>
                                                            <asp:ListItem Text="Mother-In-law" Value="Mother-In-law"></asp:ListItem>
                                                            <asp:ListItem Text="Self" Value="Self"></asp:ListItem>
                                                            <asp:ListItem Text="Wife" Value="Wife"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>

                                                       <td class="tdbkg">
                                                        </td>
                                                        <td>
                                                           
                                                        </td>

                                                    </tr>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>
                                        <at1:AccordionPane ID="AccordionPane4" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Hypothecation Details</Header>
                                            <Content>

                                                <table id="table25" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                       <td class="tdbkg">Is Financier details avaialble?
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkFinancier" runat="server" Checked="true" onclick="chkFinancierChange()"> </obout:OboutCheckBox>
                                                        </td>

                                                        <td class="tdbkg">Financier aggreement type.
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtFinacierAgrrementType" runat="server"  MaxLength="50"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Financier Name
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtFinancierName" runat="server"  MaxLength="500"></obout:OboutTextBox>
                                                            <asp:HiddenField ID="hdnFinancierCode" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnFinancierName" runat="server" Value="0" />
                                                            <asp:Button ID="btnFinancierName" runat="server" OnClick="btnFinancierName_Click" />
                                                        </td>

                                                       
                                                    </tr>
                                                     <tr>
                                                       <td class="tdbkg">Financier Address
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtFinancierAddress" runat="server" Text="" MaxLength="200"></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Loan Account Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtLoanAccountNumber" runat="server" Text="" MaxLength="200"></obout:OboutTextBox>
                                                        </td>

                                                       <td class="tdbkg">
                                                           File Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtFileNumber" runat="server" Text="" MaxLength="200"></obout:OboutTextBox>                                                          
                                                        </td>

                                                    </tr>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>
                                        </Panes>
                                    </at1:Accordion>

                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                    <div style="float:left;width: 600px;text-align: left;"> 
                                       <asp:Label ID="lblStatusSaveProposal" runat="server" style="color:#054271"></asp:Label>
                                    </div>
                                    <asp:Button ID="btnSaveProposal" class="btn btn-primary" runat="server" Text="Save Proposal" OnClientClick="CloseModalSaveProposal();" OnClick="btnSaveProposal_Click" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>

             <div class="modal fade" id="myModalSuccess" role="dialog" data-backdrop="static">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Status</h4>
                                </div>
                                <div class="modal-body">
                                     <asp:Label ID="lblStatusSuccess" runat="server" style="color:red" />
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
                                     <asp:Label ID="lblstatus" runat="server" style="color:red" />
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
                        <h4><i class="fa fa-sign-in"></i>PASS - Private Car Finalize Quote</h4>
                        <div id="divLogo" class="LogoCSS">
                         <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                        </div>
                    </div>
                    <div class="form-body theme-blue" style="padding-top: 16px;">
                        <div class="frm-row">
                            <div class="section colm colm12">
                                <%-- <asp:Panel ID="Panel1" runat="server" BorderWidth="0px">
                                    <asp:ValidationSummary CssClass="validationsummary" ValidationGroup="vp" runat="server"
                                        HeaderText="<div class='validationheader'>&nbsp;Please Correct the following Errors:</div>" />
                                    <br />

                                </asp:Panel>--%>

                                
                                <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td class="tdbkg">
                                            Quote Number: 
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtSearchQuoteNumber" runat="server"></obout:OboutTextBox>
                                        </td>
                                        <td class="tdbkg">
                                            From Date: 
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtFromDate" runat="server" ReadOnly="false"></obout:OboutTextBox>
                                            <img src="images/calendar.png" alt="" id="datepickerImageFromDate">
                                            <asp:HiddenField ID="HdnFromDate" Value="" runat="server" />
                                        </td>
                                        <td class="tdbkg">
                                            To Date: 
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtToDate" runat="server" ReadOnly="false"></obout:OboutTextBox>
                                            <img src="images/calendar.png" alt="" id="datepickerImageToDate">
                                        </td>
                                        <td class="tdbkg">
                                            Customer Name: 
                                        </td>
                                        <td>
                                            <obout:OboutTextBox ID="txtCustomerName" runat="server"></obout:OboutTextBox>
                                        </td>
                                        <td>
                                            <obout:OboutButton ID="btnSearchQuoteNumber" runat="server" Text="Search" OnClick="btnSearchQuoteNumber_Click" ></obout:OboutButton>
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
                                             <asp:TemplateField HeaderText="QuoteNumber">
                                             <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkQuoteNumber" CommandArgument='<%#Eval("QuoteNumber") %>' CommandName="QuoteNumber"><%#Eval("QuoteNumber") %></asp:LinkButton>
                                             </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="QuoteVersion" HeaderText="Version" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="TotalPremium" HeaderText="Premium" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <%--<asp:BoundField DataField="QuoteDate" HeaderText="Quote Date" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />--%>
                                            <asp:TemplateField HeaderText = "Quote Date" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuoteDate" runat="server" Text='<%#Eval("QuoteDate", "{0:dd/MM/yyyy HH:mm:ss tt}").Replace("-", "/") %>'></asp:Label>
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                            <%--<asp:BoundField DataField="PolicyStartDate" HeaderText="Policy Start Date" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />--%>
                                              <asp:TemplateField HeaderText = "Policy Start Date" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGVPolicyStartDate" runat="server" Text='<%#Eval("PolicyStartDate") %>'></asp:Label>
                                                    <asp:TextBox ID="txtPolicyStartDate" runat="server" Enabled="false" Text='<%#Eval("PolicyStartDate") %>' style="width: 100px;border: 1px solid black;text-align: center;"></asp:TextBox>
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                            <asp:BoundField DataField="Make" HeaderText="Make" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="Model" HeaderText="Model" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="Variant" HeaderText="Variant" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="BusinessType" HeaderText="Business Type" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <%--<asp:BoundField DataField="CustomerType" HeaderText="Customer Type" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />--%>
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="SourceQuoteCreator" HeaderText="Created By" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <%--<asp:BoundField DataField="ProposalNumber" HeaderText="Proposal Number" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />--%>
                                            <asp:TemplateField HeaderText="Proposal Number">
                                                <ItemTemplate>
                                                     <asp:HyperLink runat="server" ID="hypReviewConfirmLink" Text='<%#Eval("ProposalNumber") %>' NavigateUrl='<%#Eval("ReviewAndConfirmLink") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="PolicyNumber" HeaderText="Policy Number" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="CampaignCode" HeaderText="Campaign Code" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:TemplateField HeaderText="Finalize Quote">
                                             <ItemTemplate>
                                                 <asp:HiddenField ID="hdnProposalNumber" Value='<%#Eval("ProposalNumber") %>' runat="server" />
                                                 <asp:HiddenField ID="hdnIsProposalExistsForQuoteNumber" Value='<%#Eval("IsProposalExistsForQuoteNumber") %>' runat="server" />
                                                 <asp:HiddenField ID="hdnIsAllowPolicyStartDateEdit" Value='<%#Eval("IsAllowPolicyStartDateEdit") %>' runat="server" />
                                                 <asp:LinkButton runat="server" ID="lnkRecalculate" CommandArgument='<%#Eval("QuoteNumber") %>' CommandName="recalculate">Finalize</asp:LinkButton>
                                             </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Modify">
                                             <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkModifyQuote" CommandArgument='<%#Eval("QuoteNumber") %>' CommandName="modifyquote">Modify</asp:LinkButton>
                                             </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                        </Columns>
                                    </asp:GridView>
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

                         <div class="modal fade" id="modalViewPremium" role="dialog" data-backdrop="static" style="margin-top:-17px">
                        <div class="modal-dialog" style="width:80%;">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Premium Breakup</h4>
                                </div>
                                <div class="modal-body">
                                     <asp:Panel ID="pnlTest" runat="server"><div style="display: none"><table style="font-size: 7px" cellspacing="0" cellpadding="2" border="0"><tr style="background-color:gainsboro;color: dodgerblue;">
                                         <td style="font-size:7px">Quote Number</td>
                                         <td>IMD Code</td>
                                         <td style="font-size:9px">Customer Id</td>
                                         <td>Customer Name</td></tr>
                                        <tr>
                                            <td style="font-size:7px"><asp:Label runat="server" ID="lblQuoteNumber" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblIMDCode" Text="-"></asp:Label></td>
                                            <td style="font-size:7px"><asp:Label runat="server" ID="lblPDFCustomerId" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblCreditScoreCustomerName" Text="-"></asp:Label></td>
                                        </tr>
                                         <tr style="background-color:gainsboro;color: dodgerblue;">
                                            <td>Proposal Number</td>
                                            <td>Cover Type</td>
                                            <td>Ownership Type</td>
                                            <td>Policy Holder Type</td> 
                                        </tr>
                                        <tr>                                          
                                            <td><asp:Label runat="server" ID="lblPDFProposalNumber" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblCoverType" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblOwnershipType" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPolicyHolderType" Text="-"></asp:Label></td>
                                        </tr>
                                       <tr style="background-color:gainsboro;color: dodgerblue;">
                                           <td>Make</td> 
                                           <td>Model</td>
                                           <td>Variant and Fuel Type</td>   
                                           <td>RTO</td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblMake" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblModel" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblVariant" Text="-"></asp:Label> (<asp:Label runat="server" ID="lblFuelType" Text="-"></asp:Label>)</td>
                                            <td><asp:Label runat="server" ID="lblRTO" Text="-"></asp:Label></td>
                                        </tr>
                                         <tr style="background-color:gainsboro;color: dodgerblue;">
                                            <td>Cubic Capacity</td>
                                            <td>Seating Capacity</td>
                                            <td>Non Electrical Accessories</td>
                                            <td>Electrical Accessories</td> 
                                        </tr>
                                        <tr>
                                            
                                            <td><asp:Label runat="server" ID="lblCubicCapacity" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblSeatingCapacity" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblNonElectricalAccessoriesIDV" Text="0"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblElectricalAccessoriesIDV" Text="0"></asp:Label></td>
                                        </tr>
                                        <tr style="background-color:gainsboro;color: dodgerblue;">
                                           <td>Credit Score</td>
                                           <td>Previous Policy Expiry Date</td> 
                                           <td><asp:Label runat="server" ID="lblDORorDOP" Text="Purchase Date"></asp:Label></td>                                       
                                           <td>Policy Start Date</td>                                               
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblCreditScore" Text="0"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPreviousPolicyExpiryDate" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblRagistrationDate" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPolicyStartDate" Text="-"></asp:Label></td>
                                        </tr>
                                        <tr style="background-color:gainsboro;color: dodgerblue;">
                                           <td><asp:Label runat="server" ID="lblCustomerIDProof" Text="Pan Number"></asp:Label></td>
                                           <td>Customer Gender</td> 
                                            <td></td>
                                            <td></td>
                                        </tr>
                                         <tr>
                                             <td><asp:Label runat="server" ID="lblCustomerIDProofNumber" Text="-"></asp:Label></td>
                                             <td><asp:Label runat="server" ID="lblCustomerGender" Text="-"></asp:Label></td>
                                             <td></td>
                                            <td></td>
                                         </tr>
                                    </table></div><table style="font-size: 11px" cellspacing="10" width="100%"><tr><td valign="top" style="padding-right:20px">
                                        <table id="table3" style="width: 100%; text-align: left;" cellspacing="0" cellpadding="2" border="1" valign="top">
                                                    <tr style="text-align: center; background-color: gray; color: white;">
                                                        <td style="color:white">OWN DAMAGE <asp:Label runat="server" ID="lblODYearText" Text=""></asp:Label></td>
                                                        <td style="color:white">Premium</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Own Damage Premium</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblOwnDamagePremium" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Electrical/Electronic Items</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblElectronicSI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Non Electrical/Electronic Items</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblNonElectronicSI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>External Bi Fuel Kit</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblExternalBiFuelSI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                   
                                                    <tr style="text-align: center; background-color: gray; color: white;">
                                                        <td colspan="2" style="color:white">ADD-ONs</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Engine Protect Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblEngineProtect" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Return to Invoice Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblReturnToInvoice" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Consumable Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblConsumableCover" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Depreciation Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblDepreciationCover" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Road Side Assistance (RSA)</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRSA" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Daily Car Allowance</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblDailyCarAllowance" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Key Replacement</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblKeyReplacement" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Tyre Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblTyreCover" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>NCB Protect</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblNCBProtect" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Loss of Personal Belongings</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblLossofPersonalBelongings" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr style="text-align: center; background-color: gray; color: white;">
                                                        <td colspan="2" style="color:white">Discount</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Voluntary Deduction</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblVoluntaryDeduction" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Voluntary Deduction for Dep Waiver</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblVoluntaryDeductionforDepWaiver" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>No Claim Bonus (NCB) <asp:Label runat="server" ID="lblNCBPercentage" Text="0%"></asp:Label></td>
                                                        <td align="right"><asp:Label runat="server" ID="lblNCB" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Total (A)</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblTotalPremiumOwnDamage" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table></td><td valign="top">
                                                    <table id="table4" style="width: 100%; text-align: left;" cellspacing="0" cellpadding="2" border="1">

                                                    <tr style="text-align: center; background-color: gray; color: white;">
                                                        <td style="color:white">LIABILITY <asp:Label runat="server" ID="lblTPYearText" Text=""></asp:Label></td>
                                                        <td style="color:white">Premium</td>
                                                    </tr>

                                                    <tr>
                                                        <td>Basic TP including TPPD premium</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblBasicTPPremium" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Liability For Bi-Fuel Kit</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblLiabilityForBiFuel" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>PA For Unnamed Passenger</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblPAForUnnamedPassengerSI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>PA For Named Passenger</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblPAForNamedPassengerSI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>PA To Paid Driver</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblPAToPaidDriverSI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label runat="server" ID="lblTenureOwnerDriver" Text="1"></asp:Label> PA Cover For Owner Driver</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblPACoverForOwnerDriver" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Legal Liability for paid driver cleaner conductor</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblLegalLiabilityToPaidDriverNo" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Legal Liability for Employees other than paid driver conductor cleaner</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblLLEOPDCC" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Total (B)</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblTotalPremiumLiability" Text="0.00"></asp:Label></td>
                                                    </tr>

                                                </table>
                                                <br />

                                                <table id="table5" style="width: 100%; text-align: left;" cellspacing="0" cellpadding="2" border="1">
                                                    <tr>
                                                        <td colspan="2" align="center" style="background-color: gray; color: white;">TOTAL PREMIUM</td>
                                                    </tr>

                                                    <tr>
                                                        <td>Sytem IDV</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblSystemIDV" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Final IDV</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblFinalIDV" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Net Premium (A) + (B)</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblNetPremium" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                           <%--final--%><td><asp:Label runat="server" ID="lblGSTOrServiceTax" Text="Service Tax"></asp:Label> @ <asp:Label runat="server" ID="lblPercentServiceTax" Text="0.00"></asp:Label>%</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblServiceTax" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Total Premium</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblTotalPremium" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table><div style="font-size:7px; display:none;line-height:8px;text-align:left;"><span style="text-align:left">Disclaimer:</span>
                                             <ul>
                                                 <li>The calculator is an online tool to facilitate the premium computation and is not intended to replace (or incorporate) the underwriting guidelines.</li>
                                                 <li>The company reserves the right to modify the rates, IDV, underwriting guidelines etc without prior intimation.</li>
                                                 <li>Accessories sum insured should not be more than 25% of IDV.</li>
                                                 <li>For any other combination of add-ons please contact our nearest branch.</li>
                                                 <li>In case of Invoice value, the IDV would be invoice value-5% and no further deviation from this value would be allowed. Also, if invoice value is applied supporting document must be provided with the proposal form.</li>
                                                 <li>This quote will be valid till 7 days from quote date.</li>
                                                 <li>Risk inception is subject to receipt of premium.</li>
                                                 <li>This quote is issued without CPA coverage based on the Insured's declaration stating that he/she holds a 24 hour Personal Accident cover of Rs 15 lacs or above for the same period</li>
                                                 <li id="lilblPremiumWithoutPAtoOwnerDriver" runat="server" visible="true">OD only cover (including GST) Rs. 
                                                 <asp:Label ID="lblPremiumWithoutPAtoOwnerDriver" runat="server" Text=""></asp:Label>
                                                 </li>
                                             </ul>
                                         </div>
                                </asp:Panel>

                                    <table id="table26" style="width: 38%;text-align: left;font-size: 11px;" cellspacing="0" cellpadding="2" border="1" valign="top">
                                                    <tr style="text-align: center; background-color: gray; color: white;">
                                                        <td style="color:white">Cover</td>
                                                        <td style="color:white">GLM Rate</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Basic OD</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateBasicOD" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Return To Invoice</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateRTI" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Depreciation Waiver</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateDC" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Engine Protect</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateEP" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Consumable Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateCC" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Daily Car Allowance</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateDCA" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Key Replacement</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateKR" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Tyre Cover</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateTC" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>NCB Protect</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateNCBP" Text="0.00"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Loss of Personal Belongings</td>
                                                        <td align="right"><asp:Label runat="server" ID="lblRateLOPB" Text="0.00"></asp:Label></td>
                                                    </tr>
                                        </table>
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                     <div style="float:left;width: 650px;text-align: left;"> 
                                        <asp:Label ID="lblWarningText" runat="server" style="color:red"></asp:Label>
                                         <asp:HiddenField id="hdnIsWarningPresent" Value="0"  runat="server"/>
                                    </div>
                                    <asp:Button ID="btnDownloadPremiumBreak" OnClientClick="setFormSubmitting();" class="btn btn-primary" runat="server" Text="Download PDF" OnClick="btnDownloadPremiumBreakup_Click"  />
                                    <asp:Button ID="btnOpenCustomerPopUp" class="btn btn-primary" runat="server" Text="Finalize Quote" OnClientClick="CloseModalViewPremium();" OnClick="btnOpenCustomerPopUp_Click" />
                                    <%--<button type="submit" class="btn btn-default" runat="server" id="btnDownloadPremiumBreakupPDF" onclick="btnDownloadPremiumBreakup_Click">Download PDF</button>--%>
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>

                       





                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSendPDF" />
            <asp:PostBackTrigger ControlID="btnDownloadPremiumBreak" />
            <asp:PostBackTrigger ControlID="btnExit" />
            
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


