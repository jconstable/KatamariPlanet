using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "UISounds Data")]
public class UISounds : ScriptableObject
{
    public enum SoundEvent
    {
        MenuForwards,
        MenuBack,
        LevelSelect
    }

    [System.Serializable]
    public class SoundMap
    {
        public SoundEvent Event;
        public AudioClip Clip;
    }

    public SoundMap[] Sounds;
}
