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
/// This class controls the orbit of the giant spheres that players are trying to hit. They are ranomized to rotate at a given radius away from the player
/// </summary>
public class RandomOrbit : MonoBehaviour {

	public static float OrbitSpeed = 20f; //speed of orbit
	public static float OrbitAxisChange = 350f; //how much of a direction change that is made to the orbit

	private Vector3 orbitAxis; //the current axis of rotation

	/// <summary>
	/// Method used for initalizing the start orbit axis
	/// </summary>
	void Start () {
		orbitAxis = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
	}
	
	/// <summary>
	/// Called once per frame to move a giant sphere and randomize the direction it orbits in.
	/// </summary>
	void Update () {
		this.gameObject.transform.RotateAround (Camera.main.transform.position, orbitAxis, OrbitSpeed*Time.deltaTime);
		orbitAxis += (new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), Random.Range (-1f, 1f)).normalized / OrbitAxisChange*OrbitSpeed);
		orbitAxis.Normalize ();
	}
}