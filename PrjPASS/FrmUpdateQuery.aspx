<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master"  CodeBehind="FrmUpdateQuery.aspx.cs" Inherits="PrjPASS.FrmUpdateQuery" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>





<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Update IMD</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel3" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 0%; left: 1%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutRadioButton ID="rdoPASS" runat="server" Text="AP" Checked="True" AutoPostBack="true" OnCheckedChanged="rdo_CheckedChanged" GroupName="AppName"></obout:OboutRadioButton>
                                <span style="margin-left: 60px"></span>
                                <obout:OboutRadioButton ID="rdoBPOS" runat="server" Text="BPOS" AutoPostBack="true" OnCheckedChanged="rdo_CheckedChanged" GroupName="AppName"></obout:OboutRadioButton>
                            </div><br />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <%--<asp:HiddenField ID="hdnMintenure" runat="server" />
                            <asp:HiddenField ID="hdnMaxtenure" runat="server" />--%>
                            <div style="position: absolute; top: 1%; left: 1%; width: 80%; margin-top: 10px; margin-left: 300px">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text="Select drop down values and then select file for upload "/>
                                </div>
                            <div style="position: absolute; top: 45%; left: 54%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Select File" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 45%; left: 65%; width: 40%; margin-top: 10px; margin-left: 10px">
                                <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" />
                            </div>
                           <%-- <div style="position: absolute; top: 55%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label2" runat="server" Text="Plan" Width="100%"></asp:Label>
                            </div>--%>
                        <%--    <div style="position: absolute; top: 55%; left: 21%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:ComboBox ID="drpPlanList" runat="server" Width="100%" Height="100px" OnSelectedIndexChanged="drpPlanList_SelectedIndexChanged" AutoPostBack="true" AllowEdit="false"></obout:ComboBox>
                            </div>--%>
                           
                            <div style="position: absolute; top: 25%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label4" runat="server" Text="SELECT" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 25%; left: 21%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:ComboBox ID="drpType" runat="server" Width="100%" OnSelectedIndexChanged="drpType_SelectedIndexChanged" AutoPostBack="true" AllowEdit="false" AppendDataBoundItems="true"> </obout:ComboBox>
                                
                            </div>

                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel4" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="OboutButton1" runat="server" Text="Import Data" Width="100%" OnClick="Upload" OnClientClick="return Fn_ValidateUpload();" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="OboutButton2" runat="server" Text="Get Template" Width="100%" OnClick="btnExport_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="OboutButton3" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click" />
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label5" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Fn_ValidateUpload() {
            if ($('#FileUpload2').val() == '') {
                alert('Please select the file to be Uploaded');
                $('#FileUpload2').focus();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>



