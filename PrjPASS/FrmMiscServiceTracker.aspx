<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMiscServiceTracker.aspx.cs" Inherits="PrjPASS.FrmMiscServiceTracker" %>

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


<body style="background-color: white;">

    <script src="css/newcssjs/js/jquery.js"></script>


    <form id="form1" runat="server" style="font-size: 12px;">



        <div class="wrapper" style="overflow-x: inherit; background-color: #fff8e8">
            <!-- top navbar-->
            <header class="topnavbar-wrapper">
                <!-- START Top Navbar-->
                <nav role="navigation" class="navbar topnavbar">
                    <!-- START navbar header-->
                    <div class="navbar-header">
                        <a href="https://www.kotakgeneralinsurance.com/" class="navbar-brand">
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



            <section id="sectionMain" runat="server">
                <div class="content-wrapper" style="min-height: 440px;">
                    <div class="container container-md">

                        <div class="row mb-lg">
                            <div class="col-lg-12" style="text-align: center;">
                                <div>
                                    <span class="h3 text-bold">Daily MISC Application and Service Tracker</span>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-8">
                                <div class="row">

                                    <div class="col-lg-6">

                                        <div id="panelDemo13" class="panel panel-info">
                                            <div class="panel-heading">SMS Status</div>
                                            <div class="panel-body">

                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>SMS For</th>
                                                                <th>Status</th>
                                                                <th>Status As On</th>
                                                            </tr>
                                                        </thead>
                                                        <asp:Literal ID="LiteralSMSStatus" runat="server" Text=""></asp:Literal>
                                                    </table>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="col-lg-6">

                                        <div id="panelDemo8" class="panel panel-primary">
                                            <div class="panel-heading">Service Availability Status</div>
                                            <div class="panel-body">

                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>Application</th>
                                                                <th>Status</th>
                                                                <th>Status As On</th>
                                                            </tr>
                                                        </thead>
                                                        <asp:Literal ID="LiteralServiceAvailabilityStatus" runat="server" Text=""></asp:Literal>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">

                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo18" class="panel panel-danger">
                                            <div class="panel-heading">Today's Overall Error Messages</div>
                                            <div class="panel-body">

                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>Module</th>
                                                                <th>Error Message</th>
                                                                <th>Error Datetime</th>
                                                            </tr>
                                                        </thead>
                                                        <%--<tbody>
                                                            <tr class="success">
                                                                <td>Premium Calculation</td>
                                                                <td>The HTTP service located at http://10.221.12.124/GCIntegrationServices/CustomerPortalService.svc is unavailable.  This could be because the service is too busy or because no endpoint was found listening at the specified address. Please ensure that the address is correct and try accessing the service again later</td>
                                                                <td>02-Feb-2018 15:58 PM</td>

                                                            </tr>
                                                        </tbody>--%>

                                                        <asp:Literal ID="LiteralError" runat="server" Text=""></asp:Literal>

                                                    </table>
                                                </div>
                                            </div>

                                        </div>
                                        <!-- END panel-->
                                    </div>

                                </div>
                            </div>

                            <div class="col-lg-4">

                                <div id="panelDemo14" class="panel panel-warning">
                                    <div class="panel-heading">Daily Report Status</div>
                                    <div class="panel-body">

                                        <div class="table-responsive">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th>Report Type</th>
                                                        <th>Status</th>
                                                        <th>Status As On</th>
                                                    </tr>
                                                </thead>
                                                <asp:Literal ID="LiteralReportStatus" runat="server" Text=""></asp:Literal>
                                            </table>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>




                        <div style="display: none">
                            <div class="row">
                                <div class="col-lg-6">
                                    <!-- START panel-->
                                    <div id="panelDemo1" class="panel panel-info">
                                        <div class="panel-heading">Policy Status</div>
                                        <div class="panel-body">

                                            <div class="table-responsive">
                                                <table class="table">
                                                    <thead>
                                                        <tr>
                                                            <th>Application</th>
                                                            <th>Policy Count</th>
                                                            <th>Total Premium</th>
                                                            <th>DateTime</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr class="success">
                                                            <td>GIST</td>
                                                            <td>
                                                                <asp:Label ID="lblGISTPolicyCount" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblGISTPolicyTotalPremium" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblGISTPolicyDateTime" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                        <tr class="info">
                                                            <td>PASS</td>
                                                            <td>
                                                                <asp:Label ID="lblPASSPolicyCount" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPASSPolicyTotalPremium" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPASSPolicyCountDateTime" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                        <tr class="warning">
                                                            <td>CP</td>
                                                            <td>
                                                                <asp:Label ID="lblCPPolicyCount" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCPPolicyTotalPremium" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCPPolicyDateTime" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                        <tr class="danger">
                                                            <td>GPA</td>
                                                            <td>
                                                                <asp:Label ID="lblGPAPolicyCount" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblGPAPolicyTotalPremium" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblGPAPolicyDatetime" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                        <tr class="success">
                                                            <td>PEERLESS FTD</td>
                                                            <td>
                                                                <asp:Label ID="lblPeerlessBooking_PolicyCount_FTD" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPeerlessBooking_PolicyPremium_FTD" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lbllblPeerlessBookingDateTime_FTD" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                        <tr class="info">
                                                            <td>PEERLESS MTD</td>
                                                            <td>
                                                                <asp:Label ID="PeerlessBooking_PolicyCount_MTD" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPeerlessBooking_PolicyPremium_MTD" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lbllblPeerlessBookingDateTime_MTD" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- END panel-->
                                </div>
                            </div>

                            <div class="row">


                                <div class="col-lg-6">
                                    <!-- START panel-->

                                    <div class="col-xd-6">
                                        <div id="panelDemo9" class="panel panel-success">
                                            <div class="panel-heading">PASS Quote Count</div>
                                            <div class="panel-body">

                                                <div class="table-responsive">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>Application</th>
                                                                <th>Quote Count</th>
                                                                <th>DateTime</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr class="info">
                                                                <td>PASS</td>
                                                                <td>
                                                                    <asp:Label ID="lblPASSQuoteCount" runat="server" Text="0"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="Label27" runat="server" Text="0"></asp:Label></td>
                                                            </tr>
                                                            <tr class="danger">
                                                                <td>PARTNER</td>
                                                                <td>
                                                                    <asp:Label ID="lblPassPartnerQuoteCount" runat="server" Text="0"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="Label29" runat="server" Text="0"></asp:Label></td>
                                                            </tr>
                                                            <tr class="success">
                                                                <td>MOBILE</td>
                                                                <td>
                                                                    <asp:Label ID="lblPASSMobileQuoteCount" runat="server" Text="0"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="Label31" runat="server" Text="0"></asp:Label></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <!-- END panel-->
                                </div>

                                <div class="col-lg-6">
                                    <div id="panelDemo12" class="panel panel-primary">
                                        <div class="panel-heading">GIST Claim Count</div>
                                        <div class="panel-body">

                                            <div class="table-responsive">
                                                <table class="table">
                                                    <tbody>
                                                        <tr class="danger">
                                                            <th>FTD</th>
                                                            <th>MTD</th>
                                                            <th>DateTime</th>
                                                        </tr>
                                                        <tr class="info">
                                                            <td>
                                                                <asp:Label ID="lblGISTClaimCountFTD" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblGISTClaimCountMTD" runat="server" Text="0"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblGISTClaimCountDateTime" runat="server" Text="0"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="panel-body">
                            <div class="row">
                                <div id="PanelDemoSMSstatusTracker" class="panel panel-info">
                                    <div class="panel-heading">SMS Status Tracker</div>
                                    <div class="panel-body">
                                        <asp:Literal ID="LiteralSMSstatusTracker" runat="server"></asp:Literal>
                                    </div>
                                </div>

                                <div id="PanelDemoMailstatusTracker" class="panel panel-info">
                                    <div class="panel-heading">Mail Status Tracker</div>
                                    <div class="panel-body">
                                        <asp:Literal ID="LiteralMailstatusTracker" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                            </div>
            </section>



            <br />

            <!-- Page footer-->
            <%-- <footer style="background-color: white; z-index: 113;">
                <span style="font-size: 12px; text-align: center; padding: 10px; float: left">
                    <strong>Disclaimer:</strong>
                    Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak Mahindra General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                </span>
            </footer>--%>
        </div>

    </form>





    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script type="text/javascript">

        $(window).on('load', function () {


        });


    </script>
</body>
</html>
