using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager {

    private KatamariApp _app;
    private List<AudioSource> _channels;
    private int _currentChannel = 0;

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
        }

        public void Touch()
        {
            ChangedAt = Time.time;
        }
    }
    
    private PlayState _lastPlayState;
    private PlayState _currentPlayState;

	public void Setup( KatamariApp app, int numChannels )
    {
        _app = app;

        GameObject appProxyGo = _app.KatamariAppProxy.gameObject;

        _channels = new List<AudioSource>(numChannels);
        for( int i = 0; i < numChannels; ++i )
        {
            AudioSource s = appProxyGo.AddComponent<AudioSource>();
            _channels.Add(s);
        }
    }

    public void Teardown()
    {
        for( int i = 0; i < _channels.Count; ++i )
        {
            // Destroy the component on the GameObject
            GameObject.Destroy(_channels[i]);
        }
        _channels.Clear();
        _channels = null;
    }

    public void OnUpdate( float dt )
    {
        if (_lastPlayState != null)
        {
            float volume = Fade(_lastPlayState, _lastPlayState.Volume, 0f, Time.time);
            if( Mathf.Approximately(volume, 0f ) )
            {
                _lastPlayState.Unload( _channels );
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
        AudioSource source = _channels[s.Channel];

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


    public void PlayMusic( AudioClip clip, float volume, float fadeTime = 0f )
    {
        if (clip != null)
        {
            if (_lastPlayState != null)
            {
                _lastPlayState.Unload(_channels);
                _lastPlayState.Touch();
            }

            _lastPlayState = _currentPlayState;
            if(_lastPlayState != null )
            {
                _lastPlayState.Touch();
            }
            
            _currentChannel = (_currentChannel + 1) % _channels.Count;

            _currentPlayState = new PlayState()
            {
                Channel = _currentChannel,
                Volume = volume,
                ChangedAt = Time.time,
                FadeTime = fadeTime,
                Clip = clip
            };
            
            _channels[_currentChannel].clip = clip;
            _channels[_currentChannel].Play();
        }
    }
}
