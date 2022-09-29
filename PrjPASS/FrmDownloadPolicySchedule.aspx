<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDownloadPolicySchedule.aspx.cs" Inherits="PrjPASS.FrmDownloadPolicySchedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download Policy Schedule</title>

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

    <style>
        body {
            background-color: #f0fcf8;
        }

        #ddOr {
            position: relative;
            top: 21px;
        }

        .txtclass {
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


        #dvLoading {
            background: #000 url(images/loader.gif) no-repeat center center;
            height: 100px;
            width: 100px;
            position: fixed;
            z-index: 1000;
            left: 50%;
            top: 50%;
            margin: -25px 0 0 -25px;
        }

        @media screen and (max-width:1000px ) {
            #txtEmailforPolicy, #btnSendEmail, #btnGetPolicy {
                display: block;
                overflow: hidden;
                margin-top: 10px;
                margin: 10px;
            }
        }
    </style>
    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>

</head>
<body>


    <script src="js/SweetAlert.min.js" type="text/javascript"></script>

    <script type="text/javascript">



        function fnValidateOTPNumber(source, args) {
            var txtOtpNumber = $("#txtOtpNumber").val();
            args.IsValid = (txtOtpNumber.length > 5);
        }

        function runme() {

            $('.timer').circularCountDown({
                delayToFadeIn: 1,
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
                    $('#btnOTPSend').hide();
                    $('#btnOTPSend').attr('disabled', true);
                    $('#btnMobileReSend').attr('disabled', true);
                },
                end: function (countdown) {
                    $(".timercountTxt").show();
                    $('#btnMobileVerify').addClass('disabled');
                    $('#btnMobileVerify').attr('disabled', 'disabled');
                    $('#btnOTPSend').removeAttr('disabled');
                    $('#btnOTPSend').removeClass('disabled');
                    $('#btnMobileReSend').removeAttr('disabled');
                    $('#btnMobileReSend').removeClass('disabled');
                    document.getElementById("<%=txtTimer.ClientID%>").value = 'Your One Time Password has expired';
                }
            });
        }


        function openErrorModal() {
            $('#myModalError').modal('show');
        }


        function openInvalidPolicyErrorModal() {
            $('#InvalidPolicyModalError').modal('show');
        }

        function openPolicyEmailSentModel() {
            $('#PolicyEmailSent').modal('show');
        }


        function openPolicyEmailNotSentModel() {
            $('#PolicyEmailNotSent').modal('show');
        }



        $(document).ready(function () {
            $('#btnGetPolicy').click(function (event) {
                $('#dvLoading').fadeOut(6000);
            });
        });

    </script>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
            <ContentTemplate>
                <asp:HiddenField ID="hdnProductCode" runat="server" />
                <asp:HiddenField ID="hdnDeptCode" runat="server" />
                <div class="wrapper">
                    <!-- top navbar-->
                    <!-- top navbar-->
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
                        <div class="content-wrapper col-lg-6 col-md-6">
                            <div class="container container-md">
                                <div class="row mb-lg">
                                    <div class="col-lg-12" style="text-align: center;">
                                        <div>
                                            <span class="h3 text-bold">Download Policy Schedule</span>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="panel panel-default">
                                            <div class="panel-heading text-center" style="background-color: #d20f0f; color: white; font-weight: bold;">Fill Required Details Below</div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                        <dl>
                                                            <dt>Enter Policy Number <span style="color: red">*</span></dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtPolicyNumber" runat="server" MaxLength="40" type="tel" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>

                                                    <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                        <dl>
                                                            <dt>Registered Mobile Number </dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtMobileNumber" runat="server" MaxLength="10" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>

                                                    <div class="col-sm-1 col-1 col-lg-1" style="vertical-align: central; text-align: center">
                                                        <dl>
                                                            <dt></dt>
                                                            <dd id="ddOr">Or</dd>
                                                        </dl>

                                                    </div>

                                                    <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">

                                                        <dl>
                                                            <dt>Registered Email ID </dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtEmailID" runat="server" MaxLength="100" type="email" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>

                                                    <div class="col-sm-12 col-12 col-lg-12" id="btnOTPSendDIV" runat="server">
                                                        <div style="float: right">
                                                            <asp:Button ID="btnOTPSend" runat="server" Text="GET OTP" CssClass="btn btn-primary" OnClick="btnOTPSend_Click" />
                                                            <asp:Button ID="btnReset" runat="server" Text="RESET" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="col-lg-12">
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
                                                        <p>
                                                            <asp:Label ID="lblMobMessage" runat="server" Text=""></asp:Label>
                                                            <br />
                                                            Please enter the One Time Password (OTP) sent on your mobile number
                                                        </p>
                                                        <div class="inputBox">
                                                            <div class="block-center text-center">
                                                                <p>
                                                                    <asp:TextBox ID="txtOtpNumber" TextMode="Password" runat="server" Style="border-radius: 7px; border: 1px solid #dde6e9; padding: 2px 7px; font-size: 13px;" Text="" MaxLength="6" AutoCompleteType="Disabled" />
                                                                </p>
                                                            </div>
                                                            <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ValidationGroup="otp" Display="Dynamic"
                                                                ErrorMessage="Please provide valid otp number" ClientValidationFunction="fnValidateOTPNumber" />
                                                        </div>

                                                        <div class="otpButton align-center">
                                                            <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClick="onClickbtnMobileVerify" />
                                                            <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-warning" ClientIDMode="Static" Text="Resend OTP" OnClick="onClickbtnMobileReSend" />
                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="col-lg-12" runat="server" id="dvPolicyDetails" visible="false">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            Welcome
                                                            <asp:Label ID="lblName" runat="server"></asp:Label>, Get Your Policy Details.
                                                        </div>
                                                        <div class="panel-body" style="vertical-align: central">
                                                            <div class="row">
                                                                <div class="col-3 col-sm-3 col-md3">
                                                                    <asp:TextBox ID="txtEmailforPolicy" runat="server" MaxLength="100" type="email" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                                                </div>
                                                                <div class="col-3 col-sm-3 col-md3" style="overflow: hidden">
                                                                    <button id="btnSendEmail" runat="server" type="button" class="btn btn-labeled btn-primary" onserverclick="SendSchedulePolicyEmail"><span class="btn-label"><i class="fa fa-envelope"></i></span>E-Mail Policy Schedule</button>
                                                                </div>
                                                                <div class="col-3 col-sm-3 col-md3" style="float: left">
                                                                    <button id="btnGetPolicy" runat="server" type="button" class="btn btn-labeled btn-primary" onserverclick="DownloadSchedulePolicy"><span class="btn-label"><i class="fa fa-download"></i></span>Download Policy Schedule</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section id="sectionError" style="min-height: 500px;" runat="server" visible="false">
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


                    <section id="sectionRecordNotFound" style="display: none; min-height: 500px;" runat="server">
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

                    <div class="modal fade" id="myModalSuccess" role="dialog" data-backdrop="static">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Status</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblStatusSuccess" runat="server" Style="color: red" />
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>
                    <br />
                    <!-- Page footer-->
                    <footer style="max-height: 60px; background-color: white; z-index: 113; position: fixed; bottom: 0px">
                        <span style="font-size: 12px; text-align: center; padding: 5px; float: left"><b>Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak General Insurance Company Ltd.  CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                        </span>
                    </footer>
                </div>
                </b>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGetPolicy" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
            <ProgressTemplate>
                <div id="resultLoading">
                    <img alt="" src="Images/ajax-loader.gif">
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
    <div class="modal fade error" id="myModalError" role="dialog" data-backdrop="static">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header alert alert-info fade in">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Authentication error!!</h4>
                </div>
                <div class="modal-body">
                    Details keyed-in do not match with our records available in the policy. Please try again with your correct policy number and registered contact details as mentioned in policy.  For assistance call on 1800 266 4545 or email us to care@kotak.com. 
                </div>
                <!-- Modal footer-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <div class="modal fade error" id="InvalidPolicyModalError" role="dialog" data-backdrop="static">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header alert alert-info fade in">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Policy Not Found!!</h4>
                </div>
                <div class="modal-body">
                    Provided policy number do not match with our records. Please try again with your correct policy number and registered contact details as mentioned in policy.  For assistance call on 1800 266 4545 or email us to care@kotak.com. 
                </div>
                <!-- Modal footer-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


     <div class="modal fade error" id="PolicyEmailSent" role="dialog" data-backdrop="static">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header alert alert-info fade in">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Policy Sent !!</h4>
                </div>
                <div class="modal-body">
                    Your Policy has been sent on your E-mail ID. Kindly check you Email Inbox. 
                </div>
                <!-- Modal footer-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <div class="modal fade error" id="PolicyEmailNotSent" role="dialog" data-backdrop="static">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header alert alert-info fade in">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Email Not sent!!</h4>
                </div>
                <div class="modal-body">
                    Due to some technicall reason email not sent. For assistance call on 1800 266 4545 or email us to care@kotak.com. 
                </div>
                <!-- Modal footer-->
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

</body>

</html>
