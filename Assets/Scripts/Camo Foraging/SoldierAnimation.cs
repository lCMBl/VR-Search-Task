using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamoForaging.Spawner;

[RequireComponent(typeof(Animator))]
public class SoldierAnimation : MonoBehaviour
{
    // animates the soldiers to fire at random intervals,
    //as well as getting shot, and dissapearing after
    public AudioSource rifleShot;
    public int CharacterID {
        get { return poolID; }
    }
    Animator animator;
    float nextShotTime, timer;
    private int poolID;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nextShotTime = Random.Range(1f, 3f);
        poolID = GetComponentInParent<SpawnController>().GetCharacterID(transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextShotTime) {
            timer = 0f;
            nextShotTime = Random.Range(1f, 3f);
            animator.SetTrigger("Fire");
            LSLEventRecorder.RecordCharacterAnimationEvent(CharacterID, "Fire");
            // rifleShot.Play();
        }
    }

    public void GotShot() {
        animator.SetTrigger("GotShot");
        GetComponentInParent<Collider>().enabled = false;
        LSLEventRecorder.RecordCharacterAnimationEvent(CharacterID, "GotShot");
    }
}
