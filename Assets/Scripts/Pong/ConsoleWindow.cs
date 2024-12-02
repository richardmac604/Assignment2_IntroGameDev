using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleWindow : MonoBehaviour
{
    bool showConsole;
    private InputActions inputActions;
    private InputAction toggleConsole;
    private InputAction enterCommand;
    string input;

    public static DebugCommand CHANGE_BACKGROUND;

    public static DebugCommand<string> CHANGE_BACKGROUND_COLOR;
    public static DebugCommand<string> SET_BALL_SPEED;

    public List<object> commandList;

    private void Awake(){
        inputActions = new InputActions();
        inputActions.Player.Console.performed += OnToggleDebug;
        inputActions.Player.Return.performed += OnReturn;


        CHANGE_BACKGROUND = new DebugCommand("change_background", "Changes the background color to red", "change_background", () =>
        {
            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<Camera>().backgroundColor = Color.red;
        }
        );

        CHANGE_BACKGROUND_COLOR = new DebugCommand<string>("change_background_color", "Changes the background color of the game", "change_background_color <color>", (x) =>
        {
            float[] hexList = parseHex(x);
            Color newColor = new Color(hexList[0], hexList[1], hexList[2]);

            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<Camera>().backgroundColor = newColor;

        });

        SET_BALL_SPEED = new DebugCommand<string>("set_ball_speed", "Changes the ball speed", "set_ball_speed <speed>", (x) =>
        {
            GameObject ball = GameObject.FindWithTag("Ball");
            float newSpeed = float.Parse(x);
            ball.GetComponent<Ball>().setSpeed(newSpeed);
        });
        

        commandList = new List<object> {
            CHANGE_BACKGROUND,
            CHANGE_BACKGROUND_COLOR,
            SET_BALL_SPEED

        };
    }

    private float[] parseHex(string hexcode){
        float[] colorList = new float[3];


        string r = hexcode[0].ToString() + hexcode[1].ToString();
        string g = hexcode[2].ToString() + hexcode[3].ToString();
        string b = hexcode[4].ToString() + hexcode[5].ToString();

        colorList[0] = int.Parse(r, System.Globalization.NumberStyles.HexNumber) / 255f;
        colorList[1] = int.Parse(g, System.Globalization.NumberStyles.HexNumber) / 255f;
        colorList[2]= int.Parse(b, System.Globalization.NumberStyles.HexNumber) / 255f;

        return colorList;
    }

    private void OnEnable(){
        toggleConsole = inputActions.Player.Console;
        enterCommand = inputActions.Player.Return;
        toggleConsole.Enable();
        enterCommand.Enable();
    }

    private void OnDisable(){
        toggleConsole.Disable();
        enterCommand.Disable();
    }

    public void OnReturn(InputAction.CallbackContext context){
        Debug.Log("This is working");
        if(context.performed && showConsole) {
            Debug.Log("This is working2");
            HandleInput();
            input = ""; 
        }
    }

    public void OnToggleDebug(InputAction.CallbackContext context){
        if(context.performed) {
            showConsole = !showConsole;
        }
    }

    private void OnGUI(){
        if(!showConsole) {return;}

        float y = 0.0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    private void HandleInput() {

        string[] properties = input.Split(' ');

        for(int i = 0; i < commandList.Count; i++) {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.getCommandId)){
                if(commandList[i] as DebugCommand != null){
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<string> != null) {
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
            }
        }
    }
}