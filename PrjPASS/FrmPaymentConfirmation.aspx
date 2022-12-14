<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPaymentConfirmation.aspx.cs" Inherits="PrjPASS.FrmPaymentConfirmation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

        <form id="form1" runat="server">
            <section>

                <div class="content-wrapper" id="divSuccess" runat="server">
                    <div class="abs-center">
                        <div class="text-center mv-lg">
                            <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">Thank You</div>
                            <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                Your payment has been successfully processed. Kindly note down the following important details pertaining to your policy. 
                            <br />
                                You will receive the policy document shortly. 
                            <br />
                                <br />
                               <%-- <strong>Quote Id: </strong>
                                <asp:Label ID="lblQuoteNumber" runat="server" Text="-"></asp:Label>
                                <br />
                                <strong>Proposal Id: </strong>
                                <asp:Label ID="lblProposalNumber" runat="server" Text="-"></asp:Label>
                                <br />
                                <strong>Payment Reference Number: </strong>
                                <asp:Label ID="lblTransactionId" runat="server" Text="-"></asp:Label>
                                <br />--%>

                                 <strong>Policy Number: </strong>
                                <asp:Label ID="lblPolicyNumber" runat="server" Text=""></asp:Label>
                                <br />                                
                                <strong>Payment Reference Number: </strong>
                                <asp:Label ID="lblTransactionId" runat="server" Text="-"></asp:Label>
                                <br />
                                <asp:Button ID="btnGetPolicy" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  ClientIDMode="Static" Text="Download PDF" OnClick="btnGetPolicy_Click" />
                            </p>

                            <hr />
                            For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                            <hr />

                        </div>
                    </div>
                </div>

                <div class="content-wrapper" id="divFailure" runat="server" visible="false">
                    <div class="abs-center">
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
                                <strong>Quote Id: </strong>
                                <asp:Label ID="lblQuoteNumberFailed" runat="server" Text="-"></asp:Label>
                                <br />
                                <strong>Proposal Id: </strong>
                                <asp:Label ID="lblProposalNumberFailed" runat="server" Text="-"></asp:Label>
                                <br />
                                <strong>Error: </strong>
                                <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" />
                                <br />
                            </p>

                            <hr />
                            For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                            <hr />

                        </div>
                    </div>
                </div>


            </section>
        </form>

        <!-- Page footer-->
        <footer style="background-color: white">
            <span style="font-size: 12px; text-align: center; padding: 10px; float: left">Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
            </span>
        </footer>

    </div>



    <!-- JQUERY-->
    <script src="css/newcssjs/js/jquery.js"></script>
    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
</body>
</html>
