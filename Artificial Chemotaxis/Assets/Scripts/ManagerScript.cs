using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// The Manager class manages the GUI (buttons, sliders and text).
/// Its Generate-method spawns 10 agents, and must be called at 
/// least once before the simulation can start (by pressing play).
/// </summary>

public class ManagerScript : MonoBehaviour 
{
	public GameObject PlayPauseButton; //GUI button
	public GameObject StopButton; //GUI button
	public GameObject SpawnButton; //GUI button
	public GameObject AgentPrefab; //The agent prefab
	public Slider SpeedSlider; //GUI Slider
	public Slider ZoomSlider; //GUI Slider
	private Image spriteRenderer; //Used to change the image of the play/pause button
	public Sprite playSprite; //Play icon
	public Sprite pauseSprite; //Pause icon
	public Camera cam;

	public Text DisplayText; //GUI text

	private bool playing;
	private bool hasSpawned;

	private int spawnAmount; //Can be used to change the number of agents spawned
	private int treshold; //making sure that bias & energy average is updated once per generation
	private float biasAverage; //Total biases / number of agents
	private float highestEnergyAverage; //Total energy / number of agents

	public float TotalEnergy; //Stores the total sum of energy of all agents
	public float TotalBias; //Stores the total sum of biases

	//Use this for initialization
	void Start () 
	{
		//Set initial values
		hasSpawned = false;
		SpeedSlider.value = 0.2f;
		ZoomSlider.value = 0.8f;
		playing = false;
		treshold = 1;
		highestEnergyAverage = 0;
		biasAverage = 0;
		spawnAmount = 10;
		spriteRenderer = PlayPauseButton.GetComponent<Image> ();
		spriteRenderer.sprite = playSprite;

		//Get references to the GUI buttons 
		Button pbtn = PlayPauseButton.GetComponent<Button> ();
		pbtn.onClick.AddListener (PlayPause);

		Button stopbtn = StopButton.GetComponent<Button> ();
		stopbtn.onClick.AddListener (Reset);

		Button spawnbtn = SpawnButton.GetComponent<Button> ();
		spawnbtn.onClick.AddListener (Generate);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Connects the camera zoom to zoom slider
		cam.orthographicSize = 3 + (ZoomSlider.value * 47f);

		//Connects the speed to speed slider
		if (playing == true) 
		{
			Time.timeScale = SpeedSlider.value * 10;
		} 
		else 
		{
			Time.timeScale = 0;
		}
	}

	//Method generating spawnAmount number of agents
	void Generate()
	{

		//enables the Play/Pause-button
		if (hasSpawned == false) 
		{
			hasSpawned = true;
			PlayPauseButton.SetActive (true);
		}
			
		//Spawns 10 agents at random positions over the simulation
		for (int i = 0; i < spawnAmount; i++) 
		{
			Vector3 spawnPosition = new Vector3 (Random.Range (-52, 52), Random.Range (-38, 38), 0);
			Instantiate (AgentPrefab, spawnPosition, Quaternion.identity);
		}
	}

	//Method governing the pause and play-button
	void PlayPause()
	{
		//cheks so that agents are spawned first
		if (hasSpawned == true) 
		{
			//toggles the playing
			playing = !playing;

			//Switches the play/pause image
			if (playing == false) {
				spriteRenderer.sprite = playSprite;
			} else {
				spriteRenderer.sprite = pauseSprite;
			}

			//repeatedly invoking the UpdateText once
			bool countStarted = false;
			if (countStarted == false) 
			{
				InvokeRepeating ("UpdateText", 0, 0.1f);
				countStarted = true;
			}
		}
	}

	//Reloads the scene
	void Reset()
	{
		SceneManager.LoadScene("MainScene");
	}

	void UpdateText()
	{
		int NumberOfCells = GameObject.FindGameObjectsWithTag ("Cell").Length;
		GameObject agentt = GameObject.FindGameObjectWithTag ("Cell");
		AgentScript aScript = agentt.GetComponent<AgentScript> ();

		//treshold is used to make sure that the bias & energy average is updated only once for every generation
		if (aScript.generation > treshold)
		{
			treshold = aScript.generation;
			highestEnergyAverage = TotalEnergy / NumberOfCells;
			biasAverage = TotalBias / NumberOfCells;
			TotalEnergy = 0; //reset the total energy
			TotalBias = 0; //reset the total energy
		}

		//Updates the text
		DisplayText.text = 
			"SPEED" +
			"\nZOOM" +
			"\nCYCLE: " + aScript.cycle +
			"\nGENERATION: " + aScript.generation +
			"\nPOPULATION: " + NumberOfCells +
			"\nBIAS AVERAGE: " + biasAverage.ToString("0.000") + 
			"\nENERGY AVERAGE: " + highestEnergyAverage.ToString("0");
	}
}
