using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeWorker : IMergeWorker
{
    private IBranch baseBranch, featureBranch;
    private IOverlay baseOverlay, featureOverlay;
    private bool isMergable;
    private Relationship mergeType;
    private ICollection<VersionController> ffControllers, resolvedControllers, conflictControllers;
    private ICollection<Renderer> hiddenSprites;

    private IDictionary<VersionController, IVersion> stagingArea;

    private TriggerManager pickTrigger;

    private MergeUIController ui;

    private GameObject oldStatusUI;

    public MergeWorker(IBranch baseBranch, IBranch featureBranch, TriggerManager trigger, MergeUIController mergeUI) {
        if (baseBranch == null || featureBranch == null) {
            throw new Exception("Branch can not be null");
        }
        this.baseBranch = baseBranch;
        this.featureBranch = featureBranch;
        this.isMergable = false;
        this.mergeType = LineageAnalyser.Compare(this.baseBranch.GetTip(), this.featureBranch.GetTip());

        stagingArea = new Dictionary<VersionController, IVersion>();

        pickTrigger = trigger;
        this.ui = mergeUI;

        Initialise();

        UpdateStatus();
        RenderDiff();

        if (this.ui != null) {
            this.ui.gameObject.SetActive(true);
            Camera.main.GetComponent<MergeInterfaceCamera>().enabled = true;
            this.ui.SetMergeWorker(this);
            this.ui.PopulateConflictObjects(conflictControllers);

            oldStatusUI = GameObject.Find("Status");
            oldStatusUI.SetActive(false);
        }
    }

    /// <summary>
    /// Update status variables given the current state of the MergerWorker
    /// </summary>
    private void UpdateStatus() {
        this.isMergable = conflictControllers.Count == 0;
    }

    /// <summary>
    /// Initialise the MergeWorker, determine the state
    /// </summary>
    private void Initialise() {
        switch (this.mergeType) {
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

        ffControllers = new HashSet<VersionController>();
        resolvedControllers = new HashSet<VersionController>();
        conflictControllers = new HashSet<VersionController>();

        hiddenSprites = new HashSet<Renderer>();

        IEnumerable<VersionController> intersection = baseBranch.GetTip().GetTrackedObjects().Intersect(featureBranch.GetTip().GetTrackedObjects());
        foreach (VersionController trackedObject in intersection) {
            IVersion baseVersion = baseBranch.GetTip().getObjectVersion(trackedObject);
            IVersion featureVersion = featureBranch.GetTip().getObjectVersion(trackedObject);
            switch(LineageAnalyser.Compare<IVersion>(baseVersion, featureVersion)) {
                case Relationship.Unknown:
                    throw new Exception("Can not determine version relativity");
                case Relationship.Rewind:
                    ffControllers.Add(trackedObject);
                    stagingArea.Add(trackedObject, baseVersion);
                    break;
                case Relationship.Same:
                case Relationship.FastForward:
                    ffControllers.Add(trackedObject);
                    stagingArea.Add(trackedObject, featureVersion);
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
            ffControllers.Add(trackedObject);
            stagingArea.Add(trackedObject, version);
        }

        foreach (VersionController trackedObject in featureBranch.GetTip().GetTrackedObjects().Except(intersection)) {
            IVersion version = featureBranch.GetTip().getObjectVersion(trackedObject);
            ffControllers.Add(trackedObject);
            stagingArea.Add(trackedObject, version);
        }
    }

    public void Abort() {
        this.ui.gameObject.SetActive(false);
        oldStatusUI.SetActive(true);
        Debug.Log("In Abort");
        Camera.main.GetComponent<MergeInterfaceCamera>().enabled = false;
        this.DestroyOverlays();
    }

    public void End() {
        if (this.ui != null) {
            this.ui.gameObject.SetActive(false);
            oldStatusUI.SetActive(true);
            Camera.main.GetComponent<MergeInterfaceCamera>().enabled = false;
        }
        Debug.Log("In End");
        this.DestroyOverlays();
    }

    public bool IsResolved() {
        this.UpdateStatus();
        return this.isMergable;
    }

    public void PickVersion(VersionController versionedObject, IVersion version) {
        if (conflictControllers.Contains(versionedObject)) {
            conflictControllers.Remove(versionedObject);

            resolvedControllers.Add(versionedObject);
            stagingArea.Add(versionedObject, version);
        }
        else if (resolvedControllers.Contains(versionedObject)) {
            stagingArea[versionedObject] = version;
        }
        else {
            throw new Exception("Tried to resolve controller, but wasn't in conflict controller set");
        }
        
        this.UpdateStatus();

        if (pickTrigger != null) {
            pickTrigger.Trigger();
        }
    }

    public void RenderDiff() {
        // Blue Overlay
        this.baseOverlay = new Overlay(baseBranch.GetTip(), new Color(0.278f, 1f, 0.916f, 0.5f));

        // Pink Overlay
        this.featureOverlay = new Overlay(featureBranch.GetTip(), new Color(0.7f, 0.22f, 0.63f, 0.5f));

        foreach (VersionController ffController in ffControllers) {
            if (baseBranch.GetTip().ObjectIsTrackedInThisCommit(ffController)) {
                baseOverlay.RemoveObject(ffController);
            }

            if (featureBranch.GetTip().ObjectIsTrackedInThisCommit(ffController)) {
                featureOverlay.RemoveObject(ffController);
            }
        }

        foreach (VersionController resolvedController in resolvedControllers) {
            if (stagingArea[resolvedController] == baseBranch.GetTip().getObjectVersion(resolvedController)) {
                baseOverlay.SetColor(resolvedController, new Color(0f, 1f, 0f, 0.5f));
                featureOverlay.SetColor(resolvedController, new Color(0f, 0f, 0f, 0.5f));
            }
            else if (stagingArea[resolvedController] == featureBranch.GetTip().getObjectVersion(resolvedController)) {
                featureOverlay.SetColor(resolvedController, new Color(0f, 1f, 0f, 0.5f));
                baseOverlay.SetColor(resolvedController, new Color(0f, 0f, 0f, 0.5f));
            }
            // TODO: Somehow enable selectivity of the game object associated with this vesion controller in both overlays
        }

        //Hide the real object so that they can focus on the merge previews
        foreach (VersionController conflictController in conflictControllers) {
            Renderer conflictRenderer = conflictController.GetActiveVersion().GetComponent<Renderer>();
            if (conflictRenderer != null) {
                conflictRenderer.enabled = false;
                hiddenSprites.Add(conflictRenderer);
            }
        }
    }

    private void DestroyOverlays() {
        this.baseOverlay.Destroy();
        this.featureOverlay.Destroy();

        foreach (Renderer conflictRenderer in hiddenSprites) {
            conflictRenderer.enabled = true;
        }
    }

    public GameObject GetBasePreviewForVersionedObject(VersionController versionedObject) {
        return this.baseOverlay.GetPreviewForObject(versionedObject);
    }

    public GameObject GetFeaturePreviewForVersionedObject(VersionController versionedObject) {
        return this.featureOverlay.GetPreviewForObject(versionedObject);
    }

    public IDictionary<VersionController, IVersion> BuildStagingArea() {
        return new Dictionary<VersionController, IVersion>(stagingArea);
    }

    public Relationship GetMergeType() {
        return this.mergeType;
    }

    public MergeStatus GetStatus(VersionController versionedObject) {
        if (conflictControllers.Contains(versionedObject)) {
            return MergeStatus.Conflict;
        }
        if (resolvedControllers.Contains(versionedObject)) {
            return MergeStatus.Resolved;
        }
        if (ffControllers.Contains(versionedObject)) {
            return MergeStatus.FastForward;
        }
        return MergeStatus.Unknown;
    }

    public void PickObject(GameObject pickedObject) {
        VersionController versionedObject;
        if (baseOverlay.HasGameObject(pickedObject, out versionedObject)) {
            PickVersion(versionedObject, baseBranch.GetTip().getObjectVersion(versionedObject));
            baseOverlay.SetColor(versionedObject, new Color(0f, 1f, 0f, 0.5f));
            featureOverlay.SetColor(versionedObject, new Color(0f, 0f, 0f, 0.5f));
            this.ui.VersionPicked(versionedObject, pickedObject);
        }
        else if (featureOverlay.HasGameObject(pickedObject, out versionedObject)) {
            PickVersion(versionedObject, featureBranch.GetTip().getObjectVersion(versionedObject));
            featureOverlay.SetColor(versionedObject, new Color(0f, 1f, 0f, 0.5f));
            baseOverlay.SetColor(versionedObject, new Color(0f, 0f, 0f, 0.5f));
            this.ui.VersionPicked(versionedObject, pickedObject);
        }
    }

    public IBranch GetBaseBranch() {
        return this.baseBranch;
    }

    public IBranch GetFeatureBranch() {
        return this.featureBranch;
    }
}