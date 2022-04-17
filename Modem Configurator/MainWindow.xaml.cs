using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Modem_Configurator
{
    public partial class MainWindow : Window
    {
        private Random RNG = new Random();
        private static string[] Operators = { "Beeline", "MegaFon", "MTS", "Tele2" };
        private static int SignalLevelMax = 20;
        private string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        private static int IPMax = 256;
        private static char IPSeparator = '.';
        private static string[] ModemModes = { "2G", "3G" };
        public string Model;
        public string IMEI;
        public string Operator;
        public int SignalLevel;
        public DateTime Time;
        public double Voltage;
        public string ModemMode = "";

        public MainWindow()
        { 
            SetDefaults();
            InitializeComponent();
        }

        void SetDefaults()
        {
            Time = new DateTime();
            Voltage = 5;
            ModemMode = ModemModes[RNG.Next(ModemModes.Length)];
            ModelBox.Text = "WRX700-OEM";
            IMEIBox.Text = GenerateNumber(15);
            OperatorBox.Text = ModemModes[RNG.Next(Operators.Length)];
            SignalLevelBox.Text = RNG.Next(SignalLevelMax).ToString();
            CurrentTimeBox.Text = Time.ToString(DateTimeFormat);
            CCIDBox.Text = GenerateNumber(19);
            IMSIBox.Text = GenerateNumber(15);
            IPBox.Text = RNG.Next(IPMax).ToString() + IPSeparator + RNG.Next(IPMax).ToString() + IPSeparator + RNG.Next(IPMax).ToString() + IPSeparator + RNG.Next(IPMax).ToString();
            GSMModuleBox.Text = "GL865-DUAL-V3.1";
            ModemModeBox.Text = "";
            VoltageBox.Text = Voltage.ToString("{0:C1}");
            ControllerSoftwareBox.Text = "WRX7AA.41.00.0048";
            GSMSoftwareBox.Text = "16.01.173";
        }

        string GenerateNumber(int DigitsCount)
        {
            string Number = "";
            Random Random = new Random();

            for (int i = 0; i < DigitsCount; i++)
                Number += Random.Next(10);
            return Number;
        }
    }
}