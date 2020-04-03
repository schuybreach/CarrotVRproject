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

public class Directions : MonoBehaviour {

    //-- Directions
    /*
     * Hello! Press the 'A' or 'X'
     * buttons to continue. Press the 
     * 'B' or 'Y' buttons to go back.
     * 
     * Use the joystick on your left 
     * hand to move. Use the joystick 
     * on your right to move.
     * 
     * Some of the stems have carrots
     * attached and some do not. Pull 
     * the stems upwards to find out!
     * 
     * Press and hold the middle finger
     * button on either hand to grab 
     * the stems. They will disappear 
     * when you release them.
     * 
     * Note: Only the hands will be 
     * visible in the main experiment, 
     * the controllers will not be.
     * 
     * When you are ready to begin the 
     * main experiment, hold the 'A' and 
     * 'X' buttons for 3 seconds to proceed.
     * 
     * sub:
     * Press 'A' or 'X' to go forward.
     * Press 'B' or 'Y' to go back.
     * 
     */

    string dir1;
    string dir2;
    string dir3;
    string dir4;
    string dir5;
    string dir6;
    string subDir;
    //string subDir1;
    //string subDir2;


    public TextMesh textObject;
    public TextMesh subTextObject;

    //-- "NonSerialized" hides the following from the inspector --//
    [NonSerialized]
    public float duration = 3;

    private float timer = 0;
    private bool waitingButtonUp = false;

    int textCounter = 0;


    // Use this for initialization
    void Start () {

        dir1 = "Hello! Press the 'A' or 'X' \nbuttons to continue. Press the \n'B' or 'Y' buttons to go back.";
        dir2 = "Use the joystick on your left \nhand to move. Use the joystick \non your right to turn.";
        dir3 = "Some of the stems have carrots \nattached and some do not. Pull \nthe stems upwards to find out!";
        dir4 = "Press and hold the middle finger \nbutton on either hand to grab \nthe stems. They will disappear \n when you release them.";
        dir5 = "Note: Only the hands will be \nvisible in the main experiment, \nthe controllers will not.";
        dir6 = "When you are ready to begin the \nmain experiment, hold the 'A' and \n'X' buttons for 3 seconds to proceed.";

        subDir = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";

        subTextObject.text = "";


        //subDir1 = "Press 'A' or 'X' to go forward.";
        //subDir2 = "Press 'B' or 'Y' to go back.";
        //subDir2 = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n\t\t\t\t\t\t\t\t\t\t(1 / 5)";


    }
	
	// Update is called once per frame
	void Update () {

        //-- The following two if "trees" allow the subject / player to scroll through the directions --//
        //-- A and X buttons (to go forward) --//
        if (Input.GetKeyDown(KeyCode.T) || OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            //-- Changes the direction "slide" --//
            if (textCounter < 6)
            {
                textCounter++;
            }

            if (textCounter == 1)
            {
                textObject.text = dir2;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 2)
            {
                textObject.text = dir3;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 3)
            {
                textObject.text = dir4;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 4)
            {
                textObject.text = dir5;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 5)
            {
                textObject.text = dir6;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            
        }

        //-- B and Y buttons (to go back) --//
        if (Input.GetKeyDown(KeyCode.G) || OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four))
        {
            if (textCounter > 0)
            {
                textCounter--;
            }

            if (textCounter == 0)
            {
                textObject.text = dir1;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 1)
            {
                textObject.text = dir2;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter+1) + " / 6)";
            }

            else if (textCounter == 2)
            {
                textObject.text = dir3;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 3)
            {
                textObject.text = dir4;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 4)
            {
                textObject.text = dir5;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

            else if (textCounter == 5)
            {
                textObject.text = dir6;
                subTextObject.text = "Press 'B' or 'Y' to go back. \t\t\t\t\t\t\t\tPress 'A' or 'X' to go forward.\n(" + (textCounter + 1) + " / 6)";
            }

        }

        //-- If the following parameters are true, the subject / player is transported to the main experiment --//
        if (OVRInput.Get(OVRInput.Button.One) && OVRInput.Get(OVRInput.Button.Three) && !waitingButtonUp && LeafPosChangeTutorial.allPicked >= 4)
        {
            timer += Time.deltaTime;

            //-- Visual countdown timer the subject / player can see --//
            if (timer <= 1 && timer > 0.25f)
                subTextObject.text = "--1--";
            else if (timer <= 2 && timer > 1)
                subTextObject.text = "--2--";
            else if (timer <=3 && timer > 2)
                subTextObject.text = "--3--";

        }
        else // Do not forget to reset timer when the button is not pressed anymore
        {
            timer = 0;
            waitingButtonUp = false;
        }

        if (timer > duration)
        {
            PlayGame();
            waitingButtonUp = true;
            timer = 0;
        }

    }

    //-- Switches scenes and begins the test --//
    public void PlayGame()
    {
        SceneManager.LoadScene("CubeScene");
    }

}
