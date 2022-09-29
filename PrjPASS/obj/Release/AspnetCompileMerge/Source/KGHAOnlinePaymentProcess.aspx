<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KGHAOnlinePaymentProcess.aspx.cs" Inherits="PrjPASS.KGHAOnlinePaymentProcess" %>

<!DOCTYPE html>
<html>
<head>
    
    <title>Thank You</title>
     <script type="text/javascript" lang="javascript">               
           history.pushState(null, document.title, location.href);
           window.addEventListener('popstate', function (event) {
               history.pushState(null, document.title, location.href);
           });

           window.onload = function () { 
               document.onkeydown = function (e) {
                   return (e.which || e.keyCode) != 116;
               };
           }



           

     </script>  
<!-- Font-Awesome Stylesheet -->
<link href="css/newcssjs/fontawesome/css/font-awesome.min.css" rel="stylesheet" />
<link href="css/newcssjs/app.css" rel="stylesheet" />
<link href="css/newcssjs/bootstrap.css" rel="stylesheet">
</head>
<body>
    <style id="cssmdlThankYouconfirm">
    #mdlThankYouConfirm .panel-default > .panel-heading {
        color: #fafafa;
        background-color: #036;
        border-color: #ddd;
    }

    #mdlThankYouConfirm .text-primary {
        color: #fafafa;
    }

    #mdlThankYouConfirm .lead {
        margin-bottom: 20px;
        font-size: 16px;
        font-weight: 300;
        line-height: 1.4;
    }

    #mdlThankYouConfirm .row {
        margin-right: -15px;
        margin-left: 0px;
    }

    #mdlThankYouConfirm .btn:hover, .btn:focus, .btn.focus {
        color: #fafafa;
        text-decoration: none;
    }

    #mdlThankYouConfirm .ls-closed .navbar-brand {
        margin-left: 0px;
    }

    #mdlThankYouConfirmBreakup .panel {
        margin-bottom: 20px;
        background-color: white;
        border: 1px solid transparent;
        border-radius: 4px;
        -webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
        box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
        font-family: "Source Sans Pro", sans-serif;
        color: #656565;
    }

    #mdlThankYouConfirmBreakup h2 {
        margin-bottom: 10px;
    }

    #mdlThankYouConfirmBreakup .modal-header {
        border: none;
        padding: 25px 25px 5px 25px;
        background-color: #fff7f8;
    }

    #mdlThankYouConfirmBreakup .card .body {
        font-size: 14px;
        color: #555;
        padding: 20px;
        background-color: #fff7f8;
    }
</style>
    <form runat="server">
        <div class="wrapper" id="mdlThankYouConfirm">
            <!-- top navbar-->
            <header class="topnavbar-wrapper" id="dvHeaderThankYouPage" style="display:none">
                <!-- START Top Navbar-->
                <nav role="navigation" class="navbar topnavbar">
                    <!-- START navbar header-->
                    <div class="navbar-header">
                        <a href="#/" class="navbar-brand">
                            <div class="brand-logo">
                                <img src="../../Images/logo1.png" alt="App Logo" class="img-responsive" />
                            </div>
                            <div class="brand-logo-collapsed">
                                <img src="../../Images/logo1.png" alt="App Logo" class="img-responsive" />
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
                    <div class="content-wrapper" id="divSuccess" runat="server" >
                          <asp:HiddenField ID="hdnMSG" runat="server" ClientIDMode="Static" Value="0" />
                        <div class="abs-center" style="position:unset!important">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">Your payment is successful</div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Congratulations on your <strong>Kotak Group Health Assure policy.</strong> Kindly note down your proposal number&nbsp;<asp:Label runat="server" ID="lblproposalno"  Text="-"></asp:Label>.
                                    <br />
                                   Your policy issuance is in progress and policy documents will be emailed to you shortly.
                                    <br />
                                    
                                    <br />
                                </p>
                                <hr />
                                If you need to talk to us, we are all ears. Call us on 1800 266 4545, or shoot us an email at <a href="mailto:care@kotak.com" style="color:#003974;" target="_blank">care@kotak.com</a>.
                                <hr />
                            </div>
                        </div>
                    </div>
                    <div class="content-wrapper" id="divFailure" runat="server">
                         <div class="abs-center" style="position:unset!important">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Payment Failed!!!
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Oops! It looks like something went wrong and the payment wasn't processed.
                                    <br />
                                    Please note down following details for further communication:
                                    <br />
                                    <br />
                                 <%--   <strong>Quote Id: </strong>
                                    <asp:Label runat="server" ID="lblQuoteNumberFailed"  Text="-"></asp:Label>
                                    <br />--%>
                                    <strong>Proposal Id: </strong>
                                    <asp:Label runat="server" ID="lblProposalNumberFailed"  Text="-"></asp:Label>
                                    <br />
                                    <strong>Error: </strong>
                                    <asp:Label runat="server" ID="lblError"  Font-Bold="true" ForeColor="Red" />
                                    <br />
                                </p>
                                <hr />
                                For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                                <hr />
                            </div>
                        </div>
                    </div>
                </section>
            <!-- Page footer-->
            <%--<footer style="background-color: white">
                <span style="font-size: 12px; text-align: center; padding: 10px; float: left">
                    Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                </span>
            </footer>--%>
        </div>
    </form>
</body>
</html>