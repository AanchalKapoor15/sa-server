//using System.Linq;
using System.Reflection;
using System.Text.Json;
using server.Models;

namespace server.Services
{
    public interface IUserService
    {
        public List<User> GetUsers(string firstName, string lastName, string email);
        public User GetUser(int userId);
        public bool AddUser(User user);
        public bool UpdateUser(User user);
        public bool DeleteUser(int userId);
    }

    public class UserService : IUserService
    {
        private readonly string _filePath = @"C:\Projects\StandardsAustralia\server\server\Data\data.json";
        public List<User> GetUsers(string firstName, string lastName, string email)
        {
            var usersObject = GetUsers();

            var selectedUser = usersObject.users
                .Where(u => (firstName == null || String.Equals(firstName, u.first_name, StringComparison.OrdinalIgnoreCase))
                    && (lastName == null || String.Equals(lastName, u.last_name, StringComparison.OrdinalIgnoreCase))
                    && (email == null || String.Equals(email, u.email, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return selectedUser;
        }

        public User GetUser(int userId)
        {
            var usersObject = GetUsers();

            var selectedUser = usersObject.users
                .Where(u => u.id == userId)
                .FirstOrDefault();

            return selectedUser;
        }

        public bool AddUser(User user)
        {
            var usersObject = GetUsers();

            var lastId = GetIdOfLastUser(usersObject.users);

            user.id = ++lastId;

            usersObject.users.Add(user);
            WriteToFile(usersObject);

            return true;
        }

        public bool UpdateUser(User user)
        {
            var usersObject = GetUsers();

            var selectedUser = usersObject.users
                .Where(u => u.id == user.id)
                .FirstOrDefault();

            if (selectedUser != null)
            {
                // Could have used automapper here,
                // but then we lose fine-grained control over the 
                // properties being mapped.
                // If at a later stage any of the properties in the source/destination change,
                // we would get a compile time error this way (runtime error if we use automapper)
                selectedUser.first_name = user.first_name;
                selectedUser.last_name = user.last_name;
                selectedUser.email = user.email;
                selectedUser.gender = user.gender;
                selectedUser.status = user.status;

                WriteToFile(usersObject);

                return true;
            }
            return false;
        }

        public bool DeleteUser(int userId)
        {
            var usersObject = GetUsers();

            var selectedUser = usersObject.users
                .Where(u => u.id == userId)
                .FirstOrDefault();

            if (selectedUser != null)
            {
                usersObject.users.Remove(selectedUser);

                WriteToFile(usersObject);

                return true;
            }
            return false;
        }

        private int GetIdOfLastUser(List<User> users)
        {
            return users.Last().id;
        }

        private UsersRootObject GetUsers()
        {
            string jsonData = File.ReadAllText(_filePath);
            UsersRootObject users = JsonSerializer.Deserialize<UsersRootObject>(jsonData);

            return users;
        }

        private void WriteToFile(UsersRootObject usersObject)
        {
            var jsonData = JsonSerializer.Serialize(usersObject);

            // The entire file is re-written as part of write operations.
            // In a real world enterprise app, only the records to be updated will be impacted
            // and not the entire datastore/database.
            File.WriteAllText(_filePath, jsonData);
        }
     }
}
