using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Ewallet
{
    public partial class SendMoney : System.Web.UI.Page
    {
        public decimal CurrentBalance { get; set; }

        public string RecipientName { get; set; }

        public string RecipientAccountNumber { get; set; }

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


        protected void ValidateAccountButton_Click(object sender, EventArgs e)
        {
            string accountNumber = accountNumberTextBox.Value.Trim();

            // always get current user data
            int userId = GetLoggedInUserID();
            CurrentBalance = GetCurrentBalance(userId);

            // Reset error messages      
            errorLabel.Visible = false;

            // Check if account number is provided
            if (string.IsNullOrEmpty(accountNumber))
            {
                errorLabel.Text = "Invalid account.";
                errorLabel.Visible = true;
                return;
            }

            // Perform account validation using your server-side logic     

            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT top 1 FirstName, LastName, AccountNumber FROM [Accounts] a
                                     inner join  Users u on a.UserID = u.UserID 
                                     where AccountNumber = @AccountNumber";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        RecipientAccountNumber = "Account Number: " + reader["AccountNumber"].ToString();
                        RecipientName = "Account Name: " + reader["FirstName"].ToString() + " " + reader["LastName"].ToString();

                        recipientNameLabel.Text = RecipientName;
                        recipientNameLabel.Visible = true;

                        recipientAccountNumberLabel.Text = RecipientAccountNumber;
                        recipientAccountNumberLabel.Visible = true;

                    }
                    else
                    {

                        // Handle the exception or display an error message
                        errorLabel.Text = "Failed to validate the account. Please try again.";
                        errorLabel.Visible = true;
                    }

                    reader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message
                    errorLabel.Text = "Failed to validate the account. Please try again.";
                    errorLabel.Visible = true;
                }
            }
        }


        protected void SendMoneyButton_Click(object sender, EventArgs e)
        {
            decimal amount = decimal.Parse(amountTextBox.Value);
            int userId = GetLoggedInUserID();

            if (userId > 0)
            {
                // Get current balance
                CurrentBalance = GetCurrentBalance(userId);
                string accountNumber = accountNumberTextBox.Value.Trim();

                PerformSendMoney(userId, amount, accountNumber);

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

        private void PerformSendMoney(int userId, decimal amount, string accountNumber)
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
                string getReceiverUserIDQuery = "select UserID from Accounts where AccountNumber = @AccountNumber";
                string sendMoneyQuery = "INSERT INTO SendMoney (SenderUserID, ReceiverUserID, Amount, SendDate) VALUES (@SenderUserID, @ReceiverUserID, @Amount, @SendDate)";
                string updateBalanceQuery = @"UPDATE Accounts SET Balance = Balance - @Amount WHERE UserID = @SenderUserID
                                              UPDATE Accounts SET Balance = Balance + @Amount WHERE UserID = @ReceiverUserID";

                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    int receiverUserId = 0;

                    // get receiver account number
                    SqlCommand getReceiverUserIDCommand = new SqlCommand(getReceiverUserIDQuery, connection, transaction);
                    getReceiverUserIDCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    SqlDataReader reader = getReceiverUserIDCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        // update value
                        receiverUserId = Int32.TryParse(reader["UserID"].ToString(), out receiverUserId) ? receiverUserId : 0;
                    }

                    reader.Close();

                    // additional check
                    if (receiverUserId == 0) {
                        errorLabel.Text = "Failed to send money. Please try again.";
                        errorLabel.Visible = true;
                    }
                    

                    // Insert withdrawal record
                    SqlCommand sendCommand = new SqlCommand(sendMoneyQuery, connection, transaction);
                    sendCommand.Parameters.AddWithValue("@SenderUserID", userId);
                    sendCommand.Parameters.AddWithValue("@ReceiverUserID", receiverUserId);
                    sendCommand.Parameters.AddWithValue("@Amount", amount);
                    sendCommand.Parameters.AddWithValue("@SendDate", DateTime.Now);
                    sendCommand.ExecuteNonQuery();

                    // Update account balance
                    SqlCommand updateBalanceCommand = new SqlCommand(updateBalanceQuery, connection, transaction);
                    updateBalanceCommand.Parameters.AddWithValue("@SenderUserID", userId);
                    updateBalanceCommand.Parameters.AddWithValue("@ReceiverUserID", receiverUserId);
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

                    errorLabel.Text = "Failed to send money. Please try again.";
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
