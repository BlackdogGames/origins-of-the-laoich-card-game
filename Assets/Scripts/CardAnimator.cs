using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponentInParent<GridLayoutGroup>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateCardAttack()
    {
        StartCoroutine(AttackAnim());
    }
    public void AnimateCardHit()
    {
        StartCoroutine(Vibrate());
    }
    
    //coroutine
    public IEnumerator AttackAnim()
    {
        //disable parent object grid layout group component
        GetComponentInParent<GridLayoutGroup>().enabled = false;

        int direction = GetComponent<CardStats>().BelongsToLocalPlayer ? 1 : -1;

        float timer = 0;

        //move card upwards at an accelerating speed
        float speed = 0.01f;
        float acceleration = 8.0f;
        float currentY = transform.position.y;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            currentY += speed * Time.deltaTime;
            speed += acceleration * Time.deltaTime * direction;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            yield return null;
        }
        
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            currentY -= speed * Time.deltaTime;
            speed -= acceleration * Time.deltaTime * direction;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            yield return null;
        }

        //enable parent object grid layout group component
        GetComponentInParent<GridLayoutGroup>().enabled = true;
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
    }

    //couroutine that makes object vibrate
    public IEnumerator Vibrate()
    {
        //disable parent object grid layout group component
        GetComponentInParent<GridLayoutGroup>().enabled = false;

        Vector3 origin = transform.position;

        //randomly vibrate object

        //vibrate object
        float timer = 0;
        while (timer < .5f)
        {
            float randomX = Random.Range(-0.01f, 0.01f);
            float randomY = Random.Range(-0.01f, 0.01f);
            float randomZ = Random.Range(-0.01f, 0.01f);
            Vector3 randomVector = new Vector3(randomX, randomY, randomZ);
            
            timer += Time.deltaTime;
            transform.position = origin + randomVector;
            yield return null;
        }
        
        //enable parent object grid layout group component
        GetComponentInParent<GridLayoutGroup>().enabled = true;
        transform.position += transform.position;
    }
}