using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarrotPosChangeTutorial : MonoBehaviour {

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
            newCarrotPos = carrot.transform.position;
            pickedCarrot = false;
            source.PlayOneShot(soundClip1);
            Instantiate(dirtMound, new Vector3(oldCarrotPos.x, 0.065f, oldCarrotPos.z), Quaternion.identity);

            LeafPosChangeTutorial.allPicked++;
            Debug.Log("allPicked = " + LeafPosChangeTutorial.allPicked);

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
            carrot.transform.position = new Vector3(100, 0, 100);
        }


    }

}
