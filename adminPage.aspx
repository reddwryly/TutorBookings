<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="adminPage.aspx.cs"
    Inherits="TutorBookings.adminPage"
    MasterPageFile="~/Site.Master" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Admin Page</h2>

        <asp:GridView 
            ID="AppointmentsGrid" 
            runat="server" 
            AutoGenerateColumns="false"
            AutoGenerateEditButton="false"
            OnRowEditing="AppointmentsGrid_RowEditing"
            OnRowUpdating="AppointmentsGrid_RowUpdating"
            OnRowCancelingEdit="AppointmentsGrid_RowCancelingEdit"
            DataKeyNames="TutorID,Date,Time,StudentEmail,CourseCode"
            OnRowDataBound="AppointmentsGrid_RowDataBound">

            <Columns>
                <asp:BoundField DataField="StudentName" HeaderText="Student Name" ReadOnly="true"/>
                <asp:TemplateField HeaderText="Tutor Name">
                    <ItemTemplate>
                        <%#Eval("TutorName") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:DropDownList ID="TutorDropDownEdit" runat="server">    </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Course Code">
                    <ItemTemplate>
                        <%#Eval("CourseCode") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox ID="CourseCodeTextBox" runat="server" Text='<%# Bind("CourseCode") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <%#Eval("Date") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox ID="DateTextBox" runat="server" Text='<%# Bind("Date") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Time">
                    <ItemTemplate>
                        <%#Eval("Time") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox ID="TimeTextBox" runat="server" Text='<%# Bind("Time") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Student Email">
                    <ItemTemplate>
                        <%#Eval("StudentEmail") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox ID="StudentEmailTextBox" runat="server" Text='<%# Bind("StudentEmail") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button 
                            ID="DeleteButton"
                            runat="server"
                            Text="Delete"
                            CommandArgument='<%# Eval("Id") %>'
                            OnClick="DeleteButton_Click"
                            OnClientClick="return confirm('Delete this appointment?');" />

                        <asp:Button
                            ID="EditButton"
                            runat="server"
                            Text="Edit"
                            CommandArgument='<%# Eval("ID") %>'
                            CommandName="Edit"
                            OnClick="EditButton_Click" />

                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:Button
                            ID="UpdateButton"
                            runat="server"
                            Text="Update"
                            CommandName="Update" />
                        <asp:Button
                            ID="CancelButton"
                            runat="server"
                            Text="Cancel"
                            CommandName="Cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

        <h3> Add Apppointment</h3>

        <asp:Label ID="TutorLabel" runat="server" Text="Select Tutor: " AssociatedControlID="TutorDropdown"/>
        <br />
        <asp:DropDownList ID="TutorDropdown" runat="server" />
        <asp:TextBox ID="DateInput" runat="server" Placeholder="Date" />
        <asp:TextBox ID="TimeInput" runat="server" Placeholder="Time" />
        <asp:TextBox ID="StudentEmailInput" runat="server" Placeholder="Student Email" />
        <asp:TextBox ID="CourseCodeInput" runat="server" Placeholder="Course Code" />


        <asp:Button
            ID="AddButton"
            runat="server"
            Text="Add"
            OnClick="AddButton_Click" />
    </asp:Content>
