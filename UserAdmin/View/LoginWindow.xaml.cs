using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand komanda = new SqlCommand("SELECT * FROM Users where user_name='" + this.userNameTxtBox.Text + "' and user_pass='" + this.passwordTxtBox.Password + "' ", conn);

                komanda.ExecuteNonQuery();
                SqlDataReader reader = komanda.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;
                }
                if (count == 1)
                {
                    MainWindow mainWin = new MainWindow();
                    mainWin.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login faild. Please try again.");
                }
            }
        }
    }
}

