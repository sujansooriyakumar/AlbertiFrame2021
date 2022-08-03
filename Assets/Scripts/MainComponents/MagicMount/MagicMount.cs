using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MagicMount : ComponentFinderBase
{
  MountEvents MountEvents { get; }
  int TargetEyeLayer { get; }
  MagicAlbertiFrame Frame { get; }
}
