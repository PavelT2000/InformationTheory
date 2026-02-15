namespace CryptoSystems
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
            LeftTextBox = new RichTextBox();
            label1 = new Label();
            label3 = new Label();
            RightTextBox = new RichTextBox();
            label4 = new Label();
            KeyTextBox = new RichTextBox();
            StatusLabel = new Label();
            DataGrid = new DataGridView();
            OpenFilePlain = new Button();
            OpenFileKey = new Button();
            OpenFileCipher = new Button();
            comboBox1 = new ComboBox();
            KeyLabel = new Label();
            SaveKey = new Button();
            SaveCipher = new Button();
            SavePlain = new Button();
            ((System.ComponentModel.ISupportInitialize)DataGrid).BeginInit();
            SuspendLayout();
            // 
            // LeftTextBox
            // 
            LeftTextBox.Location = new Point(100, 150);
            LeftTextBox.Name = "LeftTextBox";
            LeftTextBox.Size = new Size(200, 150);
            LeftTextBox.TabIndex = 0;
            LeftTextBox.Text = "";
            LeftTextBox.TextChanged += LeftTextBox_TextChanged;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(100, 130);
            label1.Name = "label1";
            label1.Size = new Size(200, 20);
            label1.TabIndex = 2;
            label1.Text = "PlainText";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            label3.Location = new Point(492, 130);
            label3.Name = "label3";
            label3.Size = new Size(200, 20);
            label3.TabIndex = 5;
            label3.Text = "CipherText";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RightTextBox
            // 
            RightTextBox.Location = new Point(492, 150);
            RightTextBox.Name = "RightTextBox";
            RightTextBox.Size = new Size(200, 150);
            RightTextBox.TabIndex = 4;
            RightTextBox.Text = "";
            RightTextBox.TextChanged += RightTextBox_TextChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            label4.Location = new Point(296, 297);
            label4.Name = "label4";
            label4.Size = new Size(200, 20);
            label4.TabIndex = 7;
            label4.Text = "Key";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // KeyTextBox
            // 
            KeyTextBox.Location = new Point(200, 320);
            KeyTextBox.Name = "KeyTextBox";
            KeyTextBox.Size = new Size(400, 50);
            KeyTextBox.TabIndex = 6;
            KeyTextBox.Text = "";
            KeyTextBox.TextChanged += KeyTextBox_TextChanged;
            // 
            // StatusLabel
            // 
            StatusLabel.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            StatusLabel.Location = new Point(100, 50);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(600, 40);
            StatusLabel.TabIndex = 8;
            StatusLabel.Text = "Строка с ключом пуста, используется ключ \"1\"";
            StatusLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // DataGrid
            // 
            DataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrid.GridColor = SystemColors.WindowText;
            DataGrid.Location = new Point(100, 380);
            DataGrid.Name = "DataGrid";
            DataGrid.RightToLeft = RightToLeft.No;
            DataGrid.Size = new Size(600, 150);
            DataGrid.TabIndex = 9;
            // 
            // OpenFilePlain
            // 
            OpenFilePlain.Location = new Point(100, 12);
            OpenFilePlain.Name = "OpenFilePlain";
            OpenFilePlain.Size = new Size(200, 23);
            OpenFilePlain.TabIndex = 10;
            OpenFilePlain.Text = "Текст из файла";
            OpenFilePlain.UseVisualStyleBackColor = true;
            OpenFilePlain.Click += button1_Click;
            // 
            // OpenFileKey
            // 
            OpenFileKey.Location = new Point(300, 12);
            OpenFileKey.Name = "OpenFileKey";
            OpenFileKey.Size = new Size(200, 23);
            OpenFileKey.TabIndex = 11;
            OpenFileKey.Text = "Ключ из файла";
            OpenFileKey.UseVisualStyleBackColor = true;
            OpenFileKey.Click += OpenFileKey_Click;
            // 
            // OpenFileCipher
            // 
            OpenFileCipher.Location = new Point(500, 12);
            OpenFileCipher.Name = "OpenFileCipher";
            OpenFileCipher.Size = new Size(200, 23);
            OpenFileCipher.TabIndex = 12;
            OpenFileCipher.Text = "Шифротекст из файла";
            OpenFileCipher.UseVisualStyleBackColor = true;
            OpenFileCipher.Click += OpenFileCipher_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Метод децимаций", "Вижнера самогенерируюйщися" });
            comboBox1.Location = new Point(300, 93);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(196, 23);
            comboBox1.TabIndex = 13;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // KeyLabel
            // 
            KeyLabel.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyLabel.Location = new Point(296, 150);
            KeyLabel.Name = "KeyLabel";
            KeyLabel.Size = new Size(200, 147);
            KeyLabel.TabIndex = 14;
            KeyLabel.Text = "Key";
            KeyLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SaveKey
            // 
            SaveKey.Location = new Point(163, 347);
            SaveKey.Name = "SaveKey";
            SaveKey.Size = new Size(31, 23);
            SaveKey.TabIndex = 16;
            SaveKey.Text = "💾";
            SaveKey.UseVisualStyleBackColor = true;
            SaveKey.Click += SaveKey_Click;
            // 
            // SaveCipher
            // 
            SaveCipher.Location = new Point(661, 297);
            SaveCipher.Name = "SaveCipher";
            SaveCipher.Size = new Size(31, 23);
            SaveCipher.TabIndex = 17;
            SaveCipher.Text = "💾";
            SaveCipher.UseVisualStyleBackColor = true;
            SaveCipher.Click += SaveCipher_Click;
            // 
            // SavePlain
            // 
            SavePlain.Location = new Point(100, 297);
            SavePlain.Name = "SavePlain";
            SavePlain.Size = new Size(31, 23);
            SavePlain.TabIndex = 18;
            SavePlain.Text = "💾";
            SavePlain.UseVisualStyleBackColor = true;
            SavePlain.Click += SavePlain_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 540);
            Controls.Add(SavePlain);
            Controls.Add(SaveCipher);
            Controls.Add(SaveKey);
            Controls.Add(KeyLabel);
            Controls.Add(comboBox1);
            Controls.Add(OpenFileCipher);
            Controls.Add(OpenFileKey);
            Controls.Add(OpenFilePlain);
            Controls.Add(DataGrid);
            Controls.Add(StatusLabel);
            Controls.Add(label4);
            Controls.Add(KeyTextBox);
            Controls.Add(label3);
            Controls.Add(RightTextBox);
            Controls.Add(label1);
            Controls.Add(LeftTextBox);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)DataGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox LeftTextBox;
        private Label label1;
        private Label label3;
        private RichTextBox RightTextBox;
        private Label label4;
        private RichTextBox KeyTextBox;
        private Label StatusLabel;
        private DataGridView DataGrid;
        private Button OpenFilePlain;
        private Button OpenFileKey;
        private Button OpenFileCipher;
        private ComboBox comboBox1;
        private Label KeyLabel;
        private Button SaveKey;
        private Button SaveCipher;
        private Button SavePlain;
    }
}
