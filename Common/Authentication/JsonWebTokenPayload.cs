using System.Collections.Generic;

namespace Common.Authentication
{
    public class JsonWebTokenPayload
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        
        public IReadOnlyList<string> Roles { get; set; }
        public long Expires { get; set; }
        
        public string Audience { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}