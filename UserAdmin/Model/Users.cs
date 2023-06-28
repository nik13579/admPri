using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Model
{
    public class User : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Fields

        private int id;
        private string user_name;
        private string user_pass;
        private DateTime? _dateOfBirth;
        private bool? is_admin;
        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        #endregion

        #region Properties
        public int Id
        {
            get => id;
            set
            {
                if (id == value)
                {
                    return;
                }
                id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        public string User_name
        {
            get => user_name;
            set
            {
                if (user_name == value)
                {
                    return;
                }
                user_name = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Username can't be empty.");
                    SetErrors("User_name", errors);
                    valid = false;
                }
                if (!Regex.Match(value, @"^[a-zA-Z0-9\s]+$").Success)
                {
                    errors.Add("Username can contain letters and numbers.");
                    SetErrors("User_name", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("User_name");
                }
                OnPropertyChanged(new PropertyChangedEventArgs("User_name"));
            }
        }

        public string User_pass
        {
            get => user_pass;
            set
            {
                if (user_pass == value)
                {
                    return;
                }
                user_pass = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Password field can't be empty.");
                    SetErrors("User_pass", errors);
                    valid = false;
                }
                if (!Regex.Match(value, @"^([a-zA-Z0-9\s]).{6,15}$").Success)
                {
                    errors.Add("Password can contain letters and numbers with minimum six caracters and no spaces");
                    SetErrors("User_pass", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("User_pass");
                }
                OnPropertyChanged(new PropertyChangedEventArgs("User_pass"));
            }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null)
                {
                    errors.Add("Date of Birth can't be empty");
                    SetErrors("DateOfBirth", errors);
                    valid = false;
                }

                if (value < new DateTime(1880, 12, 12))
                {
                    errors.Add("Date of Birth can not be before December 12th of 1880.");
                    SetErrors("DateOfBirth", errors);
                    valid = false;
                }
                if (value > DateTime.Now)
                {
                    errors.Add("Date is in future.");
                    SetErrors("DateOfBirth", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("DateOfBirth");
                }
                OnPropertyChanged(new PropertyChangedEventArgs("DateOfBirth"));
            }
        }

        public bool? Is_admin
        {
            get => is_admin;
            set
            {
                is_admin = value;

                List<string> errors = new List<string>();
                bool valid = true;


                if (value == null)
                {
                    errors.Add("This field can contain only Yes or No");
                    SetErrors("Is_admin", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("Is_admin");
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Is_admin"));
            }
        }

        public bool HasErrors => errors.Count > 0;
        #endregion

        #region Constructors



        public User(string User_name, string User_pass, DateTime DateOfBirth, bool Is_admin)
        {
            this.User_name = User_name;
            this.User_pass = User_pass;
            this.DateOfBirth = DateOfBirth;
            this.Is_admin = Is_admin;
        }

        public User(int Id, string User_name, string User_pass, DateTime DateOfBirth, bool Is_admin)
        {
            this.User_name = User_name;
            this.User_pass = User_pass;
            this.DateOfBirth = DateOfBirth;
            this.Is_admin = Is_admin;
            this.Id = Id;
        }

        public User()
        {
            User_name = "";
            User_pass = "";
            DateOfBirth = null;
            Is_admin = null;
        }

        #endregion

        #region Data Access


        public static User GetUserFromResultSet(SqlDataReader reader)
        {
            User user = new User((int)reader["id"], (string)reader["user_name"], (string)reader["user_pass"], (DateTime)reader["date_of_birth"], (bool)reader["is_admin"]);
            return user;
        }


        public void UpdateUser()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Users SET user_name=@User_name, user_pass=@User_pass, date_of_birth=@DateOfBirth, is_admin=@Is_admin WHERE id=@Id", conn);

                SqlParameter userNamePar = new SqlParameter("@User_name", SqlDbType.NVarChar);
                userNamePar.Value = this.User_name;

                SqlParameter userPassPar = new SqlParameter("@User_pass", SqlDbType.NVarChar);
                userPassPar.Value = this.User_pass;

                SqlParameter dateOfBirthPar = new SqlParameter("@DateOfBirth", SqlDbType.Date);
                dateOfBirthPar.Value = this.DateOfBirth;

                SqlParameter isAdminPar = new SqlParameter("@Is_admin", SqlDbType.Bit);
                isAdminPar.Value = this.Is_admin;

                SqlParameter idPar = new SqlParameter("@Id", SqlDbType.Int, 11);
                idPar.Value = this.Id;

                command.Parameters.Add(userNamePar);
                command.Parameters.Add(userPassPar);
                command.Parameters.Add(dateOfBirthPar);
                command.Parameters.Add(isAdminPar);
                command.Parameters.Add(idPar);

                int rows = command.ExecuteNonQuery();
            }
        }



        public void InsertUser()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO Users(user_name, user_pass, date_of_birth, is_admin) VALUES(@User_name, @User_pass, @DateOfBirth, @Is_admin); SELECT IDENT_CURRENT('Users');", conn);

                SqlParameter userNameParam = new SqlParameter("@User_name", SqlDbType.NVarChar);
                userNameParam.Value = this.User_name;

                SqlParameter userPassParam = new SqlParameter("@User_pass", SqlDbType.NVarChar);
                userPassParam.Value = this.User_pass;

                SqlParameter dateOfBirthParam = new SqlParameter("@DateOfBirth", SqlDbType.Date);
                dateOfBirthParam.Value = this.DateOfBirth;

                SqlParameter isAdminParam = new SqlParameter("@Is_admin", SqlDbType.Bit);
                isAdminParam.Value = this.Is_admin;

                command.Parameters.Add(userNameParam);
                command.Parameters.Add(userPassParam);
                command.Parameters.Add(dateOfBirthParam);
                command.Parameters.Add(isAdminParam);

                var id = command.ExecuteScalar();
                if (id != null)
                {
                    this.Id = Convert.ToInt32(id);
                }
            }
        }

        public void DeleteUser()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM Users WHERE id=@Id", conn);

                SqlParameter myParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                myParam.Value = this.Id;

                command.Parameters.Add(myParam);

                int rows = command.ExecuteNonQuery();


            }
        }
        #endregion

        #region Methods
        public void Save()
        {
            if (Id == 0)
            {
                InsertUser();
            }
            else
            {
                UpdateUser();
            }
        }

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {

            errors.Remove(propertyName);
            errors.Add(propertyName, propertyErrors);
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void ClearErrors(string propertyName)
        {
            errors.Remove(propertyName);
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return errors.Values;
            }
            else
            {
                return errors.ContainsKey(propertyName) ? errors[propertyName] : null;
            }
        }


        public User Clone()
        {
            User cloneUser = new User();

            cloneUser.User_name = this.User_name;
            cloneUser.User_pass = this.User_pass;
            cloneUser.DateOfBirth = this.DateOfBirth;
            cloneUser.Is_admin = this.Is_admin;
            cloneUser.Id = this.Id;

            return cloneUser;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            User objUser = (User)obj;

            if (objUser.Id == this.Id) return true;

            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

    }
}

