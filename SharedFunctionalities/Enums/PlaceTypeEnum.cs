using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedFunctionalities.Enums
{
    public enum PlaceTypeEnum
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Home")]
        Home = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Work")]
        Work = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Unknown")]
        Unknown = 2,
        Residential = 3,
        Commercial = 4,
    }
}
