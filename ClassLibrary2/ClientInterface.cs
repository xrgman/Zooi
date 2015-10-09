using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface ClientInterface
    {
        void LoginResponse(bool loginOk, bool isPhysician);
        void GiveUserResponse(User user);
        void GiveUserResponse(List<User> users);
        void GiveUserResponse(List<UserClient> users);
    }
}
