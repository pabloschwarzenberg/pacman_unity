using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pacman_behaviour : MonoBehaviour
{
    enum Estado
    {
        Activo,
        Atrapado
    };

    private Estado estado;

    public int speed = 1;
    public int score = 0;
    public Text tx_score;

    void Start()
    {
        estado=Estado.Activo;
    }

    public void volverInicio()
    {
        GetComponent<Rigidbody2D>().MovePosition(new Vector2(5,-15));
        GetComponent<Rigidbody2D>().MoveRotation(0);
        estado =Estado.Atrapado;
    }

    bool hayColision(Vector2 direccion)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector2 posicion = transform.position;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(posicion, direccion, 5, layerMask);
        if (hit.collider != null)
            return true;
        else
            return false;
    }

    void avanzar(Vector2 direccion)
    {
        Vector2 destino;
        Vector2 posicion = transform.position;
        if (!hayColision(direccion))
        {
            destino = posicion + direccion*speed;
            destino.x = Mathf.Round(destino.x);
            destino.y = Mathf.Round(destino.y);
            GetComponent<Rigidbody2D>().MovePosition(destino);
        }
    }

    void UpdateHUD()
    {
        if(tx_score!=null)
        {
            tx_score.text = "" + score;
        }
    }

    void Update()
    {
        UpdateHUD();

        if(estado==Estado.Atrapado)
        {
            if(Mathf.Round(transform.position.x)==5 && Mathf.Round(transform.position.y)==-15)
                estado=Estado.Activo;
            else
                return;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody2D>().MoveRotation(-90);
            avanzar(new Vector2(0,1)); 
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().MoveRotation(180);
            avanzar(new Vector2(1,0)); 
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Rigidbody2D>().MoveRotation(90);
            avanzar(new Vector2(0,-1)); 
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().MoveRotation(0);
            avanzar(new Vector2(-1,0));          
        }
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name == "fantasma")
        {
            co.GetComponent<fantasma_behaviour>().volverInicio();
            volverInicio();
        }
        if(co.name.StartsWith("comida"))
        {
            score = score + 10;
            Destroy(co.gameObject);
        }
    }
}