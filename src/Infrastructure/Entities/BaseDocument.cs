using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public abstract class BaseDocument : BaseEntity
    {
        public string Type { get; set; }
        public string Content { get; set; } //json string
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }

        public void SetCreated(string updatedBy)
        {
            this.CreatedAt = DateTime.Now;
            SetUpdated(updatedBy);
        }

        public void SetUpdated(string updatedBy)
        {
            this.UpdatedBy = updatedBy;
            this.LastUpdated = DateTime.Now;
        }
    }
}
