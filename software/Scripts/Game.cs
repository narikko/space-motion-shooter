using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    
    public static Game instance { get; private set; }

    public TextMeshProUGUI textField;
    public static TextMeshProUGUI textMeshProUGUI;

    private static int numberOfObjectives = 2;
    public static void ObjectiveDestroyed()
    {
        numberOfObjectives -= 1;
        textMeshProUGUI.text = "Objective Remaining: " + numberOfObjectives.ToString();

        if (numberOfObjectives == 0)
        {
            //win
            textMeshProUGUI.text = "YOU WIN";
            numberOfObjectives = 2;
            Debug.Log("You win");
        }
    }

    private void Awake()
    {
        instance = this;
        textMeshProUGUI = textField;
        textMeshProUGUI.text = "Objective Remaining: " + numberOfObjectives.ToString();
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
