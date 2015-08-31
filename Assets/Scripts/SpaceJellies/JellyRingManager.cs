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
/// This class manages the ring of possible jellies that rotates around the player
/// </summary>
public class JellyRingManager : MonoBehaviour {

	public static JellyRingManager Instance; //global instance to this class

	public GameObject[] jellyPrefabs; //array of possible jellies that can be spawned, which is filled in the inspector

	private int numJellies = 7; 
	private float jellyRadius = 1.5f; //the distance away from the user

	private JellyRecruit[] jellies; //the array of jelly recruits that float around the user

	/// <summary>
	/// Sets up the global instance and spawns the inital set of jellies
	/// </summary>
	void Awake () {
		if (Instance == null)
			Instance = this;

		jellies = new JellyRecruit[numJellies];
		createInitalJellies ();
	}

	/// <summary>
	/// Uniformally spreads out the jelly recruits around the player 
	/// </summary>
	private void createInitalJellies(){
		float currAngle = 0f;
		for (int i = 0; i < numJellies; i++) {
			GameObject jelly = new GameObject();
			jelly.transform.parent = transform;
			Vector3 pos = transform.position;
			pos.x = Mathf.Cos(currAngle)*jellyRadius;
			pos.z = Mathf.Sin(currAngle)*jellyRadius;
			jelly.transform.position = pos;
			
			jelly.AddComponent<JellyRecruit>();
			currAngle += Mathf.PI*2f/numJellies; //update angle
			jelly.name = "Jelly " + (i+1);
			jellies[i] = jelly.GetComponent<JellyRecruit>();
		}
	}

	/// <summary>
	/// Rotates the jellies around the player
	/// </summary>
	void Update(){
		transform.RotateAround(Vector3.zero, new Vector3(0f, 1f, 0f), .25f); //roates .25 of a degree per update
	}

	/// <summary>
	/// Picks a random jelly from the list of possible prefabs
	/// </summary>
	public GameObject GetRandomJellyPrefab(){
		return jellyPrefabs [Random.Range (0, jellyPrefabs.Length)];
	}

	/// <summary>
	/// Gets the jelly that is closest to the player's line of sight
	/// </summary>
	public JellyRecruit GetClosestJelly(){
		float minAngle = 360f;
		JellyRecruit closestJelly = jellies[0];

		foreach (JellyRecruit j in jellies) {
			Vector3 targetDir = j.transform.position - Camera.main.transform.position;
			float angleBetween = Vector3.Angle (Camera.main.transform.forward, targetDir);
			if (angleBetween < minAngle){
				minAngle = angleBetween;
				closestJelly = j;
			}
		}

		return closestJelly;
	}
}