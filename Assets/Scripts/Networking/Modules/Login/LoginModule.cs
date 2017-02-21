using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Networking.Modules.Login
{
    class LoginModule : TcpModule<LoginModule, LoginRequest, User>
    {
        internal void Login(string username, string password, MessageCallback callback)
        {
            SendMessage(new LoginRequest()
            {
                un = username,
                pw = password
            }, callback);
        }
    }
}
