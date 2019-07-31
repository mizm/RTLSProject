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
	public partial class ReceiveMessage : Form
	{
		Form1 frm1;
		string from;
		public ReceiveMessage()
		{
			InitializeComponent();
		}

		private void RplyBtn_Click(object sender, EventArgs e)
		{
			frm1.MsgSender(from, ReplyText.Text);
			this.Close();
		}

		public ReceiveMessage(Form1 _form,string from, string msg )
		{
			InitializeComponent();
			this.Text = "From : " + from;
			ReceivedTextbox.AppendText(msg);
			frm1 = _form;
			this.from = from;

		}


	}
}
