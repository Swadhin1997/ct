<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmGHICustomerInfo.aspx.cs" Inherits="PrjPASS.FrmGHICustomerInfo" %>

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
    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>

<body style="background-color:#fafafa;">
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
    <h3></h3>
    <form class="form-horizontal" runat="server" style="padding-left: 15px; padding-right:15px;">
        <header>
                <!-- START Top Navbar-->
                <nav role="navigation">
                    <!-- START navbar header-->
                    <div class="header">
                    </div>
                    <!-- END navbar header-->
                    <!-- START Nav wrapper-->
                    <div class="row" style="padding-bottom: 15px;">

                        <!-- START Right Navbar-->
                        
                        <%--<ul style="text-align: center;">
                            <li>
                                <h2>Kotak Group Health Care</h2>
                            </li>
                        </ul>--%>
                        <h2 style="text-align: center;">Kotak Group Health Care – Details for Enrolment</h2>
                        <!-- END Right Navbar-->
                    </div>
                    <!-- END Nav wrapper-->
                </nav>
                <!-- END Top Navbar-->
            </header>
        <div class="row" runat="server" id="MainRow" style="padding-left: 10px; padding-right:10px;">

            <%--<asp:Panel ID="pnlInfo" runat="server" Visible="true">--%>
                <div class="form-group row">
                    
                    <label class="control-label col-sm-2" for="EmployeeCode" style="text-align:left;width: 130px;">Employee Code:</label>
                    <div class="col-sm-2">
                        <%--<input type="email" class="form-control" id="email" placeholder="Enter email" />--%>
                        <input type="text" id="txtEmployeeCode" name="Employee Code" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                   <label class="control-label col-sm-2" for="Name" style="text-align:left;width: 130px;">Name:</label>
                    <div class="col-sm-2">
                        <%--<asp:TextBox ID="txtName" runat="server" placeholder="Enter Name" CssClass="form-control"></asp:TextBox>--%>
                        <input type="text" id="txtName" name="Name" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="DateOfBith" style="text-align:left;width: 130px;">Date Of Birth:</label>
                    <div class="col-sm-2">
                        <input type="text" id="txtDOB" name="Date Of Birth" class="form-control" readonly="true" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="Age" style="text-align:left;width: 130px;">Age:</label>
                    <div class="col-sm-2">
                        <input type="text" id="txtAge" name="Age" class="form-control" readonly="true"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="Gender" style="text-align:left;width: 130px;">Gender:</label>
                    <div class="col-sm-2">
                        <%--<input type="text" id="txtGender" name="Gender" class="form-control" />--%>
                        <select id="cboGender" class="form-control" >
                            <option value="0" selected="selected">Select</option>
                            <option value="1">Male</option><option value="2">Female</option></select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="EmailID" style="text-align:left;width: 130px;">Email ID:</label>
                    <div class="col-sm-2">
                        <input type="text" id="txtEmailID" name="Email ID" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="MobileNo" style="text-align:left;width: 130px;">Mobile No:</label>
                    <div class="col-sm-2">
                        <input type="text" id="txtMobileNo"  name="Mobile No" class="form-control"  maxlength="10"/>
                    </div>
                </div>
                <%--<div class="form-group">
                    <div class="col-sm-offset-2 col-sm-3">
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" />
                                Remember me</label>
                        </div>
                    </div>
                </div>--%>
            <%--<div class="form-group row">
    <label for="staticEmail" class="col-sm-2 col-form-label">PN</label>
    <div class="col-sm-10">
      <input type="text"  class="form-control-plaintext" id="staticEmail" value="email@example.com"/>
    </div>
  </div>--%>
                <div class="form-group" style="padding-left: 20px;padding-right: 0px;">
                    <div class="col-sm-offset-1 col-sm-2" style="padding-left: 15px;">
                        <button type="submit" class="btn btn-default" id="BtnSubmit" runat="server" style="background-color:#ff902b;width: 194px;">Submit</button>
                    </div>
                </div>
           <%-- </asp:Panel>--%>
            <footer style="background-color: white; z-index: 113;">
                <span style="font-size: 12px; text-align: center; float: left"><b>
                    Please note: Online enrolment link will be shared on the above mentioned email ID for completion of the enrolment process
                </b></span>
            </footer>
        </div>
    </form>
    <%--<form id="form1" runat="server">
        <section id="mainsection">
            <div class="panel-body">
    <div class="row">
        <div class="form-group">
            <label for="EmployeeCode">Employee Code</label>
            <asp:TextBox ID="txtEmployee" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div></div>
        </section>
    
    </form>--%>

    <script src="css/newcssjs/js/bootstrap.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#txtMobileNo').keypress(function (event) {
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
                var age = "";
                var today = "";

                if (id.indexOf('txtDOB') >= 0) {
                    $(document).on('focus', "#" + id, function () {
                        $(this).datepicker({
                            onSelect: function (value, ui) {
                                var today = new Date();
                                age = today.getFullYear() - ui.selectedYear;
                                $('#txtAge').val(age);
                                dateFormat: 'dd/mm/yy';
                            },
                            changeMonth: true,
                            changeYear: true,
                            showButtonPanel: true,
                            dateFormat: 'dd/mm/yy',
                            yearRange: '1920:' + year + '', defaultDate: crndt
                        });
                    });
                }
            });
        });

        //$(document).ready(function () {
        //    var crndt = new Date();
        //    var year = crndt.getFullYear() - 1;
        //    crndt.setFullYear(year);
        //    var age = "";
        //    $('#txtDOB').datepicker({
        //        onSelect: function (value, ui) {
        //            var today = new Date();
        //            age = today.getFullYear() - ui.selectedYear;
        //            $('#txtAge').val(age);
        //            dateFormat: 'dd/mm/yy';
        //            //yearRange: '1920:' + year + ''; defaultDate: crndt
        //        },
        //        changeMonth: true,
        //        changeYear: true
        //    })
        //})
        function Resetfields() {
            $('#txtEmployeeCode').val("");
            $('#txtName').val("");
            $('#txtDOB').val("");
            $('#txtAge').val("");
            $('#cboGender').prop('selectedIndex', 0);
            $('#txtEmailID').val("");
            $('#txtMobileNo').val("");

        }

        $('#BtnSubmit').click(function () {
            var isValid = true;
            var phoneno = /^\d{10}$/;
            $('#MainRow').children('.form-group').find('.form-control').each(function () {

                
                var element = $(this);
                var id = $(this).attr('id');
                var name = $(this).attr('name');
                var EmailAddress = $('#txtEmailID').val();

                if (element.val() == "") {
                    isValid = false;
                    alert('Please Enter ' + name);
                    return false;
                }
                //else if (name.indexOf('ight') >= 0) {
                //    var isNotANumber = isNaN(element.val());
                //    if (isNotANumber == true) {
                //        isValid = false;
                //        alert('Please Enter Valid ' + name);
                //        return false;
                //    }
                //    else if (parseInt(element.val()) <= 0) {
                //        isValid = false;
                //        alert('Please Enter Valid ' + name);
                //        return false;
                //    }
                //}
                var cboNomineeRelation = $('#cboGender').val();
                if (cboNomineeRelation == "Select") {
                    isValid = false;
                    alert("Please Select Gender.");
                    return false;
                }

                var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
                if (testEmail.test(EmailAddress)) {
                    isValid = true
                }
                    // Do whatever if it passes.
                else {
                    isValid = false;
                    alert("Please enter valid email address.")
                    return false;
                }

                if (txtMobileNo.value.match(phoneno)) {
                    isValid = true
                }

                else {
                    isValid = false;
                    alert("Please enter 10 digit Mobile number.");
                    return false;
                }

                
                // Do whatever if it fails.
                //if (isValid == false)
                //{
                //    return false;
                //}
            });

            if (isValid == true) {
                var objCustomerDetails = {};

                objCustomerDetails = {


                    MobileNumber: $("#txtMobileNo").val(),
                    EmployeeCode: $("#txtEmployeeCode").val(),
                    Name: $("#txtName").val(),
                    EmailId: $("#txtEmailID").val(),
                    Age: $("#txtAge").val(),
                    DOB: $("#txtDOB").val(),
                    //Gender: $("#cboGender").val()
                    Gender: $('#cboGender :selected').text()


                };
                debugger;
                $.ajax({
                    type: "POST",
                    url: "FrmGHICustomerInfo.aspx/ValidateAndAddEmployee",
                    data: '{objCustomerDetails: ' + JSON.stringify(objCustomerDetails) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        if (response.d == "success") {
                            debugger;
                            //$("#sectionThankYou").show();
                            //$("#sectionError").hide();
                            //$("#sectionMain").hide();
                            //$('#divTimer').hide();

                            //$('#otpPanel').hide();
                            // isSuccess = true;
                            alert("Data Successfully Saved.")
                            Resetfields()
                        }
                        else {
                            //$("#sectionThankYou").hide();
                            //$("#sectionError").show();
                            //$("#sectionMain").hide();
                            alert("EmployeeCode Already Exist.")
                            Resetfields()
                        }
                    },
                    error: function (response) {

                        alert(JSON.stringify(response));
                    }
                });
                return false;
            }
            else
            {
                

                return false;
            }
           

        });
        </script>
</body>
</html>
