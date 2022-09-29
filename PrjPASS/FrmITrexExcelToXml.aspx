<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmITrexExcelToXml.aspx.cs" Inherits="PrjPASS.FrmITrexExcelToXml" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>


<asp:Content ID="ContentID" ContentPlaceHolderID="MstCntFormContent" runat="server"> 
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4 ">
            <div class="form-header header-blue">
                <h4>MIS - Convert ExcelToXML</h4>
                 <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                </div>
            </div>
            <div class="form-body  theme-blue">
               <div class="frm-row">
                   <div class="section colm colm12">
                      <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                         <asp:Label ID="Label1" runat="server" Text="Select File" style="position: absolute; top: 45%; left: 20%; width: 34%; margin-top: 10px; margin-left: 10px"></asp:Label>
                               <asp:FileUpload ID="FileUpload1" runat="server" style="position: absolute; top: 45%; left: 31%; width: 22%; margin-top: 10px; margin-left: 10px" />
                               <br />
                      </asp:Panel>
                   </div>
                   <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnDownloadMIS" runat="server" Text="GenerateXML" Width="100%" OnClick="BtnUpload_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExport" runat="server" Text="Exit" Width="100%" Onclick="btnExit_Click"/>
                                </div>
                                </asp:Panel>
                                </div>
                             </div>
            </div>
</div>
    </div>
</asp:Content>