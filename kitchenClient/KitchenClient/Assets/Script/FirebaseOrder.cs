using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseOrder : MonoBehaviour
{
    DatabaseReference reference;

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
        string newKey = reference.Child("recipeData").Push().Key;
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
}
