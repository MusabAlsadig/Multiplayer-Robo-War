using UnityEngine;

namespace CustomSettings
{
    [CreateAssetMenu(fileName = "Helper Settings", menuName = "Settings/HelperSettings")]
    public class HelperSettings : SettingsBase
    {
        public bool callActionsNormaly;


        public void Init()
        {
            callActionsNormaly = false;
        }
    }

}