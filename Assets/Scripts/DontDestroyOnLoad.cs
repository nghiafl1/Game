using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Giữ lại GameObject khi chuyển Scene
    }
}
