using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//https://github.com/adragonite/UnityJSON#deserialization
using UnityJSON;


public class ButtplugOfLegendsUnity : MonoBehaviour
{
    string nameURL = "https://127.0.0.1:2999/liveclientdata/activeplayername";
    string eventURL = "https://127.0.0.1:2999/liveclientdata/eventdata";
    string playerName = string.Empty;
    int countOfEventsInLastEvaluation = 0;

    [SerializeField] List<ButtplugController> controllers;
    [SerializeField] ScoreManager scoreManager;

    void Start()
    {
        StartCoroutine(GetClientDataRoutine());
    }
    
    void Update()
    {
        foreach(var controller in controllers)
            controller.SetValue(scoreManager.GetScore()/100f);
    }

    IEnumerator GetClientDataRoutine()
    {
        while (true)
        {
            if (PlayerNameIsValid() == false)
            {
                var request = CreateGetRequest(nameURL);
                yield return request.SendWebRequest();
                if (request.responseCode == 200)
                    playerName = request.downloadHandler.text.Replace("\"", "");
            }
            else
            {
                var request = CreateGetRequest(eventURL);
                yield return request.SendWebRequest();
                if (request.responseCode == 200)
                {
                    var response = request.downloadHandler.text;
                    var events = JSON.Deserialize<Dictionary<string, List<LeagueEventData>>>(response)["Events"];
                    if(countOfEventsInLastEvaluation != events.Count)
                        EvaluateEvents(events);
                    countOfEventsInLastEvaluation = events.Count;
                }
            }
        }
    }

    void EvaluateEvents(List<LeagueEventData> events)
    {
        if (events.Count == 0)
            return;
        float timeOfLastEvent = events[events.Count - 1].EventTime;
        foreach (var eventData in events)
        {
            if(eventData.EventTime == timeOfLastEvent)
            {
                scoreManager.ApplyEvent(eventData, playerName);
            }
        }
    }

    bool PlayerNameIsValid()
    {
        return string.IsNullOrEmpty(playerName) == false && playerName != "\"\"";
    }

    UnityWebRequest CreateGetRequest(string URL)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
        request.certificateHandler = new BypassCertificateHandler(); //Avoid Curl error 60: Cert verify failed: UNITYTLS_X509VERIFY_FLAG_NOT_TRUSTED
        return request;
    }

    class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}