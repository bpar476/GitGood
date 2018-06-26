using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeWorker : IMergeWorker
{
    private IBranch baseBranch, featureBranch;
    private IOverlay baseOverlay, featureOverlay;
    private VersionManager versionManager;
    private bool isMergable;
    private Relationship relationship;
    private ICollection<VersionController> resolvedControllers, conflictControllers;

    public MergeWorker(VersionManager versionManager, IBranch baseBranch, IBranch featureBranch) {
        if (baseBranch == null || featureBranch == null) {
            throw new Exception("Branch can not be null");
        }
        this.baseBranch = baseBranch;
        this.featureBranch = featureBranch;
        this.isMergable = false;
        this.versionManager = versionManager;
        this.relationship = LineageAnalyser.Compare(this.baseBranch.GetTip(), this.featureBranch.GetTip());
        Initialise();

        UpdateStatus();
        RenderDiff();
    }

    private void UpdateStatus() {
        this.isMergable = conflictControllers.Count == 0;
    }

    private void Initialise() {
        switch (this.relationship) {
            case Relationship.Rewind:
                throw new Exception("Base branch is ahead of feature branch, merge redundant");
            case Relationship.Same:
                throw new Exception("Branches are the same");
            case Relationship.Unknown:
                throw new Exception("Can not determine branch relativity");
            case Relationship.FastForward:
                break;
            case Relationship.Divergent:
                break;
            default:
                break;
        }

        resolvedControllers = new HashSet<VersionController>();
        conflictControllers = new HashSet<VersionController>();

        IEnumerable<VersionController> intersection = baseBranch.GetTip().GetTrackedObjects().Intersect(featureBranch.GetTip().GetTrackedObjects());
        foreach (VersionController trackedObject in intersection) {
            IVersion baseVersion = baseBranch.GetTip().getObjectVersion(trackedObject);
            IVersion featureVersion = featureBranch.GetTip().getObjectVersion(trackedObject);
            switch(LineageAnalyser.Compare<IVersion>(baseVersion, featureVersion)) {
                case Relationship.Unknown:
                    throw new Exception("Can not determine version relativity");
                case Relationship.Rewind:
                    resolvedControllers.Add(trackedObject);
                    versionManager.Add(trackedObject, baseVersion);
                    break;
                case Relationship.Same:
                case Relationship.FastForward:
                    resolvedControllers.Add(trackedObject);
                    versionManager.Add(trackedObject, featureVersion);
                    break;
                case Relationship.Divergent:
                    conflictControllers.Add(trackedObject);
                    break;
                default:
                    break;
            }
        }

        foreach (VersionController trackedObject in baseBranch.GetTip().GetTrackedObjects().Except(intersection)) {
            IVersion version = baseBranch.GetTip().getObjectVersion(trackedObject);
            resolvedControllers.Add(trackedObject);
            versionManager.Add(trackedObject, version);
        }

        foreach (VersionController trackedObject in featureBranch.GetTip().GetTrackedObjects().Except(intersection)) {
            IVersion version = featureBranch.GetTip().getObjectVersion(trackedObject);
            resolvedControllers.Add(trackedObject);
            versionManager.Add(trackedObject, version);
        }
    }

    public void Abort() {
        this.DestroyOverlays();
    }

    public void End() {
        this.DestroyOverlays();
    }

    public bool IsResolved() {
        return this.isMergable;
    }

    public void PickVersion(VersionController vc, IVersion version) {
        throw new NotImplementedException();
    }

    public void RenderDiff() {
        this.baseOverlay = new Overlay(baseBranch.GetTip(), new Color(0.5f, 0f, 1f, 0.5f));
        this.featureOverlay = new Overlay(featureBranch.GetTip(), new Color(1f, 1f, 0f, 0.5f));
    }

    private void DestroyOverlays() {
        this.baseOverlay.Destroy();
        this.featureOverlay.Destroy();
    }
}