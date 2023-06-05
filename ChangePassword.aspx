<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Ewallet.ChangePassword" %>

<!DOCTYPE html>
<html>
<head>
    <title>Change Password</title>
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

        h2 {
            text-align: center;
            margin-bottom: 20px;
        }

        .home-link {
            text-align: center;
        }

        label {
            display: block;
            margin-bottom: 10px;
        }

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
    </style>
</head>
<body>
    <form id="changePasswordForm" runat="server">
        <div class="container">
            <h2>Change Password</h2>
            <div>
                <label for="oldPassword">Old Password:</label>
                <input type="password" id="oldPassword" runat="server" />
            </div>
            <div>
                <label for="newPassword">New Password:</label>
                <input type="password" id="newPassword" runat="server" />
            </div>
            <div>
                <label for="confirmNewPassword">Confirm New Password:</label>
                <input type="password" id="confirmNewPassword" runat="server" />
            </div>
            <div class="error">
                <asp:Label ID="errorLabel" runat="server" Visible="false"></asp:Label>
            </div>
            <div style="text-align: center;">
                <input type="submit" value="Change Password" id="changePasswordButton" runat="server" onserverclick="ChangePasswordButton_Click" />
            </div>
            <div class="home-link">
                <br>
                <a href="HomePage.aspx">Go back to home page.</a>
            </div>
        </div>
    </form>
</body>
</html>
