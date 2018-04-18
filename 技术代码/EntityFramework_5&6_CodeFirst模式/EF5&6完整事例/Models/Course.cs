using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF事例.Models
{

    public class Course
    {
        // Primary key 主键默认规则 ID 或者 类名+ID
        //手动标签指定
        // [Key]
        //指定在数据库中列名
        //[Column(name:"ID")]
        public int CourseID { get; set; }

        public string Title { get; set; }
        public int Credits { get; set; }

        // Foreign key 外键 规则类名+ID并创建该类的引用必须带有 virtual 关键字
        public int? DepartmentID { get; set; }

        // Navigationproperties 引用类
        public virtual Department Department { get; set; }
    }
}
