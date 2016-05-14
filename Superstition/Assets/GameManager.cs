using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public Character character;

	public Transform ER_Center;

	public Transform[] endZones;
	public int roundNumber;

	void Awake()
	{
		character = GameObject.FindObjectOfType<Character>();
	}

	public void GameOver()
	{
		print("gameover");
	}
	public void NextRound()
	{
		print("next roudn");
		roundNumber++;

		//character.DisableCharacter();	
		character.bez.enabled = false;	
		Destroy(character.gameObject);

		GameObject newChar = Resources.Load("HandPrefab") as GameObject;
		Vector3 spawnLoc = endZones[roundNumber % 2].position;
		newChar = Instantiate(newChar, spawnLoc, Quaternion.identity) as GameObject;
		character = newChar.GetComponent<Character>();
		character.playerNum = roundNumber % 2;
		//spawn new sleeve
		GameObject newSleeve = Resources.Load("Sleeve") as GameObject;
		newSleeve = Instantiate(newSleeve, endZones[roundNumber % 2].position, Quaternion.identity) as GameObject;

		character.bez = newSleeve.GetComponent<BezierCurveMesh>();
		character.bez.ER_Target = character.sleeveCenter;

		//spawn new pepper
		spawnLoc = new Vector3(Random.Range(-25f, 25f), Random.Range(1f, 10f), Random.Range(-40f, 40f));

		GameObject newPep = Resources.Load("PepperPrefab") as GameObject;
		Instantiate(newPep, spawnLoc, Quaternion.identity);


	}
}
