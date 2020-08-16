using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{

    public AutoType typer;

    public IEnumerator TypeAnimation(string message) {
        yield return typer.TypeText(message);
    }

    public void ClearLogText() {
        this.typer.ClearText();
    }
}
