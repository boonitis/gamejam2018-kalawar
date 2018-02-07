using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    Text scoreUIText;

    int score;

    public int Score
    {
        get
        {
            return this.score;
        }
        set
        {
            score = value;
            UpdateScoreTextUI();
        }
    }

	// Use this for initialization
	void Start () {

        //Get the Text UI commponent of this gameObject
        scoreUIText = GetComponent<Text>();
	}

    //Function to update the score text UI
    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:000000000}", Score);
        scoreUIText.text = scoreStr;
    }   
}
