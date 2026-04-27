<%@ Page Title="Book Tutoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BookTutoring.aspx.cs" Inherits="TutorBookings.BookTutoring" UnobtrusiveValidationMode="None" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="Main">
            <!--Dynamic Content-->
            <fieldset class="bookformfeildset">

                <div class="bookformdiv">
                    <table class="bookform">
                        <tr>
                            <td></td>
                            <td></td>
                             <td></td>
                            <td colspan="5" rowspan="6" style="padding-left:50px">
                                <asp:Calendar runat="server" ID="Date" OnDayRender="LoadDates" OnSelectionChanged="DateSelected" autopostback="true" CssClass="cal"></asp:Calendar>
                                <asp:CustomValidator ID="cvDate" runat="server"
                                    ErrorMessage="Select a Date *"
                                    ForeColor="Red"
                                    OnServerValidate="cvDate_Validation">
                                </asp:CustomValidator>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                           <td style="text-align: right;">
                                <div>Tutor:</div> 
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="TutorDDL" OnSelectedIndexChanged="TutorSelected" AutoPostBack="true">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvTutor" runat="server"
                                    ControlToValidate="TutorDDL"
                                    ErrorMessage="*"
                                    ForeColor="Red"
                                    OnServerValidate="cvTutor_Validation">
                                </asp:CustomValidator>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <dv>Course:</dv> 
                            </td>
                            <td colspan="2">
                                <asp:DropDownList runat="server" ID="CourseDDL" OnSelectedIndexChanged="CourseSelected" AutoPostBack="True" >
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvCourse" runat="server"
                                    ControlToValidate="CourseDDL"
                                    ErrorMessage="*"
                                    ForeColor="Red"
                                    OnServerValidate="cvCourse_Validation">
                                </asp:CustomValidator>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <div>Time:</div>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="TimeDDL" AutoPostBack="true" Enabled="false">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvTime" runat="server"
                                    ControlToValidate="TimeDDL"
                                    ErrorMessage="*"
                                    ForeColor="Red"
                                    OnServerValidate="cvTime_Validation">
                                </asp:CustomValidator>

                            </td>
                            <td></td>
                        </tr>
                        <tr></tr>
                        <tr></tr>
                        <tr>
                            <td style="text-align: right;" class="td2">
                                <div>First:</div>
                            </td>
                            <td class="td2">
                                <asp:TextBox runat="server" ID="FName" MaxLength="50" TextMode="SingleLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="FName" ErrorMessage="*" ForeColor="Red" />
                            </td>
                            <td style="text-align: right;" class="td2">
                                <div>Last:</div>
                            </td>
                            <td class="td2">
                                <asp:TextBox runat="server" ID="LName" MaxLength="50" TextMode="SingleLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="LName" ErrorMessage="*" ForeColor="Red" />
                            </td>
                            <td style="text-align: right;" class="td2">
                                <div>Email:</div>
                            </td>
                            <td class="td2">
                                <asp:Textbox runat="server" ID="Email" MaxLength="50" TextMode="SingleLine"></asp:Textbox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="Email" ErrorMessage="*" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align:right;">
                                <asp:Button runat="server" ID="Submit" Text="Book!" OnClick="SubmitButton" />
                            </td>
                        </tr>
                    </table>
                    <h4><asp:Label runat="server" ID="confirmed" cssclass="label"></asp:Label></h4>
                </div>
            </fieldset>
        </div>
    </main>

</asp:Content>
