using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCollider : MonoBehaviour
{ // A collider that just needs to return true or false
    public bool CollisionCheck(Vector2 objectPos, Vector2 obstaclePos, Vector2 playerScale, Vector2 obstacleScale)
    {
        if (objectPos.y >= obstaclePos.y - (obstacleScale.y + playerScale.y) && objectPos.y <= obstaclePos.y + (obstacleScale.y + playerScale.y) &&
            objectPos.x >= obstaclePos.x - (obstacleScale.x + playerScale.y) && objectPos.x <= obstaclePos.x + (obstacleScale.x + playerScale.y))
        {
            return true;
        }
        return false;
    }
}
