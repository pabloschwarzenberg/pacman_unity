using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fantasma_behaviour : MonoBehaviour
{
    public float speed=0.25f;
    private GameObject pacman;

    void Start()
    {
        pacman=GameObject.Find("pacman");
        InvokeRepeating("perseguir", 0.5f, 0.5f);
    }

    bool hayColision(Vector2 direccion)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector2 posicion = transform.position;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(posicion, direccion, 5, layerMask);
        if(hit.collider!=null)
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
            destino = posicion + direccion*5;
            destino.x = Mathf.Round(destino.x);
            destino.y = Mathf.Round(destino.y);
            GetComponent<Rigidbody2D>().MovePosition(destino);
        }
    }

    void Update()
    {
    }

    void perseguir()
    {
        int i;
        float menor_distancia=100000.0f;
        float distancia=100000.0f;
        int posicion=-1;

        if (transform.position == pacman.transform.position)
            return;

        Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        for (i=0;i<direcciones.Length;i++)
        {
            distancia=calcularDistancia(direcciones[i]);
            if(distancia<menor_distancia)
            {
                menor_distancia = distancia;
                posicion = i;
            }
        }
        Debug.Log($"Elección {direcciones[posicion].x} {direcciones[posicion].y} {menor_distancia}");
        avanzar(direcciones[posicion]);
    }

    float calcularDistancia(Vector2 direccion)
    {
        float distancia = 100000.0f;
        Vector2 posicion_pacman;
        Vector2 destino;
        if (!hayColision(direccion))
        {
            posicion_pacman = (Vector2)pacman.transform.position;
            destino = (Vector2)transform.position + direccion*5;
            distancia = Vector2.Distance(posicion_pacman,destino);
        }
        return distancia;
    }

    public void volverInicio()
    {
        GetComponent<Rigidbody2D>().MovePosition(new Vector2(5, 25));
    }
}