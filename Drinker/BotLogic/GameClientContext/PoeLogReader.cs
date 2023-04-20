using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drinker.BotLogic.GameClientContext
{
    internal static class PoeLogReader
    {
        public delegate void ZoneWasChanged();
        public static ZoneWasChanged OnZoneWasChanged;


        private static readonly string[] zonesChangedText = { "You have entered", "Вы вошли в область" };
        private static readonly string[] needPauseZones = { "убежище", "Hideout" };

        public static bool characterIsInPauseZone = false;
        private static string lastZoneChangedText = "";

        private static string _logFilePath;
        private static long initlogFileSize;

        public static void InintChek(string logFilePath)
        {
            _logFilePath = logFilePath;
            initlogFileSize = GetFileSize();


            var logLines = GetLogTextLines();
            if (TryGetLastZonechangedLine(logLines, out string zonechangedtext))
            {
                ParseZonechangedText_And_SetPauseFlag(zonechangedtext);
                OnZoneWasChanged?.Invoke();
            }
        }

        public static void Update()
        {
            var nowLogSize = GetFileSize();
            if (nowLogSize > initlogFileSize)
            {
                var updatedBytes = ReadFileBytes(initlogFileSize, nowLogSize - initlogFileSize);
                var updatetdText = BytesArrayToString(updatedBytes);
                var updatetdText_lines = updatetdText.Split('\n');

                if (TryGetLastZonechangedLine(updatetdText_lines, out string zonechangedtext))
                {
                    ParseZonechangedText_And_SetPauseFlag(zonechangedtext);
                    OnZoneWasChanged?.Invoke();
                }
            }

            initlogFileSize = nowLogSize;
        }



        private static string[] GetLogTextLines()
        {
            using (FileStream fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string[] allData = sr.ReadToEnd().Split('\n');
                    return allData;
                }
            }
        }

        private static bool TryGetLastZonechangedLine(string[] logLines, out string lastZonechangedLine)
        {
            lastZonechangedLine = "";
            for (int i = logLines.Length - 1; i >= 0; i--)
            {
                foreach (string zoneChangedText in zonesChangedText) 
                {
                    if (logLines[i].Contains(zoneChangedText) && logLines[i] != lastZoneChangedText)
                    {
                        // zone changed!
                        lastZonechangedLine = logLines[i];
                        return true;
                    }
                }
            }
            return false;
        }

        private static void ParseZonechangedText_And_SetPauseFlag(string zonechangedText)
        {
            if (zonechangedText != lastZoneChangedText)
            {
                lastZoneChangedText = zonechangedText;


                foreach (string zone in needPauseZones)
                {
                    if (zonechangedText.Contains(zone))
                    {
                        if (!characterIsInPauseZone)
                            characterIsInPauseZone = true;
                    }
                    else if (characterIsInPauseZone)
                        characterIsInPauseZone = false;
                }
            }
        }

        private static long GetFileSize()
        {
            FileInfo fileInfo = new FileInfo(_logFilePath);
            return fileInfo.Length;
        }

        private static byte[] ReadFileBytes(long offset, long count)
        {
            using (FileStream fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] buffer = new byte[count];
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)count);
                return buffer;
            }
        }

        private static string BytesArrayToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }
    }
}
