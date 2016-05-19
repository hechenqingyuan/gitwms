/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-09-01 12:25:31
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-09-01 12:25:31       情缘
*********************************************************************************/

using Git.Framework.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Git.Storage.Web.Lib
{
    public partial class AjaxPage : MainPage
    {
        /// <summary>
        /// 调用父类的初始化方法
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        private JsonObject _returnJson;
        /// <summary>
        /// JSON返回值
        /// </summary>
        public JsonObject ReturnJson
        {
            get
            {
                if (_returnJson == null)
                {
                    _returnJson = new JsonObject();
                }
                if (!_returnJson.HasProperty("LoginUserName"))
                {
                    _returnJson.AddProperty("LoginUserName", this.LoginUserName);
                }
                if (!_returnJson.HasProperty("LoginUserCode"))
                {
                    _returnJson.AddProperty("LoginUserCode", this.LoginUserCode);
                }
                return _returnJson;
            }
            set
            {
                if (value != null)
                {
                    _returnJson = value; 
                }
            }
        }
    }
}