using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class GestureDetector : MonoBehaviour {
	
	Controller controller;
	const int oldGesturesMaxCount = 10;
	List<int> oldGestures = new List<int>(oldGesturesMaxCount);
	
	const float directionDetectionThreshold = 0.71f;
	const long durationDetectionThreshold = 23000;
	
	// Use this for initialization
	void Start () {
		
		controller = new Controller ();
		controller.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
	}
	
	void AddToOldGestures(int id) {
		if (oldGestures.Count >= oldGesturesMaxCount) {
			oldGestures.RemoveAt (0);
		}
		
		oldGestures.Add (id);
	}
	
	bool IsIdInOldGesturesOrAdd(int id) {
		
		if (oldGestures.Contains(id)) {
			return true;
		} else {
			
			AddToOldGestures(id);
			
			return false;
			
		}
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		Frame frame = controller.Frame();
		
		for(int g = 0; g < frame.Gestures().Count; g++) {
			Gesture gesture = frame.Gestures()[g];
			switch (gesture.Type) {
			case Gesture.GestureType.TYPE_CIRCLE:
				CircleGesture circleGesture = new CircleGesture(gesture);
				
				if (circleGesture.Progress >= 1.0f) {
					
					if (!IsIdInOldGesturesOrAdd(circleGesture.Id)) {
						//Debug.Log ("CIRCLE: " + circleGesture.Id.ToString());
						
					}
					
				}
				
				//Handle circle gestures
				break;
				
			case Gesture.GestureType.TYPE_SWIPE:
				SwipeGesture swipeGesture = new SwipeGesture(gesture);
				
				if (gesture.State == Gesture.GestureState.STATE_UPDATE && swipeGesture.Duration > durationDetectionThreshold) {
					
					if (!IsIdInOldGesturesOrAdd(swipeGesture.Id)) {
						
						Vector direction = swipeGesture.Direction;

						if (direction.y > directionDetectionThreshold) {
							//Debug.Log ("\tUP");
							
						} else if (direction.y < -directionDetectionThreshold) {
							//Debug.Log ("\tDOWN");
							
						} else if (direction.x > directionDetectionThreshold) {
							//Debug.Log ("\tRIGHT");
							GameController.Instance.OnGestureSwipe(toLeft:false);
							
						} else if (direction.x < -directionDetectionThreshold) {
							//Debug.Log ("\tLEFT");
							GameController.Instance.OnGestureSwipe(toLeft:true);
							
						}
					}
					
				}
				
				// Handle swipe gestures
				break;
			}
		}
	}
}
