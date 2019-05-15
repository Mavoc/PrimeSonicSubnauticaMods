﻿namespace Common
{
    using System.IO;
    using Common.EasyMarkup;

    internal static class SaveDataExtensions
    {
        public static void Save<T>(this T data, string directory, string fileLocation) where T : EmProperty
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllLines(fileLocation, new[]
            {
                "# Generated with EasyMarkup #",
                data.ToString(),
            });
        }

        public static bool Load<T>(this T data, string directory, string fileLocation) where T : EmProperty
        {
            if (!File.Exists(fileLocation))
            {
                data.Save(directory, fileLocation);
                return false;
            }

            string serializedData = File.ReadAllText(fileLocation);

            bool validData = data.FromString(serializedData);

            if (!validData)
            {
                data.Save(directory, fileLocation);
                return false;
            }

            return true;
        }
    }
}
