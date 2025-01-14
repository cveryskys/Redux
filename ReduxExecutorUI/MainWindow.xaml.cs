using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReduxExecutorUI {
    public partial class MainWindow : Window {
        private static readonly HttpClient httpClient = new HttpClient();

        public MainWindow() {
            InitializeComponent();
        }

        // Execute Button Click Event
        private async void Execute_Click(object sender, RoutedEventArgs e) {
            string script = ScriptEditor.Text;

            if (string.IsNullOrWhiteSpace(script)) {
                MessageBox.Show("Please enter a script to execute.");
                return;
            }

            var result = await SendScriptToApi(script);

            if (result.IsSuccessStatusCode) {
                MessageBox.Show("Script executed successfully!");
            } else {
                MessageBox.Show("Failed to execute the script.");
            }
        }

        private async Task<HttpResponseMessage> SendScriptToApi(string script) {
            var payload = new {
                script = script
            };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            return await httpClient.PostAsync("http://localhost:8000/execute_script", content);
        }
    }
}
