using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Scores scores = default;
    public string summonerName = default;

    public class Scores
    {
        public int creepScore = default;
        public float wardScore = default;
    }
}
