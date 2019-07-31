using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clientTest2
{
	public partial class SendMesage : Form
	{
		Form1 frm1;
		string target;
		public SendMesage()
		{
			InitializeComponent();
		}
		public SendMesage(Form1 frm, string target)
		{
			InitializeComponent();
			this.target = target;
			this.Text = "To : " + target;
			this.frm1 = frm;
		}

		private void SendBtn_Click(object sender, EventArgs e)
		{
			frm1.MsgSender(target, SendText.Text);
			this.Close();
		}
	}
}
