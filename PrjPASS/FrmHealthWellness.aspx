<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmHealthWellness.aspx.cs" Inherits="PrjPASS.FrmHealthWellness" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link href="css/Wellness/Wellness.css" rel="stylesheet" />--%>
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


    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="js/bootstrap/bootstrap.min.js" type="text/javascript"></script>


    <%-- Added for sweet alert  --%>
    <%--<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>--%>
    <%-- Added for sweet alert  --%>

    <script src="js/SweetAlert.min.js" type="text/javascript"></script>

    <script type="text/javascript">


        $(document).ready(function () {
            ApplyDatePicker();
            $("#btnShow2").click(function () {
                // openmodalHealthWelness();
                ShowDisclaimerSweetAlert();
            });

        });


        //$(document).ready(function () {
        //    $(".form-control").form - control();
        //});


        function openmodalHealthWelness() {
            var member = $('#drpMembers option:selected').val();
            if (member == '0') {
                alert('Please select the member');
                return;
            }
            else {
                $('#modalHealthWelness').modal('show');
            }


        }


        function ClosemodalHealthWelness() {
            $('#modalHealthWelness').modal('hide');
        }

        function ApplyDatePicker() {
            $("[id$=txtDateofBirth]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/1940',
                yearRange: "-70:-18"
            });


            $("#datepickerImagedob").click(function () {
                $("[id$=txtDateofBirth]").datepicker("show");
            });


        }


        function ShowSweetAlert(msg) {
            swal({
                title: "Success!",
                text: msg,
                type: "success"
            }).then(okay => {
                if (okay) {
                    window.location.href = "FrmHealthWellness.aspx";
                }
            });
        }


        function ShowDisclaimerSweetAlert(url) {
            var span = document.createElement("span");
            span.innerHTML = "<ol type='1' style='font-size: 11px' ><li><P style='font-size: 11px' ALIGN='LEFT'>You agree that by clicking on the link, you will be redirected to the website of Truworth Health Technologies Private Limited (Truworth Wellness), our wellness service provider, to facilitate your request and begin your wellness journey. This link is provided only for your convenience and Kotak Mahindra General Insurance Company Limited does not in any manner undertake any liability, responsibility or reliability of any service, statement or other information displayed or provided through the website of Truworth Wellness or otherwise.</P></li><li><P style='font-size: 11px' ALIGN='LEFT'>We request you to refer the policy wordings pertaining to “Health and Rewards” and “Value Added Benefits” covers for detailed terms and conditions in this regard.</P></li><li><P style='font-size: 11px' ALIGN='LEFT'>You agree to share your policy information and/or any other personal information with Truworth Wellness as may be required by Truworth Wellness to avail or utilize the wellness services. You are requested to kindly refer and familiarize yourself to the Privacy Policy of Truworth Wellness before you share any details for the purpose of availing the Wellness services.</P></li><li><P style='font-size: 11px' ALIGN='LEFT'>You authorize Truworth Wellness to contact you over phone call/sms/ email for providing the Wellness services.</P></li><li><P style='font-size: 11px' ALIGN='LEFT'>The use of website of Truworth Wellness is subject to the terms of use, other terms and guidelines, if any, applicable to that website.</P></li><li><P style='font-size: 11px' ALIGN='LEFT'> Kotak Health Premier Product UIN: KOTHLIP21065V032021</P></li> <li><P style='font-size: 11px' ALIGN='LEFT'> In case you have any queries, doubts or concerns of any of the above conditions, you are requested to kindly get in touch with our Customer Care team at 1800 266 4545</P></li>"

            swal({
                title: "(*) Disclaimer:",
                content: span,
                html: true,
                buttons: {
                    cancel: true,
                    confirm: true,
                }
            }).then(okay => {
                if (okay) {
                    window.location.href = url;
                }
            });
        }

    </script>
</head>


<body>

    <style type="text/css">
        #tblSearchPolicy .td {
            margin: 5px;
        }

        .nav-tabs {
            border-bottom: 2px solid #DDD;
            background-color: #f0fcf8;
        }

            .nav-tabs > li.active > a, .nav-tabs > li.active > a:focus, .nav-tabs > li.active > a:hover {
                border-width: 0;
                background-color: #f0fcf8;
            }

            .nav-tabs > li > a {
                border: none;
                color: #666;
                background-color: #f0fcf8;
            }

                .nav-tabs > li.active > a, .nav-tabs > li > a:hover {
                    border: none;
                    color: #4285F4 !important;
                    background: transparent;
                    background-color: #f0fcf8;
                }

                .nav-tabs > li > a::after {
                    content: "";
                    /*background: #4285F4;*/
                    background-color: #f0fcf8;
                    height: 2px;
                    position: absolute;
                    width: 100%;
                    left: 0px;
                    bottom: -1px;
                    transition: all 250ms ease 0s;
                    transform: scale(0);
                }

            .nav-tabs > li.active > a::after, .nav-tabs > li:hover > a::after {
                transform: scale(1);
            }

        .tab-nav > li > a::after {
            background: #21527d none repeat scroll 0% 0%;
            background-color: #f0fcf8;
            color: #fff;
        }

        .tab-pane {
            padding: 15px 0;
        }

        .tab-content {
            padding: 20px;
        }

        .card {
            /*background: #FFF none repeat scroll 0% 0%;*/
            background: #f0fcf8 none repeat scroll 0% 0%;
            box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.3);
            margin-bottom: 30px;
            background-color: #f0fcf8;
        }


        table, th, td {
            border: 1px solid black;
        }

        body {
            font-family: "Source Sans Pro", sans-serif;
            /*color: #656565;*/
            /*background-color: #fff9ec;*/
            background-color: #f0fcf8;
        }

        .row {
            margin: 10px;
        }


        .center {
            display: block;
            margin: 0 auto;
        }


        #drpMembers {
            width: auto;
        }

        .lblWelcomeMessage {
            text-align: center;
        }

        #BloodGroup {
            width: auto;
        }

        #dvlblwelcomeMessage {
            text-align: center;
            font-size: 2.1em;
            text-align: center;
        }

        .header {
            width: 100%;
            float: left;
            position: relative;
            padding: 20px 0 0 0;
            margin-top: -20px;
            background-color: #f0fcf8;
            border: none !important;
        }

        .logo {
            padding: 15px 0 0 0;
            float: left;
        }

        .container {
            width: 100%;
            max-width: 1350px;
            margin: 0 auto;
            padding: 0 15px;
            position: relative;
            height: 100%;
            background-color: #f0fcf8;
        }

        .navbar-header {
            background-color: #f0fcf8;
            display: block;
            border: none !important;
        }

        .navigation {
            background-color: #f0fcf8;
            border: none !important;
        }

        .nav-wrapper {
            background-color: #f0fcf8;
            box-shadow: none;
        }

        .nav {
            background-color: #f0fcf8;
            border: none !important;
        }

        .navbar-brand {
            background-color: #f0fcf8;
            border: none !important;
        }

        .img-responsive {
            background-color: #f0fcf8;
        }

        .navbar-header {
            background-color: #f0fcf8;
            border: none !important;
        }

        .navbar {
            background-color: #f0fcf8;
            border: none !important;
        }

        .topnavbar {
            background-color: #f0fcf8;
            border: none !important;
        }

        .navbar-header {
            background-color: #f0fcf8;
            border: none !important;
        }

        .nav {
            background-color: #f0fcf8;
            border: none !important;
        }

        .navbar-nav {
            background-color: #f0fcf8;
            border: none !important;
        }

        .navbar-right {
            background-color: #f0fcf8;
            border: none !important;
        }

        .brand-logo-collapsed {
            background-color: #f0fcf8;
            border: none !important;
        }



        @media only screen and (max-width: 800px) {

            /* Force table to not be like tables anymore */
            #no-more-tables table,
            #no-more-tables thead,
            #no-more-tables tbody,
            #no-more-tables th,
            #no-more-tables td,
            #no-more-tables tr {
                display: block;
            }

                /* Hide table headers (but not display: none;, for accessibility) */
                #no-more-tables thead tr {
                    position: absolute;
                    top: -9999px;
                    left: -9999px;
                }

            #no-more-tables tr {
                border: 1px solid #ccc;
            }

            #no-more-tables td {
                /* Behave  like a "row" */
                border: none;
                border-bottom: 1px solid #eee;
                position: relative;
                padding-left: 50%;
                white-space: normal;
                text-align: left;
            }

                #no-more-tables td:before {
                    /* Now like a table header */
                    position: absolute;
                    /* Top/left values mimic padding */
                    top: 6px;
                    left: 6px;
                    width: 45%;
                    padding-right: 10px;
                    white-space: nowrap;
                    text-align: left;
                    font-weight: bold;
                }


                #no-more-tables td:before {
                    content: attr(data-title);
                }


            #healthbg {
                display: none;
            }
        }

        @media only screen and (max-width: 1900px) and (min-width: 1024px) {
            .bgimage img {
                width: 83%;
                display : block;
            }
            #healthbg {
            display : block;
            }
        }


        @media only screen and (max-width: 1024px) and (min-width: 768px) {
            .bgimage.leftAlign, .bgimage-inner.leftAlign {
                left: 0px !important;
                transform: translate(0, 0) !important;
                -webkit-transform: translate(0, 0) !important;
               // display : block;
            }
        }


        @media only screen and (max-width: 900px) {
            .bgimage {
                display: none;
            }
            #healthbg {
            visibility :hidden;
            display : none
            }
        }


        @media only screen and (max-width: 1024px) {
            .bgimage {
                max-width: 280px;
                clear: both;
                float: left;
                position: relative;
                top: 0 !important;
                display  :block
            }
        }

        @media screen and (max-width: 1366px), screen and (max-height: 640px) {
            .big-size-image {
                max-width: 395px;
            }

            .bgimage {
                position: absolute;
                bottom: 0;
                left: 0;
                line-height: 0;
                width: 100%;
                max-width: 430px;
                display: none;
                z-index: 0;
            }

        }


         @media only screen  (max-width: 1008px ) and (min-width:875px) {
            .bgimage {
                display: none;
            }
            #healthbg {
            visibility :hidden;
            display : none
            }
        }

    </style>



    <form id="form1" runat="server">
        <asp:HiddenField ID="HdnMemberID" runat="server" Value="0" />
        <asp:HiddenField ID="HdnPolicyNumber" runat="server" Value="0" />
        <asp:HiddenField ID="HdnPlanToken" runat="server" Value="0" />

        <div class="header" style="border: none">
            <nav role="navigation" class="navbar topnavbar" style="border: none">
                <div class="navbar-header" style="background-color: inherit; border: none">
                    <a href="#/" class="navbar-brand">
                        <div class="brand-logo">
                            <img src="Images/logo1.png" alt="App Logo" class="img-responsive" />
                        </div>
                        <div class="brand-logo-collapsed">
                            <img src="Images/logo1.png" alt="App Logo" class="img-responsive" />
                        </div>
                    </a>
                </div>
                <div class="nav-wrapper" style="border: none !important">

                    <ul class="nav navbar-nav navbar-right" style="border: none !important">
                        <li>
                            <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em>&nbsp;care@kotak.com</a>
                        </li>

                        <li>
                            <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em>&nbsp;1800 266 4545</a>
                        </li>

                    </ul>
                </div>

            </nav>
        </div>

        <div id="dvlblwelcomeMessage">
            <lable id="lblwelcomeMessage" runat="server"></lable>
        </div>

        <div class="row" id="tblSearchPolicy" runat="server">
            <div class="wrapper">
                <div class="block-center mt-xl wd-xl" style="width: 400px">
                    <div class="row">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4>Proposer Information</h4>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="form-group has-feedback col-lg-12 col-sm-12 col-xs-12">
                                        <label>Policy Number</label>
                                        <input id="txtpolicyNumber" type="text" runat="server" placeholder="Enter policy number" required class="form-control" autocomplete="off">
                                        <span class="fa form-control-feedback text-muted"></span>
                                    </div>
                                    <div class="form-group has-feedback col-lg-12 col-sm-12 col-xs-12">
                                        <label>Date of Birth</label>
                                        <input id="txtDateofBirth" runat="server" type="text" placeholder="Enter DOB DD/MM/YYYY" required class="form-control" autocomplete="off">
                                        <span class="fa form-control-feedback text-muted">
                                            <img style="margin-left: -20px" src="images/calendar.png" alt="" id="datepickerImagedob" /></span>
                                    </div>
                                    <div class="col-lg-12 col-sm-12 col-xs-12">
                                        <asp:Button ID="btnSearchPolicyDetails" runat="server" class="mb-sm btn btn-success center" ClientIDMode="Static" Text="Submit" OnClick="btnSearchPolicyDetails_Click" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
            <div class="bgimage leftAlign big-size-image"  id="healthbg">
                <img class="healthinsubg" style="display: block" src="forhealth-bg.png" />
            </div>
        </div>
        <br />

        <%--https://bootsnipp.com/snippets/featured/no-more-tables-respsonsive-table  --%>


        <div class="block-center" id="dvTblMembers" runat="server" visible="false">
            <div class="panel panel-default">
                <div class="panel-heading">Members</div>
                <div class="panel-body">
                    <div id="no-more-tables">
                        <asp:Literal ID="ltPolicyDetails" runat="server" Visible="true" Text=""></asp:Literal>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" id="dvDrpMemberDetails" runat="server" visible="false" align="center">
            <div class="col-3 col-md-3 col-lg-3 col-sm-3"></div>
            <div class="col-3 col-md-3 col-lg-3 col-sm-3">
                <asp:DropDownList ID="drpMembers" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpMembers_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="col-3 col-md-3 col-lg-3 col-sm-3">
                <%--<button id="btnShow2" type="button" value="Show" class="btn btn-success" runat="server" >Add Details</button>--%>
                <asp:Button ID="btnShowMember" runat="server" Text="Register" class="btn btn-success" OnClick="btnShowMember_Click" />
            </div>
            <div class="col-3 col-md-3 col-lg-3 col-sm-3"></div>

        </div>


        <div class="row" id="dvMessage" runat="server" align="center" visible="true">
            <div>
                <b>
                    <label id="lblMessage" runat="server"></label>
                    <u>
                        <asp:LinkButton ID="lbtnLogOut" runat="server" OnClick="lbtnLogOut_Click"> Click here to go back </asp:LinkButton>
                    </u>
                </b>
            </div>
        </div>


        <div class="modal fade" id="modalHealthWelness" role="dialog" data-backdrop="static" style="margin-top: -17px">
            <div class="modal-dialog" style="width: 96%; background-color: white">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #ec1c24; color: white">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h3 class="modal-title">
                            <label id="lblTitle" runat="server" />
                        </h3>
                    </div>
                    <div class="modal-body" style="background-color: white">
                        <div class="container" style="background-color: white">
                            <div class="row">
                                <div class="row">
                                    <label id="lblerrorMessage" runat="server" class="alert-danger .alert-dismissible" style="display: block"></label>
                                </div>
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <!-- Nav tabs -->
                                    <div class="card" style="background-color: white">
                                        <ul class="nav nav-tabs" role="tablist" style="background-color: lightgrey">
                                            <li role="presentation" class="active"><a href="#Biometrics" aria-controls="home" role="tab" data-toggle="tab">Biometrics</a></li>
                                            <li role="presentation"><a href="#PersonalHealth" aria-controls="PersonalHealth" role="tab" data-toggle="tab">Personal Health</a></li>
                                            <li role="presentation"><a href="#MedicalHistory" aria-controls="MedicalHistory" role="tab" data-toggle="tab">Medical History</a></li>
                                            <li role="presentation"><a href="#Lifestyle" aria-controls="Lifestyle" role="tab" data-toggle="tab">Lifestyle</a></li>
                                            <li role="presentation"><a href="#OccupationalHealth" aria-controls="settings" role="tab" data-toggle="tab">Occupational Health</a></li>
                                            <li role="presentation"><a href="#Social" aria-controls="settings" role="tab" data-toggle="tab">Social and Environmental Wellness </a></li>
                                        </ul>

                                        <!-- Tab panes -->
                                        <div class="tab-content" style="background-color: white">
                                            <div role="tabpanel" class="tab-pane active" id="Biometrics">
                                                <table cellspacing="10" border="1" cellpadding="10" class="table table-responsive">
                                                    <tr>
                                                        <td width="4%">1</td>
                                                        <td>Height
                                                        </td>
                                                        <td>
                                                            <input id="txtHeightFeet" type="text" runat="server">
                                                            Feet
                                                                <input id="txtHeightInch" type="text" runat="server">
                                                            Inch</td>
                                                    </tr>

                                                    <tr>
                                                        <td>2</td>
                                                        <td>Weight
                                                        </td>
                                                        <td>
                                                            <input id="txtWeight" type="text" runat="server">
                                                            Kg.
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>3</td>
                                                        <td>Blood Group
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="BloodGroup" runat="server" class="form-control col-sm-4">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem>A +</asp:ListItem>
                                                                <asp:ListItem>A -</asp:ListItem>
                                                                <asp:ListItem>B +</asp:ListItem>
                                                                <asp:ListItem>B -</asp:ListItem>
                                                                <asp:ListItem>AB +</asp:ListItem>
                                                                <asp:ListItem>AB -</asp:ListItem>
                                                                <asp:ListItem>O +</asp:ListItem>
                                                                <asp:ListItem>O -</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3">What is your blood pressure now?
                                                        </td>

                                                    </tr>


                                                    <tr>
                                                        <td>4</td>
                                                        <td>Blood pressure - High
                                                        </td>
                                                        <td>
                                                            <input id="BloodPressureHigh" type="text" runat="server">
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>5</td>
                                                        <td>Blood pressure - Low

                                                        </td>
                                                        <td>
                                                            <input id="BloodPressureLow" type="text" runat="server" min="60" max="100">
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="PersonalHealth">
                                                <table cellspacing="10" border="1" cellpadding="10" class="table table-responsive">

                                                    <tr>
                                                        <td width="4%">1</td>
                                                        <td>Diabetes</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="RdodiabYes" type="radio" name="i-radioDiabetes" value="Yes" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="RdodiabNo" type="radio" name="i-radioDiabetes" value="No" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>2</td>
                                                        <td>High Blood Pressure</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="inlineradio1BloodPressure" type="radio" name="i-radioBloodPressure" value="Yes" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="inlineradio2BloodPressure" type="radio" name="i-radioBloodPressure" value="No" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>3</td>
                                                        <td>Cholestrol Check? (Have you checked your cholestrol in last 5 years)</td>
                                                        <td>

                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="inlineradio1Cholestrol" type="radio" name="i-radioCholestrol" value="Yes" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="inlineradio2Cholestrol" type="radio" name="i-radioCholestrol" value="No" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>4</td>
                                                        <td>Cholesterol Level ( leave it blank if you don’t know)</td>
                                                        <td>

                                                            <input type="text" id="txtCholesterolLevel" runat="server" class="form-group-sm" />
                                                            mg/dl
                                                            
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>5</td>
                                                        <td>HDL "good" Cholesterol ( leave it blank if you don’t know)</td>
                                                        <td>

                                                            <input type="text" id="txtHDLCholesterol" runat="server" class="form-group-sm" />
                                                            mg/dl
                                                            
                                                        </td>

                                                    </tr>
                                                </table>


                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="MedicalHistory">
                                                <table cellspacing="10" border="1" cellpadding="10" class="table table-responsive">

                                                    <tr>
                                                        <td colspan="3">
                                                            <b>Have you suffered from any of the following diseases?
                                                            </b>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="4%">1</td>
                                                        <td width="60%">Cancer
                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Cancer" type="radio" name="i-radioCancer" value="Yes" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Cancer" type="radio" name="i-radioCancer" value="No" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>2</td>
                                                        <td>Heart Related
                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1HeartRelated" type="radio" name="i-radioHeartRelated" value="Yes" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2HeartRelated" type="radio" name="i-radioHeartRelated" value="No" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>3</td>
                                                        <td>Stroke
                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Stroke" type="radio" name="i-radioStroke" value="Yes" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Stroke" type="radio" name="i-radioStroke" value="No" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>4</td>
                                                        <td>Kidney/ Lung related
                                                        </td>
                                                        <td>

                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Kidney" type="radio" name="i-radioKidney" value="option1" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Kidney" type="radio" name="i-radioKidney" value="option2" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>5</td>
                                                        <td>Have you ever suffered from any eye/ skin/ dental/ ENT problems?
                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1ent" type="radio" name="i-radioENT" value="option1" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2ent" type="radio" name="i-radioENT" value="option2" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td colspan="2">
                                                            <textarea id="txtENTspecification" runat="server" class="form-control" rows="3" cols="30" placeholder="If yes then Please specify."></textarea><br />
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>6</td>
                                                        <td>Do you have complains of any of the following - Anemia, digestive problems, frequent allergies (cold/ cough/ asthama), pain in joints, frequent headaches, sleep disorders)</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Anemia" type="radio" name="i-radioAnemia" value="option1" runat="server">
                                                                    <span class="fa fa-circle"></span>Yes</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Anemia" type="radio" name="i-radioAnemia" value="option2" runat="server">
                                                                    <span class="fa fa-circle"></span>No</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td colspan="2">
                                                            <textarea id="txtAnemiaspecification" runat="server" class="form-control" rows="3" cols="30" placeholder="If yes then Please specify."></textarea><br />
                                                        </td>

                                                    </tr>
                                                </table>


                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="Lifestyle">
                                                <table cellspacing="10" border="1" cellpadding="10" class="table table-responsive">

                                                    <tr>
                                                        <td width="4%">1</td>
                                                        <td>How would you classify your lifestyle ? </td>

                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1LifestyleClass" type="radio" name="i-radioLifestyle" value="Moderatively active " runat="server">
                                                                    <span class="fa fa-circle"></span>Moderatively active (If you do exercise but get less than the 2 hours and 30 minutes per week of the moderate aerobic activity. Examples mowing lawn, riding a bike on level).</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2LifestyleClass" type="radio" name="i-radioLifestyle" value="Sedentary" runat="server">
                                                                    <span class="fa fa-circle"></span>Sedentary (if you spend a lot of time at a desk or watching TV,do low intensity exercises like easy walk;stretching; shopping and gardening)</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3LifestyleClass" type="radio" name="i-radioLifestyle" value="Active" runat="server">
                                                                    <span class="fa fa-circle"></span>Active (Daily exercise that is equal to walking for 1 hour and 45 minutes ,jogging)</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>2</td>
                                                        <td>How many hours do you sleep?</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1SleepHours" type="radio" name="i-radioSleepHour" value="Less than 1 hour" runat="server">
                                                                    <span class="fa fa-circle"></span>Less than 1 hour
                                                                </label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2SleepHours" type="radio" name="i-radioSleepHour" value="1-3 hours" runat="server">
                                                                    <span class="fa fa-circle"></span>1-3 hours</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3SleepHours" type="radio" name="i-radioSleepHour" value="3-7 hours" runat="server">
                                                                    <span class="fa fa-circle"></span>3-7 hours</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4SleepHours" type="radio" name="i-radioSleepHour" value="7-9 hours" runat="server">
                                                                    <span class="fa fa-circle"></span>7-9 hours</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>3</td>
                                                        <td>How many servings/portions of vegetables/ fruits you consume each day ?
                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1VegFruits" type="radio" name="i-radioVegetables" value="1-2" runat="server">
                                                                    <span class="fa fa-circle"></span>1-2</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2VegFruits" type="radio" name="i-radioVegetables" value="2-3" runat="server">
                                                                    <span class="fa fa-circle"></span>2-3</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3VegFruits" type="radio" name="i-radioVegetables" value="4-9" runat="server">
                                                                    <span class="fa fa-circle"></span>4-9</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4VegFruits" type="radio" name="i-radioVegetables" value="None" runat="server">
                                                                    <span class="fa fa-circle"></span>None</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>4</td>
                                                        <td>How many glasses of water do you consume during a working day? (1 Glass=250ml)</td>
                                                        <td>
                                                            <input type="text" runat="server" id="txtWater" />
                                                            -glasses.
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>5</td>
                                                        <td>
                                                            <b>SMOKING -</b>
                                                            How would you describe your cigarette smoking habits?

                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Smoking" type="radio" name="i-radioSmoking" value="Never smoked" runat="server">
                                                                    <span class="fa fa-circle"></span>Never smoked
                                                                </label>


                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Smoking" type="radio" name="i-radioSmoking" value="Used to smoke" runat="server">
                                                                    <span class="fa fa-circle"></span>Used to smoke</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3Smoking" type="radio" name="i-radioSmoking" value="Still smoke" runat="server">
                                                                    <span class="fa fa-circle"></span>4-9</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4Smoking" type="radio" name="i-radioSmoking" value="Still smoke" runat="server">
                                                                    <span class="fa fa-circle"></span>None</label>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>6</td>
                                                        <td>
                                                            <b>ALCHOHOL - </b>How often due you consume alcohol?
                                                        </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Alchohol" type="radio" name="i-radioAlcohol" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never                           
                                                                </label>


                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Alchohol" type="radio" name="i-radioAlcohol" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3Alchohol" type="radio" name="i-radioAlcohol" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4Alchohol" type="radio" name="i-radioAlcohol" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5Alchohol" type="radio" name="i-radioAlcohol" value="Always/ Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/ Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="OccupationalHealth">

                                                <table cellspacing="10" border="1" cellpadding="10" class="table table-responsive">

                                                    <tr>
                                                        <td width="4%">1</td>
                                                        <td>Do you enjoy your work?</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1EnjoyWork" type="radio" name="i-OccupationalHealthEnjoyWork" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never</label>



                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2EnjoyWork" type="radio" name="i-OccupationalHealthEnjoyWork" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>



                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3EnjoyWork" type="radio" name="i-OccupationalHealthEnjoyWork" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>



                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4EnjoyWork" type="radio" name="i-OccupationalHealthEnjoyWork" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>



                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5EnjoyWork" type="radio" name="i-OccupationalHealthEnjoyWork" value="Always/Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>2</td>
                                                        <td>Is the  level of stress in work environment is manageable for you?</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1StressLevel" type="radio" name="i-OccupationalHealthManageable" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never</label>


                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2StressLevel" type="radio" name="i-OccupationalHealthManageable" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3StressLevel" type="radio" name="i-OccupationalHealthManageable" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4StressLevel" type="radio" name="i-OccupationalHealthManageable" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5StressLevel" type="radio" name="i-OccupationalHealthManageable" value="Always/Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>3</td>
                                                        <td>Are you satisfied with your ability to manage and control the work load?</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1WorkLoad" type="radio" name="i-OccupationalHealthAbility" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2WorkLoad" type="radio" name="i-OccupationalHealthAbility" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3WorkLoad" type="radio" name="i-OccupationalHealthAbility" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4WorkLoad" type="radio" name="i-OccupationalHealthAbility" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5WorkLoad" type="radio" name="i-OccupationalHealthAbility" value="Always/Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="Social">
                                                <table cellspacing="10" border="1" cellpadding="10" class="table table-responsive">
                                                    <tr>
                                                        <td width="4%">1</td>
                                                        <td>Do you feel you have a good balance between your work & family life? </td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Balance" type="radio" name="i-SocialGoodBalance" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Balance" type="radio" name="i-SocialGoodBalance" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3Balance" type="radio" name="i-SocialGoodBalance" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4Balance" type="radio" name="i-SocialGoodBalance" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5Balance" type="radio" name="i-SocialGoodBalance" value="Always/Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>2</td>
                                                        <td>Do you take time to have meaningful interactions with family and friends?</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Interaction" type="radio" name="i-SocialInteractions" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Interaction" type="radio" name="i-SocialInteractions" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3Interaction" type="radio" name="i-SocialInteractions" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4Interaction" type="radio" name="i-SocialInteractions" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5Interaction" type="radio" name="i-SocialInteractions" value="Always/Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>3</td>
                                                        <td>In general,  are you satisfied with your life?</td>
                                                        <td>
                                                            <div class="radio">
                                                                <label class="radio-inline c-radio" style="margin-left: 9px">
                                                                    <input id="Radio1Satisfied" type="radio" name="i-SocialSatisfied" value="Never" runat="server">
                                                                    <span class="fa fa-circle"></span>Never</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio2Satisfied" type="radio" name="i-SocialSatisfied" value="Occasionally" runat="server">
                                                                    <span class="fa fa-circle"></span>Occasionally</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio3Satisfied" type="radio" name="i-SocialSatisfied" value="Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio4Satisfied" type="radio" name="i-SocialSatisfied" value="Very Often" runat="server">
                                                                    <span class="fa fa-circle"></span>Very Often</label>

                                                                <label class="radio-inline c-radio">
                                                                    <input id="Radio5Satisfied" type="radio" name="i-SocialSatisfied" value="Always/Almost Always" runat="server">
                                                                    <span class="fa fa-circle"></span>Always/Almost Always</label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="center">
                                                            <asp:Button class="mb-sm btn btn-success" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Submit" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal footer-->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>




    </form>
</body>
</html>
