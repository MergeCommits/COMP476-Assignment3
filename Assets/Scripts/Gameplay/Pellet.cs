using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Pellet : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!PhotonNetwork.IsMasterClient) { return; }

        if (other.CompareTag("Player")) {
            PacMan player = other.GetComponent<PacMan>();
            player.AtePellet();
            
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
