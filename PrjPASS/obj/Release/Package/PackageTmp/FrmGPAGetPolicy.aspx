<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmGPAGetPolicy.aspx.cs" Inherits="ProjectPASS.FrmGPAGetPolicy" EnableEventValidation="false" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        $(function () {
            ApplyDatePicker();
            ApplyDatePickertoImage();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePickertoImage);
        });

        function ApplyDatePicker() {
            var d = new Date();
            var year = d.getFullYear() - 10;
            d.setFullYear(year);

            $("[id$=txtCustDOB]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                yearRange: '1950:' + year + '', defaultDate: d
            });
        }


        function ApplyDatePickertoImage() {
            $("#datepickerImagetxtCustDOB").click(function () {
                $("[id$=txtCustDOB]").datepicker("show");
            });
        }
        

    </script>

</asp:Content>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - View Policy Certificate(GPA)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="350px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 6%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label6" runat="server" Text="Policy Type" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 6%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutRadioButton ID="rdoGPAPolicyType" runat="server" Text="GPA POLICY" GroupName="PolicyType"></obout:OboutRadioButton>
                                <span style="margin-left: 60px"></span>
                                    <obout:OboutRadioButton ID="rdoHDColicyType" runat="server" Text="HDC POLICY" GroupName="PolicyType"></obout:OboutRadioButton>
                            </div>

                            <div style="position: absolute; top: 6%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label5" Text="Certificate Number" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 6%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertificateNumber" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>

                            <div style="position: absolute; top: 26%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Policy Number" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 26%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 26%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="Crn No" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 26%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 46%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label4" Text="Customer Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 46%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerName" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 46%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="lblAccountNumber" Text="Loan Account Number" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 46%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtLoanAccountNumber" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>

                            <div style="position: absolute; top: 66%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustDOB" runat="server" Text="Customer DOB" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 66%; left: 10%; width: 35%; margin-top: 5px; margin-left: 10px" >
                                <obout:OboutTextBox ID="txtCustDOB" runat="server" Width="50%" placeholder="dd/mm/yyyy" autocomplete="off"></obout:OboutTextBox>
                                <img src="images/calendar.png" alt="" id="datepickerImagetxtCustDOB" style="position:relative;top:5px" />
                            </div>
 
                            

 
                            
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 20%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGPACertificate" runat="server" Text="GPA / HDC Certificate" Width="100%" OnClick="btnGPACertificate_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 40%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGPACertificate_WithoutHeaderFooter" runat="server" Text="GPA Certificate Print Version" Width="100%" OnClick="btnGPACertificate_WithoutHeaderFooter_Click" />
                            </div>
                            <%-- <div style="position: absolute; top: 40%; left:50%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGetPolicy" runat="server" Text="View Policy" Width="100%" OnClick="btnGetPolicy_Click" />
                            </div>--%>
                            <div style="position: absolute; top: 40%; left: 60%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click" />
                            </div>
                            <div style="position: absolute; top: 58%; left: 45%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
