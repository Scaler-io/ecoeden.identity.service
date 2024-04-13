using System.Runtime.Serialization;

namespace IdentityServer.Models.Enums
{
    public enum Roles
    {
        [EnumMember(Value = "ADMIN")]
        Admin,
        [EnumMember(Value = "OPERATOR")]
        Operator,
        [EnumMember(Value = "AUDITOR")]
        Auditor
    }
}
