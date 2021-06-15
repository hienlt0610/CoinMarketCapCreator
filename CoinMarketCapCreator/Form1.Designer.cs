
namespace CoinMarketCapCreator
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
        private void InitializeComponent()
        {
            this.lv_reg_info = new System.Windows.Forms.ListView();
            this.col_email = new System.Windows.Forms.ColumnHeader();
            this.col_username = new System.Windows.Forms.ColumnHeader();
            this.col_password = new System.Windows.Forms.ColumnHeader();
            this.col_status = new System.Windows.Forms.ColumnHeader();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_pick_file = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lv_reg_info
            // 
            this.lv_reg_info.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_email,
            this.col_username,
            this.col_password,
            this.col_status});
            this.lv_reg_info.HideSelection = false;
            this.lv_reg_info.Location = new System.Drawing.Point(12, 12);
            this.lv_reg_info.Name = "lv_reg_info";
            this.lv_reg_info.Size = new System.Drawing.Size(804, 426);
            this.lv_reg_info.TabIndex = 0;
            this.lv_reg_info.UseCompatibleStateImageBehavior = false;
            this.lv_reg_info.View = System.Windows.Forms.View.Details;
            // 
            // col_email
            // 
            this.col_email.Text = "Email";
            this.col_email.Width = 180;
            // 
            // col_username
            // 
            this.col_username.Text = "Username";
            this.col_username.Width = 150;
            // 
            // col_password
            // 
            this.col_password.Text = "Password";
            this.col_password.Width = 150;
            // 
            // col_status
            // 
            this.col_status.Text = "Status";
            this.col_status.Width = 480;
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(691, 459);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(125, 50);
            this.btn_start.TabIndex = 1;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_pick_file
            // 
            this.btn_pick_file.Location = new System.Drawing.Point(12, 459);
            this.btn_pick_file.Name = "btn_pick_file";
            this.btn_pick_file.Size = new System.Drawing.Size(125, 50);
            this.btn_pick_file.TabIndex = 1;
            this.btn_pick_file.Text = "Load file";
            this.btn_pick_file.UseVisualStyleBackColor = true;
            this.btn_pick_file.Click += new System.EventHandler(this.btn_pick_file_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 538);
            this.Controls.Add(this.btn_pick_file);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.lv_reg_info);
            this.Name = "Form1";
            this.Text = "CoinMarketCap";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv_reg_info;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.ColumnHeader col_email;
        private System.Windows.Forms.ColumnHeader col_username;
        private System.Windows.Forms.ColumnHeader col_password;
        private System.Windows.Forms.ColumnHeader col_status;
        private System.Windows.Forms.Button btn_pick_file;
    }
}

