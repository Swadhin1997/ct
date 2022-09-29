<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHealthClaimsUploadStatus.aspx.cs" Inherits="PrjPASS.FrmHealthClaimsUploadStatus" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Health Claims Upload Status Dump</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="160px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 6%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label6" Text="Status" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 6%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="ddlStatus" runat="server" Width="100%" MaxLength="100">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="Intimation Register">Intimation Register</asp:ListItem>
                                    <asp:ListItem Value="Claim Outstanding Register">Claim Outstanding Register</asp:ListItem>
                                    <asp:ListItem Value="Paid Register">Paid Register</asp:ListItem>
                                    <asp:ListItem Value="Claim Rejection Register">Claim Rejection Register</asp:ListItem>
                                </obout:OboutDropDownList>
                            </div>
                            <br />
                            <div style="position: absolute; top: 33%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="From Date" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 33%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtFromDate" runat="server" class="input" Width="120px" />
                                <obout:Calendar ID="Calendar1" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtFromDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>
                            <div style="position: absolute; top: 33%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label2" Text="To Date" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 33%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtToDate" runat="server" class="input" Width="120px" />
                                <obout:Calendar ID="Calendar2" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtToDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>
                            </div>
                            <div style="position: absolute; top: 62%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label4" Text="KGI Claim Number" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 62%; left: 10%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimNumber" ClientIDMode="Static" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 62%; left: 48%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label runat="server" ID="Label5" Text="Upload Id" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 62%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtUploadId" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGetPolicy" runat="server" Text="Download Dump" Width="100%" OnClick="btnGetPolicy_Click" OnClientClick="return validateFilters();" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 55%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click" />
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function validateFilters() {
            debugger;
            if ($('#ctl00_MstCntFormContent_txtFromDate').val().trim() == '' && $('#ctl00_MstCntFormContent_txtToDate').val().trim() == '' && $('#ctl00_MstCntFormContent_txtClaimNumber').val().trim() == '' && $('#ctl00_MstCntFormContent_txtUploadId').val().trim() == '' && $('#ctl00_MstCntFormContent_ddlStatus').val() != 'Claim Outstanding Register') {
                alert('Please select atleast one search Filter\nOr Select Status "Claim Outstanding Register"');
                return false;
            }
            if ($('#ctl00_MstCntFormContent_ddlStatus').val() == 'Claim Outstanding Register' && ($('#ctl00_MstCntFormContent_txtFromDate').val().trim() != '' || $('#ctl00_MstCntFormContent_txtToDate').val().trim() != ''))
            {
                $('#ctl00_MstCntFormContent_txtFromDate').val('');
                $('#ctl00_MstCntFormContent_txtToDate').val('');
                alert('Date range will not be considered when selected Status is "Claim Outstanding Register"');
            }
            return true;
        }
       
    </script>
</asp:Content>
