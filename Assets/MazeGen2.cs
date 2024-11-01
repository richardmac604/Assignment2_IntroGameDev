using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGen2 : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;

    private MazeCell[,] _mazeGrid;
    private MazeCell _entranceCell;
    private MazeCell _exitCell;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Instantiate the maze grid
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        // Define entrance and exit points
        _entranceCell = _mazeGrid[0, 0];             // Bottom-left corner (or any other starting point)
        _exitCell = _mazeGrid[_mazeWidth - 1, _mazeDepth - 1];  // Top-right corner (or any other end point)

        // Generate the maze starting from the entrance
        yield return GenerateMaze(null, _entranceCell);

        // Clear the walls for entrance and exit
        ClearEntranceAndExitWalls();
    }

    private IEnumerator GenerateMaze(MazeCell prevCell, MazeCell currentCell)
    {
        currentCell.Visit();
        if (prevCell != null)
        {
            ClearWalls(prevCell, currentCell);
        }

        yield return new WaitForSeconds(0.05f); // Optional delay for visualization

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        return unvisitedCells.OrderBy(_ => Random.value).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited)
            yield return _mazeGrid[x + 1, z];

        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited)
            yield return _mazeGrid[x - 1, z];

        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited)
            yield return _mazeGrid[x, z + 1];

        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited)
            yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell prevCell, MazeCell currentCell)
    {
        if (prevCell.transform.position.x < currentCell.transform.position.x)
        {
            prevCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (prevCell.transform.position.x > currentCell.transform.position.x)
        {
            prevCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (prevCell.transform.position.z < currentCell.transform.position.z)
        {
            prevCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (prevCell.transform.position.z > currentCell.transform.position.z)
        {
            prevCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    private void ClearEntranceAndExitWalls()
    {
        // Open the wall at the entrance (bottom-left)
        _entranceCell.ClearLeftWall(); // Adjust this based on the wall direction you want open

        // Open the wall at the exit (top-right)
        _exitCell.ClearRightWall(); // Adjust this based on the wall direction you want open
    }
}
