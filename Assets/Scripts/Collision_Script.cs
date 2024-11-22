using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision_Script : MonoBehaviour
{
    public AudioSource gamemusic;
    public AudioSource keyaudio;
    public AudioSource needkeyaudio;
    public GameObject NeedKeyText;

    public bool keycollected = false;
    public bool snowballcollected = false;

    public GameObject snowballPrefab;
    public float throwForce = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ThrowSnowball();
        }
    }

    void ThrowSnowball()
    {
        if (snowballcollected == true)
        {
            Transform cameraTransform = Camera.main.transform;
            GameObject snowball = Instantiate(snowballPrefab, cameraTransform.position + cameraTransform.forward, Quaternion.identity);

            Rigidbody x = snowball.GetComponent<Rigidbody>();
            if (x != null)
            {
                x.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);
            }
            snowballcollected = false;
        }
    }

        public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "key")
        {
            Destroy(other.gameObject);
            keycollected = true;
            keyaudio.Play();
        }

        if(other.gameObject.tag == "door" && keycollected == true)
        {
            other.gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            Invoke("WinScene",1f);
        }

        if (other.gameObject.tag == "door" && keycollected == false)
        {
            NeedKeyText.SetActive(true);
            needkeyaudio.Play();
            StartCoroutine(NeedKeyTextBreak());
        }

        if (other.gameObject.tag == "snowball")
        {
            Destroy(other.gameObject);
            snowballcollected = true;
        }

        if (other.gameObject.tag == "ghost")
        {
            SceneManager.LoadScene(3);
        }
    }


    public void WinScene()
    {
        SceneManager.LoadScene(2);
    }

    IEnumerator NeedKeyTextBreak()
    {
        yield return new WaitForSeconds(3);
        NeedKeyText.SetActive(false);
    }
}
