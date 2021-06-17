using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Se a celula for preenchida e não tiver nenhum vizinho preenchido a mesma passará a não ser preenchida
//Se a celula for preenchida com um determinado tipo X e a maior parte dos vizinhos for preenchidos com o tipo Y a celula se torna do tipo Y
//Se a celula for preenchida com um tipo X e tiver dois vizinhos ou menos com o tipo Y a celula se manterá com o tipo X
//Se a celula for preenchida e não tiver nada em cima ela será uma celula de topo do tipo correspondente
//Se a celula for vazia e tiver dois ou mais vizinhos cada um de um tipo ela se tornara um dos tipos com a probabilidade 1/n
//Se a celula for vazia e tiver dois ou mais vizinhos ela terá o mesmo estado do vizinho que mais repete

public class CellGenerator : MonoBehaviour
{

    //Tamanho do mapa
    public int width;
    public int height;

    public int startPosition;

    //Tilemap é onde fica localizado os Tile Base
    public Tilemap tilemapBase;
    public Tilemap tilemapAux;

    public List<TileBaseGroup> listTileBase = new List<TileBaseGroup>();

    public int quantityGeneration;

    public bool infinityGeneration;

    // Start is called before the first frame update
    void Start()
    {
        TileBaseSort();
        if (!infinityGeneration)
        {
            //Smooth();
            for (int i = 0; i < quantityGeneration; i++)
            {
                Smooth();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (infinityGeneration)
        {
            Smooth();
        }
    }

    public void TileBaseSort()
    {
        for (int i = startPosition; i < height; i++)
        {
            for (int j = startPosition; j < width; j++)
            {
                int sortTilePosition = Random.Range(-listTileBase.Count, listTileBase.Count);

                //Atribuindo tilebase sortida
                if (sortTilePosition >= 0)
                {
                    TileBase tileBaseSelected = listTileBase[sortTilePosition].tileDefault;
                    tilemapBase.SetTile(new Vector3Int(j, i, 0), tileBaseSelected);
                }
            }
        }
    }

    public void Smooth()
    {
        for (int i = startPosition; i < height; i++)
        {
            for (int j = startPosition; j < width; j++)
            {
                if (IsAroundVoid(tilemapBase, j, i))
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), null);
                }
                else if (HasThreeOrMore(tilemapBase, j, i) != null)
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), HasThreeOrMore(tilemapBase, j, i));
                }
                else if (HasTwoOrMore(tilemapBase, j, i))
                {
                    TileBase auxSort = HasTwoOrMoreSort(tilemapBase, j, i);
                    if (auxSort != null)
                    {
                        tilemapAux.SetTile(new Vector3Int(j, i, 0), auxSort);
                    }
                    else
                    {
                        tilemapAux.SetTile(new Vector3Int(j, i, 0), tilemapBase.GetTile(new Vector3Int(j, i, 0)));
                    }

                }
                else
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), tilemapBase.GetTile(new Vector3Int(j, i, 0)));
                }
            }
        }
        for (int i = startPosition; i < height; i++)
        {
            for (int j = startPosition; j < width; j++)
            {
                if (tilemapAux.HasTile(new Vector3Int(j, i, 0)))
                {
                    if (IsTop(tilemapAux, j, i))
                    {
                        TileBaseGroup auxTileSetTopGroup = listTileBase.Find((tile) =>
                        tile.tileDefault.Equals(tilemapAux.GetTile(new Vector3Int(j, i, 0)))
                        || tile.tileTop.Equals(tilemapAux.GetTile(new Vector3Int(j, i, 0))));

                        if (auxTileSetTopGroup != null)
                        {
                            TileBase auxTileSetTop = auxTileSetTopGroup.tileTop;
                            tilemapAux.SetTile(new Vector3Int(j, i, 0), auxTileSetTop);
                        }
                    }
                    else
                    {
                        TileBaseGroup auxTileSetTopGroup = listTileBase.Find((tile) =>
                        tile.tileDefault.Equals(tilemapAux.GetTile(new Vector3Int(j, i, 0)))
                        || tile.tileTop.Equals(tilemapAux.GetTile(new Vector3Int(j, i, 0))));

                        if (auxTileSetTopGroup != null)
                        {
                            TileBase auxTileSetDefault = auxTileSetTopGroup.tileDefault;
                            tilemapAux.SetTile(new Vector3Int(j, i, 0), auxTileSetDefault);
                        }
                    }
                }
                tilemapBase.SetTile(new Vector3Int(j, i, 0), tilemapAux.GetTile(new Vector3Int(j, i, 0)));
                tilemapAux.SetTile(new Vector3Int(j, i, 0), null);
            }
        }
    }

    private bool IsAroundVoid(Tilemap tilemap, int x, int y)
    {
        bool top = tilemap.HasTile(new Vector3Int(x, y + 1, 0));
        bool bottom = tilemap.HasTile(new Vector3Int(x, y - 1, 0));
        bool right = tilemap.HasTile(new Vector3Int(x + 1, y, 0));
        bool left = tilemap.HasTile(new Vector3Int(x - 1, y, 0));

        if (!top && !bottom && !right && !left)
        {
            int sort = Random.Range(0, 100);
            if (sort >= 95)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private bool HasTwoOrMore(Tilemap tilemap, int x, int y)
    {
        List<TileBase> auxListTileBase = new List<TileBase>();
        bool repeatedTileBase = false;

        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y + 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y - 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x + 1, y, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x - 1, y, 0)));

        for (int i = 0; i < auxListTileBase.Count; i++)
        {
            List<TileBase> auxListCount = auxListTileBase.FindAll((tile) =>
            {
                if (tile != null && auxListTileBase[i] != null)
                {
                    TileBaseGroup auxTileDefaultGroup = listTileBase.Find((tileAux) =>
                    tileAux.tileDefault.Equals(tile)
                    || tileAux.tileTop.Equals(tile));

                    if (auxTileDefaultGroup != null)
                    {
                        TileBase auxTileDefault = auxTileDefaultGroup.tileDefault;
                        TileBase auxTileTop = auxTileDefaultGroup.tileTop;
                        return (auxListTileBase[i].Equals(auxTileDefault) || auxListTileBase[i].Equals(auxTileTop));
                    }
                }

                return false;
            });

            if (auxListCount != null)
            {
                if (auxListCount.Count >= 2)
                {
                    repeatedTileBase = true;
                    break;
                }
            }
        }

        return repeatedTileBase;
    }
    private TileBase HasTwoOrMoreSort(Tilemap tilemap, int x, int y)
    {
        List<TileBase> auxListTileBase = new List<TileBase>();
        TileBase repeatedTileBase = null;

        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y + 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y - 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x + 1, y, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x - 1, y, 0)));

        for (int i = 0; i < auxListTileBase.Count; i++)
        {
            List<TileBase> auxListCount = auxListTileBase.FindAll((tile) =>
            {
                if (tile != null && auxListTileBase[i] != null)
                {
                    TileBaseGroup auxTileDefaultGroup = listTileBase.Find((tileAux) =>
                    tileAux.tileDefault.Equals(tile)
                    || tileAux.tileTop.Equals(tile));

                    if (auxTileDefaultGroup != null)
                    {
                        TileBase auxTileDefault = auxTileDefaultGroup.tileDefault;
                        TileBase auxTileTop = auxTileDefaultGroup.tileTop;
                        return (auxListTileBase[i].Equals(auxTileDefault) || auxListTileBase[i].Equals(auxTileTop));
                    }
                }

                return false;
            });

            if (auxListCount != null)
            {
                if (auxListCount.Count >= 2)
                {
                    repeatedTileBase = auxListTileBase[i];
                    break;
                }
            }
        }

        int sort = Random.Range(0, 100);
        if (sort >= 99)
        {
            return repeatedTileBase;
        }
        return null;
    }

    private TileBase HasThreeOrMore(Tilemap tilemap, int x, int y)
    {
        List<TileBase> auxListTileBase = new List<TileBase>();
        TileBase repeatedTileBase = null;

        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y + 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y - 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x + 1, y, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x - 1, y, 0)));

        for (int i = 0; i < auxListTileBase.Count; i++)
        {
            List<TileBase> auxListCount = auxListTileBase.FindAll((tile) =>
            {
                if (tile != null && auxListTileBase[i] != null)
                {
                    TileBaseGroup auxTileDefaultGroup = listTileBase.Find((tileAux) =>
                    tileAux.tileDefault.Equals(tile)
                    || tileAux.tileTop.Equals(tile));

                    if (auxTileDefaultGroup != null)
                    {
                        TileBase auxTileDefault = auxTileDefaultGroup.tileDefault;
                        TileBase auxTileTop = auxTileDefaultGroup.tileTop;
                        return (auxListTileBase[i].Equals(auxTileDefault) || auxListTileBase[i].Equals(auxTileTop));
                    }
                }

                return false;
            });

            if (auxListCount != null)
            {
                if (auxListCount.Count >= 3)
                {
                    repeatedTileBase = auxListTileBase[i];
                    break;
                }
            }
        }

        return repeatedTileBase;
    }

    private bool IsTop(Tilemap tilemap, int x, int y)
    {
        bool top = tilemap.HasTile(new Vector3Int(x, y + 1, 0));

        if (!top)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

//TileBase é o tile que queremos utilizar
[System.Serializable]
public class TileBaseGroup
{
    [SerializeField]
    public TileBase tileTop;

    [SerializeField]
    public TileBase tileDefault;
}
