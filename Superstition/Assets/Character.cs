using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	
	public Transform itemSocket;
	public BezierCurveMesh bez;
	public Transform sleeveCenter;

	public int playerNum;
	
	protected Rigidbody rb;
	protected GameManager gm;

	private GameObject LastCollidedWithItem;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		gm = GameObject.FindObjectOfType<GameManager>();
		sleeveCenter = transform.Find("SleeveCenter");
	}

	void FixedUpdate()
	{
		rb.velocity = transform.forward * 8f;
		
	}
	void LateUpdate()
	{
		//rb.rotation = Quaternion.Euler(new Vector3(0f, rb.rotation.eulerAngles.y + (180 * playerNum), 0f));
	}
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Item")
		{
			GetComponentInChildren<Animation>().Play("SwingNoRootMotion");
			LastCollidedWithItem = other.gameObject;
			Invoke("GrabItem", .5f);
		}
		//if you hit an endzone and it's not your endzone
		else if (other.gameObject.GetComponent<EndZone>() && gm.roundNumber % 2 != other.gameObject.GetComponent<EndZone>().playerNum )
		{
			gm.NextRound();
		}
		else if (other.gameObject.tag == "DangerZone")
		{
			GetComponentInChildren<Animation>().Play("SwingNoRootMotion");
			other.gameObject.AddComponent<Rigidbody>();

			gm.GameOver();
		}
	}
	void OnTriggerEnter(Collider other)
	{
		
	}

	void GrabItem()
	{
		LastCollidedWithItem.GetComponent<Collider>().enabled = false;
		LastCollidedWithItem.transform.parent = itemSocket;
		LastCollidedWithItem.transform.localPosition = Vector3.zero;
	}

	public void DisableCharacter()
	{
		bez.enabled = false;
		foreach (MonoBehaviour comp in GetComponentsInChildren<MonoBehaviour>())
		{
			comp.enabled = false;
		}

		enabled = false;
	}
}
