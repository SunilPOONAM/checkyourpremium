using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
   public static class ExtensionMethod
    {
       public static bool IsBetween(this int dt, int start, int end)
       {
           return dt >= start && dt <= end;
       }
    }
}
