using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	[SerializeField]
	private AudioClip[] walkDirt;
	[SerializeField]
	private AudioClip[] walkSnow;
	[SerializeField]
	private AudioClip[] walkWood;
	[SerializeField]
	private AudioClip[] walkMetal;

	[SerializeField]
	private AudioClip[] shootHit;
	[SerializeField]
	private AudioClip[] ShootMiss;

	[SerializeField]
	private AudioClip[] stabHit;
	[SerializeField]
	private AudioClip[] StabMiss;

	[SerializeField]
	private AudioClip[] snipeHit;
	[SerializeField]
	private AudioClip[] snipeMiss;

	[SerializeField]
	private AudioClip[] bandageHit;
	[SerializeField]
	private AudioClip[] bandageMiss;

	[SerializeField]
	private AudioClip[] grenade;

	[SerializeField]
	private AudioClip[] death;


	public enum walkSourface
	{
		dirt, snow, wood, metal
	}

	private AudioSource audioSource;

	private void Awake()
	{
		GameRef.PlaySound = this;
		audioSource = audio;
	}

	private void PlayRandom(AudioClip[] someAudioClips)
	{
		if (someAudioClips.Length != 0)
		{
		int randomize = Random.Range(0, someAudioClips.Length);
		audioSource.PlayOneShot(someAudioClips[randomize]);
		}
		else
		{
			Debug.LogWarning("No sound in AudioClip[]");
		}
	}

	public void Walk()
	{
		if (Application.loadedLevel == 3)
		{
			PlayRandom(walkSnow);
		}
		else
		{
			PlayRandom(walkDirt);
		}

		/*switch(walkingOn)
		{
			case walkSourface.dirt:
			PlayRandom(walkDirt);
			break;
			case walkSourface.snow:
			PlayRandom(walkSnow);
			break;
			case walkSourface.wood:
			PlayRandom(walkWood);
			break;
			case walkSourface.metal:
			PlayRandom(walkMetal);
			break;
		}*/
	}

	public void Shoot(bool hit)
	{
		if (hit)
		{
			PlayRandom(shootHit);
		}
		else
		{
			PlayRandom(ShootMiss);
		}
	}

	public void Stab(bool hit)
	{
		if (hit)
		{
			PlayRandom(stabHit);
		}
		else
		{
			PlayRandom(StabMiss);
		}
	}

	public void Snipe(bool hit)
	{
		if (hit)
		{
			PlayRandom(snipeHit);
		}
		else
		{
			PlayRandom(snipeMiss);
		}
	}

	public void Bandage(bool hit)
	{
		if (hit)
		{
			PlayRandom(bandageHit);
		}
		else
		{
			PlayRandom(bandageMiss);
		}
	}

	public void Grenade()
	{
		PlayRandom(grenade);
	}

	public void Death()
	{
		Invoke ("DeathDelay", 1.5f);
	}

	private void DeathDelay()
	{
		PlayRandom(death);
	}


}
