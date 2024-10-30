using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DotsText : MonoBehaviour
{
    [SerializeField] private int amount = 4;
    [SerializeField] private int delay;

    [SerializeField] private TextMeshProUGUI text_txt;

    private bool work;

    private void OnEnable()
    {
        work = true;
        Run();
    }
    private void OnDisable()
    {
        work = false;
    }


    private async void Run()
    {
        while (work)
        {
            text_txt.text = string.Empty;
            for (int i = 0; i < amount; i++)
            {
                text_txt.text += ".";
                await Task.Delay(delay);


                if (!work)
                    return;
            }
        }
    }

}
