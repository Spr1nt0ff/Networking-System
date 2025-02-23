namespace ClientInterface {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            textBox2 = new TextBox();
            button1 = new Button();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(6, 22);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(318, 196);
            textBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(330, 224);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Входящие сообщения";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(54, 242);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(207, 23);
            textBox2.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(267, 242);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "Отправить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 246);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 4;
            label1.Text = "Ввод:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(354, 273);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private GroupBox groupBox1;
        private TextBox textBox2;
        private Button button1;
        private Label label1;
    }
}
