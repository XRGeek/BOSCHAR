﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

using UnityEngine.XR.ARSubsystems;
using System;

public class TapToPlaceObject : MonoBehaviour
{

    public GameObject[] objectToplace;

    public bool clicked;

    public GameObject Nav1;
    public GameObject Nav2;


    public GameObject placementIndicator;
    //private ARSessionOrigin arOrigin;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    public CharacterSlider CS;
    public int index;
    public NavMeshSurface surface;


    public Material debugMat;
    public bool Mattoggle;

    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();

        
    }

    void Update()
    {

        


        index = CS.CINDEX;

        UpdatePlacementPose();
        UpdatePlacementIndicator();



        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {

           //PlaceObject();
        }
    }


    public void PlaceObject() {

        surface.BuildNavMesh();
        Instantiate(objectToplace[index], PlacementPose.position, PlacementPose.rotation);

    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;

            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }


    public void toggleMat() {

        Mattoggle =! Mattoggle;


        if (Mattoggle == true)
        {
            Nav2.SetActive(false);
            Nav1.SetActive(true);
            debugMat.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
        }

        if (Mattoggle == false)
        {
            Nav1.SetActive(false);
            Nav2.SetActive(true);
            debugMat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }



    }



}