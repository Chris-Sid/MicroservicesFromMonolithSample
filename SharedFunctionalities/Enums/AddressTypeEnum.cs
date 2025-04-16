using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedFunctionalities.Enums
{
    public enum AddressTypeEnum
    {

        [System.Runtime.Serialization.EnumMember(Value = @"BillTo")]
        BillTo = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ShipTo")]
        ShipTo = 1,
        Primary = 2,
    }
}
