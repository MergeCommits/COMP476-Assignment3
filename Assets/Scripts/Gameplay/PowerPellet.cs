using Photon.Pun;
using UnityEngine;

public class PowerPellet : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!PhotonNetwork.IsMasterClient) { return; }
        if (!other.CompareTag("Player")) { return; }

        PacMan player = other.GetComponent<PacMan>();
        player.AtePowerPellet();
            
        PhotonNetwork.Destroy(this.gameObject);
    }
}
