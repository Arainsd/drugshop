﻿#region AgileEAS.NET-generated
//------------------------------------------------------------------------------
//     AgileEAS.NET应用开发平台，是基于敏捷并行开发思想以及.NET构件技术而开发的一个应用系统快速开发平台，用于帮助中小软件企业
//建立一条适合快速变化的开发团队，以达到节省开发成本、缩短开发时间，快速适应市场变化的目的。
//     AgileEAS.NET应用开发平台包含基础类库、资源管理平台、运行容器、开发辅助工具等四大部分，资源管理平台为敏捷并行开发提供了
//设计、实现、测试等开发过程的并行，应用系统的各个业务功能子系统，在系统体系结构设计的过程中被设计成各个原子功能模块，各个子
//功能模块按照业务功能组织成单独的程序集文件，各子系统开发完成后，由AgileEAS.NET资源管理平台进行统一的集成部署。
//
//     AgileEAS.NET SOA 中间件平台是一套免费的快速开发工具，可以不受限制的用于各种非商业开发之中，商业应用请向作者获取商业授权，
//商业授权也是免费的，但是对于非授权的商业应用视为侵权，开发人员可以参考官方网站和博客园等专业网站获取公开的技术资料，也可以向
//AgileEAS.NET官方团队请求技术支持。
//
// 官方网站：http://www.smarteas.net
// 团队网站：http://www.agilelab.cn
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由AgileEAS.NET数据模型设计工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
#endregion

using System;
using System.ComponentModel;
using EAS.Data.ORM;
using System.Runtime.Serialization;

namespace DrugShop.Entities
{
   /// <summary>
   /// 实体对象 Inventory(盘存记录)。
   /// </summary>
   [Serializable()]
   [Table("DS_INVENTORY","盘存记录")]
   partial class Inventory: DataEntity<Inventory>, IDataEntity<Inventory>
   {
       public Inventory()
       {
       }
       
       protected Inventory(SerializationInfo info, StreamingContext context)
           : base(info, context)
       {
       }
       
       #region O/R映射成员

       /// <summary>
       /// ID 。
       /// </summary>
       [Column("ID","ID"),DataSize(10),PrimaryKey]
       [DisplayName("ID")]
       public int ID
       {
           get;
           set;
       }

       /// <summary>
       /// 药品编码 。
       /// </summary>
       [Column("CODE","药品编码"),DataSize(16)]
       [DisplayName("药品编码")]
       public string Code
       {
           get;
           set;
       }

       /// <summary>
       /// 药品ID 。
       /// </summary>
       [Column("DrugID","药品ID"),DataSize(32)]
       [DisplayName("药品ID")]
       public string DrugID
       {
           get;
           set;
       }

       /// <summary>
       /// 批号 。
       /// </summary>
       [Column("BatchID","批号"),DataSize(32)]
       [DisplayName("批号")]
       public string BatchID
       {
           get;
           set;
       }

       /// <summary>
       /// 药品名称 。
       /// </summary>
       [Column("CName","药品名称"),DataSize(64)]
       [DisplayName("药品名称")]
       public string ChinseName
       {
           get;
           set;
       }

       /// <summary>
       /// 规格 。
       /// </summary>
       [Column("SPEC","规格"),DataSize(32)]
       [DisplayName("规格")]
       public string Spec
       {
           get;
           set;
       }

       /// <summary>
       /// 单位 。
       /// </summary>
       [Column("UNIT","单位"),DataSize(16)]
       [DisplayName("单位")]
       public string Unit
       {
           get;
           set;
       }

       /// <summary>
       /// 进价 。
       /// </summary>
       [Column("JobPrice","进价"),DataSize(18,4)]
       [DisplayName("进价")]
       public decimal JobPrice
       {
           get;
           set;
       }

       /// <summary>
       /// 销售价 。
       /// </summary>
       [Column("SalePrice","销售价"),DataSize(18,4)]
       [DisplayName("销售价")]
       public decimal SalePrice
       {
           get;
           set;
       }

       /// <summary>
       /// 账存 。
       /// </summary>
       [Column("NUMBER","账存"),DataSize(10)]
       [DisplayName("账存")]
       public int Number
       {
           get;
           set;
       }

       /// <summary>
       /// 实存 。
       /// </summary>
       [Column("RealNumber","实存"),DataSize(10)]
       [DisplayName("实存")]
       public int RealNumber
       {
           get;
           set;
       }

       /// <summary>
       /// 下限 。
       /// </summary>
       [Column("DownLimit","下限"),DataSize(10)]
       [DisplayName("下限")]
       public int DownLimit
       {
           get;
           set;
       }

       /// <summary>
       /// 上限 。
       /// </summary>
       [Column("UpLimit","上限"),DataSize(10)]
       [DisplayName("上限")]
       public int UpLimit
       {
           get;
           set;
       }

       /// <summary>
       /// 类型 。
       /// </summary>
       [Column("TYPE","类型"),DataSize(10)]
       [DisplayName("类型")]
       public int Type
       {
           get;
           set;
       }

       /// <summary>
       /// 效期 。
       /// </summary>
       [Column("TimeLimit","效期"),DataSize(23,3)]
       [DisplayName("效期")]
       public DateTime TimeLimit
       {
           get;
           set;
       }

       /// <summary>
       /// 供应商 。
       /// </summary>
       [Column("PROVIDER","供应商"),DataSize(50)]
       [DisplayName("供应商")]
       public string Provider
       {
           get;
           set;
       }

       /// <summary>
       /// 盘存时间 。
       /// </summary>
       [Column("EventTime","盘存时间"),DataSize(23,3)]
       [DisplayName("盘存时间")]
       public DateTime EventTime
       {
           get;
           set;
       }

       /// <summary>
       /// 原因 。
       /// </summary>
       [Column("CAUSE","原因"),DataSize(128)]
       [DisplayName("原因")]
       public string Cause
       {
           get;
           set;
       }

       /// <summary>
       /// 状态 。
       /// </summary>
       [Column("STATE","状态"),DataSize(10)]
       [DisplayName("状态")]
       public int State
       {
           get;
           set;
       }

       /// <summary>
       /// 输入码 。
       /// </summary>
       [Column("InputCode1","输入码"),DataSize(64)]
       [DisplayName("输入码")]
       public string InputCode1
       {
           get;
           set;
       }
       
       #endregion
   }
}
