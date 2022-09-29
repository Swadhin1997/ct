<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmKGHC.aspx.cs" Inherits="PrjPASS.FrmKGHC" MaintainScrollPositionOnPostback="true" %>


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


<body style="background-color: white;">
    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>

    <style>
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
            background: #f0fcf8;
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

        .txtclass {
            /*font-family: Tahoma,Arial, Helvetica, sans-serif;
            font-size: 16px;
            color: #000000;
            font-weight: normal;
            text-decoration: none;
            border: 1px solid #888888;
            border-radius: 4px;*/
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

        .BRMinfoStyle {
            position: relative;
            top: 20px;
        }

        .InnerTextBox {
            position: relative;
            top: -4px;
        }
    </style>

    <form id="form1" runat="server">

        <asp:HiddenField ID="hdnMobileNumber" runat="server" Value="" />
        <asp:HiddenField ID="hdnIsKLTEmployee" runat="server" Value="0" />
        <div id="divRedirectingInfo" style="text-align: center; font-size: 25px; margin-top: 30px">
            Please wait, Processing your request...
        </div>


        <div id="BRMCodeModel" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">IMD Code.</h4>
                    </div>
                    <div class="modal-body">
                        <p>To be left as 0000000000 if buying directly. If buying through KGI BRM, please input BRM INTERMEDIARY CODE</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>



        <div class="wrapper" id="divWrapper">
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
                                <%--<a href="https://kgipass.kotakmahindrageneralinsurance.co.in/KGIPASS/downloads/STP_Brochure.pdf" target="_blank"><em class="fa fa-download"></em>&nbsp;Download Brochure</a>--%>
                                <a href="https://kgipass.kotakgeneralinsurance.com/KGIPASS/downloads/KGHC- Customer Handbook - Online -  18112020.pdf" target="_blank"><em class="fa fa-download"></em>&nbsp;Download Brochure</a>
                                <%--<a href="http://localhost:3638/downloads/KGHC- Customer Handbook - Online -  18112020.pdf" target="_blank"><em class="fa fa-download"></em>&nbsp;Download Brochure</a>--%>
                            </li>

                            <li>
                                <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em>&nbsp;care@kotak.com</a>
                            </li>

                            <li>
                                <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em>&nbsp;1800 266 4545</a>
                            </li>

                        </ul>
                        <ul class="nav-heading" style="text-align: center;">
                            <li>
                                <h2>Kotak Group Health Care</h2>
                            </li>

                            <li style="text-align: right;">To arrange a call back<a class="link-red" href="https://www.kotakgeneralinsurance.com/contact-us/get-in-touch" style="text-decoration:underline;">&nbsp;Click here</a>
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
                                    <span class="h3 text-bold">Welcome</span>
                                    <asp:Label ID="lblEmployeeName" runat="server" Text="" CssClass="h3 text-bold"></asp:Label>
                                    <span class="h3 text-bold">(</span><asp:Label ID="lblEmployeeCode" runat="server" Text="" CssClass="h3 text-bold"></asp:Label><span class="h3 text-bold">)</span>

                                    Email: <b>
                                        <asp:Label ID="lblEmpEmailId" runat="server" Text=""></asp:Label></b> | Mobile: <b>
                                            <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label></b>
                                </div>

                                <%--<p class="text-muted">Please have one final look at the details you have provided us with. This information will be printed on your policy, so it's important to ensure that everything is accurate.</p>--%>
                                <%--<p class="text-bold" id="P1" runat="server">
                                    Get a Kotak Health Super Top Up Prime Policy for as low as Rs.
                                    <asp:Label ID="lblMinMontlyAmountPayable" runat="server" Text="0.00" Style="font-size: 20px"></asp:Label>
                                    per month!
                                </p>--%>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12">
                                <!-- START panel-->
                                <div id="panelDemo14" class="panel panel-default" style="border-top-width: 2px;">
                                    <div class="panel-heading text-center" style="background-color: #ff902b; color: white; font-weight: bold;">Verify &amp; Fill Required Details Below</div>
                                    <div class="panel-body">
                                        <div role="tabpanel">
                                            <!-- Nav tabs-->
                                            <ul role="tablist" class="nav nav-tabs">
                                                <asp:Literal ID="LiteralTabNavigation" runat="server" Text=""></asp:Literal>
                                            </ul>
                                            <!-- Tab panes-->
                                            <div class="tab-content" id="divTabContent1">
                                                
                                                <asp:Literal ID="LiteralTabContent" runat="server" Text=""></asp:Literal>
                                                   
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- END panel-->
                            </div>
                        </div>
                        <div class="block-center">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="panel panel-primary">
                                        <div class="panel-heading bb" style="text-align: center; background-color: #122858fc">
                                            <h4 class="panel-title" style="font-weight: normal;">
                                                <%-- <asp:Label ID="lblDuductible1" runat="server" Text="0.00"></asp:Label>
                                                lacs | --%>
                                                <strong>Sum Insured
                                                <asp:Label ID="lblSumInsured1" runat="server" Text="4 Lakhs"></asp:Label>
                                                    (Policy Period - 6 Months )
                                                </strong>|
                                                <%--<asp:Label ID="lblFamilyFloater1" runat="server" Text=""></asp:Label>--%>
                                            </h4>
                                        </div>
                                        <table class="table">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPremiumType1" runat="server" Text=""></asp:Label></td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <asp:Label ID="lblPremiumAmount1" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>GST @ 18%</td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <asp:Label ID="lblGST1" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTotalAmountPayable" runat="server" Text="TotalAmountPayable"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <%--<asp:Label ID="lblMonthlyAmountPayable1" runat="server" Text="0.00"></asp:Label>--%>
                                                            <asp:Label ID="lblTotalAmountPayable1" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div style="text-align: center; margin-bottom: 10px;">
                                            <div class="panel-body" style="padding: 5px;">
                                                <label class="radio-inline c-radio">
                                                    <input id="radioPlan1" type="radio" name="radioPlanType" value="option1" checked="checked" />
                                                    <span class="fa fa-check"></span>Select this Premium</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="panel panel-info">
                                        <div class="panel-heading bb" style="text-align: center; background-color: #d20f0f">
                                            <h4 class="panel-title" style="font-weight: normal;">
                                                <%-- <asp:Label ID="lblDuductible2" runat="server" Text="0.00"></asp:Label>
                                                lacs | --%>
                                                <strong>Sum Insured
                                                <asp:Label ID="lblSumInsured2" runat="server" Text="5 Lakhs"></asp:Label>
                                                    (Policy Period - 6 Months )
                                                </strong>|
                                                <%--<asp:Label ID="lblFamilyFloater2" runat="server" Text=""></asp:Label>--%>
                                            </h4>
                                        </div>
                                        <table class="table">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblPremium2" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <asp:Label ID="lblNetPremium2Amount" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>GST @ 18%</td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <asp:Label ID="lblGST2" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTotalAmountPayableType2" runat="server" Text="TotalAmountPayable"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div class="text-right text-bold">
                                                            &#x20b9;
                                                        <%--<asp:Label ID="lblMonthlyAmountPayable2" runat="server" Text="0.00"></asp:Label>--%>
                                                            <asp:Label ID="lblTotalAmountPayable2" runat="server" Text="0.00"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div style="text-align: center; margin-bottom: 10px;">
                                            <div class="panel-body" style="padding: 5px;">
                                                <label class="radio-inline c-radio">
                                                    <input id="radioPlan2" type="radio" name="radioPlanType" value="option1" />
                                                    <span class="fa fa-check"></span>Select this Premium</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mv-lg pv-lg text-dark text-center">
                    <%--<a class="link" data-toggle="modal" data-target="#ModalKnowMoreAboutSanbox" href="local" style="font-size: 20px">Know more about the Product and Sandbox Program</a>--%>
                    <%--<a class="link" href="http://localhost:3638/downloads/Product and Sandbox Program.pdf" target="_blank" style="font-size: 20px">Know more about the Product and Sandbox Program</a>--%>
                    <a class="link" href="https://kgipass.kotakgeneralinsurance.com/KGIPASS/downloads/Product and Sandbox Program.pdf" target="_blank" style="font-size: 20px">Know more about the Product and Sandbox Program</a>
                    


                </div>


                <h3 class="mv-lg pv-lg text-dark text-center">
                    <asp:Label ID="lblSelectedPremiumMessage" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblSelectedPremium" runat="server" Text="0.00"></asp:Label>
                    <%-- <br />
                    <span id="BRMinfo" class="BRMinfoStyle">
                        <button type="button" class="mb-sm btn-xs btn btn-primary btn-outline" data-toggle="modal" data-target="#BRMCodeModel">
                            IMD Code</button>
                        <span class="InnerTextBox">--%>
                    <%--<asp:TextBox ID="txtEcode" Text="GI" runat="server" Width="30px" ReadOnly CssClass="txtclass" />--%>
                    <%--<asp:TextBox ID="txtBRMCode" name="IMD Code" Text="0000000000" runat="server" MaxLength="10" Width="110px" CssClass="txtclass" />
                        </span>
                        <br />
                    </span>
                    <br />--%>
                </h3>


                <center>
                
              <%--  <div class="checkbox c-checkbox needsclick" id="agreewithbtn">
                    
                    <label>
                        <input type="checkbox" value="" class="needsclick" id="chkSummaryAgree" runat="server" />
                        <span class="fa fa-check"></span>  
                    </label>
              
                </div>--%>
                    <div class="form-group" id="divDeclarofGoodHealth">
                           <div class="">
                              <label style="padding-left:0px;text-decoration:underline;"><a class="link-red" data-toggle="modal" data-target="#myModal" href="#">Declaration Of Good Health</a></label>
                                <label class="radio-inline c-radio">
                                 <input id="inlineradioYes" type="radio" name="radioDclrofgdhlth" value="option1" />
                                 <span class="fa fa-circle"></span>Yes</label>
                              <label class="radio-inline c-radio">
                                 <input id="inlineradioNo" type="radio" name="radioDclrofgdhlth" value="option2" />
                                 <span class="fa fa-circle"></span>No</label>
                           </div>
                       <%--<asp:Button Text="Export" OnClick="ExportExcel" runat="server" />--%>
                         <div class="">
                              <label style="padding-left:0px;text-decoration:underline;"><a class="link-red" data-toggle="modal" data-target="#myModal2" href="#">I agree to the terms and conditions</a></label>
                             <%--<label style="padding-left:0px;text-decoration:underline;"><a class="link-red" data-toggle="modal" data-target="#" href="">I agree to the terms and conditions</a></label>--%>
                             <%--<label>I agree to the <a class="link-red callPopup" data-popupname="termsCondition2" target="_self" href="javascript:;">Terms &amp; Conditions</a></label> --%>
                                <label class="radio-inline c-radio">
                                 <input id="iAgreeCondition" type="checkbox" name="ckhTermsAndCondition" value="Yes" />
                                 <span class="fa fa-circle"></span></label>
                              
                           </div>
                        </div>
                    					

                    <span id="lblMessageifNoSelected" style="color:red;display:none;font-weight:bold;font-size:15px;">Thank you for your interest. Please <a class="link-red" href="https://www.kotakgeneralinsurance.com/contact-us/get-in-touch" style="text-decoration:underline;" >Click here</a> to buy offline.</span>
                <div class="popUp termsCondition2">
            <a class="close-icon" href="javascript:;"></a>
            <div class="popUpcontent">
                <div class="container">
                    <div class="row-grid">
                        <strong>Terms &amp; Conditions</strong><br />
                        <ul>
                            <li>Parents/Parent-in-laws not covered in the policy.</li>
                            <li>The above premium is calculated for self, spouse/children (wherever applicable).</li>
                            <li>The above yearly premium includes a 5% group employee discount and 2.5% online discount.</li>
                            <li id="li4" runat="server">The above yearly premium includes a 10% loading for monthly installment facility. </li>
                            <li id="li5" runat="server">In Case of an employee leaving the organization he/she needs to keep the salary account funded to continue the policy benefits or change to a yearly mode of payment.</li>
                            <li>Any pre-existing diseases are not covered for 2 years under this plan</li>
                        </ul>
                        <br />
                        <strong>Self-Declaration</strong><br />
                        <p>
                           I hereby declare, on my behalf and on behalf of all persons proposed to be insured, that the above statements, answers and/or particulars given by me are true and complete in all respects to the best of my knowledge and that I am authorized to propose on behalf of these other persons.
                        </p>
                        <p>
                            I understand that the information provided by me will form the basis of the insurance policy, is subject to the Board approved underwriting policy of the insurer and that the policy will come into force only after full payment of the premium chargeable.
                        </p>
                        <p>
                            I further declare that I will notify in writing any change occurring in the occupation or general health of the life to be insured/proposer after the proposal has been submitted but before communication of the risk acceptance by the company.
                        </p>
                        <p>
                            I declare that I consent to the company seeking medical information from any doctor or hospital who/which at any time has attended on the person to be insured/proposer or from any past or present employer concerning anything which affects the physical or mental health of the person to be insured/proposer and seeking information from any insurer to whom an application for insurance on the person to be insured /proposer has been made for the purpose of underwriting the proposal and/or claim settlement.
                        </p>
                        <p>
                            I authorize the company to share information pertaining to my proposal including the medical records of the insured/proposer for the sole purpose of underwriting the proposal and/or claims settlement and with any Governmental and/or Regulatory authority.
                        </p>
                        <p>
                            The quote displayed has been calculated based on the information provided by you and is subject to change in case you change the information while buying the policy.
                        </p>
                        <p>
                             I/We agree that this application and declaration shall be promissory and shall be the basis of the contract between me/us and Kotak Mahindra General Insurance Company Limited.
                        </p>
                        <p>
                             I/We confirm that I/We have read and understood the coverages, the terms and conditions and agree to accept the company's policy of insurance along with the said conditions prescribed by the company.
                        </p>
                        <p>
                             I/We also declare and undertake that if there is any change in information as submitted by me/us after this submission then the same would be conveyed to Kotak Mahindra General Insurance Company Limited immediately failing which it is agreed and understood by me/us that the benefits under the policy would stand forfeited.
                        </p>
                        <p>
                             I/We agree to the company taking appropriate measures to capture the voice log for all such telephonic transactions carried out by me/us as required by the procedures/regulations internal or external to the company and shall not hold the company responsible or liable for relying / using such recorded telephonic conversation.
                        </p>
                        <p>
                             I/We agree that the insurance would be effective only on acceptance of this application by the company and the payment of the requisite premium by me/us in advance. In the event of non-realization of the cheque or non-receipt of the amount of premium by the company the policy shall be deemed cancelled ab-initio and the company shall not be responsible for any liabilities of whatsoever nature under this policy.
                        </p>
                    </div>
                </div>
            </div>
        </div>

                    <div class="popUp declarationofgoodhealth">
            <a class="close-icon" href="javascript:;"></a>
            <div class="popUpcontent">
                <div class="container">
                    <div class="row-grid">
                        <p>
                           I declare that the applicant(s) proposed to be insured 
                        </p>
                        <ul>
                            <li>Are not currently suffering or have previously suffered from or have symptoms of any illness/ medical condition/ disability/ physical deformity <br />and/or</li>
                            
                            <li>Have not consulted or received treatment from any doctor/ healthcare provider / undergone any hospitalization or are taking continuous medication for same</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

                                   


                <%--<button type="button" id="btnDebitMySalary" class="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px">Debit My Salary</button>--%>
                <asp:Button ID="btnMakePayment" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px;position: relative;top: 7px"  ClientIDMode="Static" Text="Confirm & Pay" />
                 <div class="otpPanel align-center text-center" id="otpPanel" style="display:none;" runat="server">
                                        <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />
                                        <asp:HiddenField ID="hdnOTPValue" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnIsEmployeeRecordPresent" runat="server" Value="0" />
                   
                     <asp:TextBox ID="txtTimer" ClientIDMode="Static" Style="display: none" ReadOnly="true" runat="server" Text="0" CssClass="timercountTxt"></asp:TextBox>                        
                                        <!-- TIMER STARTS HERE -->
                    <br />
                                            <div class="timer" id="divTimer">
                                                <span class="secspan">sec</span>
                                            </div>    
                                            <!-- TIMER ENDS HERE -->
                                            <p>Please Enter One Time Password (OTP) Sent On Your Mobile Number</p>
                                            <div class="block-center text-center">
                                                <p>
                                                <asp:TextBox ID="txtOtpNumber" runat="server"  style="border-radius: 7px;border: 1px solid #dde6e9;padding:2px 7px;font-size: 13px;" Text="0" MaxLength="6" />
                                                    
                                                    </p>
                                            </div><br />
                    
                                            <div class="otpButton align-center">
                                               <%-- <button type="button" id="btnMobileVerify" class="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  onclick="">Verify OTP</button>--%>
                                                <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClientClick="return OtpValidate();" OnClick="btnMobileVerify_Click" />
                                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Resend OTP" />   
                                            </div>
                                        </div>
                
                
            </center>



            </section>

            <section id="sectionThankYou" style="display: none; min-height: 500px;">
                <div class="content-wrapper">
                    <div class="container container-md">

                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Thank You
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    We have received your consent to debit your salary account <b>
                                        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>.</b> You will receive a confirmation mail with details shortly on your official email ID.
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
            <br />
            <!-- Page footer-->
            <footer style="background-color: white; z-index: 113;">
                <span style="font-size: 12px; text-align: center; padding: 10px; float: left"><b>Kotak Group Health Care UIN: KOTHLGP21221V022021. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
                    Sandbox Application number – 165, “Wearable device to new customers”
                    Note: Please note this proposal is implemented as an experiment for a limited period. Any further implementation of the proposal is subject to the approval of the Authority.
                </b></span>
            </footer>




        </div>

    </form>
    <div id="myModal" class="modal fade" role="dialog" style="z-index: 9999">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Declaration of Good Health</h4>
                </div>
                <div class="modal-body">

                    <p>
                        I declare that the applicant(s) proposed to be insured 
                    </p>
                    <ul>
                        <li>Are not currently suffering or have previously suffered from or have symptoms of any illness/ medical condition/ disability/ physical deformity
                            <br />
                            and/or</li>

                        <li>Have not consulted or received treatment from any doctor/ healthcare provider / undergone any hospitalization or are taking continuous medication for same</li>
                    </ul>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <div id="myModal2" class="modal fade" role="dialog" style="z-index: 9999">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Terms and Conditions/ Declarations:</h4>
                </div>
                <div class="modal-body">

                  

<p><strong >Health Declaration:</strong></p>
                    <ul>
                        <li>
                            I hereby declare, on my behalf and on behalf of all persons proposed to be insured, that the above 
                            statements,answers and/or particulars given by me are true and complete in all
                             respects to the best of my knowledge and that I am authorized to propose on behalf of these other persons.
                        </li>
                        <li>I understand that the information provided by me will form the basis of the insurance policy, is subject to the Board approved underwriting 
                            policy of the insurer and that the policy will come into force only after full payment of the premium chargeable.
                        </li>
                        <li>
                            I further declare that I will notify in writing any change occurring in the occupation or general health of the life to be insured/proposer 
                            after the proposal has been submitted but before communication of the risk acceptance by the company.
                        </li>
                        <li>
                            I declare that I consent to the company seeking medical information from any doctor or hospital who/which at any time has attended on 
                            the person to be insured/proposer or from any past or present employer concerning anything which affects the physical or mental health of 
                            the person to be insured/proposer and seeking information from any insurer to whom an application for insurance on the person to 
                            be insured /proposer has been made for the purpose of underwriting the proposal and/or claim settlement.
                        </li>
                        <li>
                            I authorize the company to share information pertaining to my proposal including the medical records of the insured/proposer for the sole 
                            purpose of underwriting the proposal and/or claims settlement and with any Governmental and/or Regulatory authority.”
                        </li><br /></ul>
                    <p><strong>Customer consent for Sandbox:</strong></p>
                    <ul>
                        
                        <li>
                            I, ____________________________, S/D/W of _______________________ acknowledge that I have fully gone through the Benefit Illustration of the 
                            proposal. I acknowledge that I understood how the proposal will work, what the coverages are provided to the participating policyholders, 
                            what are the information to be shared with the insurers and what are the benefits available to the participating policyholders. 
                        </li>
                        <li>
                            I understood that the proposal is introduced as an experiment to test a new idea under the proposal of Regulatory Sandbox.
                             I understood that the outcome of the experiment is unknown. I acknowledge that my participation in this experiment/proposal is entirely 
                            voluntary and conscious choice.
                        </li>
                        <li>
                            I understood that once the experiment is complete or on earlier termination or withdrawal thereof,
                             the insurer may discontinue the product; however, insurer will ensure the fulfillment of any existing obligations to policyholders.
                             The continuance of the product beyond the testing period will be subject to IRDAI approval. 
                        </li>
                        <li>
                            I hereby give my informed consent and willingness to participate in the experiment after reading the terms and conditions of the proposal.
                        </li><br/>
                        </ul>
                    <p><strong>Consent for KYC:</strong></p>
                    <ul>
                        <li>
                            I give my consent to the Insurer to collect my KYC documents as available with the Group Administrator, Kotak Mahindra Bank Limited.
                        </li>
                        <br />
                        <p><strong>Note: </strong>For the purpose of enrolment under the Sandbox Program, all customers are required to provide KYC details as 
                            per the Sandbox Regulations.</p>
                       </ul>
                    <p><strong>Terms and Conditions:</strong></p>
                    <ul>
                         <li>
	In case of any claim made under the Policy, no premiums shall be refunded on cancellation of Insurance. 
                             </li>
                        <li>
	The insurance coverage shall commence from the date of receipt of premium by Kotak Mahindra General Insurance Company Limited

                        </li>
                        <li>
	The policy shall be issued basis the details provided by you and the underwriting guidelines of the Company. 
                            In case of any non-disclosure / mis-representation, the policy is liable to be cancelled.
                        </li>
                        <li>
                            	Kindly note that the proposal is implemented as an experiment for a limited period in accordance with 
                            the IRDAI (Regulatory Sandbox) Regulations, 2019. Any further implementation of the proposal (Wearable Device to New Customers) 
                            beyond the Proposal/ Sandbox period is subject to approval of the Authority.
                        </li>

                    </ul>


                    
                    <%--<ul>
                        <li>Are not currently suffering or have previously suffered from or have symptoms of any illness/ medical condition/ disability/ physical deformity
                            <br />
                            and/or</li>

                        <li>Have not consulted or received treatment from any doctor/ healthcare provider / undergone any hospitalization or are taking continuous medication for same</li>
                    </ul>--%>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script type="text/javascript">

        function SubmitEmployeePrimaryData() {
            var isSuccess = false;

            var txtOtpNumber = $("#txtOtpNumber").val();
            var lblEmployeeCode = $("#lblEmployeeCode").text();

            var MembersDetails = {};
            var objEmployeePrimaryDetails = {};

            var dataMembersDetails = [];

            var SelectedPlanName = '500000';
            var SelectedSixMonthPlan;
            var IsPlan1Checked = $('#radioPlan1').is(':checked');
            if (IsPlan1Checked) {
                SelectedPlanName = '400000';
                SelectedSixMonthPlan = $('#lblTotalAmountPayable1').text();
            }
            else {
                SelectedSixMonthPlan = $('#lblTotalAmountPayable2').text();
            }

            objEmployeePrimaryDetails = {
                SelectedPlan: SelectedPlanName,
                SelectedSixMonthPlan: SelectedSixMonthPlan,
                VerifiedOTP: txtOtpNumber,
                EmployeeAddressLine1: $("#txtSelfAddressLine1").val(),
                EmployeeAddressLine2: $("#txtSelfAddressLine2").val(),
                EmployeeAddressLine3: $("#txtSelfAddressLine3").val(),
                Pincode: $("#txtSelfPincode").val(),
                EmpMobileNumber:$("#txtSelfmobileNo").val(),
                EmployeeCode: lblEmployeeCode,
                EmployeeName: $("#lblEmployeeName").text(),
                EmployeeEmailId: $("#lblEmpEmailId").text(),
                SelectedPremium: $("#lblSelectedPremium").text(),
                Height: $("#txtSelfHeight").val(),
                Weight: $("#txtSelfWeight").val(),
                NomineeName: $("#txtSelfNomineeName").val(),
                NomineeDOB: $("#txtSelfNomineeDOB").val(),
                NomineeRelationWithProposer: $('#cboNomineeRelationWithProposerSelf :selected').text()
            };
            debugger;
            $.ajax({
                type: "POST",
                url: "FrmKGHC.aspx/SaveEmployeePrimaryDetails",
                data: '{objEmployeePrimaryDetails: ' + JSON.stringify(objEmployeePrimaryDetails) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async : false,
                success: function (response) {
                    if (response.d == "success") {
                        debugger;
                        //$("#sectionThankYou").show();
                        //$("#sectionError").hide();
                        //$("#sectionMain").hide();
                        //$('#divTimer').hide();

                        //$('#otpPanel').hide();
                        isSuccess = true;
                    }
                    else {
                        //$("#sectionThankYou").hide();
                        //$("#sectionError").show();
                        //$("#sectionMain").hide();
                    }
                },
                error: function (response) {
                    
                    alert(JSON.stringify(response));
                }
            });
           
            return isSuccess;
        }

        function OtpValidate() {
            var isSuccess = false;
            var txtOtpNumber = $("#txtOtpNumber").val();
            var lblEmployeeCode = $("#lblEmployeeCode").text();
            var dataOTP = { OTPNumber: txtOtpNumber, EmployeeCode: lblEmployeeCode };

            $.ajax({
                type: "POST",
                url: "FrmKGHC.aspx/ValidateOTP1",
                data: JSON.stringify(dataOTP),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {

                    if (response.d == "success") {
                        SubmitEmployeePrimaryData();
                        isSuccess = true;
                    }
                    else {
                        alert('OTP Does Not Match, Please Enter OTP Sent On Your Mobile Number');
                        return false;
                    }
                },
                error: function (requestObject, error, errorThrown) {
                    alert(error);
                }
            });
            return isSuccess;
        }

        $(document).ready(function () {

            $(function () {

                $('#txtSelfmobileNo').keypress(function (event) {
                    var keycode = event.which;
                    if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
                        event.preventDefault();
                    }
                });

                $('#txtSelfPincode').keypress(function (event) {
                    var keycode = event.which;
                    if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
                        event.preventDefault();
                    }
                });

                $('#txtOtpNumber').keypress(function (event) {
                    var keycode = event.which;
                    if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
                        event.preventDefault();
                    }
                });

                var crndt = new Date();
                var year = crndt.getFullYear() - 1;
                crndt.setFullYear(year);

                $("input[type='text']").each(function () {
                    var element = $(this);
                    var id = $(this).attr('id');
                    var name = $(this).attr('name');

                    if (id.indexOf('NomineeDOB') >= 0) {

                        $(document).on('focus', "#" + id, function () {
                            $(this).datepicker({
                                changeMonth: true,
                                changeYear: true,
                                showButtonPanel: true,
                                dateFormat: 'dd/mm/yy',
                                yearRange: '1920:' + year + '', defaultDate: crndt
                            });
                        });
                    }

                });

                $('#btnMakePayment').click(function () {

                    var IsAgree = $('#inlineradioYes').is(':checked');
                    var IAgreeCondition = $('#iAgreeCondition').is(':checked');

                    var phoneno = /^\d{10}$/;
                    var isValid = true;

                    //if ($('#txtBRMCode').val().length == 0 || $('#txtBRMCode').val() == "" || $('#txtBRMCode').val().length <10) {
                    //    isValid = false;
                    //    alert('To be left as 0000000000 if buying directly. If buying through KGI BRM, please input BRM INTERMEDIARY CODE.');
                    //    $("#txtBRMCode").focus();
                    //    return false;
                    //}



                    $("input[type='text']").each(function () {
                        var element = $(this);
                        var id = $(this).attr('id');
                        var name = $(this).attr('name');

                        if (element.val() == "") {
                            isValid = false;
                            alert('Please Enter ' + name);
                            return false;
                        }
                        else if (name.indexOf('ight') >= 0) {
                            var isNotANumber = isNaN(element.val());
                            if (isNotANumber == true) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                            else if (parseInt(element.val()) <= 0) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                        }
                    });

                    if (isValid == true) {

                        $('#divTabContent1').children('.tab-pane').each(function () {
                            var innerDivId = $(this).attr('id');
                            var innerDivName = $(this).attr('name');
                            var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                            $("#" + innerDivId).find("select.form-control").each(function () {
                                var element = $(this);
                                var id = $(this).attr('id');
                                var name = $(this).attr('name');
                                //if (id.indexOf('#cboNomineeRelationWithProposerSelf') >= 0) {
                                //    if (element.val() === "Select") {
                                //        isValid = false;
                                //        alert("Please Select Nominee Relationship with Proposer for " + innerDivName);
                                //        return false;
                                //    }
                                //}
                            });

                            var cboNomineeRelation = $('#cboNomineeRelationWithProposerSelf').val();
                            if (cboNomineeRelation == "Select") {
                                isValid = false;
                                alert("Please Select Nominee Relationship with Proposer");
                                return false;
                            }

                            if (txtSelfmobileNo.value.match(phoneno)) {
                                isValid = true
                            }

                            else {
                                isValid = false;
                                alert("Please enter 10 digit mobile number.");
                                return false;
                            }

                            if (isValid == false) {
                                return false;
                            }
                        });
                    }

                    if (IsAgree == false && isValid == true) {

                        isValid = false;
                        alert('Please select appropriate input by understanding the declaration of good health');
                        return false;
                    }

                    if (IAgreeCondition == false && isValid == true) {

                        isValid = false;
                        alert('Please accept the terms and condition');
                        return false;
                    }

                    if (isValid == true) {
                        ShowOTP();
                    }

                    //return isValid;
                    return false;
                });

                $('#btnMobileReSend').click(function () {

                    var IsAgree = $('#inlineradioYes').is(':checked');
                    var IAgreeCondition = $('#iAgreeCondition').is(':checked');

                    var phoneno = /^\d{10}$/;
                    var isValid = true;

                    $("input[type='text']").each(function () {
                        var element = $(this);
                        var id = $(this).attr('id');
                        var name = $(this).attr('name');

                        if (id == "txtOtpNumber") {
                            //do nothing if txtOtpNumber is blank
                        }
                        else if (element.val() == "") {
                            isValid = false;
                            alert('Please Enter ' + name);
                            return false;
                        }
                        else if (name.indexOf('ight') >= 0) {
                            var isNotANumber = isNaN(element.val());
                            if (isNotANumber == true) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                            else if (parseInt(element.val()) <= 0) {
                                isValid = false;
                                alert('Please Enter Valid ' + name);
                                return false;
                            }
                        }
                    });

                    if (isValid == true) {

                        $('#divTabContent1').children('.tab-pane').each(function () {
                            var innerDivId = $(this).attr('id');
                            var innerDivName = $(this).attr('name');
                            var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                            $("#" + innerDivId).find("select.form-control").each(function () {
                                var element = $(this);
                                var id = $(this).attr('id');
                                var name = $(this).attr('name');
                                if (id.indexOf('cboNomineeRelationWithProposerSelf') >= 0) {
                                    if (element.val() === "Select") {
                                        isValid = false;
                                        alert("Please Select Nominee Relationship with Proposer for " + innerDivName);
                                        return false;
                                    }
                                }
                            });

                            var cboNomineeRelation = $('#cboNomineeRelationWithProposerSelf').val();
                            if (cboNomineeRelation == "Select") {
                                isValid = false;
                                alert("Please Select Nominee Relationship with Proposer");
                                return false;
                            }

                            if (isValid == false) {
                                return false;
                            }
                        });
                    }

                    if (IsAgree == false && isValid == true) {

                        isValid = false;
                        alert('Please select appropriate input by understanding the declaration of good health');
                        return false;
                    }

                    if (IAgreeCondition == false && isValid == true) {

                        isValid = false;
                        alert('Please accept the terms and condition');
                        return false;
                    }

                    if (txtSelfmobileNo.value.match(phoneno)) {
                            isValid = true
                        }

                        else {
                            isValid = false;
                            alert("Please enter 10 digit mobile number.");
                            return false;
                        }

                    if (isValid == true) {
                        ShowOTP();
                    }

                    //return isValid;
                    return false;
                });

                function ShowOTP() {

                    var EmpEmailId = $("#lblEmpEmailId").text();
                    var EmpName = $("#lblEmployeeName").text();
                    //var MobNumber = $('#lblPhone').text();
                    var MobNumber = $("#txtSelfmobileNo").val();
                    var EmpCode = $("#lblEmployeeCode").text();
                    $("#hdnMobileNumber").val(MobNumber);

                    if ((MobNumber)) {
                        ShopOTPTimer();
                        var OTPData = { MobileNumber: MobNumber, EmailId: EmpEmailId, EmployeeCode: EmpCode, EmployeeName: EmpName };

                        $.ajax({
                            type: "POST",
                            url: "FrmKGHC.aspx/GenerateOTPNew",
                            data: JSON.stringify(OTPData),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (response.d == "success") {
                                    // ShopOTPTimer();
                                }
                                else {
                                    $("#sectionThankYou").hide();
                                    $("#sectionError").show();
                                    $("#sectionMain").hide();
                                }
                            },
                            error: function (response) {
                                alert(JSON.stringify(response));
                            }
                        });
                    }

                    else {
                        alert("Please enter 10 digit mobile number.");
                    }
                }

                function ShopOTPTimer() {

                    $("#otpPanel").show();
                    $("#divDeclarofGoodHealth").hide();
                    $("#txtOtpNumber").val('');

                    $('.timer').circularCountDown({
                        delayToFadeIn: 50,
                        size: 70,
                        fontColor: '#696969',
                        colorCircle: 'transparent',
                        background: '#ffa58c',
                        reverseLoading: true,
                        duration: {
                            seconds: parseInt(180)
                        },
                        beforeStart: function () {
                            $("#txtTimer").hide();
                            $('#btnMakePayment').hide();
                            $('#btnMobileReSend').attr('disabled', true);
                            $('#btnMobileVerify').attr('disabled', false);
                            $('#btnMobileVerify').removeAttr('disabled');
                            $('#btnMobileVerify').removeClass('disabled');
                        },
                        end: function (countdown) {
                            $("#txtTimer").show();
                            $('#btnMobileVerify').addClass('disabled');
                            $('#btnMobileVerify').attr('disabled', 'disabled');
                            $('#btnMakePayment').removeAttr('disabled');
                            $('#btnMakePayment').removeClass('disabled');
                            $('#btnMobileReSend').removeAttr('disabled');
                            $('#btnMobileReSend').removeClass('disabled');
                            $('#txtTimer').val('Your One Time Password has expired');
                        }
                    });
                }

                // For CR 142

                $('input[type=radio][name=radioPlanType]').change(function () {
                    var IsKLTEmployee = $('#hdnIsKLTEmployee').val();

                    var IsPlan1Checked = $('#radioPlan1').is(':checked');

                    if (IsPlan1Checked) {
                        var lblTotalAmountPayable1 = $("#lblTotalAmountPayable1").text();
                        $("#lblSelectedPremium").text(lblTotalAmountPayable1);
                    }
                    else if (!IsPlan1Checked) {
                        var lblTotalAmountPayable2 = $("#lblTotalAmountPayable2").text();
                        $("#lblSelectedPremium").text(lblTotalAmountPayable2);
                    }

                });

                // End here

                $('input[type=radio][name=radioDclrofgdhlth]').change(function () {
                    var IsYesChecked = $('#inlineradioYes').is(':checked');
                    var IsNoChecked = $('#inlineradioNo').is(':checked');
                    if (IsYesChecked) {
                        $("#btnMakePayment").show();
                        $("#lblMessageifNoSelected").hide();
                    }
                    else if (IsNoChecked) {
                        $("#btnMakePayment").hide();
                        $("#lblMessageifNoSelected").show();
                    }
                });

            });
        });

    </script>

    <script type="text/javascript">

        $(function () {
            callpopup();
        });

        $(function () {

            try {

                RedirectToPaymentGateway = getUrlVars()["RedirectToPaymentGateway"];
                if (RedirectToPaymentGateway != null) {
                    if (RedirectToPaymentGateway == "1") {
                        $("#divRedirectingInfo").show();
                        $("#divWrapper").hide();
                    }
                    else {
                        $("#divRedirectingInfo").hide();
                        $("#divWrapper").show();
                    }
                }
                else {
                    $("#divRedirectingInfo").hide();
                    $("#divWrapper").show();
                }
            } catch (e) {
                OnError(e.message);
            }

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

        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

    </script>

</body>
</html>
