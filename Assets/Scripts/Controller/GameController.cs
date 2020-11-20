using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : GameManager
{
    public GameState state;

    public GameObject player;
    public GameObject enemy;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.PLAYING;
        StartCoroutine(SetupGame());
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null) cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    IEnumerator SetupGame()
    {
        cam = Camera.main;

        player = Instantiate(player, GameObject.Find("Player Spawn Point").transform.position, Quaternion.identity);
        Instantiate(enemy, GameObject.Find("Enemy Spawn Point").transform.position, Quaternion.identity);

        yield return null;
    }

    public enum GameState
    {
        PLAYING,
        SELECTING_ABILITY,
        ATTACKING,
        CHANGE_CHARACTER,
        PAUSED,
        BUSY
    }
}
