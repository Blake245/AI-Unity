using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// Ensures that this component requires a NavAgent component
[RequireComponent(typeof(NavAgent))]
public class NavPath : MonoBehaviour
{
    // Reference to the associated NavAgent (AI movement controller)
    private NavAgent agent;

    // List to store the path (sequence of NavNodes from start to destination)
    private List<NavNode> path = new List<NavNode>();

    // The current target node in the path
    public NavNode TargetNode { get; set; } = null;

    // Returns true if there is a valid target node (used for movement checks)
    public bool HasTarget => TargetNode != null;

    // Colors for visualization in the editor
    private Color white = new(1, 1, 1, 0.5f);  // Path node color
    private Color green = new(0, 1, 0, 0.5f);  // Start node color
    private Color red = new(1, 0, 0, 0.5f);    // End node color

    /// <summary>
    /// Gets or sets the destination for pathfinding.
    /// When setting, it calculates the shortest path using Dijkstra's algorithm.
    /// </summary>
    public Vector3 Destination => (TargetNode != null) ? TargetNode.transform.position : Vector3.zero;

    public void GeneratePath(Vector3 startPosition, Vector3 endPosition)
    {
        // Find the closest navigation nodes to the agent and the destination position
        NavNode startNode = NavNode.GetNearestNavNode(startPosition);
        NavNode endNode = NavNode.GetNearestNavNode(endPosition);

        // Clear the existing path and reset all nodes before recalculating
        path.Clear();
        NavNode.ResetNodes();

        // Generate a new shortest path from the start node to the end node
        NavAStar.Generate(startNode, endNode, ref path);

        // Set the first node in the path as the new target node
        TargetNode = startNode;
    }

    private void Awake()
    {
        // Get the NavAgent component attached to the same GameObject
        agent = GetComponent<NavAgent>();
    }

    /// <summary>
    /// Retrieves the next navigation node in the path based on the given node.
    /// </summary>
    /// <param name="node">The current node.</param>
    /// <returns>The next node in the path, or null if at the end.</returns>
    public NavNode GetNextNavNode(NavNode node)
    {
        // If the path is empty, return null
        if (path.Count == 0) return null;

        // Find the index of the current node in the path list
        int index = path.FindIndex(pathNode => pathNode == node);

        // If the node is not found or it's the last node in the path, return null
        if (index == -1 || index + 1 == path.Count) return null;

        // Return the next node in the path
        return path[index + 1];
    }

    /// <summary>
    /// Draws visual representations of the path when the object is selected in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // If there are no nodes in the path, do nothing
        if (path.Count == 0) return;

        // Convert path list to an array for easier indexing
        var pathArray = path.ToArray();

        // Draw spheres for intermediate path nodes
        for (int i = 1; i < path.Count - 1; i++)
        {
            Gizmos.color = white;
            Gizmos.DrawSphere(pathArray[i].transform.position, 1);
        }

        // Draw sphere for the start node (green)
        Gizmos.color = green;
        Gizmos.DrawSphere(pathArray[0].transform.position, 1);

        // Draw sphere for the end node (red)
        Gizmos.color = red;
        Gizmos.DrawSphere(pathArray[pathArray.Length - 1].transform.position, 1);
    }
}