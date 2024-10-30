using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ally or Enemy Colors", menuName = Defaults.ScriptableObject + "/ Ally Or Enemy Colors")]
[SerializeField]
public class AllyOrEnemyColors : ScriptableObject
{
    private Dictionary<AllyOrEnemy, Color> colors = new Dictionary<AllyOrEnemy, Color>();

    public Color this[AllyOrEnemy index]
    {
        get => colors[index];
        set => colors[index] = value;
    }

    [SerializeField] private Color none = Color.white;
    [SerializeField] private Color ally = Color.blue;
    [SerializeField] private Color enemy = Color.red;

    public AllyOrEnemyColors()
    {
        colors[AllyOrEnemy.None] = none;
        colors[AllyOrEnemy.Ally] = ally;
        colors[AllyOrEnemy.Enemy] = enemy;
    }



}

public enum AllyOrEnemy { None = 0, Ally = 1, Enemy = 2 }