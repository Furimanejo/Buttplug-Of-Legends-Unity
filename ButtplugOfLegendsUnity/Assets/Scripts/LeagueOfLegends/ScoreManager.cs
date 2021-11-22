using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    float score = 0;
    [SerializeField] Text scoreText;

    [Space]
    [SerializeField] InputField decayPerMinute;
    [SerializeField] InputField gameStart;
    [SerializeField] InputField minionSpawning;
    [SerializeField] InputField kill;
    [SerializeField] InputField assist;
    [SerializeField] InputField death;
    [SerializeField] InputField destroyTurret;
    [SerializeField] InputField destroyInhibitor;
    [SerializeField] InputField win;
    [SerializeField] InputField lose;

    void Update()
    {
        if (decayPerMinute.text != "-")
            score -= Time.deltaTime * int.Parse(decayPerMinute.text) / 60f; //decay by time
        score = Mathf.Clamp(score, 0f, score);
        scoreText.text = score.ToString("F0");
    }

    public float GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void ApplyEvent(LeagueEventData eventData, string playerName)
    {
        switch (eventData.EventName)
        {
            case "GameStart":
                score += float.Parse(gameStart.text);
                break;

            case "MinionsSpawning":
                score += float.Parse(minionSpawning.text);
                break;

            case "ChampionKill":
                if (eventData.KillerName == playerName)
                    score += float.Parse(kill.text);
                if (eventData.Assisters.Contains(playerName))
                    score += float.Parse(assist.text);
                if (eventData.VictimName == playerName)
                    score += float.Parse(death.text);
                break;

            case "TurretKilled":
                if (eventData.KillerName == playerName || eventData.Assisters.Contains(playerName))
                    score += float.Parse(destroyTurret.text);
                break;

            case "InhibKilled":
                if (eventData.KillerName == playerName || eventData.Assisters.Contains(playerName))
                    score += float.Parse(destroyInhibitor.text);
                break;

            case "GameEnd":
                if (eventData.Result == "Win")
                    score += float.Parse(win.text);
                else
                    score += float.Parse(lose.text);
                break;
        }
    }
}
