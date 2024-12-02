using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class MazeGen3 : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;
    [SerializeField] private GameObject wallTriggerPrefab; 
   

    private MazeCell[,] _mazeGrid;
    private MazeCell _entranceCell;
    private MazeCell _exitCell;
    public GameObject enemy;
    public GameObject endzone;
    private Vector3 enemyStartPosition;

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
        _entranceCell = _mazeGrid[0, 0];
        _exitCell = _mazeGrid[_mazeWidth - 1, _mazeDepth - 1];

        // Generate the maze starting from the entrance
        yield return GenerateMaze(null, _entranceCell);

        // Clear the walls for entrance and exit
        ClearEntranceAndExitWalls();

        // Replace a wall with a trigger
        AddWallTrigger();

        enemyStartPosition = new Vector3((_mazeWidth / 2), 0, (_mazeDepth / 2));
        Instantiate(enemy, enemyStartPosition, Quaternion.identity);
        
    }

    private IEnumerator GenerateMaze(MazeCell prevCell, MazeCell currentCell)
    {
        currentCell.Visit();
        if (prevCell != null)
        {
            ClearWalls(prevCell, currentCell);
        }

        yield return new WaitForSeconds(0.05f);

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

    private void AddWallTrigger()
    {
        // Choose a random cell and a random wall for the trigger
        MazeCell triggerCell = _mazeGrid[Random.Range(0, _mazeWidth), Random.Range(0, _mazeDepth)];
        Vector3 triggerPosition = triggerCell.transform.position;

        // Offset to position the trigger where the wall would be
        Vector3 triggerOffset = new Vector3(1f, 0.5f, 0.3f); // Example: Replace right wall
        Instantiate(wallTriggerPrefab, triggerPosition + triggerOffset, Quaternion.identity);
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
        _entranceCell.ClearLeftWall();
        _exitCell.ClearRightWall();
        Instantiate(endzone, new Vector3(_exitCell.transform.position.x + 0.5f, 0, _exitCell.transform.position.z), Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            if (GameObject.FindWithTag("Enemy") != null)
            {
                GameObject.FindWithTag("Enemy").GetComponent<Rigidbody>().position = enemyStartPosition;
            }
        }
    }
}