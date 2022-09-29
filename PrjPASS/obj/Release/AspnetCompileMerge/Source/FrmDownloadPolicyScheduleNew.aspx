<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDownloadPolicyScheduleNew.aspx.cs" Inherits="PrjPASS.FrmDownloadPolicyScheduleNew" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>
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

        .pager {
            background-color: #c1c4d0;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .mydatagrid {
            width: 100%;
            border: solid 1px black;
            font-size: 6px;
        }

        .header {
            background-color: #c1c4d0;
            font-family: Arial;
            color: black;
            border: none 0px transparent;
            height: 20px;
            text-align: center;
            font-size: 11px;
        }

        .mydatagrid a /** FOR THE PAGING ICONS  **/ {
            background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: navy;
            text-decoration: none;
            font-weight: bold;
        }

            .mydatagrid a:hover /** FOR THE PAGING ICONS  HOVER STYLES**/ {
                color: navy;
            }

        .mydatagrid span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            color: #000;
            padding: 5px 5px 5px 5px;
        }

        .pager {
            background-color: #c1c4d0;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .mydatagrid td {
            padding: 5px;
            border: 1px solid black;
            text-align: center;
        }

        .mydatagrid th {
            padding: 3px;
            border: 1px solid black;
            text-align: center;
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
            #txtEmailforPolicy, #btnSendEmail, #lnkDownload {
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


        //    function GetSelectedTextValue(ddlSecondaryParameter) {
        //    var selectedText = ddlSecondaryParameter.options[ddlSecondaryParameter.selectedIndex].innerHTML;
        //    var selectedValue = ddlSecondaryParameter.value;
        //        alert("Selected Text: " + selectedText + " Value: " + selectedValue);
        //        if (selectedValue == 'Date Of Birth') {
        //                //show social worker stuff

        //            document.getElementById("txtNomineeDOB").style.visibility = 'visible';
        //            alert("Hide");
        //            } else {
        //                //show sponsor stuff
        //             document.getElementById('txtNomineeDOB').style.visibility = 'hidden';
        //             alert("Show");
        //            }
        //}


        //function fnValidateOTPNumber(source, args) {
        //    var txtOtpNumber = $("#txtOtpNumber").val();
        //    args.IsValid = (txtOtpNumber.length > 5);
        //}

       <%-- function runme() {

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
        }--%>


        //function openErrorModal() {
        //    $('#myModalError').modal('show');
        //}


        //function openInvalidPolicyErrorModal() {
        //    $('#InvalidPolicyModalError').modal('show');
        //}

        //function openPolicyEmailSentModel() {
        //    $('#PolicyEmailSent').modal('show');
        //}


        //function openPolicyEmailNotSentModel() {
        //    $('#PolicyEmailNotSent').modal('show');
        //}



        $(document).ready(function () {

            //$('#RadioGPSHPolicy').attr('checked', 'checked');

            $('#lnkDownload').click(function (event) {
                $('#dvLoading').fadeOut(6000);
            });

            $('[id$=ddlPrimaryParameter]').change(function () {

                $('[id$=txtPrimaryParameter]').val('');

            });

            $('[id$=ddlSecondaryParameter]').change(function () {

            $('[id$=ddlPrimaryParameter]').change(function() {  
        
                $('[id$=txtPrimaryParameter]').val(''); 
                
            }); 

             $('[id$=ddlSecondaryParameter]').change(function() {  
       
                 $('[id$=txtSecondaryParameter]').val('');  

            }); 


           

            $('#RadioGPSHPolicy').click(function () {
                if ($(this).is(':checked')) {
                    document.getElementById('txtSecondaryParameter').value = "";
                    document.getElementById('txtPrimaryParameter').value = "";

                    $("#ddlPrimaryParameter")[0].selectedIndex = 0;
                    $("#ddlSecondaryParameter")[0].selectedIndex = 0;
                    document.getElementById('txtDOB').value = "";
                    $("#ddlPrimaryParameter option[value='Policy Number']").show();
                    $("#ddlPrimaryParameter option[value='Certificate Number']").show();
                    $("#ddlPrimaryParameter option[value='CRN Number']").show();
                    $("#ddlPrimaryParameter option[value='Account Number']").show();
                    $("#ddlPrimaryParameter option[value='Loan Account Number']").show();
                    $("#ddlPrimaryParameter option[value='Group Unique Identification Number']").show();
                }

            });

            $('#RadioORPolicy').click(function () {
                if ($(this).is(':checked')) {
                    document.getElementById('txtSecondaryParameter').value = "";
                    document.getElementById('txtPrimaryParameter').value = "";
                    $("#ddlPrimaryParameter")[0].selectedIndex = 0;
                    $("#ddlSecondaryParameter")[0].selectedIndex = 0;
                    document.getElementById('txtDOB').value = "";
                    $("#ddlPrimaryParameter option[value='Policy Number']").show();
                    $("#ddlPrimaryParameter option[value='Certificate Number']").hide();
                    $("#ddlPrimaryParameter option[value='CRN Number']").hide();
                    $("#ddlPrimaryParameter option[value='Account Number']").hide();
                    $("#ddlPrimaryParameter option[value='Loan Account Number']").hide();
                    $("#ddlPrimaryParameter option[value='Group Unique Identification Number']").hide();
                }
            });

            //$("[id$=txtNomineeDOB]").datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    dateFormat: 'dd/mm/yy',
            //    minDate: '01/01/1940',
            //    yearRange: "1940:" + new Date().getFullYear().toString()
            //});

            //$("#datepickerImagenomineedob").click(function () {
            //    $("[id$=txtNomineeDOB]").datepicker("hide");
            //});

            GetSelectedTextValue(ddlSecondaryParameter);


        });


        function GetSelectedTextValue(ddlSecondaryParameter) {
            var selectedText = ddlSecondaryParameter.options[ddlSecondaryParameter.selectedIndex].innerHTML;
            var selectedValue = ddlSecondaryParameter.value;
            //alert("Selected Text: " + selectedText + " Value: " + selectedValue);
            if (selectedValue == 'Date Of Birth') {

                document.getElementById('txtSecondaryParameter').value = "";
                $('#txtSecondaryParameter').hide();
                $('#txtSecondaryParameter').attr('disabled', true);

                document.getElementById("txtDOB").style.visibility = 'visible';
                document.getElementById("datepickerImagenomineedob").style.visibility = 'visible';
                $('#divsecondaryparameter').hide();


            }
            else {
                document.getElementById('txtDOB').value = "";
                $('#divsecondaryparameter').show();
                $('#txtSecondaryParameter').show();
                $('#txtSecondaryParameter').removeAttr('disabled');
                $('#txtSecondaryParameter').removeClass('disabled');
                document.getElementById("txtDOB").style.visibility = 'hidden';
                document.getElementById("datepickerImagenomineedob").style.visibility = 'hidden';


                // alert("hide");
            }
        }

        $(function () {
            ApplyDatePicker();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
        });



        function ApplyDatePicker() {
            $("[id$=txtDOB]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/1940',
                yearRange: "1940:" + new Date().getFullYear().toString()
            });

            $("#datepickerImagenomineedob").click(function () {
                $("[id$=txtDOB]").datepicker("show");
            });

        }

    </script>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
            <ContentTemplate>--%>
        <asp:HiddenField ID="hdnProductCode" runat="server" />
        <asp:HiddenField ID="hdnDeptCode" runat="server" />
        <div class="wrapper">

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
                        <div class="row mb-lg"  style="text-align: center; color: red;" id="divtechnicalissueMessage"  runat="server"  >
                            <p >“Sorry!! Due to some technical issue we are unable to provide the required information.Please try again after sometime.</p> For assistance call on 1800 266 4545 or email us at 
                                                                <a href="mailto://care@kotak.com" > care@kotak.com</a>”
                        </div>
                        <div class="row mb-lg"  style="text-align: center; color: red;" id="DivCancelledPolicyMessage"  runat="server"  >
                            <p >“Sorry!! Policy against the provided information is In-Active hence we are unable to provide the required information. 
                                </p> For assistance call on 1800 266 4545 or email us at 
                                                                <a href="mailto://care@kotak.com" > care@kotak.com</a>”
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading text-center" style="background-color: #d20f0f; color: white; font-weight: bold;">Fill Required Details Below</div>
                                    <div class="panel-body">
                                        <%-- <div class="row">
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
                                                </div>--%>
                                        <!--Start 930-->

                                        <div class="row">


                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>Policy Details <span style="color: red">*</span></dt>

                                                </dl>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">

                                                <label class="radio-inline c-radio">
                                                    <input name="i-radio" id="RadioGPSHPolicy" type="radio" value="RadioGPSHPolicy" onclick="RadioORPolicy_SelectedIndexChanged" runat="server" />
                                                    <span class="fa fa-circle"></span>Group Personal Accident / Kotak Group Smart Cash / Group Health Insurance (Non-Employer - Employee)
                                                </label>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <label class="radio-inline c-radio">
                                                    <input name="i-radio" id="RadioORPolicy" type="radio" value="RadioORPolicy" runat="server" />
                                                    <span class="fa fa-circle"></span>Other Retails Policies</label>
                                            </div>
                                        </div>


                                        <div class="row" style="display: none">
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>Product  <span style="color: red">*</span></dt>
                                                </dl>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>
                                                        <asp:DropDownList ID="ddlProduct" runat="server" autocomplete="off" CssClass="form-control" MaxLength="40" type="tel">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0" />
                                                            <asp:ListItem Text="Arogya Sanjeevani Policy, Kotak Mahindra General Insurance Company Ltd" Value="Arogya Sanjeevani Policy, Kotak Mahindra General Insurance Company Ltd"></asp:ListItem>
                                                            <asp:ListItem Text="Contractors All Risk" Value="Contractors All Risk"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Accident Care" Value="Kotak Accident Care"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Advance Loss of Profit Insurance" Value="Kotak Advance Loss of Profit Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="KOTAK ALL RISK SECURE" Value="KOTAK ALL RISK SECURE"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Bharat Griha Raksha" Value="Kotak Bharat Griha Raksha"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Bharat Laghu Udyam Suraksha" Value="Kotak Bharat Laghu Udyam Suraksha"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Bharat Sookshma Udyam Suraksha" Value="Kotak Bharat Sookshma Udyam Suraksha"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Boiler and Pressure Plant Insurance" Value="Kotak Boiler and Pressure Plant Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Burglary Secure" Value="Kotak Burglary Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Business Secure" Value="Kotak Business Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Business Secure Bharat Laghu Udyam Suraksha" Value="Kotak Business Secure Bharat Laghu Udyam Suraksha"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Car Secure" Value="Kotak Car Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Car Secure OD Only" Value="Kotak Car Secure OD Only"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Commercial General Liability Insurance" Value="Kotak Commercial General Liability Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Commercial Vehicle" Value="Kotak Commercial Vehicle"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Commercial Vehicle Secure" Value="Kotak Commercial Vehicle Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Compulsory Personal Accident" Value="Kotak Compulsory Personal Accident"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Contractor's Plant And Machinery Insurance" Value="Kotak Contractor's Plant And Machinery Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Corporate Vehicle Secure" Value="Kotak Corporate Vehicle Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Cyber Secure" Value="Kotak Cyber Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Directors and Officers Liability" Value="Kotak Directors and Officers Liability"></asp:ListItem>
                                                            <asp:ListItem Text="KOTAK EMPLOYEES COMPENSATION INSURANCE" Value="KOTAK EMPLOYEES COMPENSATION INSURANCE"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Erection All Risk" Value="Kotak Erection All Risk"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Errors and Omissions Liability Insurance" Value="Kotak Errors and Omissions Liability Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Event Insurance" Value="Kotak Event Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Farmers Secure" Value="Kotak Farmers Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Fire Secure" Value="Kotak Fire Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Group Accident Protect" Value="Kotak Group Accident Protect"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Group Health Care Non Employer Employee" Value="Kotak Group Health Care Non Employer Employee"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Group Personal Accident Employer To Employee" Value="Kotak Group Personal Accident Employer To Employee"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Group Secure Shield" Value="Kotak Group Secure Shield"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Group Smart Health" Value="Kotak Group Smart Health"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Group Travel Insurance" Value="Kotak Group Travel Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Health Care" Value="Kotak Health Care"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Health Premier" Value="Kotak Health Premier"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Health Super Top Up" Value="Kotak Health Super Top Up"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Home Secure" Value="Kotak Home Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Housing Society Package Policy" Value="Kotak Housing Society Package Policy"></asp:ListItem>
                                                            <asp:ListItem Text="KOTAK INDIVIDUAL TRAVEL INSURANCE" Value="KOTAK INDIVIDUAL TRAVEL INSURANCE"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Jewellers Block Insurance" Value="Kotak Jewellers Block Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Long Term Two Wheeler Secure" Value="Kotak Long Term Two Wheeler Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Marine Cargo Insurance" Value="Kotak Marine Cargo Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Marine Cargo Insurance Sales Turnover Policy" Value="Kotak Marine Cargo Insurance Sales Turnover Policy"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Marine Cargo Insurance Specific Policy" Value="Kotak Marine Cargo Insurance Specific Policy"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Money Secure" Value="Kotak Money Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Motor Warranty Policy" Value="Kotak Motor Warranty Policy"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Office Secure" Value="Kotak Office Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak ProShield" Value="Kotak ProShield"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Public Liability (Act) Insurance" Value="Kotak Public Liability (Act) Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Public Liability Industrial Insurance" Value="Kotak Public Liability Industrial Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Secure Shield" Value="Kotak Secure Shield"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Shop Secure" Value="Kotak Shop Secure"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Trade Credit Insurance" Value="Kotak Trade Credit Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Two Wheeler Protect OD Only" Value="Kotak Two Wheeler Protect OD Only"></asp:ListItem>
                                                            <asp:ListItem Text="Kotak Warranty Insurance" Value="Kotak Warranty Insurance"></asp:ListItem>
                                                            <asp:ListItem Text="KOTAK WEATHER INSURANCE" Value="KOTAK WEATHER INSURANCE"></asp:ListItem>
                                                            <asp:ListItem Text="KotakConsequentialFireLoss" Value="KotakConsequentialFireLoss"></asp:ListItem>
                                                            <asp:ListItem Text="KotakElectronicEquipments" Value="KotakElectronicEquipments"></asp:ListItem>
                                                            <asp:ListItem Text="KotakFidelityGuarantee" Value="KotakFidelityGuarantee"></asp:ListItem>
                                                            <asp:ListItem Text="KotakGroupHealthCare" Value="KotakGroupHealthCare"></asp:ListItem>
                                                            <asp:ListItem Text="KotakIndustrialAllRisks" Value="KotakIndustrialAllRisks"></asp:ListItem>
                                                            <asp:ListItem Text="KotakMachineryBreakdown" Value="KotakMachineryBreakdown"></asp:ListItem>
                                                            <asp:ListItem Text="KotakMarineOpenPolicy" Value="KotakMarineOpenPolicy"></asp:ListItem>
                                                            <asp:ListItem Text="Mega Risks Insurance" Value="Mega Risks Insurance"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </dt>
                                                </dl>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>Primary Parameter <span style="color: red">*</span></dt>
                                                </dl>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>
                                                        <asp:DropDownList ID="ddlPrimaryParameter" name="ddlPrimaryParameter" runat="server" autocomplete="off" AutoPostBack="false" CssClass="form-control" MaxLength="40" type="tel">
                                                            <asp:ListItem Selected="True" Text="Select" Value="" />
                                                            <asp:ListItem Text="Policy Number" Value="Policy Number"></asp:ListItem>
                                                            <asp:ListItem Text="Certificate Number" Value="Certificate Number"></asp:ListItem>
                                                            <asp:ListItem Text="CRN Number" Value="CRN Number"></asp:ListItem>
                                                            <asp:ListItem Text="Account Number" Value="Account Number"></asp:ListItem>
                                                            <asp:ListItem Text="Loan Account Number" Value="Loan Account Number"></asp:ListItem>
                                                            <asp:ListItem Text="Group Unique Identification Number" Value="Group Unique Identification Number"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </dt>
                                                </dl>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>
                                                        <asp:TextBox ID="txtPrimaryParameter" Font-Names="" runat="server" autocomplete="off" CssClass="form-control" MaxLength="40" type="tel"></asp:TextBox>
                                                    </dt>
                                                </dl>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>Secondary Parameter <span style="color: red">*</span></dt>
                                                </dl>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>
                                                        <asp:DropDownList ID="ddlSecondaryParameter" runat="server" autocomplete="off" AutoPostBack="false" CssClass="form-control" MaxLength="40" type="tel" onchange="GetSelectedTextValue(this)">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0" />
                                                            <asp:ListItem Text="Date Of Birth" Value="Date Of Birth"></asp:ListItem>
                                                            <asp:ListItem Text="Registered Mobile Number" Value="Registered Mobile Number"></asp:ListItem>
                                                            <asp:ListItem Text="Registered Email ID" Value="Registered Email ID"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </dt>
                                                </dl>
                                            </div>
                                            <div id="divsecondaryparameter" class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>
                                                        <asp:TextBox ID="txtSecondaryParameter" runat="server" autocomplete="off" CssClass="form-control" MaxLength="40" type="tel"></asp:TextBox>
                                                    </dt>
                                                </dl>
                                            </div>
                                            <div class="col-sm-3 col-3 col-lg-3" style="vertical-align: central">
                                                <dl>
                                                    <dt>
                                                        <asp:TextBox ID="txtDOB" runat="server" ReadOnly="false" MaxLength="20" Style="visibility: hidden"></asp:TextBox>
                                                        <img src="images/calendar.png" alt="" id="datepickerImagenomineedob" style="visibility: hidden" />
                                                    </dt>
                                                </dl>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12 col-12 col-lg-12" style="align-content:center" id="Div1" runat="server">
                                                <div align="center">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search"  CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                                     <asp:Button ID="btnReset" runat="server" Text="RESET"  CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <%--<div id="d7" runat="server" style="position: absolute; top: 23%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                                <asp:Button ID="btnSearch" Width="100%" runat="server" Text="Search" OnClick="btnSearch_Click"></asp:Button>
                                                </div>--%>
                                        <div class="row">
                                            <div class="col-sm-12 col-12 col-lg-12" style="">
                                                <center>    
                                                 <div style="text-align: center">
                                                     <div class="table-responsive"> 
                                                    <asp:GridView ID="GvPolicyData" CssClass="mydatagrid" Font-Names="Arial" Font-Size="9pt" RowStyle-Font-Bold="true" runat="server" AutoGenerateColumns="false" DataKeyNames="TXT_POLICY_NO_CHAR,NUM_PRODUCT_CODE"  OnRowCommand="gvPolicyData_RowCommand"  OnPageIndexChanging="GvPolicyData_PageIndexChanging"  AllowPaging="True" PageSize="4">
                                                        <HeaderStyle CssClass="header" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" PageButtonCount="8" />
                                                        <PagerStyle CssClass="pager" />
                                                        <RowStyle Height="20px" HorizontalAlign="Center" Width="80%" />
                                            
                                                        <Columns>

                                                              <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("TXT_POLICY_NO_CHAR") %>' runat="server" data-ProductName='<%# Eval("PRODUCTNAME") %>' data-Productcode='<%# Eval("NUM_PRODUCT_CODE") %>' data-ID='<%# Eval("TXT_EMAIL") %>' data-myData='<%# Eval("TXT_CUSTOMER_NAME") %>' data-myMobile='<%# Eval("TXT_Mobile") %>' OnClick="lnkDownload_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField HeaderText="Policy Number" DataField="TXT_POLICY_NO_CHAR" >
                                                                <ItemStyle Width="200px"  />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Insured Name" DataField="TXT_CUSTOMER_NAME" ItemStyle-Width="200"  ItemStyle-Wrap="False" />
                                                            <asp:BoundField HeaderText="Policy Start Date" DataField="DAT_POLICY_EFF_FROMDATE" ItemStyle-Width="200"  ItemStyle-Wrap="False" />
                                                            <asp:BoundField HeaderText="Policy End Date" DataField="DAT_POLICY_EFF_TODATE" ItemStyle-Width="200"  ItemStyle-Wrap="False" />
                                                            <asp:BoundField HeaderText="Product Name" DataField="PRODUCTNAME" ItemStyle-Width="200"  ItemStyle-Wrap="False" />
                                                            <asp:BoundField HeaderText="Product Code" DataField="NUM_PRODUCT_CODE" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                                            <asp:BoundField HeaderText="Customer Email" DataField="TXT_EMAIL" Visible="false" />
                                                            <asp:BoundField HeaderText="Customer Mobile" DataField="TXT_MOBILE" Visible="false" />

                                                            <%--  <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_NUM_PRODUCT_CODE" runat="server" Text='<%# Eval("NUM_PRODUCT_CODE")%>'/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                          
                                                            <%--<asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkSendMail" Text="Send Mail" CommandArgument='<%# Eval("TXT_EMAIL") %>' runat="server" CommandName="SendMail"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div align="center" id="DivEmptyDataMessage" style="font-size: large; color: red; width: 1000px" runat="server"><p >“Provided information does not match our records. Please try again with your correct information as mentioned in your policy.</p> For assistance call on 1800 266 4545 or email us at 
                                                                <a href="mailto://care@kotak.com" > care@kotak.com</a>”</div>
                                                        </EmptyDataTemplate>
                                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                                         </div>
                                            </div>
                                        </center>
                                            </div>
                                        </div>
                                        </br></br></br></br></br>
                                        <div class="modal-body">
                                            <asp:Label ID="Label2" runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <!--End-->
                                    <%--<br />
                                            <div class="col-lg-12">
                                                <div id="otpPanel" runat="server" class="otpPanel align-center text-center" visible="false">
                                                    <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />
                                                    <asp:TextBox ID="txtTimer" runat="server" ClientIDMode="Static" CssClass="timercountTxt" ReadOnly="true" Style="display: none" Text=""></asp:TextBox>
                                    --%>
                                    <!-- TIMER STARTS HERE -->
                                    <%--<br />
                                                    <div class="timer">
                                    --%>     <%--<img src="Images/bg-timer.png" alt="Timer" class="timerimg" />--%>
                                    <%--       <span class="secspan">sec</span>
                                                    </div>
                                    --%>
                                    <!-- TIMER ENDS HERE -->
                                    <%-- <p>
                                                        <asp:Label ID="lblMobMessage" runat="server" Text=""></asp:Label>
                                                        <br />
                                                        Please enter the One Time Password (OTP) sent on your mobile number
                                                    </p>
                                                    <div class="inputBox">
                                                        <div class="block-center text-center">
                                                            <p>
                                                                <asp:TextBox ID="txtOtpNumber" runat="server" AutoCompleteType="Disabled" MaxLength="6" Style="border-radius: 7px; border: 1px solid #dde6e9; padding: 2px 7px; font-size: 13px;" Text="" TextMode="Password" />
                                                            </p>
                                                        </div>
                                                        <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ClientValidationFunction="fnValidateOTPNumber" Display="Dynamic" ErrorMessage="Please provide valid otp number" ValidationGroup="otp" />
                                                    </div>
                                                    <div class="otpButton align-center">
                                                        <asp:Button ID="btnMobileVerify" runat="server" ClientIDMode="Static" CssClass="btn btn-success" OnClick="onClickbtnMobileVerify" Text="Verify OTP" ValidationGroup="otp" />
                                                        <asp:Button ID="btnMobileReSend" runat="server" ClientIDMode="Static" CssClass="btn btn-warning" OnClick="onClickbtnMobileReSend" Text="Resend OTP" />
                                                    </div>
                                                </div>
                                            </div>--%>
                                    <%--   <div id="dvPolicyDetails" runat="server" class="col-lg-12" visible="false">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        Welcome
                                                            <asp:Label ID="lblName" runat="server"></asp:Label>
                                                        , Get Your Policy Details.
                                                    </div>
                                                    <div class="panel-body" style="vertical-align: central">
                                                        <div class="row">
                                                            <div class="col-3 col-sm-3 col-md3">
                                                                <asp:TextBox ID="txtEmailforPolicy" runat="server" autocomplete="off" CssClass="form-control" MaxLength="100" type="email"></asp:TextBox>
                                                            </div>
                                                            <div class="col-3 col-sm-3 col-md3" style="overflow: hidden">
                                                                <button id="btnSendEmail" runat="server" class="btn btn-labeled btn-primary" onserverclick="SendSchedulePolicyEmail" type="button">
                                                                    <span class="btn-label"><i class="fa fa-envelope"></i></span>E-Mail Policy Schedule
                                                                </button>
                                                            </div>
                                                            <div class="col-3 col-sm-3 col-md3" style="float: left">
                                                                <button id="btnGetPolicy" runat="server" class="btn btn-labeled btn-primary" onserverclick="DownloadSchedulePolicy" type="button">
                                                                    <span class="btn-label"><i class="fa fa-download"></i></span>Download Policy Schedule
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>


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
        <%--</div>--%>
                </b>
         <%--   </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGetPolicy" />
            </Triggers>
        </asp:UpdatePanel>--%>
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
                    <button type="button" class="close" data-dismiss="modal">
                    &times;            Details keyed-in do not match with our records available in the policy. Please try again with your correct policy number and registered contact details as mentioned in policy.  For assistance call on 1800 266 4545 or email us to care@kotak.com. 
                
                      ody">
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
