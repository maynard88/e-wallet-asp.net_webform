<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Deposit.aspx.cs" Inherits="Ewallet.Deposit" %>

<!DOCTYPE html>
<html>
<head>
    <title>Deposit</title>
    <style>
        /* CSS styles from your login page */
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
    <form id="depositForm" runat="server">
        <div class="container">
            <h1>Deposit</h1>
            <div>
                <label for="amountTextBox">Amount:</label>
                <input type="number" id="amountTextBox" runat="server" step="0.01" />
            </div>
            <div class="error">
                <asp:Label ID="errorLabel" runat="server" Visible="false"></asp:Label>
            </div>
            <div style="text-align: center;">
                <input type="submit" value="Deposit" id="depositButton" runat="server" onserverclick="DepositButton_Click" />
            </div>
            <div class="home-link">
                <br>
                <a href="HomePage.aspx">Go back to home page.</a>
            </div>
        </div>
    </form>
</body>
</html>
