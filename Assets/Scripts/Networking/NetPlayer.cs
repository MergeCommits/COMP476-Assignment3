using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class NetPlayer : TestPUN {
    public int netPlayerID;

    protected override void OnProperJoin() {
        netPlayerID = PhotonNetwork.PlayerList.Length;
        Debug.Log(netPlayerID);

        IEnumerable<Vector2> pelletPositions = ClearPelletsAndReturnPositions();
        IEnumerable<Vector2> powerPelletPositions = ClearPowerPelletsAndReturnPositions();
        
        PhotonNetwork.Instantiate("Player", new Vector3(0f, 0f, -4f), Quaternion.identity);
        
        if (netPlayerID == 1) {
            GeneratePellets(pelletPositions, powerPelletPositions);
            StartCoroutine(GenerateGhosts());
        }
    }

    private static IEnumerable<Vector2> ClearPelletsAndReturnPositions() {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
        Vector2[] result = new Vector2[pellets.Length];
        for (int i = 0; i < pellets.Length; i++) {
            Vector2 coord = pellets[i].transform.position.XZ();
            DestroyImmediate(pellets[i].gameObject);
            result[i] = coord;
        }

        return result;
    }

    private static IEnumerable<Vector2> ClearPowerPelletsAndReturnPositions() {
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("PowerPellet");
        Vector2[] result = new Vector2[pellets.Length];
        for (int i = 0; i < pellets.Length; i++) {
            Vector2 coord = pellets[i].transform.position.XZ();
            DestroyImmediate(pellets[i].gameObject);
            result[i] = coord;
        }

        return result;
    }

    private static void GeneratePellets(IEnumerable<Vector2> positions, IEnumerable<Vector2> powerPelletPositions) {
        foreach (Vector2 coord in positions) {
            PhotonNetwork.Instantiate("Pellet", coord.ToXZ(), Quaternion.identity);
        }

        foreach (Vector2 powerPelletPosition in powerPelletPositions) {
            PhotonNetwork.Instantiate("PowerPellet", powerPelletPosition.ToXZ(), Quaternion.identity);
        }
    }

    private static IEnumerator GenerateGhosts() {
        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < 2; i++) {
            PhotonNetwork.Instantiate("Ghost", Vector3.zero, Quaternion.identity);
        }
    }
}
