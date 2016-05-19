/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 代码工具自动生成
 * Create Date: 2013-08-19 11:54:40
 * Blog: http://www.cnblogs.com/qingyuan/ 
 * Description: Git.Framework
 * 
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 11:54:40
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Git.Framework.ORM;

namespace Git.Storage.Entity.Base
{
    [TableAttribute(DbName = "JooWMS", Name = "Admin", PrimaryKeyName = "ID", IsInternal = false)]
    public partial class AdminEntity : BaseEntity
    {
        public AdminEntity()
        {
        }

        [DataMapping(ColumnName = "ID", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = true, AutoIncrement = true, IsMap = true)]
        public Int32 ID { get; set; }

        public AdminEntity IncludeID(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ID"))
            {
                this.ColumnList.Add("ID");
            }
            return this;
        }

        [DataMapping(ColumnName = "UserName", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string UserName { get; set; }

        public AdminEntity IncludeUserName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("UserName"))
            {
                this.ColumnList.Add("UserName");
            }
            return this;
        }

        [DataMapping(ColumnName = "PassWord", DbType = DbType.String, Length = 50, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string PassWord { get; set; }

        public AdminEntity IncludePassWord(bool flag)
        {
            if (flag && !this.ColumnList.Contains("PassWord"))
            {
                this.ColumnList.Add("PassWord");
            }
            return this;
        }

        [DataMapping(ColumnName = "UserCode", DbType = DbType.String, Length = 40, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string UserCode { get; set; }

        public AdminEntity IncludeUserCode(bool flag)
        {
            if (flag && !this.ColumnList.Contains("UserCode"))
            {
                this.ColumnList.Add("UserCode");
            }
            return this;
        }

        [DataMapping(ColumnName = "RealName", DbType = DbType.String, Length = 40, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string RealName { get; set; }

        public AdminEntity IncludeRealName(bool flag)
        {
            if (flag && !this.ColumnList.Contains("RealName"))
            {
                this.ColumnList.Add("RealName");
            }
            return this;
        }

        [DataMapping(ColumnName = "Email", DbType = DbType.String, Length = 30, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Email { get; set; }

        public AdminEntity IncludeEmail(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Email"))
            {
                this.ColumnList.Add("Email");
            }
            return this;
        }

        [DataMapping(ColumnName = "Mobile", DbType = DbType.String, Length = 11, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Mobile { get; set; }

        public AdminEntity IncludeMobile(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Mobile"))
            {
                this.ColumnList.Add("Mobile");
            }
            return this;
        }

        [DataMapping(ColumnName = "Phone", DbType = DbType.String, Length = 20, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Phone { get; set; }

        public AdminEntity IncludePhone(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Phone"))
            {
                this.ColumnList.Add("Phone");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateTime", DbType = DbType.DateTime, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime CreateTime { get; set; }

        public AdminEntity IncludeCreateTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateTime"))
            {
                this.ColumnList.Add("CreateTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateIp", DbType = DbType.String, Length = 20, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string CreateIp { get; set; }

        public AdminEntity IncludeCreateIp(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateIp"))
            {
                this.ColumnList.Add("CreateIp");
            }
            return this;
        }

        [DataMapping(ColumnName = "CreateUser", DbType = DbType.String, Length = 30, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string CreateUser { get; set; }

        public AdminEntity IncludeCreateUser(bool flag)
        {
            if (flag && !this.ColumnList.Contains("CreateUser"))
            {
                this.ColumnList.Add("CreateUser");
            }
            return this;
        }

        [DataMapping(ColumnName = "LoginCount", DbType = DbType.Int32, Length = 4, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int32 LoginCount { get; set; }

        public AdminEntity IncludeLoginCount(bool flag)
        {
            if (flag && !this.ColumnList.Contains("LoginCount"))
            {
                this.ColumnList.Add("LoginCount");
            }
            return this;
        }

        [DataMapping(ColumnName = "Picture", DbType = DbType.String, Length = 60, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Picture { get; set; }

        public AdminEntity IncludePicture(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Picture"))
            {
                this.ColumnList.Add("Picture");
            }
            return this;
        }

        [DataMapping(ColumnName = "UpdateTime", DbType = DbType.DateTime, Length = 8, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public DateTime UpdateTime { get; set; }

        public AdminEntity IncludeUpdateTime(bool flag)
        {
            if (flag && !this.ColumnList.Contains("UpdateTime"))
            {
                this.ColumnList.Add("UpdateTime");
            }
            return this;
        }

        [DataMapping(ColumnName = "IsDelete", DbType = DbType.Int16, Length = 2, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int16 IsDelete { get; set; }

        public AdminEntity IncludeIsDelete(bool flag)
        {
            if (flag && !this.ColumnList.Contains("IsDelete"))
            {
                this.ColumnList.Add("IsDelete");
            }
            return this;
        }

        [DataMapping(ColumnName = "Status", DbType = DbType.Int16, Length = 2, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public Int16 Status { get; set; }

        public AdminEntity IncludeStatus(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Status"))
            {
                this.ColumnList.Add("Status");
            }
            return this;
        }

        [DataMapping(ColumnName = "DepartNum", DbType = DbType.String, Length = 20, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string DepartNum { get; set; }

        public AdminEntity IncludeDepartNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("DepartNum"))
            {
                this.ColumnList.Add("DepartNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "ParentCode", DbType = DbType.String, Length = 40, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string ParentCode { get; set; }

        public AdminEntity IncludeParentCode(bool flag)
        {
            if (flag && !this.ColumnList.Contains("ParentCode"))
            {
                this.ColumnList.Add("ParentCode");
            }
            return this;
        }

        [DataMapping(ColumnName = "RoleNum", DbType = DbType.String, Length = 20, CanNull = false, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string RoleNum { get; set; }

        public AdminEntity IncludeRoleNum(bool flag)
        {
            if (flag && !this.ColumnList.Contains("RoleNum"))
            {
                this.ColumnList.Add("RoleNum");
            }
            return this;
        }

        [DataMapping(ColumnName = "Remark", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = true)]
        public string Remark { get; set; }

        public AdminEntity IncludeRemark(bool flag)
        {
            if (flag && !this.ColumnList.Contains("Remark"))
            {
                this.ColumnList.Add("Remark");
            }
            return this;
        }

    }

    public partial class AdminEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMapping(ColumnName = "RoleName", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string RoleName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMapping(ColumnName = "DepartName", DbType = DbType.String, Length = 40, CanNull = true, DefaultValue = null, PrimaryKey = false, AutoIncrement = false, IsMap = false)]
        public string DepartName { get; set; }
    }
}
