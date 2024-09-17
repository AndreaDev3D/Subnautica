﻿using AD3D_LightSolution.Base;
using Nautilus.Json;
using System.Collections.Generic;

namespace AD3D_LightSolutionMod.BO.Config
{
    public class DatabaseConfig : ConfigFile
    {
        public List<DataItem> SwitchItemList { get; set; } = new List<DataItem>();
    }
}
