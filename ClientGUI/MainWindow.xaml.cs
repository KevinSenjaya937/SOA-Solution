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

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String mode;
        private int token;
        private Dictionary<string, string> methods;
        public MainWindow()
        {
            InitializeComponent();
            loginRadioBtn.IsChecked = true;

            this.methods = new Dictionary<string, string>();
            methods.Add("Login", "authenticator_login_path");
            methods.Add("Register", "authenticator_register_path");

            // get combox box items from registry project - Services (ADDTwoNumbers, ADDThreeNumbers, ect)
            servicesComboBox.Items.Insert(0, "Please select a service");
            servicesComboBox.Items.Add("ADDTwoNumbers");
            servicesComboBox.Items.Add("ADDThreeNumbers");
            servicesComboBox.Items.Add("MULTwoNumbers");
            servicesComboBox.Items.Add("MULThreeNumbers");
            

        }

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

        private void loginRegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            String authenticatorPath = methods[mode];

            // Use authenticator verification here
            // Authenticator verification returns token
            // Save token to private variable
        }

        private void addComboBoxItems(List<String> services)
        {

        }

        private void createOperandBoxes(int numOfOperands)
        {
            TextBox[] operandTextBoxes = new TextBox[numOfOperands];
            int x = 381;
            int y = 188;

            for (int i = 0; i < numOfOperands; i++)
            {
                TextBox textBox = new TextBox();
                operandTextBoxes[i] = textBox;

                textBox.Name = "Operand " + (i + 1).ToString();
                textBox.Text = "Operand " + (i+1).ToString();
                textBox.Margin = new Thickness(x, y + 27, 0, 0);
               
            }
        }
    }
}
