namespace EdiModuleCore
{
    using System.Collections.Generic;
	using System.Linq;
    using Newtonsoft.Json;
    using System.IO;

    public class UserManager
    {
        public void AddNewUser(User user)
        {
			user.ID = this.GetNewID();
			this.users.Add(user);
			this.Save();
        }

		public void RemoveUser(User user)
		{
			this.users.Remove(user);
			this.Save();
		}

		private int GetNewID()
		{
			int result = -1;
			result = this.users.Max(u => u.ID);
			result++;
			return result;
		}

        private void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this.users);
                try
                {
                    FileService.WriteTextFile(UserFileName, json);
                }
                catch(IOException ex)
                {
                    throw ex;
	            }
            }
            catch(JsonException ex)
            {
                throw ex;
            }
        }

        private List<User> Load()
        {
            try
            {
                string json = FileService.ReadTextFile(UserFileName);
                try
                {
                    return JsonConvert.DeserializeObject<List<User>>(json);
                }
                catch(JsonException ex)
                {
                    throw ex;
                }
                
            }
			catch(FileNotFoundException)
			{
				return new List<User>();
			}
            catch(IOException ex)
            {
                throw ex;
            }
        }

		private List<User> users;
        public List<User> Users
		{
			get
			{
				users = this.Load();
				return users;
			}
		}
        const string UserFileName = "Users.json"; 

    }
}
