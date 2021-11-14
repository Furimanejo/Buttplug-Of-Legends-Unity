using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityJSON;

[JSONObject(ObjectOptions.IgnoreUnknownKey)]
public class LeagueEventData
{
    public int EventID = -1;
    public double EventTime = -1f;
    public string EventName = default;
    public string VictimName = default;
    public string KillerName = default;
    public List<string> Assisters = new List<string>();
    public string Result = default;
}