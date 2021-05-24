using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeradorCelular : MonoBehaviour
{
    //Se a celula for preenchida e não tiver nenhum vizinho preenchido a mesma passará a não ser preenchida
    //Se a celula for preenchida com um determinado tipo X e a maior parte dos vizinhos for preenchidos com o tipo Y a celula se torna do tipo Y
    //Se a celula for preenchida com um tipo X e tiver dois vizinhos ou menos com o tipo Y a celula se manterá com o tipo X
    //Se a celula for preenchida e não tiver nada em cima ela será uma celula de topo do tipo correspondente
    //Se a celula for vazia e tiver dois ou mais vizinhos cada um de um tipo ela se tornara um dos tipos com a probabilidade 1/n
    //Se a celula for vazia e tiver dois ou mais vizinhos ela terá o mesmo estado do vizinho que mais repete


    //Tamanho do mapa
    public int width;
    public int height;

    public int startPosition;

    //Tilemap é onde fica localizado os Tile Base
    public Tilemap tilemapBase;
    public Tilemap tilemapAux;

    //TileBase é o tile que queremos utilizar
    [System.Serializable]
    public class TileBaseGroup
    {
        [SerializeField]
        public TileBase tileTop;

        [SerializeField]
        public TileBase tileDefault;
    }

    public List<TileBaseGroup> listTileBase = new List<TileBaseGroup>();

    // Start is called before the first frame update
    void Start()
    {
        TileBaseSort();
        for (int i = 0; i < 30; i++)
        {
            Smooth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Smooth();
        //tilemapBase.RefreshAllTiles();
    }

    public void TileBaseSort()
    {
        for (int i = startPosition; i < height; i++)
        {
            for (int j = startPosition; j < width; j++)
            {
                int sortTilePosition = Random.Range(-listTileBase.Count + 1, listTileBase.Count);

                //Atribuindo tilebase sotiada
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
                if(IsAroundVoid(tilemapBase, j, i))
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), null);
                }

                if(IsVoidAndHasAroundRepeat(tilemapBase, j, i) != null)
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), IsVoidAndHasAroundRepeat(tilemapBase, j, i));
                }

                if(HasThreeOrMore(tilemapBase, j, i) != null)
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), HasThreeOrMore(tilemapBase, j, i));
                }
                else
                {
                    tilemapAux.SetTile(new Vector3Int(j, i, 0), tilemapBase.GetTile(new Vector3Int(j, i, 0)));
                }

                if(IsTop(tilemapBase, j, i))
                {
                    TileBaseGroup auxTileSetTopGroup = listTileBase.Find((tile) =>
                    tile.tileDefault.Equals(tilemapBase.GetTile(new Vector3Int(j, i, 0)))
                    || tile.tileTop.Equals(tilemapBase.GetTile(new Vector3Int(j, i, 0))));

                    if(auxTileSetTopGroup != null)
                    {
                        TileBase auxTileSetTop = auxTileSetTopGroup.tileTop;
                        tilemapAux.SetTile(new Vector3Int(j, i, 0), auxTileSetTop);
                    }
                } else
                {
                    TileBaseGroup auxTileSetDefaultGroup = listTileBase.Find((tile) =>
                    tile.tileDefault.Equals(tilemapBase.GetTile(new Vector3Int(j, i, 0)))
                    || tile.tileTop.Equals(tilemapBase.GetTile(new Vector3Int(j, i, 0))));

                    if (auxTileSetDefaultGroup != null)
                    {
                        TileBase auxTileSetDefault = auxTileSetDefaultGroup.tileDefault;
                        tilemapAux.SetTile(new Vector3Int(j, i, 0), auxTileSetDefault);
                    }
                }
            }
        }
        for (int i = startPosition; i < height; i++)
        {
            for (int j = startPosition; j < width; j++)
            {
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
            return true;
        }
        else
        {
            return false;
        }
    }

    private TileBase IsVoidAndHasAroundRepeat(Tilemap tilemap, int x, int y)
    {
        if(!tilemap.HasTile(new Vector3Int(x, y + 1, 0)))
        {
            List<TileBase> auxListTileBase = new List<TileBase>();
            TileBase repeatedTileBase = null;

            auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y + 1, 0)));
            auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y - 1, 0)));
            auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x + 1, y, 0)));
            auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x - 1, y, 0)));

            for (int i = 0; i < auxListTileBase.Count; i++)
            {
                List<TileBase> auxListCount = auxListTileBase.FindAll((tile) => {
                    if (tile != null)
                    {
                        return tile.Equals(auxListTileBase[i]);
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

            return repeatedTileBase;
        }
        else
        {
            return null;
        }
    }

    private TileBase HasThreeOrMore(Tilemap tilemap, int x, int y)
    {
        /*
         * 0: Top
         * 1: Bottom
         * 2: Right
         * 3: Left
        */

        List<TileBase> auxListTileBase = new List<TileBase>();
        TileBase repeatedTileBase = null;

        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y + 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x, y - 1, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x + 1, y, 0)));
        auxListTileBase.Add(tilemap.GetTile(new Vector3Int(x - 1, y, 0)));

        for(int i = 0; i < auxListTileBase.Count; i++)
        {
            List<TileBase> auxListCount = auxListTileBase.FindAll((tile) => { 
                if(tile != null)
                {
                    return tile.Equals(auxListTileBase[i]);
                }

                return false;
            });
            if(auxListCount != null)
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
