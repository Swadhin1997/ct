<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDownloadGPAPolicy.aspx.cs" Inherits="PrjPASS.FrmDownloadGPAPolicy" %>

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
    <link rel="stylesheet" href="css/newcssjs/bootstrap-datetimepicker.min.css" />
    <link rel="stylesheet" href="css/newcssjs/bootstrap.css" id="bscss" />
    <!-- =============== APP STYLES ===============-->
    <link rel="stylesheet" href="css/newcssjs/app.css" id="maincss" />





</head>
<body>
    <script src="css/newcssjs/js/jquery.js"></script>



    <script type="text/javascript">
        $(document).ready(function () {
            $('input:radio[name=i-radio]').change(function () {
                var GPAChecked = $('#RadioGPAPolicy').is(':checked');
                if (GPAChecked) {
                    $('#lblAccountMessage').text('Bank Account Number');
                }
                else {
                    $('#lblAccountMessage').text('Loan Account Number');
                }
            });
        });

        $('input:radio[name=i-radio]').attr('checked', 'checked');

    </script>

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
                            <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em>care@kotak.com</a>
                        </li>

                        <li>
                            <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em>1800 266 4545</a>
                        </li>

                    </ul>
                    <!-- END Right Navbar-->
                </div>
                <!-- END Nav wrapper-->

            </nav>
            <!-- END Top Navbar-->
        </header>

        <form id="form1" runat="server">
            <div id="accordion1" class="panel-group">
                <div class="panel panel-default b0">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <%--<a data-toggle="collapse" data-parent="#accordion1" href="#acc1collapse1" class="collapsed" aria-expanded="true">--%>
                            <small>
                                <%--<em class="fa fa-car text-primary mr"></em>--%>
                            </small>
                            <span>Enter Details (Please Enter Any Two Fields Out Of - Policy Number/CRN/Date Of Birth/Bank Account Number)</span>
                            <%--  </a>--%>
                        </h4>
                    </div>
                    <div id="acc1collapse1" class="panel-collapse collapse in" aria-expanded="true">
                        <div class="panel-body">
                            <div class="row">

                                <div class="row">
                                    <%-- <div class="col-xs-12 col-sm-12">
                                         <dl> <dt>Policy Type </dt></dl>
                                            <div class="radio c-radio">
                                            <label>
                                                            <input name="PolicyType" type="radio" checked="checked" value="GPAPolicy" />
                                                               <span class="fa fa-circle"></span> GPA Policy</label>
                                                    <label>
                                                            <input name="PolicyType" type="radio" checked="" value="HDC"/>
                                                               <span class="fa fa-circle"></span>HDC Policy</label>
                                                </div>  
                                        </div>--%>


                                    <div class="row">
                                        <label class="col-sm-2 control-label">&nbsp; &nbsp; &nbsp; Policy Type</label>
                                        <div class="col-sm-10">
                                            <label class="radio-inline c-radio">
                                                <input name="i-radio" id="RadioGPAPolicy" type="radio" value="GPAPolicy" runat="server" />
                                                <span class="fa fa-circle"></span>GPA Policy</label>
                                            <label class="radio-inline c-radio">
                                                <input name="i-radio" id="RadioHDCPolicy" type="radio" value="HDCPolicy" runat="server" />
                                                <span class="fa fa-circle"></span>KOTAK GROUP SMART CASH</label>
                                            <label class="radio-inline c-radio">
                                                <input name="i-radio" id="RadioGHIPolicy" type="radio" value="GHIPolicy" runat="server" />
                                                <span class="fa fa-circle"></span>GHI Policy</label>
                                        </div>
                                    </div>



                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>Policy Number</dt>
                                            <%-- <dd><asp:Label ID="lblRegistrationNumber" runat="server" Text="-"></asp:Label></dd>--%>
                                        </dl>
                                    </div>
                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>
                                                <asp:TextBox ID="txtPolicyNumber" runat="server" Width="120px" MaxLength="100"></asp:TextBox></dt>
                                            <%--<dd><asp:Label ID="lblMakeText" runat="server" Text="-"></asp:Label> <asp:Label ID="lblModelText" runat="server" Text="-"></asp:Label> <asp:Label ID="lblVariantText" runat="server" Text="-"></asp:Label> </dd>--%>
                                        </dl>
                                    </div>
                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>CRN No.</dt>
                                            <%--<dd><asp:Label ID="lblFuelType" runat="server" Text="-"></asp:Label></dd>--%>
                                        </dl>
                                    </div>
                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>
                                                <asp:TextBox ID="txtCRN" runat="server" Width="120px" MaxLength="100"></asp:TextBox></dt>
                                            <%--<dd><asp:Label ID="lblChassisNumber" runat="server" Text="-"></asp:Label></dd>--%>
                                        </dl>
                                    </div>
                                    <%--        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>Date Of Birth</dt>
                                                
                                            </dl>
                                        </div>
                                        <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt>
                                                    <div class="col-sm-10" style="width:200px">
                              <div id="datetimepicker1" class="input-group date">
                                 <input type="text" class="form-control" id="txtdate" runat="server"/>
                                 <span class="input-group-addon">
                                     
                                    <span class="fa fa-calendar"></span>
                                 </span>
                              </div>
                           </div>


                                                </dt>
                                               
                                            </dl>
                                        </div>--%>
                                </div>

                                <div class="row">

                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>Date Of Birth</dt>

                                        </dl>
                                    </div>
                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>
                                                <div style="width: 150px">
                                                    <div id="datetimepicker1" class="input-group date">
                                                        <input type="text" class="form-control" id="txtdate" runat="server" />
                                                        <span class="input-group-addon">

                                                            <span class="fa fa-calendar"></span>
                                                        </span>
                                                    </div>
                                                </div>


                                            </dt>
                                            <%--<dd><asp:Label ID="lblCreditText" runat="server" Text="0"></asp:Label></dd>--%>
                                        </dl>
                                    </div>


                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <%-- <dt> Bank Account Number</dt>--%>
                                            <dt>
                                                <asp:Label ID="lblAccountMessage" runat="server" Text="Account Number"></asp:Label>
                                            </dt>
                                        </dl>
                                    </div>
                                    <div class="col-xs-6 col-sm-2">
                                        <dl>
                                            <dt>
                                                <asp:TextBox ID="txtAccountNumber" runat="server" Width="120px" MaxLength="100"></asp:TextBox></dt>
                                        </dl>
                                    </div>



                                    <%--<dd><asp:Label ID="lblCreditText" runat="server" Text="0"></asp:Label></dd>--%>
                                </div>

                                <%--   <div class="col-xs-6 col-sm-2">
                                            <dl>
                                                <dt> <asp:Button ID="Button1" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  ClientIDMode="Static" Text="Get Policy Certificate" OnClick="btnGetPolicy_Click" /> <br /></dt>
                                                
                                            </dl>
                                        </div>--%>
                            </div>
                            <div style="text-align: center">
                                <asp:Button ID="btnGetPolicy" runat="server" CssClass="btn btn-info" Style="background: #80631f !important; font-size: 1.9rem; border-radius: 8px" ClientIDMode="Static" Text="Get Policy Certificate" OnClick="btnGetPolicy_Click" />
                                <br />
                                <asp:Label ID="lblError" ForeColor="Red" runat="server" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
    


    <%--<asp:Button ID="btnGetPolicy" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px"  ClientIDMode="Static" Text="Get Policy Certificate" OnClick="btnGetPolicy_Click" />      --%>
	
        </form>
     
        <!-- Page footer-->
    <footer style="background-color: white">
        <span style="font-size: 12px; text-align: center; padding: 10px; float: left">Insurance is the subject matter of the solicitation. The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. (Formerly Kotak Mahindra General Insurance Ltd.) CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India.
        </span>
    </footer>

    </div>


    <!-- JQUERY-->
    <%-- <script src="css/newcssjs/js/jquery.js"></script>--%>
    <!-- BOOTSTRAP-->
    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script src="css/newcssjs/js/bootstrap-filestyle.js"></script>

    <script src="css/newcssjs/js/chosen.jquery.min.js"></script>

    <script src="css/newcssjs/js/bootstrap-slider.min.js"></script>
    <!-- INPUT MASK-->
    <script src="css/newcssjs/js/jquery.inputmask.bundle.js"></script>
    <!-- WYSIWYG-->
    <script src="css/newcssjs/js/bootstrap-wysiwyg.js"></script>

    <script src="css/newcssjs/js/moment-with-locales.min.js"></script>

    <script src="css/newcssjs/js/bootstrap-datetimepicker.min.js"></script>

    <script src="css/newcssjs/js/demo-forms.js"></script>

</body>
</html>
