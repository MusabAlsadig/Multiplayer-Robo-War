using UnityEngine;

namespace CustomSettings
{
    public class StaticSettingsHolder : MonoBehaviour
    {
        public HelperSettings helperSettings;


        public static StaticSettingsHolder Instance;
        private void Awake()
        {
            Instance = this;

            helperSettings.Init();
        }
    }

}