﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pastel.Nodes
{
    public enum CompilationEntityType
    {
        FUNCTION,
        ENUM,
        CONSTANT,
        GLOBAL,
        OTHER,
    }

    public interface ICompilationEntity
    {
        CompilationEntityType EntityType { get; }
    }
}
