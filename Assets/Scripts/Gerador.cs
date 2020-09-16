using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gerador : MonoBehaviour
{
    //Tamanho do mapa
    public int Size;
    private int _size = 0;

    public int MaxAutRelevo;
    public int MaxLarRelevo;

    //Tilemap é onde fica localizado os Tile Base
    public Tilemap tileTerra;
    public Tilemap tileAgua;

    //TileBase é o tile que queremos utilizar
    public TileBase terraTopo;
    public TileBase terra;
    public TileBase aguaTopo;
    public TileBase agua;

    public enum TileType
    {
        Solo,
        Agua
    }

    // Start is called before the first frame update
    void Start()
    {
        while (_size <= Size)
        {
            TileType auxTipe = Random.Range(0, 10) < 5? TileType.Agua : TileType.Solo;

            TileBase baseAuxTopo;
            TileBase baseAuxinferior;

            if(auxTipe == TileType.Agua)
            {
                baseAuxTopo = aguaTopo;
                baseAuxinferior = agua;
            }
            else//if(auxTipe == TileType.Solo)
            {
                baseAuxTopo = terraTopo;
                baseAuxinferior = terra;
            }

            _size = GerarRelevo(Random.Range(0, MaxLarRelevo), Random.Range(0, MaxAutRelevo), _size, 0, baseAuxTopo, baseAuxinferior, auxTipe);
            
            if (_size > Size)
            {
                //tileTerra.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GerarRelevo(int w, int h, int x, int y, TileBase superficie, TileBase inferior, TileType tileType)
    {
        TileBase aux;
        for(int i = y; i < h+y; i++)
        {
            //Largura mais a posição em x
            for(int j = x; j < w+x; j++)
            {
                //Altura mais a posição em y
                if (i == h+y - 1) aux = superficie;
                else aux = inferior;

                //Tipo do objeto
                if(tileType == TileType.Solo) tileTerra.SetTile(new Vector3Int(j, i, 0), aux);

                if (tileType == TileType.Agua) tileAgua.SetTile(new Vector3Int(j, i, 0), aux);
            }
        }
        //returna a ultima cordenada em X
        return x + w;
    }
}
