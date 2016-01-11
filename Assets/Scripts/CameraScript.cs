using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Transform player;
    
    void Update () {
        transform.position = new Vector3(player.position.x, player.position.y, -1);  // makes camera follow player
    }
}
