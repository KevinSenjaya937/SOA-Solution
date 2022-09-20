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
using SOA_SolutionDLL;
using System.ServiceModel;
using Newtonsoft;
using RestSharp;
using Newtonsoft.Json;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String mode;
        private int token;
        private Boolean userLoggedIn = false;

        private Dictionary<string, Service> services;
        private List<TextBox> textBoxes = new List<TextBox>();
        private List<TextBlock> textBlocks = new List<TextBlock>();
        private List<String> namesOfServices = new List<String>();
        private static IAuthenticator_Server authServer;

        private static string URL = "http://localhost:64223/";
        private static RestClient client = new RestClient(URL);


        public MainWindow()
        {
            InitializeComponent();
            loginRadioBtn.IsChecked = true;


            this.services = new Dictionary<string, Service>();
            

            // get combo box items from registry project - Services (ADDTwoNumbers, ADDThreeNumbers, ect)
            // add combo box items after authentication

            createAuthenticatorInstance();

            
        }

        private void createAuthenticatorInstance()
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

                if (token == -1)
                {
                    messagesBox.Text = "Login failed";
                }
                else
                {
                    // Populate 
                    RestRequest request = new RestRequest("api/Services/{token}");
                    request.AddUrlSegment("token", token);
                    RestResponse response = client.Execute(request);

                    ServiceResult serviceResult = JsonConvert.DeserializeObject<ServiceResult>(response.Content);
                    addComboBoxItems(serviceResult);
                }
                
            }
            else
            {
                messagesBox.Text = "FAILED";
            }
            // Use authenticator verification here
            // Authenticator verification returns token
            // Save token to private variable
        }

        

        // Combo Box Functions
        private void addComboBoxItems(ServiceResult sr)
        {
            foreach (Service service in sr.Services)
            {
                services.Add(service.Description, service);
                servicesComboBox.Items.Add(service.Description);
            }
        }

        private void servicesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = servicesComboBox.SelectedItem.ToString();
            Service selectedService = services[selected];

            createOperandBoxes(selectedService.NumOfOperands);
            createHelperTexts(selectedService.NumOfOperands);
        }

        private void createOperandBoxes(int numOfOperands)
        {
            removeTextBoxes();
            TextBox[] operandTextBoxes = new TextBox[numOfOperands];
            
            int x = 381;
            int y = 188;

            for (int i = 0; i < numOfOperands; i++)
            {
                TextBox textBox = new TextBox();
                operandTextBoxes[i] = textBox;

                textBox.Name = "Operand" + (i + 1).ToString();
                textBox.Text = "Operand " + (i+1).ToString();
                textBox.Width = 120;
                textBox.Height = 21.95;
                textBox.HorizontalAlignment = HorizontalAlignment.Left;
                textBox.VerticalAlignment = VerticalAlignment.Top;

                textBox.Margin = new Thickness(x, y+=27, 0, 0);
                operandTextBoxes[i] = textBox;
            }

            foreach (TextBox textBox in operandTextBoxes)
            {
                MainGrid.Children.Add(textBox);
                textBoxes.Add(textBox);
            }    
        }

        private void createHelperTexts(int numOfOperands)
        {
            TextBlock[] helperTexts = new TextBlock[numOfOperands];
            int x = 255;
            int y = 189;

            for (int i = 0; i < numOfOperands; i++)
            {
                TextBlock textBlock = new TextBlock();
                helperTexts[i] = textBlock;

                textBlock.Name = "HelperText" + (i + 1).ToString();
                textBlock.Text = "Enter Operand " + (i + 1).ToString() + ": ";
                textBlock.Width = 110;
                textBlock.Height = 19.95;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Top;

                textBlock.Margin = new Thickness(x, y += 27, 0, 0);
                helperTexts[i] = textBlock;
            }

            foreach (TextBlock textBlock in helperTexts)
            {
                MainGrid.Children.Add(textBlock);
                textBlocks.Add(textBlock);
            }
        }

        private void removeTextBoxes()
        {
            foreach (TextBox textBox in textBoxes)
            {
                MainGrid.Children.Remove(textBox);
            }

            foreach (TextBlock textBlock in textBlocks)
            {
                MainGrid.Children.Remove(textBlock);
            }
            this.textBoxes.Clear();
            this.textBlocks.Clear();
        }

        private void searchServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            bool found = services.ContainsKey(serviceSearchBox.Text);
            
            if (found)
            {
                bool exists = servicesComboBox.Items.Contains(services[serviceSearchBox.Text]);

                // Search for the matching services from the registry.
                if (exists)
                {
                    servicesComboBox.SelectedItem = services[serviceSearchBox.Text];
                }
                
            }
        }

        private void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            string selected = servicesComboBox.SelectedItem.ToString();
            Service selectedService = services[selected];
            List<int> operands = new List<int>();

            foreach (TextBox textBox in textBoxes)
            {
                if (textBox.Text != String.Empty)
                {
                    operands.Add(Int32.Parse(textBox.Text));
                }
            }

            string endPoint = selectedService.APIEndPoint;

            foreach (int operand in operands)
            {
                endPoint = endPoint + operand + "/";
            }

            RestRequest request = new RestRequest(endPoint);
            RestResponse response = client.Execute(request);

            Result result = JsonConvert.DeserializeObject<Result>(response.Content);

            resultBox.Text = result.Value.ToString();
        }
    }
}
