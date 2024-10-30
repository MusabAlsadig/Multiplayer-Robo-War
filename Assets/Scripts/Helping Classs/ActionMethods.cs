using System;
using Delegate = System.Delegate;
using UnityEngine;
using UnityEngine.Events;
using CustomSettings;

namespace HelpingMethods
{
    public static class ActionMethods
    {
        /// <summary>
        /// transfer methods from UnityAction to System.Action
        /// </summary>
        /// <param name="unityAction"></param>
        /// <returns></returns>
        public static Action CreateAction(UnityAction unityAction)
        {
            void result() => unityAction?.Invoke(); // creat a method to handle the invoke

            return result;
        }

        /// <summary>
        /// catch an error on any method, report it, then contiue calling the rest
        /// </summary>
        /// <param name="action"></param>
        public static void CallMethodsSeperatly(Action action)
        {
            if (action == null)
                return;

            Delegate[] allMethods = action.GetInvocationList();
            if (allMethods.Length == 0)
                return;
            for (int i = 0; i < allMethods.Length; i++)
            {
                var method = allMethods[i];
                try
                {
                    method.DynamicInvoke();
                }

                catch (Exception e)
                {
                    Debug.LogError($"<color=red>{method.Target}/{method.Method.Name}</color> have an error and will be ignored\n" +
                        $"Error message : \n" +
                        $"{e.Message}");
                }

            }
        }

        /// <summary>
        /// catch an error on any method, report it, then contiue calling the rest
        /// </summary>
        /// <param name="action"></param>
        public static void CallMethodsSeperatly<T>(Action<T> action, T param)
        {
            if (action == null)
                return;

            Delegate[] allMethods = action.GetInvocationList();
            if (allMethods.Length == 0)
                return;
            for (int i = 0; i < allMethods.Length; i++)
            {
                var method = allMethods[i];
                try
                {
                    method.DynamicInvoke(param);
                }

                catch (Exception e)
                {
                    if (StaticSettingsHolder.Instance && StaticSettingsHolder.Instance.helperSettings.callActionsNormaly)
                        action.Invoke(param);
                    else
                        Debug.LogError($"<color=red>{method.Target}/{method.Method.Name}</color> have an error and will be ignored\n" +
                            $"Error message : \n" +
                            $"{e.Message}");
                }

            }
        }

    }

}
