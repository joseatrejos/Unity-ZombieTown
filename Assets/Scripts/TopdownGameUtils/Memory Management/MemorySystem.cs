using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Platform2DUtils.MemorySystem
{
    public class MemorySystem 
    {


        public static string SavePath
        {
            get => $"{Application.persistentDataPath}/";
        }

        static string path = $"{Application.persistentDataPath}/myGame.data";

        public static void SaveData(GameData gameData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);
            string json = JsonUtility.ToJson(gameData);
            bf.Serialize(file, json);
            file.Close();
            Debug.Log(path);
        }

        public static void SaveData(GameData gameData, string fileName)
        {
            string path = $"{Application.persistentDataPath}/{fileName}.data";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);
            string json = JsonUtility.ToJson(gameData);
            bf.Serialize(file, json);
            file.Close();
            Debug.Log(path);
        }

        public static bool DataExist
        {
            get => File.Exists(path);
        }

        public static bool DataSavesExist()
        {
            DirectoryInfo info = new DirectoryInfo(SavePath);
            FileInfo[] fileInfo = info.GetFiles();
            return fileInfo.Length > 0;
        }

        public static GameData LoadData()
        {
            if(DataExist)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                string json = bf.Deserialize(file) as string;
                GameData gameData = JsonUtility.FromJson<GameData>(json);
                return gameData;
            }

            return new GameData();
        }

        public static GameData LoadData(string path)
        {
            if(DataExist)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                string json = bf.Deserialize(file) as string;
                GameData gameData = JsonUtility.FromJson<GameData>(json);
                return gameData;
            }

            return new GameData();
        }

        public static void DeleteData()
        {
            if(DataExist) File.Delete(path);
        }
    }
}

