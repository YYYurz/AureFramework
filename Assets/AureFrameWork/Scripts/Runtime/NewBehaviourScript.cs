using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    private float updateCount;
    private float fixedUpdateCount;

    // Update is called once per frame
    void Update() {
        updateCount += Time.deltaTime;
        if (updateCount >= 1f) {
            Debug.Log($"Update  {Time.deltaTime}");
            updateCount = 0f;
        }
    }

    private void FixedUpdate() {
        fixedUpdateCount += Time.deltaTime;
        if (fixedUpdateCount >= 1f) {
            Debug.Log($"Fixed  {Time.deltaTime}");
            fixedUpdateCount = 0f;
        }
    }
}
