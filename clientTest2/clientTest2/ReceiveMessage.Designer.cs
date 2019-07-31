namespace clientTest2
{
	partial class ReceiveMessage
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
			this.ReceivedTextbox = new System.Windows.Forms.TextBox();
			this.ReplyText = new System.Windows.Forms.TextBox();
			this.RplyBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ReceivedTextbox
			// 
			this.ReceivedTextbox.Location = new System.Drawing.Point(0, 0);
			this.ReceivedTextbox.Multiline = true;
			this.ReceivedTextbox.Name = "ReceivedTextbox";
			this.ReceivedTextbox.ReadOnly = true;
			this.ReceivedTextbox.Size = new System.Drawing.Size(555, 319);
			this.ReceivedTextbox.TabIndex = 0;
			// 
			// ReplyText
			// 
			this.ReplyText.Location = new System.Drawing.Point(0, 342);
			this.ReplyText.Name = "ReplyText";
			this.ReplyText.Size = new System.Drawing.Size(443, 25);
			this.ReplyText.TabIndex = 1;
			// 
			// RplyBtn
			// 
			this.RplyBtn.Location = new System.Drawing.Point(468, 341);
			this.RplyBtn.Name = "RplyBtn";
			this.RplyBtn.Size = new System.Drawing.Size(75, 23);
			this.RplyBtn.TabIndex = 2;
			this.RplyBtn.Text = "Reply";
			this.RplyBtn.UseVisualStyleBackColor = true;
			this.RplyBtn.Click += new System.EventHandler(this.RplyBtn_Click);
			// 
			// ReceiveMessage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 405);
			this.Controls.Add(this.RplyBtn);
			this.Controls.Add(this.ReplyText);
			this.Controls.Add(this.ReceivedTextbox);
			this.Name = "ReceiveMessage";
			this.Text = "ReceiveMessage";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ReceivedTextbox;
		private System.Windows.Forms.TextBox ReplyText;
		private System.Windows.Forms.Button RplyBtn;
	}
}