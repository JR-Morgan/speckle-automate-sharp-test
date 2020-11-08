﻿using System;
using System.Collections.Generic;
using System.Text;
using Objects.BuiltElements;

namespace Objects.Revit
{
  public class RevitLevel : Level, IRevit
  {
    public bool createView { get; set; }

    public Dictionary<string, object> parameters { get; set; }
    public string elementId { get; set; }
  }
}
