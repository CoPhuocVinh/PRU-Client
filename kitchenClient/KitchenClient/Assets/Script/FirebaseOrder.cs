using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseOrder : MonoBehaviour
{
    DatabaseReference reference;

    [SerializeField] private bool isFullOrder = false;
    [SerializeField] GameObject notifyFull;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void AddRecipeData(string newValue)
    {
        GetIsFullOrder();
        if (isFullOrder)
        {
            notifyFull.SetActive(true);
            return;
        }

        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
        string newKey = timestamp.ToString();
        reference.Child("recipeData").Child(newKey).SetValueAsync(newValue).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"Failed to add recipe data: {task.Exception}");
                return;
            }

            Debug.Log("Recipe data added successfully.");
        });
    }

    void GetIsFullOrder()
    {
        reference.Child("isFullOrder").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"Failed to fetch IsFullOrder value: {task.Exception}");
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (snapshot != null && snapshot.Exists)
            {
                string isFullOrder = snapshot.Value.ToString();
                if (isFullOrder.Equals("True"))
                {
                    this.isFullOrder = true;
                    Debug.Log(isFullOrder);
                }
                else
                {
                    this.isFullOrder = false;
                    Debug.Log(isFullOrder);
                }
                
            }
            else
            {
                Debug.LogWarning("IsFullOrder data not found.");
            }
        });
    }
}
