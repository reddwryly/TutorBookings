<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="adminPage.aspx.cs"
    Inherits="TutorBookings.adminPage" %>

<!DOCTYPE html>
<html>
<head>
    <title>Appointments</title>

    <style>
        body { font-family: Arial; }
        table { border-collapse: collapse; width: 60%; }
        th, td { border: 1px solid #ccc; padding: 8px; text-align: left; }
        th { background-color: #eee; }
    </style>
</head>

<body>
    <form runat="server">
        <h2>Appointments</h2>

        <asp:GridView 
            ID="AppointmentsGrid" 
            runat="server" 
            AutoGenerateColumns="false"
            AutoGenerateEditButton="false"
            OnRowEditing="AppointmentsGrid_RowEditing"
            OnRowUpdating="AppointmentsGrid_RowUpdating"
            OnRowCancelingEdit="AppointmentsGrid_RowCancelingEdit"
            DataKeyNames="TutorID,Date,Time,StudentEmail,CourseCode">

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="false" />
                <asp:BoundField DataField="StudentName" HeaderText="Student" ReadOnly="true"/>
                <asp:BoundField DataField="TutorID" HeaderText="Tutor" ReadOnly="false"/>
                <asp:BoundField DataField="CourseCode" HeaderText="Course Code" ReadOnly="false"/>
                <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="false"/>
                <asp:BoundField DataField="Time" HeaderText="Time" ReadOnly="false"/>
                <asp:BoundField DataField="StudentEmail" HeaderText="Student Email" ReadOnly="false"/>

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

        <asp:TextBox ID="TutorInput" runat="server" Placeholder="Tutor Name" />
        <asp:TextBox ID="DateInput" runat="server" Placeholder="Date" />
        <asp:TextBox ID="TimeInput" runat="server" Placeholder="Time" />
        <asp:TextBox ID="StudentEmailInput" runat="server" Placeholder="Student Email" />
        <asp:TextBox ID="CourseCodeInput" runat="server" Placeholder="Course Code" />


        <asp:Button
            ID="AddButton"
            runat="server"
            Text="Add"
            OnClick="AddButton_Click" />

    </form>
</body>
</html>