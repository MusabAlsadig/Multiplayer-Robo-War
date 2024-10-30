using Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PrematchInfo_SubPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name_txt, state_txt;
    [SerializeField] private Image image;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Button kick_btn;
    [Space]
    [SerializeField] private TeamColorsHolder teamColorsHolder;

    private Color none;

    private ulong playerId;

    private void Awake()
    {
        none = image.color;
    }

    public void Setup(PlayerInfo info, bool IsServer)
    {
        playerId = info.id;

        bool isLocalPlayer = info.id == PrematchPlayer.LocalPlayer.OwnerClientId;

        indicator.SetActive(isLocalPlayer);

        kick_btn.gameObject.SetActive(IsServer && !isLocalPlayer); // you can't kick your self
        kick_btn.onClick.AddListener(AskToKick);

        Refresh(info);
    }

    public void Refresh(PlayerInfo info)
    {
        bool isLocalPlayer = info.id == PrematchPlayer.LocalPlayer.OwnerClientId;

        if (isLocalPlayer != indicator.activeSelf)
            indicator.SetActive(isLocalPlayer);

        if (info.team > TeamName.Solo)
            image.color = teamColorsHolder.Colors[info.team];
        else
            image.color = none;

        name_txt.text = info.name.ToString();
        state_txt.text = info.isReady.ToString();
    }

    private void AskToKick()
    {
        Dialogue.ShowPanel($"Sure to kick <color=red>{name_txt.text}</color> ?", Kick, canCancel: true);
    }

    private void Kick()
    {
        MyNetworkManager.singleton.KickPlayer(playerId);
    }
}
