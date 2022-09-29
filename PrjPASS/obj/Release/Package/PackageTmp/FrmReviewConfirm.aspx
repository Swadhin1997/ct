<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmReviewConfirm.aspx.cs" Inherits="PrjPASS.FrmReviewConfirm" %>

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
    <link rel="stylesheet" href="css/newcssjs/app.css" id="maincss" />

</head>
    
    
<body>

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
  color: #696969; }

/* line 12, ../scss/imports/_timer.scss */
.timerimg {
  position: absolute;
  z-index: 99;
  left:0; }

.timercountTxt {
  background: rgba(0, 0, 0, 0) none repeat scroll 0 0;
  color: red !important;
  font-size: 1.6rem;
  border: none !important;
  width: 100%;
  text-align: center;
  padding-bottom: 5px; }

/*.otpPanel {
  width: 400px;
  padding: 20px;
  border: 1px solid #ccc;
  margin: 20px 0;
  float: left;
  clear: both; }*/

 .otpPanel .otpButton {
    margin: 20px 0 0 0; }

  .otpPanel p {
    margin: 0; }
  /* line 437, ../scss/imports/_motorinsurance.scss */
  .otpPanel .inputBox input {
    padding: 10px 15px 5px 0; }


  .timer {
  width: 84px;
  height: 84px;
  margin:auto;
  font-family: 'source_sans_probold'; }


  .error {
  font-size: 1.3rem;
  color: red;
  float: left;
  clear: both;
  padding-top: 5px; }

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
    -moz-transition: all 0.2s ease-in; }

        .link-red {
  color: #ec1c24 !important; }

          .popUp {
  background: #fff;
  display: none;
  width: 100%;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 99;
  padding-top: 40px;
  height:100%;
          }
  
  .popUp .close-icon {
    background: url(Images/close-icon.png) no-repeat;
    width: 24px;
    height: 24px;
    position: absolute;
    right: 10px;
    top: 10px; }

  .container {
  width: 100%;
  max-width: 1350px;
  margin: 0 auto;
  padding: 0 15px;
  position: relative;
  height: 100%; }
  /* line 15, ../scss/imports/_main.scss */
  .container.container2 {
    padding: 0px; }

  .row-grid {
  float: left;
  width: 100%;  
  padding: 25px 0px;
  text-align: left;
  position: relative; }

  /*.mob-text-center {
    left: 0!important;
    translate: transform(0, 0) !important;
    -moz-translate: transform(0, 0) !important;
    -webkit-translate: transform(0, 0) !important;
    -o-translate: transform(0, 0) !important;
    -ms-translate: transform(0, 0) !important; }*/

   .link-red {
  color: #ec1c24 !important; }

    .link.align-center.underline.callPopup {
    clear: both; }

    .banner-content {
  position: relative;
  width: 100%;
  float: left;
  color: #2d2d2d;
  transition: all 0.5s;
  -webkit-transition: all 0.5s;
  padding: 0;
  z-index: 0; }
  /* line 415, ../scss/imports/_main.scss */
  .banner-content h2 {
    text-align: center;
    margin-bottom: 10px;
    font-family: Lato-Regular;
    font-size: 2.5em;
    clear: both; }

    </style>
    <script>
       
    
        $(function () {                        
            callpopup();
        });


        $(function () {
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
       <asp:UpdatePanel ID="UpdatePanel_Detail1" runat="server" UpdateMode="Conditional"
    ClientIDMode="Static">
           <ContentTemplate>
               <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
                <script type="text/javascript">

        function fnValidateOTPNumber(source, args) {
            var txtOtpNumber = $(".txtOtpNumber").val();
            args.IsValid = (txtOtpNumber.length > 5);
        }

        function runme()
        {
          
        $('.timer').circularCountDown({
                        delayToFadeIn: 50,
                        size: 70,
                        fontColor: '#696969',
                        colorCircle: 'transparent',
                        background: '#ffa58c',
                        reverseLoading: false,
                        duration: {
                            seconds: parseInt(59)
                        },
                        beforeStart: function () {                            
                            $(".timercountTxt").hide();                         
                            $('#btnMakePayment').attr('disabled', true);
                            $('#btnMobileReSend').attr('disabled', true);
                        },
                        end: function (countdown) {
                            $(".timercountTxt").show();
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

        
    </script>
                  </ContentTemplate>
           </asp:UpdatePanel>
        

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
                            <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em> care@kotak.com</a>
                        </li>

                        <li>
                            <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em> 1800 266 4545</a>
                        </li>

                    </ul>
                    <!-- END Right Navbar-->
                </div>
                <!-- END Nav wrapper-->

            </nav>
            <!-- END Top Navbar-->
        </header>
            

        <section>
            <div class="content-wrapper">
                <div class="container container-md">
                    <div class="row mb-lg">
                        <div class="col-md-8">                            
                            
                            <div class="h3 text-bold">Proposal Number: <asp:Label ID="lblProposalText" runat="server" Text="-"></asp:Label></div>
                            <p class="text-muted">Please have one final look at the details you have provided us with. This information will be printed on your policy, so it's important to ensure that everything is accurate.</p>
                        </div>
                        <div class="col-md-4">
                            <div class="panel">
                                <div class="panel-body text-center" style="color:#bc8d3d;">
                                    <h4>
                                        Total Premium
                                    </h4>
                                    <p class="mb-lg"><h1> <asp:Label ID="lblTotalPremium" runat="server" Text="0.00"></asp:Label> </h1></p>
                                </div>
                            </div>
                        </div>
                    </div>
  
                    <h4 class="mv-lg pv-lg text-dark">Review Proposal Details</h4>
                    <div id="accordion1" class="panel-group">
                        <div class="panel panel-default b0">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapse1" class="collapsed" aria-expanded="true">
                                        <small>
                                            <em class="fa fa-car text-primary mr"></em>
                                        </small>
                                        <span>Vehicle Details</span>
                                    </a>
                                </h4>
                            </div>
                            <div id="acc1collapse1" class="panel-collapse collapse in" aria-expanded="true">
                                <div class="panel-body">
                                    <div class="row">
                                      
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Registration No.</dt>
                                                <dd><asp:Label ID="lblRegistrationNumber" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Car Make & Model</dt>
                                                <dd><asp:Label ID="lblMakeText" runat="server" Text="-"></asp:Label> <asp:Label ID="lblModelText" runat="server" Text="-"></asp:Label> <asp:Label ID="lblVariantText" runat="server" Text="-"></asp:Label> </dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Fuel type</dt>
                                                <dd><asp:Label ID="lblFuelType" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Chassis number</dt>
                                                <dd><asp:Label ID="lblChassisNumber" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Engine number</dt>
                                                <dd><asp:Label ID="lblEngineNumber" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Credit Score</dt>
                                                <dd><asp:Label ID="lblCreditText" runat="server" Text="0"></asp:Label></dd>
                                            </dl>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>RTO Location</dt>
                                                <dd><asp:Label ID="lblRTOText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Cubic Capacity</dt>
                                                <dd><asp:Label ID="lblCubicText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Seating Capacity</dt>
                                                <dd><asp:Label ID="lblseatingText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Date of Registration</dt>
                                                <dd><asp:Label ID="lblRegistrationText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Insured Declared Value</dt>
                                                <dd><asp:Label ID="lblFinalIDV" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Previous Policy Expiry Date </dt>
                                                <dd><asp:Label ID="lblPreviousPolicyText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Non Electrical Accessories</dt>
                                                <dd><asp:Label ID="lblNonElectricalText" runat="server" Text="0"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Electrical Accessories</dt>
                                                <dd><asp:Label ID="lblElectricalText" runat="server" Text="0"></asp:Label></dd>
                                            </dl>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default b0">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapse2" class="collapsed" aria-expanded="false">
                                        <small>
                                            <em class="fa fa-credit-card text-primary mr"></em>
                                        </small>
                                        <span>New Policy Details</span>
                                    </a>
                                </h4>
                            </div>
                            <div id="acc1collapse2" class="panel-collapse collapse" aria-expanded="false">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Policy Effective Date</dt>
                                                <dd><asp:Label ID="lblPolicyStartText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Policy End Date</dt>
                                                <dd><asp:Label ID="lblPolicyEndDate" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Policy Term</dt>
                                                <dd><asp:Label ID="lblPolicyTerm" runat="server" Text="1 Year"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Policy Type</dt>
                                                <dd><asp:Label ID="lblCoverTypeText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Quote Id</dt>
                                                <dd><asp:Label ID="lblQuoteNumber" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Policy Holder Type</dt>
                                                <dd><asp:Label ID="lblPolicyHolderText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default b0">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapse3" class="collapsed" aria-expanded="false">
                                        <small>
                                            <em class="fa fa-user text-primary mr"></em>
                                        </small>
                                        <span>Insured Details</span>
                                    </a>
                                </h4>
                            </div>
                            <div id="acc1collapse3" class="panel-collapse collapse" aria-expanded="false">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Customer Id</dt>
                                                <dd><asp:Label ID="lblCustomerID" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Customer Name</dt>
                                                <dd><asp:Label ID="lblCustomerName" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Mailing Address</dt>
                                                <dd><asp:Label ID="lblAddress" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Date of Birth</dt>
                                                <dd><asp:Label ID="lblDOB" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Pan Number</dt>
                                                <dd><asp:Label ID="lblPanNumber" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Email Address</dt>
                                                <dd><asp:Label ID="lblEmail" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                      
                                    </div>
                                    <div class="row">
                                       
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Mobile Number</dt>
                                                <dd><asp:Label ID="lblMobile" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Customer Type</dt>
                                                <dd><asp:Label ID="lblOwnerShipText" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default b0">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapse4" class="collapsed" aria-expanded="false">
                                        <small>
                                            <em class="fa fa-user text-primary mr"></em>
                                        </small>
                                        <span>Nominee Details</span>
                                    </a>
                                </h4>
                            </div>
                            <div id="acc1collapse4" class="panel-collapse collapse" aria-expanded="false">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Nominee</dt>
                                                <dd><asp:Label ID="lblNomineeName" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Relationship with insured</dt>
                                                <dd><asp:Label ID="lblRelationshipWithInsured" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Nominee DOB</dt>
                                                <dd><asp:Label ID="lblNomineeDOB" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Appointee</dt>
                                                <dd><asp:Label ID="lblAppointee" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Appointee Relationship</dt>
                                                <dd><asp:Label ID="lblAppointeeRelationship" runat="server" Text="-"></asp:Label></dd>
                                            </dl>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>


                </div>
            </div>

            <h4 class="mv-lg pv-lg text-dark text-center">Detailed Break-up Of Final Premium</h4>
            <div class="block-center mt-xl wd-xl">
                <div class="row">
                    <div class="col-lg-12">

                        <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapsebreakup1" class="collapsed" aria-expanded="false">

                            <p class="lead bb">
                                Own Damage
                            </p>
                          
                        </a>
                        
                        <div id="acc1collapsebreakup1" class="panel-collapse collapse" aria-expanded="false">
                            <div class="row">
                                <div class="col-sm-4">Own Damage Premium</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblOwnDamage" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Electrical/Electronic</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblElectricalItems" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">Non Electrical/Electronic</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblNonElectricalItems" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">External Bi Fuel Kit</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblBiFuelKit" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                        </div>

                    </div>

                </div>
                <br />
                <div class="row">
                    <div class="col-lg-12">

                        <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapsebreakup2" class="collapsed" aria-expanded="false">

                            <p class="lead bb">
                                Liability
                            </p>
                        </a>

                        <div id="acc1collapsebreakup2" class="panel-collapse collapse" aria-expanded="false">
                            <div class="row">
                                <div class="col-sm-4">Basic TP including TPPD premium</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblTPPremium" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Liability For Bi-Fuel Kit</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblLiabilityBiFuel" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">PA For Unnamed Passenger</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblPAUnnamed" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">PA For Named Passenger</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblPANamed" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">PA To Paid Driver</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblPAtoPaid" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">PA Cover For Owner Driver</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblPACoverForDriver" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">Legal Liability for paid driver cleaner conductor</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblLiabilityForDriver" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Legal Liability for Employees other than paid driver conductor cleaner</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblLiabilityEmployees" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>




                        </div>

                    </div>

                </div>


                <br />
                <div class="row">
                    <div class="col-lg-12">

                        <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapsebreakup3" class="collapsed" aria-expanded="false">

                            <p class="lead bb">
                                Add-Ons
                            </p>
                        </a>

                        <div id="acc1collapsebreakup3" class="panel-collapse collapse" aria-expanded="false">
                            <div class="row">
                                <div class="col-sm-4">Engine Protect Cover</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblEngineProtect" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Return to Invoice Cover</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblReturnToInvoice" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">Consumable Cover</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblConsumableCover" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Depreciation Cover</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblDepreciationCover" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">Road Side Assistance (RSA)</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblRSA" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Daily Car Allowance</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblDailyCarAllowance" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">Key Replacement</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblKeyReplacement" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Tyre Cover</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblTyreCover" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">NCB Protect</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblNCBProtect" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Loss of Personal Belongings</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblLossofPersonalBelongings" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                        </div>

                    </div>

                </div>

                <br />
                <div class="row">
                    <div class="col-lg-12">

                        <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapsebreakup4" class="collapsed" aria-expanded="false">

                            <p class="lead bb">
                                Discount
                            </p>
                        </a>

                        <div id="acc1collapsebreakup4" class="panel-collapse collapse" aria-expanded="false">
                            <div class="row">
                                <div class="col-sm-4">Voluntary Deduction</div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblVoluntary" runat="server" Text="0.00"></asp:Label></strong></div>
                                <div class="col-sm-4">Voluntary Deduction for Dep Waiver</div>
                                <div class="col-sm-1"><strong><asp:Label ID="lblVoluntaryDep" runat="server" Text="0.00"></asp:Label></strong></div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-4">No Claim Bonus <asp:Label ID="lblNCBPer" runat="server" Text="0"></asp:Label></div>
                                <div class="col-sm-3"><strong><asp:Label ID="lblNCBAmount" runat="server" Text="0.00"></asp:Label></strong></div>

                            </div>

                        </div>

                    </div>

                </div>
                <br />
            </div>
           
            <h2 class="mv-lg pv-lg text-dark text-center">Total Premium - <asp:Label ID="lblTotalPremium2" runat="server" Text="0.00"></asp:Label> </h2>
            <div class="block-center mt-xl wd-xl">
                
                <div class="row">
                    <div class="col-sm-4">System IDV</div>
                    <div class="col-sm-3"><strong><asp:Label ID="lblSystemIDV" runat="server" Text="0.00"></asp:Label></strong></div>
                    <div class="col-sm-4">Final IDV</div>
                    <div class="col-sm-1"><strong><asp:Label ID="lblFinalIDV2" runat="server" Text="0.00"></asp:Label></strong></div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-sm-4">Net Premium</div>
                    <div class="col-sm-3"><strong><asp:Label ID="lblNetPremium" runat="server" Text="0.00"></asp:Label></strong></div>
                    <div class="col-sm-4">GST @ 18%</div>
                    <div class="col-sm-1"><strong><asp:Label ID="lblGSTAmount" runat="server" Text="0.00"></asp:Label></strong></div>
                </div>
                <br />
            </div>

            <center>
                
                <div class="checkbox c-checkbox needsclick" id="agreewithbtn">
                    
                    <label>
                        <input type="checkbox" value="" class="needsclick" id="chkSummaryAgree" runat="server" />
                             <span class="fa fa-check"></span>                       
                        <label>I agree to the <a class="link-red callPopup" data-popupname="termsCondition2" target="_self" href="javascript:;">Terms &amp; Conditions</a></label> 
                         <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please select terms and conditions."             
            ForeColor="Red" onservervalidate="CustomValidator1_ServerValidate" Width="100%" BorderWidth="0"></asp:CustomValidator>               
                         
                    </label>
              
                </div>
                <div class="popUp termsCondition2">
            <a class="close-icon" href="javascript:;"></a>
            <div class="popUpcontent">
                <div class="container">
                    <div class="row-grid">
                        <p>
                            I/We do hereby declare that I/we have read and understood the entire text, features,
                            disclosures, benefits, terms and conditions of the policy and I/we further declare
                            that the information furnished above are true to the best of my/our knowledge and
                            no material information, which may be relevant, has been withheld or not disclosed.
                            In case any of the information above is found false during verification at a later
                            date, the company would have the right to cancel the policy and premium amount paid
                            will be forfeited. I/We also declare that any additions or alterations are carried
                            out after the submission of this proposal form then the same would be conveyed to
                            the insurers immediately.
                        </p>
                        <p>
                            The quote displayed has been calculated based on the information provided by you
                            and is subject to change in case you change the information while buying the policy.
                            The product information, including the scopes of cover, terms, conditions, exclusions
                            and limitations, available on the website is not intended to be exhaustive and is
                            indicative in nature. I / We desire to insure with Kotak Mahindra General Insurance
                            Company Limited in respect of the vehicle described above and confirm that the statements
                            contained in this proposal are my/our true and accurate representations. I/we undertake
                            that if any of the statements are found to be false or incorrect, the benefits under
                            this policy would stand forfeited. I/We agree that this application and declaration
                            shall be promissory and shall be the basis of the contract between me/us and Kotak
                            Mahindra General Insurance Company Limited. I/We confirm that I/We have read and
                            understood the coverages, the terms and conditions and agree to accept the company's
                            policy of insurance along with the said conditions prescribed by the company. I/We
                            also declare and undertake that if there is any change in information as submitted
                            by me/us after this submission then the same would be conveyed to Kotak Mahindra
                            General Insurance Company Limited immediately failing which it is agreed and understood
                            by me/us that the benefits under the policy would stand forfeited. I/We agree to
                            the company taking appropriate measures to capture the voice log for all such telephonic
                            transactions carried out by me/us as required by the procedures/regulations internal
                            or external to the company and shall not hold the company responsible or liable
                            for relying / using such recorded telephonic conversation. I/We agree that the insurance
                            would be effective only on acceptance of this application by the company and the
                            payment of the requisite premium by me/us in advance. In the event of non-realisation
                            of the cheque or non-receipt of the amount of premium by the company the policy
                            shall be deemed cancelled ab-initio and the company shall not be responsible for
                            any liabilities of whatsoever nature under this policy.
                        </p>
                        <p>
                            <strong>Declaration for No Claim Bonus</strong><br>
                            I/We declare that the rate of NCB claimed by me/us is correct and that NO CLAIM
                            has arisen in the expiring policy period. I/We further undertake that if this declaration
                            is found incorrect, all benefits under the policy in respect of section I of the
                            policy will stand forfeited.
                        </p>
                        <p>
                            <strong>General exclusions:</strong>
                        </p>
                        <p>
                            The Company shall not be liable under this Policy in respect of
                        </p>
                        <ul>
                            <li>any accidental loss or damage and/or liability caused sustained or incurred outside
                                the geographical area;</li>
                            <li>any claim arising out of any contractual liability. Please refer to policy wordings
                                for detailed list of exclusions.</li>
                        </ul>
                        <p>
                            LIMITATIONS AS TO USE<br>
                            The Policy covers use of the vehicle for any purpose other than
                        </p>
                        <ul>
                            <li>Hire or Reward</li>
                            <li>Carriage of goods (other than samples or personal luggage)</li>
                            <li>Organized racing</li>
                            <li>Pace making</li>
                            <li>Speed testing</li>
                            <li>Reliability Trials</li>
                            <li>Use in connection with Motor Trade</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>


                <%--<button type="button" id="btnMakePayment" class="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" onserverclick="btnMakePayment_Click" runat="server">Make Payment</button>--%>
                <asp:Button ID="btnMakePayment" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  ClientIDMode="Static" Text="Confirm & Pay" OnClick="btnMakePayment_Click" />
                <br />
                 <%--<a id="anchorLink" runat="server" data-toggle="modal" data-target="#myModal"  href="#" style="font-size:1.9rem;color:#80631f !important">I want to change my proposal details</a>--%>
                <div class="otpPanel align-center text-center" id="otpPanel" visible="false" runat="server">
                                        <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />
                                            
                    <asp:TextBox ID="txtTimer" ClientIDMode="Static" Style="display: none" ReadOnly="true" runat="server" Text="" CssClass="timercountTxt"></asp:TextBox>                        
                                        <!-- TIMER STARTS HERE -->
                    <br />
                                            <div class="timer">
                                                <%--<img src="Images/bg-timer.png" alt="Timer" class="timerimg" />--%>
                                                
                                                <span class="secspan">sec</span>
                                            </div>
                         
                                            <!-- TIMER ENDS HERE -->
                                            <p>Please enter the One Time Password (OTP) sent on your mobile number</p>
                                            <div class="inputBox">
                                                <asp:TextBox ID="txtOtpNumber" runat="server" CssClass="txtOtpNumber" Text="" MaxLength="6" /><br />
                                                <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ValidationGroup="otp" Display="Dynamic" 
                                                    ErrorMessage="Please provide valid otp number" ClientValidationFunction="fnValidateOTPNumber" OnServerValidate="OnServerValidatecvtxtOtpNumber" />
                                            </div><br />
                    
                                            <div class="otpButton align-center">
                                                <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClick="onClickbtnMobileVerify" />

                                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Resend OTP" ValidationGroup="summary" OnClick="onClickbtnMobileReSend" />
                                            </div>
                                        </div>
                
                
                <asp:Button ID="btnPayment" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  ClientIDMode="Static" Visible="false" Text="Make payment" OnClick="onclick_btnPayment" />
            </center>

            <p style="font-family:'Lato-Light';color: #888888;font-size: 1.3rem;text-align: left;font-style: italic;padding:50px">
                <strong>Disclaimer:</strong> 
                I hereby confirm that I am the policyholder as well as the owner of the credit card / debit card / Internet banking account or any other online payment method, through which I am paying the policy premium.

                * Please note, in-case of a refund, the amount payable will automatically credited to the same account that you are using to pay the premium or can be refunded through a cheque in the name of the proposer.
            </p>
        </section>
        <!-- Page footer-->
        <footer style="background-color:white">
            <p>
                <center>
                    <span style="font-size:9px;text-align:center">
                        Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                    </span>
                </center>
            </p>
        </footer>

    </div>
        <%--<button id="opener">open the dialog</button>
        <div id="dialog1" title="Dialog Title" hidden="hidden">I'm a dialog</div>--%>
        
        
       <%-- <a data-toggle="modal" data-target="#myModal"  href="#">I want to change my details</a>--%>
        <%--<button type="button" data-toggle="modal" data-target="#myModal" class="btn btn-info">Default modal</button>--%>
        <div id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" class="modal fade">
      <div class="modal-dialog">
         <div class="modal-content">
            <div class="modal-header">
               <button type="button" data-dismiss="modal" aria-label="Close" class="close">
                  <span aria-hidden="true">&times;</span>
               </button>
               <h4 id="myModalLabel" class="modal-title">Details Update</h4>
            </div>
            <%--<div class="modal-body">I want to change my proposal details. Kindly contact me on my mobile number and email id registered with you.</div>--%>
             <div class="modal-body">
                 <p>Kindly fill the details in below box </p>
                 <asp:TextBox ID="txtDetails" runat="server" Text="" Height="200%" Width="100%" MaxLength="900" TextMode="MultiLine" />

             </div>
            <div class="modal-footer">
               <button type="button" data-dismiss="modal" class="btn btn-primary" style="background-color:#80631f !important;">Close</button>
               <button type="button" class="btn btn-primary" runat="server" id="btnSendMail" onserverclick="btnSendMail_ServerClick"  style="background-color:#80631f !important" >Send Mail</button>
            </div>
         </div>
      </div>
   </div>
        
    </form>
    
    
    <!-- JQUERY-->
   <%-- <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.min.js"></script>--%>

    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
</body>
</html>
