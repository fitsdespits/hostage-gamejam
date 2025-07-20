using System.Collections;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public float aimTime = 2f;  // time aiming before shooting
    private Room currentRoom;
    private Pawn targetPawn;

    private Coroutine aimCoroutine;

    public void SetTargetRoom(Room room)
    {
        if (aimCoroutine != null) StopCoroutine(aimCoroutine);

        currentRoom = room;
        transform.position = GetSniperPositionForRoom(room);

        aimCoroutine = StartCoroutine(AimAndShoot());
    }

    private Vector3 GetSniperPositionForRoom(Room room)
    {
        // Place sniper visually appropriate (edge of screen, above room, etc.)
        return room.transform.position + new Vector3(0, 5f, 0);
    }

    private IEnumerator AimAndShoot()
    {
        while (currentRoom != null)
        {
            if (currentRoom.pawnsInRoom.Count > 0)
            {
                targetPawn = currentRoom.pawnsInRoom[Random.Range(0, currentRoom.pawnsInRoom.Count)];

                Debug.Log($"Sniper targeting {targetPawn.name} in {currentRoom.roomName}");

                yield return new WaitForSeconds(aimTime);

                // Check if pawn is still in the room
                if (currentRoom.pawnsInRoom.Contains(targetPawn))
                {
                    KillPawn(targetPawn);
                }
            }

            yield return null;
        }
    }

    private void KillPawn(Pawn pawn)
    {
        Debug.Log($"Sniper shot {pawn.name}!");
        currentRoom.pawnsInRoom.Remove(pawn);
        Destroy(pawn.gameObject);

        // You could also increase escalation here if desired:
        // EscalationManager.Instance.IncreaseEscalation(5f);
    }
}