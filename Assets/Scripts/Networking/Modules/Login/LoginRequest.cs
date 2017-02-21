using Assets.Scripts.Util;

namespace Assets.Scripts.Networking.Modules.Login
{
    [Serialize]
    class LoginRequest
    {
        public string un { get; set; }
        public string pw { get; set; }
    }
}
