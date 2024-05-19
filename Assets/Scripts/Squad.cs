using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Dreamteck.Splines;

public class Squad : MonoBehaviour
{
    [SerializeField] Guy prefabGuy;
    [SerializeField] SplineComputer spline;
    [SerializeField] SplineMesh splineMesh;
    [SerializeField] int startingGuysAmount = 5;

    List<Guy> guys = new();

    void Awake()
    {
        for (int i = 0; i < startingGuysAmount; i++)
        {
            var newGuy = Instantiate(prefabGuy, transform);
            AddGuy(newGuy);
        }
    }

    public void AddGuy(Guy guy)
    {
        guy.Initialize(spline);
        guy.squad = this;
        guys.Add(guy);
    }

    public void RemoveGuy(Guy guy)
    {
        guys.Remove(guy);
    }

    public void SetFormationOptimized(List<Vector3> points)
    {
        float step = ((float)points.Count-1) / guys.Count;

        for (int i = 0; i < guys.Count; i++)
        {
            int pointIndex = Mathf.RoundToInt(step * i);

            guys[i].SetSplineWithOffset(spline, points[pointIndex]);
        }
    }
}
