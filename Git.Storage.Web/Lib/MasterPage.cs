/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-01 12:23:27
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-01 12:23:27       情缘
*********************************************************************************/

using Git.Framework.Resource;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Git.Framework.Io;
using Git.Storage.Entity.Base;
using System.Text;
using Git.Storage.Provider.Base;
using Git.Storage.Provider.Store;
using Git.Storage.Entity.Store;
using Git.Storage.Provider;

namespace Git.Storage.Web.Lib
{
    public partial class MasterPage : MainPage
    {
        /// <summary>
        /// 调用父类初始化方法
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            LoadResource();

            SetMenu();

            SetStorage();

            SetNav();

            SetVersion();

            //设置左侧菜单是否展示cookie
            string menuStatus = "open";
            if (Session[CacheKey.SESSION_MENU_STATUS]!=null)
            {
                menuStatus = Session[CacheKey.SESSION_MENU_STATUS] as string;
            }
            menuStatus = menuStatus == "close" ? "sidebar-closed" : "";
            ViewBag.MenuStatus = menuStatus;
        }

        public string Title { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public string CssFile { get; set; }
        public string JsFile { get; set; }


        /// <summary>
        /// 加载资源文件包括SEO文件关键字 CSS JS
        /// </summary>
        private void LoadResource()
        {
            PageEntity pageEntity = ResourceManager.GetPageEntityByPath(this.RequestPath);
            if (!pageEntity.IsNull())
            {
                if (!pageEntity.SeoEntity.IsNull())
                {
                    Title = pageEntity.SeoEntity.Title;
                    Keyword = pageEntity.SeoEntity.KeyWords;
                    Description = pageEntity.SeoEntity.Description;
                }
                string cssFile = string.Empty;
                string jsFile = string.Empty; ;
                if (!pageEntity.FileGroupList.IsNullOrEmpty())
                {
                    foreach (FileGroup fileGroup in pageEntity.FileGroupList)
                    {
                        if (!fileGroup.IsNull() && !fileGroup.FileList.IsNullOrEmpty())
                        {
                            foreach (FileEntity fileEntity in fileGroup.FileList)
                            {
                                if (fileEntity.FileType == EFileType.Css)
                                {
                                    if (fileEntity.Browser.IsEmpty())
                                    {
                                        cssFile += "<link href=\"" + fileEntity.FilePath + "\" rel=\"stylesheet\" type=\"text/css\" />";
                                    }
                                    else
                                    {
                                        if (fileEntity.Browser.ToLower() == "ie6")
                                        {
                                            cssFile += "<!--[if IE 6]><link rel=\"stylesheet\" href=\"" + fileEntity.FilePath + "\" /><![endif]-->";
                                        }
                                        else if (fileEntity.Browser.ToLower() == "ie7")
                                        {
                                            cssFile += "<!--[if IE 7]><link rel=\"stylesheet\" href=\"" + fileEntity.FilePath + "\" /><![endif]-->";
                                        }
                                        else if (fileEntity.Browser.ToLower() == "ie8")
                                        {
                                            cssFile += "<!--[if IE 8]><link rel=\"stylesheet\" href=\"" + fileEntity.FilePath + "\" /><![endif]-->";
                                        }
                                        else if (fileEntity.Browser.ToLower() == "ie9")
                                        {
                                            cssFile += "<!--[if IE 9]><link rel=\"stylesheet\" href=\"" + fileEntity.FilePath + "\" /><![endif]-->";
                                        }
                                    }
                                }
                                if (fileEntity.FileType == EFileType.Js)
                                {
                                    jsFile += "<script type=\"text/javascript\" src=\"" + fileEntity.FilePath + "?t="+Guid.NewGuid().ToString()+"\"></script>";
                                }
                            }
                        }
                    }
                }
                CssFile = cssFile;
                JsFile = jsFile;
            }

            ViewBag.CssFile = CssFile;
            ViewBag.JsFile = JsFile;
            ViewBag.Title = Title;
            ViewBag.Keyword = Keyword;
            ViewBag.Description = Description;
            ViewBag.CurrentLoginUser = this.LoginUser;
        }


        /// <summary>
        /// 根据登录用的角色加载菜单信息
        /// </summary>
        private void SetMenu()
        {
            StringBuilder sb = new StringBuilder();
            if (IsLogin() && !this.LoginUser.RoleNum.IsEmpty())
            {
                PowerProvider provider = new PowerProvider();
                List<SysResourceEntity> list = provider.GetAllotedPower(this.LoginUser.RoleNum);

                if (!list.IsNullOrEmpty())
                {
                    foreach (SysResourceEntity parent in list.Where(a => a.ParentNum.IsEmpty()))
                    {
                        StringBuilder sbChild = new StringBuilder();
                        sbChild.AppendFormat("<ul class=\"sub\">");
                        bool flag = false;
                        bool exists = false;
                        foreach (SysResourceEntity child in list.Where(a => a.ParentNum == parent.ResNum))
                        {
                            flag = child.Url.ToLower() == this.Path.ToLower()
                                || (!child.Children.IsNullOrEmpty() && child.Children.Exists(c => c.Url.ToLower() == this.Path.ToLower()))
                                ;
                            if (flag)
                            {
                                exists = true;
                            }
                            //SysResourceEntity ParentResource = null;
                            //SysResourceEntity RootResource = null;

                            //if (child.Url.ToLower() == this.Path.ToLower())
                            //{
                            //    flag = true;
                            //}
                            //else
                            //{
                            //    if (!child.ParentNum.IsEmpty())
                            //    {
                            //        ParentResource = list.FirstOrDefault(a => a.ResNum == child.ParentNum);

                            //        if (ParentResource != null)
                            //        {
                            //            if (ParentResource.Url.ToLower() == this.Path.ToLower())
                            //            {
                            //                flag = true;
                            //            }
                            //            else
                            //            {
                            //                if (!ParentResource.ParentNum.IsEmpty())
                            //                {
                            //                    RootResource = list.FirstOrDefault(a => a.ResNum == ParentResource.ParentNum);

                            //                    if (RootResource != null)
                            //                    {
                            //                        if (RootResource.Url.ToLower() == this.Path.ToLower())
                            //                        {
                            //                            flag = true;
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}

                            //if (flag)
                            //{
                            //    exists = true;
                            //}

                            sbChild.AppendFormat("<li {0}><a href=\"{1}\">{2}</a></li>", flag ? "class=\"active\"" : "", child.Url.IsEmpty() ? "javascript:void(0)" : child.Url, child.ResName);
                        }
                        sbChild.AppendFormat("</ul>");

                        sb.AppendFormat("<li class=\"has-sub {0}\">", exists ? "active" : "");
                        sb.AppendFormat("<a href=\"javascript:void(0);\">");
                        sb.AppendFormat("<i class=\"{0}\"></i>", parent.CssName.IsEmpty() ? "icon-bookmark-empty" : parent.CssName);
                        sb.AppendFormat("<span class=\"title\">{0}</span>", parent.ResName);
                        sb.AppendFormat("<span class=\"arrow {0}\"></span>", exists ? "open" : "");
                        sb.AppendFormat("</a>");
                        sb.Append(sbChild.ToString());
                        sb.AppendFormat("</li>");
                    }
                }
            }
            ViewBag.MenuItems = sb.ToString();
        }

        /// <summary>
        /// 设置导航信息
        /// </summary>
        private void SetNav()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"breadcrumb\">");
            sb.Append("<li>");
            sb.Append("<i class=\"icon-home\"></i>");
            sb.Append("<a href=\"/Home/Welcome\">首页</a>");
            sb.Append("<i class=\"icon-angle-right\"></i>");
            sb.Append("</li>");
            if (IsLogin() && !this.LoginUser.RoleNum.IsEmpty())
            {
                PowerProvider provider = new PowerProvider();
                SysResourceProvider SysResourceProvider = new SysResourceProvider();
                List<SysResourceEntity> listSource = SysResourceProvider.GetList();
                List<SysResourceEntity> list = provider.GetRoleResource(this.LoginUser.RoleNum);
                if (!list.IsNullOrEmpty())
                {
                    SysResourceEntity item = list.SingleOrDefault(a => a.Url.ToLower() == this.Path.ToLower());
                    List<SysResourceEntity> listResult = new List<SysResourceEntity>();
                    while (item != null)
                    {
                        listResult.Insert(0, item);

                        if (item.ParentNum.IsEmpty())
                        {
                            break;
                        }
                        else
                        {
                            if (listSource.Exists(a => a.ResNum == item.ParentNum))
                            {
                                item = listSource.First(a => a.ResNum == item.ParentNum);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < listResult.Count; i++)
                    {
                        if (i != listResult.Count - 1)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<a href=\"{0}\">{1}</a>", listResult[i].Url.IsEmpty() ? "javascript:void(0)" : listResult[i].Url, listResult[i].ResName);
                            sb.Append("<i class=\"icon-angle-right\"></i>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<a href=\"javascript:void(0)\">{0}</a>", listResult[i].ResName);
                            sb.Append("</li>");
                        }
                    }
                }
            }
            sb.Append("</ul>");
            ViewBag.NavMenu = sb.ToString();
        }


        /// <summary>
        /// 设置仓库
        /// </summary>
        private void SetStorage()
        {
            StorageProvider provider = new StorageProvider();
            List<StorageEntity> list = provider.GetList();
            list = list.IsNull() ? new List<StorageEntity>() : list;
            ViewBag.MenuStorage = list;
            string storageNum = DefaultStore;
            storageNum = storageNum.IsEmpty() ? ResourceManager.GetSettingEntity("STORE_DEFAULT_PRODUCT").Value : storageNum;
            ViewBag.MenuStorageName = list.First(a => a.StorageNum == storageNum).StorageName;
        }

        /// <summary>
        /// 设置系统的编译版本
        /// </summary>
        private void SetVersion()
        {
            string version = Git.Framework.Cache.CacheHelper.Get<string>(Git.Storage.Provider.CacheKey.CACHE_SYS_VERSION);
            ViewBag.DebugVersion = version;

            string sign = Git.Framework.Encrypt.Encrypt.TripleDESDecrypting(ResourceManager.GetSettingEntity("Sign").Value);
            ViewBag.Sign = sign;
        }
    }
}