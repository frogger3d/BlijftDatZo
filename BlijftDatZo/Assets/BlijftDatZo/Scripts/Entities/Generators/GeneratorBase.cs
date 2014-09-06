using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class GeneratorBase : MonoBehaviour
{
    public abstract void Initialize(GameController gameController);

    public abstract void CollectionGoalAchieved();
}
