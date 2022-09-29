<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSecuredLogin.aspx.cs" Inherits="ProjectPASS.FrmSecuredLogin" EnableViewStateMac="true" %>
<!DOCTYPE >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <title>Policy Admin Satellite System - PASS</title>
    <%--<script src="site/scripts/jquery-1.8.2.min.js"></script>--%>
    <script src="js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="site/scripts/bootstrap/js/bootstrap.min.js"></script>
    <script src="site/scripts/easing/jquery.easing.1.3.js"></script>
    <script src="site/scripts/carousel/jquery.carouFredSel-6.2.0-packed.js"></script>
    <script src="site/scripts/camera/scripts/camera.min.js"></script>
    <script src="site/scripts/default.js"></script>
    <script src="site/scripts/bootstrap/js/bootstrap.min.js"></script>

    <link href="site/scripts/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="site/scripts/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />

    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
        <script src="js/html5shiv.js"></script>
    <![endif]-->

    <!-- Icons -->
    <link href="site/scripts/icons/general/stylesheets/general_foundicons.css" media="screen" rel="stylesheet" type="text/css" />
    <link href="site/scripts/icons/social/stylesheets/social_foundicons.css" media="screen" rel="stylesheet" type="text/css" />

    <!--[if lt IE 8]>
        <link href="site/scripts/icons/general/stylesheets/general_foundicons_ie7.css" media="screen" rel="stylesheet" type="text/css" />
        <link href="site/scripts/icons/social/stylesheets/social_foundicons_ie7.css" media="screen" rel="stylesheet" type="text/css" />
    <![endif]-->
    <link href="site/scripts/fontawesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!--[if IE 7]>
        <link rel="stylesheet" href="site/scripts/fontawesome/css/font-awesome-ie7.min.css">
    <![endif]-->
    <link href="site/scripts/carousel/style.css" rel="stylesheet" type="text/css" />
    <link href="site/scripts/camera/css/camera.css" rel="stylesheet" type="text/css" />

    <link href="https://fonts.googleapis.com/css?family=Allura" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Aldrich" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Pacifico" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Palatino+Linotype" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Calligraffitti" rel="stylesheet" type="text/css" />

    <link href="site/styles/custom.css" rel="stylesheet" type="text/css" />

    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <!--Slider-in icons-->
    <script src="https://www.google.com/recaptcha/api.js?render=6LfKsvgaAAAAAFXD_eVhrWyi4UhNIYQb-ACfttwy"></script>
    <script src="js/crypto-js.js"></script>
    <%--<script src="https://code.jquery.com/jquery-3.2.1.min.js"></script>--%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $.get('https://www.cloudflare.com/cdn-cgi/trace', function (data) {
                const ipRegex = /[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}/
                $('#hdnLoginFromIPAddress').val(data.match(ipRegex)[0]);
            });

            $("#txtUserID").focus(function () {
                $(".user-icon").css("left", "28%");
            });
            $("#txtUserID").blur(function () {
                $(".user-icon").css("left", "25%");
            });

            $("#txtPassword").focus(function () {
                $(".pass-icon").css("left", "28%");
            });
            $("#txtPassword").blur(function () {
                $(".pass-icon").css("left", "25%");
            });

            $("#Cipher").focus(function () {
                if (typeof (grecaptcha) != 'undefined') {
                    grecaptcha.ready(function () {
                        grecaptcha.execute('6LfKsvgaAAAAAFXD_eVhrWyi4UhNIYQb-ACfttwy', { action: 'submit' }).then(function (token) {
                            // Validate the Token using Google Captcha Token Validate Service
                            $("#hdnTokenFromCaptchaService").val(token);
                        });
                    });
                }
            });

            $("#btnGetProperty").click(function () {

                //$('.login-form').find('input:password').prop({ type: "text" });
                $("#txtPassword").val($('#DeCipher').text());

                var key = CryptoJS.enc.Utf8.parse('8080808080808080');
                var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

                var vEncryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse($("#txtPassword").val()), key,
                                {
                                    keySize: 128 / 8,
                                    iv: iv,
                                    mode: CryptoJS.mode.CBC,
                                    padding: CryptoJS.pad.Pkcs7
                                });

                vEncryptedPassword = btoa(vEncryptedPassword);

                $("#txtPassword").val(vEncryptedPassword);
                
            });


            $('#Cipher').on('change blur', function () {
                var strName = $(this).prop('name');
                var strEncrypted = '';
                var objOriginal = $('#' + strName + 'Original');
                var strVal = objOriginal.val();
                switch (strName) {
                    case 'Hash':
                        strEncrypted = CryptoJS.MD5(strVal)
                        break;
                    case 'Cipher':
                        strEncrypted = CryptoJS.AES.encrypt(strVal, "Secret Passphrase");
                        $('#DeCipher').text(CryptoJS.AES.decrypt(strEncrypted, "Secret Passphrase").toString(CryptoJS.enc.Utf8));
                        break;
                }
                //$(this).val('********************');
            });

            //$('#Cipher').on('keyup focus', function (e) {
            //    var intKey = e.keyCode;
            //    $(this).val($(this).val()); // Hack to set the cursor at the end
            //    var strName = $(this).prop('name');
            //    var objOriginal = $('#' + strName + 'Original');
            //    var blnIsChar = ((intKey > 47 && intKey < 91) || (intKey > 95 && inKey < 112) || (intKey > 185 && inKey < 193) || (intKey > 218 && inKey < 223)) // Create boolean for key pressed in an input character
            //    if (intKey == 8) { // If we pressed backspace
            //        var strVal = objOriginal.val()
            //        if ($(this).val().length) { //if we have some characters
            //            strVal = strVal.slice(0, $(this).val().length); // remove number of characters
            //        } else {
            //            strVal = '';
            //        }
            //        objOriginal.val(strVal);
            //    } else if (blnIsChar) {
            //        var strLetter = $(this).val().slice(-1)
            //        var strVal = objOriginal.val() + strLetter;
            //        var strNew = '';
            //        for (i = 0; i < objOriginal.val().length; i++) {
            //            strNew += '●'
            //        }
            //        objOriginal.val(strVal);
            //        $(this).val(strNew + strLetter);
            //    } else if (e.type == 'focus') {
            //        var strNew = '';
            //        for (i = 0; i < objOriginal.val().length; i++) {
            //            strNew += '●'
            //        }
            //        $(this).val(strNew);
            //    }
            //});


            function createstars(n) {
                var stars = "";
                for (var i = 0; i < n; i++) {
                    stars += "*";
                }
                return stars;
            }


            $("#Cipher").on("keypress", function (e) {
                var code = e.which;
                if (code >= 32 && code <= 127) {
                    var character = String.fromCharCode(code);
                    $("#CipherOriginal").val($("#CipherOriginal").val() + character);
                }

                if (code == 13) {

                    var strName = $(this).prop('name');
                    var strEncrypted = '';
                    var objOriginal = $('#' + strName + 'Original');
                    var strVal = objOriginal.val();
                    switch (strName) {
                        case 'Hash':
                            strEncrypted = CryptoJS.MD5(strVal)
                            break;
                        case 'Cipher':
                            strEncrypted = CryptoJS.AES.encrypt(strVal, "Secret Passphrase");
                            $('#DeCipher').text(CryptoJS.AES.decrypt(strEncrypted, "Secret Passphrase").toString(CryptoJS.enc.Utf8));
                            break;
                    }

                }

            });

            var timer = 0;

            $('#Cipher').on('keyup focus', function (e) {

                var code = e.which;

               

                if (code == 8) {
                    var length = $("#Cipher").val().length;
                    $("#CipherOriginal").val($("#CipherOriginal").val().substring(0, length));
                } else if (code == 37) {

                } else {
                    var current_val = $('#Cipher').val().length;
                    $("#Cipher").val(createstars(current_val - 1) + $("#Cipher").val().substring(current_val - 1));
                }

                clearTimeout(timer);
                timer = setTimeout(function () {
                    $("#Cipher").val(createstars($("#Cipher").val().length));
                }, 200);

            });

        });





    </script>
    

</head>
<body>
    <div id="decorative2">
        <div id="divLogo" style="position: absolute; left: 10px; top: -3px; height: 80px; width: 250px">
            <img src="Images/logo.jpg" style="height: 80px; width: 250px">
        </div>
        <div id="divMenuRight" style="position: absolute; right: 10px">
            <div class="navbar">
                <button type="button" class="btn btn-navbar-highlight btn-large btn-primary" data-toggle="collapse" data-target=".nav-collapse">
                    NAVIGATION <span class="icon-chevron-down icon-white"></span>
                </button>
                <div class="nav-collapse collapse">
                    <ul class="nav nav-pills ddmenu">
                        <li class="dropdown active"><a href="FrmSecuredLogin.aspx">Home</a></li>
                        <li class="dropdown"><a href="http://www.kotak.com">About</a></li>
                        <li class="dropdown"><a href="http://www.kotak.com">Contact</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div style="position: relative; top: 20%; background-color: #ff0000; background-image: url(pg_header_kotak.png); background-size: auto; -webkit-background-size: cover; background-repeat: no-repeat">
        <div id="headerSeparator"></div>
        <div class="row-fluid">
            <div class="span6">
            </div>
            <div class="span6">
            </div>
        </div>
        <div id="headerSeparator2">
        </div>
    </div>
    <div style="position: absolute; left: 15px; top: 34%; width: 300px">
        <!--WRAPPER-->
        <div id="wrapper">

            <!--SLIDE-IN ICONS-->
            <div class="user-icon"></div>
            <div class="pass-icon"></div>
            <!--END SLIDE-IN ICONS-->

            <!--LOGIN FORM-->
            <form id="Form1" name="login-form" class="login-form" runat="server" method="post" autocomplete="off">

                <!--HEADER-->
                <div class="header">
                    <!--TITLE-->
                    <h1>PASS - Login</h1>
                    <!--END TITLE-->
                    <!--DESCRIPTION-->
                    <span>Fill out the form below to login to PASS</span>
                    <br />
                    <span>Internal Users - Please use your <b>windows credentials</b></span>
                </div>
                <!--END HEADER-->

                <!--CONTENT-->
                <div class="content">
                    <!--USERNAME-->
                    <asp:TextBox ID="txtUserID" runat="server" class="input" value="Username" onfocus="this.value=''" autocomplete="off" /><!--END USERNAME-->
                    <!--PASSWORD-->
                    <input type="hidden" id="CipherOriginal" name="CipherOriginal" />
                    <input type="text" id="Cipher" name="Cipher" placeholder="Password" class="input" />
                    <span id="DeCipher" style="visibility:hidden"></span>
                    <asp:TextBox ID="txtPassword" style="display:none;" runat="server" class="input" value="Password" onfocus="this.value=''" autocomplete="off" /><!--END PASSWORD-->
               <%--  <label id="lblMessage" runat="server" ></label>  --%>
                    <asp:HiddenField ID="hdnMSG" runat="server" ClientIDMode="Static" Value="0" />
                    <asp:HiddenField ID="hdnTokenFromCaptchaService" runat="server" ClientIDMode="Static" Value="" />
                    <asp:Label ID="lblMessage1" runat="server" ForeColor="Red" />
                </div>
                <!--END CONTENT-->

                <!--FOOTER-->
                <div class="footer">
                    <!--LOGIN BUTTON-->
                    <asp:Button type="submit" ID="btnGetProperty" OnClick="btnGetProperty_Click" runat="server" Text="Login" class="button" /><!--END LOGIN BUTTON-->
                    <asp:HiddenField ID="hdnLoginFromIPAddress" runat="server" Value="" ClientIDMode="Static">   </asp:HiddenField>
                    <br />
                    <a href="FrmForgotPassword.aspx">Forgot Password?</a>
                    <asp:Label ID="lblstatus" runat="server" />
                </div>
                <!--END FOOTER-->

            </form>
            <!--END LOGIN FORM-->

        </div>
        <!--END WRAPPER-->

    </div>


    <div style="position: absolute; top: 34%; left: 450px; width: 300px">
        <h4 style="color: whitesmoke">About Kotak Mahindra Group</h4>
        <img src="images/claims.png" class="img-polaroid" style="margin: 5px 0px 22px; height: 200px" alt="">
        <br />
        <p style="color: whitesmoke">
            Kotak Mahindra is one of India's leading banking and financial services organizations, offering a wide range of financial services.
                                <br />
        </p>
    </div>

    <div style="position: absolute; top: 34%; left: 850px; width: 350px">
        <h4 style="color: whitesmoke">About PASS</h4>
        <!--Edit Camera Slider here-->
        <div id="camera_wrap">
            <div data-src="images/child_plans.png">
                <div class="camera_caption fadeFromBottom cap1">Manage Policy Documents.</div>
            </div>
            <div data-src="images/protection_plans.png">
                <div class="camera_caption fadeFromBottom cap2">Generate Instant Policy Certificates.</div>
            </div>
            <div data-src="images/retirement_plans.png">
                <div class="camera_caption fadeFromBottom cap1">Bulk Upload all policies.</div>
            </div>
            <div data-src="images/savings_investment_plans.png">
                <div class="camera_caption fadeFromBottom cap1">Manage User role rights.</div>
            </div>
        </div>
        <!--End Camera Slider here-->
        <p style="color: whitesmoke">
            Kotak Mahindra General Insurance Ltd is a subsidary of Kotak Mahindra Bank Ltd., its affiliates.
                                <br />
        </p>
    </div>


    <script type="text/javascript">function startCamera() { $('#camera_wrap').camera({ fx: 'scrollLeft', time: 2000, loader: 'none', playPause: false, navigation: true, height: '40%', pagination: true }); } $(function () { startCamera() });</script>
    <script type="text/javascript">$('#list_photos').carouFredSel({ responsive: true, width: '100%', scroll: 2, items: { width: 320, visible: { min: 2, max: 6 } } });</script>

</body>

</html>

