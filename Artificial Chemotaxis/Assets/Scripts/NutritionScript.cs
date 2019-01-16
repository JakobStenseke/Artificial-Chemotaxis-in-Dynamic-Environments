using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This class controls the movement of the nutrition source
public class NutritionScript : MonoBehaviour {

	//fields determining the spatial boundaries of the simulation
	//can be defined by enums later
	float maxX = 52f;
	float minX = -52f;
	float maxY = 38f;
	float minY = -38f;

	//variables used for generating random movement
	float RandomX;
	float RandomY;

	//public variable determining the speed of the source
	public float moveSpeed;


	//Initialization
	void Start () 
	{
		//Calls the ChangeDirection-method after 0 seconds and then every X-seconds
		InvokeRepeating ("ChangeDirection", 0, 2f);
	}
		
	// Update is called once per frame
	void Update () 
	{

		//Makes the nutrition source move based on the random values given in the ChangeDirection method
		transform.Translate (RandomX * moveSpeed * Time.deltaTime, RandomY * moveSpeed * Time.deltaTime, 0);

		//Rotates the Z-axis of the nutrition source
		//transform.Rotate (0, 0, 100 * Time.deltaTime);

		//Makes sure that the nutrition source stays within the boundary of the simulation
		if (transform.position.x >= maxX){
			RandomX = -1f;
		}
		if (transform.position.x <= minX) {
			RandomX = 1f;
		}
		if (transform.position.y >= maxY){
			RandomY = -1f;
		}
		if (transform.position.y <= minY) {
			RandomY = 1f;
		}
	}

	//Each time this method is called, the 
	void ChangeDirection()
	{
		RandomX = Random.Range(-1f,1f);
		RandomY = Random.Range(-0.6f,0.6f); //move slightly less in the Y-axis
		moveSpeed = Random.Range (1f, 6f);
	}
}
