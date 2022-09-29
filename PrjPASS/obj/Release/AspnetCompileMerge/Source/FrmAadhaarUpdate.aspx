<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAadhaarUpdate.aspx.cs" Inherits="PrjPASS.FrmAadhaarUpdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KGI - Update Aadhaar and PAN</title>

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
    <link href="css/newcssjs/jquery-ui.css" rel="stylesheet" />

    <style>
        .form-control2 {
            width: 100%;
            height: 25px;
            padding: 2px 7px;
            font-size: 13px;
        }
    </style>
</head>


<body style="background-color: #fff8e8; font-size: 13px;">

    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
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

        .chkRed {
            border: 1px solid red;
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


        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
            <ContentTemplate>

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


                </script>

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
                        <div class="content-wrapper" style="min-height: 500px;">

                            <div class="container container-md">
                                <div class="row mb-lg">
                                    <div class="col-lg-12" style="text-align: center;">

                                        <div>
                                            <span class="h3 text-bold">Update Your Aadhaar and PAN Number</span>
                                        </div>


                                    </div>
                                </div>
                                <br />
                                <div class="row">


                                    <div class="col-lg-8">





                                        <div class="panel panel-default">
                                            <div class="panel-heading text-center" style="background-color: #d20f0f; color: white; font-weight: bold;">Fill Required Details Below (As per Aadhaar)</div>
                                            <div class="panel-body">


                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Full Name <span style="color: red">*</span></dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control form-control2" MaxLength="50" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Date of Birth <span style="color: red">*</span></dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control form-control2" MaxLength="10" placeholder="dd/mm/yyyy" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Gender <span style="color: red">*</span></dt>
                                                            <dd>


                                                                <label class="radio-inline c-radio">
                                                                    <input id="radioMale" type="radio" name="i-radio" value="Male" checked="" runat="server" />
                                                                    <span class="fa fa-circle"></span>Male</label>
                                                                <label class="radio-inline c-radio">
                                                                    <input id="radioFeMale" type="radio" name="i-radio" value="Female" runat="server" />
                                                                    <span class="fa fa-circle"></span>Female</label>


                                                            </dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Email Id <span style="color: red">*</span></dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control form-control2" MaxLength="100" TextMode="Email" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Mobile Number <span style="color: red">*</span></dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control form-control2" MaxLength="10" TextMode="Phone" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Aadhaar Number <span style="color: red">*</span>
                                                                <a href="#" data-toggle="tooltip" data-placement="top" title="In case you dont have aadhaar number or PAN please write to us at care@kotak.com or call us at 18002664545 ( 8 am to 8 pm)"><i class="fa fa-question-circle" aria-hidden="true"></i></a>
                                                            </dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtAadhaarNumber" runat="server" CssClass="form-control form-control2" MaxLength="12" autocomplete="off"></asp:TextBox>

                                                            </dd>
                                                        </dl>
                                                    </div>

                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>PAN Number <span style="color: red">*</span>
                                                                <a href="#" data-toggle="tooltip" data-placement="top" title="In case you dont have aadhaar number or PAN please write to us at care@kotak.com or call us at 18002664545 ( 8 am to 8 pm)"><i class="fa fa-question-circle" aria-hidden="true"></i></a>
                                                            </dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtPanNumber" runat="server" CssClass="form-control form-control2" MaxLength="10" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <legend>Add Policy Numbers <span style="color: red">*</span></legend>
                                                    </div>


                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>Policy Number <span style="color: red">*</span></dt>
                                                            <dd>

                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control form-control2" MaxLength="30" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                                                    <span class="input-group-btn">
                                                                        <asp:Button ID="btnAddPolicy" runat="server" Text="+" CssClass="btn btn-primary" Style="padding: 1px 12px;" OnClick="btnAddPolicy_Click" />
                                                                    </span>
                                                                  
                                                                    <asp:HiddenField ID="hdnIsAtleastOnePolicyAddedInListBox" runat="server" Value="0" />
                                                                </div>
                                                                <span class="text-muted" style="font-size: 10px;color:red;">click Add + button post entering policy nos.</span>
                                                            </dd>
                                                        </dl>
                                                    </div>

                                                    <div class="col-sm-3">
                                                        <dl>
                                                            <dt>
                                                                <asp:Label ID="lblLabelPolicyNumber" runat="server" Visible="false">Policy Number</asp:Label></dt>
                                                            <dd>
                                                                <asp:ListBox ID="ListBox1" runat="server" Visible="false" Height="60px" Width="98px" CssClass="form-control form-control2"></asp:ListBox>
                                                                <asp:Button ID="btnRemoveSelected" Visible="false" runat="server" Text="-" CssClass="btn btn-danger" OnClick="btnRemoveSelected_Click" Style="padding: 1px 12px; margin-top: -118px; margin-left: 100px;" />
                                                            </dd>
                                                        </dl>
                                                    </div>




                                                    <br />
                                                    <div id="tempDiv" runat="server" visible="false">
                                                        <div class="col-sm-12">
                                                            <legend>Optional Upload</legend>
                                                        </div>

                                                        <div class="col-sm-4">
                                                            <dl>
                                                                <dt>Aadhaar Card (only .pdf, .jpeg, .png)</dt>
                                                                <dd>
                                                                    <asp:FileUpload ID="fileUploadAadhaar" runat="server" CssClass="form-control filestyle" accept=".pdf, .jpeg, .png" on />
                                                                    <asp:Label ID="lblAadhaarFileName" runat="server" Text=""></asp:Label>
                                                                </dd>
                                                            </dl>
                                                        </div>

                                                        <div class="col-sm-4">
                                                            <dl>
                                                                <dt>PAN Card / Form 60 (only .pdf, .jpeg, .png)</dt>
                                                                <dd>
                                                                    <asp:FileUpload ID="fileUploadPan" runat="server" CssClass="form-control filestyle" accept=".pdf, .jpeg, .png" />
                                                                    <asp:Label ID="lblPANFileName" runat="server" Text=""></asp:Label>
                                                                </dd>
                                                            </dl>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12">
                                                        <legend></legend>


                                                        <div class="checkbox c-checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkIAgree" runat="server" />
                                                                <span class="fa fa-check" id="spanIAgree"></span>

                                                                I hereby provide my consent in accordance with Aadhar Act, 2016, Prevention of Money Laundering Act, 2002, Prevention of Money Laundering (Maintenance of Records) Rules and the applicable norms for the following:
                                                                   <br />
                                                                <ul style="list-style-type: none;">
                                                                    <li>i.	To validate/authenticate my Aadhar number with UIDAI</li>
                                                                    <li>ii.	To collect, store, share and use the details provided above in accordance with the applicable norms
                                                                    </li>
                                                                </ul>

                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div style="text-align: right" class="col-sm-12">
                                                        <asp:Button ID="btnOTPSend" runat="server" Text="GET OTP" OnClick="btnOTPSend_Click" CssClass="btn btn-primary" />
                                                    </div>
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
                                                            <p>Please enter the One Time Password (OTP) sent on your mobile number</p>
                                                            <div class="inputBox">
                                                                <div class="block-center text-center">
                                                                    <p>
                                                                        <asp:TextBox ID="txtOtpNumber" TextMode="Password" runat="server" Style="border-radius: 7px; border: 1px solid #dde6e9; padding: 2px 7px; font-size: 13px;" Text="" MaxLength="6" AutoCompleteType="Disabled" />
                                                                    </p>
                                                                </div>
                                                                <br />
                                                                <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ValidationGroup="otp" Display="Dynamic"
                                                                    ErrorMessage="Please provide valid otp number" ClientValidationFunction="fnValidateOTPNumber" OnServerValidate="OnServerValidatecvtxtOtpNumber" />
                                                            </div>
                                                            <br />

                                                            <div class="otpButton align-center">
                                                                <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClick="onClickbtnMobileVerify" />

                                                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-warning" ClientIDMode="Static" Text="Resend OTP" OnClick="onClickbtnMobileReSend" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>


                                    <div class="col-lg-4">

                                        <div class="panel panel-default">
                                            <div class="panel-heading text-center" style="background-color: #003974; color: white; font-weight: bold;">Attention!</div>
                                            <div class="panel-body">

                                                <p>
                                                    As per Government of India and IRDAI, submission of Aadhaar number and Permanent Account Number are mandatory. Accordingly, please update the following details of the proposer: 
                                                </p>
                                                <ul style="font-size: 15px;">
                                                    <li>Keep your Aadhaar Card and PAN card handy along with the list of policies.</li>
                                                    <li>Mobile Number as registered with UIDAI is to be entered. OTP will be sent to the number provided. (In case your mobile number is not updated in Aadhaar, kindly contact nearby KGI Branch Office for Aadhaar and PAN Linking.)</li>
                                                    <li>After submitting the form, a message will be shown on the success of the registration for linkage.</li>
                                                </ul>

                                                <p>
                                                    In case of any query, request you to get in touch with us at 1800 266 4545 (8 am to 8 pm) or write to us at care@kotak.com
                                                </p>
                                            </div>
                                        </div>


                                    </div>
                                </div>

                            </div>
                        </div>


                    </section>
                    <section id="sectionThankYou" runat="server" visible="false">
                        <div class="content-wrapper" style="min-height: 500px;">
                            <div class="container container-md">

                                <div class="abs-center">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Thank You
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            We have received your request for linking AADHAAR and PAN with your policies via Request SR Number: 
                                            <strong>
                                                <asp:Label ID="lblSRNumber" runat="server" Text=""></asp:Label></strong>
                                            on 
                                            <asp:Label ID="lblCurrentDatetime" runat="server" Text=""></asp:Label>
                                            <br />
                                        </p>

                                        <hr />
                                        For any further assistance, kindly call us on 1800 266 4545 or write to us at care@kotak.com
                        <hr />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>

                    <section id="sectionError" runat="server" visible="false">
                        <div class="content-wrapper" style="min-height: 500px;">
                            <div class="container container-md">

                                <div class="abs-center">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Sorry, Error Occurred
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            Due to some technical issue, your request could not be processed, kindly try after sometime.
                            <br />
                                        </p>

                                        <hr />
                                        For any further assistance, kindly call us on 1800 266 4545 or write to us at care@kotak.com
                        <hr />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>

                    <br />

                    <!-- Page footer-->
                    <footer style="background-color: white; z-index: 113;">
                        <span style="font-size: 12px; text-align: center; padding: 10px; float: left"><b>Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                        </span>
                    </footer>
                </div>


            </ContentTemplate>
        </asp:UpdatePanel>
    </form>


    <!-- JQUERY-->
    <%-- <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.js"></script>
    <script src="css/newcssjs/js/jquery-1.7.1.min.js"></script>--%>

    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            LoadAllFuntion();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(LoadAllFuntion);

            function LoadAllFuntion() {

                var d = new Date();
                var year = d.getFullYear() - 15;
                d.setFullYear(year);

                $("#txtDOB").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    dateFormat: 'dd/mm/yy',
                    yearRange: '1920:' + year + '', defaultDate: d
                });

                $("#btnOTPSend").bind("click", function () {
                    if (Validation()) {

                        $("#btnOTPSend").val('Please Wait..');
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                $("#btnMobileVerify").bind("click", function () {
                    if (Validation()) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                $("#btnMobileReSend").bind("click", function () {
                    if (Validation()) {
                        $("#btnMobileReSend").val('Please Wait..');
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                function Validation() {
                    var txtFullName = $('#txtFullName').val();
                    var txtDOB = $('#txtDOB').val();
                    var txtEmailId = $('#txtEmailId').val();
                    var txtMobileNumber = $('#txtMobileNumber').val();
                    var txtAadhaarNumber = $('#txtAadhaarNumber').val();
                    var txtPanNumber = $('#txtPanNumber').val();
                    var hdnIsAtleastOnePolicyAddedInListBox = $('#hdnIsAtleastOnePolicyAddedInListBox').val();
                    var chkIAgree = $('#chkIAgree').is(':checked');

                    var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i

                    if (chkIAgree == false) {
                        alert('Please provide your consent by ticking the box above !');
                        $("#spanIAgree").css('border', '1px solid red');
                        return false;
                    }
                    else if (txtFullName.length <= 0) {
                        alert('Please enter full name !');
                        return false;
                    }
                    else if (txtDOB.length <= 0) {
                        alert('Please enter date of birth !');
                        return false;
                    }
                    else if (txtEmailId.length <= 0) {
                        alert('Please enter email id !');
                        return false;
                    }
                    else if (!pattern.test(txtEmailId)) {
                        alert('Please enter valid e-mail address');
                        return false;
                    }
                    else if (txtMobileNumber.length < 10 || txtMobileNumber.length > 10) {
                        alert('Please enter 10 digit mobile number !');
                        return false;
                    }
                    else if (isNaN(txtMobileNumber)) {
                        alert('Please enter valid numeric mobile number !');
                        return false;
                    }
                    else if (txtAadhaarNumber.length < 12 || txtAadhaarNumber.length > 12) {
                        alert('Please enter 12 digit aadhaar number !');
                        return false;
                    }
                    else if (isNaN(txtAadhaarNumber)) {
                        alert('Please enter valid numeric aadhaar number !');
                        return false;
                    }
                    else if (txtPanNumber.length < 10 || txtPanNumber.length > 10) {
                        alert('Please enter 10 digit alphanumeric pan number !');
                        return false;
                    }
                    else if (hdnIsAtleastOnePolicyAddedInListBox == "0") {
                        alert('Please click Add + button, post entering Policy nos. !');
                        return false;
                    }

                    else {
                        return true;
                    }
                }

                $('#txtDOB').keydown(function () {
                    return false;
                });
            }

        });
    </script>

    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
</body>
</html>
