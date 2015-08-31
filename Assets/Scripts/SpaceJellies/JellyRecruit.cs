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
/// This class controls the jelly model before it enters the slingshot AKA a Jelly Recruit. It is mapped to a specific location on the ring around the player
/// </summary>
public class JellyRecruit : MonoBehaviour {

	private GameObject currJelly; //handle to the jelly model
	private float maxScale = 0.25f; //scaling factor for the recruit

	/// <summary>
	/// Start spawning the jelly as soon as we are created
	/// </summary>
	void Awake () {
		StartCoroutine (SpawnNewJelly ());
	}
	
	/// <summary>
	/// Make sure the jelly always looks at the player
	/// </summary>
	void Update () {
		if (currJelly != null)
			currJelly.transform.LookAt (Camera.main.transform.position); //TODO make this not the main camera
	}

	/// <summary>
	/// Coroutine to spawn the jelly over a set period of time 
	/// </summary>
	IEnumerator SpawnNewJelly(){
		currJelly = Instantiate (JellyRingManager.Instance.GetRandomJellyPrefab (), transform.position, new Quaternion ()) as GameObject;
		currJelly.transform.parent = transform;
		currJelly.transform.localScale = Vector3.zero;

		float currScale = 0f;
		while (currScale < maxScale) {
			currScale += maxScale/100f;
			currJelly.transform.localScale = new Vector3(currScale, currScale, currScale);
			yield return new WaitForFixedUpdate();
		}
	}

	/// <summary>
	/// Returns the jelly to toss
	/// </summary>
	public GameObject GetTossableJelly() {
		return currJelly;
	}

	/// <summary>
	/// Creates a new jelly at this location in the ring
	/// </summary>
	public void CreateNewJelly() {
		StartCoroutine (SpawnNewJelly ());
	}
}