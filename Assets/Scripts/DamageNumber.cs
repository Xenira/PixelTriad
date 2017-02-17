using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour {

    public Sprite[] numbers;
	
	public void SetNumber(int number)
    {
        GetComponent<SpriteRenderer>().sprite = numbers[Mathf.Clamp(number, 1, numbers.Length) - 1];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(1.5f, 2.5f, 0), new Vector3(3, 5, 0));
    }
}
