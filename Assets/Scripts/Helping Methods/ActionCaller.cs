using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HelpingMethods
{

    public static class ActionCaller
    {
        public static async void CallAfterframes(Action action, byte frames)
        {
            if (action == null)
                return;

            for (int i = 0; i <= frames; i++)
            {
                await Task.Yield();
            }

            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }
    }

}