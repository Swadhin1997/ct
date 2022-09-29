<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmEProposalReview.aspx.cs" Inherits="PrjPASS.FrmEProposalReview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>E-Proposal Review Verification</title>

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
    <link href="css/bootstrap.css" rel="stylesheet" />
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
        p{
            word-break: break-all;
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
    <script src="js/SweetAlert.min.js" type="text/javascript"></script>
    <style>
        .topnavbar-wrapper .navbar topnavbar {
        }
    </style>
</head>
<body>



    <script type="text/javascript">
        $(document).ready(function () {


            //$("#btnOTPSend").click(function () {
            //  ShowDisclaimerSweetAlert();
            //});


        });

        function openPdfModal() {
            $('#PDFView').modal('show');
        }


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
                    //$('#btnOTPSend').hide();
                    //$('#btnOTPSend').attr('disabled', true);
                    $('#btnMobileReSend').attr('disabled', true);
                },
                end: function (countdown) {
                    $(".timercountTxt").show();
                    $('#btnMobileVerify').addClass('disabled');
                    $('#btnMobileVerify').attr('disabled', 'disabled');
                    //$('#btnOTPSend').removeAttr('disabled');
                    //$('#btnOTPSend').removeClass('disabled');
                    $('#btnMobileReSend').removeAttr('disabled');
                    $('#btnMobileReSend').removeClass('disabled');
                    document.getElementById("<%=txtTimer.ClientID%>").value = 'Your One Time Password has expired';
                }
            });
        }


        function openErrorModal() {
            $('#myModalError').modal('show');
        }
        function openSuccessModal() {
            $('#myModalSuccess').modal('show');
        }
        function openModalReject() {
            $('#myModalReject').modal('show');
        }
        function otpModalshow() {
            $('#otpModal').modal('show');
        }

        function checkotp() {
            var count = $('#<%=hiddencount.ClientID %>').val();

            var otp = $('#<%=hiddenotp.ClientID %>').val();
            var otpenter = $('#<%=txtOtpNumber.ClientID %>').val();
            if (count == 4) {
                $('#<%=lblerrormsg.ClientID %>').text('You have attempted 3 attempt,please try again with new OTP!');
                $('#btnMobileVerify').addClass('disabled');
                $('#btnMobileVerify').attr('disabled', 'disabled');
                $('#btnOTPSend').removeAttr('disabled');
                $('#btnOTPSend').removeClass('disabled');
                $('#btnMobileReSend').removeAttr('disabled');
                $('#btnMobileReSend').removeClass('disabled');
                $('#<%=hiddencount.ClientID %>').val(1);

                return false;
            }
            if (otpenter != otp) {
                count++;
                $('#<%=hiddencount.ClientID %>').val(count);

                if (count == 3) {
                    $('#<%=lblerrormsg.ClientID %>').text('You have 1 attempt left ');
                    return false;
                }
                else if (count == 2) {
                    $('#<%=lblerrormsg.ClientID %>').text('You have 2 attempt left');
                    return false;
                }
                else if (count == 4) {
                    $('#<%=lblerrormsg.ClientID %>').text('You have 3 attempt,please try again after some time!');
                    return false;
                }
            }
            else {
                return true;
            }
        }

        function ShowDisclaimerSweetAlert() {
            var selectvalue = $('#pick option:selected').val();
            $('#<%=hddntype.ClientID %>').val(selectvalue);
            $('#<%=hddnrejectreason.ClientID %>').val('');
            var span = document.createElement("span");
            span.innerHTML = "<P style='font-size: 14px' ALIGN='LEFT'>Are you Sure want to " + selectvalue + " ?</p>";

            var isreturn = false;
            if (selectvalue=="Reject") {
                span.innerHTML+=" <input class='form-control' id='txtreject' placeholder='Enter Reason For Rejection' maxlenght='500'>"
            }
            swal({
                title: "Confirmation",
                content: span,
                html: true,
                buttons: {
                    cancel: true,
                    confirm: true,
                }
            }).then(okay => {
                if (okay) {
                    if (selectvalue == "Reject") {
                        if ($('#txtreject').val().trim() != "") {
                            $('#<%=hddnrejectreason.ClientID %>').val($('#txtreject').val().trim());

                            $("#btnConfirmOTP").click();
                        } else {
                            alert('Please Enter Reason For Rejection.');

                        }
                    }

                    else {
                          $("#btnConfirmOTP").click();
                    }
                }
            });

            return isreturn;

        }


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
                        <nav role="navigation" class="navbar topnavbar" style="background-color: unset!important; background-image: unset!important;">
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
                        <div class="content-wrapper col-lg-12 col-md-12">
                            <div class="container container-md">
                                <div class="row mb-lg">
                                    <div class="col-lg-12" style="text-align: center;">
                                        <div>
                                            <span class="h3 text-bold">E-Proposal Review Verification</span>
                                        </div>
                                    </div>
                                </div>
                                <br />

                                <div class="row" id="ReviewDetails" runat="server">
                                    <div class="col-md-12">
                                        <div class="panel panel-default">
                                            <div class="panel-heading text-center" style="background-color: #d20f0f; color: white; font-weight: bold;">E-Proposal Review Details</div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Customer Name</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_customername"></p>
                                                            </div>

                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Customer Address</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                
                                                                <p class="form-control-static" runat="server" id="lbl_custaddress"></p>
                                                            </div>



                                                        

                                                       
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Customer Mobile No</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_customermobileno"></p>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Customer Email ID</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_customeremailid"></p>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Product</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_product"></p>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Sum Insured Opted</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_suminsuredamt"></p>
                                                            </div>
                                                      

                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Intermediary Code</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_Imdcode"></p>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Branch Code</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_branchcode"></p>
                                                            </div>


                                                  
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Premium Amount</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_premiumamt"></p>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-control-static">Physical Proposal No</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <p class="form-control-static" runat="server" id="lbl_physicalproposalno"></p>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <br />
                                                <div class="row" id="norecord" runat="server" style="display: none;">
                                                    <div class="col-md-12 text-center">
                                                        <p class="form-control-static" style="font-size: 16px; font: bold;" runat="server" id="lbl_recorddetails"></p>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="col-md-12" style="width: 100%; height: 500px">
                                        <asp:PlaceHolder ID="iframeDiv" runat="server"></asp:PlaceHolder>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <%--           <section id="sectionError" style="min-height: 500px;" runat="server" visible="false">
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
                    </section>--%>


                    <section id="sectionRecordNotFound" style="display: none; min-height: 500px; width: 100%;" runat="server">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Sorry, nothing to display
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            Either you have already Verified the request 
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

                    <section id="SectionSuccess" style="display: none; min-height: 500px; width: 100%;" runat="server">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Thank You 
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            Your E-proposal verification for Reference no.
                                            <asp:Label ID="lblreferenceno1" runat="server" />
                                            is recorded
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
                    <section id="SectionRejected" style="display: none; min-height: 500px; width: 100%;" runat="server">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Thank You  
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            Your E-proposal Rejection for Reference no.
                                            <asp:Label ID="lblreferenceno" runat="server" />
                                            is recorded
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

                    <div class="modal fade" id="myModalSuccess" role="dialog" data-backdrop="false">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Success</h4>
                                </div>
                                <div class="modal-body">
                                    Thank you,Your Proposal is verified Successfully . For assistance call on 1800 266 4545 or email us to care@kotak.com. 
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="modal fade" id="myModalReject" role="dialog" data-backdrop="static">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Status</h4>
                                </div>
                                <div class="modal-body">
                                    Your E-Proposal is Rejected . For assistance call on 1800 266 4545 or email us to care@kotak.com. 
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>


                    <div class="modal fade" id="otpModal" role="dialog" data-backdrop="false">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title text-center">OTP Authentication</h4>
                                </div>
                                <div class="modal-body">
                                    <div>
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="otpPanel align-center text-center" id="otpPanel" runat="server">
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
                                                        Please enter the One Time Password (OTP) sent on your mobile number or Email ID
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
                                                    <p>
                                                        <asp:Label ID="lblerrormsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                        <asp:HiddenField ID="hiddencount" runat="server" Value="1" />
                                                        <asp:HiddenField ID="hiddenotp" runat="server" Value="1" />

                                                    </p>
                                                    <div class="otpButton align-center">
                                                        <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClientClick="return checkotp();" OnClick="onClickbtnMobileVerify" />
                                                        <%----%>
                                                        <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-warning" ClientIDMode="Static" Text="Resend OTP" OnClick="onClickbtnMobileReSend" /><%----%>
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
                    <br />
                    <!-- Page footer-->
                    <footer style="max-height: 800px; background-color: white; position: fixed; z-index: 113; height: 60px; bottom: 0px">
                        <div class="panel-body" id="submitpanel" runat="server">
                            <div class="row">
                                <div class="col-md-4"></div>
                               <%-- <div class="col-md-2 text-center">
                                    <span style="font-size: 20px; text-align: center; padding: 10px;">Select option
                                    </span>
                                </div>--%>
                                <div class="col-md-2 col-xs-6 text-right" >
                                    <div class="btn-group">

                                        <select role="menu" class="form-control" aria-expanded="false" id="pick">
                                            <option value="Approve" selected="selected">Approve</option>
                                            <option value="Reject">Reject</option>

                                        </select>
                                    </div>
                                </div>



                                <div class="col-md-1 col-xs-6 text-left" >
                                    <asp:HiddenField ID="hddntype" runat="server" Value="Approve" />
                                    <asp:HiddenField ID="hddnrejectreason" runat="server" Value="" />

                                    
                                    <asp:Button ID="btnOTPSend" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="SUBMIT" OnClientClick="return ShowDisclaimerSweetAlert();" /><%-- --%>
                                    <asp:Button ID="btnConfirmOTP" runat="server" Style="display: none;" CssClass="btn btn-success" ClientIDMode="Static" Text="SUBMIT" OnClick="btnsubmit_Click" /><%-- --%>
                                </div>
                            </div>
                        </div>
                        <%--<span style="font-size: 12px; text-align: center; padding: 5px; float: left"><b>Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak General Insurance Company Ltd.  CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                        </span>--%>
                    </footer>
                </div>
                </b>
            </ContentTemplate>
            <%--  <Triggers>
                <asp:PostBackTrigger ControlID="btnsubmit" />
            </Triggers>--%>
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
                    Please try again .  For assistance call on 1800 266 4545 or email us to care@kotak.com. 
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
