namespace InsightEngine
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            sendbtn = new Button();
            denybtn = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(12, 113);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(332, 180);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // sendbtn
            // 
            sendbtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            sendbtn.Location = new Point(269, 299);
            sendbtn.Name = "sendbtn";
            sendbtn.Size = new Size(75, 23);
            sendbtn.TabIndex = 1;
            sendbtn.Text = "Send";
            sendbtn.UseVisualStyleBackColor = true;
            sendbtn.Click += sendbtn_Click;
            // 
            // denybtn
            // 
            denybtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            denybtn.Location = new Point(161, 299);
            denybtn.Name = "denybtn";
            denybtn.Size = new Size(102, 23);
            denybtn.TabIndex = 2;
            denybtn.Text = "Do not send";
            denybtn.UseVisualStyleBackColor = true;
            denybtn.Click += denybtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(41, 45);
            label1.TabIndex = 3;
            label1.Text = ":(";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label2.Location = new Point(165, 22);
            label2.Name = "label2";
            label2.Size = new Size(179, 32);
            label2.TabIndex = 4;
            label2.Text = "Crash spotted!";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(12, 69);
            label3.Name = "label3";
            label3.Size = new Size(337, 15);
            label3.TabIndex = 5;
            label3.Text = "Would you like to send the latest log file to the developers?";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 95);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 6;
            label4.Text = "Log file:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(356, 328);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(denybtn);
            Controls.Add(sendbtn);
            Controls.Add(richTextBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Location = new Point(100, 100);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button sendbtn;
        private Button denybtn;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
