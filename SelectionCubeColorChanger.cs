using UnityEngine;
using System.Collections;

public class SelectionCubeColorChanger : MonoBehaviour {

	public Unit[] units;
	Unit selectedUnit = null;

    public Pathfinding pathfinding;

	public Transform[] unwalkables;
	Color c = new Color (0, 100, 0, 70);
   // Texture2D texture = new Texture2D(1, 1);
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE
			|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
			|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.START)
        {
            Texture2D texture = new Texture2D(1, 1);
            Color c = new Color(0, 100, 0, 70);
            foreach (Unit u in units)
            {
				if (u.isSelected == 1) {
					selectedUnit = u;
				} 
            }
			if (selectedUnit != null){
				if (selectedUnit.fatigue >= selectedUnit.moveRange / 2) {
					if (pathfinding.FindPath (selectedUnit.transform.position, transform.position).Length
					                > (selectedUnit.moveRange / 2 + .5)) {
						texture.SetPixel (1, 1, c);
					}
				} else if (selectedUnit.fatigue < selectedUnit.moveRange / 2) {
					if (pathfinding.FindPath (selectedUnit.transform.position, transform.position).Length > (selectedUnit.fatigue)) {
						texture.SetPixel (1, 1, c);
					}
				}
            }

            foreach (Transform g in unwalkables)
            {
                if (gameObject.transform.position.x == g.position.x && gameObject.transform.position.z == g.position.z)
                {
                    texture.SetPixel(1, 1, c);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
            mesh_renderer.sharedMaterials[0].mainTexture = texture;
        }
	
	}


}
