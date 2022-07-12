namespace server.Models
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public bool status { get; set; }
    }

    public class UsersRootObject
    {
        public List<User> users { get; set; }
    }
}
