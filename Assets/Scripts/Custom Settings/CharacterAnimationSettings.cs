using UnityEngine;


namespace CustomSettings
{
    [CreateAssetMenu(fileName = "Character Animation Settings", menuName = "Settings/CharacterAnimation")]
    public class CharacterAnimationSettings : SettingsBase
    {
        public float turnSpeed = 100;
        public bool smoothTheTurn = true;
        public float animationSpeedMultiplier = 1;

    }
}