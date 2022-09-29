<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlinePaymentProcess.aspx.cs" Inherits="PrjPASS.OnlinePaymentProcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style id="cssmdlThankYouconfirm">
    #mdlThankYouConfirm .panel-default > .panel-heading {
        color: #fafafa;
        background-color: #036;
        border-color: #ddd;
    }

    #mdlThankYouConfirm .text-primary {
        color: #fafafa;
    }

    #mdlThankYouConfirm .lead {
        margin-bottom: 20px;
        font-size: 16px;
        font-weight: 300;
        line-height: 1.4;
    }

    #mdlThankYouConfirm .row {
        margin-right: -15px;
        margin-left: 0px;
    }

    #mdlThankYouConfirm .btn:hover, .btn:focus, .btn.focus {
        color: #fafafa;
        text-decoration: none;
    }

    #mdlThankYouConfirm .ls-closed .navbar-brand {
        margin-left: 0px;
    }

    #mdlThankYouConfirmBreakup .panel {
        margin-bottom: 20px;
        background-color: white;
        border: 1px solid transparent;
        border-radius: 4px;
        -webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
        box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
        font-family: "Source Sans Pro", sans-serif;
        color: #656565;
    }

    #mdlThankYouConfirmBreakup h2 {
        margin-bottom: 10px;
    }

    #mdlThankYouConfirmBreakup .modal-header {
        border: none;
        padding: 25px 25px 5px 25px;
        background-color: #fff7f8;
    }

    #mdlThankYouConfirmBreakup .card .body {
        font-size: 14px;
        color: #555;
        padding: 20px;
        background-color: #fff7f8;
    }
</style>
    <form id="form1" runat="server">
    <div>
    <section>
                    <div class="content-wrapper" id="divSuccess" runat="server" >
                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">Thank You</div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Your payment has been successfully processed. 
                                </p>
                                <hr />
                                For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                                <hr />
                            </div>
                        </div>
                    </div>
                    <div class="content-wrapper" id="divFailure" runat="server">
                        <div class="abs-center">
                            <div class="text-center mv-lg">
                                <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                    Payment Failed!!!
                                </div>
                                <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                    Oops! It looks like something went wrong and the payment wasn't processed.
                                    <br />
                                    Please note down following details for further communication:
                                    <br />
                                    <strong>MihPayID: </strong>
                                    <asp:Label runat="server" ID="lblMihPayIDFailed"  Text="-"></asp:Label>
                                    <br />
                                    <strong>TxnID: </strong>
                                    <asp:Label runat="server" ID="lblTxnIDFailed"  Text="-"></asp:Label>
                                    <br />
                                    <strong>Error: </strong>
                                    <asp:Label runat="server" ID="lblError"  Font-Bold="true" ForeColor="Red" />
                                    <br />
                                </p>
                                <hr />
                                For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                                <hr />
                            </div>
                        </div>
                    </div>
                </section>
    </div>
    </form>
</body>
</html>
