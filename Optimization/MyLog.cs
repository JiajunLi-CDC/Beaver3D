using System;
using System.Drawing;
using System.Windows.Forms;
using Beaver3D.Properties;

namespace Beaver3D.Optimization
{
	// Token: 0x02000013 RID: 19
	public partial class MyLog : Form
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00004D1C File Offset: 0x00002F1C
		public MyLog(FormStartPosition StartPos = FormStartPosition.Manual, Point Location = default(Point), string LogFormName = "MyLogFormName")
		{
			this.ESCKeyPressed = false;
			base.Name = LogFormName;
			base.FormBorderStyle = FormBorderStyle.Sizable;
			base.SizeGripStyle = SizeGripStyle.Auto;
			base.Width = 1200;
			base.Height = 1600;
			base.KeyPreview = true;
			base.StartPosition = StartPos;
			base.Location = Location;
			base.Icon = Resources.ConsoleIcon;
			this.Text = "Gurobi Optimization Log. Form Name: " + base.Name + ". Press [F1] to abort optimization";
			base.BringToFront();
		}

		// Token: 0x04000057 RID: 87
		public bool ESCKeyPressed = false;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MyLog
            // 
            this.ClientSize = new System.Drawing.Size(1815, 1848);
            this.Name = "MyLog";
            this.ResumeLayout(false);

        }
    }
}
