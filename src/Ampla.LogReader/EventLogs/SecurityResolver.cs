using System;
using System.Security.Principal;

namespace Ampla.LogReader.EventLogs
{
    public class SecurityResolver : ISecurityResolver
    {
        private static readonly ISecurityResolver Resolver = new SecurityResolver();

        public static ISecurityResolver Instance
        {
            get
            {
                return Resolver;
            }
        }

        public string GetName(string sid)
        {
            if (string.IsNullOrEmpty(sid)) return "";

            if (!sid.StartsWith("S-", StringComparison.InvariantCulture))
            {
                return sid;
            }

            try
            {
                SecurityIdentifier security = new SecurityIdentifier(sid);
                return security.Translate(typeof (NTAccount)).Value;
            }
            catch (IdentityNotMappedException)
            {
                // swallow 
            }
            catch (Exception ex)
            {
                throw new ArgumentException("SID: " + sid, ex);
            }
            return sid;
        }
    }
}