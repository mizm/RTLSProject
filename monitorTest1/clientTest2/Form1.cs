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
		Dictionary<string, Label> namePoint = new Dictionary<string, Label>();
		List<Point> dangerxy = new List<Point>();
		string message = string.Empty;
		public string myname = "MONITOR";
		Image gpsimage = global::clientTest2.Properties.Resources.p11;
		public Image dangerimage = global::clientTest2.Properties.Resources.p;
		int gpssize = 15;
		int dangersize = 50;
		int x = 0;
		int y = 0;
		volatile Boolean LOGINSTAT;
		const Boolean LOGINOK = true;
		const Boolean LOGINFAIL = false;
		Graphics gr;//경로그리기 그래픽스
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
			//MsgSender("PATH#123");
			gr.Clear(Color.White);
			pictureBox1.Refresh();
		}
		private void ImgCreate(Boolean check, string name, Image img, int size, Dictionary<string, PictureBox> t, int x, int y)
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
				if (check == true)
				{
					Label lb = new Label();
					lb.AutoSize = true;
					lb.Location = new System.Drawing.Point(x, y - 10);
					lb.Name = name;
					lb.Size = new System.Drawing.Size(45, 15);
					lb.Text = name;
					pictureBox1.Controls.Add(lb);
					namePoint.Add(name, lb);
				}
			}));
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
					if (item.Equals("LIST")||item.Equals("MONITOR")) continue;

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
				Point temp = new Point(x, y);
				dangerxy.Add(temp);
				ImgCreate(false,item, dangerimage, dangersize, dangerPoint, x, y);
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
				string ID = msgsplit[1];
				int x = Convert.ToInt32(msgsplit[2].Split(',')[0]);
				int y = Convert.ToInt32(msgsplit[2].Split(',')[1]);
				if (gpsPoint.ContainsKey(ID))
				{
					this.Invoke(new Action(delegate ()
					{
						gpsPoint[ID].Location = new Point(x, y);
						namePoint[ID].Location = new Point(x, y - 10);
					}));

				}
				else
				{
					ImgCreate(true, ID, gpsimage, gpssize, gpsPoint, x, y);
				}
			}
			else if (msgsplit[0].Equals("PATH"))
			{
				gr = pictureBox1.CreateGraphics();
				Pen pen = new Pen(Color.Black, 3);
				for (int i = 2; i < msgsplit.Count(); i++)
				{
					if (i + 1 >= msgsplit.Count()) break;
					int x1 = Convert.ToInt32(msgsplit[i].Split(',')[0]);
					int y1 = Convert.ToInt32(msgsplit[i].Split(',')[1]);
					int x2 = Convert.ToInt32(msgsplit[i + 1].Split(',')[0]);
					int y2 = Convert.ToInt32(msgsplit[i + 1].Split(',')[1]);
					gr.DrawLine(pen, x1, y1, x2, y2);
				}
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
				try
				{
					//clientSocket.Connect("192.168.0.11", 9999);
					clientSocket.Connect("165.229.125.117", 9999);
					stream = clientSocket.GetStream();

					string str = "LOGIN#MONITOR";
					MsgSender(str);

					Thread t_handler = new Thread(GetStream);
					t_handler.IsBackground = true;
					t_handler.Start();

				}
				catch (SocketException es)
				{
					MessageBox.Show("서버 OFF");
				}
		}

		private void LoginListBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int index = this.LoginListBox.IndexFromPoint(e.Location);
				if (index != ListBox.NoMatches)
				{
					String item = LoginListBox.Items[index].ToString();
					ContextMenu cm = new ContextMenu();
					MenuItem menu1 = new MenuItem();
					MenuItem menu2 = new MenuItem();

					menu1.Text = "메시지보내기";
					menu2.Text = "경로보기";

					menu1.Click += (senders, es) => {
						if (!item.Equals(myname))
						{
							this.Invoke(new Action(delegate ()
							{
								SendMesage frm = new SendMesage(this, item);
								frm.Show();
							}));
						}
					};
					menu2.Click += (senders, es) => {
						MsgSender("PATH#" + item);
						
					};
					cm.MenuItems.Add(menu1);
					cm.MenuItems.Add(menu2);

					cm.Show(LoginListBox, new Point(e.X, e.Y));
					
				}
			}
		}
	}
}
