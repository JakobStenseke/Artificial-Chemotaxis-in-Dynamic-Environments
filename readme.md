# Artificial Chemotaxis in Dynamic Environments

![Screenshot](/Screenshot.png)

# Project Description

The main objective of this project is to simulate agents evolving chemotaxis in a dynamic environment. More generally, the purpose is to shed light on how organisms develop chemotaxis, and more particularly how this is done in artificial agents employing the basic ’forward or rotate’ movement (as employed by the bacteria C. elegans). Whereas other studies have developed more detailed neural circuits for bacterial chemotaxis [1], this project explores whether a single probabilistic spiking perceptrons can help simpler artificial agents to achieve chemotaxis in a dynamic environment. Ideally, the system will be interesting for research in fields such as artificial biology, evolutionary computing, and artificial intelligence.

<b>Chemotaxis</b>

Chemotaxis is a biological term referring to the ability for organisms to move in response to a chemical stimulus [2]. This ability is critical in order for bacteria to acquire food, by moving toward the highest concentration of nutrition (e.g. food molecules), or away from poisons. For multicellular organisms, chemotaxis is essential for various stages of development and normal functionality, from the movement of sperm towards the egg to the migration of leukocytes during infection or injury [3].

<b>Dynamic Environment</b>

Dynamic environment in this particular project is a 2-dimensional finite space (R2) with two axes (x and y). The position of any point in the environment can thus be given by an ordered pair of real numbers — e.g. (3, 5) — referring to the distance of that point relative to the origin measured along the given axis. The environment has a point simulating a source of nutrition that is randomly moving around in the space. The environment is thus dynamic in the sense that the source of nutrition randomly changes its location over time.

<b>Artificial Agents</b>

Artificial neural networks (ANNs) are computational systems inspired by the biological neural networks of animal brains and are able to ’learn’ based on data input. Originally developed in the 50’s, the recent years has seen a significant development of various ANNs and deep learning techniques now widely used in artificial intelligence systems such as image recognition software [4]. This project will utilize a probabilistic version of spiking neural networks (SNNs) that do not fire at each propagation cycle (typical of other neural networks), but only when a membrane potential — the membrane electrical charge of the neuron — reaches a certain value [5]. In this case, neural firing is computed probabilistically, meaning that a neuron with an electrical charge of 0.7 has a 70% chance of firing.

Artificial agents are equipped with five main properties:

<b>• Motor</b> — Similar to the flagella of the bacteria Escherichia coli (also known as E. coli), the motor enables agents to behave in either one of two ways: (1) move forward in a straight line or (2) rotate at random.

<b>• Perceptor</b> — Enables the agents to receive input value from their current environment, determined by the distance between agent and the nutrition source (the environmental chemical gradient).

<b>• Neuron</b> — Takes the input value from the perceptor, subtracts it with the previous perceptor value and then adds a neural bias value unique to the agent. The result is an output that determines the probability for an agent to move forward (instead of rotating). For instance, if the new input (NI) is 0.5, previous input (PI) is 0.3, and the neural bias (B) is 0.5, the the neuron has a 70% probability of firing (since NI — PI + B = 0.7), and the agent moves forward [6]. The reason for having new input subtracted by the previous input is that it enables the agents to perceive the change of nutrition concentration in the environment over time rather than the concentration of nutrition at a given moment (which would not allow them to sense whether or not they are moving towards a higher concentration). The reason for using probabilistic neural firing rather than having the neurons building up potential over time — as done in more conventional spiking neural networks — is purely for simplification.

<b>• Energy level</b> — At birth, the agents are given an energy value that is either increasing or decreasing at every step depending on the concentration of nutrition of their current environment. Consequently, an agent able to navigate to a higher concentration of nutrition will be rewarded by an increased energy value.

<b>• Evolution</b> — After a set number of steps, the agents update their neural bias that determine the frequency of their neural firing. The new bias is calculated based on the difference between the agents current energy value and highest achieved energy value, and the corresponding difference between the current bias and the bias that achieved the smallest loss of energy.

<b>Algorithm</b>

The evolutionary algorithm is designed in the following way:
1. A number of agents are spawned and randomly distributed in the environment. The agents are given initial neural bias values randomly. The source of nutrition is placed in the environment.
2. The agents receive new input from the environment (their proximity to the nutrition source) and calculate their neural output based on their previous input and neural bias. The neural output determines the probability that agents move or rotate at random.
3. Step two repeats for a set number of times until evolution occurs, where energy levels are evaluated and the neural biases are changed.
4. Step two and three then repeats again until the simulation is stopped.

<b>Software Requirements</b>

The simulation is made using Unity Engine (version 2017.2.3p3 Personal, downloaded here: https://store.unity.com/download?ref=personal). There are several reasons for choosing Unity: it offers several easy-to-use functions, such as a framework for simulating time and space (2D/3D coordinate systems), motion and collision, monitoring, and other handy design features. Unity is thus needed in order to properly test and assess the project. However, I have also made a Windows and a Mac build of the application that can be found in the "Builds" folder. It is thus possible to run the application without downloading Unity (but one is unable to modify the simulation).

<b>Run</b>

To run the application, simply press "+10 agents" any number of times, and then the play button to start the simulation. Using Unity, make sure that the "MainScene" is the current scene. Use the speed and zoom sliders to modify the simulation. Press reset to restart the application. Different screen resolutions might result in a slightly different design of the GUI. The text displays the following: 
- <b>Cycle</b> refers to the current movement-perception cycle of the agent.
- <b>Generation</b> to the current generation (100 cycles).
- <b>Population</b> is the number of current agents.
- <b>Bias average</b> is the average bias of all the agents.
- <b>Energy average</b> is the average highest energy (or, smallest energy loss) of all the agents.

<b>Notes</b>

1. E.g. Santurkar, S & Rajendran, B. (2014) “A neural circuit for navigation inspired by C. elegans Chemotaxis”, arXiv 1410.7881 [cs.NE].

2. Chisholm, Hugh, ed. (1911). "Chemotaxis". Encyclopædia Britannica. 6 (11th ed.). Cambridge University Press. p. 77. 

3. de Oliveira S, Rosowski EE, Huttenlocher A (2016). "Neutrophil migration in infection and wound repair: going
forward in reverse". Nature Reviews. Immunology. 16 (6): 378–91.

4. Gerven, M. A. J van & Bohte, S. M. (2018). ”Artificial neural networks as models of neural information processing”.
Frontiers in Computational Neuroscience. 11 (114).

5. Maass, Wolfgang (1997). "Networks of spiking neurons: The third generation of neural network models". Neural Networks. 10 (9): 1659–1671.

6. An alternative approach is to use a number of neural weights that multiplies the input, and a threshold value that determines whether the neuron is firing or not (as often done in neural networks). However, for the purpose of simplification, I have decided to use spiking neurons that only use a single bias based on addition (rather than multiplication).
