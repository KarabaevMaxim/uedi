namespace EdiModule.Windows
{
    using System.Windows;
    using EdiModuleCore;
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
			this.UserManager = new UserManager();

		}

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = this.LoginTxt.Text;
            string pass = this.PasswordTxt.Text;

            try
            {
                CoreInit.Connect(login, pass);
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Ошибка при подключении к базе.", "Ошибка.");
                return;
            }

            CoreInit.Init();
            MainWindow window = new MainWindow();
            window.Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            UserManagerWindow window = new UserManagerWindow();
            window.ShowDialog();
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			foreach (var item in this.UserManager.Users)
				this.UsersCmb.Items.Add(item);
			
		}

		private UserManager UserManager;
	}
}
