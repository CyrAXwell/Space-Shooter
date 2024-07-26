using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHp : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private int addHp = 20;
    


    // Update is called once per frame
    // void Update()
    // {
    //     if(Input.GetKeyDown("h"))
    //     {
    //         player.GetComponent<Player>().ChangeHp(addHp);
    //     }
    // }
}
