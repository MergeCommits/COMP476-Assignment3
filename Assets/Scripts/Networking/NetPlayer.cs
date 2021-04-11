using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class NetPlayer : TestPUN {
    public int netPlayerID;

    protected override void OnProperJoin() {
        PhotonNetwork.Instantiate("Player", new Vector3(0f, 0f, 2f), Quaternion.identity);

        netPlayerID = PhotonNetwork.PlayerList.Length;
        Debug.Log(netPlayerID);

        IEnumerable<Vector2> pelletPositions = ClearPelletsAndReturnPositions();
        
        if (netPlayerID == 1) {
            GeneratePellets(pelletPositions);
        }
    }

    private static IEnumerable<Vector2> ClearPelletsAndReturnPositions() {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
        Vector2[] result = new Vector2[pellets.Length];
        for (int i = 0; i < pellets.Length; i++) {
            Vector2 coord = pellets[i].transform.position.XZ();
            Destroy(pellets[i].gameObject);
            result[i] = coord;
        }

        return result;
    }

    private static void GeneratePellets(IEnumerable<Vector2> positions) {
        foreach (Vector2 coord in positions) {
            PhotonNetwork.Instantiate("Pellet", coord.ToXZ(), Quaternion.identity);
        }
    }
}
