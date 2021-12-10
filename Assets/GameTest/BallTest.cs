using GameTest;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallTest : MonoBehaviour {
    public static Transform SpawnPoint;
    
    void OnEnable() {
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(Random.Range(-10f, 10f), Random.Range(8, 13), Random.Range(10f, -10f));
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.name.Equals("Cube")) {
            GameMain.Event.Fire(gameObject, BallDropDownEventArgs.Create());   
        }
    }
}
