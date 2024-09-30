using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collidermeja : MonoBehaviour
{
    void Start()
    {
        // Menambahkan BoxCollider jika meja tidak memiliki collider
        if (GetComponent<Collider>() == null)
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        }
    }
}
