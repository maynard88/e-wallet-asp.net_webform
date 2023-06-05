using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;


namespace Ewallet
{
    public partial class HomePage : System.Web.UI.Page
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string DateRegistered { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalSentMoney { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!User.Identity.IsAuthenticated)
            {

                // Redirect to the login page or handle unauthenticated users
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                // Access the user identity
                string username = User.Identity.Name;


                // Retrieve data from the database and assign it to the variables
                string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @" SELECT 
                            a.UserID, FirstName, LastName, AccountNumber, Balance, DateRegistered  FROM Users u
                         inner join  [Accounts] a on u.UserID = a.UserID
                         where UserName = @UserName";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserName", User.Identity.Name);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            AccountNumber = reader["AccountNumber"].ToString();
                            Name = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                            DateRegistered = ((DateTime)reader["DateRegistered"]).ToString("MM/dd/yyyy");
                            CurrentBalance = decimal.TryParse(reader["Balance"].ToString(), out decimal balance) ? balance : 0;
                            TotalSentMoney = 0;
                        }
                        reader.Close();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle
                        // Handle the exception or display an error message
                        connection.Close();
                        throw;
                    }
                }
            }
        }

        protected void DepositButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Deposit.aspx");
        }

        protected void WithdrawButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Withdrawal.aspx");
        }

        protected void SendMoneyButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("SendMoney.aspx");
        }

        protected void ReportButton_Click(object sender, EventArgs e)
        {

        }

        protected void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx");
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Clear authentication cookie
            FormsAuthentication.SignOut();

            // Clear session and user claims
            Session.Clear();
            HttpContext.Current.User = null;
            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(string.Empty), null);

            // Redirect to the login page
            Response.Redirect("Login.aspx");
        }

    }


}
