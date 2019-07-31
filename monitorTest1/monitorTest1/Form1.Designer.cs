namespace clientTest2
{
	partial class Form1
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.backPanel = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.developTextbox = new System.Windows.Forms.TextBox();
			this.developbtn = new System.Windows.Forms.Button();
			this.idtextbox = new System.Windows.Forms.TextBox();
			this.loginbtn = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.LoginListBox = new System.Windows.Forms.ListBox();
			this.backPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// backPanel
			// 
			this.backPanel.Controls.Add(this.pictureBox1);
			this.backPanel.Location = new System.Drawing.Point(10, 10);
			this.backPanel.Name = "backPanel";
			this.backPanel.Size = new System.Drawing.Size(600, 400);
			this.backPanel.TabIndex = 0;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Image = global::clientTest2.Properties.Resources.map;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(600, 400);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// developTextbox
			// 
			this.developTextbox.Location = new System.Drawing.Point(679, 396);
			this.developTextbox.Name = "developTextbox";
			this.developTextbox.Size = new System.Drawing.Size(382, 25);
			this.developTextbox.TabIndex = 0;
			// 
			// developbtn
			// 
			this.developbtn.Location = new System.Drawing.Point(1076, 398);
			this.developbtn.Name = "developbtn";
			this.developbtn.Size = new System.Drawing.Size(75, 23);
			this.developbtn.TabIndex = 1;
			this.developbtn.Text = "button1";
			this.developbtn.UseVisualStyleBackColor = true;
			this.developbtn.Click += new System.EventHandler(this.developbtn_Click);
			// 
			// idtextbox
			// 
			this.idtextbox.Location = new System.Drawing.Point(679, 12);
			this.idtextbox.Name = "idtextbox";
			this.idtextbox.Size = new System.Drawing.Size(184, 25);
			this.idtextbox.TabIndex = 2;
			// 
			// loginbtn
			// 
			this.loginbtn.Location = new System.Drawing.Point(912, 14);
			this.loginbtn.Name = "loginbtn";
			this.loginbtn.Size = new System.Drawing.Size(75, 23);
			this.loginbtn.TabIndex = 3;
			this.loginbtn.Text = "button1";
			this.loginbtn.UseVisualStyleBackColor = true;
			this.loginbtn.Click += new System.EventHandler(this.loginbtn_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(679, 73);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(382, 279);
			this.textBox1.TabIndex = 4;
			// 
			// LoginListBox
			// 
			this.LoginListBox.FormattingEnabled = true;
			this.LoginListBox.ItemHeight = 15;
			this.LoginListBox.Location = new System.Drawing.Point(1067, 73);
			this.LoginListBox.Name = "LoginListBox";
			this.LoginListBox.Size = new System.Drawing.Size(120, 274);
			this.LoginListBox.TabIndex = 5;
			this.LoginListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LoginListBox_MouseDoubleClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1215, 446);
			this.Controls.Add(this.LoginListBox);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.loginbtn);
			this.Controls.Add(this.idtextbox);
			this.Controls.Add(this.developbtn);
			this.Controls.Add(this.developTextbox);
			this.Controls.Add(this.backPanel);
			this.KeyPreview = true;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.backPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel backPanel;
		private System.Windows.Forms.TextBox developTextbox;
		private System.Windows.Forms.Button developbtn;
		private System.Windows.Forms.TextBox idtextbox;
		private System.Windows.Forms.Button loginbtn;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ListBox LoginListBox;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}

