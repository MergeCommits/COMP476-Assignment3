using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PacMan : MonoBehaviour {
    private PhotonView photonView;

    private LayerMask collision;

    private Vector2 movePosition;
    private float moveSpeed = 8f;
    
    private int score;
    private Text scoreText;
    
    private const float POWER_PELLET_TIME = 5f;
    private const float SPEED_BOOST = 3f;
    private float powerPelletTimeRemaining;
    
    private void Awake() {
        photonView = GetComponent<PhotonView>();
        
        collision = LayerMask.GetMask("Wall");

        movePosition = transform.position.XZ();

        scoreText = GameObject.Find("/Canvas/Text").GetComponent<Text>();
    }

    void FixedUpdate() {
        if (photonView.IsMine) {
            float speed = 1.5f;
            if (powerPelletTimeRemaining > 0f) {
                powerPelletTimeRemaining -= Time.deltaTime;
                speed *= SPEED_BOOST;
            } 
            
            Vector2 direction = new Vector2(InputManager.movement.axisDelta.x, InputManager.movement.axisDelta.y);
            direction = Vector2.ClampMagnitude(direction, 1.0f);

            Vector2 displacement = direction * (speed * Time.deltaTime);
            transform.Translate(displacement.x, 0f, displacement.y);
            
            UpdateMovement();
        }
    }

    private void UpdateMovement() {
        transform.position = Vector3.MoveTowards(transform.position, movePosition.ToXZ(), moveSpeed * Time.deltaTime);

        if (!(Vector2.Distance(transform.position.XZ(), movePosition) <= 0.05f)) {
            return;
        }

        if (!Mathf.Approximately(InputManager.movement.axisDelta.x, 0f)) {
            AttemptMove(new Vector2(InputManager.movement.axisDelta.x, 0f));
        } else if (!Mathf.Approximately(InputManager.movement.axisDelta.y, 0f)) {
            AttemptMove(new Vector2(0f, InputManager.movement.axisDelta.y));
        }
    }

    private void AttemptMove(Vector2 offset) {
        Vector2 attemptPosition = movePosition + offset;
        if (SafeToMoveTowards(attemptPosition)) {
            movePosition = attemptPosition;
        }
    }

    private bool SafeToMoveTowards(Vector2 newPosition) {
        Collider[] results = new Collider[1];
        Physics.OverlapSphereNonAlloc(newPosition.ToXZ(), 0.4f, results, collision.value);
            
        return results[0] == null;
    }

    [PunRPC]
    public void AtePowerPellet() {
        if (photonView.IsMine) {
            powerPelletTimeRemaining = POWER_PELLET_TIME;
        } else {
            photonView.RPC("AtePowerPellet", RpcTarget.Others);
        }
    }

    public void TouchedGhost() {
        if (!photonView.IsMine) { return; }

        Vector2 newPos = new Vector2(0f, -4f);
        transform.position = newPos.ToXZ();
        movePosition = newPos;
    }

    public void AtePellet() {
        photonView.RPC("UpdatePelletCount", RpcTarget.All);
    }

    [PunRPC]
    private void UpdatePelletCount() {
        score++;
        if (photonView.IsMine) {
            scoreText.text = "Score: " + score;
        }

        if (PhotonNetwork.IsMasterClient) {
            CheckIfAllPelletsEaten();
        }
    }

    private void CheckIfAllPelletsEaten() {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
        if (pellets.Length < 2) { // Last one's still alive at this moment.
            FigureOutWinner();
        }
    }
    
    private static void FigureOutWinner() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        PacMan winner = players
            .Select(go => go.GetComponent<PacMan>())
            .OrderByDescending(p => p.score)
            .First();
        
        winner.Winner();
        foreach (GameObject loser in players) {
            PacMan pac = loser.GetComponent<PacMan>();
            if (pac == winner) { continue; }
            
            pac.Loser();
        }
    }

    [PunRPC]
    private void Winner() {
        if (photonView.IsMine) {
            scoreText.alignment = TextAnchor.MiddleCenter;
            scoreText.text = "You Win";
        } else {
            photonView.RPC("Winner", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void Loser() {
        if (photonView.IsMine) {
            scoreText.alignment = TextAnchor.MiddleCenter;
            scoreText.text = "You Lose";
        } else {
            photonView.RPC("Loser", RpcTarget.Others);
        }
    }

    public void Teleport(Vector2 newPosition) {
        if (!photonView.IsMine) { return; }
        
        Vector3 position = transform.position;
        
        position = (position.XZ() + newPosition).ToXZ();
        transform.position = position;
        movePosition = position.XZ();
    }
}
