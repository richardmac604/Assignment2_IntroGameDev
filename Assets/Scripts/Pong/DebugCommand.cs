using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugCommandBase
{
    private string commandId;
    private string commandDescription;
    private string commandFormat;

    public string getCommandId{ get {return commandId;}}
    public string getCommandDescription{ get {return commandDescription;}}
    public string getCommandFormat{ get {return commandFormat;}}

    public DebugCommandBase(string id, string description, string format){
        commandId = id;
        commandDescription = commandDescription;
        commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase{
    private Action command;
    public DebugCommand(string id, string description, string format, Action command) : base (id, description, format){
        this.command = command;
    }

    public void Invoke(){
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase{
    private Action<T1> command;
    public DebugCommand(string id, string description, string format, Action<T1> command) : base (id, description, format){
        this.command = command;
    }

    public void Invoke(T1 value){
        command.Invoke(value);
    }
}