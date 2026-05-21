using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedTreeScript : MonoBehaviour
{

    [SerializeField] private ParticleSystem fire;
    [SerializeField] private bool onFire;
    [SerializeField] private SphereCollider fireTrigger;
    private void OnTriggerEnter(Collider other)
    {
        //Geht irgendwie net??
        print("Trigger mit " + other.gameObject.name);
        if (!onFire) return;
        if (other.gameObject.GetComponent<TreeSystem>() != null)
        {
            print("Baum wurde angezündet");
            other.gameObject.GetComponent<TreeSystem>().Ignite();
        }
        if (other.gameObject.GetComponent<ChoppedTreeScript>() != null)
        {
            print("Toter Baum wurde angezündet");
            other.gameObject.GetComponent<ChoppedTreeScript>().Ignite();
        }
    }

    private void Awake()
    {
        fireTrigger = gameObject.GetComponent<SphereCollider>();
        fireTrigger.enabled = false;
        fire.Pause();
    }

    public void Ignite()
    {
        if (onFire) return;
        if (!fire.isPlaying)
        {
            onFire = true;
            fire.Play();
            fireTrigger.enabled = true;
        }
    }

    public void SetFireRange(float range)
    {
        fireTrigger.radius = range;
    }
}
