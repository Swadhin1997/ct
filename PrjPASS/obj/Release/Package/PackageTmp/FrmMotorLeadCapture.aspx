<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMotorLeadCapture.aspx.cs" Inherits="PrjPASS.FrmMotorLeadCapture" %>

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
    <script src="css/newcssjs/js/circular-countdown.js"></script>



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
    </style>
    <script>

        $(function () {
            callpopup();
        });

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



        <div class="wrapper" style="overflow-x: inherit">
            <!-- top navbar-->
            <header class="topnavbar-wrapper">
                <!-- START Top Navbar-->
                <nav role="navigation" class="navbar topnavbar">
                    <!-- START navbar header-->
                    <div class="navbar-header">
                        <a href="https://www.kotakgeneralinsurance.com/" class="navbar-brand">
                            <div class="brand-logo">
                                <img src="Images/logo1.png" alt="App Logo" class="img-responsive" onclick />
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
                                <a href="https://www.kotakgeneralinsurance.com/docs/default-source/default-document-library/2-fold-leaflet_car-secure-finalff10a6d8ab7a60adacbfff0000d284de.pdf?sfvrsn=0" target="_blank"><em class="fa fa-download"></em>&nbsp;Download Brochure</a>
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

            <asp:HiddenField ID="hdnIsDisclaimerShown" runat="server" Value="0" />

            <section id="sectionMain" runat="server">
                <div class="content-wrapper" style="min-height: 500px;">
                    <div class="container container-md">
                        <div class="row" style="margin-bottom: 0px">
                            <div class="col-md-8">

                                <div class="h3 text-bold">
                                    Dear Customer,
                                    <asp:Label ID="lblApacNumber" runat="server" Text="-" Style="display: none;"></asp:Label>
                                </div>
                                <p class="">Please have a look  at your motor car insurance premium. This is calculated based on the details available with us.</p>
                            </div>
                            <div class="col-md-4">
                                <div class="panel b">
                                    <div class="panel-body text-center" style="color: #bc8d3d; padding: 0px">

                                        <h3>Premium (Including GST)
                                            <br />
                                        </h3>
                                        <h2>&#x20b9;
                                            <asp:Label ID="lblTotalPremium2" runat="server" Text="0.00"></asp:Label>
                                            /-
                                        </h2>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-sm-6">
                                <!-- START widget-->
                                <div id="panelPortlet5" class="panel widget">
                                    <div class="portlet-handler ui-sortable-handle">
                                        <div class="row row-table row-flush">
                                            <div class="col-xs-4 bg-info text-center">
                                                <em class="fa fa-car fa-2x"></em>
                                            </div>
                                            <div class="col-xs-8">
                                                <div class="panel-body text-center" style="padding: 10px">

                                                    <h4>
                                                        <p class="mb0 text-bold">
                                                            <asp:Label ID="lblMakeModelVariant" runat="server" Text="-"></asp:Label>
                                                        </p>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="panelPortlet6" class="panel widget">
                                    <div class="portlet-handler ui-sortable-handle">
                                        <div class="row row-table row-flush">
                                            <div class="col-xs-4 bg-danger text-center">
                                                <em class="fa fa-list-alt fa-2x"></em>
                                            </div>
                                            <div class="col-xs-8">
                                                <div class="panel-body text-center" style="padding: 10px">

                                                    <h4>
                                                        <p class="mb0 text-bold">
                                                            <asp:Label ID="lblRegistrationNumber" runat="server" Text="-"></asp:Label>
                                                        </p>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="panelPortlet4" class="panel widget">
                                    <div class="portlet-handler ui-sortable-handle">
                                        <div class="row row-table row-flush">
                                            <div class="col-xs-4 bg-success text-center">
                                                <em class="fa fa-fire fa-2x"></em>
                                            </div>
                                            <div class="col-xs-8">
                                                <div class="panel-body text-center" style="padding: 10px">

                                                    <h4>
                                                        <p class="mb0 text-bold">
                                                            <asp:Label ID="lblFuelType" runat="server" Text="-"></asp:Label>
                                                        </p>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <div id="accordion1" class="panel-group">
                                    <div class="panel panel-default b0">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapse1" class="collapsed" aria-expanded="true">
                                                    <small>
                                                        <em class="fa fa-car text-primary mr"></em>
                                                    </small>
                                                    <span>Detailed Break up</span>
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="acc1collapse1" class="panel-collapse collapse in" aria-expanded="true">
                                            <div class="panel-body">

                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>IDV</dt>
                                                            <dd>
                                                                <asp:Label ID="lblIDV" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>Basic OD Premium</dt>
                                                            <dd>
                                                                <asp:Label ID="lblBasicODPremium" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>NCB %</dt>
                                                            <dd>
                                                                <asp:Label ID="lblNCB" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>


                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>NCB Discount</dt>
                                                            <dd>
                                                                <asp:Label ID="lblNCBDiscount" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>Net OD Premium</dt>
                                                            <dd>
                                                                <asp:Label ID="lblNetODPremium" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>Basic TP Premium</dt>
                                                            <dd>
                                                                <asp:Label ID="lblBasicTPPremium" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <dl>
                                                            <dt>PA For Owner Driver</dt>
                                                            <dd>
                                                                <asp:Label ID="lblPAForOwnerDriver" runat="server" Text="-"></asp:Label></dd>
                                                        </dl>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="col-md-3">
                                <div class="panel b">
                                    <div class="panel-heading bb">
                                        <h4 class="panel-title">Final Premium</h4>
                                    </div>
                                    <table class="table">
                                        <tbody>
                                            <tr>
                                                <td>Net Premium</td>
                                                <td>
                                                    <div class="text-right text-bold">
                                                        &#x20b9;
                                                        <asp:Label ID="lblNetPremium2" runat="server" Text="0.00"></asp:Label>
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
                                        </tbody>
                                    </table>
                                    <div class="panel-body" style="padding: 15px;">
                                        <div class="clearfix">
                                            <div class="pull-right text-right">
                                                <div class="text-bold">
                                                    &#x20b9;
                                                    <asp:Label ID="lblTotalPremium" runat="server" Text="0.00"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="pull-left text-bold text-dark">TOTAL</div>
                                        </div>
                                    </div>
                                    <div style="text-align: center; margin-bottom: 10px;">
                                        <%--<div class="panel-body" style="padding: 5px;">
                                            <div class="checkbox c-checkbox needsclick" id="agreewithbtn">
                                                <label>
                                                    <input type="checkbox" value="" class="needsclick" id="chkIAgree" runat="server" /><span class="fa fa-check"></span>I Authorize Kotak Mahindra General Insurance To Call Me
                                                </label>
                                            </div>
                                        </div>

                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="* select terms.." OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>--%>
                                        <asp:Button ID="btnCallback" runat="server" Text="Request A Callback For A Customized Quote" OnClick="btnCallback_Click" CssClass="btn btn-primary btn-block" Style="width: 90%; margin: auto" ValidationGroup="vg1" />
                                    </div>
                                </div>
                            </div>

                        </div>




                        <div class="popUp termsCondition2">
                            <a class="close-icon" href="javascript:;"></a>
                            <div class="popUpcontent">
                                <div class="container">
                                    <div class="row-grid" style="background-color: white; padding: 15px;">
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


                    </div>
                </div>

                <p style="font-family: 'Lato-Light'; color: #888888; font-size: 1.3rem; text-align: left; font-style: italic; padding: 20px">
                </p>
            </section>
            <section id="sectionThankYou" runat="server">
                <div class="content-wrapper" style="min-height: 500px;">
                    <div class="container container-md">

                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Thank You
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    We have received your response. We will get back to you shortly.
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

            <section id="sectionRecordNotFound" runat="server" visible="false">
                <div class="content-wrapper" style="min-height: 500px;">
                    <div class="container container-md">

                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Sorry, nothing to display
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Either you have already submitted the request or your record not exists in the system
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
            <footer style="background-color: #fff8e8">
                <span style="font-size: 12px; text-align: center; padding: 10px; float: left">
                    <strong>Disclaimer:</strong>
                    Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak Mahindra General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                </span>
            </footer>

        </div>

    </form>

    <div id="ModalRedirectionDisclaimer" class="modal fade" role="dialog" style="z-index: 9999">
        <div class="modal-dialog modal-lg" style="font-size: 13px; width: 90%">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Disclaimer</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <strong>You are being taken to the website of Kotak Mahindra General Insurance Company Ltd. to facilitate your request. Please read the clarifications below:</strong>
                    </p>
                    <ol>
                        <li>The Customer further authorizes Kotak Mahindra General Insurance Company Ltd. and / or Kotak Mahindra Prime Ltd. to make a phone call/sms to the customer for assisting him in the process of solicitation of insurance and/or for servicing the insurance policy(ies).The Customer further authorizes Kotak Mahindra General Insurance Company Ltd. and / or Kotak Mahindra Prime Ltd. to make a phone call/sms to the customer for assisting him in the process of solicitation of insurance and/or for servicing the insurance policy(ies).</li>
                        <li>The Customer understands that Kotak Mahindra Prime Ltd. (license no- CA0271) is a corporate agent of Kotak Mahindra General Insurance Company Ltd.</li>
                        <li>The Customer also understands that contract of insurance is between him and Kotak Mahindra General Insurance Company Ltd.</li>
                        <li>The use of website of Kotak Mahindra General Insurance Company Ltd. is subject to the terms of use, other terms and guidelines, if any, contained within that website.</li>
                        <li>The Customer understands and acknowledges that availing of any services offered or any reliance on any opinion, statement, or information available on the website of Kotak Mahindra General Insurance Company Ltd. shall be at his/her sole risk.</li>
                        <li>For more details on risk factors, terms and conditions please read sales brochure carefully before concluding a sale.</li>
                        <li>The Contract of insurance would be governed by the policy terms and conditions.</li>
                    </ol>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <!-- JQUERY-->
    <%-- <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.min.js"></script>--%>

    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script type="text/javascript">

        $(window).on('load', function () {

            var IsVisible = $("#sectionRecordNotFound").is(":visible");

            if (IsVisible == false) {
                var hdnIsDisclaimerShown = $("#hdnIsDisclaimerShown").val();
                if (hdnIsDisclaimerShown == "0") {
                    $('#ModalRedirectionDisclaimer').modal('show');

                    setTimeout(function () {
                        $('#ModalRedirectionDisclaimer').modal('hide');
                    }, 10000);
                }
            }
        });


    </script>
</body>
</html>
