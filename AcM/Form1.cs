using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace AcM
{
    public partial class Welcome_Form : Form
    {
        private SqlConnection cnx;
        public Welcome_Form()
        {
            InitializeComponent();
        }
        
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            string email = email_txt.Text;
            string password = password_txt.Text;
            if (!ValidateEmail(email))
            {
                MessageBox.Show("Verify Email Structure", "Wrong Email Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!ValidatePassword(password))
            {
                MessageBox.Show("Password Must Contain at least 1 lower/Upper Case & Digit & special Character & Length >= 8", "Wrong Password Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE email = @email and password = @password", cnx);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password", password);

            SqlDataReader rdr = cmd.ExecuteReader();
            if (!rdr.HasRows)
            {
                MessageBox.Show("User doesn't Exist", "Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rdr.Close();
                return;
            }
            // Read the id value if the user exists
            int LoggedInUserId = 0; // Assuming id is an integer
            if (rdr.Read())
            {
                LoggedInUserId = rdr.GetInt32(rdr.GetOrdinal("id"));
            }

            rdr.Close();
            MessageBox.Show("User Logged In Successfully " + LoggedInUserId.ToString(), "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cnx.Close();
            main main = new main();
            main.LoggedInUserId = LoggedInUserId;

            main.Show();

            //this.Close();
            
            
        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            string email = email_txt.Text;
            string password = password_txt.Text;
            if (!ValidateEmail(email))
            {
                MessageBox.Show("Verify Email Structure", "Wrong Email Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!ValidatePassword(password)) 
            {
                MessageBox.Show("Password Must Contain at least 1 lower/Upper Case & Digit & special Character & Length >= 8", "Wrong Password Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE email = @email", cnx);
            cmd.Parameters.AddWithValue("email", email);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows) 
            {
                MessageBox.Show("User Already Exist", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            rdr.Close();
            cmd.CommandText = "INSERT INTO users (email,password) values (@email,@password);";
            //cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password", password);
            rdr = cmd.ExecuteReader();
            MessageBox.Show("User Added Successfully", "User Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cnx.Close();

        }

        private void Welcome_Form_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data source=DESKTOP-F284B6I;Initial Catalog=project; Integrated Security=true";
            cnx = new SqlConnection(connectionString);
            try
            {
                cnx.Open();
                Console.Out.WriteLine("Connection Established");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }



        }
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
        private bool ValidatePassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }
    }
}
