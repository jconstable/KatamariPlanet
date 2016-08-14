using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelScoreHub : MonoBehaviour {

    public Text ScoreLabel;
    public AudioClip ScoreTickSound;

    private int _score = 0;
    private int _lastScore = -1;

	// Use this for initialization
	void Start () {
        EventManager.AddListener(LevelStats.UpdatedScoreEventName, UpdateScore);

        ScoreLabel.text = 0.ToString("N0");

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
