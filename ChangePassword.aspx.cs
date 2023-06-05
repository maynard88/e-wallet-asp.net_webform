using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Ewallet
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            errorLabel.Visible = false;
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            oldPassword.Value = string.Empty;
            newPassword.Value = string.Empty;
            confirmNewPassword.Value = string.Empty;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();
            Response.Redirect("ErrorPage.aspx");
        }
        

        protected void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

            // Authentication set up
            string username = User.Identity.Name; 

            string oldPasswordValue = oldPassword.Value;
            string newPasswordValue = newPassword.Value;
            string confirmNewPasswordValue = confirmNewPassword.Value;

            // Validate if the new password is different from the existing password
            if (newPasswordValue == confirmNewPasswordValue)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the old password matches the current user's password
                    string query = "SELECT Password FROM Users WHERE UserName = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    string currentPassword = command.ExecuteScalar()?.ToString();

                    if (currentPassword == oldPasswordValue)
                    {

                        if (newPasswordValue == oldPasswordValue)
                        {
                            // Display an error message
                            errorLabel.Visible = true;
                            errorLabel.Text = "The new password must not be the same as the old password.";
                        }
                        else
                        {

                            // Update the user's password
                            query = "UPDATE Users SET Password = @NewPassword WHERE UserName = @Username";
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@NewPassword", newPasswordValue);
                            command.Parameters.AddWithValue("@Username", username);

                            command.ExecuteNonQuery();
                            connection.Close();

                            // Redirect to a success page
                            Response.Redirect("ChangePasswordSuccess.aspx");
                        }
                    }
                    else
                    {
                        // Display an error message if the old password is incorrect
                        errorLabel.Visible = true;
                        errorLabel.Text = "The old password is incorrect.";
                    }

                    connection.Close();
                }
            }
            else
            {
                // Display an error message if the new passwords do not match
                errorLabel.Visible = true;
                errorLabel.Text = "The new passwords do not match.";
            }
        }
    }
}

