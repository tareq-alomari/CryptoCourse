using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.WinFormsUI.Controls; // تأكد من أن هذا الـ namespace صحيح لمشروعك

namespace CryptoCourse.WinFormsUI
{
    /// <summary>
    /// The main window for the Crypto-Toolkit application.
    /// All UI elements within this form are generated programmatically.
    /// </summary>
    public partial class MainForm : Form
    {
        // Define the main UI layout components as private fields
        private TableLayoutPanel _mainLayout;
        private ComboBox _algorithmSelector;
        private Panel _contentPanel;

        public MainForm()
        {
            // --- 1. Professional Color Palette ---
            var backColorDark = Color.FromArgb(37, 37, 38);       // لون الخلفية الرئيسي
            var panelColorMedium = Color.FromArgb(45, 45, 48);   // لون اللوحات الداخلية
            var textColorLight = Color.FromArgb(241, 241, 241);    // لون النص
            var accentColorBlue = Color.FromArgb(0, 122, 204);     // لون التمييز (الأزرق)
            var borderColor = Color.FromArgb(63, 63, 70);        // لون الحدود

            // --- 2. Set modern window properties ---
            this.Text = "Crypto-Toolkit | مجموعة أدوات التشفير";
            this.Width = 900;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(850, 650);
            this.BackColor = backColorDark;
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = textColorLight;

            // --- 3. Main Layout: Header, Content, Footer ---
            _mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15),
                ColumnCount = 1,
                RowCount = 3 // Header, Content, Footer
            };
            // Row 0: Header (fixed height)
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            // Row 1: Content (takes remaining space)
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // Row 2: Footer (fixed height)
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            this.Controls.Add(_mainLayout);

            // --- 4. Header Panel Setup ---
            var headerPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            var selectorLabel = new Label
            {
                Text = "اختر خوارزمية التشفير:",
                ForeColor = textColorLight,
                Location = new Point(5, 17),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            _algorithmSelector = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 300,
                Location = new Point(220, 15),
                BackColor = panelColorMedium,
                ForeColor = textColorLight,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F)
            };
            headerPanel.Controls.Add(selectorLabel);
            headerPanel.Controls.Add(_algorithmSelector);
            _mainLayout.Controls.Add(headerPanel, 0, 0);

            // --- 5. Content Panel Setup ---
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = panelColorMedium,
                Padding = new Padding(10)
                // We removed the ugly border and now use background color for separation.
            };
            _mainLayout.Controls.Add(_contentPanel, 0, 1);

            // --- 6. Footer Panel Setup (with project info) ---
            var footerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = backColorDark
            };
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Left side of the footer
            var supervisionPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown
            };
            var supervisorLabel = new Label { Text = "إشراف: م. نادر الحشئي", AutoSize = true, ForeColor = textColorLight };
            var studentLabel = new Label { Text = "إعداد: م. طارق العمري", AutoSize = true, ForeColor = textColorLight };
            supervisionPanel.Controls.Add(supervisorLabel);
            supervisionPanel.Controls.Add(studentLabel);

            // Right side of the footer
            var universityPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                RightToLeft = RightToLeft.Yes // Align text to the right
            };
            var universityLabel = new Label { Text = "جامعة إب - علوم حاسوب وتقنية معلومات", AutoSize = true, ForeColor = textColorLight };
            var yearLabel = new Label { Text = "العام الجامعي: 2025", AutoSize = true, ForeColor = Color.Gray }; // Subtle color for the year
            universityPanel.Controls.Add(universityLabel);
            universityPanel.Controls.Add(yearLabel);

            footerLayout.Controls.Add(supervisionPanel, 0, 0);
            footerLayout.Controls.Add(universityPanel, 1, 0);
            _mainLayout.Controls.Add(footerLayout, 0, 2);

            // --- 7. Populate ComboBox and set event handler ---
            _algorithmSelector.Items.AddRange(new string[] {
                "Caesar Cipher",
                "Playfair Cipher",
                "Affine Cipher",
                "Vigenère Cipher",
                "Hill Cipher",
                "Rail Fence Cipher",
                "Reverse Text",
                "Columnar Transposition",
                "Reverse Blocks",
                "AES (Secure)",
                "RSA (Secure)",
                "** Final Project: Crypto Pipeline **"
            });
            _algorithmSelector.SelectedIndexChanged += AlgorithmSelector_SelectedIndexChanged;

            // Select the first item by default to show a UI
            _algorithmSelector.SelectedIndex = 0;
        }

        private void AlgorithmSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _contentPanel.Controls.Clear();
            if (_algorithmSelector.SelectedItem == null) return;
            string selectedCipher = _algorithmSelector.SelectedItem.ToString();

            // The logic for switching panels remains the same.
            switch (selectedCipher)
            {
                case "Caesar Cipher":
                    _contentPanel.Controls.Add(new CaesarPanel());
                    break;
                case "Playfair Cipher":
                    _contentPanel.Controls.Add(new PlayfairPanel());
                    break;
                case "Affine Cipher":
                    _contentPanel.Controls.Add(new AffinePanel());
                    break;
                case "Vigenère Cipher":
                    _contentPanel.Controls.Add(new VigenerePanel());
                    break;
                case "Hill Cipher":
                    _contentPanel.Controls.Add(new HillPanel());
                    break;
                case "Rail Fence Cipher":
                    _contentPanel.Controls.Add(new RailFencePanel());
                    break;
                case "Reverse Text":
                    _contentPanel.Controls.Add(new ReverseTextPanel());
                    break;
                case "Columnar Transposition":
                    _contentPanel.Controls.Add(new ColumnarTranspositionPanel());
                    break;
                case "Reverse Blocks":
                    _contentPanel.Controls.Add(new ReverseBlocksPanel());
                    break;
                case "AES (Secure)":
                    _contentPanel.Controls.Add(new AesPanel());
                    break;
                case "RSA (Secure)":
                    _contentPanel.Controls.Add(new RsaPanel());
                    break;
                case "** Final Project: Crypto Pipeline **":
                    _contentPanel.Controls.Add(new PipelinePanel());
                    break;
                default:
                    var notImplementedLabel = new Label
                    {
                        Text = $"'{selectedCipher}' لم يتم تنفيذه بعد.",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 14F, FontStyle.Italic),
                        ForeColor = Color.Gray
                    };
                    _contentPanel.Controls.Add(notImplementedLabel);
                    break;
            }
        }
    }
}