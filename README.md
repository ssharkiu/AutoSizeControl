# AutoSizeControl - WinForms 控件自适应缩放

一个轻量、易用的 C# 类，用于实现 Windows Forms 窗体中控件的自适应缩放，支持窗体最大化、窗口化状态下的自动布局调整，解决高分辨率屏幕下的界面错位问题。

## 🌟 功能特点

- ✅ 自动记录控件初始设计尺寸（通过 `Tag` 属性存储）
- ✅ 支持窗体**初始即最大化**状态下的正确缩放
- ✅ 同时缩放控件的**大小、位置和字体**
- ✅ 响应式布局：随窗体缩放实时调整控件
- ✅ 轻量无依赖：仅一个 `.cs` 文件，无需额外资源
- ✅ 安全可靠：自动跳过未初始化或异常控件

## 🚀 使用方法

### 1. 添加类到项目
将 `ControlScaler.cs` 文件添加到你的 WinForms 项目中。

### 2. 创建缩放管理器实例
在窗体构造函数或 `Load` 事件中初始化：

```csharp
public partial class Form1 : Form
{
    private readonly ControlScaler _scaler;

    public Form1()
    {
        InitializeComponent();
        
        // 启用自适应缩放
        _scaler = new ControlScaler(this);
    }
}
