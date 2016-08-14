using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelScoreHub : MonoBehaviour {

    public Text ScoreLabel;
    public AudioClip ScoreTickSound;

    private int _score = 0;
    private int _lastScore = -1;

    private EventManager _eventManager;

	// Use this for initialization
	public void Setup ( EventManager eventManager ) {
        _eventManager = eventManager;

        _eventManager.AddListener(LevelStats.UpdatedScoreEventName, UpdateScore);

        ScoreLabel.text = 0.ToString("N0");
	}

    public void Teardown()
    {
        _eventManager.RemoveListener(LevelStats.UpdatedScoreEventName, UpdateScore);

        _eventManager = null;
    }

    bool UpdateScore(object p)
    {
        int newScore = (int)p;
        _score = newScore;

        StartCoroutine(UIHelpers.TweenTextNumberValueCoroutine(ScoreLabel, _lastScore, _score, 0.5f));
        _lastScore = _score;

        return false;
    }
}
