using System;
using System.ComponentModel;
using System.Windows;
using Facebook;
using TakeAPicture.Common;

namespace TakeAPicture.Presenters
{
    public class MainWindowPresenter : INotifyPropertyChanged
    {
        #region Private members
        private readonly FacebookMain _view;
        private String _token;
        private String _status;
        private String _userName;
        private String _userPicture;
        private String _photoPath;
        private Boolean _showBrowser;
        private Boolean _isLogged;
        #endregion

        public MainWindowPresenter(FacebookMain view)
        {
            _view = view;           
            _showBrowser = false;
            ResetUserInfo();
        }

        /// <summary>
        /// Gets and sets the FB token of the current user.
        /// </summary>
        public String Token
        {
            get { return _token; }
            set { _token = value; OnPropertyChanged("Token"); }
        }

        /// <summary>
        /// Gets and sets the name of the currently logged user.
        /// </summary>
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        /// <summary>
        /// Gets and sets the URL of the FB user picture.
        /// </summary>
        public String UserPicture
        {
            get { return _userPicture; }
            set { _userPicture = value; OnPropertyChanged("UserPicture"); }
        }

        /// <summary>
        /// Gets and sets the text to post.
        /// </summary>
        public String Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        /// <summary>
        /// Gets and sets the photo path to post.
        /// </summary>
        public String PhotoPath
        {
            get { return _photoPath; }
            set { _photoPath = value; OnPropertyChanged("PhotoPath"); }
        }

        /// <summary>
        /// Shows and hides programmatically the web browser used to log to FB.
        /// </summary>
        public Boolean ShowBrowser
        {
            get { return _showBrowser; }
            set { _showBrowser = value; OnPropertyChanged("ShowBrowser"); }
        }

        /// <summary>
        /// Shows and hides programmatically the web browser used to log to FB.
        /// </summary>
        public Boolean IsLogged
        {
            get { return _isLogged; }
            set { _isLogged = value; OnPropertyChanged("IsLogged"); }
        }

        /// <summary>
        /// Triggers the authentication process towards Facebook
        /// </summary>
        public void Login()
        {
            var loginUrl = FbHelpers.GetLoginUrl();
            ShowBrowser = true;
            _view.Browser.Navigate(loginUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Logout()
        {
            ResetUserInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PostUpdate()
        {
            if (String.IsNullOrEmpty(PhotoPath))
                FbHelpers.Post(Token, Status);
            else
                FbHelpers.PostWithPhoto(Token, Status, PhotoPath);

            Status = String.Empty;
            PhotoPath = String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BrowseForJpegFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog {DefaultExt = ".jpg", Filter = "JPEG |*.jpg"};
            var result = dialog.ShowDialog();
            if (result == true)
            {
                PhotoPath = dialog.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oauthResult"></param>
        public void LoginSucceeded(FacebookOAuthResult oauthResult)
        {
            ShowBrowser = false;
            var token = FbHelpers.GetAccessToken(oauthResult);
            Token = token;
            dynamic user = FbHelpers.GetUser(token);
            
            UserName = String.Format("{0} {1}", user.first_name, user.last_name);
           // UserPicture = user.picture;
            IsLogged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void LoginFailed(FacebookOAuthResult result)
        {
            ShowBrowser = false;
            IsLogged = false;
            MessageBox.Show(result.ErrorDescription);
        }


        #region Private
        private void ResetUserInfo()
        {
            UserName = "No hay Usuario Logeado";
            UserPicture = String.Empty;
            IsLogged = false;
            Status = "Postea La foto en tu muro";
        }
        #endregion  

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion 
    }
}