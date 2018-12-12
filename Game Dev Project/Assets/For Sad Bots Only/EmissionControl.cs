using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put this on an empty gameObject designated particle GM,
// used to control the emission of the particles in the game
public class EmissionControl : MonoBehaviour
{
    // So just copy and paste these Public particle systems into the main Player
    // script, and it should be good to go from there

    // These particles systems are childed to the players, each player has their own copy of the
    // particle system, so when you need to assign them in the inspector, that's where
    // they are.

    // Particles need to be called in the Player script, where-ever the relevant variable is.

    // For example, movingRed and movingBlue need to Emit(1) whenever the player is moving
    // along the ground, so whenever some kind og grounded variable is true and theere is input on
    // on the horizontal axis.

    // So for the sparkRed/Blue and boltRed/Blue emitters, the emission needs to happen when players collide,
    // so whenever the screen flashes red or blue and time is slowed down.

    // For landingRed/Blue, the frame grounded variable returns true, Emit(3), so basically, in the code
    // whenver the boolean that checks whether the player is grounded switches from false to true
    // (meaning the player has just landed on the ground) Emit landingRed/Blue

    // For smokeRed/Blue, once the opposite player has greater than 1 point, start emitting smoke puffs,
    // starting at 1 and eventually going up to Emit(4) once the opponent has 4 points, to reflect the damage
    // done to the player. This could go in Update

    // dashRed/Blue is tricky. I'm not sure if this will work, but try this: in the Player script whenever
    // the momentum is applied to the player after the button is pressed, so basically:
    // Button is pressed-
    // Momentum is applied-
    // dashRed/Blue.Emit(10)
    // ^ The above order should work. Let me know if it doesn't, then I can try and help you fix it.

    // For when the player is damaged, emits a puff of smoke
    public ParticleSystem smokeRed;
	public ParticleSystem smokeBlue;
	
	// On hit, emits a number of springs, bolts, etc.
	public ParticleSystem boltRed;
	public ParticleSystem boltBlue;
	
	// On hit, emits a burst of electrical sparks
	public ParticleSystem sparkRed;
	public ParticleSystem sparkBlue;
	
	// Quick puff of smoke when landing on the ground
	public ParticleSystem landingRed;
	public ParticleSystem landingBlue;
	
	// When moving on the ground, emits a puff of smoke
	public ParticleSystem movingRed;
	public ParticleSystem movingBlue;
	
	// The dash effect for the players, similar to Celeste
	public ParticleSystem dashRed;
	public ParticleSystem dashBlue;
	
	// Update is called once per frame
}
