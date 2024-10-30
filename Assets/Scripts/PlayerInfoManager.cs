using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{

    [SerializeField] private Transform info_hp_fill;
    [SerializeField] private TextMesh txt;
    [Space]
    [SerializeField] private AllyOrEnemyColors colors;

    public Transform HealthBar => info_hp_fill;

    public void SetUp(string playerName, AllyOrEnemy a)
    {
        txt.text = playerName;
        txt.color = colors[a];
        info_hp_fill.GetComponent<SpriteRenderer>().color = colors[a];
    }

    private void Update()
    {
        if (GameManager.Instance.LocalPlayer == null)
        {
            Debug.LogWarning("Local player still missing", this);
            return;
        }

        transform.LookAt(GameManager.Instance.LocalPlayer.transform);
    }
}
