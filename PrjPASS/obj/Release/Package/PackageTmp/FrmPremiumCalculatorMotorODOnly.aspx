<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmPremiumCalculatorMotorODOnly.aspx.cs" Inherits="PrjPASS.FrmPremiumCalculatorMotorODOnly" %>

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
        }
    </script>

    <style type="text/css">
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

        #table23 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table3 td {
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

        #table25 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table26 td {
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
            //ChangeDORLabel();

            //var crntDate = new Date();
            //var year = crntDate.getFullYear();           
            //$("[id$=txtMfgYear]").val(year);

        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ChangeDOPLabel);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ChangeDORLabel);
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(openModalSaveProposal);
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
        
        function ChangeDOPLabel()
        {
            var IsNewBusinessType = $('input[id*=rbbtNewBusiness]').is(":checked");
            if (IsNewBusinessType) {
                $('#<%=lblDOR.ClientID%>').html("Date of Purchase (dd/MM/yyyy)");

                <%--$("#<%=drpProductType.ClientID%>").html('');
                var newOption = "<option value='1063'>Kotak Car Secure –Bundled (3+1)</option><option value='1062'>Kotak Car Secure –3 years (3+3)</option>";
                $("#<%=drpProductType.ClientID%>").append(newOption);--%>

            }
            else {
                $('#<%=lblDOR.ClientID%>').html("Date of Registration (dd/MM/yyyy)");
            }
        }
        
        function ChangeDORLabel() {
            var selectedProductType = $('#<%=drpProductType.ClientID %> option:selected').val();
            var selectedTenureOwnerDriver = $('#<%=drpTenureOwnerDriver.ClientID %> option:selected').val();

            var IsNewBusinessType = $('input[id*=rbbtNewBusiness]').is(":checked");
            if (IsNewBusinessType) {
                $('#<%=lblDOR.ClientID%>').html("Date of Purchase (dd/MM/yyyy)");
            }
            else {
                $('#<%=lblDOR.ClientID%>').html("Date of Registration (dd/MM/yyyy)");
            }

            var d_minDate = new Date();

            $("#<%=drpProductType.ClientID%>").html('');
            $("#<%=drpTenureOwnerDriver.ClientID%>").html('');
            
            if (IsNewBusinessType) {
                var dy = d_minDate.getFullYear();
                //dy = dy - 1;
                var oyd = '09/01/2018'; //HARD CODING MIN DATE TO BE 01-Sep-2018
                d_minDate = new Date(oyd);
                //d_minDate.setYear(dy);

                //$("#<%=drpProductType.ClientID%>").prop("disabled", false);

                var newOption = ""; var tenureOption = "";
                if (selectedProductType == "1062") {
                    newOption = "<option value='1063'>Kotak Car Secure –Bundled (3+1)</option><option value='1062' selected>Kotak Car Secure –3 years (3+3)</option>";
                }
                else
                {
                    newOption = "<option value='1063' selected>Kotak Car Secure –Bundled (3+1)</option><option value='1062'>Kotak Car Secure –3 years (3+3)</option>";
                }
                //var newOption = "<option value='1063'>Kotak Car Secure –Bundled (3+1)</option><option value='1062'>Kotak Car Secure –3 years (3+3)</option>";
                $("#<%=drpProductType.ClientID%>").append(newOption);

                if (selectedTenureOwnerDriver == "1") {
                    tenureOption = "<option value='1' selected>1</option><option value='3'>3</option><option value='0'>0</option>";
                }
                else if (selectedTenureOwnerDriver == "3") {
                    tenureOption = "<option value='1'>1</option><option value='3' selected>3</option><option value='0'>0</option>";
                }
                else if (selectedTenureOwnerDriver == "0") {
                    tenureOption = "<option value='1'>1</option><option value='3'>3</option><option value='0' selected>0</option>";
                }
                //var tenureOption = "<option value='1'>1</option><option value='3'>3</option><option value='0'>0</option>";
                $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);
            }
            else {

                var dy = d_minDate.getFullYear();
                dy = dy - 12;
                d_minDate.setYear(dy);

                var newOption = "<option value='1011'>Kotak Car Secure-OD only</option>";
                $("#<%=drpProductType.ClientID%>").append(newOption);

                var tenureOption = "<option value='1'>1</option><option value='0'>0</option>";
                $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);
            }

            var PolicyHolderType = $('#<%=drpPolicyHolderType.ClientID %> option:selected').text(); //$("#<%=drpPolicyHolderType.ClientID%>").text();
            if (PolicyHolderType == 'Unlisted Limited Company' || PolicyHolderType == 'Government Organization') {
                $("#<%=drpTenureOwnerDriver.ClientID%>").html('');
                var tenureOption = "<option value='0'>0</option>";
                $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);
            }

            if (IsNewBusinessType) {
                var d_maxDate = new Date();
                var dy = d_maxDate.getFullYear();
                dy = dy + 0;
                d_maxDate.setYear(dy);
                SetMinMaxDate(d_minDate, d_maxDate);
            }
            else
            {
                var d_maxDate = new Date();
                var new_d_maxDate = new Date();
                new_d_maxDate.setDate(d_maxDate.getDate() - 272); //272 days
                var newMaxDate = new Date(new_d_maxDate);
                SetMinMaxDate(d_minDate, newMaxDate);
            }
        }

        function SetMinMaxDate(vminDate, vmaxDate)
        {
            var DtRgstrn = $("[id$=txtDateOfRegistration]").val().split("/");
            var compare_DtRgstrn = new Date(DtRgstrn[2], DtRgstrn[1] - 1, DtRgstrn[0])

            if (compare_DtRgstrn < vminDate || compare_DtRgstrn > vmaxDate) {

                //alert($("[id$=txtDateOfRegistration]").val());
                $("[id$=txtDateOfRegistration]").datepicker('destroy');

                $("[id$=txtDateOfRegistration]").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy',
                    minDate: vminDate,
                    maxDate: vmaxDate
                });

                var crntDate = new Date();
                var dat = ('0' + crntDate.getDate()).slice(-2);
                var month = ('0' + (crntDate.getMonth() + 1)).slice(-2);
                var year = crntDate.getFullYear();
                var strDate = dat + '/' + month + '/' + year;

                $("[id$=txtDateOfRegistration]").val(strDate);
                $("[id$=txtMfgYear]").val(year);

                var IsNewBusinessType = $('input[id*=rbbtNewBusiness]').is(":checked");
                if (!IsNewBusinessType) {

                    var datDateOfRegistration = new Date();
                    datDateOfRegistration.setFullYear(datDateOfRegistration.getFullYear() - 1);
                    datDateOfRegistration.setDate(datDateOfRegistration.getDate());

                    var newStrDate = newdat + '/' + newmonth + '/' + newyear;
                    //alert(datDateOfRegistration);


                    var newdat = ('0' + (datDateOfRegistration.getDate())).slice(-2);
                    var newmonth = ('0' + (datDateOfRegistration.getMonth() + 1)).slice(-2);
                    var newyear = datDateOfRegistration.getFullYear();
                    var newStrDate = newdat + '/' + newmonth + '/' + newyear;

                    //alert(newStrDate);
                    $("[id$=txtDateOfRegistration]").val(newStrDate);
                    $("[id$=txtMfgYear]").val(newyear);

                }
            }
        }

        function SetAutoComplete() {
            $("[id$=txtRTOAuthorityCode]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmPremiumCalculatorMotor.aspx/GetRTO") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",                      
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    val: item
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
                    $("[id$=txtRTOAuthorityCode]").val(i.item.val);
                    $("[id*=btnGetRtoCode]").click();
                },                
                minLength: 2,
                autoFocus: true
            });


            $("[id$=txtIntermediaryName]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmPremiumCalculatorMotor.aspx/GetIntermediaryCode") %>',
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
                    var strItems2 = i.item.label.split("~");
                    $("[id$=txtPinCode]").val(strItems2[0]);
                    $("[id$=hdnPinCode]").val(strItems2[0]);
                    $("[id$=hdnPinCodeLocality]").val(i.item.val);
                    $("[id*=btnGetPincodeDetails]").click();
                },                
                minLength: 3,
                autoFocus: true
            });

            var SelectedProductType = $("#<%=drpProductType.ClientID%>").val();
            var SelectedTenureOwnerDriver = $("#<%=drpTenureOwnerDriver.ClientID%> option:selected").val();
            
            $("#<%=drpProductType.ClientID%>").html('');
            $("#<%=drpTenureOwnerDriver.ClientID%>").html('');

            var d_minDate = new Date();
            var IsNewBusinessType = $('input[id*=rbbtNewBusiness]').is(":checked");
            if (IsNewBusinessType) {
                var dy = d_minDate.getFullYear();
                //dy = dy - 1; 
                var oyd = '09/01/2018'; //HARD CODING MIN DATE TO BE 01-Sep-2018
                d_minDate = new Date(oyd);
                //d_minDate.setYear(dy);

                var newOption = "<option value='1063'>Kotak Car Secure –Bundled (3+1)</option><option value='1062'>Kotak Car Secure –3 years (3+3)</option>";
                $("#<%=drpProductType.ClientID%>").append(newOption);

                var tenureOption = "<option value='1'>1</option><option value='3'>3</option><option value='0'>0</option>";
                $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);

                $("#<%=drpProductType.ClientID%>").val(SelectedProductType);
                $("#<%=drpTenureOwnerDriver.ClientID%>").val(SelectedTenureOwnerDriver);
            }
            else
            {
                var dy = d_minDate.getFullYear();
                dy = dy - 12;
                d_minDate.setYear(dy);

                var newOption = "<option value='1011'>Kotak Car Secure-OD only</option>";
                $("#<%=drpProductType.ClientID%>").append(newOption);

                var tenureOption = "<option value='1'>1</option><option value='0'>0</option>";
                $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);
                $("#<%=drpTenureOwnerDriver.ClientID%>").val(SelectedTenureOwnerDriver);
            }

            var PolicyHolderType = $('#<%=drpPolicyHolderType.ClientID %> option:selected').text(); //$("#drpPolicyHolderType option:selected").text(); //$("#<%=drpPolicyHolderType.ClientID%>").text();
            
            if (PolicyHolderType == 'Unlisted Limited Company' || PolicyHolderType == 'Government Organization') {
                $("#<%=drpTenureOwnerDriver.ClientID%>").html('');
                var tenureOption = "<option value='0'>0</option>";
                $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);
            }

            var FinalMaxDate = new Date();

            if (IsNewBusinessType) {
                var d_maxDate = new Date();
                var dy = d_maxDate.getFullYear();
                dy = dy + 0;
                d_maxDate.setYear(dy);
                FinalMaxDate = d_maxDate;
            }
            else
            {
                var newMaxDate = new Date();
                var new_d_maxDate = new Date();
                new_d_maxDate.setDate(newMaxDate.getDate() - 272); //272 days
                newMaxDate = new Date(new_d_maxDate);
                FinalMaxDate = newMaxDate;
            }

            $("[id$=txtCustomerDOB]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#datepickerImagedob").click(function () {
                $("[id$=txtCustomerDOB]").datepicker("show");
            });

            $("[id$=txtDateOfRegistration]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: d_minDate,
                maxDate: FinalMaxDate
            });

            $("#datepickerImagedor").click(function () {
                $("[id$=txtDateOfRegistration]").datepicker("show");
            });

            $("#datepickerImagePPED").click(function () {
                $("[id$=txtPreviousPolicyExpiryDate]").datepicker("show");
            });

            $("[id$=txtPreviousPolicyExpiryDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });


            $("#datepickerImagePolicyStartDate").click(function () {
                $("[id$=txtPolicyStartDate]").datepicker("show");
            });

            $("[id$=txtPolicyStartDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            /*
            $("[id$=txtDateOfPurchase]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });

            $("#datepickerImagedop").click(function () {
                $("[id$=txtDateOfPurchase]").datepicker("show");
            });
            */

            $("#<%=drpPolicyHolderType.ClientID%>").change(function () {
                var PolicyHolderType = $('option:selected', this).text()
                if (PolicyHolderType == 'Unlisted Limited Company' || PolicyHolderType == 'Government Organization') {
                    $("#<%=drpTenureOwnerDriver.ClientID%>").html('');
                    var tenureOption = "<option value='0'>0</option>";
                    $("#<%=drpTenureOwnerDriver.ClientID%>").append(tenureOption);
                }
                else
                {
                    var IsItNewBusinessType = $('input[id*=rbbtNewBusiness]').is(":checked");
                    if (IsItNewBusinessType) {

                        var tenureOption = "<option value='1'>1</option><option value='3'>3</option><option value='0'>0</option>";
                        $("#<%=drpTenureOwnerDriver.ClientID%>").html(tenureOption);
                    }
                    else
                    {
                        var tenureOption = "<option value='1'>1</option><option value='0'>0</option>";
                        $("#<%=drpTenureOwnerDriver.ClientID%>").html(tenureOption);
                    }
                }
            });
        }

       
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MstCntFormContent" runat="server">

   
     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

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
                        <h4><i class="fa fa-sign-in"></i>PASS - Private Car Premium Calculator</h4>
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
                                            <Header>Policy Details</Header>
                                            <Content>

                                                <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>

                                                         <td class="tdbkg">Business Type
                                                        </td>
                                                        <td>
                                                            <obout:OboutRadioButton GroupName="bt" ID="rbbtNewBusiness" Text="New Business" runat="server" onclick="ChangeDORLabel();" OnCheckedChanged="rbbtNewBusiness_CheckedChanged" AutoPostBack="true" Visible="false"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton GroupName="bt" ID="rbbtRollOver" Text="Roll Over" runat="server" onclick="ChangeDORLabel();"  Checked="true" OnCheckedChanged="rbbtRollOver_CheckedChanged" AutoPostBack="true"></obout:OboutRadioButton>
                                                            <%--<obout:OboutRadioButton GroupName="bt" ID="rbbtRenewalBusiness" Text="Renewal Business" runat="server"></obout:OboutRadioButton>--%>
                                                        </td>
                                                       

                                                       <td class="tdbkg">Product Type
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpProductType" runat="server" EnableViewState="true" style="width: 230px;" AutoPostBack="true" OnSelectedIndexChanged="drpProductType_SelectedIndexChanged">
                                                                <%--<asp:ListItem Text="Select" Value="0"></asp:ListItem>--%>
                                                                <asp:ListItem Text="Kotak Car Secure –Bundled (3+1)" Value="1063" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Kotak Car Secure –3 years (3+3)" Value="1062"></asp:ListItem>
                                                                <asp:ListItem Text="Kotak Car Secure-OD only" Value="1011"></asp:ListItem>
                                                            <%--<asp:ListItem Text="Comprehensive Policy" Value="Comprehensive Policy" Selected="True"></asp:ListItem>--%>
                                                                <%--<asp:ListItem Text="Liability Only Policy" Value="Liability Only Policy"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Package Policy" Value="Package Policy"></asp:ListItem>
                                                                    <asp:ListItem Text="Fire Only Policy" Value="Fire Only Policy"></asp:ListItem>
                                                                <asp:ListItem Text="Theft Only Policy" Value="Theft Only Policy"></asp:ListItem>
                                                                <asp:ListItem Text="Fire and Theft Only Policy" Value="Fire and Theft Only Policy"></asp:ListItem>
                                                                <asp:ListItem Text="Liability Only Policy with Fire and Theft" Value="Liability Only Policy with Fire and Theft"></asp:ListItem>
                                                                <asp:ListItem Text="Liability Only Policy with Fire" Value="Liability Only Policy with Fire"></asp:ListItem>
                                                                <asp:ListItem Text="Liability Only Policy with Theft" Value="Liability Only Policy with Theft"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </td>

                                                         <td class="tdbkg">Type of Policy Holder
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpPolicyHolderType" runat="server" OnSelectedIndexChanged="drpPolicyHolderType_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Individual Owner" Value="Individual Owner"></asp:ListItem>
                                                                <asp:ListItem Text="Unlisted Limited Company" Value="Unlisted Limited Company"></asp:ListItem>
                                                                <%--<asp:ListItem Text="Limited Company" Value="Limited Company"></asp:ListItem>--%>
                                                                <asp:ListItem Text="Government Organization" Value="Government Organization"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>

                                                        
                                                        

                                                    </tr>

                                                    <tr>

                                                        


                                                        <%--<td class="tdbkg">Branch Name
                                                        </td>
                                                        <td style="width: 200px">
                                                            <asp:DropDownList CssClass="drp" ID="drpBranchName" runat="server"></asp:DropDownList>
                                                        </td>--%>
                                                        <td class="tdbkg">Intermediary Name
                                                        </td>
                                                        <td>
                                                            <%--<asp:DropDownList CssClass="drp" ID="drpIntermediaryName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIntermediaryName_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>

                                                            <obout:OboutTextBox ID="txtIntermediaryName" runat="server" Text=""></obout:OboutTextBox>
                                                            <asp:HiddenField ID="hfIntermediaryCode" runat="server" Value="" />
                                                            <asp:Button ID="btnGetIntermediaryCode" runat="server" OnClick="btnGetIntermediaryCode_Click" />

                                                        </td>
                                                        <td class="tdbkg">Intermediary Code
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <asp:Label ID="lblIntermediaryCode" runat="server" Text="-"></asp:Label>
                                                            <%--<asp:RequiredFieldValidator ID="rfv1"
                                                                runat="server" ControlToValidate="txtIntermediaryCode"
                                                                ErrorMessage="Please enter Intermediary Code!" ToolTip="Mandatory" Text="*" ValidationGroup="vp" Display="None">
                                                            </asp:RequiredFieldValidator>--%>
                                                        </td>

                                                         <td class="tdbkg">Intermediary/Business Channel Type
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <%--<asp:DropDownList CssClass="drp" ID="drpIntermediaryBusineeChannelType" runat="server">
                                                            </asp:DropDownList>--%>

                                                            <%--    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="BANCASSURANCE" Value="BANCASSURANCE"></asp:ListItem>
                                                                <asp:ListItem Text="BROKER CORPORATE" Value="BROKER CORPORATE"></asp:ListItem>
                                                                <asp:ListItem Text="CORPORATE AGENT" Value="CORPORATE AGENT"></asp:ListItem>
                                                                <asp:ListItem Text="DIRECT AGENT" Value="DIRECT AGENT"></asp:ListItem>
                                                                <asp:ListItem Text="DIRECT SALES" Value="DIRECT SALES"></asp:ListItem>
                                                                <asp:ListItem Text="INDIVIDUAL AGENT" Value="INDIVIDUAL AGENT"></asp:ListItem>
                                                                <asp:ListItem Text="INTERNET" Value="INTERNET"></asp:ListItem>
                                                                <asp:ListItem Text="MOTOR DEALER" Value="MOTOR DEALER"></asp:ListItem>
                                                                <asp:ListItem Text="OTHER INTERMEDIARIES" Value="OTHER INTERMEDIARIES"></asp:ListItem>
                                                                <asp:ListItem Text="TELE SALES" Value="TELE SALES"></asp:ListItem>
                                                                <asp:ListItem Text="WEB AGGREGATOR" Value="WEB AGGREGATOR"></asp:ListItem>
                                                            --%>
                                                            <asp:Label ID="lblIntermediaryBusineeChannelType" runat="server" Text="-"></asp:Label>
                                                            <asp:HiddenField ID="hdnPrimaryMOCode" runat="server" Value="" />
                                                            <asp:HiddenField ID="hdnPrimaryMOName" runat="server" Value="" />
                                                            <asp:HiddenField ID="hdnOfficeCode" runat="server" Value="" />
                                                            <asp:HiddenField ID="hdnOfficeName" runat="server" Value="" />
                                                            <asp:HiddenField ID="hdnMinMarketMovement" runat="server" Value="" />
                                                            <asp:HiddenField ID="hdnMaxMarketMovement" runat="server" Value="" />
                                                            <asp:HiddenField ID="hdnIntermediaryType" runat="server" Value="" />
                                                        </td>
                                                      
                                                    </tr>
                                                    <tr>


                                                        <%-- <td class="tdbkg">Field User ID</td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtFieldUserId" runat="server" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox></td>
                                                        <td class="tdbkg">Lead Generator </td>
                                                        <td colspan="3">
                                                            <obout:OboutTextBox ID="txtLeadGenerator" runat="server" Text="bbbb"></obout:OboutTextBox>
                                                        </td>--%>
                                                    </tr>
                                                    <tr>



                                                        <td class="tdbkg">Customer Type</td>
                                                        <td>
                                                            <obout:OboutRadioButton GroupName="ct" ID="rbctIndividual" Text="Individual" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="rbctIndividual_CheckedChanged"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton GroupName="ct" ID="rbctOrganization" Text="Organization" runat="server" AutoPostBack="true" OnCheckedChanged="rbctOrganization_CheckedChanged"></obout:OboutRadioButton>
                                                        </td>

                                                        <td class="tdbkg">RTO Authority Code
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtRTOAuthorityCode" runat="server" Text="" MaxLength="5"></obout:OboutTextBox>
                                                            <span style="font-size:10px">e.g. MH01</span>
                                                            <asp:HiddenField ID="hfSelectedRTO" runat="server" Value="" />
                                                            <asp:Button ID="btnGetRtoCode" runat="server" OnClick="btnGetRtoCode_Click" />
                                                        </td>
                                                        <td class="tdbkg">Registration Authority Location
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" runat="server" ID="drpRTOLocation" AutoPostBack="true" OnSelectedIndexChanged="drpRTOLocation_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>





                                                        <td class="tdbkg">RTO Cluster
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <asp:Label ID="lblRTOCluster" runat="server" Text="-"></asp:Label>
                                                        </td>

                                                        <td class="tdbkg">Vehicle Make
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" runat="server" ID="drpVehicleMake" AutoPostBack="true" OnSelectedIndexChanged="drpVehicleMake_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                        <td class="tdbkg">Vehicle Model
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpVehicleModel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpVehicleModel_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>

                                                    </tr>

                                                    <tr>

                                                        <td class="tdbkg">Vehicle Sub Type
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpVehicleSubType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpVehicleSubType_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>

                                                        <td class="tdbkg">Model Cluster
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <asp:Label ID="lblModelCluster" runat="server" Text="-"></asp:Label>
                                                        </td>

                                                        <td class="tdbkg">Vehicle Segment
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <%--<obout:OboutTextBox ID="txtVehicleSegment" runat="server" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>--%>
                                                            <asp:Label ID="lblVehicleSegment" runat="server" Enabled="false" ToolTip="Readonly" Text="-"></asp:Label>
                                                        </td>

                                                    </tr>

                                                    <tr>

                                                        <td class="tdbkg">Cubic Capacity
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <asp:Label ID="lblCubicCapacityt" runat="server" Text="-"></asp:Label>
                                                        </td>
                                                        <td class="tdbkg">Seating Capacity
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <asp:Label ID="lblSeatingCapacityt" runat="server" Text="-"></asp:Label>
                                                        </td>

                                                        <td class="tdbkg">Fuel Type
                                                        </td>
                                                        <td class="tdbkg2">
                                                            <%--<asp:DropDownList CssClass="drp" ID="drpFuelType" runat="server" Enabled="false" ToolTip="Readonly">
                                                                <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Diesel" Value="Diesel"></asp:ListItem>
                                                                <asp:ListItem Text="Petrol" Value="Petrol"></asp:ListItem>
                                                                <asp:ListItem Text="CNG" Value="CNG"></asp:ListItem>
                                                                <asp:ListItem Text="LPG" Value="LPG"></asp:ListItem>
                                                                <asp:ListItem Text="Electric" Value="Electric"></asp:ListItem>
                                                                <asp:ListItem Text="Hybrid" Value="Hybrid"></asp:ListItem>
                                                                <asp:ListItem Text="Battery" Value="Battery"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                            <asp:Label ID="lblFuelTypet" runat="server" Text="-"></asp:Label>
                                                        </td>

                                                    </tr>

                                                    <tr>



                                                        <td class="tdbkg" style="min-width:175px;"><asp:Label ID="lblDOR" runat="server" Text="Date of Purchase (dd/MM/yyyy)"></asp:Label>
                                                        </td>
                                                        <td>
                                                            
                                                            <%--<at1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDateOfRegistration" CultureName="en-US"> </at1:MaskedEditExtender>--%>
                                                            <%--<input type="text" id="datepicker">--%>
                                                            <obout:OboutTextBox ID="txtDateOfRegistration" runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtDateOfRegistration_TextChanged"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagedor" />
                                                            <%--<asp:ImageButton ID="imgPopup" ImageUrl="images/calendar.png" ImageAlign="Bottom" runat="server" />--%>
                                                            <%--<at1:CalendarExtender ID="calRegistration" PopupButtonID="imgPopup" PopupPosition="TopRight" runat="server" TargetControlID="txtDateOfRegistration" Format="dd/MM/yyyy"></at1:CalendarExtender>--%>

                                                           <%--<obout:Calendar runat="server" ID="calRegistration"
                                                                StyleFolder="css/datepickercss"
                                                                DatePickerMode="true"
                                                                TextBoxId="txtDateOfRegistration"
                                                                DatePickerImagePath="css/datepickercss/icon2.gif"
                                                                MonthWidth="160"
                                                                MonthHeight="130" DateFormat="dd/MM/yyyy" AutoPostBack="true"/>--%>
                                                            <%--<at1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1" ControlToValidate="txtDateOfRegistration" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid" IsValidEmpty="False"></at1:MaskedEditValidator>--%>
                                                        </td>

                                                        <%--<td class="tdbkg">Date of Purchase (dd/MM/yyyy)</td>
                                                        <td>

                                                            <obout:OboutTextBox ID="txtDateOfPurchase" runat="server" ReadOnly="false" OnTextChanged="txtDateOfPurchase_TextChanged" AutoPostBack="true"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagedop" />
                                                            <obout:Calendar runat="server" ID="calPurchaseDate"
                                                                StyleFolder="css/datepickercss"
                                                                DatePickerMode="true"
                                                                TextBoxId="txtDateOfPurchase"
                                                                DatePickerImagePath="css/datepickercss/icon2.gif"
                                                                MonthWidth="160"
                                                                MonthHeight="130" DateFormat="dd/MM/yyyy" AutoPostBack="true" />
                                                        </td>--%>

                                                         <td class="tdbkg">Voluntary Deductible Amount
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpVDA" runat="server">
                                                                <asp:ListItem Text="0" Value="0" Selected="true" />
                                                                <asp:ListItem Text="2500" Value="2500" />
                                                                <asp:ListItem Text="2700" Value="2700" />
                                                                <asp:ListItem Text="5000" Value="5000" />
                                                                <asp:ListItem Text="5200" Value="5200" />
                                                              <%--  <asp:ListItem Text="7500" Value="7500" />
                                                                <asp:ListItem Text="15000" Value="15000" /> --%>
                                                            </asp:DropDownList>
                                                        </td>


                                                       

                                                       <%--<td class="tdbkg">Vehicle Age
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtVehicleAge" runat="server" Text="0" Enabled="false"></obout:OboutTextBox>
                                                        </td>--%>

                                                        <td class="tdbkg">IRF1 <%--IRF1 = MARKET MOVEMENT--%>
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtMarketMovement" runat="server" Text="0" MaxLength="6"></obout:OboutTextBox>
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td class="tdbkg">
                                                            Policy Start Date
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtPolicyStartDate" runat="server" ReadOnly="false" placeholder="dd/mm/yyyy" AutoPostBack="true" OnTextChanged="txtPolicyStartDate_TextChanged"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagePolicyStartDate" />
                                                        </td>
                                                        
                                                        <td class="tdbkg">
                                                            Manufacturer Year
                                                        </td>
                                                        <td colspan="3">
                                                            <obout:OboutTextBox ID="txtMfgYear" runat="server" ReadOnly="false" placeholder="yyyy" AutoPostBack="true" ></obout:OboutTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id = "trDeviation" runat = "server">
                                                        <td class="tdbkg">
                                                            IRF2 BasicOD
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtBasicODdeviation" runat="server" Text="0"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">
                                                            IRF2 Addon
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtAddOnDeviation" runat="server" Text="0"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg"></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdbkg">Customer Name</td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtCustomerName" runat="server" Text="" MaxLength="50"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">
                                                            <asp:Label id="lblCustGender" runat="server" Text="Customer Gender"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList CssClass="drp" ID="drpCustomerGender" runat="server" AllowEdit="false" AutoPostBack="true" OnSelectedIndexChanged="drpCustomerGender_SelectedIndexChanged">
                                                                <asp:ListItem Text="MALE" Value="MALE" />
                                                                <asp:ListItem Text="FEMALE" Value="FEMALE" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="display:none">

                                                                                         <td class="tdbkg">Compulsory PA with Owner Driver</td>

                                                        <td>
                                                            <obout:OboutRadioButton GroupName="cpw" ID="rbCpwYes" Text="Yes" runat="server" Checked="true"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton GroupName="cpw" ID="rbCpwNo" Text="No" runat="server"></obout:OboutRadioButton>
                                                        </td>

                                                        <td class="tdbkg" style="display:none">Insured Credit Score
                                                        </td>
                                                        <td style="display:none">
                                                            <obout:OboutTextBox ID="txtInsuredCreditScore" runat="server" Text="0" TextMode="Number"></obout:OboutTextBox>
                                                        </td>


                                                        <%--<td class="tdbkg">No. of Claim Free Years Completed
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNoofClaimFreeYearsCompleted" runat="server" Text="1"></obout:OboutTextBox>
                                                        </td>--%>

                                                       

                                                       <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>


                                                    <%--                                                    <tr>

                                                       <td class="tdbkg">Imported Vehicle Without Payment of Customs Duty</td>
                                                        <td>
                                                            <obout:OboutRadioButton GroupName="ivwp" ID="rbivwpYes" Text="Yes" runat="server"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton GroupName="ivwp" ID="rbivwpNo" Text="No" runat="server" Checked="true"></obout:OboutRadioButton>
                                                        </td>
                                                      
                                                    </tr>--%>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>

                                        <at1:AccordionPane ID="accIDV" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Insured's Declared Value</Header>
                                            <Content>
                                                <table id="table8" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>

                                                        <td class="tdbkg">Non Electrical Accessories Required
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkNEAR" runat="server" AutoPostBack="true" OnCheckedChanged="chkNEAR_CheckedChanged"></obout:OboutCheckBox>

                                                        </td>
                                                        <td class="tdbkg">Non Electrical Accessories Sum Insured
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNeaSumInsured" runat="server" Text="0" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>
                                                        </td>

                                                    </tr>

                                                    <tr>

                                                        <td class="tdbkg">Electrical Accessories Required
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkEAR" runat="server" AutoPostBack="true" OnCheckedChanged="chkEAR_CheckedChanged"></obout:OboutCheckBox>
                                                        </td>
                                                        <td class="tdbkg">Electrical Accessories Sum Insured
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtEaSumInsured" runat="server" Text="0" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td class="tdbkg">External CNG/LPG Available?
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkCNGLPG" runat="server" AutoPostBack="true" OnCheckedChanged="chkCNGLPG_CheckedChanged"></obout:OboutCheckBox>
                                                        </td>
                                                        <td class="tdbkg">CNG/ LPG KIT Sum Insured
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtLPGKitSumInsured" runat="server" Text="0" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdbkg">IDV of the Vehicle</td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtIDVofVehicle" runat="server" Text="0" Enabled="false"></obout:OboutTextBox>
                                                            <asp:HiddenField ID="hdnFinalIDV" runat="server" Value="0" />
                                                        </td>
                                                    </tr>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>

                                        <at1:AccordionPane ID="accPPD" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Past Policy Details</Header>
                                            <Content>

                                                <table id="table9" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td class="tdbkg">Previous Policy Type/Type of Cover
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpCoverType" runat="server" AllowEdit="false">
                                                                <asp:ListItem Text="Bundled-1+3" Value="ComprehensivePolicy" Selected="true" />
                                                            </asp:DropDownList>
                                                        </td>



                                                        <td class="tdbkg">Previous Year NCB Slab
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpPreviousYearNCBSlab" runat="server" AllowEdit="false">
                                                                <asp:ListItem Text="0" Value="0" Selected="true" />
                                                                <asp:ListItem Text="20" Value="20" />
                                                                <asp:ListItem Text="25" Value="25" />
                                                                <asp:ListItem Text="35" Value="35" />
                                                                <asp:ListItem Text="45" Value="45" />
                                                                <asp:ListItem Text="50" Value="50" />
                                                                <asp:ListItem Text="55" Value="55" />
                                                                <asp:ListItem Text="65" Value="65" />
                                                            </asp:DropDownList>
                                                        </td>

                                                        <td class="tdbkg">Total Claim Count
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpTotalClaimCount" runat="server" AllowEdit="false" AutoPostBack="true" OnSelectedIndexChanged="drpTotalClaimCount_SelectedIndexChanged">
                                                                <asp:ListItem Text="No Claim" Value="No Claim" Selected="true" />
                                                                <asp:ListItem Text="1 OD Claim" Value="1 OD Claim" />
                                                                <asp:ListItem Text="2 OD Claims" Value="2 OD Claims" />
                                                                <asp:ListItem Text="1 TP Claim" Value="1 TP Claim" />
                                                                <asp:ListItem Text="2 TP Claims" Value="2 TP Claims" />
                                                                <asp:ListItem Text="1 OD and 1 TP Claim" Value="1 OD and 1 TP Claim" />
                                                                <asp:ListItem Text="1 OD and 2 TP Claims" Value="1 OD and 2 TP Claims" />
                                                                <asp:ListItem Text="2 OD and 1 TP Claims" Value="2 OD and 1 TP Claims" />
                                                                <asp:ListItem Text="2 OD and 2 TP Claims" Value="2 OD and 2 TP Claims" />
                                                            </asp:DropDownList>
                                                        </td>


                                                    </tr>

                                                    <tr>


                                                        <td class="tdbkg">Total Claim Amount
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtTotalClaimAmount" runat="server" Text="0" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>

                                                        </td>

                                                        <td class="tdbkg">Previous Policy Expiry Date
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtPreviousPolicyExpiryDate" runat="server" ReadOnly="false"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagePPED" />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </table>

                                            </Content>
                                        </at1:AccordionPane>

                                        <at1:AccordionPane ID="accCD" runat="server" HeaderCssClass="accordionHeader" Visible="false"
                                            HeaderSelectedCssClass="accordionHeaderSelected"
                                            ContentCssClass="accordionContent">
                                            <Header>Coverage Details</Header>
                                            <Content>
                                                <table id="table7" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td class="tdbkg">PA Cover for Unnamed Persons
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkPACoverUnnamedPersons" runat="server" AutoPostBack="true" OnCheckedChanged="chkPACoverUnnamedPersons_CheckedChanged"></obout:OboutCheckBox>
                                                        </td>

                                                        <td class="tdbkg">Number of Persons (Unnamed)
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNumberOfPersonsUnnamed" runat="server" Text="1" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">Capital Sum Insured Per Person (Unnamed)
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpCapitalSIPerPerson" runat="server" AllowEdit="false" Enabled="false" ToolTip="Readonly">
                                                                <asp:ListItem Text="0" Value="0" Selected="true" />
                                                                <asp:ListItem Text="10000" Value="10000" />
                                                                <asp:ListItem Text="20000" Value="20000" />
                                                                <asp:ListItem Text="30000" Value="30000" />
                                                                <asp:ListItem Text="40000" Value="40000" />
                                                                <asp:ListItem Text="50000" Value="50000" />
                                                                <asp:ListItem Text="60000" Value="60000" />
                                                                <asp:ListItem Text="70000" Value="70000" />
                                                                <asp:ListItem Text="80000" Value="80000" />
                                                                <asp:ListItem Text="90000" Value="90000" />
                                                                <asp:ListItem Text="100000" Value="100000" />
                                                                <asp:ListItem Text="110000" Value="110000" />
                                                                <asp:ListItem Text="120000" Value="120000" />
                                                                <asp:ListItem Text="130000" Value="130000" />
                                                                <asp:ListItem Text="140000" Value="140000" />
                                                                <asp:ListItem Text="150000" Value="150000" />
                                                                <asp:ListItem Text="160000" Value="160000" />
                                                                <asp:ListItem Text="170000" Value="170000" />
                                                                <asp:ListItem Text="180000" Value="180000" />
                                                                <asp:ListItem Text="190000" Value="190000" />
                                                                <asp:ListItem Text="200000" Value="200000" />
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>

                                                    <%--<tr>
                                                        <td class="tdbkg">PA Cover for Named Persons
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkPACoverNamedPersons" runat="server" AutoPostBack="true" OnCheckedChanged="chkPACoverNamedPersons_CheckedChanged"></obout:OboutCheckBox>
                                                        </td>

                                                        <td class="tdbkg">Number of persons (Named)
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNumberofPersonsNamed" runat="server" Text="0" Enabled="false" ToolTip="Readonly"></obout:OboutTextBox>
                                                        </td>
                                                         <td class="tdbkg">Capital Sum Insured Per Person (Named)
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpCapitalSINamed" runat="server" AllowEdit="false" Enabled="false" ToolTip="Readonly">
                                                                <asp:ListItem Text="0" Value="0" Selected="true" />
                                                                <asp:ListItem Text="10000" Value="10000" />
                                                                <asp:ListItem Text="20000" Value="20000" />
                                                                <asp:ListItem Text="30000" Value="30000" />
                                                                <asp:ListItem Text="40000" Value="40000" />
                                                                <asp:ListItem Text="50000" Value="50000" />
                                                                <asp:ListItem Text="60000" Value="60000" />
                                                                <asp:ListItem Text="70000" Value="70000" />
                                                                <asp:ListItem Text="80000" Value="80000" />
                                                                <asp:ListItem Text="90000" Value="90000" />
                                                                <asp:ListItem Text="100000" Value="100000" />
                                                                <asp:ListItem Text="110000" Value="110000" />
                                                                <asp:ListItem Text="120000" Value="120000" />
                                                                <asp:ListItem Text="130000" Value="130000" />
                                                                <asp:ListItem Text="140000" Value="140000" />
                                                                <asp:ListItem Text="150000" Value="150000" />
                                                                <asp:ListItem Text="160000" Value="160000" />
                                                                <asp:ListItem Text="170000" Value="170000" />
                                                                <asp:ListItem Text="180000" Value="180000" />
                                                                <asp:ListItem Text="190000" Value="190000" />
                                                                <asp:ListItem Text="200000" Value="200000" />
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>--%>

                                                    <tr>
                                                        <td class="tdbkg">PA Cover for Paid Driver
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkPACoverPaidDriver" runat="server" AutoPostBack="true" OnCheckedChanged="chkPACoverPaidDriver_CheckedChanged"></obout:OboutCheckBox>
                                                        </td>

                                                        <td class="tdbkg">Number of Paid Drivers
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpNoofPaidDrivers" runat="server" Enabled="false" ToolTip="Readonly">
                                                                <asp:ListItem Text="0" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>


                                                        <td class="tdbkg">Sum Insured for Paid Driver
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpSIPaidDriver" runat="server" AllowEdit="false" Enabled="false" ToolTip="Readonly">
                                                                <asp:ListItem Text="0" Value="0" Selected="True" />
                                                                <asp:ListItem Text="10000" Value="10000" />
                                                                <asp:ListItem Text="20000" Value="20000" />
                                                                <asp:ListItem Text="30000" Value="30000" />
                                                                <asp:ListItem Text="40000" Value="40000" />
                                                                <asp:ListItem Text="50000" Value="50000" />
                                                                <asp:ListItem Text="60000" Value="60000" />
                                                                <asp:ListItem Text="70000" Value="70000" />
                                                                <asp:ListItem Text="80000" Value="80000" />
                                                                <asp:ListItem Text="90000" Value="90000" />
                                                                <asp:ListItem Text="100000" Value="100000" />
                                                                <asp:ListItem Text="110000" Value="110000" />
                                                                <asp:ListItem Text="120000" Value="120000" />
                                                                <asp:ListItem Text="130000" Value="130000" />
                                                                <asp:ListItem Text="140000" Value="140000" />
                                                                <asp:ListItem Text="150000" Value="150000" />
                                                                <asp:ListItem Text="160000" Value="160000" />
                                                                <asp:ListItem Text="170000" Value="170000" />
                                                                <asp:ListItem Text="180000" Value="180000" />
                                                                <asp:ListItem Text="190000" Value="190000" />
                                                                <asp:ListItem Text="200000" Value="200000" />
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td class="tdbkg">Wider Legal Liability to Paid Driver/Cleaner/Conductor-IMT 28 
                                                
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkWLLPD" runat="server" AutoPostBack="true" OnCheckedChanged="chkWLLPD_CheckedChanged" Checked="true"></obout:OboutCheckBox>
                                                        </td>
                                                        <td class="tdbkg">No Of Person Wider Legal Liability
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNoofPersonsWLL" runat="server" Enabled="false" ToolTip="Readonly" Text="1"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">Tenure For PA Cover Owner Driver</td>
                                                        <td>
                                                            <asp:DropDownList CssClass="drp" ID="drpTenureOwnerDriver" EnableViewState="true" runat="server" Enabled="true">
                                                                <asp:ListItem Text="0" Value="0" />
                                                                <asp:ListItem Text="1" Value="1" Selected="True" />
                                                                <asp:ListItem Text="3" Value="3" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdbkg">Legal Liability to Employees excluding paid driver/cleaner/conductor - IMT 29</td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkLLEE" runat="server" AutoPostBack="true" OnCheckedChanged="chkLLEE_CheckedChanged"></obout:OboutCheckBox>
                                                        </td>

                                                        <td class="tdbkg">No of Employees
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtNoOfEmployees" runat="server" Enabled="false" ToolTip="Readonly" Text="0"></obout:OboutTextBox>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </at1:AccordionPane>


                                        <at1:AccordionPane ID="accAOC" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected">
                                            <Header>Add On Covers</Header>
                                            <Content>
                                                <table id="table6" style="width: 100%;" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td class="tdbkg">Select all Non-tariff Add on covers as shown below</td>
                                                        <td colspan="5">
                                                            <asp:CheckBox ID="chkSelectAllCovers" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAllCovers_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                         <td class="tdbkg">Return to Invoice
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkReturnToInvoice" runat="server" OnCheckedChanged="chkReturnToInvoice_CheckedChanged" AutoPostBack="true" />
                                                        </td>
                                                        <td class="tdbkg">Engine Protect</td>
                                                        <td colspan="3">
                                                            <asp:CheckBox ID="chkEngineProtect" runat="server" OnCheckedChanged="chkEngineProtect_CheckedChanged" AutoPostBack="true" />
                                                        </td>
                                                       
                                                    </tr>

                                                    <tr>

                                                         <td class="tdbkg">Depreciation cover
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkDepreciationCover" runat="server" OnCheckedChanged="chkDepreciationCover_CheckedChanged" AutoPostBack="true" />
                                                        </td>


                                                       
                                                          <td class="tdbkg">Roadside Assistance
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkRoadsideAssistance" runat="server" />
                                                        </td>

                                                          <td class="tdbkg">Consumable Cover
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkConsumableCover" runat="server" />
                                                        </td>
                                                       

                                                    </tr>

                                                    <tr id="trNewAddOns" runat="server" visible="true">
                                                       
                                                        <td class="tdbkg">
                                                            
                                                            <span>
                                                                Loss of Personal Belongings SI
                                                            </span>
                                                            
                                                            
                                                        </td>

                                                         <td>
                                                            <asp:CheckBox ID="chkLossofPersonalBelongings" runat="server" OnCheckedChanged="chkLossofPersonalBelongings_CheckedChanged" AutoPostBack="true" /> 
                                                             <asp:DropDownList CssClass="drp" ID="ddlLossofPersonalBelongingsSI" EnableViewState="true" runat="server" Enabled="true">
                                                            </asp:DropDownList>
                                                        </td>

                                                         <td class="tdbkg">
                                                            
                                                            <span>
                                                                Key Replacement SI
                                                            </span>
                                                            
                                                            
                                                        </td>

                                                         <td>
                                                            <asp:CheckBox ID="chkKeyReplacement" runat="server" OnCheckedChanged="chkKeyReplacement_CheckedChanged" AutoPostBack="true" /> 
                                                             <asp:DropDownList CssClass="drp" ID="ddlKeyReplacement" EnableViewState="true" runat="server" Enabled="true">
                                                            </asp:DropDownList>
                                                        </td>

                                                         <td class="tdbkg">
                                                            
                                                            <span>
                                                                Daily Car Allowance SI
                                                            </span>
                                                            
                                                            
                                                        </td>

                                                         <td>
                                                            <asp:CheckBox ID="chkDailyCarAllowance" runat="server" Text="" OnCheckedChanged="chkDailyCarAllowance_CheckedChanged" AutoPostBack="true" /> 
                                                             <asp:DropDownList CssClass="drp" ID="ddlDailyCarAllowance" EnableViewState="true" runat="server" Enabled="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                          <td class="tdbkg" id="tdNCBProtectCover1" runat="server" visible="false">NCB Protect</td>
                                                        <td id="tdNCBProtectCover2" runat="server" visible="false">
                                                            <asp:CheckBox ID="chkNCBProtect" runat="server" />
                                                        </td>

                                                         <td class="tdbkg" id="tdTyreCover1" runat="server" visible="true">
                                                            <span>
                                                                Tyre Cover
                                                            </span>
                                                        </td>
                                                        <td colspan="3" id="tdTyreCover2" runat="server" visible="true">
                                                            <asp:CheckBox ID="chkTyreCover" runat="server" OnCheckedChanged="chkTyreCover_CheckedChanged" AutoPostBack="true" /> 
                                                            <obout:OboutTextBox ID="txtTyreCoverDetails" runat="server" MaxLength="100" Width="300px" placeholder="enter tyre details"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>
                                                   

                                                </table>
                                            </Content>
                                        </at1:AccordionPane>

                                        
                                        <at1:AccordionPane ID="accCreditScore" runat="server" HeaderCssClass="accordionHeader"
                                            HeaderSelectedCssClass="accordionHeaderSelected">
                                            <Header>Credit Score</Header>
                                            <Content>
                                                <table id="table25" style="width: 100%;" cellspacing="0" cellpadding="2">

                                                    
                                                     <tr>
                                                         <td colspan="6" style="background:lightgray"><obout:OboutCheckBox AutoPostBack="true" ID="chkIsGetCreditScore" runat="server" Text="Get Credit Score" OnCheckedChanged="chkIsGetCreditScore_CheckedChanged" Checked="true" Enabled="false"></obout:OboutCheckBox>
                                                        </td>
                                                        
                                                    </tr>

                                                     <tr>
                                                         <td class="tdbkg">Customer First Name
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtFirstName" runat="server" Enabled="true"></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg">Customer Middle Name</td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtMiddleName" runat="server" Enabled="true"></obout:OboutTextBox>
                                                        </td>
                                                         <td class="tdbkg">Customer Last Name</td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtLastName" runat="server" Enabled="true"></obout:OboutTextBox>
                                                        </td>
                                                    </tr>

                                                   
                                                    <tr><td class="tdbkg">Gender
                                                        </td>
                                                        <td>
                                                            <obout:OboutRadioButton Enabled="false" ID="rbtnMale" runat="server" Text="Male" GroupName="grpGender" Checked="true"></obout:OboutRadioButton>
                                                            <obout:OboutRadioButton Enabled="false" ID="rbtnFemale" runat="server" Text="Female" GroupName="grpGender"></obout:OboutRadioButton>
                                                        </td>

                                                           <td class="tdbkg">
                                                               <obout:OboutDropDownList ID="drpDrivingLicenseNumberOrAadhaarNumber" runat="server" Enabled="true">
                                                                 <asp:ListItem Value="PanNumber" Text="Pan Number"></asp:ListItem>
                                                                 <asp:ListItem Value="Passport" Text="Passport Number"></asp:ListItem>
                                                                 <%--<asp:ListItem Value="DrivingLicense" Text="Driving License Number"></asp:ListItem>--%>
                                                                 <%--<asp:ListItem Value="AADHAARNumber" Text="AADHAAR Number"></asp:ListItem>--%>
                                                                 <%--<asp:ListItem Value="VoterIDNumber" Text="Voter ID Number"></asp:ListItem>--%>
                                                             </obout:OboutDropDownList>
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox Enabled="true" ID="txtDrivingLicenseNumberOrAadhaarNumber" runat="server" placeholder="enter pan number"></obout:OboutTextBox>
                                                        </td>
                                                        </td>

                                                         <td class="tdbkg">Customer Mobile Nos
                                                             
                                                        </td>
                                                        <td>
                                                             <obout:OboutTextBox ID="txtMobileNumber" runat="server" Enabled="true" MaxLength="10"></obout:OboutTextBox>
                                                        </td>
                                                       

                                                    </tr>

                                                     <tr><td class="tdbkg">Customer DOB
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtCustomerDOB" runat="server" ReadOnly="false" Enabled="true"></obout:OboutTextBox>
                                                            <img src="images/calendar.png" alt="" id="datepickerImagedob" />
                                                        </td>

                                                           <td class="tdbkg">Customer Pin Code
                                                        </td>
                                                        <td>
                                                            <obout:OboutTextBox ID="txtPinCode" runat="server" Enabled="true"></obout:OboutTextBox>
                                                             <asp:HiddenField ID="hdnPinCode" runat="server" Value="0" />
                                                             <asp:HiddenField ID="hdnPinCodeLocality" runat="server" Value="" />
                                                            <asp:Button ID="btnGetPincodeDetails" runat="server" OnClick="btnGetPincodeDetails_Click" />
                                                        </td>
                                                        </td>

                                                          <td class="tdbkg">Pincode Locality
                                                        </td>
                                                        <td>
                                                           <asp:Label ID="lblPincodeLocality" runat="server" Text="-"></asp:Label>
                                                        </td>
                                                       

                                                    </tr>

                                                     <tr><td class="tdbkg">State
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
                                        <obout:OboutButton ID="btnViewPremium" runat="server" Text="View Premium" Width="100%"
                                            OnClick="btnViewPremium_Click" OnClientClick="return Reset();" />
                                    </div>



                                    <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
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
                                     <asp:Panel ID="pnlTest" runat="server"><div style="display: none"><table style="font-size: 8px" cellspacing="0" cellpadding="2" border="0"><tr style="background-color:gainsboro;color: dodgerblue;">
                                            <td style="font-size:9px">Quote Number</td>
                                            <td>IMD Code</td>
                                            <td>Customer Name</td>
                                            <td>Customer Gender</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size:9px"><asp:Label runat="server" ID="lblQuoteNumber" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblIMDCode" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblCreditScoreCustomerName" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblCustomerGender" Text="-"></asp:Label></td>
                                        </tr>
                                        <tr style="background-color:gainsboro;color: dodgerblue;">
                                            <td>Cover Type</td>
                                            <td>Ownership Type</td>
                                            <td>Policy Holder Type</td> 
                                            <td><asp:Label runat="server" ID="lblDORorDOP" Text="Purchase Date"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblCoverType" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblOwnershipType" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPolicyHolderType" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblRagistrationDate" Text="-"></asp:Label></td>
                                        </tr>
                                        <tr style="background-color:gainsboro;color: dodgerblue;">
                                            <td>RTO</td>
                                            <td>Make</td> 
                                            <td>Model</td>
                                            <td>Variant and Fuel Type</td>                                          
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblRTO" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblMake" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblModel" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblVariant" Text="-"></asp:Label> (<asp:Label runat="server" ID="lblFuelType" Text="-"></asp:Label>)</td>
                                        </tr>
                                        <tr style="background-color:gainsboro;color: dodgerblue;">
                                           <td>Cubic Capacity</td>
                                           <td>Seating Capacity</td>
                                           <td>Previous Policy Expiry Date-OD</td> 
                                           <td>Policy Start Date</td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblCubicCapacity" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblSeatingCapacity" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPreviousPolicyExpiryDate" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPolicyStartDate" Text="-"></asp:Label></td>
                                        </tr>
                                        <tr style="background-color:gainsboro;color: dodgerblue;">
                                           <td>Credit Score</td>                                          
                                           <td><asp:Label runat="server" ID="lblCustomerIDProof" Text=""></asp:Label></td>
                                           <td>Non Electrical Accessories</td>
                                           <td>Electrical Accessories</td>                                          
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblCreditScore" Text="0"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblCustomerIDProofNumber" Text="-"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblNonElectricalAccessoriesIDV" Text="0"></asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblElectricalAccessoriesIDV" Text="0"></asp:Label></td>
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
                                                 <li>The insurance coverage provided under this quote is only for Own Damage of the Vehicle and does not cover any other liability/third party liability in respect of the insured vehicle.</li>
                                                 <li>Customer needs to ensure that there is a valid TP cover at all times.</li>
                                             </ul>
                                         </div>
                                </asp:Panel>

                                    <table id="table26" style="width: 37.5%;text-align: left;font-size: 11px;" cellspacing="0" cellpadding="2" border="1" valign="top">
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
                                    <div style="float:left"> 
                                        <div style="float:left"> <asp:TextBox ID="txtEmailId" placeholder="Please Enter Emailids" runat="server" Text="" BorderStyle="Solid" style="border:1px solid" Width="250px" Height="25px"></asp:TextBox>
                                            <caption>
                                                <span style="font-size:10px;padding-left:8px">For Multiple Email Ids Use Semicolon (;)</span>
                                            </caption>
                                        </div>
                                        <br />
                                        <br />
                                        
                                         <div style="float:left;padding-top:3px">
                                             <asp:TextBox ID="txtRemarks" placeholder="Enter Remarks If Any" runat="server" Text="" style="border:1px solid" Width="250px" Height="25px"></asp:TextBox>
                                         </div>
                                        
                                        <div style="float:left;padding-left: 10px;"><asp:Button ID="btnSendPDF" class="btn btn-primary" runat="server" Text="Email Quote" OnClick="btnSendPDF_Click" /> </div>
                                    </div>
                                    <asp:Button ID="btnDownloadPremiumBreak" OnClientClick="setFormSubmitting();" class="btn btn-primary" runat="server" Text="Download PDF" OnClick="btnDownloadPremiumBreakup_Click"  />
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


