using System;
using System.Drawing;
using System.Windows.Forms;

namespace autosize
{
    /// <summary>
    /// 控件自适应缩放管理器
    /// 支持窗体初始最大化状态下的正确缩放
    /// </summary>
    public class ControlScaler
    {
        // 基准尺寸（设计时的原始尺寸）
        private float _baseWidth;
        private float _baseHeight;

        // 关联的窗体
        private readonly Form _targetForm;
        // 标记是否已初始化基准尺寸
        private bool _isInitialized = false;

        /// <summary>
        /// 初始化控件缩放管理器
        /// </summary>
        /// <param name="form">需要应用缩放的窗体</param>
        public ControlScaler(Form form)
        {
            _targetForm = form ?? throw new ArgumentNullException(nameof(form));

            // 绑定窗体事件
            _targetForm.Load += OnFormLoaded;
            _targetForm.Resize += OnFormResized;
        }

        /// <summary>
        /// 窗体加载完成事件
        /// </summary>
        private void OnFormLoaded(object sender, EventArgs e)
        {
            // 保存当前窗口状态
            FormWindowState originalState = _targetForm.WindowState;

            // 临时将窗口恢复为正常状态以获取设计尺寸
            if (_targetForm.WindowState == FormWindowState.Maximized)
            {
                _targetForm.WindowState = FormWindowState.Normal;
            }

            // 记录初始设计尺寸作为基准
            _baseWidth = _targetForm.Width;
            _baseHeight = _targetForm.Height;

            // 初始化控件的Tag属性（存储初始尺寸信息）
            SaveControlInitialSizes(_targetForm);

            // 恢复窗口状态
            _targetForm.WindowState = originalState;

            _isInitialized = true;

            // 如果是最大化状态，触发一次缩放
            if (originalState == FormWindowState.Maximized)
            {
                OnFormResized(sender, e);
            }
        }

        /// <summary>
        /// 窗体尺寸改变事件
        /// </summary>
        private void OnFormResized(object sender, EventArgs e)
        {
            // 确保初始化完成并且窗体不是最小化状态
            if (_isInitialized && _targetForm.WindowState != FormWindowState.Minimized)
            {
                // 获取当前实际尺寸（考虑最大化情况）
                int currentWidth = _targetForm.WindowState == FormWindowState.Maximized
                    ? Screen.FromControl(_targetForm).WorkingArea.Width
                    : _targetForm.Width;

                int currentHeight = _targetForm.WindowState == FormWindowState.Maximized
                    ? Screen.FromControl(_targetForm).WorkingArea.Height
                    : _targetForm.Height;

                // 计算缩放比例
                float scaleX = currentWidth / _baseWidth;
                float scaleY = currentHeight / _baseHeight;

                // 应用缩放
                ScaleControls(scaleX, scaleY, _targetForm);
            }
        }

        /// <summary>
        /// 保存控件初始尺寸信息到Tag属性
        /// </summary>
        private void SaveControlInitialSizes(Control container)
        {
            foreach (Control control in container.Controls)
            {
                // 存储格式：宽度;高度;左边距;顶边距;字体大小
                control.Tag = $"{control.Width};{control.Height};{control.Left};{control.Top};{control.Font.Size}";

                // 递归处理子控件
                if (control.Controls.Count > 0)
                {
                    SaveControlInitialSizes(control);
                }
            }
        }

        /// <summary>
        /// 根据缩放比例调整控件大小和位置
        /// </summary>
        private void ScaleControls(float scaleX, float scaleY, Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control.Tag != null)
                {
                    string[] sizeInfo = control.Tag.ToString().Split(';');
                    if (sizeInfo.Length == 5)
                    {
                        try
                        {
                            // 应用缩放
                            control.Width = (int)(float.Parse(sizeInfo[0]) * scaleX);
                            control.Height = (int)(float.Parse(sizeInfo[1]) * scaleY);
                            control.Left = (int)(float.Parse(sizeInfo[2]) * scaleX);
                            control.Top = (int)(float.Parse(sizeInfo[3]) * scaleY);

                            // 调整字体大小
                            float newFontSize = float.Parse(sizeInfo[4]) * scaleY;
                            // 限制字体大小在合理范围内
                            newFontSize = Math.Max(6, Math.Min(newFontSize, 24));
                            control.Font = new Font(control.Font.Name, newFontSize, control.Font.Style);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"缩放控件 {control.Name} 时出错: {ex.Message}");
                        }
                    }
                }

                // 递归处理子控件
                if (control.Controls.Count > 0)
                {
                    ScaleControls(scaleX, scaleY, control);
                }
            }
        }
    }
}
