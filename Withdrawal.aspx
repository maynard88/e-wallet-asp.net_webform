<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Withdrawal.aspx.cs" Inherits="Ewallet.Withdrawal" %>

<!DOCTYPE html>
<html>
<head>
    <title>Withdrawal</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
        }

        .container {
            max-width: 400px;
            margin: 0 auto;
            padding: 20px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        h1 {
            text-align: center;
            margin-bottom: 20px;
        }

        label {
            display: block;
            margin-bottom: 10px;
        }

        input[type="number"] {
            width: 94%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        input[type="submit"] {
            width: 94%;
            padding: 10px;
            background-color: #4CAF50;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

            input[type="submit"]:hover {
                background-color: #45a049;
            }

        .error {
            color: red;
            margin-bottom: 10px;
            text-align: center;
        }

        .home-link {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="withdrawalForm" runat="server">
        <div class="container">
            <h1>Withdrawal</h1>
            <div class="data-row">
                <span class="label">Current Balance:</span>
                <span class="value"><%= CurrentBalance.ToString("C", new System.Globalization.CultureInfo("en-PH")) %></span>                
            </div>
            <div>
                <br>
                <label for="amountTextBox">Enter Amount:</label>
                <input type="number" id="amountTextBox" runat="server" step="0.1"/>
            </div>
            <div class="error">
                <asp:Label ID="errorLabel" runat="server" Visible="false"></asp:Label>
            </div>
            <div style="text-align: center;">
                <input type="submit" value="Withdraw" id="withdrawButton" runat="server" onserverclick="WithdrawButton_Click" />
            </div>
            <div class="home-link">
                <br>
                <a href="HomePage.aspx">Go back to home page.</a>
            </div>
        </div>
    </form>
</body>
</html>
