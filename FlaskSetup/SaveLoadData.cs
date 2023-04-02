using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FlaskSetup
{
    public struct FlasksSetupData
    {
        public FlaskSettings Flask_1 { get; set; }
        public FlaskSettings Flask_2 { get; set; }
        public FlaskSettings Flask_3 { get; set; }
        public FlaskSettings Flask_4 { get; set; }
        public FlaskSettings Flask_5 { get; set; }

        public FlasksSetupData(FlaskSettings[] data)
        {
            Flask_1 = data[0];
            Flask_2 = data[1];
            Flask_3 = data[2];
            Flask_4 = data[3];
            Flask_5 = data[4];
        }

        public FlasksSetupData(List<FlaskSettings> data)
        {
            Flask_1 = data[0];
            Flask_2 = data[1];
            Flask_3 = data[2];
            Flask_4 = data[3];
            Flask_5 = data[4];
        }
        public FlasksSetupData(Dictionary<string, FlaskSettings> data)
        {
            Flask_1 = data["Flask 1"];
            Flask_2 = data["Flask 2"];
            Flask_3 = data["Flask 3"];
            Flask_4 = data["Flask 4"];
            Flask_5 = data["Flask 5"];
        }
        public Dictionary<string, FlaskSettings> ToDcit()
        {
            return new Dictionary<string, FlaskSettings>
            {
                ["Flask 1"] = Flask_1,
                ["Flask 2"] = Flask_2,
                ["Flask 3"] = Flask_3,
                ["Flask 4"] = Flask_4,
                ["Flask 5"] = Flask_5,
            };
        }
        public FlaskSettings[] ToArray()
        {
            FlaskSettings[] flaskSetUps = new FlaskSettings[5];
            flaskSetUps[0] = Flask_1;
            flaskSetUps[1] = Flask_2;
            flaskSetUps[2] = Flask_3;
            flaskSetUps[3] = Flask_4;
            flaskSetUps[4] = Flask_5;

            return flaskSetUps;
        }
    }
    public static class SaveLoadData
    {
        private static readonly string SavePath = "FlaskConfig.json";


        public static void SaveData(FlaskSettings[] saveData)
        {
            FlasksSetupData savedData = new FlasksSetupData(saveData);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(savedData, options);
            File.WriteAllText(SavePath, json);
        }

        public static FlasksSetupData TryLoadData()
        {
            FlasksSetupData flasksSetupData;
            try
            {
                flasksSetupData = LoadData();
            }
            catch (Exception e)
            {
                flasksSetupData = GetDefloat();
                Console.WriteLine(e);
            }

            return flasksSetupData;
        }

        private static FlasksSetupData LoadData()
        {
            string json = File.ReadAllText(SavePath);
            FlasksSetupData dataBack = JsonSerializer.Deserialize<FlasksSetupData>(json);

            return dataBack;
        }

        private static FlasksSetupData GetDefloat()
        {
            return new FlasksSetupData
            {
                Flask_1 = new FlaskSettings { ActivationType = "Нет", ActivatePercent = 0, MinCD = 0.5f, HotKey = "1", Group = "Нет" },
                Flask_2 = new FlaskSettings { ActivationType = "Нет", ActivatePercent = 0, MinCD = 0.5f, HotKey = "2", Group = "Нет" },
                Flask_3 = new FlaskSettings { ActivationType = "Нет", ActivatePercent = 0, MinCD = 0.5f, HotKey = "3", Group = "Нет" },
                Flask_4 = new FlaskSettings { ActivationType = "Нет", ActivatePercent = 0, MinCD = 0.5f, HotKey = "4", Group = "Нет" },
                Flask_5 = new FlaskSettings { ActivationType = "Нет", ActivatePercent = 0, MinCD = 0.5f, HotKey = "5", Group = "Нет" },
            };
        }
    }
}
