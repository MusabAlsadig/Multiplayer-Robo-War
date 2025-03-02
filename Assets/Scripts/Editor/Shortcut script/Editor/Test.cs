using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static EasierFileManager.Utilities;

namespace Assets
{
    [ExecuteInEditMode]
    internal class Test : EditorWindow
    {
        [MenuItem("Example/Simple Recorder")]
        static void Init()
        {
            var window =
                (Test)EditorWindow.GetWindow(typeof(Test));

            
        }
        [InitializeOnLoadMethod]
        private static void AddTheShortcuts()
        {
            EditorApplication.projectWindowItemOnGUI += UpdateTest;
            Debug.Log("Onreload");
        }

        private static void UpdateTest(string guid, Rect selectionRect)
        {

            if (Keyboard.current.altKey.isPressed)
            {
                HandleAltKey();
                return;
            }

            char? currentLetter = null;
            foreach(var key in Keyboard.current.allKeys)
            {
                if (key.isPressed && IsLetterKey(key.keyCode, out char letter))
                {
                    currentLetter = letter;
                    break;
                }
            }

            if (currentLetter == null)
                return;


            SelectNextObject(currentLetter.Value.ToString());
        }


        private static bool IsLetterKey(Key key, out char letter)
        {
            return char.TryParse(key.ToString(), out letter);
        }

        private static void HandleAltKey()
        {
            // still npt working
            Debug.Log("Handling alt");
            if (Keyboard.current.upArrowKey.isPressed)
            {
                GoUp();
                Debug.Log("going up");
            }
        }
    }
}
