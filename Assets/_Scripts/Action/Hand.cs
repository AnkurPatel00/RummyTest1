using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	public float uptime;
	public float speed;
	private float timer;
	private bool goingUp;
	private bool goingDown;
	// Use this for initialization
	void Start () {
		goingUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer < uptime && goingUp)
		{
			timer+=Time.deltaTime*speed;
			transform.position+=new Vector3(0,Time.deltaTime*speed,0);
		}else{
			if(!goingDown){
				timer = uptime;
				goingUp = false;
				goingDown = true;
			}

		}

		if(goingDown && timer > 0){
			timer-=Time.deltaTime*speed;
			transform.position-=new Vector3(0,Time.deltaTime*speed,0);
		}
		else{
			if(!goingUp){
				timer = 0;
				goingDown = false;
				goingUp = true;
			}
		}
	}
}
