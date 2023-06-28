using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWinViewModel : INotifyPropertyChanged
    {

        #region Fields

        private User currentUser;
        private UserCollection usersList;
        private ListCollectionView userListView;
        private string filteringText;
        private Mediator mediator;


        #endregion

        #region Properties


        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (currentUser == value)
                {
                    return;
                }
                currentUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentUser"));
            }
        }

        public UserCollection UsersList
        {
            get { return usersList; }
            set
            {
                if (usersList == value)
                {
                    return;
                }
                usersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UsersList"));
            }
        }

        public ListCollectionView UserListView
        {
            get { return userListView; }
            set
            {
                if (userListView == value)
                {
                    return;
                }
                userListView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserListView"));
            }
        }

        public String FilteringText
        {
            get { return filteringText; }
            set
            {
                if (filteringText == value)
                {
                    return;
                }
                filteringText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilteringText"));
            }
        }

        #endregion

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                if (deleteCommand == value)
                {
                    return;
                }
                deleteCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommand"));
            }
        }

        void DeleteExecute(object obj)
        {
            CurrentUser.DeleteUser();
            UsersList.Remove(CurrentUser);
        }

        bool CanDelete(object obj)
        {
            if (CurrentUser == null)
            {
                return false;
            }

            return true;
        }
        #region Constructor

        public MainWinViewModel(Mediator mediator)
        {
            this.mediator = mediator;

            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);

            this.PropertyChanged += MainWinViewModel_PropertyChanged;

            UsersList = UserCollection.GetAllUsers();
            UserListView = new ListCollectionView(UsersList);
            userListView.Filter = UserFilter;
            CurrentUser = new User();

            mediator.Register("UserChange", UserChange);
        }

        private void MainWinViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FilteringText"))
            {
                UserListView.Refresh();
            }
        }

        private bool UserFilter(object obj)
        {
            if (FilteringText == null)
            {
                return true;
            }
            if (FilteringText.Equals(""))
            {
                return true;
            }
            User user = obj as User;
            return user.User_name.ToLower().StartsWith(FilteringText.ToLower());
        }

        private void UserChange(object obj)
        {
            User user = (User)obj;

            int index = UsersList.IndexOf(user);
            if (index != -1)
            {
                UsersList.RemoveAt(index);
                UsersList.Insert(index, user);
            }
            else
            {
                UsersList.Add(user);
            }

        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}
