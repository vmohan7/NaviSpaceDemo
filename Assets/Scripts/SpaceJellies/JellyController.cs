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
/// This class the life span of the jelly. 
/// </summary>
[RequireComponent (typeof(AudioSource))]
public class JellyController : MonoBehaviour {

	private AudioSource audio; //the audio clip to be played when the jelly is launched
	public const float ShrinkTime = 10f; //the time it takes for the jelly to disappear

	/// <summary>
	/// Initalization
	/// </summary>
	void Start(){
		audio = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// Called when the jelly is tossed
	/// </summary>
	public void OnTossed() {
		audio.Play ();
		iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.zero, "easeType", "linear", "time", ShrinkTime, "oncomplete", "OnShrinkEnd", "oncompletetarget", gameObject));
	}

	/// <summary>
	/// When the timer ends, we subtract points and clean up by destroying the game object.
	/// </summary>
	private void OnShrinkEnd(){
		ScoreManager.Instance.SubtractScore (ScoreManager.JellyMiss);
		Destroy (this.gameObject);
	}

	/// <summary>
	/// When this object hits something, we grant points
	/// </summary>
	private void OnCollisionEnter(){
		ScoreManager.Instance.AddScore (ScoreManager.JellyHit);
	}
}