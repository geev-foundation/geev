using Geev.Authorization.Users;

namespace Geev.Zero.SampleApp.Users
{
    public class User : GeevUser<User>
    {
        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }
    }
}