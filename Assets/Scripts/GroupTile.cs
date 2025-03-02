using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class GroupTile : RuleTile<GroupTile.Neighbor> {
    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return tile != null;
            case Neighbor.NotThis: return tile == null;
        }
        return false;
    }
}