using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneTag.SharedCodebase
{
    public static class CastExtension
    {
        public static T Cast<T>(this object i_Obj, T i_Type)
        {
            return (T)i_Obj;
        }
    }
}
