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
    private List<L> fragments;
    public List<GameObject> puzzleObj;
    public int height=1, width = 1;
    public float spaceX = 1, spaceZ = 1;
    public GameObject boundaryWall;

    private void Start()
    {
        fragments = PuzzlesToFragments(puzzleObj);
        Floor f = GenerateStruct(height, width);
        Generate(f);
    }

    Dictionary<int, List<Square>> leftSqMask = new Dictionary<int, List<Square>>();
    Dictionary<int, List<Row>> topRowMask = new Dictionary<int, List<Row>>();

    public void Generate(Floor f)
    {
        float z = 0;
        Debug.Log("rows: "+f.rows.Count);

        for(float i = 0; i < width * spaceX*4; i += spaceX)
        {
            Instantiate(boundaryWall, new Vector3(i, 0, 0), Quaternion.Euler(new Vector3(0, 180, 0)), transform);
            Instantiate(boundaryWall, new Vector3(i, 0, spaceZ*height*2-1), Quaternion.Euler(new Vector3(0, 0, 0)), transform);
        }

        for(float i = 0; i < spaceZ * height*2; i += spaceZ)
        {
            Instantiate(boundaryWall, new Vector3(0, 0, i), Quaternion.Euler(new Vector3(0, 270, 0)), transform);
            Instantiate(boundaryWall, new Vector3(width * spaceX * 4-1, 0, i), Quaternion.Euler(new Vector3(0, 90, 0)), transform);
        }

        foreach(Row r in f.rows)
        {
            float x = 0;
            foreach(Square s in r.squares)
            {
                Instantiate(s.a.prefab, new Vector3(x, 0, z), Quaternion.Euler(Vector3.zero), transform);
                Instantiate(s.b.prefab, new Vector3(x+3*spaceX,0, z+spaceZ), Quaternion.Euler(new Vector3(0,180,0)), transform);

                x += 4 * spaceX;
            }
            z += 2*spaceZ;
        }
    }

    private List<Square> allSq;
    private List<Row> allRows;
    bool first = true;
    public Floor GenerateStruct(int height, int width)
    {
        if (first)
        {
            allSq = GetAllL(fragments);
            foreach (Square s in allSq)
            {
                if (leftSqMask.ContainsKey(s.mleft)) leftSqMask[s.mleft].Add(s);
                else leftSqMask[s.mleft] = new List<Square>() { s };

            }

            allRows = GetAllRows(allSq, width);
            foreach (Row r in allRows)
            {
                if (topRowMask.ContainsKey(r.mtop)) topRowMask[r.mtop].Add(r);
                else topRowMask[r.mtop] = new List<Row>() { r };
            }
            first = false;
        }

        Debug.Log(allRows.Count);
        Floor f = GetFloor(allRows,height);

        return f;
    }

    public List<L> PuzzlesToFragments(List<GameObject> l)
    {
        List<L> res = new List<L>();

        foreach(GameObject o in l)
        {
            if(!o.GetComponent<Puzzle>()) { Debug.LogWarning("Puzzle doesnt have a puzzle component"); continue; }

            Puzzle p = o.GetComponent<Puzzle>();

            L newL = new L();
            newL.minside = p.GetInside();
            newL.mlong = p.GetLong();
            newL.mside = p.GetSide();
            newL.mshort = p.GetShort();
            newL.prefab = o;
            res.Add(newL);
        }

        return res;
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
                string s = curr.prefab.name + ": " + S;
                if (!map.ContainsKey(S)) { Debug.Log(s + " failed A"); continue; }
                foreach( L next in map[S]) //every potential to submask
                {
                    //check if good

                    if (curr.mlong + next.mshort <= 0) { Debug.Log(s + " failed B"); continue; } //top bad
                        if (curr.mshort + next.mlong <= 0) { Debug.Log(s + " failed C"); continue; } //bottom bad

                    //add one square
                    {
                        Square newS = new Square();
                        newS.a = curr;
                        newS.b = next;
                        newS.mleft = curr.mside;

                        int d = 3;
                        if (next.mside == 2) d = 1;
                        else if (next.mside == 1) d = 2;
                        else if (next.mside == 0) { d = 0; Debug.LogWarning("A fragments side has no exit"); }

                        newS.mright = d;

                        newS.mtop = curr.mlong + next.mshort;

                        d = (curr.mshort + next.mlong) & ((~0) << 2);
                        if (d == 12) newS.mdown = 3;
                        else if (d == 4) newS.mdown = 2;
                        else if (d == 8) newS.mdown = 1;
                        newS.mdown += ((curr.mshort + next.mlong) << 2) & ((1 << 4) - 1);

                        Debug.Log(s + " gut");
                        res.Add(newS);
                    }
                    //add a square with reversed positions
                    if(curr.prefab != next.prefab){
                        Square newS = new Square();
                        newS.a = next;
                        newS.b = curr;
                        newS.mleft = next.mside;

                        int d = 3;
                        if (curr.mside == 2) d = 1;
                        else if (curr.mside == 1) d = 2;
                        else if (curr.mside == 0) { d = 0; Debug.LogWarning("A fragments side has no exit"); }

                        newS.mright = d;

                        newS.mtop = next.mlong + curr.mshort;

                        d = (next.mshort + curr.mlong) & ((~0) << 2);
                        if (d == 12) newS.mdown = 3;
                        else if (d == 4) newS.mdown = 2;
                        else if (d == 8) newS.mdown = 1;
                        newS.mdown += ((next.mshort + curr.mlong) << 2) & ((1 << 4) - 1);

                        Debug.Log(s + " gut reverse");
                        res.Add(newS);
                    }
                }
            }
        }

        Debug.Log("Frag: " + frag.Count);
        Debug.Log("Squares: " + res.Count);
        

        return res;
    }

    

    public List<List<Square>> GetAllSqChains(List<List<Square>> prevOpt, int size, int endSize, int limit)
    {
        List<List<Square>> res = new List<List<Square>>();
        while (size < endSize)
        {
            int cLimit = 0;

            foreach (List<Square> l in prevOpt) //start row with this square
            {
                Square curr = l[size - 1];
                Debug.Log("mr: " + curr.mright);
                for (int S = curr.mright; S > 0; S = (S - 1) & curr.mright) //every lower submask
                {
                    Debug.Log("S: " + S);
                    if (!leftSqMask.ContainsKey(S)) continue;

                    var newList = new List<Square>(l);
                    int j = Random.Range(0, leftSqMask[S].Count);
                    newList.Add(leftSqMask[S][j]);
                    cLimit++;
                    res.Add(newList);
                    Debug.Log(S.ToString() + " mask is gut");

                    if (cLimit >= limit) break;
                }
                if (cLimit >= limit) break;
                for (int S = curr.mright+1; S < 3; S++) //every higher submask
                {
                    if (S == 2 && curr.mright == 1) S += 1;

                    Debug.Log("S: " + S);
                    if (!leftSqMask.ContainsKey(S)) continue;

                    var newList = new List<Square>(l);
                    int j = Random.Range(0, leftSqMask[S].Count);
                    newList.Add(leftSqMask[S][j]);
                    cLimit++;
                    res.Add(newList);
                    Debug.Log(S.ToString() + " mask is gut");

                    if (cLimit >= limit || S == 3) break;
                }
                if (cLimit >= limit) break;
            }

            Debug.Log("ii " + res.Count.ToString());

            prevOpt = res;
            size++;
            res = new List<List<Square>>();
        }

        Debug.Log("ii " + prevOpt.Count.ToString());
        return prevOpt;
    }

    public List<Row> GetAllRows(List<Square> frag, int size, int limit=10000)
    {
        if (size > 8) size = 8;

        List<List<Square>> chains = new List<List<Square>>();

        foreach(Square s in frag)
        {
            chains.Add( new List<Square>(){s} );
        }

        Debug.Log("ch bef: "+chains.Count);

        chains = GetAllSqChains(chains, 1, size, limit);

        Debug.Log("ch aft: "+chains.Count);

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
                mtop += s.mtop << (4*i);
                mdown += s.mdown << (4*i);
                i++;
            }
            newRow.mtop = mtop;
            newRow.mdown = mdown;

            res.Add(newRow);
        }


        return res;
    }

    public Floor GetFloor(List<Row> frag, int size)
    {
        List<Row> sel = new List<Row>();

        int currSize = 1;
        int ir = Random.Range(0, frag.Count);
        Row curr = frag[ir];
        List<int> myMasks = new List<int>();
        sel.Add(curr);

        int endP = (int)Mathf.Pow(2, 4*size-1);

        int limit = 10000;
        int cLimit = 0;

        int limit2 = 10000;
        int cLimit2 = 0;

        Dictionary<int, List<int>> downMasks = new Dictionary<int, List<int>>();

        bool masksHigher = true;
        bool masksLower = true;

        while (currSize < size)
        {
            myMasks = new List<int>();

                cLimit = 0;
                if (masksLower)
                {
                    for (int S = curr.mdown; S > 0; S = (S - 1) & curr.mdown) //every lower submask
                    {
                        myMasks.Add(S);
                        cLimit++;
                        if (cLimit >= limit) break;
                    }
                }
                if (masksHigher)
                {
                    int Sh;
                    for (int S = ~curr.mdown; S < 0; S = (S + 1) & curr.mdown) //every HIGHER submask
                    {
                        Sh = ~S;
                        myMasks.Add(Sh);

                        if (S == endP) break;
                        cLimit++;
                        if (cLimit >= limit) break;
                    }
                }

            int currM=-1, i=0;
            bool bad = (myMasks.Count <= 0);

            if (!bad)
            {
                i = Random.Range(0, myMasks.Count);
                currM = myMasks[i]; //get random submask
            }


            while (!bad && myMasks.Count > 0 && (!topRowMask.ContainsKey(currM) || topRowMask[currM].Count <= 0)) // while the curr submask is not good get next one and check
            {
                myMasks.RemoveAt(i);
                i = Random.Range(0, myMasks.Count);

                if (myMasks.Count <= 0) break;
                currM = myMasks[i]; //get random submask
            }

            if(bad || (myMasks.Count<=0 && (!topRowMask.ContainsKey(currM) || topRowMask[currM].Count <= 0))) //if still no submask good 
            {
                sel.RemoveAt(currSize - 1);
                ir = Random.Range(0, frag.Count); //try next row
                curr = frag[ir];
                sel.Add(curr);
            } else
            {
                int j = Random.Range(0, topRowMask[currM].Count);
                Row newR = topRowMask[currM][j];

                Debug.Log("New row added with botM: " + currM + " at index: " + j);
                string s = "Row contains: ";
                foreach(Square sq in newR.squares)
                {
                    s += "{" + sq.a.prefab.name + ", " + sq.b.prefab.name + "}, ";
                }
                Debug.Log(s);
                sel.Add(newR);
                curr = newR;
                currSize++;

                cLimit2 = 0;
            }

            cLimit2++;
            if (cLimit2 >= limit2)
            {
                Debug.LogError("Row Checking limit reached");
                break;
            }
        }

        Floor f = new Floor();
        f.rows = sel;
        return f;
    }
}
