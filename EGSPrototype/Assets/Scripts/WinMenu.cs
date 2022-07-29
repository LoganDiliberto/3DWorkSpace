using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
	public Transform buttonLocationL;
	public Transform buttonLocationM;
	public Transform buttonLocationR;
	public GameObject[] buffButtons;
	public GameObject player;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	void Awake(){
		selectRandomBuffOptions();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void selectRandomBuffOptions(){
		GameObject buffButton1;
		GameObject buffButton2;
		GameObject buffButton3;
		int buttonSelection;
		ArrayList selectedButtons = new ArrayList();

		while(selectedButtons.Count < 3){
			buttonSelection = Random.Range(0, buffButtons.Length);
			if (!selectedButtons.Contains(buttonSelection)){
				selectedButtons.Add(buttonSelection);
			}
		}	

		buffButton1 = Instantiate<GameObject>(buffButtons[(int) selectedButtons[0]].gameObject, buttonLocationL);
		buffButton1.transform.position = buttonLocationL.position;
		buffButton1.GetComponent<BuffManager>().player = player;
		
		buffButton2 = Instantiate<GameObject>(buffButtons[(int) selectedButtons[1]].gameObject, buttonLocationM);
		buffButton2.transform.position = buttonLocationM.position;
		buffButton2.GetComponent<BuffManager>().player = player;
		
		buffButton3 = Instantiate<GameObject>(buffButtons[(int) selectedButtons[2]].gameObject, buttonLocationR);
		buffButton3.transform.position = buttonLocationR.position;
		buffButton3.GetComponent<BuffManager>().player = player;
	}
}
