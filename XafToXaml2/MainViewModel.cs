using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using XafToXaml2.Helpers;

namespace XafToXaml2
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Properties

        #region FilePath

        private string _FilePath = String.Empty;

        /// <summary>
        /// Gets or sets the FilePath property. This observable property 
        /// indicates the path to the .xaf file.
        /// </summary>
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                if (_FilePath != value)
                {
                    _FilePath = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #endregion Properties

        #region Commands

        #region Browse

        private ICommand _BrowseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_BrowseCommand == null)
                {
                    _BrowseCommand = new RelayCommand<object>(Browse_Executed);
                }
                return _BrowseCommand;
            }
        }

        private void Browse_Executed(object parameter)
        {
            Browse();
        }

        #endregion

        #region Load

        private ICommand _LoadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                {
                    _LoadCommand = new RelayCommand<object>(Load_Executed);
                }
                return _LoadCommand;
            }
        }

        private void Load_Executed(object parameter)
        {
            Load();
        }

        #endregion

        #endregion Commands

        #region Methods

        private void Browse()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = ".xaf files|*.xaf|All files|*.*";

            Nullable<bool> result = dialog.ShowDialog();
            if (true == result)
                FilePath = dialog.FileName;
        }

        private void Load()
        {
            // Outer level validation
            if (String.IsNullOrWhiteSpace(FilePath))
            {
                MessageBox.Show("Please enter a file path");
                return;
            }
            else if (!File.Exists(FilePath))
            {
                MessageBox.Show("Invalid file path");
                return;
            }

            new XafToXaml2Engine().LoadXaf(FilePath);
        }

        #endregion Methods

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged implementation
    }
}
