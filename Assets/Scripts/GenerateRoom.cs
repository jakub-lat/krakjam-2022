using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct L
{
    public GameObject prefab;
    public int mlong;
    public int mshort;
    public int mside;
    public int minside;
}

public struct Square
{
    public L a;
    public L b;
    public int mleft;
    public int mright;
    public int mtop;
    public int mdown;
}

public struct Row
{
    public List<Square> squares;
    public int mtop;
    public int mdown;
}

public struct Floor
{
    public List<Row> rows;
}

public class GenerateRoom : MonoBehaviour
{
    public List<L> fragments;
    public int height=1, width = 1;
    public float spaceX = 1, spaceY = 1;

    private void Start()
    {
        Floor f = GenerateStruct(height, width);
        Generate(f);
    }

    Dictionary<int, List<Square>> leftSqMask = new Dictionary<int, List<Square>>();
    Dictionary<int, List<Row>> topRowMask = new Dictionary<int, List<Row>>();

    public void Generate(Floor f)
    {
        float y = 0;
        foreach(Row r in f.rows)
        {
            float x = 0;
            foreach(Square s in r.squares)
            {
                Instantiate(s.a.prefab, new Vector3(x, y, 0), Quaternion.Euler(Vector3.zero), transform);
                Instantiate(s.b.prefab, new Vector3(x+3*spaceX, y+spaceY, 0), Quaternion.Euler(new Vector3(0,180,0)), transform);
                x += 4 * spaceX;
            }
            y += 2*spaceY;
        }
    }

    public Floor GenerateStruct(int height, int width)
    {
        List<Square> allSq = GetAllL(fragments);
        foreach(Square s in allSq)
        {
            if (leftSqMask.ContainsKey(s.mleft)) leftSqMask[s.mleft].Add(s);
            else leftSqMask[s.mleft] = new List<Square>() { s };

        }

        Debug.Log(allSq.Count);
        List<Row> allRows = GetAllRows(allSq, width);
        foreach(Row r in allRows)
        {
            if (topRowMask.ContainsKey(r.mtop)) topRowMask[r.mtop].Add(r);
            else topRowMask[r.mtop] = new List<Row>() { r };
        }
        Debug.Log(allRows.Count);
        Floor f = GetFloor(allRows,height);

        return f;
    }

    public List<Square> GetAllL(List<L> frag)
    {
        List<Square> res = new List<Square>();

        Dictionary<int, List<L>> map = new Dictionary<int, List<L>>();

        foreach (L curr in frag)
        {
            if (map.ContainsKey(curr.minside))
            {
                map[curr.minside].Add(curr);
            }
            else
            {
                map[curr.minside] = new List<L>() { curr };
            }
        }

        foreach (L curr in frag)
        {
            for (int S = curr.minside; S>0; S = (S - 1) & curr.minside) //every submask
            {
                if (!map.ContainsKey(S)) continue;
                foreach( L next in map[S]) //every potential to submask
                {
                    //check if good

                    if (curr.mlong + next.mshort <=0) continue; //top bad
                    if (curr.mshort + next.mlong <= 0) continue; //bottom bad

                    //add a square
                    Square newS = new Square();
                    newS.a = curr;
                    newS.b = next;
                    newS.mleft = curr.mside;
                    newS.mright = ~(-next.mside);
                    newS.mtop = curr.mlong+next.mshort;
                    newS.mdown = ~(-(curr.mshort+next.mlong));

                    res.Add(newS);
                }
            }
        }

        return res;
    }

    

    public List<List<Square>> GetAllSqChains(List<List<Square>> prevOpt, int size, int endSize, int limit)
    {

        List<List<Square>> res = new List<List<Square>>();
        int cLimit = 0;

        int i = 0;
        foreach (List<Square> l in prevOpt) //start row with this square
        {
            Square curr = l[size - 1];
            Debug.Log(curr.mright);
            for (int S = curr.mright; S > 0; S = (S - 1) & curr.mright) //every submask
            {
                Debug.Log(S);
                while(!leftSqMask.ContainsKey(S)) continue;
                
                var newList = new List<Square>(l);
                newList.Add(leftSqMask[S][Random.Range(0,leftSqMask[S].Count)]);
                cLimit++;

                res.Add(newList);
                Debug.Log(S.ToString() + " mask is gut");
                    
                break;
            }
            i++;
            if (cLimit >= limit) break;
        }

        if (size+1 >= endSize) return res;
        Debug.Log("ii " + res.Count.ToString());

        res = GetAllSqChains(res, size + 1, endSize, limit);

        return res;
    }

    public List<Row> GetAllRows(List<Square> frag, int size, int limit=10)
    {
        if (size > 8) size = 8;

        List<List<Square>> chains = new List<List<Square>>();

        foreach(Square s in frag)
        {
            chains.Add( new List<Square>(){s} );
        }

        Debug.Log(chains.Count);

        chains = GetAllSqChains(chains, 1, size, limit);

        Debug.Log(chains.Count);

        List<Row> res = new List<Row>();

        foreach(List<Square> sl in chains)
        {
            Row newRow = new Row();

            newRow.squares = sl;

            int mtop = 0;
            int mdown = 0;
            int i = 0;
            foreach(Square s in sl)
            {
                mtop += s.mtop << i;
                mdown += s.mdown << i;
                i++;
            }
            newRow.mtop = mtop;
            newRow.mdown = ~(-mdown);
        }


        return res;
    }

    public Floor GetFloor(List<Row> frag, int size)
    {
        List<Row> sel = new List<Row>();

        int currSize = 1;
        Row curr = frag[Random.Range(0, frag.Count)];
        List<int> myMasks = new List<int>();

        while (currSize < size)
        {
            myMasks = new List<int>();

            for (int S = curr.mdown; S > 0; S = (S - 1) & curr.mdown) //every submask
            {
                myMasks.Add(S);
            }

            int currM=0;
            if (myMasks.Count > 0)
            {
                currM = myMasks[Random.Range(0, myMasks.Count)]; //get random submask
            }


            while (myMasks.Count > 0 && (!topRowMask.ContainsKey(currM) || topRowMask[currM].Count <= 0)) // while the curr submask is not good get next one and check
            {
                myMasks.RemoveAt(currM);
                currM = myMasks[Random.Range(0, myMasks.Count)];
            }

            if(myMasks.Count<=0 && (!topRowMask.ContainsKey(currM) || topRowMask[currM].Count <= 0)) //if still no submask good 
            {
                sel.RemoveAt(currSize - 1);
                int r = Random.Range(0, frag.Count); //try next row
                curr = frag[r];
                sel.Add(frag[r]);
            } else
            {
                Row newR = topRowMask[currM][Random.Range(0, topRowMask[currM].Count)];
                sel.Add(newR);
                curr = newR;
                currSize++;
            }
        }

        Floor f = new Floor();
        f.rows = sel;
        return f;
    }
}
