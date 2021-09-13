using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Marker : MonoBehaviour
{
    public ObjectColor Color;
    public ObjectType Type;
}

enum ObjectType
{
    Flower, Spirit, Gate
}