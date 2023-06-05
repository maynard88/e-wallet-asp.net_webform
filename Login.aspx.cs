using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace Ewallet
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page or any desired page
                Response.Redirect("HomePage.aspx");
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Value;
            string password = passwordTextBox.Value;

            // Validate the login credentials
            if (ValidateLogin(username, password))
            {
                // Set authentication cookie with persistent option
                FormsAuthentication.SetAuthCookie(username, true);

                // Set the User.Identity.Name
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(username), null);


                // Redirect to home page or any desired page
                Response.Redirect("HomePage.aspx");
            }
            else
            {
                // Display error message
                errorLabel.Text = "Invalid username or password.";
                errorLabel.Visible = true;
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    return false;
                }
            }
        }
    }
}
