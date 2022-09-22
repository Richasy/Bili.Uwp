// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Toolbox
{
    /// <summary>
    /// AV/BV互转视图模型的接口定义.
    /// </summary>
    public interface IAvBvConverterViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 转换命令.
        /// </summary>
        IRelayCommand ConvertCommand { get; }

        /// <summary>
        /// 输入的Id.
        /// </summary>
        string InputId { get; set; }

        /// <summary>
        /// 输出的Id.
        /// </summary>
        string OutputId { get; }

        /// <summary>
        /// 是否出错.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// 错误信息.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// 是否正在转换中.
        /// </summary>
        bool IsConverting { get; }
    }
}
