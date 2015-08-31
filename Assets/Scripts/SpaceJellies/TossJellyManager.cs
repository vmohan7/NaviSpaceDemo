/*
 * This file is part of NaviSpaceDemo.
 * Copyright 2015 Vasanth Mohan. All Rights Reserved.
 * 
 * NaviSpaceDemo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * NaviSpaceDemo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with NaviSpaceDemo.  If not, see <http://www.gnu.org/licenses/>.
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// This class manages the user touch input to control pulling the slingshot back and tossing the current jelly
/// </summary>
public class TossJellyManager : MonoBehaviour {

	public static TossJellyManager Instance; //global instance of the class

	public GameObject slingshotTarget; //the center of the slingshot that the jelly will pass through when fired; it is set in the inspector
	private GameObject slingshotCenter; //the gameobject that the jelly will be parent to; this object will move around

	private GameObject tossableJelly; //the current jelly to toss
	
	private Vector2 prevPos; //previous touch input

	private const float SwapTime = 1f; //time it takes to get a new jelly
	private const float SwapSpeed = 5f; //speed the jelly travels to reach the slingshot

	private bool tossIsReady = false; //whether it is ok to fire the slingshot

	/// <summary>
	/// Initalization of global instance
	/// </summary>
	void Awake () {
		if (Instance == null)
			Instance = this;
	}

	/// <summary>
	/// Initalize events and get the first jelly to toss
	/// </summary>
	void Start () {
		slingshotCenter = new GameObject (); 
		slingshotCenter.transform.position = slingshotTarget.transform.position;
		slingshotCenter.transform.parent = slingshotTarget.transform.parent;

		TouchManager.OnTouchDown += HandleOnTouchDown;
		TouchManager.OnTouchUp += HandleOnTouchUp;
		TouchManager.OnTouchMove += MoveTarget;
		TouchManager.OnTouchStayed += MoveTarget;

		GetJelly ();
	}

	/// <summary>
	/// Method responsible for getting jelly from the ring manager and transfering it into the slingshot
	/// </summary>
	private void GetJelly(){
		JellyRecruit jelly = JellyRingManager.Instance.GetClosestJelly ();
		tossableJelly = jelly.GetTossableJelly ();
		jelly.CreateNewJelly ();
		
		tossableJelly.transform.parent = null;

		iTween.ScaleTo(tossableJelly, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "easeType", "linear", "time", SwapTime));
		iTween.RotateTo(tossableJelly, iTween.Hash("rotation", new Vector3(0f, 180f, 0f), "easeType", "linear", "time", SwapTime));
		StartCoroutine (MoveToTarget ());
		//iTween.MoveTo(tossableJelly, iTween.Hash("position", transform, "easeType", "linear", "time", SwapTime, "oncomplete", "OnAnimationEnd", "oncompletetarget", gameObject));

		tossIsReady = false;
	}

	/// <summary>
	/// Coroutine that moves the jelly towards the slingshot target until it reaches it 
	/// </summary>
	IEnumerator MoveToTarget(){
		Vector3 dir = slingshotTarget.transform.position - tossableJelly.transform.position;
		while (dir.magnitude > 0.1f) {
			tossableJelly.transform.position += dir.normalized*Time.deltaTime*SwapSpeed;
			yield return new WaitForFixedUpdate();
			dir = slingshotTarget.transform.position - tossableJelly.transform.position;
		}

		tossableJelly.transform.parent = transform;
		tossableJelly.GetComponent<Rigidbody> ().isKinematic = false;
		tossIsReady = true;
	}

	/// <summary>
	/// When the player taps for the first time, we save that location as the reference location in prevPos
	/// </summary>
	private void HandleOnTouchDown (int fingerID, Vector2 pos)
	{
		prevPos = pos;
	}

	/// <summary>
	/// When the player moves their finger, we move the slingshot to match pulling it back.
	/// </summary>
	private void MoveTarget(int fingerID, Vector2 pos){
		if (tossIsReady) { 
			Vector2 dir = (pos - prevPos);
			Vector3 dir3D = new Vector3 (dir.x, 0f, dir.y); //controlls XZ plane
		
			slingshotTarget.transform.localPosition += .01f * dir3D;
		
			prevPos = pos;
		}
	}
	
	/// <summary>
	/// When the player releases we fire the jelly from its current position to the target
	/// </summary>
	private void HandleOnTouchUp (int fingerID, Vector2 pos)
	{
		Vector3 dir = (slingshotCenter.transform.position - slingshotTarget.transform.position);
		if (tossableJelly != null && tossIsReady && dir.magnitude > 0f) {
			tossableJelly.GetComponent<Rigidbody> ().AddForce (dir *5f, ForceMode.Impulse);
			tossableJelly.GetComponent<JellyController> ().OnTossed();
			tossableJelly.transform.parent = null;

			slingshotTarget.transform.localPosition = slingshotCenter.transform.localPosition;

			GetJelly();
		}
	}
}