namespace clientTest2
{
	partial class SendMesage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SendText = new System.Windows.Forms.TextBox();
			this.SendBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// SendText
			// 
			this.SendText.Location = new System.Drawing.Point(0, 0);
			this.SendText.Multiline = true;
			this.SendText.Name = "SendText";
			this.SendText.Size = new System.Drawing.Size(554, 350);
			this.SendText.TabIndex = 0;
			// 
			// SendBtn
			// 
			this.SendBtn.Location = new System.Drawing.Point(479, 370);
			this.SendBtn.Name = "SendBtn";
			this.SendBtn.Size = new System.Drawing.Size(75, 23);
			this.SendBtn.TabIndex = 1;
			this.SendBtn.Text = "Send";
			this.SendBtn.UseVisualStyleBackColor = true;
			this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
			// 
			// SendMesage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 405);
			this.Controls.Add(this.SendBtn);
			this.Controls.Add(this.SendText);
			this.Name = "SendMesage";
			this.Text = "SendMessage";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox SendText;
		private System.Windows.Forms.Button SendBtn;
	}
}