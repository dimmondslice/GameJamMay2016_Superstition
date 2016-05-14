using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public Character character;

	void Awake()
	{
		character = GameObject.FindObjectOfType<Character>();
	}

	public void GameOver()
	{
		character.GetComponentInChildren<Animation>().Play("SwingNoRootMotion");
		print("gameover");
	}
}
