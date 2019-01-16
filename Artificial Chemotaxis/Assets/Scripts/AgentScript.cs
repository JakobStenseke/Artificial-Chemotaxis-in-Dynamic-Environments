using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Agent class governs all the aspects of the agents, including the Perceptor, Move, Energy, and Evolve-methods.
/// Simply put, the class do the following:
/// 1. Perceptor -- agents get an input value depending on the distance to the nutrition source.
/// 2. Move -- The agents either move or rotate depending their previous input - new input + bias.
/// 3. Energy -- Each cycle, agent energy level is updated depending on their distance to the nutrition source.
/// 4. Evolve -- Called every 100th cycle and updates the agents bias based on the highest achieved energy.
/// The fields can be turned public in order to be monitored real-time in Unity inspector.
/// </summary>

public class AgentScript : MonoBehaviour {

	private float moveSpeed; //Controls the movement speed of the agent
	private float rotationSpeed; //Controls the rotation speed
	private float cycleRate = 0.1f; //How fast (in seconds) the Percepter and Move methods invoke eachother

	private float newInput; //Distance to the nutrition source
	private float previousInput; //Stores the value for the next cycle

	private float bias; //Determines the probability for firing (i.e., agent moving forward). 1 = always firing, 0 = never firing
	private float oldBias; //Stores the bias for the next generation
	private float bestBias; //Stores the bias that achieved the smallest energy loss
	private float learningRate; //Determines how much the bias is changed each generation

	private float energy; //Updated each cycle, monitors the loss of energy depending on distance to nutrition source
	private float highestEnergy; //Stores the highest achieved energy (i.e, smallest energy loss)

	public int cycle; //Monitors the cycles
	public int generation; //Monitors the generations
	private bool fire; //Determines whether the agent moves or rotates
	private float fireProbability; // Short for previousInput - newInput + bias

	public SpriteRenderer BiasColor; //Reference to the agent color in order to change it depending on bias valye
	private Rigidbody2D rBody; //Reference to the agent body to control movement

	// Use this for initialization
	void Start () {

		//Set initial values
		energy = 0;
		cycle = 0;
		generation = 0;
		previousInput = 0;
		learningRate = 0.2f;
		bias = Random.Range (0.0f, 1.0f); //sets bias randomly between 0 and 1
		fire = false;
		rBody = GetComponent<Rigidbody2D>();

		//Initialize the cycle by calling the Perceptor method
		Invoke ("Perceptor", cycleRate);
		
	}
	
	// Update is called once per frame
	void Update () {

		//Controls the movement of the agents
		rBody.velocity = moveSpeed * transform.right;
		rBody.angularVelocity = rotationSpeed;

		//If the neuron is firing, the agent moves forward in the current direction, if not, it rotates.
		if (fire == true) 
		{
			moveSpeed = 10;
			rotationSpeed = 0;
		} 
		else 
		{
			moveSpeed = 0;
		}

		//Updates the R-value (from blue to pink) of the color of agents body depending on bias value.
		BiasColor.gameObject.GetComponent<SpriteRenderer>().color = new Color (bias, 0.23f, 0.52f, 1f);
	
	}
		
	//Agents get an input determined by the distance to the nutrition source
	void Perceptor ()
	{
		//gets a reference of the Nutrition Source object (tagged with nutrition)
		GameObject nutrition = GameObject.FindWithTag("Nutrition");

		//new input is set as the distance between agent and nutrition source
		newInput = Vector3.Distance(transform.position, nutrition.transform.position);

		Energy ();

		//calls evolve method at every 100th cycle and resets the cycle value
		if (cycle >= 99)
		{ 
			Evolve();
			cycle = 0;
		}
		//calls the Move method after some delay
		Invoke ("Move", cycleRate);
	}

	/// <summary>
	/// Move method:
	/// fireProbability is given by previous input - new input + bias, and is then compared to a random value (probability to spike).
	/// previousInput - newInput gives a negative value if the newly perceved distance to the source is
	/// greater than the previously perceived distance. Positive value if the distance is smaller.
	/// This causes firing (moving forward) to be more probable if the agent is moving towards the source,
	/// and correspondly, more probable to not fire (rotate) if its moving away from the source.
	/// The biases vary among agents. A value too high (over 1) causes the agent to always move forward.
	/// A value too low (under 0.5) causes the agent to always rotate.
	/// </summary>
	void Move ()
	{
		//The agents neurons fire in the first cycle since there is no previousInput yet.
		if (cycle == 0) 
		{
				fire = true;
		}
		else 
		{
			fireProbability = previousInput - newInput + bias;
			float number = Random.Range (0.0f, 1.0f);

			if (number < fireProbability)
			{
				fire = true;
			} 
			else 
			{
				fire = false;
				rotationSpeed = Random.Range (400.0f, 2000.0f); //make the agent rotate at a random speed
			}
		}
		cycle ++; //increase the cycle value
		previousInput = newInput; //The newInput is stored in the previousInput
		Invoke ("Perceptor", cycleRate); //Repeats the cycle
	}

	//Updates the energy loss
	void Energy ()
	{
		energy -= newInput;
	}
		
	/// <summary>
	/// The Evolve method changes the bias of the agent, so that it can change its firingrate depending on the energyloss
	/// </summary>
	void Evolve ()
	{

		if (generation == 0)
		{
			highestEnergy = energy; //sets an initial highest energy
		}

		if (generation > 0) //skips the first generation since nothing can be compared
		{

			if (energy > highestEnergy) //If the current energy is higher than the highest energy
			{
				highestEnergy = energy; //stores the current energy as the new highest achieved energy
				bestBias = bias; //stores the bias as the bestbias, thus connects the best bias to the highest achieved energy
			}
				
			if (bestBias > oldBias) {
				bias += Random.Range (0.025f, learningRate); //more likely to fire, since it is proven successful (more energy)
			} 
			else //if the old bias is higher
			{
				bias -= Random.Range (0.025f, learningRate); //less likely to fire, since it is proven successful (more energy)
			}

			//otherwise the agent might never fire
			if (bias < 0f) 
			{
				bias += learningRate;
			}

			//otherwise the agent might always fire
			if (bias > 1)
			{
				bias -= learningRate;
			}
		}

		oldBias = bias; //store the old bias
		energy = 0; //resets energy
		generation++; //start next generation

		//Updates the TotalBias and TotalEnergy in the Manager
		GameObject man = GameObject.Find ("Manager");
		ManagerScript manScript = man.GetComponent<ManagerScript> ();
		manScript.TotalBias += bias;
		manScript.TotalEnergy += highestEnergy;
	}
}
