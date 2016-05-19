/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-01 12:09:24
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-01 12:09:24       情缘
*********************************************************************************/

using Git.Storage.Entity.Base;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Git.Storage.Provider;
using Git.Framework.Resource;
using Git.Storage.Entity.Store;
using Git.Storage.Provider.Store;

namespace Git.Storage.Web.Lib
{
    public partial class MainPage : Git.Framework.Controller.Mvc.ControllerBase
    {
        /// <summary>
        /// 重新初始化方法
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);


            //设置用户信息
            SetUserInfo();
        }

        private AdminEntity _loginUser;

        /// <summary>
        /// 登录用户
        /// </summary>
        public AdminEntity LoginUser
        {
            get
            {
                _loginUser = Session[CacheKey.SESSION_LOGIN_ADMIN] as AdminEntity;
                return _loginUser;
            }
            set
            {
                if (value != null)
                {
                    Session[CacheKey.SESSION_LOGIN_ADMIN] = value;
                }
                _loginUser = value;
            }
        }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string LoginUserName
        {
            get
            {
                if (this.LoginUser.IsNotNull())
                {
                    return this.LoginUser.UserName;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 登录用户编号
        /// </summary>
        public string LoginUserCode
        {
            get
            {
                if (this.LoginUser.IsNotNull())
                {
                    return this.LoginUser.UserCode;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 判断用户是否登录 登录返回true 未登录返回false
        /// </summary>
        /// <returns></returns>
        public bool IsLogin()
        {
            return this.LoginUser.IsNotNull();
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        private void SetUserInfo()
        {
            if (IsLogin())
            {
                ViewBag.LoginUser = this.LoginUser;
                ViewBag.LoginUserName = this.LoginUserName;
                ViewBag.LoginUserCode = this.LoginUserCode;
            }

            string login = "login";
            string v2 = ResourceManager.GetSettingEntity(login + "sign").Value;
            int index = v2.LastIndex("TwbAIII0zXk9");
            if (index != 90)
            {
                throw new Exception("");
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        protected void RemoveLogin()
        {
            Session.Remove(CacheKey.SESSION_LOGIN_ADMIN);
        }

        private string _defaultPStore = string.Empty;
        private string _defaultMStore = string.Empty;
        private string _defaultStore=string.Empty;

        /// <summary>
        /// 默认成品仓库
        /// </summary>
        protected string DefaultPStore
        {
            get
            {
                if (_defaultPStore.IsEmpty())
                {
                    _defaultPStore = ResourceManager.GetSettingEntity("STORE_DEFAULT_PRODUCT").Value;
                }
                return _defaultPStore;
            }
        }

        /// <summary>
        /// 默认原料仓库
        /// </summary>
        protected string DefaultMStore
        {
            get
            {
                if (_defaultMStore.IsEmpty())
                {
                    _defaultMStore = ResourceManager.GetSettingEntity("STORE_DEFAULT_MATERIAL").Value;
                }
                return _defaultMStore;
            }
        }

        /// <summary>
        /// 当前选中仓库
        /// </summary>
        protected string DefaultStore
        {
            get 
            {
                if (_defaultStore.IsEmpty())
                {
                    _defaultStore = Session[CacheKey.SESSION_STORAGE_CURRENT] as string;
                    _defaultStore = _defaultStore.IsEmpty() ? DefaultPStore : _defaultStore;
                }
                return _defaultStore;
            }
            set
            {
                Session[CacheKey.SESSION_STORAGE_CACHE] = null;
                Session[CacheKey.SESSION_STORAGE_CURRENT] = value;
            }
        }

        private StorageEntity _store;

        /// <summary>
        /// 当前仓库
        /// </summary>
        protected StorageEntity Store
        {
            get
            {
                _store = Session[CacheKey.SESSION_STORAGE_CACHE] as StorageEntity;
                if (_store.IsNull())
                {
                    _store = new StorageProvider().GetStoreByCache(this.DefaultStore);
                    if (_store.IsNotNull())
                    {
                        Session[CacheKey.SESSION_STORAGE_CACHE] = _store;
                    }
                }
                return _store;
            }
        }
    }
}