using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Security.Permissions;

namespace Data
{

    [Serializable]
    public class GameData
    {
        public static GameData Current { get { return d; } }

        static GameData d = new GameData();

        public static string path = Application.persistentDataPath;

        #region Data to save
        public PlayerInfo playerInfo = new PlayerInfo();
        #endregion



        public static void Save()
        {

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, d);
            stream.Close();

            Debug.Log("Data has benn Saved");
        }


        public static void Load()
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                d = formatter.Deserialize(stream) as GameData;
                stream.Close();

                Debug.Log("Data has been Loaded");
            }

            else
            {
                d = new GameData();
                Debug.LogError("Faild to load data");
            }
        }

        public static void Delete()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("file deleted");
            }
            else
                Debug.Log("no file to delete");
        }
    }

}