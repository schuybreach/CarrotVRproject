using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeafPosChangeTutorial : MonoBehaviour {

    private OVRGrabbable grabbableSource;

    public GameObject leaf;
    public GameObject dirtMound;

    public GameObject grablink;

    private bool grabbed;

    Vector3 newLeafPos;
    Vector3 oldLeafPos;

    bool pickedLeaf = true;

    [NonSerialized]
    public static int allPicked = 0;


    // Use this for initialization
    void Start () {

        //-- Gets the original starting position of the stem --//
        newLeafPos = leaf.transform.position;
        oldLeafPos = newLeafPos;

        grabbableSource = grablink.GetComponent<OVRGrabbable>();

    }
	
	// Update is called once per frame
	void Update () {

        grabbed = grabbableSource.isGrabbed;

        //-- If the position of the stem changes, spawn a dirt mound and add to the stem counter --//
        if (newLeafPos != leaf.transform.position && pickedLeaf)
        {
            newLeafPos = leaf.transform.position;
            pickedLeaf = false;

            Instantiate(dirtMound, new Vector3(oldLeafPos.x, 0.065f, oldLeafPos.z), Quaternion.identity);

            allPicked++;
            Debug.Log("allPicked = " + allPicked);

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
            leaf.transform.position = new Vector3(100, 0, 100);
        }

    }

}
