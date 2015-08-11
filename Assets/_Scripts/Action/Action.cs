using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

	Vector3 FirstScale;
	public bool PlayAnim = true;
	public bool ActionOnClick = false;
	public bool ActionOnHold = false;

	private bool clickOnce;

	public void Start(){
		FirstScale = transform.localScale;
	}

	void OnMouseDown () {

		if(PlayAnim){
			transform.localScale = FirstScale;
			Invoke("ScaleDown", 0.01f);
		}
		if(ActionOnClick){
			TackAction();
		}
	}
	
	void OnMouseDrag(){
		if(ActionOnHold){
			TackAction();
		}
	}
	
	void OnMouseUp () {
		if(PlayAnim){
			CancelInvoke("ScaleDown");
			Invoke("ScaleUp", 0.01f);
		}

	}

	void ScaleDown(){
		transform.localScale *= 0.97f;
		if(transform.localScale.magnitude>FirstScale.magnitude*0.95f){
			Invoke("ScaleDown", 0.01f);
		}
	}
	
	void ScaleUp(){
		transform.localScale *= 1.03f;
		if(transform.localScale.magnitude<FirstScale.magnitude*1.05f){
			Invoke("ScaleUp", 0.01f);	
		}else{
			Invoke("ScaleUpDown", 0.01f);
		}
	}
	void ScaleUpDown(){
		transform.localScale *= 0.985f;
		if(transform.localScale.magnitude>FirstScale.magnitude){
			Invoke("ScaleUpDown", 0.01f);
		}else{
			transform.localScale = FirstScale;
			if(!ActionOnClick)TackAction();
		}
	}
	
	public virtual void TackAction(){
		print("Action");
	}
}
