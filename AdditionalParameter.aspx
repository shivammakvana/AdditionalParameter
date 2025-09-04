<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdditionalParameter.aspx.cs" Inherits="YourNamespace.AdditionalParameter" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Additional Parameters</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f7f7f7;
        }
        h2 { color: deepskyblue; }
        .tabs { margin-bottom: 20px; }
        .tabs a {
            text-decoration: none;
            margin-right: 10px;
            font-weight: bold;
        }
        .form-group { margin-bottom: 10px;
            text-align: center;
        }
        label { display: inline-block;
            text-align: left;
        }
        input[type="text"], select {
            width: 250px;
            padding: 5px;
        }
        .btn { margin-right: 8px; }
        .gridview { margin-top: 20px; text-align: left; margin-right: 1px; }
        .gridview th {
            background-color: #87CEEB;
            color: white;
            padding: 8px;
        }
        .gridview td { padding: 8px; }
        .panel {
            padding: 20px;
            background-color: white;
            border: 1px solid #ddd;
            margin-bottom: 20px;
        }
        .gridview {
        margin-top: 20px;
        border-collapse: collapse;
        box-shadow: 0 2px 6px rgba(0,0,0,0.15);
        background-color: white;
        }
        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: center;
        }
        .auto-style1 {
            margin-bottom: 10px;
            text-align: center;
        }
        .auto-style2 {
            text-align: center;
        }
        .auto-style4 {
            margin-bottom: 10px;
            text-align: left;
        }
        .auto-style5 {
            text-align: left;
        }
        .auto-style6 {
            margin-bottom: 20px;
            text-align: left;
        }
        #form1 {
            text-align: right;
        }
    </style>
</head>
<body>
    <div class="auto-style2">
    <form id="form1" runat="server">
        <h2 class="auto-style5">Additional Parameters</h2>

        <div class="auto-style6">
            <asp:LinkButton ID="linkbtn1" runat="server" CssClass="btn" OnClick="linkbtn1_Click">Student Additional Field Group</asp:LinkButton>
            <asp:LinkButton ID="linkbtn2" runat="server" CssClass="btn" OnClick="linkbtn2_Click">Student Additional Parameter</asp:LinkButton>
        </div>

        <br />
<a href="StudentRegistration.aspx" class="button">Student Registration</a>


        <!-- group_panel -->
        <asp:Panel ID="pnlGroup" runat="server" CssClass="panel" Visible="true">
            <div style="display:flex; justify-content:center;">
                <asp:GridView ID="gvGroup" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    DataKeyNames="GroupID" OnRowCommand="gvGroup_RowCommand" Width="600px">
                    <Columns>
                        <asp:BoundField HeaderText="Group Name" DataField="GroupName" />
                        <asp:BoundField HeaderText="Group Order" DataField="GroupOrder" />
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:Button ID="btnEditGroup" runat="server" Text="Edit" 
                                    CommandName="EditRow" CommandArgument="<%# Container.DataItemIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="auto-style2">
                <br />
            </div>
            <div class="auto-style1">
                <label>
                Group Name:</label>
                <asp:TextBox ID="txtGroupName" runat="server" />
                &nbsp;
                <label>
                Group Order:</label>
                <asp:TextBox ID="txtGroupOrder" runat="server" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Style="background-color: #28a745; color: white; padding: 8px 15px; border: none; border-radius: 5px; margin-right: 8px;" Text="Submit" />
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this record?');" Style="background-color: #dc3545; color: white; padding: 8px 15px; border: none; border-radius: 5px; margin-right: 8px;" Text="Delete" />
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return confirm('Are you sure you want to update?');" Style="background-color: #007bff; color: white; padding: 8px 15px; border: none; border-radius: 5px; margin-right: 8px;" Text="Update" />
                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Style="background-color: #ffc107; color: black; padding: 8px 15px; border: none; border-radius: 5px; margin-right: 8px;" Text="Clear" />
            </div>
        </asp:Panel>

        <!-- para_panel -->
        <asp:Panel ID="pnlParameter" runat="server" CssClass="panel">

            <div class="auto-style5">
                <p style="color: red;">
                    Note: Only AlphaNumeric characters, space, question mark(?) and hyphen(-) and (.) is allowed in additional parameter name.
                </p>
                <div class="form-group">
                    <label>
                    <div class="auto-style2"> Additional Parameter Name:</div>
                    </label>
                    &nbsp;<asp:TextBox ID="txtAddParaName" runat="server" CssClass="form-control" Style="width:250px;height:35px;" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<label>Parameter Type:</label>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlParaType" runat="server" AutoPostBack="true" CssClass="form-select" OnSelectedIndexChanged="ddlParaType_SelectedIndexChanged" Style="width:250px;height:35px;" />
                    &nbsp;&nbsp;&nbsp;
                    <label>
                    Order:&nbsp;
                    </label>
                    &nbsp;
                    <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" Style="width:250px;height:35px;" />
                </div>

                <div class="auto-style4">
                    <div class="auto-style2">
                        <asp:ListBox ID="lstDdOption" runat="server" CssClass="form-control" Style="width:250px;height:40px;" Visible="false"></asp:ListBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtDdOption" runat="server" CssClass="form-control mb-2" Style="width:250px;height:35px;" Visible="false"></asp:TextBox>
                        <br />
                        <br />
                    </div>
                    <div class="auto-style2" style="margin-top:10px;">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAddOption" runat="server" OnClick="btnAddOption_Click" Style="background-color: #28a745; color: white; padding: 8px 15px;
                           border: none; border-radius: 5px; margin-right: 8px;" Text="Add" Visible="false" />
                        <asp:Button ID="btnUpdateOption" runat="server" OnClick="btnUpdateOption_Click" Style="background-color: #007bff; color: white; padding: 8px 15px;
                           border: none; border-radius: 5px; margin-right: 8px;" Text="Update" Visible="false" />
                        <asp:Button ID="btnDeleteOption" runat="server" OnClick="btnDeleteOption_Click" Style="background-color: #dc3545; color: white; padding: 8px 15px;
                           border: none; border-radius: 5px; margin-right: 8px;" Text="Delete" Visible="false" />
                        <asp:Button ID="btnClearOption" runat="server" OnClick="btnClearOption_Click" Style="background-color: #ffc107; color: black; padding: 8px 15px;
                           border: none; border-radius: 5px; margin-right: 8px;" Text="Clear" Visible="false" />
                    </div>
                </div>
                <div class="auto-style4">
                </div>
                <div class="form-group">
                    <asp:CheckBox ID="cbCompulsory" runat="server" Text="Is Compulsory" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="cbHide" runat="server" Text="Hide From Parents" />
                </div>
                <div class="form-group">
                    <label>
                    Default Value:&nbsp;&nbsp;
                    </label>
                    <asp:TextBox ID="txtDefValue" runat="server" CssClass="form-control" Style="width:250px;height:35px;" />
                    &nbsp;&nbsp;&nbsp;
                    <label>
                    Group:</label>&nbsp;
                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select" Style="width:250px;height:35px;" />
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="lnkShowGreeting" runat="server" OnClick="lnkShowGreeting_Click" Style=" background-color: #dc3545; cursor: pointer;">
                    Do you want to associate greeting with it?</asp:LinkButton>
                    <br />
                    <asp:TextBox ID="txtGreeting" runat="server" CssClass="form-control" Style="width:250px;height:60px;" TextMode="MultiLine" Visible="false" />
                </div>
             <div class="form-group">
                <asp:Button ID="btnParamSubmit" runat="server" OnClick="btnParamSubmit_Click"
                    Style="background-color: #28a745; color: white; padding: 8px 15px; 
                    border: none; border-radius: 5px; margin-right: 8px;" 
                    Text="Submit" />

                <asp:Button ID="btnParamUpdate" runat="server" OnClick="btnParamUpdate_Click"
                    Style="background-color: #007bff; color: white; padding: 8px 15px; 
                    border: none; border-radius: 5px; margin-right: 8px;" 
                    Text="Update" Visible="false"
                    OnClientClick="return confirm('Are you sure you want to update this parameter?');" />

                <asp:Button ID="btnParamDelete" runat="server" OnClick="btnParamDelete_Click"
                    Style="background-color: #dc3545; color: white; padding: 8px 15px; 
                    border: none; border-radius: 5px; margin-right: 8px;" 
                    Text="Delete" Visible="false"
                    OnClientClick="return confirm('Are you sure you want to delete this parameter? This action cannot be undone.');" />

                <asp:Button ID="btnParamClear" runat="server" OnClick="btnParamClear_Click"
                    Style="background-color: #ffc107; color: black; padding: 8px 15px; 
                    border: none; border-radius: 5px; margin-right: 8px;" 
                    Text="Clear" />

                 <br />
                 <br />
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" style="text-align: left"></asp:Label>
            </div>
                &nbsp;<div style="display:flex; width:100%; justify-content:center; margin-top:20px;">
                    <asp:GridView ID="gvParameter" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="ParameterID,GroupID" OnRowCommand="gvParameter_RowCommand" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="ParameterName" HeaderText="Parameter Name" />
                            <asp:BoundField DataField="ParameterType" HeaderText="Parameter Type" />
                            <asp:BoundField DataField="IsCompulsory" HeaderText="Is Compulsory" />
                            <asp:BoundField DataField="DefaultValue" HeaderText="Default Value" />
                            <asp:BoundField DataField="ParentHide" HeaderText="Parent Hide" />
                            <asp:BoundField DataField="GreetingText" HeaderText="Greeting Text" />
                            <asp:BoundField DataField="Order" HeaderText="Order" />
                            <asp:BoundField DataField="GroupName" HeaderText="Group" />
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:Button ID="btnEditParameter" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditRow" Text="Edit" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

    </asp:Panel>
    </form>
    </div>
</body>
</html>
