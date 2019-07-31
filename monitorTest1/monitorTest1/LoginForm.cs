using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace clientTest2
{
	
	public partial class LoginForm : Form
	{
		Boolean STAT = true;
		TcpClient clientSocket;
		NetworkStream stream = default(NetworkStream);
		Thread t_handler;
		public LoginForm()
		{
			InitializeComponent();
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
					byte[] body = new byte[s];
					stream.Read(body, 0, s);
					string msg = Encoding.Unicode.GetString(body, 0, s);
					string[] msgsplit = msg.Split('#');
					if (msgsplit[0].Equals("LOGINACCEPT"))
					{
						MessageBox.Show(msgsplit[1] + "님 접속을 환영합니다");
						Form1 f1 = (Form1)this.Owner;
						f1.myname = msgsplit[1];
						this.DialogResult = DialogResult.OK;
						break;
					}
					else if (msgsplit[0].Equals("LOGINFAIL"))
					{
						MessageBox.Show("ID가 없습니다");
						break;
					}
				}
				catch (SocketException ss)
				{
				}
			}
			
		}
		private void LoginBtn_Click(object sender, EventArgs e)
		{
			
			if (textBox1.Text.Length < 2)
			{
				MessageBox.Show("ID를 확인하세요");
				return;
			}
			//if(t_handler != null)
			//t_handler.Join();
			clientSocket = new TcpClient();
			clientSocket.Connect("192.168.0.11", 9999);
			///clientSocket.Connect("165.229.125.179", 9999);
			stream = clientSocket.GetStream();

			string str = "LOGINTRY#" + this.textBox1.Text;

			MsgSender(str);

			t_handler = new Thread(GetStream);
			t_handler.IsBackground = true;
			t_handler.Start();
		}
	}
}
