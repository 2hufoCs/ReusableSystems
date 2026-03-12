using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ResponseManager
{
    static Choice currentChoice;

    public static void SetChoiceResponse(Choice choice)
    {
        currentChoice = choice;
        switch (choice.name)
        {
            case "choice1":
                // Add listeners to buttons
                choice.OnChoose += TestResponse;
                break;
        }
    }

    public static void TestResponse(string response)
    {
        if (response == "Scene2Bedroom") Debug.Log("go to bed");
        if (response == "Scene3Library") Debug.Log("whoopsies");
    }
}
