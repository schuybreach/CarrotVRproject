using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO; 
using System;
using TMPro;
using UnityEngine.SceneManagement;

//******************************//
//                              //
//  "//--" == Schuyler's Notes  //
//                              //
//******************************//


[Serializable]
public class StaticVariables
{
    public static int carrotsCollected = 0;
    public static int stemsCollected = 0;
    public static int totalStems = 200;
    
}


public class ReadCSVFromScratch : MonoBehaviour {

    //****************** Allows user to specify their own input file in the Unity Inspector ******************//
    //-- **May need to hard code the file in here for exported version of program --//
    //-- To understand some of the Unity functions below, look to Unity documentation online for help --//
    //-- https://docs.unity3d.com/Manual/index.html --// 

    //-- Input CSV File --//
    public TextAsset csvFile;

    private List<string[]> rowData = new List<string[]>();


    //-- Grabbable Carrot Object / prefab --//
    public GameObject carrot;

    //-- Grabbable Leaf Object / prefab --//
    public GameObject leaf;

    //-- Player / Subject Variables --//
    public OVRPlayerController subject;
    public GameObject Subject_Container;
    //public GameObject TestTube;

    private GameObject SubjectPosition; // GameObject.FindGameObjectWithTag("Player");
    private Vector3 SubjectPositionV = new Vector3(0, 0, 0);
    private int SubjectPositionVx = 999;
    private int SubjectPositionVz = 999;
    public GameObject TextUISky;

    //-- Date and other CSV parameter variables --//
    public static string newPath = "";
    string newLine;
    string delimiter = ",";

    string _day = "";
    string _month = "";
    string _year = "";
    string _date = "";
    string _seconds = "";
    string _minutes = "";
    string _hours = "";
    string _time = "";


    //****************** Under the hood variables ******************//

    //-- Counting variable to determine which line of the CSV is being read --//
    int N = 0;

    //-- The "X" and "Z" grid size count (default is 20x20 grid size) --//
    int GridX = 0;
    int GridZ = 0;

    string[] lines;
    string[] parts;
    string[] rowDataTemp = new string[2];

    string playerStartPos;
    int x;
    int z;
    int x2;
    int z2;
    int xs;
    int zs;

    public GameObject _compass;
    public Canvas _canvas;
    public Camera camUI;

    //-- Radius of object detection, see below for more explanation --//
    float sphereRadius = 0.25f;

    //-- Variables for the UI - amounts of carrots, carrots collected, etc. --//
    int Count = 0;
    //public static int carrotsCollected = 0;
    //public static int stemsCollected = 0;
    //public static int totalStems = 200;
    public Text CountText;

    //private TextMeshProUGUI resourceCount;
    //private int resourcesCollected = 0;
    //private int resourcesLeft = 0;

          
    //** Timer variables
    public GameObject countdown;
    TextMeshProUGUI ourComponent;
    private float currentTime = 0f;
    private float startingTime = 0f;     // 20 min * 60 sec/min
    public static string currentTimeString;
    private float waitTimer = 0;
    private float waitTime = 1.0f;

    private int trailVisibility = 0;
    private int timerVisibility = 0;
    private int compassVisibility = 0;
    //private int resourcesCollectedVisibility = 0;
    //private int resourcesLeftVisibility = 0;
    public static int carrotCountVisibility = 0;
    public static int plucksLeftVisibility = 0;

    //****************** Runs when program starts ******************//
    public void Awake()
    {
        
    }
    
    // Use this for initialization
    public void Start ()
    {

        Subject_Container.SetActive(false); //-- So there is not two cameras in the same scene
        
        //-- Reads the CSV --//
        ReadCSV(); 
        SubjectPosition = GameObject.FindGameObjectWithTag("Player");
        SubjectPositionV = SubjectPosition.transform.position;
        Debug.Log(SubjectPosition + " is at " + SubjectPositionV);

        //-- For the UIinTheSky script - sets variables for visiability --//
        TextUISky.GetComponent<UIinTheSky>().UISkyVis();

        //-- Creates the output CSV file --//
        save();
        currentTime = startingTime;

        countdown = GetComponent<GameObject>();

        OutputStaticData();
        OutputData();
    }

    //-- Runs at start --//
    public void ReadCSV()
    {
        //-- Confirmation / failsafe that the leaves will spawn - may not be needed --//
        //leaf.SetActive(true);

        //-- Separates the CSV file into lines - based on the new line character --//
        lines = csvFile.text.Split("\n"[0]);

        //-- For the amount of lines (~30ish) in the CSV, separate the data in each column - based on the comma --// 
        for (var i = 0; i < lines.Length; i++)
        {

            parts = lines[i].Split(","[0]);
            //Debug.Log(parts[0]);
            //Debug.Log(parts[1]);

            //-- The first line of the CSV should be the grid size (ex. 20x20) --//
            //-- parts[0] = column 1, parts[1] = column 2 --//
            if (i == 0)
            {
                //-- "int.Parse" converts the string values from the CSV into integers --//
                GridX = int.Parse(parts[0]);
                GridZ = int.Parse(parts[1]);
            }

            //-- The second line of the CSV should be the amount of hidden carrots there are (not leaves) --//
            else if (i == 1)
            {
                N = int.Parse(parts[0]);
                Count = N;
            }

            //-- The next "N" lines of the CSV are the "X" and (technically) "Z" coordinates of the hidden carrots --//
            else if (i >= 2 && i <= N)
            {
                x = int.Parse(parts[0]);
                z = int.Parse(parts[1]);

                //-- It was requested that the grid start at (1,1), however Unity likes to start things at (0,0).
                //-- The statements below help alleviate that, somehow, even though you would think it should be 
                //-- "x + 1" instead of "x - 1". But it works, so don't touch it. --//  
                //x2 = x - 1;
                //z2 = z - 1;
                //int xa = x * 2;
                //int za = z * 2;
                
                //Debug.Log("Carrot #" + x + ", " + z);
                //Debug.Log("Carrot 2#" + x2 + ", " + z2);
                //Instantiate(carrot, new Vector3(x-1, 0.1f, z-1), Quaternion.identity); //-- 1 to 1 scale

                //-- Spawn the carrot at the given location on the grid --//
                Instantiate(carrot, new Vector3((x - 1) * 2, -0.297f, (z - 1) * 2), Quaternion.identity); //-- 2 to 1 scale // y = 0.07f for low poly carrots
                
                OutputArray();
            }

            //****************** For all the variable functions they want added ******************//
            //****************** Leaf Spawn & Trail Visibility ******************//
            else if (i == N + 1)
            {
                //-----------------------------------------------------------//
                //-- The below fixes the missing last carrot issue --//
                //-- The following lines are the same as the above --//
                x = int.Parse(parts[0]);
                z = int.Parse(parts[1]);
                //Debug.Log("LCarrot #" + x + ", " + z);
                Instantiate(carrot, new Vector3((x - 1) * 2, -0.297f, (z - 1) * 2), Quaternion.identity); //-- 2 to 1 scale // y = 0.07f for low poly carrots

                OutputArray();
                //-----------------------------------------------------------//

                //-- For x- and z- positions on the grid, the following places the carrots leafs on the grid --//
                //Debug.Log("N = " + N);
                for (var j = 0; j < GridX; j++)
                {
                    for (var k = 0; k < GridZ; k++)
                    {
                        /*--"Physics.CheckSphere" checks the position given whether or not there is physics collider
                            (I think?), at that location. (Ex. Is there a carrot already at that position?) If there
                            is not something already there, a carrot leaf will spawn, otherwise it will not. The 
                            "sphereRadius" determines how big of an area to check. The center / origin of the sphere
                            is at the given location (ex. (1,1) ) and the sphere expands outwards from there. --*/

                        if (Physics.CheckSphere(new Vector3(j * 2, 1, k * 2), sphereRadius))
                        {
                            
                        }
                        else
                        {
                            //Debug.Log("Leaf #" + j + ", " + k);
                            //Instantiate(leaf, new Vector3(j, 0.1f, k), Quaternion.identity);
                            Instantiate(leaf, new Vector3(j * 2, 0.293f, k * 2), Quaternion.identity); //-- y = 0.459f for low poly leaves

                        }
                    }
                }
            }

            //****************** Trail Visibility ******************//
            else if (i == N + 2) 
            {
                if (parts[0] == "1")
                {
                    trailVisibility = 1;
                }
            }

            //****************** Compass Visibility ******************//
            else if (i == N + 3) 
            {
                if (parts[0] == "1")
                {
                    _compass.SetActive(true);
                    compassVisibility = 1;
                }
                else
                {
                    _compass.SetActive(false);
                }
            }

            //****************** Timer Visibility ******************//
            else if (i == N + 4) 
            {
                if(parts[0] == "1")
                {
                    timerVisibility = 1;                    
                }
            }

            //****************** Carrot Count Visibility (Formally "Resource Count (Total?) Visibility") ******************//
            else if (i == N + 5) 
            {
                if (parts[0] == "1")
                {
                    //resourcesCollectedVisibility = 1;
                    carrotCountVisibility = 1;
                }
            }

            //****************** Plucks Left Visibility (Formally "Resource Count Left Visibility") ******************//
            else if (i == N + 6) 
            {
                if (parts[0] == "1")
                {
                    //resourcesLeftVisibility = 1;
                    plucksLeftVisibility = 1;
                }
            }

            //****************** Starting Point - Works! ******************//
            else if (i == N + 7)
            {
                xs = int.Parse(parts[0]);
                zs = int.Parse(parts[1]);
            }

            //****************** Starting Point and Orientation - Works! *****************//
            else if (i == N + 8) 
            {
                int orin = int.Parse(parts[0]);
                Debug.Log("orin: " + orin);

                if (orin == 1) // North
                {
                    Instantiate(subject, new Vector3((xs - 1.1f) * 2, 1f, (zs - 1) * 2), Quaternion.Euler(0, 90, 0), transform.parent);
                }
                else if (orin == 2) // East
                {
                    Instantiate(subject, new Vector3((xs - 1.1f) * 2, 1f, (zs - 1) * 2), Quaternion.Euler(0, 180, 0), transform.parent);
                }                                                   
                else if (orin == 3) // South
                {
                    Instantiate(subject, new Vector3((xs - 1.1f) * 2, 1f, (zs - 1) * 2), Quaternion.Euler(0, -90, 0), transform.parent);
                }
                else if (orin == 4) // West
                {
                    Instantiate(subject, new Vector3((xs - 1.1f) * 2, 1f, (zs - 1) * 2), Quaternion.Euler(0, 0, 0), transform.parent);
                }

                playerStartPos = xs.ToString() + ", " + zs.ToString();
                OutputArray();

                _canvas.worldCamera = camUI;
                Debug.Log("Canvas = " + _canvas);
                Debug.Log("Camera = " + camUI);

            }
        }
    }

    //-- Runs every frame --//
    public void Update()
    {
        int OLDSPVx = SubjectPositionVx;
        int OLDSPVz = SubjectPositionVz;
        GetSubjectPosition();

        //String headPos;
        
        //-- Checks if the subject / player has moved --//
        if (OLDSPVx != SubjectPositionVx || OLDSPVz != SubjectPositionVz)
        {
            Debug.Log("Subject Position Changed to: " + (SubjectPositionVx + 1) + ", " + (SubjectPositionVz + 1));
            OutputData();
			
            //-- ************************************************************* --// 
            //headPos = OVRManager.tracker.GetPose(0).orientation.ToString();
            //Debug.Log("HeadPos: " + headPos); //-- *Does not work!
            //-- ************************************************************* --// 


            //Debug.Log("Carrot Count: " + carrotsCollected);
        }
        
        //Resource Count Test
        //resourceCount.text = resourcesCollected.ToString();
        //resourcesCollected++;

        //-- Not Implemented --//
        if (trailVisibility == 1)
        {
            // show trail
        }

        //-- Not needed here, implemented above --//
        if (compassVisibility == 1)
        {
            // show compass
        }

        //------------ Everything below is for the UI and has not been implemented ------------//
        //-- The timer is hidden behind the mountain --//
        if (timerVisibility == 1)
        {
            currentTime += 1 * Time.deltaTime;
            int currentTimeDisplay = Convert.ToInt32(Math.Ceiling(currentTime));
            currentTimeString = currentTimeDisplay.ToString();

            countdown = GameObject.Find("countdown");
            ourComponent = countdown.GetComponent<TextMeshProUGUI>();
            ourComponent.text = currentTimeString;

        }

        //if (resourcesCollectedVisibility == 1)
        //{
        //    // show collected resources
        //}

        //if (resourcesLeftVisibility == 1)
        //{
        //    // show remaining resources
        //}

        //------------------------------------------------------------------------------------//

        EndGame();

        ////-- Counts the total number of stems the subject / player has collected --//
        //if (StaticVariables.stemsCollected >= StaticVariables.totalStems)
        //{
        //    Debug.Log("Stems collected: " + StaticVariables.stemsCollected + " / " + StaticVariables.totalStems + " allowed.");

        //    waitTimer += Time.deltaTime;
            
        //    //-- If the total amount of stems has been collected, the subject / player is transported to the end screen --//
        //    if (waitTimer > waitTime)
        //    {
        //        SceneManager.LoadScene("EndTest");

        //    }
        //}

    }

    //-- Gives the subject's / player's position at a given time --//
    void GetSubjectPosition()
    {
        SubjectPositionV = SubjectPosition.transform.position;

        //-- Converts the numbers from floats to ints --//
        SubjectPositionVx = Mathf.FloorToInt(SubjectPosition.transform.position.x);
        SubjectPositionVz = Mathf.FloorToInt(SubjectPosition.transform.position.z);

        SubjectPositionVx = (SubjectPositionVx +1) /2;
        SubjectPositionVz = (SubjectPositionVz +1) /2;
    }

    /*-- This function is called a bunch and it's from the beginning of the project
     *   and I don't know why it's needed, but maybe just don't delete it 
     *   --*/
    void OutputArray()
    {
        rowDataTemp = new string[2];
        rowDataTemp[0] = parts[0]; 
        rowDataTemp[1] = parts[1]; 

        rowData.Add(rowDataTemp);
    }

    //-- Export Data to CSV --//
    public string save()
    {

        //-- --------------------[ new test ]-------------------- --//
        //-- Converts time and date to individual pieces so that the data can be manipulated in the output filename --//
        _day = DateTime.Now.Day.ToString();
        _month = DateTime.Now.Month.ToString();
        _year = DateTime.Now.Year.ToString();
        _date = _month + "-" + _day + "-" + _year;
        _seconds = DateTime.Now.Second.ToString().PadLeft(2, '0');
        _minutes = DateTime.Now.Minute.ToString().PadLeft(2,'0');
        _hours = DateTime.Now.Hour.ToString().PadLeft(2, '0');
        _time = _hours + "-" + _minutes + "-" + _seconds;

        //-- Filename - "/" indicate a new directory / folder in the file path - change as needed --//
        newPath = Application.dataPath + "/DataOutput/" + "_SavedData_" + _date + "_" + _time + "_NewTest.csv"; 
        Debug.Log("File newPath: " + newPath);

        //-- If a file doesn't exist, create a new one --//
        /*-- Note: since the filename includes the time in seconds, there should never be
         *   a file with the same name, but just in case.
         *   --*/
        if (!File.Exists(newPath))
        {
            string createText = _date + "," + _hours + ":" + _minutes + ":" + _seconds + Environment.NewLine;
            File.WriteAllText(newPath, createText);
        }

        return (newPath);
    }

    //-- Outputs the data from the input CSV to the output file before recording the test data --//
    void OutputStaticData()
    {
        delimiter = ",";
        
        string appendText = "------------ Input Variables ------------" + Environment.NewLine;
        newLine = Environment.NewLine;
        File.AppendAllText(newPath, newLine);
        File.AppendAllText(newPath, appendText);
        File.AppendAllText(newPath, Count.ToString());
        File.AppendAllText(newPath, newLine);
        //File.AppendAllText(newPath, playerStartPos);
        //File.AppendAllText(newPath, newLine);

        string[][] output2 = new string[rowData.Count][];

        for (int i = 0; i < output2.Length; i++)
        {
            output2[i] = rowData[i];
        }

        int length2 = output2.GetLength(0);

        for (int index2 = 0; index2 < length2 - 1; index2++)
        {
            string sb2 = string.Join(delimiter, output2[index2]);
            Debug.Log("sb2 = " + sb2);
            File.AppendAllText(newPath, sb2);
        }

        File.AppendAllText(newPath, newLine);
        appendText = "------------ Data Collection ------------" + Environment.NewLine;
        File.AppendAllText(newPath, appendText);
        appendText = "---- Ignore the first line (for now) ----" + Environment.NewLine;
        File.AppendAllText(newPath, appendText);

    }

    //-- Outputs the subject's / player's position to the CSV when they move --//
    void OutputData()
    {
        string appendText = currentTimeString + "," + "Position: " + "," + (SubjectPositionVx + 1) + "," + (SubjectPositionVz + 1) + Environment.NewLine; 
        File.AppendAllText(newPath, appendText);
    }

    //****************** On Screen UI | Text ******************//
    /**
    void SetCountText()
    {
        CountText.text = "Carrots Collected: " + carrotsCollected.ToString() + " / " + Count.ToString();
    }

    */

    void EndGame()
    {
        //-- Counts the total number of stems the subject / player has collected --//
        if (StaticVariables.stemsCollected >= StaticVariables.totalStems)
        {
            Debug.Log("Stems collected: " + StaticVariables.stemsCollected + " / " + StaticVariables.totalStems + " allowed.");

            waitTimer += Time.deltaTime;

            //-- If the total amount of stems has been collected, the subject / player is transported to the end screen --//
            if (waitTimer > waitTime)
            {
                SceneManager.LoadScene("EndTest");

            }
        }
    }
}
