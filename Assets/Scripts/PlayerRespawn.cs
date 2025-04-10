using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint;   // Výchozí respawn (např. začátek levelu)
    public Transform[] checkpoints;  // Seznam všech checkpointů v levelu
    private int lastCheckpointIndex = -1; // Index posledního aktivovaného checkpointu (-1 = žádný aktivní)

    private void Start()
    {
        if (respawnPoint == null)
            respawnPoint = transform; // Pokud není nastaveno, použije startovní pozici
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (collision.transform == checkpoints[i])
            {
                lastCheckpointIndex = i;
                break;
            }
        }
    }

    public void Respawn(bool useCheckpoint)
    {
        if (useCheckpoint && lastCheckpointIndex >= 0)
        {
            transform.position = checkpoints[lastCheckpointIndex].position;
        }
        else
        {
            transform.position = respawnPoint.position;
        }
    }
}
