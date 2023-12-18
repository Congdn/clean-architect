using Default.Application.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.EF.Entities
{
    public class Category : AuditedEntity, ISoftDelete
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Order { get; set; } = 0;
        public bool IsDeleted { get; set; }
    }
}
