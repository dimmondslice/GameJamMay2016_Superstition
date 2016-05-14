using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	protected Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		rb.velocity = transform.forward * 8f;
		
	}
	void LateUpdate()
	{
		rb.rotation = Quaternion.Euler(new Vector3(0f, rb.rotation.eulerAngles.y, 0f));
	}
	void OnCollisionEnter(Collision other)
	{
		GameObject.FindObjectOfType<GameManager>().GameOver();
	}
}
