﻿using System.Collections.Generic;

namespace Praxis.Business.Communication
{
    abstract internal class CommunicationProtocol
    {
        abstract internal Engine Engine { get; set; }

        internal CommunicationProtocol(Engine engine)
        {
            Engine = engine;
        }

        abstract internal List<string> ProcessInput(string input);
    }
}
