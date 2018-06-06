using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AnchorMap : MonoBehaviour {

	// Use this for initialization
	// void Start () {
		
	// }
	
	// Update is called once per frame
	void Update () {
		Touch touch = Input.GetTouch(0);

		if(Input.touchCount<1 || touch.phase != TouchPhase.Began) {
			return;
		}

		// raycast against the point where the player touched
		TrackableHit hit;

		TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds|TrackableHitFlags.PlaneWithinPolygon|TrackableHitFlags.FeaturePointWithSurfaceNormal;

		if(Frame.Raycast(touch.position.x,touch.position.y,raycastFilter,out hit)) {
			var anchor = hit.Trackable.CreateAnchor(hit.Pose);
			transform.position = anchor.transform.position;
		}

		
	}
}
