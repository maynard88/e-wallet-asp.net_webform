<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMoney.aspx.cs" Inherits="Ewallet.SendMoney" %>

<!DOCTYPE html>
<html>
<head>
    <title>Send Money</title>
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

        input[type="text"],
        input[type="password"] {
            width: 94%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        input[type="number"] {
            width: 94%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        #sendButton, input[type="submit"] {
            width: 100%;
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
    <form id="form1" runat="server">
        <div class="container">
            <h1>Send Money</h1>
            <div class="data-row">
                <span class="label">Current Balance:</span>
                <span class="value"><%= CurrentBalance.ToString("C", new System.Globalization.CultureInfo("en-PH")) %></span>
            </div>
            <p></p>
            <div>
                <label for="accountNumberTextBox">Recipient's Account Number:</label>
                <input type="number" id="accountNumberTextBox" runat="server" step="0.01" />
                <span class="error" id="accountNumberValidator" style="display: none;">Recipient's Account Number is required.</span>
                <button type="button" id="validateAccountButton" class="btn-validate" runat="server" onserverclick="ValidateAccountButton_Click">Validate Recipient Account</button>
                <p></p>          
                <asp:Label ID="recipientNameLabel" runat="server" Visible="false"></asp:Label>
                <p></p>
                <asp:Label ID="recipientAccountNumberLabel" runat="server" Visible="false"></asp:Label>

                <br />
                <p></p>
                <label for="amountTextBox">Amount:</label>
                <input type="number" id="amountTextBox" runat="server" step="0.1" />
                <span class="error" id="amountValidator" style="display: none;">Amount is required.</span>
                <span class="error" id="amountFormatValidator" style="display: none;">Invalid amount format. Please enter a valid number.</span>
                <label for="passwordTextBox">Password:</label>
                <input type="password" id="passwordTextBox" />
                <span class="error" id="passwordValidator" style="display: none;">Password is required.</span>

                <button type="submit" id="sendButton"  runat="server" class="btn-send" onserverclick="SendMoneyButton_Click">Send</button>
                <p></p>
                <div class="error">
                    <asp:Label ID="errorLabel" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="home-link">
                <br>
                <a href="HomePage.aspx">Go back to home page.</a>
            </div>
        </div>
    </form>
</body>
</html>

