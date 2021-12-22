﻿using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Internal.Intializers
{
    internal class ConfigManagerInitializer : IInitializer
    {
        public void Init()
        {
            ConfigManager.Initialize();
        }
    }
}
