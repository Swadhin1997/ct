<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSTPRenewal.aspx.cs" Inherits="PrjPASS.FrmSTPRenewal" MaintainScrollPositionOnPostback="true" %>

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
    <link rel="stylesheet" href="css/newcssjs/app_stp.css" id="maincss" />
    <link href="css/newcssjs/jquery-ui.css" rel="stylesheet" />
</head>


<body style="background-color: white;">

    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>


    <%--  <script src="css/newcssjs/js/jquery-1.7.1.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.min.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>--%>
    <%--<script src="css/newcssjs/js/circular-countdown.min.js"></script>--%>
    <style>
        .secspan {
            position: absolute;
            left: 42%;
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
            background: #f0fcf8;
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

        .txtclass {
            /*font-family: Tahoma,Arial, Helvetica, sans-serif;
            font-size: 16px;
            color: #000000;
            font-weight: normal;
            text-decoration: none;
            border: 1px solid #888888;
            border-radius: 4px;*/
            height: 25px;
            padding: 2px 7px;
            font-size: 13px;
            box-shadow: 0 0 0 #000 !important;
            line-height: 1.52857143;
            color: #3a3f51;
            background-color: #fff;
            background-image: none;
            border: 1px solid #dde6e9;
            border-radius: 4px;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

        .BRMinfoStyle {
            position: relative;
            top: 20px;
        }

        .InnerTextBox {
            position: relative;
            top: -4px;
        }
    </style>
    <script>
        // Rechecking
        $(function () {
            callpopup();

            $("#dialog1").dialog({
                autoOpen: false
            });

            $("#opener").click(function () {
                $("#dialog1").dialog('open');
            });
        });

        $("#dialog").dialog({ autoOpen: false, modal: true, buttons: { "Ok": function () { $(this).dialog("close"); } } });

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
    </script>

    <form id="form1" runat="server">

        <asp:HiddenField ID="hdnAccountNumber" runat="server" Value="" />
        <asp:HiddenField ID="hdnIsKLTEmployee" runat="server" Value="0" />
        <asp:HiddenField ID="hdnExpiringPolicyNumber" runat="server" Value="0" />

        <div id="BRMCodeModel" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">IMD Code.</h4>
                    </div>
                    <div class="modal-body">
                        <p>To be left as 0000000000 if buying directly. If buying through KGI BRM, please input BRM INTERMEDIARY CODE</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
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
                                <img src="Images/logo1.png" alt="App Logo" class="img-responsive" />
                            </div>
                            <div class="brand-logo-collapsed">
                                <img src="Images/logo1.png" alt="App Logo" class="img-responsive" />
                            </div>
                        </a>
                    </div>
                    <!-- END navbar header-->
                    <!-- START Nav wrapper-->
                    <div class="nav-wrapper">

                        <!-- START Right Navbar-->
                        <ul class="nav navbar-nav navbar-right">
                            <li>
                                <a href="https://kgipass.kotakmahindrageneralinsurance.co.in/KGIPASS/downloads/STP_Brochure.pdf" target="_blank"><em class="fa fa-download"></em>&nbsp;Download Brochure</a>
                            </li>

                            <li>
                                <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em>&nbsp;care@kotak.com</a>
                            </li>

                            <li>
                                <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em>&nbsp;1800 266 4545</a>
                            </li>

                        </ul>
                        <!-- END Right Navbar-->
                    </div>
                    <!-- END Nav wrapper-->

                </nav>
                <!-- END Top Navbar-->
            </header>


            <section id="sectionMain">
                <div class="content-wrapper">
                    <div class="container container-md">
                        <div class="row mb-lg">
                            <div class="col-lg-12" style="text-align: center;">

                                <div>
                                    <span class="h3 text-bold">Welcome</span>
                                    <asp:Label ID="lblEmployeeName" runat="server" Text="" CssClass="h3 text-bold"></asp:Label>
                                    <span class="h3 text-bold">(</span><asp:Label ID="lblEmployeeCode" runat="server" Text="" CssClass="h3 text-bold"></asp:Label><span class="h3 text-bold">)</span>

                                    Email: <b>
                                        <asp:Label ID="lblEmpEmailId" runat="server" Text=""></asp:Label></b> | Mobile: <b>
                                            <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label></b>
                                </div>

                                <p class="text-bold" id="P1" runat="server" style="margin:0 0 0">
                                    Thank you for choosing <span style="color:red;">Kotak General Insurance</span> as your preferred insurance partner.
                                </p>
                                <p class="text-bold" id="P2">
                                    We would like to remind you that your <span style="color:red;">Kotak Health Super Top-Up insurance Policy No.</span>
                                    <asp:Label ID="lblExpiringPolicyNumber" runat="server" Text="" Style="font-size: 18px;color:red;"></asp:Label>
                                    is due for renewal. Your policy expires on <asp:Label ID="lblPolicyExpiryDate" runat="server" Text="" Style="font-size: 18px;color:red;"></asp:Label>, Renew your policy now and stay protected!
                                </p>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12">
                                <!-- START panel-->
                                <div id="panelDemo14" class="panel panel-default" style="border-top-width: 2px;">
                                    <div class="panel-heading text-center" style="background-color: #ff902b; color: white; font-weight: bold;">Verify &amp; Fill Required Details Below</div>
                                    <div class="panel-body">
                                        <div role="tabpanel">
                                            <!-- Nav tabs-->
                                            <ul role="tablist" class="nav nav-tabs">
                                                <asp:Literal ID="LiteralTabNavigation" runat="server" Text=""></asp:Literal>
                                            </ul>
                                            <!-- Tab panes-->
                                            <div class="tab-content" id="divTabContent1">
                                                <asp:Literal ID="LiteralTabContent" runat="server" Text=""></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- END panel-->
                            </div>
                        </div>
                        <div class="block-center mt-xl wd-xl">
                            <div class="row">
                         
                                <asp:HiddenField ID="hdnMonthlyPremiumAmount" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnMonthlyGST" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnMonthlyTotalPayable" runat="server" Value="0" />

                                <asp:HiddenField ID="hdnYearlyPremiumAmount" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnYearlyGST" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnYearlyTotalPayable" runat="server" Value="0" />

                                    <div class="panel panel-primary">
                                        <div class="panel-heading bb" style="text-align: center; background-color: #122858fc">
                                            <h4 class="panel-title" style="font-weight: normal;">Deductible 
                                                <asp:Label ID="lblDeductible" runat="server" Text="0.00"></asp:Label>
                                                lacs | 
                                                <strong>Sum Insured
                                                <asp:Label ID="lblSumInsured" runat="server" Text="0.00"></asp:Label>
                                                    lacs
                                                </strong>|
                                                <asp:Label ID="lblFamilyFloater" runat="server" Text=""></asp:Label>
                                            </h4>
                                        </div>
                                        <table class="table">
                                            <tbody>
                                                <tr>
                                                    <td>Select Payment Option
                                                    </td>
                                                    <td>
                                                        <div class="text-right text-bold">

                                                            <div class="">
                                                                <label style="padding-left: 0px; text-decoration: underline;"></label>
                                                                <label class="radio-inline c-radio">
                                                                    <input id="inlineradioMonthly" type="radio" name="radioPaymentOption" value="Monthly" />
                                                                    <span class="fa fa-check"></span>Monthly</label>
                                                                <label class="radio-inline c-radio">
                                                                    <input id="inlineradioYearly" type="radio" name="radioPaymentOption" value="Yearly" checked="checked" />
                                                                    <span class="fa fa-check"></span>Yearly</label>
                                                            </div>

                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Premium Amount</td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                            <asp:Label ID="lblPremiumAmount" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>GST @ 18%</td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <asp:Label ID="lblGST" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Amount Payable
                                                    </td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <asp:Label ID="lblAmountPayable" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                               
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mv-lg pv-lg text-dark text-center">
                    <a class="link" data-toggle="modal" data-target="#ModalHowDeductibleWorks" href="#" style="font-size: 20px">How does deductible work?</a>
                </div>


                <h3 class="mv-lg pv-lg text-dark text-center">
                    <asp:Label ID="lblSelectedPremiumMessage" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblSelectedPremium" runat="server" Text="0.00"></asp:Label>
                    <br />
                    <span id="BRMinfo" class="BRMinfoStyle">
                        <button type="button" class="mb-sm btn-xs btn btn-primary btn-outline" data-toggle="modal" data-target="#BRMCodeModel">
                            IMD Code</button>
                        <span class="InnerTextBox">
                            <%--<asp:TextBox ID="txtEcode" Text="GI" runat="server" Width="30px" ReadOnly CssClass="txtclass" />--%>
                            <asp:TextBox ID="txtBRMCode" name="IMD Code" Text="0000000000" runat="server" MaxLength="10" Width="110px" CssClass="txtclass" />
                        </span>
                        <br />
                    </span>
                    <br />
                </h3>


                <center>
                
              <%--  <div class="checkbox c-checkbox needsclick" id="agreewithbtn">
                    
                    <label>
                        <input type="checkbox" value="" class="needsclick" id="chkSummaryAgree" runat="server" />
                        <span class="fa fa-check"></span>  
                    </label>
              
                </div>--%>
                    <div class="form-group" id="divDeclarofGoodHealth">
                           <div class="">
                              <label style="padding-left:0px;text-decoration:underline;"><a class="link-red" data-toggle="modal" data-target="#myModal" href="#">Declaration Of Good Health</a></label>
                                <label class="radio-inline c-radio">
                                 <input id="inlineradioYes" type="radio" name="radioDclrofgdhlth" value="option1" />
                                 <span class="fa fa-circle"></span>Yes</label>
                              <label class="radio-inline c-radio">
                                 <input id="inlineradioNo" type="radio" name="radioDclrofgdhlth" value="option2" />
                                 <span class="fa fa-circle"></span>No</label>
                           </div>
                       
                        </div>

                    <span id="lblMessageifNoSelected" style="color:red;display:none;font-weight:bold;font-size:15px;">Thank you for your interest. Please call us on 1800 266 4545 to buy offline.</span>
                <div class="popUp termsCondition2">
            <a class="close-icon" href="javascript:;"></a>
            <div class="popUpcontent">
                <div class="container">
                    <div class="row-grid">
                        <strong>Terms &amp; Conditions</strong><br />
                        <ul>
                            <li>Parents/Parent-in-laws not covered in the policy.</li>
                            <li>The above premium is calculated for self, spouse/children (wherever applicable).</li>
                            <li>The above yearly premium includes a 5% group employee discount and 2.5% online discount.</li>
                            <li id="li4" runat="server">The above yearly premium includes a 10% loading for monthly installment facility. </li>
                            <li id="li5" runat="server">In Case of an employee leaving the organization he/she needs to keep the salary account funded to continue the policy benefits or change to a yearly mode of payment.</li>
                            <li>Any pre-existing diseases are not covered for 2 years under this plan</li>
                        </ul>
                        <br />
                        <strong>Self-Declaration</strong><br />
                        <p>
                           I hereby declare, on my behalf and on behalf of all persons proposed to be insured, that the above statements, answers and/or particulars given by me are true and complete in all respects to the best of my knowledge and that I am authorized to propose on behalf of these other persons.
                        </p>
                        <p>
                            I understand that the information provided by me will form the basis of the insurance policy, is subject to the Board approved underwriting policy of the insurer and that the policy will come into force only after full payment of the premium chargeable.
                        </p>
                        <p>
                            I further declare that I will notify in writing any change occurring in the occupation or general health of the life to be insured/proposer after the proposal has been submitted but before communication of the risk acceptance by the company.
                        </p>
                        <p>
                            I declare that I consent to the company seeking medical information from any doctor or hospital who/which at any time has attended on the person to be insured/proposer or from any past or present employer concerning anything which affects the physical or mental health of the person to be insured/proposer and seeking information from any insurer to whom an application for insurance on the person to be insured /proposer has been made for the purpose of underwriting the proposal and/or claim settlement.
                        </p>
                        <p>
                            I authorize the company to share information pertaining to my proposal including the medical records of the insured/proposer for the sole purpose of underwriting the proposal and/or claims settlement and with any Governmental and/or Regulatory authority.
                        </p>
                        <p>
                            The quote displayed has been calculated based on the information provided by you and is subject to change in case you change the information while buying the policy.
                        </p>
                        <p>
                             I/We agree that this application and declaration shall be promissory and shall be the basis of the contract between me/us and Kotak Mahindra General Insurance Company Limited.
                        </p>
                        <p>
                             I/We confirm that I/We have read and understood the coverages, the terms and conditions and agree to accept the company's policy of insurance along with the said conditions prescribed by the company.
                        </p>
                        <p>
                             I/We also declare and undertake that if there is any change in information as submitted by me/us after this submission then the same would be conveyed to Kotak Mahindra General Insurance Company Limited immediately failing which it is agreed and understood by me/us that the benefits under the policy would stand forfeited.
                        </p>
                        <p>
                             I/We agree to the company taking appropriate measures to capture the voice log for all such telephonic transactions carried out by me/us as required by the procedures/regulations internal or external to the company and shall not hold the company responsible or liable for relying / using such recorded telephonic conversation.
                        </p>
                        <p>
                             I/We agree that the insurance would be effective only on acceptance of this application by the company and the payment of the requisite premium by me/us in advance. In the event of non-realization of the cheque or non-receipt of the amount of premium by the company the policy shall be deemed cancelled ab-initio and the company shall not be responsible for any liabilities of whatsoever nature under this policy.
                        </p>
                    </div>
                </div>
            </div>
        </div>

                    <div class="popUp declarationofgoodhealth">
            <a class="close-icon" href="javascript:;"></a>
            <div class="popUpcontent">
                <div class="container">
                    <div class="row-grid">
                        <p>
                           I declare that the applicant(s) proposed to be insured 
                        </p>
                        <ul>
                            <li>Are not currently suffering or have previously suffered from or have symptoms of any illness/ medical condition/ disability/ physical deformity <br />and/or</li>
                            
                            <li>Have not consulted or received treatment from any doctor/ healthcare provider / undergone any hospitalization or are taking continuous medication for same</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

                                   


                <%--<button type="button" id="btnDebitMySalary" class="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px">Debit My Salary</button>--%>
                <asp:Button ID="btnMakePayment" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px;position: relative;top: 7px"  ClientIDMode="Static" Text="Debit My Salary Account" />
                 <div class="otpPanel align-center text-center" id="otpPanel" style="display:none;">
                                        <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />
                                        <asp:HiddenField ID="hdnOTPValue" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnIsEmployeeRecordPresent" runat="server" Value="0" />
                   
                     <asp:TextBox ID="txtTimer" ClientIDMode="Static" Style="display: none" ReadOnly="true" runat="server" Text="0" CssClass="timercountTxt"></asp:TextBox>                        
                                        <!-- TIMER STARTS HERE -->
                    <br />
                                            <div class="timer" id="divTimer">
                                                <span class="secspan">sec</span>
                                            </div>    
                                            <!-- TIMER ENDS HERE -->
                                            <p>Please Enter One Time Password (OTP) Sent On Your Mobile Number And Email</p>
                                            <div class="block-center text-center">
                                                <p>
                                                <asp:TextBox ID="txtOtpNumber" runat="server"  style="border-radius: 7px;border: 1px solid #dde6e9;padding:2px 7px;font-size: 13px;" Text="0" MaxLength="6" />
                                                    </p>
                                            </div><br />
                    
                                            <div class="otpButton align-center">
                                                <button type="button" id="btnMobileVerify" class="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px">Verify OTP</button>
                                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Resend OTP" />
                                            </div>
                                        </div>
                
                
            </center>

                <p id="term_KLT" runat="server" style="color: black; font-size: 1.3rem; text-align: left; font-style: italic; padding: 50px">
                    <%--<strong>By clicking on the above button, you hereby give your consent to debit your salary account for the above mentioned amount towards Kotak Health Super Top Up policy and agree to the </strong>
                    <a class="link-red callPopup" data-popupname="termsCondition2" target="_self" href="javascript:;">terms & conditions</a>--%>
                    <strong>By clicking on the above button, you hereby give your consent to debit your salary account for the above mentioned amount towards Kotak Health Super Top Up policy and agree to the <a data-popupname="termsCondition2" class="callPopup" style="color: black" target="_self" href="javascript:;"><u>terms & conditions </u></a></strong>
                </p>
                <p id="term_NonKLT" runat="server" style="color: black; font-size: 1.3rem; text-align: left; font-style: italic; padding: 50px">
                    <%--<strong>By clicking on the above button, you hereby give your consent to debit your salary account for the above mentioned amount on a monthly basis towards Kotak Health Super Top Up policy and agree to the </strong>
                    <a class="link-red callPopup" data-popupname="termsCondition2" target="_self" href="javascript:;">terms & conditions</a>--%>
                    <strong>By clicking on the above button, you hereby give your consent to debit your salary account for the above mentioned amount towards Kotak Health Super Top Up policy and agree to the <a data-popupname="termsCondition2" class="callPopup" style="color: black" target="_self" href="javascript:;"><u>terms & conditions </u></a></strong>
                </p>
            </section>

            <section id="sectionThankYou" style="display: none; min-height: 500px;">
                <div class="content-wrapper">
                    <div class="container container-md">

                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Thank You
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    We have received your consent to debit your salary account <b>
                                        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>.</b> You will receive a confirmation mail with details shortly on your official email ID.
                            <br />
                                </p>

                                <hr />
                                For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                        <hr />

                            </div>
                        </div>

                    </div>
                </div>
            </section>

            <section id="sectionError" style="display: none; min-height: 500px;">
                <div class="content-wrapper">
                    <div class="container container-md">

                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Sorry, Error Occurred
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Due to some technical issue, your request could not be processed.
                            <br />
                                </p>

                                <hr />
                                For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                        <hr />

                            </div>
                        </div>

                    </div>
                </div>
            </section>


            <section id="sectionRecordNotFound" style="display: none; min-height: 500px;">
                <div class="content-wrapper">
                    <div class="container container-md">

                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Sorry, nothing to display
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Either you have already submitted the request or your record not present in the system
                            <br />
                                </p>

                                <hr />
                                For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                        <hr />

                            </div>
                        </div>

                    </div>
                </div>
            </section>
            <br />
            <!-- Page footer-->
            <footer style="background-color: white; z-index: 113;">
                <span style="font-size: 12px; text-align: center; padding: 10px; float: left"><b>Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak General Insurance Company Ltd.  CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                </b></span>
            </footer>




        </div>

    </form>

    <!-- Modal -->
    <div id="myModal" class="modal fade" role="dialog" style="z-index: 9999">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Declaration of Good Health</h4>
                </div>
                <div class="modal-body">

                    <p>
                        I declare that the applicant(s) proposed to be insured 
                    </p>
                    <ul>
                        <li>Are not currently suffering or have previously suffered from or have symptoms of any illness/ medical condition/ disability/ physical deformity
                            <br />
                            and/or</li>

                        <li>Have not consulted or received treatment from any doctor/ healthcare provider / undergone any hospitalization or are taking continuous medication for same</li>
                    </ul>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <div id="ModalHowDeductibleWorks" class="modal fade" role="dialog" style="z-index: 9999">
        <div class="modal-dialog modal-lg" style="font-size: 13px; width: 90%">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">How deductible works?</h4>
                </div>
                <div class="modal-body">

                    <p>
                        <strong>What is a deductible?</strong>
                    </p>
                    <p>
                        Deductible is the amount payable by the 1st insurance policy or the customer individually in case of a claim.  For example, if you opt for a deductible of Rs.3 lacs, any claim upto Rs.3 lac will have to be paid by your 1st insurance policy or by you. The below illustration shows how this works. 
                    </p>
                    <p>
                        <strong>Illustration</strong>
                    </p>
                    <p>
                        The illustration below shows how our Super Top Up Policy works in case of a deductible for INR 3 lacs & Sum Insured of INR 7 lacs for different claims incurred. 
                    </p>
                    <div class="table-responsive">
                        <table class="table">
                            <thead style="background-color: #ec1c24;">
                                <tr>
                                    <th style="color: white;">Claims</th>
                                    <th style="color: white;">Deductible Chosen</th>
                                    <th style="color: white;">Super Top Up Sum Insured</th>
                                    <th style="color: white;">Claim Amount</th>
                                    <th style="color: white;">Payable by 1st Policy / Customer</th>
                                    <th style="color: white;">Payable by Kotak Super Top Up</th>
                                </tr>
                            </thead>
                            <tbody style="color: black;">
                                <tr class="warning">
                                    <td>Claim 1</td>
                                    <td>300000</td>
                                    <td>700000</td>
                                    <td>250000</td>
                                    <td>250000</td>
                                    <td>Nil</td>
                                </tr>
                                <tr class="info">
                                    <td>Claim 2</td>
                                    <td>300000</td>
                                    <td>700000</td>
                                    <td>400000</td>
                                    <td>50000</td>
                                    <td>350000</td>
                                </tr>
                                <tr class="warning">
                                    <td>Claim 3</td>
                                    <td>300000</td>
                                    <td>700000</td>
                                    <td>600000</td>
                                    <td>Nil</td>
                                    <td>350000 (*)</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <p>
                        (*) The base annual sum insured under the Super Top Up policy is fully utilised and hence for Claim 3, only 3.5 lacs will be paid out of the Claim Amount.
Additional amount may be eligible for claim depending on the nature of claim and coverage being available under the Restoration of Sum Insured and Double Sum Insured for Hospitalization due to Accident covers.
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {


            $(function () {

                var SelectedPremiumAmount = $("#hdnYearlyTotalPayable").val();
                var ExpiringPolicyNumber = $("#hdnExpiringPolicyNumber").val();

                var hdnMonthlyTotalPayable = $("#hdnMonthlyTotalPayable").val();
                var hdnYearlyTotalPayable = $("#hdnYearlyTotalPayable").val();

                var SelectedMonthlyPremiumAmount = hdnMonthlyTotalPayable;
                var SelectedYearlyPremiumAmount = hdnYearlyTotalPayable;
                
                var hdnIsEmployeeRecordPresent = $("#hdnIsEmployeeRecordPresent").val();
                if (hdnIsEmployeeRecordPresent == "0") {
                    $("#sectionThankYou").hide();
                    $("#sectionError").hide();
                    $("#sectionRecordNotFound").show();
                    $("#sectionMain").hide();
                }

                var crndt = new Date();
                var year = crndt.getFullYear() - 1;
                crndt.setFullYear(year);

                $("input[type='text']").each(function () {
                    var element = $(this);
                    var id = $(this).attr('id');
                    var name = $(this).attr('name');

                    if (id.indexOf('NomineeDOB') >= 0) {

                        $(document).on('focus', "#" + id, function () {
                            $(this).datepicker({
                                changeMonth: true,
                                changeYear: true,
                                showButtonPanel: true,
                                dateFormat: 'dd/mm/yy',
                                yearRange: '1920:' + year + '', defaultDate: crndt
                            });
                        });
                    }

                });

                $('#btnMakePayment').click(function () {

                    var IsAgree = $('#inlineradioYes').is(':checked');


                    var isValid = true;

                    if ($('#txtBRMCode').val().length == 0 || $('#txtBRMCode').val() == "" || $('#txtBRMCode').val().length < 10) {
                        isValid = false;
                        alert('To be left as 0000000000 if buying directly. If buying through KGI BRM, please input BRM INTERMEDIARY CODE.');
                        $("#txtBRMCode").focus();
                        return false;
                    }



                    $("input[type='text']").each(function () {
                        var element = $(this);
                        var id = $(this).attr('id');
                        var name = $(this).attr('name');

                        if (element.val() == "") {
                            isValid = false;
                            alert('Please Enter ' + name);
                            return false;
                        }
                        else if (name.indexOf('ight') >= 0) {
                            var isNotANumber = isNaN(element.val());
                            if (isNotANumber == true) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                            else if (parseInt(element.val()) <= 0) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                        }
                    });

                    $('#divTabContent1').children('.tab-pane').each(function () {
                        var innerDivId = $(this).attr('id');
                        var innerDivName = $(this).attr('name');
                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                        $("#" + innerDivId).find("select.form-control").each(function () {
                            var element = $(this);
                            var id = $(this).attr('id');
                            var name = $(this).attr('name');
                            if (id.indexOf('cboNomineeRelationWithProposer') >= 0) {
                                if (element.val() === "Select") {
                                    isValid = false;
                                    alert("Please Select Nominee Relationship with Proposer for " + innerDivName);
                                    return false;
                                }
                            }
                        });

                        if (isValid == false) {
                            return false;
                        }
                    });

                    if (IsAgree == false && isValid == true) {

                        isValid = false;
                        alert('Please select appropriate input by understanding the declaration of good health');
                        return false;
                    }

                    if (isValid == true) {
                        ShowOTP();
                    }

                    //return isValid;
                    return false;
                });

                $('#btnMobileReSend').click(function () {

                    var IsAgree = $('#inlineradioYes').is(':checked');


                    var isValid = true;

                    $("input[type='text']").each(function () {
                        var element = $(this);
                        var id = $(this).attr('id');
                        var name = $(this).attr('name');

                        if (id == "txtOtpNumber") {
                            //do nothing if txtOtpNumber is blank
                        }
                        else if (element.val() == "") {
                            isValid = false;
                            alert('Please Enter ' + name);
                            return false;
                        }
                        else if (name.indexOf('ight') >= 0) {
                            var isNotANumber = isNaN(element.val());
                            if (isNotANumber == true) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                            else if (parseInt(element.val()) <= 0) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                        }
                    });

                    $('#divTabContent1').children('.tab-pane').each(function () {
                        var innerDivId = $(this).attr('id');
                        var innerDivName = $(this).attr('name');
                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                        $("#" + innerDivId).find("select.form-control").each(function () {
                            var element = $(this);
                            var id = $(this).attr('id');
                            var name = $(this).attr('name');
                            if (id.indexOf('cboNomineeRelationWithProposer') >= 0) {
                                if (element.val() === "Select") {
                                    isValid = false;
                                    alert("Please Select Nominee Relationship with Proposer for " + innerDivName);
                                    return false;
                                }
                            }
                        });

                        if (isValid == false) {
                            return false;
                        }
                    });

                    if (IsAgree == false && isValid == true) {

                        isValid = false;
                        alert('Please select appropriate input by understanding the declaration of good health');
                        return false;
                    }

                    if (isValid == true) {
                        ShowOTP();
                    }

                    //return isValid;
                    return false;
                });

                $('#btnMobileVerify').click(function () {

                    var txtOtpNumber = $("#txtOtpNumber").val();
                    var lblEmployeeCode = $("#lblEmployeeCode").text();

                    var MembersDetails = {};
                    var objEmployeePrimaryDetails = {};

                    var dataMembersDetails = [];

                    $('#divTabContent1').children('.tab-pane').each(function () {
                        var innerDivId = $(this).attr('id');
                        var innerDivName = $(this).attr('name');
                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                        var Height = '';
                        var Weight = '';
                        var NomineeName = '';
                        var NomineeDOB = '';
                        var EmployeeCD = $("#lblEmployeeCode").text();
                        var Title = '';
                        var NomineeRelationWithProposer = '';

                        $("#" + innerDivId).find("input[type=text]").each(function () {
                            var element = $(this);
                            var id = $(this).attr('id');
                            var name = $(this).attr('name');

                            if (id.indexOf('Height') >= 0) {
                                Height = element.val();
                            }
                            else if (id.indexOf('Weight') >= 0) {
                                Weight = element.val();
                            }
                            else if (id.indexOf('NomineeName') >= 0) {
                                NomineeName = element.val();
                            }
                            else if (id.indexOf('NomineeDOB') >= 0) {
                                NomineeDOB = element.val();
                            }
                        });

                        $("#" + innerDivId).find("select.form-control").each(function () {
                            var element = $(this);
                            var id = $(this).attr('id');
                            var name = $(this).attr('name');
                            if (id.indexOf('cboTitle') >= 0) {
                                Title = element.val();
                            }
                        });

                        $("#" + innerDivId).find("select.form-control").each(function () {
                            var element = $(this);
                            var id = $(this).attr('id');
                            var name = $(this).attr('name');
                            if (id.indexOf('cboNomineeRelationWithProposer') >= 0) {
                                NomineeRelationWithProposer = element.val();
                            }
                        });

                        MembersDetails = { UniqueRowId: innerDivMemberId, ReleationshipWithEmployee: innerDivName, Height: Height, Weight: Weight, NomineeName: NomineeName, NomineeDOB: NomineeDOB, EmployeeCode: EmployeeCD, MemberTitle: Title, MemberNomineeRelationWithProposer: NomineeRelationWithProposer, ExpiringPolicyNumber: ExpiringPolicyNumber };

                        dataMembersDetails.push(MembersDetails);

                    });

                    //console.log(data);

                    var SelectedPlanName = 'Yearly';
                    var IsMonthlyChecked = $('#inlineradioMonthly').is(':checked');
                    if (IsMonthlyChecked) {
                        SelectedPlanName = 'Monthly';
                    }
                    else {
                        SelectedPlanName = 'Yearly';
                    }

                    objEmployeePrimaryDetails = {
                        SelectedPlan: SelectedPlanName,
                        FinalOneTimePasswordEnteredByEmployee: txtOtpNumber,
                        EmployeeAddressLine1: $("#txtSelfAddressLine1").val(),
                        EmployeeAddressLine2: $("#txtSelfAddressLine2").val(),
                        Pincode: $("#txtSelfPincode").val(),
                        EmployeeCode: lblEmployeeCode,
                        EmployeeName: $("#lblEmployeeName").text(),
                        EmployeeEmailId: $("#lblEmpEmailId").text(),
                        SelectedPremium: SelectedPremiumAmount, //$("#lblSelectedPremium").text(),
                        AccountNumber: $("#lblAccountNumber").text(),
                        IsKLTEmployee: $("#hdnIsKLTEmployee").val(),
                        AlternateMobileNumber: $("#txtAlternateMobileNumber").val(),
                        BRMCode: $("#txtBRMCode").val(),
                        ExpiringPolicyNumber: ExpiringPolicyNumber
                    };

                    var dataOTP = { OTPNumber: txtOtpNumber, EmployeeCode: lblEmployeeCode };

                    $.ajax({
                        type: "POST",
                        url: "FrmSTPRenewal.aspx/ValidateOTP",
                        data: JSON.stringify(dataOTP),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {

                            if (response.d == "success") {
                                SubmitEmployeePrimaryData(objEmployeePrimaryDetails, dataMembersDetails);
                            }
                            else {
                                alert('OTP Does Not Match, Please Enter OTP Sent On Your Email And Mobile Number');
                                return false;
                            }
                        },
                        error: function (requestObject, error, errorThrown) {
                            alert(error);
                        }
                    });


                });


                function SubmitEmployeePrimaryData(objEmployeePrimaryDetails, dataMembersDetails) {

                    //alert(JSON.stringify(objEmployeePrimaryDetails));

                    $.ajax({
                        type: "POST",
                        url: "FrmSTPRenewal.aspx/SaveEmployeePrimaryDetails",
                        data: '{objEmployeePrimaryDetails: ' + JSON.stringify(objEmployeePrimaryDetails) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == "success") {
                                SubmitMemberData(dataMembersDetails);
                            }
                            else {
                                $("#sectionThankYou").hide();
                                $("#sectionError").show();
                                $("#sectionMain").hide();
                            }
                        },
                        error: function (response) {
                            alert(JSON.stringify(response));
                        }
                    });
                }

                function SubmitMemberData(dataMembersDetails) {

                    $.ajax({
                        type: "POST",
                        url: "FrmSTPRenewal.aspx/SaveMemberDetails",
                        data: '{listMemberDetails: ' + JSON.stringify(dataMembersDetails) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == "success") {
                                $("#sectionThankYou").show();
                                $("#sectionError").hide();
                                $("#sectionMain").hide();

                            }
                            else {
                                $("#sectionThankYou").hide();
                                $("#sectionError").show();
                                $("#sectionMain").hide();
                            }
                        },
                        error: function (response) {
                            alert(JSON.stringify(response));
                        }
                    });
                }

                function ShowOTP() {

                    var EmpEmailId = $("#lblEmpEmailId").text();
                    var EmpName = $("#lblEmployeeName").text();
                    var MobNumber = $("#txtAlternateMobileNumber").val();
                    var EmpCode = $("#lblEmployeeCode").text();
                    //Added for CR 172 Mobile validation
                    var phoneno = /^\d{10}$/;
                    if (txtAlternateMobileNumber.value.match(phoneno)) {
                        ShopOTPTimer();
                        var OTPData = { MobileNumber: MobNumber, EmailId: EmpEmailId, EmployeeCode: EmpCode, EmployeeName: EmpName };

                        $.ajax({
                            type: "POST",
                            url: "FrmSTPRenewal.aspx/GenerateOTPNew",
                            data: JSON.stringify(OTPData),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (response.d == "success") {
                                    // ShopOTPTimer();
                                }
                                else {
                                    $("#sectionThankYou").hide();
                                    $("#sectionError").show();
                                    $("#sectionMain").hide();
                                }
                            },
                            error: function (response) {
                                alert(JSON.stringify(response));
                            }
                        });
                    }

                    else {
                        alert("Please enter 10 digit mobile number.");
                    }
                }


                function ShopOTPTimer() {

                    $("#otpPanel").show();
                    $("#divDeclarofGoodHealth").hide();
                    $("#txtOtpNumber").val('');

                    $('.timer').circularCountDown({
                        delayToFadeIn: 50,
                        size: 70,
                        fontColor: '#696969',
                        colorCircle: 'transparent',
                        background: '#ffa58c',
                        reverseLoading: true,
                        duration: {
                            seconds: parseInt(180)
                        },
                        beforeStart: function () {
                            $("#txtTimer").hide();
                            $('#btnMakePayment').hide();
                            $('#btnMobileReSend').attr('disabled', true);
                            $('#btnMobileVerify').attr('disabled', false);
                            $('#btnMobileVerify').removeAttr('disabled');
                            $('#btnMobileVerify').removeClass('disabled');
                        },
                        end: function (countdown) {
                            $("#txtTimer").show();
                            $('#btnMobileVerify').addClass('disabled');
                            $('#btnMobileVerify').attr('disabled', 'disabled');
                            $('#btnMakePayment').removeAttr('disabled');
                            $('#btnMakePayment').removeClass('disabled');
                            $('#btnMobileReSend').removeAttr('disabled');
                            $('#btnMobileReSend').removeClass('disabled');
                            document.getElementById("<%=txtTimer.ClientID%>").value = 'Your One Time Password has expired';
                        }
                    });
                }


                $("#chkNomineeIsSame").on("click", function () {
                    var IsNomineeIsSame = $('#chkNomineeIsSame').is(':checked');
                    if (IsNomineeIsSame) {
                        var txtSelfNomineeName = $("#txtSelfNomineeName").val();
                        var txtSelfNomineeDOB = $("#txtSelfNomineeDOB").val();

                        $("input[type='text']").each(function () {

                            var element = $(this);
                            var id = $(this).attr('id');
                            var name = $(this).attr('name');

                            if (id.indexOf('NomineeName') >= 0) {
                                element.val(txtSelfNomineeName);
                            }

                            if (id.indexOf('NomineeDOB') >= 0) {
                                element.val(txtSelfNomineeDOB);
                            }
                        });

                    }
                });

                $('input[type=radio][name=radioDclrofgdhlth]').change(function () {
                    var IsYesChecked = $('#inlineradioYes').is(':checked');
                    if (IsYesChecked) {
                        $("#btnMakePayment").show();
                        $("#lblMessageifNoSelected").hide();
                    }
                    else {
                        $("#btnMakePayment").hide();
                        $("#lblMessageifNoSelected").show();
                    }
                });

                $('input[type=radio][name=radioPaymentOption]').change(function () {
                    var IsMonthlyChecked = $('#inlineradioMonthly').is(':checked');

                    var hdnMonthlyPremiumAmount = $("#hdnMonthlyPremiumAmount").val();
                    var hdnMonthlyGST = $("#hdnMonthlyGST").val();
                    var hdnMonthlyTotalPayable = $("#hdnMonthlyTotalPayable").val();

                    var hdnYearlyPremiumAmount = $("#hdnYearlyPremiumAmount").val();
                    var hdnYearlyGST = $("#hdnYearlyGST").val();
                    var hdnYearlyTotalPayable = $("#hdnYearlyTotalPayable").val();

                    if (IsMonthlyChecked) {
                        $("#lblPremiumAmount").text(hdnMonthlyPremiumAmount);
                        $("#lblGST").text(hdnMonthlyGST);
                        $("#lblAmountPayable").text(hdnMonthlyTotalPayable);
                        $("#lblSelectedPremiumMessage").text("Monthly Amount Payable - ₹ ");
                        $("#lblSelectedPremium").text(hdnMonthlyTotalPayable);
                        SelectedPremiumAmount = SelectedMonthlyPremiumAmount;
                    }
                    else {
                        $("#lblPremiumAmount").text(hdnYearlyPremiumAmount);
                        $("#lblGST").text(hdnYearlyGST);
                        $("#lblAmountPayable").text(hdnYearlyTotalPayable);
                        $("#lblSelectedPremiumMessage").text("Yearly Amount Payable - ₹ ")
                        $("#lblSelectedPremium").text(hdnYearlyTotalPayable);
                        SelectedPremiumAmount = SelectedYearlyPremiumAmount;
                    }
                });
            });
        });
    </script>

</body>
</html>
