using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	[SerializeField] new AudioSource audio;
	[SerializeField] AudioSource clapAudio;

	[Header("Audio Clips")]
	[SerializeField] AudioClip[] tic;
	[SerializeField] AudioClip[] tac;
	[SerializeField] AudioClip[] clap;

	[SerializeField] AudioClip[] success;
	[SerializeField] AudioClip[] failure;
	[SerializeField] AudioClip[] applause;
	[SerializeField] AudioClip[] boo;

	public static AudioManager instance;

	bool nextPlayTac;

	private void Awake()
	{
		if (instance) Destroy(instance);
		instance = this;
	}

	public void PlayTicTac()
	{
		audio.PlayOneShot(nextPlayTac ? GetRandomClip(tac) : GetRandomClip(tic));
		nextPlayTac = !nextPlayTac;
	}

	public void PlayClap()
	{
		foreach (var clap in clap)
		{
			clapAudio.PlayOneShot(clap);
		}
	}

	public void StopClap()
	{
		StartCoroutine(StopClap(.5f));
	}

	IEnumerator StopClap(float delay)
	{
		yield return new WaitForSeconds(delay);
		clapAudio.Stop();
	}

	public void RestartClap()
	{
		clapAudio.Stop();
		PlayClap();
	}

	public void PlaySuccess()
	{
		audio.PlayOneShot(GetRandomClip(success));
	}

	public void PlayFailure()
	{
		audio.PlayOneShot(GetRandomClip(failure));
	}

	public void PlayApplause()
	{
		audio.PlayOneShot(GetRandomClip(applause));
	}

	public void PlayBoo()
	{
		audio.PlayOneShot(GetRandomClip(boo));
	}

	AudioClip GetRandomClip(AudioClip[] clips) => clips[Random.Range(0, clips.Length)];
}