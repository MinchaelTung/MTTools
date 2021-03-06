﻿using System.ComponentModel.DataAnnotations;

namespace System.Web.Mvc.MemuInfo
{
    /// <summary>
    /// 菜单子项
    /// </summary>
    [Serializable]
    public class MemuItem
    {
        private string _Title = string.Empty;
        /// <summary>
        /// 显示标题
        /// </summary>
        [Display(Name = "菜单标题")]
        [Required(ErrorMessage = "必须填写菜单标题")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }
        private string _NavigateUrl = string.Empty;
        /// <summary>
        /// 导航路径
        /// </summary>
        [Display(Name = "菜单指向地址")]
        [Required(ErrorMessage = "菜单指向地址必填")]
        public string NavigateUrl
        {
            get
            {
                return this._NavigateUrl;
            }
            set
            {
                this._NavigateUrl = value;
            }
        }

        private string _Authority = "*";
        /// <summary>
        /// 权限*号没限制，多个权限使用"|"分隔
        /// </summary>
        [Display(Name = "访问权限")]
        [Required(ErrorMessage = "访问权限不能为空")]
        public string Authority
        {
            get
            {
                return this._Authority;
            }
            set
            {
                this._Authority = string.IsNullOrEmpty(value) == true ? "*" : value;
            }
        }
    }
}
