using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using Photon;
using Photon.Pun;

public class NetPlayer : TestPUN {
    public int netPlayerID;
    public Camera playerCam;

    void FixedUpdate() {
        // RaycastHit hit;
        // Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        //
        // if (InputManager.mouse.mouse1.isHit) {
        //     if (Physics.Raycast(ray, out hit)) {
        //         if (hit.transform.name == "BilliardSphere(Clone)" && hit.transform.GetComponent<PhotonView>().IsMine) {
        //             hit.transform.gameObject.GetComponent<Rigidbody>()
        //                 .AddForceAtPosition(-hit.normal * 1000.0f, hit.point);
        //         }
        //
        //         // Do something with the object that was hit by the raycast.
        //     }
        // }
    }

    protected override void OnProperJoin() {
        GameObject playerBall =
            PhotonNetwork.Instantiate("Cylinder", new Vector3(0f, 1f, 0f), Quaternion.identity, 0);

        netPlayerID = PhotonNetwork.PlayerList.Length;
        Debug.Log(netPlayerID);
        // if (netPlayerID == 1) {
        //     for (int i = 0; i < 100; ++i) {
        //         GameObject newCube = PhotonNetwork.Instantiate("Cube",
        //             new Vector3(-32.5f + ((i % 10) * 7.5f), 0, 8.7f + 32.5f - ((i / 10) * 7.5f)), Quaternion.identity,
        //             0);
        //
        //         //newCube.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //         newCube.name = newCube.name + i;
        //     }
        // }
    }
}
