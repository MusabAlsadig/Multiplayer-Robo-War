using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public void TestWaiting()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Dialogue.ShowWaitingPanel("doing a test", cancellationTokenSource);

        _ = TestTask(cancellationTokenSource.Token);
    }

    private async Task TestTask(CancellationToken token)
    {
        Debug.Log("<color=green>starting</color> the test");
        for (int i = 0; i < 2000; i++)
        {
            if (token.IsCancellationRequested)
            {
                Debug.Log("<color=blue>canceled</color> the test");
                token.ThrowIfCancellationRequested();
            }
            await Task.Delay(1);
            Debug.Log("<color=yellow>running</color> the test");
        }
        Debug.Log("<color=red>finishing</color> the test");
    }
}
