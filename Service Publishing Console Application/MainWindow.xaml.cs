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

        private string userName;
        private string password;
        private Service serviceToPublish;
        private Service serviceToUnpublish;
        private string srvDescrip;

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
        private async void loginRegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            this.userName = usernameBox.Text;
            this.password = passwordBox.Text;
            loginRegisterProgressBarSwitch(true);

            if (mode == "Register" && !loggedIn)
            {
                Task<string> task = new Task<string>(registerUser);
                

                task.Start();
                messagesBox.Text = "Registration Commmenced...";

                string success = await task;
                messagesBox.Text = success;
            }
            else if (mode == "Login" && !loggedIn)
            {
                Task<int> task = new Task<int>(loginUser);

                task.Start();
                messagesBox.Text = "Login Commenced...";

                this.token = await task;

                if (token == -1)
                {
                    messagesBox.Text = "Login Failed";
                }
                else if (loggedIn)
                {
                    messagesBox.Text = "Your already logged in";
                }
                else 
                {
                    this.loggedIn = true;
                    messagesBox.Text = "Logged Successful";

                    //Get all services
                    updateComboBoxItems();
                }
            }
            else
            {
                messagesBox.Text = "FAILED";
            }
            loginRegisterProgressBarSwitch(false);
        }

        private void updateComboBoxItems()
        {
            servicesComboBox.Items.Clear();
            services.Clear();
            RestRequest request = new RestRequest("api/Services/{token}", Method.Get);
            request.AddUrlSegment("token", token);
            RestResponse response = serviceClient.Execute(request);

            ServiceResult serviceResult = JsonConvert.DeserializeObject<ServiceResult>(response.Content);
            addComboBoxItems(serviceResult);
        }

        private int loginUser()
        {
            return authServer.Login(this.userName, this.password);
        }

        private string registerUser()
        {
            return authServer.Register(this.userName, this.password);
        }

        private void disableForm(Boolean switchBool)
        {
            registerRadioBtn.IsEnabled = !switchBool;
            loginRadioBtn.IsEnabled = !switchBool;
            loginRegisterBtn.IsEnabled = !switchBool;

            publishBtn.IsEnabled = !switchBool;
            unpublishBtn.IsEnabled = !switchBool;

            servicesComboBox.IsEnabled = !switchBool;

            usernameBox.Dispatcher.Invoke(new Action(() => usernameBox.IsReadOnly = switchBool));
            passwordBox.Dispatcher.Invoke(new Action(() => passwordBox.IsReadOnly = switchBool));
            serviceNameBox.Dispatcher.Invoke(new Action(() => serviceNameBox.IsReadOnly = switchBool));
            serviceDescBox.Dispatcher.Invoke(new Action(() => serviceDescBox.IsReadOnly = switchBool));
            serviceAPIEndpointBox.Dispatcher.Invoke(new Action(() => serviceAPIEndpointBox.IsReadOnly = switchBool));
            serviceOperandsBox.Dispatcher.Invoke(new Action(() => serviceOperandsBox.IsReadOnly = switchBool));
            serviceOperandTypeBox.Dispatcher.Invoke(new Action(() => serviceOperandTypeBox.IsReadOnly = switchBool));
        }

        private void loginRegisterProgressBarSwitch(Boolean switchBool)
        {

            registerLoginProgressBar.IsIndeterminate = switchBool;
            disableForm(switchBool);
        }

        private void addComboBoxItems(ServiceResult sr)
        {
            foreach (Service service in sr.Services)
            {
                services.Add(service.Description, service);
                servicesComboBox.Items.Add(service.Description);
            }
        }

        private async void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            

            if (loggedIn)
            {
                if (serviceNameBox.Text != String.Empty &&
                    serviceDescBox.Text != String.Empty &&
                    serviceAPIEndpointBox.Text != String.Empty &&
                    serviceOperandsBox.Text != String.Empty &&
                    serviceOperandTypeBox.Text != String.Empty)
                {
                    bool inputOk = true;
                    if (int.TryParse(serviceOperandsBox.Text, out int num))
                    {
                        this.serviceToPublish = new Service()
                        {
                            Name = serviceNameBox.Text,
                            Description = serviceDescBox.Text,
                            APIEndPoint = serviceAPIEndpointBox.Text,
                            NumOfOperands = Int16.Parse(serviceOperandsBox.Text), //Validate this
                            OperandType = serviceOperandTypeBox.Text
                        };
                    }
                    else
                    {
                        inputOk = false;
                    }

                    
                    if (inputOk)
                    {
                        Task<ServiceResult> task = new Task<ServiceResult>(publishService);
                        publishProgressBarSwitch(true);

                        task.Start();
                        publishStatusText.Text = "Commencing Publish...";

                        ServiceResult serviceResult = await task;

                        if (serviceResult != null)
                        {
                            if (serviceResult.Status == Result.ResultCodes.Success)
                            {
                                publishStatusText.Text = serviceResult.Status.ToString();
                                updateComboBoxItems();
                            }
                            else
                            {
                                publishStatusText.Text = serviceResult.Reason.ToString();
                                messagesBox.Text = "LOGGED OUT";
                                loggedIn = false;
                                publishStatusText.Text = "Please Login";
                            }
                        }
                        else
                        {
                            publishStatusText.Text = "Null return value";
                        }
                    }
                    else
                    {
                        publishStatusText.Text = "Please Fill Number Of Operand with an Integer";
                    }
                }
                else
                {
                    publishStatusText.Text = "Please Fill All Fields";
                }
                    
            }
            else 
            {
                publishStatusText.Text = "Please Login";
            }
            publishProgressBarSwitch(false);
            
        }

        private ServiceResult publishService()
        {
            RestRequest restRequest = new RestRequest("api/Services/{token}", Method.Post);
            restRequest.AddUrlSegment("token", this.token);
            restRequest.AddJsonBody<Service>(this.serviceToPublish);
            RestResponse restResponse = serviceClient.Execute(restRequest);

            ServiceResult serviceResult = JsonConvert.DeserializeObject<ServiceResult>(restResponse.Content);

            return serviceResult;
        }

        private void publishProgressBarSwitch(Boolean switchBool)
        {
            publishProgressBar.IsIndeterminate = switchBool;
            disableForm(switchBool);
        }

        private async void unpublishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loggedIn && servicesComboBox.SelectedIndex != -1)
            {
                this.srvDescrip = servicesComboBox.SelectedItem.ToString();

                Task<ServiceResult> task = new Task<ServiceResult>(unpublishService);
                unpublishProgressBarSwitch(true);
                
                task.Start();
                unpublishStatusText.Text = "Unpublish Commencing...";

                ServiceResult serviceResult = await task;

                if (serviceResult != null)
                {
                    if (serviceResult.Status == Result.ResultCodes.Success)
                    {
                        services.Remove(servicesComboBox.SelectedItem.ToString());
                        servicesComboBox.Items.Remove(servicesComboBox.SelectedItem.ToString());
                        unpublishStatusText.Text = "Service Unpublished";
                    }
                    else
                    {
                        unpublishStatusText.Text = serviceResult.Reason.ToString();
                        messagesBox.Text = "LOGGED OUT";
                        loggedIn = false;
                        unpublishStatusText.Text = "Please Login";
                    }
                }
                else
                {
                    unpublishStatusText.Text = "Null return value";
                }
            }
            else if (servicesComboBox.SelectedIndex == -1 && loggedIn)
            {
                unpublishStatusText.Text = "Please select a service to unpublish";
            }
            else 
            {
                unpublishStatusText.Text = "Please login";
            }
            unpublishProgressBarSwitch(false);
        }

        private ServiceResult unpublishService()
        {
            RestRequest restRequest = new RestRequest("api/Services/{token}", Method.Delete);
            restRequest.AddUrlSegment("token", this.token);
            restRequest.AddJsonBody<Service>(new Service { APIEndPoint = services[srvDescrip].APIEndPoint, Description = srvDescrip });
            RestResponse restResponse = serviceClient.Execute(restRequest);

            ServiceResult serviceResult = JsonConvert.DeserializeObject<ServiceResult>(restResponse.Content);

            return serviceResult;
        }

        private void unpublishProgressBarSwitch(Boolean switchBool)
        {
            unpublishProgressBar.IsIndeterminate = switchBool;
            disableForm(switchBool);
        }
    }
}
