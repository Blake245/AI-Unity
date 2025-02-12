using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Priority_Queue;

public class NavAStar
{
    public static bool Generate(NavNode startNode, NavNode endNode, ref List<NavNode> path)
    {
        var nodes = new SimplePriorityQueue<NavNode>();

        startNode.Cost = 0;
        float heuristic = (endNode.transform.position - startNode.transform.position).magnitude;
        nodes.EnqueueWithoutDuplicates(startNode, startNode.Cost + heuristic);

        bool found = false;
        while (nodes.Count > 0 && !found)
        {
            var currentNode = nodes.Dequeue();
            if (currentNode == endNode)
            {
                found = true;
                break;
            }

            foreach (var neighbor in currentNode.neighbors)
            {
                float cost = currentNode.Cost + (currentNode.transform.position - neighbor.transform.position).magnitude;
                if (cost < neighbor.Cost)
                {
                    neighbor.Cost = cost;
                    neighbor.Previous = currentNode;

                    heuristic = (neighbor.transform.position - endNode.transform.position).magnitude;
                    nodes.EnqueueWithoutDuplicates(neighbor, cost + heuristic);
                }
            }
        }
        
        if (found)
        {
            NavNode.CreatePath(endNode, ref path);
        }

        return found;
    }
}
