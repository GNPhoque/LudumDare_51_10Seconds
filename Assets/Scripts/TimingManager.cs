using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimingManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] Transform clockHand;

	[SerializeField] float[] clockHandPositions;
	[SerializeField] float startCameraScale;
	[SerializeField] float endCameraScale;

	float startTimeOffset;
	float scoreTotalOffset;
	int currentLoopCount;
	int currentSecond;
	int clapStopTime;
	bool startClapping = true;
	bool canClick;
	public static TimingManager instance;

	private void Awake()
	{
		if (instance) Destroy(instance);
		instance = this;
		
		UpdateScore();
		StartTimer();
	}

	private void Update()
	{
		if (canClick && Input.GetKeyDown(KeyCode.Space))
		{
			canClick = false;
			float currentTime = Time.time - startTimeOffset;
			int timePrecision = (int)((currentTime - (int)currentTime) * 100);
			print($"{currentSecond} : {timePrecision}");
			if (currentSecond == 9)
			{
				if (timePrecision >= 50)
				{
					scoreTotalOffset += (100f - timePrecision) / 100f;
					UpdateScore();
					AudioManager.instance.PlaySuccess();
					return;
				}
			}
			if (currentSecond == 0 || currentSecond == 10)
			{
				if (timePrecision < 50)
				{
					scoreTotalOffset += timePrecision / 100f;
					UpdateScore();
					AudioManager.instance.PlaySuccess();
					return;
				}
			}
			AudioManager.instance.PlayFailure();
		}
	}

	public void StartTimer()
	{
		startTimeOffset = Time.time;
		InvokeRepeating("OnSecondPassedCue", 1f, 1f);
	}

	void OnSecondPassedCue()
	{
		currentSecond++;
		SetClockHand();
		float currentTime = Time.time - startTimeOffset;
		if ((int)currentTime != currentSecond + currentLoopCount * 10) Debug.LogError($"{currentSecond} differs from time : {currentTime}");

		AudioManager.instance.PlayTicTac();
		if (startClapping)
		{
			AudioManager.instance.PlayClap();
			startClapping = false;
		}

		if(currentSecond < 10 && 10 - currentLoopCount <= currentSecond)
		{
			AudioManager.instance.StopClap();
		}

		if(currentSecond == 1)
		{
			canClick = true;
		}
		else if (currentSecond == 10)
		{
			OnTenSecond();
		}
	}

	void OnTenSecond()
	{
		currentLoopCount++;
		currentSecond = 0;
		if(10 - currentLoopCount > currentSecond)
		AudioManager.instance.RestartClap();
	}

	void UpdateScore()
	{
		scoreText.text = $"SCORE : {scoreTotalOffset}";
	}

	void SetClockHand()
	{
		clockHand.rotation = Quaternion.Euler(clockHand.rotation.x, clockHand.rotation.y, clockHandPositions[currentSecond - 1]);
	}
}
