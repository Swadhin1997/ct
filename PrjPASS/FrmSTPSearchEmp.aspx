<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSTPSearchEmp.aspx.cs" Inherits="PrjPASS.FrmSTPSearchEmp" MaintainScrollPositionOnPostback="true" %>

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


<body style="background-color: #f0fcf8;">

    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>


    <%--  <script src="css/newcssjs/js/jquery-1.7.1.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.min.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>--%>
    <%--<script src="css/newcssjs/js/circular-countdown.min.js"></script>--%>
    <style>
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

        #DivORchild {
            position: relative;
            top: 7px;
            left : 21px;
        }
    </style>


    <form id="form1" runat="server">

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
                                    <span class="h3 text-bold">Kotak Health Super Top Up, Get more for less!</span>
                                </div>


                            </div>
                        </div>

                        <div class="row block-center mt-xl wd-xl">
                            <div class="col-sm-12">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-lg-12">
                                                <input type="text" class="form-control" placeholder="Enter Employee Code" name="txtEmpCode" id="txtEmpCode" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-lg-5">
                                                <input type="email" id="txtEmailID" name="txtEmailID" placeholder="Kotak Email ID" class="form-control" maxlength="50" />
                                            </div>
                                            <div class="col-lg-2 center-block" id="DivORParent">
                                                <div id="DivORchild" class="center-block">
                                                    <b>OR</b>
                                                </div>
                                            </div>
                                            <div class="col-lg-5">
                                                <input type="text" id="txtContactNumber" name="txtContactNumber" placeholder="Contact Number" class="form-control" maxlength="10" type="tel" />
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <button type="button" class="btn btn-default center-block" id="btnGo" onclick="">Go!</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </section>





            <br />
        <!-- Page footer-->
        <footer style="background-color: white; z-index: 113; position: fixed;">
            <span style="font-size: 12px; text-align: center; padding: 10px; float: left"><b>Kotak Health Super Top Up UIN: KOTHLIP18011V011718.</b>&nbsp;Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
            </span>
        </footer>


        </div>

    </form>

    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $(function () {

                $('#btnGo').click(function () {
                    debugger;
                    var txtEmpCode = $("#txtEmpCode").val();
                    var txtEmailID = $("#txtEmailID").val();
                    var txtContactNumber = $("#txtContactNumber").val();
                    if (txtEmpCode.length <= 0) {
                        alert('please enter employee code !');
                        return false;
                    }

                    if (txtEmailID.length <= 0 && txtContactNumber.length <= 0) {
                        alert('please enter either Kotak Email id or Registered Mobile Number!');
                        return false;
                    }

                    //else call webmethod 

                    var dataEmp = { EmployeeCode: txtEmpCode, EmailId: txtEmailID, ContactNumber: txtContactNumber };

                    $.ajax({
                        type: "POST",
                        url: "FrmSTPSearchEmp.aspx/ValidateAndSearchEmpCode",
                        data: JSON.stringify(dataEmp),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == "passwordnotmatch") {
                                alert('Provided details not matched !');
                                return false;
                            }
                            else if (response.d == "norecordfound") {
                                alert('Emp Code Does Not Match !');
                                return false;
                            }
                            else {
                                $("#txtEmpCode").val('');
                                $("#txtEmailID").val('');
                                $("#txtContactNumber").val('');
                                var redirectWindow = window.open(response.d, '_blank');
                                redirectWindow.location;
                            }
                        },
                        error: function (requestObject, error, errorThrown) {
                            alert('requestObject   ' + requestObject);
                            alert(error);
                        }
                    });

                });
            });
        });
    </script>

</body>
</html>
