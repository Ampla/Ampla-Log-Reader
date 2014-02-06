using System;

namespace Ampla.LogReader
{
    public static class ArgumentCheck
    {
        public static void IsNotNull(object param)
        {
            if (param == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void IsNotNull(object param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void IsNotEmpty(string param)
        {
            if (param == string.Empty)
            {
                throw new ArgumentException(@"Argument can't be an empty string");
            }
        }

        public static void IsNotEmpty(string param, string paramName)
        {
            if (param == string.Empty)
            {
                throw new ArgumentException(@"Argument can't be an empty string", paramName);
            }
        }
    }
}