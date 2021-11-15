using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using FullSerializer;

public class ButtplugOfLegendsUnity : MonoBehaviour
{
    fsSerializer serializer = new fsSerializer();
    string nameURL = "https://127.0.0.1:2999/liveclientdata/activeplayername";
    string eventURL = "https://127.0.0.1:2999/liveclientdata/eventdata";
    string playerName = string.Empty;
    string eventResponseText = default;
    int countOfEventsInLastEvaluation = 0;

    [SerializeField] List<ButtplugController> controllers;
    [SerializeField] ScoreManager scoreManager;

    void Start()
    {
        StartCoroutine(UpdateClientData());
        Application.wantsToQuit += () => { LogClientData(); return true; } ;
    }
    
    void Update()
    {
        foreach(var controller in controllers)
            controller.SetValue(scoreManager.GetScore()/100f);
    }

    private void LogClientData()
    {
        Debug.Log($"playerName: {playerName}");
        Debug.Log($"events: {eventResponseText}");
    }

    IEnumerator UpdateClientData()
    {
        while (true)
        {
            yield return GetPlayerName();
            if(PlayerNameIsValid())
                yield return GetEvents();            
        }
    }

    IEnumerator GetPlayerName()
    {
        var request = CreateGetRequest(nameURL);
        yield return request.SendWebRequest();
        if (request.responseCode == 200)
            playerName = request.downloadHandler.text.Replace("\"", "");
    }

    IEnumerator GetEvents()
    {
        var request = CreateGetRequest(eventURL);
        yield return request.SendWebRequest();
        if (request.responseCode == 200)
        {
            eventResponseText = request.downloadHandler.text;
            var events = new Dictionary<string, List<LeagueEventData>>();
            var data = fsJsonParser.Parse(eventResponseText);
            serializer.TryDeserialize(data, ref events);

            var eventDataList = events["Events"];

            if (countOfEventsInLastEvaluation != eventDataList.Count)
                EvaluateEvents(eventDataList);
            countOfEventsInLastEvaluation = eventDataList.Count;            
        }
    }

    void EvaluateEvents(List<LeagueEventData> events)
    {
        if (events.Count == 0)
            return;
        var timeOfLastEvent = events[events.Count - 1].EventTime;
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