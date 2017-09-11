using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Data.SqlClient;

namespace RunTecMs.Model.Parameter
{
    public class ParaStruct
    {
        /// <summary>
        /// 定义公司参数结构
        /// </summary>
        public struct CompanyStruct
        {
            public string Name;
            public string Code;
            public DateTime StartTime;
            public DateTime EndTime;
            public int CompanyID;
        }

        /// <summary>
        /// 定义部门参数结构
        /// </summary>
        public struct DepartStruct
        {
            /// <summary>
            /// 部门名
            /// </summary>
            public string Name;
            /// <summary>
            /// 部门代码
            /// </summary>
            public string Code;
            /// <summary>
            /// 所属公司ID
            /// </summary>
            public int CompanyID;
        }

        /// <summary>
        /// 定义员工参数结构
        /// </summary>
        public struct EmployeeStruct
        {
            /// <summary>
            /// 员工ID
            /// </summary>
            public int EmployeeID;

            /// <summary>
            /// 角色ID
            /// </summary>
            public int RoleID;

            /// <summary>
            /// 最大角色ID
            /// </summary>
            public int MaxRoleID;

            /// <summary>
            /// 所属公司ID
            /// </summary>
            public int CompanyID;

            /// <summary>
            /// 部门ID
            /// </summary>
            public int DepID;

            /// <summary>
            /// 业务ID
            /// </summary>
            public int BusinessValue;

            /// <summary>
            /// 员工名
            /// </summary>
            public string Name;

            /// <summary>
            /// 手机号
            /// </summary>
            public string Phone;

            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime? StartTime;

            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime? EndTime;

            /// <summary>
            /// 分配状态
            /// </summary>
            public int DistributionStatus;

            /// <summary>
            /// 离职状态
            /// </summary>
            public string DepartureStatus;

            /// <summary>
            /// 当前页数
            /// </summary>
            public int page;

            /// <summary>
            /// 每页数据
            /// </summary>
            public int rows;
        }
        
        /// <summary>
        /// 业务
        /// </summary>
        public struct Business
        {
            //业务ID
            public string BusinessID;
            //原业务ID
            public string ReBusinessID;
            //业务名称
            public string Name;
            //业务值（新的）
            public string BusinessValue;
            // 业务名（后台所用）
            public string BusinessName;
            //排序
            public string OrderValue;
            //超管标识
            public int IsSYSDBA;
        
        }
        

        public struct PromotionType
        {
            /// <summary>
            /// 活动类型ID
            /// </summary>
            public string PromotionTypeID;
            /// <summary>
            /// 名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 值
            /// </summary>
            public string Value;
            /// <summary>
            /// 排序
            /// </summary>
            public int OrderValue;
            /// <summary>
            /// 追加时间
            /// </summary>
            public string AddTime;
            /// <summary>
            /// 超管标识
            /// </summary>
            public bool IsSYSDBA;
            /// <summary>
            /// 是否删除
            /// </summary>
            public bool IsDel;
        }

       

        /// <summary>
        /// 客户归属参数结构
        /// </summary>
        public struct CustomerOwenShip
        {
            /// <summary>
            /// 登录名
            /// </summary>
            public string LoginName;

            /// <summary>
            /// 登录员工ID
            /// </summary>
            public int LoginEmployeeID;

            /// <summary>
            /// 最大权限的角色ID
            /// </summary>
            public int MaxRoleID;

            /// <summary>
            /// 公司ID
            /// </summary>
            public int CompanyID;

            /// <summary>
            /// 部门ID
            /// </summary>
            public int DepID;

            /// <summary>
            /// 员工名
            /// </summary>
            public string EmployeeName;
            /// <summary>
            /// 员工Id
            /// </summary>
            public int EmployeeID;

            /// <summary>
            /// 客户名
            /// </summary>
            public string CustomerName;

            /// <summary>
            /// 客户登录名
            /// </summary>
            public string CustLoginName;

            /// <summary>
            /// 客户昵称
            /// </summary>
            public string CustomerNickName;

            /// <summary>
            /// 客户QQ
            /// </summary>
            public string CustomerQQ;

            /// <summary>
            /// 客户微信
            /// </summary>
            public string CustomerWechat;

            /// <summary>
            /// 业务ID
            /// </summary>
            public int BusinessID;

            /// <summary>
            /// 客户手机号
            /// </summary>
            public string CustomerPhone;
            /// <summary>
            /// 员工手机号
            /// </summary>
            public string EmployeePhone;
            /// <summary>
            /// 页码
            /// </summary>
            public int pageIndex;
            /// <summary>
            /// 表示件数
            /// </summary>
            public int pageSize;
            /// <summary>
            /// 分配状态
            /// </summary>
            public int DistributionStatus;
            /// <summary>
            /// 归属状态
            /// </summary>
            public int OwnershipStatus;
            /// <summary>
            /// 查询方式
            /// </summary>
            public int SearchKbn;
            /// <summary>
            /// 注册时间（开始查询时间）
            /// </summary>
            public string RegistStartTime;
            /// <summary>
            /// 注册时间（结束查询时间）
            /// </summary>
            public string RegistEndTime;
            /// <summary>
            /// 客户开始时间（开始查询时间）
            /// </summary>
            public string StartDateStartTime;
            /// <summary>
            /// 客户结束时间（结束查询时间）
            /// </summary>
            public string StartDateEndTime;
            /// <summary>
            /// 跟踪类型
            /// </summary>
            public int TraceTypeID;
            /// <summary>
            /// 客户来源
            /// </summary>
            public string AgentString;
            /// <summary>
            /// 排序项目
            /// </summary>
            public string SortItem;
            /// <summary>
            /// 排序条件
            /// </summary>
            public string SortCon;
        }


        /// <summary>
        /// 导出类
        /// </summary>
        public class ExportCustomerList
        {
            //昵称
            public string CustomerNickName { get; set; }
            //真名
            public string NickName { get; set; }
            //手机
            public string CustomerPhone { get; set; }
            //微信
            public string CustomerWechat { get; set; }
            //QQ
            public string CustomerQQ { get; set; }
            //所属员工
            public string EmployeeName { get; set; }
            //业务
            public string BusinessName { get; set; }
            //分配时间
            public DateTime DistributionTime { get; set; }
            //归属关系
            public string OwnershipStatusName { get; set; }
            //public int OwnershipStatus { get; set; }
            //归属时间
            public string BelongTime { get; set; }
            //是否升级
            public string IsUpdateCustomer { get; set; }
            //升级员工
            public string UpEmployeeName { get; set; }
            //注册时间
            public DateTime RegistTime { get; set; }
            //客户来源
            public string AgentString { get; set; }
            //QQ验证消息
            public string QQVerifyInfo { get; set; }
            //public int TraceTypeId { get; set; }
            //跟踪
            public string TraceTypeName { get; set; }
            //资源池
            public string SourcePool { get; set; }
            //public string IsDistributionTemp { get; set; }
            
        }


        public class Export
        {
            public string[] ColumnsName { get; set; }
            public string FileName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
        }

        

        public struct ModulePage
        {
            public string ModuleID;
            //名称
            public string Name;
            //对应关系值
            public string Value;
            //父菜单
            public string ParentModuleID;
            /// <summary>
            /// 路径
            /// </summary>
            public string Path;
            /// <summary>
            /// 是否表示
            /// </summary>
            public string Enabled;
            /// <summary>
            /// 说明
            /// </summary>
            public string Description;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;

        }

        
        /// <summary>
        /// app渠道
        /// </summary>
        public struct AppMarketChannel // ChannelName, AddTime, deletetime, IsSYSDBA
        {
            /// <summary>
            /// 页数
            /// </summary>
            public int pageIndex;
            /// <summary>
            /// 每页的个数
            /// </summary>
            public int pageSize;
            /// <summary>
            /// 渠道名称
            /// </summary>
            public string ChannelName;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string starttime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime;
            /// <summary>
            /// 添加时间
            /// </summary>
            public string AddTime;
            /// <summary>
            /// 删除时间
            /// </summary>
            public string deletetime;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;
        }

        
        /// <summary>
        /// 敏感字
        /// </summary>
        public struct SensitiveWord
        {
            /// <summary>
            /// 页码
            /// </summary>
            public int PageIndex;
            /// <summary>
            /// 页面尺寸
            /// </summary>
            public int PageSize;
            /// <summary>
            /// 内容
            /// </summary>
            public string Content;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime;
            /// <summary>
            /// 分类
            /// </summary>
            public string Classify;
            /// <summary>
            /// 级别
            /// </summary>
            public string Level;
            /// <summary>
            /// 预备1
            /// </summary>
            public string Remark1;
            /// <summary>
            /// 预备2
            /// </summary>
            public string Remark2;
            /// <summary>
            /// 添加时间
            /// </summary>
            public string AddTime;

        }
        /// <summary>
        /// 职位的参数
        /// </summary>
        public struct JobPage
        {
            /// <summary>
            /// 职位名称
            /// </summary>
            public string jobName;
            /// <summary>
            /// 职位值
            /// </summary>
            public string jobValue;
            /// <summary>
            /// 部门
            /// </summary>
            public string department;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;


        }
        /// <summary>
        /// 角色
        /// </summary>
        public struct RolePage
        {
            /// <summary>
            /// 角色名
            /// </summary>
            public string Name;
            /// <summary>
            /// 角色值
            /// </summary>
            public string Value;
            /// <summary>
            /// 角色数据范围
            /// </summary>
            public string RoleDataRange;
            /// <summary>
            /// 角色描述
            /// </summary>
            public string Descrption;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;


        }
        /// <summary>
        /// 数据范围
        /// </summary>
        public struct DataRange
        {
            /// <summary>
            /// 数据范围名
            /// </summary>
            public string Name;
            /// <summary>
            /// 数据范围值
            /// </summary>
            public string Value;
            /// <summary>
            /// 数据范围
            /// </summary>
            public string DialogDataRange;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        public struct LogError
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName;
            /// <summary>
            /// 员工名称
            /// </summary>
            public string EmployeeName;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime;
            /// <summary>
            /// 错误url
            /// </summary>
            public string ErrorUrl;
            /// <summary>
            /// 错误动作
            /// </summary>
            public string ErrorAction;
            /// <summary>
            /// 错误类名
            /// </summary>
            public string ErrorClassName;
            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrorInfo;
            /// <summary>
            /// 业务ID
            /// </summary>
            public string BusinessID;

        }
        /// <summary>
        /// 画面管理
        /// </summary>
        public struct PageList
        {
            public string PageIDStr;
            public int BusinessID;
            public string PageID;
            public string PageName;
            public string BelongAction;
            public string Comment;
            public string IsUsing;
            public string AddUser;
            public string UpdateUser;
            public string starttime;
            public string endTime;
            public string PageBusID;
        }
        /// <summary>
        /// 主题使用场所
        /// </summary>
        public struct ThemeUsedPlace
        {
            /// <summary>
            /// 使用说明
            /// </summary>
            public string SiteName;
            /// <summary>
            /// 超管标识    
            /// </summary>
            public int IsSYSDBA;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string starttime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime;

        }
        /// <summary>
        /// 主题用途区分
        /// </summary>
        public struct ThemeMaster
        {
            /// <summary>
            /// 区分说明
            /// </summary>
            public string MasterName;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string starttime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;
        }
        /// <summary>
        /// 主题管理
        /// </summary>
        public struct ThemeManager
        {
            public string Flag;
            /// <summary>
            /// 业务
            /// </summary>
            public string BusinessID;
            /// <summary>
            /// 用途
            /// </summary>
            public string MasterID;
            /// <summary>
            /// 使用场所
            /// </summary>
            public string ThemeUsedPlaceID;
            /// <summary>
            /// 表示文字名称
            /// </summary>
            public string WordName;
            /// <summary>
            /// 图片路径
            /// </summary>
            public string ImagePath;
            /// <summary>
            /// 迁移画面ID
            /// </summary>
            public string MovePageID;
            /// <summary>
            /// 迁移条件
            /// </summary>
            public string MoveCondition;
            /// <summary>
            /// 主题管理ID
            /// </summary>
            public string ThemeManagerID;
            /// <summary>
            /// 图片位置
            /// </summary>
            public string ImageSite;
            //画面名称
            public string PageName;
        }

        /// <summary>
        /// 商户管理
        /// </summary>
        public struct MerchantManager
        {
            /// <summary>
            /// 商户ID
            /// </summary>
            public int MerchantID;
            /// <summary>
            /// 业务ID
            /// </summary>
            public int BusinessValue;
            /// <summary>
            /// 商户名
            /// </summary>
            public string MerchantName;
            /// <summary>
            /// 密码
            /// </summary>
            public string Password;
            /// <summary>
            /// 密码盐
            /// </summary>
            public string Passwordsalt;
            /// <summary>
            /// 商户账户
            /// </summary>
            public string MerchantAccount;
            /// <summary>
            /// 商户类型ID
            /// </summary>
            public int MerchantTypeID;
            /// <summary>
            /// 手续费比例
            /// </summary>
            public decimal Poundage;
            /// <summary>
            /// 打款方式
            /// </summary>
            public int PaymentType;
            /// <summary>
            /// 自动打款时间
            /// </summary>
            public string PaymentTime;
            /// <summary>
            /// 商户账户类型ID
            /// </summary>
            public int MerchantAccountTypeID;
            /// <summary>
            /// 是否正在使用
            /// </summary>
            public string Isusing;
            /// <summary>
            /// 地址
            /// </summary>
            public string Address;
            /// <summary>
            /// 联系电话
            /// </summary>
            public string Mobile;
            /// <summary>
            /// 邮箱
            /// </summary>
            public string Email;
            /// <summary>
            /// 追加时间
            /// </summary>
            public string AddTime;
            /// <summary>
            /// 追加者
            /// </summary>
            public string AddUser;
            /// <summary>
            /// 更新时间
            /// </summary>
            public string UpdateTime;
            /// <summary>
            /// 更新者
            /// </summary>
            public string UpdateUser;

            public string starttime;

            public string endTime;

            public string MerchantTypeIDStr;
            public string MerchantIDStr;

        }

        /// <summary>
        /// 发发版本管理
        /// </summary>
        public struct FafaVersion
        {

            /// 版本ID
            /// </summary>
            public int VersionID;
            /// <summary>
            /// 渠道ID
            /// </summary>
            public string ChannelName;
            /// <summary>
            /// 版本名称
            /// </summary>
            public string VersionName;
            /// <summary>
            /// 版本编码
            /// </summary>
            public int VersionCode;
            /// <summary>
            /// 升级描述
            /// </summary>
            public string UpdateDescription;
            /// <summary>
            /// 下载链接
            /// </summary>
            public string Link;
            /// <summary>
            /// 系统版本
            /// </summary>
            public string MinimumOsVersion;
            /// <summary>
            /// 是否使用
            /// </summary>
            public bool IsEnable;
            /// <summary>
            /// 是否使用
            /// </summary>
            public int AddIsEnable;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string starttime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime;
            /// <summary>
            /// 是否正在使用
            /// </summary>
            public string SearchIsEnable;

        }

        /// <summary>
        /// 内容管理
        /// </summary>
        public struct ContentManagement
        {
            public string IsAdd;
            public string ContentID;
            public string Title;
            public string BusinessID;
            public string ContentType;
            public string SubTitle;
            public string Price;
            public string OnTop;
            public string Summary;
            public string Sequence;
            public string content;
            public string ImageUrl;
            public string starttime;
            public string endTime;
            public string LastModCustomerID;
            public string ContentTypeID;
            public string TypeExplain;
            public string SubscriptionID;
            public string CreateUser;
            public bool isPush;
            public string IsPublic;


        }

        /// <summary>
        /// 文章类型
        /// </summary>
        public struct ContentType
        {
            public string TypeID;

            public string BusinessValue;

            public string Name;

            public string Value;

            public string Description;

            public string AddEmpID;

            public int IsSYSDBA;

            public string starttime;

            public string endTime;

            public string timestamp;
            public string AddTime;
            public string SubscriptionID;

        }

        /// <summary>
        /// 大决策活动参数
        /// </summary>
        public struct Promotion
        {

            /// <summary>
            /// 活动ID
            /// </summary>
            public int PromotionID;
            /// <summary>
            /// 业务值
            /// </summary>
            public int? BusinessValue;
            /// <summary>
            /// 活动内容
            /// </summary>
            public string PromotionContent;
            /// <summary>
            /// 申请人
            /// </summary>
            public int EmployeeID;
            /// <summary>
            /// 申请人昵称
            /// </summary>
            public string NickName;

            /// <summary>
            /// 申请时间
            /// </summary>
            public string  ApplyTime;

            /// <summary>
            /// 开始时间
            /// </summary>
            public string  StartTime;

            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime;

            /// <summary>
            /// 审批时间
            /// </summary>
            public DateTime ExamineAndApproveTime;

            /// <summary>
            /// 审批状态
            /// </summary>
            public int ExamineAndApproveStatus;

            /// <summary>
            /// 审批状态
            /// </summary>
            public string  ExamineAndApproveStatusID;
            /// <summary>
            /// 审批内容
            /// </summary>
            public string EAContent;

            /// <summary>
            /// 正常价
            /// </summary>
            public  decimal  Price;

            /// <summary>
            /// 折扣价
            /// </summary>
            public decimal  DiscountPrice;

            /// <summary>
            /// 结束时间修改次数
            /// </summary>
            public int EndTimeModifyCount;

            /// <summary>
            /// 修改次数
            /// </summary>
            public int ModifyCount;

   
        
        }
        /// <summary>
        /// 活动参与信息参数
        /// </summary>
        public struct PromotionParticipation 
        {
            /// <summary>
            /// 参与ID
            /// </summary>
            public string ParticipationID;

            /// <summary>
            /// 活动ID
            /// </summary>
            public int? PromotionID;
            /// <summary>
            /// 渠道ID
            /// </summary>
            public int? CUSChannelID;
            /// <summary>
            /// 渠道
            /// </summary>
            public int? CUSChannel;
            /// <summary>
            /// 客户ID
            /// </summary>
            public int? CustomerID;
            /// <summary>
            /// 客户昵称
            /// </summary>
            public string NickName;
            /// <summary>
            /// 手机号
            /// </summary>
            public string Mobile;
            /// <summary>
            /// qq
            /// </summary>
            public string QQ;
            /// <summary>
            /// 微信
            /// </summary>
            public string WeChat;
           /// <summary>
           /// 活动
           /// </summary>
            public string Promotion;
            /// <summary>
            /// 入金金额
            /// </summary>
            public decimal  Deposits;

            /// <summary>
            /// 入金时间
            /// </summary>
            public string  DepositTime;

            /// <summary>
            /// 购买产品ID
            /// </summary>
            public int ProductID;

            /// <summary>
            /// 购买价格
            /// </summary>
            public decimal ProductPrice;

            /// <summary>
            /// 服务开始时间
            /// </summary>
            public string  StartTime;

            /// <summary>
            /// 服务结束时间
            /// </summary>
            public string  EndTime;

            /// <summary>
            /// 参与状态
            /// </summary>
            public int StatusID;

            /// <summary>
            /// 参与时间
            /// </summary>
            public string  AddTime;

            /// <summary>
            /// 服务id
            /// </summary>
            public string ServiceStatusID;
            /// <summary>
            /// 参与id
            /// </summary>
            public string ParticStatusID;

        
        
        }

        /// <summary>
        /// 权限集
        /// </summary>
        public struct PermissionSet
        {
            /// <summary>
            /// ID
            /// </summary>
            public string  PermissionID;
            /// <summary>
            /// 数据模型ID
            /// </summary>
            public string ModuleID;
            /// <summary>
            /// 角色ID
            /// </summary>
            public string RoleID;
            /// <summary>
            /// 部门ID
            /// </summary>
            public string DepID;
            /// <summary>
            /// 权限名称
            /// </summary>
            public string PermissionName;
            /// <summary>
            /// 后台管理区分
            /// </summary>
            public string ManagerTypeID;
            /// <summary>
            /// 操作ID集合
            /// </summary>
            public string OpIDs;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;
            /// <summary>
            /// 角色名称
            /// </summary>
            public string RoleName;
            /// <summary>
            /// 业务名称
            /// </summary>
            public string BusinessName;
            /// <summary>
            /// 业务值
            /// </summary>
            public string BusinessValue;
            /// <summary>
            /// 管理名称
            /// </summary>
            public string ManagerName;
            /// <summary>
            /// 新的ModuleID
            /// </summary>
            public string NewModuleID;
            /// <summary>
            /// 新的ModuleText
            /// </summary>
            public string NewModuleText;
            /// <summary>
            /// 旧的OldModuleID
            /// </summary>
            public string OldModuleID;
            /// <summary>
            /// 模板和后台区分拼成的字符串
            /// </summary>
            public string ModuleManagerTypeID;
        }
        /// <summary>
        /// 获取资源设置
        /// </summary>
        public struct ReSourceSet
        {
            /// <summary>
            /// 角色部门区分
            /// </summary>
            public string RoleDepDiff;
            /// <summary>
            /// 设置类型
            /// </summary>
            public string SetType;
            /// <summary>
            /// 资源获取设置ID
            /// </summary>
            public string  ReSourceSetID;
            /// <summary>
            /// 业务值
            /// </summary>
            public int BusinessValue;
            /// <summary>
            /// 角色ID
            /// </summary>
            public int RoleID;
            /// <summary>
            /// 员工ID
            /// </summary>
            public int EmployeeID;
            /// <summary>
            /// 当前登录的员工ID
            /// </summary>
            public int CurrentEmployeeID;
            /// <summary>
            /// 公共资源个数
            /// </summary>
            public int PubReSourceNumber;
            /// <summary>
            /// 新资源个数
            /// </summary>
            public int NewReSourceNumber;
            /// <summary>
            /// 旧资源个数
            /// </summary>
            public int OldReSourceNumber;
            /// <summary>
            /// 获取资源次数
            /// </summary>
            public int ReSourceCount;
            /// <summary>
            /// 追加时间
            /// </summary>
            public DateTime AddTime;
            /// <summary>
            /// 删除时间
            /// </summary>
            public DateTime DeleteTime;
            /// <summary>
            /// 超管标识
            /// </summary>
            public int IsSYSDBA;
            /// <summary>
            /// 业务值
            /// </summary>
            public string BusinessName;
            /// <summary>
            /// 角色名
            /// </summary>
            public string RoleName;
            /// <summary>
            /// 员工昵称
            /// </summary>
            public string NickName;
            /// <summary>
            /// 业务值str
            /// </summary>
            public string BusinessValueStr;
            /// <summary>
            /// 角色值str
            /// </summary>
            public string RoleIDStr;
            /// <summary>
            /// 当前业务值
            /// </summary>
            public int CurrentBusinessValue;
            /// <summary>
            /// 部门ID
            /// </summary>
            public int depID;
            /// <summary>
            /// 部门ID
            /// </summary>
            public string depIDStr;
            /// <summary>
            /// 部门名称
            /// </summary>
            public string depName;
            /// <summary>
            /// 最大部门
            /// </summary>
            public int maxDep;
            /// <summary>
            /// 公司ID
            /// </summary>
            public int CompanyID;

        }

        /// <summary>
        /// 客户归属参数结构
        /// </summary>
        public struct EmployeeInfo
        {
            /// <summary>
            /// 员工ID
            /// </summary>
            public int EmployeeID;

            /// <summary>
            /// 公司ID
            /// </summary>
            public int CompanyID;

            /// <summary>
            /// 部门ID
            /// </summary>
            public int DepID;

            /// <summary>
            /// 员工名
            /// </summary>
            public string EmployeeName;

            /// <summary>
            /// 业务值
            /// </summary>
            public int BusinessValue;

            /// <summary>
            /// 角色ID
            /// </summary>
            public int RoleID;
        }

        /// <summary>
        /// 垃圾客户申请
        /// </summary>
        public struct RubbishCustomerExamine
        {
            /// <summary>
            /// 客户ID（各客户ID用，隔开）
            /// </summary>
            public string CustomerID;

            /// <summary>
            /// 业务值
            /// </summary>
            public int BusinessID;

            /// <summary>
            /// 公司ID
            /// </summary>
            public int CompanyID;

            /// <summary>
            /// 部门ID
            /// </summary>
            public int DepID;

            /// <summary>
            /// 员工ID
            /// </summary>
            public int EmployeeID;

            /// <summary>
            /// 原因
            /// </summary>
            public string Reason;

            /// <summary>
            /// 审批内容
            /// </summary>
            public string EAContent;
        }

        /// <summary>
        /// 更改资源池类型
        /// </summary>
        public struct ChangeResPoolType
        {
            /// <summary>
            /// 客户id
            /// </summary>
            public string CustomerID;
            /// <summary>
            /// 资源池类型
            /// </summary>
            public int ResPoolType;
            /// <summary>
            /// 业务值
            /// </summary>
            public int intBusinessValue;
            /// <summary>
            /// 是否新客户
            /// </summary>
            public int IsNewCustomer;
            /// <summary>
            /// 操作类型
            /// </summary>
            public string OperateType;
            /// <summary>
            /// 发发业务值
            /// </summary>
            public string FaFaBusinessValue;
            /// <summary>
            /// 大决策业务值
            /// </summary>
            public string DJCBusinessValue;
            /// <summary>
            /// 沐融业务值
            /// </summary>
            public string MRBusinessValue;
            /// <summary>
            /// FaFa客户
            /// </summary>
            public string FaFaCustomerID;
            /// <summary>
            /// DJC客户
            /// </summary>
            public string DJCCustomerID;
            /// <summary>
            /// 沐融客户
            /// </summary>
            public string MRCustomerID;
            /// <summary>
            /// 业务值(来源)
            /// </summary>
            public int fromBusinessVlue;

        }

        /// <summary>
        /// 变更软件有效期
        /// </summary>
        public struct ChangeExpiryDate
        {
            /// <summary>
            /// 客户id
            /// </summary>
            public string CustomerID;
            /// <summary>
            /// 变更后有效期
            /// </summary>
            public string ExpiryDate;
            /// <summary>
            /// 变更后级别
            /// </summary>
            public int changeLevel;
        }

        /// <summary>
        /// 员工客户参数
        /// </summary>
        public struct CustomerOwnership
        {
            //public string startTime;
            //public string endTime;
            public string BelongType;
            public string AgentString;
            public int  TraceType;
            public int CustomerLevel;
            public int CUSChannelID;
            public int  StatisticalItem;
            public int CompanyID;
            public int DepID;
            public int EmployeeID;
            public int BusinessValue;
            public string EmployeeIDs;
            public int  CurrentBusinessValue;
            /// <summary>
            /// 开始分配时间
            /// </summary>
            public string distributStartTime;
            /// <summary>
            /// 结束分配时间
            /// </summary>
            public string distributEndTime;
            /// <summary>
            /// 注册开始时间
            /// </summary>
            public string registerStartTime;
            /// <summary>
            /// 注册结束时间
            /// </summary>
            public string registerEndTime;
            /// <summary>
            /// 统计全部
            /// </summary>
            public bool ToStatistics;
            /// <summary>
            /// 当前员工id
            /// </summary>
            public int CurrentEmployeeID;
        
        }

        /// <summary>
        /// 微信订单
        /// </summary>
        public struct WxBuyRecord
        {
            /// <summary>
            /// 订单ID
            /// </summary>
            public string  BuyRecordID{get;set;}
            /// <summary>
            /// 价格
            /// </summary>
            public int Price { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string  StartTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string  EndTime { get; set; }
            /// <summary>
            /// 支付状态
            /// </summary>
            public int State { get; set; }
            /// <summary>
            /// 公众号名称
            /// </summary>
            public string MPName { get; set; }
            /// <summary>
            /// 昵称
            /// </summary>
            public string NickName { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public int Sex { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 支付状态
            /// </summary>
            public string ChargeStatusName { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            public string stateStr { get; set; }
            /// <summary>
            /// 用户id
            /// </summary>
            public int MPUserID { get; set; }
           /// <summary>
           ///会员等级ID 
           /// </summary>
            public int MembershipLevel { get; set; }
        
        }

        ///<summary>
        /// 临时消息
        /// </summary>
        public struct TempMessage
        {
            //房间号
            public string RoomId;
            //房间名
            public string RoomName;
            //消息号
            public string MessageId;
            //消息内容
            public string MessageContent;
            //消息更新时间
            public string MessageTime;
            //直播员
            public string CommentatorName;
        }

        /// <summary>
        /// 客户排名
        /// </summary>
        public struct CustomerOrder
        {
            /// <summary>
            /// 公司ID
            /// </summary>
            public int CompanyID{get;set;}
            /// <summary>
            /// 部门ID
            /// </summary>
            public int DepID { get; set; }
            /// <summary>
            /// 金额类型
            /// </summary>
            public string PriceType;
            /// <summary>
            /// 排名类型
            /// </summary>
            public string OrderType;
            /// <summary>
            /// 员工类型
            /// </summary>
            public string EmpType;
            /// <summary>
            /// 部门ID
            /// </summary>
            public int BusinessDepID;

            /// <summary>
            /// 业务值
            /// </summary>
            public int BusinessValue { get; set; }
            /// <summary>
            /// 公司ID
            /// </summary>
            public int SCompanyID { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string startTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime { get; set; }
            /// <summary>
            /// 获取一个员工标识
            /// </summary>
            public int CurrentEmployeeID { get; set; }
            /// <summary>
            /// 订单状态
            /// </summary>
            public string OrderStatus;
        
        }


        /// <summary>
        /// 订单
        /// </summary>
        public struct Order
        {
            public string IsAdd;
            // 订单ID
            public string  OrderId;

            // 订购用户
            public string  CustomerID;


            public string CustomerName;

            // 订购时间
            public string OrderTime;

            // 商品参与活动ID
            public string  PromotionID;

            // 订单金额
            public string  OrderPrice;

            // 优惠金额
            public string  PromotionPrice;

            // 支付金额
            public string  PayPrice;

            // 聊天记录
            public string ChatRecordPath;

            // 电话录音
            public string CallRecordPath;

            // 支付凭证
            public string PayOrderPath;

            // 合规录音
            public string ComplianceRecordPath;

            // 合同
            public string Contract;

            // 发票信息
            public string InvoiceInfo;

            // 邮寄地址
            public string MailAddress;

            // 是否开票
            public string  IsInvoice;

            // 是否已开票
            public string  AlreadyInvoice;

            // 订单状态
            public string  OrderStatus;

            // 支付ID
            public string RechargeID;

            // 订单编号
            public string OrderNumber;

            // 联系电话
            public string TelNum;

            // 是否删除
            public string IsDel;

            // 业务值
            public string BusinessValue;

            // 业务员ID
            public int BusinessEmpID;

            //public string BusinessEmpName;

            // 业务员公司ID
            public int BusinessCompanyID;

            //public string BusinessCompanyName;

            // 业务员部门ID
            public int BusinessDepID;

            //public string BusinessDepName;

            // 客服ID
            public int ServiceEmpID;

            //public string ServiceEmpName;

            // 客服公司ID
            public int ServiceCompanyID;

            //public string ServiceCompanyName;

            // 客服部门ID
            public int ServiceDepID;

            //public string ServiceDepName;

            // 升级员工ID
            public int UPEmployeeID;

            //public string UPEmployeeName;

            // 升级公司ID
            public int UPCompanyID;

            //public string UPCompanyName;

            // 升级部门ID
            public int UPDepID;

            //public string UPDepName;

            // 升级开始时间
            public string UPStartTime;

            // 升级终了时间
            public string UPEndTime;

            public string StartTime;

            public string EndTime;
            //订单类型
            public string  OrderType;

            //讲师
            public string LectureName;

            //订单内容
            public string PromotionContent;
            
            //活动类型
            public string  PromotionTypeID;

            //订阅类型
            public string  SubscriptionTypeID;

            //讲师
            public string  LectureID;

            //订阅开始时间
            public string  SubscriptionStartTime;

            //订阅结束时间
            public string  SubscriptionEndTime;

            //产品类型
            public string  ProductTypeID;

            //发币兑换率
            public string  FacoinRateTypeID;
            //变更原因
            public string ChangeReason;

            //财务审核信息
            public string FinanceDismissInfo;

            //财务审核时间
            public string FinanceDismissTime;

            //合规审核信息
            public string ComplianceDismissInfo;

            //合规审核时间
            public string ComplianceDismissTime;

            //订阅期间
            public string SubscriptionTime;

            //商品类型
            public string BuyType;

            //商品ID
            public string ProductID;

            //商品名
            public string ProductName;
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        public struct OrderDetail
        {
            // 订单详细内容ID
            public string DetailId;

            // 订单编号
            public string OrderId;

            // 商品类型ID
            public string  ProductTypeID;

            // 产品ID
            public string ProductId;

            // 开始时间
            public string StartTime;

            // 结束时间
            public string EndTime;

            // 订购数量
            public string Quantity;

            // 产品名称
            public string Name;

            // 价格
            public string Price;

            // 退款订单号
            public string RefundOrder;

            // 退款凭证
            public string RefundID;
        }


        //订单履历
        public struct OrderHistory
        {
            // 履历ID
            public string  HistoryID;

            // 订单ID
            public string OrderId;

            // 变更前订单状态
            public string BeforeOrderStatus;

            // 变更后订单状态
            public string AfterOrderStatus;

            // 添加时间
            public string AddTime;

            // 变更原因
            public string ChangeReason;

            // 订购用户
            public string CustomerID;

            // 订购时间
            public string OrderTime;

            // 商品参与活动ID
            public string PromotionID;

            // 订单金额
            public string OrderPrice;

            // 优惠金额
            public string PromotionPrice;

            // 支付金额
            public string PayPrice;

            // 聊天记录
            public string ChatRecordPath;

            // 电话录音
            public string CallRecordPath;

            // 支付凭证
            public string PayOrderPath;

            // 合规录音
            public string ComplianceRecordPath;

            // 合同
            public string Contract;

            // 发票信息
            public string InvoiceInfo;

            // 邮寄地址
            public string MailAddress;

            // 是否开票
            public string IsInvoice;

            // 是否已开票
            public string AlreadyInvoice;

            // 订单状态
            public string OrderStatus;

            // 支付ID
            public string RechargeID;

            // 订单编号
            public string OrderNumber;

            // 联系电话
            public string TelNum;

            // 是否删除
            public string IsDel;

            // 业务值
            public string BusinessValue;

            // 业务员ID
            public string BusinessEmpID;

            // 业务员公司ID
            public string BusinessCompanyID;

            // 业务员部门ID
            public string BusinessDepID;

            // 客服ID
            public string ServiceEmpID;

            // 客服公司ID
            public string ServiceCompanyID;

            // 客服部门ID
            public string ServiceDepID;

            // 升级员工ID
            public string UPEmployeeID;

            // 升级公司ID
            public string UPCompanyID;

            // 升级部门ID
            public string UPDepID;

            // 升级开始时间
            public string UPStartTime;

            // 升级终了时间
            public string UPEndTime;

            //订单类型
            public string OrderType;

            public string FinanceDismissInfo;

            public string FinanceDismissTime;

            public string ComplianceDismissInfo;

            public string ComplianceDismissTime;
        }


        //订单状态
        public struct OrderStatus
        {
            // 状态ID
            public string OrderStatusID;

            // 名称
            public string OrderStatusName;

            // 使用角色
            public string UsedRole;

            // 时间戳
            public string timestamp;
        }

        /// 评论参数
        /// </summary>
        public struct CommentPage
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ArticleCommentID;
            /// <summary>
            /// 评论内容
            /// </summary>
            public string Content;
            /// <summary>
            /// 评分
            /// </summary>
            public int Rating;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string starttime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime;
        }

        //大决策用户
        public struct DjcUser
        {
            public string CustomerID;

            public string CustomerTypeID;

            public string CustomerLevel;

            public string CustomerQualityID;

            public string SoftPurpose;

            public string CUSChannelID;

            public string Remark;

            public string RegisterTime;

            public string AppOpenID;

            public string MPOpenID;

            public string WebOpenID;

            public string UnionID;

            public string IsNewCustomer;

            public string NickNames;
            public string TrueNames;
            public string Mobiles;
        }

        //大决策用户
        public struct MRUser
        {
            public string CustomerID;

            public string CustomerTypeID;

            public string CustomerLevel;

            public string CustomerQualityID;

            public string SoftPurpose;

            public string CUSChannelID;

            public string Remark;

            public string RegisterTime;

            public string AppOpenID;

            public string MPOpenID;

            public string WebOpenID;

            public string UnionID;

            public string IsNewCustomer;

            public string NickNames;
            public string TrueNames;
            public string Mobiles;
            public string LoginNames;
        }

        /// <summary>
        /// 商品类别参数
        /// </summary>
        public struct ProductGroup
        {
            /// <summary>
            /// 讲师名称
            /// </summary>
            public string LectureName { get; set; }

            /// <summary>
            /// 类型ID
            /// </summary>
            public int ProductTypeId { get; set; }

            /// <summary>
            /// 类别名
            /// </summary>
            public string ProductGroupName { get; set; }

            /// <summary>
            /// 是否推荐
            /// </summary>
            public string IsRecommend { get; set; }

            /// <summary>
            /// 类别最小价格
            /// </summary>
            public string CoursePriceFrom { get; set; }

            /// <summary>
            /// 类别最大价格
            /// </summary>
            public string CoursePriceTo { get; set; }

            /// <summary>
            /// 发布开始时间
            /// </summary>
            public string PubStartTime { get; set; }

            /// <summary>
            /// 发布结束时间
            /// </summary>
            public string PubEndTime { get; set; }
        }

        /// <summary>
        /// 商品参数
        /// </summary>
        public struct Product
        {
            /// <summary>
            /// 商品ID
            /// </summary>
            public int ProductId { get; set; }

            /// <summary>
            /// 讲师名称
            /// </summary>
            public string LectureName { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 类型ID
            /// </summary>
            public int ProductTypeId { get; set; }

            /// <summary>
            /// 类别ID
            /// </summary>
            public string ProductGroupId { get; set; }

            /// <summary>
            /// 是否推荐
            /// </summary>
            public string IsRecommend { get; set; }

            /// <summary>
            /// 上架
            /// </summary>
            public Nullable<bool> IsSale { get; set; }

            /// <summary>
            /// 最小价格
            /// </summary>
            public string PriceFrom { get; set; }

            /// <summary>
            /// 最大价格
            /// </summary>
            public string PriceTo { get; set; }

            /// <summary>
            /// 发布开始时间
            /// </summary>
            public string PubStartTime { get; set; }

            /// <summary>
            /// 发布结束时间
            /// </summary>
            public string PubEndTime { get; set; }

            /// <summary>
            /// 商品状态
            /// </summary>
            public string ProductState { get; set; }
        }

        /// <summary>
        /// 直播房间参数
        /// </summary>
        public struct LiveRoom
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string RoomName { get; set; }

            /// <summary>
            /// 业务值
            /// </summary>
            public int BusinessValue { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            public string IsUsing { get; set; }

            /// <summary>
            /// 发布开始时间
            /// </summary>
            public string PubStartTime { get; set; }

            /// <summary>
            /// 发布结束时间
            /// </summary>
            public string PubEndTime { get; set; }
        }

        /// <summary>
        /// 直播流参数
        /// </summary>
        public struct LiveStream
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string LiveStreamName { get; set; }

            /// <summary>
            /// 应用名
            /// </summary>
            public string AppName { get; set; }

            /// <summary>
            /// 流名称
            /// </summary>
            public string StreamName { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            public string IsUsing { get; set; }
        }

        /// <summary>
        /// 基础表
        /// </summary>
        public struct BaseTablePara
        {
            /// <summary>
            /// 查询条件
            /// </summary>
            public string SearchCondition;
            /// <summary>
            /// 主键
            /// </summary>
            public string PkColumn;
            /// <summary>
            /// 查询参数
            /// </summary>
            public List<SqlParameter> ConditionParalist;
            /// <summary>
            /// 表名
            /// </summary>
            public string TableName;
            /// <summary>
            /// 表示项目
            /// </summary>
            public string ShowColumn;
            /// <summary>
            /// 排序条件
            /// </summary>
            public string SortCondition;
            /// <summary>
            /// 排序方式
            /// </summary>
            public int SortType;
            /// <summary>
            /// 页码
            /// </summary>
            public int PageIndex;
            /// <summary>
            /// 表示件数
            /// </summary>
            public int PageSize;
            /// <summary>
            /// 参数
            /// </summary>
            public CommonPara commonPara;
            
        }

        /// <summary>
        /// 基础表共通参数
        /// </summary>
        public struct CommonPara
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 内容/摘要
            /// </summary>
            public string Description;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string starttime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime;
            /// <summary>
            /// 超管标识
            /// </summary>
            public bool IsSYSDBA;
            /// <summary>
            /// 正在使用
            /// </summary>
            public string IsUsing;
        }

        /// <summary>
        /// 聊天室内容
        /// </summary>
        public struct ChatConntent
        {
            /// <summary>
            /// 聊天室
            /// </summary>
            public int RoomId { get; set; }

            /// <summary>
            /// 发送者
            /// </summary>
            public string SendUser { get; set; }

            /// <summary>
            /// 聊天内容
            /// </summary>
            public string ChatContent { get; set; }

            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime { get; set; }
        }

        /// <summary>
        /// 课程表参数
        /// </summary>
        public struct Syllabus
        {
            /// <summary>
            /// 课程类型名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            public string IsUsing { get; set; }

            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime { get; set; }
        }

        /// <summary>
        /// 入学测试参数
        /// </summary>
        public struct EntranceExamination
        {
            /// <summary>
            /// 客户名称
            /// </summary>
            public string CustomerName { get; set; }

            /// <summary>
            /// 客户登录名
            /// </summary>
            public string LoginName { get; set; }

            /// <summary>
            /// 最小分数
            /// </summary>
            public int FromScore { get; set; }

            /// <summary>
            /// 最大分数
            /// </summary>
            public int ToScore { get; set; }

            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime { get; set; }

            /// <summary>
            /// 类型
            /// </summary>
            public int SurverType { get; set; }

            /// <summary>
            /// 登录员工ID
            /// </summary>
            public int LoginEmpID { get; set; }
        }

        /// <summary>
        /// 答案详情参数
        /// </summary>
        public struct AnswerInfo
        {
            /// <summary>
            /// 试题ID
            /// </summary>
            public int SurveyId { get; set; }

            /// <summary>
            /// 客户ID
            /// </summary>
            public int CustomerID { get; set; }

            /// <summary>
            /// 回答时间
            /// </summary>
            public string AnswerTime { get; set; }

        }

        /// <summary>
        /// 客户日活详情
        /// </summary>
        public struct CustomerBehaviorDetail
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public string strStartTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public string strEndTime { get; set; }

            /// <summary>
            /// 登录员工ID
            /// </summary>
            public int EmployeeId { get; set; }

            /// <summary>
            /// 排序项目
            /// </summary>
            public string sort { get; set; }

            /// <summary>
            /// 排序条件
            /// </summary>
            public string order { get; set; }

        }
    }
}
