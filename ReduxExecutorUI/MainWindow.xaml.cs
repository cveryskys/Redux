using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;

namespace ReduxExecutorUI {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.MouseDown += (sender, e) => {
                if (e.ChangedButton == MouseButton.Left) {
                    this.DragMove();
                }
            };
        }
        private void Close_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        // Add Script Button Click Event
        private void AddScript_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Lua Scripts (*.lua)|*.lua"
            };

            if (openFileDialog.ShowDialog() == true) {
                string scriptName = Path.GetFileName(openFileDialog.FileName);
                ScriptList.Items.Add(scriptName);
                ScriptEditor.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        // Execute Button Click Event
        private void Execute_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Executing script: " + ScriptEditor.Text);
        }
    }
}
