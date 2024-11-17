using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Script : MonoBehaviour
{
    public bool ghoststopped = false;

    public GameObject player;
    public float speed = 0.4f;
    public float rotationspeed = 1f;

    void Update()
    {
        GhostMovement();
    }

    public void GhostMovement()
    {
        if (ghoststopped == false)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationspeed);

            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "snowball")
        {
            Destroy(other.gameObject);
            GhostStop();
        }
    }

    public void GhostStop()
    {
        ghoststopped = true;
        StartCoroutine(GhostStopBreak());
    }

    IEnumerator GhostStopBreak()
    {
        yield return new WaitForSeconds(5);
        ghoststopped = false;
    }
}
