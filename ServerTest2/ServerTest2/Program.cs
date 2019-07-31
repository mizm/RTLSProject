using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace ServerTest2
{
	public class DBconnector
	{
		MySqlConnection conn;

		public DBconnector()
		{
			string connstr = string.Format(@"server=localhost; user=root; password=didehgur; database=test;");
			this.conn = new MySqlConnection(connstr);
			try
			{
				this.conn.Open();
				Console.WriteLine("DB Connected");
			}
			catch
			{
				this.conn.Close();
				Console.WriteLine("DB Fail");
			}
		}
		public string GetXY(string id)
		{
			try
			{
				string strSelect = "SELECT * FROM user WHERE ID = +" + id + ";";
				MySqlCommand cmd1 = new MySqlCommand(strSelect, this.conn);
				MySqlDataReader rdr = cmd1.ExecuteReader();
				if (!rdr.Read())
				{
					rdr.Close();
					return "Error - data is not selected";
				}
				else
				{
					string x = rdr.GetString("X");
					string y = rdr.GetString("Y");
					string re = x + "," + y;
					rdr.Close();
					return re;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return "Error";
		}
		public void SetXY(string ID, int x, int y)
		{
			string sql = "UPDATE user Set X = " + x.ToString() +", Y = " + y.ToString() +" WHERE ID = "+ID;//, Y = " + y.ToString() +
			string sql2 = "INSERT INTO path(ID,X,Y) VALUES (" + ID + "," + x + "," + y + ")";
			try
			{
				MySqlCommand cmd1 = new MySqlCommand(sql, this.conn);
				cmd1.ExecuteNonQuery();//user 테이블에 x y변경
				MySqlCommand cmd2 = new MySqlCommand(sql2, this.conn);
				cmd2.ExecuteNonQuery();//path 테이블에 변경점 저장
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}

		}
		public string GetPath(string ID)
		{
			string path = "PATH#" + ID;
			DataSet ds = new DataSet();
			try
			{
				string sql = "SELECT * FROM path WHERE ID = " + ID + " ORDER BY no DESC";
				MySqlDataAdapter adapter = new MySqlDataAdapter();
				adapter.SelectCommand = new MySqlCommand(sql, this.conn);
				adapter.Fill(ds);
				
				if (ds.Tables.Count > 0)
				{
					
					int count = 0;
					foreach (DataRow r in ds.Tables[0].Rows)
					{
						if (count > 10) break;
						path += "#"+ r["x"] +","+ r["y"];
						count++;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return path;
		}
		public Boolean checkID(string id)
		{
			string strSelect = "SELECT * FROM user WHERE ID = +"+id+";";
			try
			{
				MySqlCommand cmd1 = new MySqlCommand(strSelect, this.conn);
				MySqlDataReader rdr = cmd1.ExecuteReader();
				if (!rdr.Read())
				{
					rdr.Close();
					return false;
				}
				else
				{
					rdr.Close();
					return true;
				}
			}
			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}
			return false;
			//Console.WriteLine(str_Temp);//읽어온 데이터 출력
		}
	}

	class Program
	{

		static Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();
		static Dictionary<string, TcpClient> clientListR = new Dictionary<string, TcpClient>();
		static DBconnector db;
		static void Main(string[] args)
		{
			Console.WriteLine("gg");

			//Console.WriteLine(db.checkID("123"));
			//Console.WriteLine(db.GetXY("123"));
			 db = new DBconnector();
			//DataSet d = db.GetPath("123");
			Thread serverThread = new Thread(ServerFunc);
			serverThread.IsBackground = true;
			serverThread.Start();

			//db.Reader();
			Console.Read();
			
			/*byte []test = null;
			test = Encoding.Unicode.GetBytes("B");
			Console.WriteLine(test[0]);
			*/
		}
		private static void ServerFunc(object obj)
		{
			
			TcpListener server = new TcpListener(IPAddress.Any, 9999);
			TcpClient clientSocket = default(TcpClient);
			server.Start();
			Console.WriteLine("Server Started");
			
			while (true)
			{
				try
				{
					clientSocket = server.AcceptTcpClient();

					NetworkStream stream = clientSocket.GetStream();
					byte[] header = new byte[4];
					stream.Read(header, 0, 4);
					int s = BitConverter.ToInt32(header, 0);
					byte[] body = new byte[s];
					stream.Read(body, 0, s);
					string msg1 = Encoding.Unicode.GetString(body, 0, s);
					string[] msgsplit = msg1.Split('#');
					Console.WriteLine(msg1);
					if (LoginUser(msgsplit[0], msgsplit[1], clientSocket))
					{
						handleClient h_client = new handleClient();
						h_client.OnReceived += new handleClient.MessageHandler(OnReceived);
						h_client.OnDisconnected += new handleClient.DisconnectedHandler(OnDisconnected);
						h_client.startClient(clientSocket, clientList);
					}
				}
				catch (SocketException e)
				{

				}
				catch (Exception s)
				{

				}
			}
			clientSocket.Close();
			server.Stop();


		}
		static void OnDisconnected(TcpClient clientSocket)
		{
			if (clientList.ContainsKey(clientSocket))
			{
				clientListR.Remove(clientList[clientSocket]);
				clientList.Remove(clientSocket);
				LoginListSender();
			}
		}
		static Boolean LoginUser(string cmd, string username, TcpClient clientSocket)
		{
			string sendMsg;
			if (cmd.Equals("LOGINTRY"))
			{
				if (db.checkID(username))
				{
					sendMsg = "LOGINACCEPT#"+username;
				}
				else
				{
					sendMsg = "LOGINFAIL#"+username;
				}
				//Console.WriteLine(sendMsg);
				MessageSender(clientSocket, sendMsg);
				return false;
			}
			else if (cmd.Equals("LOGIN"))
			{
				clientList.Add(clientSocket, username);
				clientListR.Add(username, clientSocket);
				Console.WriteLine(username + " 접속");
				LoginListSender();
				//danger 좌표 송신
				sendMsg = null;
				sendMsg = "MAPDATA#70,70#150,150#300,250";
				MessageSender(clientList[clientSocket], sendMsg);
				if (!username.Equals("MONITOR"))
				{
					sendMsg = "XYDATA#" + db.GetXY(username);
					MessageSender(clientList[clientSocket], sendMsg);
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		static void LoginListSender()
		{
			String sendMsg = "LIST";
			foreach (var var in clientList)
			{
				sendMsg += "#" + var.Value;
			}
			//Console.WriteLine(sendMsg);
			MessageSenderAll(sendMsg);
		}
		static void MessageSender(string target, string msg)
		{
			TcpClient client = clientListR[target];
			NetworkStream stream = client.GetStream();
			byte[] body = null;
			byte[] header = null;
			Console.WriteLine(msg);
			body = Encoding.Unicode.GetBytes(msg);
			int size = body.Length;
			int buffersize = 4 + size;
			byte[] buffer = new byte[buffersize];
			header = BitConverter.GetBytes(size);
			Array.Copy(header, 0, buffer, 0, header.Length);
			Array.Copy(body, 0, buffer, header.Length, body.Length);
			stream.Write(buffer, 0, buffer.Length); // 버퍼 쓰기
			stream.Flush();

		}
		static void MessageSender(TcpClient client, string msg)
		{
			NetworkStream stream = client.GetStream();
			byte[] body = null;
			byte[] header = null;

			body = Encoding.Unicode.GetBytes(msg);
			int size = body.Length;
			int buffersize = 4 + size;
			byte[] buffer = new byte[buffersize];
			header = BitConverter.GetBytes(size);
			Array.Copy(header, 0, buffer, 0, header.Length);
			Array.Copy(body, 0, buffer, header.Length, body.Length);
			stream.Write(buffer, 0, buffer.Length); // 버퍼 쓰기
			stream.Flush();

		}
		static void MsgControl(string[] msgsplit)
		{
			
			string send, from, to;
			from = msgsplit[1];
			to = msgsplit[2];
			send = msgsplit[3];
			if (msgsplit.Length > 4)
			{
				for (int i = 4; i < msgsplit.Length; i++)
				{
					send += "#"+msgsplit[i];
				}
			}
			MessageSender(to, "MSG#" + from + "#" + send);

		}
		static void MessageSenderAll(string msg)
		{
			foreach (var var in clientList)
			{
				MessageSender(var.Value, msg);
			}
		}
		static void OnReceived(string msg, TcpClient clientSocket)
		{
			string[] msgsplit = msg.Split('#');
			Console.WriteLine(msg);
			if (msgsplit[0].Equals("MSG"))
			{
				MsgControl(msgsplit);
			}
			else if (msgsplit[0].Equals("RTDATA"))
			{
				string ID = msgsplit[1];
				int x = Convert.ToInt32(msgsplit[2].Split(',')[0]);
				int y = Convert.ToInt32(msgsplit[2].Split(',')[1]);
				string temp = db.GetXY(ID);
				int Orix = Convert.ToInt32(temp.Split(',')[0]);
				int Oriy = Convert.ToInt32(temp.Split(',')[1]);
				if (Orix != x || Oriy != y)
				{
					db.SetXY(ID, x, y);
					string sendmsg = "XYDATA#" + ID + "#" + x + "," + y;
					if (clientListR.ContainsKey("MONITOR"))
						MessageSender("MONITOR", sendmsg);
				}
			}
			else if (msgsplit[0].Equals("PATH"))
			{
				string ID = msgsplit[1];
				MessageSender("MONITOR",db.GetPath(ID));

			}
		}
	}
}
