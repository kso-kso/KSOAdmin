﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using KSOAdmin.Models.Base;

namespace KSOAdmin.Models.DomainModels.System
{
    [Table("Sys_RoleAuth")]
    public class Sys_RoleAuth : BaseModel

    {
        /// <summary>
        ///
        /// </summary>
        [Key]
       [Display(Name ="")]
       [Required(AllowEmptyStrings=false)]
       public int Auth_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [Column(TypeName="int")]
       public int? Role_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [Column(TypeName="int")]
       public int? User_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Menu_Id { get; set; }

       /// <summary>
       ///用户权限
       /// </summary>
       [Display(Name ="用户权限")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       [Required(AllowEmptyStrings=false)]
       public string AuthValue { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(1000)")]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [Column(TypeName="datetime")]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(1000)")]
       public string Modifier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

        [ForeignKey("Role_Id")]
        public List<Sys_RoleAuth> RoleAuths { get; set; }
    }
}
