using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;


namespace EasierFileManager
{
    public class Utilities
    {

        internal static bool TrySelectNextObject(string firstLetter)
        {
            if (!TryGetSelectedFolderName(out string path))
            {
                if (Settings.useDebug_Log)
                    Debug.Log("having truble locating current folder");

                return false;
            }

            List<Object> assets = GetAssetsAt(path);
            var validAssets = assets.Where(x => x.name.StartsWith(firstLetter, true, CultureInfo.CurrentCulture));

            var filteredValidAssets = validAssets.SkipWhile(x => x != Selection.activeObject);

            if (validAssets.Count() == 0)
            {
                EditorApplication.Beep();

                if (Settings.useDebug_Log)
                    Debug.Log("No object/folder with the letter:<color=red> " + firstLetter + "</color>");

                return false;
            }

            Object objectToSelect = null;
            // get the object after the selected one
            objectToSelect = filteredValidAssets.FirstOrDefault(x => x != Selection.activeObject);

            // if no items left, go to the first valid object
            if (objectToSelect == null)
                objectToSelect = validAssets.First();

            Selection.activeObject = objectToSelect;

            return false;
        }

        internal static bool TryGetSelectedFolderName(out string path)
        {
            var _tryGetActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

            object[] args = new object[] { null };
            bool found = (bool)_tryGetActiveFolderPath.Invoke(null, args);
            path = (string)args[0];

            return found;
        }


        #region Private methodes

        private static bool IsInsideProjectWindow(bool focusIfPosible = true)
        {
            EditorWindow mouseOverWindow = EditorWindow.mouseOverWindow;

            if (mouseOverWindow == null)
                return false;

            if (!mouseOverWindow.ToString().Contains("ProjectBrowser"))
                return false;

            if (focusIfPosible)
                mouseOverWindow.Focus();

            return true;
        }


        private static List<Object> GetAssetsAt(string path)
        {
            var subDirectories = Directory.GetDirectories(path);
            var subFiles = Directory.GetFiles(path);

            List<Object> assets = new List<Object>();
            foreach (var s in subDirectories)
            {
                string p = s.Substring(s.LastIndexOf("Assets"));
                assets.Add(AssetDatabase.LoadMainAssetAtPath(p));
            }

            foreach (var s in subFiles)
            {
                if (s.EndsWith(".meta"))
                    continue;

                string p = s.Substring(s.LastIndexOf("Assets"));
                assets.Add(AssetDatabase.LoadMainAssetAtPath(p));

            }

            return assets;
        }
        internal static void GoUp()
        {
            IsInsideProjectWindow();
            if (!TryGetSelectedFolderName(out string path))
                return;

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        }
        #endregion
    }
}
