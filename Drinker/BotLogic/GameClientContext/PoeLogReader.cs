﻿using System.IO;
using System.Text;

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

            bool finded_eng = TryGetLastZonechangedLine_ENG(logLines, out string lastZonechangedLine_eng, out int foundPos_eng);
            bool finded_rus = TryGetLastZonechangedLine_RUS(logLines, out string lastZonechangedLine_rus, out int foundPos_rus);

            if (lastZonechangedLine_eng != lastZoneChangedText && lastZonechangedLine_rus != lastZoneChangedText)
            {
                if (finded_eng && !finded_rus)
                {
                    lastZonechangedLine = lastZonechangedLine_eng;
                    return true;
                }
                else if (!finded_eng && finded_rus)
                {
                    lastZonechangedLine = lastZonechangedLine_rus;
                    return true;
                }
                else if (finded_eng && finded_rus)
                {
                    lastZonechangedLine = foundPos_eng > foundPos_rus ? lastZonechangedLine_eng : lastZonechangedLine_rus;
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetLastZonechangedLine_RUS(string[] logLines, out string lastZonechangedLine, out int foundPos)
        {
            lastZonechangedLine = "non";
            foundPos = -1;
            for (int i = logLines.Length - 1; i >= 0; i--)
            {
                if (logLines[i].Contains(zonesChangedText[1]))
                {
                    foundPos = i;
                    lastZonechangedLine = logLines[i];
                    return true;
                }
            }
            return false;
        }

        private static bool TryGetLastZonechangedLine_ENG(string[] logLines, out string lastZonechangedLine, out int foundPos)
        {
            lastZonechangedLine = "non";
            foundPos = -1;
            for (int i = logLines.Length - 1; i >= 0; i--)
            {
                if (logLines[i].Contains(zonesChangedText[0]))
                {
                    foundPos = i;
                    lastZonechangedLine = logLines[i];
                    return true;
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
                        {
                            characterIsInPauseZone = true;
                            return;
                        }
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
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
