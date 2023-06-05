<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ewallet.Login" %>

<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
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

        .register-link {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="loginForm" runat="server">
        <div class="container">
            <h1>Login</h1>
            <div style="">
                <div>
                    <label for="usernameTextBox">Username:</label>
                    <input type="text" id="usernameTextBox" runat="server" />
                </div>
                <div>
                    <label for="passwordTextBox">Password:</label>
                    <input type="password" id="passwordTextBox" runat="server" />
                </div>
            </div>
            <div class="error">
                <asp:Label ID="errorLabel" runat="server" Visible="false"></asp:Label>
            </div>
            <div style="text-align: center;">
                <input type="submit" value="Login" id="loginButton" runat="server" onserverclick="LoginButton_Click" />
            </div>
            <div class="register-link">
                <br>
                <a href="Registration.aspx">Don't have an account? Register here.</a>
            </div>
        </div>
    </form>
</body>
</html>





