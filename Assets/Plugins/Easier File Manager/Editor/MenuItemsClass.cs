using UnityEngine;
using UnityEditor;

namespace EasierFileManager
{
    internal static class MenuItemsClass
    {
        private static bool isEnabled;

        [InitializeOnLoadMethod]
        [MenuItem("EasierFileManager/Enable")]
        private static void EnableTheShortcuts()
        {
            EditorApplication.projectWindowItemOnGUI += MainClass.OnProjectWindowUpdate;
            isEnabled = true;

            if (Settings.useDebug_Log)
                Debug.Log("shortcuts enabled");
        }

        [MenuItem("EasierFileManager/Disable")]
        private static void DisableTheShortcuts()
        {
            EditorApplication.projectWindowItemOnGUI -= MainClass.OnProjectWindowUpdate;
            isEnabled = false;

            if (Settings.useDebug_Log)
                Debug.Log("shortcuts disabled");
        }

        [MenuItem("EasierFileManager/Settings")]
        private static void OpenSettings()
        {
            EditorWindow.GetWindow(typeof(Settings));
        }

        #region Validations


        [MenuItem("EasierFileManager/Enable", true)]
        private static bool CanEnableTheShortcuts()
        {
            return !isEnabled;
        }

        [MenuItem("EasierFileManager/Disable", true)]
        private static bool CanDisableTheShortcuts()
        {
            return isEnabled;
        } 
        #endregion

    }
}
