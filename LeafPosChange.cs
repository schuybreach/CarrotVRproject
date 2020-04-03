using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeafPosChange : MonoBehaviour {

    private OVRGrabbable grabbableSource;

    public GameObject leaf;
    public GameObject dirtMound;

    public GameObject grablink;

    private bool grabbed;

    Vector3 newLeafPos;
    Vector3 oldLeafPos;

    bool pickedLeaf = true;


    // Use this for initialization
    void Start () {

        //-- Gets the original starting position of the stem --//
        newLeafPos = leaf.transform.position;
        oldLeafPos = newLeafPos;

        grabbableSource = grablink.GetComponent<OVRGrabbable>();

    }
	
	// Update is called once per frame
	void Update () {

        //timer += Time.deltaTime;

        grabbed = grabbableSource.isGrabbed;

        //-- If the position of the stem changes, spawn a dirt mound and add to the stem counter --//
        if (newLeafPos != leaf.transform.position && pickedLeaf)
        {
            
            LData();

            newLeafPos = leaf.transform.position;
            pickedLeaf = false;

            Instantiate(dirtMound, new Vector3(oldLeafPos.x, 0.065f, oldLeafPos.z), Quaternion.identity);

            StaticVariables.stemsCollected += 1;
            Debug.Log("Stems collected: " + StaticVariables.stemsCollected + " / " + StaticVariables.totalStems + " allowed.");

        }

        //-- If the stem is released teleport it very far away --//
        if (!grabbed && !pickedLeaf)
        {
            OVRGrabber[] grabbers = FindObjectsOfType<OVRGrabber>();
            foreach (OVRGrabber grabber in grabbers)
            {
                grabber.ForceRelease(grabbableSource);
            }
            //leaf.SetActive(false);
            //Debug.Log("Grabbable: " + grabbableSource + " isGrabbed? " + grabbed);

            //found a fucking workaround by placing object to outer space
            leaf.transform.position = new Vector3(100, 5, 100);
        }

    }

    //-- Send the "picked" stem position data to the output CSV file --//
    public void LData()
    {
        int leafPosX = Mathf.FloorToInt(leaf.transform.position.x);
        int leafPosZ = Mathf.FloorToInt(leaf.transform.position.z);

        string appendText = ReadCSVFromScratch.currentTimeString + "," + "Leaf at" + "," + (((leafPosX + 1) / 2) + 1) + ","
            + (((leafPosZ + 1) / 2) + 1) + "," + "has been picked" + Environment.NewLine;

        File.AppendAllText(ReadCSVFromScratch.newPath, appendText);

        Debug.Log("Leaf at: " + (((leafPosX + 1) / 2) + 1) + "," + (((leafPosZ + 1) / 2) + 1) + " has been picked");

    }

    //-- This was causing glitches --//
    //IEnumerator leafGone()
    //{
    //    yield return new WaitForSeconds(1);
    //    leaf.SetActive(false);

    //    if (!grabbed)
    //    {
    //        leaf.SetActive(false);
    //    }

    //}

}
