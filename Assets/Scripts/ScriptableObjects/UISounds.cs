using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "UISounds Data")]
public class UISounds : ScriptableObject
{
    public enum SoundEvent
    {
        MenuForwards,
        MenuBack,
        LevelSelect,
        PointTick
    }

    [System.Serializable]
    public class SoundMapStatic
    {
        public SoundEvent Event;
        public AudioClip Clip;
    }

    [System.Serializable]
    public class SoundMapCustom
    {
        public string clipName;
        public AudioClip Clip;
    }

    public List< SoundMapStatic > StaticSounds;
    public List< SoundMapCustom > CustomSounds;

    public AudioClip FindClipBySoundEvent( SoundEvent eventType )
    {
        AudioClip clip = null;
        SoundMapStatic staticMap = StaticSounds.Find(x => x.Event == eventType);
        if( staticMap != null )
        {
            clip = staticMap.Clip;
        }

        return clip;
    }

    public AudioClip FindClipByName( string customName )
    {
        AudioClip clip = null;
        SoundMapCustom custom = CustomSounds.Find(x => x.clipName == customName);
        if( custom != null )
        {
            clip = custom.Clip;
        }
        return clip;
    }
}
