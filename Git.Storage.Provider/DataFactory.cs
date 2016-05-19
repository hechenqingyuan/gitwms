/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2013-08-19 10:22:44
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2013-08-19 10:22:44       情缘
*********************************************************************************/

using Git.Framework.Resource;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Storage.DataAccess.Bad;
using Git.Storage.DataAccess.Base;
using Git.Storage.DataAccess.Check;
using Git.Storage.DataAccess.InStorage;
using Git.Storage.DataAccess.Move;
using Git.Storage.DataAccess.Order;
using Git.Storage.DataAccess.OutStorage;
using Git.Storage.DataAccess.Procedure;
using Git.Storage.DataAccess.Report;
using Git.Storage.DataAccess.Return;
using Git.Storage.DataAccess.Store;
using Git.Storage.IDataAccess.Bad;
using Git.Storage.IDataAccess.Base;
using Git.Storage.IDataAccess.Check;
using Git.Storage.IDataAccess.InStorage;
using Git.Storage.IDataAccess.Move;
using Git.Storage.IDataAccess.Order;
using Git.Storage.IDataAccess.OutStorage;
using Git.Storage.IDataAccess.Procedure;
using Git.Storage.IDataAccess.Report;
using Git.Storage.IDataAccess.Return;
using Git.Storage.IDataAccess.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Storage.Provider
{
    public partial class DataFactory
    {

        public DataFactory()
        {
            string v1 = ResourceManager.GetSettingEntity("Sign").Value;
            int index=v1.LastIndex("TaSJ6cBqg");
            if (index != 65)
            {
                throw new Exception("");
            }
        }

        /************************************ Procedure 命名空间 *************************************/

        public IProc_SwiftNum Proc_SwiftNum { get { return new Proc_SwiftNumDataAccess(); } }

        public ITNum TNum { get { return new TNumDataAccess(); } }

        public ISequence Sequence { get { return new SequenceDataAccess(); } }

        /************************************ Base 命名空间 *************************************/
        public ISysDepart SysDepart { get { return new SysDepartDataAccess(); } }

        public ISysRole SysRole { get { return new SysRoleDataAccess(); } }

        public IAdmin Admin { get { return new AdminDataAccess(); } }

        public ISysResource SysResource { get { return new SysResourceDataAccess(); } }

        public ISysRelation SysRelation { get { return new SysRelationDataAccess(); } }

        public IVnProvince VnProvince { get { return new VnProvinceDataAccess(); } }

        public IVnCity VnCity { get { return new VnCityDataAccess(); } }

        public IVnArea VnArea { get { return new VnAreaDataAccess(); } }

        public IMeasure Measure { get { return new MeasureDataAccess(); } }

        public IMeasureRel MeasureRel { get { return new MeasureRelDataAccess(); } }

        /************************************ InStorage 命名空间 *************************************/

        public IInStorage InStorage { get { return new InStorageDataAccess(); } }

        public IInStorDetail InStorDetail { get { return new InStorDetailDataAccess(); } }

        public IInStorSingle InStorSingle { get { return new InStorSingleDataAccess(); } }

        public IProc_AuditeInStorage Proc_AuditeInStorage { get { return new Proc_AuditeInStorageDataAccess(); } }

        public IProc_InStorageReport Proc_InStorageReport { get { return new Proc_InStorageReportDataAccess(); } }

        /************************************ OutStorage 命名空间 *************************************/

        public IOutStorage OutStorage { get { return new OutStorageDataAccess(); } }

        public IOutStoDetail OutStoDetail { get { return new OutStoDetailDataAccess(); } }

        public IOutStorSingle OutStorSingle { get { return new OutStorSingleDataAccess(); } }

        public IProc_AuditeOutStorage Proc_AuditeOutStorage { get { return new Proc_AuditeOutStorageDataAccess(); } }

        public IProc_OutStorageReport Proc_OutStorageReport { get { return new Proc_OutStorageReportDataAccess(); } }
        /************************************ Store 命名空间 *************************************/

        public IStorage Storage { get { return new StorageDataAccess(); } }

        public ILocation Location { get { return new LocationDataAccess(); } }

        public IEquipment Equipment { get { return new EquipmentDataAccess(); } }

        public ISupplier Supplier { get { return new SupplierDataAccess(); } }

        public ICustomer Customer { get { return new CustomerDataAccess(); } }

        public ICusAddress CusAddress { get { return new CusAddressDataAccess(); } }

        public IProc_ProductReport Proc_ProductReport { get { return new Proc_ProductReportDataAccess(); } }

        public IProductCategory ProductCategory { get { return new ProductCategoryDataAccess(); } }

        public IProduct Product { get { return new ProductDataAccess(); } }

        public ILocalProduct LocalProduct { get { return new LocalProductDataAccess(); } }

        /************************************ InventoryBook 命名空间 *************************************/

        public IInventoryBook InventoryBook { get { return new InventoryBookDataAccess(); } }

        /************************************ Bad 命名空间 *************************************/

        public IBadReport BadReport { get { return new BadReportDataAccess(); } }

        public IBadReportDetail BadReportDetail { get { return new BadReportDetailDataAccess(); } }

        public IProc_AuditeBadReport Proc_AuditeBadReport { get { return new Proc_AuditeBadReportDataAccess(); } }

        public IProc_BadTOP10Num Proc_BadTOP10Num { get { return new Proc_BadTOP10NumDataAccess(); } }

        /************************************ Move 命名空间 *************************************/

        public IMoveOrder MoveOrder { get { return new MoveOrderDataAccess(); } }

        public IMoveOrderDetail MoveOrderDetail { get { return new MoveOrderDetailDataAccess(); } }

        public IProc_AuditeMove Proc_AuditeMove { get { return new Proc_AuditeMoveDataAccess(); } }

        /************************************ Return 命名空间 *************************************/
        public IReturnOrder ReturnOrder { get { return new ReturnOrderDataAccess(); } }

        public IReturnDetail ReturnDetail { get { return new ReturnDetailDataAccess(); } }

        public IProc_AuditeReturn Proc_AuditeReturn { get { return new Proc_AuditeReturnDataAccess(); } }

        public IProc_ReturnTOP10Num Proc_ReturnTOP10Num { get { return new Proc_ReturnTOP10NumDataAccess(); } }

        /************************************ Check 命名空间 *************************************/
        public ICheckStock CheckStock { get { return new CheckStockDataAccess(); } }

        public ICheckStockInfo CheckStockInfo { get { return new CheckStockInfoDataAccess(); } }

        public ICheckData CheckData { get { return new CheckDataDataAccess(); } }

        public ICloneTemp CloneTemp { get { return new CloneTempDataAccess(); } }

        public ICloneHistory CloneHistory { get { return new CloneHistoryDataAccess(); } }

        public IProc_CreateCheck Proc_CreateCheck { get { return new Proc_CreateCheckDataAccess(); } }

        public IProc_AuditeCheck Proc_AuditeCheck { get { return new Proc_AuditeCheckDataAccess(); } }


        /************************************ Order 命名空间 *************************************/

        public IOrders Orders { get { return new OrdersDataAccess(); } }

        public IOrderDetail OrderDetail { get { return new OrderDetailDataAccess(); } }

        /************************************ Report 命名空间 *************************************/

        public IReports Reports { get { return new ReportsDataAccess(); } }

        public IReportParams ReportParams { get { return new ReportParamsDataAccess(); } }
    }
}
