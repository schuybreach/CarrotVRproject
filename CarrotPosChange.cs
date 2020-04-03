using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarrotPosChange : MonoBehaviour {

    private OVRGrabbable grabbableSource;

    private bool grabbed;

    public GameObject grablink;

    public GameObject carrot;
    public GameObject dirtMound;

    public AudioClip soundClip1;

    Vector3 newCarrotPos;
    Vector3 oldCarrotPos;

    bool pickedCarrot = true;

    private AudioSource source;


    // Use this for initialization
    void Start () {

        //-- Gets the original starting position of the stem --//
        newCarrotPos = carrot.transform.position;
        oldCarrotPos = newCarrotPos;
        source = FindObjectOfType<AudioSource>();

        grabbableSource = grablink.GetComponent<OVRGrabbable>();

    }
	
	// Update is called once per frame
	void Update () {

        grabbed = grabbableSource.isGrabbed;

        //-- If the position of the stem changes, spawn a dirt mound and add to the stem counter --//
        if (newCarrotPos != carrot.transform.position && pickedCarrot)
        {
            
            CLData();

            newCarrotPos = carrot.transform.position;
            pickedCarrot = false;
            source.volume = 0.35f;
            source.PlayOneShot(soundClip1);
            Instantiate(dirtMound, new Vector3(oldCarrotPos.x, 0.065f, oldCarrotPos.z), Quaternion.identity);

            StaticVariables.carrotsCollected += 1;
            StaticVariables.stemsCollected += 1;
            Debug.Log("Stems collected: " + StaticVariables.stemsCollected + " / " + StaticVariables.totalStems + " allowed.");

        }

        //-- If the stem is released teleport it very far away --//
        if (!grabbed && !pickedCarrot)
        {
            OVRGrabber[] grabbers = FindObjectsOfType<OVRGrabber>();
            foreach (OVRGrabber grabber in grabbers)
            {
                grabber.ForceRelease(grabbableSource);
            }
            //carrot.SetActive(false);


            //found a fucking workaround by placing object to outer space
            carrot.transform.position = new Vector3(100, 5, 100);
        }


    }

    //-- Send the "picked" stem position data to the output CSV file --//
    public void CLData()
    {
        int carrotPosX = Mathf.FloorToInt(carrot.transform.position.x);
        int carrotPosZ = Mathf.FloorToInt(carrot.transform.position.z);

        string appendText = ReadCSVFromScratch.currentTimeString + "," + "Carrot at" + "," + (((carrotPosX + 1)/2)+1) + ","
            + (((carrotPosZ + 1)/2)+1) + "," + "has been picked" + Environment.NewLine;

        File.AppendAllText(ReadCSVFromScratch.newPath, appendText);

        Debug.Log("Carrot at: " + (((carrotPosX + 1)/2)+1) + "," + (((carrotPosZ + 1)/2)+1) + " has been picked");

    }

    //-- This was causing glitches --//
    //IEnumerator carrotGone()
    //{
    //    // timer for carrots!
    //    yield return new WaitForSeconds(1);
    //    carrot.SetActive(false);

    //    if (!grabbed)
    //    {
    //        carrot.SetActive(false);
    //    }

    //}
}
