using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour {
    private NavMeshAgent navMeshAgent;
    private PhotonView photonView;
    
    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        photonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate() {
        if (!photonView.IsMine) { return; }

        UpdateAgentTarget();
    }

    private void UpdateAgentTarget() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Vector2 myDistanceXZ = transform.position.XZ();

        if (players.Length > 0) {
            Vector2 closestPlayer = players
                .Select(p => p.transform.position.XZ())
                .OrderBy(d => Vector2.Distance(myDistanceXZ, d))
                .First();

            navMeshAgent.destination = closestPlayer.ToXZ();
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) { return; }

        PacMan player = other.GetComponent<PacMan>();
        player.TouchedGhost();
    }
}
