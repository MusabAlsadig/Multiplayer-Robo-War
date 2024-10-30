using HelpingMethods;
using MinimapClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private TeamName team;


    private static readonly List<RespawnPoint> allPoints = new List<RespawnPoint>();
    private static readonly Dictionary<TeamName, RespawnPoint> teamPoints = new Dictionary<TeamName, RespawnPoint>();

    private Vector3 Position => transform.position;

    private void Awake()
    {
        allPoints.Add(this);
        teamPoints[team] = this;
    }

    private void OnDestroy()
    {
        allPoints.Remove(this);
        teamPoints[team] = null;
    }


    public static void ChangePoseFor(Player _player ,TeamName _team)
    {
        print("someone asked for respawn point");

        RespawnPoint point;
        if (_team == TeamName.Solo)
        {
            point = GetRandomPointFor(_player);
        }
        else
        {
            if (!teamPoints.TryGetValue(_team, out point) || point == null)
                point = GetRandomPointFor(_player);
        }


        // TODO : if a player is very close, make some offset
        
        _player.transform.SetPositionAndRotation(point.transform.position, point.transform.rotation);
    }

    private static RespawnPoint GetRandomPointFor(Player _player)
    {
        List<RespawnPoint> validPoints = 
            allPoints.FindAll(point => !Searcher.AnyClosePlayers(point.Position, _player));

        if (validPoints.Count == 0)
        {
            Debug.LogWarning("No empty Respawn point for this player", _player);
            return null;
        }
        else
        {
            int index = Random.Range(0, validPoints.Count);
            return validPoints[index];
        }
    }
}
