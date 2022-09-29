<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUpdateNominee.aspx.cs" Inherits="PrjPASS.FrmUpdateNominee" %>

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

    <style>
        body {
            background-color: #f0fcf8;
        }

        #btnOTPSend {
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

        @media only screen and (max-width: 800px) {

            #tblData {
            border :0px;
            border :none;
            }

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
    </style>
    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>
</head>
<body>
    <script src="js/SweetAlert.min.js" type="text/javascript"></script>

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
                    yearRange: '1920:' + year + '', defaultDate: d,
                    beforeShow: function () {
                        setTimeout(function () {
                            $('.ui-datepicker').css('z-index', 99999999999999);
                        }, 0);
                    },
                    onSelect: function (dateText) {
                        var currentDate = new Date();
                        var DOB = $("#txtDOB").datepicker("getDate");
                        var diffYear = diff_years(currentDate, DOB);

                        if (diffYear < 18) {
                            $('#txtAppointeename').removeAttr('disabled');
                            $('#ddlAppointeeRelationship').prop("disabled", false);
                        }
                        else {
                            $('#txtAppointeename').attr('disabled', 'disabled');
                            $('#txtAppointeename').val('');
                            $("#ddlAppointeeRelationship").val("Select").change();;
                            $('#ddlAppointeeRelationship').prop("disabled", true);
                        }
                    }

                });

                function diff_years(dt2, dt1) {
                    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
                    diff /= (60 * 60 * 24);
                    return Math.abs(Math.round(diff / 365.25));
                }


                $("#txtAppointeeDOB").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    dateFormat: 'dd/mm/yy',
                    yearRange: '1920:' + year + '', defaultDate: d,
                    beforeShow: function () {
                        setTimeout(function () {
                            $('.ui-datepicker').css('z-index', 99999999999999);
                        }, 0);
                    }
                });

                $('#txtDOB').keydown(function () {
                    return false;
                });
            }
        });


        function ShowSuccessAlert(msg) {
            swal({
                title: "Success!",
                text: msg,
                type: "success"
            }).then(okay => {
                if (okay) {
                    window.location.href = "https://www.kotakgeneralinsurance.com/";
                }
            });
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


    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
            <ContentTemplate>
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




                    <section id="sectionMain">
                        <div class="content-wrapper col-lg-6 col-md-6">
                            <div class="container container-md">
                                <div class="row mb-lg">
                                    <div class="col-lg-12" style="text-align: center;">
                                        <div>
                                            <span class="h3 text-bold">Update Nominee Details</span>
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
                                                    <div class="col-sm-3" style="vertical-align: central">
                                                        <dl>
                                                            <dt>Enter Policy Number <span style="color: red">*</span></dt>
                                                            <dd>
                                                                <asp:TextBox ID="txtPolicyNumber" runat="server" MaxLength="50" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                                            </dd>
                                                        </dl>
                                                    </div>



                                                    <div class="col-sm-3" id="btnOTPSendDIV" runat="server">
                                                        <asp:Button ID="btnOTPSend" runat="server" Text="GET OTP" CssClass="btn btn-primary" OnClick="btnOTPSend_Click" />
                                                    </div>

                                                    <div class="col-sm-3" id="PolicyHolderNameDIV" runat="server">
                                                        <dl>
                                                            <dt><b>Policy Holder Name </b></dt>
                                                            <dd>
                                                                <h4>
                                                                    <asp:Label ID="lblPolicyHolderName" runat="server" Text=""></asp:Label></h4>
                                                            </dd>
                                                        </dl>
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
                                                                    ErrorMessage="Please provide valid otp number" ClientValidationFunction="fnValidateOTPNumber" OnServerValidate="OnServerValidatecvtxtOtpNumber" />
                                                            </div>

                                                            <div class="otpButton align-center">
                                                                <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClick="onClickbtnMobileVerify" />
                                                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-warning" ClientIDMode="Static" Text="Resend OTP" OnClick="onClickbtnMobileReSend" />
                                                            </div>

                                                        </div>
                                                        <div class="row-grid" id="NoteDiv" runat="server" visible="false">
                                                            Note :
                                                                <ol type="1">
                                                                    <li>If the mobile number doesn't match or is not updated in our records,  please connect with us on our toll free number 1800 266 4545 or email us on care@kotak.com.  please ensure to mention your policy numbers in all communication.</li>
                                                                    <li>Nominee information updated will be recorded within 48 working hours of providing the request & subsequent communication will be sent to your registered communication address provided at the time of Policy issuance.</li>
                                                                </ol>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-12" runat="server" id="dvNomineeDetails" visible="false">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">Nominee Details</div>
                                                            <div class="panel-body">

                                                                <div id="no-more-tables" style="border: none">
                                                                    <table id="tblData" class="table-bordered table-striped table-condensed cf col-md-12">
                                                                        <thead class="cf">
                                                                            <tr>
                                                                                <th>Nominee Name</th>
                                                                                <th>DOB</th>
                                                                                <th>Relation</th>
                                                                                <th id="thApponteeName" runat="server">Appointee Name</th>
                                                                                <th id="thApponteeRelationship" runat="server">Appointee Relationship</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td data-title="Nominee Name">
                                                                                    <asp:TextBox ID="txtNomineeName" name="NomineeName" Text="" runat="server" CssClass="form-control" /></td>
                                                                                <td data-title="DOB">
                                                                                    <asp:TextBox ID="txtDOB" name="txtDOB" Text="" runat="server" CssClass="form-control" placeholder='dd/mm/yyyy' />
                                                                                </td>
                                                                                <td data-title="Relation">

                                                                                    <asp:DropDownList CssClass="form-control" ID="drpNomineeRelationship" runat="server">
                                                                                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="Aunt" Value="Aunt"></asp:ListItem>
                                                                                        <asp:ListItem Text="Brother" Value="Brother"></asp:ListItem>
                                                                                        <asp:ListItem Text="Brother-In-law" Value="Brother-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="CHAUFFEUR" Value="CHAUFFEUR"></asp:ListItem>
                                                                                        <asp:ListItem Text="chaffeur" Value="chaffeur"></asp:ListItem>
                                                                                        <asp:ListItem Text="Daughter" Value="Daughter"></asp:ListItem>
                                                                                        <asp:ListItem Text="Daughter-In-law" Value="Daughter-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                                                                        <asp:ListItem Text="Employer" Value="Employer"></asp:ListItem>
                                                                                        <asp:ListItem Text="FRIEND" Value="FRIEND"></asp:ListItem>
                                                                                        <asp:ListItem Text="Father" Value="Father"></asp:ListItem>
                                                                                        <asp:ListItem Text="Father-In-law" Value="Father-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Fiance" Value="Fiance"></asp:ListItem>
                                                                                        <asp:ListItem Text="Friend" Value="Friend"></asp:ListItem>
                                                                                        <asp:ListItem Text="Granddaughter" Value="Granddaughter"></asp:ListItem>
                                                                                        <asp:ListItem Text="Grandfather" Value="Grandfather"></asp:ListItem>
                                                                                        <asp:ListItem Text="GrandMother" Value="GrandMother"></asp:ListItem>
                                                                                        <asp:ListItem Text="Grandson" Value="Grandson"></asp:ListItem>
                                                                                        <asp:ListItem Text="Husband" Value="Husband"></asp:ListItem>
                                                                                        <asp:ListItem Text="INSURED (SELF-DRIVING)" Value="INSURED (SELF-DRIVING)"></asp:ListItem>
                                                                                        <asp:ListItem Text="Insured" Value="Insured"></asp:ListItem>
                                                                                        <asp:ListItem Text="Insured Estate" Value="Insured Estate"></asp:ListItem>
                                                                                        <asp:ListItem Text="Mother" Value="Mother"></asp:ListItem>
                                                                                        <asp:ListItem Text="Mother-In-law" Value="Mother-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Nephew" Value="Nephew"></asp:ListItem>
                                                                                        <asp:ListItem Text="Niece" Value="Niece"></asp:ListItem>
                                                                                        <asp:ListItem Text="OTHERS" Value="OTHERS"></asp:ListItem>
                                                                                        <asp:ListItem Text="Owner" Value="Owner"></asp:ListItem>
                                                                                        <asp:ListItem Text="Partner" Value="Partner"></asp:ListItem>
                                                                                        <asp:ListItem Text="RELATIVE" Value="RELATIVE"></asp:ListItem>
                                                                                        <asp:ListItem Text="Relatives" Value="Relatives"></asp:ListItem>
                                                                                        <asp:ListItem Text="Self" Value="Self"></asp:ListItem>
                                                                                        <asp:ListItem Text="Sister" Value="Sister"></asp:ListItem>
                                                                                        <asp:ListItem Text="Sister-In-law" Value="Sister-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Son" Value="Son"></asp:ListItem>
                                                                                        <asp:ListItem Text="Son-In-law" Value="Son-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Spouse" Value="Spouse"></asp:ListItem>
                                                                                        <asp:ListItem Text="Uncle" Value="Uncle"></asp:ListItem>
                                                                                        <asp:ListItem Text="Wife" Value="Wife"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td data-title="Appointee Name" id="tdApponteeName" runat="server">
                                                                                    <asp:TextBox ID="txtAppointeename" name="Appointeename" Text="" runat="server" CssClass="form-control" /></td>
                                                                                <td data-title="Appointee Relationship" id="tdApponteeRelationship" runat="server">
                                                                                    <asp:DropDownList CssClass="form-control" ID="ddlAppointeeRelationship" runat="server">
                                                                                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="Aunt" Value="Aunt"></asp:ListItem>
                                                                                        <asp:ListItem Text="Brother" Value="Brother"></asp:ListItem>
                                                                                        <asp:ListItem Text="Brother-In-law" Value="Brother-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="CHAUFFEUR" Value="CHAUFFEUR"></asp:ListItem>
                                                                                        <asp:ListItem Text="chaffeur" Value="chaffeur"></asp:ListItem>
                                                                                        <asp:ListItem Text="Daughter" Value="Daughter"></asp:ListItem>
                                                                                        <asp:ListItem Text="Daughter-In-law" Value="Daughter-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                                                                        <asp:ListItem Text="Employer" Value="Employer"></asp:ListItem>
                                                                                        <asp:ListItem Text="FRIEND" Value="FRIEND"></asp:ListItem>
                                                                                        <asp:ListItem Text="Father" Value="Father"></asp:ListItem>
                                                                                        <asp:ListItem Text="Father-In-law" Value="Father-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Fiance" Value="Fiance"></asp:ListItem>
                                                                                        <asp:ListItem Text="Friend" Value="Friend"></asp:ListItem>
                                                                                        <asp:ListItem Text="Granddaughter" Value="Granddaughter"></asp:ListItem>
                                                                                        <asp:ListItem Text="Grandfather" Value="Grandfather"></asp:ListItem>
                                                                                        <asp:ListItem Text="GrandMother" Value="GrandMother"></asp:ListItem>
                                                                                        <asp:ListItem Text="Grandson" Value="Grandson"></asp:ListItem>
                                                                                        <asp:ListItem Text="Husband" Value="Husband"></asp:ListItem>
                                                                                        <asp:ListItem Text="INSURED (SELF-DRIVING)" Value="INSURED (SELF-DRIVING)"></asp:ListItem>
                                                                                        <asp:ListItem Text="Insured" Value="Insured"></asp:ListItem>
                                                                                        <asp:ListItem Text="Insured Estate" Value="Insured Estate"></asp:ListItem>
                                                                                        <asp:ListItem Text="Mother" Value="Mother"></asp:ListItem>
                                                                                        <asp:ListItem Text="Mother-In-law" Value="Mother-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Nephew" Value="Nephew"></asp:ListItem>
                                                                                        <asp:ListItem Text="Niece" Value="Niece"></asp:ListItem>
                                                                                        <asp:ListItem Text="OTHERS" Value="OTHERS"></asp:ListItem>
                                                                                        <asp:ListItem Text="Owner" Value="Owner"></asp:ListItem>
                                                                                        <asp:ListItem Text="Partner" Value="Partner"></asp:ListItem>
                                                                                        <asp:ListItem Text="RELATIVE" Value="RELATIVE"></asp:ListItem>
                                                                                        <asp:ListItem Text="Relatives" Value="Relatives"></asp:ListItem>
                                                                                        <asp:ListItem Text="Self" Value="Self"></asp:ListItem>
                                                                                        <asp:ListItem Text="Sister" Value="Sister"></asp:ListItem>
                                                                                        <asp:ListItem Text="Sister-In-law" Value="Sister-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Son" Value="Son"></asp:ListItem>
                                                                                        <asp:ListItem Text="Son-In-law" Value="Son-In-law"></asp:ListItem>
                                                                                        <asp:ListItem Text="Spouse" Value="Spouse"></asp:ListItem>
                                                                                        <asp:ListItem Text="Uncle" Value="Uncle"></asp:ListItem>
                                                                                        <asp:ListItem Text="Wife" Value="Wife"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>

                                                                    <br>
                                                                    <br>
                                                                    <br>
                                                                
                                                                </div>
                                                                <br />
                                                                <div style="float: right">
                                                                    <br />
                                                                    <asp:Button ID="BtnUpdateNominee" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="Update Details" OnClick="BtnUpdateNominee_Click" />
                                                                    <asp:Button ID="BtnExit" runat="server" CssClass="btn btn-danger" ClientIDMode="Static" Text="Exit" OnClick="BtnExit_Click" />
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

                    <section id="sectionError" style="display: none; min-height: 500px;">
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


                    <section id="sectionRecordNotFound" style="display: none; min-height: 500px;">
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
                        <span style="font-size: 12px; text-align: center; padding: 5px; float: left"><b>Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak General Insurance Company Ltd.  CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                       </b> </span>
                    </footer>




                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

</body>
</html>
