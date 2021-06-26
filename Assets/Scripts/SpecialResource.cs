using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class SpecialResource : Resource
{
    public enum SpecialResourceType { MOVE, MED}
    public SpecialResourceType type;

    public SpecialResource(SpecialResourceType type)
    {
        this.type = type;
    }
}
