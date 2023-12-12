using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public const int NUM_SAVES = 3;
    public static string SaveScene;
    public static bool HasCloneCollected;
    public static bool HasMetalCollected;
    public static bool HasMidairCollected;
    public static bool HasStraightCollected;
    public static bool HasHoldJumpCollected;
    public static int NumOfClonePacks;
    public static int NumOfHealthPacks;
    public static int CurrentSaveNum;

    public static void Save(string saveScene)
    {
        SaveScene = saveScene;
        PowerupStatus.SavePowerups();
        SaveToFile(CurrentSaveNum);
    }

    public static void SaveToFile(int number)
    {
        if(number < 1 || number > NUM_SAVES)
        {
            Debug.LogError("Int " + number + " is not in range. Not Writing.");
            return;
        }
        //Create (and overwrite) save file.
        FileStream fileStream;

        fileStream = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav");
        
        StreamWriter streamWriter = new(fileStream);

        //Write Data.
        streamWriter.WriteLine(SaveScene);
        streamWriter.WriteLine(HasCloneCollected);
        streamWriter.WriteLine(HasMetalCollected);
        streamWriter.WriteLine(HasMidairCollected);
        streamWriter.WriteLine(HasStraightCollected);
        streamWriter.WriteLine(HasHoldJumpCollected);
        streamWriter.WriteLine(NumOfClonePacks);
        streamWriter.WriteLine(NumOfHealthPacks);
        streamWriter.Close();
        fileStream.Close();
    }

    public static void LoadSaveFromFile(int number)
    {
        if(number < 1 || number > NUM_SAVES)
        {
            Debug.LogError("Int " + number + " is not in range. Not Writing.");
            return;
        }

        FileStream fileStream;

        try
        {
            fileStream = File.OpenRead(Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav");
        }
        catch (Exception) //File does not exist. Create new and init.
        {
            fileStream = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav");
            StreamWriter streamWriter = new(fileStream);
            streamWriter.WriteLine("Factory1"); //replace with actual starting area scene name later on
            streamWriter.WriteLine(false.ToString());
            streamWriter.WriteLine(false.ToString());
            streamWriter.WriteLine(false.ToString());
            streamWriter.WriteLine(false.ToString());
            streamWriter.WriteLine(false.ToString());
            streamWriter.WriteLine(0.ToString());
            streamWriter.WriteLine(0.ToString());
            streamWriter.Close();
        }
        fileStream = File.OpenRead(Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav");
        StreamReader streamReader = new(fileStream);
        SaveScene               = streamReader.ReadLine();
        HasCloneCollected       = GetBool(streamReader.ReadLine());
        HasMetalCollected       = GetBool(streamReader.ReadLine());
        HasMidairCollected      = GetBool(streamReader.ReadLine());
        HasStraightCollected    = GetBool(streamReader.ReadLine());
        HasHoldJumpCollected    = GetBool(streamReader.ReadLine());
        NumOfClonePacks         = GetInt(streamReader.ReadLine());
        NumOfHealthPacks        = GetInt(streamReader.ReadLine());
        
        CurrentSaveNum = number;
        streamReader.Close();
        fileStream.Close();
        
        static bool GetBool(string str)
        {
            if (bool.TryParse(str, out bool result)) return result;
            else
            {
                Debug.LogError("String " + str + " is not a valid boolean. Returning false.");
                return false;
            }
        }

        static int GetInt(string str)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            else
            {
                Debug.LogError("String " + str + " is not a valid integer. Returning 0.");
                return 0;
            }
        }
    }
}
