using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical; // Import all our ciphers

namespace CryptoCourse.WinFormsUI.Controls
{
    public class PipelinePanel : Panel
    {
        private ListBox _availableAlgorithmsList;
        private ListBox _pipelineList;
        private TextBox _plaintextBox;
        private TextBox _resultTextBox;

        public PipelinePanel()
        {
            this.Dock = DockStyle.Fill;
            var mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Padding = new Padding(10) };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            this.Controls.Add(mainLayout);

            // --- Column 1: Available Algorithms ---
            var leftPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _availableAlgorithmsList = new ListBox { Dock = DockStyle.Fill };
            _availableAlgorithmsList.Items.AddRange(new string[] {
                "Caesar Cipher", "Rail Fence Cipher", "Reverse Text", "Reverse Blocks", "Columnar Transposition"
                // We exclude complex-key ciphers like Playfair/Hill for simplicity in this pipeline example
            });
            leftPanel.Controls.Add(new Label { Text = "الخوارزميات المتاحة:", AutoSize = true }, 0, 0);
            leftPanel.Controls.Add(_availableAlgorithmsList, 0, 1);
            mainLayout.Controls.Add(leftPanel, 0, 0);

            // --- Column 2: Control Buttons ---
            var middlePanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5 };
            var addButton = new Button { Text = "أضف ->", Dock = DockStyle.Fill };
            var removeButton = new Button { Text = "<- إزالة", Dock = DockStyle.Fill };
            var moveUpButton = new Button { Text = "↑ فوق", Dock = DockStyle.Fill };
            var moveDownButton = new Button { Text = "↓ تحت", Dock = DockStyle.Fill };
            middlePanel.Controls.Add(addButton, 0, 1);
            middlePanel.Controls.Add(removeButton, 0, 2);
            middlePanel.Controls.Add(moveUpButton, 0, 3);
            middlePanel.Controls.Add(moveDownButton, 0, 4);
            mainLayout.Controls.Add(middlePanel, 1, 0);

            // --- Column 3: Pipeline and Execution ---
            var rightPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5 };
            _pipelineList = new ListBox { Dock = DockStyle.Fill };
            _plaintextBox = new TextBox { Dock = DockStyle.Fill };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.White };
            var runButton = new Button { Text = "نفذ خط الأنابيب", Dock = DockStyle.Top, Height = 40, BackColor = Color.LightGreen };

            rightPanel.Controls.Add(new Label { Text = "خط الأنابيب (التسلسل):", AutoSize = true }, 0, 0);
            rightPanel.Controls.Add(_pipelineList, 0, 1);
            rightPanel.Controls.Add(new Label { Text = "النص الأولي:", AutoSize = true }, 0, 2);
            rightPanel.Controls.Add(_plaintextBox, 0, 3);
            rightPanel.Controls.Add(runButton, 0, 4);
            rightPanel.Controls.Add(new Label { Text = "النتيجة النهائية:", AutoSize = true }, 0, 5);
            rightPanel.Controls.Add(_resultTextBox, 0, 6);
            mainLayout.Controls.Add(rightPanel, 2, 0);

            // --- Event Handlers ---
            addButton.Click += AddButton_Click;
            removeButton.Click += RemoveButton_Click;
            moveUpButton.Click += MoveUpButton_Click;
            moveDownButton.Click += MoveDownButton_Click;
            runButton.Click += RunButton_Click;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (_availableAlgorithmsList.SelectedItem != null)
            {
                _pipelineList.Items.Add(_availableAlgorithmsList.SelectedItem);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_pipelineList.SelectedItem != null)
            {
                _pipelineList.Items.Remove(_pipelineList.SelectedItem);
            }
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            int index = _pipelineList.SelectedIndex;
            if (index > 0)
            {
                object item = _pipelineList.SelectedItem;
                _pipelineList.Items.RemoveAt(index);
                _pipelineList.Items.Insert(index - 1, item);
                _pipelineList.SelectedIndex = index - 1;
            }
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            int index = _pipelineList.SelectedIndex;
            if (index != -1 && index < _pipelineList.Items.Count - 1)
            {
                object item = _pipelineList.SelectedItem;
                _pipelineList.Items.RemoveAt(index);
                _pipelineList.Items.Insert(index + 1, item);
                _pipelineList.SelectedIndex = index + 1;
            }
        }

        // The core logic for the pipeline
        private void RunButton_Click(object sender, EventArgs e)
        {
            string currentText = _plaintextBox.Text;

            foreach (var item in _pipelineList.Items)
            {
                string algorithm = item.ToString();
                try
                {
                    switch (algorithm)
                    {
                        case "Caesar Cipher":
                            string caesarKeyStr = ShowInputDialog("أدخل مفتاح الإزاحة لـ Caesar:", "مفتاح");
                            if (int.TryParse(caesarKeyStr, out int caesarKey))
                                currentText = CaesarCipher.Process(currentText, caesarKey);
                            else { MessageBox.Show("مفتاح غير صالح، تم تخطي المرحلة."); }
                            break;

                        case "Rail Fence Cipher":
                            string railKeyStr = ShowInputDialog("أدخل عدد القضبان لـ Rail Fence:", "مفتاح");
                            if (int.TryParse(railKeyStr, out int railKey))
                                currentText = RailFenceCipher.Encrypt(currentText, railKey);
                            else { MessageBox.Show("مفتاح غير صالح، تم تخطي المرحلة."); }
                            break;

                        case "Reverse Text":
                            currentText = ReverseTextCipher.Process(currentText);
                            break;

                        case "Reverse Blocks":
                            string blockSizeStr = ShowInputDialog("أدخل حجم البلوك لـ Reverse Blocks:", "مفتاح");
                            if (int.TryParse(blockSizeStr, out int blockSize))
                                currentText = ReverseBlocksCipher.Process(currentText, blockSize);
                            else { MessageBox.Show("مفتاح غير صالح، تم تخطي المرحلة."); }
                            break;

                        case "Columnar Transposition":
                            string colKey = ShowInputDialog("أدخل المفتاح النصي لـ Columnar:", "مفتاح");
                            if (!string.IsNullOrEmpty(colKey))
                                currentText = ColumnarTranspositionCipher.Encrypt(currentText, colKey);
                            else { MessageBox.Show("مفتاح غير صالح، تم تخطي المرحلة."); }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ أثناء تطبيق {algorithm}: {ex.Message}", "خطأ في التنفيذ");
                    return; // Stop pipeline on error
                }
            }
            _resultTextBox.Text = currentText;
        }

        // Helper function to show a simple input dialog
        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 300 };
            Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}