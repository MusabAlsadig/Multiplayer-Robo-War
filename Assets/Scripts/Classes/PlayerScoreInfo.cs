using Unity.Netcode;
using System;
using UnityEngine;

public struct PlayerScoreInfo : INetworkSerializable, IEquatable<PlayerScoreInfo>
{
    public ulong playerId;
    public TeamName team;
    public ushort kills;
    public ulong points;

    // unserialized
    public Score Score => new Score(kills, points);

    public bool Equals(PlayerScoreInfo other)
    {
        return playerId == other.playerId;
    }

    void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        serializer.SerializeValue(ref playerId);
        serializer.SerializeValue(ref team);
        serializer.SerializeValue(ref kills);
        serializer.SerializeValue(ref points);
    }

    public PlayerScoreInfo(ulong _playerId, TeamName _team)
    {
        playerId = _playerId;
        team = _team;
        kills = 0;
        points = 0;
    }

    public static PlayerScoreInfo IdChecker(ulong _id)
    {
        return new PlayerScoreInfo() { playerId = _id };
    }
}


public struct Score
{
    public ushort kills;
    public ulong points;

    public Score(ushort kills, ulong points)
    {
        this.kills = kills;
        this.points = points;
    }

    public void Add(Score score)
    {
        kills += score.kills;
        points += score.points;
    }

    public static Score operator +(Score a, Score b)
    {
        a.Add(b);
        return a;
    }


    public readonly ulong Current
    {
        get
        {
            Mode mode = GameManager.Instance.CurrentMode;

            switch (mode)
            {
                case Mode.TDM:
                    return kills;

                case Mode.Survival:
                case Mode.CTF:
                    return points;

                default:
                    Debug.LogError("Unknown match mode");
                    return ulong.MaxValue;
            }
        }
    }

}
