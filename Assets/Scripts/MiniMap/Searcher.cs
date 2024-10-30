using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinimapClasses
{
    public class Searcher : MonoBehaviour
    {


        private static Searcher Instance { set; get; }

        private void Awake()
        {
            if (Instance != null)
                Debug.LogWarning("Searcher already have instance");

            Instance = this;

            
        }



        public static bool AnyClosePlayers(Vector3 position,Player asker, int range = 3)
        {
            List<Player> playerTransforms = Player.AllPlayers;
            bool haveClosePlayer = playerTransforms.Any(player 
                => Vector3.Distance(player.transform.position, position) < range &&
                   player != asker);
            return haveClosePlayer;
        }

    }
}