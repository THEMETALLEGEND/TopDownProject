using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    //Это блюпринт базового состояния. Скрипт, который будет являться для всех
    //состояний разных стейт машин. Этот шаблон трогать не нужно.

    public string name;
    protected StateMachine stateMachine;

    public BaseState(string name, StateMachine stateMachine) //сюда скрипт принимает название состояния и её стейт машину
    {
        this.name = name; //присваиваем этому скрипту название и, видимо, от этого оно и работает
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }

}
