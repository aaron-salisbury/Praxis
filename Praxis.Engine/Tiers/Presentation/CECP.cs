﻿using System.Collections.Generic;

namespace Praxis.Engine.Tiers.Presentation
{
    //TODO: 
    internal class CECP : CommunicationProtocol
    {
        override internal Engine Engine { get; set; }

        internal CECP(Engine engine) : base(engine) { }

        override internal List<string> ProcessInput(string input)
        {
            return new List<string>();
        }
    }
}
