using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LogMessage {
    public string message;
    public Sprite icon;

    public LogMessage(string message, Sprite icon) {
        this.message = message;
        this.icon = icon;
    }

}

public class LogController : MonoBehaviour
{

    public AutoType typer;
    public Image talkerIcon;

    public List<ActionItemUI> actionItems;

    public void TypeAnimationFunc(LogMessage message) {
        this.StartCoroutine(this.TypeAnimation(message));
    }

    public IEnumerator TypeAnimation(LogMessage message) {
        if (message.icon == null) {
            talkerIcon.gameObject.SetActive(false);
        } else {
            talkerIcon.sprite = message.icon;
            talkerIcon.gameObject.SetActive(true);
        }

        if (this.typer.isTyping) {
            typer.InterruptWithAnotherMessage(message.message);
        } else {
            yield return typer.TypeText(message.message);
        }

    }

    public void ClearLogText() {
        this.talkerIcon.gameObject.SetActive(false);
        this.typer.ClearText();
    }
}
