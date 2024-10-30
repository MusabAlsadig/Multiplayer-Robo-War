using System;
using UnityEngine;


[CreateAssetMenu(fileName = "__________ Scenes Holder", menuName = Defaults.ScriptableObject + "/ Scenes Holder")]
[Serializable]
public class ScenesHolder : ScriptableObject
{
    public Level[] levels;

    [Serializable]
    public class Level
    {
        public string name;
        /// <summary>
        /// a texture that show this map
        /// </summary>
        public Texture texture;

        public string actualName;
    }
}


