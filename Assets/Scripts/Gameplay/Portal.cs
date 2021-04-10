using Photon.Pun;
using UnityEngine;

public class Portal : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PacMan player = other.GetComponent<PacMan>();
            if (transform.position.x < 0f) {
                player.Teleport(new Vector2(26f, 0f));
            } else {
                player.Teleport(new Vector2(-26f, 0f));
            }
        }
    }
}
