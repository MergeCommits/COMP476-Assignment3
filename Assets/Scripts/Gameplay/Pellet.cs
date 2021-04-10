using Photon.Pun;
using UnityEngine;

public class Pellet : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Wd");
        if (PhotonNetwork.IsMasterClient) {
            if (other.CompareTag("Player")) {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}