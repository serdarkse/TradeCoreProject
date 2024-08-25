
using TradeCore.AuthService.Models.Response.Command.AppLdapUser;

namespace TradeCore.AuthService.Models.Request.Command.AppLdapUser
{
    public class RegisterUserCommandRequest : RequestBase<RegisterUserCommandResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsSystemAdmin { get; set; }
    }
}
