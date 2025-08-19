using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autosize
{
    public partial class Form1 : Form
    {
        // 声明缩放管理器
        private ControlScaler _controlScaler;

        public Form1()
        {
            InitializeComponent();
            // 初始化缩放管理器，传入当前窗体
            _controlScaler = new ControlScaler(this);
        }
    }
}
