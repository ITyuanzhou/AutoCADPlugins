namespace cadplugin
{
    partial class PickPointDialog
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancleButton = new System.Windows.Forms.Button();
            this.pickPointButton = new System.Windows.Forms.Button();
            this.positionText = new System.Windows.Forms.TextBox();
            this.stopPickButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(468, 146);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancleButton
            // 
            this.cancleButton.Location = new System.Drawing.Point(563, 146);
            this.cancleButton.Name = "cancleButton";
            this.cancleButton.Size = new System.Drawing.Size(75, 23);
            this.cancleButton.TabIndex = 1;
            this.cancleButton.Text = "取消";
            this.cancleButton.UseVisualStyleBackColor = true;
            this.cancleButton.Click += new System.EventHandler(this.cancleButton_Click);
            // 
            // pickPointButton
            // 
            this.pickPointButton.Location = new System.Drawing.Point(49, 146);
            this.pickPointButton.Name = "pickPointButton";
            this.pickPointButton.Size = new System.Drawing.Size(338, 23);
            this.pickPointButton.TabIndex = 2;
            this.pickPointButton.Text = "切入到AutoCAD拾取点";
            this.pickPointButton.UseVisualStyleBackColor = true;
            this.pickPointButton.Click += new System.EventHandler(this.pickPointButton_Click);
            // 
            // positionText
            // 
            this.positionText.Location = new System.Drawing.Point(49, 70);
            this.positionText.Name = "positionText";
            this.positionText.Size = new System.Drawing.Size(338, 21);
            this.positionText.TabIndex = 3;
            // 
            // stopPickButton
            // 
            this.stopPickButton.Location = new System.Drawing.Point(49, 214);
            this.stopPickButton.Name = "stopPickButton";
            this.stopPickButton.Size = new System.Drawing.Size(338, 23);
            this.stopPickButton.TabIndex = 4;
            this.stopPickButton.Text = "停止拾取点";
            this.stopPickButton.UseVisualStyleBackColor = true;
            this.stopPickButton.Click += new System.EventHandler(this.stopPickButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 262);
            this.Controls.Add(this.stopPickButton);
            this.Controls.Add(this.positionText);
            this.Controls.Add(this.pickPointButton);
            this.Controls.Add(this.cancleButton);
            this.Controls.Add(this.okButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancleButton;
        private System.Windows.Forms.Button pickPointButton;
        public System.Windows.Forms.TextBox positionText;
        private System.Windows.Forms.Button stopPickButton;
    }
}