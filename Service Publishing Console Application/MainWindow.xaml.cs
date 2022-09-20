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
using System.ServiceModel;
using SOA_SolutionDLL;
using RestSharp;
using Newtonsoft;

namespace Service_Publishing_Console_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IAuthenticator_Server authServer;
        private static string URL = "http://localhost:64223/";
        private static RestClient client = new RestClient(URL);

        private String mode;
        private int token;
        public MainWindow()
        {
            InitializeComponent();
            CreateAuthenticatorInstance();
            loginRadioBtn.IsChecked = true;
        }



        // Authenticator Instance
        private void CreateAuthenticatorInstance()
        {
            ChannelFactory<IAuthenticator_Server> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/AuthenticationService";
            foobFactory = new ChannelFactory<IAuthenticator_Server>(tcp, URL);
            authServer = foobFactory.CreateChannel();
        }


        // Radio Button Functions
        private void registerRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            mode = "Register";
            changeLoginRegisterBtn();
        }

        private void loginRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            mode = "Login";
            changeLoginRegisterBtn();
        }

        private void changeLoginRegisterBtn()
        {
            if (registerRadioBtn.IsChecked == true)
            {
                loginRadioBtn.IsChecked = false;
            }
            else
            {
                registerRadioBtn.IsChecked = false;
            }

            loginRegisterBtn.IsEnabled = true;
            loginRegisterBtn.Content = mode;
        }

        // Login / Register Button Function
        private void loginRegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mode == "Register")
            {
                string success = authServer.Register(usernameBox.Text, passwordBox.Text);
                messagesBox.Text = success;
            }
            else if (mode == "Login")
            {
                token = authServer.Login(usernameBox.Text, passwordBox.Text);
            }
            else
            {
                messagesBox.Text = "FAILED";
            }
            // Use authenticator verification here
            // Authenticator verification returns token
            // Save token to private variable
        }

        private void changeLoginRegisterMsgColour()
        {
            
        }

        private void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (serviceNameBox.Text != String.Empty &&
                serviceDescBox.Text != String.Empty &&
                serviceAPIEndpointBox.Text != String.Empty &&
                serviceOperandsBox.Text != String.Empty &&
                serviceOperandTypeBox.Text != String.Empty)
            {
                RestRequest request = new RestRequest("api/Services/{token}");
                request.AddUrlSegment("token", this.token);
                RestResponse response = 
        }
    }
}
