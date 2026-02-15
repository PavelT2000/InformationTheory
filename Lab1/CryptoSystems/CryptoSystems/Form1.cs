using System.Text;

namespace CryptoSystems
{
    public partial class Form1 : Form
    {
        public ICryptoSystem CryptoSystem = new DecimationCryptoSystem();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string output;
            CryptoSystem.SetKey("", out output);
            StatusLabel.Text = output;
            KeyLabel.Text = CryptoSystem.GetKey();
        }

        private void KeyTextBox_TextChanged(object sender, EventArgs e)
        {
            string output;
            CryptoSystem.SetKey(KeyTextBox.Text, out output);
            RightTextBox.TextChanged -= RightTextBox_TextChanged;
            RightTextBox.Text = CryptoSystem.Cipher(LeftTextBox.Text, DataGrid);
            RightTextBox.TextChanged += RightTextBox_TextChanged;
            StatusLabel.Text = output;
            KeyLabel.Text = CryptoSystem.GetKey();

        }

        private void LeftTextBox_TextChanged(object sender, EventArgs e)
        {
            RightTextBox.TextChanged -= RightTextBox_TextChanged;
            RightTextBox.Text = CryptoSystem.Cipher(LeftTextBox.Text, DataGrid);
            RightTextBox.TextChanged += RightTextBox_TextChanged;
            KeyLabel.Text = CryptoSystem.GetKey();
        }

        private void RightTextBox_TextChanged(object sender, EventArgs e)
        {
            LeftTextBox.TextChanged -= LeftTextBox_TextChanged;
            LeftTextBox.Text = CryptoSystem.DeCipher(RightTextBox.Text, DataGrid);
            LeftTextBox.TextChanged += LeftTextBox_TextChanged;
            KeyLabel.Text = CryptoSystem.GetKey();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                // 3. Открытие диалога и проверка результата
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем путь к выбранному файлу
                    string filePath = openFileDialog.FileName;

                    // Читаем содержимое файла
                    try
                    {

                        string fileContent = File.ReadAllText(filePath);

                        MessageBox.Show($"файл открыт");
                        LeftTextBox.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
                    }
                }
            }
            KeyLabel.Text = CryptoSystem.GetKey();
        }

        private void OpenFileCipher_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {

                        string fileContent = File.ReadAllText(filePath);

                        MessageBox.Show($"файл открыт");
                        RightTextBox.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
                    }
                }
            }
            KeyLabel.Text = CryptoSystem.GetKey();
        }

        private void OpenFileKey_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        string fileContent = File.ReadAllText(filePath);

                        MessageBox.Show($"файл открыт");
                        KeyTextBox.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
                    }
                }
            }
            KeyLabel.Text = CryptoSystem.GetKey();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                CryptoSystem = new DecimationCryptoSystem();
                string output;
                CryptoSystem.SetKey(KeyTextBox.Text, out output);
                RightTextBox.TextChanged -= RightTextBox_TextChanged;
                RightTextBox.Text = CryptoSystem.Cipher(LeftTextBox.Text, DataGrid);
                RightTextBox.TextChanged += RightTextBox_TextChanged;
                StatusLabel.Text = output;
                KeyLabel.Text = CryptoSystem.GetKey();
            }
            else
            {
                CryptoSystem = new VigneraGeneratedCryptoSystem();
                string output;
                CryptoSystem.SetKey(KeyTextBox.Text, out output);
                RightTextBox.TextChanged -= RightTextBox_TextChanged;
                RightTextBox.Text = CryptoSystem.Cipher(LeftTextBox.Text, DataGrid);
                RightTextBox.TextChanged += RightTextBox_TextChanged;
                StatusLabel.Text = output;
                KeyLabel.Text = CryptoSystem.GetKey();
            }
        }

        private void SavePlain_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {

                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить результат шифрования";
                saveFileDialog.DefaultExt = "txt"; // Расширение по умолчанию
                saveFileDialog.AddExtension = true; // Автоматически добавлять .txt, если пользователь забыл


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        File.WriteAllText(saveFileDialog.FileName, LeftTextBox.Text, Encoding.UTF8);

                        MessageBox.Show("Файл успешно сохранен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveKey_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {

                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить результат шифрования";
                saveFileDialog.DefaultExt = "txt"; // Расширение по умолчанию
                saveFileDialog.AddExtension = true; // Автоматически добавлять .txt, если пользователь забыл


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        File.WriteAllText(saveFileDialog.FileName, KeyTextBox.Text, Encoding.UTF8);

                        MessageBox.Show("Файл успешно сохранен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveCipher_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {

                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить результат шифрования";
                saveFileDialog.DefaultExt = "txt"; // Расширение по умолчанию
                saveFileDialog.AddExtension = true; // Автоматически добавлять .txt, если пользователь забыл


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        File.WriteAllText(saveFileDialog.FileName, RightTextBox.Text, Encoding.UTF8);

                        MessageBox.Show("Файл успешно сохранен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
