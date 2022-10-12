using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace ONIMod.IceBox
{
    [RestartRequired]
    [JsonObject((MemberSerialization)1)]
    internal class Settings
    {
        public static Settings GetSettings()
        {
            return Settings.instance;
        }

        [Option("冰箱能存多少千克食物？", "How much food can be stored?(KG)", null)]
        [JsonProperty]
        public float Defultstorage { get; set; }

        [Option("冰箱使用多少电力？", "How much power is used in the IceBox ?", null)]
        [JsonProperty]
        public float DefultPower { get; set; }

        [Option("原版冰箱使用多少电力？", "How much power is used in the Refrigerator ?", null)]
        [JsonProperty]
        public float RefrigeratorPower { get; set; }

        [Option("原版冰箱拥有多大容量？", "How much food can be stored?(KG) in the Refrigerator ?", null)]
        [JsonProperty]
        public float RefrigeratorStorage { get; set; }

        [JsonProperty]
        public float SimulatedInternalTemperature { get; set; }

        [Option("原版冰箱发热量？", "原版冰箱工作时发热多少?", null)]
        [JsonProperty]
        public float RefrigeratorSelfHeatKilowattsWhenActive { get; set; }

        public static void Init(Settings settings)
        {
            bool flag = settings != null;
            if (flag)
            {
                Settings.instance = settings;
            }
        }
        public Settings()
        {
            this.Defultstorage = 520f;
            this.SimulatedInternalTemperature = 233.15f;
            this.DefultPower = 240f;
            this.RefrigeratorPower = 120f;
            this.RefrigeratorStorage = 100f;
            this.RefrigeratorSelfHeatKilowattsWhenActive = 0f;
        }

        private static Settings instance = new Settings();
    }
}
