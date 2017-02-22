using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Networking.Modules.Login
{
    class LoginModule : TcpModule<LoginModule, LoginRequest, User>
    {
        public LoginModule() : base() { cmd = 1; }

        internal User user { get; private set; }
        internal void Login(string username, string password, MessageCallback callback)
        {
            SendMessage(new LoginRequest()
                {
                    un = username,
                    pw = password
                }, (error, msg) =>
                {
                    if (error != null)
                    {
                        user = null;
                    }
                    else
                    {
                        user = msg.data;
                    }

                    callback(error, msg);
                }
            );
        }
    }
}
