using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace clientTest2
{

	public partial class Form1 : Form
	{
		TcpClient clientSocket = new TcpClient();
		NetworkStream stream = default(NetworkStream);
		Dictionary<string ,PictureBox > gpsPoint = new Dictionary<string, PictureBox>();
		Dictionary<string, PictureBox> dangerPoint = new Dictionary<string, PictureBox>();
		string message = string.Empty;
		public string myname = string.Empty;
		Image gpsimage = global::clientTest2.Properties.Resources.p11;
		Image dangerimage = global::clientTest2.Properties.Resources.p;
		int gpssize = 15;
		int dangersize = 50;
		int x = 0;
		int y = 0;
		volatile Boolean LOGINSTAT;
		const Boolean LOGINOK = true;
		const Boolean LOGINFAIL = false;

		public Form1()
		{
			InitializeComponent();
		}
		public Form1(string username)
		{
			this.myname = username;
			InitializeComponent();
			
		}

		private void loginbtn_Click(object sender, EventArgs e)
		{	



		}
		private void ImgCreate(string name, Image img, int size, Dictionary<string, PictureBox> t, int x, int y)
		{
			this.Invoke(new Action(delegate ()
			{
				PictureBox pb = new PictureBox();
				pb.Image = img;
				pb.Location = new System.Drawing.Point(x, y);
				pb.Name = name;
				pb.Size = new System.Drawing.Size(size, size);
				pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				pb.Visible = true;
				pb.Show();
				pictureBox1.Controls.Add(pb);
				t.Add(name, pb);
			}));
		}
		private void RealTime()
		{
			
			while (true)
			{
				string str = "RTDATA#"+ myname + "#" + x.ToString() + "," + y.ToString();

				MsgSender(str);
				Thread.Sleep(2000);
			}
		}
		public void MsgSender(string target, string msg)
		{
			string str = "MSG#"+myname + "#" + target + "#" + msg;
			byte[] body = null;
			byte[] header = null;
			
			body = Encoding.Unicode.GetBytes(str);
			int size = body.Length;
			int buffersize = 4 + size;
			byte[] buffer = new byte[buffersize];
			header = BitConverter.GetBytes(size);
			Array.Copy(header, 0, buffer, 0, header.Length);
			Array.Copy(body, 0, buffer, header.Length, body.Length);
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		
		}
		public void MsgSender(string msg)
		{
			byte[] body = null;
			byte[] header = null;
			body = Encoding.Unicode.GetBytes(msg);
			int size = body.Length;
			int buffersize = 4 + size;
			byte[] buffer = new byte[buffersize];
			header = BitConverter.GetBytes(size);
			Array.Copy(header, 0, buffer, 0, header.Length);
			Array.Copy(body, 0, buffer, header.Length, body.Length);
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();

		}
		private void ListReader(string[] msgsplit)
		{
			this.Invoke(new Action(delegate ()
			{
				this.LoginListBox.Items.Clear();
				foreach (string item in msgsplit)
				{
					if (item.Equals("LIST")) continue;

					this.LoginListBox.Items.Add(item);
				}
			}));
		}
		private void MapDataControl(string[] msgsplit)
		{
			foreach (string item in msgsplit)
			{
				if (item.Equals("MAPDATA")) continue;
				int x = Convert.ToInt32(item.Split(',')[0]);
				int y = Convert.ToInt32(item.Split(',')[1]);
				ImgCreate(item, dangerimage, dangersize, dangerPoint, x, y);
			}
		}
		private void MsgControl(string[] msgsplit)
		{
			string from = msgsplit[1];
			string message = msgsplit[2];
			if (msgsplit.Length > 3)
			{
				for (int i = 3; i < msgsplit.Length; i++)
				{
					message += "#" + msgsplit[i];
				}
			}
			this.Invoke(new Action(delegate ()
			{
				ReceiveMessage frm = new ReceiveMessage(this, from, message);//
				frm.Show();
			}));
		}
		private void StreamControl(string msg)
		{
			string[] msgsplit = msg.Split('#');
			if (msgsplit[0].Equals("LIST"))
			{
				ListReader(msgsplit);
			}
			else if (msgsplit[0].Equals("MSG"))
			{
				MsgControl(msgsplit);
			}
			else if (msgsplit[0].Equals("MAPDATA"))
			{
				MapDataControl(msgsplit);
			}
			else if (msgsplit[0].Equals("XYDATA"))
			{			
				int x = Convert.ToInt32(msgsplit[1].Split(',')[0]);
				int y = Convert.ToInt32(msgsplit[1].Split(',')[1]);
				ImgCreate(myname, gpsimage, 15, gpsPoint, x,y);
				this.x = x;
				this.y = y;
			}


		}
		private void GetStream()
		{
			while (true)
			{
				try
				{
					stream = clientSocket.GetStream();
					int BUFFERSIZE = clientSocket.ReceiveBufferSize;
					byte[] header = new byte[4];
					stream.Read(header, 0, 4);
					int s = BitConverter.ToInt32(header, 0);
					if (s + 4 < BUFFERSIZE)
					{
						int chksize = 0;
						do
						{
							byte[] body = new byte[s];
							stream.Read(body, 0, s);
							string msg = Encoding.Unicode.GetString(body, 0, s);
							StreamControl(msg);
							chksize += 4;
							chksize += s;
							stream.Read(header, 0, 4);
							s = BitConverter.ToInt32(header, 0);
						}
						while (chksize < BUFFERSIZE);
					}
					else
					{
						byte[] body = new byte[s];
						stream.Read(body, 0, s);
						string msg = Encoding.Unicode.GetString(body, 0, s);
						StreamControl(msg);
					}
				}
				catch (SocketException ss)
				{
				}
			}
		}
		private void developbtn_Click(object sender, EventArgs e)
		{
			byte[] buffer = Encoding.Unicode.GetBytes(this.developTextbox.Text);
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
			{
				switch (e.KeyCode)
				{
					case Keys.Up:
						if (y >= 10)
							y -= 5;
						break;
					case Keys.Down:
						if (y <= 290)
							y += 5;
						break;
					case Keys.Right:
						if (x <= 490)
							x += 5;
						break;
					case Keys.Left:
						if (x >= 10)
							x -= 5;
						break;
				}
				Point p = new Point(x, y);
				gpsPoint[myname].Location = p;

				textBox1.AppendText(x + "," + y + "\n");
			}
		}

		private void LoginListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (LoginListBox.SelectedItems.Count == 1)
			{
				string select = LoginListBox.SelectedItem.ToString();
				if (!select.Equals(myname))
				{
					this.Invoke(new Action(delegate ()
					{
						SendMesage frm = new SendMesage(this, select);
						frm.Show();
					}));
				}
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
			LoginForm frm = new LoginForm();
			if (frm.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					clientSocket.Connect("192.168.0.11", 9999);
					///clientSocket.Connect("165.229.125.179", 9999);
					stream = clientSocket.GetStream();

					string str = "LOGIN#" + myname;
					MsgSender(str);

					Thread t_handler = new Thread(GetStream);
					t_handler.IsBackground = true;
					t_handler.Start();

					Thread RT_handler = new Thread(RealTime);
					RT_handler.IsBackground = true;
					RT_handler.Start();
					//ImgCreate(myname, gpsimage, 15, gpsPoint, 20, 20);
				}
				catch (SocketException es)
				{
					MessageBox.Show("서버 OFF");
				}
			}
			else
			{
				this.Close();
			}
		}
	}
}
