using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAIController))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyAIController fov = (EnemyAIController)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusView);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angleView / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleView / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radiusView);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radiusView);

    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); 
    }
}
