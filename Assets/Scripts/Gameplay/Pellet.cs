using Photon.Pun;
using UnityEngine;

public class Pellet : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!PhotonNetwork.IsMasterClient) { return; }
        if (!other.CompareTag("Player")) { return; }

        PacMan player = other.GetComponent<PacMan>();
        player.AtePellet();
            
        PhotonNetwork.Destroy(this.gameObject);
    }
}
