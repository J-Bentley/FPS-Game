using UnityEngine;
using System.Collections;

public class CamShake : MonoBehaviour {
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 orignalPosition = transform.localPosition;
        float elapsed = 0f;
        while(elapsed < duration) {
            float x = Random.Range(-0.5f, 0.5f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = orignalPosition;
    }
}
