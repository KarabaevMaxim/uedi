namespace EdiModule.Windows
{
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
    using System.Windows.Shapes;
    using EdiModuleCore;

    /// <summary>
    /// Логика взаимодействия для UserManagerWindow.xaml
    /// </summary>
    public partial class UserManagerWindow : Window
    {
        public UserManagerWindow()
        {
            InitializeComponent();
			this.bindings.Add("ИД", "ID");
			this.bindings.Add("Логин", "Login");
            this.bindings.Add("Пароль", "Password");
            this.bindings.Add("Путь до базы данных", "DbFolder");
            this.bindings.Add("Путь до хранилища накладных", "WaybillFolder");
            this.bindings.Add("Путь до архива накладных", "ArchiveFolder");
            this.UserManager = new UserManager();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateTable();
        }

        private void UsersTbl_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void SaveUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.LoginTxt.Text) 
                || !string.IsNullOrWhiteSpace(this.WaybillFolderTxt.Text) 
                || !string.IsNullOrWhiteSpace(this.ArchiveFolderTxt.Text))
            {
                User user = new User
                {
                    Login = this.LoginTxt.Text,
                    Password = this.PasswordTxt.Text,
                    WaybillFolder = this.WaybillFolderTxt.Text,
                    ArchiveFolder = this.ArchiveFolderTxt.Text
                };
                this.UserManager.AddNewUser(user);
                this.UpdateTable();
            }
        }

        private void UpdateTable()
        {
            this.UsersTbl.Columns.Clear();

            foreach (var item in this.bindings)
                this.UsersTbl.Columns.Add(new DataGridTextColumn { Header = item.Key, Binding = new Binding(item.Value) });

            this.UsersTbl.Items.Clear();

            foreach (var item in this.UserManager.Users)
                this.UsersTbl.Items.Add(item);
        }

		private void RemoveUserBtn_Click(object sender, RoutedEventArgs e)
		{
			if(UsersTbl.SelectedItems.Count == 1)
			{
				if(UsersTbl.SelectedItems[0] is User user)
				{
					this.UserManager.RemoveUser(user);
					this.UpdateTable();
				}
			}
			
		}

		private UserManager UserManager;
        private Dictionary<string, string> bindings = new Dictionary<string, string>();


	}
}
