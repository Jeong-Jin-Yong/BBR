using UnityEngine;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

public class StageLooper : MonoBehaviour
{
    public GameManager gm;

    public List<GameObject> stage1;
    public List<GameObject> stage2;
    public List<GameObject> stage3;
    private void Update()
    {
        if (stage1 != null && stage2 != null && stage3 != null)
        {
            StageMovement();
        }
    }

    void StageMovement()
    {
        if (gm.stageID == 1)
        {
            foreach (var stage in stage1)
            {
                stage.transform.Translate(Vector3.left * 4.0f * Time.deltaTime);
                if (stage.transform.position.x <= -17.3f)
                {
                    stage.transform.position = new Vector3(28.6f, 0f, 0f);
                }
            }
        }
        else if (gm.stageID == 2)
        {
            foreach (var stage in stage1)
            {
                stage.SetActive(false);
            }
            foreach (var stage in stage2)
            {
                stage.transform.Translate(Vector3.left * 4.0f * Time.deltaTime);
                stage.SetActive(true); // -2 16.2
                if (stage.transform.position.x <= -18.2f)
                {
                    stage.transform.position = new Vector3(30.4f, 0f, 0f);
                }
            }
        }
        else if (gm.stageID == 3)
        {
            foreach (var stage in stage2)
            {
                stage.SetActive(false);
            }
            foreach (var stage in stage3)
            {
                stage.transform.Translate(Vector3.left * 4.0f * Time.deltaTime);
                stage.SetActive(true); // -2 13.3
                if (stage.transform.position.x <= -16.4f)
                {
                    stage.transform.position = new Vector3(30.4f, 0f, 0f);
                }
            }
        }
        else return;
    }
}
