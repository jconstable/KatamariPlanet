using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager {

    private KatamariApp _app;
    private List<AudioSource> _musicChannels;
    private List<AudioSource> _sfxChannels;
    private int _currentMusicChannel = 0;
    private int _currentSFXChannel = 0;

    private class PlayState
    {
        public int Channel;
        public AudioClip Clip;
        public float Volume;
        public float ChangedAt;
        public float FadeTime;

        public void Unload(List<AudioSource> channels)
        {
            channels[Channel].Stop();
            Resources.UnloadAsset(Clip);
            Clip = null;
            channels[Channel].clip = null;
        }

        public void Touch()
        {
            ChangedAt = Time.time;
        }
    }
    
    private PlayState _lastPlayState;
    private PlayState _currentPlayState;

    private UISounds _uiSounds;

	public void Setup( KatamariApp app, int numMusicChannels, int numSFXChannels )
    {
        _app = app;

        GameObject appProxyGo = _app.KatamariAppProxy.gameObject;

        _musicChannels = new List<AudioSource>(numMusicChannels);
        for( int i = 0; i < numMusicChannels; ++i )
        {
            AudioSource s = appProxyGo.AddComponent<AudioSource>();
            _musicChannels.Add(s);
        }

        _sfxChannels = new List<AudioSource>(numSFXChannels);
        for (int i = 0; i < numSFXChannels; ++i)
        {
            AudioSource s = appProxyGo.AddComponent<AudioSource>();
            _sfxChannels.Add(s);
        }

        _uiSounds = Resources.Load(Files.UISoundsResourcePath) as UISounds;
        DebugUtils.Assert(_uiSounds != null, "Unable to load UI sounds at resource path: " + Files.UISoundsResourcePath);
    }

    public void Teardown()
    {
        for( int i = 0; i < _musicChannels.Count; ++i )
        {
            // Destroy the component on the GameObject
            GameObject.Destroy(_musicChannels[i]);
        }
        _musicChannels.Clear();
        _musicChannels = null;

        for (int i = 0; i < _sfxChannels.Count; ++i)
        {
            // Destroy the component on the GameObject
            GameObject.Destroy(_sfxChannels[i]);
        }
        _sfxChannels.Clear();
        _sfxChannels = null;

        _uiSounds = null;
    }

    public void OnUpdate( float dt )
    {
        if (_lastPlayState != null)
        {
            float volume = Fade(_lastPlayState, _lastPlayState.Volume, 0f, Time.time);
            if( Mathf.Approximately(volume, 0f ) )
            {
                _lastPlayState.Unload(_musicChannels);
                _lastPlayState = null;
            }
        }    
        if( _currentPlayState != null )
        {
            Fade(_currentPlayState, 0f, _currentPlayState.Volume, Time.time);
        }
    }

    private float Fade( PlayState s, float from, float to, float time )
    {
        AudioSource source = _musicChannels[s.Channel];

        if (!Mathf.Approximately(source.volume, to))
        {
            if (s.FadeTime > 0f)
            {
                source.volume = Mathf.Lerp(from, to, (time - s.ChangedAt) / s.FadeTime);
            }
            else
            {
                _lastPlayState.Volume = to;
            }
        }

        return source.volume;
    }


    public void PlayMusic( AudioClip clip, float volume, float fadeTime = 0f, bool loop = true )
    {
        if (clip != null)
        {
            if (_lastPlayState != null)
            {
                _lastPlayState.Unload(_musicChannels);
                _lastPlayState.Touch();
            }

            _lastPlayState = _currentPlayState;
            if(_lastPlayState != null )
            {
                _lastPlayState.Touch();
            }

            _currentMusicChannel = (_currentMusicChannel + 1) % _musicChannels.Count;

            _currentPlayState = new PlayState()
            {
                Channel = _currentMusicChannel,
                Volume = volume,
                ChangedAt = Time.time,
                FadeTime = fadeTime,
                Clip = clip
            };

            AudioSource source = _musicChannels[_currentMusicChannel];
            source.clip = clip;
            source.Play();
            source.loop = true;
        }
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            _currentSFXChannel = (_currentSFXChannel + 1) % _sfxChannels.Count;

            AudioSource source = _sfxChannels[_currentSFXChannel];
            source.clip = clip;
            source.Play();
        }
    }

    public void PlayUISound( UISounds.SoundEvent eventType, float volume = 1f )
    {
        AudioClip clip = _uiSounds.FindClipBySoundEvent(eventType);
        if( clip != null )
        {
            PlayClip(clip, volume);
        }
    }

    public void PlayCustomSound( string soundName, float volume = 1f)
    {
        AudioClip clip = _uiSounds.FindClipByName(soundName);
        if (clip != null)
        {
            PlayClip(clip, volume);
        }
    }
}
