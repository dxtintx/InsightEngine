using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InsightEngine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void denybtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _webhookUrl = ConfigurationManager.AppSettings["WebhookURL"];

        private void Form1_Load(object sender, EventArgs e)
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                (screen.Width - this.Width) / 2,
                (screen.Height - this.Height) / 2
            );

            string? filePath = ConfigurationManager.AppSettings["LatestLogPath"];

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please enter a file path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("File does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string fileContent = File.ReadAllText(filePath);
                richTextBox1.Text = fileContent;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access to the file is denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static (T[] extracted, T[] remaining) ExtractEverySecondElement<T>(T[] inputArray)
        {
            if (inputArray == null || inputArray.Length == 0)
            {
                return (Array.Empty<T>(), Array.Empty<T>());
            }

            int extractedSize = (inputArray.Length + 1) / 2;
            int remainingSize = inputArray.Length / 2;

            T[] extracted = new T[extractedSize];
            T[] remaining = new T[remainingSize];

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (i % 2 == 1)
                {
                    extracted[i / 2] = inputArray[i];
                }
                else
                {
                    remaining[i / 2] = inputArray[i];
                }
            }

            return (extracted, remaining);
        }

        Program.SystemInfo si = new Program.SystemInfo();

        public string FormatRAMInfo()
        {
            List<string[]> ramSticks = si.GetRAMInfoFromWMI();
            StringBuilder result = new StringBuilder();

            foreach (string[] stick in ramSticks)
            {
                if (stick.Length >= 3)
                {
                    result.Append($"{stick[1]} {stick[0].Replace(" MB", "MB")} {stick[2].Replace(" MHz", "MHz")}-||-");
                }
            }

            return result.ToString().Trim();
        }

        private async void sendbtn_Click(object sender, EventArgs e)
        {
            string[] osdet = si.getOperatingSystemInfo();
            string[] gpudet = si.GetGPUInfo();
            string procdet = si.getProcessorInfo();

            var (extracted, remaining) = ExtractEverySecondElement(gpudet);
            List<string[]> ramdet = si.GetRAMInfoFromWMI();

            string repo = """
                {
                
                  "content": "",
                  "tts": false,
                  "embeds": [
                    {
                      "id": 890498287,
                      "description": "",
                      "fields": [
                        {
                          "id": 835139754,
                          "name": "OS Name",
                          "value": "-GETOS-0",
                          "inline": true
                        },
                        {
                          "id": 192389965,
                          "name": "OS Architecture",
                          "value": "-GETOS-1",
                          "inline": true
                        },
                        {
                          "id": 90347345,
                          "name": "OS Service Pack",
                          "value": "-GETOS-2",
                          "inline": true
                        },
                        {
                          "id": 546572948,
                          "name": "GPUs",
                          "value": "-GETGPU-",
                          "inline": true
                        },
                        {
                          "id": 906977037,
                          "name": "GPUs VRAM",
                          "value": "-GETGPUV-",
                          "inline": true
                        },
                        {
                          "id": 356664289,
                          "name": "CPU",
                          "value": "-GETCPU-",
                          "inline": true
                        },
                        {
                          "id": 156099467,
                          "name": "RAMs",
                          "value": "-GETRAM-",
                          "inline": true
                        },
                        {
                          "id": 728125556,
                          "name": "C: DRIVE",
                          "value": "-GETDRIVE-",
                          "inline": true
                        }
                      ],
                      "author": {
                        "name": "New crash reported"
                      }
                    }
                  ],
                  "components": [],
                  "actions": {},
                  "flags": 0,
                  "username": "InsightEngine"
                }
                """;

            repo = repo.Replace("-GETOS-0", osdet[0]);
            repo = repo.Replace("-GETOS-1", osdet[1]);
            repo = repo.Replace("-GETOS-2", osdet[2]);

            repo = repo.Replace("-GETGPU-", string.Join("""\n""", remaining));
            repo = repo.Replace("-GETGPUV-", string.Join("""\n""", extracted));

            repo = repo.Replace("-GETCPU-", procdet);
            repo = repo.Replace("-GETRAM-", FormatRAMInfo().Replace("-||-", """\n"""));
            repo = repo.Replace("-GETDRIVE-", si.GetDiskInfo());

            string filePath = ConfigurationManager.AppSettings["LatestLogPath"];

            using (var formData = new MultipartFormDataContent())
            {
                var jsonContent = new StringContent(repo, Encoding.UTF8, "application/json");
                formData.Add(jsonContent, "payload_json");

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                    formData.Add(fileContent, "file", Path.GetFileName(filePath));
                }

                var response = await _httpClient.PostAsync(_webhookUrl, formData);
                response.EnsureSuccessStatusCode();
            }

            MessageBox.Show("Log file has been successfully delivered to developers.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
