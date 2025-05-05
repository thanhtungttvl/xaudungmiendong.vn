using System.ComponentModel;
using System.Text.Json.Serialization;

namespace UserEnum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleOption
    {
        [Description("Guest")] Guest = 0,
        [Description("Client")] User = 1,
        [Description("Admin")] Admin = 2,
        [Description("Editer")] Editer = 3,
        [Description("Root")] Root = 999,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusOption
    {
        [Description("Locked")] Locked = 0,
        [Description("Active")] Active = 1,
    }
}
