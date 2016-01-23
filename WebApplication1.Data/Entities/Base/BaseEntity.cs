using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
