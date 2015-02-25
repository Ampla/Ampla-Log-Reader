using System;
using System.Globalization;
using System.Threading;

namespace Ampla.LogReader
{
    public class ThreadCulture
    {
        public static IDisposable SetUICulture(string culture)
        {
            return new SetThreadUICulture(culture);
        }

        private class SetThreadUICulture : IDisposable
        {
            private CultureInfo previousCulture;

            public SetThreadUICulture(string culture)
            {
                previousCulture = Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }

            public void Dispose()
            {
                if (previousCulture != null)
                {
                    Thread.CurrentThread.CurrentUICulture = previousCulture;
                    previousCulture = null;
                }
            }
        }
    }
}