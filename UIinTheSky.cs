using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

//******************************//
//                              //
//  "//--" == Schuyler's Notes  //
//                              //
//******************************//

public class UIinTheSky : MonoBehaviour {

    string carrotsFoundText = "";
    string plucksRemainingText = "";
    string moneyEarnedText = "";

    public TextMesh textObject0;
    public TextMesh textObject1;
    public TextMesh textObject2;
    public TextMesh textObject3;

    private int UISkyDisplay = 3;


    // Use this for initialization
    void Start () {

        carrotsFoundText = "Carrots Found: " + StaticVariables.carrotsCollected;
        plucksRemainingText = "Plucks Remaining: " + StaticVariables.stemsCollected;
        moneyEarnedText = "Money Earned: $" + (StaticVariables.carrotsCollected * 0.25);
        //GameObject.FindGameObjectWithTag("UISky").SetActive(true);

        //-- Sets all the text objects (all four corners) to be the same --//
        textObject3.text = textObject0.text;
        textObject2.text = textObject0.text;
        textObject1.text = textObject0.text; 
        //UISkyVis();

    }
	
	// Update is called once per frame
	void Update () {

        //-- Sets what the text in the sky displays. --//
        if (UISkyDisplay == 1)
        {
            textObject0.text = GetCarrotsPicked();
        }
        else if (UISkyDisplay == 2)
        {
            textObject0.text = GetPlucksLeft();
        }
        else if (UISkyDisplay == 3)
        {
            textObject0.text = GetCarrotsPicked() + "\n" + GetMoneyEarned() + "\n" + GetPlucksLeft();
        }
        else if (UISkyDisplay == 0)
        {
            textObject0.text = "";
            //GameObject.FindGameObjectWithTag("UISky").SetActive(false);
        }

        textObject3.text = textObject0.text;
        textObject2.text = textObject0.text;
        textObject1.text = textObject0.text;


        //textObjectCat.text = GetCarrotsPicked();
        //textObjectPlu.text = "\n" + GetPlucksLeft();


    }

    //-- This function is called in the "ReadCSVFromScratch.cs" --//
    //-- It sets the visiability of the carrots collected and plucks left text in the sky. --//
    public void UISkyVis()
    {
        //Debug.Log("CarrotCountVis = " + ReadCSVFromScratch.carrotCountVisibility + "\nPlucksLeftVis = " + ReadCSVFromScratch.plucksLeftVisibility);

        if (ReadCSVFromScratch.carrotCountVisibility != 1 && ReadCSVFromScratch.plucksLeftVisibility != 1)
        {
            textObject0.text = "";
            //-- Hides the gold background of the text when there is no text. --//
            GameObject.FindGameObjectWithTag("UISky").SetActive(false);
            UISkyDisplay = 0;
        }
        else if (ReadCSVFromScratch.carrotCountVisibility == 1 && ReadCSVFromScratch.plucksLeftVisibility != 1)
        {
            //GameObject.FindGameObjectWithTag("UISky").SetActive(true);
            textObject0.text = GetCarrotsPicked() + "\n" + GetMoneyEarned();
            UISkyDisplay = 1;
        }
        else if (ReadCSVFromScratch.carrotCountVisibility != 1 && ReadCSVFromScratch.plucksLeftVisibility == 1)
        {
            //GameObject.FindGameObjectWithTag("UISky").SetActive(true);
            textObject0.text = GetPlucksLeft();
            UISkyDisplay = 2;
        }
        else if (ReadCSVFromScratch.carrotCountVisibility == 1 && ReadCSVFromScratch.plucksLeftVisibility == 1)
        {
            //GameObject.FindGameObjectWithTag("UISky").SetActive(true);
            textObject0.text = GetCarrotsPicked() + "\n" + GetMoneyEarned() + "\n" + GetPlucksLeft();
            UISkyDisplay = 3;
        }

        textObject3.text = textObject0.text;
        textObject2.text = textObject0.text;
        textObject1.text = textObject0.text;

    }

    //-- Returns the current amount of carrots collected. --//
    string GetCarrotsPicked()
    {
        carrotsFoundText = "Carrots Found: " + StaticVariables.carrotsCollected;

        return carrotsFoundText;
    }

    //-- Returns the current amount of plucks remaining. --//
    string GetPlucksLeft()
    {
        plucksRemainingText = "Plucks Remaining: " + (StaticVariables.totalStems - StaticVariables.stemsCollected);

        return plucksRemainingText;
    }

    //-- Returns the money earned as a function of carrots found. --//
    string GetMoneyEarned()
    {
        moneyEarnedText = "Money Earned: $" + (StaticVariables.carrotsCollected * 0.25);

        return moneyEarnedText;
    }

}
