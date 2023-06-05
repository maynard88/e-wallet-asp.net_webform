using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Ewallet
{
    public partial class Deposit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!User.Identity.IsAuthenticated)
            {

                // Redirect to the login page or handle unauthenticated users
                Response.Redirect("Login.aspx");
            }

        }


        protected void DepositButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(amountTextBox.Value)) {
                errorLabel.Text = "Invalid amount. Please enter an amount between 100.00 and 10,000.00 that is divisible by 100.00.";
                errorLabel.Visible = true;
                return;
            }

            decimal amount = Convert.ToDecimal(amountTextBox.Value);

            // Get the logged-in user ID from the session or any other authentication mechanism
            int userID = GetLoggedInUserID();

            if (userID > 0)
            {
                InsertDeposit(userID, amount, DateTime.Now);

                if (errorLabel.Visible)
                {
                    return;
                }
                else
                {                  
                    Response.Redirect("HomePage.aspx"); // Redirect to the HomePage or any other page after successful deposit
                }
            }
            else
            {
                errorLabel.Text = "User not logged in.";
                errorLabel.Visible = true;
            }
        }

        private void InsertDeposit(int userID, decimal amount, DateTime depositDate)
        {
            if (amount < 100.00m || amount > 10000.00m || amount % 100.00m != 0)
            {
                // Display an error message indicating the invalid amount
                // You can use a label control or any other mechanism to show the error message to the user
                errorLabel.Text = "Invalid amount. Please enter an amount between 100.00 and 10,000.00 that is divisible by 100.00.";
                errorLabel.Visible = true;
                return;
            }

            decimal currentBalance = GetCurrentBalance(userID);
            decimal totalBalanceAfterDeposit = currentBalance + amount;

            if (totalBalanceAfterDeposit > 50000.00m)
            {
                // Display an error message indicating the exceeding balance limit
                // You can use a label control or any other mechanism to show the error message to the user
                errorLabel.Text = "Deposit failed. The total balance after the deposit would exceed the limit of 50,000.00.";
                errorLabel.Visible = true;
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Insert the deposit record
                string depositQuery = "INSERT INTO Deposits (UserID, Amount, DepositDate) VALUES (@UserID, @Amount, @DepositDate)";
                SqlCommand depositCommand = new SqlCommand(depositQuery, connection);
                depositCommand.Parameters.AddWithValue("@UserID", userID);
                depositCommand.Parameters.AddWithValue("@Amount", amount);
                depositCommand.Parameters.AddWithValue("@DepositDate", depositDate);

                // Update the account balance
                string updateQuery = "UPDATE Accounts SET Balance = Balance + @Amount WHERE UserID = @UserID";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@UserID", userID);
                updateCommand.Parameters.AddWithValue("@Amount", amount);

                try
                {
                    connection.Open();
                    // Start a database transaction
                    SqlTransaction transaction = connection.BeginTransaction();
                    depositCommand.Transaction = transaction;
                    updateCommand.Transaction = transaction;

                    try
                    {
                        // Execute the deposit query
                        depositCommand.ExecuteNonQuery();

                        // Execute the update query
                        updateCommand.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        connection.Close();
                        // Handle the exception or display an error message
                        errorLabel.Text = "Something went wrong!";
                        errorLabel.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    connection.Close();
                    throw;
                }
            }
        }

        private decimal GetCurrentBalance(int userID)
        {
            decimal currentBalance = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Balance FROM Accounts WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        currentBalance = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    throw;
                }
            }

            return currentBalance;
        }


        private int GetLoggedInUserID()
        {
            int userID = 0;

            string username = User.Identity.Name;

            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID FROM Users WHERE UserName = @UserName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", username);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userID = Convert.ToInt32(result);
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    throw;
                }
            }

            return userID;
        }

    }
}

