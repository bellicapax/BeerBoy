using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSpawner : MonoBehaviour {


    public GameObject goTile1;
    public GameObject goFirstTile1;
    public GameObject goLongTentTransforms;
    public GameObject goSmallTentTransforms;
    public GameObject[] goLongTents;
    public GameObject goSmallTent;
    public GameObject goTable;
    public GameObject goHotDogStand;
    public GameObject goTrashCan;

    private GameObject[] smallItems = new GameObject[3];

    // Variables needed for the transforms
    private Vector3[] longTentPositions;
    private Quaternion[] longTentRotations; 
    private Vector3[] smallTentPositions;
    private Quaternion[] smallTentRotations;
    private string strLong = "LongTent";
    private string strSmall = "SmallTent";


    //private Transform[] table;
    //private Transform[] hotDogStand;

    // More global variables
    public Transform tileTransform;
    private int tileCounter = 1;
    private int numberOfTrashPositionItemsToSpawn;
    private Vector3 offScreen = new Vector3(0.0f, -1000.0f, 0.0f);
    private Vector3 nextTilePlus = new Vector3(0.0f, 0.0f, 208.6048f);
    private PlayerInput scriptInput;

    // Variables for spawning trash cans
    private int startingNumberOfTrashCans = 5;
    private int trashCanNumberWide = 9;
    private int trashCanNumberDeep = 63;            // was 65
    private int minimumTrashCanDistance = 5;
    private int baseNumberOfItemsPerSquare;
    private int numberOfSquaresWithOneAdditionalItem;
    private float trashCanSpacing = 3.158224f;
    public List<Vector2> trashCanReferences = new List<Vector2>();
    private List<Vector2> usedTrashCanPositions = new List<Vector2>();
    public List<Vector2> remainingTrashCanPositions = new List<Vector2>();
    private Vector3 playerStartingPosition;
    //private Vector3 trashCanZminXmin = new Vector3(15.09241f, 0.008809675f, 102.6423f);
    private Vector3 trashCanZminXmin = new Vector3(15.09241f, 0.008809675f, 105.800524f);

    public Vector3[,] trashCanPositions;
 
    // Variables for spawning small items
    private List<int> masterSmallItemsList = new List<int>();
    private List<int> currentSmallItemsList;
    private List<int> currentlongTentList = new List<int>();

    // Variables for spawning party goers
    public GameObject goGirl;
    public GameObject goDude;
    private int startingNumberOfPartyGoers = 1;

    // Variables for spawning beer and soda
    public GameObject goBeer;
    public GameObject goSoda;

	// Use this for initialization
	void Start () 
    {
        scriptInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        // *** Initialize the longTent and smallTent arrays ***

        longTentPositions = new Vector3[goLongTentTransforms.GetComponentsInChildren<Transform>().Length];
        longTentRotations = new Quaternion[goLongTentTransforms.GetComponentsInChildren<Transform>().Length];
        smallTentPositions = new Vector3[goSmallTentTransforms.GetComponentsInChildren<Transform>().Length];
        smallTentRotations = new Quaternion[goSmallTentTransforms.GetComponentsInChildren<Transform>().Length];

        int k = 0;

        foreach (Transform t in goLongTentTransforms.GetComponentsInChildren<Transform>())
        {
            longTentPositions[k] = t.localPosition;
            longTentRotations[k] = t.localRotation;
            k++;
        }

        k = 0;

        foreach (Transform t in goSmallTentTransforms.GetComponentsInChildren<Transform>())
        {
            smallTentPositions[k] = t.localPosition;
            smallTentRotations[k] = t.localRotation;
            k++;
        }


        // *** Initialize the master and current smallItems lists ***

        for (int i = 0; i < smallTentPositions.Length; i++)
        {
            masterSmallItemsList.Add(i);
        }
        currentSmallItemsList = new List<int>(masterSmallItemsList);

       
        // *** Add the small items to the smallItems array ***

        smallItems[0] = goTable;
        smallItems[1] = goSmallTent;
        smallItems[2] = goHotDogStand;


        // *** Fill the array of trashCanPositions ***
        
        trashCanPositions = new Vector3[trashCanNumberWide, trashCanNumberDeep];
        playerStartingPosition = GameObject.FindWithTag("Player").transform.position;
        
        for (int i = 0; i < trashCanNumberWide; i++)
        {
            for (int j = 0; j < trashCanNumberDeep; j++)
            {
                trashCanPositions[i, j] = new Vector3(trashCanZminXmin.x - (i * trashCanSpacing), trashCanZminXmin.y, trashCanZminXmin.z - (j * trashCanSpacing));
                trashCanReferences.Add(new Vector2(i, j));
            }
        }

        foreach (Vector2 v in trashCanReferences)
        {
            remainingTrashCanPositions.Add(v);
        }

        // *** Instantiate the original tile scenery ***
        //minimumTrashCanDistance = (startingNumberOfPartyGoers * 2.0f * tileCounter) + (NumOfBeerAndSoda() * 2.0f * tileCounter) + startingNumberOfTrashCans;

        //SpawnGoAtTrashPositions(goFirstTile1.transform, goTrashCan, startingNumberOfTrashCans, true, true);

        //SpawnGoAtTrashPositions(goFirstTile1.transform, goGirl, startingNumberOfPartyGoers, true);

        //SpawnGoAtTrashPositions(goFirstTile1.transform, goDude, startingNumberOfPartyGoers, true);

        //SpawnGoAtTrashPositions(tileTransform, goBeer, NumOfBeerAndSoda(), true);

        //SpawnGoAtTrashPositions(tileTransform, goSoda, NumOfBeerAndSoda(), true);

        SpawnBeginningEndingTables(goFirstTile1.transform);

        SpawnLongTents(goFirstTile1.transform);

        RemoveBlockedAreasForSmallItems();

        SpawnSmallItems(goFirstTile1.transform);

        StartCoroutine(CreateNextTile());
	}

    public bool CheckTrashCanPositions(Vector2 possiblePosition)
    {
        foreach (Vector2 v in usedTrashCanPositions)
        {
            if ((Mathf.Abs(possiblePosition.x - v.x) + Mathf.Abs(possiblePosition.y - v.y)) < minimumTrashCanDistance)     // If the x + z value of the object is less than the minimum distance away, try again
                return true;
        }
        return false;
    }

    private void RemovePositionsAroundObject(Vector2 position)
    {
        for (int i = 0; i < minimumTrashCanDistance; i++)
        {
            //  To keep j from staying at 15 every time, we subtract the i and only let it get down to k
            int k = -1;
            for (int j = minimumTrashCanDistance; j > k; j--)
            {
                // For every position in a diamond shape around the object, remove it.  Subtracting i from j in the y position allows us to keep the diamond shape and not a square
                if (remainingTrashCanPositions.Contains(new Vector2(position.x + i, position.y + j - i)))
                {
                    print("Removing Vector " + new Vector2(position.x + i, position.y + j - i));
                    remainingTrashCanPositions.Remove(new Vector2(position.x + i, position.y + j - i));
                }
                if (remainingTrashCanPositions.Contains(new Vector2(position.x - i, position.y - j + i)))
                {
                    print("Removing Vector " + new Vector2(position.x - i, position.y - j + i));
                    remainingTrashCanPositions.Remove(new Vector2(position.x - i, position.y - j + i));
                }
            }
            k++;
        }
    }

    private void SpawnGoAtTrashPositions(Transform currentTile, GameObject goToClone, int numberToClone, bool trashCans = false)
    {
        // Since trash cans are the starting item, clear the used list and repopulate the unused list
        if (trashCans)
        {
            usedTrashCanPositions.Clear();
            remainingTrashCanPositions.Clear();
            foreach (Vector2 v in trashCanReferences)
            {
                remainingTrashCanPositions.Add(v);
            }
        }
        for (int i = 0; i < numberToClone; i++)
        {
            if (remainingTrashCanPositions.Count > 0)                                                                                                                   // Make sure we can spawn more stuff
            {
                GameObject goClone = GameObject.Instantiate(goToClone, offScreen, goToClone.transform.rotation) as GameObject;                                                    // Instantiate the object
                goClone.transform.parent = currentTile;                                                                                                                  // Make it a child of the tile
                int listPosition = Random.Range(0, remainingTrashCanPositions.Count - 1);                                                                                 // Create a random position in the remaining positions
                //if (numberOfTrashPositionItemsToSpawn < 146)
                //{
                //    while (CheckTrashCanPositions(remainingTrashCanPositions[listPosition]))                                                                             // This will return true if they are too close to each other or the player
                //    {
                //        listPosition = Random.Range(0, remainingTrashCanPositions.Count - 1);                                                                                       // If it is, make another and try again
                //    }
                //}
                //RemovePositionsAroundObject(remainingTrashCanPositions[listPosition]);
                usedTrashCanPositions.Add(remainingTrashCanPositions[listPosition]);                                                                                                            // Add the unique position to the list
                goClone.transform.localPosition = trashCanPositions[(int)remainingTrashCanPositions[listPosition].x, (int)remainingTrashCanPositions[listPosition].y];    // Set the clone's local position to that position
                remainingTrashCanPositions.RemoveAt(listPosition);                                                                                                              // Remove the position from the available positions
            }
        }
    }

    private int NumOfBeerAndSoda()
    {
        int num = Mathf.CeilToInt((startingNumberOfPartyGoers + tileCounter) / 6.0f);
        print("Number Of Beers and Soda: " + num);
            return num;
    }

    private void ReadyTrashCanNumbers()
    {
        numberOfTrashPositionItemsToSpawn = ((startingNumberOfPartyGoers + tileCounter) * 2) + (NumOfBeerAndSoda() * 2) + startingNumberOfTrashCans;

        //baseNumberOfItemsPerSquare = Mathf.FloorToInt(numberOfTrashPositionItemsToSpawn / trashCanNumberWide);

        //numberOfSquaresWithOneAdditionalItem = numberOfTrashPositionItemsToSpawn % trashCanNumberWide;


        //float zDistance;
        //if (65 / numberOfTrashPositionItemsToSpawn > 1)
        //    zDistance = Mathf.Floor(65 / numberOfTrashPositionItemsToSpawn);
        //else
        //    zDistance = 65 / numberOfTrashPositionItemsToSpawn;

        //if (zDistance > 1)
        //{
        //    minimumTrashCanDistance = (int)(zDistance + 4);
        //}
        //else
        //{
        //    minimumTrashCanDistance = Mathf.FloorToInt(4 * zDistance);
        //}
    }

    private void CalculateTrashCanDistance(int numberOfItems)
    {
        minimumTrashCanDistance = Mathf.FloorToInt(trashCanNumberWide / numberOfItems);
    }

    private void SpawnBeginningEndingTables(Transform currentTile)
    {
        // Spawn the two end tents
        GameObject[] beginningEndingTentClone = new GameObject[2];
        for (int i = 0; i < beginningEndingTentClone.Length; i++)
        {
            beginningEndingTentClone[i] = GameObject.Instantiate(goLongTents[Random.Range(0, 2)], offScreen, Quaternion.identity) as GameObject;
            beginningEndingTentClone[i].transform.parent = currentTile;
            beginningEndingTentClone[i].transform.localPosition = longTentPositions[i + 1];
            beginningEndingTentClone[i].transform.localRotation = longTentRotations[i + 1];
        }
    }

    private bool CheckVector3(Vector3 possible, List<Vector3> listPos)
    {
        foreach (Vector3 t in listPos)
        {
            if (possible == t)
                return true;
        }
        return false;
    }

    private void SpawnLongTents(Transform parLTParent)
    {
        // Determine how many longTents there are going to be
        int numOfLongTents = Random.Range(1, 4);
        GameObject[] goLongTentsClone = new GameObject[numOfLongTents];
        List<Vector3> usedPositions = new List<Vector3>();
        List<Quaternion> usedRotations = new List<Quaternion>();

        // Determine where they are going to be and spawn them
        for (int i = 0; i < numOfLongTents; i++)
        {
            int possibleX = Random.Range(3, longTentPositions.Length);
            while (CheckVector3(longTentPositions[possibleX], usedPositions))
            {
                possibleX = Random.Range(3, longTentPositions.Length);
            }

            // If possibleX is even, then add its odd counterpart first (which is the XMin value)
            if (possibleX % 2 == 0)
            {
                usedPositions.Add(longTentPositions[possibleX - 1]);
                usedPositions.Add(longTentPositions[possibleX]);
                usedRotations.Add(longTentRotations[possibleX - 1]);
                usedRotations.Add(longTentRotations[possibleX]);
            }

            // If possibleX is odd, then add it first and then its even counterpart (which is the XMax value)
            else
            {
                usedPositions.Add(longTentPositions[possibleX]);
                usedPositions.Add(longTentPositions[possibleX + 1]);
                usedRotations.Add(longTentRotations[possibleX]);
                usedRotations.Add(longTentRotations[possibleX + 1]);
            }

            // Add the possibleX to our currently used longTentTransforms
            currentlongTentList.Add(possibleX);

            goLongTentsClone[i] = GameObject.Instantiate(goLongTents[Random.Range(0, 2)], offScreen, Quaternion.identity) as GameObject;
            goLongTentsClone[i].transform.parent = parLTParent;
            goLongTentsClone[i].transform.localPosition = new Vector3(Random.Range(usedPositions[2 * i].x, usedPositions[(2 * i) + 1].x), usedPositions[2 * i].y, usedPositions[2 * i].z);
            goLongTentsClone[i].transform.localRotation = usedRotations[2 * i];
        }
        usedPositions.Clear();
        usedRotations.Clear();
    }

    private void RemoveBlockedAreasForSmallItems()
    {
        foreach (int i in currentlongTentList)
        {
            if (i == 3 || i == 4)
            {
                currentSmallItemsList.Remove(1);
                currentSmallItemsList.Remove(2);
                currentSmallItemsList.Remove(3);
                currentSmallItemsList.Remove(4);
            }
            else if (i == 5 || i == 6)
            {
                currentSmallItemsList.Remove(5);
                currentSmallItemsList.Remove(6);
                currentSmallItemsList.Remove(7);
                currentSmallItemsList.Remove(8);
                currentSmallItemsList.Remove(9);
                currentSmallItemsList.Remove(10);
            }
            else if (i == 7 || i == 8)
            {
                currentSmallItemsList.Remove(11);
                currentSmallItemsList.Remove(12);
                currentSmallItemsList.Remove(13);
                currentSmallItemsList.Remove(14);
                currentSmallItemsList.Remove(15);
                currentSmallItemsList.Remove(16);
            }
            else if (i == 9 || i == 10)
            {
                currentSmallItemsList.Remove(15);
                currentSmallItemsList.Remove(16);
                currentSmallItemsList.Remove(17);
                currentSmallItemsList.Remove(18);
                currentSmallItemsList.Remove(19);
                currentSmallItemsList.Remove(20);
            }
            else if (i == 11 || i == 12)
            {
                currentSmallItemsList.Remove(19);
                currentSmallItemsList.Remove(20);
                currentSmallItemsList.Remove(21);
                currentSmallItemsList.Remove(22);
                currentSmallItemsList.Remove(23);
                currentSmallItemsList.Remove(24);
            }
            else if (i == 13 || i == 14)
            {
                currentSmallItemsList.Remove(23);
                currentSmallItemsList.Remove(24);
                currentSmallItemsList.Remove(25);
                currentSmallItemsList.Remove(26);
                currentSmallItemsList.Remove(27);
                currentSmallItemsList.Remove(28);
            }
        }
        currentlongTentList.Clear();
    }

    private void SpawnSmallItems(Transform parParent)
    {
        int numOfSmallItemsToSpawn = Random.Range(1, (currentSmallItemsList.Count - 3) / 2);

        GameObject[] goSmallItemsClone = new GameObject[numOfSmallItemsToSpawn];
        for (int i = 0; i < numOfSmallItemsToSpawn; i++)
        {
            int z = currentSmallItemsList[Random.Range(4, currentSmallItemsList.Count)];
            if (z % 2 == 0)
            {
                currentSmallItemsList.Remove(z);
                currentSmallItemsList.Remove(z - 1);
                z--;
            }
            else
            {
                currentSmallItemsList.Remove(z);
                currentSmallItemsList.Remove(z + 1);
            }
            goSmallItemsClone[i] = GameObject.Instantiate(smallItems[Random.Range(0, smallItems.Length)], offScreen, Quaternion.identity) as GameObject;
            goSmallItemsClone[i].transform.parent = parParent;
            goSmallItemsClone[i].transform.localPosition = new Vector3(Random.Range(smallTentPositions[z].x, smallTentPositions[z + 1].x), smallTentPositions[z].y, smallTentPositions[z].z);
            goSmallItemsClone[i].transform.localRotation = smallTentRotations[z];
        }

        // Reset the currentSmallItemsList so we can do it again
        currentSmallItemsList.Clear();
        foreach (int i in masterSmallItemsList)
        {
            currentSmallItemsList.Add(i);
        }
    }

    public IEnumerator CreateNextTile()
    {
        GameObject tileClone = GameObject.Instantiate(goTile1, nextTilePlus * tileCounter, goTile1.transform.rotation) as GameObject;
        tileTransform = tileClone.transform;
        yield return null;

        ReadyTrashCanNumbers();
        //print("Minimum Distance = " + minimumTrashCanDistance);
        //print("Number of items to spawn = " + numberOfTrashPositionItemsToSpawn);

        // Spawn Trash Cans    

        SpawnGoAtTrashPositions(tileTransform, goTrashCan, startingNumberOfTrashCans, true);
        yield return null;

        // Spawn Kids

        SpawnGoAtTrashPositions(tileTransform, goGirl, startingNumberOfPartyGoers + tileCounter);
        yield return null;

        // Spawn Adults

        SpawnGoAtTrashPositions(tileTransform, goDude, startingNumberOfPartyGoers + tileCounter);
        yield return null;

            
        // Spawn Beer

        SpawnGoAtTrashPositions(tileTransform, goBeer, NumOfBeerAndSoda());
        yield return null;


        // Spawn Soda

        SpawnGoAtTrashPositions(tileTransform, goSoda, NumOfBeerAndSoda());
        yield return null;


        // Create Beginning and Ending Tables

        SpawnBeginningEndingTables(tileTransform);
        yield return null;


        // Spawn long tents

        SpawnLongTents(tileTransform);
        yield return null;


        // Determine where small objects can spawn

        RemoveBlockedAreasForSmallItems();
        yield return null;


        // Determine how many will spawn, where they will spawn, and spawn them

        SpawnSmallItems(tileTransform);


        // Create PartyGoers

        //yield return new WaitForSeconds(secondsTillNextTile - Time.time);
        //secondsTillNextTile = Time.time + secondsToCreateNextTile;
        tileCounter++;
    }

}
