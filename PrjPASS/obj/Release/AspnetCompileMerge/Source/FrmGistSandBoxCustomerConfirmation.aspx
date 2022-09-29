<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmGistSandBoxCustomerConfirmation.aspx.cs" Inherits="PrjPASS.FrmGistSandBoxCustomerConfirmation" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="description" content="Bootstrap Admin App + jQuery" />
    <meta name="keywords" content="app, responsive, jquery, bootstrap, dashboard, admin" />
    <!-- =============== VENDOR STYLES ===============-->
    <!-- FONT AWESOME-->
    <link rel="stylesheet" href="css/newcssjs/fontawesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/newcssjs/bootstrap.css" id="bscss" />
    <!-- =============== APP STYLES ===============-->
    <link rel="stylesheet" href="css/newcssjs/app_old.css" id="maincss" />

    <style>
        .spinnermodal {
            background-color: #FFFFFF;
            height: 100%;
            left: 0;
            opacity: 0.5;
            position: fixed;
            top: 0;
            width: 100%;
            z-index: 100002;
        }

        .loading-gif {
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -50px 0px 0px -50px;
        }

        .footer-agileits a {
            color: #e64942;
            font-family: 'Lobster', cursive;
        }
    </style>

</head>


<body>
    <script src="http://localhost:56888/ApplicationScripts/GenericScripts/jsApplicationGenericScripts.js"></script>
    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>


    <%--  <script src="css/newcssjs/js/jquery-1.7.1.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.min.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>--%>
    <%--<script src="css/newcssjs/js/circular-countdown.min.js"></script>--%>
    <style>
        .secspan {
            position: absolute;
            left: 50%;
            z-index: 999;
            top: 49%;
            transform: translateX(-50%);
            -webkit-transform: translateX(-50%);
            -o-transform: translateX(-50%);
            -moz-transform: translateX(-50%);
            -ms-transform: translateX(-50%);
            color: #696969;
        }

        /* line 12, ../scss/imports/_timer.scss */
        .timerimg {
            position: absolute;
            z-index: 99;
            left: 0;
        }

        .timercountTxt {
            background: rgba(0, 0, 0, 0) none repeat scroll 0 0;
            color: red !important;
            font-size: 1.6rem;
            border: none !important;
            width: 100%;
            text-align: center;
            padding-bottom: 5px;
        }

        /*.otpPanel {
  width: 400px;
  padding: 20px;
  border: 1px solid #ccc;
  margin: 20px 0;
  float: left;
  clear: both; }*/

        .otpPanel .otpButton {
            margin: 20px 0 0 0;
        }

        .otpPanel p {
            margin: 0;
        }
        /* line 437, ../scss/imports/_motorinsurance.scss */
        .otpPanel .inputBox input {
            padding: 10px 15px 5px 0;
        }


        .timer {
            width: 84px;
            height: 84px;
            margin: auto;
            font-family: 'source_sans_probold';
        }


        .error {
            font-size: 1.3rem;
            color: red;
            float: left;
            clear: both;
            padding-top: 5px;
        }

        /*.btn {
            border-radius: 8px;
            color: #bbaf99;
            float: left;
            position: relative;
            font-size: 1.6rem;
            padding: 15px 30px;
            margin: 10px 0;
        }*/

        .btn.btn-blue {
            background: #013976 !important;
            color: #fff !important;
            transition: all 0.2s ease-in;
            -webkit-transition: all 0.2s ease-in;
            -moz-transition: all 0.2s ease-in;
        }

        .link-red {
            color: #ec1c24 !important;
        }

        .popUp {
            background: #fff;
            display: none;
            width: 100%;
            position: absolute;
            top: 0;
            left: 0;
            z-index: 99;
            padding-top: 40px;
            height: 100%;
        }

            .popUp .close-icon {
                background: url(Images/close-icon.png) no-repeat;
                width: 24px;
                height: 24px;
                position: absolute;
                right: 10px;
                top: 10px;
            }

        .container {
            width: 100%;
            max-width: 1350px;
            margin: 0 auto;
            padding: 0 15px;
            position: relative;
            height: 100%;
        }
            /* line 15, ../scss/imports/_main.scss */
            .container.container2 {
                padding: 0px;
            }

        .row-grid {
            float: left;
            width: 100%;
            padding: 25px 0px;
            text-align: left;
            position: relative;
        }

        /*.mob-text-center {
    left: 0!important;
    translate: transform(0, 0) !important;
    -moz-translate: transform(0, 0) !important;
    -webkit-translate: transform(0, 0) !important;
    -o-translate: transform(0, 0) !important;
    -ms-translate: transform(0, 0) !important; }*/

        .link-red {
            color: #ec1c24 !important;
        }

        .link.align-center.underline.callPopup {
            clear: both;
        }

        .banner-content {
            position: relative;
            width: 100%;
            float: left;
            color: #2d2d2d;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            padding: 0;
            z-index: 0;
        }
            /* line 415, ../scss/imports/_main.scss */
            .banner-content h2 {
                text-align: center;
                margin-bottom: 10px;
                font-family: Lato-Regular;
                font-size: 2.5em;
                clear: both;
            }
        .auto-style1 {
            width: 1280px;
            height: 24px;
        }
        .auto-style2 {
            width: 1244px;
            height: 584px;
        }
        .auto-style3 {
            color: #000000;
        }
    </style>

<%--    <script type="text/javascript">

        var vUserServiceUrl = 'http://localhost:59715/';
        var vGenericServiceUrl = 'http://localhost:62354/';
        var vPrivateCarServiceUrl = 'http://localhost:58729/';
        var vGenericServiceName = 'wsGenericManagementServices.svc';
        var vMotorQuoteServiceName = 'wsPrivateCarManagementServices.svc';
        var objRenewalPolicyInformation = {};
        var objPrivateCarRequestXMLPremiumData = {};
        var objNewProposalDetails = {};
        var objNBDetails = {};
        var objNewPolicyDetails = {};
        var PolicyNumber = 'NA';
        var CRNNumber = '';

        $(function () {
            callpopup();
        });


        $(function () {

            try {

                PolicyNumber = getUrlVars()["vPolicyno"];
                CRNNumber = getUrlVars()["CRNNumber"];

                if (PolicyNumber != null) {
                    if (CRNNumber != null) {

                        if (CRNNumber != "") {
                            $("#btnMakePayment").hide();
                            $("#divNBOptions").show();
                            GetCustomerAccountInformation();
                        }
                        else {
                            $("#btnMakePayment").show();
                            $("#divNBOptions").hide();
                            $("#btnDebitNetbankingAccount").hide();
                        }

                    }
                    else {
                        $("#btnMakePayment").show();
                        $("#divNBOptions").hide();
                        $("#btnDebitNetbankingAccount").hide();
                    }
                }
            } catch (e) {
                //OnError(e.message);
            }

            //$("#dialog1").dialog({
            //    autoOpen: false
            //});

            //$("#opener").click(function () {
            //    $("#dialog1").dialog('open');
            //});

         
            $("#btnDebitNetbankingAccount").click(function () {

                if ($('#chkSummaryAgree').is(":checked")) {
                    GetPolicy();
                }
                else
                {
                    alert("kindly read and agree to terms and conditions");
                }
            });

            $("#accountNumbers").change(function () {
                $("#lblAccountBalance").text(objNBDetails.AccountBalance[this.selectedIndex]);
            });

            $('input[type=radio][name=PaymentOptions]').change(function () {
                $("#lblOption1").removeClass();
                $("#lblOption2").removeClass();

                if (this.value == 'Account') {
                    $("#lblOption1").addClass('btn btn-primary active focus');
                    $("#lblOption2").addClass('btn btn-default');

                    $("#divCardNumbers").hide();
                    $("#divAccountNumbers").show();
                    $("#divAccountBalance").show();
                    $("#btnDebitNetbankingAccount").show();
                    $("#btnMakePayment").hide();
                }
                else if (this.value == 'Card') {
                    $("#lblOption1").addClass('btn btn-default');
                    $("#lblOption2").addClass('btn btn-primary active focus');

                    $("#divCardNumbers").show();
                    $("#divAccountNumbers").hide();
                    $("#divAccountBalance").hide();
                    $("#btnDebitNetbankingAccount").show();
                    $("#btnMakePayment").hide();
                }
            });



        });
        //$("#dialog").dialog({ autoOpen: false, modal: true, buttons: { "Ok": function () { $(this).dialog("close"); } } });
        function callpopup() {
            $(document).on("click", ".callPopup", function () {
                var popname = $(this).attr('data-popupname');
                $('.banner-content').addClass('popupActive');
                $(window).scrollTop(0)
                $('.overlay').fadeIn(300);
                $('.' + popname).fadeIn(300);

                if (popname == "detail-breakup-enhanced" || popname == "detail-breakup-basic") {
                    $('.' + popname).find('.accord-wrap .accord').eq(0).addClass('active').siblings().removeClass('active');
                    $('.' + popname).find('.accord-wrap .accord').eq(0).find('.accord-content').show();
                }
            });
            $(document).on("click", ".close-icon", function () {
                $('.overlay, .popUp').fadeOut(300);
                $('.banner-content').removeClass('popupActive');
            });
        }

       
        function jsFn_CustomerAccountInformation(objServiceResult, vSuccessParaString) {
            hideKotakGenericLoader();
            try {
                //window.prompt(JSON.stringify(objServiceResult), JSON.stringify(objServiceResult));

                objNBDetails = objServiceResult;

                if (objServiceResult.ErrorMessage == '') {

                    //hideKotakGenericLoader();

                    var listItems_AccountNumbers = "";
                    for (var i = 0; i < objServiceResult.AccountNumbers.length; i++) {
                        listItems_AccountNumbers += "<option value='" + objServiceResult.AccountNumbers[i] + "'>" + objServiceResult.AccountNumbers[i] + "</option>";
                    }
                    $("#accountNumbers").html(listItems_AccountNumbers);
                    $("#lblAccountBalance").text(objNBDetails.AccountBalance[0]);

                    var listItems_CardNumbers = "";
                    for (var i = 0; i < objServiceResult.CardNumbers.length; i++) {
                        listItems_CardNumbers += "<option value='" + objServiceResult.CardNumbers[i] + "'>" + objServiceResult.CardNumbers[i] + "</option>";
                    }
                    $("#cardNumbers").html(listItems_CardNumbers);
                }
                else {
                    OnError(objServiceResult.ErrorMessage);
                }

            } catch (e) {
                OnError(e.message);
            }
        }

        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function jsFn_On_Get_Options_Failed(objServiceResult) {

        }

        function GetPolicy() {
            try {
                sessionStorage.setItem("vEncryptedLogin", "Q2FqMVlmYWd5bmk1aHhlZWhFV2tIQT09");
                sessionStorage.setItem("vEncryptedPassword", "eTBUakJhZ1d4Q3BNeWR6ZlV5MnVXdz09");
                sessionStorage.setItem("vRanKey", "5447997681446268");

                var SelectedCardNumber = "NONE";
                var SelectedAccountNumber = "NONE";
                var IsCard = "false";
                var PaymentOptions = $('input[name=PaymentOptions]:checked').val();
                if (PaymentOptions == "Card") {
                    IsCard = "true";
                    SelectedCardNumber = $("#cardNumbers").val();
                }
                else if (PaymentOptions == "Account") {
                    SelectedAccountNumber = $("#accountNumbers").val();
                }
                else {
                    return false;
                }

                if (PaymentOptions == "Card" || PaymentOptions == "Account") {

                    var hdnProductCode = $("#" + '<%= hdnProductCode.ClientID %>').val();
                    var vProposalNumber = $("#" + '<%= lblProposalText.ClientID %>').text();
                    var vCustomerId = $("#" + '<%= lblCustomerID.ClientID %>').text();
                    var vTotalPremium = $("#" + '<%= lblTotalPremium.ClientID %>').text();

                    var vSuccessParaString = "";
                    vPremiumFunctionName = 'Fn_CustomerAccountDebitAndCreatePolicy';
                    vCallType = 'POST';
                    vParameterString = '/GC0062/' + vProposalNumber + '/' + vCustomerId + '/' + vTotalPremium + '/' + CRNNumber + '/' + SelectedAccountNumber + '/' + SelectedCardNumber + '/' + IsCard + '/' + hdnProductCode;
                    oData = null; //JSON.stringify(postCustomerData);
                    vContentType = 'application/json; charset=utf-8';
                    vDataType = 'json'
                    showKotakGenericLoader();
                    js_Fn_Call_Service_With_Token(vGenericServiceUrl, vCallType, vGenericServiceName, vPremiumFunctionName, vParameterString, oData, vContentType, vDataType, jsFn_CustomerPolicyDetails, vSuccessParaString, jsFn_On_Get_Options_Failed);
                }

            } catch (e) {
                OnError(e.message);
            }
        }

        function jsFn_CustomerPolicyDetails(objServiceResult, vSuccessParaString) {

            try {
                //window.prompt(JSON.stringify(objServiceResult), JSON.stringify(objServiceResult));
                objNewPolicyDetails = objServiceResult;
                if (objServiceResult.ErrorMsg == "Success") {
                    $("#divSuccess").show();
                    $("#divFailure").hide();

                    $("#lblPolicyNumber").text(objServiceResult.PolicyNumber);
                    $("#lblBankReferenceNumber").text(objServiceResult.BankReferenceNo);

                    if (objServiceResult.PolicyNumber != null) {

                        if (objServiceResult.PolicyNumber != "") {
                            $("#btnDownloadNewPolicy").show();
                        }
                        else {
                            $("#btnDownloadNewPolicy").hide();
                        }
                    }
                    else {
                        $("#btnDownloadNewPolicy").hide();
                    }
                }
                else {
                    $("#divFailure").show();
                    $("#divSuccess").hide();


                    $("#lblProposalNumberFailed").text(objServiceResult.ProposalNumber);
                    $("#lblBankReferenceNumberFailed").text(objServiceResult.BankReferenceNo);
                    $("#lblRemarksFailed").text(objServiceResult.ErrorMsg);

                    if (objServiceResult.IsPaymentDebited) {
                        $("#lblPaymentStatusCaption").text("Payment Status!!!");
                        $("#lblOopsMsg").text("Oops! your payment was successful but policy could not created.");
                    }
                    else {
                        $("#lblPaymentStatusCaption").text("Payment Failed!!!");
                        $("#lblOopsMsg").text("Oops! It looks like something went wrong and the payment wasn't processed.");
                    }
                }

                $("#divNBOptions").hide();
                $("#divReview").hide();

                hideKotakGenericLoader();

            } catch (e) {
                alert(e.message);
            }
        }

        function jsFn_Get_Policy_PDF(vProposalNumber, vPolicyNumber, vProductCode) {

            sessionStorage.setItem("vEncryptedLogin", "Q2FqMVlmYWd5bmk1aHhlZWhFV2tIQT09");
            sessionStorage.setItem("vEncryptedPassword", "eTBUakJhZ1d4Q3BNeWR6ZlV5MnVXdz09");
            sessionStorage.setItem("vRanKey", "5447997681446268");

            showKotakGenericLoader();
            var vSuccessParaString = "";

            vFunctionName = 'Fn_Get_Policy_PDF';
            vCallType = 'GET',
            vParameterString = '/' + vProposalNumber + '/' + vPolicyNumber + '/' + vProductCode + '/GC0062';
            oData = null;
            vContentType = 'application/json; charset=utf-8';
            vDataType = 'json'

            js_Fn_Call_Service_With_Token(vGenericServiceUrl, vCallType, vGenericServiceName, vFunctionName, vParameterString, oData, vContentType, vDataType, jsFn_Download_Policy, vPolicyNumber, jsFn_On_Get_Options_Failed);

        }

        function jsFn_Download_Policy(objServiceResult, vPolicyNumber) {
            hideKotakGenericLoader();
            if (objServiceResult == "Invalid Token Identified, System will not proceed ahead!") {
                OnError(objServiceResult);
            }
            else {
                var sampleArr = jsFn_Base64_To_ArrayBuffer(objServiceResult);
                jsFn_Save_Byte_Array(vPolicyNumber, sampleArr, "pdf");
            }
        }

        function OnError(err_message) {
            $("#divServiceErrors").show();
            $("#lblServiceErrorResponseMsg").text(err_message);
            $("#divReview").hide();
            $("#divNBOptions").hide();
            $("#btnDebitNetbankingAccount").hide();
            $("#divFailure").hide();
            $("#divSuccess").hide();
        }

    </script>--%>


    <form id="form1" runat="server">
            <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />

                    <asp:HiddenField ID="hdnvCustomerId" runat="server" ClientIDMode="Static" Value="0" />
                    <asp:HiddenField ID="hdnvMobileNo" runat="server" ClientIDMode="Static" Value="0" />
                    <asp:HiddenField ID="hdnvPolicyNo" runat="server" ClientIDMode="Static" Value="0" />
                    <asp:HiddenField ID="hdnvEmail" runat="server" ClientIDMode="Static" Value="0" />

            <asp:ScriptManager runat="server"></asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel_Detail1" runat="server" UpdateMode="Conditional"
            ClientIDMode="Static">
            <ContentTemplate>
                


        <div class="spinnermodal" id="PreLoaderKotakGeneric" style="display: none; z-index: 10001">
            <div class="loading-gif">
                <img src="../../images/KotakLogo.gif" alt="Loading..." />
            </div>
        </div>

        <div class="wrapper">



            <!-- top navbar-->
            <header class="topnavbar-wrapper">
                <!-- START Top Navbar-->
                <nav role="navigation" class="navbar topnavbar">
                    <!-- START navbar header-->
                    <div class="navbar-header">
                        <a href="#/" class="navbar-brand">
                            <div class="brand-logo">
                               <%-- <img src="./Images/logo.png" alt="App Logo" class="img-responsive" />--%>

                                  <img src="./Images/logo1.jpg" style="height: 70px; width: 230px" alt="">
                            </div>
                            <div class="brand-logo-collapsed">
                              <%-- <img src="./Images/logo.png" alt="App Logo" class="img-responsive" />--%>

                             <%--     <img src="./Images/logo1.jpg" style="height: 70px; width: 230px" alt="">--%>
                            </div>
                        </a>
                    </div>
                    <!-- END navbar header-->
                    <!-- START Nav wrapper-->
                    <div class="nav-wrapper">
                        <!-- START Right Navbar-->
                        <ul class="nav navbar-nav navbar-right">
                          <%--  <li>
                                <a href="mailto://care@kotak.com" class="email"><em class="fa fa-shield"></em>
                                    <asp:Label ID="lblProductNameVal" runat="server" Text="-"></asp:Label></a>
                            </li>--%>
                            <li>
                                <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em>care@kotak.com</a>
                            </li>

                            <li>
                                <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em>1800 266 4545</a>
                            </li>

                        </ul>
                        <!-- END Right Navbar-->
                    </div>
                    <!-- END Nav wrapper-->

                </nav>
                <!-- END Top Navbar-->
            </header>

       <section>
                <div class="content-wrapper" id="divReview">
                    <div class="container container-md" style="left: 24px; top: 3px; width: 98%; height: 1067px">
                        <div class="row mb-lg">
                            <div class="col-md-12">
                                <div>
                                    <div style="color: black;">

               



   <div id="acc1collapse1" class="panel-collapse collapse in" aria-expanded="true">
<div><center><h3><u>Consent Form </u></h3></center></div>

<div><h4>Sandbox Proposal No-140- “Wearable Device to Existing Customers”</h4></div>
       <%--<center>Sandbox proposal No-140- “Wearable Device to Existing Customers”</center>--%>
<div><h4>Name of Product- Kotak Health Care (UIN No- KOTHLIP19059V021819) </h4></div>

<p>
    </p>
<div>I,<strong><asp:Label ID="lblCustomerName" runat="server" style="color:black" /> </strong>, S/D/W of _______________________  acknowledge that I have fully gone through the <asp:LinkButton ID="btnTerms" runat="server" OnClientClick="window.open('https://www.kotakgeneralinsurance.com/health-insurance-wearable-device'); return false;"><u>Benefit Illustration</u> </asp:LinkButton> of the proposal. I acknowledge that I understood how the proposal will work, what are the coverages provided to the participating policyholders, what are the information to be shared with the insurers and what are the benefits available to the participating policyholders. </div>
<p>
    </p>
<div>I understood that the proposal is introduced as an experiment to test a new idea under the proposal of Regulatory Sandbox. I understood that the outcome of the experiment is unknown. I acknowledge that my participation in this experiment/proposal is entirely voluntary and conscious choice. </div>
<p>
    </p>
<div>I understood that once the experiment is complete or on earlier termination or withdrawal thereof, the insurer may discontinue the product; however, insurer will ensure the fulfillment of any existing obligations to policyholders. The continuance of the product beyond the testing period will be subject to IRDAI approval. </div>

<p>
    </p>
<div>I hereby give my informed consent and willingness to participate in the experiment after reading the terms and conditions of the proposal. </div>
<p>
    </p>
<h4>Signature of the proposer</h4>

<strong>
<div>Date:<asp:Label ID="lbldate" runat="server" style="color:black" /></div>

<div>Place:<asp:Label ID="lblPlace" runat="server" style="color:black" /></div>
</strong>
                                         
                                          
             
                                    </div>
                                </div>
                            </div>
                        </div>

       
                                                <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>




                        <center>
                                        <%--<div class="checkbox c-checkbox needsclick" id="agreewithbtn">--%>
                                            <div class="form-group">
                                                <input type="checkbox" value="" class="needsclick" id="chkSummaryAgree" runat="server" />
                            <%-- <span class="fa fa-check"></span>--%>                       
<%--                        <label>I agree to the <a class="link-red callPopup" target="_self" href="javascript:;">Terms &amp; Conditions</a></label> --%>
                                                <asp:LinkButton ID="LinkbtnTerms2" runat="server" OnClientClick="window.open('https://www.kotakgeneralinsurance.com/health-insurance-wearable-device'); return false;"><u>Terms &amp; Conditions</u></asp:LinkButton>
                         <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please select terms and conditions."             
            ForeColor="Red" onservervalidate="CustomValidator1_ServerValidate" Width="100%" BorderWidth="0"></asp:CustomValidator>  
                                            </div>
                                        <%--</div>--%>
                                        
                                         <asp:Button ID="btnMakePayment" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  ClientIDMode="Static" Text="Send OTP" OnClick="btnMakePayment_Click" Height="26px" Visible="false"/>
                                         <asp:Button ID="btnConfirm" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Confirm" OnClick="btnConfirm_Click" />
                                        <br />
                                        <div class="row">
                                            <div class="otpPanel align-center text-center" id="otpPanel" visible="false" runat="server">
                                                <asp:TextBox ID="txtTimer" ClientIDMode="Static" Style="display: none" ReadOnly="true" runat="server" Text="" CssClass="timercountTxt"></asp:TextBox>  
                                                <!-- TIMER STARTS HERE -->
                                                <br />
                                                <div class="timer" id="otptimer" style="display:none">
                                                    <span class="secspan">sec</span>
                                                </div>
                                                <!-- TIMER ENDS HERE -->
                                                <p>Please enter the One Time Password (OTP) sent on your mobile number</p>
                                                <div class="inputBox">
                                                    <asp:TextBox ID="txtOtpNumber" runat="server" CssClass="txtOtpNumber" Text="" MaxLength="6" /><br />
                                                     <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ValidationGroup="otp" Display="Dynamic" 
                                                    ErrorMessage="Please provide valid otp number" ClientValidationFunction="fnValidateOTPNumber" OnServerValidate="OnServerValidatecvtxtOtpNumber" />

                                                  <%--  <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ValidationGroup="otp" Display="Dynamic" 
                                                    ErrorMessage="Please provide valid otp number" ClientValidationFunction="fnValidateOTPNumber" OnServerValidate="OnServerValidatecvtxtOtpNumber" />--%>
                                                </div><br />
                                                <div class="otpButton align-center">
                                                    <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClick="onClickbtnMobileVerify" />
                                                    <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Resend OTP" ValidationGroup="summary" OnClick="onClickbtnMobileReSend" />
                                                </div>
                                            </div>
                                        </div>
                     
                         
    </center>



                                       
                    </div>
                </div>


            

            </section>



            <!-- Page footer-->
            <footer style="background-color: white">
                <p>
                    <center>
                    <span style="font-size:9px;text-align:center">
                        Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                    </span>
                </center>
                 
                    <p>
                    </p>

                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>

                    <p>
                    </p>

                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                    <p>
                    </p>


                </p>
            </footer>

        </div>
        <%--<button id="opener">open the dialog</button>--%>
        <%--<div id="dialog1" title="Dialog Title" hidden="hidden">I'm a dialog</div>--%>


        <%-- <a data-toggle="modal" data-target="#myModal"  href="#">I want to change my details</a>--%>
        <%--<button type="button" data-toggle="modal" data-target="#myModal" class="btn btn-info">Default modal</button>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
