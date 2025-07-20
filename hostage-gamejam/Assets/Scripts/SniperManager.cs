using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperManager : MonoBehaviour
{
    public List<Room> roomsToTarget = new List<Room>();
    public Sniper sniper;  // Assign in inspector
    

    private void Start()
    {
        StartCoroutine(SniperRoutine());
    }

    private IEnumerator SniperRoutine()
    {
        while (true)
        {
            float escalation = EscalationManager.Instance.currentEscalation;

            // Determine time between room switches based on escalation
            float delay = Mathf.Lerp(10f, 3f, escalation / EscalationManager.Instance.maxEscalation);

            // Pick a random room
            Room targetRoom = roomsToTarget[Random.Range(0, roomsToTarget.Count)];

            sniper.SetTargetRoom(targetRoom);

            yield return new WaitForSeconds(delay);
        }
    }
}
