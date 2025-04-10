using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFinishLevel : MonoBehaviour
{
    public bool useCheckpoint = true;  // Pokud je TRUE, hráč se respawne na posledním checkpointu

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerRespawn playerRespawn = collision.GetComponent<PlayerRespawn>();

            if (playerRespawn != null)
            {
                playerRespawn.Respawn(useCheckpoint);
            }
        }
    }
}
