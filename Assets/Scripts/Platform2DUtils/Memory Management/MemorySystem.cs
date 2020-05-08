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

        ///<summary>
        /// Saves the game data into a JSON file
        ///<param name="gameData">Instance of gameData to save</param>
        ///</summary>
        public static void SaveData(GameData gameData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);
            string json = JsonUtility.ToJson(gameData);
            bf.Serialize(file, json);
            file.Close();
            Debug.Log(path);
        }

        ///<summary>
        /// Saves the game data into a JSON file specifying the filename
        ///<param name="gameData">Instance of gameData to save</param>
        ///<param name="fileName">The name of the file</param>
        ///</summary>
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

        ///<summary>
        /// Returns if exists the data file
        ///</summary>
        public static bool DataExist
        {
            get => File.Exists(path);
        }

        ///<summary>
        /// Returns if exists data files
        ///</summary>
        public static bool DataSavesExist()
        {
            DirectoryInfo info = new DirectoryInfo(SavePath);
            FileInfo[] fileInfo = info.GetFiles();
            return fileInfo.Length > 0;
        }

        ///<summary>
        /// Load the game data from a JSON file
        ///</summary>
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

        ///<summary>
        /// Load the game data from a JSON file specifying the path
        ///<param name="path">The path of the data file</param>
        ///</summary>
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

        ///<summary>
        /// Delete the game data file
        ///</summary>
        public static void DeleteData()
        {
            if(DataExist) File.Delete(path);
        }
    }
}

