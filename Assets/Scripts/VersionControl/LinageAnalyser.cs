using System;
using System.Collections.Generic;

public class LineageAnalyser {
    public static Relationship Compare<T>(T baseT, T featureT) where T : IChainLink<T> {
        if (baseT.Equals(featureT)) {
            return Relationship.Same;
        }

        HashSet<T> baseTs = new HashSet<T>();
        HashSet<T> featureTs = new HashSet<T>();

        T tempT = featureT;
        while (tempT != null) {
            if (tempT.Equals(baseT)) {
                return Relationship.FastForward;
            }
            featureTs.Add(tempT);
            tempT = tempT.GetParent();
        }

        tempT = baseT;
        while (tempT != null) {
            if (tempT.Equals(featureT)) {
                return Relationship.Rewind;
            }
            baseTs.Add(tempT);
            tempT = tempT.GetParent();
        }

        tempT = featureT;
        while (tempT != null) {
            if (baseTs.Contains(tempT)) {
                return Relationship.Divergent;
            }
            tempT = tempT.GetParent();
        }

        return Relationship.Unknown;
    }
}