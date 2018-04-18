using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF事例.Models
{
    [ComplexType]
    public class BlogDetails
    {
        public DateTime? DateCreated { get; set; }

        //  [MaxLength(250)]
        // public string Description { get; set; }
        [Column("BlogDescription", TypeName = "ntext")]
        public String Description { get; set; }
    }
}
