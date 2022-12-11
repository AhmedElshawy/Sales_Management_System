
using System.Runtime.Serialization;

namespace Core.Constants
{
    public enum InvoiceStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "confirmed")]
        Confirmed
    }
}
