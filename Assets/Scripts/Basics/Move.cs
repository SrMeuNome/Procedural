using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float SpeedBase { get; set; }
    private static float Speed { get; set; }
    private static string MoveXRight { get; set; }
    private static string MoveXLeft { get; set; }

    public static string MoveSpeed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SpeedBase = 5;
        Speed = SpeedBase;
        MoveXRight = "d";
        MoveXLeft = "a";
        MoveSpeed = "left shift";
    }

    // Update is called once per frame
    void Update()
    {
        //Mover para direita
        if (Input.GetKey(MoveXRight))
        {
            transform.Translate(new Vector2(Speed * Time.smoothDeltaTime, 0));
            if  (transform.localScale.x < 0) transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        //Mover para esquerda
        if (Input.GetKey(MoveXLeft))
        {
            transform.Translate(new Vector2(-(Speed * Time.smoothDeltaTime), 0));
            if (transform.localScale.x > 0) transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        //Correr
        if (Input.GetKeyDown(MoveSpeed))
        {
            Speed = Speed + (Speed * 0.5f);
        }
        
        //Parar de Correr
        if (Input.GetKeyUp(MoveSpeed))
        {
            Speed = SpeedBase;
        }

    }
}
