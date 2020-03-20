using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Loading : MonoBehaviour {

	public GameObject image;

	private Vector3 scale;
	private float relation = 0.1f;
	private float startTime = 0.0f;
	private float countTime = 0.0f;

	// Use this for initialization
	void Start () {
		image = gameObject.transform.Find ("Image").gameObject;
		scale = new Vector3 (1.0f, 1.0f, 1.0f);
		relation = 0.1f;
		startTime = Time.time;
		countTime = Time.time;

		LogView.setViewText ("UI_Loading,Start,image=="+image);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - countTime >= 0.1f){
			countTime = Time.time;
			relation = relation + 0.4f;
			if(relation>5.0f){
				relation = 0.1f;
			}
			//LogView.setViewText ("UI_Loading,Update,relation=="+relation);

			image.transform.localScale = scale * relation;
			
		}

		if(Time.time - startTime > 10f){
			UIManager.CloseUI ("UI_Loading");
		}

	}
}
