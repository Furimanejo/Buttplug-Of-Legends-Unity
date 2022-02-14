using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int creepScore = 0;
    float wardScore = 0f;
    float BPScore = 0;
    [SerializeField] Text scoreText;

    [Space]
    [SerializeField] InputField decayPerMinute;
    //[SerializeField] InputField gameStart;
    //[SerializeField] InputField minionSpawning;
    [SerializeField] InputField kill;
    [SerializeField] InputField assist;
    [SerializeField] InputField death;
    [SerializeField] InputField destroyTurret;
    [SerializeField] InputField destroyInhibitor;
    [SerializeField] InputField win;
    [SerializeField] InputField lose;
    [SerializeField] InputField tenCreep;
    [SerializeField] InputField wardScoreMultiplier;

    List<InputField> inputFields = new List<InputField>();

    private void OnEnable()
    {
        inputFields = new List<InputField>()
        {
            decayPerMinute, 
            kill, 
            assist, 
            death, 
            destroyTurret, 
            destroyInhibitor, 
            win, 
            lose, 
            tenCreep, 
            wardScoreMultiplier
        };
        LoadValuesFromPlayerPrefs(inputFields);
        Application.wantsToQuit += () => { SaveValuesToPlayerPrefs(inputFields); return true; };
    }
    
    void LoadValuesFromPlayerPrefs(List<InputField> inputFields)
    {
        foreach (var field in inputFields)
        {
            var value = PlayerPrefs.GetInt(GetFieldName(field), int.MinValue);
            if (value != int.MinValue)
                field.text = value.ToString();
        }
    }
    void SaveValuesToPlayerPrefs(List<InputField> inputFields)
    {
        foreach (var field in inputFields)
        {
            PlayerPrefs.SetInt(GetFieldName(field), int.Parse(field.text));
        }
    }

    void Update()
    {
        if (decayPerMinute.text != "-")
            BPScore -= Time.deltaTime * int.Parse(decayPerMinute.text) / 60f; //decay by time
        BPScore = Mathf.Clamp(BPScore, 0f, BPScore);
        scoreText.text = BPScore.ToString("F0");
    }

    public float GetScore()
    {
        return BPScore;
    }

    public void ResetScore()
    {
        BPScore = 0;
    }

    public void ApplyEvent(LeagueEventData eventData, string playerName)
    {
        switch (eventData.EventName)
        {
            case "GameStart":
                creepScore = 0;
                wardScore = 0;
                //BPScore += ParseField(gameStart);
                break;

            case "MinionsSpawning":
                //BPScore += ParseField(minionSpawning);
                break;

            case "ChampionKill":
                if (eventData.KillerName == playerName)
                    BPScore += ParseField(kill);
                if (eventData.Assisters.Contains(playerName))
                    BPScore += ParseField(assist);
                if (eventData.VictimName == playerName)
                    BPScore += ParseField(death);
                break;

            case "TurretKilled":
                if (eventData.KillerName == playerName || eventData.Assisters.Contains(playerName))
                    BPScore += ParseField(destroyTurret);
                break;

            case "InhibKilled":
                if (eventData.KillerName == playerName || eventData.Assisters.Contains(playerName))
                    BPScore += ParseField(destroyInhibitor);
                break;

            case "GameEnd":
                if (eventData.Result == "Win")
                    BPScore += ParseField(win);
                else
                    BPScore += ParseField(lose);
                break;
        }
    }

    public void UpdateCreepsAndWards(PlayerData.Scores scores)
    {
        if(scores.creepScore != creepScore)
        {
            BPScore += ParseField(tenCreep);
            creepScore = scores.creepScore;
        }
        if(scores.wardScore != wardScore)
        {
            BPScore += (scores.wardScore - wardScore) * ParseField(wardScoreMultiplier);
            wardScore = scores.wardScore;
        }
    }

    float ParseField(InputField field)
    {
        return float.Parse(field.text);
    }

    string GetFieldName(InputField field)
    {
        return field.gameObject.transform.parent.name;
    }
}
