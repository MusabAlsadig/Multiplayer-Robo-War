using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static EasierFileManager.Utilities;
using static EasierFileManager.Settings;

namespace EasierFileManager
{
    internal class MainClass
    {
        private static float currentCooldown;

        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        static Task task;


        internal static void OnProjectWindowUpdate(string guid, Rect selectionRect)
        {
            var currentEvent = Event.current;
            

            if (currentEvent.type == EventType.KeyUp && task != null)
                Reset();

            else if (currentEvent.type == EventType.KeyDown)
            {
                if (IsLetterKey(currentEvent.keyCode, out char letter) && task == null)
                    task = RunTask(cancellationTokenSource.Token, letter);
            }
            else
                return;

        }

        private static bool IsLetterKey(KeyCode key, out char letter)
        {
            return char.TryParse(key.ToString(), out letter);
        }

        private static async Task RunTask(CancellationToken cancellationToken, char letter)
        {
            currentCooldown = maxCooldown;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();


                TrySelectNextObject(letter.ToString());

                await Task.Delay((int)(currentCooldown));

                if (currentCooldown > minCooldown)
                    currentCooldown = (currentCooldown * renewRate);

            }
        }

        private static void Reset()
        {
            Debug.Log("Reset");
            currentCooldown = maxCooldown;
            cancellationTokenSource.Cancel();

            task = null;
            cancellationTokenSource = new CancellationTokenSource();
        }
    }

    
}
