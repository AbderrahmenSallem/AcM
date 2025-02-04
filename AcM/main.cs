using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AcM
{
    public partial class main : Form
    {
        private SqlConnection cnx;
        private int loggedin_id;
        public main()
        {
            InitializeComponent();
        }
        public int LoggedInUserId
        {
            get { return loggedin_id; }
            set { loggedin_id = value; }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void main_Load(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            contact_panel.Hide();
            account_panel.Hide();
            init_favorites();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            favorites_panel.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (favorite_datagrid.Rows.Count > 0)
            {
                // Move the scroll position to the top row
                favorite_datagrid.FirstDisplayedScrollingRowIndex = 0;

                // Clear any existing selections and select the top row
                favorite_datagrid.ClearSelection();
                favorite_datagrid.Rows[0].Selected = true;
            }
            else
            {
                MessageBox.Show("The DataGridView is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void favorites_panel_Paint(object sender, PaintEventArgs e)
        {

            init_favorites();

            //adapter.Update(dataSet, "TableName");
        }

        private void btn_min_favorites_Click(object sender, EventArgs e)
        {
            int rowCount = favorite_datagrid.Rows.Count;

            if (rowCount > 0)
            {
                // Select the bottom row directly
                favorite_datagrid.ClearSelection();
                favorite_datagrid.Rows[rowCount - 1].Selected = true;
            }
            else
            {
                MessageBox.Show("The DataGridView is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_up_favorites_Click(object sender, EventArgs e)
        {
            if (favorite_datagrid.SelectedRows.Count > 0)
            {
                int currentIndex = favorite_datagrid.SelectedRows[0].Index;
                if (currentIndex > 0)
                {
                    // Deselect the current row
                    favorite_datagrid.SelectedRows[0].Selected = false;

                    // Select the row above it
                    favorite_datagrid.Rows[currentIndex - 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the top.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_down_favorites_Click(object sender, EventArgs e)
        {
            if (favorite_datagrid.SelectedRows.Count > 0)
            {
                int currentIndex = favorite_datagrid.SelectedRows[0].Index;
                if (currentIndex != favorite_datagrid.Rows.Count - 1)
                {
                    // Deselect the current row
                    favorite_datagrid.SelectedRows[0].Selected = false;

                    // Select the row above it
                    favorite_datagrid.Rows[currentIndex + 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the bottom.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void init_favorites() 
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SELECT name,type,date FROM favoritess WHERE user_id = @LoggedInUserId;", cnx);
            cmd.Parameters.AddWithValue("LoggedInUserId", LoggedInUserId);
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "favoritess");
            //int LastIndex = dataSet.Tables["favoritess"].Rows.Count - 1;
            //dataSet.Tables["favoritess"].Rows.RemoveAt(LastIndex); 
            // Assuming you have already filled the DataSet as shown in your code

            // Set the DataSource property of dataGrid to the "users" table from the DataSet
            favorite_datagrid.DataSource = dataSet.Tables["favoritess"];

            int first = 0;
            foreach (DataGridViewColumn column in favorite_datagrid.Columns)
            {
                if (column.HeaderText == "name") // Check if the column is "name"
                {
                    column.MinimumWidth = 150; // Set the width for the "name" column
                }
                else if (column.HeaderText == "type") // Check if the column is "type"
                {
                    column.MinimumWidth = 170; // Set the width for the "type" column
                }
                else if (column.HeaderText == "date") // Check if the column is "date"
                {
                    column.MinimumWidth = 200; // Set the width for the "date" column
                }
                else
                {
                    column.MinimumWidth = 100; // Set a default width for other columns
                }
            }
        }

        private void btn_add_favorites_Click(object sender, EventArgs e)
        {
            string name = favorite_name.Text;
            string type = favorite_type.Text;
            DateTime dateTime;
            DateTime.TryParse(favorite_date.Text, out dateTime);

                SqlCommand cmd = new SqlCommand("INSERT INTO favoritess (user_id, name, type, date) VALUES (@LoggedInUserId, @name, @type, @dateTime);", cnx);
                cmd.Parameters.AddWithValue("@LoggedInUserId", LoggedInUserId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@dateTime", dateTime); // Use @dateTime instead of @date

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Favorite Added Successfully", "Favorite Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add favorite", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
        }

        private void btn_clear_favorites_Click(object sender, EventArgs e)
        {
            favorite_name.Text = "";
            favorite_type.Text = "";
            favorite_date.ResetText();
        }

        private void favorite_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void account_panel_Paint(object sender, PaintEventArgs e)
        {
            init_accounts();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            contact_panel.Hide();
            account_panel.Show();
            init_accounts();
        }
        private void init_accounts()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SELECT email,password FROM accountss WHERE user_id = @LoggedInUserId;", cnx);
            cmd.Parameters.AddWithValue("LoggedInUserId", LoggedInUserId);
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "accountss");
            //int LastIndex = dataSet.Tables["favoritess"].Rows.Count - 1;
            //dataSet.Tables["favoritess"].Rows.RemoveAt(LastIndex); 
            // Assuming you have already filled the DataSet as shown in your code

            // Set the DataSource property of dataGrid to the "users" table from the DataSet
            account_dataview.DataSource = dataSet.Tables["accountss"];

            int first = 0;
            foreach (DataGridViewColumn column in account_dataview.Columns)
            {
                if (column.HeaderText == "email") // Check if the column is "name"
                {
                    column.MinimumWidth = 250; // Set the width for the "name" column
                }
                else if (column.HeaderText == "password") // Check if the column is "type"
                {
                    column.MinimumWidth = 370; // Set the width for the "type" column
                }
            }
        }

        private void account_add_btn_Click(object sender, EventArgs e)
        {
            string email = account_email.Text;
            string password = account_password.Text;
            if (!ValidateEmail(email)) 
            {
                MessageBox.Show("Verify Email Structure", "Wrong Email Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            SqlCommand cmd = new SqlCommand("INSERT INTO accountss (user_id, email, password) VALUES (@LoggedInUserId, @email, @password);", cnx);
            cmd.Parameters.AddWithValue("@LoggedInUserId", LoggedInUserId);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Account Added Successfully", "Account Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        private void account_max_btn_Click(object sender, EventArgs e)
        {
            if (account_dataview.Rows.Count > 0)
            {
                // Move the scroll position to the top row
                account_dataview.FirstDisplayedScrollingRowIndex = 0;

                // Clear any existing selections and select the top row
                account_dataview.ClearSelection();
                account_dataview.Rows[0].Selected = true;
            }
            else
            {
                MessageBox.Show("The DataGridView is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void account_min_btn_Click(object sender, EventArgs e)
        {
            int rowCount = account_dataview.Rows.Count;

            if (rowCount > 0)
            {
                // Select the bottom row directly
                account_dataview.ClearSelection();
                account_dataview.Rows[rowCount - 1].Selected = true;
            }
            else
            {
                MessageBox.Show("The DataGridView is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void account_down_btn_Click(object sender, EventArgs e)
        {
            if (account_dataview.SelectedRows.Count > 0)
            {
                int currentIndex = account_dataview.SelectedRows[0].Index;
                if (currentIndex != account_dataview.Rows.Count - 1)
                {
                    // Deselect the current row
                    account_dataview.SelectedRows[0].Selected = false;

                    // Select the row above it
                    account_dataview.Rows[currentIndex + 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the bottom.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void account_up_btn_Click(object sender, EventArgs e)
        {
            if (account_dataview.SelectedRows.Count > 0)
            {
                int currentIndex = account_dataview.SelectedRows[0].Index;
                if (currentIndex > 0)
                {
                    // Deselect the current row
                    account_dataview.SelectedRows[0].Selected = false;

                    // Select the row above it
                    account_dataview.Rows[currentIndex - 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the top.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void contact_btn_Click(object sender, EventArgs e)
        {
            contact_panel.Show();
            init_contact();

        }
        private void init_contact()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SELECT name,number FROM contactss WHERE user_id = @LoggedInUserId;", cnx);
            cmd.Parameters.AddWithValue("LoggedInUserId", LoggedInUserId);
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "contactss");
            //int LastIndex = dataSet.Tables["favoritess"].Rows.Count - 1;
            //dataSet.Tables["favoritess"].Rows.RemoveAt(LastIndex); 
            // Assuming you have already filled the DataSet as shown in your code

            // Set the DataSource property of dataGrid to the "users" table from the DataSet
            contact_dataview.DataSource = dataSet.Tables["contactss"];

            int first = 0;
            foreach (DataGridViewColumn column in contact_dataview.Columns)
            {
                if (column.HeaderText == "name") // Check if the column is "name"
                {
                    column.MinimumWidth = 250; // Set the width for the "name" column
                }
                else if (column.HeaderText == "number") // Check if the column is "type"
                {
                    column.MinimumWidth = 300; // Set the width for the "type" column
                }
            }
        }

        private void contact_panel_Paint(object sender, PaintEventArgs e)
        {
            init_contact();
        }

        private void contact_up_btn_Click(object sender, EventArgs e)
        {
            if (contact_dataview.SelectedRows.Count > 0)
            {
                int currentIndex = contact_dataview.SelectedRows[0].Index;
                if (currentIndex > 0)
                {
                    // Deselect the current row
                    contact_dataview.SelectedRows[0].Selected = false;

                    // Select the row above it
                    contact_dataview.Rows[currentIndex - 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the top.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void contact_down_btn_Click_1(object sender, EventArgs e)
        {
            if (contact_dataview.SelectedRows.Count > 0)
            {
                int currentIndex = contact_dataview.SelectedRows[0].Index;
                if (currentIndex != contact_dataview.Rows.Count - 1)
                {
                    // Deselect the current row
                    contact_dataview.SelectedRows[0].Selected = false;

                    // Select the row above it
                    contact_dataview.Rows[currentIndex + 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the bottom.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void contact_up_maxbtn_Click(object sender, EventArgs e)
        {
            if (contact_dataview.SelectedRows.Count > 0)
            {
                int currentIndex = contact_dataview.SelectedRows[0].Index;
                if (currentIndex > 0)
                {
                    // Deselect the current row
                    contact_dataview.SelectedRows[0].Selected = false;

                    // Select the row above it
                    contact_dataview.Rows[currentIndex - 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Already at the top.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to move.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void contact_upmin_btn_Click(object sender, EventArgs e)
        {
            int rowCount = contact_dataview.Rows.Count;

            if (rowCount > 0)
            {
                // Select the bottom row directly
                contact_dataview.ClearSelection();
                contact_dataview.Rows[rowCount - 1].Selected = true;
            }
            else
            {
                MessageBox.Show("The DataGridView is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void contact_add_btn_Click(object sender, EventArgs e)
        {
            string name = contact_name.Text;
            string number = contact_number.Text;


            SqlCommand cmd = new SqlCommand("INSERT INTO contactss (user_id, name, number) VALUES (@LoggedInUserId, @name, @number);", cnx);
            cmd.Parameters.AddWithValue("@LoggedInUserId", LoggedInUserId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@number", number);
            

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Contact Added Successfully", "Contact Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contact_clear_btn_Click(object sender, EventArgs e)
        {
            contact_name.Text = "";
            contact_number.Text = "";
        }

        private void account_clear_btn_Click(object sender, EventArgs e)
        {
            account_email.Text = "";
            account_password.Text = "";
        }
    }
}
