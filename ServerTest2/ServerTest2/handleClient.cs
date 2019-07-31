using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerTest2
{

	class handleClient
	{

		TcpClient clientSocket = null;
		public Dictionary<TcpClient, string> clientList = null;
		public void startClient(TcpClient clientSocket, Dictionary<TcpClient, string> clientList)
		{
			this.clientSocket = clientSocket;//클라이언트 소켓 받아와서 클라이언트 소켓에 할당
			this.clientList = clientList;//리스트에 할당

			Thread t_hanlder = new Thread(doChat);//쓰레드 할당
			t_hanlder.IsBackground = true;//백그라운드에 할당
			t_hanlder.Start();//실행
		}
		public delegate void MessageHandler(string msg, TcpClient clientSocket);
		public event MessageHandler OnReceived;

		public delegate void DisconnectedHandler(TcpClient clientSocket);
		public event DisconnectedHandler OnDisconnected;

		private void doChat()
		{
			NetworkStream stream = null;
			try
			{
		
				while (true)
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
							string msg1 = Encoding.Unicode.GetString(body, 0, s);
							if (OnReceived != null)
								OnReceived(msg1, clientSocket);
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
						string msg1 = Encoding.Unicode.GetString(body, 0, s);
						if (OnReceived != null)
							OnReceived(msg1, clientSocket);
					}
					
				}
			}
			catch (SocketException s)
			{
				if (clientSocket != null)
				{
					if (OnDisconnected != null)
						OnDisconnected(clientSocket);

					clientSocket.Close();
					stream.Close();
				}
			}
			catch (Exception s)
			{
				if (clientSocket != null)
				{
					if (OnDisconnected != null)
						OnDisconnected(clientSocket);

					clientSocket.Close();
					stream.Close();
				}
			}
		}
	}
}
