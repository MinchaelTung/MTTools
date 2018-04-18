using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF事例.Models
{
    [Table("InternalBlogs", Schema = "dbo")]
    public class Blog
    {
        [Key]
        public int PrimaryTrackingKey { get; set; }
        [Required]
        public string Title { get; set; }
        //[MaxLength(10), MinLength(5)]
        [ConcurrencyCheck, MaxLength(10), MinLength(5)]   
        public string BloggerName { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        
        [Timestamp]
        public Byte[] TimeStamp { get; set; }
        [NotMapped]
        public string BlogCode
        {
            get
            {
                return Title.Substring(0, 1) + ":" + BloggerName.Substring(0, 1);
            }
        }
    }
}
