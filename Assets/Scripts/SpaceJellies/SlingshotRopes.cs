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
/// This class controls re-rendering the line renders when they are stretched by the rope. 
/// </summary>
public class SlingshotRopes : MonoBehaviour {

	public LineRenderer rope1;
	public LineRenderer rope2;

	public Transform target;

	/// <summary>
	/// Initalize width of both lines
	/// </summary>
	void Start(){
		rope1.SetWidth (0f, .5f);
		rope2.SetWidth (0f, .5f);
	}

	/// <summary>
	/// Updates the renderer based on the game objects
	/// </summary>
	void Update(){
		rope1.SetPosition (0, rope1.transform.position);
		rope2.SetPosition (0, rope2.transform.position);

		rope1.SetPosition (1, target.position);
		rope2.SetPosition (1, target.position);
	}
}