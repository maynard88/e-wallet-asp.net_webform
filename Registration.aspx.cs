using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Ewallet
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameTextBox.Value;
            string lastName = lastNameTextBox.Value;
            string username = usernameTextBox.Value;
            string password = passwordTextBox.Value;

            if (IsUsernameAvailable(username))
            {
                InsertUser(firstName, lastName, username, password);
                Response.Redirect("Login.aspx"); // Redirect to the login page after successful registration
            }
            else
            {
                usernameErrorLabel.Text = "Username already exists.";
                usernameErrorLabel.Visible = true;
            }
        }

        private bool IsUsernameAvailable(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", username);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count == 0;
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    return false;
                }
            }
        }

        private void InsertUser(string firstName, string lastName, string username, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (FirstName, LastName, UserName, Password, DateRegistered) VALUES (@FirstName, @LastName, @UserName, @Password, @DateRegistered); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@DateRegistered", DateTime.Now);

                try
                {
                    connection.Open();
                    int newUserId = Convert.ToInt32(command.ExecuteScalar());

                    // Insert data into the Accounts table
                    string accountNumber = GenerateAccountNumber(); // Replace with your logic to generate an account number
                    decimal initialBalance = 0; // Adjust the initial balance as needed
                    string insertAccountQuery = "INSERT INTO Accounts (AccountID, UserID, AccountNumber, Balance) VALUES (@AccountID, @UserID, @AccountNumber, @Balance)";
                    SqlCommand insertAccountCommand = new SqlCommand(insertAccountQuery, connection);
                    insertAccountCommand.Parameters.AddWithValue("@AccountID", newUserId); // Use the newly generated User ID as the AccountID
                    insertAccountCommand.Parameters.AddWithValue("@UserID", newUserId);
                    insertAccountCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    insertAccountCommand.Parameters.AddWithValue("@Balance", initialBalance);
                    insertAccountCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    connection.Close();
                }
            }
        }
        

        private string GenerateAccountNumber()
        {        
            // Generate a random account number and check for uniqueness          
            // Generate a new account number using random characters from the 'chars' string
            // Example only but don't do this on actual work.
            const int numberLength = 16;

            Random random = new Random();
            string randomNumber = "";

            for (int i = 0; i < numberLength; i++)
            {
                int digit = random.Next(10);
                randomNumber += digit.ToString();
            }

            return randomNumber;
        }

    }
}



