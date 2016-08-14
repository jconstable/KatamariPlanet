using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AppBootstrap {

    private static bool _wasPlaying = false;

    static AppBootstrap()
    {
        EditorApplication.playmodeStateChanged += PlayStateChanged;
    }

    static void PlayStateChanged()
    {
        if( !_wasPlaying && EditorApplication.isPlaying )
        {
            _wasPlaying = true;

            UnityEditor.SceneManagement.EditorSceneManager.LoadScene( Files.BootstrapSceneName );
        } else
        {
            _wasPlaying = false;
        }
    }
}
