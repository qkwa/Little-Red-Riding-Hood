using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
{
    [SerializeField] private float stunTime;
    [SerializeField] private HeadhogSpikes headhogSpikes;
    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        headhogSpikes = GetComponent<HeadhogSpikes>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStun()
    {
        headhogSpikes.enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        animator.SetBool("IsStun", true);
        StartCoroutine(Stuning(stunTime));
    }

    IEnumerator Stuning(float time)
    {
        yield return new WaitForSeconds(time);
        OffStun();
    }

    public void OffStun()
    {
        headhogSpikes.enabled = true;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        animator.SetBool("IsStun", false);
    }

}
