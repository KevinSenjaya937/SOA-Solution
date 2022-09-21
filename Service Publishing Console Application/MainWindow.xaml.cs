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
using Newtonsoft.Json;

namespace Service_Publishing_Console_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IAuthenticator_Server authServer;
        private static readonly string URL = "http://localhost:64223/";
        private static readonly RestClient serviceClient = new RestClient(URL);

        private Dictionary<string, Service> services;
        private String mode;
        private bool loggedIn = false;
        private int token;
        public MainWindow()
        {
            InitializeComponent();
            CreateAuthenticatorInstance();
            loginRadioBtn.IsChecked = true;

            this.services = new Dictionary<string, Service>();
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
            else if (mode == "Login" && !loggedIn)
            {
                token = authServer.Login(usernameBox.Text, passwordBox.Text);
                if (token != -1)
                {
                    loggedIn = true;
                    messagesBox.Text = "Logged in";

                    //Get all services
                    RestRequest request = new RestRequest("api/Services/{token}", Method.Get);
                    request.AddUrlSegment("token", token);
                    RestResponse response = serviceClient.Execute(request);

                    ServiceResult serviceResult = JsonConvert.DeserializeObject<ServiceResult>(response.Content);
                    addComboBoxItems(serviceResult);
                }
                else if (loggedIn)
                {
                    messagesBox.Text = "Your already logged in";
                }
                else 
                {
                    messagesBox.Text = "Log in failed";
                }
            }
            else
            {
                messagesBox.Text = "FAILED";
            }
        }

        private void addComboBoxItems(ServiceResult sr)
        {
            foreach (Service service in sr.Services)
            {
                services.Add(service.Description, service);
                servicesComboBox.Items.Add(service.Description);
            }
        }

        private void changeLoginRegisterMsgColour()
        {
            
        }

        private void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loggedIn)
            {
                if (serviceNameBox.Text != String.Empty &&
                    serviceDescBox.Text != String.Empty &&
                    serviceAPIEndpointBox.Text != String.Empty &&
                    serviceOperandsBox.Text != String.Empty &&
                    serviceOperandTypeBox.Text != String.Empty)
                {
                    Service service = new Service() { 
                        Name = serviceNameBox.Text,
                        Description = serviceDescBox.Text,
                        APIEndPoint = serviceAPIEndpointBox.Text,
                        NumOfOperands = Int16.Parse(serviceOperandsBox.Text), //Validate this
                        OperandType = serviceOperandTypeBox.Text
                        };

                    RestRequest restRequest = new RestRequest("api/Services/{token}", Method.Post);
                    restRequest.AddUrlSegment("token", this.token);
                    restRequest.AddJsonBody<Service>(service);
                    RestResponse restResponse = serviceClient.Execute(restRequest);

                    ServiceResult serviceResult = JsonConvert.DeserializeObject<ServiceResult>(restResponse.Content);

                    if (serviceResult != null)
                    {
                        publishStatusText.Text = serviceResult.Status.ToString();
                    }
                    else 
                    {
                        publishStatusText.Text = "Null return value";
                    }
                }
            }
        }

        private void unpublishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loggedIn && servicesComboBox.SelectedIndex != -1)
            {
                RestRequest restRequest = new RestRequest("api/Services/{token}", Method.Delete);
                restRequest.AddUrlSegment("token", this.token);
                string srvDescrip = servicesComboBox.SelectedItem.ToString();
                restRequest.AddJsonBody<Service>(new Service { APIEndPoint = services[srvDescrip].APIEndPoint, Description = srvDescrip });
                RestResponse restResponse = serviceClient.Execute(restRequest);

                services.Remove(servicesComboBox.SelectedItem.ToString());
                servicesComboBox.Items.Remove(servicesComboBox.SelectedItem.ToString());
                unpublishStatusText.Text = "Service Unpublished";
            }
            else if (servicesComboBox.SelectedIndex == -1) 
            {
                unpublishStatusText.Text = "Please select a service to unpublish";
            }
        }
    }
}
