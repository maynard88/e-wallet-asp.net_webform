using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Ewallet
{
    public partial class Withdrawal : System.Web.UI.Page
    {
        public decimal CurrentBalance { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to the login page or handle unauthenticated users
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                // Display the currently signed-in user's total current balance
                int userId = GetLoggedInUserID();
                CurrentBalance = GetCurrentBalance(userId);
            }
        }

        protected void WithdrawButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(amountTextBox.Value))
            {
                errorLabel.Text = "Invalid amount. Please enter an amount between 100.00 and 10,000.00 that is divisible by 100.00.";
                errorLabel.Visible = true;
                return;
            }


            decimal amount = decimal.Parse(amountTextBox.Value);
            int userId = GetLoggedInUserID();

            if (userId > 0)
            {
                // Get current balance
                CurrentBalance = GetCurrentBalance(userId);

                PerformWithdrawal(userId, amount);

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

        private void PerformWithdrawal(int userId, decimal amount)
        {
            if (amount < 100.00m || amount > 10000.00m || amount % 100.00m != 0)
            {
                // Display an error message indicating the invalid amount
                // You can use a label control or any other mechanism to show the error message to the user
                errorLabel.Text = "Invalid amount. Please enter an amount between 100.00 and 10,000.00 that is divisible by 100.00.";
                errorLabel.Visible = true;
                return;
            }

            decimal currentBalance = GetCurrentBalance(userId);
            if (amount > currentBalance)
            {
                errorLabel.Text = "Insufficient funds. Please enter a lower amount.";
                errorLabel.Visible = true;
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string withdrawalQuery = "INSERT INTO Withdrawals (UserID, Amount, WithdrawalDate) VALUES (@UserID, @Amount, @WithdrawalDate)";
                string updateBalanceQuery = "UPDATE Accounts SET Balance = Balance - @Amount WHERE UserID = @UserID";

                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    // Insert withdrawal record
                    SqlCommand withdrawalCommand = new SqlCommand(withdrawalQuery, connection, transaction);
                    withdrawalCommand.Parameters.AddWithValue("@UserID", userId);
                    withdrawalCommand.Parameters.AddWithValue("@Amount", amount);
                    withdrawalCommand.Parameters.AddWithValue("@WithdrawalDate", DateTime.Now);
                    withdrawalCommand.ExecuteNonQuery();

                    // Update account balance
                    SqlCommand updateBalanceCommand = new SqlCommand(updateBalanceQuery, connection, transaction);
                    updateBalanceCommand.Parameters.AddWithValue("@UserID", userId);
                    updateBalanceCommand.Parameters.AddWithValue("@Amount", amount);
                    updateBalanceCommand.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();               
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    errorLabel.Text = "Failed to perform the withdrawal. Please try again.";
                    errorLabel.Visible = true;
                }
            }
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

        private decimal GetCurrentBalance(int userId)
        {
            // Implement the logic to fetch the current balance of the user's account from the Accounts table
            // based on the provided UserID
            decimal currentBalance = 0;

            // Implement the logic here to fetch the current balance from the Accounts table
            // Example:
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Balance FROM Accounts WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                connection.Open();
                currentBalance = (decimal)command.ExecuteScalar();
                connection.Close();
            }

            return currentBalance;
        }
    }
}
