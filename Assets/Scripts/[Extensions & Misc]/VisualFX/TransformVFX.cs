using System.Collections;
using UnityEngine;

namespace CGames.VisualFX
{
    public static class TransformVFX
    {	
        public static IEnumerator MoveObject(Transform objectToMove, Vector3 positionToAdd, float animationTiming)
        {
            float timer = 0f;

            Vector3 startPosition = objectToMove.localPosition;

            Vector3 endPosition = new(startPosition.x + positionToAdd.x, startPosition.y + positionToAdd.y, startPosition.z + positionToAdd.z);

            while (timer < animationTiming)
            {
                objectToMove.localPosition = Vector3.Lerp(startPosition, endPosition, timer / animationTiming);

                timer += Time.deltaTime;

                yield return null;
            }

            objectToMove.localPosition = endPosition;
        }
    }
}