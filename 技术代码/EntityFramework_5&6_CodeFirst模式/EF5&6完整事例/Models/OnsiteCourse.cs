using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF事例.Models
{
    //如果没有指定表名将会在Course表中增加字段
    //当前生成父级表和子表
    [Table("OnsiteCourse")]
    public partial class OnsiteCourse : Course
    {
        public string Location { get; set; }
        public string Days { get; set; }
        public System.DateTime Time { get; set; }
    }
}
