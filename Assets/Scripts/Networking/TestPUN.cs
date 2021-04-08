using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPUN : MonoBehaviourPunCallbacks {
    public void Connect() {
        Debug.Log("Connecting to name server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected() {
        Debug.Log("==CONNECTED TO NAME SERVER==");
    }

    public override void OnConnectedToMaster() {
        Debug.Log("==CONNECTED TO MASTER==");
        JoinRandomRoom();
    }

    public void CreateRoom() {
        Debug.Log("Creating room...");
        PhotonNetwork.CreateRoom("uh");
    }

    public override void OnCreatedRoom() {
        Debug.Log("==CREATED ROOM==");
    }

    public void JoinRandomRoom() {
        Debug.Log("Joining room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom() {
        Debug.Log("==JOINED ROOM==");
        OnProperJoin();
    }

    protected virtual void OnProperJoin() { }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log($"return code: {returnCode}, message: {message}");
        Debug.Log("==Could not join a random room. Attempted to create a room instead==");
        CreateRoom();
    }

    private void Start() {
        Connect();
    }
}
