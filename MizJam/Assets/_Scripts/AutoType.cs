using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoType : MonoBehaviour {
	public float letterPause = 0.01f;
	public float longerPause = 0.2f;
    public float afterDelay = 1f;

	public AudioClip sound;
	public Text guiText;

	float thisTime;

	// Use this for initialization
    /*
	void Start () {
		message = guiText.text;
		guiText.text = "";
		StartCoroutine(TypeText ()); //Same as new Thread
	}
	void Update (){
		if (Input.anyKey) {
			guiText.text = message;
			StopCoroutine(TypeText());
			BreakLoop = true;
		}
	}
    */
	
    public void ClearText() {
        guiText.text = "";
    }

	public IEnumerator TypeText (string message) {
        this.ClearText();
		thisTime = (Time.time);
	
		foreach (char letter in message.ToCharArray()) {
			//Advances text
			guiText.text += letter;

			//Limit sound frequency and play sound
			if (Time.time > thisTime + 0.1f){
				thisTime = Time.time;
                GameManager.Instance.audio.PlayOneShot(this.sound);
			}

			//If certain punctuation, pause longer
			if (letter.ToString() == "." ||letter.ToString() == "," || letter.ToString() =="!" ||letter.ToString() == "?"){
				yield return new WaitForSeconds (longerPause);
			}
			else{
				yield return new WaitForSeconds (letterPause);
			}

		}

        yield return new WaitForSeconds(afterDelay);
	}
}
