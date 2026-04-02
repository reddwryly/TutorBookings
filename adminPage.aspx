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
            AutoGenerateColumns="false">

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="StudentName" HeaderText="Student" />
                <asp:BoundField DataField="TutorName" HeaderText="Tutor" />
                <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                <asp:BoundField DataField="Date" HeaderText="Date" />
                <asp:BoundField DataField="Time" HeaderText="Time" />
                <asp:BoundField DataField="StudentEmail" HeaderText="Student Email" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button 
                            ID="DeleteButton"
                            runat="server"
                            Text="Delete"
                            CommandArgument='<%# Eval("Id") %>'
                            OnClick="DeleteButton_Click"
                            OnClientClick="return confirm('Delete this appointment?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </form>
</body>
</html>