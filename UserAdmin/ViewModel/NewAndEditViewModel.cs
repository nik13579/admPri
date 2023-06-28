using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class NewAndEditViewModel : INotifyPropertyChanged
    {
        #region Field

        private User currentUser;
        private string windowTitle;

        private Mediator mediator;


        #endregion

        #region Property


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
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle == value)
                {
                    return;
                }
                windowTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowTitle"));
            }
        }

        #endregion


        #region Constructors

        public NewAndEditViewModel(User user, Mediator mediator)
        {
            this.mediator = mediator;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);
            CurrentUser = user;
            WindowTitle = "Edit User";
        }

        public NewAndEditViewModel(Mediator mediator)
        {
            this.mediator = mediator;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);
            CurrentUser = new User();
            WindowTitle = "New User";
        }

        #endregion


        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand == value)
                {
                    return;
                }
                saveCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaveCommand"));
            }
        }

        #region Methods

        void SaveExecute(object obj)
        {
            if (CurrentUser != null && !CurrentUser.HasErrors)
            {
                CurrentUser.Save();
                OnDone(new DoneEventArgs("User Saved."));
                mediator.Notify("UserChange", CurrentUser);
            }
            else
            {
                OnDone(new DoneEventArgs("Check your input."));
            }
        }
        bool CanSave(object obj)
        {
            return true;
        }
        #endregion

        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string message;

            public string Message
            {
                get { return message; }
                set
                {
                    if (message == value)
                    {
                        return;
                    }
                    message = value;
                }
            }
            public DoneEventArgs(string message)
            {
                this.message = message;
            }
        }
        public event DoneEventHandler Done;

        public void OnDone(DoneEventArgs e)
        {
            if (Done != null)
            {
                Done(this, e);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}
