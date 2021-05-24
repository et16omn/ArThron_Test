
using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Clicker : MonoBehaviour
{
    bool isSessionQualityOK;
    public Slider slider;
    public RawImage grab;
    public RawImage release;
    public GameObject planetOne;
    public GameObject planetTwo;
    public Text debug;
    public Text debug2;
    public Timer aTimer;
    private float count = 0;
    int counter = 0;
    
    private bool start = true;
    private bool end = false;
    private bool inTime = true;


    private bool firstSeq = false;
    private bool releaseGest = false;
    private bool grabGest = false;
    private float timer = 0f;
    private float secondTimer = 0f;
    private float relCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        debug.text = "Öppna handen...";
        grab.enabled = false;
        release.enabled = true;
        slider.value = 0f;
        ARSession.stateChanged += HandleStateChanged;
    }
    // Specify what you want to happen when the Elapsed event is raised.
  
    private void HandleStateChanged(ARSessionStateChangedEventArgs stateChangedEventArgs)
    {
        isSessionQualityOK = stateChangedEventArgs.state == ARSessionState.SessionTracking;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (firstSeq)
        {
            SpawnQube();

            if (start && (count + 0.2f < Time.time))
            {
                checkDone();
            }
        }
        else
        {
            releaseGesture();
        }

    }

    private void releaseGesture()
    {
        HandInfo handinfo = ManomotionManager.Instance.Hand_infos[0].hand_info;

        GestureInfo gestureInfo = handinfo.gesture_info;

        ManoGestureTrigger manoGestureTrigger = gestureInfo.mano_gesture_trigger;

        if (manoGestureTrigger == ManoGestureTrigger.RELEASE_GESTURE && releaseGest == false)
        {
            releaseGest = true;
            timer = Time.time;
            release.enabled = false;
            grab.enabled = true;
            debug.text = "..och sedan stäng...";
        }

        if (manoGestureTrigger == ManoGestureTrigger.GRAB_GESTURE && releaseGest==true && ((timer + 0.4f) < Time.time))
        {
            grabGest = true;
            secondTimer = Time.time;
            grab.enabled = false;
            release.enabled = true;
            debug.text = "..och sedan öppna.";
        }

        if (manoGestureTrigger == ManoGestureTrigger.RELEASE_GESTURE && grabGest == true
            && ((secondTimer + 0.4f) < Time.time))
        {
            firstSeq = true;
            release.enabled = false;
            debug.text = "klar";
            debug.color = Color.green;
        }

        
    }

    private void checkDone()
    {
        HandInfo handinfo = ManomotionManager.Instance.Hand_infos[0].hand_info;

        GestureInfo gestureInfo = handinfo.gesture_info;

        ManoGestureTrigger manoGestureTrigger = gestureInfo.mano_gesture_trigger;

        if (manoGestureTrigger == ManoGestureTrigger.RELEASE_GESTURE)
        {
            counter++;
            debug2.text = "Repetioner: "+ counter;
            relCounter = 0;
            start = false;
            relCounter = Time.time;
            debug.text = "GRAB";
            debug.color = Color.red;
            slider.value = slider.value+1f;
      
            
        }
    }


    private void SpawnQube()
    {
        
        HandInfo handinfo = ManomotionManager.Instance.Hand_infos[0].hand_info;
        
        GestureInfo gestureInfo = handinfo.gesture_info;

        ManoGestureTrigger manoGestureTrigger = gestureInfo.mano_gesture_trigger;

        if (manoGestureTrigger == ManoGestureTrigger.GRAB_GESTURE && (relCounter+0.2f<Time.time))
         {

            debug.text = "REALEASE";
            debug.color = Color.green;
            start = true;
            inTime = true;
            count = Time.time;
         
        }
    }
}