<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmDownloadMIS_TPA.aspx.cs" Inherits="PrjPASS.FrmDownloadMIS_TPA" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4>PASS - MIS Download(TPA)</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <asp:HiddenField ID="hdnProductName" runat="server" />
                         
                            <div style="position: absolute; top: 45%; left: 54%; width: 8%; margin-top: 10px; margin-left: 10px">
                            
                            <asp:LinkButton ID="lnk_template"  runat="server" Text="Download Template" OnClick="btnExport_Click" />
                            </div>
                            <div style="position: absolute; top: 45%; left: 65%; width: 40%; margin-top: 10px; margin-left: 10px">
                            
                             <obout:OboutButton ID="btnUpload" runat="server" Text="Upload" Width="40%" Onclick="btnUpload_Click"/>
                               <asp:Label ID="lblFile" runat="server" BackColor="Lime" Font-Size="Medium" Text=""></asp:Label> 
                            </div>
                           
                            <asp:Label ID="Label1" runat="server" Text="Select File" style="position: absolute; top: 45%; left: 20%; width: 34%; margin-top: 10px; margin-left: 10px"></asp:Label>
                            <asp:FileUpload ID="FileUpload1" runat="server" style="position: absolute; top: 45%; left: 31%; width: 22%; margin-top: 10px; margin-left: 10px" />
                           
                            <br />
                           
                        </asp:Panel>

                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnDownloadMIS" runat="server" Text="Download MIS" Width="100%" OnClick="btnDownloadMIS_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExport" runat="server" Text="Get Template" Width="100%" Onclick="btnExport_Click"/>
                                
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width: 15%; margin-top: 10px; margin-left: 10px">
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
</asp:Content>