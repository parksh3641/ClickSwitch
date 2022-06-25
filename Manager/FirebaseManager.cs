using Firebase;
using Firebase.Analytics;
using Firebase.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    FirebaseApp app;
    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;

                Debug.Log("파이어베이스 앱 초기화 완료");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        if (e != null)
        {
            Debug.LogFormat("[FIREBASE] Token: {0}", e.Token);
        }
    }

    void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e != null && e.Message != null && e.Message.Notification != null)
        {
            Debug.LogFormat("[FIREBASE] From: {0}, Title: {1}, Text: {2}",
                e.Message.From,
                e.Message.Notification.Title,
                e.Message.Notification.Body);
        }
    }
}
