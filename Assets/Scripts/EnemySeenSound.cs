using UnityEngine;
using System.Collections;

public class EnemySeenSound : MonoBehaviour {

    public float resetTime = 3.0f;
    bool isPlaying = false;

    public void playSound()
    {
        if (isPlaying || GetComponent<AudioSource>().isPlaying)
            return;

        GetComponent<AudioSource>().Play();
        isPlaying = true;
        StartCoroutine(resetPlayTimer());
    }

    IEnumerator resetPlayTimer()
    {
        yield return new WaitForSeconds(resetTime);
        isPlaying = false;
    }
}
