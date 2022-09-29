<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmIRDARegisterComplaint.aspx.cs" Inherits="ProjectPASS.FrmIRDARegisterComplaint" %>

<asp:Content runat="server" ContentPlaceHolderID="MstCntFormContent">
    <!--HEADER-->
                        <div class="header">
                            <!--TITLE-->
                            <h1>PASS - Policy Upload(GPA)</h1>
                            <!--END TITLE-->
                            <!--DESCRIPTION-->
                            <span>Please Browse and select the Excel File for Uploading the Policies</span><!--END DESCRIPTION-->
                        </div>
                        <!--END HEADER-->

                        <!--CONTENT-->
                        <div class="content">
                            <!--FileUpload-->
                            <asp:FileUpload ID="FileUpload1" runat="server" class="input" Width="80%"/>
                        </div>
                        <!--END CONTENT-->

                        <!--FOOTER-->
                        <div class="footer">
                            <!--EXIT BUTTON-->
                            <div style="position:absolute;top:80%;left:60%">
                                <asp:Button type="submit" ID="btnExit" OnClick="btnExit_Click"  runat="server" text="Exit" class="button" /><!--END LOGIN BUTTON-->
                            </div>
                            <!--IMPORT BUTTON-->
                            <div style="position:absolute;top:80%;left:20%">
                                <asp:Button type="submit" ID="btnImport" OnClick="Upload"  runat="server" text="Import Data" class="button" /><!--END LOGIN BUTTON-->
                            </div>
                            <asp:Label ID="lblstatus" runat="server" /><!--END REGISTER BUTTON-->
                        </div>
                        <!--END FOOTER-->
</asp:Content>