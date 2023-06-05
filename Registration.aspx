<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Ewallet.Registration" %>

<!DOCTYPE html>
<html>
<head>
    <title>User Registration</title>
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

        input[type="submit"] {
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
        }
    </style>
</head>
<body>
    <form id="registrationForm" runat="server">
        <div class="container">
            <h1>User Registration</h1>
            <div>
                <label for="firstNameTextBox">First Name:</label>
                <input type="text" id="firstNameTextBox" runat="server" />
            </div>
            <div>
                <label for="lastNameTextBox">Last Name:</label>
                <input type="text" id="lastNameTextBox" runat="server" />
            </div>
            <div>
                <label for="usernameTextBox">Username:</label>
                <input type="text" id="usernameTextBox" runat="server" />
                <asp:Label ID="usernameErrorLabel" runat="server" Visible="false" ForeColor="Red"></asp:Label>
            </div>
            <div>
                <label for="passwordTextBox">Password:</label>
                <input type="password" id="passwordTextBox" runat="server" />
            </div>
            <div>
                <input type="submit" value="Register" id="registerButton" runat="server" onserverclick="RegisterButton_Click" />
            </div>
            <div class="login-link">
                <br>
                <a href="login.aspx">Have an account? Login here.</a>
            </div>
        </div>
    </form>
</body>
</html>
