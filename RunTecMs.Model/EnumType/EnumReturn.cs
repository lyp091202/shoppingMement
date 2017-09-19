namespace RunTecMs.Model.EnumType
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public enum EnumGettingData
    {
        /// <summary>
        /// 资源不足，请稍后再试
        /// </summary>
        PoolError = -4,
        /// <summary>
        /// 超出个数
        /// </summary>
        BeyondNumbers = -3,
        /// <summary>
        /// 超出次数
        /// </summary>
        BeyondTimes = -2,
        /// <summary>
        /// 失败
        /// </summary>
        Failure = -1,
        /// <summary>
        /// 没有获取权限
        /// </summary>
        NoAvailableData = 0,
        /// <summary>
        /// 获取成功
        /// </summary>
        Succeed = 1
    }

    /// <summary>
    /// 操作，处理返回值
    /// </summary>
    public enum EnumOperationReturn
    {
        /// <summary>
        /// 参数数据有误
        /// </summary>
        DataWrong = -2,
        /// <summary>
        /// 已存在，已处理
        /// </summary>
        Existing = -1,
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 1
    }

    public enum TestType
    {
        /// <summary>
        /// 成功（正确）
        /// </summary>
        success = 000,

        /// <summary>
        /// 文本框为空
        /// </summary>
        Empty = 001,

        /// <summary>
        /// 已存在
        /// </summary>
        Existence = 002,

        /// <summary>
        /// 未修改
        /// </summary>
        Undisposed = 003,

    }
}
