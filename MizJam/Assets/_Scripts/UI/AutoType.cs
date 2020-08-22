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
    private char[] messageArray;

    public bool isTyping = false;

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

    private int messageIndex = 0;
	
    public void ClearText() {
        guiText.text = "";
    }

    public void InterruptWithAnotherMessage(string message) {
        this.ClearText();
        this.messageArray = message.ToCharArray();
        this.messageIndex = 0;
    }

	public IEnumerator TypeText (string message) {
        this.ClearText();
        
		thisTime = (Time.time);
        this.isTyping = true;
        this.messageIndex = 0;
        this.messageArray = message.ToCharArray();

        while (this.messageIndex < this.messageArray.Length) {
            char letter = messageArray[messageIndex];

			//Advances text
			guiText.text += letter;

			//Limit sound frequency and play sound
			if (Time.time > thisTime + 0.1f){
				thisTime = Time.time;
                GameManager.Instance.audio.PlayOneShot(this.sound);
			}

            messageIndex++;

			//If certain punctuation, pause longer
			if (letter.ToString() == "." ||letter.ToString() == "," || letter.ToString() =="!" ||letter.ToString() == "?"){
				yield return new WaitForSeconds (longerPause);
			}
			else{
				yield return new WaitForSeconds (letterPause);
			}


		}

        this.isTyping = false;
        yield return new WaitForSeconds(afterDelay);
	}
}
