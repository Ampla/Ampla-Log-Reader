using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Ampla.LogReader.EventLogs
{
    public class SecurityResolver : ISecurityResolver
    {
        private static readonly ISecurityResolver Resolver = new SecurityResolver();
        private readonly Dictionary<string, string> cache = new Dictionary<string, string>();

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

            string resolved;
            if (cache.TryGetValue(sid, out resolved))
            {
                return resolved;
            }

            if (!sid.StartsWith("S-", StringComparison.InvariantCulture))
            {
                cache[sid] = sid;
                return sid;
            }

            try
            {
                SecurityIdentifier security = new SecurityIdentifier(sid);
                string value = security.Translate(typeof (NTAccount)).Value;
                cache[sid] = value;
                return value;
            }
            catch (IdentityNotMappedException)
            {
                // swallow 
            }
            catch (Exception ex)
            {
                throw new ArgumentException("SID: " + sid, ex);
            }
            cache[sid] = sid; 
            return sid;
        }
    }
}