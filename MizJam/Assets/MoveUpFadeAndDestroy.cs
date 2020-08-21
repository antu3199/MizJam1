using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpFadeAndDestroy : MonoBehaviour
{
    public CanvasGroup content;
    public float timeToDisappear = 1f;

    private float timeCounter = 0;

    public float moveUpSpeed = 0.4f;

    public bool useVariance = false;
    public float variance = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        if (useVariance) {
            float varianceX = Random.Range(-variance, variance);
            float varianceY = Random.Range(-variance, variance);

            this.gameObject.transform.position = new Vector3(this.transform.position.x + varianceX, this.transform.position.y + varianceY, this.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, moveUpSpeed * Time.deltaTime, 0); 

        timeCounter += Time.deltaTime;

        float t = timeCounter / timeToDisappear;
        content.alpha = 1.0f-t;

        if (timeCounter >= timeToDisappear) {
            this.timeToDisappear = 0;
            Destroy(this.gameObject);
        }
    }
}
