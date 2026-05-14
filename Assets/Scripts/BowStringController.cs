using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowStringController : MonoBehaviour
{
    [Header("References")]
    public LineRenderer bowString;         // BowStringRenderer with LineRenderer
    public Transform stringTop;            // top anchor
    public Transform stringMiddleRest;     // middle rest position
    public Transform stringBottom;         // bottom anchor
    public Transform arrowVisual;          // Arrow child on the bow (visual)
    public BowController bow;  
    public Vector3 arrowTailOffset = new Vector3(-0.3f, 0f, 0f);            // BowController on Bow and Arrow

    void Awake()
    {
        if (bowString != null)
            bowString.positionCount = 3;
    }

    void LateUpdate()
    {
        if (bowString == null || stringTop == null || stringMiddleRest == null || stringBottom == null)
            return;

        // Start in rest position
        Vector3 midPos = stringMiddleRest.position;

        // While holding and arrow still on the bow, middle of string follows arrow
        if (bow != null && bow.IsHolding() && bow.HasArrow() && arrowVisual != null)
        {
            midPos = arrowVisual.TransformPoint(arrowTailOffset);
        }

        // Use world positions because Use World Space is true
        bowString.SetPosition(0, stringTop.position);
        bowString.SetPosition(1, midPos);
        bowString.SetPosition(2, stringBottom.position);
    }
}
