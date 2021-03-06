﻿using UnityEngine;
using System.Collections;

public class Peach : MonoBehaviour {

	public Transform mario;
	public Transform bowser;

	public GameObject heartParticles;
	public GameObject helpParticles;
	public GameObject angryEmote;

	public enum EmotionLevel{neutral, scared, happy};
	public EmotionLevel emotionLevel;

	public bool isReset = true;

	GameObject currentHearts;
	GameObject currentHelp;
	GameObject currentAngryEmote;

	[Header("Audio")]
	public GameObject audioManager;
	public AudioClip[] peachGoodSounds;
	public AudioClip peachHappy;
	public AudioClip peachScared;


	AudioSource sfxAudio;
	AudioSource audioSource;



	void Start(){
		audioSource = audioManager.GetComponent<AudioSource>();
		sfxAudio = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		//face mario because you love him
		if (mario.position.x < gameObject.transform.position.x){
			gameObject.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}

		//moving peach with mouseclick
		if (Input.GetMouseButtonDown(0)){

			//return peach back to neutral emotion
			emotionLevel = EmotionLevel.neutral;
			isReset = true;

			//clean hierarchy of heart or help particles
			if (currentHearts != null){
				Destroy(currentHearts);
			}
			if (currentHelp != null){
				Destroy(currentHelp);
			}

			if (angryEmote != null){
				Destroy(currentAngryEmote);
			}
		}
	}

	public void MakeGoodSound(){
		int goodSoundIndex = Random.Range(0, peachGoodSounds.Length);
		sfxAudio.PlayOneShot(peachGoodSounds[goodSoundIndex], 0.5f);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (emotionLevel == EmotionLevel.neutral){

			//if mario reaches peach
			if (col.gameObject == mario.gameObject){
				currentHearts = Instantiate(heartParticles, transform.position + Vector3.up * 0.5f, Quaternion.Euler(-90, 0, 0)) as GameObject;
				emotionLevel = EmotionLevel.happy;

				currentAngryEmote = Instantiate(angryEmote, bowser.position + Vector3.up * 0.7f, Quaternion.identity) as GameObject;
				isReset = false;
				audioSource.pitch = Time.timeScale = 1f;
				sfxAudio.PlayOneShot(peachHappy, 0.5f);

				//hey bowser, stop following peach!
				FindObjectOfType<Bowser>().StopCoroutine("FollowPath");
			}

			//if bowser reaches peach
			if (col.gameObject == bowser.gameObject){
				currentHelp = Instantiate(helpParticles, transform.position + Vector3.up * 0.5f, Quaternion.Euler(-90, 0, 0)) as GameObject;
				emotionLevel = EmotionLevel.scared;
				isReset = false;
				audioSource.pitch = Time.timeScale = 1f;
				sfxAudio.PlayOneShot(peachScared, 0.5f);
			}
		}
	}
}
