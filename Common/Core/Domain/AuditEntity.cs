using System;

namespace Common.Core.Domain
{
    public class AuditEntity<T> : Entity<T>
    {
        public int CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }

    public class AuditEntity : AuditEntity<int>
    {
    }
}