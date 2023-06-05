<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Ewallet.HomePage" %>

<!DOCTYPE html>
<html>
<head>
    <title>Home Page</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 600px;
            margin: 50px auto;
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }

        h1 {
            text-align: center;
            color: #333;
            margin-top: 0;
        }

        .data-row {
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
        }

        .label {
            font-weight: bold;
        }

        .value {
            text-align: right;
        }

        .buttons-container {
            margin-top: 20px;
            text-align: center;
        }

        .action-button {
            padding: 10px 20px;
            font-size: 16px;
            background-color: #4caf50;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

            .action-button:hover {
                background-color: #45a049;
            }

        .navigation-container {
            margin-top: 20px;
            text-align: center;
        }

        .hidden {
            display: none;
        }

        .nav-link {
            display: inline-block;
            margin: 0 10px;
            color: #333;
            text-decoration: none;
            font-weight: bold;
            transition: color 0.3s;
        }

            .nav-link:hover {
                color: #555;
            }
    </style>
</head>
<body>
    <form id="homeForm" runat="server">
        <div class="container">
            <h1>Welcome to the Home Page</h1>

            <div class="data-row">
                <span class="label">Account Number:</span>
                <span class="value"><%= AccountNumber %></span>
            </div>

            <div class="data-row">
                <span class="label">Name:</span>
                <span class="value"><%= Name %></span>
            </div>

            <div class="data-row">
                <span class="label">Date Registered:</span>
                <span class="value"><%= DateRegistered %></span>
            </div>

            <div class="data-row">
                <span class="label">Current Balance:</span>
                <span class="value"><%= CurrentBalance.ToString("C", new System.Globalization.CultureInfo("en-PH")) %></span>
            </div>

            <div class="data-row">
                <span class="label">Total Sent Money:</span>
                <span class="value"><%= TotalSentMoney.ToString("C", new System.Globalization.CultureInfo("en-PH")) %></span>
            </div>

            <div class="buttons-container">
                <asp:Button ID="depositButton" runat="server" Text="Deposit" OnClick="DepositButton_Click" CssClass="action-button" />
                <asp:Button ID="withdrawButton" runat="server" Text="Withdraw" OnClick="WithdrawButton_Click" CssClass="action-button" />
                <asp:Button ID="sendMoneyButton" runat="server" Text="Send Money" OnClick="SendMoneyButton_Click" CssClass="action-button" />
                <asp:Button ID="reportButton" runat="server" Text="Report" OnClick="ReportButton_Click" CssClass="action-button" />
            </div>

            <div class="navigation-container">
                <a href="#" class="nav-link" onclick="changePasswordButtonClick()">Change Password</a>
                <asp:Button ID="changePasswordButton" runat="server" CssClass="hidden" OnClick="ChangePasswordButton_Click" />
                <a href="#" class="nav-link" onclick="logoutButtonClick()">Logout</a>
                <asp:Button ID="logoutButton" runat="server" CssClass="hidden" OnClick="LogoutButton_Click" />
            </div>
        </div>
    </form>
    <script>
        function changePasswordButtonClick() {
            document.getElementById('<%= changePasswordButton.ClientID %>').click();
        }
        function logoutButtonClick() {
            document.getElementById('<%= logoutButton.ClientID %>').click();
        }
    </script>
</body>
</html>

