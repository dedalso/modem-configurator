using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Modem_Configurator
{
    public class ModemConfigure
    {
        public string? Model { get; set; }
        public string? IMEI { get; set; }
        public string? Operator { get; set; }
        public int SignalLevel { get; set; }
        [JsonIgnore]
        public DateTime Time { get; set; }
        public string? CCID { get; set; }
        public string? IMSI { get; set; }
        public string? IP { get; set; }
        public string? GSMModule { get; set; }
        public string? ModemMode { get; set; }
        public double Voltage { get; set; }
        public string? ControllerSoftware { get; set; }
        public string? GSMSoftware { get; set; }
    }

    public partial class MainWindow : Window
    {
        const string ConfigName = "ModemConfiguration.json";
        private Random RNG = new Random();
        private static string[] Operators = { "Beeline", "MegaFon", "MTS", "Tele2" };
        private static int SignalLevelMax = 20;
        private string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        private static int IPMax = 256;
        private static char IPSeparator = '.';
        private static string[] ModemModes = { "2G", "3G" };
        public ModemConfigure Configure = new ModemConfigure();

        public MainWindow()
        {
            SetConfigDefaults();
            InitializeComponent();
            UpdateComponent();
        }

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            LoadConfig();
            UpdateComponent();
            LogEvent("Загрузка из " + ConfigName + ".");
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            ReadComponent();
            SaveConfig();
            LogEvent("Сохранение в " + ConfigName + ".");
        }

        private void UpdateComponent()
        {
            ModelBox.Text = Configure.Model;
            IMEIBox.Text = Configure.IMEI;
            OperatorBox.Text = Configure.Operator;
            SignalLevelBox.Text = Configure.SignalLevel.ToString();
            CurrentTimeBox.Text = Configure.Time.ToString(DateTimeFormat);
            CCIDBox.Text = Configure.CCID;
            IMSIBox.Text = Configure.IMSI;
            IPBox.Text = Configure.IP;
            GSMModuleBox.Text = Configure.GSMModule;
            ModemModeBox.Text = Configure.ModemMode;
            VoltageBox.Text = Configure.Voltage.ToString("{0:C1}");
            ControllerSoftwareBox.Text = Configure.ControllerSoftware;
            GSMSoftwareBox.Text = Configure.GSMSoftware;
        }

        private void ReadComponent()
        {
            Configure.Model = ModelBox.Text;
            Configure.IMEI = IMEIBox.Text;
            Configure.Operator = OperatorBox.Text;

            try
            {
                Configure.SignalLevel = int.Parse(SignalLevelBox.Text);
            }
            catch (FormatException) { }
            Configure.CCID = CCIDBox.Text;
            Configure.IMSI = IMSIBox.Text;
            Configure.IP = IPBox.Text;
            Configure.GSMModule = GSMModuleBox.Text;
            Configure.ModemMode = ModemModeBox.Text;

            try
            {
                Configure.Voltage = double.Parse(VoltageBox.Text);
            }
            catch (FormatException) { }
            Configure.ControllerSoftware = ControllerSoftwareBox.Text;
            Configure.GSMSoftware = GSMSoftwareBox.Text;
        }

        private void SetConfigDefaults()
        {
            Configure.Model = "WRX700-OEM";
            Configure.IMEI = GenerateNumber(15);
            Configure.Operator = Operators[RNG.Next(Operators.Length)];
            Configure.SignalLevel = RNG.Next(SignalLevelMax);
            Configure.Time = new DateTime();
            Configure.CCID = GenerateNumber(19);
            Configure.IMSI = GenerateNumber(15);
            Configure.IP = RNG.Next(IPMax).ToString() + IPSeparator + RNG.Next(IPMax).ToString() + IPSeparator + RNG.Next(IPMax).ToString() + IPSeparator + RNG.Next(IPMax).ToString();
            Configure.GSMModule = "GL865-DUAL-V3.1";
            Configure.ModemMode = ModemModes[RNG.Next(ModemModes.Length)];
            Configure.Voltage = 5;
            Configure.ControllerSoftware = "WRX7AA.41.00.0048";
            Configure.GSMSoftware = "16.01.173";
        }

        private void SaveConfig()
        {
            var JSONOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string JSON = JsonSerializer.Serialize(Configure, JSONOptions);
            File.WriteAllText(ConfigName, JSON);
        }

        private void LoadConfig()
        {
            string JSON = File.ReadAllText(ConfigName);

            try
            {
                Configure = JsonSerializer.Deserialize<ModemConfigure>(JSON)!;
            }
            catch (JsonException) { }
        }

        private void LogEvent(string message)
        {
            EventLog.Text += DateTime.Now.ToString("[HH:mm:ss] ") + message + Environment.NewLine;
        }

        private string GenerateNumber(int DigitsCount)
        {
            string Number = "";
            Random Random = new Random();

            for (int i = 0; i < DigitsCount; i++)
                Number += Random.Next(10);
            return Number;
        }
    }
}