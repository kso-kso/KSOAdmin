using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using KSOAdmin.Models.Base;

namespace KSOAdmin.Models.DomainModels.System
{
    [Entity(TableCnName = "省市列表")]
    public class Sys_Province : BaseModel
    {
        /// <summary>
        ///
        /// </summary>
        [Key]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Required(AllowEmptyStrings = false)]
        public string pid { get; set; }

       
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        [Required(AllowEmptyStrings = false)]
        public string Sys_Provincename { get; set; }

        /// <summary>
        ///类型
        /// </summary>
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Required(AllowEmptyStrings = false)]
        public string type { get; set; }

        
    }
}
