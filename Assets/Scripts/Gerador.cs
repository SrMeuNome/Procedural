using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gerador : MonoBehaviour
{
    //Se a celula for preenchida e não tiver nada em cima ela será uma celula de topo do tipo correspondente
    //Se a celula for preenchida e não tiver nenhum vizinho preenchido a mesma passará a não ser preenchida
    //Se a celula for preenchida com um determinado tipo X e a maior parte dos vizinhos for preenchidos com o tipo Y a celula se torna do tipo Y
    //Se a celula for preenchida com um tipo X e tiver dois vizinho ou menos com o tipo Y a celula se manterá com o tipo X

    //Tamanho do mapa
    public int Size;
    private int _size = 0;

    public int MaxAltRelevo;
    public int MaxLarRelevo;

    public int MinAltRelevo;
    public int MinLarRelevo;

    //Tilemap é onde fica localizado os Tile Base
    public Tilemap tileTerra;
    public Tilemap tileAgua;

    //TileBase é o tile que queremos utilizar
    public TileBase terraTopo;
    public TileBase terra;
    public TileBase aguaTopo;
    public TileBase agua;

    struct bioma
    {
        TileBase soloTopo;
        TileBase solo;
        TileBase aguaTopo;
        TileBase agua;
        bool temAgua;
        int tamanho;
    }

    public enum TileType
    {
        Solo,
        Agua,
        Indefinido
    }

    // Start is called before the first frame update
    void Start()
    {
        int controladorAlt = 0;
        TileType beforeAuxType = TileType.Indefinido;
        while (_size <= Size)
        {
            int auxAlt = Random.Range(MinAltRelevo, MaxAltRelevo);
            int auxLar = Random.Range(MinLarRelevo, MaxLarRelevo);

            TileType auxType = Random.Range(0, 10) < 1? TileType.Agua : TileType.Solo;

            TileBase baseAuxTopo = terraTopo;
            TileBase baseAuxinferior = terra;

            if (auxType == TileType.Agua)
            {
                baseAuxTopo = aguaTopo;
                baseAuxinferior = agua;
                if(beforeAuxType == TileType.Solo)
                {
                    auxAlt = controladorAlt;
                }

                if(beforeAuxType == TileType.Agua)
                {
                    auxType = TileType.Solo;
                }
            }
            
            if(auxType == TileType.Solo)
            {
                baseAuxTopo = terraTopo;
                baseAuxinferior = terra;
                if (beforeAuxType == TileType.Agua && auxAlt < controladorAlt)
                {
                    auxAlt = controladorAlt;
                }
            }

            if (_size + auxLar > Size)
            {
                auxLar = Size - _size;
                GerarRelevo(auxLar, auxAlt, 0, baseAuxTopo, baseAuxinferior, auxType);
                break;
            }
            else
            {
                GerarRelevo(auxLar, auxAlt, 0, baseAuxTopo, baseAuxinferior, auxType);
            }

            controladorAlt = auxAlt;
            beforeAuxType = auxType;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GerarRelevo(int w, int h, int y, TileBase superficie, TileBase inferior, TileType tileType)
    {
        TileBase aux;
        for(int i = y; i < h+y; i++)
        {
            //Largura mais a posição em x
            for(int j = _size; j < w+_size; j++)
            {
                //Altura mais a posição em y
                if (i == h+y - 1) aux = superficie;
                else aux = inferior;

                //Tipo do objeto
                if(tileType == TileType.Solo) tileTerra.SetTile(new Vector3Int(j, i, 0), aux);

                if (tileType == TileType.Agua) tileAgua.SetTile(new Vector3Int(j, i, 0), aux);
            }
        }
        //retorna a ultima cordenada em X
        _size += w;
    }
}
