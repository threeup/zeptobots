﻿using System;
using UnityEngine;

[AttributeUsage( AttributeTargets.Enum | AttributeTargets.Field )]
public class EnumFlagsAttribute : PropertyAttribute
{
	public EnumFlagsAttribute() { }
}