using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Facebook;
using TakeAPicture.Presenters;
namespace TakeAPicture
{
    /// <summary>
    /// Interaction logic for Facebook.xaml
    /// </summary>
    public partial class FacebookMain : UserControl
    {
        public FacebookMain()
        {
            InitializeComponent();
            loginButton.Click += loginButton_Click;
            logoutButton.Click += logoutButton_Click;
            postButton.Click += postButton_Click;
            browseButton.Click += browseButton_Click;
            facebookBrowser.Navigated += facebookBrowser_Navigated;


            _viewPresenter = new MainWindowPresenter(this);
            DataContext = _viewPresenter;

            
        }

        public string Fotostr
        {
            get;
            set;
        }
        private readonly MainWindowPresenter _viewPresenter;
        public WebBrowser Browser
        {
            get { return facebookBrowser; }
        }

        void facebookBrowser_Navigated(Object sender, NavigationEventArgs e)
        {
            var fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (!fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
                return;

            if (oauthResult.IsSuccess)
                _viewPresenter.LoginSucceeded(oauthResult);
            else
                _viewPresenter.LoginFailed(oauthResult);
        }

        private void loginButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewPresenter.Login();
        }

        private void logoutButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewPresenter.Logout();
        }

        private void postButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewPresenter.PostUpdate();
        }

        private void browseButton_Click(Object sender, RoutedEventArgs e)
        {
            _viewPresenter.BrowseForJpegFile();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _viewPresenter.PhotoPath = Utilidades.PathInicial + @"\Efecto_" + Fotostr + ".jpg";

        }
    }
}
