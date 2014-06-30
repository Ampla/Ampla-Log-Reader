using System;

namespace Ampla.LogReader.Remoting
{
    public class IdentitySession : IComparable<IdentitySession>
    {
        public IdentitySession(string identity, string session)
        {
            Identity = identity;
            Session = session;
        }

        public string Identity { get; private set; }
        public string Session { get; private set; }

        public int CompareTo(IdentitySession other)
        {
            int compare = StringComparer.InvariantCulture.Compare(Identity, other.Identity);
            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(Session, other.Session);
            }
            return compare;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Identity, Session);
        }

        public override bool Equals(object obj)
        {
            IdentitySession identitySession = obj as IdentitySession;
            if (identitySession != null)
            {
                return ToString().Equals(obj.ToString());
            }
            return false;
        }

        public override int GetHashCode()
        {
            string toString = Identity + "|" + Session;
            return toString.GetHashCode();
        }
    }
}