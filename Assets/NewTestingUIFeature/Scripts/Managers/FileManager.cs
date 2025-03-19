using System.IO;
using System;
using UnityEngine;

public class FileManager
{
    public static bool WriteToFile(string fileName, string contents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(fileName, contents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error writing to file {fullPath}: " + e.Message);
            return false;
        }
    }
    public static bool LoadFromFile(string fileName, out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(fullPath))
        {
            File.WriteAllText(fullPath, "");
        }
        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading from file {fullPath}: " + e.Message);
            result = null;
            return false;
        }
    }
}