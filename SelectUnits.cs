using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectUnits : MonoBehaviour {

	public Unit[] unitsInGame;
	public Unit[] roster;
	public List<Unit> unitsInScene = new List<Unit> ();
	public Text[] rosterDisplay;
	//public static Unit[] activeRoster;
	public int maxUnitsInScene;
	public Text maxUnitsInSceneText;
	
	public Button[] rosterButtons;
	public Button[] attackButtons;

	public Text[] attackNumbers;

    public Unit Magnus;
    public Unit Marius;

    public TileMap startAreaMap;

	void Awake(){
      //  if (PlayerPrefs.GetInt("Magnus") == 1)
    //    {
           Magnus.isOnRoster = 1;
     //   }
     //   else {
           // Magnus.isOnRoster = 0;
    //    }
    //    if (PlayerPrefs.GetInt("Marius") == 1)
     //   {
        //    Marius.isOnRoster = 1;
     //   }
//else
//{
           // Marius.isOnRoster = 0;
    //    }
		foreach (Unit u in unitsInGame) {
		//	if (u.thisUnitStats.predeceased == true) {
		//		u.isOnRoster = 0;
		//		u.thisUnitStats.predeceased = false;
		//	} else {
				u.isOnRoster = 1;
		//	}
		}
        roster = figureOutRoster();
	}

	// Use this for initialization
	void Start () {
		for (int x = 0; x < roster.Length; x++) {
			rosterDisplay[x].text = roster[x].thisUnitStats.unitName.ToString(); 
		}
		foreach (Unit u in unitsInGame) {
			if (u.fixedStartPoint == false) {
				u.gameObject.SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		maxUnitsInSceneText.text = unitsInScene.Count.ToString () + "/" + maxUnitsInScene.ToString ();
		if (unitsInScene.Count >= maxUnitsInScene) {
			foreach (Button b in rosterButtons) {
				b.interactable = false;
			}
		} else {
			foreach (Button b in rosterButtons) {
			//	b.interactable = true;
			}
		}
	}

	Unit[] figureOutRoster(){
		List<Unit> temp = new List<Unit>();
		foreach (Unit u in unitsInGame) {
			if (u.isOnRoster == 1){
				temp.Add(u);
			}
		}
		return temp.ToArray ();
	}

	public void Clear(){
		foreach (Unit u in unitsInGame) {
			if (u.fixedStartPoint == false) {
				u.gameObject.SetActive (false);
			}
		}
		foreach (Button b in attackButtons) {
			b.gameObject.SetActive (false);
		}
		foreach (Text t in attackNumbers) {
			t.gameObject.SetActive (false);
		}
		unitsInScene.Clear ();
		foreach (Unit u in unitsInGame) {
			if (u.fixedStartPoint == true) {
				unitsInScene.Add (u);
			}
		}
		foreach (Button b in rosterButtons) {
			b.interactable = true;

		}
	}

	public void Activate0(){
		roster [0].gameObject.SetActive (true);
        unitsInScene.Add (roster [0]);
		rosterButtons [0].interactable = false;
        checkIfUnitSame(roster[0]);
	}

	public void Activate1(){
		roster [1].gameObject.SetActive (true);
        unitsInScene.Add (roster [1]);
		rosterButtons [1].interactable = false;
        checkIfUnitSame(roster[1]);
	}

	public void Activate2(){
		roster [2].gameObject.SetActive (true);
		unitsInScene.Add (roster [2]);
		rosterButtons [2].interactable = false;
        checkIfUnitSame(roster[2]);
	}

    public void Activate3()
    {
        roster[3].gameObject.SetActive(true);
        unitsInScene.Add(roster[3]);
		rosterButtons [3].interactable = false;
        checkIfUnitSame(roster[3]);
    }

	public void Activate4(){
		roster [4].gameObject.SetActive (true);
		unitsInScene.Add (roster [4]);
		rosterButtons [4].interactable = false;
        checkIfUnitSame(roster[4]);
	}

	public void Activate5(){
		roster [5].gameObject.SetActive (true);
		unitsInScene.Add (roster [5]);
		rosterButtons [5].interactable = false;
        checkIfUnitSame(roster[5]);
	}

	public void Activate6(){
		roster [6].gameObject.SetActive (true);
		unitsInScene.Add (roster [6]);
		rosterButtons [6].interactable = false;
        checkIfUnitSame(roster[6]);
	}

	public void Activate7(){
		roster [7].gameObject.SetActive (true);
		unitsInScene.Add (roster [7]);
		rosterButtons [7].interactable = false;
        checkIfUnitSame(roster[7]);
	}

	public void Activate8(){
		roster [8].gameObject.SetActive (true);
		unitsInScene.Add (roster [8]);
		rosterButtons [8].interactable = false;
        checkIfUnitSame(roster[8]);
	}

	public void Activate9(){
		roster [9].gameObject.SetActive (true);
        unitsInScene.Add (roster [9]);
		rosterButtons [9].interactable = false;
        checkIfUnitSame(roster[9]);
	}

	public void Activate10(){
		roster [10].gameObject.SetActive (true);
        unitsInScene.Add (roster [10]);
		rosterButtons [10].interactable = false;
        checkIfUnitSame(roster[10]);
	}

	public void disable(){
		gameObject.SetActive (false);
	}

    void checkIfUnitSame(Unit thisUnit){
        List<Vector3> startingMapPositions = new List<Vector3>();
        Vector3 startingPoint = new Vector3(.5f + startAreaMap.transform.position.x, startAreaMap.transform.position.y,
            .5f + startAreaMap.transform.position.z);
        float x = startingPoint.x;
        float z = startingPoint.z;
        for (int i = 1; i <= startAreaMap.size_x; i++)
        {
            for (int j = 1; j <= startAreaMap.size_z; j++)
            {
          //      Debug.Log(startingPoint);
                startingMapPositions.Add(startingPoint);
                startingPoint.z += 1;
            }
            startingPoint.x += 1;
            startingPoint.z = z;
        }
      //  Debug.Log(startingMapPositions.Count);

        foreach (Unit u in unitsInScene)
        {
            List<Vector3> freeSpots = new List<Vector3>();
            foreach (Vector3 v in startingMapPositions)
            {
                if (u.transform.position.x != v.x || u.transform.position.z != v.z)
                {
             //       Debug.Log(v);
                      freeSpots.Add(v);
                }
            }
            Debug.Log(thisUnit.transform.position);
            Debug.Log(thisUnit.thisUnitStats.unitName);
            Debug.Log(u.transform.position);
            Debug.Log(u.thisUnitStats.unitName);
            if (u.transform.position.x == thisUnit.transform.position.x && u.transform.position.z == thisUnit.transform.position.z
                && u.thisUnitStats.unitName != thisUnit.thisUnitStats.unitName){
                    Debug.Log(thisUnit.thisUnitStats.unitName);
                    Vector3[] freeSpotsArray = freeSpots.ToArray();
                    
                    thisUnit.transform.position = freeSpots[0];
            }
        }
    }
}
